using Dapper;
using System.Data;

namespace ASP.NET_Demo.Models;

public class DbService
{
    private readonly IDbConnection _conn;

    public DbService(IDbConnection conn)
    {
        _conn = conn;
    }

    public IEnumerable<T> Query<T>(string sql, object param = null)
    {
        return _conn.Query<T>(sql, param);
    }
}
