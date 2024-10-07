using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace DotnetAPI.Data;
public class DataContextDapper
{
    private readonly IConfiguration _config;
    public DataContextDapper(IConfiguration config)
    {
        _config = config;
    }

    public IEnumerable<T> LoadData<T>(string sql)
    {
        IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.Query<T>(sql);
    }

    public T LoadDataSinge<T>(string sql)
    {
        IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.QuerySingle<T>(sql);
    }

    public bool ExecuteSql(string sql)
    {
        IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.Execute(sql) > 0;
    }

    public int ExecuteSqlRowCount(string sql)
    {
        IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.Execute(sql);
    }

    public bool ExecuteSqlWithParameteres(string sql, List<SqlParameter> parameters)
    {
        SqlCommand commandWithParams = new SqlCommand(sql);
        foreach(SqlParameter parameter in parameters)
        {
            commandWithParams.Parameters.Add(parameter);
        }

        SqlConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        dbConnection.Open();

        commandWithParams.Connection = dbConnection;
        int rowsAffected = commandWithParams.ExecuteNonQuery();
        dbConnection.Close();
        return rowsAffected>0;
        // return dbConnection.Execute(sql) > 0;

    }
}