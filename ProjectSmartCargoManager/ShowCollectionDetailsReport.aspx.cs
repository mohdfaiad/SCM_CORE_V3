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
    public partial class ShowCollectionDetailsReport : System.Web.UI.Page
    {
        ReportDataSource rds1 = new ReportDataSource();
        ReportDataSource rds2 = new ReportDataSource();
        DataTable dtTable1 = new DataTable();
        DataTable dtTable2 = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    dtTable2 = (DataTable)Session["Filters"];
                    dtTable1 = (DataTable)Session["CollData"];

                    DataTable dt = null;
                    dt = dtTable1;
                    string attachment = "attachment; filename=CollectionExport.xls";
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", attachment);
                    Response.ContentType = "application/vnd.ms-excel";
                    string tab = "";

                    foreach (DataColumn dc in dt.Columns)
                    {
                        Response.Write(tab + dc.ColumnName);
                        tab = "\t";
                    }
                    Response.Write("\n");
                    int i;
                    foreach (DataRow drw in dt.Rows)
                    {
                        tab = "";
                        for (i = 0; i < dt.Columns.Count; i++)
                        {
                            Response.Write(tab + drw[i].ToString());
                            tab = "\t";
                        }
                        Response.Write("\n");
                    }
                    Response.End();

                    //ReportViewer1.Visible = true;

                    //ReportViewer1.Reset();

                    //ReportViewer1.ProcessingMode = ProcessingMode.Local;

                    //LocalReport rep1 = ReportViewer1.LocalReport;

                    //rep1.ReportPath = Server.MapPath("/Reports/CollectionDetails.rdlc");
                    ////rep1.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "CollectionDetails.rdlc";

                    //rds1.Name = "CollectionDetails_DataTable2";
                    //rds1.Value = dtTable1;
                    //rep1.DataSources.Add(rds1);

                    //ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);

                }
            }
            catch (Exception ex)
            {

            }

        }

        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("CollectionDetails_DataTable1", dtTable2));

        }
    }
}
