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
    public partial class ShowBillingBulkDataImport : System.Web.UI.Page
    {
        ReportDataSource rds1 = new ReportDataSource();
        ReportDataSource rds2 = new ReportDataSource();
        DataTable dtTable1 = new DataTable();
        DataTable dtTable2 = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                dtTable1 = (DataTable)Session["Filters"];
                dtTable2 = (DataTable)Session["BulkData"];

                ReportViewer1.Visible = true;



                ReportViewer1.Reset();

                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport rep2 = ReportViewer1.LocalReport;

                rep2.ReportPath = Server.MapPath("/Reports/BillingBulkDataImport.rdlc");
                //rep2.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "BillingBulkDataImport.rdlc";

                rds2.Name = "dsBillingBulkDataImport_DataTable2";
                rds2.Value = dtTable2;
                rep2.DataSources.Add(rds2);

                ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);

            }
            catch (Exception ex)
            {

            }

        }

        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsBillingBulkDataImport_DataTable1", dtTable1));

        }
    }
}
