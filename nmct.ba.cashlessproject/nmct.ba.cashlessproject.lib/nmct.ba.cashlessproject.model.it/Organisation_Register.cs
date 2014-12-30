using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model.it
{
    public class Organisation_Register
    {
        private int _organisationID;

        public int OrganisationID
        {   
            get { return _organisationID; }
            set { _organisationID = value; }
        }

        private int _registerID;

        public int RegisterID
        {
            get { return _registerID; }
            set { _registerID = value; }
        }

        private DateTime _fromDate;

        public DateTime FromDate
        {
            get { return _fromDate; }
            set { _fromDate = value; }
        }

        private DateTime _untilDate;

        public DateTime UntilDate
        {
            get { return _untilDate; }
            set { _untilDate = value; }
        }
        
        
    }
}
