namespace ASP.NET_Demo.Models;

public interface IDbService
{
    IEnumerable<T> Query<T>(string sql, object param = null);
    T QuerySingle<T>(string sql, object param = null);
    int Execute(string sql, object param = null);
}

