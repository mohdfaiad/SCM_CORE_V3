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
    public partial class ShowDCMPrint : System.Web.UI.Page
    {
        ReportDataSource rds1 = new ReportDataSource();
        ReportDataSource rds2 = new ReportDataSource();
        DataTable dtTable1 = new DataTable();
        DataTable dtTable2 = new DataTable();
        DataSet dsResult;
        BAL.BALDCMListing objBAL = new BAL.BALDCMListing();
        string format = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string dcmNo = Request.QueryString["DCMNO"];
            try
            {
                #region Prepare Parameters
                object[] RateCardInfo = new object[1];
                int irow = 0;

                RateCardInfo.SetValue(dcmNo, irow);

                #endregion Prepare Parameters

                GenerateAgentInvoice(RateCardInfo);


                dtTable1 = (DataTable)Session["ShowExcel"];
                dtTable2 = (DataTable)Session["AWBData"];

                ReportViewer1.Visible = true;

                ReportViewer1.Reset();

                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport rep1 = ReportViewer1.LocalReport;

                rep1.ReportPath = Server.MapPath("/Reports/DCMPrint.rdlc");
                //rep1.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "CargoInvoice.rdlc";
                rds1.Name = "dsDCMPrint_DataTable1";
                rds1.Value = dtTable1;
                rep1.DataSources.Add(rds1);

                ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);

                //Code to Save the generated Excel file
                format = Request.QueryString["Format"];

                if (format == "Excel")
                {
                    try
                    {
                        string reportType = "Excel";
                        string mimeType;
                        string encoding;
                        string fileNameExtension;
                        string deviceInfo =

                        "<DeviceInfo>" +

                        "  <OutputFormat>Excel</OutputFormat>" +

                        "</DeviceInfo>";

                        Warning[] warnings;
                        string[] streams;
                        byte[] renderedBytes;

                        //Render the report

                        renderedBytes = rep1.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                        Response.Clear();

                        Response.ContentType = mimeType;

                        Response.AddHeader("content-disposition", "attachment; filename=" + Session["CurrentDCMNumber"].ToString() + "." + fileNameExtension);

                        Response.BinaryWrite(renderedBytes);


                        //Response.End();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                {
                    try
                    {
                        string reportType = "PDF";
                        string mimeType;
                        string encoding;
                        string fileNameExtension;
                        string deviceInfo =

                        "<DeviceInfo>" +

                        "  <OutputFormat>PDF</OutputFormat>" +

                        "</DeviceInfo>";

                        Warning[] warnings;
                        string[] streams;
                        byte[] renderedBytes;

                        //Render the report

                        renderedBytes = rep1.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                        Response.Clear();

                        Response.ContentType = mimeType;

                        Response.AddHeader("content-disposition", "attachment; filename=" + Session["CurrentDCMNumber"].ToString() + "." + fileNameExtension);

                        Response.BinaryWrite(renderedBytes);


                        //Response.End();
                    }
                    catch (Exception ex)
                    {

                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        protected void GenerateAgentInvoice(object[] DCMNum)
        {
            dsResult = objBAL.GetDCMPrintDetails(DCMNum);
            //img for report
            System.IO.MemoryStream Logo = null;
            try
            {

                Logo = CommonUtility.GetImageStream(Page.Server);
            }
            catch (Exception ex)
            {
                Logo = new System.IO.MemoryStream();
            }
            //end
            if (dsResult != null)
            {
                if (dsResult.Tables != null)
                {
                    if (dsResult.Tables.Count > 0)
                    {
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            try
                            {
                                Session["CurrentDCMNumber"] = dsResult.Tables[0].Rows[0]["DCMNumber"].ToString();

                                Session["ShowExcel"] = "";

                                DataTable DTExport = new DataTable();

                                DTExport.Columns.Add("DCMNumber");
                                DTExport.Columns.Add("DCMDate");
                                DTExport.Columns.Add("AgentCode");
                                DTExport.Columns.Add("AgentName");
                                DTExport.Columns.Add("AgentAddress");
                                DTExport.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));

                                
                                DTExport.Rows.Add(
                                dsResult.Tables[0].Rows[0]["DCMNumber"].ToString(),
                                dsResult.Tables[0].Rows[0]["DCMDate"].ToString(),
                                dsResult.Tables[1].Rows[0]["AgentCode"].ToString(),
                                dsResult.Tables[1].Rows[0]["AgentName"].ToString(),
                                dsResult.Tables[1].Rows[0]["AgentAddress"].ToString(),
                                Logo.ToArray());
                                

                                Session["ShowExcel"] = DTExport;

                                Session["AWBData"] = "";
                                DataTable DTAWBData = new DataTable();
                                DTAWBData = dsResult.Tables[2];
                                Session["AWBData"] = DTAWBData;

                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No records found.');</SCRIPT>");
                            //lblStatus.Text = "No records found";
                            //lblStatus.ForeColor = Color.Blue;
                            return;
                        }

                    }
                }
            }
        }

        //Contains code to "Save" Agent Invoice xls file, send it as attachment and "Delete" Agent Invoice xls file from disk
        protected void SaveAgentDCM()
        {
            //Code to check if folder SCMAgentInvoices exists or not. If not, create.
            //string path = @"C:\SCMAgentInvoices";
            string path = System.Configuration.ConfigurationManager.AppSettings["InvoicePath"];
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //Code to Save xls file
                Microsoft.Reporting.WebForms.Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                Byte[] bytes;
                bytes = ReportViewer1.LocalReport.Render("Excel", string.Empty, out mimeType, out encoding,
                            out extension, out streamids, out warnings);
                //FileStream fs = new FileStream(@"c:\SCMAgentInvoices\" + Session["CurrentDCMNumber"].ToString() + ".xls", FileMode.Create);
                FileStream fs = new FileStream(System.Configuration.ConfigurationManager.AppSettings["InvoicePath"] + "\\\\" + Session["CurrentDCMNumber"].ToString() + ".xls", FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();

                //Code to open save dialog box
                String FileName = Session["CurrentDCMNumber"].ToString() + ".xls";
                //String FilePath = @"c:\SCMAgentInvoices\" + Session["CurrentDCMNumber"].ToString() + ".xls"; //Replace this
                String FilePath = System.Configuration.ConfigurationManager.AppSettings["InvoicePath"] + "\\\\" + Session["CurrentDCMNumber"].ToString() + ".xls";
                System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                response.ClearContent();
                response.Clear();
                response.ContentType = "text/plain";
                response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + ";");
                response.TransmitFile(FilePath);
                response.Flush();
                response.End();


                ////Code to send it as attachment in mail
                //SendMail();

                //Code to Delete Agent Invoice xls file
                //string filePath = @"c:\SCMAgentInvoices\" + Session["CurrentDCMNumber"].ToString() + ".xls";
                //if (System.IO.File.Exists(filePath))
                //{
                //    System.IO.File.Delete(filePath);
                //}

            }
            catch (Exception ex)
            {
            }

        }

        protected void SaveAgentDCMPDF()
        {
            //Code to check if folder SCMAgentInvoices exists or not. If not, create.
            //string path = @"C:\SCMAgentInvoices";
            string path = System.Configuration.ConfigurationManager.AppSettings["InvoicePath"];
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //Code to Save xls file
                Microsoft.Reporting.WebForms.Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                Byte[] bytes;
                bytes = ReportViewer1.LocalReport.Render("PDF", string.Empty, out mimeType, out encoding,
                            out extension, out streamids, out warnings);
                //FileStream fs = new FileStream(@"c:\SCMAgentInvoices\" + Session["CurrentDCMNumber"].ToString() + ".pdf", FileMode.Create);
                FileStream fs = new FileStream(System.Configuration.ConfigurationManager.AppSettings["InvoicePath"] + "\\\\" + Session["CurrentDCMNumber"].ToString() + ".pdf", FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();

                //Code to open save dialog box
                String FileName = Session["CurrentDCMNumber"].ToString() + ".pdf";
                //String FilePath = @"c:\SCMAgentInvoices\" + Session["CurrentDCMNumber"].ToString() + ".pdf"; //Replace this
                String FilePath = System.Configuration.ConfigurationManager.AppSettings["InvoicePath"] + "\\\\" + Session["CurrentDCMNumber"].ToString() + ".pdf";
                System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                response.ClearContent();
                response.Clear();
                response.ContentType = "text/plain";
                response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + ";");
                response.TransmitFile(FilePath);
                response.Flush();
                response.End();


                ////Code to send it as attachment in mail
                //SendMail();

                //Code to Delete Agent Invoice xls file
                //string filePath = @"c:\SCMAgentInvoices\" + Session["CurrentDCMNumber"].ToString() + ".xls";
                //if (System.IO.File.Exists(filePath))
                //{
                //    System.IO.File.Delete(filePath);
                //}

            }
            catch (Exception ex)
            {
            }

        }

        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsDCMPrint_DataTable2", dtTable2));
        }
    }
}
