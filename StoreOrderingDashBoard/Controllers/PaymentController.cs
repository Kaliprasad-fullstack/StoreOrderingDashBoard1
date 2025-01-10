using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAL;
using DAL;
using DbLayer.Service;
using OfficeOpenXml;
using StoreOrderingDashBoard.Models;
using System.IO;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.Security;
using DbLayer;
using System.Data.Entity.Validation;
using ClosedXML.Excel;
using System.Data;

namespace StoreOrderingDashBoard.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IStoreService _storeService;
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        private readonly ILocationService _locationService;
        private readonly IWareHouseService _wareHouseService;
        private readonly IMenuService _menuService;
        private readonly IAuditService _auditService;
        private readonly IUserService _userService;
        private readonly IItemService _itemService;
        StoreContext db = new StoreContext();
        // GET: Payment
        public ActionResult Index()
        {
            return View();
        }
    }
}