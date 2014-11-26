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
    public partial class ShowEAWB : System.Web.UI.Page
    {
        string m = "";
        ReportDataSource rds1 = new ReportDataSource();
        ReportDataSource rds2 = new ReportDataSource();
        DataTable dtTable1 = new DataTable();
        DataTable dtTable2 = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {

           
          //  string DeviceInfo=

            //try
            //{
            //    DataSet dsforrep = new DataSet();

            //    DataTable dtforrep = new DataTable();
            //    dtforrep=(DataTable) Session["DTExport"];
              
            //    ReportViewer1.Visible = true;

            //    ReportDataSource rds = new ReportDataSource();

            //    ReportViewer1.Reset();

            //    ReportViewer1.ProcessingMode = ProcessingMode.Local;

            //    LocalReport rep = ReportViewer1.LocalReport;

            //    rep.Refresh();

            //    rep.ReportPath = Server .MapPath("/Reports/AWB.rdlc");

            //    //This name must be in "<datasetname>_<datatablename>" format. This name can also be seen in dataset's datasource view.

            //    // rds.Name = "AllRecordsWithCondition_dtAllRecordsWithCondition";
            //    rds.Name = "dsShowEAWB_DataTable1";

            //    //rep.DataSources[0]=dtforrep ;
            //    // Text in bold should be your datatable's name from your current dataset i.e. AllRecordsWithCondition.

            //    rds.Value = dtforrep;

            //    rep.DataSources.Add(rds);

                
            //}
            //catch (Exception ex)
            //{

            //}

             m = Request.QueryString["ID"];
             //for (int i = 0; i < 7; i++)
             //{
             //    m = i.ToString();
                 RenderReport();
                 //RenderReport1();
            // }  
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Server.Transfer("FrmConBooking.aspx");
        }

        private void RenderReport()
        {

            try
            {
              
                string AWNNo = "";
               


                    //A method that returns a collection for our report

                    //Note: A report can have multiple data sources

                    DataTable dtforrep = new DataTable();
                    dtTable1 = (DataTable)Session["DTExport" + m];
                    DataTable dtforrep1 = new DataTable();
                    dtTable2 = (DataTable)Session["DTExportSubDetails" + m];
                    

                    //List<Employee> employeeCollection = GetData();

                    AWNNo = dtTable1.Rows[0][2].ToString();
                    string[] AWBPrefix =    AWNNo.Split('-');
                string sessionawBPrefix=Session["awbPrefix"].ToString();
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;

                    LocalReport rep1 = ReportViewer1.LocalReport;

                    if (Session["awbPrefix"].ToString() != null)
                    {
                        if (AWBPrefix[0].ToString() == sessionawBPrefix)
                        {
                            rep1.ReportPath = Server.MapPath("/Reports/EAWB.rdlc");
                        }
                        else
                        {
                            rep1.ReportPath = Server.MapPath("/Reports/EAWB.rdlc");
                        }
                    }                    
                   
                    //rep1.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "AWB.rdlc";
                    rds1.Name ="dsShowEAWB_DataTable1";// "dsShowEAWB";// "EMULDAWB_dtManifest";
                    rds1.Value = dtTable1;
                    rep1.DataSources.Add(rds1);


                    ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);

                    //LocalReport localReport = new LocalReport();

                    ////localReport.ReportPath = Server.MapPath("/Reports/AWB.rdlc");
                    //localReport.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "AWB.rdlc";

                    ////Give the collection a name (dsShowEAWB) so that we can reference it in our report designer

                    //ReportDataSource reportDataSource = new ReportDataSource("dsShowEAWB_DataTable1", dtforrep);

                    //ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);

                    //localReport.DataSources.Add(reportDataSource);



                    string reportType = "PDF";

                    string mimeType;

                    string encoding;

                    string fileNameExtension;



                    //The DeviceInfo settings should be changed based on the reportType

                    //http://msdn2.microsoft.com/en-us/library/ms155397.aspx

                    string deviceInfo =

                    "<DeviceInfo>" +

                    "  <OutputFormat>PDF</OutputFormat>" +

                    //"  <PageWidth>8.5in</PageWidth>" +

                    //"  <PageHeight>11in</PageHeight>" +

                    //"  <MarginTop>0.01in</MarginTop>" +

                    //"  <MarginLeft>0.01in</MarginLeft>" +

                    //"  <MarginRight>0.01in</MarginRight>" +

                    //"  <MarginBottom>0.01in</MarginBottom>" +

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

                    Response.AddHeader("content-disposition", "attachment; filename=" + AWNNo +"." + fileNameExtension);

                    Response.BinaryWrite(renderedBytes);
                
                
                 //Response.End();
            }
            catch (Exception ex)
            {

            }


        }


        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsShowEAWB_DataTable2", dtTable2));

        }


        private void RenderReport1()
        {

            try
            {

                string AWNNo = "";



                //A method that returns a collection for our report

                //Note: A report can have multiple data sources

                DataTable dtforrep = new DataTable();
                dtforrep = (DataTable)Session["DTExport"];

                //List<Employee> employeeCollection = GetData();

                AWNNo = dtforrep.Rows[0][2].ToString();

                LocalReport localReport = new LocalReport();

                localReport.ReportPath = Server.MapPath("/Reports/MasterEAWB.rdlc");
                //localReport.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "MasterEAWB.rdlc";

                //Give the collection a name (dsShowEAWB) so that we can reference it in our report designe
                ReportDataSource reportDataSource = new ReportDataSource("dsShowEAWB_DataTable1", dtforrep);

                localReport.DataSources.Add(reportDataSource);



                string reportType = "PDF";

                string mimeType;

                string encoding;

                string fileNameExtension;



                //The DeviceInfo settings should be changed based on the reportType

                //http://msdn2.microsoft.com/en-us/library/ms155397.aspx

                string deviceInfo =

                "<DeviceInfo>" +

                "  <OutputFormat>PDF</OutputFormat>" +

                //"  <PageWidth>8.5in</PageWidth>" +

                //"  <PageHeight>15in</PageHeight>" +

                //"  <MarginTop>0.01in</MarginTop>" +

                //"  <MarginLeft>0.01in</MarginLeft>" +

                //"  <MarginRight>0.01in</MarginRight>" +

                //"  <MarginBottom>0.01in</MarginBottom>" +

                "</DeviceInfo>";



                Warning[] warnings;

                string[] streams;

                byte[] renderedBytes;



                //Render the report

                renderedBytes = localReport.Render(

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

                Response.AddHeader("content-disposition", "attachment; filename=" + AWNNo+"1" + "." + fileNameExtension);

                Response.BinaryWrite(renderedBytes);


                // Response.End();
            }
            catch (Exception ex)
            {

            }


        }
    }
}
