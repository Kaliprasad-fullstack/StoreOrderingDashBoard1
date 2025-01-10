using System;
using System.Web.Http;
using DbLayer.Service;
using DbLayer.Repository;
using DAL;
using System.Collections.Generic;
using System.Linq;

namespace StoreOrderingDashBoard.Controllers
{
    public class WebApiController : ApiController
    {
        private readonly IOrderService _orderService;

        public WebApiController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public WebApiController()
        {

        }

        //[Authorize]
        [Route("api/data/NetSuit")]
        [HttpPost]
        public IHttpActionResult Get([FromBody]DateParameter param)
        {
            var fromDate = Convert.ToDateTime(param.FromDate);
            var todate = Convert.ToDateTime(param.ToDate);
            OrderRepository repository = new OrderRepository();
            var netsuit = repository.netSuitUploads(fromDate, todate);
            return Json(netsuit);
        }
        [Route("api/data/InboundOrder")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]List<OrderInbound> orders)
        {
            List<OrderInboundResponse> orderInboundResponses = new List<OrderInboundResponse>();
            if (orders != null && orders.Count() > 0)
                orderInboundResponses = _orderService.AddOrderInbound(orders);
            return Json(orderInboundResponses);
        }

        //[Authorize]
        //[HttpPost]
        //[Route("api/data/authenticate")]
        //public IHttpActionResult Getforauthenticate()
        //{
        //    var jsonSerialiser = new JavaScriptSerializer();
        //   // var date = jsonSerialiser.Deserialize<DateParameter>(Date);

        //    return Json("HELOO");
        //}

        //[Authorize]
        //[Route("api/deliveryitems/{anyString}")]
        //[HttpGet, HttpPost]
        //public HttpResponseMessage GetDeliveryItemsOne(string anyString)
        //{
        //    return Request.CreateResponse<string>(HttpStatusCode.OK, anyString);
        //}

        //[Route("api/Add1/{anyString}")]
        //public IHttpActionResult Add1(string title)
        //{
        //    //Creates a Movie based on the Title
        //    return Ok();
        //}

        //[HttpPost]
        //public IHttpActionResult Add(string title)
        //{
        //    //Creates a Movie based on the Title
        //    return Ok();
        //}
        //[Authorize]
        //[HttpGet]
        //[Route("api/data/authenticates")]
        //public string Getforauthenticates()
        //{

        //    List<string> st = new List<string>();
        //    st.Add("s");
        //    st.Add("p");
        //    st.Add("G");
        //    JavaScriptSerializer js = new JavaScriptSerializer();

        //    return js.Serialize(st);
        //}


        //[Authorize(Roles = "Admin")]
        //[HttpGet]
        //[Route("api/data/authorize")]
        //public IHttpActionResult Getforadmin()
        //{
        //    var identity = (ClaimsIdentity)User.Identity;
        //    var roles = identity.Claims
        //              .Where(c => c.Type == ClaimTypes.Role)
        //              .Select(c => c.Value);
        //    return Json("Hello" + identity.Name + "Role" + string.Join(",", roles.ToString()));
        //}
    }
}
