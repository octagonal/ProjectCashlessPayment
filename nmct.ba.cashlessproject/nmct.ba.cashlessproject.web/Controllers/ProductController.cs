using nmct.ba.cashlessproject.model.it;
using nmct.ba.cashlessproject.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace nmct.ba.cashlessproject.web.Controllers
{
    [Authorize]
    public class ProductController : ApiController
    {
        public List<Product> Get()
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return ProductDA.GetProducts(p.Claims);
        }

        public HttpResponseMessage Post(Product c)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            int id = ProductDA.InsertProduct(c, p.Claims);

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent(id.ToString());
            return message;
        }

        public HttpResponseMessage Put(Product c)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            ProductDA.UpdateProduct(c, p.Claims);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public HttpResponseMessage Delete(int id)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            ProductDA.DeleteProduct(id, p.Claims);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
