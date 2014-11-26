using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace ProjectSmartCargoManager
{
    public partial class showNotocDoc : System.Web.UI.Page
    {
        ReportDataSource rds1 = new ReportDataSource();
        //ReportDataSource rds2 = new ReportDataSource();
        DataTable dtTable1 = new DataTable("showNotocDoc_dtTable1");
        DataTable dtTable2 = new DataTable("showNotocDoc_dtTable2");
        DataTable dtTable3 = new DataTable("showNotocDoc_dtTable3");
        DataTable dtTable4 = new DataTable("showNotocDoc_dtTable4");
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {


                //Session["DTExport"] = "";
                //Session["DTAWBDetails"] = "";


                //DataSet dsExport = (DataSet)Session["DTExport"];
                //DataSet dsAWBDetails = (DataSet)Session["DTAWBDetails"];



                dtTable1 = (DataTable)Session["DTNOTOC"];

                dtTable2 = (DataTable)Session["DTNOTCAWBs"];
                dtTable3 = (DataTable)Session["DTNOtherCode"];
                dtTable4 = (DataTable)Session["DTNOTOCSPC"];
                ReportViewer1.Visible = true;



                ReportViewer1.Reset();

                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport rep1 = ReportViewer1.LocalReport;

                rep1.ReportPath = Server.MapPath("/Reports/NOTOC_New.rdlc");
                //rep1.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "NOTOC.rdlc";
                rds1.Name = "dsNOTOC_New_dtNOTOC_New";// "dsNotoc_dtNotoc'";
                rds1.Value = dtTable1;
                rep1.DataSources.Add(rds1);


                ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                //ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);


                //this.ReportViewer1.LocalReport.Refresh(); 

                #region Render to PDF 
                try
                {
                    string reportType = "PDF";
                    string mimeType;
                    string encoding;
                    string fileNameExtension;
                    string deviceInfo = "<DeviceInfo></DeviceInfo>";

                    //"<DeviceInfo>" +

                    //"  <OutputFormat>PDF</OutputFormat>" +

                    //"</DeviceInfo>";

                    Warning[] warnings;
                    string[] streams;
                    byte[] renderedBytes;

                    //Render the report

                    renderedBytes = rep1.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    Response.Clear();

                    Response.ContentType = mimeType;

                    Response.AddHeader("content-disposition", "attachment; filename=NOTOC Doc" + "." + fileNameExtension);

                    Response.BinaryWrite(renderedBytes);

                    Response.Flush();
                    Response.Close();

                    rep1.Dispose();


                    //Response.End();
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    if (dtTable1 != null)
                        dtTable1.Dispose();
                    if (dtTable2 != null)
                        dtTable2.Dispose();
                    if (dtTable3 != null)
                        dtTable3.Dispose();
                    if (dtTable4 != null)
                        dtTable4.Dispose();
                    
                }
                #endregion

            }
            catch (Exception ex)
            {

            }

        }


        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            //e.DataSources.Add(new ReportDataSource("dsNotoc_dtNotocAWBs", dtTable2));
            //e.DataSources.Add(new ReportDataSource("dsNotoc_DataTable1", dtTable3));
            e.DataSources.Add(new ReportDataSource("dsNOTOC_New_dtSubRpt_New", dtTable2));
            e.DataSources.Add(new ReportDataSource("dsNOTOC_New_dtSubRpt_SPC1", dtTable4));

        }


    }
}
