using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using QID.DataAccess;
using System.Drawing;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using BAL;

//using DataDynamics.Reports.Rendering.Pdf;
//using DataDynamics.Reports.Rendering.IO;
//using DataDynamics.Reports.Rendering.Excel;
using Microsoft.Reporting.WebForms;

namespace ProjectSmartCargoManager
{
    public partial class Download : System.Web.UI.Page
    {
        ReportDataSource rds1 = new ReportDataSource();
        ReportDataSource rds2 = new ReportDataSource();
        ReportDataSource rds3 = new ReportDataSource();
        DataTable dtTable1 = new DataTable();
        DataTable dtTable2 = new DataTable();
        DataTable dtTable3 = new DataTable();
        DataTable dtTable4 = new DataTable();
        DataSet dset;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    if (Request.QueryString["Mode"] == null)
                    {
                        #region Code Added for AWB Epouch

                        if (Session["DocumentsData"] != null)
                        {

                            if (Session["DocumentsData"] != null)
                            {
                                int imagecount = Convert.ToInt32(this.Page.Request.QueryString["FileName"].ToString());
                                DataSet ds = (DataSet)Session["DocumentsData"];
                                if (ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"].ToString() == "png" || ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"].ToString() == "jpeg" || ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"].ToString() == "jpg" || ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"].ToString() == "gif")
                                {
                                    System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                                    response.ClearContent();
                                    response.Clear();
                                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                    response.ContentType = "text/plain";
                                    response.AddHeader("Content-Disposition", "attachment; filename=" + ds.Tables[0].Rows[imagecount - 1]["DocumentName"] + ".png");
                                    response.BinaryWrite((byte[])ds.Tables[0].Rows[imagecount - 1]["Document"]);
                                    response.Flush();
                                    response.End();
                                }
                                else
                                {


                                    System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                                    response.ClearContent();
                                    response.Clear();
                                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                    response.ContentType = "text/plain";
                                    response.AddHeader("Content-Disposition", "attachment; filename=" + ds.Tables[0].Rows[imagecount - 1]["DocumentName"] + "." + ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"]);
                                    response.BinaryWrite((byte[])ds.Tables[0].Rows[imagecount - 1]["Document"]);
                                    response.Flush();
                                    response.End();


                                }
                            }
                        }
                        #endregion

                        #region Code Added for Flights Epouch

                        if (Session["FlightsDocumentsData"] != null)
                        {

                            if (Session["FlightsDocumentsData"] != null)
                            {
                                int imagecount = Convert.ToInt32(this.Page.Request.QueryString["FileName"].ToString());
                                DataSet ds = (DataSet)Session["FlightsDocumentsData"];
                                if (ds.Tables[0].Rows[imagecount - 1]["DocumentFlightsExtension"].ToString() == "png" || ds.Tables[0].Rows[imagecount - 1]["DocumentFlightsExtension"].ToString() == "gif" || ds.Tables[0].Rows[imagecount - 1]["DocumentFlightsExtension"].ToString() == "jpg" || ds.Tables[0].Rows[imagecount - 1]["DocumentFlightsExtension"].ToString() == "jpeg")
                                {
                                    System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                                    response.ClearContent();
                                    response.Clear();
                                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                    response.ContentType = "text/plain";
                                    response.AddHeader("Content-Disposition", "attachment; filename=" + ds.Tables[0].Rows[imagecount - 1]["DocumentName"] + ".png");
                                    response.BinaryWrite((byte[])ds.Tables[0].Rows[imagecount - 1]["Document"]);
                                    response.Flush();
                                    response.End();
                                }
                                else
                                {


                                    System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                                    response.ClearContent();
                                    response.Clear();
                                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                    response.ContentType = "text/plain";
                                    response.AddHeader("Content-Disposition", "attachment; filename=" + ds.Tables[0].Rows[imagecount - 1]["DocumentName"] + "." + ds.Tables[0].Rows[imagecount - 1]["DocumentFlightsExtension"]);
                                    response.BinaryWrite((byte[])ds.Tables[0].Rows[imagecount - 1]["Document"]);
                                    response.Flush();
                                    response.End();


                                }
                            }
                        }
                        #endregion
                    }
                    else
                        if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() == "HAWB")
                        {
                            if (Session["HAWBDocument"] != null)
                            {
                                DataSet dsHAWB = (DataSet)Session["HAWBDocument"];
                                System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                                response.ClearContent();
                                response.Clear();
                                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                response.ContentType = "text/plain";
                                response.AddHeader("Content-Disposition", "attachment; filename=" + dsHAWB.Tables[0].Rows[0]["DocumentName"]);
                                response.BinaryWrite((byte[])dsHAWB.Tables[0].Rows[0]["Document"]);
                                response.Flush();
                                response.End();
                                dsHAWB.Dispose();
                            }
                        }
                        else if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() == "ERP")
                        {
                            if (Session["ExportERP"] != null)
                            {

                                DataSet dsResult = (DataSet)Session["ExportERP"];
                                string attachment = "attachment; filename=ExportERP.xls";
                                Response.ClearContent();
                                Response.AddHeader("content-disposition", attachment);
                                Response.ContentType = "application/vnd.ms-excel";
                                string tab = "";
                                foreach (DataColumn dc in dsResult.Tables[0].Columns)
                                {
                                    Response.Write(tab + dc.ColumnName);
                                    tab = "\t";
                                }
                                Response.Write("\n");
                                int i;
                                foreach (DataRow dr in dsResult.Tables[0].Rows)
                                {
                                    tab = "";
                                    for (i = 0; i < dsResult.Tables[0].Columns.Count; i++)
                                    {
                                        Response.Write(tab + dr[i].ToString());
                                        tab = "\t";
                                    }
                                    Response.Write("\n");
                                }
                                Response.End();
                            }
                        }
                        else
                        {

                            if (Session["abc"] != null)
                            {
                                dset = new DataSet("Table1");
                                //dset.Tables.Add(((DataTable)Session["abc"]).Copy());
                                dset = (DataSet)Session["abc"];
                                if (dset != null)
                                {
                                    if (dset.Tables.Count > 0)
                                    {
                                        if (dset.Tables[0].Rows.Count > 0)
                                        {
                                            string MultipleAWBs = string.Empty;
                                            string MultipleULDs = string.Empty;
                                            string[] AWBNo;

                                            if (Session["MultipleAWB"] != null)
                                            {
                                                MultipleAWBs = Session["MultipleAWB"].ToString();
                                                //char[] charSeparator = new char[] { ',' };
                                                //AWBNo = MultipleAWBs.Split(charSeparator, StringSplitOptions.RemoveEmptyEntries);


                                            }
                                            if (Session["MultipleULD"] != null)
                                            {
                                                MultipleULDs = Session["MultipleULD"].ToString();

                                            }
                                            string[] QueryNames = { "AWBNumber", "ULDNo" };
                                            SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar };
                                            object[] QueryValues = { MultipleAWBs, MultipleULDs };

                                            SQLServer db = new SQLServer(Global.GetConnectionString());
                                            DataSet dCharge = db.SelectRecords("sp_GetChargeHeadForDelivery", QueryNames, QueryValues, QueryTypes);
                                            if (dCharge != null)
                                            {
                                                dtTable4 = dCharge.Tables[0];
                                            }
                                            db = null;

                                            ReportViewer rptUCRReport = new ReportViewer();

                                            dtTable1 = dset.Tables[0];//dtHeader
                                            dtTable2 = dset.Tables[1];//dtSurface
                                            dtTable3 = dset.Tables[2];//dtULD
                                            //dtTable4 = dset.Tables[3];

                                            rptUCRReport.ProcessingMode = ProcessingMode.Local;

                                            LocalReport rep1 = rptUCRReport.LocalReport;

                                            rep1.ReportPath = Server.MapPath("/Reports/DeliveryOrder.rdlc");

                                            //rds1.Name = "dsDeliveryOrder_DataTable1";
                                            //rds1.Value = dtTable1;
                                            //rep1.DataSources.Add(rds2);
                                            //rds1.Name = "dsDeliveryOrder_DataTable2";
                                            //rds1.Value = dtTable2;
                                            //rep1.DataSources.Add(rds3);
                                            rds1.Name = "dsDeliveryOrder_DataTable3";
                                            rds1.Value = dtTable1;// dtTable2;
                                            rep1.DataSources.Add(rds1);
                                            rptUCRReport.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                                            #region Render to PDF
                                            try
                                            {
                                                string reportType = "PDF";
                                                string fileNameExtension;

                                                string deviceInfo = "<DeviceInfo><PageWidth>13.04in</PageWidth><PageHeight>10in</PageHeight></DeviceInfo>";


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
                                                byte[] bytes = rptUCRReport.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);
                                                Response.Buffer = true;
                                                Response.Clear();
                                                Response.ContentType = mimeType;
                                                string filename = Session["UserName"].ToString() + DateTime.Now.ToString("mmddyyy hh:MM:ss") + ".pdf";
                                                string FullName = filename.Replace(" ", "");
                                                Response.AddHeader("content-disposition", "attachment; filename=" + FullName);
                                                //Response.AddHeader("content-disposition", "attachment; filename=" + "DO" + "." + ".pdf");
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
                                }
                            }

                        }
                }
            }
            catch (Exception ex)
            { }

        }

        #region Event for Loading Dataset into Report
        //private void WRAXBDetails_LocateDataSource(object sender, LocateDataSourceEventArgs e)
        //{

        //    string dname = e.DataSetName;
        //    //  if (dname.ToLower() == "DataSetValue")    
        //    if (dname == "DataSet1")
        //    {
        //        e.Data = dset;

        //    }

        //}
        #endregion

        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsDeliveryOrder_DataTable4", dtTable4));
            e.DataSources.Add(new ReportDataSource("dsDeliveryOrder_DataTable2", dtTable3));
            e.DataSources.Add(new ReportDataSource("dsDeliveryOrder_DataTable1", dtTable2)); //Amount,PayMode Removed From AWB DO Subreport
        }
    }
}
