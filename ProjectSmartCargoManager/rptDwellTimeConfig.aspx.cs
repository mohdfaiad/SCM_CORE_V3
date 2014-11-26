using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QID.DataAccess;
using System.Data;

using System.IO;
using Microsoft.Reporting.WebForms;
using BAL;
using System.Drawing;

namespace ProjectSmartCargoManager
{
    public partial class rptDwellTimeConfig : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());
        ReportBAL OBJasb = new ReportBAL();
        
        ReportBAL objReport = new ReportBAL();
        ReportBAL rptBAl = new ReportBAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            try
            {
                if (!IsPostBack)
                {
                    ds = da.SelectRecords("SPGetCommcategory");
                    ddlStn.DataSource = ds.Tables[0];
                    ddlStn.DataTextField = "Airport";
                    ddlStn.DataValueField = "AirportCode";
                    ddlStn.DataBind();
                    ddlStn.Items.Insert(0, "All");

                    ReportViewer1.Visible = false;

                    ddlCommCat.DataSource = ds.Tables[2];
                    ddlCommCat.DataTextField = "CommodityCode";
                    ddlCommCat.DataValueField = "CommodityCode";
                    ddlCommCat.DataBind();
                    ddlCommCat.Items.Insert(0, "Select");
                    Session["DwellAirport"] = ds.Tables[0];
                    Session["DwellCommCode"] = ds.Tables[2];

                }
            }
            catch (Exception ex) { }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            ReportViewer1.Visible = true;
            DataSet ds = null;
            string ErrorLog = string.Empty;
            try
            {
                if (Validate() == false)
                {
                    ReportViewer1.Visible = false;
                    return;
                }
                    lblStatus.Text = string.Empty;
                int cat = 0;
                if (ddlCommCat.SelectedValue != "Select")
                    cat = Convert.ToInt32(ddlCommCat.SelectedValue);
                string station = "All";
                if (ddlStn.SelectedItem.Text.Trim() == "All")
                { 
                station="";
                }
                else
                     station=ddlStn.SelectedValue.ToString().Trim();
                object[] param = {station, cat, txtCommCode.Text };
                string[] pname = { "Station", "CommCat", "CommCode" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                ds = da.SelectRecords("spGetDataForDwellTimeAlert", pname, param, QueryTypes);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //   rptDwellTime.Visible = true;
                        Session["ds"] = ds.Tables[0];
                        FileInfo info;
                        ////ReportRuntime runtime;
                        ////ReportDefinition definition;

                        //info = new FileInfo(Server.MapPath("/Reports/rptDwellTime.rdlx"));
                        //definition = new ReportDefinition(info);
                        //runtime = new ReportRuntime(definition);
                        //runtime.LocateDataSource += RptDwellTime_LocateDataSource;
                        //rptDwellTime.SetReport(runtime);



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

                        if (ds.Tables[0].Columns.Contains("Logo") == false)
                        {
                            DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                            col1.DefaultValue = Logo.ToArray();
                            ds.Tables[0].Columns.Add(col1);
                        }

                        ReportViewer1.ProcessingMode = ProcessingMode.Local;
                        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptDwellTimeConfig.rdlc");
                        ReportDataSource datasource = new ReportDataSource("dsrptDwellTimeConfig_dsrptDwellTimeConfig", ds.Tables[0]);
                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(datasource);
                        SaveUserActivityLog("");
                    }
                    else
                    {
                        ReportViewer1.Visible = false;

                        lblStatus.Text = "No records found.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        //rptDwellTime.Visible = false;
                    }
                }
                else
                {
                    ReportViewer1.Visible = false;

                }

            }
            catch (Exception ex)
            {
                ErrorLog = ex.Message;
                SaveUserActivityLog(ErrorLog);
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
                if (da != null)
                    da = null;
                
            }
        }
        //private void RptDwellTime_LocateDataSource(object sender, LocateDataSourceEventArgs e)
        //{

        //    try
        //    {
        //        string dname = e.DataSetName;
        //        DataTable dsDwellTime = (DataTable)Session["ds"];

        //        if (dname == "DataSet1")
        //        {
        //            e.Data = dsDwellTime;
        //            //Session["Export"] = dsLegWise.Tables[0];
        //        }
        //    }
        //    catch (Exception ex)
        //    { }

        //}
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

        private bool Validate()
        {
            
                try
                {

                    if (ddlCommCat.SelectedItem.Text.ToString() == "Select" && ddlStn.SelectedItem.Text.ToString() == "Select")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please select Station and Commodity Category";
                        
                        return false;
                    }

                    else
                    {
                         return true;
                    }

                }
                catch (Exception ex)
                {
                    
                    return false;
                }


         

           
        }
        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            // taking all parameters as user selected in report in one variable - "Param"
            string Param = "Station:" + ddlStn.Text.ToString() + ",Commodity Category:" + ddlCommCat.Text.ToString() + ", Commodity Code:" + txtCommCode.Text.ToString();

            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "Warehouse Dwell Report", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtCommCode.Text = string.Empty;
                ddlCommCat.SelectedIndex = ddlStn.SelectedIndex = 0;
                ReportViewer1.Visible = false;
                lblStatus.Text = string.Empty;
            }
            catch (Exception ex)
            { }
        }
        #region Button Export

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            DataTable dt = null;

            try
            {
               
                //if ((DataTable)Session["ds"] == null)
                    GetData();
                //ds = (DataSet)Session["ds"];
                   ds= (DataSet)Session["ds"];
                    //dt = (DataTable)Session["ds"];
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                        dt = (DataTable)ds.Tables[0];
                    else
                        return;
         
                string attachment = "attachment; filename=No-Show Report.xls";
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
        }
        #endregion

        protected void GetData()
        {
            DataSet ds = null;
            string ErrorLog = string.Empty;
            if(Validate()==false)
            {
                Session["ds"] = null;
                ReportViewer1.Visible = false;
                return;
            }
            try
            {
                lblStatus.Text = string.Empty;
                int cat = 0;
                if (ddlCommCat.SelectedValue != "Select")
                    cat = Convert.ToInt32(ddlCommCat.SelectedValue);
                object[] param = { ddlStn.SelectedItem.Text, cat, txtCommCode.Text };
                string[] pname = { "Station", "CommCat", "CommCode" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                ds = da.SelectRecords("spGetDataForDwellTimeAlert", pname, param, QueryTypes);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //   rptDwellTime.Visible = true;
                        Session["ds"] = ds;
                        FileInfo info;
                        ////ReportRuntime runtime;
                        ////ReportDefinition definition;

                        //info = new FileInfo(Server.MapPath("/Reports/rptDwellTime.rdlx"));
                        //definition = new ReportDefinition(info);
                        //runtime = new ReportRuntime(definition);
                        //runtime.LocateDataSource += RptDwellTime_LocateDataSource;
                        //rptDwellTime.SetReport(runtime);



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

                        //if (ds.Tables[1].Columns.Contains("Logo") == false)
                        //{
                        //    DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                        //    col1.DefaultValue = Logo.ToArray();
                        //    ds.Tables[1].Columns.Add(col1);
                        //}

                        //ReportViewer1.ProcessingMode = ProcessingMode.Local;
                        //ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptDwellTimeConfig.rdlc");
                        //ReportDataSource datasource = new ReportDataSource("dsrptDwellTimeConfig_dsrptDwellTimeConfig", ds.Tables[1]);
                        //ReportViewer1.LocalReport.DataSources.Clear();
                        //ReportViewer1.LocalReport.DataSources.Add(datasource);
                        SaveUserActivityLog("");
                    }
                    else
                    {
                        ReportViewer1.Visible = false;

                        lblStatus.Text = "No records found.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        //rptDwellTime.Visible = false;
                    }
                }
                else
                {
                    ReportViewer1.Visible = false;

                }

            }
            catch (Exception ex)
            {
                ErrorLog = ex.Message;
                SaveUserActivityLog(ErrorLog);
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
                if (da != null)
                    da = null;

            }

        }
    }
}
