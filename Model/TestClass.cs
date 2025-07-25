using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using NuGet.Protocol.Plugins;


namespace API.Model
{
    public class TestClass
    {
        public async Task<string> TestAsync()
        {
            var str = "hello from async task";
            Console.WriteLine(str);

            await Task.Delay(1000);

            var builder = new SqlConnectionStringBuilder
            {
                DataSource = "(localdb)\\MSSQLLocalDB",
                UserID = "api",
                Password = "api",
                InitialCatalog = "finshark"
            };

            var connectionString = builder.ConnectionString;

            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            var sql = "SELECT Id, Symbol FROM Stocks";
            await using var command = new SqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Console.WriteLine("{0} {1}", reader.GetInt32(0), reader.GetString(1));
            } 

            return str;
        }
    }
}
