using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using System.IO;
using Microsoft.Reporting.WebForms;

namespace ProjectSmartCargoManager
{
    public partial class ShowAWBDetails : System.Web.UI.Page
    {
        DataSet Dataset1 = new DataSet();
        DataSet Dataset2 = new DataSet();

        DataTable dtIATARpt = new DataTable();
        DataTable dtUCRULDRpt = new DataTable();
        DataTable dtTable1 = new DataTable();
        DataTable dtTable2 = new DataTable();
        ReportViewer rptUCRReport = new ReportViewer();
        ReportDataSource rds1 = new ReportDataSource();
        ReportDataSource rds2 = new ReportDataSource();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    DataSet dset = (DataSet)Session["awbdetailsforxray"];
                    dtTable1 = dset.Tables[0].Copy();
                    dtTable2 = (DataTable)Session["dt"];
                    if (dtTable2.Columns.Contains("User") == false)
                    {
                        DataColumn col = new DataColumn("User", typeof(string));
                        col.DefaultValue = Session["UserName"].ToString();
                        dtTable2.Columns.Add(col);
                        DataColumn col2 = new DataColumn("ExecTime", typeof(string));
                        col2.DefaultValue = ((DateTime)Session["IT"]).ToString();
                        dtTable2.Columns.Add(col2);
                    }

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
                        dtTable2.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));
                    }
                    catch (Exception ex)
                    { }
                    dtTable2.Rows[0]["Logo"] = Logo.ToArray();
                    rptUCRReport.ProcessingMode = ProcessingMode.Local;

                    LocalReport rep1 = rptUCRReport.LocalReport;

                    rep1.ReportPath = Server.MapPath("/Reports/AWBDetails_New.rdlc");
                    //rep1.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "Billing1.rdlc";
                    rds1.Name = "dsAWBScreeningRpt_DataTable2";
                    rds1.Value = dtTable2;
                    rep1.DataSources.Add(rds1);

                    rptUCRReport.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                    #region Render to PDF
                    try
                    {
                        string reportType = "PDF";
                        //string mimeType;
                        //string encoding;
                        string fileNameExtension;
                        string deviceInfo = "<DeviceInfo><PageHeight>25cm</PageHeight><PageWidth>32cm</PageWidth></DeviceInfo>";

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
                        byte[] bytes = rptUCRReport.LocalReport.Render("PDF",deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);
                        Response.Buffer = true;
                        Response.Clear();
                        Response.ContentType = mimeType;
                        Response.AddHeader("content-disposition", "attachment; filename=" + "AWB Details" + "." + ".pdf");
                        Response.BinaryWrite(bytes); // create the file
                        Response.Flush();


                        //Response.End();
                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion
                }
            }
            catch(Exception ex) { }
        }

        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsAWBScreeningRpt_DataTable1", dtTable1));
            //e.DataSources.Add(new ReportDataSource("dsAWBScreeningRpt_DataTable2", dtTable2));
        }
        
    }
}
