using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;

namespace ProjectSmartCargoManager
{
    public partial class ORReceiptDownload : System.Web.UI.Page
    {
        DataSet dsCalc = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            string ReceiptName = (string)Session["ORReceiptName"];

            DataSet ds = (DataSet)Session["ORReceiptDownload"];
            dsCalc = (DataSet)Session["ORReceiptDownloadCalc"];

            string reprinting = (string)Session["REPRINT"];
            ReportViewer rptLoadPlanReport = new ReportViewer();
            ReportDataSource rds1 = new ReportDataSource();
            rptLoadPlanReport.ProcessingMode = ProcessingMode.Local;

            LocalReport rep1 = rptLoadPlanReport.LocalReport;

            //rep1.ReportPath = Server.MapPath("/Reports/rptORReceipt.rdlc");
            //

            if (reprinting == "RePrint")
            {
                if (ds.Tables[0].Rows[0]["AWBNumberAttachment"].ToString() == "")
                    rep1.ReportPath = Server.MapPath("/Reports/rptOfficialReceipt_Reprint.rdlc");
                else
                    rep1.ReportPath = Server.MapPath("/Reports/rptOfficialReceipt_attachment_RePrint.rdlc");

            }
            else
            {
                if (ds.Tables[0].Rows[0]["AWBNumberAttachment"].ToString() == "")
                    rep1.ReportPath = Server.MapPath("/Reports/rptOfficialReceipt.rdlc");
                else
                    rep1.ReportPath = Server.MapPath("/Reports/rptOfficialReceipt_attachment.rdlc");
            }
           
            
            
           

            //rds1.Name = "dsORReceipt_dtORReceipt";
            rds1.Name = "dsOfficialReceipt_dtOfficialReceipt";
            
            rds1.Value = ds.Tables[0];
            rep1.DataSources.Add(rds1);

            rptLoadPlanReport.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);

            try
            {
                string reportType = "PDF";
                //string mimeType;
                //string encoding;
                string fileNameExtension;
                string deviceInfo = "<DeviceInfo></DeviceInfo>";

                //"<DeviceInfo>" +

                //"  <OutputFormat>PDF</OutputFormat>" +

                //"</DeviceInfo>";

                Warning[] warnings;
                string[] streamIds;
                string mimeType; //= string.Empty;
                string encoding;//= string.Empty;
                string extension;//= string.Empty;

                //Render the report
                // send it to the client to download
                byte[] bytes = rptLoadPlanReport.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);
                Response.Buffer = true;
                Response.Clear();
                Response.ContentType = mimeType;
                Response.AddHeader("content-disposition", "attachment; filename=" + ReceiptName + "." + ".pdf");
                Response.BinaryWrite(bytes); // create the file
                Response.Flush();

                //Response.Clear();
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //Response.ContentType = "text/plain";
                //Response.AddHeader("Content-Disposition", "attachment; filename=InvoiceCollection"  + ".pdf");
                //Response.BinaryWrite(bytes);
                //Response.Flush();
                //Response.End();
            }


            catch (Exception ex)
            {
            }
            finally

            { Session["REPRINT"] = null; }

        }

        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsOfficialReceipt_dtOfficialReceipt_Sub", dsCalc.Tables[0]));
        }
    }
}
