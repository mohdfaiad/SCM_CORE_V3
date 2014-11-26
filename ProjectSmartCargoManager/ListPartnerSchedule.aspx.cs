using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Data.SqlClient;

namespace ProjectSmartCargoManager
{
    public partial class ListPartnerSchedule : System.Web.UI.Page
    {
        #region Variables
        AirlineScheduleBAL OBJasb = new AirlineScheduleBAL();
        DateTime fromdate = new DateTime();
        DateTime todate = new DateTime();
        ArrayList arFlight = new ArrayList();
        ArrayList arOrigin = new ArrayList();
        public static string CheckFromDate;
        public static string CheckToDate;
        static int list = 0;

        static int rowind;
        static int scheduleid;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                try
                {
                    txtFlightFromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtFlightToDate.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");

                    // getFligtNo();
                    FillPartnerType();            //Filling Partner Type to DropDownList
                    OriginList();
                    DestinationList();
                    AirCraftType();
                    AirCraftTypeInEdit();
                    //getFligtNoSourceDestWise("All", "All");
                    LoadOrigin();
                }
                catch (Exception ex)
                {

                }
            }
        }

        #region Autopopulate GetFlightId

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string[] GetFlightId(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string");
            // SqlDataAdapter dad = new SqlDataAdapter("select distinct flightid from PartnerSchedule where FlightID  like '" + prefixText +  "%'",con);
            SqlDataAdapter dad = new SqlDataAdapter("select flightid from(select  distinct flightid,convert(int, substring( REPLACE(flightid, '*', ''),3,len(REPLACE(flightid, '*', '')))) as FlightNo from PartnerSchedule where FlightID!='' and FlightID  like '" + prefixText + "%')s order by FlightNo ASC", con);
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

        #region AirCraft type List in gridview

        private void AirCraftTypeinGridview()
        {
            try
            {

                DataSet ds = OBJasb.GetAirCraftType();

                for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
                {
                    DropDownList ddlAircraft = (DropDownList)grdScheduleinfo.Rows[i].FindControl("ddlAirCraft");
                    if (ds != null)
                    {
                        if (ds.Tables != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                //Aircraft in Gridview Dropdown
                                DataRow row = ds.Tables[0].NewRow();

                                //row[ds.Tables[0].Columns[0].ColumnName] = "Select";
                                //ds.Tables[0].Rows.Add(row);


                                ddlAircraft.DataSource = ds;
                                ddlAircraft.DataMember = ds.Tables[0].TableName;
                                ddlAircraft.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                                ddlAircraft.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                                ddlAircraft.DataBind();
                                //ddlAircraft.Text = "Select";


                                DataSet dsCapacity = OBJasb.GetCargoCapacity(ds.Tables[0].Rows[0][0].ToString());
                                if (dsCapacity != null)
                                {
                                    if (dsCapacity.Tables != null)
                                    {
                                        if (dsCapacity.Tables.Count > 0 || dsCapacity.Tables[0].Rows.Count > 0)
                                        {
                                            txtCargoCapacity.Text = dsCapacity.Tables[0].Rows[0][0].ToString();
                                            //txtCapacity
                                            ((TextBox)grdScheduleinfo.Rows[i].FindControl("txtCapacity")).Text = dsCapacity.Tables[0].Rows[0][0].ToString();
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion Aircraft type List in gridview

        #region btnList_Click
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    //Validate from and to dates.
                    if (txtFlightFromdate.Text == "" || txtFlightToDate.Text == "")
                    {
                        lblStatus.Text = "Please select valid From and To Dates";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }

                    if (txtFlightFromdate.Text.Trim() != "" || txtFlightToDate.Text.Trim() != "")
                    {
                        DateTime dt1 = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                        DateTime dt2 = DateTime.ParseExact(txtFlightToDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                        int chk = DateTime.Compare(dt1, dt2);
                        if (chk > 0)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Please enter valid To date";
                            txtFromdate.Focus();
                            return;
                        }


                    }
                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid date format e.g.: dd/MM/yyyy";
                    txtFromdate.Focus();
                    return;
                }

                if (txtFlightNo.Text == "")
                {
                    //Validate date difference based on database configuration.
                    string strResult = "";
                    strResult = GetScheduleInterval(DateTime.ParseExact(txtFlightFromdate.Text.Trim(), "dd/MM/yyyy", null),
                        DateTime.ParseExact(txtFlightToDate.Text.Trim(), "dd/MM/yyyy", null));
                    if (strResult != "")
                    {
                        lblStatus.Text = strResult;
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                }

                if (chkDomestic.Checked == false && chkInternational.Checked == false)
                {
                    lblStatus.Text = "Kindly Check atleast one from Domestic and Inernational";
                    return;
                }

                if (chkDomestic.Checked == false && chkInternational.Checked == false)
                {
                    lblStatus.Text = "Kindly Check atleast one from Domestic and Inernational";
                    return;
                }


                btnEdit.Visible = false;
                pnlUpdate.Visible = false;
                pnlDestDetails.Visible = false;
                pnlMultiple.Visible = false;
                scheduleid = 0;
                lblStatus.Text = "";
                //if (ddlFlight.SelectedItem.Text == "All") mod
                //{
                //    LoadGridSchedule();
                //}
                //else
                //{
                //    LoadGridFlight();
                //}
                if (txtFlightNo.Text.Trim() == "")
                    LoadGridSchedule();
                else
                    LoadGridFlight();
                LoadSourceInGridview();
                // DestinationList(); 
                LoadDestinationInGridview();

                AirCraftTypeinGridview();
                AddRowToGrid();

                btnEdit.Enabled = true;
            }
            catch (Exception ex)
            {
            }
        }
        #endregion btnList_Click

        #region Get Schedule Interval
        public string GetScheduleInterval(DateTime FromDate, DateTime ToDate)
        {
            double DaysConfigured = 0;
            double intReportInterval = (ToDate - FromDate).TotalDays + 1;
            LoginBL objBL = new LoginBL();

            try
            {
                string strOutput = "";
                if (Session["PartnerSchInterval"] == null)
                {
                    strOutput = objBL.GetMasterConfiguration("PartnerSchInterval");
                    Session["PartnerSchInterval"] = strOutput;
                }
                else
                {
                    strOutput = Session["PartnerSchInterval"].ToString();
                }

                if (strOutput != "")
                    DaysConfigured = Convert.ToDouble(strOutput);
                else
                    DaysConfigured = 0;
            }
            catch
            {
                DaysConfigured = 0;
            }
            finally
            {
                if (objBL != null)
                    objBL = null;
            }

            if (DaysConfigured > 0 && intReportInterval > DaysConfigured)
                return "Schedule can be listed only for " + DaysConfigured.ToString() + " days if Flight # is not specified.";
            else
                return "";
        }
        #endregion Get Schedule Interval

        #region LoadSource in Gridview Dropdown With Form State
        public void LoadSourceInGridviewState()
        {
            try
            {
                ArrayList arCheckOrigin = new ArrayList();

                DataSet ds = OBJasb.GetOriginList("");
                DropDownList ddl = new DropDownList();
                TextBox txtDeptDay = new TextBox();
                TextBox txtDepthr = new TextBox();
                TextBox txtDeptMin = new TextBox();
                TextBox txtArrDay = new TextBox();
                TextBox txtArrhr = new TextBox();
                TextBox txtArrMin = new TextBox();
                DropDownList ddlTempDest = new DropDownList();

                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
                {
                    ddl = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlFromOrigin")));

                    ddlTempDest = ((DropDownList)grdScheduleinfo.Rows[i].Cells[1].FindControl("ddlToDest"));

                    txtDeptDay = ((TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptDay")));
                    txtDepthr = ((TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeHr")));
                    txtDeptMin = ((TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeMin")));
                    txtArrDay = ((TextBox)(grdScheduleinfo.Rows[i].FindControl("txtArrivalDay")));
                    txtArrhr = ((TextBox)(grdScheduleinfo.Rows[i].FindControl("txtArrivaltimeHr"))); ;
                    txtArrMin = ((TextBox)(grdScheduleinfo.Rows[i].FindControl("txtArrivalTimeMin"))); ;

                    if (ds != null)
                    {
                        //Source in Gridview Dropdown
                        DataRow row = ds.Tables[0].NewRow();
                        if (!arCheckOrigin.Contains("Select"))
                        {
                            row[ds.Tables[0].Columns[0].ColumnName] = "Select";
                            ds.Tables[0].Rows.Add(row);
                            arCheckOrigin.Add("Select");


                        }

                        ddl.DataSource = ds;
                        ddl.DataMember = ds.Tables[0].TableName;
                        ddl.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                        ddl.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                        ddl.DataBind();
                        if (dtCurrentTable.Rows[i][3].ToString() != "")
                        {
                            ((Label)grdScheduleinfo.Rows[i].FindControl("lblFlight")).Text = dtCurrentTable.Rows[i][0].ToString(); ;

                            ddl.Text = dtCurrentTable.Rows[i][1].ToString();
                            txtDeptDay.Text = dtCurrentTable.Rows[i][3].ToString();
                            string Depttime = dtCurrentTable.Rows[i][4].ToString();
                            string[] Depttime1 = Depttime.Split(':');
                            txtDepthr.Text = Depttime1[0].ToString();//Depttime.Split(':');
                            txtDeptMin.Text = Depttime1[1].ToString();
                            string Arrtime = dtCurrentTable.Rows[i][6].ToString();
                            string[] Arrtime1 = Arrtime.Split(':');

                            txtArrDay.Text = dtCurrentTable.Rows[i][5].ToString();
                            txtArrhr.Text = Arrtime1[0].ToString();
                            txtArrMin.Text = Arrtime1[1].ToString();
                        }
                        ((DropDownList)grdScheduleinfo.Rows[i].FindControl("ddlStatus")).Text = dtCurrentTable.Rows[i][16].ToString();
                        // ddl.Text = "Select";
                        if (dtCurrentTable.Rows[i][3].ToString() != "")
                        {
                            if (dtCurrentTable.Rows.Count > 1)
                            {

                                if (dtCurrentTable.Rows[i][1].ToString() != "select" && dtCurrentTable.Rows[i][1].ToString() != "")
                                {
                                    ddl.Text = dtCurrentTable.Rows[i][1].ToString();

                                    //txtDepthr.Text = dtCurrentTable.Rows[i][2].ToString();
                                    //txtDeptMin.Text = dtCurrentTable.Rows[i][3].ToString();
                                    //txtArrhr.Text = dtCurrentTable.Rows[i][4].ToString();



                                }
                                else
                                {
                                    ddl.Text = dtCurrentTable.Rows[i - 1][2].ToString();

                                    //ddl.Text = "Select";
                                }

                            }
                        }
                        else
                        {
                            ddl.Text = "Select";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

        }

        #endregion LoadSource Dropdown


        #region LoadSource in Gridview Dropdown
        public void LoadSourceInGridview()
        {
            try
            {
                DataSet ds = OBJasb.GetOriginListforPartner();
                DropDownList ddl = new DropDownList();

                for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
                {
                    ddl = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlFromOrigin")));
                    if (ds != null)
                    {
                        //Source in Gridview Dropdown
                        DataRow row = ds.Tables[0].NewRow();

                        row[ds.Tables[0].Columns[0].ColumnName] = "Select";
                        ds.Tables[0].Rows.Add(row);

                        ddl.DataSource = ds;
                        ddl.DataMember = ds.Tables[0].TableName;
                        ddl.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                        ddl.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                        ddl.DataBind();
                        //ddl.Text = "Select";
                        ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByText(ds.Tables[0].Rows[i]["Source"].ToString()));
                    }
                    
                }
            }
            catch (Exception ex)
            {
            }

        }

        #endregion LoadSource Dropdown

        #region Loadgrid SscheduleInfo Intial Row
        private void LoadGridSchedule()
        {
            try
            {
                DataTable myDataTable = new DataTable();
                DataColumn myDataColumn;
                DataSet Ds = new DataSet();

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "RowNumber";
                myDataTable.Columns.Add(myDataColumn);

                //myDataColumn = new DataColumn();
                //myDataColumn.DataType = Type.GetType("System.String");
                //myDataColumn.ColumnName = "FlightID";
                //myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "From";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "To";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "DeptDay";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Dept TimeHr";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Dept TimeMin";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ArrDay";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Arrival TimeHr";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Arrival TimeMin";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "chkMon";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "chkTues";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "chkwed";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "chkThur";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "chkFri";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "chkSat";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "chkSun";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "AircraftType";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Capacity";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Status";
                myDataTable.Columns.Add(myDataColumn);




                DataRow dr;
                dr = myDataTable.NewRow();
                dr["RowNumber"] = 1;
                dr["From"] = "select";//"5";
                dr["To"] = "";// "5";
                dr["Deptday"] = "";

                dr["Dept TimeHr"] = "";
                dr["Dept TimeMin"] = "";
                dr["Arrday"] = "";
                dr["Arrival TimeHr"] = "";// "9";
                dr["Arrival TimeMin"] = "";


                dr["chkMon"] = "";
                dr["chkTues"] = "";
                dr["chkwed"] = "";
                dr["chkThur"] = "";
                dr["chkFri"] = "";
                dr["chkSat"] = "";
                dr["chkSun"] = "";
                dr["AircraftType"] = "";
                dr["Capacity"] = "";

                dr["Status"] = "";
                // DateTime Flightfromdate = new DateTime();
                // DateTime FlightToDate = new DateTime();
                string Flightfromdate = "", FlightToDate = "";
                try
                {
                    if (txtFlightFromdate.Text != "")
                    {
                        //  Flightfromdate = Convert.ToDateTime(txtFlightFromdate.Text).ToString("dd/MM/yyyy");
                        Flightfromdate = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null).ToString();
                        //   Convert.ToDateTime(txtFromdate.Text).ToString("yyyy-MM-dd HH:mm:ss")
                    }
                    if (txtFlightToDate.Text != "")
                    {
                        // FlightToDate = Convert.ToDateTime(txtFlightToDate.Text).ToString("dd/MM/yyyy");
                        FlightToDate = DateTime.ParseExact(txtFlightToDate.Text, "dd/MM/yyyy", null).ToString();

                    }
                }
                catch (Exception ex)
                {

                }
                string FlightNo = "";
                // FlightNo = ddlFlight.SelectedValue;

                //FlightNo = ddlFlight.SelectedItem.Text; mod


                if (txtFlightNo.Text == "")
                    FlightNo = "All";
                else
                    FlightNo = txtFlightNo.Text;

                DataSet ds = new DataSet();
                //if (ddlFlight.SelectedItem.Text == "All") mod
                //{
                //    FlightNo = "All";
                //}
                string strdomestic = "";
                if (chkDomestic.Checked == true && chkInternational.Checked == true)
                {
                    strdomestic = "All";
                }
                else if (chkDomestic.Checked == true)
                {
                    strdomestic = "true";
                }
                else
                {
                    strdomestic = "False";

                }


                //  ds = OBJasb.GetAirlineSchedule(ddlOrigin.Text, ddlDestination.Text, FlightNo, ddlAirCraftType.Text, Flightfromdate, FlightToDate, ddlStatus.Text,strdomestic);


                myDataTable.Rows.Add(dr);
                ViewState["CurrentTable"] = myDataTable;//ds.Tables[0];
                //Bind the DataTable to the Grid

                grdScheduleinfo.DataSource = null;
                grdScheduleinfo.DataSource = myDataTable; // ds.Tables[0];
                grdScheduleinfo.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion LoadgridScheduleInfo Intial Row


        #region Loadgrid SscheduleInfo DateWise
        private void LoadGridScheduleDateWise()
        {
            try
            {
                DataTable myDataTable = new DataTable();
                DataColumn myDataColumn;
                DataSet Ds = new DataSet();

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "RowNumber";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Date";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "From";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "To";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "DeptDay";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Dept TimeHr";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Dept TimeMin";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ArrDay";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Arrival TimeHr";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Arrival TimeMin";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "chkMon";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "chkTues";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "chkwed";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "chkThur";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "chkFri";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "chkSat";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "chkSun";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "AircraftType";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Capacity";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Status";
                myDataTable.Columns.Add(myDataColumn);




                DataRow dr;
                dr = myDataTable.NewRow();
                dr["RowNumber"] = 1;
                dr["Date"] = "";
                dr["From"] = "select";//"5";
                dr["To"] = "";// "5";
                dr["Deptday"] = "";

                dr["Dept TimeHr"] = "";
                dr["Dept TimeMin"] = "";
                dr["Arrday"] = "";
                dr["Arrival TimeHr"] = "";// "9";
                dr["Arrival TimeMin"] = "";


                dr["chkMon"] = "";
                dr["chkTues"] = "";
                dr["chkwed"] = "";
                dr["chkThur"] = "";
                dr["chkFri"] = "";
                dr["chkSat"] = "";
                dr["chkSun"] = "";
                dr["AircraftType"] = "";
                dr["Capacity"] = "";

                dr["Status"] = "";
                // DateTime Flightfromdate = new DateTime();
                // DateTime FlightToDate = new DateTime();
                string Flightfromdate = "", FlightToDate = "";
                try
                {
                    if (txtFlightFromdate.Text != "")
                    {
                        //  Flightfromdate = Convert.ToDateTime(txtFlightFromdate.Text).ToString("dd/MM/yyyy");
                        Flightfromdate = DateTime.ParseExact(txtFromdate.Text, "dd/MM/yyyy", null).ToString();
                        //   Convert.ToDateTime(txtFromdate.Text).ToString("yyyy-MM-dd HH:mm:ss")
                    }
                    if (txtFlightToDate.Text != "")
                    {
                        // FlightToDate = Convert.ToDateTime(txtFlightToDate.Text).ToString("dd/MM/yyyy");
                        FlightToDate = DateTime.ParseExact(txtFlightToDate.Text, "dd/MM/yyyy", null).ToString();

                    }
                }
                catch (Exception ex)
                {

                }
                string FlightNo = "";
                // FlightNo = ddlFlight.SelectedValue;

                //FlightNo = ddlFlight.SelectedItem.Text; mod


                if (txtFlightNo.Text == "")
                    FlightNo = "All";
                else
                    FlightNo = txtFlightNo.Text;

                DataSet ds = new DataSet();
                //if (ddlFlight.SelectedItem.Text == "All") mod
                //{
                //    FlightNo = "All";
                //}
                string strdomestic = "";
                if (chkDomestic.Checked == true && chkInternational.Checked == true)
                {
                    strdomestic = "All";
                }
                else if (chkDomestic.Checked == true)
                {
                    strdomestic = "true";
                }
                else
                {
                    strdomestic = "False";

                }


                //  ds = OBJasb.GetAirlineSchedule(ddlOrigin.Text, ddlDestination.Text, FlightNo, ddlAirCraftType.Text, Flightfromdate, FlightToDate, ddlStatus.Text,strdomestic);


                myDataTable.Rows.Add(dr);
                ViewState["CurrentTable"] = myDataTable;//ds.Tables[0];
                //Bind the DataTable to the Grid

                //GridView1.DataSource = null;
                //GridView1.DataSource = myDataTable; // ds.Tables[0];
                //GridView1.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion LoadgridScheduleInfo Intial Row

        #region Loadgrid flight Info Intial Row
        private void LoadGridFlight()
        {
            try
            {
                DataTable myDataTable = new DataTable();
                DataColumn myDataColumn;
                DataSet Ds = new DataSet();

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "SrNo";
                myDataTable.Columns.Add(myDataColumn);

                //myDataColumn = new DataColumn();
                //myDataColumn.DataType = Type.GetType("System.String");
                //myDataColumn.ColumnName = "FlightID";
                //myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "FlightID";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "RouteID";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Source";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Dest";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "From Date";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ToDate";
                myDataTable.Columns.Add(myDataColumn);

                //myDataColumn = new DataColumn();
                //myDataColumn.DataType = Type.GetType("System.String");
                //myDataColumn.ColumnName = "Frequency";
                //myDataTable.Columns.Add(myDataColumn);


                //myDataColumn = new DataColumn();
                //myDataColumn.DataType = Type.GetType("System.String");
                //myDataColumn.ColumnName = "chkMon";
                //myDataTable.Columns.Add(myDataColumn);

                //myDataColumn = new DataColumn();
                //myDataColumn.DataType = Type.GetType("System.String");
                //myDataColumn.ColumnName = "chkTues";
                //myDataTable.Columns.Add(myDataColumn);

                //myDataColumn = new DataColumn();
                //myDataColumn.DataType = Type.GetType("System.String");
                //myDataColumn.ColumnName = "chkwed";
                //myDataTable.Columns.Add(myDataColumn);

                //myDataColumn = new DataColumn();
                //myDataColumn.DataType = Type.GetType("System.String");
                //myDataColumn.ColumnName = "chkThur";
                //myDataTable.Columns.Add(myDataColumn);

                //myDataColumn = new DataColumn();
                //myDataColumn.DataType = Type.GetType("System.String");
                //myDataColumn.ColumnName = "chkFri";
                //myDataTable.Columns.Add(myDataColumn);

                //myDataColumn = new DataColumn();
                //myDataColumn.DataType = Type.GetType("System.String");
                //myDataColumn.ColumnName = "chkSat";
                //myDataTable.Columns.Add(myDataColumn);


                //myDataColumn = new DataColumn();
                //myDataColumn.DataType = Type.GetType("System.String");
                //myDataColumn.ColumnName = "chkSun";
                //myDataTable.Columns.Add(myDataColumn);



                DataRow dr;
                dr = myDataTable.NewRow();
                dr["SrNo"] = 1;
                // "5";
                dr["FlightID"] = "";
                dr["RouteID"] = "";
                dr["Source"] = "";
                dr["Dest"] = "";
                dr["From Date"] = "";
                dr["ToDate"] = "";// "9";
                //dr["Frequency"] = "";

                //dr["chkMon"] = "";
                //dr["chkTues"] = "";
                //dr["chkwed"] = "";
                //dr["chkThur"] = "";
                //dr["chkFri"] = "";
                //dr["chkSat"] = "";
                //dr["chkSun"] = "";
                // DateTime Flightfromdate = new DateTime();
                // DateTime FlightToDate = new DateTime();
                string Flightfromdate = "", FlightToDate = "";

                //if (txtFlightFromdate.Text != "")
                //{
                //    Flightfromdate = Convert.ToDateTime(txtFlightFromdate.Text).ToString("yyyy-MM-dd HH:mm:ss");
                //    //   Convert.ToDateTime(txtFromdate.Text).ToString("yyyy-MM-dd HH:mm:ss")
                //}
                //if (txtFlightToDate.Text != "")
                //{
                //    FlightToDate = Convert.ToDateTime(txtFlightToDate.Text).ToString("yyyy-MM-dd HH:mm:ss");
                //}




                DateTime dt1 = new DateTime();
                DateTime dt2 = new DateTime();
                try
                {
                    if (txtFlightFromdate.Text != "")
                    {
                        // Flightfromdate = Convert.ToDateTime(txtFlightFromdate.Text).ToString("yyyy-MM-dd HH:mm:ss");
                        // Flightfromdate = Convert.ToDateTime(txtFlightFromdate.Text).ToString("dd/MM/yyyy");
                        //  dt1 = DateTime.Parse(txtFlightFromdate.Text);//.ToString("dd/MM/yyyy");
                        Flightfromdate = txtFlightFromdate.Text;
                        DateTime dtFromDate = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null);
                        Flightfromdate = dtFromDate.ToString("MM/dd/yyyy");
                    }
                    if (txtFlightToDate.Text != "")
                    {
                        //FlightToDate = Convert.ToDateTime(txtFlightToDate.Text).ToString("dd/MM/yyyy");
                        // dt2 = DateTime.Parse(txtFlightToDate.Text);//.ToString("dd/MM/yyyy");
                        dt2 = DateTime.ParseExact(txtFlightToDate.Text, "dd/MM/yyyy", null);
                        FlightToDate = dt2.ToString("MM/dd/yyyy");


                    }
                }
                catch (Exception ex)
                {

                }



                string FlightNo = "";
                // FlightNo = ddlFlight.SelectedValue;
                if (txtFlightNo.Text == "")
                    FlightNo = "All";
                else
                    FlightNo = txtFlightNo.Text;

                //FlightNo = ddlFlight.SelectedItem.Text;
                //if (ddlFlight.SelectedItem.Text == "")
                //{
                //    FlightNo = "All";
                //}
                string strdomestic = "";
                if (chkDomestic.Checked == true && chkInternational.Checked == true)
                {
                    strdomestic = "All";
                }
                else if (chkDomestic.Checked == true)
                {
                    strdomestic = "true";
                }
                else
                {
                    strdomestic = "False";

                }
                string PartnerCode = "";
                // FlightNo = ddlFlight.SelectedValue;

                if (ddlPartnerCode.SelectedValue != "Select")
                    PartnerCode = ddlPartnerCode.SelectedValue;

                DataSet ds = OBJasb.GetPartnerSchedule(ddlOrigin.Text, ddlDestination.Text, FlightNo, ddlAirCraftType.Text, Flightfromdate, 
                    FlightToDate, ddlStatus.Text, chkDomestic.Checked,chkInternational.Checked,PartnerCode);

                myDataTable.Rows.Add(dr);
                ViewState["FlightTable"] = ds.Tables[0];
                //Bind the DataTable to the Grid

                grdFlight.DataSource = null;
                grdFlight.DataSource = ds.Tables[0];
                grdFlight.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion LoadgridFlightInfo Intial Row

        #region LoadDest in Gridview Dropdown
        public void LoadDestinationInGridview()
        {
            try
            {
                DataSet ds = OBJasb.GetOriginListforPartner();
                DropDownList ddl = new DropDownList();

                for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
                {
                    ddl = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlToDest")));
                    if (ds != null)
                    {
                        //LoadDest in Gridview Dropdown 
                        DataRow row = ds.Tables[0].NewRow();

                        row[ds.Tables[0].Columns[0].ColumnName] = "Select";
                        ds.Tables[0].Rows.Add(row);

                        ddl.DataSource = ds;
                        ddl.DataMember = ds.Tables[0].TableName;
                        ddl.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                        ddl.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                        ddl.DataBind();
                        ddl.Text = "Select";
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        #endregion LoadDest Dropdown

        #region AddRow To Grid
        private void AddRowToGrid()
        {
            try
            {
                int rowIndex = 0;

                string Flightfromdate = "", FlightToDate = "";
                DateTime dt1 = new DateTime();
                DateTime dt2 = new DateTime();
                try
                {
                    if (txtFlightFromdate.Text != "")
                    {
                        Flightfromdate = txtFlightFromdate.Text;
                        DateTime dtFromDate = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null);
                        Flightfromdate = dtFromDate.ToString("MM/dd/yyyy");
                    }
                    if (txtFlightToDate.Text != "")
                    {
                        dt2 = DateTime.ParseExact(txtFlightToDate.Text, "dd/MM/yyyy", null);
                        FlightToDate = dt2.ToString("MM/dd/yyyy");
                    }
                }
                catch (Exception ex)
                {

                }

                string FlightNo = "";
                // FlightNo = ddlFlight.SelectedValue;

                //FlightNo = ddlFlight.SelectedItem.Text; mod
                if (txtFlightNo.Text == "")
                    FlightNo = "All";
                else
                    FlightNo = txtFlightNo.Text;

                // FlightNo = "All";
                string strdomestic = "";
                if (chkDomestic.Checked == true && chkInternational.Checked == true)
                {
                    strdomestic = "All";
                }
                else if (chkDomestic.Checked == true)
                {
                    strdomestic = "true";
                }
                else
                {
                    strdomestic = "False";

                }
                //Changes on 2 july for Autocoplete source
                // DataSet ds = OBJasb.GetAirlineSchedule(ddlOrigin.SelectedValue, ddlDestination.SelectedValue, FlightNo, ddlAirCraftType.SelectedValue, Flightfromdate, FlightToDate, ddlStatus.SelectedValue, strdomestic);
                string Source = "All", Dest = "All";
                if (ddlAutoSource.Text.Trim() != "" && ddlAutoSource.Text.Trim() != "Select")
                {
                    Source = ddlAutoSource.Text.Trim();
                }
                if (ddlAutoDest.Text.Trim() != "" && ddlAutoDest.Text.Trim() != "Select")
                {
                    Dest = ddlAutoDest.Text.Trim();
                }
                string PartnerCode = "";
                // FlightNo = ddlFlight.SelectedValue;

                if (ddlPartnerCode.SelectedValue != "Select")
                    PartnerCode = ddlPartnerCode.SelectedValue;

                DataSet ds = OBJasb.GetPartnerSchedule(Source, Dest, FlightNo, ddlAirCraftType.SelectedValue, Flightfromdate, FlightToDate, ddlStatus.SelectedValue, chkDomestic.Checked,chkInternational.Checked,PartnerCode);

                DataTable Dtemp = new DataTable();
                DataView view = new DataView();

                
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {

                                Dtemp = ds.Tables[0];
                                try
                                {

                                    Dtemp.Columns.Add("tab_index", typeof(float));


                                    for (int row = 0; row < Dtemp.Rows.Count; row++)
                                    {


                                        
                                        string Weekdays = Dtemp.Rows[row][7].ToString();
                                        string scheduleID = Dtemp.Rows[row]["ScheduleID"].ToString();
                                        string ActDeptTimr = Dtemp.Rows[row][4].ToString();
                                        //     string flightandsource = FlightID + dtCurrentTable.Rows[i][1].ToString();
                                        if (!arFlight.Contains(scheduleID))// + " " + Weekdays))//FlightID))//&& (!arOrigin.Contains(dtCurrentTable.Rows[i][1].ToString())))
                                        {
                                            //if ((!arOrigin.Contains(dtCurrentTable.Rows[i][1].ToString())))
                                            //{


                                            arFlight.Add(scheduleID);// + " " + Weekdays);
                                            ActDeptTimr = ActDeptTimr.Replace('/', '-');
                                            ActDeptTimr = ActDeptTimr.Replace(':', '.');
                                            // ActDeptTimr = Dtemp.Rows[row][4].ToString();
                                            //ActDeptTimr = ActDeptTimr.Replace('-', '/');
                                            //ActDeptTimr = ActDeptTimr.Replace('.', ':');
                                            Dtemp.Rows[row]["tab_index"] = ActDeptTimr;
                                            //  arFlight.Add(flightandsource);
                                            // arOrigin.Add(dtCurrentTable.Rows[i][1].ToString());
                                            //}
                                        }
                                        else
                                        {
                                            //ActDeptTimr = Dtemp.Rows[row-1][4].ToString();
                                            //ActDeptTimr = ActDeptTimr.Replace(':','.');
                                            ActDeptTimr = Dtemp.Rows[row - 1]["tab_index"].ToString();
                                            Dtemp.Rows[row]["tab_index"] = ActDeptTimr;
                                        }




                                    }

                                    arFlight.Clear();


                                    // Get the DefaultViewManager of a DataTable.
                                    // DataTable dt=Dtemp.Copy();

                                    // By default, the first column sorted ascending.
                                    view = Dtemp.DefaultView;
                                    view.Sort = "tab_index";
                                    //view.Sort = "FlightID";
                                    //Dtemp = view.ToTable();
                                    //   ds.Tables[0].DefaultView.Sort = "FlightID,SchDeptTime";

                                    // Dtemp.Select(@"GROUP BY [FlightID]");
                                }
                                catch (Exception ex)
                                {
                                }

                                if (txtFlightNo.Text == "") //ddlFlight.SelectedItem.Text == "All" mod)
                                {

                                    ViewState["CurrentTable"] = view.ToTable(); //Dtemp;//ds.Tables[0]; mod vikas 1Aug
                                    DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];

                                    grdScheduleinfo.DataSource = dtCurrentTable;
                                    grdScheduleinfo.DataBind();

                                    LoadSourceInGridview();
                                    LoadDestinationInGridview();
                                    AirCraftTypeinGridview();


                                    if (dtCurrentTable.Rows.Count > 0)
                                    {
                                        //extract the TextBox values
                                        if (FlightNo != "All")
                                        {
                                            pnlSchedule.Visible = true;

                                            pnlDestDetails.Visible = true;
                                            pnlDestDetails.Enabled = false;
                                            txtFromdate.Text = Convert.ToDateTime(dtCurrentTable.Rows[0][8]).ToString("yyyy-MM-dd");
                                            txtToDate.Text = Convert.ToDateTime(dtCurrentTable.Rows[0][9]).ToString("yyyy-MM-dd");
                                            txtCargoCapacity.Text = dtCurrentTable.Rows[0][12].ToString();
                                            ddlLoadAirCraftType.Text = dtCurrentTable.Rows[0][13].ToString();
                                            ddlOrigin1.Text = dtCurrentTable.Rows[0][14].ToString();
                                            ddlDestination0.Text = dtCurrentTable.Rows[0][15].ToString();
                                        }
                                        else
                                        {
                                            pnlDestDetails.Visible = false;
                                            pnlSchedule.Visible = true;
                                        }

                                        for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
                                        {
                                            //If grid row is greater than table row count then exit loop.
                                            if (i >= dtCurrentTable.Rows.Count)
                                                break;

                                            int j = grdScheduleinfo.PageIndex * grdScheduleinfo.PageSize + i;
                                            pnlMultiple.Visible = false;
                                            DropDownList ddlFromOrigin = new DropDownList();
                                            ddlFromOrigin = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlFromOrigin")));

                                            ddlFromOrigin.SelectedValue = dtCurrentTable.Rows[j][1].ToString();

                                            DropDownList ddlDest = new DropDownList();

                                            ddlDest = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlToDest")));

                                            ddlDest.SelectedValue = dtCurrentTable.Rows[j][2].ToString();

                                            string FlightID = dtCurrentTable.Rows[j][0].ToString();

                                            string CheckWeekdays = dtCurrentTable.Rows[j][7].ToString();

                                            Label lblFromdt = ((Label)grdScheduleinfo.Rows[i].FindControl("lblFromDate"));
                                            Label lblTodt = ((Label)grdScheduleinfo.Rows[i].FindControl("lblToDate"));
                                            lblFromdt.Text = DateTime.Parse(dtCurrentTable.Rows[j][8].ToString()).ToString("dd/MM/yyyy");

                                            lblTodt.Text = DateTime.Parse(dtCurrentTable.Rows[j][9].ToString()).ToString("dd/MM/yyyy");
                                            if (!arFlight.Contains(FlightID + " " + CheckWeekdays))//FlightID))//&& (!arOrigin.Contains(dtCurrentTable.Rows[i][1].ToString())))
                                            {
                                                Label lblFlight = ((Label)(grdScheduleinfo.Rows[i].FindControl("lblFlight")));
                                                lblFlight.Text = FlightID;
                                                arFlight.Add(FlightID + " " + CheckWeekdays);
                                            }

                                            ((TextBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("txtDeptDay")).Text = dtCurrentTable.Rows[j][3].ToString();

                                            string[] HrDept = dtCurrentTable.Rows[j][4].ToString().Split(':');

                                            ((TextBox)grdScheduleinfo.Rows[i].Cells[2].FindControl("txtDeptTimeHr")).Text = HrDept[0].PadLeft(2, '0').ToString();
                                            ((TextBox)grdScheduleinfo.Rows[i].Cells[2].FindControl("txtDeptTimeMin")).Text = HrDept[1].PadLeft(2, '0').ToString();

                                            string[] HrArr = dtCurrentTable.Rows[i][6].ToString().Split(':');

                                            ((TextBox)grdScheduleinfo.Rows[i].Cells[4].FindControl("txtArrivalDay")).Text = dtCurrentTable.Rows[j][5].ToString();

                                            ((TextBox)grdScheduleinfo.Rows[i].Cells[1].FindControl("txtArrivaltimeHr")).Text = HrArr[0].PadLeft(2, '0').ToString();
                                            ((TextBox)grdScheduleinfo.Rows[i].Cells[1].FindControl("txtArrivaltimeMin")).Text = HrArr[1].PadLeft(2, '0').ToString();

                                            string[] Weekdays = dtCurrentTable.Rows[j][7].ToString().Split(',');
                                            if (Weekdays[0] == "0")
                                            {
                                                ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkMon")).Checked = false;
                                            }
                                            if (Weekdays[1] == "0")
                                            {
                                                ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkTues")).Checked = false;
                                            }

                                            if (Weekdays[2] == "0")
                                            {
                                                ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkwed")).Checked = false;
                                            }

                                            if (Weekdays[3] == "0")
                                            {
                                                ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkThur")).Checked = false;
                                            }

                                            if (Weekdays[4] == "0")
                                            {
                                                ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkFri")).Checked = false;
                                            }

                                            if (Weekdays[5] == "0")
                                            {
                                                ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkSat")).Checked = false;
                                            }

                                            if (Weekdays[6] == "0")
                                            {
                                                ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkSun")).Checked = false;
                                            }

                                            ((TextBox)grdScheduleinfo.Rows[i].Cells[4].FindControl("txtCapacity")).Text = dtCurrentTable.Rows[j]["CargoCapacity"].ToString();
                                            
                                            string Isactive = dtCurrentTable.Rows[j][11].ToString();
                                            ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlStatus"))).SelectedValue = Isactive;
                                            
                                            DropDownList ddlAircraft = new DropDownList();
                                            ddlAircraft = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlAirCraft")));
                                            
                                            if (ddlAircraft.Items.IndexOf(new ListItem(dtCurrentTable.Rows[j][17].ToString(), 
                                                dtCurrentTable.Rows[j][17].ToString())) < 0)
                                            {
                                                ddlAircraft.Items.Add(new ListItem(dtCurrentTable.Rows[j][17].ToString(),
                                                dtCurrentTable.Rows[j][17].ToString()));
                                            }
                                            ddlAircraft.SelectedValue = dtCurrentTable.Rows[j][17].ToString();

                                            DropDownList ddlTailNo = new DropDownList();
                                            ddlTailNo = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlTailNo")));
                                            if (ddlTailNo.Items.IndexOf(new ListItem(dtCurrentTable.Rows[j][20].ToString(),
                                                dtCurrentTable.Rows[j][20].ToString())) < 0)
                                            {
                                                ddlTailNo.Items.Add(new ListItem(dtCurrentTable.Rows[j][20].ToString(),
                                                 dtCurrentTable.Rows[j][20].ToString()));
                                            }
                                            ddlTailNo.SelectedValue = dtCurrentTable.Rows[j][20].ToString();

                                        }

                                        ViewState["CurrentTable"] = dtCurrentTable;

                                    }
                                }
                                else
                                {
                                    ViewState["FlightTable"] = ds.Tables[0];
                                    DataTable dtFlightTable = (DataTable)ViewState["FlightTable"];
                                    for (int i = 0; i < grdFlight.Rows.Count; i++)
                                    {
                                        pnlMultiple.Visible = true;
                                        //extract the TextBox values
                                        pnlSchedule.Visible = false;
                                        pnlDestDetails.Visible = false;

                                        int j = grdFlight.PageIndex * grdFlight.PageSize + i;

                                        Label lblSrNo = new Label();
                                        lblSrNo = ((Label)(grdFlight.Rows[i].FindControl("lblSrNo")));
                                        lblSrNo.Text = dtFlightTable.Rows[j][0].ToString();

                                        Label lblRouteID = new Label();
                                        lblRouteID = ((Label)(grdFlight.Rows[i].FindControl("lblRouteID")));
                                        lblRouteID.Text = "Route " + (i + 1);
                                        
                                        string FlightID = dtFlightTable.Rows[j]["FlightID"].ToString();

                                        LinkButton lblFlight = ((LinkButton)(grdFlight.Rows[i].FindControl("lblFlightID")));
                                        lblFlight.Text = FlightID;
                                        arFlight.Add(FlightID);

                                        ((Label)grdFlight.Rows[i].FindControl("lblSource")).Text = dtFlightTable.Rows[j]["Source"].ToString();
                                        ((Label)grdFlight.Rows[i].FindControl("lblDest")).Text = dtFlightTable.Rows[j]["Dest"].ToString();
                                        ((Label)grdFlight.Rows[i].FindControl("lblFromDate")).Text = DateTime.Parse(dtFlightTable.Rows[j]["FromtDt"].ToString()).ToString("dd/MM/yyyy");
                                        ((Label)grdFlight.Rows[i].FindControl("lblToDate")).Text = DateTime.Parse(dtFlightTable.Rows[j]["ToDt"].ToString()).ToString("dd/MM/yyyy");

                                    }
                                    ViewState["FlightTable"] = dtFlightTable;
                                }
                            }
                            else
                            {
                                pnlSchedule.Visible = false;
                                lblStatus.ForeColor = Color.Brown;
                                lblStatus.Text = "Schedule Not Available for selected criteria.";
                                pnlSchedule.Visible = false;
                            }
                        }
                        else
                        {
                            pnlSchedule.Visible = false;
                        }
                    }
                }


                //Set Previous Data on Postbacks
                // SetPreviousData();
            }
            catch (Exception ex)
            {
            }

        }
        #endregion Add new Row To Grid



        #region Add datewise dataTo Grid
        private void AddDateWiseRowToGrid()
        {
            try
            {
                int rowIndex = 0;

                string Flightfromdate = "", FlightToDate = "";
                DateTime dt1 = new DateTime();
                DateTime dt2 = new DateTime();
                try
                {
                    if (txtFlightFromdate.Text == "" || txtFlightToDate.Text == "")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please Insert From date and To date for datewise Schedule List";
                        txtFlightFromdate.Focus();
                        return;
                    }

                    if (txtFlightFromdate.Text != "")
                    {
                        // Flightfromdate = Convert.ToDateTime(txtFlightFromdate.Text).ToString("yyyy-MM-dd HH:mm:ss");
                        // Flightfromdate = Convert.ToDateTime(txtFlightFromdate.Text).ToString("dd/MM/yyyy");
                        //  dt1 = DateTime.Parse(txtFlightFromdate.Text);//.ToString("dd/MM/yyyy");
                        Flightfromdate = txtFlightFromdate.Text;
                        //DateTime dtFromDate = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null);
                        dt1 = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null);

                        Flightfromdate = dt1.ToString("MM/dd/yyyy");
                    }
                    if (txtFlightToDate.Text != "")
                    {
                        //FlightToDate = Convert.ToDateTime(txtFlightToDate.Text).ToString("dd/MM/yyyy");
                        // dt2 = DateTime.Parse(txtFlightToDate.Text);//.ToString("dd/MM/yyyy");
                        dt2 = DateTime.ParseExact(txtFlightToDate.Text, "dd/MM/yyyy", null);
                        FlightToDate = dt2.ToString("MM/dd/yyyy");


                    }
                }
                catch (Exception ex)
                {

                }


                string FlightNo = "";
                // FlightNo = ddlFlight.SelectedValue;

                //FlightNo = ddlFlight.SelectedItem.Text; mod
                if (txtFlightNo.Text == "")
                    FlightNo = "All";
                else
                    FlightNo = txtFlightNo.Text;

                // FlightNo = "All";
                string strdomestic = "";
                if (chkDomestic.Checked == true && chkInternational.Checked == true)
                {
                    strdomestic = "All";
                }
                else if (chkDomestic.Checked == true)
                {
                    strdomestic = "true";
                }
                else
                {
                    strdomestic = "False";

                }
                //Changes on 2 july for Autocoplete source
                // DataSet ds = OBJasb.GetAirlineSchedule(ddlOrigin.SelectedValue, ddlDestination.SelectedValue, FlightNo, ddlAirCraftType.SelectedValue, Flightfromdate, FlightToDate, ddlStatus.SelectedValue, strdomestic);
                string Source = "All", Dest = "All";
                if (ddlAutoSource.Text.Trim() != "" && ddlAutoSource.Text.Trim() !="Select")
                {
                    Source = ddlAutoSource.Text.Trim();
                }
                if (ddlAutoDest.Text.Trim() != "" && ddlAutoDest.Text.Trim() != "Select")
                {
                    Dest = ddlAutoDest.Text.Trim();
                }

                DataSet ds = OBJasb.GetDateWiseAirlineSchedule(Source, Dest, FlightNo, ddlAirCraftType.SelectedValue, Flightfromdate, FlightToDate, ddlStatus.SelectedValue, strdomestic);

                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                Session["DatewiseSchedule"] = "";
                                Session["FromDate"] = "";
                                Session["ToDate"] = "";
                                Session["DatewiseSchedule" + list] = ds;
                                Session["FromDate"] = Flightfromdate;
                                Session["ToDate"] = FlightToDate;




                                //  string query = "'ShowEMAWBULD.aspx?ID=" + m + "'";

                                //Response.Write("<script>");
                                //Response.Write("window.open('ShowDateWiseSchedule.aspx','_blank','left=0,top=0,width=1024px,height=500px,toolbar=0,resizable=0')");
                                //Response.Write("</script>");
                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('ShowDateWiseSchedule.aspx?ID=" + list + "','_blank','left=0,top=0,width=1024px,height=500px,toolbar=0,resizable=0');", true);
                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "newwindow.focus();window.location.reload( True );", true);

                                list = list + 1;
                                // Response.Redirect("ShowDateWiseSchedule.aspx",false);
                                return;

                            }
                            else
                            {
                                pnlSchedule.Visible = false;
                                lblStatus.ForeColor = Color.Brown;
                                lblStatus.Text = "Schedule Not Available for selected criteria.";
                                pnlSchedule.Visible = false;
                            }
                        }
                        else
                        {
                            pnlSchedule.Visible = false;
                        }
                    }
                }


                //Set Previous Data on Postbacks
                // SetPreviousData();
            }
            catch (Exception ex)
            {
            }

        }
        #endregion Add datewise dataTo Grid

        #region AirCraft type List

        private void AirCraftType()
        {
            try
            {

                DataSet ds = OBJasb.GetAirCraftType();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            //Aircraft in Gridview Dropdown
                            DataRow row = ds.Tables[0].NewRow();

                            row[ds.Tables[0].Columns[0].ColumnName] = "All";
                            ds.Tables[0].Rows.Add(row);


                            ddlAirCraftType.DataSource = ds;
                            ddlAirCraftType.DataMember = ds.Tables[0].TableName;
                            ddlAirCraftType.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            ddlAirCraftType.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                            ddlAirCraftType.DataBind();
                            ddlAirCraftType.Text = "All";
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion AirCraft List

        #region AirCraftType In Edit type List

        private void AirCraftTypeInEdit()
        {
            try
            {

                DataSet ds = OBJasb.GetAirCraftType();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            //Aircraft type in Gridview Dropdown

                            DataRow row = ds.Tables[0].NewRow();

                            row[ds.Tables[0].Columns[0].ColumnName] = "Select";
                            ds.Tables[0].Rows.Add(row);

                            ddlLoadAirCraftType.DataSource = ds;
                            ddlLoadAirCraftType.DataMember = ds.Tables[0].TableName;
                            ddlLoadAirCraftType.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            ddlLoadAirCraftType.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                            ddlLoadAirCraftType.DataBind();
                            ddlLoadAirCraftType.Text = "Select";
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion AirCraft List

        #region SetPreviousData
        private void SetPreviousData()
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        LoadSourceInGridview();
                        LoadDestinationInGridview();
                        //Fill the DropDownList with Data

                        DropDownList ddlFromOrigin = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlFromOrigin")));

                        ddlFromOrigin.Text = dt.Rows[i][1].ToString();

                        //DropDownList ddlDest = new DropDownList();

                        //ddlDest = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlToDest")));

                        //ddlDest.SelectedValue = dtCurrentTable.Rows[i][2].ToString();

                        //string[] HrDept = dtCurrentTable.Rows[i][3].ToString().Split(':');

                        //((TextBox)grdScheduleinfo.Rows[i].Cells[2].FindControl("txtDeptTimeHr")).Text = HrDept[0];
                        //((TextBox)grdScheduleinfo.Rows[i].Cells[2].FindControl("txtDeptTimeMin")).Text = HrDept[1];

                        //string[] HrArr = dtCurrentTable.Rows[i][4].ToString().Split(':');


                        //((TextBox)grdScheduleinfo.Rows[i].Cells[1].FindControl("txtArrivaltimeHr")).Text = HrArr[0];
                        //((TextBox)grdScheduleinfo.Rows[i].Cells[1].FindControl("txtArrivaltimeMin")).Text = HrArr[1];

                        //string[] Weekdays = dtCurrentTable.Rows[i][5].ToString().Split(',');
                        //if (Weekdays[0] == "0")
                        //{
                        //    ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkMon")).Checked = false;
                        //}
                        //if (Weekdays[1] == "0")
                        //{
                        //    ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkTues")).Checked = false;
                        //}

                        //if (Weekdays[2] == "0")
                        //{
                        //    ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkwed")).Checked = false;
                        //}

                        //if (Weekdays[3] == "0")
                        //{
                        //    ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkThur")).Checked = false;
                        //}

                        //if (Weekdays[4] == "0")
                        //{
                        //    ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkFri")).Checked = false;
                        //}

                        //if (Weekdays[5] == "0")
                        //{
                        //    ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkSat")).Checked = false;
                        //}

                        //if (Weekdays[6] == "0")
                        //{
                        //    ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkSun")).Checked = false;
                        //}

                        //DropDownList ddlIsActive = new DropDownList();

                        //ddlIsActive = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlIsActive")));

                        //string Isactive = dtCurrentTable.Rows[i][9].ToString();
                        //if (Isactive == "True")
                        //{
                        //    ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlIsActive"))).SelectedValue = "IsActive";
                        //    //ddlIsActive.SelectedValue ="IsActive";
                        //}
                        //else
                        //{
                        //    ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlIsActive"))).SelectedValue = "InActive";
                        //    //ddlIsActive.SelectedValue = "InActive";
                        //}




                        rowIndex++;

                    }
                }
            }


        }
        #endregion SetPreviousData



        #region Validate Data
        /// <summary>
        /// Validate data entered by user.
        /// </summary>
        /// <returns>Returns True if valid data is entered.</returns>
        private bool ValidateData()
        {
            try
            {
                if (ddlLoadAirCraftType.Text == "Select")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please select AirCraft Type";
                    ddlLoadAirCraftType.Focus();
                    return false;
                }

                if (txtCargoCapacity.Text.Trim() == "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Enter Cargo Capacity of Flight";
                    txtCargoCapacity.Focus();
                    return false;
                }


                if (txtFromdate.Text == "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please select valid From date";
                    txtFromdate.Focus();
                    return false;
                }
                if (txtToDate.Text == "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please select valid To date";
                    txtToDate.Focus();
                    return false;
                }


                // if ((Convert.ToDateTime(txtFromdate.Text).ToString("dd-MM-yyyy")) > (Convert.ToDateTime(txtToDate.Text).ToString("dd-MM-yyyy")))
                DateTime dt1 = DateTime.ParseExact(txtFromdate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                DateTime dt2 = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                int chk = DateTime.Compare(dt1, dt2);
                if (chk > 0)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid To date";
                    txtFromdate.Focus();
                    return false;
                }


                //  CheckFromDate 
                //       CheckToDate = Co

                DateTime CheckFromDate1 = Convert.ToDateTime(CheckFromDate.ToString());//DateTime.Parse((DateTime.Parse(CheckFromDate.ToString())).ToString("dd/MM/yyyy"));//DateTime.ParseExact(CheckFromDate.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat); //DateTime.Parse(txtToDate.Text);
                DateTime CheckToDate1 = Convert.ToDateTime(CheckToDate.ToString());

                //DateTime.ParseExact(DateTime.Parse(CheckToDate),"dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);

                int chkfromdate = DateTime.Compare(CheckFromDate1, dt1);
                if (chkfromdate > 0)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid from date";
                    txtFromdate.Focus();
                    return false;
                }


                int chtodate = DateTime.Compare(dt2, CheckToDate1);
                if (chtodate > 0)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid To date";
                    txtFromdate.Focus();
                    return false;
                }


                // if (Convert.ToDateTime(txtFromdate.Text) > Convert.ToDateTime(txtToDate.Text))
                //{
                //     lblStatus.ForeColor = Color.Red;
                //     lblStatus.Text = "Please select valid From date";
                //     txtFromdate.Focus();
                //     return false;
                // }

                if (txtFlightNo.Text == "")//ddlFlight.SelectedItem.Text == "") mod
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please select flight Code";
                    txtFlightNo.Focus();
                    // ddlFlight.Focus();mod
                    return false;
                }
            }
            catch (Exception ex)
            {

            }
            try
            {

                DropDownList ValidateSource = (DropDownList)(grdScheduleinfo.Rows[0].FindControl("ddlFromOrigin"));

                if (ValidateSource.SelectedValue != ddlOrigin1.Text)//txtSource.Text)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Enter Valid Source in Route";
                    ValidateSource.Focus();
                    return false;

                }
            }
            catch (Exception ex)
            {
            }

            //try
            //{
            //    DataSet dsCheck = OBJasb.GetAirlineScheduleforflight(ddlOrigin.SelectedValue, ddlFlight.Text);
            //    if (dsCheck != null)
            //    {
            //        if (dsCheck.Tables != null)
            //        {
            //            if (dsCheck.Tables.Count > 0 && dsCheck.Tables[0].Rows.Count > 0)
            //            {
            //                lblStatus.ForeColor = Color.Red;
            //                lblStatus.Text = "FlightID already register for this origin.";
            //                return false;
            //            }
            //            else
            //            {

            //            }
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{

            //}


            //Validate Route grid.
            if (grdScheduleinfo.Rows.Count < 1)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please enter Route Details";
                return false;
            }



            for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
            {
                DropDownList tempSource;

                //Validate FromSource code
                tempSource = (DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlFromOrigin"));
                if (tempSource.Text == "Select")//tempSource.SelectedIndex < 1)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please select valid Origin Code from Route (row:" + (i + 1) + ")";
                    tempSource.Focus();
                    return false;
                }

                DropDownList tempDest;

                //Validate FromSource code
                tempDest = (DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlToDest"));
                if (tempDest.Text == "Select")//tempSource.SelectedIndex < 1)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please select valid Destination Code in Route (row)" + (i + 1) + ")";
                    tempDest.Focus();
                    return false;
                }
                if (tempSource.SelectedValue == tempDest.SelectedValue)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Origin and Destination Code can not be same in Route details.(row:" + (i + 1) + ")";
                    tempDest.Focus();
                    return false;
                }





                TextBox tempdeptTimeHr;

                //Validate code description
                tempdeptTimeHr = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeHr"));
                try
                {
                    if (tempdeptTimeHr.Text == "")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid Departure time in Hrs (row:" + (i + 1) + ")";
                        tempdeptTimeHr.Focus();
                        return false;
                    }
                    if (int.Parse(tempdeptTimeHr.Text) > 24)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid Departure time in Hrs (row:" + (i + 1) + ")";
                        tempdeptTimeHr.Focus();
                        return false;
                    }
                }
                catch (Exception)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid Departure time in Hrs (row:" + (i + 1) + ")";
                    tempdeptTimeHr.Focus();
                    return false;
                }
                tempdeptTimeHr = null;
                TextBox tempDeprtureTimeMin;

                //Validate code description
                tempDeprtureTimeMin = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeMin"));
                try
                {
                    if (tempDeprtureTimeMin.Text == "")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid Departure time in Min (row:" + (i + 1) + ")";
                        tempDeprtureTimeMin.Focus();
                        return false;
                    }
                    else if (int.Parse(tempDeprtureTimeMin.Text) > 60)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid Departure time in Min (row:" + (i + 1) + ")";
                        tempDeprtureTimeMin.Focus();
                        return false;
                    }
                }
                catch (Exception)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid Departure time in Min (row:" + (i + 1) + ")";
                    tempDeprtureTimeMin.Focus();
                    return false;
                }
                tempDeprtureTimeMin = null;

                //Validate Dept Day check day is null
                TextBox txtCheckDeptDay;
                //Validate code description
                txtCheckDeptDay = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptDay"));

                try
                {
                    if (txtCheckDeptDay.Text.Trim() == "")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid Departure Day (row:" + (i + 1) + ")";
                        txtCheckDeptDay.Focus();
                        return false;
                    }
                    else if (int.Parse(txtCheckDeptDay.Text) > 2)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid Departure day (row:" + (i + 1) + ")";
                        txtCheckDeptDay.Focus();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid Departure day (row:" + (i + 1) + ")";
                    txtCheckDeptDay.Focus();
                    return false;

                }


                //Validate Dept Day check day is null
                TextBox txtCheckArrDay;
                //Validate code description
                txtCheckArrDay = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtArrivalDay"));

                try
                {
                    if (txtCheckArrDay.Text.Trim() == "")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid Arrival Day (row:" + (i + 1) + ")";
                        txtCheckArrDay.Focus();
                        return false;
                    }
                    else if (int.Parse(txtCheckArrDay.Text) > 2)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid Arrival day (row:" + (i + 1) + ")";
                        txtCheckArrDay.Focus();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid Arrival day (row:" + (i + 1) + ")";
                    txtCheckArrDay.Focus();
                    return false;

                }



                TextBox tempArrivalTimeHr;

                //Validate tempArrivalTimeHr
                tempArrivalTimeHr = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtArrivaltimeHr"));
                try
                {
                    if (tempArrivalTimeHr.Text == "")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid arrival time in Hrs (row:" + (i + 1) + ")";
                        tempArrivalTimeHr.Focus();
                        return false;
                    }
                    else if (int.Parse(tempArrivalTimeHr.Text) > 24)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid arrival time in Hrs (row:" + (i + 1) + ")";
                        tempArrivalTimeHr.Focus();
                        return false;
                    }
                }
                catch (Exception)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid arrival time in Hrs(row:" + (i + 1) + ")";
                    tempArrivalTimeHr.Focus();
                    return false;
                }
                tempArrivalTimeHr = null;

                TextBox tempArrivalTimeMin;

                //Validate code description
                tempArrivalTimeMin = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtArrivaltimeMin"));
                try
                {
                    if (tempArrivalTimeMin.Text == "")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid arrival time in Min (row:" + (i + 1) + ")";
                        tempArrivalTimeMin.Focus();
                        return false;
                    }
                    else if (int.Parse(tempArrivalTimeMin.Text) > 60)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid arrival time in Min (row:" + (i + 1) + ")";
                        tempArrivalTimeMin.Focus();
                        return false;
                    }
                }
                catch (Exception)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid arrival time in Min (row:" + (i + 1) + ")";
                    tempArrivalTimeMin.Focus();
                    return false;
                }
                tempArrivalTimeMin = null;

                //Validate Arrival Departure time
                try
                {
                    tempArrivalTimeHr = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtArrivaltimeHr"));
                    tempdeptTimeHr = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeHr"));
                    int arrday = int.Parse(txtCheckArrDay.Text);
                    int DeptDay = int.Parse(txtCheckDeptDay.Text);

                    tempArrivalTimeMin = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtArrivaltimeMin"));
                    tempDeprtureTimeMin = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeMin"));


                    int ArrTimeHr = int.Parse(tempArrivalTimeHr.Text);
                    int DeptTimeHr = int.Parse(tempdeptTimeHr.Text);
                    if ((ArrTimeHr < DeptTimeHr) && (arrday < DeptDay))
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid arrival time in Hr (row:" + (i + 1) + ")";
                        tempdeptTimeHr.Focus();
                        return false;
                    }
                    else if ((ArrTimeHr < DeptTimeHr) && (arrday == DeptDay))
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid arrival time in Hr (row:" + (i + 1) + ")";
                        tempdeptTimeHr.Focus();
                        return false;
                    }
                    else if ((ArrTimeHr == DeptTimeHr) && (arrday < DeptDay))
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid arrival day (row:" + (i + 1) + ")";
                        tempdeptTimeHr.Focus();
                        return false;
                    }
                    else if (arrday < DeptDay)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid arrival day (row:" + (i + 1) + ")";
                        txtCheckArrDay.Focus();
                        return false;
                    }

                    else if (int.Parse(tempArrivalTimeHr.Text) == int.Parse(tempdeptTimeHr.Text))
                    {
                        if (int.Parse(txtCheckArrDay.Text) == int.Parse(txtCheckDeptDay.Text))
                        {
                            if (int.Parse(tempArrivalTimeMin.Text) < int.Parse(tempDeprtureTimeMin.Text))
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Please enter valid arrival Min (row:" + (i + 1) + ")";
                                tempDeprtureTimeMin.Focus();
                                return false;
                            }
                            else if (int.Parse(tempArrivalTimeMin.Text) == int.Parse(tempDeprtureTimeMin.Text))
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Daparture time and Arrival time should be different (row:" + (i + 1) + ")";
                                tempDeprtureTimeMin.Focus();
                                return false;
                            }

                        }
                        if (int.Parse(tempArrivalTimeMin.Text) == int.Parse(tempDeprtureTimeMin.Text))
                        {
                            if (int.Parse(txtCheckArrDay.Text) < int.Parse(txtCheckDeptDay.Text))
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Please enter valid arrival Day (row:" + (i + 1) + ")";
                                txtCheckArrDay.Focus();
                                return false;
                            }
                        }

                    }

                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid Departure and Arrival time in Min";
                    //tempdeptTimeHr.Focus();
                    return false;
                }

                //Validate Source Wise Data
                try
                {




                    if (grdScheduleinfo.Rows.Count > 1)
                    {
                        if (i > 0)
                        {
                            //tempSource = (DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlFromOrigin"));
                            //tempDest = (DropDownList)(grdScheduleinfo.Rows[i - 1].FindControl("ddlToDest"));
                            //DropDownList tempStatus=(DropDownList)(grdScheduleinfo.Rows[i-1].FindControl("ddlStatus"));


                            //if ((tempSource.Text != tempDest.Text) && (tempStatus.Text =="Active"))
                            //{
                            //    lblStatus.ForeColor = Color.Red;
                            //    lblStatus.Text = "Please Select From Source Code in Route";
                            //    tempSource.Focus();
                            //    return false;

                            //}





                            //txtCheckArrDay = (TextBox)(grdScheduleinfo.Rows[i - 1].FindControl("txtArrivalDay"));
                            //txtCheckDeptDay = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptDay"));
                            //if (int.Parse(txtCheckArrDay.Text) > int.Parse(txtCheckDeptDay.Text))
                            //{
                            //    lblStatus.ForeColor = Color.Red;
                            //    lblStatus.Text = "Please enter valid Departure Day";
                            //    txtCheckArrDay.Focus();
                            //    return false;
                            //}

                            ////tempDeprtureTimeMin = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeMin"));

                            ////tempArrivalTimeMin = (TextBox)(grdScheduleinfo.Rows[i - 1].FindControl("txtArrivalTimeMin"));
                            ////if (int.Parse(tempDeprtureTimeMin.Text) < int.Parse(tempArrivalTimeMin.Text))
                            ////{
                            ////    lblStatus.ForeColor = Color.Red;
                            ////    lblStatus.Text = "Please enter valid Departure Time in Min";
                            ////    tempDeprtureTimeMin.Focus();
                            ////    return false;
                            ////}


                            //tempArrivalTimeHr = (TextBox)(grdScheduleinfo.Rows[i - 1].FindControl("txtArrivaltimeHr"));
                            //tempdeptTimeHr = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeHr"));
                            //int arrday = int.Parse(txtCheckArrDay.Text);
                            //int DeptDay = int.Parse(txtCheckDeptDay.Text);
                            //tempDeprtureTimeMin = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeMin"));

                            //tempArrivalTimeMin = (TextBox)(grdScheduleinfo.Rows[i - 1].FindControl("txtArrivalTimeMin"));


                            //int ArrTimeHr = int.Parse(tempArrivalTimeHr.Text);
                            //int DeptTimeHr = int.Parse(tempdeptTimeHr.Text);
                            //if ((ArrTimeHr > DeptTimeHr) && (arrday > DeptDay))
                            //{
                            //    lblStatus.ForeColor = Color.Red;
                            //    lblStatus.Text = "Please enter valid arrival time in Hr";
                            //    tempdeptTimeHr.Focus();
                            //    return false;
                            //}
                            //else if ((ArrTimeHr > DeptTimeHr) && (arrday == DeptDay))
                            //{
                            //    lblStatus.ForeColor = Color.Red;
                            //    lblStatus.Text = "Please enter valid Departure time in Hr";
                            //    tempdeptTimeHr.Focus();
                            //    return false;
                            //}
                            //else if ((ArrTimeHr == DeptTimeHr) && (arrday < DeptDay))
                            //{
                            //    lblStatus.ForeColor = Color.Red;
                            //    lblStatus.Text = "Please enter valid arrival time in Hr";
                            //    tempdeptTimeHr.Focus();
                            //    return false;
                            //}
                            //else if (arrday > DeptDay)
                            //{
                            //    lblStatus.ForeColor = Color.Red;
                            //    lblStatus.Text = "Please enter valid departure day";
                            //    txtCheckArrDay.Focus();
                            //    return false;
                            //}
                            //else if ((ArrTimeHr == DeptTimeHr) && (arrday == DeptDay) && (int.Parse(tempDeprtureTimeMin.Text)) < (int.Parse(tempArrivalTimeMin.Text)))
                            //{
                            //    lblStatus.ForeColor = Color.Red;
                            //    lblStatus.Text = "Please enter valid Departure time in Min";
                            //    tempdeptTimeHr.Focus();
                            //    return false;
                            //}
                            //else if ((ArrTimeHr == DeptTimeHr) && (arrday == DeptDay) && (int.Parse(tempDeprtureTimeMin.Text)) == (int.Parse(tempArrivalTimeMin.Text)))
                            //{
                            //    lblStatus.ForeColor = Color.Red;
                            //    lblStatus.Text = "Please enter valid Departure time";
                            //    tempdeptTimeHr.Focus();
                            //    return false;
                            //}

                            tempSource = (DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlFromOrigin"));
                            tempDest = (DropDownList)(grdScheduleinfo.Rows[i - 1].FindControl("ddlToDest"));

                            if (tempSource.Text != tempDest.Text)
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Please Select From Source Code in route in (row:" + (i + 1) + ")";
                                tempSource.Focus();
                                return false;

                            }
                            txtCheckArrDay = (TextBox)(grdScheduleinfo.Rows[i - 1].FindControl("txtArrivalDay"));
                            txtCheckDeptDay = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptDay"));
                            if (int.Parse(txtCheckArrDay.Text) > int.Parse(txtCheckDeptDay.Text))
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Please enter valid Departure Day (row:" + (i + 1) + ")";
                                txtCheckArrDay.Focus();
                                return false;
                            }

                            //tempDeprtureTimeMin = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeMin"));

                            //tempArrivalTimeMin = (TextBox)(grdScheduleinfo.Rows[i - 1].FindControl("txtArrivalTimeMin"));
                            //if (int.Parse(tempDeprtureTimeMin.Text) < int.Parse(tempArrivalTimeMin.Text))
                            //{
                            //    lblStatus.ForeColor = Color.Red;
                            //    lblStatus.Text = "Please enter valid Departure Time in Min";
                            //    tempDeprtureTimeMin.Focus();
                            //    return false;
                            //}


                            tempArrivalTimeHr = (TextBox)(grdScheduleinfo.Rows[i - 1].FindControl("txtArrivaltimeHr"));
                            tempdeptTimeHr = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeHr"));
                            int arrday = int.Parse(txtCheckArrDay.Text);
                            int DeptDay = int.Parse(txtCheckDeptDay.Text);
                            tempDeprtureTimeMin = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeMin"));

                            tempArrivalTimeMin = (TextBox)(grdScheduleinfo.Rows[i - 1].FindControl("txtArrivalTimeMin"));


                            int ArrTimeHr = int.Parse(tempArrivalTimeHr.Text);
                            int DeptTimeHr = int.Parse(tempdeptTimeHr.Text);
                            if ((ArrTimeHr > DeptTimeHr) && (arrday > DeptDay))
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Please enter valid arrival time in Hr (row:" + (i + 1) + ")";
                                tempdeptTimeHr.Focus();
                                return false;
                            }
                            else if ((ArrTimeHr > DeptTimeHr) && (arrday == DeptDay))
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Please enter valid Departure time in Hr (row:" + (i + 1) + ")";
                                tempdeptTimeHr.Focus();
                                return false;
                            }
                            else if ((ArrTimeHr == DeptTimeHr) && (arrday < DeptDay))
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Please enter valid arrival time in Hr (row:" + (i + 1) + ")";
                                tempdeptTimeHr.Focus();
                                return false;
                            }
                            else if (arrday > DeptDay)
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Please enter valid departure day (row:" + (i + 1) + ")";
                                txtCheckArrDay.Focus();
                                return false;
                            }
                            else if ((ArrTimeHr == DeptTimeHr) && (arrday == DeptDay) && (int.Parse(tempDeprtureTimeMin.Text)) < (int.Parse(tempArrivalTimeMin.Text)))
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Please enter valid Departure time in Min (row:" + (i + 1) + ")";
                                tempdeptTimeHr.Focus();
                                return false;
                            }
                            else if ((ArrTimeHr == DeptTimeHr) && (arrday == DeptDay) && (int.Parse(tempDeprtureTimeMin.Text)) == (int.Parse(tempArrivalTimeMin.Text)))
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Please enter valid Departure time (row:" + (i + 1) + ")";
                                tempdeptTimeHr.Focus();
                                return false;
                            }




                        }


                    }

                    //  int CheckDest = grdScheduleinfo.Rows.Count;
                    DropDownList ChecktempDest;
                    ChecktempDest = (DropDownList)(grdScheduleinfo.Rows[grdScheduleinfo.Rows.Count - 1].FindControl("ddlToDest"));
                    if (ChecktempDest.Text != ddlDestination0.Text)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid To Destination Code in Route (row:" + (grdScheduleinfo.Rows.Count - 1) + ")";
                        ChecktempDest.Focus();
                        return false;
                    }


                }
                catch (Exception ex)
                {

                }

                //Validate Weeekdays check
                string strFreq = "";
                try
                {
                    if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkMon")).Checked == false && ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkTues")).Checked == false && ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkwed")).Checked == false && ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkThur")).Checked == false && ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkFri")).Checked == false && ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSat")).Checked == false && ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSun")).Checked == false)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please Check Frequency for flights";
                        return false;
                    }
                    else
                    {
                        strFreq = "";


                        if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkMon")).Checked == true)
                            strFreq = "1";
                        else
                            strFreq = "0";

                        if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkTues")).Checked == true)
                            strFreq = strFreq + ',' + "1";
                        else
                            strFreq = strFreq + ',' + "0";

                        if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkwed")).Checked == true)
                            strFreq = strFreq + ',' + "1";
                        else
                            strFreq = strFreq + ',' + "0";



                        if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkThur")).Checked == true)
                            strFreq = strFreq + ',' + "1";
                        else
                            strFreq = strFreq + ',' + "0";



                        if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkFri")).Checked == true)
                            strFreq = strFreq + ',' + "1";
                        else
                            strFreq = strFreq + ',' + "0";


                        if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSat")).Checked == true)
                            strFreq = strFreq + ',' + "1";
                        else
                            strFreq = strFreq + ',' + "0";


                        if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSun")).Checked == true)
                            strFreq = strFreq + ',' + "1";
                        else
                            strFreq = strFreq + ',' + "0";

                    }

                    DateTime dt1 = DateTime.ParseExact(txtFromdate.Text, "dd/MM/yyyy", null);
                    DateTime dt2 = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);



                    TimeSpan span = dt2 - dt1;
                    int check = 0;
                    int uncheck = 0;
                    if (span.TotalDays < 7)
                    {

                        for (int idays = 0; idays < span.TotalDays + 1; idays++)
                        {

                            if (dt1.DayOfWeek.ToString() == "Sunday")
                            {
                                if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSun")).Checked == false)
                                {
                                    check = check + 1;

                                }
                                else if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSun")).Checked == true)
                                {
                                    uncheck = uncheck + 1;

                                }

                            }
                            if (dt1.DayOfWeek.ToString() == "Monday")
                            {
                                if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkMon")).Checked == false)
                                {
                                    check = check + 1;
                                }
                                else if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkMon")).Checked == true)
                                {
                                    uncheck = uncheck + 1;

                                }
                            }
                            if (dt1.DayOfWeek.ToString() == "Tuesday")
                            {
                                if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkTues")).Checked == false)
                                {
                                    check = check + 1;
                                }
                                else if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkTues")).Checked == true)
                                {
                                    uncheck = uncheck + 1;
                                }
                            }
                            if (dt1.DayOfWeek.ToString() == "Wednesday")
                            {
                                if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkwed")).Checked == false)
                                {
                                    check = check + 1;
                                }
                                else if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkwed")).Checked == true)
                                {
                                    uncheck = uncheck + 1;
                                }
                            }
                            if (dt1.DayOfWeek.ToString() == "Thursday")
                            {
                                if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkThur")).Checked == false)
                                {
                                    check = check + 1;
                                }
                                else if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkThur")).Checked == true)
                                {
                                    uncheck = uncheck + 1;
                                }
                            }
                            if (dt1.DayOfWeek.ToString() == "Friday")
                            {
                                if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkFri")).Checked == false)
                                {
                                    check = check + 1;
                                }
                                else if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkFri")).Checked == true)
                                {
                                    uncheck = uncheck + 1;
                                }
                            }
                            if (dt1.DayOfWeek.ToString() == "Saturday")
                            {
                                if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSat")).Checked == false)
                                {
                                    check = check + 1;
                                }
                                else if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSat")).Checked == true)
                                {
                                    uncheck = uncheck + 1;
                                }

                            }
                            dt1 = dt1.AddDays(1);

                        }
                    }
                    if (check == span.TotalDays + 1)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please Check Frequency days in given date range in (row:" + (i + 1) + ")";

                        return false;
                    }
                    //if (uncheck == span.TotalDays + 1)
                    //{
                    //    lblStatus.ForeColor = Color.Red;
                    //    lblStatus.Text = "Please Check Frequency days in given date range in (row:" + (i + 1) + ")";

                    //    return false;
                    //}

                    int ActualCheck = 0;
                    if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSun")).Checked == true)
                    {
                        ActualCheck = ActualCheck + 1;

                    }

                    if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkMon")).Checked == true)
                    {
                        ActualCheck = ActualCheck + 1;
                    }

                    if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkTues")).Checked == true)
                    {
                        ActualCheck = ActualCheck + 1;
                    }

                    if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkwed")).Checked == true)
                    {
                        ActualCheck = ActualCheck + 1;
                    }

                    if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkThur")).Checked == true)
                    {
                        ActualCheck = ActualCheck + 1;
                    }



                    if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkFri")).Checked == true)
                    {
                        ActualCheck = ActualCheck + 1;
                    }

                    if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSat")).Checked == true)
                    {
                        ActualCheck = ActualCheck + 1;
                    }






                    if ((span.TotalDays + 1) < ActualCheck)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please Check Frequency days in given date range in (row:" + (i + 1) + ")";

                        return false;
                    }



                }
                catch (Exception)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Check Frequency for flights";

                    return false;
                }

                #region Check in Databse
                try
                {// if ( Convert.ToDateTime(txtFromdate.Text) > Convert.ToDateTime(txtToDate.Text))
                    //  DateTime dt1from=Convert.ToDateTime(txtFromdate.Text);
                    //     DateTime dt1to= Convert.ToDateTime(txtToDate.Text);

                    DateTime dt1from = DateTime.ParseExact(txtFromdate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                    DateTime dt1to = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);

                    //DataSet dsCheckRoute = OBJasb.GetAirlineScheduleRouteforflight(tempSource.SelectedValue, tempDest.SelectedValue, ddlFlight.Text, strFreq, dt1from, dt1to);
                    //if (dsCheckRoute != null)
                    //{
                    //    if (dsCheckRoute.Tables != null)
                    //    {
                    //        if (dsCheckRoute.Tables.Count > 0 && dsCheckRoute.Tables[0].Rows.Count > 0)
                    //        {
                    //            for (int k = 0; k < strFreq.Length; k++)
                    //            {
                    //                string Frequency = dsCheckRoute.Tables[0].Rows[0][0].ToString();
                    //                string stract = Frequency[k].ToString();
                    //                string newfreq = strFreq[k].ToString();
                    //                if (stract != "," || newfreq != ",")
                    //                {
                    //                    if (stract == newfreq)
                    //                    {
                    //                        lblStatus.ForeColor = Color.Red;
                    //                        lblStatus.Text = "FlightID already register for this frequency in route (row :" + (i + 1) + ")";
                    //                        return false;
                    //                    }
                    //                }
                    //            }
                    //        }
                    //        else
                    //        {

                    //        }
                    //    }
                    //}

                }
                catch (Exception ex)
                {

                }
                #endregion


                TextBox txtCapacity = ((TextBox)grdScheduleinfo.Rows[i].FindControl("txtCapacity"));
                try
                {


                    if (txtCapacity.Text.Trim() == "")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid Cargo Capacity(row :" + (i + 1) + ")";
                        txtCapacity.Focus();
                        return false;
                    }
                    else if (float.Parse(txtCapacity.Text) < 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid Cargo Capacity (row :" + (i + 1) + ")";
                        txtCapacity.Focus();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid Cargo Capacity(row :" + (i + 1) + ")";
                    txtCapacity.Focus();
                    return false;

                }



            }

            return true;
        }
        #endregion Validate Data

        #region Add new row
        protected void Addrow(object sender, EventArgs e)
        {
            try
            {
                //LoadGridSchedule();
                //SetPreviousData();
                //AddNewRowToGrid();
                //LoadSourceInGridview();
                //LoadDestinationInGridview();
            }
            catch (Exception ex)
            {
            }
            // LoadGridSchedule();
        }
        #endregion

        #region  get Flight No
        private void getFligtNo(string source)
        {
            try
            {
                DataSet ds = OBJasb.GetPartnerFlightList(source);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            //flight dropdown
                            DataRow row = ds.Tables[0].NewRow();

                            row[ds.Tables[0].Columns[0].ColumnName] = "All";
                            ds.Tables[0].Rows.Add(row);

                            //mod
                            //ddlFlight.DataSource = ds;
                            //ddlFlight.DataMember = ds.Tables[0].TableName;
                            //ddlFlight.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            //ddlFlight.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                            //ddlFlight.DataBind();
                            //ddlFlight.Text = "All";
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion
        
        #region Origin List
        private void OriginList()
        {
            try
            {
                DataSet ds = OBJasb.GetOriginList(ddlOrigin.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            //Dest dropdown
                            DataRow row = ds.Tables[0].NewRow();

                            row[ds.Tables[0].Columns[0].ColumnName] = "All";
                            ds.Tables[0].Rows.Add(row);

                            ddlOrigin.DataSource = ds;
                            ddlOrigin.DataMember = ds.Tables[0].TableName;
                            ddlOrigin.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            ddlOrigin.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                            ddlOrigin.DataBind();
                            ddlOrigin.Text = "All";

                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion Origin List

        #region Origin List for Details
        private void OriginListforDetails()
        {
            try
            {
                DataSet ds = OBJasb.GetOriginList(ddlOrigin1.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            //Dest dropdown
                            // DataRow row = ds.Tables[0].NewRow();

                            //row[ds.Tables[0].Columns[0].ColumnName] = "All";
                            // ds.Tables[0].Rows.Add(row);

                            ddlOrigin1.DataSource = ds;
                            ddlOrigin1.DataMember = ds.Tables[0].TableName;
                            ddlOrigin1.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            ddlOrigin1.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                            ddlOrigin1.DataBind();
                            // ddlOrigin.Text = "All";

                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion Origin List

        #region Dest List
        private void DestinationList()
        {
            try
            {
                DataSet ds = OBJasb.GetOriginList(ddlOrigin.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            //Dest dropdown
                            DataRow row = ds.Tables[0].NewRow();

                            row[ds.Tables[0].Columns[0].ColumnName] = "All";
                            ds.Tables[0].Rows.Add(row);

                            ddlDestination.DataSource = ds;
                            ddlDestination.DataMember = ds.Tables[0].TableName;
                            ddlDestination.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            ddlDestination.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                            ddlDestination.DataBind();
                            ddlDestination.Text = "All";

                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion Dest List

        #region Origin List
        private void DestinationListforDetails()
        {
            try
            {
                DataSet ds = OBJasb.GetOriginList(ddlDestination0.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            //Dest dropdown


                            ddlDestination0.DataSource = ds;
                            ddlDestination0.DataMember = ds.Tables[0].TableName;
                            ddlDestination0.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            ddlDestination0.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                            ddlDestination0.DataBind();
                            //ddlDestination.Text = "All";

                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion Origin List 

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                lblStatus.ForeColor = Color.Red;

                if (!ValidateData())
                    return;

                int IsRouteUpdate = 0;
                #region Insert RouteDetails

                bool Isinsert = false;


                DateTime dt1 = DateTime.ParseExact(txtFromdate.Text, "dd/MM/yyyy", null);
                DateTime dt2 = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);

                //scheduleid = Int16.Parse(dt.Rows[ind]["ScheduleID"].ToString());

                for (int j = 0; j < grdScheduleinfo.Rows.Count; j++)
                {
                    #region Prepare Parameters
                    object[] RouteInfo = new object[23];
                    int i = 0;

                    //0
                    RouteInfo.SetValue(dt1, i);
                    i++;


                    //1
                    RouteInfo.SetValue(dt2, i);
                    i++;


                    //2
                    // RouteInfo.SetValue(ddlFlight.SelectedValue, i);
                    string FlightID = "";
                    Label lblFlight = ((Label)(grdScheduleinfo.Rows[0].FindControl("lblFlight")));
                    lblFlight.Text = lblFlight.Text;
                    RouteInfo.SetValue(lblFlight.Text.Trim(), i);
                    i++;


                    //3
                    // RouteInfo.SetValue(ddlOrigin.SelectedValue, i);
                    RouteInfo.SetValue(ddlOrigin1.Text, i);

                    i++;

                    //4
                    RouteInfo.SetValue(ddlDestination0.Text, i);
                    i++;

                    //5
                    RouteInfo.SetValue(((DropDownList)(grdScheduleinfo.Rows[j].FindControl("ddlFromOrigin"))).SelectedValue, i);
                    i++;
                    //6
                    RouteInfo.SetValue(((DropDownList)(grdScheduleinfo.Rows[j].FindControl("ddlToDest"))).SelectedValue, i);
                    i++;
                    string frquency = "";
                    if (((CheckBox)grdScheduleinfo.Rows[j].FindControl("chkMon")).Checked == true)
                        frquency = "1";
                    else
                        frquency = "0";

                    if (((CheckBox)grdScheduleinfo.Rows[j].FindControl("chkTues")).Checked == true)
                        frquency = frquency + ',' + "1";
                    else
                        frquency = frquency + ',' + "0";

                    if (((CheckBox)grdScheduleinfo.Rows[j].FindControl("chkwed")).Checked == true)
                        frquency = frquency + ',' + "1";
                    else
                        frquency = frquency + ',' + "0";



                    if (((CheckBox)grdScheduleinfo.Rows[j].FindControl("chkThur")).Checked == true)
                        frquency = frquency + ',' + "1";
                    else
                        frquency = frquency + ',' + "0";



                    if (((CheckBox)grdScheduleinfo.Rows[j].FindControl("chkFri")).Checked == true)
                        frquency = frquency + ',' + "1";
                    else
                        frquency = frquency + ',' + "0";


                    if (((CheckBox)grdScheduleinfo.Rows[j].FindControl("chkSat")).Checked == true)
                        frquency = frquency + ',' + "1";
                    else
                        frquency = frquency + ',' + "0";


                    if (((CheckBox)grdScheduleinfo.Rows[j].FindControl("chkSun")).Checked == true)
                        frquency = frquency + ',' + "1";
                    else
                        frquency = frquency + ',' + "0";


                    //7
                    RouteInfo.SetValue(frquency, i);
                    i++;



                    //8
                    string DeptTime1 = ((TextBox)grdScheduleinfo.Rows[j].FindControl("txtDeptTimeHr")).Text; // 
                    DeptTime1 = DeptTime1 + ":" + ((TextBox)grdScheduleinfo.Rows[j].FindControl("txtDeptTimeMin")).Text;

                    RouteInfo.SetValue(DeptTime1, i);
                    i++;

                    //9
                    string arrTime1 = ((TextBox)grdScheduleinfo.Rows[j].FindControl("txtArrivaltimeHr")).Text;
                    arrTime1 = arrTime1 + ":" + ((TextBox)grdScheduleinfo.Rows[j].FindControl("txtArrivaltimeMin")).Text;
                    //10
                    RouteInfo.SetValue(arrTime1, i);
                    i++;

                    //11
                    RouteInfo.SetValue(DateTime.Now.ToString(), i);
                    i++;
                    string UserID = Session["UserName"].ToString();
                    //12
                    RouteInfo.SetValue(UserID, i);
                    i++;

                    //13
                    string IsAtive = ((DropDownList)(grdScheduleinfo.Rows[j].FindControl("ddlStatus"))).SelectedValue;
                    //bool ActiveStatus = false;
                    //if (IsAtive == "Active")
                    //{
                    //    ActiveStatus = true;
                    //}
                    //else
                    //{
                    //    ActiveStatus = false;
                    //}

                    RouteInfo.SetValue(IsAtive, i);
                    i++;
                    //14
                    string strDeptDay = ((TextBox)(grdScheduleinfo.Rows[j].FindControl("txtDeptDay"))).Text;
                    RouteInfo.SetValue(strDeptDay, i);
                    i++;

                    //15
                    string strArrDay = ((TextBox)(grdScheduleinfo.Rows[j].FindControl("txtArrivalDay"))).Text;
                    RouteInfo.SetValue(strArrDay, i);
                    i++;

                    //16
                    RouteInfo.SetValue(txtCargoCapacity.Text.Trim(), i);
                    i++;
                    //17
                    RouteInfo.SetValue(ddlLoadAirCraftType.SelectedValue, i);
                    i++;



                    //18
                    RouteInfo.SetValue(chkDomestic0.Checked, i);
                    i++;


                    //19
                    RouteInfo.SetValue(scheduleid, i);
                    i++;

                    //20
                    RouteInfo.SetValue(((DropDownList)(grdScheduleinfo.Rows[j].FindControl("ddlAirCraft"))).SelectedValue, i);
                    i++;

                    //21
                    RouteInfo.SetValue(((TextBox)(grdScheduleinfo.Rows[j].FindControl("txtCapacity"))).Text, i);
                    i++;
                    //22
                    RouteInfo.SetValue(((TextBox)(grdScheduleinfo.Rows[j].FindControl("txtPartnerCode"))).Text, i);
                    i++;

                    //23
                    RouteInfo.SetValue(((DropDownList)(grdScheduleinfo.Rows[j].FindControl("ddlTailNo"))).SelectedValue, i);




                    #endregion Prepare Parameters



                    if (Isinsert == false)
                    {
                        IsRouteUpdate = OBJasb.UpdatePartnerRouteDetails(RouteInfo);
                        Isinsert = true;
                    }
                    if (Isinsert == true)
                    {
                        //if (txtFlightFromdate.Text == CheckFromDate && txtFlightToDate.Text == CheckToDate)
                        //{
                        IsRouteUpdate = OBJasb.UpdatePartnerRouteDetailsForSameDate(RouteInfo);
                        //}
                    }

                }
                if (IsRouteUpdate < 0)
                {
                    lblStatus.Text = "Error update Route Details. Please try again...";
                    return;
                }
                else
                {



                #endregion

                    #region ShowSavedatagrid
                    // btnList.Click(); ;

                    try
                    {

                        //if (chkDomestic.Checked == false && chkInternational.Checked == false)
                        //{
                        //    lblStatus.Text = "Kindly Check atleast one from Domestic and Inernational";
                        //    return;
                        //}


                        btnEdit.Visible = false;
                        pnlUpdate.Visible = false;
                        scheduleid = 0;
                        lblStatus.Text = "";
                        if (txtFlightNo.Text == "")//ddlFlight.SelectedItem.Text == "All")mod
                        {
                            LoadGridSchedule();
                        }
                        else
                        {
                            LoadGridFlight();
                        }
                        LoadSourceInGridview();
                        // DestinationList(); 
                        LoadDestinationInGridview();

                        AirCraftTypeinGridview();
                        AddRowToGrid();




                        btnEdit.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                    }

                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "Route Details Updated Successfully";
                    return;
                }

                    #endregion

            }
            catch (Exception ex)
            {

            }

        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                if (txtFlightNo.Text == "")//ddlFlight.SelectedItem.Text == "All")mod
                {
                    lblStatus.Text = "Please select Flight to Update Details";
                    return;
                }

                pnlDestDetails.Enabled = true;
                pnlSchedule.Enabled = true;
                pnlUpdate.Visible = true;
            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlOrigin_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // getFligtNo(ddlOrigin.SelectedValue.ToString());
                //getFligtNoSourceDestWise(ddlOrigin.SelectedValue.ToString(), ddlDestination.SelectedValue.ToString());

            }
            catch (Exception ex)
            {
            }

        }

        protected void ddlFlight_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnEdit.Enabled = false;
        }

        protected void ddlDestination_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //  getFligtNo(ddlOrigin.SelectedValue.ToString());

                //getFligtNoSourceDestWise(ddlOrigin.SelectedValue.ToString(), ddlDestination.SelectedValue.ToString());
            }
            catch (Exception ex)
            {
            }
        }

        protected void grdScheduleinfo_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {

        }

        protected void grdScheduleinfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                grdScheduleinfo.PageIndex = e.NewPageIndex;

                AddRowToGrid();

            }
            catch (Exception ex)
            {
            }

        }
        
        protected void showCapacityInGrid(object sender, EventArgs e)
        {
            for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
            {

                try
                {

                    string aircrafttype = ((DropDownList)grdScheduleinfo.Rows[i].FindControl("ddlAirCraft")).SelectedItem.ToString();

                    DataSet ds = OBJasb.GetCargoCapacity(aircrafttype);
                    if (ds != null)
                    {
                        if (ds.Tables != null)
                        {
                            if (ds.Tables.Count > 0 || ds.Tables[0].Rows.Count > 0)
                            {
                                txtCargoCapacity.Text = ds.Tables[0].Rows[0][0].ToString();
                                //txtCapacity
                                ((TextBox)grdScheduleinfo.Rows[i].FindControl("txtCapacity")).Text = ds.Tables[0].Rows[0][0].ToString();
                            }
                        }
                    }
                }

                catch (Exception)
                {
                }

                #region get Tail Number as per Aircraft Type
                try
                {
                    string AirCrtType = ((DropDownList)grdScheduleinfo.Rows[i].FindControl("ddlAirCraft")).SelectedItem.ToString();
                    DataSet dsTail = OBJasb.GetTailNumber(AirCrtType);

                    if (dsTail != null && dsTail.Tables.Count > 0 && dsTail.Tables[0].Rows.Count > 0)
                    {

                        ((DropDownList)grdScheduleinfo.Rows[i].FindControl("ddlTailNo")).Items.Clear();
                        ((DropDownList)grdScheduleinfo.Rows[i].FindControl("ddlTailNo")).DataSource = dsTail.Tables[0];
                        ((DropDownList)grdScheduleinfo.Rows[i].FindControl("ddlTailNo")).DataTextField = "TailNo";
                        ((DropDownList)grdScheduleinfo.Rows[i].FindControl("ddlTailNo")).DataValueField = "TailNo";

                        ((DropDownList)grdScheduleinfo.Rows[i].FindControl("ddlTailNo")).DataBind();

                    }
                }
                catch (Exception ex)
                {

                }
                #endregion 


            }

        }
        
        #region LoadDest in Gridview Dropdown with Form state
        public void LoadDestinationInGridviewwithState()
        {
            try
            {
                ArrayList dtarCheckdest = new ArrayList();

                DataSet ds = OBJasb.GetDestinationList("");
                DropDownList ddlest = new DropDownList();
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];

                for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
                {
                    ddlest = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlToDest")));
                    if (ds != null)
                    {
                        //LoadDest in Gridview Dropdown 
                        DataRow row = ds.Tables[0].NewRow();
                        if (!dtarCheckdest.Contains("Select"))
                        {
                            row[ds.Tables[0].Columns[0].ColumnName] = "Select";
                            ds.Tables[0].Rows.Add(row);
                            dtarCheckdest.Add("Select");
                        }
                        ddlest.DataSource = ds;
                        ddlest.DataMember = ds.Tables[0].TableName;
                        ddlest.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                        ddlest.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                        ddlest.DataBind();
                        //ddl.Text = "Select";
                        if (dtCurrentTable.Rows[i][2].ToString() != "")
                        {
                            if (dtCurrentTable.Rows.Count > 1)
                            {
                                if (dtCurrentTable.Rows[i][2].ToString() != "select" && dtCurrentTable.Rows[i][2].ToString() != "")
                                {
                                    ddlest.Text = dtCurrentTable.Rows[i][2].ToString();
                                }
                                else
                                {
                                    //ddlest.Text = "Select";
                                    ddlest.Text = ddlDestination.SelectedValue;
                                }
                            }
                        }
                        else
                        {
                            ddlest.Text = "Select";
                        }
                        //ddlest.Text = "Select";
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        #endregion LoadDest Dropdown

        protected void ddlLoadAirCraftType_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {

                DataSet ds = OBJasb.GetCargoCapacity(ddlLoadAirCraftType.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0 || ds.Tables[0].Rows.Count > 0)
                        {
                            txtCargoCapacity.Text = ds.Tables[0].Rows[0][0].ToString();
                            //txtCapacity

                            for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
                            {

                                ((TextBox)grdScheduleinfo.Rows[i].FindControl("txtCapacity")).Text = ds.Tables[0].Rows[0][0].ToString();
                                ((DropDownList)grdScheduleinfo.Rows[i].FindControl("ddlAirCraft")).Text = ddlLoadAirCraftType.SelectedValue;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }


        }




        protected void grdFlight_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void grdFlight_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Flight")
                {
                    pnlSchedule.Visible = true;
                    btnEdit.Visible = true;
                    //rowind = Convert.ToInt32(grdFlight.SelectedIndex);
                    GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                    rowind = row.RowIndex;
                    //grdBillingInfo.Rows[rowindex].Cells[0].Font.Bold = true;
                    OriginListforDetails();
                    DestinationListforDetails();
                    fillFlightDetails(rowind);
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void fillFlightDetails(int ind)
        {
            try
            {
                DataSet dsdetails = (DataSet)Session["dsDetails"];
                DataTable dt = (DataTable)ViewState["FlightTable"];

                scheduleid = Int16.Parse(dt.Rows[ind]["ScheduleID"].ToString());
                DataSet dsRoute = OBJasb.GetPartnerAirlineScheduleUsingRouteID(scheduleid);

                if (dsRoute.Tables[0].Rows.Count > 0)
                {

                    //  pnlSchedule.Enabled = false;
                    ViewState["CurrentTable"] = dsRoute.Tables[0];
                    DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                    grdScheduleinfo.DataSource = dtCurrentTable;
                    grdScheduleinfo.DataBind();
                    DataRow drCurrentRow = null;
                    LoadSourceInGridview();
                    // DestinationList(); 
                    LoadDestinationInGridview();
                    AirCraftTypeinGridview();
                    AirCraftTypeInEdit();
                    if (dtCurrentTable.Rows.Count > 0)
                    {
                        //extract the TextBox values
                        pnlSchedule.Visible = true;

                        pnlDestDetails.Visible = true;
                        pnlDestDetails.Enabled = false;
                        txtFromdate.Text = Convert.ToDateTime(dtCurrentTable.Rows[0][8]).ToString("dd/MM/yyyy");
                        txtToDate.Text = Convert.ToDateTime(dtCurrentTable.Rows[0][9]).ToString("dd/MM/yyyy");
                        try
                        {

                            // CheckFromDate = DateTime.ParseExact(dtCurrentTable.Rows[0][8].ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                            CheckFromDate = dtCurrentTable.Rows[0][8].ToString();

                            //DateTime.Parse(Convert.ToDateTime(dtCurrentTable.Rows[0][8]).ToString("dd-MM-yyyy"));
                            // CheckToDate = DateTime.ParseExact(dtCurrentTable.Rows[0][9].ToString(),"dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                            CheckToDate = dtCurrentTable.Rows[0][9].ToString();
                            //DateTime.Parse(Convert.ToDateTime(dtCurrentTable.Rows[0][9]).ToString("dd-MM-yyyy"));
                        }
                        catch (Exception ex)
                        {
                        }

                        txtCargoCapacity.Text = dtCurrentTable.Rows[0][12].ToString();
                        string strAircraft = "";
                        strAircraft = dtCurrentTable.Rows[0][13].ToString();
                        try
                        {
                            if (strAircraft != "")
                            {
                                ddlLoadAirCraftType.SelectedValue = dtCurrentTable.Rows[0]["AirCraftType"].ToString();
                            }
                        }
                        catch (Exception)
                        {
                        }
                        //    txtSource.Text = dtCurrentTable.Rows[0][14].ToString();
                        ddlOrigin1.Text = dtCurrentTable.Rows[0][14].ToString();
                        // txtDestination.Text = dtCurrentTable.Rows[0][15].ToString();
                        //Add destination in dropdown if not present in the AirportMaster.
                        if (ddlDestination0.Items.IndexOf(new ListItem(dtCurrentTable.Rows[0][15].ToString(), 
                            dtCurrentTable.Rows[0][15].ToString())) < 0)
                        {
                            ddlDestination0.Items.Add(new ListItem(dtCurrentTable.Rows[0][15].ToString(),
                            dtCurrentTable.Rows[0][15].ToString()));
                        }
                        ddlDestination0.SelectedValue = dtCurrentTable.Rows[0][15].ToString();

                        if (dtCurrentTable.Rows[0][16].ToString() == "True")
                        {
                            chkDomestic0.Checked = true;
                            chkInternational0.Checked = false;
                        }
                        else
                        {
                            chkDomestic0.Checked = false;
                            chkInternational0.Checked = true;
                        }

                        for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
                        {
                            
                            //If grid row is greater than table row count then exit loop.
                            if (i >= dtCurrentTable.Rows.Count)
                                break;
                            
                            int j = grdScheduleinfo.PageIndex * grdScheduleinfo.PageSize + i;
                            //extract the TextBox values
                            DropDownList ddlFromOrigin = new DropDownList();
                            ddlFromOrigin = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlFromOrigin")));

                            ddlFromOrigin.SelectedValue = dtCurrentTable.Rows[j][1].ToString();

                            DropDownList ddlDest = new DropDownList();

                            ddlDest = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlToDest")));

                            ddlDest.SelectedValue = dtCurrentTable.Rows[j][2].ToString();

                            Label lblFromdt = ((Label)grdScheduleinfo.Rows[i].FindControl("lblFromDate"));
                            Label lblTodt = ((Label)grdScheduleinfo.Rows[i].FindControl("lblToDate"));
                            lblFromdt.Text = Convert.ToDateTime(dtCurrentTable.Rows[j][8]).ToString("dd/MM/yyyy");

                            lblTodt.Text = Convert.ToDateTime(dtCurrentTable.Rows[j][9]).ToString("dd/MM/yyyy");

                            string FlightID = dtCurrentTable.Rows[j][0].ToString();
                            
                            if (!arFlight.Contains(FlightID))//FlightID))//&& (!arOrigin.Contains(dtCurrentTable.Rows[i][1].ToString())))
                            {
                                Label lblFlight = ((Label)(grdScheduleinfo.Rows[i].FindControl("lblFlight")));
                                lblFlight.Text = FlightID;
                                arFlight.Add(FlightID);
                            }

                            ((TextBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("txtDeptDay")).Text = dtCurrentTable.Rows[j][3].ToString();

                            string[] HrDept = dtCurrentTable.Rows[j][4].ToString().Split(':');

                            ((TextBox)grdScheduleinfo.Rows[i].Cells[2].FindControl("txtDeptTimeHr")).Text = HrDept[0];
                            ((TextBox)grdScheduleinfo.Rows[i].Cells[2].FindControl("txtDeptTimeMin")).Text = HrDept[1];

                            string[] HrArr = dtCurrentTable.Rows[j][6].ToString().Split(':');

                            ((TextBox)grdScheduleinfo.Rows[i].Cells[4].FindControl("txtArrivalDay")).Text = dtCurrentTable.Rows[j][5].ToString();

                            ((TextBox)grdScheduleinfo.Rows[i].Cells[1].FindControl("txtArrivaltimeHr")).Text = HrArr[0];
                            ((TextBox)grdScheduleinfo.Rows[i].Cells[1].FindControl("txtArrivaltimeMin")).Text = HrArr[1];

                            string[] Weekdays = dtCurrentTable.Rows[j][7].ToString().Split(',');
                            if (Weekdays[0] == "0")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkMon")).Checked = false;
                            }
                            if (Weekdays[1] == "0")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkTues")).Checked = false;
                            }

                            if (Weekdays[2] == "0")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkwed")).Checked = false;
                            }

                            if (Weekdays[3] == "0")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkThur")).Checked = false;
                            }

                            if (Weekdays[4] == "0")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkFri")).Checked = false;
                            }

                            if (Weekdays[5] == "0")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkSat")).Checked = false;
                            }

                            if (Weekdays[6] == "0")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkSun")).Checked = false;
                            }
                            try
                            {
                                ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlAirCraft"))).SelectedValue = dtCurrentTable.Rows[i][18].ToString();
                                 showCapacityInGrid(null, null);
                                ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlTailNo"))).Text = dtCurrentTable.Rows[j][19].ToString();
                            }
                            catch (Exception ex)
                            {
                            }
                            ((TextBox)grdScheduleinfo.Rows[i].FindControl("txtCapacity")).Text = dtCurrentTable.Rows[j][17].ToString();

                            DropDownList ddlIsActive = new DropDownList();

                            ddlIsActive = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlIsActive")));

                            string Isactive = dtCurrentTable.Rows[j][11].ToString();
                            ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlStatus"))).SelectedValue = Isactive;
                            
                        }

                        ViewState["CurrentTable"] = dtCurrentTable;

                    }
                }
            }
            catch (Exception ex)
            {

            }


        }

        protected void grdFlight_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            try
            {
                pnlSchedule.Visible = true;
                rowind = Convert.ToInt32(e.NewSelectedIndex);
                //grdBillingInfo.Rows[rowindex].Cells[0].Font.Bold = true;
                fillFlightDetails(rowind);
            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlDestination2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void txtSource1_TextChanged(object sender, EventArgs e)
        {

        }

        protected void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void chkDomestic01_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void chkInternational0_CheckedChanged(object sender, EventArgs e)
        {

        }


        public void SaveGridState()
        {
            try
            {

                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                dtCurrentTable.Rows.Clear();

                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 0; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        //extract the TextBox values

                        //int j = dtCurrentTable.Rows.Count;
                        //dtCurrentTable.Rows.RemoveAt(j - 1);
                        drCurrentRow = dtCurrentTable.NewRow();
                        dtCurrentTable.Rows.Add(drCurrentRow);
                        drCurrentRow["RowNumber"] = i + 1;

                        dtCurrentTable.Rows[i]["From"] = ((DropDownList)grdScheduleinfo.Rows[i].Cells[0].FindControl("ddlFromOrigin")).Text;
                        dtCurrentTable.Rows[i]["To"] = ((DropDownList)grdScheduleinfo.Rows[i].Cells[1].FindControl("ddlToDest")).Text;

                        dtCurrentTable.Rows[i]["DeptDay"] = ((TextBox)grdScheduleinfo.Rows[i - 1].Cells[2].FindControl("txtDeptDay")).Text;
                        dtCurrentTable.Rows[i]["Dept TimeHr"] = ((TextBox)grdScheduleinfo.Rows[i - 1].Cells[2].FindControl("txtDeptTimeHr")).Text;
                        dtCurrentTable.Rows[i]["Dept TimeMin"] = ((TextBox)grdScheduleinfo.Rows[i - 1].Cells[2].FindControl("txtDeptTimeMin")).Text;

                        dtCurrentTable.Rows[i]["ArrDay"] = ((TextBox)grdScheduleinfo.Rows[i - 1].Cells[1].FindControl("txtArrivalDay")).Text;

                        dtCurrentTable.Rows[i]["Arrival TimeHr"] = ((TextBox)grdScheduleinfo.Rows[i - 1].Cells[1].FindControl("txtArrivaltimeHr")).Text;
                        dtCurrentTable.Rows[i]["Arrival TimeMin"] = ((TextBox)grdScheduleinfo.Rows[i - 1].Cells[1].FindControl("txtArrivaltimeMin")).Text;

                        dtCurrentTable.Rows[i]["chkMon"] = ((CheckBox)grdScheduleinfo.Rows[i - 1].Cells[3].FindControl("chkMon")).Text;
                        dtCurrentTable.Rows[i]["chkTues"] = ((CheckBox)grdScheduleinfo.Rows[i - 1].Cells[3].FindControl("chkTues")).Text;
                        dtCurrentTable.Rows[i]["chkwed"] = ((CheckBox)grdScheduleinfo.Rows[i - 1].Cells[3].FindControl("chkwed")).Text;
                        dtCurrentTable.Rows[i]["chkThur"] = ((CheckBox)grdScheduleinfo.Rows[i - 1].Cells[3].FindControl("chkThur")).Text;
                        dtCurrentTable.Rows[i - 1]["chkFri"] = ((CheckBox)grdScheduleinfo.Rows[i - 1].Cells[3].FindControl("chkFri")).Text;
                        dtCurrentTable.Rows[i - 1]["chkSat"] = ((CheckBox)grdScheduleinfo.Rows[i - 1].Cells[3].FindControl("chkSat")).Text;
                        dtCurrentTable.Rows[i - 1]["chkSun"] = ((CheckBox)grdScheduleinfo.Rows[i - 1].Cells[3].FindControl("chkSun")).Text;
                        dtCurrentTable.Rows[i - 1]["Status"] = ((DropDownList)grdScheduleinfo.Rows[i - 1].Cells[3].FindControl("ddlStatus")).Text;

                        //rowIndex++;


                    }

                    //    dtCurrentTable.Rows.Add(drCurrentRow);

                    ViewState["CurrentTable"] = dtCurrentTable;

                    //grdScheduleinfo.DataSource = dtCurrentTable;
                    //grdScheduleinfo.DataBind();
                }
            }
            catch (Exception)
            {


            }
        }

        #region AddnewRow To Grid
        private void AddNewRowToGrid()
        {
            try
            {
                int rowIndex = 0;


                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                dtCurrentTable.Rows.Clear();
                if (grdScheduleinfo.Rows.Count > 0)
                {
                    for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
                    {

                        //    //    //extract the TextBox values

                        drCurrentRow = dtCurrentTable.NewRow();
                        dtCurrentTable.Rows.Add(drCurrentRow);
                        // drCurrentRow["RowNumber"] = i + 1;
                        dtCurrentTable.Rows[i]["FlightID"] = ((Label)grdScheduleinfo.Rows[i].Cells[1].FindControl("lblFlight")).Text;
                        dtCurrentTable.Rows[i]["Source"] = ((DropDownList)grdScheduleinfo.Rows[i].Cells[1].FindControl("ddlFromOrigin")).Text;
                        dtCurrentTable.Rows[i]["Dest"] = ((DropDownList)grdScheduleinfo.Rows[i].Cells[2].FindControl("ddlToDest")).Text;

                        dtCurrentTable.Rows[i]["SchDeptDay"] = ((TextBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("txtDeptDay")).Text;
                        string StrDepttime = ((TextBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("txtDeptTimeHr")).Text + ":" + ((TextBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("txtDeptTimeMin")).Text;

                        dtCurrentTable.Rows[i]["SchDeptTime"] = StrDepttime;

                        dtCurrentTable.Rows[i]["SchArrDay"] = ((TextBox)grdScheduleinfo.Rows[i].Cells[4].FindControl("txtArrivalDay")).Text;

                        string schArrTime = ((TextBox)grdScheduleinfo.Rows[i].Cells[4].FindControl("txtArrivaltimeHr")).Text + ":" + ((TextBox)grdScheduleinfo.Rows[i].Cells[4].FindControl("txtArrivaltimeMin")).Text;
                        dtCurrentTable.Rows[i]["SchArrTime"] = schArrTime;

                        dtCurrentTable.Rows[i]["Frequency"] = ((CheckBox)grdScheduleinfo.Rows[i].Cells[5].FindControl("chkMon")).Checked + "," + ((CheckBox)grdScheduleinfo.Rows[i].Cells[5].FindControl("chkTues")).Checked +
                "," + ((CheckBox)grdScheduleinfo.Rows[i].Cells[5].FindControl("chkwed")).Checked + "," + ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkThur")).Checked + "," + ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkFri")).Checked +
                 "," + ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkFri")).Checked + "," + ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkSat")).Checked + "," + ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkSun")).Checked;
                        dtCurrentTable.Rows[i]["Status"] = ((DropDownList)grdScheduleinfo.Rows[i].Cells[3].FindControl("ddlStatus")).Text;


                    }
                    drCurrentRow = dtCurrentTable.NewRow();
                    dtCurrentTable.Rows.Add(drCurrentRow);


                    ViewState["CurrentTable"] = dtCurrentTable;

                    grdScheduleinfo.DataSource = dtCurrentTable;
                    grdScheduleinfo.DataBind();
                }


                //Set Previous Data on Postbacks
                // SetPreviousData();
            }
            catch (Exception ex)
            {
            }


        }
        #endregion Add new Row To Grid


        #region Add New Row
        protected void btnAddNewRow_Click(object sender, EventArgs e)
        {
            try
            {
                // LoadGridSchedule();
                AddNewRowToGrid();
                AirCraftTypeinGridview();
                LoadSourceInGridviewState();
                LoadDestinationInGridviewwithState();


                //for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                //{
                //    ddl = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlFromOrigin")));



                //}
            }
            catch (Exception ex)
            {
            }

        }
        #endregion

        #region Delete Button
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdScheduleinfo.Rows.Count > 1)
                {
                    //  AddNewRowToGrid();
                    SaveGridState();

                    //  dsSlabs = (DataSet)Session["dsSlabs"];
                    DataTable dt = (DataTable)ViewState["CurrentTable"];
                    //  DataSet dsSlabsTemp = dsSlabs.Copy();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    { //((CheckBox)grdScheduleinfo.Rows[2].FindControl("CHK")).Checked=true;

                        if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("CHK")).Checked == true)
                        {
                            string RowNumber = dt.Rows[i]["RowNumber"].ToString();

                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                if (RowNumber == dt.Rows[j]["RowNumber"].ToString())
                                {
                                    dt.Rows.Remove(dt.Rows[j]);
                                    break;
                                }
                            }
                        }
                    }

                    grdScheduleinfo.DataSource = null;
                    grdScheduleinfo.DataSource = dt;
                    grdScheduleinfo.DataBind();
                    LoadSourceInGridview();
                    LoadDestinationInGridview();


                    ViewState["CurrentTable"] = dt.Copy();
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion

        protected void txtToDate_TextChanged(object sender, EventArgs e)
        {
            try
            {

                DateTime dt1 = DateTime.ParseExact(txtFromdate.Text, "dd/MM/yyyy", null);
                DateTime dt2 = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);



                TimeSpan span = dt2 - dt1;
                if (span.TotalDays < 7)
                {

                    for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
                    {
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSun")).Checked = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkMon")).Checked = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkTues")).Checked = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkwed")).Checked = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkThur")).Checked = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkFri")).Checked = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSat")).Checked = false;

                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSun")).Enabled = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkMon")).Enabled = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkTues")).Enabled = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkwed")).Enabled = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkThur")).Enabled = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkFri")).Enabled = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSat")).Enabled = false;

                        dt1 = DateTime.ParseExact(txtFromdate.Text, "dd/MM/yyyy", null);

                        for (int idays = 0; idays < span.TotalDays + 1; idays++)
                        {


                            if (dt1.DayOfWeek.ToString() == "Sunday")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSun")).Checked = true;
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSun")).Enabled = true;
                            }
                            if (dt1.DayOfWeek.ToString() == "Monday")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkMon")).Checked = true;
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkMon")).Enabled = true;
                            }
                            if (dt1.DayOfWeek.ToString() == "Tuesday")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkTues")).Checked = true;
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkTues")).Enabled = true;
                            }
                            if (dt1.DayOfWeek.ToString() == "Wednesday")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkwed")).Checked = true;
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkwed")).Enabled = true;
                            }
                            if (dt1.DayOfWeek.ToString() == "Thursday")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkThur")).Checked = true;
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkThur")).Enabled = true;
                            }
                            if (dt1.DayOfWeek.ToString() == "Friday")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkFri")).Checked = true;
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkFri")).Enabled = true;
                            }
                            if (dt1.DayOfWeek.ToString() == "Saturday")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSat")).Checked = true;
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSat")).Enabled = true;
                            }
                            dt1 = dt1.AddDays(1);

                        }
                    }
                }
                else
                {


                    DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                    for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
                    {

                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSun")).Checked = true;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkMon")).Checked = true;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkTues")).Checked = true;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkwed")).Checked = true;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkThur")).Checked = true;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkFri")).Checked = true;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSat")).Checked = true;

                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSun")).Enabled = true;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkMon")).Enabled = true;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkTues")).Enabled = true;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkwed")).Enabled = true;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkThur")).Enabled = true;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkFri")).Enabled = true;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSat")).Enabled = true;
                    }
                    for (int j = 0; j < dtCurrentTable.Rows.Count; j++)
                    {
                        string[] Weekdays = dtCurrentTable.Rows[j][7].ToString().Split(',');
                        if (Weekdays[0] == "0")
                        {
                            ((CheckBox)grdScheduleinfo.Rows[j].Cells[3].FindControl("chkMon")).Checked = false;
                        }
                        if (Weekdays[1] == "0")
                        {
                            ((CheckBox)grdScheduleinfo.Rows[j].Cells[3].FindControl("chkTues")).Checked = false;
                        }

                        if (Weekdays[2] == "0")
                        {
                            ((CheckBox)grdScheduleinfo.Rows[j].Cells[3].FindControl("chkwed")).Checked = false;
                        }

                        if (Weekdays[3] == "0")
                        {
                            ((CheckBox)grdScheduleinfo.Rows[j].Cells[3].FindControl("chkThur")).Checked = false;
                        }

                        if (Weekdays[4] == "0")
                        {
                            ((CheckBox)grdScheduleinfo.Rows[j].Cells[3].FindControl("chkFri")).Checked = false;
                        }

                        if (Weekdays[5] == "0")
                        {
                            ((CheckBox)grdScheduleinfo.Rows[j].Cells[3].FindControl("chkSat")).Checked = false;
                        }

                        if (Weekdays[6] == "0")
                        {
                            ((CheckBox)grdScheduleinfo.Rows[j].Cells[3].FindControl("chkSun")).Checked = false;
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void txtFromdate_TextChanged(object sender, EventArgs e)
        {
            try
            {

                DateTime dt1 = DateTime.ParseExact(txtFromdate.Text, "dd/MM/yyyy", null);
                DateTime dt2 = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);



                TimeSpan span = dt2 - dt1;
                if (span.TotalDays < 7)
                {

                    for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
                    {
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSun")).Checked = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkMon")).Checked = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkTues")).Checked = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkwed")).Checked = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkThur")).Checked = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkFri")).Checked = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSat")).Checked = false;

                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSun")).Enabled = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkMon")).Enabled = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkTues")).Enabled = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkwed")).Enabled = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkThur")).Enabled = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkFri")).Enabled = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSat")).Enabled = false;

                        dt1 = DateTime.ParseExact(txtFromdate.Text, "dd/MM/yyyy", null);

                        for (int idays = 0; idays < span.TotalDays + 1; idays++)
                        {


                            if (dt1.DayOfWeek.ToString() == "Sunday")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSun")).Checked = true;
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSun")).Enabled = true;
                            }
                            if (dt1.DayOfWeek.ToString() == "Monday")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkMon")).Checked = true;
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkMon")).Enabled = true;
                            }
                            if (dt1.DayOfWeek.ToString() == "Tuesday")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkTues")).Checked = true;
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkTues")).Enabled = true;
                            }
                            if (dt1.DayOfWeek.ToString() == "Wednesday")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkwed")).Checked = true;
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkwed")).Enabled = true;
                            }
                            if (dt1.DayOfWeek.ToString() == "Thursday")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkThur")).Checked = true;
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkThur")).Enabled = true;
                            }
                            if (dt1.DayOfWeek.ToString() == "Friday")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkFri")).Checked = true;
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkFri")).Enabled = true;
                            }
                            if (dt1.DayOfWeek.ToString() == "Saturday")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSat")).Checked = true;
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSat")).Enabled = true;
                            }
                            dt1 = dt1.AddDays(1);

                        }
                    }
                }
                else
                {


                    DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                    for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
                    {

                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSun")).Checked = true;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkMon")).Checked = true;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkTues")).Checked = true;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkwed")).Checked = true;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkThur")).Checked = true;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkFri")).Checked = true;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSat")).Checked = true;

                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSun")).Enabled = true;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkMon")).Enabled = true;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkTues")).Enabled = true;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkwed")).Enabled = true;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkThur")).Enabled = true;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkFri")).Enabled = true;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSat")).Enabled = true;
                    }
                    for (int j = 0; j < dtCurrentTable.Rows.Count; j++)
                    {
                        string[] Weekdays = dtCurrentTable.Rows[j][7].ToString().Split(',');
                        if (Weekdays[0] == "0")
                        {
                            ((CheckBox)grdScheduleinfo.Rows[j].Cells[3].FindControl("chkMon")).Checked = false;
                        }
                        if (Weekdays[1] == "0")
                        {
                            ((CheckBox)grdScheduleinfo.Rows[j].Cells[3].FindControl("chkTues")).Checked = false;
                        }

                        if (Weekdays[2] == "0")
                        {
                            ((CheckBox)grdScheduleinfo.Rows[j].Cells[3].FindControl("chkwed")).Checked = false;
                        }

                        if (Weekdays[3] == "0")
                        {
                            ((CheckBox)grdScheduleinfo.Rows[j].Cells[3].FindControl("chkThur")).Checked = false;
                        }

                        if (Weekdays[4] == "0")
                        {
                            ((CheckBox)grdScheduleinfo.Rows[j].Cells[3].FindControl("chkFri")).Checked = false;
                        }

                        if (Weekdays[5] == "0")
                        {
                            ((CheckBox)grdScheduleinfo.Rows[j].Cells[3].FindControl("chkSat")).Checked = false;
                        }

                        if (Weekdays[6] == "0")
                        {
                            ((CheckBox)grdScheduleinfo.Rows[j].Cells[3].FindControl("chkSun")).Checked = false;
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void txtAutoComplete_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnclear_Click(object sender, EventArgs e)
        {
            try
            {
                pnlUpdate.Visible = false;
                pnlSchedule.Visible = false;
                pnlDestDetails.Visible = false;
                pnlMultiple.Visible = false;
                txtFlightFromdate.Text = "";
                txtFlightToDate.Text = "";
                ddlAutoSource.SelectedValue = "Select";
                ddlAutoSource.SelectedValue = "Select";

                OriginList();
                DestinationList();
                AirCraftType();
                AirCraftTypeInEdit();
                //getFligtNoSourceDestWise("All", "All");
            }
            catch (Exception ex)
            {

            }

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    if (txtFlightFromdate.Text == "" || txtFlightToDate.Text == "")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter From date and To date for datewise Schedule List";
                        txtFlightFromdate.Focus();
                        return;
                    }
                    if (txtFlightFromdate.Text.Trim() != "" || txtFlightToDate.Text.Trim() != "")
                    {
                        DateTime dt1 = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                        DateTime dt2 = DateTime.ParseExact(txtFlightToDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                        int chk = DateTime.Compare(dt1, dt2);
                        if (chk > 0)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Please enter valid To date";
                            txtFromdate.Focus();
                            return;
                        }


                    }
                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                    txtFromdate.Focus();
                    return;
                }


                if (chkDomestic.Checked == false && chkInternational.Checked == false)
                {
                    lblStatus.Text = "Kindly Check atleast one from Domestic and Inernational";
                    return;
                }

                if (chkDomestic.Checked == false && chkInternational.Checked == false)
                {
                    lblStatus.Text = "Kindly Check atleast one from Domestic and Inernational";
                    return;
                }


                btnEdit.Visible = false;
                pnlUpdate.Visible = false;
                pnlDestDetails.Visible = false;
                pnlMultiple.Visible = false;
                scheduleid = 0;
                lblStatus.Text = "";
                //if (ddlFlight.SelectedItem.Text == "All") mod
                //{
                //    LoadGridSchedule();
                //}
                //else
                //{
                //    LoadGridFlight();
                //}
                // if (txtFlightNo.Text.Trim() == "")
                AddDateWiseRowToGrid();
            }
            catch (Exception ex)
            {
            }
        }


        public DataTable GroupBy(string i_sGroupByColumn, string i_sAggregateColumn, DataTable i_dSourceTable)
        {

            DataView dv = new DataView(i_dSourceTable);

            //getting distinct values for group column
            //DataTable dtGroup =dv.ToTable(true, new string[] { i_sGroupByColumn });
            DataTable dtGroup = i_dSourceTable;
            //adding column for the row count
            dtGroup.Columns.Add("Count", typeof(int));


            //looping thru distinct values for the group, counting
            foreach (DataRow dr in dtGroup.Rows)
            {
                dr["Count"] = i_dSourceTable.Compute("Count(" + i_sAggregateColumn + ")", i_sGroupByColumn + " = '" + dr[i_sGroupByColumn] + "'");
            }

            dtGroup.Select("GROUP BY FlightID");
            //returning grouped/counted result
            return dtGroup;

        }

        #region Fill Partner Type Master
        public void FillPartnerType()
        {
            try
            {
                PartnerBAL objPM = new PartnerBAL();
                DataSet dsPartnerType = objPM.GetPartnerTypeMaster();
                if (dsPartnerType != null && dsPartnerType.Tables.Count > 0)
                {
                    if (dsPartnerType.Tables[0].Rows.Count > 0)
                    {
                        drpPartnerType.Items.Clear();
                        //drpWWR.Items.Add("Select");
                        drpPartnerType.DataSource = dsPartnerType.Tables[0];
                        drpPartnerType.DataTextField = "Code";
                        drpPartnerType.DataValueField = "Description";
                        drpPartnerType.DataBind();
                        drpPartnerType.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = "Error in Fill PartnerType!";
            }
        }
        #endregion

        #region Load Origin Dropdown
        /// <summary>
        /// Lodas location list in Origin and Destination dropdowns.
        /// </summary>
        public void LoadOrigin()
        {
            try
            {
                BookingBAL objBLL = new BookingBAL();
                DataSet ds = objBLL.GetDestinationsForSource("");
                
                objBLL = null;

                if (ds != null)
                {
                    //Dest dropdown
                    DataRow row = ds.Tables[0].NewRow();

                    row["AirportCode"] = "Select";
                    ds.Tables[0].Rows.Add(row);


                    ddlAutoSource.DataSource = ds;
                    ddlAutoSource.DataMember = ds.Tables[0].TableName;
                    ddlAutoSource.DataTextField = "AirportCode";
                    ddlAutoSource.DataValueField = "AirportCode";
                    ddlAutoSource.DataBind();

                    ddlAutoSource.Text = "Select";

                    ddlAutoDest.DataSource = ds;
                    ddlAutoDest.DataMember = ds.Tables[0].TableName;
                    ddlAutoDest.DataTextField = "AirportCode";
                    ddlAutoDest.DataValueField = "AirportCode";
                    ddlAutoDest.DataBind();

                    ddlAutoDest.Text = "Select";
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion Load Location Dropdown

        protected void drpPartnerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            AirlineScheduleBAL objBal = new AirlineScheduleBAL();
            DataSet dsData = objBal.GetPartnerDetailsOnType(drpPartnerType.SelectedItem.ToString());

            if (dsData != null && dsData.Tables.Count > 0 && dsData.Tables[0].Rows.Count > 0)
            {
                ddlPartnerCode.DataSource = dsData;
                ddlPartnerCode.DataMember = dsData.Tables[0].TableName;
                ddlPartnerCode.DataTextField = "PartnerName";
                ddlPartnerCode.DataValueField = "PartnerCode";
                ddlPartnerCode.DataBind();
                ddlPartnerCode.SelectedIndex = 0;
            }
        }

        protected void grdFlight_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdFlight.PageIndex = e.NewPageIndex;
                if (ViewState["FlightTable"] != null)
                {
                    grdFlight.DataSource = (DataTable)ViewState["FlightTable"];
                    grdFlight.DataBind();

                    LoadSourceInGridview();

                    LoadDestinationInGridview();

                    AirCraftTypeinGridview();
                    AddRowToGrid();

                    btnEdit.Enabled = true;
                }
            }
            catch (Exception)
            {

            }
        }

    }
}
