using AccessControl.API.Services;
using AccessControl.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _svc;
    public CompaniesController(ICompanyService svc) => _svc = svc;

    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _svc.GetAllAsync());
    [HttpGet("{id}")] public async Task<IActionResult> GetById(Guid id) { var c = await _svc.GetByIdAsync(id); return c == null ? NotFound() : Ok(c); }
    [HttpPost] public async Task<IActionResult> Create([FromBody] CreateCompanyDto dto) => Ok(await _svc.CreateAsync(dto));
    [HttpPut("{id}")] public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCompanyDto dto) { var c = await _svc.UpdateAsync(id, dto); return c == null ? NotFound() : Ok(c); }
    [HttpDelete("{id}")] public async Task<IActionResult> Delete(Guid id) => await _svc.DeleteAsync(id) ? NoContent() : NotFound();
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BranchesController : ControllerBase
{
    private readonly IBranchService _svc;
    public BranchesController(IBranchService svc) => _svc = svc;

    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _svc.GetAllAsync());
    [HttpGet("{id}")] public async Task<IActionResult> GetById(Guid id) { var b = await _svc.GetByIdAsync(id); return b == null ? NotFound() : Ok(b); }
    [HttpPost] public async Task<IActionResult> Create([FromBody] CreateBranchDto dto) => Ok(await _svc.CreateAsync(dto));
    [HttpPut("{id}")] public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBranchDto dto) { var b = await _svc.UpdateAsync(id, dto); return b == null ? NotFound() : Ok(b); }
    [HttpDelete("{id}")] public async Task<IActionResult> Delete(Guid id) => await _svc.DeleteAsync(id) ? NoContent() : NotFound();
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ApplicationsController : ControllerBase
{
    private readonly IApplicationService _svc;
    public ApplicationsController(IApplicationService svc) => _svc = svc;

    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _svc.GetAllAsync());
    [HttpGet("{id}")] public async Task<IActionResult> GetById(Guid id) { var a = await _svc.GetByIdAsync(id); return a == null ? NotFound() : Ok(a); }
    [HttpPost] public async Task<IActionResult> Create([FromBody] CreateApplicationDto dto) => Ok(await _svc.CreateAsync(dto));
    [HttpPut("{id}")] public async Task<IActionResult> Update(Guid id, [FromBody] UpdateApplicationDto dto) { var a = await _svc.UpdateAsync(id, dto); return a == null ? NotFound() : Ok(a); }
    [HttpDelete("{id}")] public async Task<IActionResult> Delete(Guid id) => await _svc.DeleteAsync(id) ? NoContent() : NotFound();
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FunctionalitiesController : ControllerBase
{
    private readonly IFunctionalityService _svc;
    public FunctionalitiesController(IFunctionalityService svc) => _svc = svc;

    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _svc.GetAllAsync());
    [HttpGet("{id}")] public async Task<IActionResult> GetById(Guid id) { var f = await _svc.GetByIdAsync(id); return f == null ? NotFound() : Ok(f); }
    [HttpPost] public async Task<IActionResult> Create([FromBody] CreateFunctionalityDto dto) => Ok(await _svc.CreateAsync(dto));
    [HttpPut("{id}")] public async Task<IActionResult> Update(Guid id, [FromBody] UpdateFunctionalityDto dto) { var f = await _svc.UpdateAsync(id, dto); return f == null ? NotFound() : Ok(f); }
    [HttpDelete("{id}")] public async Task<IActionResult> Delete(Guid id) => await _svc.DeleteAsync(id) ? NoContent() : NotFound();
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly IRoleService _svc;
    public RolesController(IRoleService svc) => _svc = svc;

    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _svc.GetAllAsync());
    [HttpGet("{id}")] public async Task<IActionResult> GetById(Guid id) { var r = await _svc.GetByIdAsync(id); return r == null ? NotFound() : Ok(r); }
    [HttpPost] public async Task<IActionResult> Create([FromBody] CreateRoleDto dto) => Ok(await _svc.CreateAsync(dto));
    [HttpPut("{id}")] public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoleDto dto) { var r = await _svc.UpdateAsync(id, dto); return r == null ? NotFound() : Ok(r); }
    [HttpDelete("{id}")] public async Task<IActionResult> Delete(Guid id) => await _svc.DeleteAsync(id) ? NoContent() : NotFound();
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoleFunctionalitiesController : ControllerBase
{
    private readonly IRoleFunctionalityService _svc;
    public RoleFunctionalitiesController(IRoleFunctionalityService svc) => _svc = svc;

    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _svc.GetAllAsync());
    [HttpGet("{id}")] public async Task<IActionResult> GetById(Guid id) { var rf = await _svc.GetByIdAsync(id); return rf == null ? NotFound() : Ok(rf); }
    [HttpPost] public async Task<IActionResult> Create([FromBody] CreateRoleFunctionalityDto dto) => Ok(await _svc.CreateAsync(dto));
    [HttpPut("{id}")] public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoleFunctionalityDto dto) { var rf = await _svc.UpdateAsync(id, dto); return rf == null ? NotFound() : Ok(rf); }
    [HttpDelete("{id}")] public async Task<IActionResult> Delete(Guid id) => await _svc.DeleteAsync(id) ? NoContent() : NotFound();
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _svc;
    public UsersController(IUserService svc) => _svc = svc;

    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _svc.GetAllAsync());
    [HttpGet("{id}")] public async Task<IActionResult> GetById(Guid id) { var u = await _svc.GetByIdAsync(id); return u == null ? NotFound() : Ok(u); }
    [HttpPost] public async Task<IActionResult> Create([FromBody] CreateUserDto dto) => Ok(await _svc.CreateAsync(dto));
    [HttpPut("{id}")] public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto dto) { var u = await _svc.UpdateAsync(id, dto); return u == null ? NotFound() : Ok(u); }
    [HttpDelete("{id}")] public async Task<IActionResult> Delete(Guid id) => await _svc.DeleteAsync(id) ? NoContent() : NotFound();
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserApplicationRolesController : ControllerBase
{
    private readonly IUserApplicationRoleService _svc;
    public UserApplicationRolesController(IUserApplicationRoleService svc) => _svc = svc;

    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _svc.GetAllAsync());
    [HttpGet("{id}")] public async Task<IActionResult> GetById(Guid id) { var ur = await _svc.GetByIdAsync(id); return ur == null ? NotFound() : Ok(ur); }
    [HttpPost] public async Task<IActionResult> Create([FromBody] CreateUserApplicationRoleDto dto) => Ok(await _svc.CreateAsync(dto));
    [HttpDelete("{id}")] public async Task<IActionResult> Delete(Guid id) => await _svc.DeleteAsync(id) ? NoContent() : NotFound();
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PermissionsController : ControllerBase
{
    private readonly IPermissionService _svc;
    public PermissionsController(IPermissionService svc) => _svc = svc;

    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _svc.GetAllAsync());
    [HttpGet("{id}")] public async Task<IActionResult> GetById(Guid id) { var p = await _svc.GetByIdAsync(id); return p == null ? NotFound() : Ok(p); }
    [HttpPost] public async Task<IActionResult> Create([FromBody] CreatePermissionDto dto) => Ok(await _svc.CreateAsync(dto));
    [HttpPut("{id}")] public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePermissionDto dto) { var p = await _svc.UpdateAsync(id, dto); return p == null ? NotFound() : Ok(p); }
    [HttpDelete("{id}")] public async Task<IActionResult> Delete(Guid id) => await _svc.DeleteAsync(id) ? NoContent() : NotFound();
}

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IAuthService _svc;
    public AuthController(IAuthService svc) => _svc = svc;

    [HttpPost("login")] public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _svc.LoginAsync(request);
        return result == null ? Unauthorized(new { message = "Invalid credentials" }) : Ok(result);
    }

    [HttpPost("sso")] public async Task<IActionResult> SsoLogin([FromBody] SsoLoginRequest request)
    {
        var result = await _svc.SsoLoginAsync(request);
        return result == null ? Unauthorized(new { message = "SSO login failed" }) : Ok(result);
    }

    [HttpPost("refresh")] public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        var result = await _svc.RefreshTokenAsync(request);
        return result == null ? Unauthorized(new { message = "Invalid refresh token" }) : Ok(result);
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _svc;
    public DashboardController(IDashboardService svc) => _svc = svc;

    [HttpGet("summary")] public async Task<IActionResult> GetSummary() => Ok(await _svc.GetSummaryAsync());
    [HttpGet("activity")] public async Task<IActionResult> GetRecentActivity([FromQuery] int count = 10) => Ok(await _svc.GetRecentActivityAsync(count));
}
