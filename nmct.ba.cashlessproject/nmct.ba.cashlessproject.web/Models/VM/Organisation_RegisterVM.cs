using nmct.ba.cashlessproject.model.it;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public List<Register> RegisterList
        {
            get { return _registerList; }
            set { _registerList = value; }
        }

        private List<Organisation> _organisationList;
        public List<Organisation> OrganisationList
        {
            get { return _organisationList; }
            set { _organisationList = value; }
        }
        
        
    }
}