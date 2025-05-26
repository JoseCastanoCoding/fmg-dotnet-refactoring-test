using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using refactor_me.Models;

namespace refactor_me.Repositories
{
    public class ProductOptionRepository
    {
        private readonly string _connStr = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;

        public List<ProductOption> GetAll(Guid productId)
        {
            var productOptions = new List<ProductOption>();
            var connectionToDb = new SqlConnection(_connStr);
            var cmd = new SqlCommand("SELECT * FROM ProductOption WHERE ProductId = @ProductId", connectionToDb);

            cmd.Parameters.AddWithValue("@ProductId", productId);

            connectionToDb.Open();

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                productOptions.Add(new ProductOption
                {
                    Id = Guid.Parse(reader["Id"].ToString()),
                    ProductId = Guid.Parse(reader["ProductId"].ToString()),
                    Name = reader["Name"].ToString(),
                    Description = reader["Description"] as string
                });
            }

            reader.Close();

            return productOptions;
        }

        public ProductOption GetById(Guid productId, Guid id)
        {            
            var connectionToDb = new SqlConnection(_connStr);
            var cmd = new SqlCommand("SELECT * FROM ProductOption WHERE ProductId = @ProductId AND Id = @Id", connectionToDb);

            cmd.Parameters.AddWithValue("@ProductId", productId);
            cmd.Parameters.AddWithValue("@Id", id);

            connectionToDb.Open();

            var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new ProductOption
                {
                    Id = Guid.Parse(reader["Id"].ToString()),
                    ProductId = Guid.Parse(reader["ProductId"].ToString()),
                    Name = reader["Name"].ToString(),
                    Description = reader["Description"] as string
                };

            }
            reader.Close();

            return null;

        }

        public void Add(ProductOption productOption)
        {
            var connectionToDb = new SqlConnection(_connStr);
            var cmd = new SqlCommand(@"INSERT INTO ProductOption 
                (Id, ProductId, Name, Description) 
                VALUES (@Id, @ProductId, @Name, @Description)", connectionToDb);

            cmd.Parameters.AddWithValue("@Id", productOption.Id);
            cmd.Parameters.AddWithValue("@ProductId", productOption.ProductId);
            cmd.Parameters.AddWithValue("@Name", productOption.Name);
            cmd.Parameters.AddWithValue("@Description", (object)productOption.Description ?? DBNull.Value);

            connectionToDb.Open();
            cmd.ExecuteNonQuery();
        }
    }
}