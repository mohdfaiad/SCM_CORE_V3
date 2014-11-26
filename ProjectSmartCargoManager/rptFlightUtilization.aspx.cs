using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using BAL;

using System.IO;
using System.Drawing;
using QID.DataAccess;
using Microsoft.Reporting.WebForms;
using System.Data.SqlClient;

namespace ProjectSmartCargoManager
{
public partial class rptFlightUtilization : System.Web.UI.Page
{
ArrayList arFlight = new ArrayList();
AirlineScheduleBAL OBJasb = new AirlineScheduleBAL();
private DataSet Dataset1=new DataSet("FlightUtil_dsDataset1");
private DataSet Dataset2=new DataSet("FlightUtil_dsDataset2");
ReportBAL objBal = new ReportBAL();
BALException objBAL = new BALException();
BookingBAL objBLL = new BookingBAL();
        public static string CurrTime = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime dtCurr = Convert.ToDateTime(Session["IT"].ToString());
                CurrTime = dtCurr.ToString("dd/MM/yyyy hh:mm:ss tt");
                RptFlightUtilizationViewer.Visible = false;
                AirCraftType();
                LoadCountry();
                LoadRegion();
                GetOrigin();
                GetDestination();
                LoadFlightPrefix();
                GetFlights();
                txtFlightFromdate.Text = txtFlightToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }

        private void LoadCountry()
        {

            DataSet ds = new DataSet("FlightUtil_dsCountry");
            try
            {
                ds = objBAL.GetCountryCodeList(ddlCountry.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlCountry.DataSource = ds;
                            ddlCountry.DataMember = ds.Tables[0].TableName;
                            ddlCountry.DataValueField = "CountryCode";
                            ddlCountry.DataTextField = "Country";
                            ddlCountry.DataBind();
                            ddlCountry.Items[0].Value = "All";
                            ddlCountry.Items.Insert(0, "All");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        
        }
        private void LoadRegion()
        {
            StockAllocationBAL objBAL = new StockAllocationBAL();
            DataSet Region = new DataSet("FlightUtil_dsRegion");
            try
            {
                Region = objBAL.GetRegionCode();

                if (Region != null)
                {
                    if (Region.Tables != null)
                    {
                        if (Region.Tables.Count > 0)
                        {

                            ddlRegion.DataSource = Region;
                            ddlRegion.DataMember = Region.Tables[0].TableName;
                            ddlRegion.DataValueField = "RegionName";
                            ddlRegion.DataTextField = "RegionCode";
                            ddlRegion.DataBind();
                            ddlRegion.Items[0].Value = "All";
                            ddlRegion.Items.Insert(0, "All");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (Region != null)
                    Region.Dispose();
            }

        }
        private void GetOrigin()
        {

            DataSet ds = new DataSet("FlightUtil_dsOrigin");
            try
            {
                ds = objBAL.GetOriginCodeList(ddlOrigin.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlOrigin.DataSource = ds;
                            ddlOrigin.DataMember = ds.Tables[0].TableName;
                           // ddlOrigin.DataValueField = ds.Tables[2].Columns["AirportCode"].ColumnName;
                            ddlOrigin.DataValueField = "AirportCode";
                            ddlOrigin.DataTextField = "Airport";
                            //ddlOrigin.DataTextField = ds.Tables[2].Columns["Airport"].ColumnName;
                            ddlOrigin.DataBind();
                            ddlOrigin.Items.Insert(0, "All");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }
        private void GetDestination()
        {
            DataSet ds = new DataSet("FlightUtil_dsDestination");
            try
            {
                ds = objBAL.GetOriginCodeList(ddlDestination.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlDestination.DataSource = ds;
                            ddlDestination.DataMember = ds.Tables[0].TableName;
                            ddlDestination.DataValueField = "AirportCode";//ds.Tables[2].Columns["Code"].ColumnName;

                            ddlDestination.DataTextField ="Airport"; //ds.Tables[2].Columns["Code"].ColumnName;
                            ddlDestination.DataBind();
                            ddlDestination.Items.Insert(0, "All");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        
        }
           private void LoadFlightPrefix()
      {
          DataSet ds = new DataSet("FlightUtil_dsFlightPrefix");

          try
          {
              ds = objBLL.GetFlightPrefixList();
              if (ds != null)
              {
                  if (ds.Tables.Count > 0)
                  {
                      if (ds.Tables[0].Rows.Count > 0)
                      {
                          ddlFlightPrefix.Items.Clear();
                          //ddlComodityCd.Items.Add("Select");
                          ddlFlightPrefix.DataSource = ds.Tables[0];
                          //ddlComodityCd.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                          //ddlComodityCd.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                          ddlFlightPrefix.DataTextField = "PartnerCode";
                          ddlFlightPrefix.DataValueField = "PartnerCode";
                          ddlFlightPrefix.DataBind();
                          //ddlComodityCd.SelectedIndex = 0;
                          ddlFlightPrefix.Items.Insert(0, "SELECT");
                          // ddlComodityCd.SelectedValue = "GEN";
                      }
                  }
              }
          }
          catch (Exception ex)
          { }

          finally
          {
              if (ds != null)
                  ds.Dispose();
          }

            }
                    protected void TextBox2_TextChanged(object sender, EventArgs e)
                    {

                   }

                     protected void btnList_Click(object sender, EventArgs e)
               {
                   DataSet ds = new DataSet("FlightUtil_dsFlightUtilization");               
                         try
                       
                   {
                            Session["dsFltUtilization"] = null;
                            lblStatus.Text = string.Empty;
                            RptFlightUtilizationViewer.Visible = false;

                            string strResult = string.Empty;
                                try

                                {

                                     strResult = objBal.GetReportInterval(DateTime.ParseExact(txtFlightFromdate.Text.Trim(), "dd/MM/yyyy", null), DateTime.ParseExact(txtFlightToDate.Text.Trim(), "dd/MM/yyyy", null));
                                }
                                catch
                                {
                                strResult = "";
                                }
                                if (strResult != "")
                                {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = strResult;
                                txtFlightFromdate.Focus();
                                return;
                                }
                                if (txtFlightFromdate.Text == "" || txtFlightToDate.Text == "")
                                                                    {
                                    //lblStatus.ForeColor = Color.Red;
                                    //lblStatus.Text = "Please enter From date and To date for datewise Schedule List";
                                    //txtFlightFromdate.Focus();
                                    //return;
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
                                    //txtFromdate.Focus();
                                    return;
                                    }


                                    }
                                    }
                                    catch(Exception ex)
                                    {
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                                    //txtFromdate.Focus();
                                    return;
                                    }
                         finally
                         {
                             if (ds != null)
                                 ds.Dispose();
                             }


                                         if (chkDomestic.Checked == false && chkInternational.Checked == false)
                                    {  lblStatus.Text = "Kindly Check atleast one from Domestic and Inernational";
                                    return;
                                    }

                                        if (chkDomestic.Checked == false && chkInternational.Checked == false)
                                        {
                                        lblStatus.Text = "Kindly Check atleast one from Domestic and Inernational";
                                        return;
                                        }
                                        try
                                        {
                                        int rowIndex = 0;

                                        string Flightfromdate = "", FlightToDate = "";
                                        DateTime dt1 = new DateTime();
                                        DateTime dt2 = new DateTime();
                                        try
                                        {
                                                                                    //if (txtFlightFromdate.Text == "" || txtFlightToDate.Text == "")
                                            //{
                                            //    lblStatus.ForeColor = Color.Red;
                                            //    lblStatus.Text = "Please Insert From date and To date for datewise Schedule List";
                                            //    txtFlightFromdate.Focus();
                                            //    return;
                                            //}

                                            if (txtFlightFromdate.Text != "")
                                            {
                                            // Flightfromdate = Convert.ToDateTime(txtFlightFromdate.Text).ToString("yyyy-MM-dd HH:mm:ss");
                                            //Flightfromdate = Convert.ToDateTime(txtFlightFromdate.Text).ToString("dd/MM/yyyy");
                                            ////  dt1 = DateTime.Parse(txtFlightFromdate.Text);//.ToString("dd/MM/yyyy");
                                            //Flightfromdate = txtFlightFromdate.Text;
                                            ////DateTime dtFromDate = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null);
                                            dt1 = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null);
                                            //Flightfromdate = DateTime.ParseExact(txtFlightFromdate.Text, "MM/dd/yyyy", null);
                                            Flightfromdate = dt1.ToString("MM/dd/yyyy");
                                            //Flightfromdate = txtFlightFromdate.Text.Trim();// dt1.ToString("MM/dd/yyyy");
                                            }
                                            if (txtFlightToDate.Text!="")
                                                {
                                                //FlightToDate = Convert.ToDateTime(txtFlightToDate.Text).ToString("dd/MM/yyyy");
                                                // dt2 = DateTime.Parse(txtFlightToDate.Text);//.ToString("dd/MM/yyyy");
                                                dt2=DateTime.ParseExact(txtFlightToDate.Text,"dd/MM/yyyy", null);
                                                // FlightTodate = dt2.ToString("MM/dd/yyyy");
                                                FlightToDate = dt2.ToString("MM/dd/yyyy");
                                                //FlightToDate = txtFlightToDate.Text.Trim();// dt2.ToString("MM/dd/yyyy");


                                                }

                                                if (txtFlightFromdate.Text == "")
                                                {


                                                Flightfromdate="";
                                                }
                                                if(txtFlightToDate.Text=="")
                                                {

                                                FlightToDate = "";      


                                                }
                                                }
                                                catch (Exception ex)
                                                {

                                                }


string FlightNo = "";

if (ddlFlightNumber.SelectedItem.Text.Trim()=="ALL")
                    FlightNo = "All";
                else
                    FlightNo = ddlFlightNumber.SelectedValue;
                                 
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

                //DataSet ds = OBJasb.GetAirlineSchedule(ddlOrigin.SelectedValue, ddlDestination.SelectedValue, FlightNo, ddlAirCraftType.SelectedValue, Flightfromdate, FlightToDate, ddlStatus.SelectedValue, strdomestic);
                string Source = "All", Dest = "All";
                if (ddlOrigin.SelectedItem.Text.Trim() != "")
                {
                    Source = ddlOrigin.SelectedValue.ToString().Trim();
                }
                if (ddlDestination.SelectedItem.Text.Trim() != "")
                {
                    Dest = ddlDestination.SelectedValue.ToString().Trim();
                }
                string country = "All";
                if (ddlCountry.SelectedItem.Text.Trim() != "")
                {
                    country = ddlCountry.SelectedValue.ToString().Trim();
                }


                string Region = "All";
                if (ddlRegion.SelectedItem.Text.Trim() != "")
                {
                    //Region = ddlRegion.SelectedValue.ToString();
                    Region = ddlRegion.SelectedValue.ToString().Trim();
                }


                //FileInfo info;
                //ReportRuntime runtime;
                //ReportDefinition definition;
                 ds = OBJasb.GetFlightUtilization(country, Region, Source, Dest, FlightNo, ddlAirCraftType.SelectedValue, Flightfromdate.ToString(), FlightToDate.ToString(), ddlStatus.SelectedValue, strdomestic);
                //DataSet ds = OBJasb.GetFlightUtilization(country, Region, Source, Dest, FlightNo, ddlAirCraftType.SelectedValue, dt1, FlightToDate.ToString(), ddlStatus.SelectedValue, strdomestic);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if ( ds.Tables[0].Rows.Count > 0)
                        {
                            Session["dsFltUtilization"] = ds;
                            DataTable myDataTable = new DataTable();
                            DataColumn myDataColumn;
                            DataSet Ds = new DataSet("FlightUtil_dsExport");

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "Country";
                            myDataTable.Columns.Add(myDataColumn);


                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "Region";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "Origin";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "Destination";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "FlightID";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "FromDt";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "ToDt";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "AircraftType";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "Status";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "ReportDate";
                            myDataTable.Columns.Add(myDataColumn);

                    string frmdate =dt1.ToString("dd/MM/yyyy");

                    string todate = dt2.ToString("dd/MM/yyyy");
                    DataRow dr;
                            dr = myDataTable.NewRow();
                            dr["Country"] = country;
                            dr["Region"] = Region; //"5";
                            dr["Origin"] = Source;// "5";
                            dr["Destination"] = Dest;
                            dr["FlightID"] = FlightNo;

                            dr["FromDt"] = frmdate;//dt1;//Flightfromdate ;
                            dr["ToDt"] = todate;// dt2;//FlightToDate;// "9";
                            dr["AircraftType"] = ddlAirCraftType.SelectedValue;// "9";
                            dr["Status"] = ddlStatus.SelectedValue;// "9";
                            dr["ReportDate"] = CurrTime;// "9";
                            myDataTable.Rows.Add(dr);

                            //  Ds.Tables.Add(myDataTable);

                            DataSet dschk = new DataSet("FlightUtil_dsCheak");
                            dschk.Tables.Add(myDataTable);
                            Dataset2 = dschk.Copy();

                            Dataset1 = ds.Copy();

                            #region Old RDLX
                            //RptFlightUtilizationViewer.Visible = true;

                            //info = new FileInfo(Server.MapPath("/Reports/rptFlightUtilization.rdlx"));
                            ////info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptFlightUtilization.rdlx");
                            //definition = new ReportDefinition(info);
                            //runtime = new ReportRuntime(definition);


                            ////  Dataset2 = dschk;

                            //runtime.LocateDataSource += WRAXBDetails_LocateDataSource;
                            //RptFlightUtilizationViewer.SetReport(runtime);

                            //// dt.Rows[0]["From"]= txtAutoSource.Text.Trim();
                            #endregion

                            #region RDLC

                            //Logo
                            System.IO.MemoryStream Logo = null;
                            try
                            {
                                Logo = CommonUtility.GetImageStream(Page.Server);
                                //System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                            }
                            catch (Exception ex)
                            {
                                Logo = new System.IO.MemoryStream();
                            }

                            if (ds.Tables[0].Columns.Contains("Logo") == false)
                            {
                                DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                                col1.DefaultValue = Logo.ToArray();
                                ds.Tables[0].Columns.Add(col1);
                            }

                            RptFlightUtilizationViewer.Visible = true;
                            RptFlightUtilizationViewer.ProcessingMode = ProcessingMode.Local;
                            RptFlightUtilizationViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/FlightUtilizationRpt.rdlc");
                            //Customers dsCustomers = GetData("select top 20 * from customers");
                            ReportDataSource datasource = new ReportDataSource("dsFlightUtilization_dtFlightUtilization", ds.Tables[0]);
                            RptFlightUtilizationViewer.LocalReport.DataSources.Clear();
                            RptFlightUtilizationViewer.LocalReport.DataSources.Add(datasource);
                            RptFlightUtilizationViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                            SaveUserActivityLog("");

                            #endregion
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data does not exists for selected search criteria";
                            RptFlightUtilizationViewer.Visible = false;
                            Session["dsFltUtilization"] = null;
                            SaveUserActivityLog(lblStatus.Text);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SaveUserActivityLog(ex.Message);
            }
                                        if (ds != null)
                                        {
                                            ds.Dispose();
                                        }
        }

        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsFlightUtilization_dtFlightUtilizationSub", Dataset2.Tables[0]));
        }

        #region Add datewise dataTo Grid
        private DataSet AddDateWiseRowToGrid(DataSet ds)
        {
            DataSet dsShow = new DataSet("FlightUtil_dsShow");

            DataTable dtCopy = new DataTable("FlightUtil_dtDCopy");
            try
            {

                int rowIndex = 0;


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
                                        //     string flightandsource = FlightID + dtCurrentTable.Rows[i][1].ToString();
                                        if (!arFlight.Contains(scheduleID))// + " " + Weekdays))//FlightID))//&& (!arOrigin.Contains(dtCurrentTable.Rows[i][1].ToString())))
                                        {
                                            //if ((!arOrigin.Contains(dtCurrentTable.Rows[i][1].ToString())))
                                            //{


                                            arFlight.Add(scheduleID);// + " " + Weekdays);
                                            ActDeptTimr = ActDeptTimr.Replace(':', '.');
                                            // ActDeptTimr = Dtemp.Rows[row][4].ToString();
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
                                }
                                catch (Exception ex)
                                {

                                }


                                ViewState["CurrentTable"] = view.ToTable(); //Dtemp;//ds.Tables[0]; mod vikas 1Aug
                                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                                DataRow drCurrentRow = null;
                                DataRow drCurrentRow1 = null;

                                dtCopy = dtCurrentTable.Clone();

                                if (dtCurrentTable.Rows.Count > 0)
                                {
                                    //extract the TextBox values



                                    // string fromdt = txtFromdate.Text;
                                    int k = 0, r = 0, Addrecord = 0; ;
                                    try
                                    {
                                        //DateTime dt1 = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                                        //DateTime dt2 = DateTime.ParseExact(txtFlightToDate.Text, "dd/MM/yyyy", null);
                                        //string fromdt = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null);
                                        //string ToDate = Session["ToDate"].ToString();
                                        DateTime getFromDt = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null);
                                        DateTime getToDt = DateTime.ParseExact(txtFlightToDate.Text, "dd/MM/yyyy", null);


                                        TimeSpan t = getToDt - getFromDt;
                                        double NrOfDays = t.TotalDays;
                                        for (int days = 0; days <= NrOfDays; days++)
                                        {
                                            for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                                            {
                                                // DateTime getFromDt = Convert.ToDateTime(dtCurrentTable.Rows[i][8].ToString());
                                                //  DateTime getToDt = Convert.ToDateTime(dtCurrentTable.Rows[i][9].ToString());


                                                // string FlightID2= dtCurrentTable.Rows[i+1][0].ToString();

                                                DateTime dtMatchFromDt = Convert.ToDateTime(dtCurrentTable.Rows[i][8].ToString());
                                                DateTime dtMatchToDt = Convert.ToDateTime(dtCurrentTable.Rows[i][9].ToString());
                                                if (dtMatchFromDt <= getFromDt && dtMatchToDt >= getFromDt)
                                                {

                                                    string FlightID1 = dtCurrentTable.Rows[i][0].ToString();
                                                    //lblScheduleID

                                                    // string[] chkWeekdays = dtCurrentTable.Rows[i][7].ToString().Split(',');

                                                    Addrecord = 0;

                                                    //extract the TextBox values





                                                    string FlightID = dtCurrentTable.Rows[i][0].ToString();
                                                    int records = 0;
                                                    string chkfrequency = dtCurrentTable.Rows[i][7].ToString();


                                                    //string CheckWeekdays = dtCurrentTable.Rows[i + r][7].ToString();
                                                    string[] chkWeekdays = dtCurrentTable.Rows[i][7].ToString().Split(',');
                                                    string wkday = dtCurrentTable.Rows[i][7].ToString();

                                                    string org = dtCurrentTable.Rows[i][1].ToString();

                                                    int chk = DateTime.Compare(getFromDt, dt1);

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

                                                        // lblDate.Text = getFromDt.ToString();
                                                        //  dtCopy.Rows[k][0] = getFromDt.ToString("dd/MM/yyyy");


                                                        Label ddlFromOrigin = new Label();

                                                        // ddlFromOrigin = ((Label)(GridView1.Rows[k].FindControl("lblFromOrigin")));

                                                        ddlFromOrigin.Text = dtCurrentTable.Rows[i][1].ToString();



                                                        dtCopy.Rows[k][1] = dtCurrentTable.Rows[i][1].ToString();


                                                        Label ddlDest = new Label();

                                                        //  ddlDest = ((Label)(GridView1.Rows[k].FindControl("lblToDest")));
                                                        dtCopy.Rows[k][2] = dtCurrentTable.Rows[i][2].ToString();

                                                        ddlDest.Text = dtCurrentTable.Rows[i][2].ToString();


                                                        if (!arFlight.Contains(FlightID + " " + wkday))//FlightID))//&& (!arOrigin.Contains(dtCurrentTable.Rows[i][1].ToString())))
                                                        {
                                                            //if ((!arOrigin.Contains(dtCurrentTable.Rows[i][1].ToString())))
                                                            //{

                                                            // Label lblFlight = ((Label)(GridView1.Rows[k].FindControl("lblFlight")));
                                                            //lblFlight.Text = FlightID;
                                                            arFlight.Add(FlightID + " " + wkday);

                                                            //    dtCopy.Rows[k][0] = FlightID;
                                                            //  arFlight.Add(flightandsource);
                                                            // arOrigin.Add(dtCurrentTable.Rows[i][1].ToString());
                                                            //}
                                                        }

                                                        DateTime dtaddfrm = new DateTime();
                                                        dtaddfrm = Convert.ToDateTime(getFromDt.ToString());
                                                        try
                                                        {
                                                            if (dtCurrentTable.Rows[i][3].ToString() == "2")
                                                            {
                                                                dtaddfrm = dtaddfrm.AddDays(1);
                                                                dtCopy.Rows[k][3] = dtaddfrm.ToString("dd/MM/yyyy") + " " + dtCurrentTable.Rows[i][4].ToString();

                                                            }
                                                            else
                                                                dtCopy.Rows[k][3] = getFromDt.ToString("dd/MM/yyyy") + " " + dtCurrentTable.Rows[i][4].ToString();

                                                            //  string[] HrDept = dtCurrentTable.Rows[i][4].ToString().Split(':');
                                                            //  ((Label)GridView1.Rows[k].FindControl("lblDepttime")).Text = " - " + dtCurrentTable.Rows[i+r][4].ToString();
                                                            dtCopy.Rows[k][4] = " - " + dtCurrentTable.Rows[i][4].ToString();






                                                        }

                                                        catch (Exception ex)
                                                        {
                                                        }
                                                        


                                                        if (!arFlight.Contains(FlightID + " " + dtCopy.Rows[k][3].ToString()))//FlightID))//&& (!arOrigin.Contains(dtCurrentTable.Rows[i][1].ToString())))
                                                        {
                                                            //if ((!arOrigin.Contains(dtCurrentTable.Rows[i][1].ToString())))
                                                            //{

                                                            // Label lblFlight = ((Label)(GridView1.Rows[k].FindControl("lblFlight")));
                                                            //lblFlight.Text = FlightID;
                                                            arFlight.Add(FlightID + " " + dtCopy.Rows[k][3].ToString());

                                                            dtCopy.Rows[k][0] = FlightID;
                                                            //  arFlight.Add(flightandsource);
                                                            // arOrigin.Add(dtCurrentTable.Rows[i][1].ToString());
                                                            //}
                                                        }



                                                        dtaddfrm = Convert.ToDateTime(getFromDt.ToString());
                                                        if (dtCurrentTable.Rows[i + r][5].ToString() == "2")
                                                        {
                                                            dtaddfrm = dtaddfrm.AddDays(1);
                                                            dtCopy.Rows[k][5] = dtaddfrm.ToString("dd/MM/yyyy") + " " + dtCurrentTable.Rows[i + r][6].ToString(); ;
                                                        }
                                                        else
                                                            dtCopy.Rows[k][5] = getFromDt.ToString("dd/MM/yyyy") + " " + dtCurrentTable.Rows[i + r][6].ToString(); ;


                                                        //   ((Label)GridView1.Rows[k].FindControl("lblArrtime")).Text = " - " + dtCurrentTable.Rows[i+r][6].ToString();
                                                        dtCopy.Rows[k][6] = " - " + dtCurrentTable.Rows[i + r][6].ToString();

                                                        //((TextBox)GridView1.Rows[days].Cells[1].FindControl("txtArrivaltimeHr")).Text = HrArr[0];
                                                        //((TextBox)GridView1.Rows[days].Cells[1].FindControl("txtArrivaltimeMin")).Text = HrArr[1];



                                                        Label ddlIsActive = new Label();

                                                        //   ddlIsActive = ((Label)(GridView1.Rows[k].FindControl("lblStatus")));

                                                        string Isactive = dtCurrentTable.Rows[i][11].ToString();
                                                        dtCopy.Rows[k][11] = dtCurrentTable.Rows[i][11].ToString();

                                                        //((Label)(GridView1.Rows[k].FindControl("lblAirCraft"))).Text = dtCurrentTable.Rows[i+r][17].ToString();
                                                        dtCopy.Rows[k][17] = dtCurrentTable.Rows[i + r][17].ToString();

                                                        //  ((Label)(GridView1.Rows[k].FindControl("lblCapacity"))).Text = dtCurrentTable.Rows[i+r][16].ToString();
                                                        dtCopy.Rows[k][16] = dtCurrentTable.Rows[i + r][16].ToString();

                                                        dtCopy.Rows[k][18] = dtCurrentTable.Rows[i + r][18].ToString();

                                                        k = k + 1;

                                                    }
                                                    else
                                                    {
                                                    }

                                                    // }

                                                    //}

                                                }
                                                //i = i + r - 1;
                                            }
                                            getFromDt = getFromDt.AddDays(1);
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }


                                    dtCurrentTable.Rows.Add(drCurrentRow);
                                    //   dtCopy.Rows.Add(drCurrentRow1);
                                    ViewState["CurrentTable"] = dtCopy;//dtCurrentTable;

                                    dsShow.Tables.Add(dtCopy);
                                }


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
                return dsShow;
            }
            finally
            {
                if (dsShow != null)
                {
                    dsShow.Dispose();
                }
                if (dtCopy != null)
                {
                    dtCopy.Dispose();
                }
            
            
            }
            return dsShow;
        }
        #endregion Add datewise dataTo Grid

        //private void WRAXBDetails_LocateDataSource(object sender, LocateDataSourceEventArgs e)
        //{

        //    string dname = e.DataSetName;
        //    //  if (dname.ToLower() == "DataSetValue")
        //    if (dname == "dsShowFlightUtilization")
        //    {
        //        e.Data = Dataset1;
        //    }
        //    else
        //    {
        //        e.Data = Dataset2;
        //    }
        //}

      
        

        #region AirCraft type List

        private void AirCraftType()
        {
            try
            {

                DataSet ds = new DataSet("FlightUtil_dsAirCraftType"); 
                    ds= OBJasb.GetAirCraftType();
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

        protected void btnclear_Click(object sender, EventArgs e)
        {
            RptFlightUtilizationViewer.Visible = false;
           ddlDestination.SelectedIndex= 0;
            ddlOrigin.SelectedIndex = 0;
            ddlCountry.SelectedIndex = 0;
           // txtFlightNo.Text="";
            ddlFlightNumber.SelectedIndex = 0;
            txtFlightToDate.Text = "";
            ddlRegion.SelectedIndex = 0;
            txtFlightFromdate.Text = "";
            lblStatus.Text = "";


        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet("FlightUtil_dsExportData");
            DataTable dt = new DataTable("FlightUtil_dtExportData");
            Session["dsFltUtilization"] = null;
            try
            {
                //if ((DataSet)Session["dsFltUtilization"] == null)
                //    GetData();

                //dsExp = (DataSet)Session["dsFltUtilization"];
                //if (dsExp != null && dsExp.Tables[0].Rows.Count > 0)
                //    dt = (DataTable)dsExp.Tables[0];
                //else
                //{
                //    lblStatus.Text = "Data Does Not Exists";
                //    lblStatus.ForeColor = Color.Red;
                //    RptFlightUtilizationViewer.Visible = false;
                //    Session["dsFltUtilization"] = null;
                //    return;
                //}

                if (Validate() == false)
                {
                    Session["dsFltUtilization"] = null;
                   
                    return;
                }
                GetDataToExport();

                lblStatus.Text = "";
                ds = (DataSet)Session["dsFltUtilization"];
                dt = (DataTable)ds.Tables[0];
                if (Session["dsFltUtilization"] == null && dt == null)
                {
                    lblStatus.Text = "No records found";
                    lblStatus.ForeColor = Color.Red;
                    SaveUserActivityLog(lblStatus.Text);
                  
                    return;
                }
                //dt = city.GetAllCity();//your datatable 
                if (dt.Columns.Contains("Logo"))
                { dt.Columns.Remove("Logo"); }

                if (dt.Columns.Contains("SerialNumber"))
                { dt.Columns.Remove("SerialNumber"); }

                if (dt.Columns.Contains("POL"))
                { dt.Columns.Remove("POL"); }
						

                dt.Columns["FltNo"].ColumnName = "Flight No";
                dt.Columns["AircraftType"].ColumnName = "Aircraft Type";
                dt.Columns["Createdon"].ColumnName = "Date";
                dt.Columns["Org"].ColumnName = "Origin";
                dt.Columns["Dest"].ColumnName = "Destination";
                dt.Columns["CargoCapacity"].ColumnName = "Capacity";
                dt.Columns["UpliftWeight"].ColumnName = "Uplift";
                dt.Columns["UpliftPerc"].ColumnName = "% Uplift";


                string attachment = "attachment; filename=CapacityvsUtilizedReport.xls";
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

                #region need to check
                //if(dt.Columns.Contains("Logo"))
                //{ dt.Columns.Remove("Logo"); }

                //string attachment = "attachment; filename=Capacity vs Utilized Report.xls";
                //Response.ClearContent();
                //Response.AddHeader("content-disposition", attachment);
                //Response.ContentType = "application/vnd.ms-excel";
                //string tab = "";
                //foreach (DataColumn dc in dt.Columns)
                //{
                //    Response.Write(tab + dc.ColumnName);
                //    tab = "\t";
                //}
                //Response.Write("\n");
                //int i;
                //foreach (DataRow dr in dt.Rows)
                //{
                //    tab = "";
                //    for (i = 0; i < dt.Columns.Count; i++)
                //    {
                //        Response.Write(tab + dr[i].ToString());
                //        tab = "\t";
                //    }
                //    Response.Write("\n");
                //}
                //Response.End();
                #endregion
            }
            catch (Exception ex)
            { }
            finally
            {
               ds = null;
                dt = null;
            }
        }

       

        #region Validate Controls
        private bool Validate()
        {
            try
            {
                try
                {
                    if (txtFlightFromdate.Text.Trim() != "" || txtFlightToDate.Text.Trim() != "")
                    {
                        DateTime dt1 = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                        DateTime dt2 = DateTime.ParseExact(txtFlightToDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                        int chk = DateTime.Compare(dt1, dt2);
                        if (chk > 0)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Please enter valid To date";
                            txtFlightFromdate.Focus();
                            return false;
                        }


                    }
                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                    txtFlightFromdate.Focus();
                    return false;
                }
                
              

                ReportBAL objBal = new ReportBAL();
                string strResult = string.Empty;

                try
                {
                    strResult = objBal.GetReportInterval(DateTime.ParseExact(txtFlightToDate.Text.Trim(), "dd/MM/yyyy", null), DateTime.ParseExact(txtFlightToDate.Text.Trim(), "dd/MM/yyyy", null));
                }
                catch
                {
                    strResult = "";
                }
                finally
                {
                    objBal = null;
                }
                if (strResult != "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = strResult;
                    txtFlightToDate.Focus();
                    return false;
                }



            }
            catch (Exception ex)
            {


            }
            return true;

        }
        #endregion

        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            // taking all parameters as user selected in report in one variable - "Param"
            string Param = "Country:" + ddlCountry.Text.Trim() + "Region:" + ddlRegion.Text.Trim() + "Org:" + ddlOrigin.Text.Trim() + "Dest:" + ddlDestination.Text.Trim() + "FltNo:" + ddlFlightNumber.Text.Trim() + "FrmDt:" + txtFlightFromdate.Text.Trim() + "ToDt:" + txtFlightToDate.Text.Trim() + "AircraftType:" + ddlAirCraftType.Text.Trim() + "Status:" + ddlStatus.Text.Trim();
            if (chkDomestic.Checked)
                Param = Param + ",Domestic:Yes";
            if (chkInternational.Checked)
                Param = Param + ",Intl:Yes";
            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "Capacity vs Utilized", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
        }

        public string GetReportInterval(DateTime FromDate, DateTime ToDate)
        {
            double DaysConfigured = 0;
            double intReportInterval = (ToDate - FromDate).TotalDays + 1;
            LoginBL objBL = new LoginBL();

            try
            {
                string strOutput = objBL.GetMasterConfiguration("ReportInterval");

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
                objBL = null;
            }

            if (DaysConfigured > 0 && intReportInterval > DaysConfigured)
            {
                RptFlightUtilizationViewer.Visible = false;
                return "Report can be generated only for " + DaysConfigured.ToString() + " days.";
            }
            else
                return "";
        }
    public void GetDataToExport()
        {
            try
            {
                Session["dsFltUtilization"] = null;
                lblStatus.Text = string.Empty;
                RptFlightUtilizationViewer.Visible = false;

                string strResult = string.Empty;
                try
                {

                    strResult = objBal.GetReportInterval(DateTime.ParseExact(txtFlightFromdate.Text.Trim(), "dd/MM/yyyy", null), DateTime.ParseExact(txtFlightToDate.Text.Trim(), "dd/MM/yyyy", null));
                }
                catch
                {
                    strResult = "";
                }
                if (strResult != "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = strResult;
                    txtFlightFromdate.Focus();
                    return;
                }
                if (txtFlightFromdate.Text == "" || txtFlightToDate.Text == "")
                {
                    //lblStatus.ForeColor = Color.Red;
                    //lblStatus.Text = "Please enter From date and To date for datewise Schedule List";
                    //txtFlightFromdate.Focus();
                    //return;
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
                        //txtFromdate.Focus();
                        return;
                    }


                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                //txtFromdate.Focus();
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
            try
            {
                int rowIndex = 0;

                string Flightfromdate = "", FlightToDate = "";
                DateTime dt1 = new DateTime();
                DateTime dt2 = new DateTime();
                try
                {
                    //if (txtFlightFromdate.Text == "" || txtFlightToDate.Text == "")
                    //{
                    //    lblStatus.ForeColor = Color.Red;
                    //    lblStatus.Text = "Please Insert From date and To date for datewise Schedule List";
                    //    txtFlightFromdate.Focus();
                    //    return;
                    //}

                    if (txtFlightFromdate.Text != "")
                    {
                        // Flightfromdate = Convert.ToDateTime(txtFlightFromdate.Text).ToString("yyyy-MM-dd HH:mm:ss");
                        //Flightfromdate = Convert.ToDateTime(txtFlightFromdate.Text).ToString("dd/MM/yyyy");
                        ////  dt1 = DateTime.Parse(txtFlightFromdate.Text);//.ToString("dd/MM/yyyy");
                        //Flightfromdate = txtFlightFromdate.Text;
                        ////DateTime dtFromDate = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null);
                        dt1 = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null);
                        //Flightfromdate = DateTime.ParseExact(txtFlightFromdate.Text, "MM/dd/yyyy", null);
                        Flightfromdate = dt1.ToString("MM/dd/yyyy");
                        //Flightfromdate = txtFlightFromdate.Text.Trim();// dt1.ToString("MM/dd/yyyy");
                    }
                    if (txtFlightToDate.Text != "")
                    {
                        //FlightToDate = Convert.ToDateTime(txtFlightToDate.Text).ToString("dd/MM/yyyy");
                        // dt2 = DateTime.Parse(txtFlightToDate.Text);//.ToString("dd/MM/yyyy");
                        dt2 = DateTime.ParseExact(txtFlightToDate.Text, "dd/MM/yyyy", null);
                        // FlightTodate = dt2.ToString("MM/dd/yyyy");
                        FlightToDate = dt2.ToString("MM/dd/yyyy");
                        //FlightToDate = txtFlightToDate.Text.Trim();// dt2.ToString("MM/dd/yyyy");


                    }

                    if (txtFlightFromdate.Text == "")
                    {


                        Flightfromdate = "";
                    }
                    if (txtFlightToDate.Text == "")
                    {

                        FlightToDate = "";


                    }
                }
                catch (Exception ex)
                {

                }


                string FlightNo = "";

                if (ddlFlightNumber.SelectedItem.Text.Trim() == "ALL")
                    FlightNo = "All";
                else
                    FlightNo = ddlFlightNumber.SelectedValue;

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

                //DataSet ds = OBJasb.GetAirlineSchedule(ddlOrigin.SelectedValue, ddlDestination.SelectedValue, FlightNo, ddlAirCraftType.SelectedValue, Flightfromdate, FlightToDate, ddlStatus.SelectedValue, strdomestic);
                string Source = "All", Dest = "All";
                if (ddlOrigin.SelectedItem.Text.Trim() != "")
                {
                    Source = ddlOrigin.SelectedValue.ToString().Trim();
                }
                if (ddlDestination.SelectedItem.Text.Trim() != "")
                {
                    Dest = ddlDestination.SelectedValue.ToString().Trim();
                }
                string country = "All";
                if (ddlCountry.SelectedItem.Text.Trim() != "")
                {
                    country = ddlCountry.SelectedValue.ToString().Trim();
                }


                string Region = "All";
                if (ddlRegion.SelectedItem.Text.Trim() != "")
                {
                    //Region = ddlRegion.SelectedValue.ToString();
                    Region = ddlRegion.SelectedValue.ToString().Trim();
                }

                DataSet ds = new DataSet("FlightUtil_dsExpFlightUtil");
                //FileInfo info;
                //ReportRuntime runtime;
                //ReportDefinition definition;
                 ds = OBJasb.GetFlightUtilization(country, Region, Source, Dest, FlightNo, ddlAirCraftType.SelectedValue, Flightfromdate.ToString(), FlightToDate.ToString(), ddlStatus.SelectedValue, strdomestic);
                //DataSet ds = OBJasb.GetFlightUtilization(country, Region, Source, Dest, FlightNo, ddlAirCraftType.SelectedValue, dt1, FlightToDate.ToString(), ddlStatus.SelectedValue, strdomestic);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Session["dsFltUtilization"] = ds;
                            DataTable myDataTable = new DataTable();
                            DataColumn myDataColumn;
                            DataSet Ds = new DataSet("FlightUtil_dsExpExport");

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "Country";
                            myDataTable.Columns.Add(myDataColumn);


                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "Region";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "Origin";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "Destination";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "FlightID";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "FromDt";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "ToDt";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "AircraftType";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "Status";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "ReportDate";
                            myDataTable.Columns.Add(myDataColumn);


                            DataRow dr;
                            dr = myDataTable.NewRow();
                            dr["Country"] = country;
                            dr["Region"] = Region; //"5";
                            dr["Origin"] = Source;// "5";
                            dr["Destination"] = Dest;
                            dr["FlightID"] = FlightNo;

                            dr["FromDt"] = Flightfromdate;
                            dr["ToDt"] = FlightToDate;// "9";
                            dr["AircraftType"] = ddlAirCraftType.SelectedValue;// "9";
                            dr["Status"] = ddlStatus.SelectedValue;// "9";
                            dr["ReportDate"] = CurrTime;// "9";
                            myDataTable.Rows.Add(dr);

                            //  Ds.Tables.Add(myDataTable);

                            DataSet dschk = new DataSet("FlightUtil_dsExpCheck");
                            dschk.Tables.Add(myDataTable);
                            Dataset2 = dschk.Copy();

                            Dataset1 = ds.Copy();

                            #region Old RDLX
                            //RptFlightUtilizationViewer.Visible = true;

                            //info = new FileInfo(Server.MapPath("/Reports/rptFlightUtilization.rdlx"));
                            ////info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptFlightUtilization.rdlx");
                            //definition = new ReportDefinition(info);
                            //runtime = new ReportRuntime(definition);


                            ////  Dataset2 = dschk;

                            //runtime.LocateDataSource += WRAXBDetails_LocateDataSource;
                            //RptFlightUtilizationViewer.SetReport(runtime);

                            //// dt.Rows[0]["From"]= txtAutoSource.Text.Trim();
                            #endregion

                            #region RDLC

                            //Logo
                            System.IO.MemoryStream Logo = null;
                            try
                            {
                                Logo = CommonUtility.GetImageStream(Page.Server);
                                //System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                            }
                            catch (Exception ex)
                            {
                                Logo = new System.IO.MemoryStream();
                            }

                            if (ds.Tables[0].Columns.Contains("Logo") == false)
                            {
                                DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                                col1.DefaultValue = Logo.ToArray();
                                ds.Tables[0].Columns.Add(col1);
                            }

                            RptFlightUtilizationViewer.Visible = false;
                            //RptFlightUtilizationViewer.ProcessingMode = ProcessingMode.Local;
                            //RptFlightUtilizationViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/FlightUtilizationRpt.rdlc");
                            ////Customers dsCustomers = GetData("select top 20 * from customers");
                            //ReportDataSource datasource = new ReportDataSource("dsFlightUtilization_dtFlightUtilization", ds.Tables[0]);
                            //RptFlightUtilizationViewer.LocalReport.DataSources.Clear();
                            //RptFlightUtilizationViewer.LocalReport.DataSources.Add(datasource);
                            //RptFlightUtilizationViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                            //SaveUserActivityLog("");

                            #endregion
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data does not exists for selected search criteria";
                            RptFlightUtilizationViewer.Visible = false;
                            Session["dsFltUtilization"] = null;
                            SaveUserActivityLog(lblStatus.Text);
                        }
                    }

                }
                if (ds != null)
                    ds.Dispose();
            }
            catch (Exception ex)
            {
                SaveUserActivityLog(ex.Message);
            }

        }

      protected void GetData()
        {
            try
            {
                Session["dsFltUtilization"] = null;
                lblStatus.Text = string.Empty;
                RptFlightUtilizationViewer.Visible = false;

                string strResult = string.Empty;
                try
                {
                    strResult = objBal.GetReportInterval(DateTime.ParseExact(txtFlightFromdate.Text.Trim(), "dd/MM/yyyy", null), DateTime.ParseExact(txtFlightToDate.Text.Trim(), "dd/MM/yyyy", null));
                }
                catch
                {
                    strResult = "";
                }
                if (strResult != "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = strResult;
                    txtFlightFromdate.Focus();
                    return;
                }
                if (txtFlightFromdate.Text == "" || txtFlightToDate.Text == "")
                {
                    //lblStatus.ForeColor = Color.Red;
                    //lblStatus.Text = "Please enter From date and To date for datewise Schedule List";
                    //txtFlightFromdate.Focus();
                    //return;
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
                        //txtFromdate.Focus();
                        return;
                    }


                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                //txtFromdate.Focus();
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
            try
            {
                int rowIndex = 0;

                string Flightfromdate = "", FlightToDate = "";
                DateTime dt1 = new DateTime();
                DateTime dt2 = new DateTime();
                try
                {
                    //if (txtFlightFromdate.Text == "" || txtFlightToDate.Text == "")
                    //{
                    //    lblStatus.ForeColor = Color.Red;
                    //    lblStatus.Text = "Please Insert From date and To date for datewise Schedule List";
                    //    txtFlightFromdate.Focus();
                    //    return;
                    //}

                    if (txtFlightFromdate.Text != "")
                    {
                        // Flightfromdate = Convert.ToDateTime(txtFlightFromdate.Text).ToString("yyyy-MM-dd HH:mm:ss");
                        // Flightfromdate = Convert.ToDateTime(txtFlightFromdate.Text).ToString("dd/MM/yyyy");
                        ////  dt1 = DateTime.Parse(txtFlightFromdate.Text);//.ToString("dd/MM/yyyy");
                        //Flightfromdate = txtFlightFromdate.Text;
                        ////DateTime dtFromDate = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null);
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

                    if (txtFlightFromdate.Text == "")
                    {


                        Flightfromdate = "";
                    }
                    if (txtFlightToDate.Text == "")
                    {

                        FlightToDate = "";


                    }
                }
                catch (Exception ex)
                {

                }


                string FlightNo = "";
               

                if (ddlFlightNumber.SelectedItem.Text.Trim() == "ALL")
                    FlightNo = "All";
                else
                    FlightNo = ddlFlightNumber.SelectedValue;

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

                //DataSet ds = OBJasb.GetAirlineSchedule(ddlOrigin.SelectedValue, ddlDestination.SelectedValue, FlightNo, ddlAirCraftType.SelectedValue, Flightfromdate, FlightToDate, ddlStatus.SelectedValue, strdomestic);
                string Source = "All", Dest = "All";
                if (ddlOrigin.SelectedItem.Text.Trim() != "")
                {
                    Source = ddlOrigin.SelectedItem.Text;
                }
                if (ddlDestination.SelectedItem.Text.Trim() != "")
                {
                    Dest = ddlDestination.SelectedItem.Text.Trim();
                }
                string country = "All";
                if (ddlCountry.SelectedItem.Text.Trim() != "")
                {
                    country = ddlCountry.SelectedItem.Text;
                }


                string Region = "All";
                if (ddlRegion.SelectedItem.Text.Trim() != "")
                {
                    Region = ddlRegion.SelectedItem.Text;
                }
                DataSet ds = new DataSet("FlightUtil_dsFlightData");
                 ds = OBJasb.GetFlightUtilization(country, Region, Source, Dest, FlightNo, ddlAirCraftType.SelectedValue, Flightfromdate.ToString(), FlightToDate.ToString(), ddlStatus.SelectedValue, strdomestic);
               // DataSet ds = OBJasb.GetFlightUtilization(country, Region, Source, Dest, FlightNo, ddlAirCraftType.SelectedValue, Flightfromdate.ToString(), FlightToDate.ToString(), ddlStatus.SelectedValue, strdomestic);
               // DataSet ds = OBJasb.GetFlightUtilization(country, Region, Source, Dest, FlightNo, ddlAirCraftType.SelectedValue, Flightfromdate.ToString, FlightToDate.ToString, ddlStatus.SelectedValue, strdomestic);

                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            Session["dsFltUtilization"] = ds;
                            Dataset1 = ds.Copy();

                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data does not exists for selected search criteria";
                            RptFlightUtilizationViewer.Visible = false;
                            Session["dsFltUtilization"] = null;
                            SaveUserActivityLog(lblStatus.Text);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SaveUserActivityLog(ex.Message);
            }
        }

        //protected void txtFlightNo_TextChanged(object sender, EventArgs e)
        //{

        //}

        protected void ddlFlightPrefix_SelectedIndexChanged(object sender, EventArgs e)
        {

            string FlightPrefixCode = ddlFlightPrefix.SelectedItem.Value.ToString();
            GetFlight(FlightPrefixCode);

        }
        public void GetFlight(string FlightPrefixCode)
        {
            DataSet dsResult = new DataSet("FlightUtil_dsFlightListPrefixWise");
            try
            {
                if (ddlFlightPrefix.SelectedItem.Value.ToString() == "Select")
                {
                    ddlFlightNumber.DataSource = "";
                    ddlFlightNumber.DataBind();
                }


                SQLServer objSQL = new SQLServer(Global.GetConnectionString());
                dsResult = objSQL.SelectRecords("spGetAllFlightListPrefixWise", "FlightPrefix", FlightPrefixCode, SqlDbType.VarChar);
                if (dsResult != null)
                {
                    if (dsResult.Tables.Count > 0)
                    {
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            ddlFlightNumber.Items.Clear();
                            ddlFlightNumber.DataSource = dsResult.Tables[0];
                            ddlFlightNumber.DataTextField = "FlightID";
                            ddlFlightNumber.DataValueField = "FlightID";
                            ddlFlightNumber.DataBind();
                            ddlFlightNumber.Items.Insert(0, new ListItem("ALL", ""));
                            ddlFlightNumber.SelectedIndex = -1;
                        }

                        else
                        {
                            ddlFlightNumber.Items.Clear();
                            lblStatus.Text = "No Flight for this Partner";
                            lblStatus.ForeColor = Color.Red;
                            ddlFlightNumber.DataSource = null;
                            ddlFlightNumber.DataBind();
                            ddlFlightNumber.Items.Insert(0, new ListItem("Select", null));
                        }
                    }
                }

                if (dsResult != null)
                    dsResult.Dispose();

            }
            catch (Exception ex)
            { }
         
        }
        public void GetFlights()
        {
            try
            {
                SQLServer objSQL = new SQLServer(Global.GetConnectionString());
                DataSet dsInstance = new DataSet("FlightUtil_dsInstnace");
                //string FlightPrefix;
                dsInstance = objSQL.SelectRecords("GetCurrentInstance");
                string current = dsInstance.Tables[0].Rows[0][0].ToString();
                //  FlightPrefix = ddlFlightPrefix.SelectedValue.ToString().Trim();
                {
                    DataSet dsResult = new DataSet("FlightUtil_dsGetFlightPrefix");
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
                    if (dsResult != null)
                        dsResult.Dispose();
                }
                
                if (dsInstance != null)
                    dsInstance.Dispose();

            }
            catch (Exception ex)
            {
                lblStatus.Text = "" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }
        
    }
}
