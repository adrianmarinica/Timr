using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Logger
{
    public class ExceptionLogger
    {
        public static void Log(Exception ex)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(@Properties.Settings.Default.ExceptionLogFilePath, true))
                {
                    writer.Write(ex.ToString());
                }
            }
            catch
            {
                throw;
            }
            
        }

        public static void Log(string text)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(@Properties.Settings.Default.ExceptionLogFilePath, true))
                {
                    writer.Write(text);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
