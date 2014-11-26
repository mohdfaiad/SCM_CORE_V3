using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using BAL;
using System.Data;
using QID.DataAccess;

namespace ProjectSmartCargoManager
{
    public partial class ListRM : System.Web.UI.Page
    {
        #region Variables
        SQLServer db = new SQLServer(Global.GetConnectionString());
        string strfromdate, strtodate;
        BALRejectionMemo RM = new BALRejectionMemo();
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblStatus.Text = ""; //GetRMDetails(); 

                //Agent authorization
                string AgentCode = Convert.ToString(Session["AgentCode"]);

                if (AgentCode != "")
                {
                    txtAgent.Text = AgentCode;
                    txtAgent.ReadOnly = true;
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
        }
        #endregion

        #region Button Print Rejection Memo
        protected void btnPrintRM_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                RMPrint();
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion

        #region Button Print AWB Rejection Memo 
        protected void btnPrintAWBRM_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                RMPerAWBPrint();
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }
        #endregion

        #region Button List
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                GetRMDetails();
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion

        #region Button Clear
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/ListRM.aspx");
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion

        public void GetRMDetails()
        {
            try
            {
                int i = 0;
                object[] objRM = new object[9];
                objRM.SetValue(txtRM.Text, i);
                objRM.SetValue(txtPreAWB.Text.Trim() + txtAWB.Text.Trim(), ++i);
                objRM.SetValue(txtInvoiceNo.Text, ++i);
                objRM.SetValue(txtAgent.Text, ++i);

                //Validation for From date
                if (txtRMFrom.Text == "")
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
                    string day = txtRMFrom.Text.Substring(0, 2);
                    string mon = txtRMFrom.Text.Substring(3, 2);
                    string yr = txtRMFrom.Text.Substring(6, 4);
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
                if (txtRMTo.Text == "")
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
                    string day = txtRMTo.Text.Substring(0, 2);
                    string mon = txtRMTo.Text.Substring(3, 2);
                    string yr = txtRMTo.Text.Substring(6, 4);
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

                objRM.SetValue(Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), ++i);
                objRM.SetValue(Convert.ToDateTime(strtodate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"), ++i);
                objRM.SetValue(ddlRMType.SelectedValue, ++i);
                objRM.SetValue(ddlStatus.SelectedValue, ++i);
                objRM.SetValue(chkInterline.Checked, ++i);

                //ClearRM();
                DataSet ds = RM.ListRM(objRM);
                if (ds != null)
                {

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Session["dsRM"] = ds;
                            GrdRMDetails.Visible = true;
                            lblStatus.Visible = false;
                            btnPrintRM.Visible = true;
                            btnPrintAWBRM.Visible = true;
                            GrdRMDetails.DataSource = ds.Tables[0];
                            GrdRMDetails.DataBind();
                        }
                        else
                        {
                            GrdRMDetails.Visible = false;
                            lblStatus.Visible = true;
                            btnPrintRM.Visible = false;
                            btnPrintAWBRM.Visible = false;
                            lblStatus.Text = "No Data Found";
                            lblStatus.ForeColor = Color.Blue;

                        }
                    }
                }
            }
            catch (Exception ex)
            { }

        }

        protected void GrdRMDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dst = (DataSet)Session["dsRM"];
            GrdRMDetails.PageIndex = e.NewPageIndex;
            GrdRMDetails.DataSource = dst.Tables[0];
            GrdRMDetails.DataBind();
        }

        protected void RMPrint()
        {
            try
            {
                DataSet dsRM = (DataSet)Session["dsRM"];
                string RMList = "";
                for (int j = 0; j < GrdRMDetails.Rows.Count; j++)
                {
                    if (((CheckBox)GrdRMDetails.Rows[j].FindControl("ChkSelect")).Checked)
                    {

                        #region Prepare Parameters
                        object[] RateCardInfo = new object[1];
                        int irow = 0;

                        RateCardInfo.SetValue(dsRM.Tables[0].Rows[j + (GrdRMDetails.PageIndex * GrdRMDetails.PageSize)]["RMNumber"].ToString(), irow);

                        #endregion Prepare Parameters

                        if (RMList == "")
                        {
                            RMList = RMList + dsRM.Tables[0].Rows[j + (GrdRMDetails.PageIndex * GrdRMDetails.PageSize)]["RMNumber"].ToString();
                        }
                        else
                        {
                            RMList = RMList + "," + dsRM.Tables[0].Rows[j + (GrdRMDetails.PageIndex * GrdRMDetails.PageSize)]["RMNumber"].ToString();
                        }

                        //GenerateAgentInvoice(RateCardInfo);

                    }
                }

                if (RMList.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select RM to print";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
                else
                {
                    lblStatus.Text = "";
                    hfRMNo.Value = RMList;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>printRMList();</script>", false); 

                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void RMPerAWBPrint()
        {
            try
            {
                DataSet dsRM = (DataSet)Session["dsRM"];
                string AWBList = "";
                for (int j = 0; j < GrdRMDetails.Rows.Count; j++)
                {
                    if (((CheckBox)GrdRMDetails.Rows[j].FindControl("ChkSelect")).Checked)
                    {

                        #region Prepare Parameters
                        object[] RateCardInfo = new object[1];
                        int irow = 0;

                        RateCardInfo.SetValue(dsRM.Tables[0].Rows[j + (GrdRMDetails.PageIndex * GrdRMDetails.PageSize)]["AWB"].ToString() + "||" + dsRM.Tables[0].Rows[j + (GrdRMDetails.PageIndex * GrdRMDetails.PageSize)]["FlightNumber"].ToString() + "|" + dsRM.Tables[0].Rows[j + (GrdRMDetails.PageIndex * GrdRMDetails.PageSize)]["FlightDate"].ToString(), irow);

                        #endregion Prepare Parameters

                        if (AWBList == "")
                        {
                            AWBList = AWBList + dsRM.Tables[0].Rows[j + (GrdRMDetails.PageIndex * GrdRMDetails.PageSize)]["AWB"].ToString() + "||" + dsRM.Tables[0].Rows[j + (GrdRMDetails.PageIndex * GrdRMDetails.PageSize)]["FlightNumber"].ToString() + "|" + dsRM.Tables[0].Rows[j + (GrdRMDetails.PageIndex * GrdRMDetails.PageSize)]["FlightDate"].ToString();
                        }
                        else
                        {
                            AWBList = AWBList + "," + dsRM.Tables[0].Rows[j + (GrdRMDetails.PageIndex * GrdRMDetails.PageSize)]["AWB"].ToString() + "||" + dsRM.Tables[0].Rows[j + (GrdRMDetails.PageIndex * GrdRMDetails.PageSize)]["FlightNumber"].ToString() + "|" + dsRM.Tables[0].Rows[j + (GrdRMDetails.PageIndex * GrdRMDetails.PageSize)]["FlightDate"].ToString();
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
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>printRMperAWBList();</script>", false); 

                }


            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
