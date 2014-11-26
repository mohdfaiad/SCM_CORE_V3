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
    public partial class showBillingExcelImport : System.Web.UI.Page
    {
        ReportDataSource rds1 = new ReportDataSource();
        ReportDataSource rds2 = new ReportDataSource();
        DataTable dtTable1 = new DataTable();
        DataTable dtTable2 = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                dtTable1 = (DataTable)Session["ShowExcel"];
                dtTable2 = (DataTable)Session["ShowExcelHeader"];

                ReportViewer1.Visible = true;



                ReportViewer1.Reset();

                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport rep1 = ReportViewer1.LocalReport;

                rep1.ReportPath = Server.MapPath("/Reports/BillingImport1.rdlc");
                //rep1.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "BillingImport1.rdlc";

                rds1.Name = "dsBillingImport_DataTable1";
                rds1.Value = dtTable1;
                rep1.DataSources.Add(rds1);

                ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);

            }
            catch (Exception ex)
            {

            }
        }

        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsBillingImport_DataTable2", dtTable2));

        }
    }
}
