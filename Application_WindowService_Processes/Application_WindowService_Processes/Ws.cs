using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ServiceProcess; // Bunu Reference etmek gerekli !!
using System.Management; //// Bunu Reference etmek gerekli !!


namespace Application_WindowService_Processes
{
    class GetServiceStatus
    {
        private static string wsserviceName;
        public static string ServiceDescription { get; set; }
        public static string PathName { get; set; }
        public static string ServiceType { get; set; }
        public static string StartMode { get; set; }
        public static string State { get; set; }
        public string ServiceName
        {

            set
            {
                wsserviceName = value;
            }


        }
        public static void GetWindowsServiceStatus()
        {
            if (wsserviceName != null)
            {
                GetServiceStartMode(wsserviceName);
            }




        }
        private static void GetServiceStartMode(string serviceName)
        {
            string filter = String.Format("SELECT * FROM Win32_Service WHERE Name = '{0}'", serviceName);
            ManagementObjectSearcher query = new ManagementObjectSearcher(filter);

            // No match = failed conditio
            if (query != null)
            {
                try
                {
                    ManagementObjectCollection services = query.Get();

                    foreach (ManagementObject service in services)
                    {
                        DateTime now = DateTime.Now;

                        PathName = service.GetPropertyValue("PathName").ToString();
                        ServiceType = service.GetPropertyValue("ServiceType").ToString();
                        StartMode = service.GetPropertyValue("StartMode").ToString();
                        State = service.GetPropertyValue("State").ToString();
                        ServiceDescription = "None";
                        if (service.GetPropertyValue("Description") != null) ServiceDescription = service.GetPropertyValue("Description").ToString();


                    }
                }
                catch (Exception ex)
                {

                }
            }

        }


    }
    public class Ws
    {
        public ServiceController service;
        GetServiceStatus _GetServiceStatus = new GetServiceStatus();
        private const int RestartTimeout = 10000;

        public void status(string servicename)
        {


            if (servicename == null) return;
            try
            {
                ServiceControl(servicename);
                service.Refresh();

                Console.WriteLine("SUCCESS", servicename.ToString() + service.Status.ToString());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //   EventServiceLog.WriteLogService("ERROR", servicename.ToString() + ex.ToString());

                //EventServiceLog.WriteLogService("ERROR", ex.Message.ToString(), "", "", "", "", "", "");

            }

        }

        public void ServiceControl(string serviceName)
        {
            service = new ServiceController(serviceName);

            if (serviceName != null)
            {

                _GetServiceStatus.ServiceName = serviceName;
                GetServiceStatus.GetWindowsServiceStatus();


            }
        }


        public void start(string servicename)
        {

            if (servicename != null)
            {



                try
                {
                    ServiceControl(servicename);
                    service.Refresh();

                    if (service.Status != ServiceControllerStatus.Running)
                    {
                        Console.WriteLine(servicename.ToString() + "  Service Starting....");
                        //EventServiceLog.WriteLog(servicename.ToString() + " Service Starting", EventLogEntryType.Information);

                        service.Start();

                        Thread.Sleep(200);
                        int i = 0;

                        while (service.Status != ServiceControllerStatus.Running)
                        {
                            service.Refresh();
                            Thread.Sleep(1000);
                            i++;

                            if (i >= RestartTimeout / 100)
                            {
                                //   MessageBox.Show(@"Restart Stop Timeout Exceeded");
                                break;
                            }
                        }


                        service.Refresh();
                        Console.WriteLine("Service Status  " + service.Status.ToString());
                        //EventServiceLog.WriteLog("Service Status  " + service.Status.ToString(), EventLogEntryType.Information);
                        //EventServiceLog.WriteLogService("SUCCESS", servicename.ToString() + service.Status.ToString(), service.DisplayName, GetServiceStatus.ServiceDescription, GetServiceStatus.State, GetServiceStatus.StartMode, GetServiceStatus.PathName, GetServiceStatus.ServiceType);
                    }
                    else
                    {
                        // already service start
                        Console.WriteLine("Service Status  " + service.Status.ToString());
                        //EventServiceLog.WriteLog("already service start....Service Name :  " + servicename.ToString(), EventLogEntryType.Information);
                        //EventServiceLog.WriteLogService("SUCCESS", servicename.ToString() + service.Status.ToString(), service.DisplayName, GetServiceStatus.ServiceDescription, GetServiceStatus.State, GetServiceStatus.StartMode, GetServiceStatus.PathName, GetServiceStatus.ServiceType);
                    }

                }
                catch (Exception Ex)
                {

                    Console.WriteLine(Ex);
                    //EventServiceLog.WriteLog(Ex.ToString(), EventLogEntryType.Error);
                    //EventServiceLog.WriteLogService("ERROR","SERVICE NOT FOUND","","S","",);
                    // _EMVCORE.WriteLogApp("KioskLineCheckService", e.ToString());

                }





            }

        }


        public void Stop(string servicename)
        {

            if (servicename != null)
            {



                try
                {
                    ServiceControl(servicename);

                    if (service.Status != ServiceControllerStatus.Stopped)
                    {
                        Console.WriteLine(servicename.ToString() + "  Service Stoping....");
                        //EventServiceLog.WriteLog(servicename.ToString() + "  Service Stoping....", EventLogEntryType.Information);
                        service.Stop();



                        int i = 0;

                        while (service.Status != ServiceControllerStatus.Stopped)
                        {
                            service.Refresh();
                            Thread.Sleep(1000);
                            i++;

                            if (i >= RestartTimeout / 100)
                            {
                                //   MessageBox.Show(@"Restart Stop Timeout Exceeded");
                                break;
                            }
                        }


                        Console.WriteLine("Service Status  " + service.Status.ToString());
                        //EventServiceLog.WriteLog(servicename.ToString() + "  Service Status :" + service.Status.ToString(), EventLogEntryType.Information);
                        //EventServiceLog.WriteLogService("SUCCESS", servicename.ToString() + service.Status.ToString(), service.DisplayName, GetServiceStatus.ServiceDescription, GetServiceStatus.State, GetServiceStatus.StartMode, GetServiceStatus.PathName, GetServiceStatus.ServiceType);
                    }
                    else
                    {
                        Console.WriteLine(servicename.ToString() + "Service Already STOP");

                        //EventServiceLog.WriteLog(servicename.ToString() + "  Service Already STOP....", EventLogEntryType.Warning);
                        //EventServiceLog.WriteLogService("SUCCESS", servicename.ToString() + service.Status.ToString(), service.DisplayName, GetServiceStatus.ServiceDescription, GetServiceStatus.State, GetServiceStatus.StartMode, GetServiceStatus.PathName, GetServiceStatus.ServiceType);
                        // already service stop
                    }

                }
                catch (Exception ex)
                {
                    //EventServiceLog.WriteLogService("ERROR", ex.Message.ToString(), service.DisplayName, GetServiceStatus.ServiceDescription, GetServiceStatus.State, GetServiceStatus.StartMode, GetServiceStatus.PathName, GetServiceStatus.ServiceType);


                    Console.WriteLine(ex.Message.ToString());

                    // _EMVCORE.WriteLogApp("KioskLineCheckService", e.ToString());



                }





            }



        }

        public void Restart(string servicename)
        {
            if (servicename == null) return;
            int i = 0;
            try
            {
                ServiceControl(servicename);

                if (service.Status != ServiceControllerStatus.Stopped)
                {
                    service.Stop();
                    Console.WriteLine(servicename.ToString() + "  Service Stoped");
                    Thread.Sleep(1000);
                    service.Refresh();
                    //EventServiceLog.WriteLog(servicename.ToString() + "  Service Stoping....", EventLogEntryType.Information);

                    while (service.Status != ServiceControllerStatus.Stopped)
                    {
                        service.Refresh();
                        Thread.Sleep(100);
                        i++;

                        if (i >= RestartTimeout / 100)
                        {
                            //   MessageBox.Show(@"Restart Stop Timeout Exceeded");
                            break;
                        }

                        //EventServiceLog.WriteLogService("SUCCESS", servicename.ToString() + service.Status.ToString(), service.DisplayName, GetServiceStatus.ServiceDescription, GetServiceStatus.State, GetServiceStatus.StartMode, GetServiceStatus.PathName, GetServiceStatus.ServiceType);
                        Console.WriteLine(servicename.ToString() + "  Service  STOP");
                    }


                    Thread.Sleep(1000);
                    service.Refresh();
                    service.Start();

                    Thread.Sleep(1000);
                    service.Refresh();

                    Console.WriteLine(servicename.ToString() + "  Service Sterted");
                    //EventServiceLog.WriteLog(servicename.ToString() + "  Service Sterted....", EventLogEntryType.Information);

                    i = 0;
                    while (service.Status != ServiceControllerStatus.Running)
                    {
                        service.Refresh();

                        service.Start();
                        Thread.Sleep(1000);
                        i++;

                        if (i >= RestartTimeout / 100)
                        {
                            //   MessageBox.Show(@"Restart Stop Timeout Exceeded");
                            break;
                        }



                    }


                }
                else if (service.Status == ServiceControllerStatus.Stopped)
                {

                    service.Start();
                    Console.WriteLine(servicename.ToString() + "  Service Sterted");
                    //EventServiceLog.WriteLog(servicename.ToString() + "  Service Sterted....", EventLogEntryType.Information);
                    //EventServiceLog.WriteLogService("SUCCESS", servicename.ToString() + service.Status.ToString(), service.DisplayName, GetServiceStatus.ServiceDescription, GetServiceStatus.State, GetServiceStatus.StartMode, GetServiceStatus.PathName, GetServiceStatus.ServiceType);

                    i = 0;
                    while (service.Status != ServiceControllerStatus.Running)
                    {
                        service.Refresh();
                        Thread.Sleep(1000);
                        i++;




                    }



                }

                Console.WriteLine("Service Status  " + service.Status.ToString());
                ////EventServiceLog.WriteLog("Service Status" + service.Status.ToString(), EventLogEntryType.Information);
                ////EventServiceLog.WriteLogService("SUCCESS", servicename.ToString() + service.Status.ToString(), service.DisplayName, GetServiceStatus.ServiceDescription, GetServiceStatus.State, GetServiceStatus.StartMode, GetServiceStatus.PathName, GetServiceStatus.ServiceType);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //EventServiceLog.WriteLogApp("ERROR", servicename.ToString() + service.Status.ToString());

                //EventServiceLog.WriteLogService("ERROR", ex.Message.ToString(), "", "", "", "", "", "");

            }


        }
    }
}
