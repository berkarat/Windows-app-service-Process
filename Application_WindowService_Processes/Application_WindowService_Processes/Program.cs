using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_WindowService_Processes
{
    /// <summary>
    /// Berk Arat 
    /// berkarat.com  
    /// Bu program ile windows servis veya applicationlar üzerinde start stop restart işlemlerini yapabilmekteyiz.
    /// Bunun yanı sıra Status çekme işlemlerini de yapabilmekteyiz.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string command;
            string type = Console.ReadLine();
            switch (type)
            {
                case "1":
                    #region APP 

                    string app_name = null;
                    string app_path = null;
                    string app_type = null;

                    App app_ = new App();
                    Console.WriteLine("Command giriniz :\n 1 - App start\n2 - App Stop\n3 - App Restart\n4 - App Status");
                    command = Console.ReadLine();

                    app_name = "WhatsApp";
                    app_path = @"C:\Users\Berk\AppData\Local\WhatsApp\";
                    app_type = "2";

                    if (app_type == "2") app_name = app_name + ".exe"; // APPLİCATION 2 İSE


                    switch (command)
                    {
                        case "1":
                            #region app_start

                            if (app_.Start(app_name, app_path))
                            {
                                // success start
                                Console.WriteLine("Aplication " + app_name + " Start......successful");
                            }
                            else
                            {
                                Console.WriteLine("APP : " + app_name + " not starting");
                                // error
                            }

                            #endregion
                            break;
                        case "2":
                            #region app_stop

                            if (app_.Close(app_name, app_path))
                            {
                                // success start
                                Console.WriteLine("Aplication " + app_name + " Stop......successful");
                            }
                            else
                            {
                                Console.WriteLine("APP : " + app_name + " not stoping");
                                // error
                            }




                            #endregion
                            break;
                        case "3":
                            #region app_restart



                            if (app_.Restart(app_name, app_path))
                            {
                                // success start
                                Console.WriteLine("Aplication " + app_name + " ReStart......successful");
                            }
                            else
                            {
                                Console.WriteLine("Aplication : " + app_name + " not ReStarting");
                                // error
                            }



                            #endregion
                            break;
                        case "4":
                            #region app_getstatus


                            if (app_.Status(app_name, app_path))
                            {
                                // success start
                                Console.WriteLine("Aplication " + app_name + " Get Status....>successful");
                            }
                            else
                            {
                                Console.WriteLine("Aplication " + app_name + " Get Status....>Error");
                            }


                            #endregion
                            break;

                    }






                    #endregion
                    break;
                case "2":
                    #region WS 

                    string service_name = null;
                    service_name = "ALG";
                    Console.WriteLine("Command giriniz :\n 1 - Service start\n2 - Service Stop\n3 - Service Restart\n4 - Service Status");
                    command = Console.ReadLine();


                    if (service_name != null)
                    {


                        Ws ws_ = new Ws();


                        switch (command)
                        {
                            case "1":
                                ws_.start(service_name);
                                break;
                            case "2":
                                ws_.Stop(service_name);
                                break;
                            case "3":
                                ws_.Restart(service_name);
                                break;
                            case "4":
                                ws_.status(service_name);
                                Console.WriteLine(
                       GetServiceStatus.ServiceDescription + "\n" +
                      GetServiceStatus.State + "\n" +
                     GetServiceStatus.StartMode + "\n" +
                   GetServiceStatus.PathName + "\n" +
                   GetServiceStatus.ServiceType
                   );

                                break;
                        }


                    }
                    else
                    {
                        Console.WriteLine("service_name not found");
                    }



                    #endregion
                    break;
            }



            Console.Read();
        }
    }
}
