using System.Data.Common;
using Microsoft.Data.Sqlite;
using System.IO;

if (args.Length < 2)
{
    Console.WriteLine("Usage: ApplySqlApp <databasePath> <sqlFile>");
    return 1;
}

var dbPath = args[0];
var sqlFile = args[1];
if (!File.Exists(sqlFile))
{
    Console.WriteLine($"SQL file not found: {sqlFile}");
    return 1;
}

var sql = File.ReadAllText(sqlFile);
var connStr = new SqliteConnectionStringBuilder { DataSource = dbPath }.ToString();
using var conn = new SqliteConnection(connStr);
conn.Open();
using var cmd = conn.CreateCommand();
// The SQL file may contain its own BEGIN/COMMIT statements. Do not wrap in an outer transaction
cmd.CommandText = sql;
cmd.ExecuteNonQuery();
Console.WriteLine("SQL applied successfully.");
return 0;
