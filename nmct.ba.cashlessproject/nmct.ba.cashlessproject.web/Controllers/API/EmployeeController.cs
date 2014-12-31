using nmct.ba.cashlessproject.model.it;
using nmct.ba.cashlessproject.web.Models;
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
    public class EmployeeController : ApiController
    {
        public List<Employee> Get()
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return EmployeeDA.GetEmployees(p.Claims);
        }

        public HttpResponseMessage Post(Employee c)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            int id = EmployeeDA.InsertEmployee(c, p.Claims);

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent(id.ToString());
            return message;
        }

        public HttpResponseMessage Put(Employee c)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            EmployeeDA.UpdateEmployee(c, p.Claims);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public HttpResponseMessage Delete(int id)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            EmployeeDA.DeleteEmployee(id, p.Claims);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
