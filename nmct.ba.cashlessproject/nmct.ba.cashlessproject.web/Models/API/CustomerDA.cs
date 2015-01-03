﻿using nmct.ba.cashlessproject.helper;
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
    public class CustomerDA
    {
        private static ConnectionStringSettings CreateConnectionString(IEnumerable<Claim> claims)
        {
            string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
            string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
            string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

            return Database.CreateConnectionString("System.Data.SqlClient", ".", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass));
        }

        public static List<Customer> GetCustomers(IEnumerable<Claim> claims)
        {


            List<Customer> list = new List<Customer>();
            string sql = "SELECT * FROM Customer";
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql);
            while (reader.Read())
            {
                Customer c = new Customer();
                c.ID = Convert.ToInt32(reader["ID"]);
                c.CustomerName = reader["CustomerName"].ToString();
                c.Address = reader["Address"].ToString();
                if (!DBNull.Value.Equals(reader["Picture"]))
                    c.Picture = (byte[])reader["Picture"];
                else
                    c.Picture = new byte[0];
                c.Balance = Double.Parse(reader["Balance"].ToString());

                list.Add(c);
            }

            return list;
        }

        public static Customer GetCustomer(string natNum, IEnumerable<Claim> claims)
        {
            Customer c = new Customer();
            string sql = "SELECT * FROM Customer WHERE NationalNumber=@NationalNumber";
            DbParameter par1 = Database.AddParameter("AdminDB", "@NationalNumber", natNum);
            DbDataReader reader = Database.GetData(Database.GetConnection(CreateConnectionString(claims)), sql, par1);
            while (reader.Read())
            {
                c.ID = Convert.ToInt32(reader["ID"]);
                c.CustomerName = reader["CustomerName"].ToString();
                c.Address = reader["Address"].ToString();
                if (!DBNull.Value.Equals(reader["Picture"]))
                    c.Picture = (byte[])reader["Picture"];
                else
                    c.Picture = new byte[0];
                c.Balance = Double.Parse(reader["Balance"].ToString());
                c.NationalNumber = reader["NationalNumber"].ToString();
            }

            return c;
        }

        public static int InsertCustomer(Customer c, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO Customer VALUES(@CustomerName,@Address,@Picture,@Balance,@NationalNumber)";
            DbParameter par1 = Database.AddParameter("AdminDB", "@CustomerName", c.CustomerName);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Address", c.Address);
            DbParameter par3 = Database.AddParameter("AdminDB", "@Picture", new byte[0]);
            DbParameter par4 = Database.AddParameter("AdminDB", "@Balance", c.Balance);
            DbParameter par5 = Database.AddParameter("AdminDB", "@NationalNumber", c.NationalNumber);
            return Database.InsertData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4, par5);
        }

        public static void UpdateCustomer(Customer c, IEnumerable<Claim> claims)
        {
            string sql = "UPDATE Customer SET CustomerName=@CustomerName, Address=@Address, Picture=@Picture, Balance=@Balance WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@CustomerName", c.CustomerName);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Address", c.Address);
            DbParameter par3 = Database.AddParameter("AdminDB", "@Picture", c.Picture);
            DbParameter par4 = Database.AddParameter("AdminDB", "@Balance", c.Balance);
            DbParameter par5 = Database.AddParameter("AdminDB", "@ID", c.ID);
            Database.ModifyData(Database.GetConnection(CreateConnectionString(claims)), sql, par1, par2, par3, par4, par5);
        }

        public static void DeleteCustomer(int id, IEnumerable<Claim> claims)
        {
            string sql = "DELETE FROM Customer WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@ID", id);
            DbConnection con = Database.GetConnection(CreateConnectionString(claims));
            Database.ModifyData(con, sql, par1);
        }
    }
}