using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;
using QID.DataAccess;
using System.Data.SqlClient;
using BAL;


namespace ProjectSmartCargoManager
{
    public partial class frmAgentCapacityList : System.Web.UI.Page
    {
        ListBookingBAL objBAL = new ListBookingBAL();

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                LoadGridUserList();
                GetFlights();
                GetAirports();
                if (Convert.ToString(Session["AgentCode"]) != "")
                {
                    TXTAgentCode.Text = Convert.ToString(Session["AgentCode"]);
                    TXTAgentCode.ReadOnly = true;
                    IBOrigin.Visible = false;  
                }
            }
        }
        #endregion

        #region Add New Row to Grid
        public void LoadGridUserList()
        {

            DataTable myDataTable = new DataTable();
            DataColumn myDataColumn;
            DataSet Ds = new DataSet();

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "AgentCode";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "AgentName";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "FlightNo";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "FromDate";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "ToDate";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "DayOfWeek";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "FreightWeight";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "FreightRate";
            myDataTable.Columns.Add(myDataColumn);

            //myDataColumn = new DataColumn();
            //myDataColumn.DataType = Type.GetType("System.String");
            //myDataColumn.ColumnName = "GSA";
            //myDataTable.Columns.Add(myDataColumn);

            //myDataColumn = new DataColumn();
            //myDataColumn.DataType = Type.GetType("System.String");
            //myDataColumn.ColumnName = "IsAllStn";
            //myDataTable.Columns.Add(myDataColumn);


            DataRow dr;
            dr = myDataTable.NewRow();
            dr["AgentCode"] = "";
            dr["AgentName"] = "";
            dr["FlightNo"] = "";
            dr["FromDate"] = "";
            dr["ToDate"] = "";
            dr["DayOfWeek"] = "";
            dr["FreightWeight"] = "";
            dr["FreightRate"] = "";
           

            myDataTable.Rows.Add(dr);

           gdvQuoteList.DataSource = null;
           gdvQuoteList.DataSource = myDataTable;
           gdvQuoteList.DataBind();
           Session["dsdata"] = myDataTable.Copy();

        }
        #endregion


        #region Get Org Dest
        protected void GetAirports()
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = da.SelectRecords("spGetAirportCodes");
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        ddlDest.DataSource = ds;
                        ddlDest.DataMember = ds.Tables[0].TableName;
                        ddlDest.DataValueField = ds.Tables[0].Columns["AirportCode"].ColumnName;
                        ddlDest.DataTextField = ds.Tables[0].Columns["Airport"].ColumnName;
                        ddlDest.DataBind();
                        ddlDest.Items.Insert(0, new ListItem("Select", "Select"));

                        ddlOrg.DataSource = ds;
                        ddlOrg.DataMember = ds.Tables[0].TableName;
                        ddlOrg.DataValueField = ds.Tables[0].Columns["AirportCode"].ColumnName;
                        ddlOrg.DataTextField = ds.Tables[0].Columns["Airport"].ColumnName;
                        ddlOrg.DataBind();
                        ddlOrg.Items.Insert(0, new ListItem("Select", "Select"));
                    }
                }
            }
        }
        #endregion

        #region GetAgentCode
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetAgentCode(string prefixText, int count)
        {

            //string[] orgdest = new ConBooking().GetOrgDest();

            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string");
            //SqlDataAdapter dad = new SqlDataAdapter("SELECT AgentCode from dbo.AgentMaster where (AgentName like '" + prefixText + "%' or AgentCode like '" + prefixText + "%') and Station='" + orgdest[0] + "'", con);
            SqlDataAdapter dad = new SqlDataAdapter("SELECT AgentCode from dbo.AgentMaster where (AgentName like '" + prefixText + "%' or AgentCode like '" + prefixText + "%') ", con);
            DataSet ds = new DataSet();
            dad.Fill(ds);
            List<string> list = new List<string>(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(dr[0].ToString());

            }

            return list.ToArray();
        }
        #endregion

        #region getFlights
        public void GetFlights()
        {
            try
            {
                SQLServer objSQL = new SQLServer(Global.GetConnectionString());
                DataSet dsInstance = new DataSet();
                //string FlightPrefix;
                dsInstance = objSQL.SelectRecords("GetCurrentInstance");
                string current = dsInstance.Tables[0].Rows[0][0].ToString();
              //  FlightPrefix = ddlFlightPrefix.SelectedValue.ToString().Trim();
                {
                    DataSet dsResult = new DataSet();
                    string errormessage = "";
                    //if (objBAL.GetAllFlightsNew(source, dest, date, ref dsResult, ref errormessage))
                    string procedure = "sp_GetFlightPrefix";
                    dsResult = objSQL.SelectRecords(procedure);
                    if (dsResult != null)
                    {
                        if (dsResult.Tables.Count > 0)
                        {
                            if (dsResult.Tables[0].Rows.Count > 0)
                            {
                               ddlFlightPrefix.Items.Clear();
                               ddlFlightPrefix.DataSource = dsResult.Tables[0];
                               ddlFlightPrefix.DataTextField = "PartnerCode";
                               ddlFlightPrefix.DataValueField = "PartnerCode";
                               ddlFlightPrefix.DataBind();
                           
                           ddlFlightPrefix.SelectedValue = current;
                           GetFlight(current);

                            }
                        }
                    }
                    else
                    {
                        lblStatus.Text = "" + errormessage;
                        lblStatus.ForeColor = Color.Red;
                    }
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = "" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }
        #endregion

        #region btnList_Click
        protected void btnList_Click(object sender, EventArgs e)
        {
            loadList();
        }
        #endregion

        #region loadList
        private bool loadList()
        {
            bool flag = false;
            try
            {
                SQLServer objSQL = new SQLServer(BAL.Global.GetConnectionString());
                string procedure = "spGetAgentCapacityList";

                string[] paramname = new string[] {"AgentCode","FlightNo","Origin","Destination"};

                object[] paramvalue = new object[] {TXTAgentCode.Text.Trim(),ddlFlight.SelectedItem.Text.Trim(),ddlOrg.SelectedValue.ToString().Trim(),ddlDest.SelectedValue.ToString().Trim()};

                SqlDbType[] paramtype = new SqlDbType[] { SqlDbType.VarChar,SqlDbType.VarChar ,SqlDbType.VarChar ,SqlDbType.VarChar };

                DataSet dsData = new DataSet();
                dsData = objSQL.SelectRecords(procedure, paramname, paramvalue, paramtype);
                if (dsData != null)
                {
                    if (dsData.Tables.Count > 0)
                    {
                        if (dsData.Tables[0].Rows.Count > 0)
                        {
                            gdvQuoteList.DataSource = null;
                            gdvQuoteList.DataSource = dsData.Tables[0];
                            gdvQuoteList.DataBind();
                            Session["dsdata"] = dsData.Tables[0];
                            flag = true;
                            lblStatus.Text = string.Empty;
                        }
                        else
                        {
                            lblStatus.Text = "No data found for given criteria";
                            lblStatus.ForeColor = Color.Red;
                            ddlOrg.Focus();
                            Session["dsdata"] = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
            return flag;
        }
        #endregion

        #region btnBooking_Click
        protected void btnBooking_Click(object sender, EventArgs e)
        {
        }
        #endregion

        #region btnClear_Click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlFlight.SelectedIndex = 0;
            TXTAgentCode.Text = "";
            gdvQuoteList.DataSource = null;
            gdvQuoteList.DataBind();
            ddlOrg.SelectedIndex = ddlDest.SelectedIndex = 0;
            LoadGridUserList();
        }
        #endregion

        protected void gdvQuoteList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvQuoteList.PageIndex = e.NewPageIndex;
            gdvQuoteList.DataSource = (DataTable)Session["dsdata"];
            gdvQuoteList.DataBind();
        }

        protected void ddlDest_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDest.SelectedIndex == 0)
            {
                GetFlights();
            }
            else
            {
                if (ddlOrg.SelectedIndex == 0)
                { return; }
                else
                {
                    string source = ddlOrg.SelectedItem.Text.Trim();
                    string dest = ddlDest.SelectedItem.Text.Trim();
                    DataSet dsResult = new DataSet();
                    string errormessage = "";
                    if (objBAL.GetAllFlightsNew(source, dest, "", ref dsResult, ref errormessage))
                    {

                        if (dsResult != null)
                        {
                            if (dsResult.Tables.Count > 0)
                            {
                                if (dsResult.Tables[0].Rows.Count > 0)
                                {
                                    ddlFlight.Items.Clear();
                                    ddlFlight.DataSource = dsResult.Tables[0];
                                    ddlFlight.DataTextField = "FltNumber";
                                    ddlFlight.DataValueField = "DeptTime";
                                    ddlFlight.DataBind();
                                    ddlFlight.Items.Insert(0, new ListItem("Select", ""));
                                    ddlFlight.SelectedIndex = -1;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void GetFlight(string FlightPrefix)
        {

            try
            {
                DataSet dsResult = new DataSet();

                if (ddlFlightPrefix.SelectedItem.Value.ToString() == "Select")
                {
                    ddlFlight.DataSource = "";
                    ddlFlight.DataBind();
                }
                else
                {

                    SQLServer objSQL = new SQLServer(Global.GetConnectionString());
                    dsResult = objSQL.SelectRecords("spGetAllFlightListPrefixWise", "FlightPrefix", FlightPrefix, SqlDbType.VarChar);
                    if (dsResult != null)
                    {
                        if (dsResult.Tables.Count > 0)
                        {
                            if (dsResult.Tables[0].Rows.Count > 0)
                            {
                                ddlFlight.Items.Clear();
                                ddlFlight.DataSource = dsResult.Tables[0];
                                ddlFlight.DataTextField = "FlightID";
                                ddlFlight.DataValueField = "FlightID";
                                ddlFlight.DataBind();
                                ddlFlight.Items.Insert(0, new ListItem("Select", ""));
                                ddlFlight.SelectedIndex = -1;
                                lblStatus.Text = string.Empty;
                            }

                            else
                            {
                                ddlFlight.Items.Clear();
                                lblStatus.Text = "No Flight for this Partner";
                                lblStatus.ForeColor = Color.Red;
                                ddlFlight.DataSource = null;
                                ddlFlight.DataBind();
                                ddlFlight.Items.Insert(0, new ListItem("Select", null));
                            }
                        }
                    }

                }




            }
            catch (Exception)
            {


            }
        }

        protected void ddlFlightPrefix_SelectedIndexChanged(object sender, EventArgs e)
        {

             string FlightPrefix = ddlFlightPrefix.SelectedItem.Value.ToString();


             GetFlight(FlightPrefix);
        }
    }
}