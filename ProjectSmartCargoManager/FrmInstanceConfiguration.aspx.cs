using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using BAL;
using System.Drawing;

namespace ProjectSmartCargoManager
{
    public partial class FrmInstanceConfiguration : System.Web.UI.Page
    {
        #region Variables

        BALInstanceConfiguration objBAL = new BALInstanceConfiguration();

        #endregion


        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                DataSet dsSave = new DataSet();
                try
                {
                    lblStatus.Text = string.Empty;

                    dsSave = objBAL.ListConfigDetails();

                    if (dsSave != null && dsSave.Tables.Count > 0 && dsSave.Tables[0].Rows.Count > 0)
                    {
                        TxtCntId.Text = dsSave.Tables[0].Rows[0]["ClientID"].ToString();
                        TxtCntId.Enabled = false;
                        TxtClntNm.Text = dsSave.Tables[0].Rows[0]["ClientName"].ToString();
                        TxtClntNm.Enabled = false;
                        TxtAddr.Text = dsSave.Tables[0].Rows[0]["ClientAddress"].ToString();
                        TxtEmail.Text = dsSave.Tables[0].Rows[0]["EmailID"].ToString();
                        TxtPhNo.Text = dsSave.Tables[0].Rows[0]["PhoneNum"].ToString();
                        TxtMblNo.Text = dsSave.Tables[0].Rows[0]["MobileNum"].ToString();
                        TxtFax.Text = dsSave.Tables[0].Rows[0]["FaxNum"].ToString();
                        TxtRegOffAddr.Text = dsSave.Tables[0].Rows[0]["RegOfficeAddress"].ToString();
                        TxtRegoffPh.Text = dsSave.Tables[0].Rows[0]["RegOfficePhoneNum"].ToString();
                        TxtURL.Text = dsSave.Tables[0].Rows[0]["ContactURL"].ToString();
                        TxtSuprtEmail.Text = dsSave.Tables[0].Rows[0]["CustomerSupportEmail"].ToString();
                        TxtSuprtPh.Text = dsSave.Tables[0].Rows[0]["CustomerSupportPhone"].ToString();
                    }
                    else
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = "No Record Found!!!";
                        lblStatus.ForeColor = Color.Red;
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        #endregion

        #region Button Save
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            DataSet dsSave = new DataSet();

            try
            {
                lblStatus.Text = string.Empty;

                #region Prepare Parameters

                object[] InstConfig = new object[12];
                int i = 0;

                InstConfig.SetValue(TxtClntNm.Text, i);
                i++;

                InstConfig.SetValue(TxtAddr.Text,i);
                i++;

                InstConfig.SetValue(TxtEmail.Text,i);
                i++;

                InstConfig.SetValue(TxtPhNo.Text,i);
                i++;

                InstConfig.SetValue(TxtMblNo.Text,i);
                i++;

                InstConfig.SetValue(TxtFax.Text,i);
                i++;

                InstConfig.SetValue(TxtRegOffAddr.Text,i);
                i++;

                InstConfig.SetValue(TxtRegoffPh.Text,i);
                i++;

                InstConfig.SetValue(TxtURL.Text,i);
                i++;

                InstConfig.SetValue(TxtSuprtEmail.Text,i);
                i++;

                InstConfig.SetValue(TxtSuprtPh.Text,i);
                i++;

                InstConfig.SetValue(TxtCntId.Text, i);
                i++;

                #endregion

                dsSave = objBAL.SaveConfigDetails(InstConfig);

                if (dsSave != null && dsSave.Tables.Count > 0 && dsSave.Tables[0].Rows.Count > 0)
                {
                    if (dsSave.Tables[0].Rows[0][0].ToString() == "INSERTED")
                    {
                        #region for Master Audit Log
                        MasterAuditBAL ObjMAL = new MasterAuditBAL();
                        #region Prepare Parameters
                        object[] Paramsss = new object[7];
                        int k = 0;

                        //1
                        Paramsss.SetValue("Instance Configuration", k);
                        k++;

                        //2
                        string MstValue = TxtCntId.Text;
                        Paramsss.SetValue(MstValue, k);
                        k++;

                        //3
                        Paramsss.SetValue("ADD", k);
                        k++;

                        //4
                        string Msg = "";
                        Paramsss.SetValue(Msg, k);
                        k++;

                        //5
                        string Desc = "";
                        Paramsss.SetValue(Desc, k);
                        k++;

                        //6

                        Paramsss.SetValue(Session["UserName"], k);
                        k++;

                        //7
                        Paramsss.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), k);
                        k++;

                        #endregion Prepare Parameters
                        ObjMAL.AddMasterAuditLog(Paramsss);
                        #endregion

                        lblStatus.Visible = true;
                        lblStatus.Text = "Record Inserted Successfully !!!";
                        lblStatus.ForeColor = Color.Green;
                    }
                    else if (dsSave.Tables[0].Rows[0][0].ToString() == "UPDATED")
                    {
                        #region for Master Audit Log
                        MasterAuditBAL ObjMAL = new MasterAuditBAL();
                        #region Prepare Parameters
                        object[] Paramsss = new object[7];
                        int k = 0;

                        //1
                        Paramsss.SetValue("Instance Configuration", k);
                        k++;

                        //2
                        string MstValue = TxtCntId.Text;
                        Paramsss.SetValue(MstValue, k);
                        k++;

                        //3
                        Paramsss.SetValue("UPDATE", k);
                        k++;

                        //4
                        string Msg = "";
                        Paramsss.SetValue(Msg, k);
                        k++;

                        //5
                        string Desc = "";
                        Paramsss.SetValue(Desc, k);
                        k++;

                        //6

                        Paramsss.SetValue(Session["UserName"], k);
                        k++;

                        //7
                        Paramsss.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), k);
                        k++;

                        #endregion Prepare Parameters
                        ObjMAL.AddMasterAuditLog(Paramsss);
                        #endregion
                        lblStatus.Text = string.Empty;
                        lblStatus.Visible = true;
                        lblStatus.Text = "Record Updated Successfully !!!";
                        lblStatus.ForeColor = Color.Green;
                    }
                }

                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Error In Inserting Data !!!";
                    lblStatus.ForeColor = Color.Red;
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error:" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }

        #endregion


        #region Button Clear
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            TxtCntId.Text = string.Empty;
            TxtCntId.Enabled = true;
            TxtClntNm.Text = string.Empty;
            TxtClntNm.Enabled = true;
            TxtAddr.Text = string.Empty;
            TxtEmail.Text = string.Empty;
            TxtPhNo.Text = string.Empty;
            TxtMblNo.Text = string.Empty;
            TxtFax.Text = string.Empty;
            TxtRegOffAddr.Text = string.Empty;
            TxtRegoffPh.Text = string.Empty;
            TxtURL.Text = string.Empty;
            TxtSuprtEmail.Text = string.Empty;
            TxtSuprtPh.Text = string.Empty;
            lblStatus.Text = string.Empty;
        }

        #endregion
    }
}
