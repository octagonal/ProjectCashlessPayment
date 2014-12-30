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

        private Organisation_Register _orInstance;
        public Organisation_Register ORInstance
        {
            get { return _orInstance; }
            set { _orInstance = value; }
        }

        private List<Register> _registerList;
        private List<Register> RegisterList
        {
            get { return _registerList; }
            set { _registerList = value; }
        }
        public IEnumerable<SelectListItem> RegisterSelect
        {
            get { return new SelectList(RegisterList, "ID", "RegisterName"); }
        }

        private List<Organisation> _organisationList;
        private List<Organisation> OrganisationList
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
        
        
        
    }
}