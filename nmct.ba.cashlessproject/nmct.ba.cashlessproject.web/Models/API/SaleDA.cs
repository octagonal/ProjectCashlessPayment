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

namespace nmct.ba.cashlessproject.web.Models.API
{
    public class SaleDA
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", ".", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass));
        }

        public static ConnectionStringSettings CreateConnectionStringBase(string dbname, string dblogin, string dbpass)
        {
            return Database.CreateConnectionString("System.Data.SqlClient", ".", dbname, dblogin, dbpass);
        }

        public static List<Sale> GetSales(IEnumerable<Claim> claims)
        {


            List<Sale> list = new List<Sale>();
            string sql = "SELECT * FROM Sale";
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);
            while (reader.Read())
            {
                Console.WriteLine(reader["Amount"].ToString());
                list.Add(BuildModel(reader));
            }

            return list;
        }

        public static List<Sale> GetSalesByDateAndType(int id, string type, DateTime periodStart, DateTime periodEnd, IEnumerable<Claim> claims)
        {
            List<Sale> list = new List<Sale>();
            string sql = "SELECT * FROM Sale WHERE Timestamp BETWEEN @PeriodStart AND @PeriodEnd AND " + type + "=@Id";
            DbParameter par1 = Database.AddParameter("AdminDB", "@PeriodStart", periodStart);
            DbParameter par2 = Database.AddParameter("AdminDB", "@PeriodEnd", periodEnd);
            DbParameter par3 = Database.AddParameter("AdminDB", "@Id", id);
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3);
            while (reader.Read())
            {
                list.Add(BuildModel(reader));
            }

            return list;
        }

        public static List<Sale> GetSalesByDate(DateTime periodStart, DateTime periodEnd, IEnumerable<Claim> claims)
        {
            List<Sale> list = new List<Sale>();
            string sql = "SELECT * FROM Sale WHERE Timestamp BETWEEN @PeriodStart AND @PeriodEnd";
            DbParameter par1 = Database.AddParameter("AdminDB", "@PeriodStart", periodStart);
            DbParameter par2 = Database.AddParameter("AdminDB", "@PeriodEnd", periodEnd);
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2);
            while (reader.Read())
            {
                list.Add(BuildModel(reader));
            }

            return list;
        }

        private static Sale BuildModel(DbDataReader reader)
        {
            return new Sale()
            {
                Amount = Int32.Parse(reader["Amount"].ToString()),
                CustomerId = Int32.Parse(reader["CustomerID"].ToString()),
                ID = Int32.Parse(reader["ID"].ToString()),
                ProductId = Int32.Parse(reader["ProductID"].ToString()),
                RegisterId = Int32.Parse(reader["RegisterId"].ToString()),
                Timestamp = Convert.ToDateTime(reader["Timestamp"].ToString()),
                TotalPrice = Convert.ToDouble(reader["TotalPrice"].ToString())
            };
        }


        public static int InsertSale(Sale c, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO Sale (Timestamp,CustomerID,RegisterID,ProductID,Amount,TotalPrice) VALUES(@Timestamp,@CustomerID,@RegisterID,@ProductID,@Amount,@TotalPrice)";
            DbParameter par1 = Database.AddParameter("AdminDB", "@Timestamp", c.Timestamp);
            DbParameter par2 = Database.AddParameter("AdminDB", "@CustomerID", c.CustomerId);
            DbParameter par3 = Database.AddParameter("AdminDB", "@RegisterID", c.RegisterId);
            DbParameter par4 = Database.AddParameter("AdminDB", "@ProductID", c.ProductId);
            DbParameter par5 = Database.AddParameter("AdminDB", "@Amount", c.Amount);
            DbParameter par6 = Database.AddParameter("AdminDB", "@TotalPrice", c.TotalPrice);
            return Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4, par5, par6);
        }
    }
}