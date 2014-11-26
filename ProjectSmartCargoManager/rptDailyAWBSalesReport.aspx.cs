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
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
//using DataDynamics.Reports.Rendering.Excel.ExcelTemplateGenerator;
//using DataDynamics.Reports.Rendering.Excel;
//using DataDynamics.Reports.Rendering.IO;
//using DataDynamics.Reports.Extensibility.Rendering.IO;
using Microsoft.Reporting.WebForms;


namespace ProjectSmartCargoManager
{
    public partial class rptDailyAWBSalesReport : System.Web.UI.Page
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
                   string AgentCode = Convert.ToString(Session["ACode"]);

                    int RoleId = Convert.ToInt32(Session["RoleID"]);

                    DateTime dtCurr = Convert.ToDateTime(Session["IT"].ToString());
                    CurrTime = dtCurr.ToString("dd/MM/yyyy hh:mm:ss tt");
                    ReportViewer1.Visible = false;

                    //RptViewerRevenue_Station.Visible = false;
                    //FillPaymentType();
                    txtFromdate.Text = dtCurr.ToString("dd/MM/yyyy");
                    txtTodate.Text = dtCurr.ToString("dd/MM/yyyy");                    

                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion


        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
              
                System.IO.MemoryStream Logo = null;
                //Validate controls
                if (ValidateInput() == false)
                {
                    ReportViewer1.Visible = false;
                    return;
                   
                }

                lblStatus.Text = "";
                DataSet oDs1 = new DataSet();
                DataSet oDs2 = new DataSet();
                FileInfo info;
                ////ReportRuntime runtime;
                ////ReportDefinition definition;
                DataSet res_Revenue = new DataSet();
                string ErrorLog = string.Empty;
                DataTable dt = new DataTable();
                
                try
                {   
                   
                    string frmDate = "", ToDt = "";
                    
                    if (txtFromdate.Text.Trim() != "")
                        frmDate = txtFromdate.Text.Trim();

                    DateTime dtTo=new DateTime(); DateTime dtfrom=new DateTime();

                    dtfrom = DateTime.ParseExact(txtFromdate.Text.Trim(), "dd/MM/yyyy", null);
                    dtTo = DateTime.ParseExact(txtTodate.Text.Trim(), "dd/MM/yyyy", null);

                    res_Revenue = objReport.GetDailyAWBSalesReport(dtfrom, dtTo);

                    Session["res_Revenue"] = res_Revenue;

                    if (res_Revenue != null)
                    {
                        if (res_Revenue.Tables[0].Rows.Count > 0)
                        {
                            Dataset1 = res_Revenue;
                            
                            //RptViewerRevenue_Station.Visible = true;

                            //info = new FileInfo(Server.MapPath("/Reports/rptDailyAWBSalesReport.rdlx"));
                          
                            //definition = new ReportDefinition(info);
                            //runtime = new ReportRuntime(definition);
                            //runtime.LocateDataSource += WARCustWise_LocateDataSource;
                            //RptViewerRevenue_Station.SetReport(runtime);
                            ////  oDs2 = LoadParamDataset();// just temperory used, not provide real value                 Dataset2 = oDs2;                  runtime.LocateDataSource += WARCustWise_LocateDataSource;                 WARCustWise.SetReport(runtime);                 pnlMsg.Visible = false;              }             else             {                 oDs1 = null;                 WARCustWise.Visible = false;                 // lblMsg.Text = "No Data Available";                 pnlMsg.Visible = true;                 lblMsgBox.Text = "No Data Available";                 //Pic.Visible = true;               }
                            btnExportReport.Enabled = true;

                            //Logo
                            try
                            {
                                Logo = CommonUtility.GetImageStream(Page.Server);
                                //System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                            }
                            catch (Exception ex)
                            {
                                Logo = new System.IO.MemoryStream();
                            }
                            
                            if (res_Revenue.Tables[0].Rows.Count > 0)
                            {

                                dt = res_Revenue.Tables[0].Copy();
                                if (dt.Columns.Contains("Logo") == false)
                                {
                                    DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                                    col1.DefaultValue = Logo.ToArray();
                                    dt.Columns.Add(col1);
                                }
                            }
                            ReportViewer1.Visible = true;
                            ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptDailyAWBSalesReport.rdlc");
                            //Customers dsCustomers = GetData("select top 20 * from customers");
                            ReportDataSource datasource = new ReportDataSource("dsDailyAWBSalesReport_dtDailyAWBSales", dt);
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(datasource);
                            //ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandlerForSUBrpt);
                            btnExportReport.Enabled = true;
                            SaveUserActivityLog("");
                        }
                        else
                        {
                            ReportViewer1.Visible = false;
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data not available for given search criteria";
                            txtFromdate.Focus();
                           // btnExportReport.Enabled = false;
                            return;
                        }
                    }
                    else
                    {
                        ReportViewer1.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not available for given search criteria";
                        txtFromdate.Focus();
                       // btnExportReport.Enabled = false;
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
                    if (dt != null)
                    {
                        dt.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        #region To show subreport
        //private void WARCustWise_LocateDataSource(object sender, LocateDataSourceEventArgs e)
        //{
        //    string dname = e.DataSetName;
        //    //  if (dname.ToLower() == "DataSetValue")    
        //    if (dname == "DataSet1")
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
        private bool ValidateInput()
        {
            try
            {
                try
                {
                    if (txtFromdate.Text.Trim() == "")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid From date";
                        txtFromdate.Focus();
                        return false;
                    }

                    if (txtTodate.Text.Trim() == "")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid To date";
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
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = ex.Message;
                txtFromdate.Focus();
                return false;
            }

        }
        #endregion

        #region Fill Session For DDL
        private void FillSession()
        {
            DataSet City = null;
            DataSet Region = null;
            DataSet Agent = null;
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
        
        #region SearchCriteria
        private DataSet showSearchCriteria(string Agent, string PaymentType, string ControlLoc, string level, string levelCode, string FromDate, string Todate,string AWBStatus)
        {
            DataSet ds = new DataSet();
            try
            {
                DataTable dtSearch = new DataTable();
                DataColumn dcNew;

                dcNew = new DataColumn();
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
                if (ds != null)
                {
                    ds.Dispose();
                }
            }

        }
        #endregion

        #region Button Export
        
        protected void btnExportReport_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            DataTable dt = null;

            try
            {
               // if ((DataSet)Session["res_Revenue"] == null)
                    GetData();

                ds = (DataSet)Session["res_Revenue"];
                dt = (DataTable)ds.Tables[0];
                //dt = city.GetAllCity();//your datatable 
                string attachment = "attachment; filename=AWBSales.xls";
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
                ds = null;
                dt = null;
            }
        }
        #endregion

        public void ItemsSubreportProcessingEventHandlerForSUBrpt(object sender, SubreportProcessingEventArgs e)
        {
            DataTable dtAWBDetails = null;
            try
            {
            dtAWBDetails = Dataset2.Tables[0];//(DataTable)Session["ManifestGridData"];
            e.DataSources.Add(new ReportDataSource("dsDailyAWBSalesReport_dtDailyAWBSalesSub", dtAWBDetails));
            }
            catch (Exception ex)
            { }
            finally
            {
                if (dtAWBDetails != null)
                {
                    dtAWBDetails.Dispose();
                }
            }

        }

        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            // taking all parameters as user selected in report in one variable - "Param"
            string Param = "FrmDt:" + txtFromdate.Text.ToString() + ", ToDt:" + txtTodate.Text.ToString();

            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "Daily AWB Sales", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
        }

        protected void GetData()
        {
            try
            {
                System.IO.MemoryStream Logo = null;
                //Validate controls
                if (ValidateInput() == false)
                {
                    ReportViewer1.Visible = false;
                    Session["res_Revenue"] = null;
                    return;
                }

                lblStatus.Text = "";
                DataSet oDs1 = new DataSet();
                DataSet oDs2 = new DataSet();
                FileInfo info;
                ////ReportRuntime runtime;
                ////ReportDefinition definition;
                DataSet res_Revenue = new DataSet();
                string ErrorLog = string.Empty;

                try
                {

                    string frmDate = "", ToDt = "";

                    if (txtFromdate.Text.Trim() != "")
                        frmDate = txtFromdate.Text.Trim();

                    DateTime dtTo = new DateTime(); DateTime dtfrom = new DateTime();

                    dtfrom = DateTime.ParseExact(txtFromdate.Text.Trim(), "dd/MM/yyyy", null);
                    dtTo = DateTime.ParseExact(txtTodate.Text.Trim(), "dd/MM/yyyy", null);

                    res_Revenue = objReport.GetDailyAWBSalesReport(dtfrom, dtTo);

                    Session["res_Revenue"] = res_Revenue;

                    if (res_Revenue != null)
                    {
                        if (res_Revenue.Tables[0].Rows.Count > 0)
                        {
                            Dataset1 = res_Revenue;

                            //RptViewerRevenue_Station.Visible = true;

                            //info = new FileInfo(Server.MapPath("/Reports/rptDailyAWBSalesReport.rdlx"));

                            //definition = new ReportDefinition(info);
                            //runtime = new ReportRuntime(definition);
                            //runtime.LocateDataSource += WARCustWise_LocateDataSource;
                            //RptViewerRevenue_Station.SetReport(runtime);
                            ////  oDs2 = LoadParamDataset();// just temperory used, not provide real value                 Dataset2 = oDs2;                  runtime.LocateDataSource += WARCustWise_LocateDataSource;                 WARCustWise.SetReport(runtime);                 pnlMsg.Visible = false;              }             else             {                 oDs1 = null;                 WARCustWise.Visible = false;                 // lblMsg.Text = "No Data Available";                 pnlMsg.Visible = true;                 lblMsgBox.Text = "No Data Available";                 //Pic.Visible = true;               }
                            btnExportReport.Enabled = true;

                            //Logo
                            try
                            {
                                Logo = CommonUtility.GetImageStream(Page.Server);
                                //System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                            }
                            catch (Exception ex)
                            {
                                Logo = new System.IO.MemoryStream();
                            }
                            DataTable dt = new DataTable();
                            if (res_Revenue.Tables[0].Rows.Count > 0)
                            {

                                dt = res_Revenue.Tables[0].Copy();
                                if (dt.Columns.Contains("Logo") == false)
                                {
                                    DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                                    col1.DefaultValue = Logo.ToArray();
                                    dt.Columns.Add(col1);
                                }
                            }

                            //ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            //ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptDailyAWBSalesReport.rdlc");
                            ////Customers dsCustomers = GetData("select top 20 * from customers");
                            //ReportDataSource datasource = new ReportDataSource("dsDailyAWBSalesReport_dtDailyAWBSales", dt);
                            //ReportViewer1.LocalReport.DataSources.Clear();
                            //ReportViewer1.LocalReport.DataSources.Add(datasource);
                            ////ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandlerForSUBrpt);
                            //btnExportReport.Enabled = true;
                            //SaveUserActivityLog("");
                        }
                        else
                        {
                            ReportViewer1.Visible = false;
                            lblStatus.ForeColor = Color.Red;
                            Session["res_Revenue"] = null;
                            lblStatus.Text = "Data not available for given search criteria";
                            txtFromdate.Focus();
                            //btnExportReport.Enabled = false;
                            return;
                        }
                    }
                    else
                    {
                        ReportViewer1.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not available for given search criteria";
                        txtFromdate.Focus();
                        //btnExportReport.Enabled = false;
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

      
    }
        
}
