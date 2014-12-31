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
    public class RegisterController : ApiController
    {
        public List<Register> Get()
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return RegisterDA.GetRegisters(p.Claims);
        }

        public HttpResponseMessage Post(Register c)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            int id = RegisterDA.InsertRegister(c, p.Claims);

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent(id.ToString());
            return message;
        }

        public HttpResponseMessage Put(Register c)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            RegisterDA.UpdateRegister(c, p.Claims);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public HttpResponseMessage Delete(int id)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            RegisterDA.DeleteRegister(id, p.Claims);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
