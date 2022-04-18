using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendSMS
{
    class CommonData
    {
    }

    public static class Logger
    {
        static string logPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SMSandEmailErrorLog";
        static string logPath1 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SMSlTransLog";
        static string logPath2 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\EmailTransLog";

        public static void ErrorLog(DateTime dateTime, string formName, string methodName, string errorMessage)
        {

            try
            {
                if (!Directory.Exists(logPath))
                    Directory.CreateDirectory(logPath);

                if (!File.Exists(logPath + "\\SMSErrorLog.txt"))
                    File.CreateText((logPath + "\\SMSErrorLog.txt"));

                using (StreamWriter writer = new StreamWriter(logPath + "\\ErrorLog.txt", true))
                {
                    writer.WriteLine("\n" + dateTime + " || " + "Form : " + formName + " || " + "Method Name : " + methodName + " || Error :" + errorMessage.Trim());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static bool TransLog(DateTime dateTime, string formName, string methodName)
        {

            try
            {
                if (!Directory.Exists(logPath1))
                    Directory.CreateDirectory(logPath1);

                if (!File.Exists(logPath1 + "\\SMSTransLog.txt"))
                    File.CreateText((logPath1 + "\\SMSTransLog.txt"));

                using (StreamWriter writer = new StreamWriter(logPath1 + "\\SMSTransLog.txt", true))
                {
                    writer.WriteLine("\n" + dateTime + " Transaction Sucessfully ");
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

             public static bool TransLog1(DateTime dateTime, string formName, string methodName)
        {

            try
            {

                if (!Directory.Exists(logPath2))
                    Directory.CreateDirectory(logPath2);

                if (!File.Exists(logPath2 + "\\EmailTransLog.txt"))
                    File.CreateText((logPath2 + "\\EmailTransLog.txt"));

                using (StreamWriter writer = new StreamWriter(logPath2 + "\\EmailTransLog.txt", true))
                {
                    writer.WriteLine("\n" + dateTime + " Transaction Sucessfully ");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }


        }
    }
}
