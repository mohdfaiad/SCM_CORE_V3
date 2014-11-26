using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.Data;
using QID.DataAccess;

using System.IO;
using System.Configuration;
using System.Drawing;
using System.Data.SqlClient;
using System.Web.Mail;
using Microsoft.Reporting.WebForms;


namespace ProjectSmartCargoManager
{
    public partial class rptOffloadReport : System.Web.UI.Page
    {
        #region Variable
        private DataSet Dataset1 = new DataSet("Offloadrpt_dsResRevenue");
        private DataSet Dataset2 = new DataSet("Offloadrpt_dsSearchCriteria");
        StockAllocationBAL objBAL = new StockAllocationBAL();
        ReportBAL objReport = new ReportBAL();
        public static string CurrTime = "";

        #endregion

        #region On page load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["awbPrefix"] != null)
                {
                    txtAwbPrefix1.Text = Session["awbPrefix"].ToString();
                }
                DataSet City = new DataSet("Offloadrpt_dsCity");

                if (!IsPostBack)
                {
                    ReportViewer1.Visible = false;
                    ////RptViewerRevenue_Station.Visible = false;
                    //FillPaymentType();
                    btnExport.Visible = true;
                    txtFromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtTodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    btnExport.Visible = true;

                    DateTime dtCurr = Convert.ToDateTime(Session["IT"].ToString());
                    CurrTime = dtCurr.ToString("dd/MM/yyyy hh:mm:ss tt");

                    City = objBAL.GetCityCode();
                    ddlLocCode.DataSource = City.Tables[0];
                    ddlLocCode.DataTextField = "Airport";
                    ddlLocCode.DataValueField = "CityCode";
                    ddlLocCode.DataBind();
                    ddlLocCode.Items.Insert(0, "All");
                    ddlLocCode.SelectedIndex = 0;
                    GetFlights();

                    if (Session["awbPrefix"] != null)
                        txtAWBPrefix.Text = Session["awbPrefix"].ToString();
                    else
                    {
                        MasterBAL objBal = new MasterBAL();
                        Session["awbPrefix"] = objBal.awbPrefix();
                        txtAWBPrefix.Text = Session["awbPrefix"].ToString();
                    }

                }
                if (City != null)
                {
                    City.Dispose();
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        public void GetFlight(string FlightPrefix)
        {

            try
            {
                DataSet dsResult = new DataSet("Offloadrpt_dsFlight");

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
                                ddlFlight.Items.Insert(0, new ListItem("All", ""));
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

                if (dsResult != null)
                {
                    dsResult.Dispose();
                }



            }
            catch (Exception)
            {


            }
        }

        #region getFlights
        public void GetFlights()
        {
            try
            {
                SQLServer objSQL = new SQLServer(Global.GetConnectionString());
                DataSet dsInstance = new DataSet("Offloadrpt_dsInstance");
                //string FlightPrefix;
                dsInstance = objSQL.SelectRecords("GetCurrentInstance");
                string current = dsInstance.Tables[0].Rows[0][0].ToString();
                //  FlightPrefix = ddlFlightPrefix.SelectedValue.ToString().Trim();
                {
                    DataSet dsResult = new DataSet("Offloadrpt_dsFlights");
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
                if (dsInstance != null)
                {
                    dsInstance.Dispose();
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }
        #endregion


        #region On Button Click Show Data for search criteria
        protected void btnList_Click(object sender, EventArgs e)
        {
            DataSet res_Revenue = new DataSet("Offloadrpt_dsListOffload");
            DataSet oDs1 = new DataSet();
            DataSet oDs2 = new DataSet();
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

                //FileInfo info;
                //ReportRuntime runtime;
                //ReportDefinition definition;

                try
                {

                    string Flightno = "All", AWBNo = "All", Location = "All", frmDate = "", ToDt = "";
                    if (ddlFlight.SelectedItem.Text.Trim() == "Select")
                        Flightno = "All";
                    else
                        Flightno = ddlFlight.SelectedItem.Text.Trim();
                    if (txtAWBNo.Text.Trim() == "")
                        AWBNo = "All";
                    else
                        AWBNo = txtAWBNo.Text.Trim();

                    if (ddlLocCode.SelectedItem.Value.ToString() != "All")
                        Location = ddlLocCode.SelectedItem.Value.ToString();

                    if (txtFromdate.Text.Trim() != "")
                        frmDate = txtFromdate.Text.Trim();
                    DateTime dtTo = new DateTime(); DateTime dtfrom = new DateTime();
                    if (txtTodate.Text.Trim() != "" && txtFromdate.Text.Trim() != "")
                    {
                        ToDt = txtTodate.Text.Trim();

                        string day = txtFromdate.Text.Substring(0, 2);
                        string mon = txtFromdate.Text.Substring(3, 2);
                        string yr = txtFromdate.Text.Substring(6, 4);
                        frmDate = yr + "-" + mon + "-" + day;
                        dtfrom = Convert.ToDateTime(frmDate);
                        frmDate = dtfrom.ToString("dd/MM/yyyy");
                        string dayTo = txtTodate.Text.Substring(0, 2);
                        string monTo = txtTodate.Text.Substring(3, 2);
                        string yrTo = txtTodate.Text.Substring(6, 4);
                        ToDt = yrTo + "-" + monTo + "-" + dayTo;
                        dtTo = Convert.ToDateTime(ToDt);
                        ToDt = dtTo.ToString("dd/MM/yyyy");
                    }

                    //AgentCode = "All"; PaymentType = "All"; contrLocatorCode = "All"; 
                    //level = "All"; levelCode = "All";


                    res_Revenue = objReport.GetOffloadAWBDetails(Flightno, AWBNo, Location, txtFromdate.Text.Trim(), txtTodate.Text.Trim());


                    if (res_Revenue != null)
                    {
                        if (res_Revenue.Tables[0].Rows.Count > 0)
                        {
                            Session["dsExp"] = res_Revenue;
                            Dataset1 = res_Revenue;
                            Dataset2 = showSearchCriteria(Flightno, AWBNo, frmDate, ToDt, Location, CurrTime);
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

                            if (res_Revenue.Tables[0].Columns.Contains("Logo") == false)
                            {
                                DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                                col1.DefaultValue = Logo.ToArray();
                                res_Revenue.Tables[0].Columns.Add(col1);
                            }


                            #region RDLX
                            //RptViewerRevenue_Station.Visible = true;



                            //info = new FileInfo(Server.MapPath("/Reports/rptOffloadReport.rdlx"));
                            ////info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptFlightTonnage.rdlx");

                            //// info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "Report1.rdlx");

                            //definition = new ReportDefinition(info);
                            //runtime = new ReportRuntime(definition);
                            //runtime.LocateDataSource += WARCustWise_LocateDataSource;
                            //RptViewerRevenue_Station.SetReport(runtime);
                            #endregion
                            ReportViewer1.Visible = true;
                            ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptOffloadReport.rdlc");
                            ReportDataSource datasource = new ReportDataSource("dsrptOffloadReport_dtrptOffloadReport", res_Revenue.Tables[0]);
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(datasource);
                            ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                            btnExport.Visible = true;
                            SaveUserActivityLog("");

                            //  oDs2 = LoadParamDataset();// just temperory used, not provide real value                 Dataset2 = oDs2;                  runtime.LocateDataSource += WARCustWise_LocateDataSource;                 WARCustWise.SetReport(runtime);                 pnlMsg.Visible = false;              }             else             {                 oDs1 = null;                 WARCustWise.Visible = false;                 // lblMsg.Text = "No Data Available";                 pnlMsg.Visible = true;                 lblMsgBox.Text = "No Data Available";                 //Pic.Visible = true;               }
                        }
                        else
                        {
                            ////RptViewerRevenue_Station.Visible = false;
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data not avialable for given search criteria";
                            Session["dsExp"] = null;
                            ReportViewer1.Visible = false;
                            txtFromdate.Focus();
                            return;


                        }
                    }
                    else
                    {
                        ReportViewer1.Visible = false;
                        ////RptViewerRevenue_Station.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not avialable for given search criteria";
                        Session["dsExp"] = null;
                        txtFromdate.Focus();
                        return;

                    }

                }
                catch (Exception ex)
                { }
                if (oDs1 != null)
                {
                    oDs1.Dispose();
                }
               
                if (oDs2 != null)
                {
                    oDs2.Dispose();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (res_Revenue != null)
                {
                    res_Revenue.Dispose();
                }
                if (oDs1 != null)
                {
                    oDs1.Dispose();
                }
                if (oDs2 != null)
                {
                    oDs2.Dispose();
                }
            }
        }
        #endregion

        #region Autopopulate GetFlightId

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string[] GetFlightId(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string");
            // SqlDataAdapter dad = new SqlDataAdapter("select distinct flightid from AirlineSchedule where FlightID  like '" + prefixText +  "%'",con);
            SqlDataAdapter dad = new SqlDataAdapter("select flightid from(select  distinct flightid,convert(int, substring( REPLACE(flightid, '*', ''),3,len(REPLACE(flightid, '*', '')))) as FlightNo from AirlineSchedule where FlightID!='' and FlightID  like '" + prefixText + "%')s order by FlightNo ASC", con);
            DataSet ds = new DataSet("Offloadrpt_dsFlightID");
            dad.Fill(ds);
            List<string> list = new List<string>(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(dr[0].ToString());

            }

            return list.ToArray();
            if (ds != null)
            {
                ds.Dispose();
            }
        }


        #endregion

        #region To show subreport
        //private void WARCustWise_LocateDataSource(object sender, LocateDataSourceEventArgs e)
        //{
        //    string dname = e.DataSetName;
        //    //  if (dname.ToLower() == "DataSetValue")    
        //    if (dname == "dsOffload")
        //    {
        //        e.Data = Dataset1;

        //    }
        //    else
        //    {
        //        e.Data = Dataset2;
        //    }
        //}
        #endregion

        #region Validate Controls
        private bool Validate()
        {
            try
            {
                try
                {
                    if (txtFromdate.Text.Trim() != "" || txtTodate.Text.Trim() != "")
                    {
                        DateTime dt1 = DateTime.ParseExact(txtFromdate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                        DateTime dt2 = DateTime.ParseExact(txtTodate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                        int chk = DateTime.Compare(dt1, dt2);
                        if (chk > 0)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Please enter valid To date";
                            txtFromdate.Focus();
                            return false;
                        }


                    }
                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                    txtFromdate.Focus();
                    return false;
                }


                //if (ddlLocCode.SelectedItem.Value.ToString() == "All")
                //{
                //    lblStatus.ForeColor = Color.Red;
                //    lblStatus.Text = "Please select level Type";
                //    txtFromdate.Focus();
                //    return;
                //}


            }
            catch (Exception ex)
            {


            }
            ReportBAL objBal = new ReportBAL();
            string strResult = string.Empty;

            try
            {
                strResult = objBal.GetReportInterval(DateTime.ParseExact(txtFromdate.Text.Trim(), "dd/MM/yyyy", null), DateTime.ParseExact(txtTodate.Text.Trim(), "dd/MM/yyyy", null));
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
               
                txtFromdate.Focus();
                return false;
            }

            return true;

        }
        #endregion

        #region SearchCriteria
        private DataSet showSearchCriteria(string FlightNo, string AWBNo, string FromDate, string Todate, string location,string CurrTime)
        {
            DataSet ds = new DataSet("Offloadrpt_dsSearch");
            try
            {
                DataTable dtSearch = new DataTable();
                DataColumn dcNew;

                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "FlightNo";
                dtSearch.Columns.Add(dcNew);

                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "AWBno";
                dtSearch.Columns.Add(dcNew);

                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "FromDate";
                dtSearch.Columns.Add(dcNew);

                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "ToDate";
                dtSearch.Columns.Add(dcNew);

                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "location";
                dtSearch.Columns.Add(dcNew);

                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "ReportDate";
                dtSearch.Columns.Add(dcNew);

                DataRow dr;
                dr = dtSearch.NewRow();

                dr["FlightNo"] = FlightNo; //"5";
                dr["AWBno"] = AWBNo;// "5";
                dr["FromDate"] = FromDate;
                dr["ToDate"] = Todate;
                dr["location"] = location;
                dr["ReportDate"] = CurrTime;
                dtSearch.Rows.Add(dr);


                ds.Tables.Add(dtSearch);
                return ds;

                if (ds != null)
                {
                    ds.Dispose();
                }
            }
            catch (Exception ex)
            {
                return ds;
            }
            
        }
        #endregion

        #region Save user activity
        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            // taking all parameters as user selected in report in one variable - "Param"
            string Param = "AWB Number:" + txtAWBNo.Text.ToString() + "Location:" + ddlLocCode.SelectedItem.Text.ToString() + "From Date:" + txtFromdate.Text.ToString() + "To Date:" + txtTodate.Text.ToString() + "Flight Number:" + ddlFlight.SelectedItem.Text.ToString();// +"Status:" + ddlStatus.SelectedItem.Text.ToString() + "Aircraft Type:" + ddlAirCraftType.SelectedItem.Text.ToString();// +", Flight From:" + txtFlightFromdate.Text.ToString() + ",Flight To:" + txtFlightToDate.Text.ToString();
            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "Offload", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
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

        protected void txtAWBNo_TextChanged(object sender, EventArgs e)
        {

        }

        public void Getdata()
        {

            DataSet res_Revenue = new DataSet("Offloadrpt_dsGetData");
            DataSet oDs1 = new DataSet();
            DataSet oDs2 = new DataSet();
            try
            {
                Session["dsExp"] = null;
                ReportViewer1.Visible = false;
                lblStatus.Text = string.Empty;

                if (Validate() == false)
                {
                    Session["dsExp"] = null;
                    ReportViewer1.Visible = false;
                    return;
                }
                //FileInfo info;
                //ReportRuntime runtime;
                //ReportDefinition definition;

                try
                {

                    string Flightno = "All", AWBNo = "All", Location = "All", frmDate = "", ToDt = "";
                    if (ddlFlight.SelectedItem.Text.Trim() != "")
                        Flightno = ddlFlight.SelectedItem.Text.Trim();
                    if (txtAWBNo.Text.Trim() != "")
                        AWBNo = txtAWBNo.Text.Trim();

                    if (ddlLocCode.SelectedItem.Value.ToString() != "All")
                        Location = ddlLocCode.SelectedItem.Value.ToString();

                    if (txtFromdate.Text.Trim() != "")
                        frmDate = txtFromdate.Text.Trim();
                    DateTime dtTo = new DateTime(); DateTime dtfrom = new DateTime();
                    if (txtTodate.Text.Trim() != "" && txtFromdate.Text.Trim() != "")
                    {
                        ToDt = txtTodate.Text.Trim();

                        string day = txtFromdate.Text.Substring(0, 2);
                        string mon = txtFromdate.Text.Substring(3, 2);
                        string yr = txtFromdate.Text.Substring(6, 4);
                        frmDate = yr + "-" + mon + "-" + day;
                        dtfrom = Convert.ToDateTime(frmDate);
                        frmDate = dtfrom.ToString("MM/dd/yyyy");
                        string dayTo = txtTodate.Text.Substring(0, 2);
                        string monTo = txtTodate.Text.Substring(3, 2);
                        string yrTo = txtTodate.Text.Substring(6, 4);
                        ToDt = yrTo + "-" + monTo + "-" + dayTo;
                        dtTo = Convert.ToDateTime(ToDt);
                        ToDt = dtTo.ToString("MM/dd/yyyy");
                    }

                    //AgentCode = "All"; PaymentType = "All"; contrLocatorCode = "All"; 
                    //level = "All"; levelCode = "All";

                    res_Revenue = objReport.GetOffloadAWBDetails(Flightno, AWBNo, Location, txtFromdate.Text.Trim(), txtTodate.Text.Trim());
                   
                    if (res_Revenue != null)
                    {
                        if (res_Revenue.Tables[0].Rows.Count > 0)
                        {
                            Dataset1 = res_Revenue;
                            Session["dsExp"] = res_Revenue;
                          //  Dataset2 = showSearchCriteria(Flightno, AWBNo, frmDate, ToDt, Location, CurrTime);

                            #region RDLX
                            //RptViewerRevenue_Station.Visible = true;



                            //info = new FileInfo(Server.MapPath("/Reports/rptOffloadReport.rdlx"));
                            ////info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptFlightTonnage.rdlx");

                            //// info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "Report1.rdlx");

                            //definition = new ReportDefinition(info);
                            //runtime = new ReportRuntime(definition);
                            //runtime.LocateDataSource += WARCustWise_LocateDataSource;
                            //RptViewerRevenue_Station.SetReport(runtime);
                            #endregion

                            ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptOffloadReport.rdlc");
                            ReportDataSource datasource = new ReportDataSource("dsrptOffloadReport_dtrptOffloadReport", res_Revenue.Tables[0]);
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(datasource);
                            ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                            btnExport.Visible = true;
                            SaveUserActivityLog("");

                            //  oDs2 = LoadParamDataset();// just temperory used, not provide real value                 Dataset2 = oDs2;                  runtime.LocateDataSource += WARCustWise_LocateDataSource;                 WARCustWise.SetReport(runtime);                 pnlMsg.Visible = false;              }             else             {                 oDs1 = null;                 WARCustWise.Visible = false;                 // lblMsg.Text = "No Data Available";                 pnlMsg.Visible = true;                 lblMsgBox.Text = "No Data Available";                 //Pic.Visible = true;               }
                        }
                        else
                        {
                          //  RptViewerRevenue_Station.Visible = false;
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data not avialable for given search criteria";
                            Session["dsExp"] = null;
                            ReportViewer1.Visible = false;
                            SaveUserActivityLog(lblStatus.Text);
                            txtFromdate.Focus();
                            return;


                        }
                    }
                    else
                    {
                        ReportViewer1.Visible = false;
                       // RptViewerRevenue_Station.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not avialable for given search criteria";
                        SaveUserActivityLog(lblStatus.Text);
                        Session["dsExp"] = null;
                        txtFromdate.Focus();
                        return;

                    }

                }
                catch (Exception ex)
                { }
                if(res_Revenue!=null)
                {
                res_Revenue.Dispose();
                }
                if (oDs1 != null)
                {
                    oDs1.Dispose();
                } 
                if(oDs1!=null)
                {
                oDs1.Dispose();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (res_Revenue != null)
                {
                    res_Revenue.Dispose();
                }
                if (oDs1 != null)
                {
                    oDs1.Dispose();
                }
                if (oDs2 != null)
                {
                    oDs2.Dispose();
                }
            }


        }

        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsrptOffloadReport_dtrptOffloadReportSUB", Dataset2.Tables[0]));
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = new DataSet("Offloadrpt_dsExport");
            DataTable dt = new DataTable("Offloadrpt_dtExport");
            Session["dsExp"] = null;
            ReportViewer1.Visible = false;
            lblStatus.Text = string.Empty;

            try
            {
                if (Validate() == false)
                {
                    Session["dsExp"] = null;

                    return;
                }
                    Getdata();

                dsExp = (DataSet)Session["dsExp"];
                if (dsExp != null && dsExp.Tables[0].Rows.Count > 0)
                    dt = (DataTable)dsExp.Tables[0];
                else
                {
                    lblStatus.Text = "No records found";
                    lblStatus.ForeColor = Color.Red;
                    Session["dsExp"] = null;
                    ReportViewer1.Visible = false;
                    SaveUserActivityLog(lblStatus.Text);
                    return;
                }

                string attachment = "attachment; filename=Offload Report.xls";
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
            string FlightPrefix = ddlFlightPrefix.SelectedItem.Value.ToString();
            GetFlight(FlightPrefix);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            SQLServer objSQL = new SQLServer(Global.GetConnectionString());
            DataSet dsInstance = new DataSet("Offloadrpt_dsInstances");
            dsInstance = objSQL.SelectRecords("GetCurrentInstance");
            string current = dsInstance.Tables[0].Rows[0][0].ToString();
            ddlFlight.SelectedIndex = 0;
            ddlFlightPrefix.SelectedValue= current;
            ddlLocCode.SelectedIndex = 0;
            txtFromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtTodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lblStatus.Text = string.Empty;
            txtAWBNo.Text = string.Empty;
        }

    }
}
