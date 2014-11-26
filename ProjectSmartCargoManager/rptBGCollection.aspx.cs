using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;

using Microsoft.Reporting.WebForms;

namespace ProjectSmartCargoManager
{
    public partial class rptBGCollection : System.Web.UI.Page
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
                    ReportViewer1.Visible = false;
                    txtFromDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtToDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    btnExport.Visible = true;
                    DateTime dtCurr = Convert.ToDateTime(Session["IT"].ToString());
                    CurrTime = dtCurr.ToString("dd/MM/yyyy hh:mm:ss tt");
                    string AgentCode = Convert.ToString(Session["ACode"]);
                    ////WebReportViewer1.Visible = false;

                    if (AgentCode != "")
                    {
                        txtAgentCode.Text = AgentCode;
                        txtAgentCode.ReadOnly = true;
                    }
                    if (Session["awbPrefix"] != null)
                        txtAwbPrefix.Text = Session["awbPrefix"].ToString();
                    else
                    {
                        MasterBAL objBal = new MasterBAL();
                        Session["awbPrefix"] = objBal.awbPrefix();
                        txtAwbPrefix.Text = Session["awbPrefix"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Button List Click Operation
        protected void btnList_Click(object sender, EventArgs e)
        {
            DataSet dsRptData = null;
            string ErrorLog = string.Empty;
            Session["dsExp"] = null;
            lblStatus.Text = string.Empty;
            ReportViewer1.Visible = false;
            //FileInfo info;
            //ReportRuntime runtime;
            //ReportDefinition definition;
            try
            {
                if (Validate() == false)
                {
                    ReportViewer1.Visible = false;
                    return;
                }

                    DateTime dt1 = DateTime.ParseExact(txtFromDt.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                    DateTime dt2 = DateTime.ParseExact(txtToDt.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                    dsRptData = objBLL.dsGetBGCollectionMasterDetails(txtAgentCode.Text, dt1, dt2, txtAWBNo.Text.Trim(), ddlTransactionType.SelectedItem.Text.Trim());

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



                   
                    if (dsRptData != null)
                    {
                        if (dsRptData.Tables.Count > 0)
                        {
                            if (dsRptData.Tables[0].Rows.Count > 0)
                            {
                                Session["dsExp"] = dsRptData;
                                lblStatus.Text = "";
                                Dataset1 = dsRptData;
                                //Dataset2 = showSearchCriteria(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"), AWBStatus);

                                ////  WebReportViewer1.Visible = true;



                                //  info = new FileInfo(Server.MapPath("/Reports/rptBGCollection.rdlx"));
                                //  // info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptAgentWiseTonnageReport.rdlx");

                                //  // info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "Report1.rdlx");

                                //  definition = new ReportDefinition(info);
                                //  runtime = new ReportRuntime(definition);
                                //  runtime.LocateDataSource += WARCustWise_LocateDataSource;
                                //  WebReportViewer1.SetReport(runtime);
                                //  btnExport.Visible = true;



                                //  oDs2 = LoadParamDataset();// just temperory used, not provide real value                 Dataset2 = oDs2;                  runtime.LocateDataSource += WARCustWise_LocateDataSource;                 WARCustWise.SetReport(runtime);                 pnlMsg.Visible = false;              }             else             {                 oDs1 = null;                 WARCustWise.Visible = false;                 // lblMsg.Text = "No Data Available";                 pnlMsg.Visible = true;                 lblMsgBox.Text = "No Data Available";                 //Pic.Visible = true;               }
                                ReportViewer1.Visible = true;
                                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptBGCollection.rdlc");
                                ReportDataSource datasource = new ReportDataSource("dsrptBGCollection_dtrptBGCollection", dsRptData.Tables[0]);
                                ReportViewer1.LocalReport.DataSources.Clear();
                                ReportViewer1.LocalReport.DataSources.Add(datasource);
                                SaveUserActivityLog("");
                            }
                            else
                            {
                                lblStatus.Text = "No Records Found";
                                ReportViewer1.Visible = false;
                                lblStatus.ForeColor = Color.Red;
                                return;
                            }
                        }
                        else
                        {
                            lblStatus.Text = "No Records Found";
                            ReportViewer1.Visible = false;
                            lblStatus.ForeColor = Color.Red;
                            return;

                        }
                    }
                    else
                    {
                        lblStatus.Text = "No Records Found";
                        ReportViewer1.Visible = false;
                        lblStatus.ForeColor = Color.Red;
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
                if (dsRptData != null)
                    dsRptData.Dispose();
                if (Dataset1 != null)
                    Dataset1.Dispose();
                if (Dataset2 != null)
                    Dataset2.Dispose();
            }
        }
        #endregion

        #region Validate Controls
        private bool Validate()
        {
                try
                {
                    if (txtFromDt.Text.Trim() != "" || txtToDt.Text.Trim() != "")
                    {
                        DateTime dt1 = DateTime.ParseExact(txtFromDt.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                        DateTime dt2 = DateTime.ParseExact(txtToDt.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                        int chk = DateTime.Compare(dt1, dt2);
                        if (chk > 0)
                        {
                            lblStatus.ForeColor = Color.Red;
                            ReportViewer1.Visible = false;
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

                    return true;
                }
                catch (Exception ex)
                {
                    ReportViewer1.Visible = false;
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                    txtFromDt.Focus();
                    return false;
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
        //       // e.Data = Dataset2;
        //    }
        //}
        #endregion

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetAgentCode(string prefixText, int count)
        {

            string[] orgdest = new ConBooking_GHA().GetOrgDest();

            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string");
            SqlDataAdapter dad = new SqlDataAdapter("SELECT AgentCode from dbo.AgentMaster where (AgentName like '" + prefixText + "%' or AgentCode like '" + prefixText + "%') and Station='" + orgdest[0] + "'", con);
            DataSet ds = new DataSet();
            dad.Fill(ds);
            List<string> list = new List<string>(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(dr[0].ToString());

            }

            return list.ToArray();
        }
        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            // taking all parameters as user selected in report in one variable - "Param"
            string Param = "AgtCode:" + txtAgentCode.Text.ToString() + ", Valid From:" + txtFromDt.Text.ToString() + ",Valid To:" + txtToDt.Text.ToString() + ",AWB Number:" + txtAWBNo.Text.ToString() + "Transaction Type:," + ddlTransactionType.Text.ToString();
            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "BG Collection", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
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

        public void Getdata()
        {
            DataSet dsRptData = null;
            string ErrorLog = string.Empty;
            ReportViewer1.Visible = false;
            lblStatus.Text = string.Empty;
           
            Session["dsExp"] = null;
            try
            {
                if (Validate() == false)
                {
                    
                    return;
                }
                    DateTime dt1 = DateTime.ParseExact(txtFromDt.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                    DateTime dt2 = DateTime.ParseExact(txtToDt.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                    dsRptData = objBLL.dsGetBGCollectionMasterDetails(txtAgentCode.Text, dt1, dt2, txtAWBNo.Text.Trim(), ddlTransactionType.SelectedItem.Text.Trim());

                                
                    if (dsRptData != null)
                    {
                        if (dsRptData.Tables.Count > 0)
                        {
                            if (dsRptData.Tables[0].Rows.Count > 0)
                            {
                                lblStatus.Text = "";
                                Dataset1 = dsRptData;
                                //Dataset2 = showSearchCriteria(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"), AWBStatus);

                                ////  WebReportViewer1.Visible = true;


                                Session["dsExp"] = dsRptData;
                                //  info = new FileInfo(Server.MapPath("/Reports/rptBGCollection.rdlx"));
                                //  // info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptAgentWiseTonnageReport.rdlx");

                                //  // info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "Report1.rdlx");

                                //  definition = new ReportDefinition(info);
                                //  runtime = new ReportRuntime(definition);
                                //  runtime.LocateDataSource += WARCustWise_LocateDataSource;
                                //  WebReportViewer1.SetReport(runtime);
                                //  btnExport.Visible = true;



                                //  oDs2 = LoadParamDataset();// just temperory used, not provide real value                 Dataset2 = oDs2;                  runtime.LocateDataSource += WARCustWise_LocateDataSource;                 WARCustWise.SetReport(runtime);                 pnlMsg.Visible = false;              }             else             {                 oDs1 = null;                 WARCustWise.Visible = false;                 // lblMsg.Text = "No Data Available";                 pnlMsg.Visible = true;                 lblMsgBox.Text = "No Data Available";                 //Pic.Visible = true;               }

                                //ReportViewer1.ProcessingMode = ProcessingMode.Local;
                                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptBGCollection.rdlc");
                                //ReportDataSource datasource = new ReportDataSource("dsrptBGCollection_dtrptBGCollection", dsRptData.Tables[0]);
                                //ReportViewer1.LocalReport.DataSources.Clear();
                                //ReportViewer1.LocalReport.DataSources.Add(datasource);
                                SaveUserActivityLog("");
                            }
                            else
                            {
                                lblStatus.Text = "No Records Found";
                                ReportViewer1.Visible = false;
                                lblStatus.ForeColor = Color.Red;
                                return;
                            }
                        }
                        else
                        {
                            lblStatus.Text = "No Records Found";
                            ReportViewer1.Visible = false;
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                    }
                    else
                    {
                        lblStatus.Text = "No Records Found";
                        ReportViewer1.Visible = false;
                        lblStatus.ForeColor = Color.Red;
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
                if (dsRptData != null)
                    dsRptData.Dispose();
                if (Dataset1 != null)
                    Dataset1.Dispose();
                if (Dataset2 != null)
                    Dataset2.Dispose();
            }

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = null;
            DataTable dt = null;
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

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtAgentCode.Text = "";
            txtAWBNo.Text = "";

        }

        
    } 
}

