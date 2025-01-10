using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DbLayer.Service;
using DAL;
using StoreOrderingDashBoard.Models;
using System.Web.Script.Serialization;
using BAL;
using OfficeOpenXml;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
namespace StoreOrderingDashBoard.Controllers
{
    public class EditOrderController : Controller
    {
        private readonly IItemService _itemService;
        private readonly IOrderService _orderService;
        private readonly IStoreService _storeService;
        private readonly IAuditService _auditService;
        private readonly IReportService _reportService;
        private readonly ICustomerService _customerService;
        private readonly IWareHouseService _wareHouseService;
        public EditOrderController(IItemService itemService, IOrderService orderService, IStoreService storeService, IAuditService auditService, IReportService reportService, ICustomerService customerService, IWareHouseService wareHouseService)
        {
            _itemService = itemService;
            _orderService = orderService;
            _storeService = storeService;
            _auditService = auditService;
            _reportService = reportService;
            _customerService = customerService;
            _wareHouseService = wareHouseService;
        }
        // GET: EditOrder
        public ActionResult Index()
        {
            return View();
        }

    }
}