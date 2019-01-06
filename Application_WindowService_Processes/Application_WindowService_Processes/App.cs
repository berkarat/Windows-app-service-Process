using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_WindowService_Processes
{
    public class App
    {
        public string _status;
        public bool Status(string _progressname, string _apppath)
        {

            bool _retvalue = false;


            try
            {


                if (_progressname == null) return false;
                _status = "Not Found Progress";


                if (GetProgress(_progressname) != null) _status = "Running";
                if (GetProgress(_progressname) == null) _status = "Not Running";



                _retvalue = true;

            }
            catch (Exception)
            {
                _retvalue = false;

            }

            return _retvalue;

        }
        private Process GetProgress(string _progressname)
        {



            Process retPrc = null;

            try
            {


                if (_progressname.IndexOf(".exe") != -1) _progressname = _progressname.Substring(0, _progressname.IndexOf(".exe"));
                Process[] Prc = Process.GetProcessesByName(_progressname);



                if (Prc[0].Id > 0) retPrc = Prc[0];

            }
            catch (Exception ex)
            {
                retPrc = null;


            }


            return retPrc;


        }



        public bool Start(string _progressname, string _apppath)
        {

            if (_progressname == null) return false;

            bool _retvalue = false;


            string _status = "Progress Start";

            Process prcid = GetProgress(_progressname);

            if (prcid == null)
            {


                try
                {
                    string _appname = _apppath + _progressname;
                    Process proc = new Process();
                    proc.StartInfo.FileName = _appname;
                    proc.StartInfo.UseShellExecute = true;
                    proc.StartInfo.Verb = "runas";
                    proc.Start();
                    //EventServiceLog.WriteLogApp("SUCCESS", _progressname.ToString() + " Start");
                    _retvalue = true;
                }
                catch (Exception ex)
                {
                    //EventServiceLog.WriteLogApp("Error :" + ex.ToString(), _progressname.ToString() + " Starting");
                    _retvalue = false;
                }

            }
            else
            {
                //EventServiceLog.WriteLogApp("Application Already Running...>", _progressname.ToString() + " still working");
                _retvalue = false;
            }

            return _retvalue;
        }

        public bool Close(string _progressname, string _apppath)
        {


            if (_progressname == null) return false;

            bool _retvalue = false;



            Process prcid = GetProgress(_progressname);



            if (prcid != null)
            {


                try
                {



                    if (prcid.Id > 1)
                    {
                        prcid.Kill();
                        prcid.WaitForExit();
                        //EventServiceLog.WriteLogApp("SUCCESS", _progressname.ToString() + " Closed");
                        _retvalue = true;
                    }
                }
                catch (Exception ex)
                {
                    //EventServiceLog.WriteLogApp("Error :" + ex.ToString(), _progressname.ToString() + " Closing");
                    _retvalue = false;
                }

            }
            else
            {

                //EventServiceLog.WriteLogApp("Application Already Closed...>", _progressname.ToString() + " not Close  ");
                _retvalue = false;


            }

            return _retvalue;

        }


        public bool Restart(string _progressname, string _apppath)
        {


            if (_progressname == null) return false;

            bool _retvalue = false;


            try
            {

                if (Close(_progressname, _apppath))
                {

                    // close ok
                    if (Start(_progressname, _apppath))
                    {
                        //start ok
                        _retvalue = true;
                    }
                    else
                    {
                        _retvalue = false;
                    }

                }
                else
                {
                    _retvalue = false;


                    if (Start(_progressname, _apppath))
                    {
                        //start ok
                        _retvalue = true;

                    }
                    else
                    {
                        _retvalue = false;
                    }


                }



            }
            catch (Exception)
            {
                _retvalue = false;

            }



            return _retvalue;


        }
    }
}
