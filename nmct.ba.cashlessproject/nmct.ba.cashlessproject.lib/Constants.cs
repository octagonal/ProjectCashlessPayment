using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.lib
{
    public class Constants
    {
        public const string WEBURL = "http://localhost:43622/";

        public static Dictionary<string, string> MockCredentials = new Dictionary<string, string>(){
            { "Username"        , "admin"                                                                                           },
            { "Password"        , "password"                                                                                        },
            { "RegisterID"      , "1"                                                                                               },
            { "ConnectionString", "Data Source=.;Initial Catalog=CashlessCustomer_testDb;User ID=testDbLogin;Password=testDbPass"   }
        };
    }
}
