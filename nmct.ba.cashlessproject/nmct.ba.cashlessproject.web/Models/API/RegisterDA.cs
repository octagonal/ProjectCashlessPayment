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
    public class RegisterDA
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

        public static List<Register> GetRegisters(IEnumerable<Claim> claims)
        {


            List<Register> list = new List<Register>();
            string sql = "SELECT * FROM Register";
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);
            while (reader.Read())
            {
                list.Add(BuildModel(reader));
            }

            return list;
        }

        private static Register BuildModel(DbDataReader reader)
        {
            return new Register()
            {
                ID = Int32.Parse(reader["ID"].ToString()),
                RegisterName = reader["RegisterName"].ToString(),
                Device = reader["Device"].ToString()
            };
        }


        public static int InsertRegister(Register c, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO Register VALUES(@RegisterName,@Device)";
            DbParameter par1 = Database.AddParameter("AdminDB", "@RegisterName", c.RegisterName);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Device", c.Device);
            return Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2);
        }

        public static void UpdateRegister(Register c, IEnumerable<Claim> claims)
        {
            string sql = "UPDATE Register SET RegisterName=@RegisterName, Device=@Device WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@RegisterName", c.RegisterName);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Device", c.Device); ;
            DbParameter par3 = Database.AddParameter("AdminDB", "@ID", c.ID);
            Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3);
        }

        public static void DeleteRegister(int id, IEnumerable<Claim> claims)
        {
            string sql = "DELETE FROM Register WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@ID", id);
            DbConnection con = Database.GetConnection(CreateConnectionString(claims));
            Database.ModifyData(con, sql, par1);
        }
    }
}