using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model.it
{
    public class Register_Employee
    {
        private int _registerID;

        public int RegisterID
        {
            get { return _registerID; }
            set { _registerID = value; }
        }

        private int _employeeID;

        public int EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }

        private DateTime _fromTime;

        public DateTime FromTime
        {
            get { return _fromTime; }
            set { _fromTime = value; }
        }

        private DateTime _untilTime;

        public DateTime UntilTime
        {
            get { return _untilTime; }
            set { _untilTime = value; }
        }

    }
}
