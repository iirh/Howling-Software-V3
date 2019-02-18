using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using Howling_LoaderV3;
using System.IO;
using System.Threading;

namespace Howling_LoaderV3
{
    class AutoEvNr
    {
        public class Data
        {

            public decimal version
            {
                get;
                set;
            }
            public string downloadLink
            {
                get;
                set;
            }
            public string Message
            {
                get;
                set;
            }
            public string ChangeLog
            {
                get;
                set;
            }
            public string filename
            {
                get;
                set;
            }
            public bool isClose
            {
                get;
                set;
            }


        }
        public static WebClient wb = new WebClient();
        private static void Close()
        {
            Process.GetCurrentProcess().Kill();
        }
        public static decimal Check4Update(decimal currentVersion)
        {

            try
            {
                AutoEvNr.Data data = new JavaScriptSerializer().Deserialize<AutoEvNr.Data>(AutoEvNr.wb.DownloadString(Globals.UpdateLink));

                if (data.version > currentVersion)
                {
                    Globals.helper = true;
                }
                else
                {
                    Globals.helper = false;
                }
            }
            catch
            {
                Error.CstmError.Show("No Internet Connection, can´t check for Updates");
                Application.Exit();
            }
            return currentVersion;
        }
    }
}
