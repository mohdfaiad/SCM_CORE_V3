using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.Data;
using QID.DataAccess;
using System.Drawing;
using Microsoft.Reporting.WebForms;

namespace ProjectSmartCargoManager
{
    public partial class rptEmission : System.Web.UI.Page
    {
        BALEmission objBAL = new BALEmission();
        ReportBAL objReport = new ReportBAL();
        SQLServer da = new SQLServer(Global.GetConnectionString());
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    GetDestination();
                    GetOrigin();
                    txtFromDate.Text = ((DateTime)Session["IT"]).ToString("dd/MM/yyyy");
                    txtToDate.Text = ((DateTime)Session["IT"]).ToString("dd/MM/yyyy");
                }
            }
            catch (Exception ex)
            { }

        }

        # region Get Origin List

        private void GetOrigin()
        {
            try
            {
                DataSet ds = objBAL.GetOriginCodeList(ddlOrigin.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlOrigin.DataSource = ds;
                            ddlOrigin.DataMember = ds.Tables[0].TableName;
                            ddlOrigin.DataValueField = ds.Tables[0].Columns["Code"].ColumnName;

                            ddlOrigin.DataTextField = ds.Tables[0].Columns["Code"].ColumnName;
                            ddlOrigin.DataBind();
                            ddlOrigin.Items.Insert(0, "All");
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
                DataSet ds = objBAL.GetDestinationCodeList(ddlDest.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlDest.DataSource = ds;
                            ddlDest.DataMember = ds.Tables[0].TableName;
                            ddlDest.DataValueField = ds.Tables[0].Columns["Code"].ColumnName;

                            ddlDest.DataTextField = ds.Tables[0].Columns["Code"].ColumnName;
                            ddlDest.DataBind();
                            ddlDest.Items.Insert(0, "All");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        # endregion GetDestinationCode List

        #region Button Export
        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = null;
            DataTable dt = null;

            try
            {

                lblStatus.Text = string.Empty;

                if (Validate() == false)
                {
                    return;
                }
                string Origin, Dest;

                Origin = ddlOrigin.SelectedItem.Text.Trim() == "All" ? "" : ddlOrigin.SelectedItem.Text.Trim();
                Dest = ddlDest.SelectedItem.Text.Trim() == "All" ? "" : ddlDest.SelectedItem.Text.Trim();
                dsExp = objBAL.GetEmissionReport(Origin, Dest, txtFromDate.Text.Trim(), txtToDate.Text.Trim());
                if (dsExp != null)
                {
                    //dsExp = ds;
                    dt = (DataTable)dsExp.Tables[0];
                    if (dt.Columns.Contains("Logo"))
                    {
                        dt.Columns.Remove("Logo");
                    }
                    //dt = city.GetAllCity();//your datatable 
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
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No records found for the selected search criteria!');</SCRIPT>", false);
                    return;
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                dsExp = null;
                dt = null;
            }

        }
        #endregion

        #region Button List
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = string.Empty;

                if (Validate() == false)
                {
                    return;
                }
                string Origin, Dest;

                Origin = ddlOrigin.SelectedItem.Text.Trim() == "All" ? "" : ddlOrigin.SelectedItem.Text.Trim();
                Dest = ddlDest.SelectedItem.Text.Trim() == "All" ? "" : ddlDest.SelectedItem.Text.Trim();
                DataSet ds = objBAL.GetEmissionReport(Origin, Dest, txtFromDate.Text.Trim(), txtToDate.Text.Trim());
                if (ds != null)
                {

                    //Logo
                    System.IO.MemoryStream Logo = null;
                    try
                    {
                        Logo = CommonUtility.GetImageStream(Page.Server);
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
                    rptEmissionReport.Visible = true;
                    rptEmissionReport.ProcessingMode = ProcessingMode.Local;
                    rptEmissionReport.LocalReport.ReportPath = Server.MapPath("~/Reports/EmissionReport.rdlc");
                    ReportDataSource datasource = new ReportDataSource("dsEmission_DataTable1", ds.Tables[0]);
                    rptEmissionReport.LocalReport.DataSources.Clear();
                    rptEmissionReport.LocalReport.DataSources.Add(datasource);
                    SaveUserActivityLog("");
                    ds.Dispose();



                }
                else
                {
                    lblStatus.Text = "No records found for the selected search criteria!";
                    lblStatus.ForeColor = Color.Red;
                    SaveUserActivityLog(lblStatus.Text);
                    rptEmissionReport.Visible = false;
                    return;
                }




            }
            catch (Exception Ex)
            { }
        }
        #endregion

        #region Validate Controls
        private bool Validate()
        {
            try
            {
                try
                {
                    if (txtFromDate.Text.Trim() != "" || txtToDate.Text.Trim() != "")
                    {
                        DateTime dt1 = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                        DateTime dt2 = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                        int chk = DateTime.Compare(dt1, dt2);
                        if (chk > 0)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Please enter valid To date";
                            txtFromDate.Focus();
                            return false;
                        }


                    }
                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                    txtFromDate.Focus();
                    return false;
                }


                //if (ddlOrigin.SelectedItem.Value.ToString() != "All" && ddlDest.SelectedItem.Value.ToString() == "All")
                //{
                //    lblStatus.ForeColor = Color.Red;
                //    lblStatus.Text = "Please select level Type";
                //    txtFromDate.Focus();
                //    return false;
                //}
                string strResult = string.Empty;
                try
                {
                    strResult = objReport.GetReportInterval(DateTime.ParseExact(txtFromDate.Text.Trim(), "dd/MM/yyyy", null), DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", null));
                }
                catch
                {
                    strResult = "";
                }
                if (strResult != "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = strResult;
                    txtFromDate.Focus();
                    return false;
                }

            }
            catch (Exception ex)
            {


            }
            return true;
        }
        #endregion

        #region Save User Activity Log
        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            try
            {
                // taking all parameters as user selected in report in one variable - "Param"
                string Param = "FromDate:" + txtFromDate.Text.Trim() + ",ToDate:" + txtToDate.Text.Trim();

                objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "Emission Report", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
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
        #endregion

        #region Button Clear
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/rptEmission.aspx");
            }
            catch (Exception ex)
            { }
        }
        #endregion
    }
}
