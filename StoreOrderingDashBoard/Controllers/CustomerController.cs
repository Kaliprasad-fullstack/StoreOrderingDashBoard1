using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAL;
using DAL;
using DbLayer.Service;
using StoreOrderingDashBoard.Models;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml;

namespace StoreOrderingDashBoard.Controllers
{

    public class CustomerController : Controller
    {
        // GET: Customer
        private readonly ICustomerService _customerService;
        private readonly IStoreService _storeService;
        //private readonly IItemService _itemService;
        private readonly IAuditService _auditService;
        private readonly IMenuService _menuService;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        public CustomerController(ICustomerService customerService, IStoreService storeService, IAuditService AuditService, IMenuService menuService, IUserService userService, IItemService itemService, IOrderService orderService)
        {
            _customerService = customerService;
            _storeService = storeService;
            _auditService = AuditService;
            _menuService = menuService;
            _userService = userService;
            //_itemService = itemService;
            _orderService = orderService;
        }
        [AdminAuthorization]
        public ActionResult Index()
        {
            try
            {
                var customers = _customerService.GetAllCustomers();
                ViewBag.customers = customers;
                return View();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return RedirectToAction("Error500", "Home");
            }
        }

        [AdminAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCustomer(Customer customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var cust = _customerService.Customer(customer.Name);
                    if (cust == null)
                    {
                        //customer.ModifiedDate = DateTime.Now;
                        customer.Company = _customerService.Company(customer.Company.Id);
                        customer.Created = DateTime.Now;
                        customer.CreatedBy = SessionValues.UserId;
                        customer.CustomerPlan = _customerService.CustomerPlan(customer.CustomerPlan.Id);

                        int rowsaffected = _customerService.AddCustomer(customer);
                        //try
                        //{
                        //    var javascript = new JavaScriptSerializer();
                        //    var Auditcust = new Customer(customer.Id, customer.Name, customer.IsDeleted, customer.ModifiedDate, customer.Created, customer.CreatedBy, customer.ModifiedBy, customer.Company.Id, customer.CustomerPlan.Id);
                        //    var str = javascript.Serialize(Auditcust);
                        //    _auditService.InsertAudit(HelperCls.GetIPAddress(), str, Request.Url.OriginalString.ToString(), customer.CreatedBy, null);
                        //}
                        //catch (Exception ex)
                        //{
                        //    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                        //    throw ex;
                        //}
                        var cust2 = _customerService.Customer(customer.Name);
                        if (cust2 != null)
                        {
                            int rowsAffected2 = _customerService.AddUpdateStatus(CustId: cust2.Id, Type: "Order-Delete", Status: customer.StatusRemark, Days: customer.OrderDays, UserId: SessionValues.UserId);
                        }
                        return RedirectToAction("Index", "Customer");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Customer Name already exists.");
                        ViewBag.Companies = _customerService.GetCompanies();
                        ViewBag.CustomerPlan = _customerService.GetCustomerPlans();
                        return View();
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return RedirectToAction("Error500", "Home");
            }
        }

        [AdminAuthorization]
        public ActionResult AddCustomer()
        {
            ViewBag.Companies = _customerService.GetCompanies();
            ViewBag.CustomerPlan = _customerService.GetCustomerPlans();
            ViewBag.CustomerEmailTypes = _customerService.GetCustomerEmailTypes();
            return View();
        }

        [AdminAuthorization]
        public ActionResult EditCustomer(int Id)
        {
            try
            {
                var customer = _customerService.GetCustomerById(Id);
                if (customer != null)
                {
                    ViewBag.Companies = _customerService.GetCompanies();
                    ViewBag.CustomerPlan = _customerService.GetCustomerPlans();
                    List<EmailType> CustomerEmailTypes = _customerService.GetCustomerEmailTypes();
                    List<CustomerEmailMaster> customerEmailMasters = _customerService.GetSelectedEmailsTypesForCustomer(Id);
                    CustomerWiseStatusMst customerWiseStatusMst = _customerService.GetCustomerWiseStatusMst(customer.Id, Type: "Order-Delete");
                    if (customerWiseStatusMst != null)
                    {
                        customer.StatusRemark = customerWiseStatusMst.Status;
                        customer.OrderDays = customerWiseStatusMst.OrderDays;
                    }
                    ViewBag.CustomerEmailTypes = new MultiSelectList(CustomerEmailTypes, "Id", "Name", customerEmailMasters.Select(x => x.EmailTypeId).ToList());
                    return View(customer);
                }
                return RedirectToAction("Error404", "Home");
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return RedirectToAction("Error500", "Home");
            }

        }

        [AdminAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCustomer(Customer customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var customers = _customerService.GetCustomerById(customer.Id);
                    customers.ModifiedDate = DateTime.Now;
                    customers.ModifiedBy = SessionValues.UserId;
                    customers.Company = _customerService.Company(customer.Company.Id);
                    customers.CustomerPlan = _customerService.CustomerPlan(customer.CustomerPlan.Id);
                    customers.HasEscalationMatrix = customer.HasEscalationMatrix;
                    customers.PlannedOrderFlag = customer.PlannedOrderFlag;
                    customers.ItemTypeDCFlag = customer.ItemTypeDCFlag;
                    customers.EmailTypes = customer.EmailTypes;
                    int rowsaffected = _customerService.EditCustomer(customers);
                    _customerService.UpdateCustomerEmailTypes(customers);
                    int rowsAffected2 = _customerService.AddUpdateStatus(CustId: customer.Id, Type: "Order-Delete", Status: customer.StatusRemark, Days: customer.OrderDays, UserId: SessionValues.UserId);
                    try
                    {
                        //var javascript = new JavaScriptSerializer();
                        //var Auditcust = new Customer(customers.Id, customers.Name, customers.IsDeleted, customers.ModifiedDate, customers.Created, customers.CreatedBy, customers.ModifiedBy, customers.Company.Id, customers.CustomerPlan.Id);
                        //var str = javascript.Serialize(Auditcust);
                        //_auditService.InsertAudit(HelperCls.GetIPAddress(), str, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
                    }
                    catch (Exception ex)
                    {
                        HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                    }
                    return RedirectToAction("Index", "Customer");
                }
                else
                {
                    List<EmailType> CustomerEmailTypes = _customerService.GetCustomerEmailTypes();
                    List<CustomerEmailMaster> customerEmailMasters = _customerService.GetSelectedEmailsTypesForCustomer(customer.Id);
                    ViewBag.CustomerEmailTypes = new MultiSelectList(CustomerEmailTypes, "Id", "Name", customerEmailMasters.Select(x => x.EmailTypeId).ToList());
                    return View();
                }
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return RedirectToAction("Error500", "Home");
            }
        }

        [AdminAuthorization]
        public ActionResult DeleteCustomer(Customer customer)
        {
            try
            {
                var Customer1 = _customerService.GetCustomerById(customer.Id);
                var stores = _storeService.GetStoresByCustomerId(customer.Id);
                int rowsAffected = 0;
                if (stores != null)
                {
                    foreach (Store store in stores)
                    {
                        rowsAffected += _storeService.DeleteStore(store);
                        try
                        {
                            var storeJavascript = new JavaScriptSerializer();
                            var Auditstore = new Store(store.Id, store.StoreCode, store.StoreName, store.IsDeleted);
                            var storeser = storeJavascript.Serialize(Auditstore);
                            _auditService.InsertAudit(HelperCls.GetIPAddress(), storeser, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
                        }
                        catch (Exception ex)
                        {
                            HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                            throw ex;
                        }
                    }
                }
                Customer1.ModifiedDate = DateTime.Now;
                Customer1.IsDeleted = true;
                int rowsaffected = _customerService.EditCustomer(Customer1);
                try
                {
                    var javascript = new JavaScriptSerializer();
                    var Auditcust = new Customer(Customer1.Id, Customer1.Name, Customer1.IsDeleted, Customer1.ModifiedDate, Customer1.Created, Customer1.CreatedBy, Customer1.ModifiedBy, Customer1.Company.Id, Customer1.CustomerPlan.Id);
                    var str = javascript.Serialize(Auditcust);
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), str, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                    throw ex;
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return RedirectToAction("Error500", "Home");
            }
        }

        //[AdminApiAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllowedCustomers()
        {
            int userId = SessionValues.UserId;
            var data = _customerService.customerLists(userId);
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        [AdminApiAuthorization]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ChangeCustomer(int CustId)
        {
            int userId = SessionValues.UserId;
            var data = _customerService.customerLists(userId);
            if (data.Exists(x => x.Id == CustId))
            {
                string SessionTime = _menuService.GetMaster("Session Time").Value;
                ViewBag.Password = "True";
                var user = _userService.GetUsersByEmail(SessionValues.StoreUserName);
                HttpContext.Items.Add("User", user);
                ViewBag.Password = "True";
                UserSession userSession = new UserSession();
                userSession.RoleId = user.RoleId != 0 ? user.RoleId : 1;
                userSession.StoreId = 0;
                userSession.UserId = user.Id;
                userSession.UserName = SessionValues.StoreUserName;
                userSession.PlanId = user.CustomerPlan.Id;
                userSession.MkrChkrFlag = 0.ToString();
                userSession.SessionTimeout = 0.ToString();
                userSession.EmailAddress = (user.EmailAddress != null ? user.EmailAddress : "");
                userSession.StoreName = user.Name;
                userSession.CustomerId = CustId;
                var userData = new JavaScriptSerializer().Serialize(userSession);
                Session["AuthUserData"] = user.RoleId + "|" + 0 + "|" + user.Id + "|" + CustId + "|" + SessionValues.StoreUserName.ToString() + "|" + user.CustomerPlan.Id + "|" + 0 + "|" + SessionTime + "|" + (user.EmailAddress != null ? user.EmailAddress : "") + "|" + 0 + "|" + user.Name;
                var ticket = new FormsAuthenticationTicket(1, (user.Id.ToString() + user.Name), DateTime.Now, DateTime.Now.AddMinutes(Convert.ToDouble(SessionTime)), true, userData);
                string encTicket = FormsAuthentication.Encrypt(ticket);
                Response.Cookies.Clear();
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        [AdminApiAuthorization]
        public ActionResult GetAutoInventoryLogicFlag()
        {
            int userId = SessionValues.UserId;
            int CustId = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            Customer data = _customerService.GetAutoInventoryLogicFlag(userId, CustId);
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MasterFileUpload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MasterFileUpload(HttpPostedFileBase MasterFile, string MasterType)
        {
            var LoggedInCustomer = Convert.ToInt32(SessionValues.LoggedInCustId);
            List<string> ErrorList = new List<string>();
            var LoggedInUser = SessionValues.UserId;
            if (Request.Files["MasterFile"].ContentLength > 0)
            {
                try
                {
                    if ((MasterFile != null) && (MasterFile.ContentLength > 0) && !string.IsNullOrEmpty(MasterFile.FileName))
                    {
                        string fileName = MasterFile.FileName;
                        string fileContentType = MasterFile.ContentType;
                        var fileSize = MasterFile.ContentLength;
                        int RowsInserted = 0;
                        if (fileSize > GlobalValues.MegaBytes)
                        {
                            string err = "GENERAL EXCEPTION - LARGE FILE | DATA UPLOAD FAILED";
                            ErrorList.Add(err);
                            return View();
                        }
                        else
                        {
                            string _FileName = Path.GetFileName(MasterFile.FileName);
                            _FileName = LoggedInUser + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + "_" + _FileName;
                            string ExtentedFilePath = "/MasterFile/" + MasterType + "/";
                            string _path = Path.Combine(Server.MapPath("~/UploadedFiles" + ExtentedFilePath), "" + _FileName);

                            byte[] fileBytes = new byte[MasterFile.ContentLength];
                            var data = MasterFile.InputStream.Read(fileBytes, 0, Convert.ToInt32(MasterFile.ContentLength));
                            using (var package = new ExcelPackage(MasterFile.InputStream))
                            {
                                if (MasterType == "ITEM")
                                {
                                    var currentSheet = package.Workbook.Worksheets;
                                    var workSheet = currentSheet["Sheet1"] != null ? currentSheet["Sheet1"] : currentSheet.First();
                                    var headers = workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column]
                            .Select(c => c.Text)
                            .ToList();
                                    BAL.HelperCls.DebugLog("Excel Headers: " + string.Join(", ", headers));

                                    ItemMasterUploadViewModel ItemMasterUploadViewModel = new ItemMasterUploadViewModel();
                                    for (int i = 1; i <= workSheet.Dimension.End.Column; i++)
                                    {
                                        var firstRowCell = workSheet.Cells[1, i];
                                        var HeaderName = firstRowCell.Value != null ? firstRowCell.Value.ToString() : "";
                                        if (nameof(ItemMasterUploadViewModel.Customer).Trim().ToUpper() == HeaderName.Trim().ToUpper())
                                        {
                                            ItemMasterUploadViewModel.Customer_Id = i;
                                        }
                                        if (nameof(ItemMasterUploadViewModel.SKUCode).Trim().ToUpper() == HeaderName.Trim().ToUpper())
                                        {
                                            ItemMasterUploadViewModel.SKUCode_Id = i;
                                        }
                                        if (nameof(ItemMasterUploadViewModel.SKUName).Trim().ToUpper() == HeaderName.Trim().ToUpper())
                                        {
                                            ItemMasterUploadViewModel.SKUName_Id = i;
                                        }
                                        if (nameof(ItemMasterUploadViewModel.UOM).Trim().ToUpper() == HeaderName.Trim().ToUpper())
                                        {
                                            ItemMasterUploadViewModel.UOM_Id = i;
                                        }
                                        if (nameof(ItemMasterUploadViewModel.Category).Trim().ToUpper() == HeaderName.Trim().ToUpper())
                                        {
                                            ItemMasterUploadViewModel.Category_Id = i;
                                        }
                                        if (nameof(ItemMasterUploadViewModel.SubCategory).Trim().ToUpper() == HeaderName.Trim().ToUpper())
                                        {
                                            ItemMasterUploadViewModel.SubCategory_Id = i;
                                        }
                                        if (nameof(ItemMasterUploadViewModel.ItemCategoryType).Trim().ToUpper() == HeaderName.Trim().ToUpper())
                                        {
                                            ItemMasterUploadViewModel.ItemCategoryType_Id = i;
                                        }
                                        if (nameof(ItemMasterUploadViewModel.Brand).Trim().ToUpper() == HeaderName.Trim().ToUpper())
                                        {
                                            ItemMasterUploadViewModel.Brand_Id = i;
                                        }
                                        if (nameof(ItemMasterUploadViewModel.TotalWeightPerCase).Trim().ToUpper() == HeaderName.Trim().ToUpper())
                                        {
                                            ItemMasterUploadViewModel.TotalWeightPerCase_Id = i;
                                        }
                                        if (nameof(ItemMasterUploadViewModel.MinimumOrder).Trim().ToUpper() == HeaderName.Trim().ToUpper())
                                        {
                                            ItemMasterUploadViewModel.MinimumOrder_Id = i;
                                        }
                                        if (nameof(ItemMasterUploadViewModel.MaximumOrderLimit).Trim().ToUpper() == HeaderName.Trim().ToUpper())
                                        {
                                            ItemMasterUploadViewModel.MaximumOrderLimit_Id = i;
                                        }
                                        if (nameof(ItemMasterUploadViewModel.CaseConversion).Trim().ToUpper() == HeaderName.Trim().ToUpper())
                                        {
                                            ItemMasterUploadViewModel.CaseConversion_Id = i;
                                        }
                                    }
                                    if (ItemMasterUploadViewModel.Customer_Id == 0)
                                    {
                                        ErrorList.Add("COLUMN_NAME ERROR: " + nameof(ItemMasterUploadViewModel.Customer) + " | DATA UPLOAD FAILED");
                                    }
                                    if (ItemMasterUploadViewModel.SKUCode_Id == 0)
                                    {
                                        ErrorList.Add("COLUMN_NAME ERROR: " + nameof(ItemMasterUploadViewModel.SKUCode) + " | DATA UPLOAD FAILED");
                                    }
                                    if (ItemMasterUploadViewModel.SKUName_Id == 0)
                                    {
                                        ErrorList.Add("COLUMN_NAME ERROR: " + nameof(ItemMasterUploadViewModel.SKUName) + " | DATA UPLOAD FAILED");
                                    }
                                    if (ItemMasterUploadViewModel.UOM_Id == 0)
                                    {
                                        ErrorList.Add("COLUMN_NAME ERROR: " + nameof(ItemMasterUploadViewModel.UOM) + " | DATA UPLOAD FAILED");
                                    }
                                    if (ItemMasterUploadViewModel.ItemCategoryType_Id == 0)
                                    {
                                        ErrorList.Add("COLUMN_NAME ERROR: " + nameof(ItemMasterUploadViewModel.ItemCategoryType) + " | DATA UPLOAD FAILED");
                                    }
                                    if (ItemMasterUploadViewModel.Category_Id == 0)
                                    {
                                        ErrorList.Add("COLUMN_NAME ERROR: " + nameof(ItemMasterUploadViewModel.Category) + " | DATA UPLOAD FAILED");
                                    }
                                    if (ItemMasterUploadViewModel.SubCategory_Id == 0)
                                    {
                                        ErrorList.Add("COLUMN_NAME ERROR: " + nameof(ItemMasterUploadViewModel.SubCategory) + " | DATA UPLOAD FAILED");
                                    }
                                    if (ItemMasterUploadViewModel.Brand_Id == 0)
                                    {
                                        ErrorList.Add("COLUMN_NAME ERROR: " + nameof(ItemMasterUploadViewModel.Brand) + " | DATA UPLOAD FAILED");
                                    }
                                    if (ItemMasterUploadViewModel.TotalWeightPerCase_Id == 0)
                                    {
                                        ErrorList.Add("COLUMN_NAME ERROR: " + nameof(ItemMasterUploadViewModel.TotalWeightPerCase) + " | DATA UPLOAD FAILED");
                                    }
                                    if (ItemMasterUploadViewModel.MinimumOrder_Id == 0)
                                    {
                                        ErrorList.Add("COLUMN_NAME ERROR: " + nameof(ItemMasterUploadViewModel.MinimumOrder) + " | DATA UPLOAD FAILED");
                                    }
                                    if (ItemMasterUploadViewModel.MaximumOrderLimit_Id == 0)
                                    {
                                        ErrorList.Add("COLUMN_NAME ERROR: " + nameof(ItemMasterUploadViewModel.MaximumOrderLimit) + " | DATA UPLOAD FAILED");
                                    }
                                    if (ItemMasterUploadViewModel.CaseConversion_Id == 0)
                                    {
                                        ErrorList.Add("COLUMN_NAME ERROR: " + nameof(ItemMasterUploadViewModel.CaseConversion) + " | DATA UPLOAD FAILED");
                                    }
                                    if (ErrorList.Count == 0)
                                    {
                                        List<ItemMasterUploadViewModel> lstOrderExcel = new List<ItemMasterUploadViewModel>();
                                        for (int rowIterator = 2; rowIterator <= workSheet.Dimension.End.Row; rowIterator++)
                                        {
                                            var user = new Users();
                                            var import = new ItemMasterUploadViewModel();
                                            import.Error = "";
                                            try
                                            {
                                                import.Customer = workSheet.Cells[rowIterator, ItemMasterUploadViewModel.Customer_Id].Text != null ? workSheet.Cells[rowIterator, ItemMasterUploadViewModel.Customer_Id].Text : "";
                                                import.SKUCode = workSheet.Cells[rowIterator, ItemMasterUploadViewModel.SKUCode_Id].Text != null ? workSheet.Cells[rowIterator, ItemMasterUploadViewModel.SKUCode_Id].Text : "";
                                                import.SKUName = workSheet.Cells[rowIterator, ItemMasterUploadViewModel.SKUName_Id].Text != null ? workSheet.Cells[rowIterator, ItemMasterUploadViewModel.SKUName_Id].Text : "";
                                                import.UOM = workSheet.Cells[rowIterator, ItemMasterUploadViewModel.UOM_Id].Text != null ? workSheet.Cells[rowIterator, ItemMasterUploadViewModel.UOM_Id].Text : "";
                                                import.Category = workSheet.Cells[rowIterator, ItemMasterUploadViewModel.Category_Id].Text != null ? workSheet.Cells[rowIterator, ItemMasterUploadViewModel.Category_Id].Text : "";
                                                import.SubCategory = workSheet.Cells[rowIterator, ItemMasterUploadViewModel.SubCategory_Id].Text != null ? workSheet.Cells[rowIterator, ItemMasterUploadViewModel.SubCategory_Id].Text : "";
                                                import.ItemCategoryType = workSheet.Cells[rowIterator, ItemMasterUploadViewModel.ItemCategoryType_Id].Text != null ? workSheet.Cells[rowIterator, ItemMasterUploadViewModel.ItemCategoryType_Id].Text : "";
                                                import.Brand = workSheet.Cells[rowIterator, ItemMasterUploadViewModel.Brand_Id].Text != null ? workSheet.Cells[rowIterator, ItemMasterUploadViewModel.Brand_Id].Text : "";
                                                import.TotalWeightPerCase = workSheet.Cells[rowIterator, ItemMasterUploadViewModel.TotalWeightPerCase_Id].Text != null ? workSheet.Cells[rowIterator, ItemMasterUploadViewModel.TotalWeightPerCase_Id].Text : "";
                                                import.MinimumOrder = workSheet.Cells[rowIterator, ItemMasterUploadViewModel.MinimumOrder_Id].Text != null ? workSheet.Cells[rowIterator, ItemMasterUploadViewModel.MinimumOrder_Id].Text : "";
                                                import.MaximumOrderLimit = workSheet.Cells[rowIterator, ItemMasterUploadViewModel.MaximumOrderLimit_Id].Text != null ? workSheet.Cells[rowIterator, ItemMasterUploadViewModel.MaximumOrderLimit_Id].Text : "";
                                                import.CaseConversion = workSheet.Cells[rowIterator, ItemMasterUploadViewModel.CaseConversion_Id].Text != null ? workSheet.Cells[rowIterator, ItemMasterUploadViewModel.CaseConversion_Id].Text : "";
                                                import.Xls_Line_No = rowIterator;
                                                import.Upload_By = LoggedInUser;
                                                lstOrderExcel.Add(import);
                                            }
                                            catch (Exception exception)
                                            {
                                                string Excmsg = "";
                                                if (exception.InnerException != null)
                                                {
                                                    Excmsg = exception.Message + System.Environment.NewLine + exception.InnerException.Message
                                                        + System.Environment.NewLine + exception.StackTrace;
                                                }
                                                else
                                                {
                                                    Excmsg = exception.Message + System.Environment.NewLine + exception.StackTrace;
                                                }
                                                BAL.HelperCls.DebugLog(Excmsg);
                                                import.Error = Excmsg + System.Environment.NewLine + "Line No: " + rowIterator;
                                                ErrorList.Add(import.Error);
                                            }
                                        }
                                        if (ErrorList.Count == 0 && lstOrderExcel != null && lstOrderExcel.Count() > 0)
                                        {
                                            UploadedFilesMst mst = new UploadedFilesMst();
                                            mst.CustId = LoggedInCustomer;
                                            mst.Upload_By = LoggedInUser;
                                            mst.User_FileName = fileName;
                                            mst.Filepath = _path;
                                            mst.System_FileName = _FileName;
                                            mst.FileType = "MASTER-" + MasterType;
                                            long FileId = _orderService.SaveUploadedFile(mst);
                                            string InsertedMessege = _customerService.InsertItemsBulk(lstOrderExcel, FileId, mst.FileType);
                                            ViewBag.SuccessMessage = InsertedMessege;
                                            return View();
                                        }
                                        else
                                        {
                                            ErrorList.Add("No Rows Found");
                                        }
                                    }
                                }
                                else if (MasterType == "STORE")
                                {
                                    var currentSheet = package.Workbook.Worksheets;
                                    var workSheet = currentSheet["Sheet1"] != null ? currentSheet["Sheet1"] : currentSheet.First();
                                    StoreMasterUploadViewModel StoreMasterUploadViewModel = new StoreMasterUploadViewModel();
                                    for (int i = 1; i <= workSheet.Dimension.End.Column; i++)
                                    {
                                        var firstRowCell = workSheet.Cells[1, i];
                                        var HeaderName = firstRowCell.Value != null ? firstRowCell.Value.ToString() : "";
                                        if (nameof(StoreMasterUploadViewModel.Customer).Trim().ToUpper() == HeaderName.Trim().ToUpper())
                                        {
                                            StoreMasterUploadViewModel.Customer_Id = i;
                                        }
                                        if (nameof(StoreMasterUploadViewModel.StoreCode).Trim().ToUpper() == HeaderName.Trim().ToUpper())
                                        {
                                            StoreMasterUploadViewModel.StoreCode_Id = i;
                                        }
                                        if (nameof(StoreMasterUploadViewModel.StoreName).Trim().ToUpper() == HeaderName.Trim().ToUpper())
                                        {
                                            StoreMasterUploadViewModel.StoreName_Id = i;
                                        }
                                        if (nameof(StoreMasterUploadViewModel.Address).Trim().ToUpper() == HeaderName.Trim().ToUpper())
                                        {
                                            StoreMasterUploadViewModel.Address_Id = i;
                                        }
                                        if (nameof(StoreMasterUploadViewModel.EmailAddress).Trim().ToUpper() == HeaderName.Trim().ToUpper())
                                        {
                                            StoreMasterUploadViewModel.EmailAddress_Id = i;
                                        }
                                        if (nameof(StoreMasterUploadViewModel.StoreManager).Trim().ToUpper() == HeaderName.Trim().ToUpper())
                                        {
                                            StoreMasterUploadViewModel.StoreManager_Id = i;
                                        }
                                        if (nameof(StoreMasterUploadViewModel.StoreContactNo).Trim().ToUpper() == HeaderName.Trim().ToUpper())
                                        {
                                            StoreMasterUploadViewModel.StoreContactNo_Id = i;
                                        }
                                        if (nameof(StoreMasterUploadViewModel.Location).Trim().ToUpper() == HeaderName.Trim().ToUpper())
                                        {
                                            StoreMasterUploadViewModel.Location_Id = i;
                                        }
                                        if (nameof(StoreMasterUploadViewModel.WareHouse).Trim().ToUpper() == HeaderName.Trim().ToUpper())
                                        {
                                            StoreMasterUploadViewModel.WareHouse_Id = i;
                                        }
                                        if (nameof(StoreMasterUploadViewModel.PlaceOfSupply).Trim().ToUpper() == HeaderName.Trim().ToUpper())
                                        {
                                            StoreMasterUploadViewModel.PlaceOfSupply_Id = i;
                                        }
                                        if (nameof(StoreMasterUploadViewModel.NetSuitClass).Trim().ToUpper() == HeaderName.Trim().ToUpper())
                                        {
                                            StoreMasterUploadViewModel.NetSuitClass_Id = i;
                                        }
                                        if (nameof(StoreMasterUploadViewModel.EnterpriseCode).Trim().ToUpper() == HeaderName.Trim().ToUpper())
                                        {
                                            StoreMasterUploadViewModel.EnterpriseCode_Id = i;
                                        }
                                        if (nameof(StoreMasterUploadViewModel.EnterpriseName).Trim().ToUpper() == HeaderName.Trim().ToUpper())
                                        {
                                            StoreMasterUploadViewModel.EnterpriseName_Id = i;
                                        }
                                    }
                                    if (StoreMasterUploadViewModel.Customer_Id == 0)
                                    {
                                        ErrorList.Add("COLUMN_NAME ERROR: " + nameof(StoreMasterUploadViewModel.Customer) + " | DATA UPLOAD FAILED");
                                    }
                                    if (StoreMasterUploadViewModel.StoreCode_Id == 0)
                                    {
                                        ErrorList.Add("COLUMN_NAME ERROR: " + nameof(StoreMasterUploadViewModel.StoreCode) + " | DATA UPLOAD FAILED");
                                    }
                                    if (StoreMasterUploadViewModel.StoreName_Id == 0)
                                    {
                                        ErrorList.Add("COLUMN_NAME ERROR: " + nameof(StoreMasterUploadViewModel.StoreName) + " | DATA UPLOAD FAILED");
                                    }
                                    if (StoreMasterUploadViewModel.Address_Id == 0)
                                    {
                                        ErrorList.Add("COLUMN_NAME ERROR: " + nameof(StoreMasterUploadViewModel.Address) + " | DATA UPLOAD FAILED");
                                    }
                                    if (StoreMasterUploadViewModel.EmailAddress_Id == 0)
                                    {
                                        ErrorList.Add("COLUMN_NAME ERROR: " + nameof(StoreMasterUploadViewModel.EmailAddress) + " | DATA UPLOAD FAILED");
                                    }
                                    if (StoreMasterUploadViewModel.StoreManager_Id == 0)
                                    {
                                        ErrorList.Add("COLUMN_NAME ERROR: " + nameof(StoreMasterUploadViewModel.StoreManager) + " | DATA UPLOAD FAILED");
                                    }
                                    if (StoreMasterUploadViewModel.StoreContactNo_Id == 0)
                                    {
                                        ErrorList.Add("COLUMN_NAME ERROR: " + nameof(StoreMasterUploadViewModel.StoreContactNo) + " | DATA UPLOAD FAILED");
                                    }
                                    if (StoreMasterUploadViewModel.Location_Id == 0)
                                    {
                                        ErrorList.Add("COLUMN_NAME ERROR: " + nameof(StoreMasterUploadViewModel.Location) + " | DATA UPLOAD FAILED");
                                    }
                                    if (StoreMasterUploadViewModel.WareHouse_Id == 0)
                                    {
                                        ErrorList.Add("COLUMN_NAME ERROR: " + nameof(StoreMasterUploadViewModel.WareHouse) + " | DATA UPLOAD FAILED");
                                    }
                                    if (StoreMasterUploadViewModel.PlaceOfSupply_Id == 0)
                                    {
                                        ErrorList.Add("COLUMN_NAME ERROR: " + nameof(StoreMasterUploadViewModel.PlaceOfSupply) + " | DATA UPLOAD FAILED");
                                    }
                                    if (StoreMasterUploadViewModel.NetSuitClass_Id == 0)
                                    {
                                        ErrorList.Add("COLUMN_NAME ERROR: " + nameof(StoreMasterUploadViewModel.NetSuitClass) + " | DATA UPLOAD FAILED");
                                    }
                                    if (StoreMasterUploadViewModel.EnterpriseCode_Id == 0)
                                    {
                                        ErrorList.Add("COLUMN_NAME ERROR: " + nameof(StoreMasterUploadViewModel.EnterpriseCode) + " | DATA UPLOAD FAILED");
                                    }
                                    if (StoreMasterUploadViewModel.EnterpriseName_Id == 0)
                                    {
                                        ErrorList.Add("COLUMN_NAME ERROR: " + nameof(StoreMasterUploadViewModel.EnterpriseName) + " | DATA UPLOAD FAILED");
                                    }
                                    if (ErrorList.Count == 0)
                                    {
                                        List<StoreMasterUploadViewModel> lstOrderExcel = new List<StoreMasterUploadViewModel>();
                                        for (int rowIterator = 2; rowIterator <= workSheet.Dimension.End.Row; rowIterator++)
                                        {
                                            var user = new Users();
                                            var import = new StoreMasterUploadViewModel();
                                            import.Error = "";
                                            try
                                            {
                                                import.Customer = workSheet.Cells[rowIterator, StoreMasterUploadViewModel.Customer_Id].Text != null ? workSheet.Cells[rowIterator, StoreMasterUploadViewModel.Customer_Id].Text : "";
                                                import.StoreCode = workSheet.Cells[rowIterator, StoreMasterUploadViewModel.StoreCode_Id].Text != null ? workSheet.Cells[rowIterator, StoreMasterUploadViewModel.StoreCode_Id].Text : "";
                                                import.StoreName = workSheet.Cells[rowIterator, StoreMasterUploadViewModel.StoreName_Id].Text != null ? workSheet.Cells[rowIterator, StoreMasterUploadViewModel.StoreName_Id].Text : "";
                                                import.Address = workSheet.Cells[rowIterator, StoreMasterUploadViewModel.Address_Id].Text != null ? workSheet.Cells[rowIterator, StoreMasterUploadViewModel.Address_Id].Text : "";
                                                import.EmailAddress = workSheet.Cells[rowIterator, StoreMasterUploadViewModel.EmailAddress_Id].Text != null ? workSheet.Cells[rowIterator, StoreMasterUploadViewModel.EmailAddress_Id].Text : "";
                                                import.StoreManager = workSheet.Cells[rowIterator, StoreMasterUploadViewModel.StoreManager_Id].Text != null ? workSheet.Cells[rowIterator, StoreMasterUploadViewModel.StoreManager_Id].Text : "";
                                                import.StoreContactNo = workSheet.Cells[rowIterator, StoreMasterUploadViewModel.StoreContactNo_Id].Text != null ? workSheet.Cells[rowIterator, StoreMasterUploadViewModel.StoreContactNo_Id].Text : "";
                                                import.Location = workSheet.Cells[rowIterator, StoreMasterUploadViewModel.Location_Id].Text != null ? workSheet.Cells[rowIterator, StoreMasterUploadViewModel.Location_Id].Text : "";
                                                import.WareHouse = workSheet.Cells[rowIterator, StoreMasterUploadViewModel.WareHouse_Id].Text != null ? workSheet.Cells[rowIterator, StoreMasterUploadViewModel.WareHouse_Id].Text : "";
                                                import.PlaceOfSupply = workSheet.Cells[rowIterator, StoreMasterUploadViewModel.PlaceOfSupply_Id].Text != null ? workSheet.Cells[rowIterator, StoreMasterUploadViewModel.PlaceOfSupply_Id].Text : "";
                                                import.NetSuitClass = workSheet.Cells[rowIterator, StoreMasterUploadViewModel.NetSuitClass_Id].Text != null ? workSheet.Cells[rowIterator, StoreMasterUploadViewModel.NetSuitClass_Id].Text : "";
                                                import.EnterpriseCode = workSheet.Cells[rowIterator, StoreMasterUploadViewModel.EnterpriseCode_Id].Text != null ? workSheet.Cells[rowIterator, StoreMasterUploadViewModel.EnterpriseCode_Id].Text : "";
                                                import.EnterpriseName = workSheet.Cells[rowIterator, StoreMasterUploadViewModel.EnterpriseName_Id].Text != null ? workSheet.Cells[rowIterator, StoreMasterUploadViewModel.EnterpriseName_Id].Text : "";
                                                import.Xls_Line_No = rowIterator;
                                                import.Upload_By = LoggedInUser;
                                                lstOrderExcel.Add(import);
                                            }
                                            catch (Exception exception)
                                            {
                                                string Excmsg = "";
                                                if (exception.InnerException != null)
                                                {
                                                    Excmsg = exception.Message + System.Environment.NewLine + exception.InnerException.Message
                                                        + System.Environment.NewLine + exception.StackTrace;
                                                }
                                                else
                                                {
                                                    Excmsg = exception.Message + System.Environment.NewLine + exception.StackTrace;
                                                }
                                                BAL.HelperCls.DebugLog(Excmsg);
                                                import.Error = Excmsg + System.Environment.NewLine + "Line No: " + rowIterator;
                                                ErrorList.Add(import.Error);
                                            }
                                        }
                                        if (ErrorList.Count == 0 && lstOrderExcel != null && lstOrderExcel.Count() > 0)
                                        {
                                            UploadedFilesMst mst = new UploadedFilesMst();
                                            mst.CustId = LoggedInCustomer;
                                            mst.Upload_By = LoggedInUser;
                                            mst.User_FileName = fileName;
                                            mst.Filepath = _path;
                                            mst.System_FileName = _FileName;
                                            mst.FileType = "MASTER-" + MasterType;
                                            long FileId = _orderService.SaveUploadedFile(mst);
                                            string InsertedMessege = _customerService.InsertStoresBulk(lstOrderExcel, FileId, mst.FileType);
                                            ViewBag.SuccessMessage = InsertedMessege;
                                            return View();
                                        }
                                        else
                                        {
                                            ErrorList.Add("No Rows Found.");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        ErrorList.Add("unable to read file.");
                    }
                }
                catch (Exception exception)
                {
                    string Excmsg = "";
                    if (exception.InnerException != null)
                    {
                        Excmsg = exception.Message + System.Environment.NewLine + exception.InnerException.Message
                            + System.Environment.NewLine + exception.StackTrace;
                    }
                    else
                    {
                        Excmsg = exception.Message + System.Environment.NewLine + exception.StackTrace;

                    }
                    BAL.HelperCls.DebugLog(Excmsg);
                    ErrorList.Add(Excmsg);
                }
            }
            else
            {
                ErrorList.Add("unable to read file content type.");
            }
            if (ErrorList.Count() > 0)
            {
                foreach (string err in ErrorList)
                    ModelState.AddModelError("", err);
            }
            return View();
        }

        public ActionResult UploadedMasterFiles()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetUploadedMasterFiles(List<string> FileType)
        {
            int CustId = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            int UserId = Convert.ToInt32(SessionValues.UserId);
            List<UploadedFilesView> ExcelOrders = _orderService.GetUploadedFileDetails(CustId, UserId, FileType);
            return Json(new { data = ExcelOrders }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetCitiesByCountryAsync() // Asynchronous Action Method
        {
            return Json("erfe");
        }
    }
}