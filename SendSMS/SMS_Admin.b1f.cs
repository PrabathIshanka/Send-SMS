using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Specialized;
using SAPbobsCOM;
using Sap.Data.Hana;
using System.Data;
using System.Text.RegularExpressions;
using static SendSMS.SendSMS;
using System.Net;
using System.IO;

namespace SendSMS
{
    [FormAttribute("SendSMS.SMS_Admin", "SMS_Admin.b1f")]
    class SMS_Admin : UserFormBase
    {

        SAPbouiCOM.Form oForm;
        SAPbobsCOM.Company oCompany;
        SAPbobsCOM.Recordset oRecordset, oRecordset3;
        SAPbouiCOM.DBDataSource DS;
        string FMCode = "";
        SAPbouiCOM.DataTable DT_Body;

        String ReportPath;
        String Driver;
        DataTable dt;
        string valu;
        string result;



        public SMS_Admin(string Code)
        {
            try
            {
                this.FMCode = Code;
                oCompany = (SAPbobsCOM.Company)Application.SBO_Application.Company.GetDICompany();
                oRecordset = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                Application.SBO_Application.FormDataEvent += new SAPbouiCOM._IApplicationEvents_FormDataEventEventHandler(ref FormDataEvent);
                ComboBox0.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;

                String sqlServer = ConfigurationSettings.AppSettings["SS"];
                if (sqlServer == "SQL2012") oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;
                else if (sqlServer == "SQL2014") oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2014;
                else if (sqlServer == "SQL2016") oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2016;
                else if (sqlServer == "HANA") oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB;
                else oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL;

                Driver = ConfigurationSettings.AppSettings["RpoPath"];

                string error = oCompany.GetLastErrorDescription();

                HanaConnection hanaConn = new HanaConnection(Driver);

                hanaConn.Open();

                dt = new DataTable();
                HanaCommand cmd = new HanaCommand("call \"APL_DEV\".\"SendEmail\" ('\"APL_DEV\"')", hanaConn);
                HanaDataAdapter da = new HanaDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 300;
                da.Fill(dt);

                hanaConn.Close();

                DT_Body = oForm.DataSources.DataTables.Item("DT_Body");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DT_Body.Rows.Add();
                    DT_Body.SetValue("COLUMN_NAME", i, dt.Rows[i]["COLUMN_NAME"]);
                }
                LoadData();
                // SMS();
                Grid1.AutoResizeColumns();

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(DateTime.Now, "Error Details", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString());
                Application.SBO_Application.SetStatusBarMessage(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
        }
       
        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            //   this.Grid0.DoubleClickAfter += new SAPbouiCOM._IGridEvents_DoubleClickAfterEventHandler(this.Grid0_DoubleClickAfter);
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.Button0.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button0_ClickBefore);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("Item_26").Specific));
            this.StaticText6 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_41").Specific));
            this.EditText7 = ((SAPbouiCOM.EditText)(this.GetItem("txtSub").Specific));
            this.StaticText7 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_43").Specific));
            this.EditText8 = ((SAPbouiCOM.EditText)(this.GetItem("txtBody").Specific));
            this.Grid1 = ((SAPbouiCOM.Grid)(this.GetItem("Item_46").Specific));
            this.Grid1.DoubleClickAfter += new SAPbouiCOM._IGridEvents_DoubleClickAfterEventHandler(this.Grid1_DoubleClickAfter);
            this.ComboBox0 = ((SAPbouiCOM.ComboBox)(this.GetItem("Item_2").Specific));
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("Item_3").Specific));
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_4").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_5").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_6").Specific));
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_7").Specific));
            this.EditText1 = ((SAPbouiCOM.EditText)(this.GetItem("Item_8").Specific));
            this.EditText2 = ((SAPbouiCOM.EditText)(this.GetItem("Item_9").Specific));
            this.EditText3 = ((SAPbouiCOM.EditText)(this.GetItem("Item_10").Specific));
            this.Folder2 = ((SAPbouiCOM.Folder)(this.GetItem("Item_13").Specific));
            this.Folder3 = ((SAPbouiCOM.Folder)(this.GetItem("Item_14").Specific));
            this.StaticText4 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_0").Specific));
            this.ComboBox1 = ((SAPbouiCOM.ComboBox)(this.GetItem("Item_1").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.LoadAfter += new LoadAfterHandler(this.Form_LoadAfter);

        }

        private void OnCustomInitialize()
        {

        }

        private void Form_LoadAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            oForm = Application.SBO_Application.Forms.Item(pVal.FormUID);

        }

        public void FormDataEvent(ref SAPbouiCOM.BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (!BusinessObjectInfo.BeforeAction && BusinessObjectInfo.ActionSuccess && BusinessObjectInfo.FormUID == "SendSMS.SMS_Admin" && (BusinessObjectInfo.EventType == SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD))
            {
               
            }
        }

        private void LoadData()
        {
            try
            {
                oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;

                SAPbouiCOM.Conditions oConditions;
                SAPbouiCOM.Condition oCondition;
                oConditions = new SAPbouiCOM.Conditions();

                oCondition = oConditions.Add();
                oCondition.Alias = "Code";
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                oCondition.CondVal = FMCode;
                oForm.DataSources.DBDataSources.Item("@SMS_ADMINISTRATION").Query(oConditions);

              //  SAPbouiCOM.EditText TBLName = (SAPbouiCOM.EditText)oForm.Items.Item("Item_1").Specific;
            
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(DateTime.Now, "Error Details", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString());
                Application.SBO_Application.SetStatusBarMessage(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }


        }


        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;

        private void Button0_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
           //string sub = EditText7.Value;
           //string body = EditText8.Value;

           // SendSMS.subject = sub  ;
           // SendSMS.body= body  ;

        }
        private SAPbouiCOM.StaticText StaticText6;
        private SAPbouiCOM.EditText EditText7;
        private SAPbouiCOM.StaticText StaticText7;
        private SAPbouiCOM.EditText EditText8;

        private void Grid1_DoubleClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            oForm = Application.SBO_Application.Forms.Item(pVal.FormUID);
            SAPbouiCOM.EditText txtMsg = (SAPbouiCOM.EditText)oForm.Items.Item("txtMsg").Specific;



            SAPbouiCOM.DataTable DT_Body = oForm.DataSources.DataTables.Item("DT_Body");
            int selectRow = 0;
            string Hash = "#";

            for (int i = 0; i < Grid1.Rows.Count; i++)
            {
                if (Grid1.Rows.IsSelected(i))
                {
                    selectRow = i;
                    string Code = DT_Body.Columns.Item("COLUMN_NAME").Cells.Item(selectRow).Value.ToString();
                    txtMsg.Value = txtMsg.Value + Hash + Code + Hash;                  
                }
            }
           

        }

        private SAPbouiCOM.Grid Grid1;
        private SAPbouiCOM.ComboBox ComboBox0;
        private SAPbouiCOM.EditText EditText0;
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.StaticText StaticText3;
        private SAPbouiCOM.EditText EditText1;
        private SAPbouiCOM.EditText EditText2;
        private SAPbouiCOM.EditText EditText3;
        private SAPbouiCOM.Folder Folder2;
        private SAPbouiCOM.Folder Folder3;
        private SAPbouiCOM.StaticText StaticText4;
        private SAPbouiCOM.ComboBox ComboBox1;
    }
}
