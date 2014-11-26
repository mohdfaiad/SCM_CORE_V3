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
using System.Net;
using System.Text;


namespace ProjectSmartCargoManager
{
    public partial class ShowInterlineAirCargoInvoice : System.Web.UI.Page
    {
        ReportDataSource rds1 = new ReportDataSource();
        ReportDataSource rds2 = new ReportDataSource();
        DataTable dtTable1 = new DataTable();
        DataTable dtTable2 = new DataTable();
        DataSet dsResult;
        BAL.BALInterlineInvoiceListing objIntBAL = new BAL.BALInterlineInvoiceListing();
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


                dtTable1 = (DataTable)Session["ShowExcel"];
                dtTable2 = (DataTable)Session["AWBData"];

                ReportViewer1.Visible = true;

                ReportViewer1.Reset();

                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport rep1 = ReportViewer1.LocalReport;

                rep1.ReportPath = Server.MapPath("/Reports/InterlineCargoInvoiceJet.rdlc");
                //rep1.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "CargoInvoice.rdlc";
                rds1.Name = "dsInterlineCargoInvoiceJet_DataTable1";
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

                       "<DeviceInfo><PageHeight>35cm</PageHeight><PageWidth>48cm</PageWidth></DeviceInfo>";

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
            if (Request.QueryString["Type"] == "RegularInvoices")
                dsResult = objIntBAL.GetInterlineInvoiceDataForPrint(InvoiceNum);
            else if (Request.QueryString["Type"] == "RegularCreditNotes")
                dsResult = objIntBAL.GetInterlineCreditNoteDataForPrint(InvoiceNum);
            else if (Request.QueryString["Type"] == "ProformaInvoices")//Dest
                dsResult = objIntBAL.GetInterlineProformaInvoiceDataForPrint(InvoiceNum);
            else //ProformaCreditNotes
                dsResult = objIntBAL.GetInterlineProformaCreditNoteDataForPrint(InvoiceNum);
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
                                Session["CurrentInvoiceNo"] = dsResult.Tables[2].Rows[0]["InvoiceNumber"].ToString();

                                Session["ShowExcel"] = "";

                                DataTable DTExport = new DataTable();

                                DTExport.Columns.Add("FromPartnerName");
                                DTExport.Columns.Add("ToPartnerName");
                                DTExport.Columns.Add("FromAirline");
                                DTExport.Columns.Add("ToAirline");
                                DTExport.Columns.Add("FromCode");
                                DTExport.Columns.Add("ToCode");
                                DTExport.Columns.Add("FromRegNo");
                                DTExport.Columns.Add("ToRegNo");
                                DTExport.Columns.Add("InvoiceType");
                                DTExport.Columns.Add("InvoiceNumber");
                                DTExport.Columns.Add("InvoiceDate");
                                DTExport.Columns.Add("FreightCharges");
                                DTExport.Columns.Add("OtherCharges");
                                DTExport.Columns.Add("ISC");
                                DTExport.Columns.Add("NetBilling");
                                DTExport.Columns.Add("CustomerNumber");
                                DTExport.Columns.Add("CurrOfListing");
                                DTExport.Columns.Add("SettlementsType");
                                DTExport.Columns.Add("ClassOfInvoice");
                                DTExport.Columns.Add("MonYY");
                                DTExport.Columns.Add("Period");
                                DTExport.Columns.Add("CargoBillings");
                                DTExport.Columns.Add("AWBCount");
                                DTExport.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));
                                

                                DTExport.Rows.Add(
                                dsResult.Tables[0].Rows[0]["FromPartnerName"].ToString(),
                                dsResult.Tables[1].Rows[0]["ToPartnerName"].ToString(),
                                dsResult.Tables[0].Rows[0]["FromAirline"].ToString(),
                                dsResult.Tables[1].Rows[0]["ToAirline"].ToString(),
                                dsResult.Tables[0].Rows[0]["FromCode"].ToString(),
                                dsResult.Tables[1].Rows[0]["ToCode"].ToString(),
                                dsResult.Tables[0].Rows[0]["FromRegNo"].ToString(),
                                dsResult.Tables[1].Rows[0]["ToRegNo"].ToString(),
                                dsResult.Tables[2].Rows[0]["InvoiceType"].ToString(),
                                dsResult.Tables[2].Rows[0]["InvoiceNumber"].ToString(),
                                dsResult.Tables[2].Rows[0]["InvoiceDate"].ToString(),
                                dsResult.Tables[2].Rows[0]["FreightCharges"].ToString(),
                                dsResult.Tables[2].Rows[0]["OtherCharges"].ToString(),
                                dsResult.Tables[2].Rows[0]["ISC"].ToString(),
                                dsResult.Tables[2].Rows[0]["NetBilling"].ToString(),
                                dsResult.Tables[2].Rows[0]["CustomerNumber"].ToString(),
                                dsResult.Tables[2].Rows[0]["CurrOfListing"].ToString(),
                                dsResult.Tables[2].Rows[0]["SettlementsType"].ToString(),
                                dsResult.Tables[2].Rows[0]["ClassOfInvoice"].ToString(),
                                dsResult.Tables[3].Rows[0]["MonYY"].ToString(),
                                dsResult.Tables[3].Rows[0]["Period"].ToString(),
                                dsResult.Tables[3].Rows[0]["CargoBillings"].ToString(),
                                dsResult.Tables[4].Rows[0]["AWBCount"].ToString(),
                                Logo.ToArray());

                                Session["ShowExcel"] = DTExport;

                                Session["AWBData"] = "";
                                DataTable DTAWBData = new DataTable();
                                DTAWBData = dsResult.Tables[5];
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

        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsInterlineCargoInvoiceJet_DataTable2", dtTable2));
        }
    }
}
