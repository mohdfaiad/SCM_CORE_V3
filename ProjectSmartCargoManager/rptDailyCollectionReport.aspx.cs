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
using System.Text;

namespace ProjectSmartCargoManager
{
    public partial class rptDailyCollectionReport : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());
        DataTable dtTableSubSales = new DataTable();
        DataTable dtTableSubCollection = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetAirportCode();
                LoadCarrierDropdown();
                ReportViewer1.Visible = false;
                txtFromdate.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                txtTodate.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");

                txtFromTimeHr.Text = Convert.ToDateTime(Session["IT"]).ToString("HH");
                txtFromTimeMin.Text = Convert.ToDateTime(Session["IT"]).ToString("mm");

                txtToTimeHr.Text = "23"; //Convert.ToDateTime(Session["IT"]).ToString("HH");
                txtToTimeMin.Text = "59"; //Convert.ToDateTime(Session["IT"]).ToString("mm");
            }
        }
        //# region Get Origin List

        //private void GetAirportCode()
        //{
        //    BALException objBAL = new BALException();
        //    DataSet ds = null;

        //    try
        //    {
        //        ds = objBAL.GetAirportCodeList(ddlStation.SelectedValue);
        //        if (ds != null)
        //        {
        //            if (ds.Tables != null)
        //            {
        //                if (ds.Tables.Count > 0)
        //                {

        //                    ddlStation.DataSource = ds;
        //                    ddlStation.DataMember = ds.Tables[0].TableName;
        //                    ddlStation.DataValueField = "AirportCode";

        //                    ddlStation.DataTextField = "Airport";
        //                    ddlStation.DataBind();
        //                    ddlStation.Items.Insert(0, "All");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    finally
        //    {
        //        objBAL = null;
        //        if (ds != null)
        //            ds.Dispose();
        //    }
        //}

        //# endregion GetOriginCode List

        # region Get Origin List

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
            catch (Exception ex)
            {
            }
            finally
            {
                objBAL = null;
                if (ds != null)
                    ds.Dispose();
            }
        }

        # endregion GetOriginCode List

        protected void btnList_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            string ErrorLog = string.Empty;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                Session["DailyCollectionReport"] = null;
                ReportViewer1.Visible = false;
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
                    if (txtFromTimeHr.Text.Trim() != string.Empty && txtToTimeHr.Text.Trim() != string.Empty && txtFromTimeMin.Text.Trim() != string.Empty && txtToTimeMin.Text.Trim() != string.Empty)
                    {
                        int chkFromTimeHr = 0;
                        int chkToTimeHr = 0;
                        int chkFromTimeMin = 0;
                        int chkToTimeMin = 0;
                        if (!int.TryParse(txtFromTimeHr.Text.Trim(), out chkFromTimeHr) || chkFromTimeHr > 23 || !int.TryParse(txtToTimeHr.Text.Trim(), out chkToTimeHr) || chkToTimeHr > 23 || !int.TryParse(txtFromTimeMin.Text.Trim(), out chkFromTimeMin) || chkFromTimeMin > 59 || !int.TryParse(txtToTimeMin.Text.Trim(), out chkToTimeMin) || chkToTimeMin > 59)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Please enter valid time";
                            return;
                        }
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid time";
                        return;
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
                    ReportViewer1.Visible = false;
                    txtFromdate.Focus();
                    return;
                }

                System.IO.MemoryStream Logo = null;

                string fromdate, todate;
                fromdate = dt1.ToString("MM/dd/yyyy") + " " + txtFromTimeHr.Text.Trim() + ":" + txtFromTimeMin.Text.Trim();
                todate = dt2.ToString("MM/dd/yyyy") + " " + txtToTimeHr.Text.Trim() + ":" + txtToTimeMin.Text.Trim();
                object[] param = { fromdate, todate, ddlStation.SelectedItem.Value.ToString(), ddlStationType.SelectedItem.Value.ToString(), ddlCarrier.SelectedItem.Value.Trim() };
                string[] pname = { "fromdate", "todate", "Station", "StationType", "Carrier" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };



                ds = da.SelectRecords("spDailyCollectionReport", pname, param, QueryTypes);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ReportViewer1.Visible = true;
                        Session["DailyCollectionReport"] = ds;//.Tables[0];
                        dtTableSubSales = ds.Tables[0];
                        dtTableSubCollection = ds.Tables[1];
                        //Logo
                        try
                        {
                            Logo = CommonUtility.GetImageStream(Page.Server);
                        }
                        catch (Exception ex)
                        {
                            Logo = new System.IO.MemoryStream();
                        }
                        try
                        {

                        }
                        catch (Exception ex)
                        { }
                        MasterBAL m = new MasterBAL();
                        string client = m.clientName();
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
                            if (dt.Columns.Contains("") == false)
                            {
                                DataColumn col1 = new DataColumn("PreparedOn", System.Type.GetType("System.String"));
                                col1.DefaultValue = Session["IT"] != null ? ((DateTime)(Session["IT"])).ToString("dd/MM/yyyy HH:mm:ss") : string.Empty;
                                dt.Columns.Add(col1);
                            }
                            if (dt.Columns.Contains("") == false)
                            {
                                DataColumn col1 = new DataColumn("PreparedBy", System.Type.GetType("System.String"));
                                col1.DefaultValue = client;
                                dt.Columns.Add(col1);
                            }

                        }
                        ReportViewer1.Visible = true;

                        ReportViewer1.ProcessingMode = ProcessingMode.Local;
                        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptDailyCollection.rdlc");
                        ReportDataSource datasource = new ReportDataSource("dsrptDailyCollection_dsrptDailyCollection", dt);

                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(datasource);
                        ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                       
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
        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            // taking all parameters as user selected in report in one variable - "Param"
            string Param = "Station:" + ddlStation.Text.ToString() + ", FrmDt:" + txtFromdate.Text.ToString() + ", ToDt:" + txtTodate.Text.ToString();

            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "Daily Collection Report", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/rptDailyCollectionReport.aspx");
        }

        public void GetData()
        {
            lblStatus.Text = "";
            string ErrorLog = string.Empty;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                Session["DailyCollectionReport"] = null;
                ReportViewer1.Visible = false;
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
                    if (txtFromTimeHr.Text.Trim() != string.Empty && txtToTimeHr.Text.Trim() != string.Empty && txtFromTimeMin.Text.Trim() != string.Empty && txtToTimeMin.Text.Trim() != string.Empty)
                    {
                        int chkFromTimeHr = 0;
                        int chkToTimeHr = 0;
                        int chkFromTimeMin = 0;
                        int chkToTimeMin = 0;
                        if (!int.TryParse(txtFromTimeHr.Text.Trim(), out chkFromTimeHr) || chkFromTimeHr > 23 || !int.TryParse(txtToTimeHr.Text.Trim(), out chkToTimeHr) || chkToTimeHr > 23 || !int.TryParse(txtFromTimeMin.Text.Trim(), out chkFromTimeMin) || chkFromTimeMin > 59 || !int.TryParse(txtToTimeMin.Text.Trim(), out chkToTimeMin) || chkToTimeMin > 59)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Please enter valid time";
                            return;
                        }
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid time";
                        return;
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
                    ReportViewer1.Visible = false;
                    txtFromdate.Focus();
                    return;
                }

                System.IO.MemoryStream Logo = null;

                string fromdate, todate;
                fromdate = dt1.ToString("MM/dd/yyyy") + " " + txtFromTimeHr.Text.Trim() + ":" + txtFromTimeMin.Text.Trim();
                todate = dt2.ToString("MM/dd/yyyy") + " " + txtToTimeHr.Text.Trim() + ":" + txtToTimeMin.Text.Trim();
                object[] param = { fromdate, todate, ddlStation.SelectedItem.Value.ToString(), ddlStationType.SelectedItem.Value.ToString(), ddlCarrier.SelectedItem.Text.Trim() };
                string[] pname = { "fromdate", "todate", "Station", "StationType", "Carrier" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };



                ds = da.SelectRecords("spDailyCollectionReport", pname, param, QueryTypes);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ReportViewer1.Visible = true;
                        Session["DailyCollectionReport"] = ds;//.Tables[0];
                        //Logo
                        try
                        {
                            Logo = CommonUtility.GetImageStream(Page.Server);
                        }
                        catch (Exception ex)
                        {
                            Logo = new System.IO.MemoryStream();
                        }
                        try
                        {

                        }
                        catch (Exception ex)
                        { }
                        MasterBAL m = new MasterBAL();
                        string client = m.clientName();
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
                            if (dt.Columns.Contains("") == false)
                            {
                                DataColumn col1 = new DataColumn("PreparedOn", System.Type.GetType("System.String"));
                                col1.DefaultValue = Session["IT"] != null ? ((DateTime)(Session["IT"])).ToString("dd/MM/yyyy HH:mm:ss") : string.Empty;
                                dt.Columns.Add(col1);
                            }
                            if (dt.Columns.Contains("") == false)
                            {
                                DataColumn col1 = new DataColumn("PreparedBy", System.Type.GetType("System.string"));
                                col1.DefaultValue = client;
                                dt.Columns.Add(col1);
                            }

                        }


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
        protected void btnExport_Click(object sender, EventArgs e)
        {

            DataSet dt = null;
            Session["DailyCollectionReport"] = null;
            lblStatus.Text = "";
            try
            {
                if ((DataSet)Session["DailyCollectionReport"] == null)
                    GetData();

                dt = (DataSet)Session["DailyCollectionReport"];
                dt.Tables[0].TableName = "Sales";
                dt.Tables[1].TableName = "Collections";
                dt.Tables[2].TableName = "COD COLLECTIONS";
                dt.Tables[3].TableName = "Sales Detailed";
                dt.Tables[4].TableName = "CollectionOnInvoices";

                if (Session["DailyCollectionReport"] == null && dt == null)
                {
                    lblStatus.Text = "No records found";
                    lblStatus.ForeColor = Color.Red;
                    SaveUserActivityLog(lblStatus.Text);
                    ReportViewer1.Visible = false;
                    return;
                }

                ExcelHelper.ToExcel(dt, "DailyCollectionReport" + txtFromdate.Text + "" + txtTodate.Text + ".xls", Response);
                //string attachment = "attachment; filename=DailyCollectionReport.xls";
                //Response.ClearContent();
                //Response.AddHeader("content-disposition", attachment);
                //Response.ContentType = "application/vnd.ms-excel";
                //string tab = "";
                //foreach (DataColumn dc in dt.Columns)
                //{
                //    Response.Write(tab + dc.ColumnName);
                //    tab = "\t";
                //}
                //Response.Write("\n");
                //int i;
                //foreach (DataRow dr in dt.Rows)
                //{
                //    tab = "";
                //    for (i = 0; i < dt.Columns.Count; i++)
                //    {
                //        Response.Write(tab + dr[i].ToString());
                //        tab = "\t";
                //    }
                //    Response.Write("\n");
                //}
                Response.End();
            }
            catch (Exception ex)
            { }
            finally
            {

                dt = null;
            }
        }

        #region Load Carrier Dropdown
        public void LoadCarrierDropdown()
        {
            DataSet ds = da.SelectRecords("spGetCarrierCodes");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlCarrier.DataSource = ds;
                ddlCarrier.DataTextField = "CarrierName";
                ddlCarrier.DataValueField = "CarrierCode";
                ddlCarrier.DataBind();
                ddlCarrier.Items.Insert(0, "All");
            }
        }
        #endregion

        #region SubReport Processing
        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsrptDailyCollection_dsrptDailyCollectionSubSales", dtTableSubSales));
            e.DataSources.Add(new ReportDataSource("dsrptDailyCollection_dsrptDailyCollectionSubCollections", dtTableSubCollection));
        }
        #endregion

        //ExcelHelper.cs

        public class ExcelHelper
        {
            //Row limits older excel verion per sheet, the row limit for excel 2003 is 65536
            const int rowLimit = 65000;

            private static string getWorkbookTemplate()
            {
                var sb = new StringBuilder(818);
                sb.AppendFormat(@"<?xml version=""1.0""?>{0}", Environment.NewLine);
                sb.AppendFormat(@"<?mso-application progid=""Excel.Sheet""?>{0}", Environment.NewLine);
                sb.AppendFormat(@"<Workbook xmlns=""urn:schemas-microsoft-com:office:spreadsheet""{0}", Environment.NewLine);
                sb.AppendFormat(@" xmlns:o=""urn:schemas-microsoft-com:office:office""{0}", Environment.NewLine);
                sb.AppendFormat(@" xmlns:x=""urn:schemas-microsoft-com:office:excel""{0}", Environment.NewLine);
                sb.AppendFormat(@" xmlns:ss=""urn:schemas-microsoft-com:office:spreadsheet""{0}", Environment.NewLine);
                sb.AppendFormat(@" xmlns:html=""http://www.w3.org/TR/REC-html40"">{0}", Environment.NewLine);
                sb.AppendFormat(@" <Styles>{0}", Environment.NewLine);
                sb.AppendFormat(@"  <Style ss:ID=""Default"" ss:Name=""Normal"">{0}", Environment.NewLine);
                sb.AppendFormat(@"   <Alignment ss:Vertical=""Bottom""/>{0}", Environment.NewLine);
                sb.AppendFormat(@"   <Borders/>{0}", Environment.NewLine);
                sb.AppendFormat(@"   <Font ss:FontName=""Calibri"" x:Family=""Swiss"" ss:Size=""11"" ss:Color=""#000000""/>{0}", Environment.NewLine);
                sb.AppendFormat(@"   <Interior/>{0}", Environment.NewLine);
                sb.AppendFormat(@"   <NumberFormat/>{0}", Environment.NewLine);
                sb.AppendFormat(@"   <Protection/>{0}", Environment.NewLine);
                sb.AppendFormat(@"  </Style>{0}", Environment.NewLine);
                sb.AppendFormat(@"  <Style ss:ID=""s62"">{0}", Environment.NewLine);
                sb.AppendFormat(@"   <Font ss:FontName=""Calibri"" x:Family=""Swiss"" ss:Size=""11"" ss:Color=""#000000""{0}", Environment.NewLine);
                sb.AppendFormat(@"    ss:Bold=""1""/>{0}", Environment.NewLine);
                sb.AppendFormat(@"  </Style>{0}", Environment.NewLine);
                sb.AppendFormat(@"  <Style ss:ID=""s63"">{0}", Environment.NewLine);
                sb.AppendFormat(@"   <NumberFormat ss:Format=""Short Date""/>{0}", Environment.NewLine);
                sb.AppendFormat(@"  </Style>{0}", Environment.NewLine);
                sb.AppendFormat(@" </Styles>{0}", Environment.NewLine);
                sb.Append(@"{0}\r\n</Workbook>");
                return sb.ToString();
            }

            private static string replaceXmlChar(string input)
            {
                input = input.Replace("&", "&amp");
                input = input.Replace("<", "&lt;");
                input = input.Replace(">", "&gt;");
                input = input.Replace("\"", "&quot;");
                input = input.Replace("'", "&apos;");
                return input;
            }

            private static string getCell(Type type, object cellData)
            {
                var data = (cellData is DBNull) ? "" : cellData;
                if (type.Name.Contains("Int") || type.Name.Contains("Double") || type.Name.Contains("Decimal")) return string.Format("<Cell><Data ss:Type=\"Number\">{0}</Data></Cell>", data);
                if (type.Name.Contains("Date") && data.ToString() != string.Empty)
                {
                    return string.Format("<Cell ss:StyleID=\"s63\"><Data ss:Type=\"DateTime\">{0}</Data></Cell>", Convert.ToDateTime(data).ToString("yyyy-MM-dd"));
                }
                return string.Format("<Cell><Data ss:Type=\"String\">{0}</Data></Cell>", replaceXmlChar(data.ToString()));
            }
            private static string getWorksheets(DataSet source)
            {
                var sw = new StringWriter();
                if (source == null || source.Tables.Count == 0)
                {
                    sw.Write("<Worksheet ss:Name=\"Sheet1\">\r\n<Table>\r\n<Row><Cell><Data ss:Type=\"String\"></Data></Cell></Row>\r\n</Table>\r\n</Worksheet>");
                    return sw.ToString();
                }
                foreach (DataTable dt in source.Tables)
                {
                    if (dt.Rows.Count == 0)
                        sw.Write("<Worksheet ss:Name=\"" + replaceXmlChar(dt.TableName) + "\">\r\n<Table>\r\n<Row><Cell  ss:StyleID=\"s62\"><Data ss:Type=\"String\"></Data></Cell></Row>\r\n</Table>\r\n</Worksheet>");
                    else
                    {
                        //write each row data                
                        var sheetCount = 0;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if ((i % rowLimit) == 0)
                            {
                                //add close tags for previous sheet of the same data table
                                if ((i / rowLimit) > sheetCount)
                                {
                                    sw.Write("\r\n</Table>\r\n</Worksheet>");
                                    sheetCount = (i / rowLimit);
                                }
                                sw.Write("\r\n<Worksheet ss:Name=\"" + replaceXmlChar(dt.TableName) +
                                         (((i / rowLimit) == 0) ? "" : Convert.ToString(i / rowLimit)) + "\">\r\n<Table>");
                                //write column name row
                                sw.Write("\r\n<Row>");
                                foreach (DataColumn dc in dt.Columns)
                                    sw.Write(string.Format("<Cell ss:StyleID=\"s62\"><Data ss:Type=\"String\">{0}</Data></Cell>", replaceXmlChar(dc.ColumnName)));
                                sw.Write("</Row>");
                            }
                            sw.Write("\r\n<Row>");
                            foreach (DataColumn dc in dt.Columns)
                                sw.Write(getCell(dc.DataType, dt.Rows[i][dc.ColumnName]));
                            sw.Write("</Row>");
                        }
                        sw.Write("\r\n</Table>\r\n</Worksheet>");
                    }
                }

                return sw.ToString();
            }
            public static string GetExcelXml(DataTable dtInput, string filename)
            {
                var excelTemplate = getWorkbookTemplate();
                var ds = new DataSet();
                ds.Tables.Add(dtInput.Copy());
                var worksheets = getWorksheets(ds);
                var excelXml = string.Format(excelTemplate, worksheets);
                return excelXml;
            }

            public static string GetExcelXml(DataSet dsInput, string filename)
            {
                var excelTemplate = getWorkbookTemplate();
                var worksheets = getWorksheets(dsInput);
                var excelXml = string.Format(excelTemplate, worksheets);
                return excelXml;
            }

            public static void ToExcel(DataSet dsInput, string filename, HttpResponse response)
            {
                var excelXml = GetExcelXml(dsInput, filename);
                response.Clear();
                response.AppendHeader("Content-Type", "application/vnd.ms-excel");
                response.AppendHeader("Content-disposition", "attachment; filename=" + filename);
                response.Write(excelXml);
                response.Flush();
                response.End();
            }

            public static void ToExcel(DataTable dtInput, string filename, HttpResponse response)
            {
                var ds = new DataSet();
                ds.Tables.Add(dtInput.Copy());
                ToExcel(ds, filename, response);
            }
        }
    }
}
