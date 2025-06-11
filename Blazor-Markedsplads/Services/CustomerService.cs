using Npgsql;

namespace Blazor_Markedsplads.Services
{

    public class CustomerService
    {
        private readonly DBService _dbService;
        private readonly string _connectionString;

        public CustomerService(DBService dbService, IConfiguration configuration)
        {
            _dbService = dbService;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }



        // Add user to customer table in db and returns its ID
        public async Task<int> CreateCustomerAsync(CustomerModel customer)
        {
            if (string.IsNullOrWhiteSpace(customer.Name) ||
                string.IsNullOrWhiteSpace(customer.Email) ||
                customer.Age < 18 ||
                string.IsNullOrWhiteSpace(customer.Password))
            {
                throw new ArgumentException("Name, email, age and password are required.");
            }

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = @"
                INSERT INTO customer 
                    (name, email, age, address, phone, password)
                VALUES 
                    (@name, @email, @age, @address, @phone, @password)
                RETURNING id;
            ";

            await using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("name", customer.Name);
            cmd.Parameters.AddWithValue("email", customer.Email);
            cmd.Parameters.AddWithValue("age", customer.Age);
            cmd.Parameters.AddWithValue("address",
                string.IsNullOrWhiteSpace(customer.Address)
                    ? (object)DBNull.Value
                    : customer.Address);
            cmd.Parameters.AddWithValue("phone", customer.Phone);
           // cmd.Parameters.AddWithValue("isSeller", customer.IsSeller);
            cmd.Parameters.AddWithValue("password", customer.Password);

            int newId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return newId;
        }


        // Updates customer's data. Returns true if a row was updated.

        public async Task<bool> UpdateCustomerAsync(CustomerModel customer)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = @"
                UPDATE customer
                SET 
                    name      = @name,
                    email     = @email,
                    age       = @age,
                    address   = @address,
                    phone     = @phone,

                    password  = @password
                WHERE 
                    id = @id;
            ";

            await using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("id", customer.ID);
            cmd.Parameters.AddWithValue("name", customer.Name);
            cmd.Parameters.AddWithValue("email", customer.Email);
            cmd.Parameters.AddWithValue("age", customer.Age);
            cmd.Parameters.AddWithValue("address",
                string.IsNullOrWhiteSpace(customer.Address)
                    ? (object)DBNull.Value
                    : customer.Address);
            cmd.Parameters.AddWithValue("phone", customer.Phone);
          //  cmd.Parameters.AddWithValue("isSeller", customer.IsSeller);
            cmd.Parameters.AddWithValue("password", customer.Password);

            int affected = await cmd.ExecuteNonQueryAsync();
            return affected > 0;
        }

        // Deletes customer by name. Returns true if a row was deleted.
        public async Task<bool> DeleteCustomerAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name must be provided.");

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = "DELETE FROM customer WHERE name = @name;";

            await using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@name", name);

            int affected = await cmd.ExecuteNonQueryAsync();
            return affected > 0;
        }
    }
}