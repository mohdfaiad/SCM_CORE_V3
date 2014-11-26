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
using Microsoft.Reporting.WebForms;
using QID.DataAccess;


namespace ProjectSmartCargoManager
{
    public partial class rptDailyFlightSchedule : System.Web.UI.Page
    {

        #region Variable
        ArrayList arFlight = new ArrayList();
        ReportBAL OBJasb = new ReportBAL();
        private DataSet Dataset1;
        private DataSet Dataset2;
        public static string CurrTime = "";
        BookingBAL objBLL = new BookingBAL();
        BALException objBAL = new BALException();
        #endregion

        #region Page load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ReportViewer1.Visible = false;
                    ReportViewer2.Visible = false;
                    Session["DailyFltSch_CurrentTable"] = null;
                    btnExport.Visible = true;
                    DateTime dtCurr = Convert.ToDateTime(Session["IT"].ToString());
                    CurrTime = dtCurr.ToString("dd/MM/yyyy hh:mm:ss tt");
                    //  rptViewerShowScedule.Visible = false;
                    txtFlightFromdate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                    txtFlightToDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");


                    AirCraftType();
                    GetOrigin();
                    GetDestination();
                    LoadRegion();
                    GetCountry();
                    LoadFlightPrefix();
                    GetFlights();
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        private void GetDestination()
        {
            DataSet ds = null;
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
                            ddlDestination.DataValueField = "AirPortCode"; //ds.Tables[2].Columns["Code"].ColumnName;

                            ddlDestination.DataTextField = "AirPort";//ds.Tables[2].Columns["Code"].ColumnName;
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

        private void GetCountry()
        {
            DataSet ds = null;
            try
            {
                ds = objBAL.GetCountryCodeList(ddlCountry.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            //ds.Tables[0].Rows.Add("", "");
                            ddlCountry.DataSource = ds;
                            ddlCountry.DataMember = ds.Tables[0].TableName;
                            ddlCountry.DataValueField = "CountryCode";
                            ddlCountry.DataTextField = "Country";
                            ddlCountry.DataBind();
                            ddlCountry.Items.Insert(0, "Select");
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
        private void GetOrigin()
        {
            DataSet ds = null;
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
                            ddlOrigin.DataValueField = "AirPortCode";//ds.Tables[2].Columns["Code"].ColumnName;

                            ddlOrigin.DataTextField = "AirPort"; //ds.Tables[2].Columns["Code"].ColumnName;
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
        protected void LoadRegion()
        {
            StockAllocationBAL objBAL = new StockAllocationBAL();
            DataSet Region = null;
            try
            {
                Region = objBAL.GetRegionCode();

                ddlRegion.Items.Clear();
                ddlRegion.Items.Add(new ListItem("Select"));

                for (int intCount = 0; intCount < Region.Tables[0].Rows.Count; intCount++)
                {
                    ddlRegion.Items.Add(new ListItem(Region.Tables[0].Rows[intCount][0].ToString()));
                }
                ddlRegion.SelectedIndex = 0;
            }
            catch (Exception ex)
            { }
            finally
            {
                if (Region != null)
                    Region.Dispose();
            }
        }
        private void LoadFlightPrefix()
        {
            try
            {
                DataSet ds = objBLL.GetFlightPrefixList();
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




        }


        #region Validate
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


            }
            catch (Exception ex)
            {


            }

            ReportBAL objBal = new ReportBAL();
            string strResult = string.Empty;

            try
            {
                strResult = objBal.GetReportInterval(DateTime.ParseExact(txtFlightFromdate.Text.Trim(), "dd/MM/yyyy", null), DateTime.ParseExact(txtFlightToDate.Text.Trim(), "dd/MM/yyyy", null));
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
                txtFlightFromdate.Focus();
                return false;
            }

            return true;
        }
        #endregion

        #region Save user activity
        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            // taking all parameters as user selected in report in one variable - "Param"
            string Param = "Country:" + ddlCountry.Text.ToString() + "Region:" + ddlRegion.Text.ToString() + "Origin:" + ddlOrigin.Text.ToString() + "From Date:" + txtFlightFromdate.Text.ToString() + "To Date:" + txtFlightToDate.Text.ToString() + "Flight Number:" + ddlFlightNumber.Text.ToString() + "Status:" + ddlStatus.SelectedItem.Text.ToString() + "Aircraft Type:" + ddlAirCraftType.SelectedItem.Text.ToString();// +", Flight From:" + txtFlightFromdate.Text.ToString() + ",Flight To:" + txtFlightToDate.Text.ToString();
            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "Active Flight", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
        }

        #endregion

        #region Get Report Interval
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
                return "Report can be generated only for " + DaysConfigured.ToString() + " days.";
            else
                return "";
        }
        #endregion

        #region List

        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                Session["dsExp"] = null;
                ReportViewer1.Visible = false;
                lblStatus.Text = string.Empty;
                if (Validate() == false)
                {
                    ReportViewer1.Visible = false;
                    ReportViewer2.Visible = false;
                    
                    return;
                }

                if (rbActiveSchedule.Checked == true)
                {
                    ReportViewer2.Visible = false;
                }
                else
                {
                    ReportViewer1.Visible = false;
                 }
                lblStatus.Text = "";
                if (txtFlightFromdate.Text == "" || txtFlightToDate.Text == "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter From date and To date for datewise Schedule List";
                    ReportViewer1.Visible = false;
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
                        ReportViewer1.Visible = false;

                        //txtFromdate.Focus();
                        return;
                    }


                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                ReportViewer1.Visible = false;

                //txtFromdate.Focus();
                return;
            }


            DataSet ds = new DataSet();

            try
            {
                //int rowIndex = 0;
                // rptViewerShowScedule.Visible = false;


                string Flightfromdate = "", FlightToDate = "";
                DateTime dt1 = new DateTime();
                DateTime dt2 = new DateTime();
                try
                {
                    lblStatus.Text = "";
                    if (txtFlightFromdate.Text == "" || txtFlightToDate.Text == "")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please Insert From date and To date for datewise Schedule List";
                        ReportViewer1.Visible = false;
                        txtFlightFromdate.Focus();
                        return;
                    }

                    if (txtFlightFromdate.Text != "")
                    {
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


                string FlightNo = "All";
                // FlightNo = ddlFlight.SelectedValue;

                //FlightNo = ddlFlight.SelectedItem.Text; mod
                if (ddlFlightNumber.SelectedItem.Text.Trim() == "Select")
                    FlightNo = "All";
                else
                    FlightNo = ddlFlightNumber.SelectedValue.ToString().Trim();//.Text;

                // FlightNo = "All";
                string strdomestic = "";
                //if (chkDomestic.Checked == true && chkInternational.Checked == true)
                //{
                strdomestic = "All";
                //}

                //Changes on 2 july for Autocoplete source
                // DataSet ds = OBJasb.GetAirlineSchedule(ddlOrigin.SelectedValue, ddlDestination.SelectedValue, FlightNo, ddlAirCraftType.SelectedValue, Flightfromdate, FlightToDate, ddlStatus.SelectedValue, strdomestic);
                string Source = "All", Dest = "All";
                if (ddlOrigin.SelectedItem.Text.Trim() != "")
                {
                    Source = ddlOrigin.SelectedValue.ToString().Trim();//.Text;
                }
                if (ddlDestination.SelectedItem.Text.Trim() != "")
                {
                    Dest = ddlDestination.SelectedValue.ToString().Trim();
                }

                string Country;

                if (ddlCountry.SelectedItem.Text == "Select")
                {
                    Country = "All";
                }
                else
                {
                    Country = ddlCountry.SelectedValue.ToString().Trim();
                }

                string Regioncode;
                if (ddlRegion.SelectedItem.Text == "Select")
                {
                    Regioncode = "All";
                }
                else
                {
                    Regioncode = ddlRegion.SelectedValue.ToString().Trim();
                }

                Session["DailyFltSch_CurrentTable"] = null;
                AirlineScheduleBAL OBJasb1 = new AirlineScheduleBAL();
                string strFlightStatus = "";

                if (rbActiveSchedule.Checked == true)
                {
                    ReportViewer2.Visible = false;
                    ds = OBJasb1.GetDateWiseAirlineScheduleWithFlightStatus(Source, Dest, FlightNo, ddlAirCraftType.SelectedValue, Flightfromdate, FlightToDate, ddlStatus.SelectedValue, strdomestic, strFlightStatus);
              
              //      ds = OBJasb.GetDateWiseAirlineScheduleReport(Country, Regioncode, Source, Dest, FlightNo, ddlAirCraftType.SelectedValue, Flightfromdate, FlightToDate, ddlStatus.SelectedValue, strdomestic);
                }
                else
                {
                    ReportViewer1.Visible = false;
                    ds = OBJasb.GetMasterAirlineScheduleReport(Country, Regioncode, Source, Dest, FlightNo, ddlAirCraftType.SelectedValue, Flightfromdate, FlightToDate, ddlStatus.SelectedValue, strdomestic);
                }

                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                               // if (strFlightStatus.Contains("All"))
                                    AddDateWiseRowToGrid(ds);
                                //else
                                //    AddDateWiseRowToGridFlightStatus(ds);
                                //// Response.Redirect("ShowDateWiseSchedule.aspx",false);


                            }
                            else
                            {
                                //pnlSchedule.Visible = false;
                                //lblStatus.ForeColor = Color.Brown;
                                lblStatus.Text = "Schedule Not Available for selected criteria.";

                            }
                        }
                        else
                        {
                            //pnlSchedule.Visible = false;
                        }
                    }
                }
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            Session["dsExp"] = ds;
                        DataTable    dt = (DataTable)Session["ActSch_fltAllStatus"];//dtCurrentTable;
                            
                            DataTable myDataTable = new DataTable();
                            DataColumn myDataColumn;
                            DataSet Ds = new DataSet();

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
                            myDataColumn.ColumnName = "FlightID";
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
                            myDataColumn.ColumnName = "FromDate";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "ToDate";
                            myDataTable.Columns.Add(myDataColumn);


                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "IsDomestic";
                            myDataTable.Columns.Add(myDataColumn);


                            DataRow dr;
                            dr = myDataTable.NewRow();

                            //dr["From"] = ddlOrigin.SelectedValue;//txtAutoSource.Text.Trim(); //"5";
                            //dr["To"] = ddlDestination.SelectedValue;//txtAutoDest.Text.Trim();// "5";
                            //dr["FlightID"] = ddlFlightNumber.SelectedValue;//txtFlightNo.Text.Trim();
                            //dr["AircraftType"] = ddlAirCraftType.SelectedValue;
                            //dr["Status"] = ddlStatus.SelectedValue;
                            //dr["FromDate"] = txtFlightFromdate.Text.Trim();
                            //dr["ToDate"] = txtFlightToDate.Text;// "9";
                            //dr["IsDomestic"] = "";
                            //myDataTable.Rows.Add(dr);

                            dr["From"] = Source;
                            dr["To"] = Dest;
                            dr["FlightID"] = FlightNo;// ddlFlightNumber.SelectedValue;//txtFlightNo.Text.Trim();
                            dr["AircraftType"] = ddlAirCraftType.SelectedValue;
                            dr["Status"] = ddlStatus.SelectedValue;
                            dr["FromDate"] = txtFlightFromdate.Text.Trim();
                            dr["ToDate"] = txtFlightToDate.Text;// "9";
                            dr["IsDomestic"] = "";
                            myDataTable.Rows.Add(dr);

                            // Ds.Tables.Add(myDataTable);

                            DataSet dschk = new DataSet();
                            dschk.Tables.Add(myDataTable);
                            //Dataset2 = dschk.Copy();

                            //if (rbActiveSchedule.Checked == true)
                            //    Dataset1 = AddDateWiseRowToGrid(ds);
                            //else
                                Dataset1 = ds;

                            if (Dataset1.Tables[0].Rows.Count <= 0)
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Data not available for given searching criteria";
                                ReportViewer1.Visible = false;
                                ReportViewer2.Visible = false;

                                return;
                            }
                            #region old code

                            //    rptViewerShowScedule.Visible = true;

                            ////    info = new FileInfo(Server.MapPath("Report1.rdlx"));
                            //    if (rbActiveSchedule.Checked == true)
                            //    {
                            //        //info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "Report1.rdlx");
                            //        info = new FileInfo(Server.MapPath("/Reports/Report1.rdlx"));
                            //    }
                            //    else
                            //    {
                            //        //info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptMasterSchedule.rdlx");
                            //        info = new FileInfo(Server.MapPath("/Reports/rptMasterSchedule.rdlx"));
                            //    }
                            //     definition = new ReportDefinition(info);
                            //    runtime = new ReportRuntime(definition);


                            //  //  Dataset2 = dschk;

                            //    runtime.LocateDataSource += WRAXBDetails_LocateDataSource;
                            //    rptViewerShowScedule.SetReport(runtime);
                            //    btnExport.Visible = true;
                            #endregion

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
                            if (dt.Columns.Contains("Logo") == false)
                            {
                                DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                                col1.DefaultValue = Logo.ToArray();
                               dt.Columns.Add(col1);
                            }

                            if (rbActiveSchedule.Checked)
                            {
                                ReportViewer1.Visible = true;


                                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptDailyFlightSchedule.rdlc");
                                ReportDataSource datasource = new ReportDataSource("dsrptDailyFlightSchedule_dtrptDailyFlightSchedule",dt);
                                ReportViewer1.LocalReport.DataSources.Clear();
                                ReportViewer1.LocalReport.DataSources.Add(datasource);
                                //   ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                            }
                            else
                            {
                                ReportViewer2.Visible = true;
                                ReportViewer2.ProcessingMode = ProcessingMode.Local;
                                ReportViewer2.LocalReport.ReportPath = Server.MapPath("~/Reports/rptDailyFlightMasterSchedule.rdlc");
                                ReportDataSource datasource = new ReportDataSource("dsrptDailyFlightMasterSchedule_dtrptDailyFlightMasterSchedule", ds.Tables[0]);
                                ReportViewer2.LocalReport.DataSources.Clear();
                                ReportViewer2.LocalReport.DataSources.Add(datasource);

                            }
                            btnExport.Visible = true;
                            SaveUserActivityLog("");


                            // dt.Rows[0]["From"]= txtAutoSource.Text.Trim();


                        }
                        else
                        {
                            lblStatus.Text = "No records found";
                            lblStatus.ForeColor = Color.Red;
                            SaveUserActivityLog(lblStatus.Text);
                            ReportViewer1.Visible = false;
                            Session["dsExp"] = null;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                lblStatus.Text = "Data not available for given searching criteria";
                ReportViewer1.Visible = false;
                ReportViewer2.Visible = false;

                return;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }

            }

        }

        #endregion

        #region  Clear button
        protected void btnclear_Click(object sender, EventArgs e)
        {
            Session["DailyFltSch_CurrentTable"] = null;
            // rptViewerShowScedule.Visible = false;
            //txtRegion.Text = "";
            //txtCountry.Text = "";
            ddlRegion.SelectedIndex = 0;
            ddlCountry.SelectedIndex = 0;
            // txtFlightNo.Text = "";
            // txtAutoDest.Text = "";
            ddlFlightNumber.SelectedIndex = 0;
            ddlDestination.SelectedIndex = 0;
            ddlOrigin.SelectedIndex = 0;
            //txtAutoSource.Text = "";
            lblStatus.Text = "";
            ReportViewer2.Visible = false;
            ReportViewer1.Visible = false;
        }
        #endregion

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
                                                        dtCopy.Rows[k][0] = dtCurrentTable.Rows[i][0].ToString();   //code added for displying all flt#
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
                                                            string[] Hrdept = dtCurrentTable.Rows[i][4].ToString().Split(':');
                                                            string strDeptTime = Hrdept[0].PadLeft(2, '0') + ":" + Hrdept[1].PadLeft(2, '0');
                                                            dtCopy.Rows[k][4] = " - " + strDeptTime;
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
                                                            dtaddfrm = dtaddfrm.AddDays(int.Parse(dtCurrentTable.Rows[i + r][5].ToString()) - 1);
                                                            dtCopy.Rows[k][5] = getFromDt.ToString("dd/MM/yyyy");
                                                        }

                                                        string[] Hrarr = dtCurrentTable.Rows[i + r][6].ToString().Split(':');
                                                        string strArrTime = Hrarr[0].PadLeft(2, '0') + ":" + Hrarr[1].PadLeft(2, '0');

                                                        dtCopy.Rows[k][6] = " - " + strArrTime;

                                                        Label ddlIsActive = new Label();
                                                        string Isactive = dtCurrentTable.Rows[i][11].ToString();
                                                        dtCopy.Rows[k][11] = dtCurrentTable.Rows[i][11].ToString();
                                                        dtCopy.Rows[k][17] = dtCurrentTable.Rows[i + r][17].ToString();
                                                        dtCopy.Rows[k][16] = dtCurrentTable.Rows[i + r][16].ToString();
                                                        dtCopy.Rows[k][18] = dtCurrentTable.Rows[i + r][18].ToString();
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

                            //    GridView1.DataSource = null;
                               // GridView1.DataSource = dtCopy; // dtCurrentTable;
                               // GridView1.DataBind();

                               // GridView1.Visible = true;

                                arFlight.Clear();

                               // dtCopy = SetFlightsGridData(dtCopy, ds);

                               // GridView1.DataSource = null;
                               // GridView1.DataSource = dtCopy;
                               // GridView1.DataBind();
                                Session["ActSch_fltAllStatus"] = dtCopy.Copy();
                                Session["ActSch_fltAllStatusDataset"] = ds.Copy();
                               // pnlSchedule.Visible = true;
                               // pnlDestDetails.Visible = true;
                              //  Session["DailyFltSch_CurrentTable"] = ds;
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

        //private void WRAXBDetails_LocateDataSource(object sender, LocateDataSourceEventArgs e)
        //{

        //    string dname = e.DataSetName;
        //    //  if (dname.ToLower() == "DataSetValue")
        //    if (rbActiveSchedule.Checked == true)
        //    {
        //        if (dname == "dsActiveFlights")
        //        {
        //            e.Data = Dataset1;
        //        }
        //        else
        //        {
        //            e.Data = Dataset2;
        //        }
        //    }
        //    else
        //    {
        //        if (dname == "dsMasterSchedule")
        //        {
        //            e.Data = Dataset1;
        //        }
        //        else
        //        {
        //            e.Data = Dataset2;
        //        }
        //    }
        //}


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


        /*   public void Getdata()
        {
            try
            {
                if (Validate() == false)
                {
                    Session["dsExp"] = null;
                    ReportViewer1.Visible = false;
                    return;
                }
                lblStatus.Text = "";
                if (txtFlightFromdate.Text == "" || txtFlightToDate.Text == "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter From date and To date for datewise Schedule List";
                    ReportViewer1.Visible = false;                    
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
                        ReportViewer1.Visible = false;
                        //txtFromdate.Focus();
                        return;
                    }


                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                ReportViewer1.Visible = false;
                //txtFromdate.Focus();
                return;
            }

            DataSet dschk = new DataSet();

            DataSet ds = new DataSet();
            try
            {
                int rowIndex = 0;
                rptViewerShowScedule.Visible = false;
                string Flightfromdate = "", FlightToDate = "";
                DateTime dt1 = new DateTime();
                DateTime dt2 = new DateTime();
                try
                {
                    lblStatus.Text = "";
                    if (txtFlightFromdate.Text == "" || txtFlightToDate.Text == "")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please Insert From date and To date for datewise Schedule List";
                        ReportViewer1.Visible = false;
                        txtFlightFromdate.Focus();
                        return;
                    }

                    if (txtFlightFromdate.Text != "")
                    {
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


                string FlightNo = "All";
                // FlightNo = ddlFlight.SelectedValue;

                //FlightNo = ddlFlight.SelectedItem.Text; mod
                if (ddlFlightNumber.Text == "")
                    FlightNo = "All";
                else
                    FlightNo = ddlFlightNumber.SelectedItem.ToString().Trim();//.Text;

                // FlightNo = "All";
                string strdomestic = "";
                //if (chkDomestic.Checked == true && chkInternational.Checked == true)
                //{
                strdomestic = "All";
                //}

                //Changes on 2 july for Autocoplete source
                // DataSet ds = OBJasb.GetAirlineSchedule(ddlOrigin.SelectedValue, ddlDestination.SelectedValue, FlightNo, ddlAirCraftType.SelectedValue, Flightfromdate, FlightToDate, ddlStatus.SelectedValue, strdomestic);
                string Source = "All", Dest = "All";
                if (ddlOrigin.SelectedItem.Text.Trim() != "")
                {
                    Source = ddlOrigin.SelectedValue.ToString().Trim();//.Text;
                }
                if (ddlDestination.SelectedItem.Text.Trim() != "")
                {
                    Dest = ddlDestination.SelectedValue.ToString().Trim();
                }

                string Country = "All";

                if (ddlCountry.SelectedItem.Text.Trim() != "")
                {
                    Country = ddlCountry.SelectedValue.ToString().Trim();
                }

                string Regioncode = "All";
                if (ddlRegion.SelectedItem.Text.Trim() != "")
                {
                    Regioncode = ddlRegion.SelectedValue.ToString().Trim();
                }



               // FileInfo info;
               // ReportRuntime runtime;
               // ReportDefinition definition;

                if (rbActiveSchedule.Checked == true)
                    ds = OBJasb.GetDateWiseAirlineScheduleReport(Country, Regioncode, Source, Dest, FlightNo, ddlAirCraftType.SelectedValue, Flightfromdate, FlightToDate, ddlStatus.SelectedValue, strdomestic);
                else
                    ds = OBJasb.GetMasterAirlineScheduleReport(Country, Regioncode, Source, Dest, FlightNo, ddlAirCraftType.SelectedValue, Flightfromdate, FlightToDate, ddlStatus.SelectedValue, strdomestic);
            
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            Session["dsExp"] = ds;

                            DataTable myDataTable = new DataTable();
                            DataColumn myDataColumn;
                            DataSet Ds = new DataSet();

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
                            myDataColumn.ColumnName = "FlightID";
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
                            myDataColumn.ColumnName = "FromDate";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "ToDate";
                            myDataTable.Columns.Add(myDataColumn);


                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "IsDomestic";
                            myDataTable.Columns.Add(myDataColumn);


                            DataRow dr;
                            dr = myDataTable.NewRow();

                            dr["From"] = ddlOrigin.SelectedValue;//txtAutoSource.Text.Trim(); //"5";
                            dr["To"] = ddlDestination.SelectedValue;//txtAutoDest.Text.Trim();// "5";
                            dr["FlightID"] = ddlFlightNumber.SelectedValue;//txtFlightNo.Text.Trim();
                            dr["AircraftType"] = ddlAirCraftType.SelectedValue;
                            dr["Status"] = ddlStatus.SelectedValue;
                            dr["FromDate"] = txtFlightFromdate.Text.Trim();
                            dr["ToDate"] = txtFlightToDate.Text;// "9";
                            dr["IsDomestic"] = "";
                            myDataTable.Rows.Add(dr);

                            // Ds.Tables.Add(myDataTable);

                          //  DataSet dschk = new DataSet();
                            dschk.Tables.Add(myDataTable);
                            //Dataset2 = dschk.Copy();

                            if (rbActiveSchedule.Checked == true)
                                Dataset1 = AddDateWiseRowToGrid(ds);
                            else
                                Dataset1 = ds;

                            if (Dataset1.Tables[0].Rows.Count <= 0)
                            {
                                lblStatus.Text = "Data not available for given searching criteria";
                                ReportViewer1.Visible = false;
                                return;
                            }
                            #region old code

                            //    rptViewerShowScedule.Visible = true;

                            ////    info = new FileInfo(Server.MapPath("Report1.rdlx"));
                            //    if (rbActiveSchedule.Checked == true)
                            //    {
                            //        //info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "Report1.rdlx");
                            //        info = new FileInfo(Server.MapPath("/Reports/Report1.rdlx"));
                            //    }
                            //    else
                            //    {
                            //        //info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptMasterSchedule.rdlx");
                            //        info = new FileInfo(Server.MapPath("/Reports/rptMasterSchedule.rdlx"));
                            //    }
                            //     definition = new ReportDefinition(info);
                            //    runtime = new ReportRuntime(definition);


                            //  //  Dataset2 = dschk;

                            //    runtime.LocateDataSource += WRAXBDetails_LocateDataSource;
                            //    rptViewerShowScedule.SetReport(runtime);
                            //    btnExport.Visible = true;
                            #endregion

                            //System.IO.MemoryStream Logo = null;
                            //try
                            //{
                            //    Logo = CommonUtility.GetImageStream(Page.Server);
                            //    //System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                            //}
                            //catch (Exception ex)
                            //{
                            //    Logo = new System.IO.MemoryStream();
                            //}

                            //if (ds.Tables[0].Columns.Contains("Logo") == false)
                            //{
                            //    DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                            //    col1.DefaultValue = Logo.ToArray();
                            //    ds.Tables[0].Columns.Add(col1);
                            //}

                            //ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            //ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptDailyFlightSchedule.rdlc");
                            //ReportDataSource datasource = new ReportDataSource("dsrptDailyFlightSchedule_dtrptDailyFlightSchedule", ds.Tables[0]);
                            //ReportViewer1.LocalReport.DataSources.Clear();
                            //ReportViewer1.LocalReport.DataSources.Add(datasource);
                            ////   ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);

                            btnExport.Visible = true;
                            SaveUserActivityLog("");


                            // dt.Rows[0]["From"]= txtAutoSource.Text.Trim();


                        }
                    }
                }
            }


            catch (Exception ex)
            {
                ReportViewer1.Visible = false;
                lblStatus.Text = "Data not available for given searching criteria";

                return;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
                if(dschk!=null)
                dschk.Dispose();
            }

        }
        */

        #region Getdata to export
        public void Getdata()
        {
            AirlineScheduleBAL OBJasb1 = new AirlineScheduleBAL();

            try
            {
                
                Session["dsExp"] = null;
                ReportViewer1.Visible = false;
                lblStatus.Text = string.Empty;
                if (Validate() == false)
                {
                    ReportViewer1.Visible = false;
                    return;
                }
                lblStatus.Text = "";
                if (txtFlightFromdate.Text == "" || txtFlightToDate.Text == "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter From date and To date for datewise Schedule List";
                    ReportViewer1.Visible = false;
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
                        ReportViewer1.Visible = false;

                        //txtFromdate.Focus();
                        return;
                    }


                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                ReportViewer1.Visible = false;

                //txtFromdate.Focus();
                return;
            }


            DataSet ds = new DataSet();

            try
            {
                //int rowIndex = 0;
                // rptViewerShowScedule.Visible = false;


                string Flightfromdate = "", FlightToDate = "";
                DateTime dt1 = new DateTime();
                DateTime dt2 = new DateTime();
                try
                {
                    lblStatus.Text = "";
                    if (txtFlightFromdate.Text == "" || txtFlightToDate.Text == "")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please Insert From date and To date for datewise Schedule List";
                        ReportViewer1.Visible = false;
                        txtFlightFromdate.Focus();
                        return;
                    }

                    if (txtFlightFromdate.Text != "")
                    {
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


                string FlightNo = "All";
                // FlightNo = ddlFlight.SelectedValue;

                //FlightNo = ddlFlight.SelectedItem.Text; mod
                if (ddlFlightNumber.SelectedItem.Text.Trim() == "Select")
                    FlightNo = "All";
                else
                    FlightNo = ddlFlightNumber.SelectedValue.ToString().Trim();//.Text;

                // FlightNo = "All";
                string strdomestic = "";
                //if (chkDomestic.Checked == true && chkInternational.Checked == true)
                //{
                strdomestic = "All";
                //}

                //Changes on 2 july for Autocoplete source
                // DataSet ds = OBJasb.GetAirlineSchedule(ddlOrigin.SelectedValue, ddlDestination.SelectedValue, FlightNo, ddlAirCraftType.SelectedValue, Flightfromdate, FlightToDate, ddlStatus.SelectedValue, strdomestic);
                string Source = "All", Dest = "All";
                if (ddlOrigin.SelectedItem.Text.Trim() != "")
                {
                    Source = ddlOrigin.SelectedValue.ToString().Trim();//.Text;
                }
                if (ddlDestination.SelectedItem.Text.Trim() != "")
                {
                    Dest = ddlDestination.SelectedValue.ToString().Trim();
                }

                string Country;

                if (ddlCountry.SelectedItem.Text == "Select")
                {
                    Country = "All";
                }
                else
                {
                    Country = ddlCountry.SelectedValue.ToString().Trim();
                }

                string Regioncode;
                if (ddlRegion.SelectedItem.Text == "Select")
                {
                    Regioncode = "All";
                }
                else
                {
                    Regioncode = ddlRegion.SelectedValue.ToString().Trim();
                }
                string strFlightStatus = "";

                Session["DailyFltSch_CurrentTable"] = null;

                if (rbActiveSchedule.Checked == true)
                    ds = OBJasb1.GetDateWiseAirlineScheduleWithFlightStatus(Source, Dest, FlightNo, ddlAirCraftType.SelectedValue, Flightfromdate, FlightToDate, ddlStatus.SelectedValue, strdomestic, strFlightStatus);
                else
                    ds = OBJasb.GetMasterAirlineScheduleReport(Country, Regioncode, Source, Dest, FlightNo, ddlAirCraftType.SelectedValue, Flightfromdate, FlightToDate, ddlStatus.SelectedValue, strdomestic);


                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                // if (strFlightStatus.Contains("All"))
                                AddDateWiseRowToGrid(ds);
                                //else
                                //    AddDateWiseRowToGridFlightStatus(ds);
                                //// Response.Redirect("ShowDateWiseSchedule.aspx",false);


                            }
                            else
                            {
                                //pnlSchedule.Visible = false;
                                //lblStatus.ForeColor = Color.Brown;
                                lblStatus.Text = "Schedule Not Available for selected criteria.";

                            }
                        }
                        else
                        {
                            //pnlSchedule.Visible = false;
                        }
                    }
                }


                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            Session["dsExp"] = ds;
                            DataTable dt = (DataTable)Session["ActSch_fltAllStatus"];//dtCurrentTable;

                            DataTable myDataTable = new DataTable();
                            DataColumn myDataColumn;
                            DataSet Ds = new DataSet();

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
                            myDataColumn.ColumnName = "FlightID";
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
                            myDataColumn.ColumnName = "FromDate";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "ToDate";
                            myDataTable.Columns.Add(myDataColumn);


                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "IsDomestic";
                            myDataTable.Columns.Add(myDataColumn);


                            DataRow dr;
                            dr = myDataTable.NewRow();

                            //dr["From"] = ddlOrigin.SelectedValue;//txtAutoSource.Text.Trim(); //"5";
                            //dr["To"] = ddlDestination.SelectedValue;//txtAutoDest.Text.Trim();// "5";
                            //dr["FlightID"] = ddlFlightNumber.SelectedValue;//txtFlightNo.Text.Trim();
                            //dr["AircraftType"] = ddlAirCraftType.SelectedValue;
                            //dr["Status"] = ddlStatus.SelectedValue;
                            //dr["FromDate"] = txtFlightFromdate.Text.Trim();
                            //dr["ToDate"] = txtFlightToDate.Text;// "9";
                            //dr["IsDomestic"] = "";
                            //myDataTable.Rows.Add(dr);

                            dr["From"] = Source;
                            dr["To"] = Dest;
                            dr["FlightID"] = FlightNo;// ddlFlightNumber.SelectedValue;//txtFlightNo.Text.Trim();
                            dr["AircraftType"] = ddlAirCraftType.SelectedValue;
                            dr["Status"] = ddlStatus.SelectedValue;
                            dr["FromDate"] = txtFlightFromdate.Text.Trim();
                            dr["ToDate"] = txtFlightToDate.Text;// "9";
                            dr["IsDomestic"] = "";
                            myDataTable.Rows.Add(dr);

                            // Ds.Tables.Add(myDataTable);

                            DataSet dschk = new DataSet();
                            dschk.Tables.Add(myDataTable);
                            //Dataset2 = dschk.Copy();

                            //if (rbActiveSchedule.Checked == true)
                            //    Dataset1 = AddDateWiseRowToGrid(ds);
                            //else
                            Dataset1 = ds;

                            if (Dataset1.Tables[0].Rows.Count <= 0)
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Data not available for given searching criteria";
                                ReportViewer1.Visible = false;
                                return;
                            }
                            #region old code

                            //    rptViewerShowScedule.Visible = true;

                            ////    info = new FileInfo(Server.MapPath("Report1.rdlx"));
                            //    if (rbActiveSchedule.Checked == true)
                            //    {
                            //        //info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "Report1.rdlx");
                            //        info = new FileInfo(Server.MapPath("/Reports/Report1.rdlx"));
                            //    }
                            //    else
                            //    {
                            //        //info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptMasterSchedule.rdlx");
                            //        info = new FileInfo(Server.MapPath("/Reports/rptMasterSchedule.rdlx"));
                            //    }
                            //     definition = new ReportDefinition(info);
                            //    runtime = new ReportRuntime(definition);


                            //  //  Dataset2 = dschk;

                            //    runtime.LocateDataSource += WRAXBDetails_LocateDataSource;
                            //    rptViewerShowScedule.SetReport(runtime);
                            //    btnExport.Visible = true;
                            #endregion

                            //System.IO.MemoryStream Logo = null;
                            //try
                            //{
                            //    Logo = CommonUtility.GetImageStream(Page.Server);
                            //    //System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                            //}
                            //catch (Exception ex)
                            //{
                            //    Logo = new System.IO.MemoryStream();
                            //}

                            //if (ds.Tables[0].Columns.Contains("Logo") == false)
                            //{
                            //    DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                            //    col1.DefaultValue = Logo.ToArray();
                            //    ds.Tables[0].Columns.Add(col1);
                            //}

                            //if (rbActiveSchedule.Checked)
                            //{
                            //    ReportViewer1.Visible = true;


                            //    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            //    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptDailyFlightSchedule.rdlc");
                            //    ReportDataSource datasource = new ReportDataSource("dsrptDailyFlightSchedule_dtrptDailyFlightSchedule", ds.Tables[0]);
                            //    ReportViewer1.LocalReport.DataSources.Clear();
                            //    ReportViewer1.LocalReport.DataSources.Add(datasource);
                            //    //   ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                            //}
                            //else
                            //{
                            //    ReportViewer2.Visible = true;
                            //    ReportViewer2.ProcessingMode = ProcessingMode.Local;
                            //    ReportViewer2.LocalReport.ReportPath = Server.MapPath("~/Reports/rptDailyFlightMasterSchedule.rdlc");
                            //    ReportDataSource datasource = new ReportDataSource("dsrptDailyFlightMasterSchedule_dtrptDailyFlightMasterSchedule", ds.Tables[0]);
                            //    ReportViewer2.LocalReport.DataSources.Clear();
                            //    ReportViewer2.LocalReport.DataSources.Add(datasource);

                            //}
                            btnExport.Visible = true;
                            SaveUserActivityLog("");


                            // dt.Rows[0]["From"]= txtAutoSource.Text.Trim();


                        }
                        else
                        {
                            lblStatus.Text = "No records found";
                            lblStatus.ForeColor = Color.Red;
                            SaveUserActivityLog(lblStatus.Text);
                            ReportViewer1.Visible = false;
                            Session["dsExp"] = null;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                lblStatus.Text = "Data not available for given searching criteria";
                ReportViewer1.Visible = false;

                return;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }

            }

        }
        #endregion

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = null;
            DataTable dt = null;
            Session["dsExp"] = null;
            Session["ActSch_fltAllStatusDataset"] = null;
            ReportViewer1.Visible = false;
            ReportViewer2.Visible = false;

          
            try
            {
                if (Session["dsExp"] == null)
                    Getdata();


                if (rbActiveSchedule.Checked == true)
                {
                  
              dt = (DataTable)Session["ActSch_fltAllStatus"];//dtCurrentTable;

                    if(dt.Rows.Count>0)
                    {
                       dt.Columns.Add("ArrivalTime");
                       dt.Columns.Add("DepartureTime");
             
                        foreach (DataRow r in dt.Rows)
                
                        {
                            r["ArrivalTime"] = Convert.ToString(r["SchArrDay"]) + "/" + Convert.ToString(r["SchArrTime"]);
              
                        }


              
                        foreach (DataRow r in dt.Rows)
              
                        {
                  
                            r["DepartureTime"] = Convert.ToString(r["SchDeptDay"]) + "/" + Convert.ToString(r["SchDeptTime"]);
              
                        }

             
                        if (dt.Columns.Contains("Dest1"))
              
                        {
                  
                            dt.Columns.Remove("Dest1");
              
                        }
            
                        if (dt.Columns.Contains("SchDeptDay"))
              
                        {
                  
                            dt.Columns.Remove("SchDeptDay");
              
                        }
              
                        if (dt.Columns.Contains("SchArrDay"))
              
                        {
                 
                            dt.Columns.Remove("SchArrDay");
              
                        }
              
                        if (dt.Columns.Contains("EquipmentNo"))
              
                        {
                
                            dt.Columns.Remove("EquipmentNo");
              
                        }
             
                        if (dt.Columns.Contains("Source1"))
              
                        {
                
                            dt.Columns.Remove("Source1");
              
                        }
            
                        if (dt.Columns.Contains("Dest2"))
              
                        {
                
                            dt.Columns.Remove("Dest2");
              
                        }

                        if (dt.Columns.Contains("CargoCapacity"))
                        {

                            dt.Columns.Remove("CargoCapacity");

                        }
             
                        if (dt.Columns.Contains("ScheduleID"))
              
                        {
                 
                            dt.Columns.Remove("ScheduleID");
              
                        }
             
                        if (dt.Columns.Contains("Frequency"))
              
                        {
                
                            dt.Columns.Remove("Frequency");
              
                        }
           
                        if (dt.Columns.Contains("FromtDt"))
              
                        {
                 
                            dt.Columns.Remove("FromtDt");
              
                        }
            
                        if (dt.Columns.Contains("ToDt"))
              
                        {
               
                            dt.Columns.Remove("ToDt");
              
                        }
           
                        if (dt.Columns.Contains("OperationStatus"))
             
                        {
                
                            dt.Columns.Remove("OperationStatus");
              
                        }

                        if (dt.Columns.Contains("SchDeptTime"))
                        {
                            dt.Columns.Remove("SchDeptTime");
                        }
                        if (dt.Columns.Contains("SchArrTime"))
                        {
                            dt.Columns.Remove("SchArrTime");
                        }
                        if (dt.Columns.Contains("tab_index"))
              
                        {
                  
                      dt.Columns.Remove("tab_index");
              
                        }

                        dt.Columns["DepartureTime"].SetOrdinal(3);
                        dt.Columns["ArrivalTime"].SetOrdinal(4);
                        dt.Columns["AirCraftType"].SetOrdinal(5);

                        dt.Columns["CargoCapacity1"].SetOrdinal(6);
                       
                        dt.Columns["Status"].SetOrdinal(7);

                
                    }
                else
                {
                    lblStatus.Text = "No records found";
                    SaveUserActivityLog(lblStatus.Text);
                    Session["ActSch_fltAllStatus"] = null;
                    return;
                }

                }
                else
                {
                    dsExp = (DataSet)Session["dsExp"];
                    dt = (DataTable)dsExp.Tables[0];

                    if (dsExp != null && dsExp.Tables[0].Rows.Count > 0)
                    
                    {                

                    if (dt.Columns.Contains("Dest1"))
                    {
                        dt.Columns.Remove("Dest1");
                    }
                    if (dt.Columns.Contains("SchDeptDay"))
                    {
                        dt.Columns.Remove("SchDeptDay");
                    }
                    if (dt.Columns.Contains("SchArrDay"))
                    {
                        dt.Columns.Remove("SchArrDay");
                    }
                    if (dt.Columns.Contains("EquipmentNo"))
                    {
                        dt.Columns.Remove("EquipmentNo");
                    }
                    if (dt.Columns.Contains("Source1"))
                    {
                        dt.Columns.Remove("Source1");
                    }
                    if (dt.Columns.Contains("Dest2"))
                    {
                        dt.Columns.Remove("Dest2");
                    }
                    if (dt.Columns.Contains("CargoCapacity1"))
                    {
                        dt.Columns.Remove("CargoCapacity1");
                    }
                    if (dt.Columns.Contains("ScheduleID"))
                    {
                        dt.Columns.Remove("ScheduleID");
                    }
                   
                  
                    if (dt.Columns.Contains("OperationStatus"))
                    {
                        dt.Columns.Remove("OperationStatus");
                    }

                    if (dt.Columns.Contains("tab_index"))
                    {
                        dt.Columns.Remove("tab_index");
                    }
                    dt.Columns["FromtDt"].SetOrdinal(3);
                    dt.Columns["ToDt"].SetOrdinal(4);

                    dt.Columns["AirCraftType"].SetOrdinal(8);
                    dt.Columns["CargoCapacity"].SetOrdinal(9);
                    dt.Columns["Status"].SetOrdinal(10);


                }
                    else
                    {
                        lblStatus.Text = "No records found";
                        SaveUserActivityLog(lblStatus.Text);
                        Session["dsExp"] = null;
                        return;
                    }
                }
                
              
                
                string attachment;

                if (rbActiveSchedule.Checked == true)
                {
                     attachment = "attachment; filename=ActiveScheduleReport.xls";
                }
                else
                {
                    attachment = "attachment; filename=MasterScheduleReport.xls";
                }
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
            }
            catch (Exception ex)
            { }
            finally
            {
                dsExp = null;
                dt = null;
            }
        }

        protected void ddlFlightPrefix_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FlightPrefixCode = ddlFlightPrefix.SelectedItem.Value.ToString();
            GetFlight(FlightPrefixCode);
        }

        public void GetFlight(string FlightPrefixCode)
        {
            DataSet dsResult = new DataSet();
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
                            ddlFlightNumber.Items.Insert(0, new ListItem("Select", ""));
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


            }
            catch (Exception ex)
            { }

        }
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
    }
}
