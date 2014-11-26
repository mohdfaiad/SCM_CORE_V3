using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using BAL;


namespace ProjectSmartCargoManager
{

    public partial class ShowEMAWBULD : System.Web.UI.Page
    {
        #region variables
        BLExpManifest bl = new BLExpManifest();
        ReportDataSource rds1 = new ReportDataSource();
        ReportDataSource rds2 = new ReportDataSource();
        DataTable dtTable1 = new DataTable("AWBULD_DT1");
        DataTable dtTable2 = new DataTable("AWBULD_DT2");
        DataTable dtTable3 = new DataTable("AWBULD_DT3");
        string FlightDetails = "Manifest";
        LoginBL lBal = new LoginBL();
        #endregion

        #region Load data on Page Load 
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Session["DTExport"] = "";
                //Session["DTAWBDetails"] = "";

                if (Session["awbPrefix"] != null)
                {
                    TextBox1.Text = Session["awbPrefix"].ToString();

                }
                else
                {
                    MasterBAL objBal = new MasterBAL();
                    Session["awbPrefix"] = objBal.awbPrefix();
                    TextBox1.Text = Session["awbPrefix"].ToString();
                }
                 string i= Request.QueryString["ID"];


                
                    dtTable1 = (DataTable)Session["DTExport"+i];

                    dtTable2 = (DataTable)Session["DTAWBDetails"+i];

                    dtTable3 = (DataTable)Session["DTTransitExport" + i];
                   

                    ReportViewer1.Visible = true;



                    ReportViewer1.Reset();

                    ReportViewer1.ProcessingMode = ProcessingMode.Local;

                    LocalReport rep1 = ReportViewer1.LocalReport;

                    string Paramvalue = lBal.GetMasterConfiguration("ExportManifestRpt");
                    if (Paramvalue != null && Paramvalue.ToUpper() == "GTAFORMAT")
                        rep1.ReportPath = Server.MapPath("/Reports/EXP_ULDMAWB_GTA.rdlc");
                    else
                        rep1.ReportPath = Server.MapPath("/Reports/EXP_ULDMAWB.rdlc");

                //rep1.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "EXP_ULDMAWB.rdlc";
                    try
                    {
                        if (dtTable1.Rows.Count > 0)
                        {
                            FlightDetails = dtTable1.Rows[0][2].ToString();
                            FlightDetails = FlightDetails.Trim().ToUpper().Replace("     ", "-");
                            FlightDetails = FlightDetails.Replace(" ", "");
                            FlightDetails = FlightDetails + "-" + dtTable1.Rows[0][3].ToString() + i;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    rds1.Name = "EMULDAWB_dtManifest";

                    rds1.Value = dtTable1;
                    rep1.DataSources.Add(rds1);

                    //rds1.Name = "EMULDAWB_dtTransitAWB";

                    //rds1.Value = dtTable3;
                    //rep1.DataSources.Add(rds1);

                    ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);



                    string reportType = "PDF";

                    string mimeType;

                    string encoding;

                    string fileNameExtension;



                    //The DeviceInfo settings should be changed based on the reportType

                    //http://msdn2.microsoft.com/en-us/library/ms155397.aspx

                    string deviceInfo =

                    "<DeviceInfo>" +

                    "  <OutputFormat>PDF</OutputFormat>" +

                     "</DeviceInfo>";



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

                    Response.AddHeader("content-disposition", "attachment; filename=" + FlightDetails +"."+ fileNameExtension);

                    Response.BinaryWrite(renderedBytes);
                
                  
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("EMULDAWB_dtAWBDetails", dtTable2));
            
                e.DataSources.Add(new ReportDataSource("EMULDAWB_dtTransitAWB", dtTable3));
            

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

                rep1.ReportPath = Server.MapPath("/Reports/Billing1.rdlc");
                //rep1.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "Billing1.rdlc";
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
                DataSet ds = new DataSet("AWBULD_DS1");
                    ds = bl.GetManifestDetails(strFlight, dt, ManifestdateTo, txtAirportCode.Text.Trim());






            }
            catch (Exception ex)
            {

            }
        }


    }
}
