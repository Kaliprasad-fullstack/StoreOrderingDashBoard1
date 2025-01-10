using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Reflection;
using System.Configuration;

namespace EscalationService
{
    public partial class Service1 : ServiceBase
    {
        Timer Timer = new Timer();
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Timer.Enabled = true;
            string MinIntervalStr = string.Empty;
            Assembly executingAssembly = Assembly.GetAssembly(typeof(ProjectInstaller));
            string targetDir = executingAssembly.Location;
            Configuration config = ConfigurationManager.OpenExeConfiguration(targetDir);
            MinIntervalStr = config.AppSettings.Settings["IntervalMins"].Value.ToString();
            double Interval = Convert.ToDouble(MinIntervalStr);
            Timer.Interval = 60000 * Interval;
            Timer.Elapsed += Timer_Elapsed;
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                DBLayer layer = new DBLayer();
                #region --
                //var orderno = DBLayer.GetOrderno(true, true, true, false, false);
                //if (orderno.Count() > 0)
                //{
                //    foreach (Order order in orderno)
                //    {
                //        Sonumber sonumber = new Sonumber();
                //        sonumber.sonumber = order.SoNumber;
                //        var fullfillment = GetResultFromFullFillmentAPI(sonumber);
                //        if (fullfillment != null)
                //        {
                //            if (fullfillment.data != null)
                //            {
                //                foreach (Datum head in fullfillment.data)
                //                {
                //                    Int64 headerid = layer.InsertFullHeader(head);
                //                    if (headerid > 0)
                //                    {
                //                        foreach (Items item in head.Items)
                //                        {
                //                            Int64 detailid = layer.InsertFullDetail(item, headerid);
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                #endregion
                string MinIntervalStr = string.Empty;
                Assembly executingAssembly = Assembly.GetAssembly(typeof(ProjectInstaller));
                string targetDir = executingAssembly.Location;
                Configuration config = ConfigurationManager.OpenExeConfiguration(targetDir);
                MinIntervalStr = config.AppSettings.Settings["IntervalMins"].Value.ToString();
                int Interval = Convert.ToInt32(MinIntervalStr);
                layer.GetCustomersForEscalationMatrix(DateTime.Now.TimeOfDay,Interval);
            }
            catch (Exception ex)
            {
                DBLayer.DebugLog(ex.Message + Environment.NewLine + ex.StackTrace);
                //throw ex;
            }


        }
        protected override void OnStop()
        {
        }
    }
}
