using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace TodoWebApi.Context
{
    public class DapperContext
    {
        // interface generica usada para acessar as configurações da aplicação(conexões com banco de dados (que é feita no appsettings.json), configurações personalizadas).
        private readonly IConfiguration _configuration;

        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection() => new MySqlConnection(_connectionString);
    }
}
