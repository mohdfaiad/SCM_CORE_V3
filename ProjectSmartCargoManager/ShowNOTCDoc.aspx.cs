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
    public partial class ShowNOTCDoc : System.Web.UI.Page
    {
        ReportDataSource rds1 = new ReportDataSource();
        ReportDataSource rds2 = new ReportDataSource();
        DataTable dtTable1 = new DataTable();
        DataTable dtTable2 = new DataTable();
        DataTable dtTable3= new DataTable();

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

                ReportViewer1.Visible = true;



                ReportViewer1.Reset();

                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport rep1 = ReportViewer1.LocalReport;

                rep1.ReportPath = Server.MapPath("/Reports/NOTOC.rdlc");
                //rep1.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "NOTOC.rdlc";
                rds1.Name = "dsNotoc_dtOtherSpecialLoad";//""dsNotoc_dtNotoc";
                rds1.Value = dtTable1;
                rep1.DataSources.Add(rds1);

                
                ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                //ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);

                //this.ReportViewer1.LocalReport.Refresh(); 

            }
            catch (Exception ex)
            {

            }
        }

        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsNotoc_dtNotocAWBs", dtTable2));
            //e.DataSources.Add(new ReportDataSource("dsNotoc_dtOtherSpecialLoad", dtTable3));

        }





    }
}
