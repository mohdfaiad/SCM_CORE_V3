using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.IO;
using System.Drawing;


namespace ProjectSmartCargoManager
{
    public partial class rptSLAPerformance : System.Web.UI.Page
    {
        public static string CurrTime = "";
        StockAllocationBAL objBAL = new StockAllocationBAL();
        public string AgentCode;
        ReportBAL objReport = new ReportBAL();


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    if (Session["UserName"] == null || Session["UserName"].ToString() == "")
                    {
                        Response.Redirect("~/Login.aspx");
                        return;
                    }

                    if (!IsPostBack)
                    {

                        AgentCode = Convert.ToString(Session["ACode"]);

                        int RoleId = Convert.ToInt32(Session["RoleID"]);

                        if (RoleId == 1 && AgentCode != "")
                        {
                           // txtAgentCode.Text = AgentCode;
                           //txtAgentCode.Enabled = false;
                        }


                        
                        ReportViewer1.Visible = false;
          
                        DateTime dtCurr = Convert.ToDateTime(Session["IT"].ToString());
                        CurrTime = dtCurr.ToString("dd/MM/yyyy hh:mm:ss tt");

                        txtFlightFromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFlightToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        btnExport.Visible = true;
                                  
                        DataSet Station = objBAL.GetStation();
                        ddlStation.DataSource = Station.Tables[0];
                        ddlStation.DataTextField = "StnCode";
                        ddlStation.DataValueField = "StnCode";
                       
                        ddlStation.DataBind();
                        ddlStation.Items.Insert(0, "All");
                        ddlStation.SelectedIndex = 0;
                        if (Station != null)
                        {
                            Station.Dispose();
                        }
                    }

                   
              

                }
            }
            catch (Exception ex)
            {

            }

        }
        public bool Validate()
        {
            try
            {
                try
                {
                    if (txtFlightFromdate.Text.Trim() != "" || txtFlightToDate.Text.Trim() != "")
                    {
                        DateTime dt1 = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                        DateTime dt2 = DateTime.ParseExact(txtFlightToDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                        int chk = DateTime.Compare(dt1, dt2);
                        if (chk > 0)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Please enter valid To date";
                           txtFlightFromdate.Focus();
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                    txtFlightFromdate.Focus();
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
                strResult = objBal.GetReportInterval(DateTime.ParseExact(txtFlightFromdate.Text.Trim(), "dd/MM/yyyy", null), DateTime.ParseExact(txtFlightToDate.Text.Trim(), "dd/MM/yyyy", null));
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
               txtFlightFromdate.Focus();
                return false;
            }

            return true;
        }
       public void Getdata()
        {
            try
            {
                System.IO.MemoryStream Logo = null;

                if (Validate() == false)
                {
                    ReportViewer1.Visible = false;
                    return;
                }
                lblStatus.Text = "";
                DataSet oDs1 = new DataSet();
                DataSet oDs2 = new DataSet();

                string ErrorLog = string.Empty;
                DataTable dt = new DataTable();
                DataSet res_Revenue = new DataSet();

                string StationCode = ddlStation.SelectedValue.ToString();

                string FlightNo = txtFlightNo.Text;

                string FromDt = "", ToDt = "", FlightDt = "";

                DateTime dt1, dt2, dt3;

                if (txtFlightFromdate.Text != "")
                {
                    dt1 = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null);

                    FromDt = dt1.ToString("dd/MM/yyyy");
                }

                if (txtFlightToDate.Text != "")
                {
                    dt2 = DateTime.ParseExact(txtFlightToDate.Text, "dd/MM/yyyy", null);
                    ToDt = dt2.ToString("dd/MM/yyyy");

                }

                if (txtFlightDate.Text != "")
                {
                    dt3 = DateTime.ParseExact(txtFlightDate.Text, "dd/MM/yyyy", null);
                    FlightDt = dt3.ToString("dd/MM/yyyy");


                }

                try
                {


                    res_Revenue = objReport.GetSLAPerformanceDetails(StationCode, FlightNo, FlightDt, FromDt, ToDt);
                  
                    if (res_Revenue != null)
                    {
                        if (res_Revenue.Tables[0].Rows.Count > 0)
                        {
                            Session["dsExp"] = res_Revenue;

                                ReportViewer1.Visible = true;

                            //try
                            //{
                            //    Logo = CommonUtility.GetImageStream(Page.Server);
                              
                            //}
                            //catch (Exception ex)
                            //{
                            //    Logo = new System.IO.MemoryStream();
                            //}

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
                         
                            btnExport.Visible = true;
                            SaveUserActivityLog("");
                        }
                        else
                        {
                            ReportViewer1.Visible = false;
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data not avialable for given search criteria";
                            SaveUserActivityLog(lblStatus.Text);
                            txtFlightFromdate.Focus();
                            return;
                        }
                    }
                    else
                    {
                        ReportViewer1.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not avialable for given search criteria";
                        SaveUserActivityLog(lblStatus.Text);
                        txtFlightFromdate.Focus();
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


        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                System.IO.MemoryStream Logo = null;
               
                if (Validate() == false)
                {
                    ReportViewer1.Visible = false;
                    return;
                }
                lblStatus.Text = "";
                DataSet oDs1 = new DataSet();
                DataSet oDs2 = new DataSet();
             
                string ErrorLog = string.Empty;
                DataTable dt = new DataTable();
                DataSet res_Revenue = new DataSet();

                try
                {
                    string StationCode = ddlStation.SelectedValue.ToString();
                    
                    string FlightNo = txtFlightNo.Text;
                    
                    string FromDt="", ToDt="",FlightDt="";
                                        
                    DateTime dt1, dt2,dt3;
                    
                    if (txtFlightFromdate.Text != "")
                    {
                        dt1 = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null);

                        FromDt = dt1.ToString("dd/MM/yyyy");
                    }

                    if (txtFlightToDate.Text != "")
                    {
                        dt2 = DateTime.ParseExact(txtFlightToDate.Text, "dd/MM/yyyy", null);
                        ToDt = dt2.ToString("dd/MM/yyyy");
                        
                    }

                    if (txtFlightDate.Text != "")
                    {
                        dt3 = DateTime.ParseExact(txtFlightDate.Text, "dd/MM/yyyy", null);
                        FlightDt = dt3.ToString("dd/MM/yyyy");


                    }

                    res_Revenue = objReport.GetSLAPerformanceDetails(StationCode, FlightNo, FlightDt, FromDt, ToDt);
      
                    if (res_Revenue != null)
                    {
                        if (res_Revenue.Tables[0].Rows.Count > 0)
                        {
                            Session["dsExp"] = res_Revenue;
                           
                           // Dataset2 = showSearchCriteria(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"), AWBStatus);

                            ReportViewer1.Visible = true;

                            try
                            {
                                Logo = CommonUtility.GetImageStream(Page.Server);}
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
                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptSLAPerformance.rdlc");

                            ReportDataSource datasource = new ReportDataSource("dsrptSLAPerformance_dsrptSLAPerformance", res_Revenue.Tables[0]);
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(datasource);
                            //ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandlerForSUBrpt);

                            btnExport.Visible = true;
                            SaveUserActivityLog("");
                        }

                        else
                        {
                            ReportViewer1.Visible = false;
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data not avialable for given search criteria";
                            SaveUserActivityLog(lblStatus.Text);
                           txtFlightFromdate.Focus();
                            return;
                        }
                    
                }    else
                    {
                        ReportViewer1.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not avialable for given search criteria";
                        SaveUserActivityLog(lblStatus.Text);
                        txtFlightFromdate.Focus();
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
       

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = null;
            DataTable dt = null;
            lblStatus.Text = "";
            Session["dsExp"] = null;
            try
            {
           
                Getdata();

                dsExp = (DataSet)Session["dsExp"];

                dt = (DataTable)dsExp.Tables[0];
                if (Session["dsExp"] == null && dt == null)
                {
                    lblStatus.Text = "No records found";
                    lblStatus.ForeColor = Color.Red;
                    SaveUserActivityLog(lblStatus.Text);
                    ReportViewer1.Visible = false;
                    return;

                }
                string attachment = "attachment; filename=SLAPerformanceReport.xls";
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
            string Param = "AWBPrefix:" + txtAWBPrefix.Text.ToString() + "AWBNo:" + txtAWBNo.Text.ToString() + ", FromDt:" + txtFlightFromdate.Text.ToString() + ", ToDate:" + txtFlightToDate.Text.ToString() + ",FlightNo:" + txtFlightNo.Text.ToString() + ",Station:" + ddlStation.SelectedValue.ToString();

            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "SLA Performance", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
        }
        protected void btnclear_Click(object sender, EventArgs e)
        {

        }
    }
}
