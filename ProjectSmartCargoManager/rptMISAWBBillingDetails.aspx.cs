using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using BAL;
using QID.DataAccess;

using System.IO;
using System.Data.SqlClient;
using System.Drawing;
//using DataDynamics.Reports.Rendering.Excel;
//using DataDynamics.Reports.Rendering.IO;
using Microsoft.Reporting.WebForms;

namespace ProjectSmartCargoManager
{
    public partial class rptMISAWBBillingDetails : System.Web.UI.Page
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
               
                if (Session["UserName"] == null || Session["UserName"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                    
                }

                if (!IsPostBack)
                {
                    RptAWBDetailsViewer.Visible = false;
                    Session["dsAWBDet"] = null;

                    string AgentCode = Convert.ToString(Session["ACode"]);

                    int RoleId = Convert.ToInt32(Session["RoleID"]);

                    if (RoleId == 1 && AgentCode != "")
                    {
                        txtAgentCode.Text = AgentCode;
                        txtAgentCode.Enabled = false;
                    }
                    DateTime dtCurr = Convert.ToDateTime(Session["IT"].ToString());
                    CurrTime = dtCurr.ToString("dd/MM/yyyy hh:mm:ss tt");



                    //RptViewerRevenue_Station.Visible = false;
                    RptAWBDetailsViewer.Visible = false;
                    //FillPaymentType();
                    txtFromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtTodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    DataSet City = objBAL.GetCityCode();
                    ddlCode.DataSource = City.Tables[0];
                    ddlCode.DataTextField = "CityCode";
                    ddlCode.DataValueField = "CityCode";
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

        protected void btnList_Click(object sender, EventArgs e)
        {
            DataSet oDs1 = new DataSet("AWBDetails_oDs1");
            DataSet oDs2 = new DataSet("AWBDetails_oDs2");
            DataSet res_Revenuelist = new DataSet("AWBDetails_res_Revenuelist");
            Session["dsAWBDet"] = null;
            lblStatus.Text = null;
            try
            {
                
                //Validate controls
                if (Validate() == false)
                {
                    RptAWBDetailsViewer.Visible = false;
                    return;
                }

                lblStatus.Text = "";

                //FileInfo info;
                //ReportRuntime runtime;
                //ReportDefinition definition;

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
                        frmDate = dtfrom.ToString("MM/dd/yyyy");
                        string dayTo = txtTodate.Text.Substring(0, 2);
                        string monTo = txtTodate.Text.Substring(3, 2);
                        string yrTo = txtTodate.Text.Substring(6, 4);
                        ToDt = yrTo + "-" + monTo + "-" + dayTo;
                        dtTo = Convert.ToDateTime(ToDt);
                        ToDt = dtTo.ToString("MM/dd/yyyy");
                    }


                    //AgentCode = "All"; PaymentType = "All"; contrLocatorCode = "All"; 
                    // level = "All"; levelCode = "All";


                    res_Revenuelist = objReport.GetAWBBookingwithBillingDetails(AgentCode, PaymentType, contrLocatorCode, level, levelCode, frmDate, ToDt, AWBStatus);

                    

                    if (res_Revenuelist != null)
                    {
                        if (res_Revenuelist.Tables[0].Rows.Count > 0)
                        {
                            Session["dsAWBDet"] = res_Revenuelist;
                            Dataset1 = res_Revenuelist;
                            Dataset2 = showSearchCriteria(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"), AWBStatus);

                            #region Old RDLX
                            //RptViewerRevenue_Station.Visible = true;
                            //info = new FileInfo(Server.MapPath("/Reports/rptDetailMISBookingwithBilling.rdlx"));
                            // info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptDetailMISBooking.rdlx");
                            // info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "Report1.rdlx");
                            //definition = new ReportDefinition(info);
                            //runtime = new ReportRuntime(definition);
                            //runtime.LocateDataSource += WARCustWise_LocateDataSource;
                            //RptViewerRevenue_Station.SetReport(runtime);
                            //  oDs2 = LoadParamDataset();// just temperory used, not provide real value                 Dataset2 = oDs2;                  runtime.LocateDataSource += WARCustWise_LocateDataSource;                 WARCustWise.SetReport(runtime);                 pnlMsg.Visible = false;              }             else             {                 oDs1 = null;                 WARCustWise.Visible = false;                 // lblMsg.Text = "No Data Available";                 pnlMsg.Visible = true;                 lblMsgBox.Text = "No Data Available";                 //Pic.Visible = true;               }
                            //btnExportReport.Enabled = true;
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

                            if (res_Revenuelist.Tables[0].Columns.Contains("Logo") == false)
                            {
                                DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                                col1.DefaultValue = Logo.ToArray();
                                res_Revenuelist.Tables[0].Columns.Add(col1);
                            }

                            RptAWBDetailsViewer.Visible = true;
                            RptAWBDetailsViewer.ProcessingMode = ProcessingMode.Local;
                            RptAWBDetailsViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/AWBDetails.rdlc");
                            //Customers dsCustomers = GetData("select top 20 * from customers");
                            ReportDataSource datasource = new ReportDataSource("dsAWBDetails_dtAWBDetails", res_Revenuelist.Tables[0]);
                            RptAWBDetailsViewer.LocalReport.DataSources.Clear();
                            RptAWBDetailsViewer.LocalReport.DataSources.Add(datasource);
                            RptAWBDetailsViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                            SaveUserActivityLog("");

                            #endregion
                        }
                        else
                        {
                            RptAWBDetailsViewer.Visible = false;
                           
                            //RptViewerRevenue_Station.Visible = false;
                            Session["dsAWBDet"] = null;
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data not available for given search criteria";
                            txtFromdate.Focus();
                            //btnExportReport.Enabled = false;
                            return;


                        }
                    }
                    else
                    {
                        //RptViewerRevenue_Station.Visible = false;
                        Session["dsAWBDet"] = null;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not available for given search criteria";
                        txtFromdate.Focus();
                        //btnExportReport.Enabled = false;
                        return;

                    }

                }
                catch (Exception ex)
                { }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (oDs1 != null)
                    oDs1.Dispose();
                if (oDs2 != null)
                    oDs2.Dispose();
                if (res_Revenuelist != null)
                    res_Revenuelist.Dispose();
            }
        }

        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsAWBDetails_dtAWBDetailsSub", Dataset2.Tables[0]));
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

        #region dropdown list to select list
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet City = new DataSet("AWBDetails_city");
            DataSet Agent = new DataSet("AWBDetails_Agent");
            DataSet Region = new DataSet("AWBDetails_Region");
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
                    ddlCode.DataTextField = "CityCode";
                    ddlCode.DataValueField = "CityCode";
                    ddlCode.DataBind();
                    ddlCode.Items.Insert(0, "All");
                    ddlCode.SelectedIndex = 0;

                }


                if (ddlType.SelectedItem.Value.ToString() == "Airport")
                {
                    City = objBAL.GetCityCode();
                    ddlCode.DataSource = City.Tables[0];
                    ddlCode.DataTextField = "CityCode";
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
                    City = null;
                }
                if (Region != null)
                {
                    Region = null;
                }
                if (Agent != null)
                {
                    Agent = null;
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
        {
            DataSet Region = new DataSet("AWB_region");
            DataSet City = new DataSet("AWB_city");
            DataSet Agent = new DataSet("AWB_agent");
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
                {
                    Region = null;
                }
                if (Agent != null)
                {
                    Agent = null;
                }

                if (City != null)
                {
                    City = null;
                }
                
            }
        }
        #endregion

        #region Fill Session For DDL
        private void FillPaymentType()
        {DataSet PaymentCode=new DataSet("AWBDetails_PaymentCode");
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

                PaymentCode = null;
            }
        }
        }
        #endregion

        #region SearchCriteria
        private DataSet showSearchCriteria(string Agent, string PaymentType, string ControlLoc, string level, string levelCode, string FromDate, string Todate, string AWBStatus)
        {
            DataSet ds = new DataSet("AWBDetails_ds");
            DataTable dtSearch = new DataTable("AWBDetails_dtSearch");
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
                    ds = null;
                }
                if (dtSearch != null)
                {
                    dtSearch = null;
                }
            
            }

        }
        #endregion

        #region Button Export
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                //Validate controls
                Validate();

                lblStatus.Text = "";
                DataSet oDs1_ex = new DataSet("AWBDetails_oDs1_ex");
                DataSet oDs2_ex = new DataSet("AWBDetails_oDs2_ex");
                DataSet res_Revenue1 = new DataSet("AWBDetails_res_Revenue1");
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


                    res_Revenue1 = objReport.GetAWBBookingwithBillingDetails(AgentCode, PaymentType, contrLocatorCode, level, levelCode, frmDate, ToDt, AWBStatus);


                    if (res_Revenue1 != null)
                    {
                        if (res_Revenue1.Tables[0].Rows.Count > 0)
                        {
                            Dataset1 = res_Revenue1;
                            Dataset2 = showSearchCriteria(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"), AWBStatus);

                            //RptViewerRevenue_Station.Visible = false;
                            RptAWBDetailsViewer.Visible = false;
                            #region NewCode
                            Random random = new Random();
                            int RandomNo = random.Next();
                            string UniqueNo = RandomNo.ToString().Substring(0, 3);
                            ////DataDynamics.Reports.ReportDefinition _reportDef = new DataDynamics.Reports.ReportDefinition(new FileInfo(Server.MapPath("/Reports/rptDetailMISBooking.rdlx")));
                            ////DataDynamics.Reports.ReportRuntime _reportRuntime = new DataDynamics.Reports.ReportRuntime(_reportDef);
                            ////_reportRuntime.LocateDataSource += WARCustWise_LocateDataSource;
                            string exportFile = System.IO.Path.GetTempFileName() + ".xls";
                            System.IO.FileInfo myFile = new System.IO.FileInfo(exportFile);
                            System.Collections.Specialized.NameValueCollection settings = new System.Collections.Specialized.NameValueCollection();
                            settings.Add("hideToolbar", "True");
                            settings.Add("hideMenubar", "True");
                            settings.Add("hideWindowUI", "True");
                            ////ExcelTransformationDevice _renderingExtension = new DataDynamics.Reports.Rendering.Excel.ExcelTransformationDevice();
                            ////FileStreamProvider _provider = new DataDynamics.Reports.Rendering.IO.FileStreamProvider(myFile.Directory, Path.GetFileNameWithoutExtension(myFile.Name));
                            ////_reportRuntime.Render(_renderingExtension, _provider, settings, true, true);
                            Response.Clear();
                            Response.ContentType = "application/xls";
                            string filename = Session["UserName"].ToString() + UniqueNo + DateTime.Now.ToString("mmddyyy hh:MM:ss") + ".xls";
                            string FullName = filename.Replace(" ", "");
                            Response.AddHeader("content-disposition", "attachment; filename=" + FullName);
                            Response.BinaryWrite(File.ReadAllBytes(exportFile));
                            myFile.Delete();
                            #endregion

                            //  oDs2 = LoadParamDataset();// just temperory used, not provide real value                 Dataset2 = oDs2;                  runtime.LocateDataSource += WARCustWise_LocateDataSource;                 WARCustWise.SetReport(runtime);                 pnlMsg.Visible = false;              }             else             {                 oDs1 = null;                 WARCustWise.Visible = false;                 // lblMsg.Text = "No Data Available";                 pnlMsg.Visible = true;                 lblMsgBox.Text = "No Data Available";                 //Pic.Visible = true;               }
                        }
                        else
                        {
                            //RptViewerRevenue_Station.Visible = false;
                            RptAWBDetailsViewer.Visible = false;
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data not avialable for given search criteria";
                            txtFromdate.Focus();
                            return;


                        }
                    }
                    else
                    {
                        //RptViewerRevenue_Station.Visible = false;
                        RptAWBDetailsViewer.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not avialable for given search criteria";
                        txtFromdate.Focus();
                        return;

                    }

                }
                catch (Exception ex)
                { }

                finally
                {
                    if (res_Revenue1 != null)
                    {
                        res_Revenue1 = null;

                    }
                }
                if (oDs1_ex != null)
                {
                    oDs1_ex = null;

                }
                if (oDs2_ex != null)
                {
                    oDs2_ex = null;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            { 
            
            
            }

        }
        #endregion

        protected void btnExportReport_Click(object sender, EventArgs e)
        {
            // Commented By : Deepak  Walmiki 27/09/2013
            #region Commented Code
            //DataSet ds = null;
            //DataTable dt = null;

            //try
            //{
            //    if ((DataSet)Session["dsAWBDet"] == null)
            //        return;

            //    ds = (DataSet)Session["dsAWBDet"];
            //    dt = (DataTable)ds.Tables[0];
            //    //dt = city.GetAllCity();//your datatable 
            //    string attachment = "attachment; filename=AWBDetail.xls";
            //    Response.ClearContent();
            //    Response.AddHeader("content-disposition", attachment);
            //    Response.ContentType = "application/vnd.ms-excel";
            //    string tab = "";
            //    foreach (DataColumn dc in dt.Columns)
            //    {
            //        Response.Write(tab + dc.ColumnName);
            //        tab = "\t";
            //    }
            //    Response.Write("\n");
            //    int i;
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        tab = "";
            //        for (i = 0; i < dt.Columns.Count; i++)
            //        {
            //            Response.Write(tab + dr[i].ToString());
            //            tab = "\t";
            //        }
            //        Response.Write("\n");
            //    }
            //    Response.End();
            //}
            //catch (Exception ex)
            //{ }
            //finally
            //{
            //    ds = null;
            //    dt = null;
            //}
            #endregion


            //Added By : Deepak  Walmiki 27/09/2013
            DataSet ds = new DataSet("AWBDetails_ds");
            DataTable dt = new DataTable("AWBDetails_dt");
            lblStatus.Text = string.Empty;
            Session["dsAWBDet"] = null;
            lblStatus.Text = string.Empty;
            try
            {
                if (Validate() == false)
                {
                    RptAWBDetailsViewer.Visible = false;
                    return;
                }
                GetData();
                ds = null;
                dt = null;

                ds = (DataSet)Session["dsAWBDet"];
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    dt = (DataTable)ds.Tables[0];
                    ExportToExcel(dt, "AWBDetailReport.xls");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No records found for the selected search criteria!');</SCRIPT>", false);
                    return;
                }
                
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

            finally
                {
                    if (ds != null)
                    { ds = null; }
                    if (dt != null)
                    { dt = null; }
                }
           
        }

        // Coded By : Deepak Walmiki   Date: 27/09/2013
        #region Export to DataTable
        public void ExportToExcel(DataTable dt, string FileName)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    string filename = FileName;
                    System.IO.StringWriter tw = new System.IO.StringWriter();
                    System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                    DataGrid dgGrid = new DataGrid();
                    dgGrid.DataSource = dt;
                    dgGrid.DataBind();

                    //Get the HTML for the control.
                    dgGrid.RenderControl(hw);
                    //Write the HTML back to the browser.
                    //Response.ContentType = application/vnd.ms-excel;
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                    this.EnableViewState = false;
                    Response.Write(tw.ToString());
                    Response.End();
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            try
            {
                // taking all parameters as user selected in report in one variable - "Param"
                string Param = "AgtCode:" + txtAgentCode.Text.ToString() + ", PayMode:" + ddlPaymentType.Text.ToString() + ",CntlName:" + ddlControlingLocator.Text.ToString() + ", Level:" + ddlType.Text.ToString() + ", Loc:" + ddlCode.Text.ToString() + ",FrmDt:" + txtFromdate.Text.ToString() + ", ToDt:" + txtTodate.Text.ToString();
                objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "AWB Details", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
            }
            catch (Exception ex)
            { }
            finally
            {
                if (objBAL != null)
                    objBAL = null;
            }
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
                if (objBL != null)
                    objBL = null;
            }

            if (DaysConfigured > 0 && intReportInterval > DaysConfigured)
                return "Report can be generated only for " + DaysConfigured.ToString() + " days.";
            else
                return "";
        }

        protected void GetData()
        {
            DataSet oDs1 = new DataSet("AWBDetails_oDs1");
            DataSet oDs2 = new DataSet("AWBDetails_oDs2");
            DataSet res_Revenue = new DataSet("AWBDetails_res_Revenue");
            try
            {
                Session["dsAWBDet"] = null;
                lblStatus.Text = "";

                FileInfo info;
                ////ReportRuntime runtime;
                ////ReportDefinition definition;

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
                        frmDate = dtfrom.ToString("MM/dd/yyyy");
                        string dayTo = txtTodate.Text.Substring(0, 2);
                        string monTo = txtTodate.Text.Substring(3, 2);
                        string yrTo = txtTodate.Text.Substring(6, 4);
                        ToDt = yrTo + "-" + monTo + "-" + dayTo;
                        dtTo = Convert.ToDateTime(ToDt);
                        ToDt = dtTo.ToString("MM/dd/yyyy");
                    }


                    //AgentCode = "All"; PaymentType = "All"; contrLocatorCode = "All"; 
                    // level = "All"; levelCode = "All";


                    res_Revenue = objReport.GetAWBBookingwithBillingDetails(AgentCode, PaymentType, contrLocatorCode, level, levelCode, frmDate, ToDt, AWBStatus);
                    if (res_Revenue != null)
                    {
                        if (res_Revenue.Tables[0].Rows.Count > 0)
                        {
                            Session["dsAWBDet"] = res_Revenue;
                            Dataset1 = res_Revenue;
                            Dataset2 = showSearchCriteria(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"), AWBStatus);
                        }
                        else
                        {
                            Session["dsAWBDet"] = null;
                            txtFromdate.Focus();
                            return;

                        }
                    }
                    else
                    {
                        Session["dsAWBDet"] = null;
                        txtFromdate.Focus();
                        return;

                    }

                }
                catch (Exception ex)
                { }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (oDs1 != null)
                    oDs1.Dispose();
                if (oDs2 != null)
                    oDs2.Dispose();
                if (res_Revenue != null)
                    res_Revenue.Dispose();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
           // Response.Redirect("~/rptMISAAWBBillingDetails.aspx", false);
            RptAWBDetailsViewer.Visible = false;
            lblStatus.Text = "";
            txtAgentCode.Text=txtFromdate.Text=txtTodate.Text="";
            
        }
    }
}
