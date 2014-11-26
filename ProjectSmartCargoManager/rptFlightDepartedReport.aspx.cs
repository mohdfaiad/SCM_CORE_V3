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
using Microsoft.Reporting.WebForms;


namespace ProjectSmartCargoManager
{
    public partial class rptFlightDepartedReport : System.Web.UI.Page
    {
        #region Variable
        private DataSet Dataset1=new DataSet("rptFltDep_dsdataset1");
        private DataSet Dataset2 = new DataSet("rptFltDep_dsdataset2");
        StockAllocationBAL objBAL = new StockAllocationBAL();

        ReportBAL objReport = new ReportBAL();
        ReportBAL rptBAl = new ReportBAL();
        public static string CurrTime = "";
        #endregion

        #region On page load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string AgentCode = Convert.ToString(Session["ACode"]);

                    int RoleId = Convert.ToInt32(Session["RoleID"]);

                    DateTime dtCurr = Convert.ToDateTime(Session["IT"].ToString());
                    CurrTime = dtCurr.ToString("dd/MM/yyyy hh:mm:ss tt");

                    ////RptViewerFlightDeparted.Visible = false;

                    txtFromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtTodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    btnExport.Visible = true;

                    ReportViewer1.Visible = false;

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
            try
            {
                //Validate controls
                if (Validate() == false)
                {
                    ReportViewer1.Visible = false;
                    return;
                }
                 lblStatus.Text = "";

                 DataSet oDs1 = new DataSet("rptFltDep_dsOds1");
                 DataSet oDs2 = new DataSet("rptFltDep_dsOds2");
                FileInfo info;
                ////ReportRuntime runtime;
                ////ReportDefinition definition;
                DataSet res_Revenue = new DataSet("rptFltDep_dsFltManifst");
                string ErrorLog = string.Empty;

                try
                {

                    string frmDate = "", ToDt = "", FlightStatus = "All";


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


                    res_Revenue = objReport.GetFlightManifestedDetails(dtfrom, dtTo, FlightStatus);

                  
                    if (res_Revenue != null)
                    {
                        if (res_Revenue.Tables[0].Rows.Count > 0)
                        {
                            Dataset1 = res_Revenue;
                            // Dataset2 = showSearchCriteria( dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"), FlightStatus);

                            #region old report
                            //RptViewerFlightDeparted.Visible = true;



                            //info = new FileInfo(Server.MapPath("/Reports/rptFlightManifestDetails.rdlx"));
                            //// info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptBGControlReport.rdlx");

                            //// info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptBGControlReport.rdlx");

                            //definition = new ReportDefinition(info);
                            //runtime = new ReportRuntime(definition);
                            //runtime.LocateDataSource += WARCustWise_LocateDataSource;
                            //RptViewerFlightDeparted.SetReport(runtime);
                            #endregion
                            Session["dsExp"] = res_Revenue;
                            ReportViewer1.Visible = true;
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

                            if (res_Revenue.Tables[0].Columns.Contains("FromDate") == false)
                            {
                                DataColumn col1 = new DataColumn("FromDate", typeof(string));
                                col1.DefaultValue = txtFromdate.Text.Trim();
                                res_Revenue.Tables[0].Columns.Add(col1);
                            }
                            if (res_Revenue.Tables[0].Columns.Contains("ToDate") == false)
                            {
                                DataColumn col1 = new DataColumn("ToDate", typeof(string));
                                col1.DefaultValue = txtTodate.Text.Trim();
                                res_Revenue.Tables[0].Columns.Add(col1);
                            }


                            ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptFlightDepartedReport.rdlc");
                            ReportDataSource datasource = new ReportDataSource("dsrptFlightDepartedReport_dtrptFlightDepartedReport", res_Revenue.Tables[0]);
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(datasource);
                            ReportViewer1.Visible = true;
                            btnExport.Visible = true;
                            SaveUserActivityLog("");

                        }
                        else
                        {
                            
                            ReportViewer1.Visible = false;
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data not available for given search criteria";
                            txtFromdate.Focus();
                            return;


                        }
                    }
                    else
                    {
                        Session["dsExp"] = null;
                        ReportViewer1.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not available for given search criteria";
                        txtFromdate.Focus();
                        return;

                    }

                }
                catch (Exception ex)
                {
                    ErrorLog = ex.Message;
                    SaveUserActivityLog(ErrorLog);
                }
                finally
                {
                    if (res_Revenue != null)
                    {
                        res_Revenue.Dispose();
                    }
                    if (Dataset2 != null)
                    {
                        Dataset2.Dispose();
                    }
                    if (Dataset1 != null)
                    {
                        Dataset1.Dispose();
                    }
                    
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            // taking all parameters as user selected in report in one variable - "Param"
            string Param = "Valid From:" + txtFromdate.Text.ToString() + ",Valid To:" + txtTodate.Text.ToString();
            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "Flight Manifested", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
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
                return "Report can be generated only for " + DaysConfigured.ToString() + " days.";
            else
                return "";
        }
        #region To show subreport
        //private void WARCustWise_LocateDataSource(object sender, LocateDataSourceEventArgs e)
        //{
        //    string dname = e.DataSetName;
        //    //  if (dname.ToLower() == "DataSetValue")    
        //    if (dname == "dsDepart")
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
                    if (txtFromdate.Text.Trim() == "" || txtTodate.Text.Trim() == "")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter dates for Search";
                        txtFromdate.Focus();
                        return false;
                    }

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
        private DataSet showSearchCriteria(string FromDate, string Todate, string FlightStatus)
        {
            DataSet ds = new DataSet("rptFltDep_dsSearchCriteria");
            DataTable dtSearch = new DataTable("rptFltDep_dtSearchCriteria");
            try
            {
                DataColumn dcNew;

                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "FrmDate";
                dtSearch.Columns.Add(dcNew);

                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "Todate";
                dtSearch.Columns.Add(dcNew);


                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "status";
                dtSearch.Columns.Add(dcNew);

                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "ReportTime";
                dtSearch.Columns.Add(dcNew);



                DataRow dr;
                dr = dtSearch.NewRow();

                dr["FrmDate"] = FromDate;
                dr["Todate"] = Todate;// "9";

                dr["status"] = FlightStatus;
                dr["ReportTime"] = CurrTime;
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
                if (ds != null)
                    ds.Dispose();
               
            }

        }
        #endregion

        #region Fill Session For DDL
        private void FillSession()
        {
            DataSet City = new DataSet("rptFltDep_dsSessionCity");
            DataSet Region = new DataSet("rptFltDep_dsSessionRegion");
            DataSet Agent = new DataSet("rptFltDep_dsSessionAgent");
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
                if (City != null)
                {
                    City.Dispose();
                }
                if (Agent != null)
                {
                    Agent.Dispose();
                }
                if (Region != null)
                {
                    Region.Dispose();
                }
            }
        }
        #endregion

        public void Getdata()
        {

            try
            {
                //Validate controls
                if (Validate() == false)
                {
                    Session["dsExp"] = null;
                    ReportViewer1.Visible = false;
                    return;

                } lblStatus.Text = "";


                DataSet oDs1 = new DataSet("rptFltDep_dsOds1getData");
                DataSet oDs2 = new DataSet("rptFltDep_dsOds2getData");
                FileInfo info;
                ////ReportRuntime runtime;
                ////ReportDefinition definition;
                DataSet res_Revenue = new DataSet("rptFltDep_dsgetDataResReven");
                string ErrorLog = string.Empty;

                try
                {

                    string frmDate = "", ToDt = "", FlightStatus = "All";


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


                    res_Revenue = objReport.GetFlightManifestedDetails(dtfrom, dtTo, FlightStatus);

                    Session["dsExp"] = res_Revenue;
                    if (res_Revenue != null)
                    {
                        if (res_Revenue.Tables[0].Rows.Count > 0)
                        {
                            Dataset1 = res_Revenue;
                            // Dataset2 = showSearchCriteria( dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"), FlightStatus);

                            #region old report
                            //RptViewerFlightDeparted.Visible = true;



                            //info = new FileInfo(Server.MapPath("/Reports/rptFlightManifestDetails.rdlx"));
                            //// info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptBGControlReport.rdlx");

                            //// info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptBGControlReport.rdlx");

                            //definition = new ReportDefinition(info);
                            //runtime = new ReportRuntime(definition);
                            //runtime.LocateDataSource += WARCustWise_LocateDataSource;
                            //RptViewerFlightDeparted.SetReport(runtime);
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

                            //if (res_Revenue.Tables[0].Columns.Contains("Logo") == false)
                            //{
                            //    DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                            //    col1.DefaultValue = Logo.ToArray();
                            //    res_Revenue.Tables[0].Columns.Add(col1);
                            //}

                            //if (res_Revenue.Tables[0].Columns.Contains("FromDate") == false)
                            //{
                            //    DataColumn col1 = new DataColumn("FromDate", typeof(string));
                            //    col1.DefaultValue = txtFromdate.Text.Trim();
                            //    res_Revenue.Tables[0].Columns.Add(col1);
                            //}
                            //if (res_Revenue.Tables[0].Columns.Contains("ToDate") == false)
                            //{
                            //    DataColumn col1 = new DataColumn("ToDate", typeof(string));
                            //    col1.DefaultValue = txtTodate.Text.Trim();
                            //    res_Revenue.Tables[0].Columns.Add(col1);
                            //}


                            //ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            //ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptFlightDepartedReport.rdlc");
                            //ReportDataSource datasource = new ReportDataSource("dsrptFlightDepartedReport_dtrptFlightDepartedReport", res_Revenue.Tables[0]);
                            //ReportViewer1.LocalReport.DataSources.Clear();
                            //ReportViewer1.LocalReport.DataSources.Add(datasource);

                            btnExport.Visible = true;

                        }
                        else
                        {
                            ReportViewer1.Visible = false;
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data not avialable for given search criteria";
                            txtFromdate.Focus();
                            return;


                        }
                    }
                    else
                    {
                        ReportViewer1.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not avialable for given search criteria";
                        txtFromdate.Focus();
                        return;

                    }

                }
                catch (Exception ex)
                {
                    ErrorLog = ex.Message;
                    SaveUserActivityLog(ErrorLog);
                }
                finally
                {
                    if (res_Revenue != null)
                    {
                        res_Revenue.Dispose();
                    }
                    if (Dataset2 != null)
                    {
                        Dataset2.Dispose();
                    }
                    if (Dataset1 != null)
                    {
                        Dataset1.Dispose();
                    }

                }
            }
            catch (Exception ex)
            {

            }

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = null;
            DataTable dt = null;
          
            try
            {              
                    Getdata();
                
              dsExp = (DataSet)Session["dsExp"];
              if (dsExp != null && dsExp.Tables[0].Rows.Count > 0)
              {
                  lblStatus.Visible = false;
                  dt = (DataTable)dsExp.Tables[0];

              }
              else
                  return;
               
                dt = (DataTable)dsExp.Tables[0];
                //dt = city.GetAllCity();//your datatable 
                string attachment = "attachment; filename=Flight Manifested Report.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                string tab = "";
                lblStatus.Text = string.Empty;
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

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtFromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtTodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            ReportViewer1.Visible = false;  
        }

    }
}
