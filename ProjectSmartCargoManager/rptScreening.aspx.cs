using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.Data;
using QID.DataAccess;

using System.IO;
using System.Configuration;
using System.Drawing;
//using DataDynamics.Reports.Rendering.Pdf;
//using DataDynamics.Reports.Rendering.IO;

namespace ProjectSmartCargoManager
{
    public partial class rptScreening : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             //DataSet ds = (DataSet)Session["Rptdataset"];
            //GridView1.DataSource = ds.Tables[1];
            //GridView1.DataBind();


            ////DataDynamics.Reports.ReportDefinition _reportDef = new ReportDefinition();
            ////_reportDef = new DataDynamics.Reports.ReportDefinition(new FileInfo(Server.MapPath("/Reports/Screening.rdlx")));

            ////DataDynamics.Reports.ReportRuntime _reportRuntime = new DataDynamics.Reports.ReportRuntime(_reportDef);
            ////_reportRuntime.LocateDataSource += WARCustWise_LocateDataSource;
            string exportFile = System.IO.Path.GetTempFileName() + ".pdf";
            System.IO.FileInfo myFile = new System.IO.FileInfo(exportFile);
            System.Collections.Specialized.NameValueCollection settings = new System.Collections.Specialized.NameValueCollection();

            settings.Add("PageWidth", "15in");
            settings.Add("PageHeight", "8in");
            ////PdfRenderingExtension _renderingExtension = new DataDynamics.Reports.Rendering.Pdf.PdfRenderingExtension();
            ////FileStreamProvider _provider = new DataDynamics.Reports.Rendering.IO.FileStreamProvider(myFile.Directory, Path.GetFileNameWithoutExtension(myFile.Name));
            ////_reportRuntime.Render(_renderingExtension, _provider, settings);
            Response.Clear();
            Response.ContentType = "application/pdf";
            string filename = Session["UserName"].ToString() + DateTime.Now.ToString("mmddyyy hh:MM:ss") + ".pdf";
            string FullName = filename.Replace(" ", "");
            Response.AddHeader("content-disposition", "attachment; filename=" + FullName);
            Response.BinaryWrite(File.ReadAllBytes(exportFile));
            myFile.Delete();

            //FileInfo info;
            //ReportRuntime runtime;
            //ReportDefinition definition;
            

            //info = new FileInfo(Server.MapPath("/Reports/Screening.rdlx"));
            //definition = new ReportDefinition(info);
            //runtime = new ReportRuntime(definition);
            //runtime.LocateDataSource += WARCustWise_LocateDataSource;
            //RptViewerXRay.SetReport(runtime);
           
                            
        }
        
        //private void WARCustWise_LocateDataSource(object sender, LocateDataSourceEventArgs e)
        //{
          
           
        //    DataSet ds = (DataSet)Session["dsnew"];
        //    if(ds.Tables[0].Rows.Count>0)
        //    { 
        //        e.Data = ds;
        //    Session["Export"] = ds.Tables[0];
        //    }

        //    else if (ds.Tables[0].Rows.Count <= 0)
        //    {
        //        DataSet dsnew = (DataSet)Session["Rptdataset"];
        //        e.Data = dsnew.Tables[1];
        //        DataTable dtnew = new DataTable();
        //        dtnew = dsnew.Tables[1];
        //        dtnew.Columns.Remove("CCSF");
        //        dtnew.Columns.Remove("XRayID");
        //        dtnew.Columns.Remove("XRayTime");
        //        dtnew.Columns.Remove("CanineID"); 
        //        dtnew.Columns.Remove("CanineTime");
        //        dtnew.Columns.Remove("ETDID"); 
        //        dtnew.Columns.Remove("ETDTime");
        //        dtnew.Columns.Remove("PhysicalID");
        //        dtnew.Columns.Remove("PhysicalTime");
        //        dtnew.Columns.Remove("TagID");
        //        dtnew.Columns.Remove("IsSubTag");
        //        dtnew.Columns.Remove("fulTagID");
        //        dtnew.Columns.Remove("XRayLblFrm");
        //        dtnew.Columns.Remove("XRayLblTo");
        //        dtnew.Columns.Remove("ScrnID");
        //        Session["Export"] = dtnew;// ds.Tables[1];
        //    }

        //}
        
        protected void btnExport_Click(object sender, EventArgs e)
        {
            //DataSet ds = (DataSet)Session["Export"];
            DataTable dt = new DataTable();
            dt = (DataTable)Session["Export"];
           
            try
            {
                //if (ds == null)
                //if(ds == null)
                //    return;

                
                //dsExp = ds;
                //dt = ds.Tables[0];
                //dt = city.GetAllCity();//your datatable 
                string attachment = "attachment; filename=Screnning Report.xls";
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
                foreach (DataRow dr in dt.Rows)
                {
                    tab = "";
                    for (i = 0; i < dt.Columns.Count; i++)
                    {
                        Response.Write(tab + dr[i].ToString());
                        tab = "\t";
                    }
                    Response.Write("\n");
                }
                Response.End();
            }
            catch (Exception ex)
            { }
            finally
            {
                //ds = null;
                dt = null;
            }
        }

    }
}
