using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using QID.DataAccess;
using BAL;
using System.Drawing;


namespace ProjectSmartCargoManager
{
    public partial class CustomAgentTab : System.Web.UI.UserControl
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());
        BALDashboard objDashboard = new BALDashboard();
        ListBookingBAL objBAL = new ListBookingBAL();
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
            #region Commented Code
            //try
            //{

            //    DataSet dsData = new DataSet();
            //    BAL.Jquery objJQ = new Jquery();
            //    string Location = "";
            //    DateTime FromDate = DateTime.Now, ToDate = DateTime.Now;
            //    {
            //        if (ddlLocation.SelectedIndex > 0)
            //        {
            //            Location = ddlLocation.SelectedItem.Text.Trim();
            //        }
            //        FromDate = DateTime.ParseExact(txtFrmDate.Text.Trim(), "dd/MM/yyyy", null);
            //        ToDate = DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", null);
            //    }
                
            //    string JSONString = string.Empty;
            //    object[] QueryPlotVal = { FromDate, ToDate,Session["UserName"].ToString(), Location };

            //    try
            //    {
            //        //DataSet ds = da.SelectRecords("sp_DashBoard_BookedCargoFlight", QueryPlotNames, QueryPlotVal, QueryPlotType);
            //        DataSet ds = objDashboard.GetFlightDataAgent(QueryPlotVal);
            //        if (ds != null)
            //        {
            //            if (ds.Tables[0].Rows.Count > 0)
            //            {
            //                if (ds.Tables.Count > 0)
            //                {
            //                    //Serializing Dataset to JSON string for javascript
            //                    DataTable dt = ds.Tables[0];
            //                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            //                    Dictionary<string, object> row;
            //                    foreach (DataRow dr in dt.Rows)
            //                    {
            //                        row = new Dictionary<string, object>();
            //                        foreach (DataColumn col in dt.Columns)
            //                        {
            //                            row.Add(col.ColumnName, dr[col]);
            //                        }
            //                        rows.Add(row);
            //                    }

            //                    JSONString = serializer.Serialize(rows);

            //                    //Populating FlightNo dropdown
            //                    ddlFlightNo.DataSource = ds;
            //                    ddlFlightNo.DataTextField = "FlightNo";
            //                    ddlFlightNo.DataValueField = "FlightNo";
            //                    ddlFlightNo.DataBind();

            //                    object[] QueryValTab2 = { "", "", Session["UserName"].ToString(), Session["Station"].ToString(), Session["UserName"].ToString(), "", txtFrmDate.Text, txtToDate.Text };
            //                    DataSet dsTab2 = objDashboard.GetAWBDetailsPerFlight(QueryValTab2);
            //                    if (dsTab2 != null)
            //                    {
            //                        grdAWBDetailsFlightWise.DataSource = dsTab2;
            //                        grdAWBDetailsFlightWise.DataBind();
            //                    }

            //                }
            //            }
            //        }
            //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>GeneratePlot('" + JSONString + "');</SCRIPT>", false);

            //    }
            //    catch (Exception ex)
            //    { }
            //}
            //catch (Exception ex)
            //{ }
            #endregion
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

                        object[] QueryPlotVal = { FromDate, ToDate, Session["AgentCode"].ToString(), Location };

                        try
                        {
                            LoadFlightDropdown(Session["AgentCode"].ToString(), FromDate, ToDate, Location);
                            //DataSet ds = da.SelectRecords("sp_DashBoard_BookedCargoFlight", QueryPlotNames, QueryPlotVal, QueryPlotType);
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
                        if (hdFlightNo.Value != "")
                        {
                            GetDataPerFlight_Click(null, null);
                            return;
                        }
                        lblStatus.Text = string.Empty;
                        grdAWBDetailsFlightWise.DataSource = null;
                        grdAWBDetailsFlightWise.DataBind();
                        string FlightNo = string.Empty;
                        //if (ddlFlightNo.SelectedIndex != 0)
                        //{ FlightNo = ddlFlightNo.Text; }
                        object[] QueryValTab2 = { "", "", Session["UserName"].ToString(), ddlLocation.SelectedItem.Text.Trim(), Session["AgentCode"].ToString(), FlightNo, txtFrmDate.Text, txtToDate.Text };
                        LoadFlightDropdown(Session["AgentCode"].ToString(), FromDate, ToDate, Location);
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
                    else
                        #region TabPanel 3 
                        if (TabContainer1.ActiveTabIndex == 2)
                        {
                            lblStatus.Text = string.Empty;
                            grdInvoiceListing.DataSource = null;
                            grdInvoiceListing.DataBind();
                            DataSet dsTab3 = objDashboard.GetInvoiceAgentListing(Session["AgentCode"].ToString(), txtFrmDate.Text, txtToDate.Text);
                            if (dsTab3 != null)
                            {
                                grdInvoiceListing.DataSource = dsTab3;
                                grdInvoiceListing.DataBind();
                                dsTab3.Dispose();
                            }
                            else
                            {
                                lblStatus.Text = "No Records Found!";
                                lblStatus.ForeColor = Color.Blue;
                                return;
                            }
                            
                        }
                        #endregion
                        else
                            #region TabPanel4
                            if (TabContainer1.ActiveTabIndex == 3)
                            {
                                lblStatus.Text = string.Empty;
                                grdDeals.DataSource = null;
                                grdDeals.DataBind();
                                DataSet dsTab3 = objDashboard.GetAgentDeals(Session["AgentCode"].ToString(), txtFrmDate.Text, txtToDate.Text);
                                if (dsTab3 != null)
                                {
                                    grdDeals.DataSource = dsTab3;
                                    grdDeals.DataBind();
                                    dsTab3.Dispose();
                                }
                                else
                                {
                                    lblStatus.Text = "No Records Found!";
                                    lblStatus.ForeColor = Color.Blue;
                                    return;
                                }
                            }
                            #endregion
                            else
                                #region TabPanel 5
                                if (TabContainer1.ActiveTabIndex == 4)
                                {
                                    lblStatus.Text = string.Empty;
                                    grdAgentQuotes.DataSource = null;
                                    grdAgentQuotes.DataBind();
                                    DataSet dsTab4 = objDashboard.GetAgentQuotes(Session["AgentCode"].ToString(), Location, txtFrmDate.Text, txtToDate.Text);
                                    if (dsTab4 != null)
                                    {
                                        grdAgentQuotes.DataSource = dsTab4;
                                        grdAgentQuotes.DataBind();
                                        dsTab4.Dispose();
                                    }
                                    else
                                    {
                                        lblStatus.Text = "No Records Found!";
                                        lblStatus.ForeColor = Color.Blue;
                                        return;
                                    }
                                }
                            #endregion
                                else
                                    #region TabPanel 6
                                    if (TabContainer1.ActiveTabIndex == 5)
                                    {
                                        lblStatus.Text = "";
                                        grdAgentCapacity.DataSource = null;
                                        grdAgentCapacity.DataBind();
                                        DataSet dsTab4 = objDashboard.GetAgentCapacity(Session["AgentCode"].ToString(), Location, txtFrmDate.Text, txtToDate.Text);
                                        if (dsTab4 != null)
                                        {
                                            grdAgentCapacity.DataSource = dsTab4;
                                            grdAgentCapacity.DataBind();
                                            dsTab4.Dispose();
                                        }
                                        else
                                        {
                                            lblStatus.Text = "No Records Found!";
                                            lblStatus.ForeColor = Color.Blue;
                                            return;
                                        }

                                    }
                                    #endregion
                                    else
                                        #region TabPanel 7
                                        if (TabContainer1.ActiveTabIndex == 6)
                                        {
                                            lblStatus.Text = "";
                                            grdClaims.DataSource = null;
                                            grdClaims.DataBind();
                                            DataSet dsTab4 = objDashboard.GetAgentClaims(Session["AgentCode"].ToString(), Location, txtFrmDate.Text, txtToDate.Text);
                                            if (dsTab4 != null)
                                            {
                                                grdClaims.DataSource = dsTab4;
                                                grdClaims.DataBind();
                                                dsTab4.Dispose();
                                            }
                                            else
                                            {
                                                lblStatus.Text = "No Records Found!";
                                                lblStatus.ForeColor = Color.Blue;
                                                return;
                                            }
                                        }
                                        #endregion
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Load Flights Dropdown
        public void LoadFlightDropdown(string AgentCode,DateTime FromDt,DateTime ToDt,string Station)
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

        #region Button Flight List Click
        protected void GetDataPerFlight_Click(object sender, EventArgs e)
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
                string FlightNo = string.Empty;
                //LoadFlightDropdown(Session["AgentCode"].ToString(), FromDate, ToDate, Location);
                if (hdFlightNo.Value == "")
                {
                    if (ddlFlightNo.SelectedIndex != 0)
                    { FlightNo = ddlFlightNo.Text; }
                }
                else
                {
                    FlightNo = hdFlightNo.Value;
                    ddlFlightNo.Text = FlightNo;
                }
                object[] QueryValTab2 = { "", "", Session["UserName"].ToString(), ddlLocation.SelectedItem.Text.Trim(), Session["AgentCode"].ToString(), FlightNo, txtFrmDate.Text, txtToDate.Text };
                DataSet dsTab2 = objDashboard.GetAWBDetailsPerFlight(QueryValTab2);
                hdFlightNo.Value = "";
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



    }
}