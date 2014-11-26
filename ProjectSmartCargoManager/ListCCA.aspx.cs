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
    public partial class ListCCA : System.Web.UI.Page
    {

        SQLServer da = new SQLServer(Global.GetConnectionString());
        CCABal CCA = new CCABal();
        string strfromdate, strtodate;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    lblStatus.Text = ""; //GetCCADetails(); 

                    //Agent authorization
                    string AgentCode = Convert.ToString(Session["AgentCode"]);

                    if (AgentCode != "")
                    {
                        txtAgent.Text = AgentCode;
                        txtAgent.ReadOnly = true;
                    }
                    txtCCAFrom.Text = Session["IT"] != null ? ((DateTime)Session["IT"]).ToString("dd/MM/yyyy") : string.Empty;
                    txtCCATo.Text = Session["IT"] != null ? ((DateTime)Session["IT"]).ToString("dd/MM/yyyy") : string.Empty;

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
            catch (Exception ex)
            { }

        }

        public void GetCCADetails()
        {
            try
            {
                int i = 0;
                object[] objCCA = new object[8];
                objCCA.SetValue(txtCCA.Text, i);
                string strAWBNo = "";
                if (txtPreAWB.Text.Trim()=="" || txtAWB.Text.Trim()=="")
                    strAWBNo = "";
                else
                    strAWBNo = txtPreAWB.Text.Trim() + txtAWB.Text.Trim();
                objCCA.SetValue(strAWBNo, ++i);
                objCCA.SetValue(txtInvoiceNo.Text.Trim(), ++i);
                objCCA.SetValue(txtAgent.Text.Trim(), ++i);

                //Validation for From date
                if (txtCCAFrom.Text == "")
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
                    string day = txtCCAFrom.Text.Substring(0, 2);
                    string mon = txtCCAFrom.Text.Substring(3, 2);
                    string yr = txtCCAFrom.Text.Substring(6, 4);
                    strfromdate = yr + "/" + mon + "/" + day;
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
                if (txtCCATo.Text == "")
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
                    string day = txtCCATo.Text.Substring(0, 2);
                    string mon = txtCCATo.Text.Substring(3, 2);
                    string yr = txtCCATo.Text.Substring(6, 4);
                    strtodate = yr + "/" + mon + "/" + day;
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

                objCCA.SetValue(Convert.ToDateTime(strfromdate).ToString("yyyy/MM/dd HH:mm:ss"), ++i);
                objCCA.SetValue(Convert.ToDateTime(strtodate).AddDays(1).ToString("yyyy/MM/dd HH:mm:ss"), ++i);
                objCCA.SetValue(ddlCCAType.SelectedValue, ++i);
                objCCA.SetValue(ddlStatus.SelectedValue, ++i);

                //ClearCCA();
                DataSet ds = CCA.ListCCA(objCCA);
                if (ds != null)
                {

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Session["dsCCA"] = ds;
                            GrdCCADetails.Visible = true;
                            lblStatus.Visible = false;
                            btnPrintCCA.Visible = true;
                            btnPrintAWBCCA.Visible = true;
                            GrdCCADetails.DataSource = ds.Tables[0];
                            GrdCCADetails.DataBind();
                            //btnAccept.Visible = true;
                            //btnReject.Visible = true;
                        }
                        else
                        {
                            GrdCCADetails.Visible = false;
                            lblStatus.Visible = true;
                            btnPrintCCA.Visible = false;
                            btnPrintAWBCCA.Visible = false;
                            //btnAccept.Visible = false;
                            //btnReject.Visible = false;
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
            GetCCADetails();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearCCA();
        }

        private void ClearCCA()
        {
            try
            {
                GrdCCADetails.DataSource = null;
                GrdCCADetails.DataBind();
                txtAgent.Text = txtCCAFrom.Text = txtCCATo.Text = txtAWB.Text = txtCCA.Text = txtInvoiceNo.Text = "";
                lblStatus.Text = "";
            }
            catch (Exception ex)
            { }
        }

        protected void btnPrintCCA_Click(object sender, EventArgs e)
        {
            CCAPrint();

            //Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "printCCAList();", true);


        }

        protected void CCAPrint()
        {
            try
            {
                DataSet dsCCA = (DataSet)Session["dsCCA"];
                string CCAList = "";
                for (int j = 0; j < GrdCCADetails.Rows.Count; j++)
                {
                    if (((CheckBox)GrdCCADetails.Rows[j].FindControl("ChkSelect")).Checked)
                    {

                        #region Prepare Parameters
                        object[] RateCardInfo = new object[1];
                        int irow = 0;

                        RateCardInfo.SetValue(dsCCA.Tables[0].Rows[j + (GrdCCADetails.PageIndex * GrdCCADetails.PageSize)]["CCANumber"].ToString(), irow);

                        #endregion Prepare Parameters

                        if (CCAList == "")
                        {
                            CCAList = CCAList + dsCCA.Tables[0].Rows[j + (GrdCCADetails.PageIndex * GrdCCADetails.PageSize)]["CCANumber"].ToString();
                        }
                        else
                        {
                            CCAList = CCAList + "," + dsCCA.Tables[0].Rows[j + (GrdCCADetails.PageIndex * GrdCCADetails.PageSize)]["CCANumber"].ToString();
                        }

                        //GenerateAgentInvoice(RateCardInfo);

                    }
                }

                if (CCAList.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select CCA to print";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
                else
                {
                    lblStatus.Text = "";
                    hfCCANo.Value = CCAList;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>printCCAList();</script>", false);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void GrdCCADetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataSet dst = (DataSet)Session["dsCCA"];
                GrdCCADetails.PageIndex = e.NewPageIndex;
                GrdCCADetails.DataSource = dst.Tables[0];
                GrdCCADetails.DataBind();
            }
            catch (Exception ex)
            { }
        }

        protected void btnPrintAWBCCA_Click(object sender, EventArgs e)
        {
            CCAPerAWBPrint();



            //Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "printCCAperAWBList();", true);
        }

        protected void CCAPerAWBPrint()
        {
            try
            {
                DataSet dsCCA = (DataSet)Session["dsCCA"];
                string AWBList = "";
                for (int j = 0; j < GrdCCADetails.Rows.Count; j++)
                {
                    if (((CheckBox)GrdCCADetails.Rows[j].FindControl("ChkSelect")).Checked)
                    {

                        #region Prepare Parameters
                        object[] RateCardInfo = new object[1];
                        int irow = 0;

                        RateCardInfo.SetValue(dsCCA.Tables[0].Rows[j + (GrdCCADetails.PageIndex * GrdCCADetails.PageSize)]["AWB"].ToString() + "||" + dsCCA.Tables[0].Rows[j + (GrdCCADetails.PageIndex * GrdCCADetails.PageSize)]["FlightNumber"].ToString() + "|" + dsCCA.Tables[0].Rows[j + (GrdCCADetails.PageIndex * GrdCCADetails.PageSize)]["FlightDate"].ToString(), irow);

                        #endregion Prepare Parameters

                        if (AWBList == "")
                        {
                            AWBList = AWBList + dsCCA.Tables[0].Rows[j + (GrdCCADetails.PageIndex * GrdCCADetails.PageSize)]["AWB"].ToString() + "||" + dsCCA.Tables[0].Rows[j + (GrdCCADetails.PageIndex * GrdCCADetails.PageSize)]["FlightNumber"].ToString() + "|" + dsCCA.Tables[0].Rows[j + (GrdCCADetails.PageIndex * GrdCCADetails.PageSize)]["FlightDate"].ToString();
                        }
                        else
                        {
                            AWBList = AWBList + "," + dsCCA.Tables[0].Rows[j + (GrdCCADetails.PageIndex * GrdCCADetails.PageSize)]["AWB"].ToString() + "||" + dsCCA.Tables[0].Rows[j + (GrdCCADetails.PageIndex * GrdCCADetails.PageSize)]["FlightNumber"].ToString() + "|" + dsCCA.Tables[0].Rows[j + (GrdCCADetails.PageIndex * GrdCCADetails.PageSize)]["FlightDate"].ToString();
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
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>printCCAperAWBList();</script>", false);

                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Button Accept
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                lblStatus.Text = "";
                string CCANumber = "", AWBNumber = "";
                for (int i = 0; i < GrdCCADetails.Rows.Count; i++)
                {
                    if (((CheckBox)GrdCCADetails.Rows[i].FindControl("ChkSelect")).Checked)
                    {
                        count++;
                        if (count == 1)
                        {
                            CCANumber = "$" + ((Label)GrdCCADetails.Rows[i].FindControl("lblCCANo")).Text;
                            AWBNumber = "$" + ((Label)GrdCCADetails.Rows[i].FindControl("lblAWBNumber")).Text;
                        }
                        else
                        {

                            CCANumber += "$,$" + ((Label)GrdCCADetails.Rows[i].FindControl("lblCCANo")).Text;
                            AWBNumber += "$,$" + ((Label)GrdCCADetails.Rows[i].FindControl("lblAWBNumber")).Text;

                        }

                    }

                }
                if (count < 1)
                {
                    lblStatus.Text = "Please select the CCA to accept !";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (i == count - 1)
                        {
                            CCANumber += "$";
                            AWBNumber += "$";
                        }
                    }
                    object[] QueryValues = { CCANumber, AWBNumber, "Accepted" };
                    if (CCA.UpdateStatus(QueryValues))
                    {
                        btnList_Click(sender, e);
                        lblStatus.Text = "CCA accepted successfully !";
                        lblStatus.ForeColor = Color.Green;
                        lblStatus.Visible = true;
                    }
                    else
                    {
                        lblStatus.Text = "CCA could not be accepted! Please try again...";
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Visible = true;
                        return;

                    }
                }




            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }
        #endregion

        #region Reject
        protected void btnReject_Click(object sender, EventArgs e)
        {

            try
            {
                int count = 0;
                lblStatus.Text = "";
                string CCANumber = "", AWBNumber = "";
                for (int i = 0; i < GrdCCADetails.Rows.Count; i++)
                {
                    if (((CheckBox)GrdCCADetails.Rows[i].FindControl("ChkSelect")).Checked)
                    {
                        count++;
                        if (count == 1)
                        {
                            CCANumber = "$" + ((Label)GrdCCADetails.Rows[i].FindControl("lblCCANo")).Text;
                            AWBNumber = "$" + ((Label)GrdCCADetails.Rows[i].FindControl("lblAWBNumber")).Text;
                        }
                        else
                        {

                            CCANumber += "$,$" + ((Label)GrdCCADetails.Rows[i].FindControl("lblCCANo")).Text;
                            AWBNumber += "$,$" + ((Label)GrdCCADetails.Rows[i].FindControl("lblAWBNumber")).Text;

                        }

                    }

                }
                if (count < 1)
                {
                    lblStatus.Text = "Please select the CCA to Reject !";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (i == count - 1)
                        {
                            CCANumber += "$";
                            AWBNumber += "$";
                        }
                    }
                    object[] QueryValues = { CCANumber, AWBNumber, "Rejected" };
                    if (CCA.UpdateStatus(QueryValues))
                    {
                        btnList_Click(sender, e);
                        lblStatus.Text = "CCA Rejected Successfully !";
                        lblStatus.ForeColor = Color.Green;
                        lblStatus.Visible = true;
                    }
                    else
                    {
                        lblStatus.Text = "CCA could not be Rejected! Please try again...";
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Visible = true;
                        return;

                    }
                }




            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }
        #endregion

        protected void GrdCCADetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "CCADetails")
                {
                    string[] Values = e.CommandArgument.ToString().Split(',');
                    Response.Redirect("frmCCAProcessing.aspx?AWBNo=" + Values[0] + "&CCANo=" + Values[1] + "&FlightNo=" + Values[2] + "&FlightDate=" + Values[3]);
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;

            }

        }

        protected void GrdCCADetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            int intIndex = GrdCCADetails.SelectedIndex;
            string strCCANo = ((LinkButton)GrdCCADetails.Rows[intIndex].FindControl("lnkCCANo")).Text;
            string strAWBPre = ((Label)GrdCCADetails.Rows[intIndex].FindControl("lblAWBPrefix")).Text;
            string strAWBNo = ((Label)GrdCCADetails.Rows[intIndex].FindControl("lblAWBNumber")).Text;
            Response.Redirect("frmCCAProcessing.aspx?AWBNo=" +strAWBPre+ strAWBNo + "&CCANo=" + strCCANo + "&Mode=View");
        }

        protected void GrdCCADetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int intIndex = e.NewEditIndex;
            string strCCANo = ((LinkButton)GrdCCADetails.Rows[intIndex].FindControl("lnkCCANo")).Text;
            string strAWBPre = ((Label)GrdCCADetails.Rows[intIndex].FindControl("lblAWBPrefix")).Text;
            string strAWBNo = ((Label)GrdCCADetails.Rows[intIndex].FindControl("lblAWBNumber")).Text;
            Response.Redirect("frmCCAProcessing.aspx?AWBNo=" +strAWBPre+ strAWBNo + "&CCANo=" + strCCANo + "&Mode=Edit");

        }

    }
}
