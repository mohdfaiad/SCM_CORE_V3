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
    public partial class rptAWBNotDeparted: System.Web.UI.Page
    {

        #region Variable
        private DataSet Dataset1;
        private DataSet Dataset2;
        StockAllocationBAL objBAL = new StockAllocationBAL();
        ReportBAL objReport = new ReportBAL();
        BALException objBAL1 = new BALException();
       
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
                    Session["dsAWBNotDeparted"] = null;
                    string AgentCode = Convert.ToString(Session["ACode"]);

                    int RoleId = Convert.ToInt32(Session["RoleID"]);

                    if (RoleId == 1 && AgentCode != "")
                    {
                        txtAgentCode.Text = AgentCode;
                        txtAgentCode.Enabled = false;
                    }

                    AWBNotDepartedRptViewer.Visible = false;

                    txtFromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtTodate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                    DateTime dtCurr = Convert.ToDateTime(Session["IT"].ToString());
                    CurrTime = dtCurr.ToString("dd/MM/yyyy hh:mm:ss tt");
                  
                    GetOrigin();
                    GetDestination();
                   

                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        # region Get Origin List

        private void GetOrigin()
        {
            try
            {
                DataSet ds = objBAL1.GetOriginCodeList(ddlOrigin.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlOrigin.DataSource = ds;
                            ddlOrigin.DataMember = ds.Tables[0].TableName;
                            ddlOrigin.DataValueField = "AirPortCode";//ds.Tables[0].Columns["Code"].ColumnName;

                            ddlOrigin.DataTextField = "AirPort";//ds.Tables[0].Columns["Code"].ColumnName;
                            ddlOrigin.DataBind();
                            ddlOrigin.Items.Insert(0, "ALL");
                            ddlOrigin.Items[0].Value = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        # endregion GetOriginCode List

        # region Get destination List

        private void GetDestination()
        {
            try
            {
                DataSet ds = objBAL1.GetOriginCodeList(ddlDest.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlDest.DataSource = ds;
                            ddlDest.DataMember = ds.Tables[0].TableName;
                            ddlDest.DataValueField = "AirPortCode";//ds.Tables[0].Columns["Code"].ColumnName;

                            ddlDest.DataTextField = "AirPort";// ds.Tables[0].Columns["Code"].ColumnName;
                            ddlDest.DataBind();
                            ddlDest.Items.Insert(0, "ALL");
                            ddlDest.Items[0].Value = "";

                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        # endregion GetDestinationCode List

        #region On Button Click Show Data for search criteria
        protected void btnList_Click(object sender, EventArgs e)
        {
            DataSet oDs1 = new DataSet();
            DataSet oDs2 = new DataSet();
            DataSet res_Revenue = new DataSet();

            try
            {
                if (txtFromdate.Text == "" || txtTodate.Text == "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Enter From Date and To Date ";
                    txtFromdate.Focus();
                    return;
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
                        //TxtFrmDt.Focus();
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                //txtFromdate.Focus();
                return;
            }

            try
            {
                Session["dsAWBNotDeparted"] = null;
                lblStatus.Text = string.Empty;
                AWBNotDepartedRptViewer.Visible = false;
                //Validate controls
                if (Validate() == false)
                {
                    Session["dsAWBNotDeparted"] = null;
                    AWBNotDepartedRptViewer.Visible = false;
                    return;
                }

                try
                {

                    string AgentCode = "All",Origin = "All", Destination = "All",dtFrom="",dtTo="";
                    DateTime FromDate;
                    DateTime ToDate;
                    if (txtAgentCode.Text.Trim() != "")
                        AgentCode = txtAgentCode.Text.Trim();

                    if (ddlOrigin.SelectedItem.Text != "ALL")
                        Origin = ddlOrigin.SelectedValue.ToString();
                    if (ddlDest.SelectedItem.Text != "ALL")
                        Destination = ddlDest.SelectedValue.ToString();

                   
                    if (txtTodate.Text.Trim() != "" && txtFromdate.Text.Trim() != "")
                    {

                        try
                        {
                          FromDate = DateTime.ParseExact(txtFromdate.Text, "dd/MM/yyyy", null);
                          ToDate = DateTime.ParseExact(txtTodate.Text, "dd/MM/yyyy", null);
                          dtFrom = FromDate.ToString("MM/dd/yyyy");
                          dtTo = ToDate.ToString("MM/dd/yyyy");


                        }
                        catch(Exception ex)
                        {
                        }
                           
                     
                    }

            

                res_Revenue = objReport.GetAWBNotDeparted(Origin, Destination, AgentCode, dtFrom, dtTo);
                    if (res_Revenue != null)
                    {
                        if (res_Revenue.Tables[0].Rows.Count > 0)
                        {
                            Dataset1 = res_Revenue;
                            Dataset2 = showSearchCriteria(AgentCode,txtFromdate.Text,txtTodate.Text, Origin, Destination);
                            Session["dsAWBNotDeparted"] = res_Revenue;
                            AWBNotDepartedRptViewer.Visible = true;

                            #region Old RDLX
                            //info = new FileInfo(Server.MapPath("/Reports/rptStockControlReport.rdlx"));
                            //// info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptStockControlReport.rdlx");

                            //// info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptStockControlReport.rdlx");

                            //definition = new ReportDefinition(info);
                            //runtime = new ReportRuntime(definition);
                            //runtime.LocateDataSource += WARCustWise_LocateDataSource;
                            //AWBNotDepartedRptViewer.SetReport(runtime);
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

                            AWBNotDepartedRptViewer.Visible = true;
                            AWBNotDepartedRptViewer.ProcessingMode = ProcessingMode.Local;
                            AWBNotDepartedRptViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/rptAWBNotDeparted.rdlc");
                            //Customers dsCustomers = GetData("select top 20 * from customers");
                            ReportDataSource datasource = new ReportDataSource("dsrptAWBNotDeparted_dsrptAWBNotDeparted", res_Revenue.Tables[0]);
                            AWBNotDepartedRptViewer.LocalReport.DataSources.Clear();
                            AWBNotDepartedRptViewer.LocalReport.DataSources.Add(datasource);
                           AWBNotDepartedRptViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                            SaveUserActivityLog("");

                            #endregion
                        }
                        else
                        {
                            AWBNotDepartedRptViewer.Visible = false;
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data not avialable for given search criteria";
                            Session["dsAWBNotDeparted"] = null;
                            txtFromdate.Focus();
                            SaveUserActivityLog(lblStatus.Text);
                            return;


                        }
                    }
                    else
                    {
                        AWBNotDepartedRptViewer.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not avialable for given search criteria";
                        Session["dsAWBNotDeparted"] = null;
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
            e.DataSources.Add(new ReportDataSource("dsrptAWBNotDeparted_dsrptAWBNotDeparted_SUB", Dataset2.Tables[0]));
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
        private DataSet showSearchCriteria(string Agent,string FromDate, string Todate, string Origin, string Destination)
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
                dcNew.ColumnName = "FromDate";
                dtSearch.Columns.Add(dcNew);

                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "ToDate";
                dtSearch.Columns.Add(dcNew);


                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "Origin";
                dtSearch.Columns.Add(dcNew);


                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "Destination";
                dtSearch.Columns.Add(dcNew);

              

                DataRow dr;
                dr = dtSearch.NewRow();

                dr["AgentCode"] = Agent; //"5";

                dr["FromDate"] = FromDate;
                dr["ToDate"] = Todate;// "9";
                dr["Origin"] = Origin;
                dr["Destination"] = Destination;
               
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
            try
            {

                DataSet Region = objBAL.GetRegionCode();
                Session["Region"] = Region.Tables[0];
                DataSet City = objBAL.GetCityCode();
                Session["City"] = City.Tables[0];
                DataSet Agent = objBAL.GetAgentCode();
                Session["Agent"] = Agent.Tables[0];

            }
            catch (Exception)
            {


            }
        }
        #endregion

     
        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = null;
            DataTable dt = null;

            try
            {
                Session["dsAWBNotDeparted"] = null;
                lblStatus.Text = string.Empty;
                AWBNotDepartedRptViewer.Visible = false;

                if ((DataSet)Session["dsAWBNotDeparted"] == null)
                    GetData();

                dsExp = (DataSet)Session["dsAWBNotDeparted"];
                if (dsExp != null && dsExp.Tables[0].Rows.Count > 0)
                    dt = (DataTable)dsExp.Tables[0];
                else
                {
                    lblStatus.Text = "No records found";
                    lblStatus.ForeColor = Color.Red;
                    Session["dsAWBNotDeparted"] = null;
                    lblStatus.Text = string.Empty;
                    AWBNotDepartedRptViewer.Visible = false;
                    return;
                }
             
             

                string attachment = "attachment; filename=AWBNotDeparted.xls";
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
            string Param = "AgtCode:" + txtAgentCode.Text.ToString() + ",FromDt:" + txtFromdate.Text.Trim() + ",ToDt:" + txtTodate.Text.Trim() ;

            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "AWB Not Departed", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
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
            DataSet oDs1 = new DataSet();
            DataSet oDs2 = new DataSet();
            DataSet res_Revenue = new DataSet();

            try
            {
                if (txtFromdate.Text == "" || txtTodate.Text == "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Enter From Date and To Date ";
                    txtFromdate.Focus();
                    return;
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
                        //TxtFrmDt.Focus();
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                //txtFromdate.Focus();
                return;
            }

            try
            {
                Session["dsAWBNotDeparted"] = null;
                lblStatus.Text = string.Empty;
                AWBNotDepartedRptViewer.Visible = false;
                //Validate controls
                if (Validate() == false)
                {
                    Session["dsAWBNotDeparted"] = null;
                    AWBNotDepartedRptViewer.Visible = false;
                    return;
                }

                try
                {

                    string AgentCode = "All", Origin = "All", Destination = "All", dtFrom = "", dtTo = "";
                    DateTime FromDate;
                    DateTime ToDate;
                    if (txtAgentCode.Text.Trim() != "")
                        AgentCode = txtAgentCode.Text.Trim();

                    if (ddlOrigin.SelectedItem.Text != "ALL")
                        Origin = ddlOrigin.SelectedValue.ToString();
                    if (ddlDest.SelectedItem.Text != "ALL")
                        Destination = ddlDest.SelectedValue.ToString();


                    if (txtTodate.Text.Trim() != "" && txtFromdate.Text.Trim() != "")
                    {

                        try
                        {
                            FromDate = DateTime.ParseExact(txtFromdate.Text, "dd/MM/yyyy", null);
                            ToDate = DateTime.ParseExact(txtTodate.Text, "dd/MM/yyyy", null);
                            dtFrom = FromDate.ToString("MM/dd/yyyy");
                            dtTo = ToDate.ToString("MM/dd/yyyy");


                        }
                        catch (Exception ex)
                        {
                        }


                    }

                    res_Revenue = objReport.GetAWBNotDeparted(Origin, Destination, AgentCode, dtFrom, dtTo);
                    if (res_Revenue != null)
                    {
                        if (res_Revenue.Tables[0].Rows.Count > 0)
                        {
                            Dataset1 = res_Revenue;
                            // Dataset2 = showSearchCriteria(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"), AWBStatus, CurrTime);
                            Session["dsAWBNotDeparted"] = res_Revenue;
                            AWBNotDepartedRptViewer.Visible = true;

                          
                            #region RDLC

                       

                           // AWBNotDepartedRptViewer.Visible = true;
                           // AWBNotDepartedRptViewer.ProcessingMode = ProcessingMode.Local;
                            //AWBNotDepartedRptViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/rptAWBNotDeparted.rdlc");
                            //Customers dsCustomers = GetData("select top 20 * from customers");
                           // ReportDataSource datasource = new ReportDataSource("dsrptAWBNotDeparted_dsrptAWBNotDeparted", res_Revenue.Tables[0]);
                           // AWBNotDepartedRptViewer.LocalReport.DataSources.Clear();
                           // AWBNotDepartedRptViewer.LocalReport.DataSources.Add(datasource);
                            //  AWBNotDepartedRptViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                            SaveUserActivityLog("");

                            #endregion
                        }
                        else
                        {
                            AWBNotDepartedRptViewer.Visible = false;
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data not avialable for given search criteria";
                            Session["dsAWBNotDeparted"] = null;
                            txtFromdate.Focus();
                            SaveUserActivityLog(lblStatus.Text);
                            return;


                        }
                    }
                    else
                    {
                        AWBNotDepartedRptViewer.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not avialable for given search criteria";
                        Session["dsAWBNotDeparted"] = null;
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


        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/rptAWBNotDeparted.aspx");
        }
    }
}