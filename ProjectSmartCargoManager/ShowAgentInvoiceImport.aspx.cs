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
    public partial class ShowAgentInvoiceImport : System.Web.UI.Page
    {
        ReportDataSource rds1 = new ReportDataSource();
        ReportDataSource rds2 = new ReportDataSource();
        DataTable dtTable1 = new DataTable();
        DataTable dtTable2 = new DataTable();
        DataSet dsResult;
        BAL.BALInvoiceListing objBAL = new BAL.BALInvoiceListing();

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

                rep1.ReportPath = Server.MapPath("/Reports/AgentInvoiceImport.rdlc");
               // rep1.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "AgentInvoiceImport.rdlc";
                rds1.Name = "dsAgentInvoiceImport_DataTable1";
                rds1.Value = dtTable1;
                rep1.DataSources.Add(rds1);

                ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);

                //Code to Save the generated Excel file
                SaveAgentInvoice();

                //Closing current tab
                Response.Write("<script type='text/javascript' language='javascript'>window.close();</script>");

            }
            catch (Exception ex)
            {

            }
           
        }

        protected void GenerateAgentInvoice(object[] InvoiceNum)
        {
            dsResult = objBAL.GetInvoiceDataImport(InvoiceNum);

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
                                Session["CurrentInvoiceNo"] = dsResult.Tables[0].Rows[0]["InvoiceNumber"].ToString();

                                Session["ShowExcel"] = "";

                                DataTable DTExport = new DataTable();

                                DTExport.Columns.Add("AgentName");
                                DTExport.Columns.Add("AgentAddress1");
                                DTExport.Columns.Add("AgentAddress2");
                                DTExport.Columns.Add("AgentAddress3");
                                DTExport.Columns.Add("InvoiceNumber");
                                DTExport.Columns.Add("InvoiceDate");
                                DTExport.Columns.Add("ServiceTaxNo");
                                DTExport.Columns.Add("PANNo");
                                DTExport.Columns.Add("BillMonthYr");
                                DTExport.Columns.Add("ChargeableWt");
                                DTExport.Columns.Add("Freight");
                                DTExport.Columns.Add("FSC");
                                DTExport.Columns.Add("AHC");
                                DTExport.Columns.Add("FACHC");
                                DTExport.Columns.Add("AWBDO");
                                DTExport.Columns.Add("DueAgent");
                                DTExport.Columns.Add("SubTotal");
                                DTExport.Columns.Add("ServiceTax");
                                DTExport.Columns.Add("EduCess");
                                DTExport.Columns.Add("HighEduCess");
                                DTExport.Columns.Add("TotalInvoiceValue");
                                DTExport.Columns.Add("TotalInvoiceWords");


                                DTExport.Rows.Add(
                                    dsResult.Tables[0].Rows[0]["AgentName"].ToString(),
                                    dsResult.Tables[0].Rows[0]["AgentAddress1"].ToString(),
                                    dsResult.Tables[0].Rows[0]["AgentAddress2"].ToString(),
                                    dsResult.Tables[0].Rows[0]["AgentAddress3"].ToString(),
                                    dsResult.Tables[0].Rows[0]["InvoiceNumber"].ToString(),
                                    dsResult.Tables[0].Rows[0]["InvoiceDate"].ToString(),
                                    dsResult.Tables[0].Rows[0]["ServiceTaxNo"].ToString(),
                                    dsResult.Tables[0].Rows[0]["PANNo"].ToString(),
                                    dsResult.Tables[0].Rows[0]["BillMonthYr"].ToString(),
                                    dsResult.Tables[0].Rows[0]["ChargeableWt"].ToString(),
                                    dsResult.Tables[0].Rows[0]["Freight"].ToString(),
                                    dsResult.Tables[0].Rows[0]["FSC"].ToString(),
                                    dsResult.Tables[0].Rows[0]["AHC"].ToString(),
                                    dsResult.Tables[0].Rows[0]["FACHC"].ToString(),
                                    dsResult.Tables[0].Rows[0]["AWBDO"].ToString(),
                                    dsResult.Tables[0].Rows[0]["DueAgent"].ToString(),
                                    dsResult.Tables[0].Rows[0]["SubTotal"].ToString(),
                                    dsResult.Tables[0].Rows[0]["ServiceTax"].ToString(),
                                    dsResult.Tables[0].Rows[0]["EduCess"].ToString(),
                                    dsResult.Tables[0].Rows[0]["HighEduCess"].ToString(),
                                    dsResult.Tables[0].Rows[0]["TotalInvoiceValue"].ToString(),
                                    dsResult.Tables[0].Rows[0]["TotalInvoiceWords"].ToString());


                                Session["ShowExcel"] = DTExport;

                                Session["AWBData"] = "";
                                DataTable DTAWBData = new DataTable();
                                DTAWBData = dsResult.Tables[1];
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
            string path = @"C:\SCMAgentInvoices";
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
                FileStream fs = new FileStream(@"c:\SCMAgentInvoices\" + Session["CurrentInvoiceNo"].ToString() + ".xls", FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();

                //Code to open save dialog box
                String FileName = Session["CurrentInvoiceNo"].ToString() + ".xls";
                String FilePath = @"c:\SCMAgentInvoices\" + Session["CurrentInvoiceNo"].ToString() + ".xls"; //Replace this
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
            catch(Exception ex)
            {
            }

        }

        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsAgentInvoiceImport_DataTable2", dtTable2));
        }

    }
}
