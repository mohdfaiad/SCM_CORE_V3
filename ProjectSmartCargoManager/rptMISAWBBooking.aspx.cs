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
    public partial class rptMISAWBBooking : System.Web.UI.Page
    {
        #region Variable
        private DataSet Dataset1=new DataSet("AWBTonnage_dsDataset1");
        private DataSet Dataset2 = new DataSet("AWBTonnage_dsDataset2");
        StockAllocationBAL objBAL = new StockAllocationBAL();
        ReportBAL objReport = new ReportBAL();
        public static string CurrTime = "";
        
        
        
        
        #endregion


        #region On page load
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet City = new DataSet("AWBTonnage_dsCity");
            try
            {
                if (!IsPostBack)
                {
                   string AgentCode = Convert.ToString(Session["ACode"]);

                    int RoleId = Convert.ToInt32(Session["RoleID"]);

                    if (RoleId == 1 && AgentCode != "")
                    {
                        txtAgentCode.Text = AgentCode;
                       txtAgentCode.Enabled = false;
                    }
                    DateTime dtCurr = Convert.ToDateTime(Session["IT"].ToString());
                    CurrTime = dtCurr.ToString("dd/MM/yyyy hh:mm:ss tt");



                    ReportViewer1.Visible = false;
                    //FillPaymentType();
                    txtFromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtTodate.Text = DateTime.Now.ToString("dd/MM/yyyy"); 
                     City = objBAL.GetCityCode();
                    ddlCode.DataSource = City.Tables[0];
                    ddlCode.DataTextField = "CityCode";
                    ddlCode.DataValueField = "CityCode";
                    ddlCode.DataBind();
                    ddlCode.Items.Insert(0, "All");
                    ddlCode.SelectedIndex = 0;
                    if (City != null)
                    {
                        City.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
            }

            bool blnFlag = false;
            DataTable dt = (DataTable)Session["StatusMaster"];
            try
            {
                if (dt != null)
                {
                    for (int intCount = 0; intCount < dt.Rows.Count; intCount++)
                    {
                        for (int i = 0; i < ddlAWBStatus.Items.Count; i++)
                        {
                            if (dt.Rows[intCount]["Status"].ToString().Trim() == ddlAWBStatus.Items[i].Text.Trim())
                            {
                                blnFlag = true;
                                break;
                            }
                        }
                        if (blnFlag)
                            break;
                    }

                    if (blnFlag == false)
                    {
                        for (int intCount = 0; intCount < dt.Rows.Count; intCount++)
                        {
                            ListItem item = new ListItem();
                            item.Value = dt.Rows[intCount]["StatusCode"].ToString();
                            item.Text = dt.Rows[intCount]["Status"].ToString();
                            ddlAWBStatus.Items.Add(item);
                        }
                    }

                    dt = null;
                }
            }
            catch (Exception ex) { }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
            
        }
        #endregion


        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                System.IO.MemoryStream Logo = null;
                //Validate controls
                if (Validate() == false)
                {
                    Session["res_Revenue"] = null;
                    ReportViewer1.Visible = false;
                    return;
                }
                lblStatus.Text = "";
                DataSet oDs1 = new DataSet("AWBTonnage_dsoDs1");
                DataSet oDs2 = new DataSet("AWBTonnage_dsoDs2");
                FileInfo info;
                ////ReportRuntime runtime;
                ////ReportDefinition definition;
                DataSet res_Revenue = new DataSet("AWBTonnage_dsAWBTonnage");
                DataTable dt = new DataTable("AWBTonnage_dtAWBTonnage");
                string ErrorLog = string.Empty;
                
                try
                {   
                   
                    string AgentCode = "All", PaymentType = "All", contrLocatorCode = "All", level = "All", levelCode = "All", frmDate = "", ToDt = "",AWBStatus="All";
                    if (txtAgentCode.Text.Trim() != "")
                        AgentCode = txtAgentCode.Text.Trim();
                    if (ddlPaymentType.SelectedItem.Value.ToString() != "All")
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

                    DateTime dtTo=new DateTime(); DateTime dtfrom=new DateTime();
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
                   // level = "All"; levelCode = "All";
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
                    DateTime dt1;
                    DateTime dt2;
                    dt1 = DateTime.ParseExact(txtFromdate.Text, "dd/MM/yyyy",null);
                    dt2 = DateTime.ParseExact(txtTodate.Text,"dd/MM/yyyy",null);
                    frmDate = dt1.ToString("MM/dd/yyyy");
                    ToDt = dt2.ToString("MM/dd/yyyy");
                    if (ddlDateCriteria.SelectedItem.Text == "Execution Date")
                    {
                        res_Revenue = objReport.GetAWBBookingDetails(AgentCode, PaymentType, contrLocatorCode, level, levelCode, frmDate, ToDt, AWBStatus);
                    }
                    else
                    {
                       
                        res_Revenue = objReport.GetAWBBookingDetails_FlightDateWise(AgentCode, PaymentType, contrLocatorCode, level, levelCode, frmDate, ToDt, AWBStatus);
                    }

                    Session["res_Revenue"] = res_Revenue;
                    
                    if (res_Revenue != null)
                    {
                        if (res_Revenue.Tables[0].Rows.Count > 0)
                        {
                            Dataset1 = res_Revenue;
                            Dataset2 = showSearchCriteria(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"),AWBStatus);
                            //Session["SUBDS"] = Dataset2.Tables[0];

                            ReportViewer1.Visible = true;



                         
                           // info = new FileInfo(Server.MapPath("/Reports/rptDetailMISBooking.rdlx"));
                           //// info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptDetailMISBooking.rdlx");
                      
                           // // info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "Report1.rdlx");

                           // definition = new ReportDefinition(info);
                           // runtime = new ReportRuntime(definition);
                           // runtime.LocateDataSource += WARCustWise_LocateDataSource;
                           // RptViewerRevenue_Station.SetReport(runtime);
                            //  oDs2 = LoadParamDataset();// just temperory used, not provide real value Dataset2 = oDs2;                  runtime.LocateDataSource += WARCustWise_LocateDataSource;                 WARCustWise.SetReport(runtime);                 pnlMsg.Visible = false;              }             else             {                 oDs1 = null;                 WARCustWise.Visible = false;                 // lblMsg.Text = "No Data Available";                 pnlMsg.Visible = true;                 lblMsgBox.Text = "No Data Available";                 //Pic.Visible = true;               }
                            
                            
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
                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/AWBTonnage.rdlc");
                            //Customers dsCustomers = GetData("select top 20 * from customers");
                            ReportDataSource datasource = new ReportDataSource("dsAWBTonnage_dtAWBTonnage", dt);
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(datasource);
                            ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandlerForSUBrpt);
                            btnExportReport.Enabled = true;
                            SaveUserActivityLog("");
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
        //    if (Dataset1 != null)
        //    {
        //        Dataset1.Dispose();
        //    }
        //    if (Dataset2 != null)
        //    {
        //        Dataset2.Dispose();
        //    }
        //}
        #endregion


        #region dropdown list to select list
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet City = new DataSet("AWBTonnage_dsCityType");
            DataSet Region = new DataSet("AWBTonnage_dsRegionType"); ;
            DataSet Agent = new DataSet("AWBTonnage_dsAgentType"); ;
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
                    ddlCode.DataTextField = "CountryCode";
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
            DataSet City = new DataSet("AWBTonnage_dsCityFill"); ;
            DataSet Region = new DataSet("AWBTonnage_dsRegionFill"); ;
            DataSet Agent = new DataSet("AWBTonnage_dsAgentFill"); ;
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

        #region Fill Session For DDL
        private void FillPaymentType()
        {
            DataSet PaymentCode = new DataSet("AWBTonnage_dsPayementType"); ;
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
        private DataSet showSearchCriteria(string Agent, string PaymentType, string ControlLoc, string level, string levelCode, string FromDate, string Todate,string AWBStatus)
        {
            DataSet ds = new DataSet("AWBTonnage_dsSearchCriteria");
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

        protected void ddlAWBStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #region Button Export
        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet oDs1 = new DataSet("AWBTonnage_dsoDs1");
            DataSet oDs2 = new DataSet("AWBTonnage_dsoDs2");
            try
            {
                lblStatus.Text = string.Empty;
                //Validate controls
                if (Validate() == false)
                {
                    Session["res_Revenue"] = null;
                    ReportViewer1.Visible = false;
                    return;
                }

                GetData();
                lblStatus.Text = "";
               
                FileInfo info;
                ////ReportRuntime runtime;
                ////ReportDefinition definition;
                DataSet res_Revenue = new DataSet("AWBTonnage_dsExportData");

                try
                {

                    string AgentCode = "All", PaymentType = "All", contrLocatorCode = "All", level = "All", levelCode = "All", frmDate = "", ToDt = "", AWBStatus = "All";
                    if (txtAgentCode.Text.Trim() != "")
                        AgentCode = txtAgentCode.Text.Trim();
                    if (ddlPaymentType.SelectedItem.Value.ToString() != "All")
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
                    // level = "All"; levelCode = "All";

                    if (ddlDateCriteria.SelectedItem.Text == "Execution Date")
                    {
                        res_Revenue = objReport.GetAWBBookingDetails(AgentCode, PaymentType, contrLocatorCode, level, levelCode, frmDate, ToDt, AWBStatus);
                    }
                    else
                    {
                        res_Revenue = objReport.GetAWBBookingDetails_FlightDateWise(AgentCode, PaymentType, contrLocatorCode, level, levelCode, frmDate, ToDt, AWBStatus);
                    }


                    if (res_Revenue != null)
                    {
                        if (res_Revenue.Tables[0].Rows.Count > 0)
                        {
                            Dataset1 = res_Revenue;
                            Dataset2 = showSearchCriteria(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"), AWBStatus);

                            ReportViewer1.Visible = false;

                            #region Old Code

                            //  info = new FileInfo(Server.MapPath("/Reports/rptDetailMISBooking.rdlx"));
                            //  // info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptDetailMISBooking.rdlx");

                            //  // info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "Report1.rdlx");

                            //  definition = new ReportDefinition(info);
                            //  runtime = new ReportRuntime(definition);
                            //  runtime.LocateDataSource += WARCustWise_LocateDataSource;
                            //  ExcelTransformationDevice device = new ExcelTransformationDevice();
                            //  ExcelTemplateGenerator template = new ExcelTemplateGenerator();
                            //  MemoryStream templateStream = new MemoryStream();
                            //System.Collections.Specialized.NameValueCollection settings=new System.Collections.Specialized.NameValueCollection();


                            //  //template.GenerateTemplate(runtime.ReportDefinition, templateStream);
                            //  templateStream.Position = 0;
                            //  device.TemplateStream = templateStream;
                            //  FileStreamProvider fileStream;
                            //  Random random = new Random();
                            //  if (!File.Exists(ConfigurationManager.AppSettings["ExcelPath"].ToString() + "xlsexport.xls"))
                            //  {
                            //      fileStream = new FileStreamProvider(new DirectoryInfo(ConfigurationManager.AppSettings["ExcelPath"].ToString()), "xlsexport.xls");
                            //  }
                            //  else
                            //  {
                            //      File.Delete(ConfigurationManager.AppSettings["ExcelPath"].ToString() + "xlsexport.xls");
                            //      fileStream = new FileStreamProvider(new DirectoryInfo(ConfigurationManager.AppSettings["ExcelPath"].ToString()), "xlsexport.xls");

                            //  }
                            //  runtime.Render(device, fileStream,settings,true);
                            //  if (File.ReadAllBytes(ConfigurationManager.AppSettings["ExcelPath"].ToString()+"xlsexport.xls").Length > 0)
                            //  {

                            //      int RandomNo = random.Next();
                            //      string UniqueNo = RandomNo.ToString().Substring(0, 3);

                            //      Response.Clear();
                            //      Response.ContentType = "application/xls";
                            //      string filename=Session["UserName"].ToString()+UniqueNo+DateTime.Now.ToString("mmddyyy hh:MM:ss")+".xls";
                            //      Response.AddHeader("content-disposition", "attachment; filename="+filename);
                            //      Response.BinaryWrite(File.ReadAllBytes(ConfigurationManager.AppSettings["ExcelPath"].ToString() + "xlsexport.xls"));
                            //      File.Delete(ConfigurationManager.AppSettings["ExcelPath"].ToString() + "xlsexport.xls");
                            //      lblStatus.Text = "File Exported Successfully";
                            //      lblStatus.ForeColor = Color.Green;

                            //  }


                            #endregion

                            //#region NewCode
                            //Random random = new Random();
                            //int RandomNo = random.Next();
                            //string UniqueNo = RandomNo.ToString().Substring(0, 3);
                            //DataDynamics.Reports.ReportDefinition _reportDef = new DataDynamics.Reports.ReportDefinition(new FileInfo(Server.MapPath("/Reports/rptDetailMISBooking.rdlx")));
                            //DataDynamics.Reports.ReportRuntime _reportRuntime = new DataDynamics.Reports.ReportRuntime(_reportDef);
                            //_reportRuntime.LocateDataSource += WARCustWise_LocateDataSource;
                            //string exportFile = System.IO.Path.GetTempFileName() + ".xls";
                            //System.IO.FileInfo myFile = new System.IO.FileInfo(exportFile);
                            //System.Collections.Specialized.NameValueCollection settings = new System.Collections.Specialized.NameValueCollection();
                            //settings.Add("hideToolbar", "True");
                            //settings.Add("hideMenubar", "True");
                            //settings.Add("hideWindowUI", "True");
                            //ExcelTransformationDevice _renderingExtension = new DataDynamics.Reports.Rendering.Excel.ExcelTransformationDevice();
                            //FileStreamProvider _provider = new DataDynamics.Reports.Rendering.IO.FileStreamProvider(myFile.Directory, Path.GetFileNameWithoutExtension(myFile.Name));
                            //_reportRuntime.Render(_renderingExtension, _provider, settings, true, true);
                            //Response.Clear();
                            //Response.ContentType = "application/xls";
                            //string filename = Session["UserName"].ToString() + UniqueNo + DateTime.Now.ToString("mmddyyy hh:MM:ss") + ".xls";
                            //string FullName = filename.Replace(" ", "");
                            //Response.AddHeader("content-disposition", "attachment; filename=" + FullName);
                            //Response.BinaryWrite(File.ReadAllBytes(exportFile));
                            //myFile.Delete();
                            //#endregion


                            //  oDs2 = LoadParamDataset();// just temperory used, not provide real value                 Dataset2 = oDs2;                  runtime.LocateDataSource += WARCustWise_LocateDataSource;                 WARCustWise.SetReport(runtime);                 pnlMsg.Visible = false;              }             else             {                 oDs1 = null;                 WARCustWise.Visible = false;                 // lblMsg.Text = "No Data Available";                 pnlMsg.Visible = true;                 lblMsgBox.Text = "No Data Available";                 //Pic.Visible = true;               }

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

        protected void btnExportReport_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            DataTable dt = null;
            Session["res_Revenue"] = null;

            lblStatus.Text = string.Empty;
            try
            {
                //lblStatus.Text = string.Empty;
                //Validate controls
                if (Validate() == false)
                {
                    Session["res_Revenue"] = null;
                    ReportViewer1.Visible = false;
                    return;
                }
                //if ((DataSet)Session["res_Revenue"] == null)
              
                    GetData();


                    lblStatus.Text = "";
               ds = (DataSet)Session["res_Revenue"];
               dt = (DataTable)ds.Tables[0];
                if (Session["res_Revenue"] == null && dt == null)
                {
                    lblStatus.Text = "No records found";
                    lblStatus.ForeColor = Color.Red;
                    SaveUserActivityLog(lblStatus.Text);
                    ReportViewer1.Visible = false;
                    return;
                }
                //dt = city.GetAllCity();//your datatable 
                if (dt.Columns.Contains("Logo"))
                { dt.Columns.Remove("Logo"); }
                string attachment = "attachment; filename=AWBTonnage.xls";
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




        public void ItemsSubreportProcessingEventHandlerForSUBrpt(object sender, SubreportProcessingEventArgs e)
        {
            DataTable dtAWBDetails = null;
            try
            {
                dtAWBDetails = Dataset2.Tables[0];//(DataTable)Session["ManifestGridData"];
                e.DataSources.Add(new ReportDataSource("dsAWBTonnage_dtAWBTonnageSub", dtAWBDetails));
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
            string Param = "AgtCode:" + txtAgentCode.Text.ToString() + ", PayMode:" + ddlPaymentType.Text.ToString() + ", CntlCode:" + txtControllingCode.Text.ToString() + ", CntlName:" + ddlControlingLocator.Text.ToString() + ", Level:" + ddlType.Text.ToString() + ", Loc:" + ddlCode.Text.ToString() + ", AWBStatus:" + ddlAWBStatus.Text.ToString() + ", FrmDt:" + txtFromdate.Text.ToString() + ", ToDt:" + txtTodate.Text.ToString();

            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "AWB Tonnage", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
        }


        protected void GetData()
        {
            try
            {
                System.IO.MemoryStream Logo = null;
                //Validate controls
                if (Validate() == false)
                {
                    ReportViewer1.Visible = false;
                    Session["res_Revenue"] = null;
                    return;
                }
                lblStatus.Text = "";
                DataSet oDs1 = new DataSet("AWBTonnage_dsGetData1");
                DataSet oDs2 = new DataSet("AWBTonnage_dsGetData2");
                FileInfo info;
                ////ReportRuntime runtime;
                ////ReportDefinition definition;
                DataSet res_Revenue = new DataSet();
                string ErrorLog = string.Empty;

                try
                {

                    string AgentCode = "All", PaymentType = "All", contrLocatorCode = "All", level = "All", levelCode = "All", frmDate = "", ToDt = "", AWBStatus = "All";
                    if (txtAgentCode.Text.Trim() != "")
                        AgentCode = txtAgentCode.Text.Trim();
                    if (ddlPaymentType.SelectedItem.Value.ToString() != "All")
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
                    // level = "All"; levelCode = "All";
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

                    if (ddlDateCriteria.SelectedItem.Text == "Execution Date")
                    {
                        res_Revenue = objReport.GetAWBBookingDetails(AgentCode, PaymentType, contrLocatorCode, level, levelCode, frmDate, ToDt, AWBStatus);
                    }
                    else
                    {
                        res_Revenue = objReport.GetAWBBookingDetails_FlightDateWise(AgentCode, PaymentType, contrLocatorCode, level, levelCode, frmDate, ToDt, AWBStatus);
                    }

                    Session["res_Revenue"] = res_Revenue;
                    SaveUserActivityLog("");
                    if (res_Revenue != null)
                    {
                        if (res_Revenue.Tables[0].Rows.Count > 0)
                        {
                            Dataset1 = res_Revenue;
                            Dataset2 = showSearchCriteria(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"), AWBStatus);
                            //Session["SUBDS"] = Dataset2.Tables[0];

                            ReportViewer1.Visible = true;




                            //info = new FileInfo(Server.MapPath("/Reports/rptDetailMISBooking.rdlx"));
                            //// info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptDetailMISBooking.rdlx");

                            //// info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "Report1.rdlx");

                            //definition = new ReportDefinition(info);
                            //runtime = new ReportRuntime(definition);
                            //runtime.LocateDataSource += WARCustWise_LocateDataSource;
                            //RptViewerRevenue_Station.SetReport(runtime);
                            ////  oDs2 = LoadParamDataset();// just temperory used, not provide real value Dataset2 = oDs2;                  runtime.LocateDataSource += WARCustWise_LocateDataSource;                 WARCustWise.SetReport(runtime);                 pnlMsg.Visible = false;              }             else             {                 oDs1 = null;                 WARCustWise.Visible = false;                 // lblMsg.Text = "No Data Available";                 pnlMsg.Visible = true;                 lblMsgBox.Text = "No Data Available";                 //Pic.Visible = true;               }
                            //btnExportReport.Enabled = true;
                            //DataTable dt = new DataTable();
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
                            //ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/AWBTonnage.rdlc");
                            ////Customers dsCustomers = GetData("select top 20 * from customers");
                            //ReportDataSource datasource = new ReportDataSource("dsAWBTonnage_dtAWBTonnage", dt);
                            //ReportViewer1.LocalReport.DataSources.Clear();
                            //ReportViewer1.LocalReport.DataSources.Add(datasource);
                            //ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandlerForSUBrpt);
                        }
                        else
                        {
                            ReportViewer1.Visible = false;//new line
                            Session["res_Revenue"] = null;
                            lblStatus.ForeColor = Color.Red;
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
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/rptMISAWBBooking.aspx", false);
        }
    }

    
    
        #endregion
    
}
