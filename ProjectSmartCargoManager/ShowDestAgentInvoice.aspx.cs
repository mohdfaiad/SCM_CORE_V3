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
    public partial class ShowDestAgentInvoice : System.Web.UI.Page
    {
        ReportDataSource rds1 = new ReportDataSource();
        ReportDataSource rds2 = new ReportDataSource();
        DataTable dtTable1 = new DataTable();
        DataTable dtTable2 = new DataTable();
        DataSet dsResult;
        BAL.BALInvoiceListing objBAL = new BAL.BALInvoiceListing();
        string format = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string invNo = Request.QueryString["INVNO"];
            try
            {
                #region Prepare Parameters
                object[] RateCardInfo = new object[1];
                int irow = 0;

                RateCardInfo.SetValue(invNo, irow);

                #endregion Prepare Parameters

                GenerateAgentInvoice(RateCardInfo);
                //Logo
                System.IO.MemoryStream Logo = null;
                try
                {
                    Logo = CommonUtility.GetImageStream(Page.Server);
                    //System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                }
                catch (Exception ex)
                {
                    Logo = new System.IO.MemoryStream();
                }

                dtTable1 = (DataTable)Session["ShowExcel"];
                dtTable2 = (DataTable)Session["AWBData"];

                if (dtTable1.Columns.Contains("Logo") == false)
                {
                    DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                    col1.DefaultValue = Logo.ToArray();
                    dtTable1.Columns.Add(col1);
                }
               
                //dtTable1.Rows.Add(Logo.ToArray());
                

                ReportViewer1.Visible = true;

                ReportViewer1.Reset();

                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport rep1 = ReportViewer1.LocalReport;

                rep1.ReportPath = Server.MapPath("/Reports/CargoInvoice.rdlc");
                //rep1.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "CargoInvoice.rdlc";
                rds1.Name = "dsCargoInvoice_DataTable1";
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

                        Response.AddHeader("content-disposition", "attachment; filename=" + Session["CurrentInvoiceNo"].ToString() + "." + fileNameExtension);

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

                        Response.AddHeader("content-disposition", "attachment; filename=" + Session["CurrentInvoiceNo"].ToString() + "." + fileNameExtension);

                        Response.BinaryWrite(renderedBytes);


                        //Response.End();
                    }
                    catch (Exception ex)
                    {

                    }
                }

                //Closing current tab
                //Response.Write("<script type='text/javascript' language='javascript'>window.close();</script>");



            }
            catch (Exception ex)
            {

            }
        }

        protected void GenerateAgentInvoice(object[] InvoiceNum)
        {
            dsResult = objBAL.GetDestAgentInvoiceData(InvoiceNum);

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
                                Session["CurrentInvoiceNo"] = dsResult.Tables[1].Rows[0]["InvoiceNumber"].ToString();

                                Session["ShowExcel"] = "";

                                DataTable DTExport = new DataTable();

                                DTExport.Columns.Add("ClientName");
                                DTExport.Columns.Add("ClientAddress");
                                DTExport.Columns.Add("EmailID");
                                DTExport.Columns.Add("PhoneNum");
                                DTExport.Columns.Add("FaxNum");
                                DTExport.Columns.Add("AgentName");
                                DTExport.Columns.Add("AgentAddress");
                                DTExport.Columns.Add("InvoiceNumber");
                                DTExport.Columns.Add("InvoiceDate");
                                DTExport.Columns.Add("AgentCode");
                                DTExport.Columns.Add("TotalChargesDueAirline");
                                DTExport.Columns.Add("TotalBaseAmtForST");
                                DTExport.Columns.Add("TotalSTDueAirline");
                                DTExport.Columns.Add("TDSOnCommission");
                                DTExport.Columns.Add("CommissionableSales");
                                DTExport.Columns.Add("AgentsCommission");
                                DTExport.Columns.Add("OtherChargesDueAgent");
                                DTExport.Columns.Add("STOnCommission");
                                DTExport.Columns.Add("TotalDeductions");
                                DTExport.Columns.Add("NetDueAirlinesAgentINR");
                                DTExport.Columns.Add("WordsINR");
                                DTExport.Columns.Add("ServiceTaxNumber");
                                DTExport.Columns.Add("PanCardNumber");
                                DTExport.Columns.Add("TanNumber");
                                DTExport.Columns.Add("InvoiceType");
                                DTExport.Columns.Add("RegOfficeAddress");
                                DTExport.Columns.Add("RegOfficePhoneNum");
                                //DTExport.Columns.Add("TotalChargableWeight");
                                //DTExport.Columns.Add("TotalPPFreight");
                                //DTExport.Columns.Add("TotalPPOCDC");
                                //DTExport.Columns.Add("TotalPPOCDA");
                                //DTExport.Columns.Add("TotalCCFreight");
                                //DTExport.Columns.Add("TotalCCOCDC");
                                //DTExport.Columns.Add("TotalCCOCDA");
                                //DTExport.Columns.Add("TotalServiceTax");
                                //DTExport.Columns.Add("SubTotalAWBCharges");
                                //DTExport.Columns.Add("TotalSpotFreight");

                                DTExport.Rows.Add(
                                dsResult.Tables[0].Rows[0]["ClientName"].ToString(),
                                dsResult.Tables[0].Rows[0]["ClientAddress"].ToString(),
                                dsResult.Tables[0].Rows[0]["EmailID"].ToString(),
                                dsResult.Tables[0].Rows[0]["PhoneNum"].ToString(),
                                dsResult.Tables[0].Rows[0]["FaxNum"].ToString(),
                                dsResult.Tables[1].Rows[0]["AgentName"].ToString(),
                                dsResult.Tables[1].Rows[0]["AgentAddress"].ToString(),
                                dsResult.Tables[1].Rows[0]["InvoiceNumber"].ToString(),
                                dsResult.Tables[1].Rows[0]["InvoiceDate"].ToString(),
                                dsResult.Tables[1].Rows[0]["AgentCode"].ToString(),
                                dsResult.Tables[3].Rows[0]["TotalChargesDueAirline"].ToString(),
                                dsResult.Tables[3].Rows[0]["TotalBaseAmtForST"].ToString(),
                                dsResult.Tables[3].Rows[0]["TotalSTDueAirline"].ToString(),
                                dsResult.Tables[3].Rows[0]["TDSOnCommission"].ToString(),
                                dsResult.Tables[3].Rows[0]["CommissionableSales"].ToString(),
                                dsResult.Tables[3].Rows[0]["AgentsCommission"].ToString(),
                                dsResult.Tables[3].Rows[0]["OtherChargesDueAgent"].ToString(),
                                dsResult.Tables[3].Rows[0]["STOnCommission"].ToString(),
                                dsResult.Tables[3].Rows[0]["TotalDeductions"].ToString(),
                                dsResult.Tables[3].Rows[0]["NetDueAirlinesAgentINR"].ToString(),
                                dsResult.Tables[3].Rows[0]["WordsINR"].ToString(),
                                dsResult.Tables[0].Rows[0]["ServiceTaxNumber"].ToString(),
                                dsResult.Tables[0].Rows[0]["PanCardNumber"].ToString(),
                                dsResult.Tables[0].Rows[0]["TanNumber"].ToString(),
                                dsResult.Tables[0].Rows[0]["InvoiceType"].ToString(),
                                dsResult.Tables[0].Rows[0]["RegOfficeAddress"].ToString(),
                                dsResult.Tables[0].Rows[0]["RegOfficePhoneNum"].ToString());
                                //dsResult.Tables[2].Rows[0]["TotalChargableWeight"].ToString(),
                                //dsResult.Tables[2].Rows[0]["TotalPPFreight"].ToString(),
                                //dsResult.Tables[2].Rows[0]["TotalPPOCDC"].ToString(),
                                //dsResult.Tables[2].Rows[0]["TotalPPOCDA"].ToString(),
                                //dsResult.Tables[2].Rows[0]["TotalCCFreight"].ToString(),
                                //dsResult.Tables[2].Rows[0]["TotalCCOCDC"].ToString(),
                                //dsResult.Tables[2].Rows[0]["TotalCCOCDA"].ToString(),
                                //dsResult.Tables[2].Rows[0]["TotalServiceTax"].ToString(),
                                //dsResult.Tables[2].Rows[0]["SubTotalAWBCharges"].ToString(),
                                //dsResult.Tables[2].Rows[0]["TotalSpotFreight"].ToString()


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
        protected void SaveAgentInvoice()
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
                //FileStream fs = new FileStream(@"c:\SCMAgentInvoices\" + Session["CurrentInvoiceNo"].ToString() + ".xls", FileMode.Create);
                FileStream fs = new FileStream(System.Configuration.ConfigurationManager.AppSettings["InvoicePath"] + "\\\\" + Session["CurrentInvoiceNo"].ToString() + ".xls", FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();

                //Code to open save dialog box
                String FileName = Session["CurrentInvoiceNo"].ToString() + ".xls";
                //String FilePath = @"c:\SCMAgentInvoices\" + Session["CurrentInvoiceNo"].ToString() + ".xls"; //Replace this
                String FilePath = System.Configuration.ConfigurationManager.AppSettings["InvoicePath"] + "\\\\" + Session["CurrentInvoiceNo"].ToString() + ".xls";
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
                //string filePath = @"c:\SCMAgentInvoices\" + Session["CurrentInvoiceNo"].ToString() + ".xls";
                //if (System.IO.File.Exists(filePath))
                //{
                //    System.IO.File.Delete(filePath);
                //}

            }
            catch (Exception ex)
            {
            }

        }

        protected void SaveAgentInvoicePDF()
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
                //FileStream fs = new FileStream(@"c:\SCMAgentInvoices\" + Session["CurrentInvoiceNo"].ToString() + ".pdf", FileMode.Create);
                FileStream fs = new FileStream(System.Configuration.ConfigurationManager.AppSettings["InvoicePath"] + "\\\\" + Session["CurrentInvoiceNo"].ToString() + ".pdf", FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();

                //Code to open save dialog box
                String FileName = Session["CurrentInvoiceNo"].ToString() + ".pdf";
                //String FilePath = @"c:\SCMAgentInvoices\" + Session["CurrentInvoiceNo"].ToString() + ".pdf"; //Replace this
                String FilePath = System.Configuration.ConfigurationManager.AppSettings["InvoicePath"] + "\\\\" + Session["CurrentInvoiceNo"].ToString() + ".pdf";
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
                //string filePath = @"c:\SCMAgentInvoices\" + Session["CurrentInvoiceNo"].ToString() + ".xls";
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
            e.DataSources.Add(new ReportDataSource("dsCargoInvoice_DataTable2", dtTable2));
        }
    }
}
