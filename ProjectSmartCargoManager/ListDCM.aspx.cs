using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls.WebParts;
using QID.DataAccess;
using System.IO;
using System.Collections;
using BAL;
using System.Drawing;

namespace ProjectSmartCargoManager
{
    public partial class ListDCM : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());
        DebitCreditProcessingBAL DCM = new DebitCreditProcessingBAL();
        string strfromdate, strtodate;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblStatus.Text = ""; //GetDCMDetails(); 

                //Agent authorization
                string AgentCode = Convert.ToString(Session["AgentCode"]);

                if (AgentCode != "")
                {
                    txtAgent.Text = AgentCode;
                    txtAgent.ReadOnly = true;
                }

            }
            if (Session["awbPrefix"] != null)
            {
               txtPreAWB.Text = Session["awbPrefix"].ToString();

            }
            else
            {
                MasterBAL objBal = new MasterBAL();
                Session["awbPrefix"] = objBal.awbPrefix();
                txtPreAWB.Text = Session["awbPrefix"].ToString();
            }

        }

        public void GetDCMDetails()
        {
            try
            {
                int i = 0;
                object[] objDCM = new object[7];
                objDCM.SetValue(txtDCM.Text, i);
                objDCM.SetValue(txtAWB.Text, ++i);
                objDCM.SetValue(txtInvoiceNo.Text, ++i);
                objDCM.SetValue(txtAgent.Text, ++i);

                //Validation for From date
                if (txtDCMFrom.Text == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select Valid date";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

                DateTime dt;

                try
                {
                    //dt = Convert.ToDateTime(txtInvoiceFrom.Text);
                    //Change 03082012
                    string day = txtDCMFrom.Text.Substring(0, 2);
                    string mon = txtDCMFrom.Text.Substring(3, 2);
                    string yr = txtDCMFrom.Text.Substring(6, 4);
                    strfromdate = yr + "-" + mon + "-" + day;
                    dt = Convert.ToDateTime(strfromdate);
                }
                catch (Exception ex)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Selected Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                //Validation for To date
                if (txtDCMTo.Text == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select Valid date";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

                DateTime dtto;

                try
                {
                    //dtto = Convert.ToDateTime(txtInvoiceTo.Text);
                    //Change 03082012
                    string day = txtDCMTo.Text.Substring(0, 2);
                    string mon = txtDCMTo.Text.Substring(3, 2);
                    string yr = txtDCMTo.Text.Substring(6, 4);
                    strtodate = yr + "-" + mon + "-" + day;
                    dtto = Convert.ToDateTime(strtodate);
                }
                catch (Exception ex)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Selected Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                if (dtto < dt)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "To date should be greater than From date";
                    lblStatus.ForeColor = Color.Red;

                    return;
                }

                objDCM.SetValue(Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), ++i);
                objDCM.SetValue(Convert.ToDateTime(strtodate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"), ++i);
                objDCM.SetValue(ddlDCMType.SelectedValue, ++i);

                //ClearDCM();
                DataSet ds = DCM.ListDCM(objDCM);
                if (ds != null)
                {
                   
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Session["dsDCM"] = ds;
                            GrdDCMDetails.Visible = true;
                            lblStatus.Visible = false;
                            btnPrintDCM.Visible = true;
                            btnPrintAWBDCM.Visible = true;
                            GrdDCMDetails.DataSource = ds.Tables[0];
                            GrdDCMDetails.DataBind();
                        }
                        else
                        {
                            GrdDCMDetails.Visible = false;
                            lblStatus.Visible = true;
                            btnPrintDCM.Visible = false;
                            btnPrintAWBDCM.Visible = false;
                            lblStatus.Text = "No Data Found";
                            lblStatus.ForeColor = Color.Blue;

                        }
                    }
                }
            }
            catch (Exception ex)
            { }

        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            GetDCMDetails();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearDCM();
        }

        private void ClearDCM()
        {
            GrdDCMDetails.DataSource = null;
            GrdDCMDetails.DataBind();
            txtAgent.Text = txtDCMFrom.Text = txtDCMTo.Text = txtAWB.Text = txtDCM.Text = txtInvoiceNo.Text = txtPreAWB.Text = "";
            lblStatus.Text = "";
        }

        protected void btnPrintDCM_Click(object sender, EventArgs e)
        {
            DCMPrint();

            Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "printDCMList();", true);
           
        }

        protected void DCMPrint()
        {
            try
            {
                DataSet dsDCM = (DataSet)Session["dsDCM"];
                string DCMList = "";
                for (int j = 0; j < GrdDCMDetails.Rows.Count; j++)
                {
                    if (((CheckBox)GrdDCMDetails.Rows[j].FindControl("ChkSelect")).Checked)
                    {

                        #region Prepare Parameters
                        object[] RateCardInfo = new object[1];
                        int irow = 0;

                        RateCardInfo.SetValue(dsDCM.Tables[0].Rows[j + (GrdDCMDetails.PageIndex * GrdDCMDetails.PageSize)]["DCMNumber"].ToString(), irow);

                        #endregion Prepare Parameters

                        if (DCMList == "")
                        {
                            DCMList = DCMList + dsDCM.Tables[0].Rows[j + (GrdDCMDetails.PageIndex * GrdDCMDetails.PageSize)]["DCMNumber"].ToString();
                        }
                        else
                        {
                            DCMList = DCMList + "," + dsDCM.Tables[0].Rows[j + (GrdDCMDetails.PageIndex * GrdDCMDetails.PageSize)]["DCMNumber"].ToString();
                        }

                        //GenerateAgentInvoice(RateCardInfo);

                    }
                }

                if (DCMList.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select DCM to print";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
                else
                {
                    lblStatus.Text = "";
                    hfDCMNo.Value = DCMList;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void GrdDCMDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dst = (DataSet)Session["dsDCM"];
            GrdDCMDetails.PageIndex = e.NewPageIndex;
            GrdDCMDetails.DataSource = dst.Tables[0];
            GrdDCMDetails.DataBind();
        }

        protected void btnPrintAWBDCM_Click(object sender, EventArgs e)
        {
            DCMPerAWBPrint();

            Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "printDCMperAWBList();", true);
        }

        protected void DCMPerAWBPrint()
        {
            try
            {
                DataSet dsDCM = (DataSet)Session["dsDCM"];
                string AWBList = "";
                for (int j = 0; j < GrdDCMDetails.Rows.Count; j++)
                {
                    if (((CheckBox)GrdDCMDetails.Rows[j].FindControl("ChkSelect")).Checked)
                    {

                        #region Prepare Parameters
                        object[] RateCardInfo = new object[1];
                        int irow = 0;

                        RateCardInfo.SetValue(dsDCM.Tables[0].Rows[j + (GrdDCMDetails.PageIndex * GrdDCMDetails.PageSize)]["AWBNumber"].ToString(), irow);

                        #endregion Prepare Parameters

                        if (AWBList == "")
                        {
                            AWBList = AWBList + dsDCM.Tables[0].Rows[j + (GrdDCMDetails.PageIndex * GrdDCMDetails.PageSize)]["AWBNumber"].ToString();
                        }
                        else
                        {
                            AWBList = AWBList + "," + dsDCM.Tables[0].Rows[j + (GrdDCMDetails.PageIndex * GrdDCMDetails.PageSize)]["AWBNumber"].ToString();
                        }

                        //GenerateAgentInvoice(RateCardInfo);

                    }
                }

                if (AWBList.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select AWB to print";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
                else
                {
                    lblStatus.Text = "";
                    hfAWBNo.Value = AWBList;
                }
               

            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
