using nmct.ba.cashlessproject.model.it;
using nmct.ba.cashlessproject.web.Models.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace nmct.ba.cashlessproject.web.Controllers.API
{
    [Authorize]
    public class Register_EmployeeController : ApiController
    {
        public List<Register_Employee> Get()
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return Register_EmployeeDA.GetRegister_Employees(p.Claims);
        }

        public HttpResponseMessage Post(Register_Employee c)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            int id = Register_EmployeeDA.InsertRegister_Employee(c, p.Claims);

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent(id.ToString());
            return message;
        }

        public HttpResponseMessage Put(Register_Employee c)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            Register_EmployeeDA.UpdateRegister_Employee(c, p.Claims);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public HttpResponseMessage Delete(Register_Employee c)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            Register_EmployeeDA.DeleteRegister_Employee(c, p.Claims);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
