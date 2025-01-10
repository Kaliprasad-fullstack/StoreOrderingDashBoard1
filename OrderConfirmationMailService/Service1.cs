using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace OrderConfirmationMailService
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
                string MinIntervalStr = string.Empty;
                Assembly executingAssembly = Assembly.GetAssembly(typeof(ProjectInstaller));
                string targetDir = executingAssembly.Location;
                Configuration config = ConfigurationManager.OpenExeConfiguration(targetDir);
                MinIntervalStr = config.AppSettings.Settings["IntervalMins"].Value.ToString();
                int Interval = Convert.ToInt32(MinIntervalStr);
                layer.GetOrderGenerated(DateTime.Now, Interval);
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
