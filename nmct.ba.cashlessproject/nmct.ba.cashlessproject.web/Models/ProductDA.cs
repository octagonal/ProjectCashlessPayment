using nmct.ba.cashlessproject.helper;
using nmct.ba.cashlessproject.model.it;
using nmct.ba.cashlessproject.web.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace nmct.ba.cashlessproject.web.Models
{
    public class ProductDA
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", ".", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass));
        }

        public static List<Product> GetProducts(IEnumerable<Claim> claims)
        {


            List<Product> list = new List<Product>();
            string sql = "SELECT * FROM Product";
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);
            while (reader.Read())
            {
                list.Add(BuildModel(reader));
            }

            return list;
        }

        private static Product BuildModel(DbDataReader reader)
        {
            return new Product()
            {
                ID = Int32.Parse(reader["ID"].ToString()),
                Price = Convert.ToDouble(reader["Price"].ToString()),
                ProductName = reader["ProductName"].ToString()
            };
        }


        public static int InsertProduct(Product c, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO Product VALUES(@ProductName,@Price)";
            DbParameter par1 = Database.AddParameter("AdminDB", "@ProductName", c.ProductName);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Price", c.Price);
            return Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2);
        }

        public static void UpdateProduct(Product c, IEnumerable<Claim> claims)
        {
            string sql = "UPDATE Product SET ProductName=@ProductName, Price=@Price WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@ProductName", c.ProductName);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Price", c.Price);
            DbParameter par3 = Database.AddParameter("AdminDB", "@ID", c.ID);
            Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3);
        }

        public static void DeleteProduct(int id, IEnumerable<Claim> claims)
        {
            string sql = "DELETE FROM Product WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@ID", id);
            DbConnection con = Database.GetConnection(CreateConnectionString(claims));
            Database.ModifyData(con, sql, par1);
        }
    }
}