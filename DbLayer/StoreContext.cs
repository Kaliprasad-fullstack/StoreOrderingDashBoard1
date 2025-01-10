using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;



namespace DbLayer
{
    public class StoreContext : DbContext
    {
        public StoreContext() : base("StoreConnectionString")
        {
            Database.SetInitializer<StoreContext>(new MigrateDatabaseToLatestVersion<StoreContext, DbLayer.Migrations.Configuration>());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           
            //modelBuilder.Entity<OrderHeader>()
            //            .MapToStoredProcedures();
            //modelBuilder.Entity<OrderDetail>()
            //            .MapToStoredProcedures();
            // modelBuilder.Types().Configure(t => t.MapToStoredProcedures());
        }
        //public DbSet<TicketType2> TicketTypeMST
        //{
        //    get; set;
        //}
        public DbSet<Users> Users { get; set; }
        public DbSet<Locations> Locations { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<OrderHeader> orderHeaders { get; set; }
        public DbSet<OrderDetail> orderDetails { get; set; }
        public DbSet<WareHouseDC> wareHouseDCs { get; set; }
        public DbSet<Region> regions { get; set; }
        public DbSet<UOMmaster> uOMmasters { get; set;}
        public DbSet<Category> categories { get; set; }
        public DbSet<SubCategory> subCategories { get; set; }
        public DbSet<TempTbl> tempTbls { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<TicketGenration> TicketGenration { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }

        public DbSet<PermitableCls> Permitables { get; set; }
        public DbSet<MenuItem> menuItems { get; set; }
        public DbSet<CustomerPlan> customerPlans { get; set; }
        public DbSet<Plan> plans { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<NetSuitMapCls> netSuitMapCls { get; set; }
        public DbSet<PasswordCustomer> passwordCustomers { get; set; }
        public DbSet<Question> questions { get; set; }
        public DbSet<Answer> answers { get; set; }
        public DbSet<DashBoardPer> dashBoardPers { get; set; }
        public DbSet<DashBoardMapping> dashBoardMappings { get; set; }
        public DbSet<InvoiceHeader> invoiceHeaders { get; set; }
        public DbSet<InvoiceDetail> invoiceDetails { get; set; }
        public DbSet<FullfillmentHeader> fullfillmentHeaders { get; set; }
        public DbSet<FullFillmentDetail> fullFillmentDetails { get; set; }
        public DbSet<Audit> audits { get; set; }
        public DbSet<UserStore> userStores { get; set; }
        public DbSet<SoApproveHeader> soApproveHeaders { get; set; }
        public DbSet<SoApproveDetail> soApproveDetails { get; set; }
        public DbSet<StateMst> stateMsts { get; set; }
        public DbSet<Master> masters { get; set; }        
        public DbSet<EscalationLevelMaster> EscalationLevelMasters { get; set; }        
        public DbSet<EscalationAction> EscalationActions { get; set; }
        public DbSet<CustomerEmployee> customerEmployees { get; set; }
        public DbSet<CurrentItemInventoryMst> currentItemInventoryMsts { get; set; }
        public DbSet<PODDetail> PODDetails { get; set; }
        public DbSet<PODHeader> PODHeaders { get; set; }
        public DbSet<InvoiceDispatch> InvoiceDispatchList { get; set; }
        public DbSet<ExcelParameters> ExcelParameters { get; set; }
        public DbSet<OrderFormatExcel> OrderFormatExcel { get; set; }
        public DbSet<SoStatusMaster> SoStatusMasters { get; set; }
        public DbSet<SoPoMapping> SoPoMappings { get; set; }
        public DbSet<SoStatusDetail> SoStatusDetails { get; set; }
        public DbSet<SoDetails> SoDetails { get; set; }
        public DbSet<OrderExcelData> OrderExcelData { get; set; }
        public DbSet<ReturnAuthorizations> ReturnAuthorizations { get; set; }
        public DbSet<OrderExcel> OrderExcels { get; set; }
        public DbSet<UploadedFilesMst> UploadedFilesMsts { get; set; }
        public DbSet<File_Error_Logs> File_Error_Logs { get; set; }
        public DbSet<TempExcelData> TempExcelData { get; set; }
        public DbSet<Store_ItemType_DC_Master> Store_ItemType_DC_Master { get; set; }
        public DbSet<CategoryWiseReasonMaster> CategoryWiseReasonMaster { get; set; }
        public DbSet<StoreDraftOrder> StoreDraftOrders { get; set; }
        public DbSet<PODHeaderArchive> PODHeaderArchive { get; set; }
        public DbSet<CSVFileMaster> CSVFileMasters { get; set; }
        //added by rekha nirgude 2019-12-20
        public DbSet<PODImages> PODImages { get; set; }
        //added by Nikhil Kadam 2020-01-06
        public DbSet<CategoryType> ItemType { get; set; }
        public DbSet<StoreItemMst> StoreItemMst { get; set; }
        public DbSet<OrderGroupMst> OrderGroupMst { get; set; }
        public DbSet<CustomerEmailMaster> CustomerEmailMaster { get; set; }
        public DbSet<EmailType> EmailType { get; set; }
        public DbSet<MailData> OrderMailData { get; set;}

    }
    //public StoreContext() : base("StoreConnectionString")
    //{
    //    Database.SetInitializer(new DropCreateDatabaseIfModelChanges<QuizzContext>()); // to sync table structure and model class
    //}
}
