using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace EnjoyOurTour.Helpers.Logging
{
    public class Logging
    {
        public static void WriteLog(string strMessage)
        {
            try
            {
                string strLogPath = Path.Combine(Path.GetDirectoryName(HttpContext.Current.Server.MapPath("~/Log/")) + "\\", string.Format("Tour_{0}_{1}.log",
                                                             DateTime.Now.Month.ToString(),
                                                             DateTime.Now.Year.ToString()));
                bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Log/"));
                if (!exists)
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Log/"));
                }
                using (StreamWriter sWriter = new StreamWriter(strLogPath, true))
                {
                    sWriter.WriteLine(DateTime.Now.ToString() + ": " + strMessage);
                }

                //Console.WriteLine(DateTime.Now.ToString() + ": " + strMessage);
            }
            catch (IOException ex)
            {

            }
        }
    }
}