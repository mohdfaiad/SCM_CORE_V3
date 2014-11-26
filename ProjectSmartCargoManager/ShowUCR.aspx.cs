using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;
using System.Data;

namespace ProjectSmartCargoManager
{
    public partial class ShowUCR : System.Web.UI.Page
    {
        #region Variable
        string m = "";
        ReportDataSource rds1 = new ReportDataSource();
        ReportDataSource rds2 = new ReportDataSource();
        ReportDataSource rds3 = new ReportDataSource();

        DataTable dtTable1 = new DataTable();
        DataTable dtTable2 = new DataTable();
        DataTable dtTable3 = new DataTable();
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            RenderReport();
        }
        #endregion

        private void RenderReport()
        {

            try
            {
                ReportViewer rptUCRReport = new ReportViewer();

                string AWNNo = "";



                //A method that returns a collection for our report

                //Note: A report can have multiple data sources

                dtTable1 = new DataTable();
                dtTable1 = (DataTable)Session["DtUCRReport" + m];

                dtTable2 = new DataTable();
                dtTable2 = (DataTable)Session["dtIATARpt" + m];

                dtTable3 = new DataTable();
                dtTable3 = (DataTable)Session["dtUCRULDRpt" + m];

                //List<Employee> employeeCollection = GetData();

                AWNNo = dtTable1.Rows[0][2].ToString();
                System.IO.MemoryStream Logo = null;
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
                    dtTable1.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));
                }
                catch (Exception ex)
                { }
                //dtTable1.Rows.Add(Logo.ToArray());
                dtTable1.Rows[0]["Logo"] = Logo.ToArray();
                //LocalReport localReport = new LocalReport();
                //localReport.ReportPath = Server.MapPath("~/rptUCR.rdlc");

                //ReportDataSource reportDataSource = new ReportDataSource("dsShowUCR_DataTable1", dtTable1);
                //localReport.DataSources.Add(reportDataSource);





                rptUCRReport.Reset();

                rptUCRReport.ProcessingMode = ProcessingMode.Local;

                LocalReport rep1 = rptUCRReport.LocalReport;

                rep1.ReportPath = Server.MapPath("~/rptUCR.rdlc");
                //rep1.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "Billing1.rdlc";
                rds1.Name = "dsUCR_DataTable1";
                rds1.Value = dtTable1;
                rep1.DataSources.Add(rds1);

                rptUCRReport.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);

                #region Render to PDF
                try
                {
                    string reportType = "PDF";
                    //string mimeType;
                    //string encoding;
                    string fileNameExtension;
                    string deviceInfo = "<DeviceInfo><PageHeight>35cm</PageHeight><PageWidth>48cm</PageWidth></DeviceInfo>";

                    //"<DeviceInfo>" +

                    //"  <OutputFormat>PDF</OutputFormat>" +

                    //"</DeviceInfo>";

                    Warning[] warnings;
                    string[] streamIds;
                    string mimeType = string.Empty;
                    string encoding = string.Empty;
                    string extension = string.Empty;

                    //Render the report
                    // send it to the client to download
                    byte[] bytes = rptUCRReport.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                    Response.Buffer = true;
                    Response.Clear();
                    Response.ContentType = mimeType;
                    Response.AddHeader("content-disposition", "attachment; filename=" + "UCR" + "." + ".pdf");
                    Response.BinaryWrite(bytes); // create the file
                    Response.Flush();


                    //Response.End();
                }
                catch (Exception ex)
                {

                }
                #endregion

                //Response.End();
            }
            catch (Exception ex)
            {

            }


        }

        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsUCR_DataTable2", dtTable2));
            e.DataSources.Add(new ReportDataSource("dsUCR_DataTable3", dtTable3));
        }
    }
}
