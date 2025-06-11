using Npgsql;

namespace Blazor_Markedsplads.Services
{
    public class LoginService
{
        private readonly DBService _dbService;
        private readonly string _connectionString;

        public LoginService(DBService dbService, IConfiguration configuration)
        {
            _dbService = dbService;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<(bool IsValid, string Name, string Address, int Age, int Id)> ValidateLoginAsync(string email, string password)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

           
            const string sql = @"
                SELECT password, name, age, address, id
                FROM customer
                WHERE email = @email
                
            ";

            await using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("email", email);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
            {
                return (false, "", "", 0, 0);
            }

           
            var storedPassword = reader.GetString(reader.GetOrdinal("password"));
            var name = reader.GetString(reader.GetOrdinal("name"));
            var address = reader.GetString(reader.GetOrdinal("address"));
            var age = reader.GetInt32(reader.GetOrdinal("age"));
            var id = reader.GetInt32(reader.GetOrdinal("id"));


            if (storedPassword == password)
            {
                return (true, name, address, age, id);
            }
            else
            {
                return (false, "", "", 0, 0);
            }
        }
    }
}
