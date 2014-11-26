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
    public partial class ShowAirCargoInvoice : System.Web.UI.Page
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

                System.IO.MemoryStream Logo = CommonUtility.GetImageStream(Page.Server);
                dtTable1 = (DataTable)Session["ShowExcel"];
                dtTable2 = (DataTable)Session["AWBData"];
                //dtTable1.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));
                //System.Data.DataColumn Logo = new System.Data.DataColumn("Logo", typeof(System.CannotUnloadAppDomainException));
                //dtTable1.Columns.Add(Logo);


                ReportViewer1.Visible = true;

                ReportViewer1.Reset();

                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport rep1 = ReportViewer1.LocalReport;

                rep1.ReportPath = Server.MapPath("/Reports/CargoInvoiceJet.rdlc");
                //rep1.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "CargoInvoice.rdlc";
                rds1.Name = "dsCargoInvoiceJet_DataTable1";
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
                        string deviceInfo = "<DeviceInfo><PageHeight>35cm</PageHeight><PageWidth>48cm</PageWidth></DeviceInfo>";

                        //"<DeviceInfo>" +

                        //"  <OutputFormat>PDF</OutputFormat>" +

                        //"</DeviceInfo>";

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
            //Invoice printing for CORE(New format)
            if (Request.QueryString["Type"] == "Regular")
                dsResult = objBAL.GetInvoiceDataForPrint(InvoiceNum);
            else if (Request.QueryString["Type"] == "Proforma")
                dsResult = objBAL.GetProformaInvoiceDataForPrint(InvoiceNum);
            else if (Request.QueryString["Type"] == "WalkIn")
                dsResult = objBAL.GetWalkInInvoiceDataForPrint(InvoiceNum);
            else //Dest
                dsResult = objBAL.GetDestInvoiceDataForPrint(InvoiceNum);

            ////Invoice printing for GoAir(Go Air format)
            //if (Request.QueryString["Type"] == "Regular")
            //    dsResult = objBAL.GetInvoiceDataImport(InvoiceNum);
            //else if (Request.QueryString["Type"] == "Proforma")
            //    dsResult = objBAL.GetProformaInvoiceDataImport(InvoiceNum);
            //else if (Request.QueryString["Type"] == "WalkIn")
            //    dsResult = objBAL.GetWalkInAgentInvoiceData(InvoiceNum);
            //else //Dest
            //    dsResult = objBAL.GetDestAgentInvoiceData(InvoiceNum);

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
                                string GLAccountAirport = "", GLAccountFreight = "", GLAccountOCDC = "", GLAccountOCDA = "", GLAccountSecurity = "",
                                 GLAccountFuel = "", GLAccountCartage = "", GLAccountScreening = "", GLAccountMisc = "", GLAccountCommission = "",
                                 GLAccountDiscount = "", GLAccountST = "", GLAccountTaxOnCommission = "", GLAccountTaxOnDiscount = "",
                                 GLAccountTaxOnFreight = "", GLAccountTDS = "";

                                if (dsResult.Tables[4].Rows.Count > 0)
                                    GLAccountAirport = dsResult.Tables[4].Rows[0][0].ToString();
                                if (dsResult.Tables[5].Rows.Count > 0)
                                    GLAccountFreight = dsResult.Tables[5].Rows[0][0].ToString();
                                if (dsResult.Tables[6].Rows.Count > 0)
                                    GLAccountOCDC = dsResult.Tables[6].Rows[0][0].ToString();
                                if (dsResult.Tables[7].Rows.Count > 0)
                                    GLAccountOCDA = dsResult.Tables[7].Rows[0][0].ToString();
                                if (dsResult.Tables[8].Rows.Count > 0)
                                    GLAccountSecurity = dsResult.Tables[8].Rows[0][0].ToString();
                                if (dsResult.Tables[9].Rows.Count > 0)
                                    GLAccountFuel = dsResult.Tables[9].Rows[0][0].ToString();
                                if (dsResult.Tables[10].Rows.Count > 0)
                                    GLAccountCartage = dsResult.Tables[10].Rows[0][0].ToString();
                                if (dsResult.Tables[11].Rows.Count > 0)
                                    GLAccountScreening = dsResult.Tables[11].Rows[0][0].ToString();
                                if (dsResult.Tables[12].Rows.Count > 0)
                                    GLAccountMisc = dsResult.Tables[12].Rows[0][0].ToString();
                                if (dsResult.Tables[13].Rows.Count > 0)
                                    GLAccountCommission = dsResult.Tables[13].Rows[0][0].ToString();
                                if (dsResult.Tables[14].Rows.Count > 0)
                                    GLAccountDiscount = dsResult.Tables[14].Rows[0][0].ToString();
                                if (dsResult.Tables[15].Rows.Count > 0)
                                    GLAccountST = dsResult.Tables[15].Rows[0][0].ToString();
                                if (dsResult.Tables[16].Rows.Count > 0)
                                    GLAccountTaxOnCommission = dsResult.Tables[16].Rows[0][0].ToString();
                                if (dsResult.Tables[17].Rows.Count > 0)
                                    GLAccountTaxOnDiscount = dsResult.Tables[17].Rows[0][0].ToString();
                                if (dsResult.Tables[18].Rows.Count > 0)
                                    GLAccountTaxOnFreight = dsResult.Tables[18].Rows[0][0].ToString();
                                if (dsResult.Tables[19].Rows.Count > 0)
                                    GLAccountTDS = dsResult.Tables[19].Rows[0][0].ToString();


                                Session["CurrentInvoiceNo"] = dsResult.Tables[1].Rows[0]["InvoiceNumber"].ToString();

                                Session["ShowExcel"] = "";

                                DataTable DTExport = new DataTable();

                                DTExport.Columns.Add("ClientName");
                                DTExport.Columns.Add("ClientAddress");
                                DTExport.Columns.Add("PhoneNum");
                                DTExport.Columns.Add("PanCardNumber");
                                DTExport.Columns.Add("ServiceTaxNumber");
                                DTExport.Columns.Add("TanNumber");
                                DTExport.Columns.Add("AgentCode");
                                DTExport.Columns.Add("AgentName");
                                DTExport.Columns.Add("AgentAddress");
                                DTExport.Columns.Add("AgentPanCardNumber");
                                DTExport.Columns.Add("Currency");
                                DTExport.Columns.Add("InvoiceNumber");
                                DTExport.Columns.Add("InvoiceDate");
                                DTExport.Columns.Add("TotalFreightAndValuation");
                                DTExport.Columns.Add("CCCharges");
                                DTExport.Columns.Add("TotalPP");
                                DTExport.Columns.Add("CommOnFreightAndValuation");
                                DTExport.Columns.Add("CreditNotesIncPLI");
                                DTExport.Columns.Add("SpotDiscount");
                                DTExport.Columns.Add("Totalcommission");
                                DTExport.Columns.Add("NetAmount1");
                                DTExport.Columns.Add("DueCarrierPP");
                                DTExport.Columns.Add("FuelSurchargePP");
                                DTExport.Columns.Add("TotalDueCarrierPP");
                                DTExport.Columns.Add("NetAmount2");
                                DTExport.Columns.Add("STOnAWBPP");
                                DTExport.Columns.Add("NetAmount3");
                                DTExport.Columns.Add("STonTotalcommission");
                                DTExport.Columns.Add("CommOnFreightAndValuationST");
                                DTExport.Columns.Add("NetAmount4");
                                DTExport.Columns.Add("TDS194HOnTotalCommission");
                                DTExport.Columns.Add("CommOnFreightAndValuationTDS");
                                DTExport.Columns.Add("CreditNotesIncPLITDS");
                                DTExport.Columns.Add("SpotDiscountTDS");
                                DTExport.Columns.Add("TDSOnServiceTax");
                                DTExport.Columns.Add("TDSSubTotal");
                                DTExport.Columns.Add("NetAmount5");
                                DTExport.Columns.Add("DebitNotes");
                                DTExport.Columns.Add("NetAmount6");
                                DTExport.Columns.Add("DueAgentCC");
                                DTExport.Columns.Add("RecoverableFromAgent");
                                DTExport.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));
                                DTExport.Columns.Add("GLAccountAirport");
                                DTExport.Columns.Add("GLAccountFreight");
                                DTExport.Columns.Add("GLAccountOCDC");
                                DTExport.Columns.Add("GLAccountOCDA");
                                DTExport.Columns.Add("GLAccountSecurity");
                                DTExport.Columns.Add("GLAccountFuel");
                                DTExport.Columns.Add("GLAccountCartage");
                                DTExport.Columns.Add("GLAccountScreening");
                                DTExport.Columns.Add("GLAccountMisc");
                                DTExport.Columns.Add("GLAccountCommission");
                                DTExport.Columns.Add("GLAccountDiscount");
                                DTExport.Columns.Add("GLAccountST");
                                DTExport.Columns.Add("GLAccountTaxOnCommission");
                                DTExport.Columns.Add("GLAccountTaxOnDiscount");
                                DTExport.Columns.Add("GLAccountTaxOnFreight");
                                DTExport.Columns.Add("GLAccountTDS");


                                //DTExport.Rows.Add(
                                //dsResult.Tables[0].Rows[0]["ClientName"].ToString(),
                                //dsResult.Tables[0].Rows[0]["ClientAddress"].ToString(),
                                //dsResult.Tables[0].Rows[0]["PhoneNum"].ToString(),
                                //dsResult.Tables[0].Rows[0]["PanCardNumber"].ToString(),
                                //dsResult.Tables[0].Rows[0]["ServiceTaxNumber"].ToString(),
                                //dsResult.Tables[0].Rows[0]["TanNumber"].ToString(),
                                //dsResult.Tables[1].Rows[0]["AgentCode"].ToString(),
                                //dsResult.Tables[1].Rows[0]["AgentName"].ToString(),
                                //dsResult.Tables[1].Rows[0]["AgentAddress"].ToString(),
                                //dsResult.Tables[1].Rows[0]["AgentPanCardNumber"].ToString(),
                                //dsResult.Tables[1].Rows[0]["Currency"].ToString(),
                                //dsResult.Tables[1].Rows[0]["InvoiceNumber"].ToString(),
                                //dsResult.Tables[1].Rows[0]["InvoiceDate"].ToString(),
                                //dsResult.Tables[3].Rows[0]["TotalFreightAndValuation"].ToString(),                                
                                //dsResult.Tables[3].Rows[0]["CCCharges"].ToString(),
                                //dsResult.Tables[3].Rows[0]["TotalPP"].ToString(),
                                //dsResult.Tables[3].Rows[0]["CommOnFreightAndValuation"].ToString(),
                                //dsResult.Tables[3].Rows[0]["CreditNotesIncPLI"].ToString(),
                                //dsResult.Tables[3].Rows[0]["SpotDiscount"].ToString(),
                                //dsResult.Tables[3].Rows[0]["Totalcommission"].ToString(),
                                //dsResult.Tables[3].Rows[0]["NetAmount1"].ToString(),
                                //dsResult.Tables[3].Rows[0]["DueCarrierPP"].ToString(),
                                //dsResult.Tables[3].Rows[0]["FuelSurchargePP"].ToString(),
                                //dsResult.Tables[3].Rows[0]["TotalDueCarrierPP"].ToString(),
                                //dsResult.Tables[3].Rows[0]["NetAmount2"].ToString(),
                                //dsResult.Tables[3].Rows[0]["STOnAWBPP"].ToString(),
                                //dsResult.Tables[3].Rows[0]["NetAmount3"].ToString(),
                                //dsResult.Tables[3].Rows[0]["STonTotalcommission"].ToString(),
                                //dsResult.Tables[3].Rows[0]["CommOnFreightAndValuationST"].ToString(),
                                //dsResult.Tables[3].Rows[0]["NetAmount4"].ToString(),
                                //dsResult.Tables[3].Rows[0]["TDS194HOnTotalCommission"].ToString(),
                                //dsResult.Tables[3].Rows[0]["CommOnFreightAndValuationTDS"].ToString(),
                                //dsResult.Tables[3].Rows[0]["CreditNotesIncPLITDS"].ToString(),
                                //dsResult.Tables[3].Rows[0]["SpotDiscountTDS"].ToString(),                                
                                //dsResult.Tables[3].Rows[0]["TDSOnServiceTax"].ToString(),                                
                                //dsResult.Tables[3].Rows[0]["TDSSubTotal"].ToString(),
                                //dsResult.Tables[3].Rows[0]["NetAmount5"].ToString(),
                                //dsResult.Tables[3].Rows[0]["DebitNotes"].ToString(),
                                //dsResult.Tables[3].Rows[0]["NetAmount6"].ToString(),
                                //dsResult.Tables[3].Rows[0]["DueAgentCC"].ToString(),
                                //dsResult.Tables[3].Rows[0]["RecoverableFromAgent"],
                                //Logo.ToArray(),
                                //dsResult.Tables[4].Rows[0][0].ToString(),
                                //dsResult.Tables[5].Rows[0][0].ToString(),
                                //dsResult.Tables[6].Rows[0][0].ToString(),
                                //dsResult.Tables[7].Rows[0][0].ToString(),
                                //dsResult.Tables[8].Rows[0][0].ToString(),
                                //dsResult.Tables[9].Rows[0][0].ToString(),
                                //dsResult.Tables[10].Rows[0][0].ToString(),
                                //dsResult.Tables[11].Rows[0][0].ToString(),
                                //dsResult.Tables[12].Rows[0][0].ToString(),
                                //dsResult.Tables[13].Rows[0][0].ToString(),
                                //dsResult.Tables[14].Rows[0][0].ToString(),
                                //dsResult.Tables[15].Rows[0][0].ToString(),
                                //dsResult.Tables[16].Rows[0][0].ToString(),
                                //dsResult.Tables[17].Rows[0][0].ToString(),
                                //dsResult.Tables[18].Rows[0][0].ToString(),
                                //dsResult.Tables[19].Rows[0][0].ToString());

                                DTExport.Rows.Add(
                                dsResult.Tables[0].Rows[0]["ClientName"].ToString(),
                                dsResult.Tables[0].Rows[0]["ClientAddress"].ToString(),
                                dsResult.Tables[0].Rows[0]["PhoneNum"].ToString(),
                                dsResult.Tables[0].Rows[0]["PanCardNumber"].ToString(),
                                dsResult.Tables[0].Rows[0]["ServiceTaxNumber"].ToString(),
                                dsResult.Tables[0].Rows[0]["TanNumber"].ToString(),
                                dsResult.Tables[1].Rows[0]["AgentCode"].ToString(),
                                dsResult.Tables[1].Rows[0]["AgentName"].ToString(),
                                dsResult.Tables[1].Rows[0]["AgentAddress"].ToString(),
                                dsResult.Tables[1].Rows[0]["AgentPanCardNumber"].ToString(),
                                dsResult.Tables[1].Rows[0]["Currency"].ToString(),
                                dsResult.Tables[1].Rows[0]["InvoiceNumber"].ToString(),
                                dsResult.Tables[1].Rows[0]["InvoiceDate"].ToString(),
                                dsResult.Tables[3].Rows[0]["TotalFreightAndValuation"].ToString(),
                                dsResult.Tables[3].Rows[0]["CCCharges"].ToString(),
                                dsResult.Tables[3].Rows[0]["TotalPP"].ToString(),
                                dsResult.Tables[3].Rows[0]["CommOnFreightAndValuation"].ToString(),
                                dsResult.Tables[3].Rows[0]["CreditNotesIncPLI"].ToString(),
                                dsResult.Tables[3].Rows[0]["SpotDiscount"].ToString(),
                                dsResult.Tables[3].Rows[0]["Totalcommission"].ToString(),
                                dsResult.Tables[3].Rows[0]["NetAmount1"].ToString(),
                                dsResult.Tables[3].Rows[0]["DueCarrierPP"].ToString(),
                                dsResult.Tables[3].Rows[0]["FuelSurchargePP"].ToString(),
                                dsResult.Tables[3].Rows[0]["TotalDueCarrierPP"].ToString(),
                                dsResult.Tables[3].Rows[0]["NetAmount2"].ToString(),
                                dsResult.Tables[3].Rows[0]["STOnAWBPP"].ToString(),
                                dsResult.Tables[3].Rows[0]["NetAmount3"].ToString(),
                                dsResult.Tables[3].Rows[0]["STonTotalcommission"].ToString(),
                                dsResult.Tables[3].Rows[0]["CommOnFreightAndValuationST"].ToString(),
                                dsResult.Tables[3].Rows[0]["NetAmount4"].ToString(),
                                dsResult.Tables[3].Rows[0]["TDS194HOnTotalCommission"].ToString(),
                                dsResult.Tables[3].Rows[0]["CommOnFreightAndValuationTDS"].ToString(),
                                dsResult.Tables[3].Rows[0]["CreditNotesIncPLITDS"].ToString(),
                                dsResult.Tables[3].Rows[0]["SpotDiscountTDS"].ToString(),
                                dsResult.Tables[3].Rows[0]["TDSOnServiceTax"].ToString(),
                                dsResult.Tables[3].Rows[0]["TDSSubTotal"].ToString(),
                                dsResult.Tables[3].Rows[0]["NetAmount5"].ToString(),
                                dsResult.Tables[3].Rows[0]["DebitNotes"].ToString(),
                                dsResult.Tables[3].Rows[0]["NetAmount6"].ToString(),
                                dsResult.Tables[3].Rows[0]["DueAgentCC"].ToString(),
                                dsResult.Tables[3].Rows[0]["RecoverableFromAgent"],
                                Logo.ToArray(),
                                GLAccountAirport, 
                                GLAccountFreight, 
                                GLAccountOCDC, 
                                GLAccountOCDA, 
                                GLAccountSecurity,
                                GLAccountFuel, 
                                GLAccountCartage, 
                                GLAccountScreening, 
                                GLAccountMisc, 
                                GLAccountCommission,
                                GLAccountDiscount, 
                                GLAccountST, 
                                GLAccountTaxOnCommission, 
                                GLAccountTaxOnDiscount,
                                GLAccountTaxOnFreight, 
                                GLAccountTDS);

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

        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsCargoInvoiceJet_DataTable2", dtTable2));
        }
    }
}
