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
    public partial class ListDCMAWBDeals : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());
        DCMGenerateBAL DCM = new DCMGenerateBAL();
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
                txtDCMFrom.Text = Session["IT"] != null ? ((DateTime)Session["IT"]).ToString("dd-MM-yyyy") : string.Empty;
                txtDCMTo.Text = Session["IT"] != null ? ((DateTime)Session["IT"]).ToString("dd-MM-yyyy") : string.Empty;



            }

        }

        public void GetDCMDetails()
        {
            try
            {
                DataSet ds;
                int i = 0;
                object[] objDCM = new object[7];
                objDCM.SetValue(txtDCM.Text, i);
                objDCM.SetValue(txtPreAWB.Text.Trim() + txtAWB.Text.Trim(), ++i);
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
                if (rbDCMAWB.Checked)
                    ds = DCM.ListDCM(objDCM);
                else
                    ds = DCM.ListDCMDeals(objDCM);

                if (ds != null)
                {

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Session["dsDCM"] = ds;
                            GrdDCMDetails.Visible = true;
                            lblStatus.Visible = false;
                            //btnPrintDCM.Visible = true;
                            btnPrintAWBDCM.Visible = true;
                            btnExportToERP.Visible = true;
                            GrdDCMDetails.DataSource = ds.Tables[0];
                            GrdDCMDetails.DataBind();
                            //Hide blank columns AWB #, FlightNo, AgentCode from grid if DCM against Deals
                            if (rbDCMDeals.Checked)
                            {
                                GrdDCMDetails.Columns[2].Visible = false;
                                GrdDCMDetails.Columns[3].Visible = false;
                                GrdDCMDetails.Columns[5].Visible = false;
                            }
                        }
                        else
                        {
                            GrdDCMDetails.Visible = false;
                            lblStatus.Visible = true;
                            btnPrintAWBDCM.Visible = false;
                            btnExportToERP.Visible = false;
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
            txtAgent.Text = txtDCMFrom.Text = txtDCMTo.Text = txtAWB.Text = txtDCM.Text = txtInvoiceNo.Text = "";
            lblStatus.Text = "";
        }

        protected void btnPrintAWBDCM_Click(object sender, EventArgs e)
        {

            if (rbDCMAWB.Checked)
            {
                DCMPerAWBPrint();
            }
            else
            {
                DCMDealPrint();
            }
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

                        RateCardInfo.SetValue(dsDCM.Tables[0].Rows[j + (GrdDCMDetails.PageIndex * GrdDCMDetails.PageSize)]["AWB"].ToString() + "||" + dsDCM.Tables[0].Rows[j + (GrdDCMDetails.PageIndex * GrdDCMDetails.PageSize)]["FlightNumber"].ToString() + "|" + dsDCM.Tables[0].Rows[j + (GrdDCMDetails.PageIndex * GrdDCMDetails.PageSize)]["FlightDate"].ToString(), irow);

                        #endregion Prepare Parameters

                        if (AWBList == "")
                        {
                            AWBList = AWBList + dsDCM.Tables[0].Rows[j + (GrdDCMDetails.PageIndex * GrdDCMDetails.PageSize)]["AWB"].ToString() + "||" + dsDCM.Tables[0].Rows[j + (GrdDCMDetails.PageIndex * GrdDCMDetails.PageSize)]["FlightNumber"].ToString() + "|" + dsDCM.Tables[0].Rows[j + (GrdDCMDetails.PageIndex * GrdDCMDetails.PageSize)]["FlightDate"].ToString();
                        }
                        else
                        {
                            AWBList = AWBList + "," + dsDCM.Tables[0].Rows[j + (GrdDCMDetails.PageIndex * GrdDCMDetails.PageSize)]["AWB"].ToString() + "||" + dsDCM.Tables[0].Rows[j + (GrdDCMDetails.PageIndex * GrdDCMDetails.PageSize)]["FlightNumber"].ToString() + "|" + dsDCM.Tables[0].Rows[j + (GrdDCMDetails.PageIndex * GrdDCMDetails.PageSize)]["FlightDate"].ToString();
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
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "printDCMperAWBList();", true);

                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void rbDCMAWB_CheckedChanged(object sender, EventArgs e)
        {
            txtAWB.Text = "";
            txtAgent.Text = "";
            txtAWB.Enabled = true;
            txtAgent.Enabled = true;
        }

        protected void rbDCMDeals_CheckedChanged(object sender, EventArgs e)
        {
            txtAWB.Text = "";
            txtAgent.Text = "";
            txtAWB.Enabled = false;
            txtAgent.Enabled = false;
        }

        protected void GrdDCMDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dst = (DataSet)Session["dsDCM"];
            GrdDCMDetails.PageIndex = e.NewPageIndex;
            GrdDCMDetails.DataSource = dst.Tables[0];
            GrdDCMDetails.DataBind();
        }

        protected void DCMDealPrint()
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

                        RateCardInfo.SetValue(dsDCM.Tables[0].Rows[j + (GrdDCMDetails.PageIndex * GrdDCMDetails.PageSize)]["DCMNumber"].ToString(), irow);

                        #endregion Prepare Parameters

                        if (AWBList == "")
                        {
                            AWBList = AWBList + dsDCM.Tables[0].Rows[j + (GrdDCMDetails.PageIndex * GrdDCMDetails.PageSize)]["DCMNumber"].ToString();
                        }
                        else
                        {
                            AWBList = AWBList + "," + dsDCM.Tables[0].Rows[j + (GrdDCMDetails.PageIndex * GrdDCMDetails.PageSize)]["DCMNumber"].ToString();
                        }

                        //GenerateAgentInvoice(RateCardInfo);

                    }
                }

                if (AWBList.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select Records to print";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
                else
                {
                    lblStatus.Text = "";
                    hfAWBNo.Value = AWBList;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "printDCMDealsList();", true);

                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Button Export To ERP
        protected void btnExportToERP_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = string.Empty;
                DataSet dsDCM = (DataSet)Session["dsDCM"];
                string AWBList = "";
                for (int j = 0; j < GrdDCMDetails.Rows.Count; j++)
                {
                    if (((CheckBox)GrdDCMDetails.Rows[j].FindControl("ChkSelect")).Checked)
                    {

                        #region Prepare Parameters
                        object[] RateCardInfo = new object[1];

                        #endregion Prepare Parameters

                        if (AWBList == "")
                        {
                            AWBList = AWBList + dsDCM.Tables[0].Rows[j + (GrdDCMDetails.PageIndex * GrdDCMDetails.PageSize)]["AWB"].ToString();
                        }
                        else
                        {
                            AWBList = AWBList + "," + dsDCM.Tables[0].Rows[j + (GrdDCMDetails.PageIndex * GrdDCMDetails.PageSize)]["AWB"].ToString();
                        }
                        //GenerateAgentInvoice(RateCardInfo);

                    }
                }
                string strDCMNumber = "";
                for (int j = 0; j < GrdDCMDetails.Rows.Count; j++)
                {
                    if (((CheckBox)GrdDCMDetails.Rows[j].FindControl("ChkSelect")).Checked)
                    {

                        #region Prepare Parameters
                        object[] RateCardInfo = new object[1];

                        #endregion Prepare Parameters

                        if (strDCMNumber == "")
                        {
                            strDCMNumber = strDCMNumber + dsDCM.Tables[0].Rows[j + (GrdDCMDetails.PageIndex * GrdDCMDetails.PageSize)]["DCMNumber"].ToString();
                        }
                        else
                        {
                            strDCMNumber = strDCMNumber + "," + dsDCM.Tables[0].Rows[j + (GrdDCMDetails.PageIndex * GrdDCMDetails.PageSize)]["DCMNumber"].ToString();
                        }
                        //GenerateAgentInvoice(RateCardInfo);

                    }
                }
                //if (AWBList.Trim() == "")
                //{
                //    lblStatus.Visible = true;
                //    lblStatus.Text = "Please select Records to Export!";
                //    lblStatus.ForeColor = Color.Blue;
                //    return;
                //}
                //else
                //{
                DateTime dtFrm = new DateTime();
                DateTime dtTo = new DateTime();
                string[] paramname = new string[3];
                paramname[0] = "DCMNumber";
                paramname[1] = "FromDate";
                paramname[2] = "ToDate";
                if (strDCMNumber.Trim() == "")
                {
                    lblStatus.Text = "";
                    hfAWBNo.Value = AWBList;
                    if (txtDCMFrom.Text == "")
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = "Please select Valid date";
                        lblStatus.ForeColor = Color.Blue;
                        return;
                    }
                    try
                    {
                        //dt = Convert.ToDateTime(txtInvoiceFrom.Text);
                        //Change 03082012
                        string day = txtDCMFrom.Text.Substring(0, 2);
                        string mon = txtDCMFrom.Text.Substring(3, 2);
                        string yr = txtDCMFrom.Text.Substring(6, 4);
                        strfromdate = yr + "-" + mon + "-" + day;
                        dtFrm = Convert.ToDateTime(strfromdate);
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
                    try
                    {
                        //dtto = Convert.ToDateTime(txtInvoiceTo.Text);
                        //Change 03082012
                        string day = txtDCMTo.Text.Substring(0, 2);
                        string mon = txtDCMTo.Text.Substring(3, 2);
                        string yr = txtDCMTo.Text.Substring(6, 4);
                        strtodate = yr + "-" + mon + "-" + day;
                        dtTo = Convert.ToDateTime(strtodate);
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = "Selected Date format invalid";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    if (dtTo < dtFrm)
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = "To date should be greater than From date";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                }
                object[] paramvalue = new object[3];
                paramvalue[0] = strDCMNumber;
                paramvalue[1] = (strDCMNumber == "") ? dtFrm.ToString("yyyy/MM/dd") : "";
                paramvalue[2] = (strDCMNumber == "") ? dtTo.ToString("yyyy/MM/dd") : "";
                SqlDbType[] paramtype = new SqlDbType[3];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                DataSet ds = DCM.GetExportERPData(paramname, paramvalue, paramtype);
                if (ds != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)ds.Tables[0];
                    //dt = city.GetAllCity();//your datatable 
                    string attachment = "attachment; filename=ExportERPDCM.xls";
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", attachment);
                    Response.ContentType = "application/vnd.ms-excel";
                    string tab = "";
                    foreach (DataColumn dc in dt.Columns)
                    {
                        Response.Write(tab + dc.ColumnName);
                        tab = "\t";
                    }
                    Response.Write("\n");
                    int i;
                    foreach (DataRow dr in dt.Rows)
                    {
                        tab = "";
                        for (i = 0; i < dt.Columns.Count; i++)
                        {
                            Response.Write(tab + dr[i].ToString());
                            tab = "\t";
                        }
                        Response.Write("\n");
                    }
                    Response.End();
                }

                //}


            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        protected void GrdDCMDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int index = e.NewEditIndex;
            string strDCMNumber = ((Label)GrdDCMDetails.Rows[index].FindControl("lblDCMNo")).Text;
            string strAWBPre = ((Label)GrdDCMDetails.Rows[index].FindControl("lblAWBPrefix")).Text;
            string strAWBNumber = ((Label)GrdDCMDetails.Rows[index].FindControl("lblAWBNumber")).Text;
            Response.Redirect("DCMGenerate.aspx?Mode=Edit&DCMNumber=" + strDCMNumber + "&AWBPre=" + strAWBPre + "&AWBNumber=" + strAWBNumber);
        }

        protected void GrdDCMDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = GrdDCMDetails.SelectedIndex;
            string strDCMNumber = ((Label)GrdDCMDetails.Rows[index].FindControl("lblDCMNo")).Text;
            string strAWBPre = ((Label)GrdDCMDetails.Rows[index].FindControl("lblAWBPrefix")).Text;
            string strAWBNumber = ((Label)GrdDCMDetails.Rows[index].FindControl("lblAWBNumber")).Text;
            Response.Redirect("DCMGenerate.aspx?Mode=View&DCMNumber=" + strDCMNumber + "&AWBPre=" + strAWBPre + "&AWBNumber=" + strAWBNumber);
            
        }
    }

}
