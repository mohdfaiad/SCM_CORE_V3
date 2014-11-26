using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using BAL;
using System.Globalization;
using QID.DataAccess;
using System.IO;
//using Office = Microsoft.Office.Core;


/*

  2012-07-04 vinayak
  2012-07-06  vinayak Edit/View

*/

namespace ProjectSmartCargoManager
{
    public partial class ListBooking_GHA : System.Web.UI.Page
    {
        ListBookingBAL objBAL = new ListBookingBAL();
        string errormessage = "";
        #region Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                Session["dsListBooking"] = null;
                Session["BookingURL"] = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "BookingURL");
                
                txtAWBFromDt.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                txtAWBToDt.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                LoadStationDropDowns();
                CHKfailed.Visible = false;

                if (Session["ACode"] != null && Convert.ToString(Session["ACode"]) != "")
                {
                    TXTAgentCode.Text = Convert.ToString(Session["ACode"]);
                    TXTAgentCode.Enabled = false;
                }

                if (Session["AirlinePrefix"] != null)
                   txtFlightCode.Text = Session["AirlinePrefix"].ToString();
                else
                {
                    MasterBAL objBal = new MasterBAL();
                    Session["AirlinePrefix"] = objBal.AirlinePrefix();
                    txtFlightCode.Text = Session["AirlinePrefix"].ToString();
                }
                if (Session["awbPrefix"] != null)
                    txtAWBPrefix.Text = Session["awbPrefix"].ToString();
                else
                {
                    MasterBAL objBal = new MasterBAL();
                    Session["awbPrefix"] = objBal.awbPrefix();
                    txtAWBPrefix.Text = Session["awbPrefix"].ToString();
                }

                #region Define PageSize for grid as per configuration
                try
                {
                    GRDBooking.PageSize = Convert.ToInt32(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "StdListPageSize"));
                }
                catch (Exception ex)
                { }
                #endregion
            }

            bool blnFlag = false;
            DataTable dt = new DataTable("ListBooking_dt1");
            dt = (DataTable)Session["StatusMaster"];
            try
            {
                if (dt != null)
                {
                    for (int intCount = 0; intCount < dt.Rows.Count; intCount++)
                    {
                        for (int i = 0; i < DDLStatus.Items.Count; i++)
                        {
                            if (dt.Rows[intCount]["Status"].ToString().Trim() == DDLStatus.Items[i].Text.Trim())
                            {
                                blnFlag = true;
                                break;
                            }
                        }
                        if (blnFlag)
                            break;
                    }

                    if (blnFlag == false)
                    {
                        for (int intCount = 0; intCount < dt.Rows.Count; intCount++)
                        {
                            ListItem item = new ListItem();
                            item.Value = dt.Rows[intCount]["StatusCode"].ToString();
                            item.Text = dt.Rows[intCount]["Status"].ToString();
                            DDLStatus.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex) { }
            finally 
            {
                if (dt != null)
                    dt.Dispose();
            }
        }
      

        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                // txtAWBNo.Focus();
                LBLNoOfRecords.Text = "";
                lblTotal.Text = "";
                Session["dsListBooking"] = null;

                if (!IsValide())
                    return;

                DataSet dsResult = new DataSet("ListBooking_dsResult");

                string source, dest, flight, fromdate, awbnumber, awbprefix, todate, status, CommodityCode, ExecutedBy;
                source = dest = flight = fromdate = todate = awbnumber = "";

                string IsFFRflag = "";

                source = ddlSource.SelectedItem.Text.Trim() == "All" ? "" : ddlSource.SelectedItem.Text.Trim();
                dest = ddlDest.SelectedItem.Text.Trim() == "All" ? "" : ddlDest.SelectedItem.Text.Trim();
                //flight=ddlFlight.SelectedItem.Text.Trim()=="All"?"":ddlFlight.SelectedItem.Text.Trim();
                if (txtFlightCode.Text.Trim() != "" && txtFlightID.Text.Trim() != "")
                    flight = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();

                awbnumber = txtAWBNo.Text.Trim();
                ExecutedBy = txtUsers.Text.Trim(); //drpUsers.SelectedItem.Text.Trim() == "All" ? "" : drpUsers.SelectedItem.Text.Trim();

                string AWBFromdt = txtAWBFromDt.Text.Trim();
                string AWBTodt = txtAWBToDt.Text.Trim();

                string FltFromdt = txtFltFromDt.Text.Trim();
                string FltTodt = txtFltToDt.Text.Trim();

                status = DDLStatus.SelectedItem.Value;
                awbprefix = txtAWBPrefix.Text.Trim();


                if (CHKinFFR.Checked == true)
                {
                    IsFFRflag = "True";
                }

                else
                {
                    IsFFRflag = "";
                }

                string AgentCode = TXTAgentCode.Text.Trim();

                CommodityCode = txtCommodityCode.Text.Trim();

                bool ViaTemplate = false;
                if (chkViaTemplate.Checked)
                {
                    ViaTemplate = true;
                }

                if (objBAL.GetAllAWBs(source, dest, flight, FltFromdt, FltTodt, AWBFromdt, AWBTodt, status, awbprefix, awbnumber, IsFFRflag, ref dsResult, ref errormessage, AgentCode, CommodityCode, ExecutedBy, ViaTemplate, txtshiper.Text.Trim()))
                {
                    GRDBooking.DataSource = dsResult.Copy();
                    GRDBooking.DataBind();
                    Session["dsListBooking"] = dsResult.Copy();
                    //26Feb2013
                    try
                    {
                        int pcs = 0;
                        float grossWgt = 0;
                        float chWgt = 0;
                        try
                        {
                            pcs = Convert.ToInt32(dsResult.Tables[0].Compute("Sum(PiecesCount)", ""));
                            grossWgt = float.Parse(dsResult.Tables[0].Compute("Sum(GrossWeight)", "").ToString());
                            chWgt = float.Parse(dsResult.Tables[0].Compute("Sum(ChargedWeight)", "").ToString());
                        }
                        catch (Exception)
                        {
                        }

                        lblTotal.Text = "       Total Pieces: " + pcs.ToString() + "        Total Gross Wgt: " + grossWgt.ToString() + "        Total Chargable Wgt: " + chWgt.ToString();
                        LBLNoOfRecords.Text = "No. Of Records:  " + dsResult.Tables[0].Rows.Count;

                    }
                    catch (Exception ex)
                    {
                    }
                }
                if (dsResult != null)
                    dsResult.Dispose();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error :" + ex.Message;
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
        }

        protected void GRDBooking_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "Edit" || e.CommandName == "View")
            {
                string preAWB=string.Empty;  
                if (Session["awbPrefix"] != null)
                {
                     preAWB= Session["awbPrefix"].ToString();
                }
                else
                {
                    MasterBAL objBal = new MasterBAL();
                    Session["awbPrefix"] = objBal.awbPrefix();
                    preAWB= Session["awbPrefix"].ToString();
                }
                //Response.Redirect("~//GHA_ConBooking.aspx?command=" + e.CommandName + "&AWBNumber=" +GRDBooking.Rows[int.Parse(e.CommandArgument.ToString())].Cells[1].Text.Trim());
                string BookingURL = Convert.ToString(Session["BookingURL"]);
                if (BookingURL != "")
                {
                    Response.Redirect("~//"+BookingURL+"?command=" + e.CommandName + "&AWBNumber=" + ((HyperLink)GRDBooking.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lnkAWBNumber")).Text.Trim());

                }
                else
                {
                    Response.Redirect("~//GHA_ConBooking.aspx?command=" + e.CommandName + "&AWBNumber=" + ((HyperLink)GRDBooking.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lnkAWBNumber")).Text.Trim()); 
                }
            }
        }

        protected void GRDBooking_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dsResult = new DataSet("ListBooking_dsResult1");
            dsResult = (DataSet)Session["dsListBooking"];

            GRDBooking.PageIndex = e.NewPageIndex;
            GRDBooking.DataSource = dsResult.Copy();
            GRDBooking.DataBind();

            if(dsResult != null)
                dsResult.Dispose();
        }

        #endregion

        #region UserDefined

        public void LoadStationDropDowns()
        {
            //LoginBL objBal = new LoginBL();
            HomeBL objBLL = new HomeBL();
            string restStation = "false";
            DataSet dsResult = new DataSet("ListBooking_dsResult_LoadStations");
            restStation = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "RestrictStationAccess");
            //restStation = objBal.GetMasterConfiguration("RestrictStationAccess");
            try
            {
            if (restStation == "true" && restStation != null&&restStation!="")
            { 
                object[] UserParams = new string[1];
                int i = 0;
                UserParams.SetValue(Session["UserName"].ToString(), i);

                //dsResult = new DataSet("ListBooking_dsResult_LoadStations");
                if (objBAL.GetAllStaions(ref dsResult, ref errormessage))
                {

                    DataRow row = dsResult.Tables[0].NewRow();
                    row["Code"] = "All";
                    dsResult.Tables[0].Rows.Add(row);
                    ddlDest.DataSource = dsResult.Tables[0].Copy();
                    ddlDest.DataTextField = "Code";
                    ddlDest.DataValueField = "Code";
                    ddlDest.DataBind();
                    //ddlDest.SelectedValue = "All";

                }


                DataSet ds = new DataSet("MasterPage_getStationLB_ds");
                ds = objBLL.GetUserRollDetails(UserParams);
                if (ddlSource.DataSource == null||ddlSource.DataSource=="")
                {
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[2].NewRow();
                        row["StationCode"] = "All";
                        ds.Tables[2].Rows.Add(row);
                        ddlSource.DataSource = null;
                        ddlSource.DataSource = ds;
                        ddlSource.DataMember = ds.Tables[2].TableName;
                        ddlSource.DataValueField = ds.Tables[2].Columns[0].ColumnName;
                        ddlSource.DataTextField = ds.Tables[2].Columns[0].ColumnName;
                        ddlSource.DataBind();
                        ddlSource.Text = Session["Station"].ToString();

                    }
                }
               
                if (Session["RoleName"].ToString() == "Super User")
                {
                    ddlSource.SelectedValue = "All";
                }
                else
                {
                    ddlSource.SelectedValue = Session["Station"].ToString();
                }
                ddlDest.SelectedValue = "All";
                
                dsResult = null;
                ds = null;
            }
            else
            {
              //dsResult = new DataSet("ListBooking_dsResult_LoadStations");
            try
            {
                if (objBAL.GetAllStaions(ref dsResult, ref errormessage))
                {

                    DataRow row = dsResult.Tables[0].NewRow();
                    row["Code"] = "All";
                    dsResult.Tables[0].Rows.Add(row);

                    ddlSource.DataSource = dsResult.Tables[0].Copy();
                    ddlSource.DataTextField = "Code";
                    ddlSource.DataValueField = "Code";
                    ddlSource.DataBind();

                    ddlDest.DataSource = dsResult.Tables[0].Copy();
                    ddlDest.DataTextField = "Code";
                    ddlDest.DataValueField = "Code";
                    ddlDest.DataBind();

                    if (Session["RoleName"].ToString() == "Super User")
                    {
                        ddlSource.SelectedValue = "All";
                    }
                    else
                    {
                        ddlSource.SelectedValue = Session["Station"].ToString();
                    }
                    ddlDest.SelectedValue = "All";

                }
                else
                {
                    lblStatus.Text = "" + errormessage;
                }
            }
            catch(Exception)
            {
            }
            }
            }
         
            catch (Exception)
            {
            }
            
            }

        public bool IsValide()
        {
            lblStatus.Text = "";
            try
            {
                int.Parse(txtAWBNo.Text.Trim() == "" ? "0" : txtAWBNo.Text.Trim());
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: AWBNumber not valid.";
                return false;
            }

            DateTime dtDatetime;
            if (txtFltFromDt.Text.Trim() != "")
            {
                try
                {
                    dtDatetime = DateTime.ParseExact(txtFltFromDt.Text.Trim(), "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Please enter valid Flight From date.";
                    txtFltFromDt.Focus();
                    return false;
                }
            }

            if (txtFltToDt.Text.Trim() != "")
            {
                try
                {
                    dtDatetime = DateTime.ParseExact(txtFltToDt.Text.Trim(), "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Please enter valid Flight To date.";
                    txtFltToDt.Focus();
                    return false;
                }
            }

            if (txtAWBFromDt.Text.Trim() != "")
            {
                try
                {
                    dtDatetime = DateTime.ParseExact(txtAWBFromDt.Text.Trim(), "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Please enter valid AWB From date.";
                    txtAWBFromDt.Focus();
                    return false;
                }
            }

            if (txtAWBToDt.Text.Trim() != "")
            {
                try
                {
                    dtDatetime = DateTime.ParseExact(txtAWBToDt.Text.Trim(), "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Please enter valid AWB To date.";
                    txtAWBToDt.Focus();
                    return false;
                }
            }

            if (txtFltFromDt.Text.Trim() != "" && txtFltToDt.Text.Trim() != "")
            {
                if (DateTime.ParseExact(txtFltFromDt.Text.Trim(), "dd/MM/yyyy", null) > DateTime.ParseExact(txtFltToDt.Text.Trim(), "dd/MM/yyyy", null))
                {
                    lblStatus.Text = "Flight From date should be smaller than Flight to date.";
                    return false;
                }
            }

            if (txtAWBFromDt.Text.Trim() != "" && txtAWBToDt.Text.Trim() != "")
            {
                if (DateTime.ParseExact(txtAWBFromDt.Text.Trim(), "dd/MM/yyyy", null) > DateTime.ParseExact(txtAWBToDt.Text.Trim(), "dd/MM/yyyy", null))
                {
                    lblStatus.Text = "AWB From date should be smaller than AWB to date.";
                    return false;
                }
            }

            return true;
        }

        #endregion

        protected void CHKinFFR_CheckedChanged(object sender, EventArgs e)
        {

            if (CHKinFFR.Checked == true)
            {
                CHKfailed.Visible = false;
            }
            else
            {
                CHKfailed.Visible = false;
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet("ListBooking_ds_btnPrint");
            DataTable dt = new DataTable("ListBooking_dt_btnPrint");

            try
            {
                if ((DataSet)Session["dsListBooking"] == null)
                    return;
                
                ds = (DataSet)Session["dsListBooking"];
                dt = (DataTable)ds.Tables[0];
                //dt = city.GetAllCity();//your datatable 
                string attachment = "attachment; filename=ListBooking.xls";
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
                //Response.Flush();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
                if (dt != null)
                    dt.Dispose();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/GHA_ListBooking.aspx",false);
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetCommodityCodesWithName(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string"); 
            SqlDataAdapter dad = null;
            DataSet ds = new DataSet();
            dad = new SqlDataAdapter("SELECT CommodityCode + '(' + Description + ')' from CommodityMaster where (Description like '%" + prefixText + "%' or CommodityCode like '%" + prefixText + "%')", con);
            dad.Fill(ds);
            List<string> list = new List<string>(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(dr[0].ToString());
            }
            dad.Dispose();
            if (ds != null)
            {
                ds.Dispose();
            }
            
            
            return list.ToArray();
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetAgentCodeWithName(string prefixText, int count)
        {
            try
            {
                string[] orgdest = new ListBooking_GHA().GetOrgDest();

                string con = Global.GetConnectionString();
                //// SqlConnection con = new SqlConnection("connection string"); 
                SqlDataAdapter dad = new SqlDataAdapter("SELECT AgentCode + '(' + AgentName + ')' from dbo.AgentMaster where (AgentName like '%" + prefixText + "%' or AgentCode like '%" + prefixText + "%') and Station='" + orgdest[0] + "'", con);
                DataSet ds = new DataSet();
                dad.Fill(ds);

                //DataSet ds = new DataSet();
                //AgentBAL bal = new AgentBAL();
                //ds = bal.GetAgentList(prefixText, orgdest[0], System.DateTime.Now.ToString("dd/MM/yyyy"));
                List<string> list = new List<string>(ds.Tables[0].Rows.Count);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(dr[0].ToString());

                }

                if (ds != null)
                    ds.Dispose();
                //bal = null;
                return list.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetShipperCodeWithName(string prefixText, int count)
        {
            try
            {
                string[] orgdest = new ListBooking_GHA().GetOrgDest();

                string con = Global.GetConnectionString();

                SqlDataAdapter dad = new SqlDataAdapter("SELECT AccountCode + '(' + AccountName + ')' from dbo.AccountMaster where (AccountName like '%" + prefixText + "%' or AccountCode like '%" + prefixText + "%') ", con);
                DataSet ds = new DataSet();
                dad.Fill(ds);
                
                List<string> list = new List<string>(ds.Tables[0].Rows.Count);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(dr[0].ToString());

                }

                if (ds != null)
                    ds.Dispose();
                //bal = null;
                return list.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetUserName(string prefixText, int count)
        {
            try
            {
                string[] orgdest = new ListBooking_GHA().GetOrgDest();

                string con = Global.GetConnectionString();
                //// SqlConnection con = new SqlConnection("connection string"); 
                SqlDataAdapter dad = new SqlDataAdapter("SELECT LoginName from UserMasterNew where LoginName like '%" + prefixText + "%'", con);
                DataSet ds = new DataSet();
                dad.Fill(ds);

                List<string> list = new List<string>(ds.Tables[0].Rows.Count);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(dr[0].ToString());
                }

                if (ds != null)
                    ds.Dispose();
                //bal = null;
                return list.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string[] GetOrgDest()
        {
            string[] result = { "", "" };
            if (Session["Station"] != null)
            {
                result[0] = Session["Station"].ToString();
            }
            if (Session["Destination"] != null)
            {
                result[1] = Session["Destination"].ToString();
            }

            return result;
        }

        protected void txtAWBNo_TextChanged(object sender, EventArgs e)
        {
            txtAWBNo.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            //btnList_Click(null, null);
        }

    }
}
