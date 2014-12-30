using nmct.ba.cashlessproject.model.it;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nmct.ba.cashlessproject.web.Models.VM
{
    public class Organisation_RegisterVM
    {

        public Organisation_RegisterVM()
        {
            OrganisationList = OrganisationDA.ReadOrganisations();
            RegisterList = RegisterDA.ReadRegisters();
            ORInstance = new Organisation_Register();
        }

        public static List<Organisation_RegisterVM> FillVMList()
        {
            List<Organisation_Register> orgs = Organisation_RegisterDA.ReadOrganisation_Registers();
            List<Organisation_RegisterVM> vmlist = new List<Organisation_RegisterVM>();
            foreach (Organisation_Register item in orgs)
            {
                vmlist.Add(new Organisation_RegisterVM() { ORInstance = item });
            }

            return vmlist;
        }

        private Organisation_Register _orInstance;
        public Organisation_Register ORInstance
        {
            get { return _orInstance; }
            set { _orInstance = value;}
        }

        private List<Register> _registerList;
        public List<Register> RegisterList
        {
            get { return _registerList; }
            set { _registerList = value; }
        }
        public IEnumerable<SelectListItem> RegisterSelect
        {
            get { return new SelectList(RegisterList, "ID", "RegisterName"); }
        }

        private List<Organisation> _organisationList;
        public List<Organisation> OrganisationList
        {
            get { return _organisationList; }
            set { _organisationList = value; }
        }
        public IEnumerable<SelectListItem> OrganisationSelect
        {
            get { return new SelectList(OrganisationList, "ID", "OrganisationName"); }
        }

        private int _newRegID;

        public int NewRegID
        {
            get { return _newRegID; }
            set { _newRegID = value; }
        }

        private int _newOrgID;

        public int NewOrgID
        {
            get { return _newOrgID; }
            set { _newOrgID = value; }
        }

        public string RegFriendlyName
        {
            get { return ORInstance.RegisterID + " (" + RegisterList.Find(x => x.ID == ORInstance.RegisterID).RegisterName + ")"; }
            //set { _regFriendlyName = RegisterList.Find(x => x.ID == ORInstance.RegisterID).RegisterName; }
        }

        public string OrgFriendlyName
        {
            get { return ORInstance.OrganisationID + " (" + OrganisationList.Find(x => x.ID == ORInstance.OrganisationID).OrganisationName + ")"; }
            //set { _orgFriendlyName = OrganisationList.Find(x => x.ID == ORInstance.OrganisationID).OrganisationName; }
        }
    }
}