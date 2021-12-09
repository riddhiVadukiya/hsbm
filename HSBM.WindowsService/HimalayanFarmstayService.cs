using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;

namespace HSBM.WindowsService
{
    partial class HimalayanFarmstayService : ServiceBase
    {
        Timer timeDelay;
        int count;  

        public HimalayanFarmstayService()
        {
            InitializeComponent();
            timeDelay = new System.Timers.Timer();
            timeDelay.Elapsed += new System.Timers.ElapsedEventHandler(WorkProcess);  
        }
        public void WorkProcess(object sender, System.Timers.ElapsedEventArgs e)
        {

            //To do all code
            string process = "Timer Tick " + count;
            LogService(process);
            count++;
        }  

        protected override void OnStart(string[] args)
        {
            LogService("Service is Started");
            timeDelay.Enabled = true;  
        }

        protected override void OnStop()
        {
            LogService("Service Stoped");
            timeDelay.Enabled = false; 
        }

        private void LogService(string content)  
        {  
            FileStream fs = new FileStream(@"d:\TestServiceLog.txt", FileMode.OpenOrCreate, FileAccess.Write);  
            StreamWriter sw = new StreamWriter(fs);  
            sw.BaseStream.Seek(0, SeekOrigin.End);  
            sw.WriteLine(content);  
            sw.Flush();  
            sw.Close();  
        }
    }
}
