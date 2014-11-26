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
    public partial class rptAWBStockControlReport : System.Web.UI.Page
    {

        #region Variable
        private DataSet Dataset1=new DataSet("AWBStock_dsDataset1");
        private DataSet Dataset2 = new DataSet("AWBStock_dsDataset2");
        StockAllocationBAL objBAL = new StockAllocationBAL();
        ReportBAL objReport = new ReportBAL();
        public static string CurrTime = "";

        #endregion

        #region On page load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                txtAgentCode.Enabled = true;

                if (!IsPostBack)
                {
                    Session["dsAWBStock"] = null;
                    string AgentCode = Convert.ToString(Session["ACode"]);

                    int RoleId = Convert.ToInt32(Session["RoleID"]);

                    if (RoleId == 1 && AgentCode != "")
                    {
                        txtAgentCode.Text = AgentCode;
                        txtAgentCode.Enabled = false;
                    }

                    AWBStockRptViewer.Visible = false;

                    txtFromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtTodate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                    DateTime dtCurr = Convert.ToDateTime(Session["IT"].ToString());
                    CurrTime = dtCurr.ToString("dd/MM/yyyy hh:mm:ss tt");
                    ddlType_SelectedIndexChanged(null, null);

                    //DataSet City = objBAL.GetCityCode();
                    //ddlCode.DataSource = City.Tables[0];
                    //ddlCode.DataTextField = "CityCode";
                    //ddlCode.DataValueField = "CityCode";
                    //ddlCode.DataBind();
                    //ddlCode.Items.Insert(0, "All");
                    //ddlCode.SelectedIndex = 0;

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
            DataSet oDs1 = new DataSet("AWBStock_dsoDs1");
            DataSet oDs2 = new DataSet("AWBStock_dsoDs2");
            DataSet res_Revenue = new DataSet("AWBStock_dsAWBStock");
            try
            {
                Session["dsAWBStock"] = null;
                lblStatus.Text = string.Empty;
                AWBStockRptViewer.Visible = false;
                //Validate controls
                if (Validate() == false)
                {
                    Session["dsAWBStock"] = null;
                    AWBStockRptViewer.Visible = false;
                    return;
                }

                //FileInfo info;
                //ReportRuntime runtime;
                //ReportDefinition definition;

                try
                {

                    string AgentCode = "All", PaymentType = "All", contrLocatorCode = "All", level = "All", levelCode = "All", frmDate = "", ToDt = "", AWBStatus = "All";
                    if (txtAgentCode.Text.Trim() != "")
                        AgentCode = txtAgentCode.Text.Trim();
                    //if (ddlPaymentType.SelectedItem.Value.ToString() != "All(Without FOC)")
                    //    PaymentType = ddlPaymentType.SelectedItem.Value.ToString();
                    //if (ddlControlingLocator.SelectedItem.Value.ToString() != "All")
                    //    contrLocatorCode = ddlControlingLocator.SelectedItem.Value.ToString();
                    if (ddlType.SelectedItem.Value.ToString() != "All")
                        level = ddlType.SelectedItem.Value.ToString();
                    if (ddlCode.SelectedItem.Value.ToString() != "All")
                        levelCode = ddlCode.SelectedItem.Value.ToString();
                    //if (ddlAWBStatus.SelectedItem.Value.ToString() != "All")
                    //    AWBStatus = ddlAWBStatus.SelectedItem.Value.ToString();
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

                    DateTime dt1;
                    DateTime dt2;
                    dt1 = DateTime.ParseExact(txtFromdate.Text, "dd/MM/yyyy", null);
                    dt2 = DateTime.ParseExact(txtTodate.Text, "dd/MM/yyyy", null);
                    frmDate = dt1.ToString("MM/dd/yyyy");
                    ToDt = dt2.ToString("MM/dd/yyyy");

                    res_Revenue = objReport.GetStockControlDetails(AgentCode, PaymentType, contrLocatorCode, level, levelCode, frmDate, ToDt, AWBStatus);

                    if (res_Revenue != null)
                    {
                        if (res_Revenue.Tables[0].Rows.Count > 0)
                        {
                            Dataset1 = res_Revenue;
                            Dataset2 = showSearchCriteria(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"), AWBStatus, CurrTime);
                            Session["dsAWBStock"] = res_Revenue;
                            AWBStockRptViewer.Visible = true;

                            #region Old RDLX
                            //info = new FileInfo(Server.MapPath("/Reports/rptStockControlReport.rdlx"));
                            //// info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptStockControlReport.rdlx");

                            //// info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptStockControlReport.rdlx");

                            //definition = new ReportDefinition(info);
                            //runtime = new ReportRuntime(definition);
                            //runtime.LocateDataSource += WARCustWise_LocateDataSource;
                            //AWBStockRptViewer.SetReport(runtime);
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

                            AWBStockRptViewer.Visible = true;
                            AWBStockRptViewer.ProcessingMode = ProcessingMode.Local;
                            AWBStockRptViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/AWBStockRpt.rdlc");
                            //Customers dsCustomers = GetData("select top 20 * from customers");
                            ReportDataSource datasource = new ReportDataSource("dsAWBStock_dtAWBStock", res_Revenue.Tables[0]);
                            AWBStockRptViewer.LocalReport.DataSources.Clear();
                            AWBStockRptViewer.LocalReport.DataSources.Add(datasource);
                            AWBStockRptViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                            SaveUserActivityLog("");

                            #endregion
                        }
                        else
                        {
                            AWBStockRptViewer.Visible = false;
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data not avialable for given search criteria";
                            Session["dsAWBStock"] = null;
                            txtFromdate.Focus();
                            SaveUserActivityLog(lblStatus.Text);
                            return;


                        }
                    }
                    else
                    {
                        AWBStockRptViewer.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not avialable for given search criteria";
                        Session["dsAWBStock"] = null;
                        SaveUserActivityLog(lblStatus.Text);
                        txtFromdate.Focus();
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

        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsAWBStock_dtAWBStockSub", Dataset2.Tables[0]));
        }

        #region To show subreport
        //private void WARCustWise_LocateDataSource(object sender, LocateDataSourceEventArgs e)
        //{
        //    string dname = e.DataSetName;
        //    //  if (dname.ToLower() == "DataSetValue")    
        //    if (dname == "dsBGControl")
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

        #region SearchCriteria
        private DataSet showSearchCriteria(string Agent, string PaymentType, string ControlLoc, string level, string levelCode, string FromDate, string Todate, string AWBStatus, string currTime)
        {
            DataSet ds = new DataSet("AWBStock_dsSearchCriteria");
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
                dcNew.ColumnName = "FromDate";
                dtSearch.Columns.Add(dcNew);

                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "ToDate";
                dtSearch.Columns.Add(dcNew);


                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "Level";
                dtSearch.Columns.Add(dcNew);


                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "Code";
                dtSearch.Columns.Add(dcNew);

                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "ReportDate";
                dtSearch.Columns.Add(dcNew);



                DataRow dr;
                dr = dtSearch.NewRow();

                dr["AgentCode"] = Agent; //"5";

                dr["FromDate"] = FromDate;
                dr["ToDate"] = Todate;// "9";
                dr["Level"] = level;
                dr["Code"] = levelCode;
                dr["ReportDate"] = currTime;
                dtSearch.Rows.Add(dr);


                ds.Tables.Add(dtSearch);
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }

        }
        #endregion

        #region Fill Session For DDL
        private void FillSession()
        {
            DataSet Region = new DataSet("AWBStock_dsRegion");
            DataSet City = new DataSet("AWBStock_dsCity");
            DataSet Agent = new DataSet("AWBStock_dsAgnet");

            try
            {

                    Region= objBAL.GetRegionCode();
                Session["Region"] = Region.Tables[0];
                    City= objBAL.GetCityCode();
                Session["City"] = City.Tables[0];
                    Agent= objBAL.GetAgentCode();
                Session["Agent"] = Agent.Tables[0];


            }
            catch (Exception)
            {


            }

            if (Region != null)
            {
                Region.Dispose();
            }
            if (City != null)
            {
                City.Dispose();
            }
            if (Agent != null)
            {
                Agent.Dispose();
            }
        }
        #endregion

        #region dropdown list to select list
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet City = new DataSet("AWBStock_dsCityType");
            DataSet Agent = new DataSet("AWBStock_dsAgentType");
            DataSet Region = new DataSet("AWBStock_dsRegionType");
           
            try
            {

                //if (ddlType.SelectedItem.Value.ToString() == "All")
                //{
                //    ddlCode.DataSource = "";
                //    ddlCode.DataBind();
                //}

                if (ddlType.SelectedItem.Value.ToString() == "All")
                {
                    ddlCode.Items.Clear();
                    ddlCode.Items.Insert(0, "All");
                    ddlCode.SelectedIndex = 0;

                }
                if (ddlType.SelectedItem.Value.ToString() == "Airport")
                {
                     City = da.SelectRecords("spGetAirportCodes");// objBAL.GetCityCode();
                    ddlCode.DataSource = City.Tables[0];
                    ddlCode.DataTextField = "Airport";// "CityCode";
                    ddlCode.DataValueField = "AirportCode";// "CityCode";
                    ddlCode.DataBind();
                    ddlCode.Items.Insert(0,new ListItem("All","All"));
                    ddlCode.SelectedIndex = 0;

                }


                //if (ddlType.SelectedItem.Value.ToString() == "Airport")
                //{
                //    DataSet City = objBAL.GetCityCode();
                //    ddlCode.DataSource = City.Tables[0];
                //    ddlCode.DataTextField = "CityCode";
                //    ddlCode.DataValueField = "CityCode";
                //    ddlCode.DataBind();
                //    ddlCode.Items.Insert(0, "All");
                //    ddlCode.SelectedIndex = 0;

                //}


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

                    Agent = da.SelectRecords("SP_GetAllStationCodeName","level","country",SqlDbType.VarChar);// objBAL.GetCountryCode();
                    ddlCode.DataSource = Agent.Tables[0];
                    ddlCode.DataTextField = "CountryDesc";// "CountryCode";
                    ddlCode.DataValueField = "CountryCode";

                    ddlCode.DataBind();
                    ddlCode.Items.Insert(0, new ListItem("All","All"));
                    ddlCode.SelectedIndex = 0;

                }

            }
            catch (Exception)
            {


            }

            if (Region != null)
            {
                Region.Dispose();
            }
            if (City != null)
            {
                City.Dispose();
            }
            if (Agent != null)
            {
                Agent.Dispose();
            }
            
        }
        #endregion

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = new DataSet("AWBStock_dsExportData");
            DataTable dt = new DataTable("AWBStock_dtExportData");

            try
            {
                Session["dsAWBStock"] = null;
                lblStatus.Text = string.Empty;
                AWBStockRptViewer.Visible = false;

                if ((DataSet)Session["dsAWBStock"] == null)
                    GetData();

                dsExp = (DataSet)Session["dsAWBStock"];
                if (dsExp != null && dsExp.Tables[0].Rows.Count > 0)
                    dt = (DataTable)dsExp.Tables[0];
                else
                {
                    lblStatus.Text = "No records found";
                    lblStatus.ForeColor = Color.Red;
                    Session["dsAWBStock"] = null;
                    lblStatus.Text = string.Empty;
                    AWBStockRptViewer.Visible = false;
                    return;
                }
                if (dt.Columns.Contains("Logo"))
                    dt.Columns.Remove("Logo");
                if (dt.Columns.Contains("AgentCode"))
                    dt.Columns.Remove("AgentCode");
                if (dt.Columns.Contains("Level"))
                    dt.Columns.Remove("Level");
                if (dt.Columns.Contains("Code"))
                    dt.Columns.Remove("Code");
                if (dt.Columns.Contains("FromDate"))
                    dt.Columns.Remove("FromDate");
                if (dt.Columns.Contains("ToDate"))
                    dt.Columns.Remove("ToDate");
                if (dt.Columns.Contains("ALevel"))
                    dt.Columns.Remove("Alevel");



                dt.Columns["AllocatedTo"].ColumnName = "Allocated To";
                dt.Columns["AgentName"].ColumnName = "Agent Name";
                dt.Columns["AFrom"].ColumnName = "AWB From";
                dt.Columns["ATo"].ColumnName = "AWB To";
                dt.Columns["AWBIssued"].ColumnName = "AWB Issued";
                dt.Columns["AWBUtilized"].ColumnName = "AWB Utilized";
                dt.Columns["BalanceAWB"].ColumnName = "Balance AWB";
                


                string attachment = "attachment; filename=AWBStockReport.xls";
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
            // taking all parameters as user selected in report in one variable - "Param"
            string Param = "AgtCode:" + txtAgentCode.Text.ToString() + ",FromDt:" + txtFromdate.Text.Trim() + ",ToDt:" + txtTodate.Text.Trim() + ", Level:" + ddlType.Text.ToString() + ", Loc:" + ddlCode.Text.ToString();

            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "AWB Stock", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
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

        protected void GetData()
        {
            try
            {
                Session["dsAWBStock"] = null;
                lblStatus.Text = string.Empty;
                AWBStockRptViewer.Visible = false;
                //Validate controls
                if (Validate() == false)
                    return;

                DataSet oDs1 = new DataSet("AWBStock_dsoDs1");
                DataSet oDs2 = new DataSet("AWBStock_dsoDs2");
                DataSet res_Revenue = new DataSet("AWBStock_dsGetAWBStock");
                FileInfo info;
                ////ReportRuntime runtime;
                ////ReportDefinition definition;

                try
                {

                    string AgentCode = "All", PaymentType = "All", contrLocatorCode = "All", level = "All", levelCode = "All", frmDate = "", ToDt = "", AWBStatus = "All";
                    if (txtAgentCode.Text.Trim() != "")
                        AgentCode = txtAgentCode.Text.Trim();
                    //if (ddlPaymentType.SelectedItem.Value.ToString() != "All(Without FOC)")
                    //    PaymentType = ddlPaymentType.SelectedItem.Value.ToString();
                    //if (ddlControlingLocator.SelectedItem.Value.ToString() != "All")
                    //    contrLocatorCode = ddlControlingLocator.SelectedItem.Value.ToString();
                    if (ddlType.SelectedItem.Value.ToString() != "All")
                        level = ddlType.SelectedItem.Value.ToString();
                    if (ddlCode.SelectedItem.Value.ToString() != "All")
                        levelCode = ddlCode.SelectedItem.Value.ToString();
                    //if (ddlAWBStatus.SelectedItem.Value.ToString() != "All")
                    //    AWBStatus = ddlAWBStatus.SelectedItem.Value.ToString();
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
                    DateTime dt1;
                    DateTime dt2;
                    dt1 = DateTime.ParseExact(txtFromdate.Text, "dd/MM/yyyy", null);
                    dt2 = DateTime.ParseExact(txtTodate.Text, "dd/MM/yyyy", null);
                    frmDate = dt1.ToString("MM/dd/yyyy");
                    ToDt = dt2.ToString("MM/dd/yyyy");


                    res_Revenue = objReport.GetStockControlDetails(AgentCode, PaymentType, contrLocatorCode, level, levelCode, frmDate, ToDt, AWBStatus);
                    if (res_Revenue != null)
                    {
                        if (res_Revenue.Tables[0].Rows.Count > 0)
                        {
                            Dataset1 = res_Revenue;
                            Dataset2 = showSearchCriteria(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"), AWBStatus, CurrTime);
                            SaveUserActivityLog("");
                            Session["dsAWBStock"] = res_Revenue;
                        }
                        else
                        {
                            AWBStockRptViewer.Visible = false;
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data not avialable for given search criteria";
                            Session["dsAWBStock"] = null;
                            SaveUserActivityLog(lblStatus.Text);
                            txtFromdate.Focus();
                            return;
                        }
                    }
                    else
                    {
                        AWBStockRptViewer.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not avialable for given search criteria";
                        Session["dsAWBStock"] = null;
                        SaveUserActivityLog(lblStatus.Text);
                        txtFromdate.Focus();
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
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtFromdate.Text = txtTodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtAgentCode.Text = string.Empty;
            ddlType.SelectedIndex = 0;
            ddlType_SelectedIndexChanged(null, null);
            AWBStockRptViewer.Visible = false;
        }
    }
}