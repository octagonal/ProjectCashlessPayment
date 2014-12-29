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
        public static int                   UpdateRegister(Register item)
        {
            return EditRegister(item);
        }
        public static int                   DeleteRegister(int id)
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
        public static int                   CreateRegister(Register o)
        {
            return InsertRegister(o);
        }
        #endregion

        #region Logica
        public static Register CheckCredentials(string username, string password)
        {
            string sql = "SELECT * FROM Register WHERE Login=@Login AND Password=@Password";
            DbParameter par1 = Database.AddParameter("AdminDB", "@Login", Cryptography.Encrypt(username));
            DbParameter par2 = Database.AddParameter("AdminDB", "@Password", Cryptography.Encrypt(password));
            try
            {
                DbDataReader reader = Database.GetData(Database.GetConnection("AdminDB"), sql, par1, par2);
                reader.Read();
                return BuildModel(reader);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        private static Register BuildModel(DbDataReader reader)
        {
            return new Register()
            {
                DbPassword = reader["DbPassword"].ToString(),
                RegisterName = reader["RegisterName"].ToString(),
                Address = reader["Address"].ToString(),
                Email = reader["Email"].ToString(),
                Phone = reader["Phone"].ToString()
            };
        }

        private static Register EncryptModel(Register item)
        {
            return new Register()
            {
                ID = item.ID,
                RegisterName = item.RegisterName,
                Address = item.Address,
                Email = item.Email,
                Phone = item.Phone
            };
        }

        private static Register DecryptModel(Register item)
        {
            return new Register()
            {
                ID = item.ID,
                DbPassword = Cryptography.Decrypt(item.DbPassword),
                RegisterName = item.RegisterName,
                Address = item.Address,
                Email = item.Email,
                Phone = item.Phone
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
            
            item = DecryptModel(BuildModel(reader));

            return item;
        }

        private static Register GetRegisterByLoginAndPassword(string username, string password)
        {
            string sql = "SELECT * FROM Register WHERE Login=@Login AND Password=@Password";
            DbParameter par1 = Database.AddParameter("AdminDB", "@Login", username);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Password", password);

            try
            {
                DbDataReader reader = Database.GetData(Database.GetConnection("AdminDB"), sql, par1, par2);
                reader.Read();
                return new Register()
                {
                    ID = Int32.Parse(reader["ID"].ToString()),
                    Login = reader["Login"].ToString(),
                    Password = reader["Password"].ToString(),
                    DbName = reader["DbName"].ToString(),
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static int EditRegister(Register o)
        {
            o = EncryptModel(o);

            string sql = "UPDATE Register SET Login=@Login,Password=@Password,DbName=@DbName,DbLogin=@DbLogin,DbPassword=@DbPassword,RegisterName=@RegisterName,Address=@Address,Email=@Email,Phone=@Phone WHERE ID=@ID";
            DbParameter par1  = Database.AddParameter("AdminDB", "@Login", o.Login);
            DbParameter par2  = Database.AddParameter("AdminDB", "@Password", o.Password);
            DbParameter par3  = Database.AddParameter("AdminDB", "@DbName", o.DbName);
            DbParameter par4  = Database.AddParameter("AdminDB", "@DbLogin", o.DbLogin);
            int idDb = Database.InsertData(Database.GetConnection("AdminDB"), sql, par1, par2, par3, par4, par5, par6, par7, par8, par9, par10);
            return idDb;

        }
        private static int InsertRegister(Register o)
        {
            o.DbName = "CashlessCustomer_" + o.DbName;
            o = EncryptModel(o);

            string sql = "INSERT INTO Register VALUES(@Login,@Password,@DbName,@DbLogin,@DbPassword,@RegisterName,@Address,@Email,@Phone)";
            DbParameter par1 = Database.AddParameter("AdminDB", "@Login", o.Login);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Password", o.Password);
            DbParameter par3 = Database.AddParameter("AdminDB", "@DbName", o.DbName);
            DbParameter par4 = Database.AddParameter("AdminDB", "@DbLogin", o.DbLogin);
            DbParameter par5 = Database.AddParameter("AdminDB", "@DbPassword", o.DbPassword);
            DbParameter par6 = Database.AddParameter("AdminDB", "@RegisterName", o.RegisterName);
            DbParameter par7 = Database.AddParameter("AdminDB", "@Address", o.Address);
            DbParameter par8 = Database.AddParameter("AdminDB", "@Email", o.Email);
            DbParameter par9 = Database.AddParameter("AdminDB", "@Phone", o.Phone);
            int id = Database.InsertData(Database.GetConnection("AdminDB"), sql, par1, par2, par3, par4, par5, par6, par7, par8, par9);

            CreateDatabase(DecryptModel(o));

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