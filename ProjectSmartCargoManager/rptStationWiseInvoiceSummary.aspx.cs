using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QID.DataAccess;
using System.Data;
using System.Data.SqlClient;

using System.IO;
using BAL;
using System.Globalization;
using Microsoft.Reporting.WebForms;
using System.Drawing;
using System.Text;

namespace ProjectSmartCargoManager
{
    public partial class rptStationWiseInvoiceSummary : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());

        protected void Page_Load(object sender, EventArgs e)
        {
            //use SP_rptPPCCSummery for list btn click

            if (!IsPostBack)
            {
                txtfrmdate.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                txttodate.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                GetAirportCode();
                Session["ds_CCPPSummery"] = null;

            }
        }
        private void GetAirportCode()
        {
            BALException objBAL = new BALException();
            DataSet ds = null;

            try
            {
                ds = objBAL.GetAirportCodeList(ddlStation.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlStation.DataSource = ds;
                            ddlStation.DataMember = ds.Tables[0].TableName;
                            ddlStation.DataValueField = "AirportCode";
                            ddlStation.DataTextField = "Airport";
                            ddlStation.DataBind();
                            ddlStation.Items.Insert(0, "All");
                        }
                    }
                }
            }
            finally
            {
                objBAL = null;
                if (ds != null)
                    ds.Dispose();
            }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            string ErrorLog = string.Empty;
            try
            {
                DateTime dtFrmDate, dtToDate;
                if (txtfrmdate.Text.Trim() != "")
                {
                    dtFrmDate = DateTime.ParseExact(txtfrmdate.Text.Trim(), "dd/MM/yyyy", null);
                }
                else
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Enter From Date";
                    txtfrmdate.Focus();
                    return;
                }

                if (txttodate.Text.Trim() != "")
                {
                    dtToDate = DateTime.ParseExact(txttodate.Text.Trim(), "dd/MM/yyyy", null);
                }
                else
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Enter To Date";
                    txtfrmdate.Focus();
                    return;
                }
                // Validate Date Interval
                ReportBAL objBal = new ReportBAL();
                string strResult = string.Empty;

                try
                {
                    strResult = objBal.GetReportInterval(dtFrmDate, dtToDate);
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
                    txtfrmdate.Focus();
                    return;
                }


                object[] param = { ddlStation.SelectedItem.Value.ToString(), dtFrmDate.ToString("yyyy/MM/dd"), dtToDate.ToString("yyyy/MM/dd") };
                string[] pname = { "Station", "frmDate", "toDate" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.DateTime };
                DataSet ds = da.SelectRecords("SP_rptPPCCSummery", pname, param, QueryTypes);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataTable dt = ds.Tables[0].Copy();
                            grdPPCCSummery.DataSource = dt;
                            grdPPCCSummery.DataBind();
                            Session["ds_CCPPSummery"] = ds;
                        }
                    }
                    else
                    {
                        lblStatus.Text = "No records found.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorLog = ex.Message;
            }
            finally
            {
                if (da != null)
                    da = null;
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dt = null;
            lblStatus.Text = string.Empty;
            try
            {
                lblStatus.Text = "";
                //DataSet ds = null;

                if ((Session["ds_CCPPSummery"] == null))
                    btnList_Click(sender, e);

                dt = (DataSet)Session["ds_CCPPSummery"];

                if (dt != null && dt.Tables.Count > 0)
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        ExportToExcel(dt.Tables[0], "CCPPSummery.xls");
                        grdPPCCSummery.DataSource = dt;
                        grdPPCCSummery.DataBind();
                    }
                    else
                    {
                        lblStatus.Text = "No record found";
                        lblStatus.ForeColor = Color.Red;
                    }
                }
                else
                {
                    lblStatus.Text = "No record found";
                    lblStatus.ForeColor = Color.Red;
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }

        }
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
    }
}