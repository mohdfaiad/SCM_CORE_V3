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
    public partial class rptBillingDCMDetails : System.Web.UI.Page
    {
        #region Variable
        private DataSet Dataset1;
        private DataSet Dataset2;
        ReportBAL objReport = new ReportBAL();
        public static string CurrTime = "";
        string strfromdate, strtodate;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //RptViewerRevenue_Station.Visible = false;
                    btnExport.Visible = true;
                    lblStatus.Text = string.Empty;
                    ReportViewer1.Visible = false;
                }
            }
            catch (Exception ex)
            {
   
            }

        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            DataSet res = new DataSet("DCM_res");
            Dataset1 = new DataSet("Table");
            try
            {
                //RptViewerRevenue_Station.Visible = false;
                lblStatus.Text = string.Empty;

                // FileInfo info;
                // ReportRuntime runtime;
                // ReportDefinition definition;

                if (Validate() == false)
                {
                    ReportViewer1.Visible = false;
                    return;
                }
                lblStatus.Text = "";
                // btnExport.Visible = false;

                res = objReport.GetBillingDCMData(Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(strtodate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"));

                if (res != null)
                {
                    if (res.Tables[0].Rows.Count > 0)
                    {
                        // RptViewerRevenue_Station.Visible = true;
                        lblStatus.Text = string.Empty;
                        //Dataset1 = res;
                        ReportViewer1.Visible = true;
                        /////////////Code to edit result dataset (hiding duplicate values for AWB)////////////////////
                        DataTable dtRes = res.Tables[0];
                        DataTable dtNew = editDatasetValuesNew(dtRes);
                        Session["dsListDCM"] = dtNew;
                        btnExport.Visible = true;


                        //Dataset1 = new DataSet("Table");
                        Dataset1.Tables.Add(dtNew);
                        //////////////////////////////////////////////////////////////////////////////////////////////

                        //RptViewerRevenue_Station.Visible = true;

                        //info = new FileInfo(Server.MapPath("/Reports/rptBillingDCMDetails.rdlx"));
                        ////info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptStationWiseTonnageReport.rdlx");

                        //// info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "Report1.rdlx");

                        //definition = new ReportDefinition(info);
                        //runtime = new ReportRuntime(definition);
                        //runtime.LocateDataSource += WARCustWise_LocateDataSource;
                        //RptViewerRevenue_Station.SetReport(runtime);
                        //  oDs2 = LoadParamDataset();// just temperory used, not provide real value                 Dataset2 = oDs2;                  runtime.LocateDataSource += WARCustWise_LocateDataSource;                 WARCustWise.SetReport(runtime);                 pnlMsg.Visible = false;              }             else             {                 oDs1 = null;                 WARCustWise.Visible = false;                 // lblMsg.Text = "No Data Available";                 pnlMsg.Visible = true;                 lblMsgBox.Text = "No Data Available";                 //Pic.Visible = true;               }

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

                        if (res.Tables[0].Columns.Contains("Logo") == false)
                        {
                            DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                            col1.DefaultValue = Logo.ToArray();
                            res.Tables[0].Columns.Add(col1);
                        }
                        //RptViewerRevenue_Station.Visible = false;
                        ReportViewer1.ProcessingMode = ProcessingMode.Local;
                        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptBillingDCMDetails.rdlc");
                        ReportDataSource datasource = new ReportDataSource("dsrptBillingDCMDetails_dtrptBillingDCMDetails", res.Tables[0]);
                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(datasource);
                        SaveUserActivityLog("");

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
                else
                {
                    ReportViewer1.Visible = false;
                    //RptViewerRevenue_Station.Visible = false;
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Data not available for given search criteria";
                    txtFromdate.Focus();
                    return;

                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                res = null;
                Dataset1 = null;
            }
        }

        protected DataTable editDatasetValues(DataTable DSRes)
        {
            try
            {
                string OldAWBNo = "", NewAWBNo = "";
                int FirstLoop = 0;
                for (int y = 0; y < DSRes.Rows.Count; y++)
                {
                    if (FirstLoop == 0)
                        OldAWBNo = DSRes.Rows[0]["AWBNumber"].ToString();
                    else
                        OldAWBNo = DSRes.Rows[y - 1]["AWBNumber"].ToString();

                    if (DSRes.Rows.Count > y)
                    {
                        NewAWBNo = DSRes.Rows[y]["AWBNumber"].ToString();
                    }
                    
                    if (NewAWBNo == OldAWBNo && y != 0)
                    {
                        DSRes.Rows[y]["OCDueCar"] = DBNull.Value;
                        DSRes.Rows[y]["OCDueAgent"] = DBNull.Value;
                        DSRes.Rows[y]["CommissionAmt"] = DBNull.Value;
                        DSRes.Rows[y]["TDSOnCommAmt"] = DBNull.Value;
                        DSRes.Rows[y]["ServiceTax"] = DBNull.Value;
                        DSRes.Rows[y]["STOnComm"] = DBNull.Value;
                        DSRes.Rows[y]["FinalAmt"] = DBNull.Value;
                    }
                    else
                    {
                        FirstLoop = 1;
                        OldAWBNo = DSRes.Rows[y]["AWBNumber"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            
            return DSRes.Copy();
        }

        protected DataTable editDatasetValuesNew(DataTable DSRes)
        {
            try
            {
                string OldAWBNo = "", NewAWBNo = "";
                double OldTotal = 0, NewTotal = 0, OldChwt = 0, OldRate = 0, NewChwt = 0, NewRate = 0;
                int FirstLoop = 0;
                for (int y = 0; y < DSRes.Rows.Count; y++)
                {
                    if (FirstLoop == 0)
                    {
                        OldAWBNo = DSRes.Rows[0]["AWBNumber"].ToString();
                        OldTotal = Convert.ToDouble(DSRes.Rows[0]["FinalAmt"].ToString());
                        OldChwt = Convert.ToDouble(DSRes.Rows[0]["ChargableWt"].ToString());
                        OldRate = Convert.ToDouble(DSRes.Rows[0]["RatePerKg"].ToString());
                    }
                    else
                    {
                        OldAWBNo = DSRes.Rows[y - 1]["AWBNumber"].ToString();

                        if (DSRes.Rows[y - 1]["FinalAmt"].ToString() != "")
                        {
                            OldTotal = Convert.ToDouble(DSRes.Rows[y - 1]["FinalAmt"].ToString());
                            OldChwt = Convert.ToDouble(DSRes.Rows[y - 1]["ChargableWt"].ToString());
                            OldRate = Convert.ToDouble(DSRes.Rows[y - 1]["RatePerKg"].ToString());
                        }
                    }

                    if (DSRes.Rows.Count > y)
                    {
                        NewAWBNo = DSRes.Rows[y]["AWBNumber"].ToString();
                        NewTotal = Convert.ToDouble(DSRes.Rows[y]["FinalAmt"].ToString());
                        NewChwt = Convert.ToDouble(DSRes.Rows[y]["ChargableWt"].ToString());
                        NewRate = Convert.ToDouble(DSRes.Rows[y]["RatePerKg"].ToString());
                    }
                    if (NewAWBNo == OldAWBNo && y != 0 && DSRes.Rows[y]["RatePerKg"].ToString() == DSRes.Rows[y]["FreightRate"].ToString())
                    {
                        DSRes.Rows[y]["FreightRate"] = DBNull.Value;
                        DSRes.Rows[y]["OCDueCar"] = DBNull.Value;
                        DSRes.Rows[y]["OCDueAgent"] = DBNull.Value;
                        DSRes.Rows[y]["CommissionAmt"] = DBNull.Value;
                        DSRes.Rows[y]["TDSOnCommAmt"] = DBNull.Value;
                        DSRes.Rows[y]["ServiceTax"] = DBNull.Value;
                        DSRes.Rows[y]["STOnComm"] = DBNull.Value;
                        DSRes.Rows[y]["FinalAmt"] = DBNull.Value;
                    }
                    else if (NewAWBNo == OldAWBNo && y != 0 && DSRes.Rows[y]["SpotRateStatus"].ToString() == "Applicable" && DSRes.Rows[y]["DCMNumber"].ToString() == "")
                    {
                        DSRes.Rows[y]["FreightRate"] = DBNull.Value;
                        DSRes.Rows[y]["OCDueCar"] = DBNull.Value;
                        DSRes.Rows[y]["OCDueAgent"] = DBNull.Value;
                        DSRes.Rows[y]["CommissionAmt"] = DBNull.Value;
                        DSRes.Rows[y]["TDSOnCommAmt"] = DBNull.Value;
                        DSRes.Rows[y]["ServiceTax"] = DBNull.Value;
                        DSRes.Rows[y]["STOnComm"] = DBNull.Value;
                        DSRes.Rows[y]["FinalAmt"] = DBNull.Value;
                    }
                    else if (NewAWBNo == OldAWBNo && NewTotal == OldTotal && y != 0)
                    {
                        DSRes.Rows[y]["OCDueCar"] = DBNull.Value;
                        DSRes.Rows[y]["OCDueAgent"] = DBNull.Value;
                        DSRes.Rows[y]["CommissionAmt"] = DBNull.Value;
                        DSRes.Rows[y]["TDSOnCommAmt"] = DBNull.Value;
                        DSRes.Rows[y]["ServiceTax"] = DBNull.Value;
                        DSRes.Rows[y]["STOnComm"] = DBNull.Value;
                        DSRes.Rows[y]["FinalAmt"] = DBNull.Value;
                    }
                    else
                    {
                        FirstLoop = 1;
                        OldAWBNo = DSRes.Rows[y]["AWBNumber"].ToString();

                        if (DSRes.Rows[y]["FinalAmt"].ToString() != "")
                        {
                            OldTotal = Convert.ToDouble(DSRes.Rows[y]["FinalAmt"].ToString());
                            OldChwt = Convert.ToDouble(DSRes.Rows[y]["ChargableWt"].ToString());
                            OldRate = Convert.ToDouble(DSRes.Rows[y]["RatePerKg"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return DSRes.Copy();
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
        //        //e.Data = Dataset2;
        //    }
        //}
        #endregion

        #region Validate Controls
        private bool Validate()
        {
            try
            {
                DateTime dt;
                

                try
                {
                    //dt = Convert.ToDateTime(txtbillingfrom.Text);
                    //Change 03082012
                    string day = txtFromdate.Text.Substring(0, 2);
                    string mon = txtFromdate.Text.Substring(3, 2);
                    string yr = txtFromdate.Text.Substring(6, 4);
                    strfromdate = yr + "-" + mon + "-" + day;
                    dt = Convert.ToDateTime(strfromdate);

                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                    lblStatus.Text = "Selected Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }

                //Validation for To date
                if (txtTodate.Text == "")
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please select Valid date');</SCRIPT>");
                    lblStatus.Text = "Please select Valid date";
                    lblStatus.ForeColor = Color.Blue;
                    return false;
                }

                DateTime dtto;

                try
                {
                    //dtto = Convert.ToDateTime(txtbillingto.Text);
                    //Change 03082012
                    string day = txtTodate.Text.Substring(0, 2);
                    string mon = txtTodate.Text.Substring(3, 2);
                    string yr = txtTodate.Text.Substring(6, 4);
                    strtodate = yr + "-" + mon + "-" + day;
                    dtto = Convert.ToDateTime(strtodate);
                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                    lblStatus.Text = "Selected Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }

                if (dtto < dt)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('To date should be greater than From date');</SCRIPT>");
                    lblStatus.Text = "To date should be greater than From date";
                    lblStatus.ForeColor = Color.Red;
                    // MessageBox.Show("Please Enter FlightID's which is not Operated");

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

            return true;
        }
        #endregion

        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            try
            {
                // taking all parameters as user selected in report in one variable - "Param"
                string Param = "From Date:" + txtFromdate.Text.ToString() + "To Date:" + txtTodate.Text.ToString();
                objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "Billing DCM", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
            }
            catch (Exception ex)
            {

            }
            finally
            {
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
                objBL = null;
            }

            if (DaysConfigured > 0 && intReportInterval > DaysConfigured)
                return "Report can be generated only for " + DaysConfigured.ToString() + " days.";
            else
                return "";
        }
        public void Getdata()
        {
            DataSet rest = new DataSet("DCM_rest");
            try
            {
                //RptViewerRevenue_Station.Visible = false;
                lblStatus.Text = string.Empty;

                //  FileInfo info;
                //ReportRuntime runtime;
                //ReportDefinition definition;
                if (Validate() == false)
                {
                    Session["dsListDCM"] = null;
                    ReportViewer1.Visible = false;
                    return;
                }

                lblStatus.Text = "";
                //btnExport.Visible = false;
                Session["dsListDCM"] = null;

                rest = objReport.GetBillingDCMData(Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(strtodate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"));

                if (rest != null)
                {
                    if (rest.Tables[0].Rows.Count > 0)
                    {
                        // RptViewerRevenue_Station.Visible = true;
                        lblStatus.Text = string.Empty;
                        //Dataset1 = res;
                        //ReportViewer1.Visible = true;
                        /////////////Code to edit result dataset (hiding duplicate values for AWB)////////////////////
                        DataTable dtRes = rest.Tables[0];
                        DataTable dtNew = editDatasetValuesNew(dtRes);
                        Session["dsListDCM"] = rest;
                        btnExport.Visible = true;


                        //Dataset1 = new DataSet("Table");
                        //Dataset1.Tables.Add(dtNew);
                        //////////////////////////////////////////////////////////////////////////////////////////////

                        //RptViewerRevenue_Station.Visible = true;

                        //info = new FileInfo(Server.MapPath("/Reports/rptBillingDCMDetails.rdlx"));
                        ////info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptStationWiseTonnageReport.rdlx");

                        //// info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "Report1.rdlx");

                        //definition = new ReportDefinition(info);
                        //runtime = new ReportRuntime(definition);
                        //runtime.LocateDataSource += WARCustWise_LocateDataSource;
                        //RptViewerRevenue_Station.SetReport(runtime);
                        //  oDs2 = LoadParamDataset();// just temperory used, not provide real value                 Dataset2 = oDs2;                  runtime.LocateDataSource += WARCustWise_LocateDataSource;                 WARCustWise.SetReport(runtime);                 pnlMsg.Visible = false;              }             else             {                 oDs1 = null;                 WARCustWise.Visible = false;                 // lblMsg.Text = "No Data Available";                 pnlMsg.Visible = true;                 lblMsgBox.Text = "No Data Available";                 //Pic.Visible = true;               }

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

                        //if (res.Tables[0].Columns.Contains("Logo") == false)
                        //{
                        //    DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                        //    col1.DefaultValue = Logo.ToArray();
                        //    res.Tables[0].Columns.Add(col1);
                        //}
                        //RptViewerRevenue_Station.Visible = false;
                        //ReportViewer1.ProcessingMode = ProcessingMode.Local;
                        //ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptBillingDCMDetails.rdlc");
                        //ReportDataSource datasource = new ReportDataSource("dsrptBillingDCMDetails_dtrptBillingDCMDetails", res.Tables[0]);
                        //ReportViewer1.LocalReport.DataSources.Clear();
                        //ReportViewer1.LocalReport.DataSources.Add(datasource);

                    }
                    else
                    {
                        //ReportViewer1.Visible = false;
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

            }
            finally
            {
                rest.Dispose();
            }
        }

        #region Button Export
        protected void btnExport_Click(object sender, EventArgs e)
        {
            
                lblStatus.Text = "";
                DataSet ds =new DataSet("DCM_ds");
                DataTable dt = new DataTable("DCM_dt");

                try
                {
                   // if ((DataTable)Session["dsListDCM"] == null)
                       // return;
                    Getdata();

                    ds = (DataSet)Session["dsListDCM"];
                    dt = (DataTable)ds.Tables[0];
                    //dt = city.GetAllCity();//your datatable 
                    string attachment = "attachment; filename=Billing DCM Report.xls";
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



                //}
                //catch (Exception ex)
                //{
                //    lblStatus.Text = ex.Message;
                //    lblStatus.ForeColor = Color.Red;
                //}

                //Added By : Deepak  Walmiki 27/09/2013

                //try
                //{
                //    lblStatus.Text = "";
                //    //DataSet ds = null;
                //    DataTable dt = null;

                //    if ((DataTable)Session["dsListDCM"] == null)
                //        return;

                //    //ds = (DataSet)Session["dsListDCM"];
                //    dt = (DataTable)Session["dsListDCM"];
                //    ExportToExcel(dt, "DCMBillingReport.xls");
                //}
                //catch (Exception ex)
                //{
                //    lblStatus.Text = ex.Message;
                //    lblStatus.ForeColor = Color.Red;
                //}

            
        }
        #endregion

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

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtFromdate.Text = txtTodate.Text = string.Empty;
            ReportViewer1.Visible = false;
            lblStatus.Text = "";
        }

    }
}
