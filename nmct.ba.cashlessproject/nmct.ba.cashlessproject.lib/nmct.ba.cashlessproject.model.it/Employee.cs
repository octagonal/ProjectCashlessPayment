using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model.it
{
    public class Employee
    {
        private int _id;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _employeeName;

        public string EmployeeName
        {
            get { return _employeeName; }
            set { _employeeName = value; }
        }

        private string _address;

        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        private string _phone;

        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        private string _nationalNumber;

        public string NationalNumber
        {
            get { return  _nationalNumber; }
            set {  _nationalNumber = value; }
        }

	private Boolean _Hidden;

	public Boolean Hidden
	{
		get { return _Hidden; }
		set { _Hidden = value; }
	}
        
    }
}
