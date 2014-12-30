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
    public class OrganisationDA
    {
        #region Public CRUD functies
        public static int                   UpdateOrganisation(Organisation item)
        {
            return EditOrganisation(item);
        }
        public static int                   DeleteOrganisation(int id)
        {
            return DeleteOrganisation(GetOrganisation(id));
        }
        public static Organisation          ReadOrganisation(int id)
        {
            return GetOrganisation(id);
        }
        public static List<Organisation>    ReadOrganisations()
        {
            return GetOrganisations();
        }
        public static int                   CreateOrganisation(Organisation o)
        {
            return InsertOrganisation(o);
        }
        #endregion

        #region Logica
        public static Organisation CheckCredentials(string username, string password)
        {
            string sql = "SELECT * FROM Organisation WHERE Login=@Login AND Password=@Password";
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

        private static Organisation BuildModel(DbDataReader reader)
        {
            return new Organisation()
            {
                ID = Int32.Parse(reader["ID"].ToString()),
                Login = reader["Login"].ToString(),
                Password = reader["Password"].ToString(),
                DbName = reader["DbName"].ToString(),
                DbLogin = reader["DbLogin"].ToString(),
                DbPassword = reader["DbPassword"].ToString(),
                OrganisationName = reader["OrganisationName"].ToString(),
                Address = reader["Address"].ToString(),
                Email = reader["Email"].ToString(),
                Phone = reader["Phone"].ToString()
            };
        }

        private static Organisation EncryptModel(Organisation item)
        {
            return new Organisation()
            {
                ID = item.ID,
                Login = Cryptography.Encrypt(item.Login),
                Password = Cryptography.Encrypt(item.Password),
                DbName = Cryptography.Encrypt(item.DbName),
                DbLogin = Cryptography.Encrypt(item.DbLogin),
                DbPassword = Cryptography.Encrypt(item.DbPassword),
                OrganisationName = item.OrganisationName,
                Address = item.Address,
                Email = item.Email,
                Phone = item.Phone
            };
        }

        private static Organisation DecryptModel(Organisation item)
        {
            return new Organisation()
            {
                ID = item.ID,
                Login = Cryptography.Decrypt(item.Login),
                Password = Cryptography.Decrypt(item.Password),
                DbName = Cryptography.Decrypt(item.DbName),
                DbLogin = Cryptography.Decrypt(item.DbLogin),
                DbPassword = Cryptography.Decrypt(item.DbPassword),
                OrganisationName = item.OrganisationName,
                Address = item.Address,
                Email = item.Email,
                Phone = item.Phone
            };
        }

        private static List<Organisation> GetOrganisations()
        {
            List<Organisation> list = new List<Organisation>();
            string sql = "SELECT * FROM Organisation";
            DbDataReader reader = Database.GetData("AdminDB", sql);

            while (reader.Read())
            {
                list.Add(BuildModel(reader));
            }

            return list;
        }

        private static Organisation GetOrganisation(int id)
        {
            Organisation item = new Organisation();
            string sql = "SELECT * FROM Organisation WHERE Id=@Id";
            DbParameter par1 = Database.AddParameter("AdminDB","@Id", id);

            DbDataReader reader = Database.GetData("AdminDB", sql, par1);
            reader.Read();
            
            item = DecryptModel(BuildModel(reader));

            return item;
        }

        private static Organisation GetOrganisationByLoginAndPassword(string username, string password)
        {
            string sql = "SELECT * FROM Organisation WHERE Login=@Login AND Password=@Password";
            DbParameter par1 = Database.AddParameter("AdminDB", "@Login", username);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Password", password);

            try
            {
                DbDataReader reader = Database.GetData(Database.GetConnection("AdminDB"), sql, par1, par2);
                reader.Read();
                return new Organisation()
                {
                    ID = Int32.Parse(reader["ID"].ToString()),
                    Login = reader["Login"].ToString(),
                    Password = reader["Password"].ToString(),
                    DbName = reader["DbName"].ToString(),
                    DbLogin = reader["DbLogin"].ToString(),
                    DbPassword = reader["DbPassword"].ToString(),
                    OrganisationName = reader["OrganisationName"].ToString(),
                    Address = reader["Address"].ToString(),
                    Email = reader["Email"].ToString(),
                    Phone = reader["Phone"].ToString()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private static int EditOrganisation(Organisation o)
        {
            o = EncryptModel(o);

            string sql = "UPDATE Organisation SET Login=@Login,Password=@Password,DbName=@DbName,DbLogin=@DbLogin,DbPassword=@DbPassword,OrganisationName=@OrganisationName,Address=@Address,Email=@Email,Phone=@Phone WHERE ID=@ID";
            DbParameter par1  = Database.AddParameter("AdminDB", "@Login", o.Login);
            DbParameter par2  = Database.AddParameter("AdminDB", "@Password", o.Password);
            DbParameter par3  = Database.AddParameter("AdminDB", "@DbName", o.DbName);
            DbParameter par4  = Database.AddParameter("AdminDB", "@DbLogin", o.DbLogin);
            DbParameter par5  = Database.AddParameter("AdminDB", "@DbPassword", o.DbPassword);
            DbParameter par6  = Database.AddParameter("AdminDB", "@OrganisationName", o.OrganisationName);
            DbParameter par7  = Database.AddParameter("AdminDB", "@Address", o.Address);
            DbParameter par8  = Database.AddParameter("AdminDB", "@Email", o.Email);
            DbParameter par9  = Database.AddParameter("AdminDB", "@Phone", o.Phone);
            DbParameter par10 = Database.AddParameter("AdminDB", "@ID", o.ID);
            int idDb = Database.InsertData(Database.GetConnection("AdminDB"), sql, par1, par2, par3, par4, par5, par6, par7, par8, par9, par10);
            return idDb;

        }
        private static int InsertOrganisation(Organisation o)
        {
            o.DbName = "CashlessCustomer_" + o.DbName;
            o = EncryptModel(o);

            string sql = "INSERT INTO Organisation VALUES(@Login,@Password,@DbName,@DbLogin,@DbPassword,@OrganisationName,@Address,@Email,@Phone)";
            DbParameter par1 = Database.AddParameter("AdminDB", "@Login", o.Login);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Password", o.Password);
            DbParameter par3 = Database.AddParameter("AdminDB", "@DbName", o.DbName);
            DbParameter par4 = Database.AddParameter("AdminDB", "@DbLogin", o.DbLogin);
            DbParameter par5 = Database.AddParameter("AdminDB", "@DbPassword", o.DbPassword);
            DbParameter par6 = Database.AddParameter("AdminDB", "@OrganisationName", o.OrganisationName);
            DbParameter par7 = Database.AddParameter("AdminDB", "@Address", o.Address);
            DbParameter par8 = Database.AddParameter("AdminDB", "@Email", o.Email);
            DbParameter par9 = Database.AddParameter("AdminDB", "@Phone", o.Phone);
            int id = Database.InsertData(Database.GetConnection("AdminDB"), sql, par1, par2, par3, par4, par5, par6, par7, par8, par9);

            CreateDatabase(DecryptModel(o));

            return id;
        }

        private static int DeleteOrganisation(Organisation o)
        {
            string sql = "DELETE FROM Organisation WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@ID", o.ID);
            int id = Database.InsertData(Database.GetConnection("AdminDB"), sql, par1);
            return id;
        }

        private static void CreateDatabase(Organisation o)
        {
            // create the actu  al database
            string create = File.ReadAllText(HostingEnvironment.MapPath(@"~/App_Data/create.txt")); // only for the web
            //string create = File.ReadAllText(@"..\..\Data\create.txt"); // only for desktop
            string sql = create.Replace("@@DbName", o.DbName).Replace("@@DbLogin", o.DbLogin).Replace("@@DbPassword", o.DbPassword);
            foreach (string commandText in RemoveGo(sql))
            {
                Database.ModifyData(Database.GetConnection("AdminDB"), commandText);
            }

            // create login, user and tables
            DbTransaction trans = null;
            try
            {
                trans = Database.BeginTransaction("AdminDB");

                string fill = File.ReadAllText(HostingEnvironment.MapPath(@"~/App_Data/fill.txt")); // only for the web
                //string fill = File.ReadAllText(@"..\..\Data\fill.txt"); // only for desktop
                string sql2 = fill.Replace("@@DbName", o.DbName).Replace("@@DbLogin", o.DbLogin).Replace("@@DbPassword", o.DbPassword);

                foreach (string commandText in RemoveGo(sql2))
                {
                    Database.ModifyData(trans, commandText);
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                Console.WriteLine(ex.Message);
            }
        }

        private static string[] RemoveGo(string input)
        {
            //split the script on "GO" commands
            string[] splitter = new string[] { "\r\nGO\r\n" };
            string[] commandTexts = input.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            return commandTexts;
        }
        #endregion
    }
}