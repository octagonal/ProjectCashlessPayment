using nmct.ba.cashlessproject.helper;
using nmct.ba.cashlessproject.model.it;
using nmct.ba.cashlessproject.web.Helper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace nmct.ba.cashlessproject.web.Models
{
    public class OrganisationDA
    {
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

        public static List<Organisation> GetOrganisations()
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

    }
}