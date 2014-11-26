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
    public partial class rptBankGuaranteeReport : System.Web.UI.Page
    {
        #region Variable
        private DataSet Dataset1;
        private DataSet Dataset2;
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
                    ReportViewer1.Visible = false;
                    string AgentCode = Convert.ToString(Session["ACode"]);

                    int RoleId = Convert.ToInt32(Session["RoleID"]);

                    if (RoleId == 1 && AgentCode != "")
                    {
                        txtAgentCode.Text = AgentCode;
                        txtAgentCode.Enabled = false;
                    }

                    ////RptViewerBankGuarantee.Visible = false;
                   
                    txtFromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtTodate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                    DateTime dtCurr = Convert.ToDateTime(Session["IT"].ToString());
                    CurrTime = dtCurr.ToString("dd/MM/yyyy hh:mm:ss tt");


                    DataSet City = objBAL.GetCityCode();
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
        }
        #endregion

        #region On Button Click Show Data for search criteria
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                if (Validate() == false)
                {
                    ReportViewer1.Visible = false;
                    return;
                }
                lblStatus.Text = string.Empty;


                DataSet oDs1 = new DataSet("rptBG_oDs1");
                DataSet oDs2 = new DataSet("rptBG_oDs2");
                //FileInfo info;
                //ReportRuntime runtime;
                //ReportDefinition definition;
                DataSet res_Revenue = new DataSet("rptBG_res_Revenue"); ;
                string ErrorLog = string.Empty;

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


                    res_Revenue = objReport.GetBanGuranteeDetails(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom, dtTo, AWBStatus);

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

                    if (res_Revenue != null)
                    {
                        if (res_Revenue.Tables[0].Rows.Count > 0)
                        {
                            Dataset1 = res_Revenue;
                            //Dataset2 = showSearchCriteria(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"), AWBStatus);

                            //RptViewerBankGuarantee.Visible = true;

                            Session["dsExp"] = res_Revenue;


                            //info = new FileInfo(Server.MapPath("/Reports/rptBGControlReport.rdlx"));
                            //// info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptBGControlReport.rdlx");

                            //// info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptBGControlReport.rdlx");

                            //definition = new ReportDefinition(info);
                            //runtime = new ReportRuntime(definition);
                            //runtime.LocateDataSource += WARCustWise_LocateDataSource;
                            //RptViewerBankGuarantee.SetReport(runtime);

                            ReportViewer1.Visible = true;

                            ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptBankGuaranteeReport.rdlc");
                            ReportDataSource datasource = new ReportDataSource("dsrptBankGuaranteeReport_dtrptBankGuaranteeReport", res_Revenue.Tables[0]);
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(datasource);

                            SaveUserActivityLog("");
                        }
                        else
                        {
                           // ReportViewer1.Visible = false;
                            Session["dsExp"] = null;

                            ReportViewer1.Visible = false;
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data not available for given search criteria";
                            txtFromdate.Focus();
                            return;


                        }
                    }
                    else
                       
                    {
                        ReportViewer1.Visible = false;
                        Session["dsExp"] = null;
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
                    if (oDs1 != null)
                    {
                        oDs1 = null;
                    }
                    if (oDs2 != null)
                    {
                        oDs2 = null;
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
            try
            {
                // taking all parameters as user selected in report in one variable - "Param"
                string Param = "AgtCode:" + txtAgentCode.Text.ToString() + ", Valid From:" + txtFromdate.Text.ToString() + ",Valid To:" + txtTodate.Text.ToString();
                objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "Bank Guarantee", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
            }
            catch (Exception ex)
            { }
            finally
            {

                if (objBAL != null)
                {
                    objBAL = null;
                }
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
                string strResult = string.Empty;
                try
                {
                    strResult = rptBAl.GetReportInterval(DateTime.ParseExact(txtFromdate.Text.Trim(), "dd/MM/yyyy", null), DateTime.ParseExact(txtTodate.Text.Trim(), "dd/MM/yyyy", null));
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
        #endregion

        #region SearchCriteria
        private DataSet showSearchCriteria(string Agent, string PaymentType, string ControlLoc, string level, string levelCode, string FromDate, string Todate, string AWBStatus)
        {
            DataSet ds = new DataSet("rptBG_ds");
            DataTable dtSearch = new DataTable("rptBG_dtSearch");
            try
            {
                
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
                    ds.Dispose();
                if (dtSearch != null)
                {
                    dtSearch = null;
                }
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

        public void Getdata()
        {

            try
            {
                Session["dsExp"] = null;
                ReportViewer1.Visible = false;
                lblStatus.Text = "";
                if (Validate() == false)
                {
                    ReportViewer1.Visible = false;
                    Session["dsExp"] = null;
                    return;
                }



                DataSet oDs1_ex = new DataSet("rptBG_oDs1_ex");
                DataSet oDs2_ex = new DataSet("rptBG_oDs2_ex");
                FileInfo info;
                ////ReportRuntime runtime;
                ////ReportDefinition definition;
                DataSet res_Revenue1 = new DataSet("rptBG_res_Revenue1");
                string ErrorLog = string.Empty;

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


                    res_Revenue1 = objReport.GetBanGuranteeDetails(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom, dtTo, AWBStatus);

                    
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

                    if (res_Revenue1 != null)
                    {
                        if (res_Revenue1.Tables[0].Rows.Count > 0)
                        {
                            Dataset1 = res_Revenue1;
                            Session["dsExp"] = res_Revenue1;
                            //Dataset2 = showSearchCriteria(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"), AWBStatus);

                            //RptViewerBankGuarantee.Visible = true;



                            //info = new FileInfo(Server.MapPath("/Reports/rptBGControlReport.rdlx"));
                            //// info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptBGControlReport.rdlx");

                            //// info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptBGControlReport.rdlx");

                            //definition = new ReportDefinition(info);
                            //runtime = new ReportRuntime(definition);
                            //runtime.LocateDataSource += WARCustWise_LocateDataSource;
                            //RptViewerBankGuarantee.SetReport(runtime);



                            //ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            //ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptBankGuaranteeReport.rdlc");
                            //ReportDataSource datasource = new ReportDataSource("dsrptBankGuaranteeReport_dtrptBankGuaranteeReport", res_Revenue.Tables[0]);
                            //ReportViewer1.LocalReport.DataSources.Clear();
                            //ReportViewer1.LocalReport.DataSources.Add(datasource);

                            SaveUserActivityLog("");
                        }
                        else
                        {
                            // ReportViewer1.Visible = false;
                            
                            ReportViewer1.Visible = false;
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data not available for given search criteria";
                            txtFromdate.Focus();
                            return;


                        }
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

        #region dropdown list to select list
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet City = new DataSet("rptBG_city");
            DataSet Region = new DataSet("rptBG_Rfgion");
            DataSet Agent = new DataSet("rptBG_Agent");
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
                    //ddlCode.DataSource = City.Tables[0];
                    //ddlCode.DataTextField = "CityCode";
                    //ddlCode.DataValueField = "CityCode";
                    //ddlCode.DataBind();
                    //ddlCode.Items.Insert(0, "All");
                    //ddlCode.SelectedIndex = 0;

                    ddlCode.DataSource = City;
                    ddlCode.DataMember = City.Tables[0].TableName;
                    ddlCode.DataTextField = City.Tables[0].Columns["Airport"].ColumnName;
                    ddlCode.DataValueField = City.Tables[0].Columns["CityCode"].ColumnName;
                    ddlCode.DataBind();
                    //ddlCode.Items.Insert(0, new ListItem("Select", "0"));
                    ddlCode.Items.Insert(0,"All");
                    ddlCode.SelectedIndex = 0;

                }


                if (ddlType.SelectedItem.Value.ToString() == "Airport")
                {
                    City = objBAL.GetCityCode();

                    //ddlCode.DataSource = City.Tables[0];
                    //ddlCode.DataTextField = "CityCode";
                    //ddlCode.DataValueField = "CityCode";
                    //ddlCode.DataBind();
                    //ddlCode.Items.Insert(0, "All");
                    //ddlCode.SelectedIndex = 0;

                    ddlCode.DataSource = City;
                    ddlCode.DataMember = City.Tables[0].TableName;
                    ddlCode.DataTextField = City.Tables[0].Columns["Airport"].ColumnName;
                    ddlCode.DataValueField = City.Tables[0].Columns["CityCode"].ColumnName;
                    ddlCode.DataBind();
                   // ddlCode.Items.Insert(0, new ListItem("Select", "0"));
                    ddlCode.Items.Insert(0,"All");
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

                    //ddlCode.DataSource = City.Tables[0];
                    //ddlCode.DataTextField = "CityCode";
                    //ddlCode.DataValueField = "CityCode";
                    //ddlCode.DataBind();
                    //ddlCode.Items.Insert(0, "All");
                    //ddlCode.SelectedIndex = 0;

                    ddlCode.DataSource = City;
                    ddlCode.DataMember = City.Tables[0].TableName;
                    ddlCode.DataTextField = City.Tables[0].Columns["Airport"].ColumnName;
                    ddlCode.DataValueField = City.Tables[0].Columns["CityCode"].ColumnName;
                    ddlCode.DataBind();
                    //ddlCode.Items.Insert(0, new ListItem("Select", "0"));
                    ddlCode.Items.Insert(0,"All");
                    ddlCode.SelectedIndex = 0;

                }
                if (ddlType.SelectedItem.Value.ToString() == "Country")
                {

                    Agent = objBAL.GetCountryCode();


                    ddlCode.DataSource = Agent;
                    ddlCode.DataMember = Agent.Tables[0].TableName;
                    ddlCode.DataTextField = Agent.Tables[0].Columns["Country"].ColumnName;
                    ddlCode.DataValueField = Agent.Tables[0].Columns["CountryCode"].ColumnName;
                    ddlCode.DataBind();
                    //ddlCode.Items.Insert(0, new ListItem("Select", "0"));
                    ddlCode.Items.Insert(0,"All");
                    ddlCode.SelectedIndex = 0;

                    //ddlCode.DataSource = Agent.Tables[0];
                    //ddlCode.DataTextField = "CountryCode";
                    //ddlCode.DataValueField = "CountryCode";

                    //ddlCode.DataBind();
                    //ddlCode.Items.Insert(0, "All");
                    //ddlCode.SelectedIndex = 0;

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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = new DataSet("rptBG_dsExp");
            DataTable dt = new DataTable("rptBG_dt");
            lblStatus.Text = string.Empty;
            try
            {
               // if ((DataSet)Session["dsExp"] == null)
                    //if(ds == null)
                    Getdata();


                dsExp = (DataSet)Session["dsExp"];
                if (dsExp != null && dsExp.Tables[0].Rows.Count > 0)
                    dt = (DataTable)dsExp.Tables[0];
                else
                    return;
          
           
                string attachment = "attachment; filename=Report.xls";
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

        #region Button Clear
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            txtAgentCode.Text = string.Empty;
            ddlType.SelectedIndex = 0;
            ddlCode.SelectedIndex = 0;
            txtFromdate.Text = string.Empty;
            txtTodate.Text = string.Empty;
            ReportViewer1.Visible = false;
            ////RptViewerBankGuarantee.Visible = false;
        }

        #endregion


    }
}
