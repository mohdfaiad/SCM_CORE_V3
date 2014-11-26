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
    public partial class RptProofOfReceipt : System.Web.UI.Page
    {
        #region Variable
        ReportBAL objBal = new ReportBAL();
        StockAllocationBAL objBAL = new StockAllocationBAL();
        public static string CurrTime = "";
        SQLServer da = new SQLServer(Global.GetConnectionString());

        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    RPTViewer.Visible = false;
                    getStation();

                    txtFrmDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                    DateTime dtCurr = Convert.ToDateTime(Session["IT"].ToString());
                    CurrTime = dtCurr.ToString("dd/MM/yyyy hh:mm:ss tt");

                }
                catch (Exception ex)
                {

                }
            }
        }
        #endregion 

        #region Station
        public void getStation()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = objBAL.GetCityCode();
                ddlStation.DataSource = ds.Tables[0];
                ddlStation.DataTextField = "CityCode";
                ddlStation.DataValueField = "CityCode";
                ddlStation.DataBind();
                ddlStation.Items.Insert(0, "All");
                ddlStation.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
               
            }
        }
        #endregion 

        #region Button List
        protected void BtnList_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            System.IO.MemoryStream Logo = null;

            try
            {
                lblStatus.Text = "";

                //Validate controls
                if (Validate() == false)
                {
                    RPTViewer.Visible = false;
                    return;
                }

                string FromDate;
                string ToDate;

                if (txtFrmDate.Text.Trim() != "")
                {
                    string day = txtFrmDate.Text.Substring(0, 2);
                    string mon = txtFrmDate.Text.Substring(3, 2);
                    string yr = txtFrmDate.Text.Substring(6, 4);

                    FromDate = day + "/" + mon + "/" + yr;
                }

                if (txtToDate.Text.Trim() != "")
                {
                    string day = txtToDate.Text.Substring(0, 2);
                    string mon = txtToDate.Text.Substring(3, 2);
                    string yr = txtToDate.Text.Substring(6, 4);

                    ToDate = day + "/" + mon + "/" + yr;

                }


                #region Parameters
                
                object[] paramValue = new object[5];
                int i = 0;

                //0
                paramValue.SetValue(TxtAgentNm.Text.Trim(), i);
                i++;

                //1
                paramValue.SetValue(txtFrmDate.Text.Trim(), i);
                i++;

                //2
                paramValue.SetValue(txtToDate.Text.Trim(), i);
                i++;

                //3
                if (ddlPaymode.SelectedIndex ==0)
                {
                    paramValue.SetValue("ALL", i);
                    i++;
                }
                else
                {
                    paramValue.SetValue(ddlPaymode.SelectedValue, i);
                    i++;
                }

                //4
                paramValue.SetValue(ddlStation.SelectedValue, i);
                i++;

                string[] paramName = { "AgentName", "FromDate", "Todate", "PayMode", "Station" };

                SqlDbType[] paramType = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };

                #endregion

                ds = da.SelectRecords("SP_GetDetailsforProofofReceipt", paramName, paramValue, paramType);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //Session["dsProfRt"] = ds;
                    RPTViewer.Visible = true;

                    #region Logo
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
                    #endregion

                    #region RDLC
                    RPTViewer.Visible = true;
                    RPTViewer.ProcessingMode = ProcessingMode.Local;
                    RPTViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/Rpt_ProofofReceipt.rdlc");
                    ReportDataSource datasource = new ReportDataSource("dsProfofReceipt_DtProfReceipt", ds.Tables[0]);
                    RPTViewer.LocalReport.DataSources.Clear();
                    RPTViewer.LocalReport.DataSources.Add(datasource);
                    //RPTViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);

                    #endregion
                    SaveUserActivityLog("");
                }
                else 
                {
                    RPTViewer.Visible = false;
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Data not avialable for given search criteria";
                    //txtFromdate.Focus();
                    return;
                }

            }
            catch (Exception ex)
            {
                String ErrorLog = ex.Message;
                SaveUserActivityLog(ErrorLog);
            }

        }
        #endregion

        #region Button Clar
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/RptProofOfReceipt.aspx");
        }
        #endregion

        #region Validate Controls
        private bool Validate()
        {
            try
            {
                if (txtFrmDate.Text.Trim() != "" || txtToDate.Text.Trim() != "")
                {
                    DateTime dt1 = DateTime.ParseExact(txtFrmDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                    DateTime dt2 = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                    int chk = DateTime.Compare(dt1, dt2);
                    if (chk > 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid To date";
                        txtFrmDate.Focus();
                        return false;
                    }


                }

                string strResult = string.Empty;
                try
                {
                    strResult = objBal.GetReportInterval(DateTime.ParseExact(txtFrmDate.Text.Trim(), "dd/MM/yyyy", null), DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", null));
                }
                catch
                {
                    strResult = "";
                }
                if (strResult != "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = strResult;
                    txtFrmDate.Focus();
                    return false;
                }

            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                txtFrmDate.Focus();
                return false;
            }
            return true;
        }
        #endregion 

        #region Export Button
        protected void BtnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = null;
            DataTable dt = null;
            //Session["dsProfRt"] = null;
            lblStatus.Text = "";

            try
            {
                if (Session["dsProfRt"] == null)
                    GetData();

                dsExp = (DataSet)Session["dsProfRt"];
                if (dsExp != null && dsExp.Tables[0].Rows.Count > 0)
                    dt = (DataTable)dsExp.Tables[0];


                else
                {
                    lblStatus.Text = "Records does not exist for selected criteria";
                    lblStatus.ForeColor = Color.Red;
                    RPTViewer.Visible = false;
                    SaveUserActivityLog(lblStatus.Text);
                    return;
                }

                if (dt.Columns.Contains("Logo"))
                { 
                    dt.Columns.Remove("Logo"); 
                }

                try
                {
                    string attachment = "attachment; filename= ProofOfReceiptReport.xls";
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
                    //Response.Redirect("RptProofOfReceipt.aspx", false);
                    Response.End();
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Error" + ex.Message;
                    lblStatus.ForeColor = Color.Red;
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

            finally
            {
                if (dsExp != null)
                    dsExp = null;
                if (dt != null)
                    dt = null;
            }
        }
        #endregion

        #region Get ReportInterval
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
                if (objBL != null)
                    objBL = null;
            }

            if (DaysConfigured > 0 && intReportInterval > DaysConfigured)
                return "Report can be generated only for " + DaysConfigured.ToString() + " days.";
            else
                return "";
        }

        #endregion

        #region Get Data
        protected void GetData()
        {
            lblStatus.Text = string.Empty;
            try
            {
                string FromDate;
                string ToDate;

                if (txtFrmDate.Text.Trim() != "")
                {
                    string day = txtFrmDate.Text.Substring(0, 2);
                    string mon = txtFrmDate.Text.Substring(3, 2);
                    string yr = txtFrmDate.Text.Substring(6, 4);

                    FromDate = day + "/" + mon + "/" + yr;
                }

                if (txtToDate.Text.Trim() != "")
                {
                    string day = txtToDate.Text.Substring(0, 2);
                    string mon = txtToDate.Text.Substring(3, 2);
                    string yr = txtToDate.Text.Substring(6, 4);

                    ToDate = day + "/" + mon + "/" + yr;

                }
                
            }
            catch (Exception ex)
            {
                lblStatus.Visible = true;
                lblStatus.Text = "Selected Date Format Invalid";
                lblStatus.ForeColor = Color.Red;
                return;
            }


            try
            {
                #region Parameters

                object[] paramValue = new object[5];
                int i = 0;

                //0
                paramValue.SetValue(TxtAgentNm.Text.Trim(), i);
                i++;

                //1
                paramValue.SetValue(txtFrmDate.Text.Trim(), i);
                i++;

                //2
                paramValue.SetValue(txtToDate.Text.Trim(), i);
                i++;

                //3
                if (ddlPaymode.SelectedIndex == 0)
                {
                    paramValue.SetValue("ALL", i);
                    i++;
                }
                else
                {
                    paramValue.SetValue(ddlPaymode.SelectedValue, i);
                    i++;
                }

                //4
                paramValue.SetValue(ddlStation.SelectedValue, i);
                i++;

                string[] paramName = { "AgentName", "FromDate", "Todate", "PayMode", "Station" };

                SqlDbType[] paramType = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };

                #endregion

                DataSet ds = da.SelectRecords("SP_GetDetailsforProofofReceipt", paramName, paramValue, paramType);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Session["dsProfRt"] = ds;
                }
                else
                {
                    RPTViewer.Visible = false;
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Data not avialable for given search criteria";
                    //txtFromdate.Focus();
                    return;
                }

            }
            catch (Exception ex)
            {
               
            }
        }
        #endregion
        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            // taking all parameters as user selected in report in one variable - "Param"
            string Param = "Station:" + ddlStation.Text.ToString() + ", FrmDt:" + txtFrmDate.Text.ToString() + ", ToDt:" + txtToDate.Text.ToString();

            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "Proof Of Receipt Report", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
        }
    }
}
