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

    public partial class ShowEMAWBULD1 : System.Web.UI.Page
    {
        BLExpManifest bl = new BLExpManifest();
        ReportDataSource rds1 = new ReportDataSource();
        ReportDataSource rds2 = new ReportDataSource();
        DataTable dtTable1 = new DataTable("AWBULD1_DT1");
        DataTable dtTable2 = new DataTable("AWBULD1_DT2");
        DataTable dtTable3 = new DataTable("AWBULD1_DT3");

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Session["DTExport"] = "";
                //Session["DTAWBDetails"] = "";
                
                 string i= Request.QueryString["ID"];


                //DataSet dsExport = (DataSet)Session["DTExport"];
                //DataSet dsAWBDetails = (DataSet)Session["DTAWBDetails"];


                
                    dtTable1 = (DataTable)Session["DTExport"+i];

                    dtTable2 = (DataTable)Session["DTAWBDetails"+i];
                    //dtTable1 = dsExport.Tables[i];
                    //dtTable2 = dsAWBDetails.Tables[i];
                    dtTable3 = (DataTable)Session["DTULDDetails"+i];

                    ReportViewer1.Visible = true;



                    ReportViewer1.Reset();

                    ReportViewer1.ProcessingMode = ProcessingMode.Local;

                    LocalReport rep1 = ReportViewer1.LocalReport;
                    rep1.ReportPath = Server.MapPath("/Reports/EXP_ULDArrival.rdlc");
                    //rep1.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "EXP_ULDArrival.rdlc";
                    rds1.Name = "dsArrival_dtManifest";
                    rds1.Value = dtTable1;
                    rep1.DataSources.Add(rds1);


                    ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                //ItemsSubreportProcessingEventHandler(Object, e);

                #region "Print as PDF"

                    string reportType = "PDF";

                    string mimeType;

                    string encoding;

                    string fileNameExtension;



                    //The DeviceInfo settings should be changed based on the reportType

                    //http://msdn2.microsoft.com/en-us/library/ms155397.aspx

                    string deviceInfo = "<DeviceInfo><PageHeight>30cm</PageHeight><PageWidth>30cm</PageWidth></DeviceInfo>";

                    //"<DeviceInfo>" +

                    //"  <OutputFormat>PDF</OutputFormat>" +

                    ////"  <PageWidth>8.5in</PageWidth>" +

                    ////"  <PageHeight>11in</PageHeight>" +

                    ////"  <MarginTop>0.01in</MarginTop>" +

                    ////"  <MarginLeft>0.01in</MarginLeft>" +

                    ////"  <MarginRight>0.01in</MarginRight>" +

                    ////"  <MarginBottom>0.01in</MarginBottom>" +

                    //"</DeviceInfo>";



                    Warning[] warnings;

                    string[] streams;

                    byte[] renderedBytes;



                    //Render the report

                    renderedBytes = rep1.Render(

                        reportType,

                        deviceInfo,

                        out mimeType,

                        out encoding,

                        out fileNameExtension,

                        out streams,

                        out warnings);



                    //Clear the response stream and write the bytes to the outputstream

                    //Set content-disposition to "attachment" so that user is prompted to take an action

                    //on the file (open or save)

                    Response.Clear();

                    Response.ContentType = mimeType;

                    Response.AddHeader("content-disposition", "attachment; filename=" + "Arrival_Manifest" + "." + fileNameExtension);

                    Response.BinaryWrite(renderedBytes);

                #endregion

            }
            catch (Exception ex)
            {

            }
        }

        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsArrival_dtAWBDetails", dtTable2));
            e.DataSources.Add(new ReportDataSource("dsArrival_dtULDDetails", dtTable3));

        }


        public void Showreport()
        {
            try
            {



                dtTable1 = (DataTable)Session["ShowExcel"];
                dtTable2 = (DataTable)Session["ShowExcelHeader"];

                ReportViewer1.Visible = true;



                ReportViewer1.Reset();

                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport rep1 = ReportViewer1.LocalReport;

                rep1.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "Billing1.rdlc";
                rds1.Name = "dsBilling_DataTable1";
                rds1.Value = dtTable1;
                rep1.DataSources.Add(rds1);

                ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);

            }
            catch (Exception ex)
            {

            }

        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                string ManifestDate = "", ManifestdateFrom = "", ManifestdateTo = "";
                string strFlight = TextBox1.Text.Trim() + txtFlightNo.Text.Trim();

                ManifestDate = txtFlightDate.Text.ToString();
                if (ManifestDate.Length > 10)
                {
                    ManifestdateFrom = ManifestDate.Substring(0, 10) + " 00:00:00";
                    ManifestdateTo = ManifestDate.Substring(0, 10) + " 23:59:59";

                }
                else
                {
                    ManifestdateFrom = ManifestDate + " 00:00:00";
                    ManifestdateTo = ManifestDate + " 23:59:59";
                }
                DateTime dt = new DateTime();
                dt = DateTime.Parse(txtFlightDate.Text.ToString());
                DataSet ds = new DataSet("AWBULD1_DS1");
                ds = bl.GetManifestDetails(strFlight, dt, ManifestdateTo, txtAirportCode.Text.Trim());






            }
            catch (Exception ex)
            {

            }
        }


    }
}
