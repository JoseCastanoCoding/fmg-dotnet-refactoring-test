using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using refactor_me.Models;
using System.Configuration;

namespace refactor_me.Repositories
{
    public class ProductRepository
    {
        private readonly string _connStr = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;

        public IEnumerable<Product> GetAll(string name = null)
        {
            var products = new List<Product>();
            var connectionToDb = new SqlConnection(_connStr);

            connectionToDb.Open();

            var sql = "SELECT * FROM Product";
            if (!string.IsNullOrEmpty(name))
            {
                sql += " WHERE (Name) Like @Name";
            }

            var cmd = new SqlCommand(sql, connectionToDb);

            if (!string.IsNullOrEmpty(name))
            {
                cmd.Parameters.AddWithValue("@Name", $"%{name.ToLower()}%");
            }

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                products.Add(new Product
                {
                    Id = Guid.Parse(reader["Id"].ToString()),
                    Name = reader["Name"].ToString(),
                    Description = reader["Description"] as string,
                    Price = decimal.Parse(reader["Price"].ToString()),
                    DeliveryPrice = decimal.Parse(reader["DeliveryPrice"].ToString())
                });
            }
            reader.Close();
            
            return products;

        }

        public Product GetById(Guid id)
        {
            var connectionToDb = new SqlConnection(_connStr);
            var cmd = new SqlCommand("SELECT * FROM Product WHERE Id = @Id", connectionToDb);

            cmd.Parameters.AddWithValue("@Id", id);
            connectionToDb.Open();

            var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Product
                {
                    Id = Guid.Parse(reader["Id"].ToString()),
                    Name = reader["Name"].ToString(),
                    Description = reader["Description"] as string,
                    Price = decimal.Parse(reader["Price"].ToString()),
                    DeliveryPrice = decimal.Parse(reader["DeliveryPrice"].ToString())
                };
            }
            reader.Close();

            return null;
        }

        public void Add(Product product)
        {
            var connectionToDb = new SqlConnection(_connStr);
            var cmd = new SqlCommand(@"INSERT INTO Product 
                 (Id, Name, Description, Price, DeliveryPrice)
                 VALUES (@Id, @Name, @Description, @Price, @DeliveryPrice)", connectionToDb);

            cmd.Parameters.AddWithValue("@Id", product.Id);
            cmd.Parameters.AddWithValue("@Name", product.Name);
            cmd.Parameters.AddWithValue("@Description", (object)product.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            cmd.Parameters.AddWithValue("@DeliveryPrice", product.DeliveryPrice);

            connectionToDb.Open();
            cmd.ExecuteNonQuery();

        }

        public void Update(Product product)
        {
            var connectionToDb = new SqlConnection(_connStr);
            var cmd = new SqlCommand(@"UPDATE Product SET
                Name = @Name, Description = @Description, 
                Price = @Price, DeliveryPrice = @DeliveryPrice
                WHERE Id = @Id", connectionToDb);

            cmd.Parameters.AddWithValue("@Id", product.Id);
            cmd.Parameters.AddWithValue("@Name", product.Name);
            cmd.Parameters.AddWithValue("@Description", (object)product.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            cmd.Parameters.AddWithValue("@DeliveryPrice", product.DeliveryPrice);

            connectionToDb.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(Guid id)
        {
            var connectionToDb = new SqlConnection(_connStr);
            var cmd = new SqlCommand(@"DELETE FROM Product WHERE Id = @Id", connectionToDb);

            cmd.Parameters.AddWithValue("@Id", id);

            connectionToDb.Open();
            cmd.ExecuteNonQuery();
        }
    }
}