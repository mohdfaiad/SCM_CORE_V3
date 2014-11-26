using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QID.DataAccess;
using System.Data;

using System.IO;
using BAL;
using System.Globalization;
using Microsoft.Reporting.WebForms;
using System.Drawing;

namespace ProjectSmartCargoManager
{
    public partial class rptAWBNoShow : System.Web.UI.Page
    {
        ListBookingBAL objBAL = new ListBookingBAL();
        ReportBAL objrpt = new ReportBAL();
        string errormessage = "";
        SQLServer da = new SQLServer(Global.GetConnectionString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStationDropDowns();
                ReportViewer1.Visible = false;

                txtfrmdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }
        public void LoadStationDropDowns()
        {
            DataSet dsResult = new DataSet();
            DataSet ds = new DataSet();
            try
            {
                if (objBAL.GetAllStaions(ref dsResult, ref errormessage))
                {

                    DataRow row = dsResult.Tables[0].NewRow();
                    row["Code"] = "All";
                    dsResult.Tables[0].Rows.Add(row);

                    ddlStn.DataSource = dsResult.Tables[0].Copy();
                    ddlStn.DataTextField = "Airport";
                    ddlStn.DataValueField = "Code";
                    ddlStn.DataBind();
                    ddlStn.Items.Insert(0, "All");
                    ddlStn.SelectedIndex = 0;

                    ddldest.DataSource = dsResult.Tables[0].Copy();
                    ddldest.DataTextField = "Airport";
                    ddldest.DataValueField = "Code";
                    ddldest.DataBind();
                    ddldest.Items.Insert(0, "All");
                   
                    ddldest.SelectedIndex = 0;


                    ds = da.SelectRecords("SPGetCommcategory");
                   
                    ddlCC.DataSource = ds.Tables[2];
                    ddlCC.DataTextField = "CommodityCode";
                    ddlCC.DataValueField = "CommodityCode";
                    ddlCC.DataBind();
                    ddlCC.Items.Insert(0, "Select");
                    ddlCC.Items[0].Value = "";
                    Session["DwellCommcategory"] = ds.Tables[2];

                    if (Session["RoleName"].ToString() == "Super User")
                    {
                        ddlStn.SelectedValue = "All";
                    }
                    else
                    {
                        ddlStn.SelectedValue = Session["Station"].ToString();

                    }
                    ddldest.SelectedValue = "All";

                }
                else
                {
                    //lblStatus.Text = "" + errormessage;
                }
            }
            catch (Exception ex)
            {
                dsResult = null;
            }
            finally
            {
                if (dsResult != null)
                    dsResult.Dispose();
            }

        }
        protected void btnList_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            string ErrorLog = string.Empty;
            DataTable dt = new DataTable();
            DataSet ds = null;
            try
            {  
                Session["ds"] = null;
                ReportViewer1.Visible = false;
                if (Validate() == false)
                {
                    Session["ds"] = null;
                    ReportViewer1.Visible = false;
                    return;
                }
                System.IO.MemoryStream Logo = null;

                string source, dest;
                source = ddlStn.SelectedItem.Text.Trim() == "All" ? "" : ddlStn.SelectedItem.Text.Trim();
                dest = ddldest.SelectedItem.Text.Trim() == "All" ? "" : ddldest.SelectedItem.Text.Trim();
                object[] param = { source, dest, txtfrmdate.Text, txttodate.Text, ddlCC.SelectedValue.ToString().Trim()};
                string[] pname = { "Source", "Dest", "AWBFromDate", "AWBToDate", "CommodityCode" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };

                ReportBAL objBal = new ReportBAL();
                string strResult = string.Empty;

                try
                {
                    strResult = objBal.GetReportInterval(DateTime.ParseExact(txtfrmdate.Text.Trim(), "dd/MM/yyyy", null), DateTime.ParseExact(txttodate.Text.Trim(), "dd/MM/yyyy", null));
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
                    ReportViewer1.Visible = false;
                    txtfrmdate.Focus();
                    return;
                }

                ds = da.SelectRecords("SP_GetrptNoShow", pname, param, QueryTypes);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ReportViewer1.Visible = true;
                        Session["ds"] = ds.Tables[0];
                        //FileInfo info;
                        //ReportRuntime runtime;
                        //ReportDefinition definition;

                        //info = new FileInfo(Server.MapPath("/Reports/NNoShowAWB.rdlx"));
                        //definition = new ReportDefinition(info);
                        //runtime = new ReportRuntime(definition);
                        //runtime.LocateDataSource += RptDwellTime_LocateDataSource;
                        //rptNNoSHow.SetReport(runtime);


                        //Logo
                        try
                        {
                            Logo = CommonUtility.GetImageStream(Page.Server);
                            //System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        }
                        catch (Exception ex)
                        {
                            Logo = new System.IO.MemoryStream();
                        }

                        dt = null;
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            dt = ds.Tables[0].Copy();
                            if (dt.Columns.Contains("Logo") == false)
                            {
                                DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                                col1.DefaultValue = Logo.ToArray();
                                dt.Columns.Add(col1);
                            }
                        }

                        ReportViewer1.ProcessingMode = ProcessingMode.Local;
                        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/NoShow.rdlc");
                        //Customers dsCustomers = GetData("select top 20 * from customers");
                        ReportDataSource datasource = new ReportDataSource("dsNoShow_dtNoShow", dt);
                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(datasource);
                        //ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandlerForSUBrpt);

                        //btnExport.Visible = true;
                        SaveUserActivityLog("");
                    }
                    else
                    {
                        lblStatus.Text = "No records found.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        ReportViewer1.Visible = false;
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorLog = ex.Message;
                SaveUserActivityLog(ErrorLog);
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                if (ds != null)
                    ds.Dispose();
                if (da != null)
                    da = null;
            }
        }
        //private void RptDwellTime_LocateDataSource(object sender, LocateDataSourceEventArgs e)
        //{
        //    DataTable dsDwellTime = null;
        //    try
        //    {
        //        string dname = e.DataSetName;
        //        dsDwellTime = (DataTable)Session["ds"];

        //        if (dname == "DataSet1")
        //        {
        //            e.Data = dsDwellTime;
        //            //Session["Export"] = dsLegWise.Tables[0];
        //        }
        //    }
        //    catch (Exception ex)
        //    { }
        //    finally
        //    {
        //        if (dsDwellTime != null)
        //            dsDwellTime.Dispose();
        //    }

        //}
        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            // taking all parameters as user selected in report in one variable - "Param"
            string Param = "Origin:" + ddlStn.Text.ToString() + ", Destination:" + ddldest.Text.ToString() + ", Comm.Code:" + ddlCC.SelectedValue.ToString().Trim()+ ", FrmDt:" + txtfrmdate.Text.ToString() + ", ToDt:" + txttodate.Text.ToString();

            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "No-Show Report", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
        }

        #region Button Export

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            DataTable dt = null;
            Session["ds"] = null;
            lblStatus.Text = "";
            try
            {
                if ((DataTable)Session["ds"] == null)
                    GetData();
                //ds = (DataSet)Session["ds"];
                dt = (DataTable)Session["ds"];

                if (Session["ds"] == null && dt == null)
                {
                    lblStatus.Text = "No records found";
                    lblStatus.ForeColor = Color.Red;
                    SaveUserActivityLog(lblStatus.Text);
                    ReportViewer1.Visible = false;
                    return;
                }
                //ds = (DataSet)Session["ds"];
                //dt = (DataTable)ds.Tables[0];
                //dt = city.GetAllCity();//your datatable 
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
            string ErrorLog = string.Empty;
            DataTable dt = new DataTable();
            DataSet ds = null;
            try
            {
                Validate();
                lblStatus.Text = string.Empty;
                ReportViewer1.Visible = false;

                System.IO.MemoryStream Logo = null;

                string source, dest;
                source = ddlStn.SelectedItem.Text.Trim() == "All" ? "" : ddlStn.SelectedItem.Text.Trim();
                dest = ddldest.SelectedItem.Text.Trim() == "All" ? "" : ddldest.SelectedItem.Text.Trim();
                object[] param = { source, dest, txtfrmdate.Text, txttodate.Text,ddlCC.SelectedValue.ToString().Trim()};
                string[] pname = { "Source", "Dest", "AWBFromDate", "AWBToDate", "CommodityCode" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };

                ReportBAL objBal = new ReportBAL();
                string strResult = string.Empty;

                try
                {
                    strResult = objBal.GetReportInterval(DateTime.ParseExact(txtfrmdate.Text.Trim(), "dd/MM/yyyy", null), DateTime.ParseExact(txttodate.Text.Trim(), "dd/MM/yyyy", null));
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
                    ReportViewer1.Visible = false;
                    txtfrmdate.Focus();
                    return;
                }

                ds = da.SelectRecords("SP_GetrptNoShow", pname, param, QueryTypes);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ReportViewer1.Visible = true;
                        Session["ds"] = ds.Tables[0];
                        //FileInfo info;
                        //ReportRuntime runtime;
                        //ReportDefinition definition;

                        //info = new FileInfo(Server.MapPath("/Reports/NNoShowAWB.rdlx"));
                        //definition = new ReportDefinition(info);
                        //runtime = new ReportRuntime(definition);
                        //runtime.LocateDataSource += RptDwellTime_LocateDataSource;
                        //rptNNoSHow.SetReport(runtime);


                        ////Logo
                        //try
                        //{
                        //    Logo = CommonUtility.GetImageStream(Page.Server);
                        //    //System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        //}
                        //catch (Exception ex)
                        //{
                        //    Logo = new System.IO.MemoryStream();
                        //}

                        //dt = null;
                        //if (ds.Tables[0].Rows.Count > 0)
                        //{

                        //    dt = ds.Tables[0].Copy();
                        //    if (dt.Columns.Contains("Logo") == false)
                        //    {
                        //        DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                        //        col1.DefaultValue = Logo.ToArray();
                        //        dt.Columns.Add(col1);
                        //    }
                        //}

                        //ReportViewer1.ProcessingMode = ProcessingMode.Local;
                        //ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/NoShow.rdlc");
                        ////Customers dsCustomers = GetData("select top 20 * from customers");
                        //ReportDataSource datasource = new ReportDataSource("dsNoShow_dtNoShow", dt);
                        //ReportViewer1.LocalReport.DataSources.Clear();
                        //ReportViewer1.LocalReport.DataSources.Add(datasource);
                        ////ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandlerForSUBrpt);

                        ////btnExport.Visible = true;
                        SaveUserActivityLog("");
                    }
                    else
                    {
                        lblStatus.Text = "No records found.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        ReportViewer1.Visible = false;
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorLog = ex.Message;
                SaveUserActivityLog(ErrorLog);
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                if (ds != null)
                    ds.Dispose();
                if (da != null)
                    da = null;
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
            {
                ReportViewer1.Visible = false;
                return "Report can be generated only for " + DaysConfigured.ToString() + " days.";
            }
            else
                return "";
        }
      private bool Validate()
        {
            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();
            try
            {
                if (txtfrmdate.Text.Trim() != "" || txttodate.Text.Trim() != "")
                {
                    dt1 = DateTime.ParseExact(txtfrmdate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                    dt2 = DateTime.ParseExact(txttodate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                    int chk = DateTime.Compare(dt1, dt2);
                    if (chk > 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid To date";
                        txtfrmdate.Focus();
                        return false;
                    }

                }

                string strResult = string.Empty;
                try
                {
                    strResult = objrpt.GetReportInterval(DateTime.ParseExact(txtfrmdate.Text.Trim(), "dd/MM/yyyy", null), DateTime.ParseExact(txttodate.Text.Trim(), "dd/MM/yyyy", null));
                }
                catch
                {
                    strResult = "";
                }
                if (strResult != "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = strResult;
                    txtfrmdate.Focus();
                    return false;
                }



            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                txtfrmdate.Focus();
                return false;
            }
            
            return true;
        }

      protected void btnClear_Click(object sender, EventArgs e)
      {
          ddlCC.SelectedIndex = 0;
          ddldest.SelectedIndex = 0;
          ddlStn.SelectedIndex = 0;
          txtfrmdate.Text = string.Empty;
          txttodate.Text = string.Empty;
          txtfrmdate.Focus();
          ReportViewer1.Visible = false;
          lblStatus.Text = string.Empty;


      }
            }
        }

    
