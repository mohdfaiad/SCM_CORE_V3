using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.IO;
using BAL;


namespace ProjectSmartCargoManager
{
    public partial class ShowInvoiceSummary : System.Web.UI.Page
    {
        ReportDataSource rds1 = new ReportDataSource();
        DataTable dtTable1 = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                dtTable1 = (DataTable)Session["BulkData"];
                
                ReportViewer1.Visible = true;

                ReportViewer1.Reset();

                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport rep1 = ReportViewer1.LocalReport;

                rep1.ReportPath = Server.MapPath("/Reports/InvoiceSummary.rdlc");
                //rep2.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "BillingBulkDataImport.rdlc";

                rds1.Name = "dsInvoiceSummary_DataTable1";
                rds1.Value = dtTable1;
                rep1.DataSources.Add(rds1);

            }
            catch (Exception ex)
            {

            }
        }
    }
}
