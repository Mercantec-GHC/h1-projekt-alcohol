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
        public async Task<(bool IsValid, bool IsSeller)> ValidateLoginAsync(string email, string password)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

           
            const string sql = @"
                SELECT password, isseller
                FROM customer
                WHERE email = @email
                
            ";

            await using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("email", email);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
            {
                return (false, false);
            }

           
            var storedPassword = reader.GetString(reader.GetOrdinal("password"));
            var isSeller = reader.GetBoolean(reader.GetOrdinal("isseller"));

           
            if (storedPassword == password)
            {
                return (true, isSeller);
            }
            else
            {
                return (false, false);
            }
        }
    }
}
