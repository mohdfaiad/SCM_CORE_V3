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
using Microsoft.Reporting.WebForms;

namespace ProjectSmartCargoManager
{
    public partial class rptFlightWiseTonnageReport : System.Web.UI.Page
    {
        #region Variable
        private DataSet Dataset1;
        private DataSet Dataset2;
        StockAllocationBAL objBAL = new StockAllocationBAL();
        ReportBAL objReport = new ReportBAL();
        public static string CurrTime = "";
        #endregion


        #region On page load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    FlightPerformanceRptViewer.Visible = false;
                    FlightPerformanceRptViewer_Detail.Visible = false;
                    Session["dsExpFlightTonnage"] = null;
                    //FillPaymentType();
                    txtFromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtTodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    DateTime dtCurr = Convert.ToDateTime(Session["IT"].ToString());
                    CurrTime = dtCurr.ToString("dd/MM/yyyy hh:mm:ss tt");

                    //DataSet City = objBAL.GetCityCode();
                    //ddlCode.DataSource = City.Tables[0];
                    //ddlCode.DataTextField = "CityCode";
                    //ddlCode.DataValueField = "CityCode";
                    ddlCode.DataSource = null;
                    ddlCode.DataBind();
                    ddlCode.Items.Insert(0, "All");
                    ddlCode.SelectedIndex = 0;

                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region On Button Click Show Data for search criteria
        protected void btnList_Click(object sender, EventArgs e)
        {

            DataSet oDs1 = new DataSet("FltPer_oDs1");
            DataSet oDs2 = new DataSet("Flt_oDs2");
            DataSet res_Revenue = new DataSet("Flt_res_Revenue");
            DataSet res_RevenueDetail = new DataSet("Flt_res_RevenueDetail");
            
                ReportsTracking rt=new ReportsTracking();
            try
            {
                lblStatus.Text = string.Empty;
                Session["dsExpFlightTonnage"] = null;
                FlightPerformanceRptViewer.Visible = false;
                FlightPerformanceRptViewer_Detail.Visible = false;
             
                //Validate controls
                if (Validate() == false)
                {
                    Session["dsExpFlightTonnage"] = null;
                    FlightPerformanceRptViewer_Detail.Visible = false;
                    FlightPerformanceRptViewer.Visible = false;
                    return;
                }
                res_Revenue.Dispose();

                #region Save Report History - code by Amit

                // taking all parameters as user selected in report in one variable - "Param"
                string Param = txtAgentCode.Text.ToString() + "," + ddlPaymentType.Text.ToString() + "," + txtControllingCode.Text.ToString() + "," + ddlControlingLocator.Text.ToString() + "," + ddlType.Text.ToString() + "," + ddlCode.Text.ToString() + "," + ddlAWBStatus.Text.ToString() + "," + txtFromdate.Text.ToString() + "," + txtTodate.Text.ToString();
 
                rt.ReportTrack("FlightWise Productivity Report", Session["UserName"].ToString(), Session["Station"].ToString(), Session["IpAddress"].ToString(), Param, Convert.ToDateTime(Session["IT"].ToString()));


                #endregion Save Report History


                FileInfo info;
                ////ReportRuntime runtime;
                ////ReportDefinition definition;
                DateTime dtTo = new DateTime();
                DateTime dtfrom = new DateTime();
                try
                {

                    string AgentCode = "All", PaymentType = "All", contrLocatorCode = "All", level = "All", levelCode = "All", frmDate = "", ToDt = "", AWBStatus = "All";
                    if (txtAgentCode.Text.Trim() != "")
                        AgentCode = txtAgentCode.Text.Trim();
                    if (ddlPaymentType.SelectedItem.Value.ToString() != "All(Without FOC)")
                        PaymentType = ddlPaymentType.SelectedItem.Value.ToString();
                    if (ddlPaymentType.SelectedItem.Value.ToString() == "All(Without FOC)")
                        PaymentType = "AllWFOC";
                    if (ddlControlingLocator.SelectedItem.Value.ToString() != "All")
                        contrLocatorCode = ddlControlingLocator.SelectedItem.Value.ToString();
                    //  contrLocatorCode=txtControllingCode.Text.tr 
                    if (ddlType.SelectedItem.Value.ToString() != "All")
                        level = ddlType.SelectedItem.Value.ToString();
                    if (ddlCode.SelectedItem.Value.ToString() != "All")
                        levelCode = ddlCode.SelectedItem.Value.ToString();
                    if (ddlAWBStatus.SelectedItem.Value.ToString() != "All")
                        AWBStatus = ddlAWBStatus.SelectedItem.Value.ToString();
                    if (txtFromdate.Text.Trim() != "")
                        frmDate = txtFromdate.Text.Trim();

                    if (txtTodate.Text.Trim() != "" && txtFromdate.Text.Trim() != "")
                    {
                        ToDt = txtTodate.Text.Trim();

                        string day = txtFromdate.Text.Substring(0, 2);
                        string mon = txtFromdate.Text.Substring(3, 2);
                        string yr = txtFromdate.Text.Substring(6, 4);
                        frmDate = yr + "-" + mon + "-" + day;
                        dtfrom = Convert.ToDateTime(frmDate);
                        frmDate = dtfrom.ToString();
                        string dayTo = txtTodate.Text.Substring(0, 2);
                        string monTo = txtTodate.Text.Substring(3, 2);
                        string yrTo = txtTodate.Text.Substring(6, 4);
                        ToDt = yrTo + "-" + monTo + "-" + dayTo;
                        dtTo = Convert.ToDateTime(ToDt);
                        ToDt = dtTo.ToString();
                    }

                    //AgentCode = "All"; PaymentType = "All"; contrLocatorCode = "All"; 
                    //level = "All"; levelCode = "All";

                    if (rbSummaryReport.Checked == true)
                        res_Revenue = objReport.GetFlightWiseAWBSummary(AgentCode, PaymentType, contrLocatorCode, level, levelCode, txtFromdate.Text.Trim(), txtTodate.Text.Trim(), AWBStatus);
                    else
                        res_Revenue = objReport.GetAWBManifestedAWBReport(AgentCode, PaymentType, contrLocatorCode, level, levelCode, txtFromdate.Text.Trim(), txtTodate.Text.Trim(), AWBStatus);
                    if (res_Revenue != null)
                    {
                        if (res_Revenue.Tables[0].Rows.Count > 0)
                        {
                            Session["dsExpFlightTonnage"] = res_Revenue;
                            Dataset2 = showSearchCriteria(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"), AWBStatus);
                            Dataset1 = res_Revenue;
                            lblStatus.Text = string.Empty;

                            #region Old RDLX
                            //if (rbSummaryReport.Checked == true)
                            //info = new FileInfo(Server.MapPath("/Reports/rptFlightTonnage.rdlx"));
                            //else
                            // info = new FileInfo(Server.MapPath("/Reports/rptDetailAWBFlightwise.rdlx"));

                            ////info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptFlightTonnage.rdlx");

                            //// info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "Report1.rdlx");
                            //if (rbSummaryReport.Checked == true)
                            //{
                            //    definition = new ReportDefinition(info);
                            //    runtime = new ReportRuntime(definition);
                            //    runtime.LocateDataSource += WARCustWise_LocateDataSource;
                            //    FlightPerformanceRptViewer.SetReport(runtime);

                            //}
                            //else
                            //{
                            //    definition = new ReportDefinition(info);
                            //    runtime = new ReportRuntime(definition);
                            //    runtime.LocateDataSource += WARCustWise_LocateDataSourceDetail;
                            //    FlightPerformanceRptViewer.SetReport(runtime);

                            //}

                            ////  oDs2 = LoadParamDataset();// just temperory used, not provide real value                 Dataset2 = oDs2;                  runtime.LocateDataSource += WARCustWise_LocateDataSource;                 WARCustWise.SetReport(runtime);                 pnlMsg.Visible = false;              }             else             {                 oDs1 = null;                 WARCustWise.Visible = false;                 // lblMsg.Text = "No Data Available";                 pnlMsg.Visible = true;                 lblMsgBox.Text = "No Data Available";                 //Pic.Visible = true;               }
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

                            if (res_Revenue.Tables[0].Columns.Contains("Logo") == false)
                            {
                                DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                                col1.DefaultValue = Logo.ToArray();
                                res_Revenue.Tables[0].Columns.Add(col1);
                            }

                            if (rbSummaryReport.Checked)
                            {
                                FlightPerformanceRptViewer.Visible = true;
                                FlightPerformanceRptViewer_Detail.Visible = false;
                                FlightPerformanceRptViewer.ProcessingMode = ProcessingMode.Local;
                                FlightPerformanceRptViewer.LocalReport.DataSources.Clear();
                                FlightPerformanceRptViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/FlightTonnageSummaryRpt.rdlc");
                                ReportDataSource datasource = new ReportDataSource("dsFlightTonnage_dtFlightTonnageSummary", res_Revenue.Tables[0]);
                                FlightPerformanceRptViewer.LocalReport.DataSources.Add(datasource);
                                FlightPerformanceRptViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                            }
                            else
                            {
                                FlightPerformanceRptViewer.Visible = false;
                                FlightPerformanceRptViewer_Detail.Visible = true;
                                FlightPerformanceRptViewer_Detail.ProcessingMode = ProcessingMode.Local;
                                FlightPerformanceRptViewer_Detail.LocalReport.DataSources.Clear();
                                FlightPerformanceRptViewer_Detail.LocalReport.ReportPath = Server.MapPath("~/Reports/FlightTonnageDetailRpt.rdlc");
                                ReportDataSource datasource = new ReportDataSource("dsFlightTonnage_dtFlightTonnageDetail", res_Revenue.Tables[0]);
                                FlightPerformanceRptViewer_Detail.LocalReport.DataSources.Add(datasource);
                                FlightPerformanceRptViewer_Detail.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler_Detail);
                            }
                            SaveUserActivityLog("");

                            #endregion
                        }
                        else
                        {
                            FlightPerformanceRptViewer.Visible = false;
                            FlightPerformanceRptViewer_Detail.Visible = false;
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data not avialable for given search criteria";
                            SaveUserActivityLog(lblStatus.Text);
                            Session["dsExpFlightTonnage"] = null;
                            txtFromdate.Focus();
                            return;


                        }
                    }
                    else
                    {
                        FlightPerformanceRptViewer.Visible = false;
                        FlightPerformanceRptViewer_Detail.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not avialable for given search criteria";
                        txtFromdate.Focus();
                        SaveUserActivityLog(lblStatus.Text);
                        Session["dsExpFlightTonnage"] = null;
                        return;

                    }

                }
                catch (Exception ex)
                {
                    SaveUserActivityLog(ex.Message);
                }
            }
            catch (Exception ex)
            {
                SaveUserActivityLog(ex.Message);
            }
            finally
            {
                if (oDs1 != null)
                { oDs1.Dispose(); }
                if (oDs2 != null)
                { oDs2.Dispose(); }
                if (res_Revenue != null)
                { res_Revenue.Dispose(); }
                if (res_RevenueDetail != null)
                { res_RevenueDetail.Dispose(); }
            }
        }
        #endregion

        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
                e.DataSources.Add(new ReportDataSource("dsFlightTonnage_dtFlightTonnageSub", Dataset2.Tables[0]));
        }
        public void ItemsSubreportProcessingEventHandler_Detail(object sender, SubreportProcessingEventArgs e)
        {
                e.DataSources.Add(new ReportDataSource("dsFlightTonnage_dtFlightTonnageDetailSub", Dataset2.Tables[0]));
        }

        #region To show subreport
        //private void WARCustWise_LocateDataSource(object sender, LocateDataSourceEventArgs e)
        //{

        //    string dname = e.DataSetName;
        //    //  if (dname.ToLower() == "DataSetValue") 
        //    string dsname = "";

        //    if (dname == "dsSectorWise")
        //        {
        //            e.Data = Dataset1;

        //        }
        //        else
        //        {
        //            e.Data = Dataset2;
        //        }
            

        //}
        #endregion
        
         #region To show subreport
        //private void WARCustWise_LocateDataSourceDetail(object sender, LocateDataSourceEventArgs e)
        //{

        //    string dname = e.DataSetName;
        //    //  if (dname.ToLower() == "DataSetValue") 
        //    string dsname = "";

           
        //    if (dname == "DataSet1")
        //        {
        //            e.Data = Dataset1;

        //        }
        //        else
        //        {
        //            e.Data = Dataset2;
        //        }
            

        //}
        #endregion

        #region dropdown list to select list
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (ddlType.SelectedItem.Value.ToString() == "All")
                {
                    ddlCode.DataSource = "";
                    ddlCode.DataBind();
                }

                if (ddlType.SelectedItem.Value.ToString() == "All")
                {

                    ddlCode.Items.Insert(0, "All");
                    ddlCode.SelectedIndex = 0;

                }
                if (ddlType.SelectedItem.Value.ToString() == "Airport")
                {
                    DataSet City = objBAL.GetCityCode();
                    ddlCode.DataSource = City.Tables[0];
                    ddlCode.DataTextField = "Airport";
                    ddlCode.DataValueField = "CityCode";
                    ddlCode.DataBind();
                    ddlCode.Items.Insert(0, "All");
                    ddlCode.SelectedIndex = 0;

                }


                if (ddlType.SelectedItem.Value.ToString() == "Airport")
                {
                    DataSet City = objBAL.GetCityCode();
                    ddlCode.DataSource = City.Tables[0];
                    ddlCode.DataTextField = "Airport";
                    ddlCode.DataValueField = "CityCode";
                    ddlCode.DataBind();
                    ddlCode.Items.Insert(0, "All");
                    ddlCode.SelectedIndex = 0;

                }


                if (ddlType.SelectedItem.Value.ToString() == "Region")
                {
                    DataSet Region = objBAL.GetRegionCode();
                    ddlCode.DataSource = Region.Tables[0];
                    ddlCode.DataTextField = "RegionCode";
                    ddlCode.DataValueField = "RegionCode";
                    ddlCode.DataBind();
                    ddlCode.Items.Insert(0, "All");
                    ddlCode.SelectedIndex = 0;


                }
                if (ddlType.SelectedItem.Value.ToString() == "City")
                {

                    DataSet City = objBAL.GetCityCode();
                    ddlCode.DataSource = City.Tables[0];
                    ddlCode.DataTextField = "CityCode";
                    ddlCode.DataValueField = "CityCode";
                    ddlCode.DataBind();
                    ddlCode.Items.Insert(0, "All");
                    ddlCode.SelectedIndex = 0;
                }
                if (ddlType.SelectedItem.Value.ToString() == "Country")
                {

                    DataSet Agent = objBAL.GetCountryCode();
                    ddlCode.DataSource = Agent.Tables[0];
                    ddlCode.DataTextField = "Country";
                    ddlCode.DataValueField = "CountryCode";

                    ddlCode.DataBind();
                    ddlCode.Items.Insert(0, "All");
                    ddlCode.SelectedIndex = 0;

                }

            }
            catch (Exception)
            {


            }

        }
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


                if (ddlType.SelectedItem.Value.ToString() != "All" && ddlCode.SelectedItem.Value.ToString() == "All")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please select level Type";
                    txtFromdate.Focus();
                    return false;
                }
                string strResult = string.Empty;
                try
                {
                    strResult = objReport.GetReportInterval(DateTime.ParseExact(txtFromdate.Text.Trim(), "dd/MM/yyyy", null), DateTime.ParseExact(txtTodate.Text.Trim(), "dd/MM/yyyy", null));
                }
                catch
                {
                    strResult = "";
                }
                if (strResult != "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = strResult;
                    txtFromdate.Focus();
                    return false;
                }

            }
            catch (Exception ex)
            {


            }
            return true;
        }
        #endregion

        #region Fill Session For DDL
        private void FillSession()
        {  DataSet Region=new DataSet("flt_region");
            DataSet City=new DataSet("flt_city");
            DataSet Agent = new DataSet("flt_agent");
            try
            {

                 Region = objBAL.GetRegionCode();
                Session["Region"] = Region.Tables[0];
                City = objBAL.GetCityCode();
                Session["City"] = City.Tables[0];
                Agent = objBAL.GetAgentCode();
                Session["Agent"] = Agent.Tables[0];

            }
            catch (Exception)
            {


            }
            finally 
            {
                if (Region != null)
                { Region = null; }
                if (Agent != null)
                { Agent = null; }
                if (City != null)
                { City = null; }
            }
        }
        #endregion

        #region Fill Session For DDL
        private void FillPaymentType()
        {
            DataSet PaymentCode = new DataSet("flt_PaymentCode");
            try
            {

                PaymentCode = objReport.GetPaymentCode();
                ddlPaymentType.DataSource = PaymentCode.Tables[0];
                ddlPaymentType.DataTextField = "PayMode";
                ddlPaymentType.DataValueField = "PayMode";

                ddlPaymentType.DataBind();
                ddlPaymentType.Items.Insert(0, "All");
                ddlPaymentType.SelectedIndex = 0;



            }
            catch (Exception)
            {


            }
            finally
            {
                if (PaymentCode != null)
                { PaymentCode = null; }
            }
        }
        #endregion

        #region SearchCriteria
        private DataSet showSearchCriteria(string Agent, string PaymentType, string ControlLoc, string level, string levelCode, string FromDate, string Todate, string AWBStatus)
        {
            DataSet ds = new DataSet("Flt_ds");
            DataTable dtSearch = new DataTable("Flt_dtSearch");
            DataColumn dcNew= new DataColumn();
            try
            {


                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "AgentCode";
                dtSearch.Columns.Add(dcNew);

                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "PaymentType";
                dtSearch.Columns.Add(dcNew);

                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "ControlingLoc";
                dtSearch.Columns.Add(dcNew);

                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "level";
                dtSearch.Columns.Add(dcNew);


                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "levelCode";
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
                dcNew.ColumnName = "AWBStatus";
                dtSearch.Columns.Add(dcNew);


                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "ReportDate";
                dtSearch.Columns.Add(dcNew);

                DataRow dr;
                dr = dtSearch.NewRow();

                dr["AgentCode"] = Agent; //"5";
                dr["PaymentType"] = PaymentType;// "5";
                dr["ControlingLoc"] = ControlLoc;
                dr["level"] = level;
                dr["levelCode"] = levelCode;
                dr["FromDate"] = FromDate;
                dr["ToDate"] = Todate;// "9";
                dr["AWBStatus"] = AWBStatus;
                dr["ReportDate"] = CurrTime;
                dtSearch.Rows.Add(dr);


                ds.Tables.Add(dtSearch);
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
            finally
            {
                if (dtSearch != null)
                {
                    dtSearch = null;
                }
                if (dcNew != null)
                {
                    dcNew = null;
                }
            
            }

        }
        #endregion

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = null;
            DataTable dt = null;
            Session["dsExpFlightTonnage"] = null;
            if (Validate() == false)
            {
                FlightPerformanceRptViewer.Visible = false;
                FlightPerformanceRptViewer_Detail.Visible = false;
                Session["dsExpFlightTonnage"] = null;
                return;
            }
            try
            {
                if ((DataSet)Session["dsExpFlightTonnage"] == null)
                    GetData();

                dsExp = (DataSet)Session["dsExpFlightTonnage"];
                if(dsExp!=null && dsExp.Tables[0].Rows.Count>0)
                dt = (DataTable)dsExp.Tables[0];

                else
                {
                    lblStatus.Text = "No records found";
                    lblStatus.ForeColor = Color.Red;
                    SaveUserActivityLog(lblStatus.Text);
                    FlightPerformanceRptViewer.Visible = false;
                    FlightPerformanceRptViewer_Detail.Visible = false;
                    return;
                }

                if (dt.Columns.Contains("Logo"))
                    dt.Columns.Remove("Logo");
                string attachment = "attachment; filename=Flight Performance Report.xls";
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

        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            try
            {
                // taking all parameters as user selected in report in one variable - "Param"
                string Param = "AgtCode:" + txtAgentCode.Text.ToString() + ", PayMode:" + ddlPaymentType.Text.ToString() + ",CntlName:" + ddlControlingLocator.Text.ToString() + ", Level:" + ddlType.Text.ToString() + ", Loc:" + ddlCode.Text.ToString() + ",FrmDt:" + txtFromdate.Text.ToString() + ", ToDt:" + txtTodate.Text.ToString();
                if (rbDetailedReport.Checked)
                    Param = Param + "DetailedReport";
                if (rbSummaryReport.Checked)
                    Param = Param + "SummaryReport";
                objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "Flight Performance", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
            }
            catch (Exception ex)
            { }
            finally
            {
                if (objBAL != null)
                { objBAL = null; }
            }
        }
        public string GetReportInterval(DateTime FromDate, DateTime ToDate)
        {
            double DaysConfigured = 0;
            double intReportInterval = (ToDate - FromDate).TotalDays + 1;
            LoginBL objBL = new LoginBL();

            try
            {
                Session["dsExpFlightTonnage"] = null;
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
                FlightPerformanceRptViewer.Visible = false;
                FlightPerformanceRptViewer_Detail.Visible = false;
                return "Report can be generated only for " + DaysConfigured.ToString() + " days.";
            }
            else
                return "";
        }

        protected void GetData()
        {
            DataSet oDs1 = new DataSet("fltPerformance_oDs1");
            DataSet oDs2 = new DataSet("fltPerformance_oDs2");
            DataSet res_Revenue1 = new DataSet("FLT_res_Revenue1");
            DateTime dtTo = new DateTime(); 
            DateTime dtfrom = new DateTime();
            ReportsTracking rt = new ReportsTracking();

            try
            {
                lblStatus.Text = string.Empty;
                Session["dsExpFlightTonnage"] = null;
                FlightPerformanceRptViewer.Visible = false;
                FlightPerformanceRptViewer_Detail.Visible = false;

                //Validate controls
                if (Validate() == false)
                {
                    FlightPerformanceRptViewer.Visible = false;
                    FlightPerformanceRptViewer_Detail.Visible = false;
                    Session["dsExpFlightTonnage"] = null;
                    return;
                }



                #region Save Report History - code by Amit

                // taking all parameters as user selected in report in one variable - "Param"
                string Param = txtAgentCode.Text.ToString() + "," + ddlPaymentType.Text.ToString() + "," + txtControllingCode.Text.ToString() + "," + ddlControlingLocator.Text.ToString() + "," + ddlType.Text.ToString() + "," + ddlCode.Text.ToString() + "," + ddlAWBStatus.Text.ToString() + "," + txtFromdate.Text.ToString() + "," + txtTodate.Text.ToString();


                rt.ReportTrack("FlightWise Productivity Report", Session["UserName"].ToString(), Session["Station"].ToString(), Session["IpAddress"].ToString(), Param, Convert.ToDateTime(Session["IT"].ToString()));


                #endregion Save Report History

                try
                {

                    string AgentCode = "All", PaymentType = "All", contrLocatorCode = "All", level = "All", levelCode = "All", frmDate = "", ToDt = "", AWBStatus = "All";
                    if (txtAgentCode.Text.Trim() != "")
                        AgentCode = txtAgentCode.Text.Trim();
                    if (ddlPaymentType.SelectedItem.Value.ToString() != "All(Without FOC)")
                        PaymentType = ddlPaymentType.SelectedItem.Value.ToString();
                    if (ddlPaymentType.SelectedItem.Value.ToString() == "All(Without FOC)")
                        PaymentType = "AllWFOC";
                    if (ddlControlingLocator.SelectedItem.Value.ToString() != "All")
                        contrLocatorCode = ddlControlingLocator.SelectedItem.Value.ToString();
                    //  contrLocatorCode=txtControllingCode.Text.tr 
                    if (ddlType.SelectedItem.Value.ToString() != "All")
                        level = ddlType.SelectedItem.Value.ToString();
                    if (ddlCode.SelectedItem.Value.ToString() != "All")
                        levelCode = ddlCode.SelectedItem.Value.ToString();
                    if (ddlAWBStatus.SelectedItem.Value.ToString() != "All")
                        AWBStatus = ddlAWBStatus.SelectedItem.Value.ToString();
                    if (txtFromdate.Text.Trim() != "")
                        frmDate = txtFromdate.Text.Trim();

                    if (txtTodate.Text.Trim() != "" && txtFromdate.Text.Trim() != "")
                    {
                        ToDt = txtTodate.Text.Trim();

                        string day = txtFromdate.Text.Substring(0, 2);
                        string mon = txtFromdate.Text.Substring(3, 2);
                        string yr = txtFromdate.Text.Substring(6, 4);
                        frmDate = yr + "-" + mon + "-" + day;
                        dtfrom = Convert.ToDateTime(frmDate);
                        frmDate = dtfrom.ToString();
                        string dayTo = txtTodate.Text.Substring(0, 2);
                        string monTo = txtTodate.Text.Substring(3, 2);
                        string yrTo = txtTodate.Text.Substring(6, 4);
                        ToDt = yrTo + "-" + monTo + "-" + dayTo;
                        dtTo = Convert.ToDateTime(ToDt);
                        ToDt = dtTo.ToString();
                    }

                    //AgentCode = "All"; PaymentType = "All"; contrLocatorCode = "All"; 
                    //level = "All"; levelCode = "All";

                    if (rbSummaryReport.Checked == true)
                        res_Revenue1 = objReport.GetFlightWiseAWBSummary(AgentCode, PaymentType, contrLocatorCode, level, levelCode, txtFromdate.Text.Trim(), txtTodate.Text.Trim(), AWBStatus);
                    else
                        res_Revenue1 = objReport.GetAWBManifestedAWBReport(AgentCode, PaymentType, contrLocatorCode, level, levelCode, txtFromdate.Text.Trim(), txtTodate.Text.Trim(), AWBStatus);

                    if (res_Revenue1 != null)
                    {
                        if (res_Revenue1.Tables[0].Rows.Count > 0)
                        {
                            Session["dsExpFlightTonnage"] = res_Revenue1;
                            SaveUserActivityLog("");
                        }
                        else
                        {
                            FlightPerformanceRptViewer.Visible = false;
                            FlightPerformanceRptViewer_Detail.Visible = false;
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data not avialable for given search criteria";
                            Session["dsExpFlightTonnage"] = null;
                            SaveUserActivityLog(lblStatus.Text);
                            txtFromdate.Focus();
                            return;
                        }
                    }

                    else
                    {
                        FlightPerformanceRptViewer.Visible = false;
                        FlightPerformanceRptViewer_Detail.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not avialable for given search criteria";
                        txtFromdate.Focus();
                        Session["dsExpFlightTonnage"] = null;
                        SaveUserActivityLog(lblStatus.Text);
                        return;

                    }

                }
                catch (Exception ex)
                {
                    SaveUserActivityLog(ex.Message);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (res_Revenue1 != null)

                {
                    res_Revenue1.Dispose();
                }
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlPaymentType.SelectedIndex = 0;
            ddlCode.SelectedIndex = 0;
            ddlType.SelectedIndex = 0;
            ddlControlingLocator.SelectedIndex = 0;
            txtFromdate.Text = string.Empty;
            txtTodate.Text = string.Empty;
            txtAgentCode.Text = string.Empty;
            txtFromdate.Focus();
            lblStatus.Text = string.Empty;
            FlightPerformanceRptViewer_Detail.Visible = false;
            FlightPerformanceRptViewer.Visible = false;
            
        }
    }
}
