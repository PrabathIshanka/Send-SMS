using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;
using System.Net;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Configuration;

namespace SendSMS
{


    public class SendSMS
    {
        SAPbouiCOM.Application oApp;
        SAPbobsCOM.Recordset oRecordset, oRecordset2 , oRecordset3, oRecordset4, oRecordset5, oRecordset6;
        SAPbouiCOM.Form oForm;
        SAPbobsCOM.Company oCompany;

        string result;
        List<string> termsList = new List<string>();
        string Tbale;
        string[] data;
        string[] terms;
        string U_Message;
        List<string> HashCode = new List<string>();
        string newMessage;
        string RepMessage;
        List<string> dataChe1 = new List<string>();
       // string dataChe;
        string dataValue;
        string sendM;
        string mail;
        string mailPassword;
        string smpt;
       // bool r;
        //static string mailSubject;
        //static string mailBody;
        //public static string subject
        //{
        //    get
        //    {
        //        return mailSubject;
        //    }
        //    set
        //    {
        //        mailSubject = subject;
        //    }
        //}

        //public static string body
        //{
        //    get
        //    {
        //        return mailBody;
        //    }
        //    set
        //    {
        //        mailBody = subject;
        //    }
        //}

        public SendSMS()
        {

            oCompany = (SAPbobsCOM.Company)SAPbouiCOM.Framework.Application.SBO_Application.Company.GetDICompany();
            oRecordset = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordset2 = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordset3 = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordset4 = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordset5 = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordset6 = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            mail = System.Configuration.ConfigurationManager.AppSettings["email"];
            mailPassword = ConfigurationManager.AppSettings["ePw"];
            smpt = ConfigurationManager.AppSettings["smpt"];

           

            SAPbouiCOM.Framework.Application.SBO_Application.FormDataEvent += new SAPbouiCOM._IApplicationEvents_FormDataEventEventHandler(ref SBO_Application_DataEvent);
        }

 

        private void SBO_Application_DataEvent(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (!BusinessObjectInfo.BeforeAction && BusinessObjectInfo.ActionSuccess && 
                    ( BusinessObjectInfo.FormTypeEx == "133" || BusinessObjectInfo.FormTypeEx == "149" || BusinessObjectInfo.FormTypeEx == "139" || BusinessObjectInfo.FormTypeEx == "140" ||
                      BusinessObjectInfo.FormTypeEx == "180" || BusinessObjectInfo.FormTypeEx == "234234567" || BusinessObjectInfo.FormTypeEx == "65300" || BusinessObjectInfo.FormTypeEx == "179" ||
                      BusinessObjectInfo.FormTypeEx == "60091" || BusinessObjectInfo.FormTypeEx == "170" || BusinessObjectInfo.FormTypeEx == "426") 
                    && BusinessObjectInfo.EventType == SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD)

                {

                    string Qy = "select \"U_FormStatus\" from \"@SMS_ADMINISTRATION\" where \"U_FormStatus\" is not null  ";
                    oRecordset6.DoQuery(Qy);
                    int FormStatus = Convert.ToInt32( oRecordset6.Fields.Item("U_FormStatus").Value);

                    //if (FormStatus==1)
                    //{
                    //    return;
                    //}
                    //else if (FormStatus == 2)
                    //{
                    //    callSMS();
                    //}
                    //else if (FormStatus == 3)
                    //{
                    //    sendMail();
                    //}
                    //else if(FormStatus == 4)
                    //{
                    //    callSMS();
                    //    sendMail();
                    //}



                   oForm = SAPbouiCOM.Framework.Application.SBO_Application.Forms.Item(BusinessObjectInfo.FormUID);

                    System.Xml.XmlDocument xmldoc = new System.Xml.XmlDocument();
                    xmldoc.LoadXml(BusinessObjectInfo.ObjectKey);
                    Int32 doc = Convert.ToInt32(xmldoc.GetElementsByTagName("DocEntry").Item(0).InnerText);

                    Dictionary<string, string> sms = new Dictionary<string, string>();
                
                    string varble = BusinessObjectInfo.FormTypeEx;

                    if ( varble=="133")
                    {
                        Tbale = "OINV";                       
                    }

                    else if (varble == "149")
                    {
                        Tbale = "OQUT";
                    }

                    else if ((varble == "139"))
                    {
                        Tbale = "ORDR";
                    }

                    else if ((varble == "140"))
                    {
                        Tbale = "ODLN";
                    }

                    else if((varble == "180"))
                    {
                        Tbale = "ORDN";
                    }
                    else if((varble == "234234567"))
                    {
                        Tbale = "ORRR";
                    }

                    else if ((varble == "65300"))
                    {
                        Tbale = "ODPI";
                    }

                    else if ((varble == "179"))
                    {
                        Tbale = "ORIN";
                    }

                    else if ((varble == "60091"))
                    {
                        Tbale = "OINV";
                    }

                    else if ((varble == "170"))
                    {
                        Tbale = "OCRT";
                    }
                    else if ((varble == "426"))
                    {
                        Tbale = "OVPM";
                    }
                   
                    string Qry = "select \"U_Message\" from \"@SMS_ADMINISTRATION\" where \"U_TableCode\" = '" + Tbale + "' and \"U_Message\" <> '' ";
                    oRecordset2.DoQuery(Qry);
                                          
                     U_Message = (oRecordset2.Fields.Item("U_Message").Value).ToString();

                            string[] words = U_Message.Split(' ');


                            for (int i = 0; i < words.LongLength; i++)
                            {
                                string nambr = words[i];

                                string FirstCharacter = nambr.Substring(0, 1);
                                string LastCharscter = nambr.Substring(nambr.Length - 1);


                                if (FirstCharacter.Contains("#") && LastCharscter.Contains("#"))
                                {
                                    string HsahNum = nambr;
                                    HashCode.Add(nambr) ;
                                    int start = nambr.IndexOf("#") + 1;
                                    int end = nambr.IndexOf("#", start);
                                    result = nambr.Substring(start, end - start);
                                    termsList.Add(result);

                                }

                            }
                           terms = termsList.ToArray();

                        string Query = "select * from " + Tbale + " where \"DocEntry\" = " + doc + " ";
                        oRecordset.DoQuery(Query);

                            for (int j = 0; j < oRecordset.Fields.Count; j++)
                            {


                             for (int n = 0; n < termsList.Count; n++)
                             {

                              string dataset = termsList[n];
                              string field = oRecordset.Fields.Item(j).Name.ToString();
                                dataValue = (oRecordset.Fields.Item(j).Value).ToString();

                                if (dataset == field)
                                {
                                    if (sms.ContainsKey(result))
                                    {

                                    }
                                    else
                                    {
                                        sms.Add(field, dataValue);
                                    }

                                }
                            }
                        }
                   

                    newMessage = U_Message;

                    foreach (var item in sms)
                    {
                      
                       newMessage = newMessage.Replace("#" + item.Key + "#", "" + item.Value + "");

                    }

                    if (FormStatus == 1)
                    {
                        return;
                    }
                    else if (FormStatus == 2)
                    {
                        callSMS();
                    }
                    else if (FormStatus == 3)
                    {
                        sendMail();
                    }
                    else if (FormStatus == 4)
                    {
                        callSMS();
                        sendMail();
                    }

                }
               

            }
            catch (Exception ex)
            {
               // Logger.ErrorLog(DateTime.Now, "Error Details", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString());
                SAPbouiCOM.Framework.Application.SBO_Application.SetStatusBarMessage(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }

        }

        public void callSMS()
        {
            try
            {
                string Qry1 = "Select \"Phone1\" from OCRD where \"Phone1\" IS NOT NULL";
                oRecordset3.DoQuery(Qry1);
                string PhoneNo = (oRecordset3.Fields.Item("Phone1").Value).ToString();

                string sURL;
                sURL = "http://sms.textware.lk:5000/sms/send_sms.php?username=caschool&password=FU5YatE2vV&src=CA-NEWS&dst=0710403186&msg= " + newMessage + "&dr=1";

                WebRequest wrGETURL;
                wrGETURL = WebRequest.Create(sURL);

                WebProxy myProxy = new WebProxy("myproxy", 80);
                myProxy.BypassProxyOnLocal = true;

                wrGETURL.Proxy = WebProxy.GetDefaultProxy();

                Stream objStream;
                objStream = wrGETURL.GetResponse().GetResponseStream();

                StreamReader objReader = new StreamReader(objStream);

                bool r = Logger.TransLog(DateTime.Now, " Send Details", System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(DateTime.Now, "Error Details", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString());
            }
        }

        public void sendMail()
        {
            //SAPbobsCOM.Recordset oRecordset4 = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordset4.DoQuery("select \"CardCode\",\"E_Mail\",\"U_SendEmail\" from \"OCRD\" where \"E_Mail\" is not null and \"U_SendEmail\" = 'Y' ");

            string[] array = new string[oRecordset4.RecordCount];
            for (int x = 0; x < array.LongLength; x++)
            {
                array[x] = oRecordset4.Fields.Item("E_Mail").Value.ToString();
                sendM = array[x];

                MailMessage message = new MailMessage();
                message.From = new MailAddress(mail);
                message.To.Add(sendM);

                SAPbobsCOM.Recordset oRecordset5 = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                oRecordset5.DoQuery("select \"U_Subject\",\"U_Body\" from \"@SMS_ADMINISTRATION\" where  \"U_Subject\" is not null ");

                String subject = oRecordset5.Fields.Item("U_Subject").Value.ToString();
                String body = oRecordset5.Fields.Item("U_Body").Value.ToString();


                message.Subject = subject;
                message.Body = body;

                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient(smpt, 587);
                System.Net.NetworkCredential basicCredential1 = new
                System.Net.NetworkCredential(mail, mailPassword);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = basicCredential1;

                ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate, X509Chain chain,
                SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };

                try
                {
                    client.Send(message);
                    bool b = Logger.TransLog1(DateTime.Now, " Send Details", System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
               
                catch (Exception ex)
                {
                    Logger.ErrorLog(DateTime.Now, "Error Details", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString());
                    // Logger.ErrorLog(DateTime.Now, "Error Details", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString());
                }

                oRecordset4.MoveNext();

            }

        }
    }

    }
