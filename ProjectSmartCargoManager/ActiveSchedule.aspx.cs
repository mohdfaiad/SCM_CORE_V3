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
    public partial class ActiveSchedule : System.Web.UI.Page
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
        static int ScheduleID = 0;
        DateTime dtPerDay = new DateTime();
        static int rowind;
        static int scheduleid;
        string strNew = "";
        #endregion

        #region Load data on form load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {           
                    LoadGridScheduleDateWise();   
                    OriginList();
                    DestinationList();
                    AirCraftType();
                    AirCraftTypeInEdit();
                    GetTailNumber();
                    getFligtNoSourceDestWise("All", "All");
                    LoadOrigin();
                    grid.Visible = false;
                    txtFlightFromdate.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                    txtFlightToDate.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                    #region Define PageSize for grid as per configuration
                    try
                    {
                        LoginBL objConfig = new LoginBL();
                        GridView1.PageSize = Convert.ToInt32(objConfig.GetMasterConfiguration("StdListPageSize"));
                        objConfig = null;
                    }
                    catch (Exception ex)
                    { }
                    #endregion
                }
                catch (Exception ex)
                {

                }
            }
            
        }
        #endregion

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


        #region Autopopulate GetFlightId

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string[] GetFlightId(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string");
            // SqlDataAdapter dad = new SqlDataAdapter("select distinct flightid from AirlineSchedule where FlightID  like '" + prefixText +  "%'",con);
            SqlDataAdapter dad = new SqlDataAdapter("select flightid from(select  distinct flightid,convert(int, substring( REPLACE(flightid, '*', ''),3,len(REPLACE(flightid, '*', '')))) as FlightNo from AirlineSchedule where FlightID!='' and FlightID  like '" + prefixText + "%')s order by FlightNo ASC", con);
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


        #region  get Flight No as per Source and Dest
        private void getFligtNoSourceDestWise(string source, string Dest)
        {
            try
            {
                DataSet ds = OBJasb.GetFlightListAsPerSourceDest(source, Dest);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            //flight dropdown
                            //DataRow row = ds.Tables[0].NewRow();

                            //row[ds.Tables[0].Columns[1].ColumnName] = "All";
                            //ds.Tables[0].Rows.Add(row);

                            //ddlFlight.DataSource = ds;
                            //ddlFlight.DataMember = ds.Tables[0].TableName;
                            //ddlFlight.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            //ddlFlight.DataTextField = ds.Tables[0].Columns[1].ColumnName;
                            //ddlFlight.DataBind();
                            //ddlFlight.Text = "All";

                            //flight dropdown
                            DataRow row = ds.Tables[0].NewRow();

                            row[ds.Tables[0].Columns[1].ColumnName] = "All";
                            ds.Tables[0].Rows.Add(row);

                            //mod
                            //ddlFlight.DataSource = ds;
                            //ddlFlight.DataMember = ds.Tables[0].TableName;
                            //ddlFlight.DataValueField = ds.Tables[0].Columns[1].ColumnName;
                            //ddlFlight.DataTextField = ds.Tables[0].Columns[1].ColumnName;
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

        #region Load date wise schedule
        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                Session["ActSch_fltAllStatus"] = null;
                Session["ActSch_fltAllStatusDataset"] = null;
                Session["ActSch_fltSpecificStatus"] = null;
                try
                {
                    lblResult.Text = "";
                    lblStatus.Text = "";
                    pnlDestDetails.Visible = false;
                    pnlSchedule.Visible = false;
                    //btnSave.Visible = false;
                    //BtnCLose.Visible = false;
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
                if (txtFlightNo.Text == "")
                {
                    //Validate date difference based on database configuration.
                    string strResult = "";
                    strResult = GetScheduleInterval(DateTime.ParseExact(txtFlightFromdate.Text.Trim(), "dd/MM/yyyy", null),
                        DateTime.ParseExact(txtFlightToDate.Text.Trim(), "dd/MM/yyyy", null));
                    GridView1.Visible = false;

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

                scheduleid = 0;
                lblStatus.Text = "";
               
                AddDateWiseRowToGrid();
            }
            catch (Exception ex)
            {
            }
            ApplyPermissions();
        }
        #endregion

        #region Get Schedule Interval
        public string GetScheduleInterval(DateTime FromDate, DateTime ToDate)
        {
            double DaysConfigured = 0;
            double intReportInterval = (ToDate - FromDate).TotalDays + 1;
            LoginBL objBL = new LoginBL();

            try
            {
                string strOutput = "";
                if (Session["ActiveSchInterval"] == null)
                {
                    strOutput = objBL.GetMasterConfiguration("ActiveSchInterval");
                    Session["ActiveSchInterval"] = strOutput;
                }
                else
                {
                    strOutput = Session["ActiveSchInterval"].ToString();
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
                //if (txtAutoSource.Text.Trim() != "")
                //{
                //    Source = txtAutoSource.Text;
                //}
                //if (txtAutoDest.Text.Trim() != "")
                //{
                //    Dest = txtAutoDest.Text.Trim();
                //}

                if (ddlAutoSource.Text.Trim() != "Select")
                {
                    Source = ddlAutoSource.Text;
                }
                if (ddlAutoDest.Text.Trim() != "Select")
                {
                    Dest = ddlAutoDest.Text;
                }
                string strFlightStatus="";
                 strNew = "";

            foreach (ListItem item in CheckBoxList1.Items)
            {
              if (item.Selected)
              {
                  if (item.Text != "All")
                  {
                      string text = "";
                      if (item.Text == "Manifested")
                          text = "M";
                      if (item.Text == "Departed")
                          text = "D";
                      else if (item.Text == "Reopened")
                          text = "R";

                      
                      strFlightStatus += text +",";

                    
                  }
                  else if (item.Text == "All")
                  {
                      strFlightStatus = "All";
                  }
                  if (item.Text == "New" && strFlightStatus != "All,")
                  {
                      strNew  = "N";
                  }
                  
              }
            }
            if (strFlightStatus.Contains("All"))
                strFlightStatus = "All";
            else
            {
                strFlightStatus = strFlightStatus.Remove(strFlightStatus.Length - 1);
            }

                
            DataSet ds = OBJasb.GetDateWiseAirlineScheduleWithFlightStatus(Source, Dest, FlightNo, ddlAirCraftType.SelectedValue, Flightfromdate, FlightToDate, ddlStatus.SelectedValue, strdomestic, strFlightStatus);

                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (strFlightStatus.Contains("All"))
                                AddDateWiseRowToGrid(ds);
                                else
                                   AddDateWiseRowToGridFlightStatus(ds);
                                // Response.Redirect("ShowDateWiseSchedule.aspx",false);
                              

                            }
                            else
                            {
                                pnlSchedule.Visible = false;
                                lblStatus.ForeColor = Color.Brown;
                                lblStatus.Text = "Schedule Not Available for selected criteria.";
                             
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

        #region Add datewise dataTo Grid
        private void AddDateWiseRowToGrid(DataSet ds)
        {
            try
            {
                DateTime dt1 = new DateTime();
                DateTime dt2 = new DateTime();
                ArrayList arsource = new ArrayList();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                DataTable Dtemp = ds.Tables[0];
                                DataView view = new DataView();
                                try
                                {
                                    Dtemp.Columns.Add("tab_index", typeof(float));
                                    for (int row = 0; row < Dtemp.Rows.Count; row++)
                                    {
                                        string Weekdays = Dtemp.Rows[row][7].ToString();
                                        string scheduleID = Dtemp.Rows[row]["ScheduleID"].ToString();
                                        string ActDeptTimr = Dtemp.Rows[row][4].ToString();
                                        if (!arFlight.Contains(scheduleID))
                                        {
                                            arFlight.Add(scheduleID);// + " " + Weekdays);
                                            ActDeptTimr = ActDeptTimr.Replace(':', '.');
                                            Dtemp.Rows[row]["tab_index"] = ActDeptTimr;
                                        }
                                        else
                                        {
                                            ActDeptTimr = Dtemp.Rows[row - 1]["tab_index"].ToString();
                                            Dtemp.Rows[row]["tab_index"] = ActDeptTimr;
                                        }
                                    }
                                    arFlight.Clear();
                                    // By default, the first column sorted ascending.
                                    view = Dtemp.DefaultView;
                                    view.Sort = "tab_index";
                                }
                                catch (Exception)
                                {
                                }
                                ViewState["CurrentTable"] = view.ToTable(); //Dtemp;//ds.Tables[0]; mod vikas 1Aug
                                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                                DataRow drCurrentRow = null;
                                DataRow drCurrentRow1 = null;

                                DataTable dtCopy = new DataTable();
                                dtCopy = dtCurrentTable.Clone();

                                if (dtCurrentTable.Rows.Count > 0)
                                {
                                    int k = 0, r = 0;
                                    try
                                    {
                                        string fromdt = txtFlightFromdate.Text;
                                        string ToDate = txtFlightToDate.Text.Trim();
                                        DateTime getFromDt = DateTime.ParseExact(txtFlightFromdate.Text.Trim(), "dd/MM/yyyy", null);
                                        DateTime getToDt = DateTime.ParseExact(txtFlightToDate.Text.Trim(), "dd/MM/yyyy", null);
                                        //fromdt = getFromDt.ToString("MM/dd/yyyy");
                                        //ToDate=getToDt.ToString("MM/dd/yyyy");
                                        
                                        //getFromDt = Convert.ToDateTime(fromdt);
                                        //getToDt = Convert.ToDateTime(ToDate);

                                        TimeSpan t = getToDt - getFromDt;
                                        double NrOfDays = t.TotalDays;
                                        for (int days = 0; days <= NrOfDays; days++)
                                        {
                                            for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                                            {
                                                DateTime dtMatchFromDt = Convert.ToDateTime(dtCurrentTable.Rows[i][8].ToString());
                                                DateTime dtMatchToDt = Convert.ToDateTime(dtCurrentTable.Rows[i][9].ToString());
                                                if (dtMatchFromDt <= getFromDt && dtMatchToDt >= getFromDt)
                                                {

                                                    string FlightID1 = dtCurrentTable.Rows[i][0].ToString();
                                                    
                                                    //extract the TextBox values
                                                    string FlightID = dtCurrentTable.Rows[i][0].ToString();
                                                    string chkfrequency = dtCurrentTable.Rows[i][7].ToString();

                                                    string[] chkWeekdays = dtCurrentTable.Rows[i][7].ToString().Split(',');
                                                    string wkday = dtCurrentTable.Rows[i][7].ToString();

                                                    string org = dtCurrentTable.Rows[i][1].ToString();

                                                    //int chk = DateTime.Compare(getFromDt, dt1);

                                                    string dateday = getFromDt.DayOfWeek.ToString();

                                                    int dateok = 0;

                                                    if (dateday == "Monday")
                                                        dateok = 0;
                                                    if (dateday == "Tuesday")
                                                        dateok = 1;
                                                    if (dateday == "Wednesday")
                                                        dateok = 2;
                                                    if (dateday == "Thursday")
                                                        dateok = 3;
                                                    if (dateday == "Friday")
                                                        dateok = 4;
                                                    if (dateday == "Saturday")
                                                        dateok = 5;
                                                    if (dateday == "Sunday")
                                                        dateok = 6;

                                                    if (chkWeekdays[dateok].ToString() == "1")
                                                    {
                                                        drCurrentRow = dtCurrentTable.NewRow();
                                                        drCurrentRow1 = dtCopy.NewRow();
                                                        dtCopy.Rows.Add(drCurrentRow1);

                                                        Label ddlFromOrigin = new Label();
                                                        ddlFromOrigin.Text = dtCurrentTable.Rows[i][1].ToString();
                                                        dtCopy.Rows[k][1] = dtCurrentTable.Rows[i][1].ToString();
                                                        Label ddlDest = new Label();
                                                        dtCopy.Rows[k][2] = dtCurrentTable.Rows[i][2].ToString();
                                                        ddlDest.Text = dtCurrentTable.Rows[i][2].ToString();
                                                        if (!arFlight.Contains(FlightID + " " + wkday))//FlightID))//&& (!arOrigin.Contains(dtCurrentTable.Rows[i][1].ToString())))
                                                        {
                                                            arFlight.Add(FlightID + " " + wkday);
                                                        }

                                                        DateTime dtaddfrm = new DateTime();
                                                        dtaddfrm = Convert.ToDateTime(getFromDt.ToString());
                                                        try
                                                        {
                                                            if (dtCurrentTable.Rows[i][3].ToString() == "2")
                                                            {
                                                                dtaddfrm = dtaddfrm.AddDays(1);
                                                                dtCopy.Rows[k][3] = dtaddfrm.ToString("dd/MM/yyyy");
                                                            }
                                                            else
                                                            {
                                                                dtCopy.Rows[k][3] = getFromDt.ToString("dd/MM/yyyy");
                                                            }

                                                            string Hrdept = dtCurrentTable.Rows[i][4].ToString();
                                                            //                  dtCurrentTable.Rows[i][4].ToString().Split('.');
                                                            dtCopy.Rows[k][4] = " " +Hrdept;
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                        }
                                                        if (!arFlight.Contains(FlightID + " " + dtCopy.Rows[k][3].ToString()))//FlightID))//&& (!arOrigin.Contains(dtCurrentTable.Rows[i][1].ToString())))
                                                        {
                                                            arFlight.Add(FlightID + " " + dtCopy.Rows[k][3].ToString());
                                                            dtCopy.Rows[k][0] = FlightID;
                                                        }
                                                        dtaddfrm = Convert.ToDateTime(getFromDt.ToString());
                                                        //If arrival day is previous day.
                                                        if (dtCurrentTable.Rows[i + r][5].ToString() == "-1")
                                                        {
                                                            dtaddfrm = dtaddfrm.AddDays(-1);
                                                            dtCopy.Rows[k][5] = dtaddfrm.ToString("dd/MM/yyyy");
                                                        }
                                                        else
                                                        {// If arrival day is today or future day
                                                            dtaddfrm = dtaddfrm.AddDays(int.Parse(dtCurrentTable.Rows[i + r][5].ToString()) -1);
                                                            dtCopy.Rows[k][5] = dtaddfrm.ToString("dd/MM/yyyy");
                                                        }

                                                        string[] Hrarr = dtCurrentTable.Rows[i + r][6].ToString().Split(':');
                                                        string strArrTime = Hrarr[0].PadLeft(2, '0') + ":" + Hrarr[1].PadLeft(2, '0');

                                                        dtCopy.Rows[k][6] = " " +strArrTime; 

                                                        Label ddlIsActive = new Label();
                                                        string Isactive = dtCurrentTable.Rows[i][11].ToString();
                                                        dtCopy.Rows[k][11] = dtCurrentTable.Rows[i][11].ToString();
                                                        dtCopy.Rows[k][17] = dtCurrentTable.Rows[i + r][17].ToString();
                                                        dtCopy.Rows[k][16] = dtCurrentTable.Rows[i + r][16].ToString();
                                                        dtCopy.Rows[k][18] = dtCurrentTable.Rows[i + r][18].ToString();
                                                        dtCopy.Rows[k][19] = dtCurrentTable.Rows[i + r][19].ToString();
                                                        //dtCopy.Rows[k][20] = dtCurrentTable.Rows[i + r][20].ToString();
                                                        k = k + 1;
                                                    }
                                                    else
                                                    {
                                                    }
                                                }
                                            }
                                            getFromDt = getFromDt.AddDays(1);
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    //dtCurrentTable.Rows.Add(drCurrentRow);
                                    ////   dtCopy.Rows.Add(drCurrentRow1);
                                    //ViewState["CurrentTable"] = dtCopy;//dtCurrentTable;

                                }

                                GridView1.DataSource = null;
                                GridView1.DataSource = dtCopy; // dtCurrentTable;
                                GridView1.DataBind();
                                
                                GridView1.Visible = true;

                                arFlight.Clear();

                                dtCopy = SetFlightsGridData(dtCopy,ds);
                                                                
                                GridView1.DataSource = null;
                                GridView1.DataSource = dtCopy; 
                                GridView1.DataBind();
                                Session["ActSch_fltAllStatus"] = dtCopy.Copy();
                                Session["ActSch_fltAllStatusDataset"] = ds.Copy();
                                pnlSchedule.Visible = false;
                                pnlDestDetails.Visible = false;

                            }
                            else
                            {

                            }
                        }
                        else
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion Add datewise dataTo Grid

        private DataTable SetFlightsGridData(DataTable dtCopy, DataSet ds)
        {
            try
            {
                ArrayList arsource = new ArrayList();
                for (int addrow = 0; addrow < GridView1.Rows.Count; addrow++)
                {

                    int j = GridView1.PageSize * GridView1.PageIndex + addrow;

                    Label ddlFromOrigin = new Label();
                    Label lblSchedule = (Label)(GridView1.Rows[addrow].FindControl("lblFromOrigin"));
                    lblSchedule.Text = dtCopy.Rows[j][18].ToString();
                    ddlFromOrigin = ((Label)(GridView1.Rows[addrow].FindControl("lblFromOrigin")));
                    ddlFromOrigin.Text = dtCopy.Rows[j][1].ToString();

                    Label ddlDest = new Label();
                    ddlDest = ((Label)(GridView1.Rows[addrow].FindControl("lblToDest")));
                    ddlDest.Text = dtCopy.Rows[j][2].ToString();

                    string FlightID = dtCopy.Rows[j][0].ToString();
                    if (!arFlight.Contains(FlightID))//+ " " + CheckWeekdays))//FlightID))//&& (!arOrigin.Contains(dtCurrentTable.Rows[i][1].ToString())))
                    {
                        Label lblFlight = ((Label)(GridView1.Rows[addrow].FindControl("lblFlight")));
                        lblFlight.Text = FlightID;
                        arFlight.Add(FlightID);// + " " + CheckWeekdays);
                    }

                    ((Label)GridView1.Rows[addrow].FindControl("lblFromDt")).Text = dtCopy.Rows[j][4].ToString();
                    if (!arsource.Contains(dtCopy.Rows[j][1].ToString()))
                    {
                        arsource.Add(dtCopy.Rows[j][1].ToString());
                    }
                    else
                    {
                        ((Label)(GridView1.Rows[addrow].FindControl("lblFlight"))).Text = FlightID;
                    }
                    string[] HrDept = dtCopy.Rows[j][4].ToString().Split(':');
                    string strDeptTime = HrDept[0].PadLeft(3, '0') + ":" + HrDept[1].PadLeft(2, '0');
                    ((Label)GridView1.Rows[addrow].FindControl("lblDepttime")).Text = " - " + strDeptTime;
                    ((Label)GridView1.Rows[addrow].FindControl("lblToDt")).Text = dtCopy.Rows[j][6].ToString();
                    string[] HrArr = dtCopy.Rows[j][6].ToString().Split(':');
                    string strArrtime = HrArr[0].PadLeft(3, '0') + ":" + HrArr[1].PadLeft(2, '0');
                    ((Label)GridView1.Rows[addrow].FindControl("lblArrtime")).Text = " - " + strArrtime;
                    Label ddlIsActive = new Label();
                    ddlIsActive = ((Label)(GridView1.Rows[addrow].FindControl("lblStatus")));
                    string Isactive = dtCopy.Rows[j][11].ToString();
                    ((Label)(GridView1.Rows[addrow].FindControl("lblAirCraft"))).Text = dtCopy.Rows[j][17].ToString();
                    ((Label)(GridView1.Rows[addrow].FindControl("lblTailNo"))).Text = dtCopy.Rows[j][18].ToString();
                    ((Label)(GridView1.Rows[addrow].FindControl("lblCapacity"))).Text = dtCopy.Rows[j][16].ToString();

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        bool Isavailable = false;
                        for (int k = 0; k < ds.Tables[1].Rows.Count; k++)
                        {
                            DateTime dtaddfrm = new DateTime();
                            DateTime.TryParseExact(dtCopy.Rows[j][3].ToString(),"dd/MM/yyyy",null, 
                                System.Globalization.DateTimeStyles.None,out dtaddfrm);
                            DateTime dtManofest = new DateTime();
                            DateTime.TryParseExact(ds.Tables[1].Rows[k]["Date"].ToString(),"dd/MM/yyyy",null,
                                 System.Globalization.DateTimeStyles.None, out dtManofest);

                            string FlightId = dtCopy.Rows[j][0].ToString();
                            string Maniflight = ds.Tables[1].Rows[k]["FltNo"].ToString();

                            string frdate = dtCopy.Rows[j][3].ToString();
                            string manidate = dtManofest.ToString("dd/MM/yyyy");
                            string FrmSource = dtCopy.Rows[j]["Source"].ToString();
                            string FRMFlightPOL = ds.Tables[1].Rows[k]["POL"].ToString();

                            if (frdate == manidate)
                            {
                                if (FlightId == Maniflight && FrmSource == FRMFlightPOL)
                                {
                                    ((Label)(GridView1.Rows[addrow].FindControl("lblOperationStatus"))).Text = ds.Tables[1].Rows[k]["OperationStatus"].ToString();
                                    dtCopy.Rows[j]["OperationStatus"] = ds.Tables[1].Rows[k]["OperationStatus"].ToString();
                                    Isavailable = true;
                                    break;
                                }
                                else
                                {
                                    ((Label)(GridView1.Rows[addrow].FindControl("lblOperationStatus"))).Text = "New";
                                    dtCopy.Rows[j]["OperationStatus"] = "New";

                                }
                            }
                            else
                            {
                                ((Label)(GridView1.Rows[addrow].FindControl("lblOperationStatus"))).Text = "New";
                                dtCopy.Rows[j]["OperationStatus"] = "New";

                            }
                        }
                        if (strNew == "N" && Isavailable == true)
                        {
                            dtCopy.Rows[j].Delete();
                        }
                    }
                    else
                    {
                        ((Label)(GridView1.Rows[addrow].FindControl("lblOperationStatus"))).Text = "New";
                        dtCopy.Rows[j]["OperationStatus"] = "New";
                    }
                }
            }
            catch (Exception ex)
            {
            }
            dtCopy.AcceptChanges();
            return (dtCopy);
        }

        #region Add datewise dataTo Grid flight Status
        private void AddDateWiseRowToGridFlightStatus(DataSet ds)
        {
            try
            {

                DateTime dt1 = new DateTime();
                DateTime dt2 = new DateTime();

                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[1].Rows.Count > 0)
                            {

                                DataTable Dtemp = ds.Tables[1];
                                DataView view = new DataView();
                                try
                                {
                                    Dtemp.Columns.Add("tab_index", typeof(float));
                                    for (int row = 0; row < Dtemp.Rows.Count; row++)
                                    {
                                        string Weekdays = Dtemp.Rows[row][7].ToString();
                                        string scheduleID = Dtemp.Rows[row]["ScheduleID"].ToString();
                                        string ActDeptTimr = Dtemp.Rows[row][4].ToString();
                                        string FlightID = Dtemp.Rows[row]["FlightID"].ToString();
                                        if (ActDeptTimr == "" || FlightID == "")
                                        {   //Delete row from schedule which is not present in AirlineSchedule table.
                                            Dtemp.Rows[row].Delete();
                                            Dtemp.AcceptChanges();
                                            row = row - 1;
                                            continue;
                                        }
                                        
                                        if (!arFlight.Contains(scheduleID))// + " " + Weekdays))//FlightID))//&& (!arOrigin.Contains(dtCurrentTable.Rows[i][1].ToString())))
                                        {
                                            arFlight.Add(scheduleID);// + " " + Weekdays);
                                            ActDeptTimr = ActDeptTimr.Replace(':', '.');
                                            // ActDeptTimr = Dtemp.Rows[row][4].ToString();
                                            Dtemp.Rows[row]["tab_index"] = ActDeptTimr;
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
                                }
                                catch (Exception ex)
                                {

                                }
                                ViewState["CurrentTable"] = view.ToTable(); //Dtemp;//ds.Tables[0]; mod vikas 1Aug
                                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];

                                DataTable dtCopy = new DataTable();
                                dtCopy = dtCurrentTable.Clone();

                                if (dtCurrentTable.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                                    {

                                        DateTime dtadd = new DateTime();
                                        dtadd = Convert.ToDateTime(dtCurrentTable.Rows[i]["Date"].ToString());
                                        dtCurrentTable.Rows[i]["SchDeptTime"] = dtadd.ToString("dd/MM/yyyy") + " - " + dtCurrentTable.Rows[i]["SchDeptTime"].ToString();
                                        dtCurrentTable.Rows[i]["SchArrTime"] = dtadd.ToString("dd/MM/yyyy") + " - " + dtCurrentTable.Rows[i]["SchArrTime"].ToString();
                                        dtCurrentTable.Rows[i]["SchDeptDay"] = "";
                                        dtCurrentTable.Rows[i]["SchArrDay"] = "";
                                    }

                                }

                                #region Commented
                                //    //extract the TextBox values

                                //    // string fromdt = txtFromdate.Text;
                                //    int k = 0, r = 0, Addrecord = 0; ;
                                //    try
                                //    {
                                //        string fromdt = txtFlightFromdate.Text;// DateTime.ParseExact(txtFlightFromdate.Text.Trim(), "dd/MM/yyyy", null); //Session["FromDate"].ToString();
                                //        string ToDate = txtFlightToDate.Text.Trim();//Session["ToDate"].ToString();
                                //        DateTime getFromDt = DateTime.ParseExact(txtFlightFromdate.Text.Trim(), "dd/MM/yyyy", null);//Convert.ToDateTime(fromdt.ToString("MM/dd/yyyy"));
                                //        DateTime getToDt = DateTime.ParseExact(txtFlightToDate.Text.Trim(), "dd/MM/yyyy", null);
                                //        fromdt = getFromDt.ToString("MM/dd/yyyy");
                                //        ToDate = getToDt.ToString("MM/dd/yyyy");

                                //        getFromDt = Convert.ToDateTime(fromdt);
                                //        getToDt = Convert.ToDateTime(ToDate);




                                //        TimeSpan t = getToDt - getFromDt;
                                //        double NrOfDays = t.TotalDays;
                                //        for (int days = 0; days <= NrOfDays; days++)
                                //        {
                                //            for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                                //            {

                                //                DateTime dtMatchFromDt = Convert.ToDateTime(dtCurrentTable.Rows[i][8].ToString());
                                //                DateTime dtMatchToDt = Convert.ToDateTime(dtCurrentTable.Rows[i][9].ToString());
                                //                if (dtMatchFromDt <= getFromDt && dtMatchToDt >= getFromDt)
                                //                {

                                //                    string FlightID1 = dtCurrentTable.Rows[i][0].ToString();
                                //                    //lblScheduleID

                                //                    // string[] chkWeekdays = dtCurrentTable.Rows[i][7].ToString().Split(',');

                                //                    Addrecord = 0;

                                //                    //extract the TextBox values





                                //                    string FlightID = dtCurrentTable.Rows[i][0].ToString();
                                //                    int records = 0;
                                //                    string chkfrequency = dtCurrentTable.Rows[i][7].ToString();


                                //                    string[] chkWeekdays = dtCurrentTable.Rows[i][7].ToString().Split(',');
                                //                    string wkday = dtCurrentTable.Rows[i][7].ToString();

                                //                    string org = dtCurrentTable.Rows[i][1].ToString();

                                //                    int chk = DateTime.Compare(getFromDt, dt1);

                                //                    string dateday = getFromDt.DayOfWeek.ToString();


                                //                    int dateok = 0;

                                //                    if (dateday == "Monday")
                                //                        dateok = 0;
                                //                    if (dateday == "Tuesday")
                                //                        dateok = 1;
                                //                    if (dateday == "Wednesday")
                                //                        dateok = 2;
                                //                    if (dateday == "Thursday")
                                //                        dateok = 3;
                                //                    if (dateday == "Friday")
                                //                        dateok = 4;
                                //                    if (dateday == "Saturday")
                                //                        dateok = 5;
                                //                    if (dateday == "Sunday")
                                //                        dateok = 6;

                                //                    if (chkWeekdays[dateok].ToString() == "1")
                                //                    {
                                //                        drCurrentRow = dtCurrentTable.NewRow();
                                //                        drCurrentRow1 = dtCopy.NewRow();
                                //                        dtCopy.Rows.Add(drCurrentRow1);

                                //                        // lblDate.Text = getFromDt.ToString();
                                //                        //  dtCopy.Rows[k][0] = getFromDt.ToString("dd/MM/yyyy");


                                //                        Label ddlFromOrigin = new Label();

                                //                        // ddlFromOrigin = ((Label)(GridView1.Rows[k].FindControl("lblFromOrigin")));

                                //                        ddlFromOrigin.Text = dtCurrentTable.Rows[i][1].ToString();


                                //                        dtCopy.Rows[k][1] = dtCurrentTable.Rows[i][1].ToString();


                                //                        Label ddlDest = new Label();

                                //                        //  ddlDest = ((Label)(GridView1.Rows[k].FindControl("lblToDest")));
                                //                        dtCopy.Rows[k][2] = dtCurrentTable.Rows[i][2].ToString();

                                //                        ddlDest.Text = dtCurrentTable.Rows[i][2].ToString();


                                //                        //string FlightID = dtCurrentTable.Rows[i][0].ToString();

                                //                        //string CheckWeekdays = dtCurrentTable.Rows[i][7].ToString();

                                //                        //     string flightandsource = FlightID + dtCurrentTable.Rows[i][1].ToString();
                                //                        if (!arFlight.Contains(FlightID + " " + wkday))//FlightID))//&& (!arOrigin.Contains(dtCurrentTable.Rows[i][1].ToString())))
                                //                        {
                                //                            arFlight.Add(FlightID + " " + wkday);

                                //                        }

                                //                        DateTime dtaddfrm = new DateTime();
                                //                        dtaddfrm = Convert.ToDateTime(getFromDt.ToString());
                                //                        string FlightStatus = "";

                                //                        try
                                //                        {
                                //                            if (dtCurrentTable.Rows[i][3].ToString() == "2")
                                //                            {
                                //                                dtaddfrm = dtaddfrm.AddDays(1);
                                //                                dtCopy.Rows[k][3] = dtaddfrm.ToString("dd/MM/yyyy");

                                //                            }
                                //                            else
                                //                                dtCopy.Rows[k][3] = getFromDt.ToString("dd/MM/yyyy");

                                //                            //  string[] HrDept = dtCurrentTable.Rows[i][4].ToString().Split(':');
                                //                            //  ((Label)GridView1.Rows[k].FindControl("lblDepttime")).Text = " - " + dtCurrentTable.Rows[i+r][4].ToString();
                                //                            dtCopy.Rows[k][4] = " - " + dtCurrentTable.Rows[i][4].ToString();






                                //                        }

                                //                        catch (Exception ex)
                                //                        {
                                //                        }


                                //                        if (!arFlight.Contains(FlightID + " " + dtCopy.Rows[k][3].ToString()))//FlightID))//&& (!arOrigin.Contains(dtCurrentTable.Rows[i][1].ToString())))
                                //                        {
                                //                            //if ((!arOrigin.Contains(dtCurrentTable.Rows[i][1].ToString())))
                                //                            //{

                                //                            // Label lblFlight = ((Label)(GridView1.Rows[k].FindControl("lblFlight")));
                                //                            //lblFlight.Text = FlightID;
                                //                            arFlight.Add(FlightID + " " + dtCopy.Rows[k][3].ToString());

                                //                            dtCopy.Rows[k][0] = FlightID;
                                //                            //  arFlight.Add(flightandsource);
                                //                            // arOrigin.Add(dtCurrentTable.Rows[i][1].ToString());
                                //                            //}
                                //                        }



                                //                        dtaddfrm = Convert.ToDateTime(getFromDt.ToString());
                                //                        if (dtCurrentTable.Rows[i + r][5].ToString() == "2")
                                //                        {
                                //                            dtaddfrm = dtaddfrm.AddDays(1);
                                //                            dtCopy.Rows[k][5] = dtaddfrm.ToString("dd/MM/yyyy");
                                //                        }
                                //                        else
                                //                            dtCopy.Rows[k][5] = getFromDt.ToString("dd/MM/yyyy");


                                //                        //   ((Label)GridView1.Rows[k].FindControl("lblArrtime")).Text = " - " + dtCurrentTable.Rows[i+r][6].ToString();
                                //                        dtCopy.Rows[k][6] = " - " + dtCurrentTable.Rows[i + r][6].ToString(); ;

                                //                        //((TextBox)GridView1.Rows[days].Cells[1].FindControl("txtArrivaltimeHr")).Text = HrArr[0];
                                //                        //((TextBox)GridView1.Rows[days].Cells[1].FindControl("txtArrivaltimeMin")).Text = HrArr[1];



                                //                        Label ddlIsActive = new Label();

                                //                        //   ddlIsActive = ((Label)(GridView1.Rows[k].FindControl("lblStatus")));

                                //                        string Isactive = dtCurrentTable.Rows[i][11].ToString();
                                //                        dtCopy.Rows[k][11] = dtCurrentTable.Rows[i][11].ToString();

                                //                        //((Label)(GridView1.Rows[k].FindControl("lblAirCraft"))).Text = dtCurrentTable.Rows[i+r][17].ToString();
                                //                        dtCopy.Rows[k][17] = dtCurrentTable.Rows[i + r][17].ToString();

                                //                        //  ((Label)(GridView1.Rows[k].FindControl("lblCapacity"))).Text = dtCurrentTable.Rows[i+r][16].ToString();
                                //                        dtCopy.Rows[k][16] = dtCurrentTable.Rows[i + r][16].ToString();

                                //                        dtCopy.Rows[k][18] = dtCurrentTable.Rows[i + r][18].ToString();

                                //                        k = k + 1;

                                //                    }
                                //                    else
                                //                    {
                                //                    }

                                //                    // }

                                //                    //}

                                //                }
                                //                //i = i + r - 1;
                                //            }
                                //            getFromDt = getFromDt.AddDays(1);
                                //        }
                                //    }
                                //    catch (Exception ex)
                                //    {

                                //    }


                                //    //dtCurrentTable.Rows.Add(drCurrentRow);
                                //    ////   dtCopy.Rows.Add(drCurrentRow1);
                                //    //ViewState["CurrentTable"] = dtCopy;//dtCurrentTable;


                                //}
                                #endregion Commented

                                GridView1.DataSource = null;
                                GridView1.DataSource =dtCurrentTable;
                                GridView1.DataBind();

                                Session["ActSch_fltSpecificStatus"] = dtCurrentTable;

                                GridView1.Visible = true;
                                arFlight.Clear();

                                #region Commented
                                //try
                                //{
                                //    for (int addrow = 0; addrow < GridView1.Rows.Count; addrow++)
                                //    {


                                //        Label ddlFromOrigin = new Label();


                                //        Label lblSchedule = (Label)(GridView1.Rows[addrow].FindControl("lblFromOrigin"));
                                //        lblSchedule.Text = dtCopy.Rows[addrow][18].ToString();

                                //        ddlFromOrigin = ((Label)(GridView1.Rows[addrow].FindControl("lblFromOrigin")));

                                //        ddlFromOrigin.Text = dtCopy.Rows[addrow][1].ToString();


                                //        Label ddlDest = new Label();

                                //        ddlDest = ((Label)(GridView1.Rows[addrow].FindControl("lblToDest")));
                                //        ddlDest.Text = dtCopy.Rows[addrow][2].ToString();


                                //        string FlightID = dtCopy.Rows[addrow][0].ToString();

                                //        //string CheckWeekdays = dtCurrentTable.Rows[i][7].ToString();

                                //        //     string flightandsource = FlightID + dtCurrentTable.Rows[i][1].ToString();
                                //        if (!arFlight.Contains(FlightID))//+ " " + CheckWeekdays))//FlightID))//&& (!arOrigin.Contains(dtCurrentTable.Rows[i][1].ToString())))
                                //        {
                                //            //if ((!arOrigin.Contains(dtCurrentTable.Rows[i][1].ToString())))
                                //            //{

                                //            Label lblFlight = ((Label)(GridView1.Rows[addrow].FindControl("lblFlight")));
                                //            lblFlight.Text = FlightID;
                                //            arFlight.Add(FlightID);// + " " + CheckWeekdays);


                                //            //  arFlight.Add(flightandsource);
                                //            // arOrigin.Add(dtCurrentTable.Rows[i][1].ToString());
                                //            //}
                                //        }


                                //        ((Label)GridView1.Rows[addrow].FindControl("lblFromDt")).Text = dtCopy.Rows[addrow][4].ToString();
                                //        if (!arsource.Contains(dtCopy.Rows[addrow][1].ToString()))
                                //        {
                                //            // ((Label)(GridView1.Rows[addrow].FindControl("lblFlight"))).Text= FlightID;
                                //            arsource.Add(dtCopy.Rows[addrow][1].ToString());
                                //        }
                                //        else
                                //        {
                                //            ((Label)(GridView1.Rows[addrow].FindControl("lblFlight"))).Text = FlightID;
                                //        }


                                //        //  string[] HrDept = dtCurrentTable.Rows[i][4].ToString().Split(':');
                                //        ((Label)GridView1.Rows[addrow].FindControl("lblDepttime")).Text = " - " + dtCopy.Rows[addrow][5].ToString();


                                //        ((Label)GridView1.Rows[addrow].FindControl("lblToDt")).Text = dtCopy.Rows[addrow][6].ToString();


                                //        ((Label)GridView1.Rows[addrow].FindControl("lblArrtime")).Text = " - " + dtCopy.Rows[addrow][7].ToString();

                                //        Label ddlIsActive = new Label();

                                //        ddlIsActive = ((Label)(GridView1.Rows[addrow].FindControl("lblStatus")));

                                //        string Isactive = dtCopy.Rows[addrow][11].ToString();
                                //        //dtCopy.Rows[k][11] = dtCurrentTable.Rows[i][11].ToString();

                                //        ((Label)(GridView1.Rows[addrow].FindControl("lblAirCraft"))).Text = dtCopy.Rows[addrow][17].ToString();
                                //        //dtCopy.Rows[k][17] = dtCurrentTable.Rows[i + r][17].ToString();

                                //        ((Label)(GridView1.Rows[addrow].FindControl("lblCapacity"))).Text = dtCopy.Rows[addrow][16].ToString();



                                //        if (ds.Tables[1].Rows.Count > 0)
                                //        {
                                //            for (int k = 0; k < ds.Tables[1].Rows.Count; k++)
                                //            {
                                //                //for (int j = 0; j < dtCopy.Rows.Count; j++)
                                //                //{
                                //                DateTime dtaddfrm = new DateTime();
                                //                dtaddfrm = Convert.ToDateTime(dtCopy.Rows[addrow][3].ToString());
                                //                DateTime dtManofest = new DateTime();
                                //                dtManofest = Convert.ToDateTime(ds.Tables[1].Rows[k][1].ToString());

                                //                string FlightStatus = "";
                                //                string FlightId = dtCopy.Rows[addrow][0].ToString();
                                //                string Maniflight = ds.Tables[1].Rows[k][0].ToString();

                                //                string frdate = dtCopy.Rows[addrow][3].ToString();
                                //                string manidate = dtManofest.ToString("dd/MM/yyyy");
                                //                string FrmSource = dtCopy.Rows[addrow]["Source"].ToString();
                                //                string FRMFlightPOL = ds.Tables[1].Rows[k]["POL"].ToString();

                                //                //DateTime  dt
                                //                //DateTime dtFlightdate = new DateTime();
                                //                //dtFlightdate = Convert.ToDateTime(ds.Tables[1].Rows[k][1].ToString());
                                //                if (frdate == manidate)
                                //                {
                                //                    if (FlightId == Maniflight && FrmSource == FRMFlightPOL)
                                //                    {
                                //                        ((Label)(GridView1.Rows[addrow].FindControl("lblOperationStatus"))).Text = ds.Tables[1].Rows[k][2].ToString();
                                //                        dtCopy.Rows[addrow]["OperationStatus"] = ds.Tables[1].Rows[k][2].ToString();
                                //                        break;
                                //                    }
                                //                    else
                                //                    {
                                //                        ((Label)(GridView1.Rows[addrow].FindControl("lblOperationStatus"))).Text = "New";
                                //                        dtCopy.Rows[addrow]["OperationStatus"] = "New";

                                //                    }
                                //                }
                                //                else
                                //                {
                                //                    ((Label)(GridView1.Rows[addrow].FindControl("lblOperationStatus"))).Text = "New";
                                //                    dtCopy.Rows[addrow]["OperationStatus"] = "New";

                                //                }
                                //                //   break;


                                //                // }
                                //            }
                                //        }


                                //    }
                                //}
                                //catch (Exception ex)
                                //{
                                //}


                                //GridView1.DataSource = null;
                                //GridView1.DataSource = dtCopy; // dtCurrentTable;
                                //GridView1.DataBind();
                                #endregion Commented
                                
                                pnlSchedule.Visible = true;
                                /// pnlUpdate.Visible = true;
                                pnlDestDetails.Visible = true;

                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            // pnlSchedule.Visible = false;
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
        #endregion Add datewise dataTo Grid flisht status

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
                myDataColumn.ColumnName = "TailNo";
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
                dr["TailNo"] = "";
                dr["Capacity"] = "";

                dr["Status"] = "";
                // DateTime Flightfromdate = new DateTime();
                // DateTime FlightToDate = new DateTime();
                string Flightfromdate = "", FlightToDate = "";
                try
                {
                    //if (txtFlightFromdate.Text != "")
                   
                }
                catch (Exception ex)
                {

                }
                string FlightNo = "";
                // FlightNo = ddlFlight.SelectedValue;

                //FlightNo = ddlFlight.SelectedItem.Text; mod




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

        #region On gridciew row command selected record show
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName != "Page")
                {
                    int id = 0;
                    id = Convert.ToInt32(GridView1.SelectedIndex);
                    GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                    id = row.RowIndex;
                    string strid = ((Label)GridView1.Rows[id].FindControl("lblScheduleID")).Text;
                    ScheduleID = int.Parse(strid);
                    string perday = ((Label)GridView1.Rows[id].FindControl("lblFromDt")).Text;// +" 00:00:000"; ;
                    dtPerDay = DateTime.ParseExact(perday, "dd/MM/yyyy", null);
                    //grdBillingInfo.Rows[rowindex].Cells[0].Font.Bold = true;

                    if (ScheduleID != 0)
                    {
                        fillFlightDetails(ScheduleID);
                        Showlist.Visible = false;
                        pnlSearch.Visible = false;
                        grid.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
        
        #region Origin List
        private void OriginList()
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
                            DataRow row = ds.Tables[0].NewRow();

                            row[ds.Tables[0].Columns[0].ColumnName] = "All";
                            ds.Tables[0].Rows.Add(row);

                            ddlOrigin1.DataSource = ds;
                            ddlOrigin1.DataMember = ds.Tables[0].TableName;
                            ddlOrigin1.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            ddlOrigin1.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                            ddlOrigin1.DataBind();
                            ddlOrigin1.Text = "All";

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
                DataSet ds = OBJasb.GetOriginList(ddlOrigin1.SelectedValue);
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

                            ddlDestination0.DataSource = ds;
                            ddlDestination0.DataMember = ds.Tables[0].TableName;
                            ddlDestination0.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            ddlDestination0.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                            ddlDestination0.DataBind();
                            ddlDestination0.Text = "All";

                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion Dest List

        #region Fill flight Details using scheduleID
        protected void fillFlightDetails(int id)
        {
            try
            {

                DataSet dsRoute = OBJasb.GetAirlineScheduleUsingRouteID(id);

                if (dsRoute.Tables[0].Rows.Count > 0)
                {
                  //  Showlist.Visible = false;
                    //  pnlSchedule.Enabled = false;
                    //  ViewState["CurrentTable"] = dsRoute.Tables[0];
                    DataTable dtCurrentTable = dsRoute.Tables[0];// (DataTable)ViewState["CurrentTable"];
                    grdScheduleinfo.DataSource = dtCurrentTable;
                    grdScheduleinfo.DataBind();
                    DataRow drCurrentRow = null;
                    LoadSourceInGridview();
                    
                    LoadDestinationInGridview();
                    AirCraftTypeinGridview();
                    GetTailNumber();
                    AirCraftTypeInEdit();
                    if (dtCurrentTable.Rows.Count > 0)
                    {
                        //extract the TextBox values
                        pnlSchedule.Visible = true;

                        pnlDestDetails.Visible = true;
                        // pnlDestDetails.Enabled = false;
                        txtFromdate.Text = dtPerDay.ToString("dd/MM/yyyy");
                        txtToDate.Text = dtPerDay.ToString("dd/MM/yyyy");
                       
                        txtCargoCapacity.Text = dtCurrentTable.Rows[0][12].ToString();
                        string strAircraft = "";
                        strAircraft = dtCurrentTable.Rows[0][13].ToString();
                        try
                        {
                            if (strAircraft != "")
                            {
                                ddlLoadAirCraftType.Text = dtCurrentTable.Rows[0][13].ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        //    txtSource.Text = dtCurrentTable.Rows[0][14].ToString();
                        ddlOrigin1.Text = dtCurrentTable.Rows[0][14].ToString();
                        // txtDestination.Text = dtCurrentTable.Rows[0][15].ToString();
                        ddlDestination0.Text = dtCurrentTable.Rows[0][15].ToString();
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

                        // ddlDestination.SelectedValue = dtCurrentTable.Rows[0][8].ToString();

                        for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                        {
                            //extract the TextBox values

                            drCurrentRow = dtCurrentTable.NewRow();
                            // drCurrentRow["RowNumber"] = i + 1;




                            DropDownList ddlFromOrigin = new DropDownList();

                            ddlFromOrigin = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlFromOrigin")));

                            ddlFromOrigin.SelectedValue = dtCurrentTable.Rows[i][1].ToString();

                            DropDownList ddlDest = new DropDownList();

                            ddlDest = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlToDest")));

                            ddlDest.SelectedValue = dtCurrentTable.Rows[i][2].ToString();



                            Label lblflight = ((Label)grdScheduleinfo.Rows[i].FindControl("lblFlight"));
                            lblflight.Text = dtCurrentTable.Rows[i][0].ToString();
                           

                            ((TextBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("txtDeptDay")).Text = dtCurrentTable.Rows[i][3].ToString();

                            string[] HrDept = dtCurrentTable.Rows[i][4].ToString().Contains(":") ? dtCurrentTable.Rows[i][4].ToString().Split(':') : dtCurrentTable.Rows[i][4].ToString().Split('.');
                            ;

                            ((TextBox)grdScheduleinfo.Rows[i].Cells[2].FindControl("txtDeptTimeHr")).Text = HrDept[0];
                            ((TextBox)grdScheduleinfo.Rows[i].Cells[2].FindControl("txtDeptTimeMin")).Text = HrDept[1];

                            string[] HrArr = dtCurrentTable.Rows[i][6].ToString().Split(':');

                            ((TextBox)grdScheduleinfo.Rows[i].Cells[4].FindControl("txtArrivalDay")).Text = dtCurrentTable.Rows[i][5].ToString();

                            ((TextBox)grdScheduleinfo.Rows[i].Cells[1].FindControl("txtArrivaltimeHr")).Text = HrArr[0];
                            ((TextBox)grdScheduleinfo.Rows[i].Cells[1].FindControl("txtArrivaltimeMin")).Text = HrArr[1];


                            try
                            {
                                ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlAirCraft"))).SelectedValue = dtCurrentTable.Rows[i][18].ToString();
                                ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlTailNo"))).SelectedValue = dtCurrentTable.Rows[i][19].ToString();
                            }
                            catch (Exception ex)
                            {
                            }
                            ((TextBox)grdScheduleinfo.Rows[i].FindControl("txtCapacity")).Text = dtCurrentTable.Rows[i][17].ToString();

                            DropDownList ddlIsActive = new DropDownList();

                            ddlIsActive = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlIsActive")));

                            string Isactive = dtCurrentTable.Rows[i][11].ToString();
                           
                            ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlStatus"))).SelectedValue = Isactive;
                           
                        }


                       




                    }
                }
            }
            catch (Exception ex)
            {

            }


        }
        #endregion

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

                                //GetTailNumber(null, null);
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


        #region LoadDest in Gridview Dropdown
        public void LoadDestinationInGridview()
        {
            try
            {
                DataSet ds = OBJasb.GetDestinationList("");
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

        #region LoadSource in Gridview Dropdown
        public void LoadSourceInGridview()
        {
            try
            {
                DataSet ds = OBJasb.GetOriginList("");
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
                        ddl.Text = "Select";
                    }
                }
            }
            catch (Exception ex)
            {
            }

        }

        #endregion LoadSource Dropdown

        #region Save Active Schedule
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string FltNoAudit = "";
            try
            {
                lblResult.Text = "";
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
                    object[] RouteInfo = new object[22];
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
                    FltNoAudit = lblFlight.Text;
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

                    string dateday = dt1.DayOfWeek.ToString();




                    if (dateday == "Monday")
                        frquency = "1,0,0,0,0,0,0";
                    if (dateday == "Tuesday")
                        frquency = "0,1,0,0,0,0,0";
                    if (dateday == "Wednesday")
                        frquency = "0,0,1,0,0,0,0";
                    if (dateday == "Thursday")
                        frquency = "0,0,0,1,0,0,0";
                    if (dateday == "Friday")
                        frquency = "0,0,0,0,1,0,0";
                    if (dateday == "Saturday")
                        frquency = "0,0,0,0,0,1,0";
                    if (dateday == "Sunday")
                        frquency = "0,0,0,0,0,0,1";



                    //7
                    RouteInfo.SetValue(frquency, i);
                    i++;



                    //8
                    string DeptTime1 = ((TextBox)grdScheduleinfo.Rows[j].FindControl("txtDeptTimeHr")).Text; // 
                    DeptTime1 = DeptTime1.PadLeft(2, '0');
                    DeptTime1 = DeptTime1 + ":" + ((TextBox)grdScheduleinfo.Rows[j].FindControl("txtDeptTimeMin")).Text.PadLeft(2,'0');

                    RouteInfo.SetValue(DeptTime1, i);
                    i++;

                    //9
                    string arrTime1 = ((TextBox)grdScheduleinfo.Rows[j].FindControl("txtArrivaltimeHr")).Text.PadLeft(2, '0');
                    arrTime1 = arrTime1 + ":" + ((TextBox)grdScheduleinfo.Rows[j].FindControl("txtArrivaltimeMin")).Text.PadLeft(2, '0');
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
                    RouteInfo.SetValue(ScheduleID, i);
                    i++;

                    //20
                    RouteInfo.SetValue(((DropDownList)(grdScheduleinfo.Rows[j].FindControl("ddlAirCraft"))).SelectedValue, i);
                    i++;

                    //21
                    RouteInfo.SetValue(((TextBox)(grdScheduleinfo.Rows[j].FindControl("txtCapacity"))).Text, i);
                    i++;

                    //22
                    RouteInfo.SetValue(((DropDownList)(grdScheduleinfo.Rows[j].FindControl("ddlTailNo"))).SelectedValue, i);

                    #endregion Prepare Parameters

                    if (Isinsert == false)
                    {
                        IsRouteUpdate = OBJasb.UpdateAirlineRouteDetails(RouteInfo);
                        Isinsert = true;
                    }
                    if (Isinsert == true)
                    {
                        //if (txtFlightFromdate.Text == CheckFromDate && txtFlightToDate.Text == CheckToDate)
                        //{
                        IsRouteUpdate = OBJasb.UpdateAirlineRouteDetailsForSameDate(RouteInfo);
                        //}
                    }

                }
                if (IsRouteUpdate < 0)
                {
                    lblResult.Text = "Error update Route Details. Please try again...";
                    return;
                }
                else
                {



                #endregion

                #region Show Save data grid
                    // btnList.Click();
                    try
                    {

                        lblStatus.Text = "";
                        
                        //LoadSourceInGridview();
                        //// DestinationList(); 
                        //LoadDestinationInGridview();

                        //AirCraftTypeinGridview();
                        //AddRowToGrid();
                        
                        //btnEdit.Enabled = true;

                    }
                    catch (Exception)
                    {
                    }
                    #region for Master Audit Log
                    MasterAuditBAL ObjMAL = new MasterAuditBAL();
                    #region Prepare Parameters
                    object[] Paramsss = new object[7];
                    int k = 0;

                    //1
                    Paramsss.SetValue("Active Flight", k);
                    k++;

                    //2
                    string Value = FltNoAudit + "(" + ddlOrigin1.SelectedItem.Text + "-" + ddlDestination0.SelectedItem.Text + ")";
                    Paramsss.SetValue(Value, k);
                    k++;

                    //3
                    Paramsss.SetValue("UPDATE", k);
                    k++;

                    //4
                    string Msg = "Route Updated";
                    Paramsss.SetValue(Msg, k);
                    k++;

                    //5
                    string Desc = "";
                    Paramsss.SetValue(Desc, k);
                    k++;

                    //6

                    Paramsss.SetValue(Session["UserName"], k);
                    k++;

                    //7
                    Paramsss.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), k);
                    k++;

                    #endregion Prepare Parameters
                    ObjMAL.AddMasterAuditLog(Paramsss);
                    #endregion
                    Showlist.Visible = true;
                    pnlSearch.Visible = true;
                    lblResult.ForeColor = Color.Green;
                    lblResult.Text = "Route Details Updated Successfully";
                    
                    return;
                }

                    #endregion
               
                Button1_Click(null,null);
               
            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region Validate Data
        /// <summary>
        /// Validate data entered by user.
        /// </summary>
        /// <returns>Returns True if valid data is entered.</returns>
        private bool ValidateData()
        {
            lblStatus.Text = "";

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

                DateTime CheckFromDate1;
                DateTime.TryParseExact(txtFromdate.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out CheckFromDate1);
                DateTime CheckToDate1;
                DateTime.TryParseExact(txtToDate.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out CheckToDate1);

                int chkfromdate = DateTime.Compare(CheckFromDate1, dt1);
                if (chkfromdate > 0)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid from date";
                    txtFromdate.Focus();
                    return false;
                }


                int chtodate = DateTime.Compare(dt2, CheckToDate1);
                //if (chtodate > 0)
                //{
                //    lblStatus.ForeColor = Color.Red;
                //    lblStatus.Text = "Please enter valid To date";
                //    txtFromdate.Focus();
                //    return false;
                //}


              
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
                    else if (int.Parse(txtCheckArrDay.Text) > 4 || int.Parse(txtCheckArrDay.Text) == 0)
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
                //tempArrivalTimeMin = null;

                ////Validate Arrival Departure time
                //try
                //{
                //    tempArrivalTimeHr = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtArrivaltimeHr"));
                //    tempdeptTimeHr = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeHr"));
                //    int arrday = int.Parse(txtCheckArrDay.Text);
                //    int DeptDay = int.Parse(txtCheckDeptDay.Text);

                //    tempArrivalTimeMin = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtArrivaltimeMin"));
                //    tempDeprtureTimeMin = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeMin"));


                //    int ArrTimeHr = int.Parse(tempArrivalTimeHr.Text);
                //    int DeptTimeHr = int.Parse(tempdeptTimeHr.Text);
                //    if ((ArrTimeHr < DeptTimeHr) && (arrday < DeptDay))
                //    {
                //        lblStatus.ForeColor = Color.Red;
                //        lblStatus.Text = "Please enter valid arrival time in Hr (row:" + (i + 1) + ")";
                //        tempdeptTimeHr.Focus();
                //        return false;
                //    }
                //    else if ((ArrTimeHr < DeptTimeHr) && (arrday == DeptDay))
                //    {
                //        lblStatus.ForeColor = Color.Red;
                //        lblStatus.Text = "Please enter valid arrival time in Hr (row:" + (i + 1) + ")";
                //        tempdeptTimeHr.Focus();
                //        return false;
                //    }
                //    else if ((ArrTimeHr == DeptTimeHr) && (arrday < DeptDay))
                //    {
                //        lblStatus.ForeColor = Color.Red;
                //        lblStatus.Text = "Please enter valid arrival day (row:" + (i + 1) + ")";
                //        tempdeptTimeHr.Focus();
                //        return false;
                //    }
                //    else if (arrday < DeptDay)
                //    {
                //        lblStatus.ForeColor = Color.Red;
                //        lblStatus.Text = "Please enter valid arrival day (row:" + (i + 1) + ")";
                //        txtCheckArrDay.Focus();
                //        return false;
                //    }

                //    else if (int.Parse(tempArrivalTimeHr.Text) == int.Parse(tempdeptTimeHr.Text))
                //    {
                //        if (int.Parse(txtCheckArrDay.Text) == int.Parse(txtCheckDeptDay.Text))
                //        {
                //            if (int.Parse(tempArrivalTimeMin.Text) < int.Parse(tempDeprtureTimeMin.Text))
                //            {
                //                lblStatus.ForeColor = Color.Red;
                //                lblStatus.Text = "Please enter valid arrival Min (row:" + (i + 1) + ")";
                //                tempDeprtureTimeMin.Focus();
                //                return false;
                //            }
                //            else if (int.Parse(tempArrivalTimeMin.Text) == int.Parse(tempDeprtureTimeMin.Text))
                //            {
                //                lblStatus.ForeColor = Color.Red;
                //                lblStatus.Text = "Daparture time and Arrival time should be different (row:" + (i + 1) + ")";
                //                tempDeprtureTimeMin.Focus();
                //                return false;
                //            }

                //        }
                //        if (int.Parse(tempArrivalTimeMin.Text) == int.Parse(tempDeprtureTimeMin.Text))
                //        {
                //            if (int.Parse(txtCheckArrDay.Text) < int.Parse(txtCheckDeptDay.Text))
                //            {
                //                lblStatus.ForeColor = Color.Red;
                //                lblStatus.Text = "Please enter valid arrival Day (row:" + (i + 1) + ")";
                //                txtCheckArrDay.Focus();
                //                return false;
                //            }
                //        }

                //    }

                //}
                //catch (Exception ex)
                //{
                //    lblStatus.ForeColor = Color.Red;
                //    lblStatus.Text = "Please enter valid Departure and Arrival time in Min";
                //    //tempdeptTimeHr.Focus();
                //    return false;
                //}

                //Validate Source Wise Data
                try
                {
                    if (grdScheduleinfo.Rows.Count > 1)
                    {
                        if (i > 0)
                        {

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

                            tempArrivalTimeHr = (TextBox)(grdScheduleinfo.Rows[i - 1].FindControl("txtArrivaltimeHr"));
                            tempdeptTimeHr = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeHr"));
                            int arrday = int.Parse(txtCheckArrDay.Text);
                            int DeptDay = int.Parse(txtCheckDeptDay.Text);
                            tempDeprtureTimeMin = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeMin"));
                            tempArrivalTimeMin = (TextBox)(grdScheduleinfo.Rows[i - 1].FindControl("txtArrivalTimeMin"));

                            //int ArrTimeHr = int.Parse(tempArrivalTimeHr.Text);
                            //int DeptTimeHr = int.Parse(tempdeptTimeHr.Text);
                            //if ((ArrTimeHr > DeptTimeHr) && (arrday > DeptDay))
                            //{
                            //    lblStatus.ForeColor = Color.Red;
                            //    lblStatus.Text = "Please enter valid arrival time in Hr (row:" + (i + 1) + ")";
                            //    tempdeptTimeHr.Focus();
                            //    return false;
                            //}
                            //else if ((ArrTimeHr > DeptTimeHr) && (arrday == DeptDay))
                            //{
                            //    lblStatus.ForeColor = Color.Red;
                            //    lblStatus.Text = "Please enter valid Departure time in Hr (row:" + (i + 1) + ")";
                            //    tempdeptTimeHr.Focus();
                            //    return false;
                            //}
                            //else if ((ArrTimeHr == DeptTimeHr) && (arrday < DeptDay))
                            //{
                            //    lblStatus.ForeColor = Color.Red;
                            //    lblStatus.Text = "Please enter valid arrival time in Hr (row:" + (i + 1) + ")";
                            //    tempdeptTimeHr.Focus();
                            //    return false;
                            //}
                            //else if (arrday > DeptDay)
                            //{
                            //    lblStatus.ForeColor = Color.Red;
                            //    lblStatus.Text = "Please enter valid departure day (row:" + (i + 1) + ")";
                            //    txtCheckArrDay.Focus();
                            //    return false;
                            //}
                            //else if ((ArrTimeHr == DeptTimeHr) && (arrday == DeptDay) && (int.Parse(tempDeprtureTimeMin.Text)) < (int.Parse(tempArrivalTimeMin.Text)))
                            //{
                            //    lblStatus.ForeColor = Color.Red;
                            //    lblStatus.Text = "Please enter valid Departure time in Min (row:" + (i + 1) + ")";
                            //    tempdeptTimeHr.Focus();
                            //    return false;
                            //}
                            //else if ((ArrTimeHr == DeptTimeHr) && (arrday == DeptDay) && (int.Parse(tempDeprtureTimeMin.Text)) == (int.Parse(tempArrivalTimeMin.Text)))
                            //{
                            //    lblStatus.ForeColor = Color.Red;
                            //    lblStatus.Text = "Please enter valid Departure time (row:" + (i + 1) + ")";
                            //    tempdeptTimeHr.Focus();
                            //    return false;
                            //}
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
                  
                    DateTime dt1 = DateTime.ParseExact(txtFromdate.Text, "dd/MM/yyyy", null);
                    DateTime dt2 = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);





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

        protected void BtnCLose_Click1(object sender, EventArgs e)
        {
            Showlist.Visible = true;
            pnlSearch.Visible = true;
            pnlDestDetails.Visible = false;
            pnlSchedule.Visible = false;
            grid.Visible = false;
        }

        protected void btnAddNewRow_Click(object sender, EventArgs e)
        {

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            ScheduleID = Convert.ToInt32(e.NewSelectedIndex);

            if (ScheduleID != 0)
            {
            }
        }

        protected void ddlLoadAirCraftType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridView1.PageIndex = e.NewPageIndex;
                if (Session["ActSch_fltAllStatus"] != null)
                {
                    GridView1.DataSource = (DataTable)Session["ActSch_fltAllStatus"];
                    GridView1.DataBind();
                    Session["ActSch_fltAllStatus"] = SetFlightsGridData((DataTable)Session["ActSch_fltAllStatus"],
                        (DataSet)Session["ActSch_fltAllStatusDataset"]);
                    GridView1.DataSource = (DataTable)Session["ActSch_fltAllStatus"];
                    GridView1.DataBind();
                }
                else if (Session["ActSch_fltSpecificStatus"] != null)
                {
                    GridView1.DataSource = (DataTable)Session["ActSch_fltSpecificStatus"];
                    GridView1.DataBind();
                }
            }
            catch (Exception)
            {

            }
        }

        //Roles

        private void AddControls(ControlCollection page, ArrayList controlList, DataSet dsControls)
        {
            foreach (Control c in page)
            {
                if (c.ID != null)
                {
                    controlList.Add(c.ID);

                    for (int i = 0; i < dsControls.Tables[0].Rows.Count; i++)
                    {
                        if (c.ID == dsControls.Tables[0].Rows[i]["ControlId"].ToString())
                        {
                            if (c is DropDownList)
                                ((DropDownList)(c)).Enabled = false;
                            else if (c is Button)
                                ((Button)(c)).Enabled = false;
                            else if (c is TextBox)
                                ((TextBox)(c)).Enabled = false;
                            else if (c is CheckBox)
                                ((CheckBox)(c)).Enabled = false;
                            else if (c is LinkButton)
                                ((LinkButton)(c)).Enabled = false;
                            break;
                        }
                    }

                }

                if (c.HasControls())
                {
                    AddControls(c.Controls, controlList, dsControls);
                }
            }
        }

        private void ApplyPermissions()
        {
            ArrayList controlList = new ArrayList();
            string AgentCode = string.Empty;
            AgentCode = Convert.ToString(Session["ACode"]);

            HomeBL objHome = new HomeBL();
            int RoleId = Convert.ToInt32(Session["RoleID"]);
            DataSet objDS = objHome.GetUserPermissions(((System.Web.UI.TemplateControl)(Page)).AppRelativeVirtualPath, RoleId);

            AddControls(Page.Controls, controlList, objDS);

            controlList = null;
        }

        protected void btnclear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ActiveSchedule.aspx");
        }

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



        //end
        #region GetTailNumber as per AirCratftType
        private void GetTailNumber()
        {
            DataSet ds = new DataSet();
            try
            {
                for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
                {
                    string AirCraftType = ((DropDownList)grdScheduleinfo.Rows[i].FindControl("ddlAirCraft")).SelectedItem.ToString();
                    ds = OBJasb.GetTailNumber(AirCraftType);

                    DropDownList ddlTailNo = (DropDownList)grdScheduleinfo.Rows[i].FindControl("ddlTailNo");

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].NewRow();
                        ddlTailNo.DataSource = ds;
                        ddlTailNo.DataMember = ds.Tables[0].TableName;
                        ddlTailNo.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                        ddlTailNo.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                        ddlTailNo.DataBind();
                    }

                }
            }
            catch (Exception)
            {
                
            }
            
           
        }
        #endregion

        #region DDL AirCraft IndexChanged Event Handler
        protected void ddlDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvrow = (GridViewRow)((DropDownList)sender).NamingContainer;

                DropDownList ddlAirCraft = (DropDownList)gvrow.FindControl("ddlAirCraft");
                DropDownList ddlTailNo = (DropDownList)gvrow.FindControl("ddlTailNo");

                DataSet ds = new DataSet();
                ds = OBJasb.GetTailNumber(ddlAirCraft.SelectedValue);
                
                ddlTailNo.DataSource = ds;
                ddlTailNo.DataMember = ds.Tables[0].TableName;
                ddlTailNo.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                ddlTailNo.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                ddlTailNo.DataBind();


            }
            catch (Exception)
            {

            }

        }
        #endregion

    }
}
