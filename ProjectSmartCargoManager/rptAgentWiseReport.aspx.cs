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
    public partial class rptAgentWiseReport : System.Web.UI.Page
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
                    ReportViewer1.Visible = false;
                    //FillPaymentType();
                    txtFromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtTodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    DateTime dtCurr = Convert.ToDateTime(Session["IT"].ToString());
                    CurrTime = dtCurr.ToString("dd/MM/yyyy hh:mm:ss tt");

                    //DataSet City = objBAL.GetCityCode();
                    //ddlCode.DataSource = City.Tables[0];
                    //ddlCode.DataTextField = "";
                    //ddlCode.DataValueField = "";
                    ddlCode.DataSource = null;
                    ddlCode.DataBind();
                    ddlCode.Items.Insert(0, "All");
                    ddlCode.SelectedIndex = 0;

                    string AgentCode = Convert.ToString(Session["ACode"]);

                    if (AgentCode != "")
                    {
                        txtAgentCode.Text = AgentCode;
                        txtAgentCode.ReadOnly = true;
                    }
                    //if (City != null)
                    //{
                    //    City.Dispose();
                    //}
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
                System.IO.MemoryStream Logo = null;
                //Validate controls
                if (Validate() == false)
                {
                    ReportViewer1.Visible = false;
                    return;

                } lblStatus.Text = "";


                DataSet oDs1 = new DataSet("Agent_oDs1");
                DataSet oDs2 = new DataSet("Agent_oDs2");
                FileInfo info;
                ////ReportRuntime runtime;
                ////ReportDefinition definition;
                string ErrorLog = string.Empty;
                DataSet res_Revenue = new DataSet("Agent_res_Revenue");
                DataTable dt = new DataTable("Agent_dt");

                try
                {

                    string AgentCode = "All", PaymentType = "All", contrLocatorCode = "All", level = "All", levelCode = "All", frmDate = "", ToDt = "", AWBStatus = "All";
                    if (txtAgentCode.Text.Trim() != "")
                        AgentCode = txtAgentCode.Text.Trim();
                    if (ddlPaymentType.SelectedItem.Value.ToString() != "All(Without FOC)")
                        PaymentType = ddlPaymentType.SelectedItem.Value.ToString();
                    if (ddlControlingLocator.SelectedItem.Value.ToString() != "All")
                        contrLocatorCode = ddlControlingLocator.SelectedItem.Value.ToString();
                    if (ddlType.SelectedItem.Value.ToString() != "All")
                        level = ddlType.SelectedItem.Value.ToString();
                    if (ddlCode.SelectedItem.Value.ToString() != "All")
                        levelCode = ddlCode.SelectedItem.Value.ToString();
                    if (ddlAWBStatus.SelectedItem.Value.ToString() != "All")
                        AWBStatus = ddlAWBStatus.SelectedItem.Value.ToString();
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


                    res_Revenue = objReport.GetAgentWiseAWBSummary(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom, dtTo, AWBStatus);

                    Session["dsExp"] = res_Revenue;
                    if (res_Revenue != null)
                    {
                        if (res_Revenue.Tables[0].Rows.Count > 0)
                        {
                            Dataset1 = res_Revenue;
                            Dataset2 = showSearchCriteria(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"), AWBStatus);

                            ReportViewer1.Visible = true;



                           // info = new FileInfo(Server.MapPath("/Reports/rptAgentWiseTonnageReport.rdlx"));
                           //// info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptAgentWiseTonnageReport.rdlx");

                           // // info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "Report1.rdlx");

                           // definition = new ReportDefinition(info);
                           // runtime = new ReportRuntime(definition);
                           // runtime.LocateDataSource += WARCustWise_LocateDataSource;
                           // RptViewerRevenue_Station.SetReport(runtime);
                            btnExport.Visible = true;
                            //  oDs2 = LoadParamDataset();// just temperory used, not provide real value                 Dataset2 = oDs2;                  runtime.LocateDataSource += WARCustWise_LocateDataSource;                 WARCustWise.SetReport(runtime);                 pnlMsg.Visible = false;              }             else             {                 oDs1 = null;                 WARCustWise.Visible = false;                 // lblMsg.Text = "No Data Available";                 pnlMsg.Visible = true;                 lblMsgBox.Text = "No Data Available";                 //Pic.Visible = true;               }

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
                            
                            dt = null;
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

                            ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/AgentPerformance.rdlc");
                            //Customers dsCustomers = GetData("select top 20 * from customers");
                            ReportDataSource datasource = new ReportDataSource("dsAgentPerformance_dtAgentPerformance", dt);
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(datasource);
                            ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandlerForSUBrpt);

                            btnExport.Visible = true;
                            SaveUserActivityLog("");
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
                    if (dt != null)
                    {
                        dt.Dispose();
                    }
                    if (oDs1 != null)
                    { oDs1 = null; }
                    if (oDs2 != null)
                    { oDs2 = null; }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region To show subreport
        //private void WARCustWise_LocateDataSource(object sender, LocateDataSourceEventArgs e)
        //{
        //    string dname = e.DataSetName;
        //    //  if (dname.ToLower() == "DataSetValue")    
        //    if (dname == "dsSectorWise")
        //    {
        //        e.Data = Dataset1;

        //    }
        //    else
        //    {
        //        e.Data = Dataset2;
        //    }
        //}
        #endregion


        #region dropdown list to select list
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["dsExp"] = null;
            ReportViewer1.Visible = false;

            DataSet City = null;
            DataSet Region = null;
            DataSet Agent = null;
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
                    City = objBAL.GetCityCode();
                    ddlCode.DataSource = City.Tables[0];
                    ddlCode.DataTextField = "Airport";
                    ddlCode.DataValueField = "CityCode";
                    ddlCode.DataBind();
                    ddlCode.Items.Insert(0, "All");
                    ddlCode.SelectedIndex = 0;

                }


                if (ddlType.SelectedItem.Value.ToString() == "Airport")
                {
                    City = objBAL.GetCityCode();
                    ddlCode.DataSource = City.Tables[0];
                    ddlCode.DataTextField = "Airport";
                    ddlCode.DataValueField = "CityCode";
                    ddlCode.DataBind();
                    ddlCode.Items.Insert(0, "All");
                    ddlCode.SelectedIndex = 0;

                }


                if (ddlType.SelectedItem.Value.ToString() == "Region")
                {
                    Region = objBAL.GetRegionCode();
                    ddlCode.DataSource = Region.Tables[0];
                    ddlCode.DataTextField = "RegionCode";
                    ddlCode.DataValueField = "RegionCode";
                    ddlCode.DataBind();
                    ddlCode.Items.Insert(0, "All");
                    ddlCode.SelectedIndex = 0;


                }
                if (ddlType.SelectedItem.Value.ToString() == "City")
                {

                    City = objBAL.GetCityCode();
                    ddlCode.DataSource = City.Tables[0];
                    ddlCode.DataTextField = "CityCode";
                    ddlCode.DataValueField = "CityCode";
                    ddlCode.DataBind();
                    ddlCode.Items.Insert(0, "All");
                    ddlCode.SelectedIndex = 0;
                }
                if (ddlType.SelectedItem.Value.ToString() == "Country")
                {

                    Agent = objBAL.GetCountryCode();
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

                


            }
                
            catch (Exception ex)
            {


            }
            return true;
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
        }
        #endregion

        #region Fill Session For DDL
        private void FillPaymentType()
        {
            DataSet PaymentCode = null;
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
                {
                    PaymentCode.Dispose();
                }
            }
        }
        #endregion

        #region SearchCriteria
        private DataSet showSearchCriteria(string Agent, string PaymentType, string ControlLoc, string level, string levelCode, string FromDate, string Todate, string AWBStatus)
        {
            DataSet ds = new DataSet("Agent_ds");
            DataTable dtSearch = new DataTable("Agent_dtSearch");
            try
            {
                
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
                if (dtSearch != null)
                {
                    dtSearch.Dispose();
                }
            }

        }
        #endregion

        protected void ddlCode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = new DataSet("Agent_dsExp");
            DataTable dt1 = new DataTable("Agent_dt1"); ;
            lblStatus.Text = "";
            try
            {
               // if ((DataSet)Session["dsExp"] == null)
                    GetData();
                    

                dsExp = (DataSet)Session["dsExp"];
                //dsExp = ds;
                dt1 = (DataTable)dsExp.Tables[0];
                if (dt1.Columns.Contains("CNetRevenue"))
                {
                    dt1.Columns.Remove("CNetRevenue");
                
                }
                if (dt1.Columns.Contains("CNetYield"))
                {
                    dt1.Columns.Remove("CNetYield");
                }
                if (dt1.Columns.Contains("FromDate"))
                {
                    dt1.Columns.Remove("FromDate");
                }
                if (dt1.Columns.Contains("ToDate"))
                {
                    dt1.Columns.Remove("ToDate");
                }



                 dt1.Columns["AgentName"].ColumnName="Agent Name";
                dt1.Columns["CTonnage"].ColumnName = "Tonnage";
                dt1.Columns["CGrossRevenue"].ColumnName = "Gross Revenue";
                dt1.Columns["GrossYield"].ColumnName = "Gross Yield";
               

                //dt = city.GetAllCity();//your datatable 
                string attachment = "attachment; filename=AgentPerformanceReport.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                string tab = "";
                foreach (DataColumn dc in dt1.Columns)
                {
                    Response.Write(tab + dc.ColumnName);
                    tab = "\t";
                }
                Response.Write("\n");
                int i;
                foreach (DataRow dr in dt1.Rows)
                {
                    tab = "";
                    for (i = 0; i < dt1.Columns.Count; i++)
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
                dt1 = null;
            }
        }

        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            try
            {
                // taking all parameters as user selected in report in one variable - "Param"
                string Param = "AgtCode:" + txtAgentCode.Text.ToString() + ", PayMode:" + ddlPaymentType.Text.ToString() + ", CntlCode:" + txtControllingCode.Text.ToString() + ", CntlName:" + ddlControlingLocator.Text.ToString() + ", Level:" + ddlType.Text.ToString() + ", Loc:" + ddlCode.Text.ToString() + ", AWBStatus:" + ddlAWBStatus.Text.ToString() + ", FrmDt:" + txtFromdate.Text.ToString() + ", ToDt:" + txtTodate.Text.ToString();

                objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "Agent Performance", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
            }
            catch(Exception ex)
            {}
            finally
            {
                if (objBAL != null)
                { objBAL = null; }
            
            }
        }

        public void ItemsSubreportProcessingEventHandlerForSUBrpt(object sender, SubreportProcessingEventArgs e)
        {
            DataTable dtAWBDetails = new DataTable("Agent_dtAWBDetails"); ;
            try
            {
            dtAWBDetails = Dataset2.Tables[0];//(DataTable)Session["ManifestGridData"];
            e.DataSources.Add(new ReportDataSource("dsAgentPerformance_dtAgentPerformanceSub", dtAWBDetails));
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

        protected void GetData()
        {
            try
            {
                System.IO.MemoryStream Logo = null;
                //Validate controls
                if (Validate() == false)
                {
                    Session["dsExp"] = null;
                    ReportViewer1.Visible = false;
                    return;

                }
                lblStatus.Text = "";


                //DataSet oDs1 = new DataSet();
                //DataSet oDs2 = new DataSet();
                FileInfo info;
                ////ReportRuntime runtime;
                ////ReportDefinition definition;
                string ErrorLog = string.Empty;
                DataSet res_Revenue1 = new DataSet("Agent_res_Revenue1");


                try
                {

                    string AgentCode = "All", PaymentType = "All", contrLocatorCode = "All", level = "All", levelCode = "All", frmDate = "", ToDt = "", AWBStatus = "All";
                    if (txtAgentCode.Text.Trim() != "")
                        AgentCode = txtAgentCode.Text.Trim();
                    if (ddlPaymentType.SelectedItem.Value.ToString() != "All(Without FOC)")
                        PaymentType = ddlPaymentType.SelectedItem.Value.ToString();
                    if (ddlControlingLocator.SelectedItem.Value.ToString() != "All")
                        contrLocatorCode = ddlControlingLocator.SelectedItem.Value.ToString();
                    if (ddlType.SelectedItem.Value.ToString() != "All")
                        level = ddlType.SelectedItem.Value.ToString();
                    if (ddlCode.SelectedItem.Value.ToString() != "All")
                        levelCode = ddlCode.SelectedItem.Value.ToString();
                    if (ddlAWBStatus.SelectedItem.Value.ToString() != "All")
                        AWBStatus = ddlAWBStatus.SelectedItem.Value.ToString();
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


                    res_Revenue1 = objReport.GetAgentWiseAWBSummary(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom, dtTo, AWBStatus);

                    Session["dsExp"] = res_Revenue1;
                    if (res_Revenue1 != null)
                    {
                        if (res_Revenue1.Tables[0].Rows.Count > 0)
                        {
                            Dataset1 = res_Revenue1;
                            Dataset2 = showSearchCriteria(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"), AWBStatus);

                            //RptViewerRevenue_Station.Visible = true;



                            //info = new FileInfo(Server.MapPath("/Reports/rptAgentWiseTonnageReport.rdlx"));
                            //// info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptAgentWiseTonnageReport.rdlx");

                            //// info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "Report1.rdlx");

                            //definition = new ReportDefinition(info);
                            //runtime = new ReportRuntime(definition);
                            //runtime.LocateDataSource += WARCustWise_LocateDataSource;
                            //RptViewerRevenue_Station.SetReport(runtime);
                            //btnExport.Visible = true;
                            ////  oDs2 = LoadParamDataset();// just temperory used, not provide real value                 Dataset2 = oDs2;                  runtime.LocateDataSource += WARCustWise_LocateDataSource;                 WARCustWise.SetReport(runtime);                 pnlMsg.Visible = false;              }             else             {                 oDs1 = null;                 WARCustWise.Visible = false;                 // lblMsg.Text = "No Data Available";                 pnlMsg.Visible = true;                 lblMsgBox.Text = "No Data Available";                 //Pic.Visible = true;               }

                            ////Logo
                            //try
                            //{
                            //    Logo = CommonUtility.GetImageStream(Page.Server);
                            //    //System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                            //}
                            //catch (Exception ex)
                            //{
                            //    Logo = new System.IO.MemoryStream();
                            //}
                            //DataTable dt = new DataTable();
                            //dt = null;
                            //if (res_Revenue.Tables[0].Rows.Count > 0)
                            //{

                            //    dt = res_Revenue.Tables[0].Copy();
                            //    if (dt.Columns.Contains("Logo") == false)
                            //    {
                            //        DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                            //        col1.DefaultValue = Logo.ToArray();
                            //        dt.Columns.Add(col1);
                            //    }
                            //}

                            //ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            //ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptSectorWiseTonnage.rdlc");
                            ////Customers dsCustomers = GetData("select top 20 * from customers");
                            //ReportDataSource datasource = new ReportDataSource("dsSectorWise_dtSectorwise", dt);
                            //ReportViewer1.LocalReport.DataSources.Clear();
                            //ReportViewer1.LocalReport.DataSources.Add(datasource);
                            //ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandlerForSUBrpt);

                            //btnExport.Visible = true;
                            SaveUserActivityLog("");
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
                    if (res_Revenue1 != null)
                    {
                        res_Revenue1.Dispose();
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
            ReportViewer1.Visible = false;
        }

       
    }
}
