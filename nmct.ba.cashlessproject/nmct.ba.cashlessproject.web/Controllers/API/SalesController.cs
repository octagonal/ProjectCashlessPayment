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

        public List<Sale> Get(string type, int id, long periodStart, long periodEnd)
        {
            DateTime periodStartDT = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(periodStart);
            DateTime periodEndDT = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(periodEnd);
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            if(type == "register")
                return SaleDA.GetSalesByDateAndType(id, "RegisterID", new DateTime(1970,1,1,0,0,0,DateTimeKind.Utc).AddSeconds(periodStart), new DateTime(1970,1,1,0,0,0,DateTimeKind.Utc).AddSeconds(periodEnd), p.Claims);
            if(type == "product")
                return SaleDA.GetSalesByDateAndType(id, "ProductID", new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(periodStart), new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(periodEnd), p.Claims);
            //if (type == "all")
            return SaleDA.GetSalesByDate(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(periodStart), new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(periodEnd), p.Claims);
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
