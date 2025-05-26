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
            using (var command = new NpgsqlCommand("SELECT * FROM Customer", connection))
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

    public async Task<bool> AddListingAsync(Listing listing)
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = "INSERT INTO listing (product_id, customer_id, title, description, price, category, image_url) VALUES (@productId, @customerId, @title, @description, @price, @category, @imageUrl)";

            using var command = new NpgsqlCommand(query, connection);
            // command.Parameters.AddWithValue("@productId", listing.ProductId);
            // command.Parameters.AddWithValue("@customerId", listing.CustomerId);
            command.Parameters.AddWithValue("@title", listing.Title);
            command.Parameters.AddWithValue("@description", listing.Description ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@price", listing.Price);
            command.Parameters.AddWithValue("@category", listing.Category);
            command.Parameters.AddWithValue("@imageUrl", listing.ImageUrl);

            return await command.ExecuteNonQueryAsync() > 0;
        }
        catch (Exception ex)
        {
            //Console.WriteLine?
            Console.WriteLine($"Error adding listing: {ex.Message}");
            return false;
        }
    }

    public async Task<List<Listing>> GetAllListingsAsync()
{
    var listings = new List<Listing>();

    using var connection = new NpgsqlConnection(_connectionString);
    await connection.OpenAsync();

    string query = "SELECT * FROM listing";

    using var command = new NpgsqlCommand(query, connection);
    using var reader = await command.ExecuteReaderAsync();

    while (await reader.ReadAsync())
    {
        listings.Add(new Listing
        {
            ID = reader.GetInt32(reader.GetOrdinal("id")),
            // ProductId = reader.GetInt32(reader.GetOrdinal("product_id")),
            // CustomerId = reader.GetInt32(reader.GetOrdinal("customer_id")),
            Title = reader.GetString(reader.GetOrdinal("title")),
            Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
            Price = reader.GetDecimal(reader.GetOrdinal("price")),
            Category = reader.GetString(reader.GetOrdinal("category")),
            ImageUrl = reader.GetString(reader.GetOrdinal("image_url"))
        });
    }
    return listings;
}


}