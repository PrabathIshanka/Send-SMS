using System;
using System.Collections.Generic;
using System.Xml;
using SAPbouiCOM.Framework;

namespace SendSMS
{
    [FormAttribute("SendSMS.Form1", "Form1.b1f")]
    class Form1 : UserFormBase
    {
        SAPbouiCOM.Form oForm;
        SAPbobsCOM.Company oCompany;
        SAPbobsCOM.Recordset oRecordset;
        SAPbouiCOM.DataTable DTForms;
       // string Code;

        public Form1()
        {
            try
            { 
            oCompany = (SAPbobsCOM.Company)Application.SBO_Application.Company.GetDICompany();
            oRecordset = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
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
            this.Grid0 = ((SAPbouiCOM.Grid)(this.GetItem("Item_0").Specific));
            this.Grid0.DoubleClickAfter += new SAPbouiCOM._IGridEvents_DoubleClickAfterEventHandler(this.Grid0_DoubleClickAfter);
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("Item_2").Specific));
            this.Button1.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.Button1_ClickAfter);
            // this.Button2.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button2_ClickBefore);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.LoadAfter += new LoadAfterHandler(this.Form_LoadAfter);

        }

        private SAPbouiCOM.Grid Grid0;

        private void OnCustomInitialize()
        {

        }

        private SAPbouiCOM.Button Button0;

        private void Form_LoadAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            oForm = Application.SBO_Application.Forms.Item(pVal.FormUID);

        }

        private SAPbouiCOM.Button Button1;

        private void Button1_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                oForm = Application.SBO_Application.Forms.Item(pVal.FormUID);
                DTForms = oForm.DataSources.DataTables.Item("DT_Forms");
                string Qry = "SELECT \"Code\",\"Name\",\"U_Status\",\"U_Message\" FROM \"@SMS_ADMINISTRATION\"";
                DTForms.ExecuteQuery(Qry);
                Grid0.DataTable = DTForms;

            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusBarMessage(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }

        }

        private void Grid0_DoubleClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            int selectRow = 0;

            for (int i = 0; i < Grid0.Rows.Count; i++)
            {
                if (Grid0.Rows.IsSelected(i))
                {
                    selectRow = i;
                    // Code = DTForms.Columns.Item("Code").Cells.Item(selectRow).Value.ToString();
                    break;

                }
            }

            if (selectRow < 0)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.SetStatusBarMessage("Please select the row", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                return;
            }

            if (selectRow >= 0 && Convert.ToInt32(DTForms.GetValue("Code", selectRow)) > 0)
            {
                SMS_Admin activeForm = new SMS_Admin(DTForms.GetValue("Code", selectRow).ToString());
                activeForm.Show();
                //formActive = true;
            }

        }
    }
}