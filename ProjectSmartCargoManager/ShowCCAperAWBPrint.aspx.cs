﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.IO;

namespace ProjectSmartCargoManager
{
    public partial class ShowCCAperAWBPrint : System.Web.UI.Page
    {
        ReportDataSource rds1 = new ReportDataSource();
        DataTable dtTable1 = new DataTable();
        DataSet dsResult;
        BAL.BALCCAListing objBAL = new BAL.BALCCAListing();
        string format = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //string awbNo = Request.QueryString["AWBNO"];
                string awbFlightNo = Request.QueryString["AWBFlightNO"];
                string awbNo = awbFlightNo.Substring(0, awbFlightNo.IndexOf("||"));
                //string FlightDet = awbFlightNo.Substring(awbFlightNo.IndexOf("||") + 2);

                //string[] FlightDetails = FlightDet.Split('|');
                //string FlightDate = awbFlightNo.Substring(awbFlightNo.IndexOf("||") + 3);
                //string[] Params = awbFlightNo.Split(',');
                //string awbNo = Params[0];
                //string FlightNo = FlightDetails[0];
                //string FlightDate = FlightDetails[1];

                #region Prepare Parameters
                object[] RateCardInfo = new object[1];
                int irow = 0;

                RateCardInfo.SetValue(awbNo, irow);
                irow++;
                //RateCardInfo.SetValue(FlightNo, irow);
                //irow++;
                //RateCardInfo.SetValue(FlightDate, irow);


                #endregion Prepare Parameters
                GenerateAgentInvoice(RateCardInfo);
                //img for report-
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


                dtTable1 = (DataTable)Session["ShowExcel"];

                ReportViewer1.Visible = true;

                ReportViewer1.Reset();

                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport rep1 = ReportViewer1.LocalReport;

                rep1.ReportPath = Server.MapPath("/Reports/CCAperAWBPrint.rdlc");
                //rep1.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "CargoInvoice.rdlc";
                rds1.Name = "dsCCAperAWBPrint_DataTable1";
                rds1.Value = dtTable1;
                rep1.DataSources.Add(rds1);

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

                        Response.AddHeader("content-disposition", "attachment; filename=" + Session["CurrentAWBNumber"].ToString() + "." + fileNameExtension);

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

                        Response.AddHeader("content-disposition", "attachment; filename=" + Session["CurrentAWBNumber"].ToString() + "." + fileNameExtension);

                        Response.BinaryWrite(renderedBytes);


                        //Response.End();
                    }
                    catch (Exception ex)
                    {

                    }
                }

                //if (format == "Excel")
                //    SaveAgentCCA();
                //else
                //    SaveAgentCCAPDF();

                ////Closing current tab
                //Response.Write("<script type='text/javascript' language='javascript'>window.close();</script>");


            }
            catch (Exception ex)
            {

            }
        }

        protected void GenerateAgentInvoice(object[] AWBNum)
        {
            dsResult = objBAL.GetCCAperAWBPrintDetails(AWBNum);
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
                                Session["CurrentAWBNumber"] = dsResult.Tables[2].Rows[0]["AWBPrefix"].ToString()+dsResult.Tables[2].Rows[0]["AWBNumber"].ToString();

                                Session["ShowExcel"] = "";

                                DataTable DTExport = new DataTable();
                                DTExport.Columns.Add("AWBPrefix");
                                DTExport.Columns.Add("AWBNumber");
                                DTExport.Columns.Add("CCANumber");
                                DTExport.Columns.Add("CCADate");
                                DTExport.Columns.Add("AgentCode");
                                DTExport.Columns.Add("AgentName");
                                DTExport.Columns.Add("AgentAddress");
                                DTExport.Columns.Add("GrossWeight");
                                DTExport.Columns.Add("ChargableWeight");
                                DTExport.Columns.Add("FreightRate");
                                DTExport.Columns.Add("OCDC");
                                DTExport.Columns.Add("OCDA");
                                DTExport.Columns.Add("ServiceTax");
                                DTExport.Columns.Add("Commission");
                                DTExport.Columns.Add("TDSCommission");
                                DTExport.Columns.Add("STCommission");
                                DTExport.Columns.Add("CurrentTotal");
                                DTExport.Columns.Add("RevisedGrossWeight");
                                DTExport.Columns.Add("RevisedChargableWeight");
                                DTExport.Columns.Add("RevisedFreightRate");
                                DTExport.Columns.Add("RevisedOCDC");
                                DTExport.Columns.Add("RevisedOCDA");
                                DTExport.Columns.Add("RevisedServiceTax");
                                DTExport.Columns.Add("RevisedCommission");
                                DTExport.Columns.Add("RevisedTDSCommission");
                                DTExport.Columns.Add("RevisedSTCommission");
                                DTExport.Columns.Add("RevisedTotal");
                                DTExport.Columns.Add("CCA");
                                DTExport.Columns.Add("Type");
                                DTExport.Columns.Add("FlightNumber");
                                DTExport.Columns.Add("Flightdate");
                                DTExport.Columns.Add("Origin");
                                DTExport.Columns.Add("Destination");
                                DTExport.Columns.Add("AWBDate");
                                DTExport.Columns.Add("Remarks");
                                DTExport.Columns.Add("STNumber");
                                DTExport.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));


                                DTExport.Rows.Add(
                                dsResult.Tables[2].Rows[0]["AWBPrefix"].ToString(),
                                dsResult.Tables[2].Rows[0]["AWBNumber"].ToString(),
                                dsResult.Tables[0].Rows[0]["CCANumber"].ToString(),
                                dsResult.Tables[0].Rows[0]["CCADate"].ToString(),
                                dsResult.Tables[1].Rows[0]["AgentCode"].ToString(),
                                dsResult.Tables[1].Rows[0]["AgentName"].ToString(),
                                dsResult.Tables[1].Rows[0]["AgentAddress"].ToString(),
                                dsResult.Tables[2].Rows[0]["GrossWeight"].ToString(),
                                dsResult.Tables[2].Rows[0]["ChargableWeight"].ToString(),
                                dsResult.Tables[2].Rows[0]["FreightRate"].ToString(),
                                dsResult.Tables[2].Rows[0]["OCDC"].ToString(),
                                dsResult.Tables[2].Rows[0]["OCDA"].ToString(),
                                dsResult.Tables[2].Rows[0]["ServiceTax"].ToString(),
                                dsResult.Tables[2].Rows[0]["Commission"].ToString(),
                                dsResult.Tables[2].Rows[0]["TDSCommission"].ToString(),
                                dsResult.Tables[2].Rows[0]["STCommission"].ToString(),
                                dsResult.Tables[2].Rows[0]["CurrentTotal"].ToString(),
                                dsResult.Tables[2].Rows[0]["RevisedGrossWeight"].ToString(),
                                dsResult.Tables[2].Rows[0]["RevisedChargableWeight"].ToString(),
                                dsResult.Tables[2].Rows[0]["RevisedFreightRate"].ToString(),
                                dsResult.Tables[2].Rows[0]["RevisedOCDC"].ToString(),
                                dsResult.Tables[2].Rows[0]["RevisedOCDA"].ToString(),
                                dsResult.Tables[2].Rows[0]["RevisedServiceTax"].ToString(),
                                dsResult.Tables[2].Rows[0]["RevisedCommission"].ToString(),
                                dsResult.Tables[2].Rows[0]["RevisedTDSCommission"].ToString(),
                                dsResult.Tables[2].Rows[0]["RevisedSTCommission"].ToString(),
                                dsResult.Tables[2].Rows[0]["RevisedTotal"].ToString(),
                                dsResult.Tables[2].Rows[0]["CCA"].ToString(),
                                dsResult.Tables[2].Rows[0]["Type"].ToString(),
                                dsResult.Tables[3].Rows[0]["FlightNumber"].ToString(),
                                dsResult.Tables[3].Rows[0]["FlightDate"].ToString(),
                                dsResult.Tables[3].Rows[0]["Origin"].ToString(),
                                dsResult.Tables[3].Rows[0]["Destination"].ToString(),
                                dsResult.Tables[3].Rows[0]["AWBDate"].ToString(),
                                dsResult.Tables[2].Rows[0]["Remarks"].ToString(),
                                dsResult.Tables[4].Rows[0]["STNumber"].ToString(),
                                Logo.ToArray());


                                Session["ShowExcel"] = DTExport;
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
        protected void SaveAgentCCA()
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
                //FileStream fs = new FileStream(@"c:\SCMAgentInvoices\" + Session["CurrentAWBNumber"].ToString() + ".xls", FileMode.Create);
                FileStream fs = new FileStream(System.Configuration.ConfigurationManager.AppSettings["InvoicePath"] + "//" + Session["CurrentAWBNumber"].ToString() + ".xls", FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();

                //Code to open save dialog box
                String FileName = Session["CurrentAWBNumber"].ToString() + ".xls";
                //String FilePath = @"c:\SCMAgentInvoices\" + Session["CurrentAWBNumber"].ToString() + ".xls"; //Replace this
                String FilePath = System.Configuration.ConfigurationManager.AppSettings["InvoicePath"] + "//" + Session["CurrentAWBNumber"].ToString() + ".xls";
                System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                response.ClearContent();
                response.Clear();
                //response.ContentType = "text/plain";
                response.ContentType = "application/vnd.xls";
                //response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + ";");

                response.TransmitFile(FilePath);
                response.Flush();
                response.End();


                ////Code to send it as attachment in mail
                //SendMail();

                //Code to Delete Agent Invoice xls file
                //string filePath = @"c:\SCMAgentInvoices\" + Session["CurrentAWBNumber"].ToString() + ".xls";
                //if (System.IO.File.Exists(filePath))
                //{
                //    System.IO.File.Delete(filePath);
                //}

            }
            catch (Exception ex)
            {
            }

        }

        protected void SaveAgentCCAPDF()
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
                //FileStream fs = new FileStream(@"c:\SCMAgentInvoices\" + Session["CurrentAWBNumber"].ToString() + ".pdf", FileMode.Create);
                FileStream fs = new FileStream(System.Configuration.ConfigurationManager.AppSettings["InvoicePath"] + "\\\\" + Session["CurrentAWBNumber"].ToString() + ".pdf", FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();

                //Code to open save dialog box
                String FileName = Session["CurrentAWBNumber"].ToString() + ".pdf";
                //String FilePath = @"c:\SCMAgentInvoices\" + Session["CurrentAWBNumber"].ToString() + ".pdf"; //Replace this
                String FilePath = System.Configuration.ConfigurationManager.AppSettings["InvoicePath"] + "\\\\" + Session["CurrentAWBNumber"].ToString() + ".pdf";
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
                //string filePath = @"c:\SCMAgentInvoices\" + Session["CurrentAWBNumber"].ToString() + ".xls";
                //if (System.IO.File.Exists(filePath))
                //{
                //    System.IO.File.Delete(filePath);
                //}

            }
            catch (Exception ex)
            {
            }

        }

    }
}
