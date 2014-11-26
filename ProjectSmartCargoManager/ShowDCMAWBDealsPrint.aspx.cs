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
    public partial class ShowDCMAWBDealsPrint : System.Web.UI.Page
    {
        ReportDataSource rds1 = new ReportDataSource();
        DataTable dtTable1 = new DataTable();
        DataSet dsResult;
        BAL.BALDCMListing objBAL = new BAL.BALDCMListing();
        string format = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string InvNo = Request.QueryString["InvoiceNO"];

            try
            {
                GenerateAgentInvoice(InvNo);


                dtTable1 = (DataTable)Session["ShowExcel"];

                ReportViewer1.Visible = true;

                ReportViewer1.Reset();

                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport rep1 = ReportViewer1.LocalReport;

                rep1.ReportPath = Server.MapPath("/Reports/DCMperAWBPrint.rdlc");
                //rep1.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "CargoInvoice.rdlc";
                rds1.Name = "dsDCMperAWBPrint_DataTable1";
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
                //    SaveAgentDCM();
                //else
                //    SaveAgentDCMPDF();

                ////Closing current tab
                //Response.Write("<script type='text/javascript' language='javascript'>window.close();</script>");

            }
            catch (Exception ex)
            {

            }
        }

        protected void GenerateAgentInvoice(string InvoiceNo)
        {
            dsResult = objBAL.GetDCMAWBDealsPrintDetails(InvoiceNo);
            Session["ShowExcel"] = null;
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
                                Session["CurrentAWBNumber"] = dsResult.Tables[2].Rows[0]["InvoiceNumber"].ToString();

                                Session["ShowExcel"] = "";

                                DataTable DTExport = new DataTable();

                                DTExport.Columns.Add("AWBPrefix");
                                DTExport.Columns.Add("AWBNumber");
                                DTExport.Columns.Add("DCMNumber");
                                DTExport.Columns.Add("DCMDate");
                                DTExport.Columns.Add("AgentCode");
                                DTExport.Columns.Add("AgentName");
                                DTExport.Columns.Add("AgentAddress");
                                DTExport.Columns.Add("FlightNo");
                                DTExport.Columns.Add("GrossWeight");
                                DTExport.Columns.Add("ChargableWeight");
                                DTExport.Columns.Add("RatePerKg");
                                DTExport.Columns.Add("FreightRate");
                                DTExport.Columns.Add("OCDC");
                                DTExport.Columns.Add("OCDA");
                                DTExport.Columns.Add("ServiceTax");
                                DTExport.Columns.Add("Commission");
                                DTExport.Columns.Add("TDSCommission");
                                DTExport.Columns.Add("STCommission");
                                DTExport.Columns.Add("CurrentTotal");
                                DTExport.Columns.Add("RevisedFlightNo");
                                DTExport.Columns.Add("RevisedGrossWeight");
                                DTExport.Columns.Add("RevisedChargableWeight");
                                DTExport.Columns.Add("RevisedRatePerKg");
                                DTExport.Columns.Add("RevisedFreightRate");
                                DTExport.Columns.Add("RevisedOCDC");
                                DTExport.Columns.Add("RevisedOCDA");
                                DTExport.Columns.Add("RevisedServiceTax");
                                DTExport.Columns.Add("RevisedCommission");
                                DTExport.Columns.Add("RevisedTDSCommission");
                                DTExport.Columns.Add("RevisedSTCommission");
                                DTExport.Columns.Add("RevisedTotal");
                                DTExport.Columns.Add("DCM");
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
                                dsResult.Tables[2].Rows[0]["InvoiceNumber"].ToString(),
                                dsResult.Tables[0].Rows[0]["DCMNumber"].ToString(),
                                dsResult.Tables[0].Rows[0]["DCMDate"].ToString(),
                                dsResult.Tables[1].Rows[0]["AgentCode"].ToString(),
                                dsResult.Tables[1].Rows[0]["AgentName"].ToString(),
                                dsResult.Tables[1].Rows[0]["AgentAddress"].ToString(),
                                dsResult.Tables[2].Rows[0]["FlightNo"].ToString(),
                                dsResult.Tables[2].Rows[0]["GrossWeight"].ToString(),
                                dsResult.Tables[2].Rows[0]["ChargableWeight"].ToString(),
                                dsResult.Tables[2].Rows[0]["RatePerKg"].ToString(),
                                dsResult.Tables[2].Rows[0]["FreightRate"].ToString(),
                                dsResult.Tables[2].Rows[0]["OCDC"].ToString(),
                                dsResult.Tables[2].Rows[0]["OCDA"].ToString(),
                                dsResult.Tables[2].Rows[0]["ServiceTax"].ToString(),
                                dsResult.Tables[2].Rows[0]["Commission"].ToString(),
                                dsResult.Tables[2].Rows[0]["TDSCommission"].ToString(),
                                dsResult.Tables[2].Rows[0]["STCommission"].ToString(),
                                dsResult.Tables[2].Rows[0]["CurrentTotal"].ToString(),
                                dsResult.Tables[2].Rows[0]["RevisedFlightNo"].ToString(),
                                dsResult.Tables[2].Rows[0]["RevisedGrossWeight"].ToString(),
                                dsResult.Tables[2].Rows[0]["RevisedChargableWeight"].ToString(),
                                dsResult.Tables[2].Rows[0]["RevisedRatePerKg"].ToString(),
                                dsResult.Tables[2].Rows[0]["RevisedFreightRate"].ToString(),
                                dsResult.Tables[2].Rows[0]["RevisedOCDC"].ToString(),
                                dsResult.Tables[2].Rows[0]["RevisedOCDA"].ToString(),
                                dsResult.Tables[2].Rows[0]["RevisedServiceTax"].ToString(),
                                dsResult.Tables[2].Rows[0]["RevisedCommission"].ToString(),
                                dsResult.Tables[2].Rows[0]["RevisedTDSCommission"].ToString(),
                                dsResult.Tables[2].Rows[0]["RevisedSTCommission"].ToString(),
                                dsResult.Tables[2].Rows[0]["RevisedTotal"].ToString(),
                                dsResult.Tables[2].Rows[0]["DCM"].ToString(),
                                dsResult.Tables[2].Rows[0]["Type"].ToString(),
                                "",
                                "",
                                "",
                                "",
                                "",
                                dsResult.Tables[2].Rows[0]["Remarks"].ToString(),
                                dsResult.Tables[3].Rows[0]["STNumber"].ToString(),
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
    }
}
