using BAL;
using QID.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectSmartCargoManager
{
    public partial class rptAWBAnalysis : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["FromDate"] != null && Request.QueryString["ToDate"] != null)
                {
                    txtFromdate.Text = Request.QueryString["FromDate"].ToString();
                    txtTodate.Text = Request.QueryString["ToDate"].ToString();

                    btnExport_Click(null, null);
                }
                else
                {

                    txtFromdate.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                    txtTodate.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");

                }

            }
        }
        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            // taking all parameters as user selected in report in one variable - "Param"
            string Param = "FrmDt:" + txtFromdate.Text.ToString() + ", ToDt:" + txtTodate.Text.ToString();

            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "Operations Performance Report", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/rptAWBAnalysis.aspx", false);
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {

            lblStatus.Text = "";
            lblStatus.Text = "";
            string ErrorLog = string.Empty;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {

                DateTime dt1 = new DateTime();
                DateTime dt2 = new DateTime();
                try
                {
                    if (txtFromdate.Text.Trim() != "" || txtTodate.Text.Trim() != "")
                    {
                        dt1 = DateTime.ParseExact(txtFromdate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                        dt2 = DateTime.ParseExact(txtTodate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                        int chk = DateTime.Compare(dt1, dt2);
                        if (chk > 0)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Please enter valid To date";
                            txtFromdate.Focus();
                            return;
                        }

                    }


                }
                catch (Exception ex)
                { }

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
                    return;
                }

                System.IO.MemoryStream Logo = null;

                string fromdate, todate;
                fromdate = dt1.ToString("dd/MM/yyyy");
                todate = dt2.ToString("dd/MM/yyyy");
                object[] param = { fromdate, todate };
                string[] pname = { "fromdate", "todate" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar };



                ds = da.SelectRecords("spAWBAnalysisReport", pname, param, QueryTypes);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        dt = null;
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            dt = ds.Tables[0].Copy();
                            SaveUserActivityLog(string.Empty);
                            string attachment = "attachment; filename=OperationsPerformanceReport.xls";
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
                            Response.Flush();
                            Response.Close();

                        }


                    }
                    else
                    {
                      
   
                            lblStatus.Text = "No records found";
                            lblStatus.ForeColor = Color.Red;
                            SaveUserActivityLog(lblStatus.Text);
                            return;
                        
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
    }
}