using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using QID.DataAccess;

using System.IO;
using System.Configuration;
using System.Drawing;
using BAL;
using Microsoft.Reporting.WebForms;

namespace ProjectSmartCargoManager
{
    public partial class rptLegWiseReport : System.Web.UI.Page
    {
        #region Variable
        DataSet Dataset1 = new DataSet();
        ReportBAL objBal = new ReportBAL();
        SQLServer da=new SQLServer(Global.GetConnectionString());
        DataTable dtSearch = new DataTable();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                  RptLegWiseViewer.Visible = false;
                    txtFromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtTodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    Session["ExportLegWise"] = null;
                }
            }
            catch (Exception ex)
            { }
        }

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


                //if (ddlLocCode.SelectedItem.Value.ToString() == "All")
                //{
                //    lblStatus.ForeColor = Color.Red;
                //    lblStatus.Text = "Please select level Type";
                //    txtFromdate.Focus();
                //    return;
                //}


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

        protected void btnList_Click(object sender, EventArgs e)
        {
            DataSet Ds = new DataSet();
            lblStatus.Text = string.Empty;
            Session["ExportLegWise"] = null;
            RptLegWiseViewer.Visible = false;

            if (Validate() == false)
            {
                RptLegWiseViewer.Visible = false;
                return;
            }

            try
            {
                string org = "", dest = "";
                DateTime frmdt, todt;
                frmdt = DateTime.ParseExact(txtFromdate.Text, "dd/MM/yyyy", null);
                todt = DateTime.ParseExact(txtTodate.Text, "dd/MM/yyyy", null);


                object[] PName = new object[4];
                PName[0] = org;
                PName[1] = dest;
                PName[2] = txtFromdate.Text;
                PName[3] = txtTodate.Text;

                Ds = objBal.GetLegWiseReport(PName);
                if (Ds != null)
                {
                    if (Ds.Tables.Count > 0)
                    {
                        if (Ds.Tables[0].Rows.Count > 0)
                        {

                            Session["ExportLegWise"] = Ds;
                            RptLegWiseViewer.Visible = true;

                            dtSearch.Columns.Add("FromDate");
                            dtSearch.Columns.Add("ToDate");

                            DataRow dr = dtSearch.NewRow();
                            dr["FromDate"] = txtFromdate.Text.Trim();
                            dr["ToDate"] = txtTodate.Text.Trim();
                            dtSearch.Rows.Add(dr);

                            #region Old Rdlx
                            //FileInfo info;
                            //ReportRuntime runtime;
                            //ReportDefinition definition;

                            //info = new FileInfo(Server.MapPath("/Reports/rptLegWise_New.rdlx"));
                            //definition = new ReportDefinition(info);
                            //runtime = new ReportRuntime(definition);
                            //runtime.LocateDataSource += RptLegWise_LocateDataSource;
                            //RptLegWise.SetReport(runtime);
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

                            if (Ds.Tables[0].Columns.Contains("Logo") == false)
                            {
                                DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                                col1.DefaultValue = Logo.ToArray();
                                Ds.Tables[0].Columns.Add(col1);
                            }

                            RptLegWiseViewer.Visible = true;
                            RptLegWiseViewer.ProcessingMode = ProcessingMode.Local;
                            RptLegWiseViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/LegWiseRpt.rdlc");
                            //Customers dsCustomers = GetData("select top 20 * from customers");
                            ReportDataSource datasource = new ReportDataSource("dsLegwise_dtLegwise", Ds.Tables[0]);
                            RptLegWiseViewer.LocalReport.DataSources.Clear();
                            RptLegWiseViewer.LocalReport.DataSources.Add(datasource);
                            RptLegWiseViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                            SaveUserActivityLog("");

                            #endregion
                        }
                        else
                        {
                            lblStatus.Text = "No records found for selected search criteria";
                            SaveUserActivityLog(lblStatus.Text);
                            RptLegWiseViewer.Visible = false;
                            return;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                SaveUserActivityLog(ex.Message);
            }
            finally
            {
                if (Ds != null)
                    Ds.Dispose();
            }
        }

        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsLegwise_dtLegwiseSub", dtSearch));
        }

        //private void RptLegWise_LocateDataSource(object sender, LocateDataSourceEventArgs e)
        //{

        //    try
        //    {
        //        string dname=e.DataSetName;
        //        DataSet dsLegWise = (DataSet)ViewState["dsnew_routewise"];
                
        //        if (dname=="dsLegWise")
        //        {
        //            e.Data = dsLegWise;
        //            Session["Export_routewise"] = dsLegWise.Tables[0];
        //        }
        //    }
        //    catch (Exception ex)
        //    { }

        //}

        protected void btnExport_Click(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            lblStatus.Text = string.Empty;
            try
            {
                if (Session["ExportLegWise"] == null)
                GetData();


                ds = (DataSet)Session["ExportLegWise"];

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];
                    lblStatus.Text = string.Empty;
                }

                else
                {
                    //lblStatus.Text = "Data does not exist";
                    //lblStatus.ForeColor = Color.Red;
                    SaveUserActivityLog(lblStatus.Text);
                    RptLegWiseViewer.Visible = false;
                    return;
                }
                string attachment = "attachment; filename=RouteWise Report.xls";
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
                if (dt != null)
                    dt.Dispose();
                if (ds != null)
                    ds.Dispose();
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

        public void GetData()
        {
            DataSet Ds = new DataSet();
            lblStatus.Text = string.Empty;
            Session["ExportLegWise"] = null;
            if (Validate() == false)
            {
                RptLegWiseViewer.Visible = false;
                Session["ExportLegWise"] = null;
                return;
            }
            try
            {
                string org = "", dest = "";
                DateTime frmdt, todt;

                frmdt = DateTime.ParseExact(txtFromdate.Text, "dd/MM/yyyy", null);
                todt = DateTime.ParseExact(txtTodate.Text, "dd/MM/yyyy", null);

                object[] PName = new object[4];
                PName[0] = org;
                PName[1] = dest;
                PName[2] = txtFromdate.Text;
                PName[3] = txtTodate.Text;

                Ds = objBal.GetLegWiseReport(PName);
                if (Ds != null)
                {
                    if (Ds.Tables.Count > 0)
                    {
                        if (Ds.Tables[0].Rows.Count > 0)
                        {
                            Session["ExportLegWise"] = Ds;
                            SaveUserActivityLog("");
                        }
                        else
                        {
                            lblStatus.Text = "No Records Found For Selected Search Criteria";
                            SaveUserActivityLog(lblStatus.Text);
                            return;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                SaveUserActivityLog(ex.Message);
            }
            finally
            {
                if (Ds != null)
                    Ds.Dispose();
            }
        }

        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            try
            {
                // taking all parameters as user selected in report in one variable - "Param"
                string Param = "FromDate:" + txtFromdate.Text.Trim() + ",ToDate:" + txtTodate.Text.Trim();

                objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "Route Wise Tonnage", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (objBAL != null)
                    objBAL = null;
            }
        }
    }
}