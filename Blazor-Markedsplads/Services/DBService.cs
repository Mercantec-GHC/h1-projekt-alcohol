using Npgsql;
using Blazor_Markedsplads.Models;
public partial class DBService
{
    private readonly string _connectionString;

    public DBService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<Customer>> GetAllUsers()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand("SELECT * FROM Customer",
                 connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var customers = new List<Customer>();
                    while (await reader.ReadAsync())
                    {
                        customers.Add(
                            new Customer
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("id")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                Email = reader.GetString(reader.GetOrdinal("email")),
                                //*Password = reader.GetString(reader
                                //.GetOrdinal("password")),
                                Age = reader.GetInt32(reader.GetOrdinal("age")),
                                Address = reader.GetString(reader.GetOrdinal("address")),
                                Phone = reader.GetInt32(reader.GetOrdinal("phone"))
                            }
                        );
                    }
                    return customers;
                }
            }
        }
}