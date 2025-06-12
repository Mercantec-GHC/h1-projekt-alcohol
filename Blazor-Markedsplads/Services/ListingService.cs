
using Npgsql;
namespace Blazor_Markedsplads.Services
{

    public class ListingService
    {

        private readonly DBService _dbService;
        private readonly string _connectionString;

        public ListingService(DBService dbService, IConfiguration configuration)
        {
            _dbService = dbService;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void CreateListing(ListingModel listing)
        {
            if (string.IsNullOrWhiteSpace(listing.Title)
                || string.IsNullOrWhiteSpace(listing.Category)
                || listing.Price <= 0)
            {
                throw new ArgumentException("Listing must have a Title, Category, and valid Price.");
            }

            var product = new ProductModel //creates Product to get its ID, using  DBService
            {
                ProductName = listing.Title,
                Price = listing.Price,
                ProductType = listing.Category,
                // Description = listing.Description,
                Nationality = listing.Nationality,
                Percent = listing.Percent,
                Age = listing.Age,
                ImageUrl = listing.ImageUrl,
                CustomerID = listing.CustomerId
            };
            int newProductId = _dbService.CreateProduct(product);

            using var connection = new NpgsqlConnection(_connectionString);//new item in listing table with product id
            connection.Open();

            using var transaction = connection.BeginTransaction();
            try
            {
                const string insertListingSql = @"
                INSERT INTO listings
                    (title, description, price, category, image_url, product_id, customer_id)
                VALUES
                    (@l_title, @l_desc, @l_price, @l_cat, @l_img, @l_prod_id, @l_cust_id);
            ";

                using var cmdList = new NpgsqlCommand(insertListingSql, connection, transaction);
                cmdList.Parameters.AddWithValue("l_title", listing.Title);
                // cmdList.Parameters.AddWithValue("l_desc", (object?)listing.Description ?? DBNull.Value);
                cmdList.Parameters.AddWithValue("l_price", listing.Price);
                cmdList.Parameters.AddWithValue("l_cat", listing.Category);

                if (string.IsNullOrWhiteSpace(listing.ImageUrl))
                    cmdList.Parameters.AddWithValue("l_img", DBNull.Value);
                else
                    cmdList.Parameters.AddWithValue("l_img", listing.ImageUrl);

                cmdList.Parameters.AddWithValue("l_prod_id", newProductId);
                cmdList.Parameters.AddWithValue("l_cust_id", listing.CustomerId);

                cmdList.ExecuteNonQuery();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<List<ProductModel>> GetProductsByUserAsync(int customerId)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = @"SELECT id, product_name, price, description, product_type, image_url, age
            FROM product WHERE customer_id = @customerId";

            await using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("customerId", customerId);

            var products = new List<ProductModel>();

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                products.Add(new ProductModel
                {
                    ID = reader.GetInt32(reader.GetOrdinal("id")),
                    ProductName = reader.GetString(reader.GetOrdinal("product_name")),
                    Price = reader.GetDecimal(reader.GetOrdinal("price")),
                    Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                    ProductType = reader.GetString(reader.GetOrdinal("product_type")),
                    ImageUrl = reader.GetString(reader.GetOrdinal("image_url")),
                    Age = reader.GetInt32(reader.GetOrdinal("age"))
                });
            }

            return products;
        }
    }
}


//private readonly string _connectionString;

//public ListingService(IConfiguration configuration)
//{
//     _connectionString = configuration.GetConnectionString("DefaultConnection");
//}
//private readonly List<Product> listings = new List<Product>();

//public void AddListing(Product item)
//{
//    listings.Add(item);
//}


