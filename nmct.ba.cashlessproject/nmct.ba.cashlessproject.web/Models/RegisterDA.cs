using nmct.ba.cashlessproject.helper;
using nmct.ba.cashlessproject.model.it;
using nmct.ba.cashlessproject.web.Helper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace nmct.ba.cashlessproject.web.Models
{
    public class RegisterDA
    {
        #region Public CRUD functies
        public static int               UpdateRegister(Register item)
        {
            return EditRegister(item);
        }
        public static int               DeleteRegister(int id)
        {
            return DeleteRegister(GetRegister(id));
        }
        public static Register          ReadRegister(int id)
        {
            return GetRegister(id);
        }
        public static List<Register>    ReadRegisters()
        {
            return GetRegisters();
        }
        public static int               CreateRegister(Register o)
        {
            return InsertRegister(o);
        }
        #endregion

        #region Logica
        private static Register BuildModel(DbDataReader reader)
        {
            return new Register()
            {
                ID = Convert.ToInt32(reader["ID"]),
                RegisterName = reader["RegisterName"].ToString(),
                Device = reader["Device"].ToString(),
                PurchaseDate = Convert.ToDateTime(reader["PurchaseDate"].ToString()),
                ExpiresDate = Convert.ToDateTime(reader["ExpiresDate"].ToString())
            };
        }

        private static List<Register> GetRegisters()
        {
            List<Register> list = new List<Register>();
            string sql = "SELECT * FROM Register";
            DbDataReader reader = Database.GetData("AdminDB", sql);

            while (reader.Read())
            {
                list.Add(BuildModel(reader));
            }

            return list;
        }

        private static Register GetRegister(int id)
        {
            Register item = new Register();
            string sql = "SELECT * FROM Register WHERE Id=@Id";
            DbParameter par1 = Database.AddParameter("AdminDB","@Id", id);

            DbDataReader reader = Database.GetData("AdminDB", sql, par1);
            reader.Read();
            
            item = (BuildModel(reader));

            return item;
        }

        private static int EditRegister(Register o)
        {
            string sql = "UPDATE Register SET RegisterName=@RegisterName,Device=@Device,PurchaseDate=@PurchaseDate,ExpiresDate=@ExpiresDate WHERE ID=@ID";
            DbParameter par1  = Database.AddParameter("AdminDB", "@RegisterName", o.RegisterName);
            DbParameter par2  = Database.AddParameter("AdminDB", "@Device", o.Device);
            DbParameter par3  = Database.AddParameter("AdminDB", "@PurchaseDate", o.PurchaseDate);
            DbParameter par4  = Database.AddParameter("AdminDB", "@ExpiresDate", o.ExpiresDate);
            DbParameter par5  = Database.AddParameter("AdminDB", "@ID", o.ID);
            int idDb = Database.InsertData(Database.GetConnection("AdminDB"), sql, par1, par2, par3, par4, par5);
            return idDb;

        }
        private static int InsertRegister(Register o)
        {
            string sql = "INSERT INTO Register VALUES(@RegisterName,@Device,@PurchaseDate,@ExpiresDate)";
            DbParameter par1 = Database.AddParameter("AdminDB", "@RegisterName", o.RegisterName);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Device", o.Device);
            DbParameter par3 = Database.AddParameter("AdminDB", "@PurchaseDate", o.PurchaseDate);
            DbParameter par4 = Database.AddParameter("AdminDB", "@ExpiresDate", o.ExpiresDate);
            int id = Database.InsertData(Database.GetConnection("AdminDB"), sql, par1, par2, par3, par4);
            return id;
        }

        private static int DeleteRegister(Register o)
        {
            string sql = "DELETE FROM Register WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@ID", o.ID);
            int id = Database.InsertData(Database.GetConnection("AdminDB"), sql, par1);
            return id;
        }
        #endregion
    }
}