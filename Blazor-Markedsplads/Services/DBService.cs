using Npgsql;
using Blazor_Markedsplads.Models;
namespace Blazor_Markedsplads.Services
{
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
 
        public async Task<List<CustomerModel>> GetAllUsers(string query)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var customers = new List<CustomerModel>();
                    while (await reader.ReadAsync())
                    {
                        customers.Add(
                            new CustomerModel
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
 
        public async Task<bool> AddListingAsync(ProductModel listing)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                //describtion, image_url - @describtion, @imageurl
                string query = "INSERT INTO product (customer_id, product_name, price, product_type, image_url, nationality, percent, age) VALUES (@customerId, @product_name, @price, @product_type, @image_url, @nationality, @percent, @age)";
 
                using var command = new NpgsqlCommand(query, connection);
 
                command.Parameters.AddWithValue("@customerId", listing.CustomerID);
                command.Parameters.AddWithValue("@product_name", listing.ProductName);
                command.Parameters.AddWithValue("@description", listing.Description ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@price", listing.Price);
                command.Parameters.AddWithValue("@product_type", listing.ProductType);
                command.Parameters.AddWithValue("@image_url", listing.ImageUrl);
                command.Parameters.AddWithValue("@nationality", listing.Nationality);
                command.Parameters.AddWithValue("@percent", listing.Percent);
                command.Parameters.AddWithValue("@age", listing.Age);
 
                return await command.ExecuteNonQueryAsync() > 0;
            }
            catch (Exception ex)
            {
                //Console.WriteLine?
                Console.WriteLine($"Error adding listing: {ex.Message}");
                return false;
            }
        }
 
        //Added ListingService into DBService, 
        // so to keep database operations in one place
        public async Task<List<ProductModel>> GetAllListingsAsync()
        {
            var listings = new List<ProductModel>();
 
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
 
            string query = "SELECT * FROM product";
 
            using var command = new NpgsqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();
 
            while (await reader.ReadAsync())
            {
                listings.Add(new ProductModel
                {
                    ID = reader.GetInt32(reader.GetOrdinal("id")),
                    ProductName = reader.GetString(reader.GetOrdinal("product_name")),
                    Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                    Price = reader.GetDecimal(reader.GetOrdinal("price")),
                    ProductType = reader.GetString(reader.GetOrdinal("product_type")),
                    ImageUrl = reader.GetString(reader.GetOrdinal("image_url")),
                    CustomerID = reader.GetInt32(reader.GetOrdinal("customer_id")),
                    Nationality = reader.IsDBNull(reader.GetOrdinal("nationality")) ? null : reader.GetString(reader.GetOrdinal("nationality")),
                    Percent = reader.GetDecimal(reader.GetOrdinal("percent")),
                    Age = reader.IsDBNull(reader.GetOrdinal("age")) ? null : reader.GetInt32(reader.GetOrdinal("age"))
                });
            }
            return listings;
        }
 
 
        public int CreateProduct(ProductModel product)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
 
            const string sql = @"
            INSERT INTO product
                (product_name, price, product_type, image_url, nationalty, percent, age, customer_id)
            VALUES
                (@p_name, @p_price, @p_type, @p_image, @p_nationality, @p_percent, @p_age, @p_customer)
            RETURNING id;
        ";
 
            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("p_name", product.ProductName);
            cmd.Parameters.AddWithValue("p_price", product.Price);
            cmd.Parameters.AddWithValue("p_type", product.ProductType);
 
            if (string.IsNullOrWhiteSpace(product.ImageUrl))
                cmd.Parameters.AddWithValue("p_image", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("p_image", product.ImageUrl);
 
            cmd.Parameters.AddWithValue("p_customer", product.CustomerID);
            cmd.Parameters.AddWithValue("p_nationality", product.Nationality);
            cmd.Parameters.AddWithValue("p_percent", product.Percent);
            cmd.Parameters.AddWithValue("p_age", product.Age);
 
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
    }
}