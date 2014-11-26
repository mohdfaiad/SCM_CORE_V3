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

namespace ProjectSmartCargoManager.CustomControls
{
    public partial class CustomManagementTab_CEBU : System.Web.UI.UserControl
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
                #region TabPanel Top Ten Stations Yield
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
                            DataSet ds = objDashboard.GetTopTenLocationsYieldMgtDashboard(Location, FromDate, ToDate);
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
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>GeneratePlotLocationYield('" + JSONString + "');</SCRIPT>", false);

                        }
                        catch (Exception ex)
                        { }
                    }
                    catch (Exception ex)
                    { }
                }
                #endregion
                else
                    #region TabPanel Top Ten Stations Revenue
                    if (TabContainer1.ActiveTabIndex == 1)
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
                                DataSet ds = objDashboard.GetTopTenLocationsRevenueMgtDashboard(Location, FromDate, ToDate);
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
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>GeneratePlotLocationRevenue('" + JSONString + "');</SCRIPT>", false);

                            }
                            catch (Exception ex)
                            { }
                        }
                        catch (Exception ex)
                        { }
                    }
                    #endregion
                    else
                        #region TabPanel Top Ten Stations Volume
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
                                    //DataSet ds = da.SelectRecords("sp_DashBoard_BookedCargoFlight", QueryPlotNames, QueryPlotVal, QueryPlotType);
                                    DataSet ds = objDashboard.GetTopTenLocationsVolumeMgtDashboard(Location, FromDate, ToDate);
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
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>GeneratePlotLocationVolume('" + JSONString + "');</SCRIPT>", false);

                                }
                                catch (Exception ex)
                                { }
                            }
                            catch (Exception ex)
                            { }
                        }
                        #endregion

                        else
                            #region TabPanel Top Ten Stations Shipper
                            if (TabContainer1.ActiveTabIndex == 3)
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
                                        DataSet ds = objDashboard.GetTopTenLocationsShipperMgtDashboard(Location, FromDate, ToDate);
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
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>GeneratePlotLocationShipper('" + JSONString + "');</SCRIPT>", false);

                                    }
                                    catch (Exception ex)
                                    { }
                                }
                                catch (Exception ex)
                                { }
                            }
                            #endregion
                            else
                                #region TabPanel Top Ten Stations Commodity
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
                                            DataSet ds = objDashboard.GetTopTenLocationsCommodityMgtDashboard(Location, FromDate, ToDate);
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
                                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>GeneratePlotLocationCommodity('" + JSONString + "');</SCRIPT>", false);

                                        }
                                        catch (Exception ex)
                                        { }
                                    }
                                    catch (Exception ex)
                                    { }
                                }
                                #endregion

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

    }
}