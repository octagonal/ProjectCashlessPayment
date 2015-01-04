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
    public class SaleController : ApiController
    {
        public List<Sale> Get()
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return SaleDA.GetSales(p.Claims);
        }

        public HttpResponseMessage Post(Sale c)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            int id = SaleDA.InsertSale(c, p.Claims);

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent(id.ToString());
            return message;
        }
    }
}
