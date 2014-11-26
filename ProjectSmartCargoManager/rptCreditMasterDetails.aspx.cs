using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.Data;
using System.Drawing;
using System.IO;

using Microsoft.Reporting.WebForms;

namespace ProjectSmartCargoManager
{
    public partial class rptCreditMasterDetails : System.Web.UI.Page
    {
        #region Variable
        AgentBAL objBLL = new AgentBAL();
        public static string CurrTime = "";
        private DataSet Dataset1;
        private DataSet Dataset2;
        ReportBAL rptBAl = new ReportBAL();
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    txtFromDt.Text = Convert.ToDateTime(Session["IT"].ToString()).ToString("dd/MM/yyyy");
                    txtToDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    DateTime dtCurr = Convert.ToDateTime(Session["IT"].ToString());
                    CurrTime = dtCurr.ToString("dd/MM/yyyy hh:mm:ss tt");
                    string AgentCode = Convert.ToString(Session["ACode"]);
                    // WebReportViewer1.Visible = false;
                    Session["dsAWBOC"] = null;
                    ReportViewer1.Visible = false;
                    //if (AgentCode != "")
                    //{
                    //    txtAgentCode.Text = AgentCode;
                    //    txtAgentCode.ReadOnly = true;
                    //}
                }
            }
            catch (Exception ex)
            {

            }

        }
        #endregion

        #region btnList Click
        protected void btnList_Click(object sender, EventArgs e)
        {
            DataSet dsRptData = new DataSet();
            ReportDataSource datasource = new ReportDataSource();
            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();
            try
            {
                ReportViewer1.Visible = false;
                lblStatus.Text = "";

                //FileInfo info;
                //ReportRuntime runtime;
                //ReportDefinition definition;
                if (Validate() == false)
                    return;
                Session["dsBGColl"] = null;
                if (Validate() == true)
                {

                    dt1 = DateTime.ParseExact(txtFromDt.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                   dt2 = DateTime.ParseExact(txtToDt.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                    dsRptData = objBLL.dsGetCreditMasterDetails(txtAgentCode.Text.Trim(), txtFromDt.Text.Trim(), txtToDt.Text.Trim());

                    if (dsRptData != null)
                    {
                        if (dsRptData.Tables.Count > 0)
                        {
                            if (dsRptData.Tables[0].Rows.Count > 0)
                            {
                                Session["dsBGColl"] = dsRptData;
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

                                if (dsRptData.Tables[0].Columns.Contains("Logo") == false)
                                {
                                    DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                                    col1.DefaultValue = Logo.ToArray();
                                    dsRptData.Tables[0].Columns.Add(col1);
                                }
                                lblStatus.Text = "";
                                Dataset1 = dsRptData;
                                //Dataset2 = showSearchCriteria(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"), AWBStatus);

                                #region old  rdlx
                                //WebReportViewer1.Visible = true;



                                //info = new FileInfo(Server.MapPath("/Reports/rptCreditMasterDetails.rdlx"));
                                //// info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptAgentWiseTonnageReport.rdlx");

                                //// info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "Report1.rdlx");

                                //definition = new ReportDefinition(info);
                                //runtime = new ReportRuntime(definition);
                                //runtime.LocateDataSource += WARCustWise_LocateDataSource;
                                //WebReportViewer1.SetReport(runtime);

                                ////  oDs2 = LoadParamDataset();// just temperory used, not provide real value                 Dataset2 = oDs2;                  runtime.LocateDataSource += WARCustWise_LocateDataSource;                 WARCustWise.SetReport(runtime);                 pnlMsg.Visible = false;              }             else             {                 oDs1 = null;                 WARCustWise.Visible = false;                 // lblMsg.Text = "No Data Available";                 pnlMsg.Visible = true;                 lblMsgBox.Text = "No Data Available";                 //Pic.Visible = true;               }
                                #endregion

                                //new coed for rdlc report
                                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptCreditMasterDetails.rdlc");
                                //Customers dsCustomers = GetData("select top 20 * from customers");
                                datasource = new ReportDataSource("dsBGDetails_dtBGDetails", dsRptData.Tables[0]);

                                ReportViewer1.LocalReport.DataSources.Clear();
                                ReportViewer1.LocalReport.DataSources.Add(datasource);

                                SaveUserActivityLog("");
                                ReportViewer1.Visible = true;



                            }
                            else
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "No Records Found";
                                ReportViewer1.Visible = false;
                                SaveUserActivityLog(lblStatus.Text);
                               
                               
                            }
                        }
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                SaveUserActivityLog(ex.Message);
            }
            finally
            {
                if (dsRptData != null)
                {
                    dsRptData.Dispose();
                
                }
              
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
        private bool Validate()
        {
            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();
            try
            {
                if (txtFromDt.Text.Trim() != "" || txtToDt.Text.Trim() != "")
                {
                    dt1 = DateTime.ParseExact(txtFromDt.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                    dt2 = DateTime.ParseExact(txtToDt.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                    int chk = DateTime.Compare(dt1, dt2);
                    if (chk > 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid To date";
                        txtFromDt.Focus();
                        return false;
                    }

                }

                string strResult = string.Empty;
                try
                {
                    strResult = rptBAl.GetReportInterval(DateTime.ParseExact(txtFromDt.Text.Trim(), "dd/MM/yyyy", null), DateTime.ParseExact(txtToDt.Text.Trim(), "dd/MM/yyyy", null));
                }
                catch
                {
                    strResult = "";
                }
                if (strResult != "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = strResult;
                    txtFromDt.Focus();
                    return false;
                }



            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                txtFromDt.Focus();
                return false;
            }
            
            return true;
        }
        #endregion

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = null;
            DataTable dt = null;
            Session["dsBGColl"] = null;

            try
            {
                if ((DataSet)Session["dsBGColl"] == null)
                    GetData();

                dsExp = (DataSet)Session["dsBGColl"];
                //dsExp = ds;
                dt = (DataTable)dsExp.Tables[0];

                if (Session["dsBGColl"] == null && dt == null)
                {
                    lblStatus.Text = "No records found";
                    lblStatus.ForeColor = Color.Red;
                    SaveUserActivityLog(lblStatus.Text);
                    ReportViewer1.Visible = false;
                    return;
                }
                if (dt.Columns.Contains("Logo"))
                { dt.Columns.Remove("Logo"); }
                lblStatus.Text = "";
                string attachment = "attachment; filename=Report.xls";
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
            string Param = "AgtCode:" + txtAgentCode.Text.ToString() + ", Valid From:" + txtFromDt.Text.ToString() + ",Valid To:" + txtToDt.Text.ToString();
            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "BG Detail", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
        }

        public string GetReportInterval(DateTime FromDate, DateTime ToDate)
        {
            double DaysConfigured = 0;
            double intReportInterval = (ToDate - FromDate).TotalDays + 1;
            LoginBL objBL = new LoginBL();


            try
            {
                Session["dsBGColl"] = null;
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
            {
                ReportViewer1.Visible = false;
                return "Report can be generated only for " + DaysConfigured.ToString() + " days.";
            }
            else
                return "";
        }

        protected void GetData()
        {
            DataSet dsRptData = new DataSet();
            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();
            try
            {
                ReportViewer1.Visible = false;
                Session["dsBGColl"] = null;
                if (Validate() == false)
                {
                    return;

                }

                if (Validate() == true)
                {
                    Session["dsBGColl"] = null;

                     dt1 = DateTime.ParseExact(txtFromDt.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                    dt2 = DateTime.ParseExact(txtToDt.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                    dsRptData = objBLL.dsGetCreditMasterDetails(txtAgentCode.Text.Trim(), txtFromDt.Text, txtToDt.Text);

                    if (dsRptData != null)
                    {
                        if (dsRptData.Tables.Count > 0)
                        {
                            if (dsRptData.Tables[0].Rows.Count > 0)
                            {
                                Session["dsBGColl"] = dsRptData;
                                lblStatus.Text = "";
                                Dataset1 = dsRptData;
                                SaveUserActivityLog("");
                                ReportViewer1.Visible = false;


                            }
                            else
                            {
                                lblStatus.Text = "No Records Found";
                                SaveUserActivityLog(lblStatus.Text);
                                SaveUserActivityLog(lblStatus.Text);
                               
                                lblStatus.ForeColor = Color.Red;
                                ReportViewer1.Visible = false;
                            }
                        }
                    }
                }

                else
                {
                    return;
                }
            }

            catch (Exception ex)
            { SaveUserActivityLog(ex.Message); }

            finally
            {
                if (dsRptData != null)

                {
                    dsRptData.Dispose();
                }
            }


        }


        #region Button Clear
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/rptCreditMasterDetails.aspx");
              
            }
            catch (Exception ex)
            {
               
            }
        }

        #endregion

    }

}