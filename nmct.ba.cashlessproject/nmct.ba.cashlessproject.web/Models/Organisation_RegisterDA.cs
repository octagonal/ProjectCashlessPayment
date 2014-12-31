using nmct.ba.cashlessproject.helper;
using nmct.ba.cashlessproject.model.it;
using nmct.ba.cashlessproject.web.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace nmct.ba.cashlessproject.web.Models
{
    public class Organisation_RegisterDA
    {
        #region Public CRUD functies
        public static int               UpdateOrganisation_Register(Organisation_Register item, int newReg, int newOrg)
        {
            return EditOrganisation_Register(item, newReg, newOrg);
        }
        public static int               DeleteOrganisation_Register(int regId, int orgId)
        {
            return DeleteOrganisation_Register(GetOrganisation_Register(regId, orgId));
        }
        public static Organisation_Register          ReadOrganisation_Register(int regId, int orgId)
        {
            return GetOrganisation_Register(regId, orgId);
        }
        public static List<Organisation_Register>    ReadOrganisation_Registers()
        {
            return GetOrganisation_Registers();
        }
        public static int               CreateOrganisation_Register(Organisation_Register o)
        {
            return InsertOrganisation_Register(o);
        }
        #endregion

        #region Logica
        private static Organisation_Register BuildModel(DbDataReader reader)
        {
            return new Organisation_Register()
            {
                RegisterID =        Convert.ToInt32(reader["RegisterID"]),
                OrganisationID =    Convert.ToInt32(reader["OrganisationID"]),
                FromDate =          Convert.ToDateTime(reader["FromDate"].ToString()),
                UntilDate =         Convert.ToDateTime(reader["UntilDate"].ToString()),
            };
        }

        private static List<Organisation_Register> GetOrganisation_Registers()
        {
            List<Organisation_Register> list = new List<Organisation_Register>();
            string sql = "SELECT * FROM Organisation_Register";
            DbDataReader reader = Database.GetData("AdminDB", sql);

            while (reader.Read())
            {
                list.Add(BuildModel(reader));
            }

            return list;
        }

        private static Organisation_Register GetOrganisation_Register(int registerID, int organisationID)
        {
            Organisation_Register item = new Organisation_Register();
            string sql = "SELECT * FROM Organisation_Register WHERE RegisterID=@RegisterID AND OrganisationID=@OrganisationID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@RegisterID", registerID);
            DbParameter par2 = Database.AddParameter("AdminDB", "@OrganisationID", organisationID);

            DbDataReader reader = Database.GetData("AdminDB", sql, par1, par2);
            reader.Read();
            
            item = (BuildModel(reader));

            return item;
        }

        #region OrgDb Helpers

        private static ConnectionStringSettings createConString(Organisation org)
        {
            // hic sunt dracones
            return nmct.ba.cashlessproject.web.Models.API.RegisterDA.CreateConnectionStringBase(org.DbName, org.DbLogin, org.DbPassword);
        }
        private static int AssignToOrgDb(Organisation_Register c)
        {
            Organisation org = OrganisationDA.ReadOrganisation(c.OrganisationID);
            Register reg = RegisterDA.ReadRegister(c.RegisterID);

            string sql = "INSERT INTO Register VALUES(@RegisterName,@Device)";
            DbParameter par1 = Database.AddParameter("AdminDB", "@RegisterName", reg.RegisterName);
            DbParameter par2 = Database.AddParameter("AdminDB", "@Device", reg.Device);

            return Database.InsertData(Database.GetConnection(createConString(org)), sql, par1, par2);
        }

        private static int DeleteFromOrgDb(Organisation_Register c)
        {
            Organisation org = OrganisationDA.ReadOrganisation(c.OrganisationID);
            Register reg = RegisterDA.ReadRegister(c.RegisterID);

            string sql = "DELETE FROM Register WHERE ID=@ID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@ID", reg.ID);

            return Database.InsertData(Database.GetConnection(createConString(org)), sql, par1);
        }
        #endregion

        private static int EditOrganisation_Register(Organisation_Register o, int newRegID, int newOrgID)
        {
            string sql = "UPDATE Organisation_Register SET RegisterID=@NewRegisterID, OrganisationID=@NewOrganisationID, FromDate=@FromDate, UntilDate=@UntilDate WHERE RegisterID=@RegisterID AND OrganisationID=@OrganisationID";
            DbParameter par1 = Database.AddParameter("AdminDB", "@NewRegisterID", newRegID);
            DbParameter par2 = Database.AddParameter("AdminDB", "@NewOrganisationID", newOrgID);
            DbParameter par3 = Database.AddParameter("AdminDB", "@FromDate", o.FromDate);
            DbParameter par4 = Database.AddParameter("AdminDB", "@UntilDate", o.UntilDate);
            DbParameter par5 = Database.AddParameter("AdminDB", "@RegisterID", o.RegisterID);
            DbParameter par6 = Database.AddParameter("AdminDB", "@OrganisationID", o.OrganisationID);
            int idDb = Database.InsertData(Database.GetConnection("AdminDB"), sql, par1, par2, par3, par4, par5, par6);
            
            if (newOrgID != o.OrganisationID)
            {
                DeleteFromOrgDb(o);

                o.RegisterID = newRegID;
                o.OrganisationID = newOrgID;
                AssignToOrgDb(o);
            }
            return idDb;
        }
        private static int InsertOrganisation_Register(Organisation_Register o)
        {
            string sql = "INSERT INTO Organisation_Register VALUES(@OrganisationID,@RegisterID,@FromDate,@UntilDate)";
            DbParameter par1 = Database.AddParameter("AdminDB", "@RegisterID", o.RegisterID);
            DbParameter par2 = Database.AddParameter("AdminDB", "@OrganisationID", o.OrganisationID);
            DbParameter par3 = Database.AddParameter("AdminDB", "@FromDate", o.FromDate);
            DbParameter par4 = Database.AddParameter("AdminDB", "@UntilDate", o.UntilDate);
            int id = Database.InsertData(Database.GetConnection("AdminDB"), sql, par1, par2, par3, par4);
            AssignToOrgDb(o);
            return id;
        }

        private static int DeleteOrganisation_Register(Organisation_Register o)
        {
            string sql = "DELETE FROM Organisation_Register WHERE RegisterID=@RegisterID AND OrganisationID=@OrganisationID AND FromDate=@FromDate AND UntilDate=@UntilDate";
            DbParameter par1 = Database.AddParameter("AdminDB", "@RegisterID", o.RegisterID);
            DbParameter par2 = Database.AddParameter("AdminDB", "@OrganisationID", o.OrganisationID);
            DbParameter par3 = Database.AddParameter("AdminDB", "@FromDate", o.FromDate);
            DbParameter par4 = Database.AddParameter("AdminDB", "@UntilDate", o.UntilDate);
            int id = Database.InsertData(Database.GetConnection("AdminDB"), sql, par1, par2, par3, par4);
            DeleteFromOrgDb(o);
            return id;
        }
        #endregion
    }
}