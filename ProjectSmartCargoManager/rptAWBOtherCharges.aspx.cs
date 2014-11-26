using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.Data;
using QID.DataAccess;
//
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using Microsoft.Reporting.WebForms;

namespace ProjectSmartCargoManager
{
    public partial class rptAWBOtherCharges : System.Web.UI.Page
    {

        #region Variable
        FileInfo info;
        //ReportRuntime runtime;
        //ReportDefinition definition;
        object[] ChargesInfo = new object[2];
        private DataSet DataSet1;
        private DataSet Dataset2;

        ReportBAL objReport = new ReportBAL();

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                lblStatus.Text = "";
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                btnExport.Visible = true;
                Session["dsAWBOC"] = null;
                ReportViewer1.Visible = false;
            }


        }

        # region RptViwerAWBOtherCharges_LocateDataSource
        //private void RptViwerAWBOtherCharges_LocateDataSource(object sender, LocateDataSourceEventArgs e)
        //{
        //    string dname = e.DataSetName;
        //    //  if (dname.ToLower() == "DataSetValue")
        //    if (dname == "DataSet1")
        //    {
        //        e.Data = DataSet1;
        //    }
        //    else
        //    {
        //        e.Data = Dataset2;
        //    }
        //}

        # endregion RptViwerAWBOtherCharges_LocateDataSource

        protected void btnList_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            DataSet dschk = new DataSet();
            try
            {
                Session["dsAWBOC"] = null;
                lblStatus.Text = "";
                if (Validate() == false)
                {
                    ReportViewer1.Visible = false;
                    Session["dsAWBOC"] = null;
                    return;
                }

              
                if (txtFromDate.Text == "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter From date";
                    txtFromDate.Focus();
                    return;
                }

                if (txtToDate.Text == "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter To date";
                    txtToDate.Focus();
                    return;
                }


                #region Prepare Parameters

                int i = 0;
                // object[] ChargesInfo = new object[2];
                ChargesInfo.SetValue(txtFromDate.Text, i);

                i++;

                ChargesInfo.SetValue(txtToDate.Text, i);



                #endregion Prepare Parameters
                ds = objReport.GetAWBOtherCharges(ChargesInfo);
                DataSet1 = ds;

                DataTable myDataTable = new DataTable();
                DataColumn myDataColumn;

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "frmDate";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "toDate";
                myDataTable.Columns.Add(myDataColumn);


                DataRow dr;
                dr = myDataTable.NewRow();

                dr["frmDate"] = txtFromDate.Text;// "5";
                dr["toDate"] = txtToDate.Text;
                myDataTable.Rows.Add(dr);

                dschk.Tables.Add(myDataTable);
                Dataset2 = dschk;
                Session["dsAWBOC"] = ds;
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {

                                Session["dsAWBOC"] = ds;
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
                                #region oldrdlx
                                ////old rdlx
                                //RptViwerAWBOtherCharges.Visible = true;


                                //info = new FileInfo(Server.MapPath("/Reports/rptAWBOtherCharges.rdlx"));
                                //definition = new ReportDefinition(info);
                                //runtime = new ReportRuntime(definition);

                                //runtime.LocateDataSource += RptViwerAWBOtherCharges_LocateDataSource;
                                //RptViwerAWBOtherCharges.SetReport(runtime);
                                //btnExport.Visible = true;

                                #endregion
                                ////rdlc report
                                ReportViewer1.Visible = true;
                                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptAWBOtherCharges.rdlc");
                                //Customers dsCustomers = GetData("select top 20 * from customers");
                                ReportDataSource datasource = new ReportDataSource("dsAWBOtherCharges_dtAWBOtherCharges", ds.Tables[0]);
                                ReportViewer1.LocalReport.DataSources.Clear();
                                ReportViewer1.LocalReport.DataSources.Add(datasource);
                                SaveUserActivityLog("");


                            }
                            else
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "No Data Found";
                                ReportViewer1.Visible = false;
                                ReportViewer1.Visible = false;
                                SaveUserActivityLog(lblStatus.Text);

                            }
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
                if (ds != null)
                { 
                    ds.Dispose();
                }
                if (dschk != null)
                {
                    dschk.Dispose();
                }
            
            
            }
        }
        private bool Validate()
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
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                txtFromDate.Focus();
                return false;
            }
            return true;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = null;
            DataTable dt = null;

            lblStatus.Text = "";
            Session["dsAWBOC"] = null;
            try
            {
                if (Validate() == false)
                {
                    Session["dsAWBOC"] = null;
                    ReportViewer1.Visible = false;
                    return;
                }
                if ((DataSet)Session["dsAWBOC"] == null)
                {
                    GetData();
                }

                dsExp = (DataSet)Session["dsAWBOC"];
               
                dt = (DataTable)dsExp.Tables[0];
                if (Session["dsAWBOC"] == null && dt == null)
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
            string Param = " Valid From:" + txtFromDate.Text.ToString() + ",Valid To:" + txtToDate.Text.ToString();
            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "AWB CUT Charge", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
        }
        public string GetReportInterval(DateTime FromDate, DateTime ToDate)
        {
            double DaysConfigured = 0;
            double intReportInterval = (ToDate - FromDate).TotalDays + 1;
            LoginBL objBL = new LoginBL();



            try
            {
                Session["dsAWBOC"] = null;
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
            DataSet ds = new DataSet();
            try
            {
                ReportViewer1.Visible = false;
                Session["dsAWBOC"] = null;
                if (Validate() == false)
                    return;

                if (Validate() == true)
                {
                    Session["dsAWBOC"] = null;
                    DateTime dt1 = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                    DateTime dt2 = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);
                    #region Prepare Parameters

                    int i = 0;
                    // object[] ChargesInfo = new object[2];
                    ChargesInfo.SetValue(txtFromDate.Text, i);

                    i++;

                    ChargesInfo.SetValue(txtToDate.Text, i);



                    #endregion Prepare Parameters
                    ds = objReport.GetAWBOtherCharges(ChargesInfo);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                Session["dsAWBOC"] = ds;
                                lblStatus.Text = "";
                                DataSet1 = ds;
                                SaveUserActivityLog("");
                                ReportViewer1.Visible = false;

                            }
                            else
                            {
                                lblStatus.Text = "No Records Found";
                                Session["dsAWBOC"] = null;
                                SaveUserActivityLog(lblStatus.Text);
                                SaveUserActivityLog(lblStatus.Text);
                                lblStatus.ForeColor = Color.Red;
                                SaveUserActivityLog(lblStatus.Text);
                                ReportViewer1.Visible = false;
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            { SaveUserActivityLog(ex.Message); }

            finally
            {
                if (ds != null)
                {
                    ds.Dispose();

                }
            
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtFromDate.Text = txtToDate.Text = string.Empty;
            ReportViewer1.Visible = false;
        }


    }
}

