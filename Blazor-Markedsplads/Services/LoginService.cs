﻿using Npgsql;

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
        public async Task<CustomerModel?> ValidateLoginAsync(string email, string password)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = @"
        SELECT id, name, email, age, address, phone, password
        FROM customer
        WHERE email = @email
    ";

            await using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("email", email);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return null;

            var storedPassword = reader.GetString(reader.GetOrdinal("password"));

            if (storedPassword != password)
                return null;

            var customer = new CustomerModel
            {
                ID = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                Email = reader.GetString(reader.GetOrdinal("email")),
                Age = reader.GetInt32(reader.GetOrdinal("age")),
                Address = reader.GetString(reader.GetOrdinal("address")),
                Phone = reader.GetInt32(reader.GetOrdinal("phone")),
                
                // Password = null 
            };

            return customer;
        }
        public async Task<(bool IsValid, string Name, string Address, int Age, int Id)> ValidateLogin(string email, string password)
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
