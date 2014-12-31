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
    public class Register_EmployeeDA
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

        public static List<Register_Employee> GetRegister_Employees(IEnumerable<Claim> claims)
        {


            List<Register_Employee> list = new List<Register_Employee>();
            string sql = "SELECT * FROM Register_Employee";
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);
            while (reader.Read())
            {
                list.Add(BuildModel(reader));
            }

            return list;
        }

        private static Register_Employee BuildModel(DbDataReader reader)
        {
            return new Register_Employee()
            {
                RegisterID = Int32.Parse(reader["RegisterID"].ToString()),
                EmployeeID = Int32.Parse(reader["EmployeeID"].ToString()),
                FromTime   = Convert.ToDateTime(reader["Device"].ToString()),
                UntilTime  = Convert.ToDateTime(reader["UntilTime"].ToString())
            };
        }


        public static int InsertRegister_Employee(Register_Employee c, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO Register_Employee VALUES(@RegisterID,@EmployeeID,@FromTime,@UntilTime)";
            DbParameter par1 = Database.AddParameter("AdminDB", "@RegisterID", c.RegisterID);
            DbParameter par2 = Database.AddParameter("AdminDB", "@EmployeeID", c.EmployeeID);
            DbParameter par3 = Database.AddParameter("AdminDB", "@FromTime", c.FromTime);
            DbParameter par4 = Database.AddParameter("AdminDB", "@UntilTime", c.UntilTime);
            return Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4);
        }

        public static void UpdateRegister_Employee(Register_Employee c, IEnumerable<Claim> claims)
        {
            string sql = "UPDATE Register_Employee SET FromTime=@FromTime, UntilTime=@UntilTime WHERE RegisterID=@RegisterID AND EmployeeID=@EmployeeID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@RegisterID", c.RegisterID);
            DbParameter par2 = Database.AddParameter("AdminDB", "@EmployeeID", c.EmployeeID);
            DbParameter par3 = Database.AddParameter("AdminDB", "@FromTime", c.FromTime);
            DbParameter par4 = Database.AddParameter("AdminDB", "@UntilTime", c.UntilTime);
            Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4);
        }

        public static void DeleteRegister_Employee(Register_Employee c, IEnumerable<Claim> claims)
        {
            string sql = "DELETE FROM Register_Employee WHERE RegisterID=@RegisterID AND EmployeeID=@EmployeeID AND FromTime=@FromTime AND UntilTime=@UntilTime";
            DbParameter par1 = Database.AddParameter("AdminDB", "@RegisterID", c.RegisterID);
            DbParameter par2 = Database.AddParameter("AdminDB", "@EmployeeID", c.EmployeeID);
            DbParameter par3 = Database.AddParameter("AdminDB", "@FromTime", c.FromTime);
            DbParameter par4 = Database.AddParameter("AdminDB", "@UntilTime", c.UntilTime);
            DbConnection con = Database.GetConnection(CreateConnectionString(claims));
            Database.ModifyData(con, sql, par1, par2, par3, par4);
        }
    }
}