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
    public class ErrorlogDA
    {
        #region Public CRUD functies
        public static int               UpdateErrorlog(Errorlog item)
        {
            return EditErrorlog(item);
        }
        public static int               DeleteErrorlog(int id)
        {
            return DeleteErrorlog(GetErrorlog(id));
        }
        public static Errorlog          ReadErrorlog(int id)
        {
            return GetErrorlog(id);
        }
	public static List<Errorlog> ReadErrorlogs(int id)
	{
		return GetErrorlogs(id);
	}
        public static List<Errorlog>    ReadErrorlogs()
        {
            return GetErrorlogs();
        }
        public static int               CreateErrorlog(Errorlog o)
        {
            return InsertErrorlog(o);
        }
        #endregion

        #region Logica
        private static Errorlog BuildModel(DbDataReader reader)
        {
            return new Errorlog()
            {
		Message = reader["Message"].ToString(),
		RegisterID = Int32.Parse(reader["RegisterID"].ToString()),
		Stacktrace = reader["Stacktrace"].ToString(),
		Timestamp = Convert.ToDateTime(reader["Timestamp"].ToString()),
            };
        }

        private static List<Errorlog> GetErrorlogs()
        {
            List<Errorlog> list = new List<Errorlog>();
            string sql = "SELECT * FROM Errorlog";
            DbDataReader reader = Database.GetData("AdminDB", sql);

            while (reader.Read())
            {
                list.Add(BuildModel(reader));
            }

            return list;
        }

	private static List<Errorlog> GetErrorlogs(int id)
	{
		List<Errorlog> list = new List<Errorlog>();
		string sql = "SELECT * FROM Errorlog WHERE RegisterID=@RegisterID";
		DbParameter par1 = Database.AddParameter("AdminDB", "@RegisterID", id);

		DbDataReader reader = Database.GetData("AdminDB", sql,par1);

		while (reader.Read())
		{
			list.Add(BuildModel(reader));
		}

		return list;
	}

        private static Errorlog GetErrorlog(int id)
        {
            Errorlog item = new Errorlog();
            string sql = "SELECT * FROM Errorlog WHERE RegisterID=@RegisterID";
            DbParameter par1 = Database.AddParameter("AdminDB","@RegisterID", id);

            DbDataReader reader = Database.GetData("AdminDB", sql, par1);
            reader.Read();
            
            item = (BuildModel(reader));

            return item;
        }

        private static int EditErrorlog(Errorlog o)
        {
	    string sql = "UPDATE Errorlog SET Message=@Message,Stacktrace=@Stacktrace,Timestamp=@Timestamp WHERE RegisterID=@RegisterID";
	    DbParameter par1 = Database.AddParameter("AdminDB", "@RegisterID", o.RegisterID);
	    DbParameter par2 = Database.AddParameter("AdminDB", "@Message", o.Message);
	    DbParameter par3 = Database.AddParameter("AdminDB", "@Stacktrace", o.Stacktrace);
	    DbParameter par4 = Database.AddParameter("AdminDB", "@Timestamp", o.Timestamp);
            int idDb = Database.InsertData(Database.GetConnection("AdminDB"), sql, par1, par2, par3, par4);
            return idDb;

        }
        private static int InsertErrorlog(Errorlog o)
        {
	    string sql = "INSERT INTO Errorlog VALUES(@RegisterID,@Timestamp,@Message,@Stacktrace)";
	    DbParameter par1 = Database.AddParameter("AdminDB", "@RegisterID", o.RegisterID);
	    DbParameter par2 = Database.AddParameter("AdminDB", "@Message", o.Message);
	    DbParameter par3 = Database.AddParameter("AdminDB", "@Stacktrace", o.Stacktrace);
	    DbParameter par4 = Database.AddParameter("AdminDB", "@Timestamp", o.Timestamp);
            int id = Database.InsertData(Database.GetConnection("AdminDB"), sql, par1, par2, par3, par4);
            return id;
        }

        private static int DeleteErrorlog(Errorlog o)
        {
		string sql = "DELETE FROM Errorlog WHERE RegisterID=@RegisterID";
	    DbParameter par1 = Database.AddParameter("AdminDB", "@RegisterID", o.RegisterID);
            int id = Database.InsertData(Database.GetConnection("AdminDB"), sql, par1);
            return id;
        }
        #endregion
    }
}