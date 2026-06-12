using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Serilog;

// Configure Serilog early
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/accesscontrol-.log", rollingInterval: Serilog.RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.AddControllers();

// Enforce authentication globally by default
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register infrastructure services
builder.Services.AddScoped<AccessControl.Domain.Interfaces.IJwtService, AccessControl.Infrastructure.Services.JwtService>();
builder.Services.AddScoped<AccessControl.Domain.Interfaces.IAuthService, AccessControl.Infrastructure.Services.AuthService>();
builder.Services.AddScoped<AccessControl.Infrastructure.Services.SsoService>();
// Register DbContext (SQLite for development)
builder.Services.AddDbContext<AccessControl.Infrastructure.Persistence.AccessControlDbContext>(options =>
{
    var conn = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=accesscontrol.db";
    options.UseSqlite(conn);
});

// HttpClient for SSO token exchange
builder.Services.AddHttpClient();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// JWT simplificado
var jwtKey = builder.Configuration["Jwt:PrivateKey"] ?? "chave-minima-32-caracteres-para-dev";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// Health checks
builder.Services.AddHealthChecks();
builder.Services.AddHealthChecks().AddDbContextCheck<AccessControl.Infrastructure.Persistence.AccessControlDbContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// Ensure controllers/actions without explicit [AllowAnonymous] require auth
// (FallbackPolicy already set at service registration)
app.MapControllers();

// Endpoints de teste
app.MapGet("/", () => "AccessControl API Running").AllowAnonymous();
app.MapHealthChecks("/api/health/ready").AllowAnonymous();
app.MapGet("/api/health", () => new { status = "OK", timestamp = DateTime.UtcNow }).AllowAnonymous();
app.MapGet("/api/test", () => new { message = "API working!", version = "1.0.0" }).AllowAnonymous();

app.Run();
