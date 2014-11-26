using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using QID.DataAccess;
using BAL;


namespace ProjectSmartCargoManager
{
    public partial class CustomOperationsTab : System.Web.UI.UserControl
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());
        BALDashboard BFC = new BALDashboard();
        string gvUniqueID = String.Empty;
        int gvNewPageIndex = 0;
        int gvEditIndex = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFltDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFrmDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                LoadDestination();
                GetData_Click(sender, e);
            }

        }

        #region OnActiveTab_Changed
        protected void OnActiveTab_Changed(object sender, EventArgs e)
        {
            BALDashboard objDashboard = new BALDashboard();
            string Location = Convert.ToString(Session["Station"]);
            DateTime FromDate = DateTime.Now, ToDate = DateTime.Now;
            FromDate = DateTime.ParseExact(txtFrmDate.Text.Trim(), "dd/MM/yyyy", null);
            ToDate = DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", null);
            
            #region TabIndex1-OutFlights
            if (TabContainer1.ActiveTabIndex == 0)
            {
                try
                {
                    DataSet dsData = new DataSet();
                    BAL.Jquery objJQ = new Jquery();

                    if (ddlLocation.SelectedIndex > 0)
                    {
                        Location = ddlLocation.SelectedItem.Text.Trim();
                    }

                    string JSONString = string.Empty;
                    object[] QueryPlotVal = { FromDate, ToDate, "", Location };

                    try
                    {   
                        DataSet ds = objDashboard.GetFlightDataAgent(QueryPlotVal);
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
                                    //Populating FlightNo dropdown
                                    ddlFlightNo.DataSource = ds;
                                    ddlFlightNo.DataTextField = "FlightNo";
                                    ddlFlightNo.DataValueField = "FlightNo";
                                    ddlFlightNo.DataBind();
                                }
                            }
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

            #region TabIndex2-OutAWBS
            else if (TabContainer1.ActiveTabIndex == 1)
            {
                //GetRevenueManagement();
                GetDataPerFlight_Click(null, null);
            }
            #endregion

            #region TabIndex3-ExportInventory
            else if (TabContainer1.ActiveTabIndex == 2)
            {
                try
                {
                    object[] QueryPlotVal = { FromDate, ToDate, Location };
                    DataSet ds = objDashboard.GetExportWarehouseData(QueryPlotVal);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        grdexportWareHouseList.DataSource = ds;
                        grdexportWareHouseList.DataBind();
                    }
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>GeneratePlot('" + JSONString + "');</SCRIPT>", false);

                }
                catch (Exception ex)
                { }
            }
            #endregion

            #region TabIndex4-IcomingFlights
            else if (TabContainer1.ActiveTabIndex == 3)
            {
                string JSONString = string.Empty;

                try
                {
                    object[] QueryPlotVal = { FromDate, ToDate, Location };
                    DataSet ds = objDashboard.GetInComingFlightsData(QueryPlotVal);
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
                            }
                        }
                    }
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>GeneratePlotForIncoming('" + JSONString + "');</SCRIPT>", false);

                }
                catch (Exception ex)
                { }
            }
            #endregion

            #region TabIndex5-ImportInventory
            else if (TabContainer1.ActiveTabIndex == 4)
            {
                try
                {
                    object[] QueryPlotVal = { FromDate, ToDate, Location };
                    DataSet ds = objDashboard.GetImportWarehouseData(QueryPlotVal);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        grdImportWarehouse.DataSource = ds;
                        grdImportWarehouse.DataBind();
                    }
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>GeneratePlot('" + JSONString + "');</SCRIPT>", false);

                }
                catch (Exception ex)
                { }
            }
            #endregion

            #region TabIndex6-OutGoing-NotUsed
            else if (TabContainer1.ActiveTabIndex == 5)
            {
                try
                {
                    object[] QueryPlotVal = { FromDate, ToDate, Location };
                    DataSet ds = objDashboard.GetOutGoingFlightsData(QueryPlotVal);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        grdOutGoingFlight.DataSource = ds;
                        grdOutGoingFlight.DataBind();
                    }                    
                }
                catch (Exception ex)
                { }
            }
            #endregion                        
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
            OnActiveTab_Changed(null, null);
        }
        #endregion

        #region Load Flights Dropdown
        public void LoadFlightDropdown(string AgentCode, DateTime FromDt, DateTime ToDt, string Station)
        {
            BALDashboard objDashboard = new BALDashboard();
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
            objDashboard = null;
        }
        #endregion

        #region Validate Search Dates
        public bool IsValide()
        {
            //lblStatus.Text = "";

            if (txtFrmDate.Text.Trim() == "" && txtToDate.Text.Trim() != "")
            {
                //lblStatus.Text = "Select From Date";
                return false;
            }
            if (txtFrmDate.Text.Trim() != "" && txtToDate.Text.Trim() == "")
            {
                //lblStatus.Text = "Select To Date";
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
                    //lblStatus.Text = "From date is invalid";
                    return false;
                }

                try
                {
                    dtTo = DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", null);

                }
                catch (Exception ex)
                {
                    //lblStatus.Text = "To date is invalid";
                    return false;
                }

                if (dtFrom > dtTo)
                {
                    //lblStatus.Text = "From date should be smaller than to date.";
                    return false;
                }
            }

            return true;
        }
        #endregion Validate Search Criteria for Revenue Management

        #region Get data for revenue management
        private void getdata(string source, string dest, string flight, string status, string fromdate, string todate,
            string awbnumber, string StrAgentCode, string prefix)
        {
            DataSet ds = new DataSet();
            try
            {
                string errormessage = "";
                BFC.GetAllAWBs(source, dest, flight, fromdate, todate, prefix, status, awbnumber, ref ds, ref errormessage, StrAgentCode);
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
                //lblStatus.Text = ex.Message;
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
            pnlRevenueDetails.Visible = false;
            //lblStatus.Text = "No Records Found";
            //lblStatus.ForeColor = Color.Red;
            GridView1.DataSource = null;
            GridView1.DataBind();
            dispgrid.Visible = false;
        }
        #endregion Get data for revenue management

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
                    //{
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Session["ExtraInf"] = ds.Tables[0];
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

                    //}
                }
            }
            catch (Exception ex)
            {
                ds = null;
                dt1 = null;
                //lblStatus.Text = ex.Message;
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

        #region GridView1_PageIndexChanging
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GetDataPerFlight_Click(null, null);
        }
        #endregion GridView1_PageIndexChanging

        protected void GetDataPerFlight_Click(object sender, EventArgs e)
        {
            if (!IsValide())
                return;

            string source, dest, flight, fromdate, awbnumber, awbprefix, todate, status, StrAgentCode;
            source = dest = flight = fromdate = todate = awbnumber = StrAgentCode = "";

            source = ddlLocation.SelectedValue;
            dest = "";
            flight = ddlFlightNo.SelectedValue;
            awbnumber = "";

            fromdate = txtFltDate.Text;
            todate = txtFltDate.Text;
            status = "";
            awbprefix = "";
            StrAgentCode = "";

            getdata(source, dest, flight, status, fromdate, todate, awbnumber, StrAgentCode, awbprefix);
        }

        protected void ConfirmShipment(object sender, EventArgs e)
        {
            try
            {
                BALFlightCapacity objBal = new BALFlightCapacity();
                int rowindex = 0;
                Button TextBox = (Button)sender;
                GridViewRow grRow = (GridViewRow)TextBox.NamingContainer;
                rowindex = grRow.RowIndex;
                string FlightNo = ddlFlightNo.SelectedItem.ToString();
                string FlightDate = txtFrmDate.Text;

                string AWBNumber = ((HyperLink)grRow.FindControl("hlnkAWBNumber")).Text.Trim();

                string strAWBPrefix = AWBNumber.Substring(0, 3);
                
                if (AWBNumber.Length > 8)
                    AWBNumber = AWBNumber.Substring(AWBNumber.Length - 8, 8);

                //flight No & date Need to pass
                objBal.ConfirmAWBNumber(strAWBPrefix,AWBNumber, FlightNo, FlightDate, Convert.ToString(Session["UserName"]), Convert.ToDateTime(Session["IT"]).ToString("MM/dd/yyyy HH:mm"));
                GetDataPerFlight_Click(null, null);
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "Error: " + ex.Message + "", true);
            }
        }
    }
}