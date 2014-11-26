using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;  
using System.Drawing;  
using BAL;
using QID.DataAccess;
//7-9-2012

namespace ProjectSmartCargoManager
{
    public partial class ListSpotRateApproval : System.Web.UI.Page
    {
        ListBookingBAL objBAL = new ListBookingBAL();
        BALSpotRate BLSRate = new BALSpotRate();
        SQLServer da = new SQLServer(Global.GetConnectionString());
        string errormessage = "";

        bool IsSelected; bool ds;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if(Request.QueryString["id"]=="0")
                {

                    btnApprove.Visible = true;
                    btnClose.Visible = true;
                    btnReject.Visible = true;
                    lblHeader.Text = "Spot Rate Approval/Rejection"; 
                
                }
                else if (Request.QueryString["id"] == "1")
                {
                    btnApprove.Visible = false ;
                    btnClose.Visible = false ;
                    btnReject.Visible = false;
                    lblHeader.Text = "Spot Rate Listing";
                }
                LoadStationDropDowns();
                getflightnumber();

                AddEmptyRow();

                txtFromdate.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                txtTodate.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");

                if (Session["awbPrefix"] != null)
                    txtAWBPrefix.Text = Convert.ToString(Session["awbPrefix"]);

                #region Define PageSize for grid as per configuration
                try
                {
                    LoginBL objConfig = new LoginBL();
                    grdCreditdetails.PageSize = Convert.ToInt32(objConfig.GetMasterConfiguration("StdListPageSize"));
                    objConfig = null;
                }
                catch (Exception ex)
                { }
                #endregion
            }

        }
        //Get Flight Details  from tables
        #region GetFlightDropdown
        protected void getflightnumber()
        {
            try
            {
                DataSet ds = da.SelectRecords("SP_GetFlightID");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlFlight.Items.Clear();
                            
                            ddlFlight.DataSource = ds.Tables[0];
                            ddlFlight.DataTextField = "FlightID";
                            ddlFlight.DataValueField = "FlightID";
                            ddlFlight.DataBind();
                            //ddlFlight.Items.Add("Select");
                            ddlFlight.Items.Insert(0, new ListItem("All", ""));
                            ddlFlight.SelectedIndex = -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion GetFlightDropdown
        #region GetFlights
        public void GetFlights()
        {
            try
            {
                string source, dest, date;
                source = dest = date = "";

                if (ddlSource.SelectedItem.Text.Trim() == "All" || ddlDest.SelectedItem.Text.Trim() == "All")
                {
                    ddlFlight.Items.Clear();
                    ddlFlight.Items.Add(new ListItem("All", "All"));
                    ddlFlight.SelectedIndex = 0;
                }
                else
                {
                    source = ddlSource.SelectedItem.Text;
                    dest = ddlDest.SelectedItem.Text;
                    date = "";// txtDate.Text;


                    DataSet dsResult = new DataSet();

                    //if (objBAL.GetAllFlights(source, dest, date, ref dsResult, ref errormessage))
                    //{

                    //    DataRow row = dsResult.Tables[0].NewRow();
                    //    row["FltNumber"] = "All";
                    //    dsResult.Tables[0].Rows.Add(row);

                    //    ddlFlight.DataSource = dsResult.Tables[0].Copy();
                    //    ddlFlight.DataTextField = "FltNumber";
                    //    ddlFlight.DataValueField = "FltNumber";
                    //    ddlFlight.DataBind();
                    //}
                    //else
                    //{
                    //    lblStatus.Text = "" + errormessage;
                    //}
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = "" + ex.Message;
            }

        }

#endregion GetFlights
        //Load stations in a dropdown 
        #region LoadStation
        public void LoadStationDropDowns()
        {
            DataSet dsResult = new DataSet();

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


                ddlSource.SelectedItem.Text = "All";
                ddlDest.SelectedItem.Text = "All";

                ddlFlight.Items.Clear();
                ddlFlight.Items.Add(new ListItem("All", "All"));

            }
            else
            {
                lblStatus.Text = "" + errormessage;
            }

        }
        #endregion LoadStation
        //commentEd
        #region Source
        protected void ddlSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //GetFlights();
            }
            catch (Exception ex)
            { }
        }
        #endregion Source
        //commented
        #region Destinatio
        protected void ddlDest_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //GetFlights();
            }
            catch (Exception ex)
            { }
        }
        #endregion Destination
        //Geting AgentCodes
        #region AgentCode
        protected void txtAgentCode_TextChanged(object sender, EventArgs e)
        {
            try
            {

                string[] orgdest = new ConBooking_GHA().GetOrgDest();

                string con = Global.GetConnectionString();
                // SqlConnection con = new SqlConnection("connection string");
                SqlDataAdapter dad = new SqlDataAdapter("SELECT AgentName from dbo.AgentMaster where AgentCode ='" +txtAgentCode.Text.Trim() + "'", con);
                DataSet ds = new DataSet();
                dad.Fill(ds);

                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                {
                    //txtAgentCode.Text = ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Agent code invalid.";
                    return;
                }


            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error :" + ex.Message;
            }
        }
        #endregion AgentCode
        //
        #region GetAgent
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetAgentCode(string prefixText, int count)
        {

            string[] orgdest = new ConBooking_GHA().GetOrgDest();

            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string");
            SqlDataAdapter dad = new SqlDataAdapter("SELECT AgentCode from dbo.AgentMaster where (AgentName like '" + prefixText + "%' or AgentCode like '" + prefixText + "%') and Station='" + orgdest[0] + "'", con);
            DataSet ds = new DataSet();
            dad.Fill(ds);
            List<string> list = new List<string>(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(dr[0].ToString());

            }

            return list.ToArray();
        }
        #endregion GetAgent
        //Geting data listing according to paramter
        #region List
        protected void btnList_Click(object sender, EventArgs e)
        {
            string source = "", Destination = "", FlightNo = "",AgentCode="";
            string FlghtDate = "", AWBNumber=""; 
            string SpotRateID="" ;int ID=0;
            LBLNoOfRecords.Visible = true;
            lblStatus.Visible = true;

            try
            {
                lblStatus.Text = "";
                LBLNoOfRecords.Text = "";

                if (ddlSource.SelectedItem.Text == "All")
                    source = "All";
                else
                    source = ddlSource.SelectedItem.Text;
                if (ddlDest.SelectedItem.Text == "All")
                    Destination = "All";
                else
                Destination = ddlDest.SelectedItem.Text;
                if (ddlFlight.SelectedItem.Text == "All")
                    FlightNo = "";
                else
                    FlightNo = ddlFlight.SelectedItem.Text;    
   
                if (txtAgentCode.Text == "")
                    AgentCode = "";
                else
                    AgentCode = txtAgentCode.Text;
                if (txtFlightDate.Text == "")
                  SpotRateID="";
                else
                    FlghtDate = txtFlightDate.Text;

                if(txtSpotRateId.Text=="")
                    SpotRateID="";
                else
                    SpotRateID=txtSpotRateId.Text;

                if(Request.QueryString["id"]=="0")
                {
                    ID=0;   
                
                }
                else if (Request.QueryString["id"] == "1")
                {
                   ID=1;  
                }

                if(txtawbno.Text.Trim() != "")
                    AWBNumber = txtAWBPrefix.Text.Trim() + txtawbno.Text.Trim();

                DataSet ds1;
                ds1 = BLSRate.GetSpotRateDetails(source, Destination, FlightNo, AgentCode, FlghtDate, SpotRateID, ID, AWBNumber,txtFromdate.Text.Trim(),
                    txtTodate.Text.Trim(),ddlStatus.SelectedValue);

                if (ds1 != null)
                {
                    if (ds1.Tables.Count > 0)
                    {
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            grdCreditdetails.Visible = true;
                            grdCreditdetails.DataSource = ds1.Tables[0];
                            grdCreditdetails.DataBind();
                            Session["dsSpotRateDetails"] = ds1.Copy();
                            LBLNoOfRecords.Text = "No. Of Records :" + ds1.Tables[0].Rows.Count;

                            #region Enable/Disable Code

                            for (int i = 0; i < grdCreditdetails.Rows.Count; i++)
                            {
                                string approval = ((Label)grdCreditdetails.Rows[i].FindControl("lblAproval")).Text;
                               // string awbnumber = ((Label)grdCreditdetails.Rows[i].FindControl("lblAwbno")).Text;

                                if (approval.Equals("Approved", StringComparison.OrdinalIgnoreCase))
                                {
                                    ((TextBox)grdCreditdetails.Rows[i].FindControl("txtSpotRate")).Enabled=false;                                    
                                }
                                if (approval.Equals("Rejected", StringComparison.OrdinalIgnoreCase))
                                {
                                    ((TextBox)grdCreditdetails.Rows[i].FindControl("txtSpotRate")).Enabled = false;
                                    ((CheckBox)grdCreditdetails.Rows[i].FindControl("check")).Enabled = false;
                                }
                            }
                           

                            #endregion
                        }
                        else
                        {
                            lblStatus.Text = "No Records Found";
                            lblStatus.ForeColor = Color.Red;

                            grdCreditdetails.Visible = false;
                            //LBLNoOfRecords.Text = "No Records Found";
                            //LBLNoOfRecords.ForeColor = Color.Red;

                            //grdCreditdetails.DataSource = null;
                            //grdCreditdetails.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error Message :" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
            
        }
        #endregion List
        //Approve spot Rate.
        #region Approve
        protected void btnApprove_Click(object sender, EventArgs e)
        {
           
            try
            {
                string Approve = "";
                
                bool IsSelected = false;
                for (int i = 0; i < grdCreditdetails.Rows.Count; i++)
                {
                    if (((CheckBox)grdCreditdetails.Rows[i].FindControl("check")).Checked == true)
                    {
                        IsSelected = true;

                    }
                }
                if (IsSelected)
                {
                    for (int i=0; i<grdCreditdetails.Rows.Count ; i++)
                    {
                        if (((CheckBox)grdCreditdetails.Rows[i].FindControl("check")).Checked == true)
                        {
                            IsSelected = true;

                            string AWBNumber = ((Label)grdCreditdetails.Rows[i].FindControl("lblAwbno")).Text;
                            string spotrate = ((TextBox)grdCreditdetails.Rows[i].FindControl("txtSpotRate")).Text;
                            string UserName = Session["UserName"].ToString();
                            DateTime UpdatedOn = Convert.ToDateTime(Session["IT"]);
                            string spotID = ((Label)(grdCreditdetails.Rows[i].FindControl("lblSpotID"))).Text.Trim();

                            ds = BLSRate.UpdateStatusApproval(AWBNumber, spotrate, UserName, UpdatedOn, "Approved",spotID);

                            if (ds)
                            {
                                #region For Master Audit Log
                                MasterAuditBAL ObjMAL = new MasterAuditBAL();
                                #region Prepare Parameters
                                object[] Params = new object[7];
                                int j = 0;

                                //1
                                Params.SetValue("Spot Rate Approveal" ,j);
                                j++;

                                //2
                                Params.SetValue(AWBNumber, j);
                                j++;

                                //3

                                Params.SetValue("APPROVED", j);
                                j++;

                                //4

                                Params.SetValue("", j);
                                j++;

                                //5
                                Params.SetValue("", j);
                                j++;

                                //6

                                Params.SetValue(Session["UserName"], j);
                                j++;

                                //7
                                Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), j);
                                j++;


                                #endregion Prepare Parameters
                                ObjMAL.AddMasterAuditLog(Params);
                                #endregion
                            }

                        }
                        
                    }
                    if (ds == true)
                    {
                        btnList_Click(sender, e);
                        lblStatus.Text = "Spot Rate Approved Successfully";
                        lblStatus.ForeColor = Color.Green;
                        
                        return;
                    }
                }
                if (!IsSelected)
                {
                    lblStatus.Text = "Please select AWB Number(s) for Approval.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
            }
            catch (Exception ex)
            { }
        }
#endregion Approve
        //For grid paging is done for the multiple records
        #region grdCreditdetails_PageIndexChanging
        protected void grdCreditdetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dsResult = (DataSet)Session["dsSpotRateDetails"];

            grdCreditdetails.PageIndex = e.NewPageIndex;
            grdCreditdetails.DataSource = dsResult.Copy();
            grdCreditdetails.DataBind();

        }
#endregion grdCreditdetails_PageIndexChanging

        protected void grdCreditdetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (((System.Web.UI.WebControls.CommandEventArgs)(e)).CommandName.ToUpper() == "PAGE")
                return;

            int index = Convert.ToInt32(e.CommandArgument);

            if (index == -1)
                return;

            // GridViewRow row = grdCreditdetails.Rows[index];
            string AWBNumber = ((LinkButton)(grdCreditdetails.Rows[index].FindControl("lnkAwbno"))).Text.Trim();

            string Appstatus = "";
            if (((Label)(grdCreditdetails.Rows[index].FindControl("lblAproval"))).Text == "New")
                Appstatus = "N";
            else
                Appstatus = "A";
            string spotID = ((Label)(grdCreditdetails.Rows[index].FindControl("lblSpotID"))).Text.Trim();

            Response.Redirect("SpotRateMaster.aspx?cmd=" + e.CommandName + "&AWBNumber=" + AWBNumber + "&AppStat=" + Appstatus+"&SpotID="+spotID);
        }

        #region Clear Button
        protected void btnClear_Click(object sender, EventArgs e)
        {
            //Response.Redirect("~/ListSpotRateApproval.aspx", false);
            txtFlightDate.Text = string.Empty;
            txtAgentCode.Text = string.Empty;
            txtSpotRateId.Text = string.Empty;
            txtawbno.Text = string.Empty;
            //txtFromdate.Text = string.Empty;
            //txtTodaxtTodate.Text = string.Empty;
            LBLNoOfRecords.Visible = false;
            lblStatus.Text = string.Empty;

            //grdCreditdetails.DataSource = null;
            //grdCreditdetails.DataBind();
            AddEmptyRow();
        }
        #endregion

        #region Export Button
        protected void btnExport_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            DataSet dsExp = new DataSet("SpotRate_dsExp");
            DataTable dt = new DataTable("SpotRate_dt");

            try
            {
                if ((DataSet)Session["dsSpotRateDetails"] == null)
                    return;

                dsExp = (DataSet)Session["dsSpotRateDetails"];
                dt = (DataTable)dsExp.Tables[0];

                if (Session["dsSpotRateDetails"] == null && dt == null)
                {
                    lblStatus.Text = "No records found";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                string attachment = "attachment;filename=SpotRatesList.xls";
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

                SaveUserActivityLog("");

                Response.End();
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
                //Response.Redirect("ListSpotRateApproval.aspx", false);


            }
            catch (Exception ex)
            {
                //lblStatus.Text = "Error Message :" + ex.Message;
                //lblStatus.ForeColor = Color.Red;
            }
            finally
            {
                if (dsExp != null)
                    dsExp = null;
                if (dt != null)
                    dt = null;
            }
        }
        #endregion

        #region Add Empty Row to Gridview
        public void AddEmptyRow()
        {
            try
            {
                DataTable dt = new DataTable("SpotR_dt2");
                DataSet ds = new DataSet("SpotRate_Ds1");
                DataColumn dc;

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "AWBNumber";
                dt.Columns.Add(dc);

                //dc = new DataColumn();
                //dc.DataType = Type.GetType("System.String");
                //dc.ColumnName = "AWBNumber";
                //dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "AgentCode";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "AgentName";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "FlightNumber";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "FlightDate";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "Origin";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "Destination";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "SpotRate";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "spotRateCategory";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "Weight";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "Commodity";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "Description";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "Aproval";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "SpotRateID";
                dt.Columns.Add(dc);


                DataRow dr;
                dr = dt.NewRow();
                dr["AWBNumber"] = "";
                //dr["AWBNumber"] = "";
                dr["AgentCode"] = "";
                dr["AgentName"] = "";
                dr["FlightNumber"] = "";
                dr["FlightDate"] = "";
                dr["Origin"] = "";
                dr["Destination"] = "";
                dr["SpotRate"] = "";
                dr["spotRateCategory"] = "";
                dr["Weight"] = "";
                dr["Commodity"] = "";
                dr["Description"] = "";
                dr["Aproval"] = "";
                dr["SpotRateID"] = "";
                
                dt.Rows.Add(dr);

                grdCreditdetails.DataSource = null;
                grdCreditdetails.DataSource = dt;
                grdCreditdetails.DataBind();
                //grdCreditdetails.Columns[1].Visible = false;

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error Message :" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion

        #region Report Audit Log
        private void SaveUserActivityLog(string ErrorLog)
        {

            try
            {
                ReportBAL objBAL = new ReportBAL();
                // taking all parameters as user selected in report in one variable - "Param"
                string Param = "Origin:" + ddlSource.SelectedItem.Text.ToString() + ",Destination:" + ddlDest.SelectedItem.Text.ToString() + ",Flight Number:" + ddlFlight.SelectedItem.Text.ToString() + ",Flt Dt:" + txtFlightDate.Text.ToString() + ", FrmDt:" + txtFromdate.Text.ToString() + ", ToDt:" + txtTodate.Text.ToString() + ",AWB Number:" + txtawbno.Text.ToString();

                objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "ListSpotRateApproval", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
            }
            catch (Exception ex)
            {

            }

        }
        #endregion

        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                string Reject = "";

                bool IsSelected = false;
                for (int i = 0; i < grdCreditdetails.Rows.Count; i++)
                {
                    if (((CheckBox)grdCreditdetails.Rows[i].FindControl("check")).Checked == true)
                    {
                        IsSelected = true;

                    }
                }
                if (IsSelected)
                {
                    for (int i = 0; i < grdCreditdetails.Rows.Count; i++)
                    {
                        if (((CheckBox)grdCreditdetails.Rows[i].FindControl("check")).Checked == true)
                        {
                            IsSelected = true;

                            string AWBNumber = ((Label)grdCreditdetails.Rows[i].FindControl("lblAwbno")).Text;
                            string spotrate = ((TextBox)grdCreditdetails.Rows[i].FindControl("txtSpotRate")).Text;
                            string UserName = Session["UserName"].ToString();
                            DateTime UpdatedOn = Convert.ToDateTime(Session["IT"]);
                            string spotID = ((Label)(grdCreditdetails.Rows[i].FindControl("lblSpotID"))).Text.Trim();

                            ds = BLSRate.UpdateStatusApproval(AWBNumber, spotrate, UserName, UpdatedOn,"REJECTED",spotID);

                            if (ds)
                            {
                                #region For Master Audit Log
                                MasterAuditBAL ObjMAL = new MasterAuditBAL();
                                #region Prepare Parameters
                                object[] Params = new object[7];
                                int j = 0;

                                //1
                                Params.SetValue("Spot Rate Rejected", j);
                                j++;

                                //2
                                Params.SetValue(AWBNumber, j);
                                j++;

                                //3

                                Params.SetValue("REJECT", j);
                                j++;

                                //4

                                Params.SetValue("", j);
                                j++;

                                //5
                                Params.SetValue("", j);
                                j++;

                                //6

                                Params.SetValue(Session["UserName"], j);
                                j++;

                                //7
                                Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), j);
                                j++;


                                #endregion Prepare Parameters
                                ObjMAL.AddMasterAuditLog(Params);
                                #endregion
                            }

                        }

                    }
                    if (ds == true)
                    {
                        btnList_Click(sender, e);
                        lblStatus.Text = "Spot Rate Rejected Successfully";
                        lblStatus.ForeColor = Color.Green;

                        return;
                    }
                }
                if (!IsSelected)
                {
                    lblStatus.Text = "Please select AWB Number(s) for Approval.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
            }
            catch (Exception ex)
            { }
        }
    }
}
