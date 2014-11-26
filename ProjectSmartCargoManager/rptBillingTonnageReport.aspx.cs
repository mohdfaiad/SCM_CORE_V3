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
////
using System.IO;
using System.Drawing;

namespace ProjectSmartCargoManager
{
    public partial class rptBillingTonnageReport : System.Web.UI.Page
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



                     ////WebReportViewer1.Visible = false;

                    txtFromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtTodate.Text = DateTime.Now.ToString("dd/MM/yyyy");




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
                Validate();
                lblStatus.Text = "";


                DataSet oDs1 = new DataSet();
                DataSet oDs2 = new DataSet();
                FileInfo info;
                ////ReportRuntime runtime;
                ////ReportDefinition definition;

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


                    DataSet res_Revenue = objReport.GetBillingTonnageDetails(dtfrom, dtTo);


                    if (res_Revenue != null)
                    {
                        if (res_Revenue.Tables[0].Rows.Count > 0)
                        {
                            Dataset1 = res_Revenue;
                            Dataset2 = showSearchCriteria(dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"), FlightStatus);

                             ////WebReportViewer1.Visible = true;


                            info = new FileInfo(Server.MapPath("/Reports/rptBillingTonnage.rdlx"));
                            // info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptBGControlReport.rdlx");

                            // info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptBGControlReport.rdlx");

                            ////definition = new ReportDefinition(info);
                            ////runtime = new ReportRuntime(definition);
                            ////runtime.LocateDataSource += WARCustWise_LocateDataSource;
                            ////WebReportViewer1.SetReport(runtime);

                        }
                        else
                        {
                             ////WebReportViewer1.Visible = false;
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data not avialable for given search criteria";
                            txtFromdate.Focus();
                            return;


                        }
                    }
                    else
                    {
                         ////WebReportViewer1.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not avialable for given search criteria";
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
        }
        #endregion

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
        private void Validate()
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
                            txtFromdate.Focus();
                            return;
                        }


                    }
                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                    txtFromdate.Focus();
                    return;
                }




            }
            catch (Exception ex)
            {


            }

        }
        #endregion

        #region SearchCriteria
        private DataSet showSearchCriteria(string FromDate, string Todate, string FlightStatus)
        {
            DataSet ds = new DataSet();
            try
            {
                DataTable dtSearch = new DataTable();
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
                dcNew.ColumnName = "ReportTime";
                dtSearch.Columns.Add(dcNew);



                DataRow dr;
                dr = dtSearch.NewRow();

                dr["FrmDate"] = FromDate;
                dr["Todate"] = Todate;// "9";

               
                dr["ReportTime"] = CurrTime;
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

    }
}
