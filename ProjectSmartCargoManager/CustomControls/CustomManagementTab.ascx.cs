﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using QID.DataAccess;
using BAL;
using System.Drawing;

namespace ProjectSmartCargoManager.CustomControls
{
    public partial class CustomManagementTab : System.Web.UI.UserControl
    {
        #region Variables
        SQLServer da = new SQLServer(Global.GetConnectionString());
        BALFlightCapacity BFC = new BALFlightCapacity();
        BALDashboard objDashboard = new BALDashboard();
        ListBookingBAL objBAL = new ListBookingBAL();
        int gvNewPageIndex = 0;
        int gvEditIndex = -1;
        string gvUniqueID = String.Empty;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                txtFrmDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                LoadDestination();
                GetData_Click(sender, e);
            }

        }

        #region OnActiveTab_Changed
        protected void OnActiveTab_Changed(object sender, EventArgs e)
        {
            try
            {
                GetData_Click(null, null);

            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region validateUser
        private bool validateUser()
        {
            bool flag = false;
            try
            {
                HomeBL objHome = new HomeBL();
                int RoleId = Convert.ToInt32(Session["RoleID"]);
                DataSet objDS = objHome.GetUserPermissions(((System.Web.UI.TemplateControl)(Page)).AppRelativeVirtualPath, RoleId);
                objHome = null;

                if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < objDS.Tables[0].Rows.Count; j++)
                    {
                        if (objDS.Tables[0].Rows[j]["ControlId"].ToString() == "FlightDashboard")
                        {
                            //string userLoc = "";
                            //userLoc = Session["Station"].ToString();
                            //ddlLocation.Text = userLoc;
                            //ddlLocation.Enabled = false;
                            flag = true;
                            break;
                        }
                    }
                }
                objDS = null;

            }
            catch (Exception ex)
            { }
            return flag;
        }
        #endregion

        #region Load Location Dropdown
        /// <summary>
        /// Lodas location list in Origin and Destination dropdowns.
        /// </summary>
        public void LoadDestination()
        {
            try
            {
                BookingBAL objBLL = new BookingBAL();
                DataSet ds = objBLL.GetDestinationsForSource("");
                if (ds != null)
                {
                    ddlLocation.Items.Clear();
                    DataRow row = ds.Tables[0].NewRow();

                    ddlLocation.Items.Add("All");
                    ddlLocation.DataSource = ds;
                    ddlLocation.DataMember = ds.Tables[0].TableName;
                    ddlLocation.DataTextField = "AirportCode";
                    ddlLocation.DataValueField = "AirportCode";
                    ddlLocation.DataBind();

                    ddlLocation.SelectedIndex = 0;


                    // ----------- For ddlLocationAg------------

                    //ddlLocationAg.Items.Add("All");
                    //ddlLocationAg.DataSource = ds;
                    //ddlLocationAg.DataMember = ds.Tables[0].TableName;
                    //ddlLocationAg.DataTextField = "AirportCode";
                    //ddlLocationAg.DataValueField = "AirportCode";
                    //ddlLocationAg.DataBind();

                    //ddlLocationAg.SelectedIndex = 0;

                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion Load Location Dropdown

        #region btnGetDataFlights_Click
        protected void GetData_Click(object sender, EventArgs e)
        {
            try
            {
                IsValide();
                lblStatus.Text = string.Empty;
                string Location = "";
                DateTime FromDate = DateTime.Now, ToDate = DateTime.Now;
                {
                    if (ddlLocation.SelectedIndex > 0)
                    {
                        Location = ddlLocation.SelectedItem.Text.Trim();
                    }
                    FromDate = DateTime.ParseExact(txtFrmDate.Text.Trim(), "dd/MM/yyyy", null);
                    ToDate = DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", null);
                }
                string Result = objDashboard.GetDashBoardInterval(FromDate, ToDate);
                if (Result != "")
                {
                    lblStatus.Text = Result;
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
                #region TabPanel 1
                if (TabContainer1.ActiveTabIndex == 0)
                {
                    try
                    {
                        lblStatus.Text = string.Empty;
                        DataSet dsData = new DataSet();
                        BAL.Jquery objJQ = new Jquery();

                        string JSONString = string.Empty;

                        try
                        {
                            //DataSet ds = da.SelectRecords("sp_DashBoard_BookedCargoFlight", QueryPlotNames, QueryPlotVal, QueryPlotType);
                            DataSet ds = objDashboard.GetTopTenFlightsMgtDashboard(Location, FromDate, ToDate);
                            if (ds != null)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    if (ds.Tables.Count > 0)
                                    {
                                        //Serializing Dataset to JSON string for javascript
                                        DataTable dt = ds.Tables[0];
                                        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                                        Dictionary<string, object> row;
                                        foreach (DataRow dr in dt.Rows)
                                        {
                                            row = new Dictionary<string, object>();
                                            foreach (DataColumn col in dt.Columns)
                                            {
                                                row.Add(col.ColumnName, dr[col]);
                                            }
                                            rows.Add(row);
                                        }

                                        JSONString = serializer.Serialize(rows);
                                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DisplayGraph();</SCRIPT>", false);
                                    }
                                    else
                                    {
                                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HideGraph();</SCRIPT>", false);
                                    }
                                }
                                else
                                {
                                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HideGraph();</SCRIPT>", false);
                                }
                            }
                            else
                            {
                                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HideGraph();</SCRIPT>", false);
                            }
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>GeneratePlot('" + JSONString + "');</SCRIPT>", false);

                        }
                        catch (Exception ex)
                        { }
                    }
                    catch (Exception ex)
                    { }
                }
                #endregion
                else
                    #region TabPanel 2
                    if (TabContainer1.ActiveTabIndex == 1)
                    {
                        try
                        {
                            LoadFlightDropdown(Location, FromDate, ToDate);
                            GetRevenueManagement();
                        }
                        catch (Exception ex)
                        { }
                    }
                    #endregion
                    else
                        #region TabPanel 3
                        if (TabContainer1.ActiveTabIndex == 2)
                        {
                            try
                            {
                                lblStatus.Text = string.Empty;
                                DataSet dsData = new DataSet();
                                BAL.Jquery objJQ = new Jquery();

                                string JSONString = string.Empty;

                                try
                                {
                                    LoadAgentDropdown(Location, FromDate, ToDate);
                                    //DataSet ds = da.SelectRecords("sp_DashBoard_BookedCargoFlight", QueryPlotNames, QueryPlotVal, QueryPlotType);
                                    DataSet ds = objDashboard.GetTopTenAgentsMgtDashboard(Location, FromDate, ToDate);
                                    if (ds != null)
                                    {
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            if (ds.Tables.Count > 0)
                                            {
                                                //Serializing Dataset to JSON string for javascript
                                                DataTable dt = ds.Tables[0];
                                                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                                                Dictionary<string, object> row;
                                                foreach (DataRow dr in dt.Rows)
                                                {
                                                    row = new Dictionary<string, object>();
                                                    foreach (DataColumn col in dt.Columns)
                                                    {
                                                        row.Add(col.ColumnName, dr[col]);
                                                    }
                                                    rows.Add(row);
                                                }

                                                JSONString = serializer.Serialize(rows);
                                                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DisplayGraph();</SCRIPT>", false);
                                            }
                                            else
                                            {
                                                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HideGraph();</SCRIPT>", false);
                                            }
                                        }
                                        else
                                        {
                                            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HideGraph();</SCRIPT>", false);
                                        }
                                    }
                                    else
                                    {
                                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HideGraph();</SCRIPT>", false);
                                    }
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>GeneratePlotAgent('" + JSONString + "');</SCRIPT>", false);

                                }
                                catch (Exception ex)
                                { }
                            }
                            catch (Exception ex)
                            { }
                        }
                        #endregion
                        else
                            #region TabPanel 4
                            if (TabContainer1.ActiveTabIndex == 3)
                            {
                                if (hdAgent.Value != "")
                                {
                                    GetDataPerAgent_Click(null, null);
                                    return;
                                }
                                lblStatus.Text = string.Empty;
                                grdAWBDetailsFlightWise.DataSource = null;
                                grdAWBDetailsFlightWise.DataBind();
                                string AgentCode = string.Empty;
                                //if (ddlAgent.SelectedIndex != 0)
                                //{ AgentCode = ddlAgent.Text; }
                                object[] QueryValTab2 = { "", "", Session["UserName"].ToString(), ddlLocation.SelectedItem.Text.Trim(), AgentCode, "", txtFrmDate.Text, txtToDate.Text };
                                LoadAgentDropdown(Location, FromDate, ToDate);
                                DataSet dsTab2 = objDashboard.GetAWBDetailsPerFlight(QueryValTab2);
                                if (dsTab2 != null)
                                {
                                    grdAWBDetailsFlightWise.DataSource = dsTab2;
                                    grdAWBDetailsFlightWise.DataBind();
                                    dsTab2.Dispose();
                                }
                                else
                                {
                                    lblStatus.Text = "No Records Found!";
                                    lblStatus.ForeColor = Color.Blue;
                                    return;
                                }
                            }
                            #endregion

                            #region Commented Top Ten Booked Flights
                            //else
                            //    #region TabPanel 5
                            //    if (TabContainer1.ActiveTabIndex == 4)
                            //    {
                            //        try
                            //        {
                            //            lblStatus.Text = string.Empty;
                            //            DataSet dsData = new DataSet();
                            //            BAL.Jquery objJQ = new Jquery();

                            //            string JSONString = string.Empty;

                            //            try
                            //            {
                            //                //DataSet ds = da.SelectRecords("sp_DashBoard_BookedCargoFlight", QueryPlotNames, QueryPlotVal, QueryPlotType);
                            //                DataSet ds = objDashboard.GetTopTenBookedFlightsMgtDashboard(Location, FromDate, ToDate);
                            //                if (ds != null)
                            //                {
                            //                    if (ds.Tables[0].Rows.Count > 0)
                            //                    {
                            //                        if (ds.Tables.Count > 0)
                            //                        {
                            //                            //Serializing Dataset to JSON string for javascript
                            //                            DataTable dt = ds.Tables[0];
                            //                            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                            //                            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                            //                            Dictionary<string, object> row;
                            //                            foreach (DataRow dr in dt.Rows)
                            //                            {
                            //                                row = new Dictionary<string, object>();
                            //                                foreach (DataColumn col in dt.Columns)
                            //                                {
                            //                                    row.Add(col.ColumnName, dr[col]);
                            //                                }
                            //                                rows.Add(row);
                            //                            }

                            //                            JSONString = serializer.Serialize(rows);
                            //                            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DisplayGraph();</SCRIPT>", false);
                            //                        }
                            //                        else
                            //                        {
                            //                            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HideGraph();</SCRIPT>", false);
                            //                        }
                            //                    }
                            //                    else
                            //                    {
                            //                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HideGraph();</SCRIPT>", false);
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HideGraph();</SCRIPT>", false);
                            //                }
                            //                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>GeneratePlotBooked('" + JSONString + "');</SCRIPT>", false);

                            //            }
                            //            catch (Exception ex)
                            //            { }
                            //        }
                            //        catch (Exception ex)
                            //        { }
                            //    }
                            //    #endregion
                            #endregion
                                
                            else
                                    #region TabPanel 6
                                    if (TabContainer1.ActiveTabIndex == 4)
                                    {
                                        try
                                        {
                                            lblStatus.Text = string.Empty;
                                            DataSet dsData = new DataSet();
                                            BAL.Jquery objJQ = new Jquery();

                                            string JSONString = string.Empty;

                                            try
                                            {
                                                //DataSet ds = da.SelectRecords("sp_DashBoard_BookedCargoFlight", QueryPlotNames, QueryPlotVal, QueryPlotType);
                                                DataSet ds = objDashboard.GetTopTenLocationsMgtDashboard(Location, FromDate, ToDate);
                                                if (ds != null)
                                                {
                                                    if (ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        if (ds.Tables.Count > 0)
                                                        {
                                                            //Serializing Dataset to JSON string for javascript
                                                            DataTable dt = ds.Tables[0];
                                                            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                                            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                                                            Dictionary<string, object> row;
                                                            foreach (DataRow dr in dt.Rows)
                                                            {
                                                                row = new Dictionary<string, object>();
                                                                foreach (DataColumn col in dt.Columns)
                                                                {
                                                                    row.Add(col.ColumnName, dr[col]);
                                                                }
                                                                rows.Add(row);
                                                            }

                                                            JSONString = serializer.Serialize(rows);
                                                            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DisplayGraph();</SCRIPT>", false);
                                                        }
                                                        else
                                                        {
                                                            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HideGraph();</SCRIPT>", false);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HideGraph();</SCRIPT>", false);
                                                    }
                                                }
                                                else
                                                {
                                                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HideGraph();</SCRIPT>", false);
                                                }
                                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>GeneratePlotLocation('" + JSONString + "');</SCRIPT>", false);

                                            }
                                            catch (Exception ex)
                                            { }
                                        }
                                        catch (Exception ex)
                                        { }
                                    }
                                    #endregion
                                    //else
                                        #region TabPanel 7
                                        //if (TabContainer1.ActiveTabIndex == 5)
                                        //{
                                        //    string Loc = string.Empty;
                                        //    if (hdStation.Value != "")
                                        //    {
                                        //        Loc = hdStation.Value;
                                        //        hdStation.Value = "";
                                        //    }
                                        //    else
                                        //    {
                                        //        if (ddlLocation.SelectedIndex != 0)
                                        //        {
                                        //            Loc = ddlLocation.Text;
                                        //        }
                                        //    }
                                        //    SettingRevenueDetails(Loc, txtFrmDate.Text.Trim(), txtToDate.Text.Trim());
                                        //}
                                        #endregion

            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Load Flights Dropdown
        public void LoadFlightDropdown(string AgentCode, DateTime FromDt, DateTime ToDt, string Station)
        {
            try
            {

                DataSet ds = objDashboard.GettingAgentFlights(AgentCode, FromDt, ToDt, Station);
                if (ds != null)
                {
                    ddlFlightNo.DataSource = ds;
                    ddlFlightNo.DataTextField = "FlightNo";
                    ddlFlightNo.DataValueField = "FlightNo";
                    ddlFlightNo.DataBind();
                    ddlFlightNo.Items.Insert(0, "ALL");
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Validate Function
        public bool IsValide()
        {
            lblStatus.Text = "";

            if (txtFrmDate.Text.Trim() == "" && txtToDate.Text.Trim() != "")
            {
                lblStatus.Text = "Select From Date";
                return false;
            }
            if (txtFrmDate.Text.Trim() != "" && txtToDate.Text.Trim() == "")
            {
                lblStatus.Text = "Select To Date";
                return false;
            }

            if (txtFrmDate.Text.Trim() != "" && txtToDate.Text.Trim() != "")
            {
                DateTime dtFrom, dtTo;

                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;

                try
                {
                    dtFrom = DateTime.ParseExact(txtFrmDate.Text.Trim(), "dd/MM/yyyy", null);

                }
                catch (Exception ex)
                {
                    lblStatus.Text = "From date is invalid";
                    return false;
                }

                try
                {
                    dtTo = DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", null);

                }
                catch (Exception ex)
                {
                    lblStatus.Text = "To date is invalid";
                    return false;
                }

                if (dtFrom > dtTo)
                {
                    lblStatus.Text = "From date should be smaller than to date.";
                    return false;
                }
            }

            return true;
        }
        #endregion

        public bool EnableUserControl
        {
            get { return PanelGraph.Enabled; }
            set { PanelGraph.Enabled = value; }
        }

        #region Top Ten Flights Details

        #region GridView1 Event Handlers
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            try
            {
                GridViewRow row = e.Row;
                string strSortfl = string.Empty;
                string strSortfldt = string.Empty;


                // Make sure we aren't in header/footer rows
                if (row.DataItem == null)
                {
                    return;
                }


                GridView gv = new GridView();
                gv = GridView2;
                Table tb = new Table();
                tb = tblLYP;
                if (gv.UniqueID == gvUniqueID)
                {
                    gv.PageIndex = gvNewPageIndex;
                    gv.EditIndex = gvEditIndex;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + ((DataRowView)e.Row.DataItem)["FlightNo"].ToString() + "," + ((DataRowView)e.Row.DataItem)["FlightDate"].ToString() + "," + ((DataRowView)e.Row.DataItem)["DeptTime"].ToString() + "," + ((DataRowView)e.Row.DataItem)["ArrTime"].ToString() + "','one');</script>", false);
                }
                strSortfl = ((DataRowView)e.Row.DataItem)["FlightNo"].ToString();
                strSortfldt = ((DataRowView)e.Row.DataItem)["FlightDate"].ToString();
                string ori = ((DataRowView)e.Row.DataItem)["Origin"].ToString();
                string dest = ((DataRowView)e.Row.DataItem)["Destination"].ToString();
                string agent = "";

                DateTime myorgDateTime = DateTime.ParseExact(strSortfldt, "dd/MM/yyyy", null);

                DateTime myDateTime = DateTime.ParseExact(strSortfldt, "dd/MM/yyyy", null);

                DateTime myDateTime360 = myDateTime.AddYears(-1);
                string alldates360 = "'" + myDateTime.ToString("yyyy-MM-dd") + "'";
                int a360 = 1;
                while (myDateTime.AddDays(-7) >= myDateTime360)
                {
                    if (myDateTime.AddDays(-7) >= myDateTime360)
                    {
                        a360 = a360 + 1;
                        alldates360 = alldates360 + "'" + myDateTime.AddDays(-7).ToString("yyyy-MM-dd") + "'";
                    }
                    myDateTime = myDateTime.AddDays(-7);
                }
                alldates360 = alldates360.Replace("''", "','");
                string errormessage = "";
                BFC.GetAWBLevelDataRev(strSortfl, strSortfldt, myorgDateTime.ToString("yyyy-MM-dd").Trim(),
                    alldates360.Trim(), strSortfl.Trim(), ori, dest, agent, ref ds, ref errormessage, "", "");
                if (ds != null || ds.Tables.Count > 0)
                {
                    //if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            Session["ExtraInf"] = ds.Tables[2];
                        }
                        else
                        {
                            Session["ExtraInf"] = ds.Tables[3];
                        }

                        gv.DataSource = ds.Tables[0];
                        gv.DataBind();
                        //       tb.Rows[3].Cells[0].Text = ds.Tables[1].Rows[0][0].ToString();

                        ((Label)tb.FindControl("lbl1dL")).Text = ds.Tables[1].Rows[0][0].ToString();
                        ((Label)tb.FindControl("lbl1dYa")).Text = ds.Tables[1].Rows[0][1].ToString();
                        ((Label)tb.FindControl("lbl52dL")).Text = ds.Tables[1].Rows[0][2].ToString();
                        ((Label)tb.FindControl("lbl52dY")).Text = ds.Tables[1].Rows[0][3].ToString();
                        ((Label)tb.FindControl("lbl30dL")).Text = ds.Tables[1].Rows[0][4].ToString();
                        ((Label)tb.FindControl("lbl30dY")).Text = ds.Tables[1].Rows[0][5].ToString();
                        ((Label)tb.FindControl("lbl365dL")).Text = ds.Tables[1].Rows[0][6].ToString();
                        ((Label)tb.FindControl("lbl365dY")).Text = ds.Tables[1].Rows[0][7].ToString();
                        ((Label)tb.FindControl("lbl1dP")).Text = ds.Tables[1].Rows[0][8].ToString();
                        ((Label)tb.FindControl("lbl52dP")).Text = ds.Tables[1].Rows[0][9].ToString();
                        ((Label)tb.FindControl("lbl30dP")).Text = ds.Tables[1].Rows[0][10].ToString();
                        ((Label)tb.FindControl("lbl365dP")).Text = ds.Tables[1].Rows[0][11].ToString();

                    }
                }
            }
            catch (Exception ex)
            {
                ds = null;
                dt1 = null;
                lblStatus.Text = ex.Message;
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
                if (dt1 != null)
                    dt1.Dispose();
            }
        }
        #endregion

        #region Get data for revenue management
        private void getdata(string source, string dest, string flight, string status, string fromdate, string todate,
            string awbnumber, string StrAgentCode, string prefix, bool ShowNilFlights)
        {
            DataSet ds = new DataSet();
            try
            {
                string errormessage = "";
                BFC.GetAllAWBs(source, dest, flight, fromdate, todate, prefix, status, awbnumber, ref ds, ref errormessage, 
                    StrAgentCode,ShowNilFlights);
                if (ds != null || ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dispgrid.Visible = true;
                        GridView1.DataSource = ds.Tables[0];
                        GridView1.DataBind();
                        //Show data on fields.
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            lblOrigin.Text = ds.Tables[0].Rows[0]["Origin"].ToString();
                            lblDestination.Text = ds.Tables[0].Rows[0]["Destination"].ToString();
                            lblFltDate.Text = ds.Tables[0].Rows[0]["FlightDate"].ToString();
                            lblFltNum.Text = ds.Tables[0].Rows[0]["FlightNo"].ToString();
                            lblDeptTime.Text = ds.Tables[0].Rows[0]["DeptTime"].ToString();
                            lblArrivalTime.Text = ds.Tables[0].Rows[0]["ArrTime"].ToString();
                            if (ds.Tables[0].Rows[0]["FlightStatus"] != null)
                                lblFltStatus.Text = ds.Tables[0].Rows[0]["FlightStatus"].ToString();
                            else
                                lblFltStatus.Text = "New";

                            lblCapacity.Text = ds.Tables[0].Rows[0]["Maximum"].ToString();
                            lblConfirmed.Text = ds.Tables[0].Rows[0]["Confirmed"].ToString();
                            lblQueued.Text = ds.Tables[0].Rows[0]["Queued"].ToString();
                            lblBlocked.Text = ds.Tables[0].Rows[0]["Blocked"].ToString();
                            lblAvailable.Text = ds.Tables[0].Rows[0]["Available"].ToString();

                            //Calculate revenue from AWB grid.
                            Decimal revenue = 0;
                            Decimal cost = 0;
                            Decimal profit = 0;
                            foreach (GridViewRow gv in GridView2.Rows)
                            {
                                if (((Label)gv.FindControl("lblrevenue")).Text != "")
                                    revenue += Convert.ToDecimal(((Label)gv.FindControl("lblrevenue")).Text);
                                else
                                    revenue += 0;
                                if (((Label)gv.FindControl("lblCost")).Text != "")
                                    cost += Convert.ToDecimal(((Label)gv.FindControl("lblCost")).Text);
                                else
                                    cost += 0;
                                if (((Label)gv.FindControl("lblProfit")).Text != "")
                                    profit += Convert.ToDecimal(((Label)gv.FindControl("lblProfit")).Text);
                                else
                                    profit += 0;
                            }
                            lblRevenue.Text = revenue.ToString();
                            lblProfitability.Text = profit.ToString();
                            lblCost.Text = cost.ToString();

                            pnlRevenueDetails.Visible = true;
                            Session["dsExp"] = ds.Tables[0];

                            return;
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                ds = null;
                lblStatus.Text = ex.Message;
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
            pnlRevenueDetails.Visible = false;
            lblStatus.Text = "No Records Found";
            lblStatus.ForeColor = Color.Red;
            GridView1.DataSource = null;
            GridView1.DataBind();
            dispgrid.Visible = false;
        }
        #endregion Get data for revenue management

        #region Get Revenue Management Data
        private void GetRevenueManagement()
        {
            try
            {
                if (!IsValide())
                    return;
                if (hdFlightNo.Value != "")
                {
                    ddlFlightNo.SelectedValue = hdFlightNo.Value;
                    hdFlightNo.Value = "";
                }
                string source, dest, flight, fromdate, awbnumber, awbprefix, todate, status, StrAgentCode;
                source = dest = flight = fromdate = todate = awbnumber = StrAgentCode = "";

                if (ddlLocation.SelectedIndex != 0)
                {
                    source = ddlLocation.SelectedValue;
                }
                dest = "";
                if (ddlFlightNo.SelectedIndex != 0)
                {
                    flight = ddlFlightNo.SelectedValue;
                }
                awbnumber = "";

                fromdate = txtFrmDate.Text;
                todate = txtToDate.Text;
                status = "";
                awbprefix = "";
                StrAgentCode = "";

                getdata(source, dest, flight, status, fromdate, todate, awbnumber, StrAgentCode, awbprefix,false);

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: " + ex.Message;

            }
        }
        #endregion Get Revenue Management Data

        #region Confirm Shipment button click event
        protected void ConfirmShipment(object sender, EventArgs e)
        {
            try
            {
                BALFlightCapacity objBal = new BALFlightCapacity();
                int rowindex = 0;
                Button TextBox = (Button)sender;
                GridViewRow grRow = (GridViewRow)TextBox.NamingContainer;
                rowindex = grRow.RowIndex;
                string FlightNo = hdnFliNo.Value;
                string FlightDate = hdnFliDt.Value;

                string AWBNumber = ((Label)grRow.FindControl("lblAWBno")).Text.Trim();

                if (AWBNumber.Length > 8)
                    AWBNumber = AWBNumber.Substring(AWBNumber.Length - 8, 8);

                //flight No & date Need to pass
                objBal.ConfirmAWBNumber(AWBNumber, FlightNo, FlightDate, Convert.ToString(Session["UserName"]), Convert.ToDateTime(Session["IT"]).ToString("MM/dd/yyyy HH:mm"));
                GetRevenueManagement();
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "Error: " + ex.Message + "", true);
            }
        }
        #endregion Confirm Shipment button click event

        #region Open AWB in Edit Mode from revenue management
        public void editMode(object sender, EventArgs e)
        {

            try
            {
                string awb = hdn.Value;

                string query = "'../GHA_ConBooking.aspx?command=Edit&AWBNumber=" + awb + "'";

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open(" + query + ",'_blank');", true);

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "newwindow.focus();", true);

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
            }
        }
        #endregion Open AWB in Edit Mode from revenue management

        #region GridView2_RowDataBound
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (Request.QueryString["command"] == null ||
                    !Request.QueryString["command"].Equals("Revenue", StringComparison.OrdinalIgnoreCase))
                {
                    e.Row.Cells[11].Visible = false;
                    e.Row.Cells[12].Visible = false;
                    e.Row.Cells[13].Visible = false;
                    e.Row.Cells[14].Visible = false;
                    e.Row.Cells[15].Visible = false;
                    //e.Row.Cells[17].Visible = false;
                }
            }
            catch (Exception ex) { }
        }
        #endregion GridView2_RowDataBound

        #region GridView1_PageIndexChanging
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GetRevenueManagement();
        }
        #endregion GridView1_PageIndexChanging

        #region Populate Flight DropDown Data

        public void LoadFlightDropdown(string Station, DateTime FromDt, DateTime ToDt)
        {
            try
            {

                DataSet ds = objDashboard.GetTopTenFlightsMgtDashboard(Station, FromDt, ToDt);
                if (ds != null)
                {
                    ddlFlightNo.DataSource = ds;
                    ddlFlightNo.DataTextField = "FlightNo";
                    ddlFlightNo.DataValueField = "FlightNo";
                    ddlFlightNo.DataBind();
                    ddlFlightNo.Items.Insert(0, "Select");
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #endregion Revenue Management

        #region Top Ten Agent Details
        #region Load Agent Dropdown
        public void LoadAgentDropdown(string Station, DateTime FromDt, DateTime ToDt)
        {
            try
            {

                DataSet ds = objDashboard.GetTopTenAgentsMgtDashboard(Station, FromDt, ToDt);
                if (ds != null)
                {
                    ddlAgent.DataSource = ds;
                    ddlAgent.DataTextField = "AgentCode";
                    ddlAgent.DataValueField = "AgentCode";
                    ddlAgent.DataBind();
                    ddlAgent.Items.Insert(0, "ALL");
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Button Agent List Click
        protected void GetDataPerAgent_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                IsValide();
                string Location = "";
                DateTime FromDate = DateTime.Now, ToDate = DateTime.Now;
                {
                    if (ddlLocation.SelectedIndex > 0)
                    {
                        Location = ddlLocation.SelectedItem.Text.Trim();
                    }
                    FromDate = DateTime.ParseExact(txtFrmDate.Text.Trim(), "dd/MM/yyyy", null);
                    ToDate = DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", null);
                }
                grdAWBDetailsFlightWise.DataSource = null;
                grdAWBDetailsFlightWise.DataBind();
                string AgentCode = string.Empty;
               // LoadAgentDropdown(Location, FromDate, ToDate);
                if (hdAgent.Value == "")
                {
                    if (ddlAgent.SelectedIndex != 0)
                    { AgentCode = ddlAgent.Text; }
                }
                else
                {
                    AgentCode = hdAgent.Value;
                    ddlAgent.Text = AgentCode;
                }
                object[] QueryValTab2 = { "", "", Session["UserName"].ToString(), ddlLocation.SelectedItem.Text.Trim(), AgentCode, "", txtFrmDate.Text, txtToDate.Text };
                DataSet dsTab2 = objDashboard.GetAWBDetailsPerFlight(QueryValTab2);
                hdAgent.Value = "";
                if (dsTab2 != null)
                {
                    grdAWBDetailsFlightWise.DataSource = dsTab2;
                    grdAWBDetailsFlightWise.DataBind();
                    dsTab2.Dispose();
                }
                else
                {
                    lblStatus.Text = "No Records Found!";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }



            }
            catch (Exception Ex)
            {
            }
        }
        #endregion
        #endregion

        #region Top Ten Stations Details

        #region Getting Revenue Details
        public void SettingRevenueDetails(string Station, string FromDate, string ToDate)
        {
            try
            {
                lblStatus.Text = string.Empty;
                DataSet dsExport = new DataSet();
                DataSet dsImport = new DataSet();
                string ErrorMsg = string.Empty;
                Decimal revenue = 0;
                Decimal cost = 0;
                Decimal profit = 0;
                Decimal TotalExpTonnage = 0;
                Decimal TotalImpTonnage = 0;
                DateTime dtFrom;
                DateTime dtTo;
                dtFrom=DateTime.ParseExact(txtFrmDate.Text,"dd/MM/yyyy",null);
                dtTo = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);

                #region Setting Export Revenue Summary
                //BFC.GetAllAWBs(Station, "", "", FromDate, ToDate, "", "", "", ref dsExport, ref ErrorMsg, "");
                //if (dsExport != null)
                //{
                //    if (dsExport.Tables.Count > 0)
                //    {
                //        if (dsExport.Tables[0].Rows.Count > 0)
                //        {
                //            //Calculate revenue from AWB grid.
                           
                //            foreach (DataRow dr in dsExport.Tables[0].Rows)
                //            {
                //                if ( dr["Total"].ToString() != "")
                //                    revenue += Convert.ToDecimal(dr["Total"].ToString());
                //                else
                //                    revenue += 0;
                //                if (dr["Cost"].ToString() != "")
                //                    cost += Convert.ToDecimal(dr["Cost"].ToString());
                //                else
                //                    cost += 0;
                //                if (dr["Profit"].ToString() != "")
                //                    profit += Convert.ToDecimal(dr["Profit"].ToString());
                //                else
                //                    profit += 0;
                //            }
                //            lblExpRevenue.Text = revenue.ToString();
                //            lblExpProfit.Text = profit.ToString();
                //            lblExpCost.Text = cost.ToString();
                //        }
                //    }
                //}
                #endregion

                #region Setting Import Revenue Summary
                //revenue = 0;
                //profit = 0;
                //cost = 0;
                //BFC.GetAllAWBs(Station, "", "", FromDate, ToDate, "", "", "", ref dsImport, ref ErrorMsg, "");
                //if (dsImport != null)
                //{
                //    if (dsImport.Tables.Count > 0)
                //    {
                //        if (dsExport.Tables[0].Rows.Count > 0)
                //        {
                //            //Calculate revenue from AWB grid.

                //            foreach (DataRow dr in dsImport.Tables[0].Rows)
                //            {
                //                if (dr["Total"].ToString() != "")
                //                    revenue += Convert.ToDecimal(dr["Total"].ToString());
                //                else
                //                    revenue += 0;
                //                if (dr["Cost"].ToString() != "")
                //                    cost += Convert.ToDecimal(dr["Cost"].ToString());
                //                else
                //                    cost += 0;
                //                if (dr["Profit"].ToString() != "")
                //                    profit += Convert.ToDecimal(dr["Profit"].ToString());
                //                else
                //                    profit += 0;
                //            }
                //            lblImpRevenue.Text = revenue.ToString();
                //            lblImpProfit.Text = profit.ToString();
                //            lblImpCost.Text = cost.ToString();
                //        }
                //    }
                //}
                #endregion

                #region Setting Total Tonnage

                DataSet dsTotalTonnage = objDashboard.GetTopTenLocationsMgtDashboard(Station, dtFrom, dtTo);
                if (dsTotalTonnage != null)
                {
                    foreach (DataRow dr in dsTotalTonnage.Tables[0].Rows)
                    {
                        if (dr["ExportWt"].ToString() != "")
                            TotalExpTonnage += Convert.ToDecimal(dr["ExportWt"].ToString());
                        else
                            TotalExpTonnage += 0;

                        if (dr["ImportWt"].ToString() != "")
                            TotalImpTonnage += Convert.ToDecimal(dr["ImportWt"].ToString());
                        else
                            TotalImpTonnage += 0;
                    }
                    lblTotalExpTonnage.Text = TotalExpTonnage.ToString();
                    lblTotalImpTonnage.Text = TotalImpTonnage.ToString();
                }
                #endregion

                #region Getting Import & Export Grid Data
                grdexportWareHouseList.DataSource = null;
                grdexportWareHouseList.DataBind();
                grdImportWarehouse.DataSource = null;
                grdImportWarehouse.DataBind();
                object[] QueryValues = { dtFrom, dtTo, Station };
                DataSet dsEx = objDashboard.GetExportWarehouseData(QueryValues);
                if (dsEx != null)
                {
                    grdexportWareHouseList.DataSource = dsEx;
                    grdexportWareHouseList.DataBind();
                }
                DataSet dsImp = objDashboard.GetImportWarehouseData(QueryValues);
                if (dsImp != null)
                {
                    grdImportWarehouse.DataSource = dsImp;
                    grdImportWarehouse.DataBind();
                }
                #endregion

                dsExport.Dispose();
                dsImport.Dispose();
                dsTotalTonnage.Dispose();
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #endregion
    }
}