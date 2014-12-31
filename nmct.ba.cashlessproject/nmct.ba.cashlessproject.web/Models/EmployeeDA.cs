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
    public class EmployeeDA
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", ".", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass));
        }

        public static List<Employee> GetEmployees(IEnumerable<Claim> claims)
        {


            List<Employee> list = new List<Employee>();
            string sql = "SELECT * FROM Employee";
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);
            while (reader.Read())
            {
                list.Add(BuildModel(reader));
            }

            return list;
        }

        private static Employee BuildModel(DbDataReader reader)
        {
            return new Employee()
            {
                ID = Int32.Parse(reader["ID"].ToString()),
                Email = reader["Email"].ToString(),
                Address = reader["Address"].ToString(),
                EmployeeName = reader["EmployeeName"].ToString(),
                NationalNumber = reader["NationalNumber"].ToString(),
                Phone = reader["Phone"].ToString()
            };
        }


        public static int InsertEmployee(Employee c, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO Employee VALUES(@EmployeeName,@Address,@Email,@Phone,@NationalNumber)";
            DbParameter par2 = Database.AddParameter("AdminDB", "@EmployeeName", c.EmployeeName);
            DbParameter par3 = Database.AddParameter("AdminDB", "@Address", c.Address);
            DbParameter par4 = Database.AddParameter("AdminDB", "@Email", c.Email);
            DbParameter par5 = Database.AddParameter("AdminDB", "@Phone", c.Phone);
            DbParameter par6 = Database.AddParameter("AdminDB", "@NationalNumber", c.NationalNumber);
            return Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par2, par3, par4, par5, par6);
        }

        public static void UpdateEmployee(Employee c, IEnumerable<Claim> claims)
        {
            string sql = "UPDATE Employee SET EmployeeName=@EmployeeName, Address=@Address, Email=@Email, Phone=@Phone, NationalNumber=@NationalNumber WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@ID", c.ID);
            DbParameter par2 = Database.AddParameter("AdminDB", "@EmployeeName", c.EmployeeName);
            DbParameter par3 = Database.AddParameter("AdminDB", "@Address", c.Address);
            DbParameter par4 = Database.AddParameter("AdminDB", "@Email", c.Email);
            DbParameter par5 = Database.AddParameter("AdminDB", "@Phone", c.Phone);
            DbParameter par6 = Database.AddParameter("AdminDB", "@NationalNumber", c.NationalNumber);
            Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4, par5, par6);
        }

        public static void DeleteEmployee(int id, IEnumerable<Claim> claims)
        {
            string sql = "DELETE FROM Employee WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@ID", id);
            DbConnection con = Database.GetConnection(CreateConnectionString(claims));
            Database.ModifyData(con, sql, par1);
        }
    }
}