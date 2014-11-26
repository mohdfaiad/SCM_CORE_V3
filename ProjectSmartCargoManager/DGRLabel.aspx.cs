using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using BAL;
//using iTextSharp.text;
//using iTextSharp.text.html.simpleparser;
//using iTextSharp.text.pdf;

namespace ProjectSmartCargoManager
{
    public partial class DGRLabel : System.Web.UI.Page
    {
        MasterBAL objbal = new MasterBAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["TotPcs"].ToString() != "" && Request.QueryString["UNID"].ToString() != "")
                    {
                        string client = objbal.clientName();
                        //int pcs = int.Parse(Request.QueryString["Pcs"].ToString());
                        string unid = Request.QueryString["UNID"].ToString();
                        string org = Request.QueryString["org"].ToString();
                        string dest = Request.QueryString["dest"].ToString();
                        string awb = Request.QueryString["AWB"].ToString();
                        string totpcs = Request.QueryString["TotPcs"].ToString();

                        int FrmPcs = int.Parse(Request.QueryString["Frm"].ToString());
                        int ToPcs = int.Parse(Request.QueryString["To"].ToString());

                        DataTable dt = new DataTable();
                        dt.Columns.Add("DGRImg", typeof(string));
                        dt.Columns.Add("AWB", typeof(string));
                        dt.Columns.Add("Pcs", typeof(string));
                        dt.Columns.Add("Org", typeof(string));
                        dt.Columns.Add("Dest", typeof(string));
                        dt.Columns.Add("Time", typeof(string));
                        dt.Columns.Add("Client", typeof(string));
                        for (int i = FrmPcs; i <= ToPcs; i++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["DGRImg"] = "~\\Images\\DGR\\" + unid + ".png";
                            dr["AWB"] = awb;
                            dr["Pcs"] = (i).ToString() + "/" + totpcs.ToString();
                            dr["Org"] = org;
                            dr["Dest"] = dest;
                            dr["Time"] = Session["IT"].ToString();
                            dr["Client"] = client;
                            dt.Rows.Add(dr);
                            
                        }
                        //GrdDGRLbl.DataSource = dt;
                        //GrdDGRLbl.DataBind();
                        Repeater1.DataSource = dt; 
                        Repeater1.DataBind();
                           
                    }
                    
                }
            }
            catch (Exception ex) { }
        }

        protected void btnPrintLbl_Click(object sender, EventArgs e)
        {
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("content-disposition", "attachment;filename=TestPage.pdf");
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(sw);
            //this.Page.RenderControl(hw);
            //StringReader sr = new StringReader(sw.ToString());
            //Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
            //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            //pdfDoc.Open();
            //htmlparser.Parse(sr);
            //pdfDoc.Close();
            //Response.Write(pdfDoc);
            //Response.End();
            //Render(hw);
        }
        //protected override void Render(HtmlTextWriter writer)
        //{
        //    MemoryStream mem = new MemoryStream();
        //    StreamWriter twr = new StreamWriter(mem);
        //    HtmlTextWriter myWriter = new HtmlTextWriter(twr);
        //    base.Render(myWriter);
        //    myWriter.Flush();
        //    myWriter.Dispose();
        //    StreamReader strmRdr = new StreamReader(mem);
        //    strmRdr.BaseStream.Position = 0;
        //    string pageContent = strmRdr.ReadToEnd();
        //    strmRdr.Dispose();
        //    mem.Dispose();
        //    writer.Write(pageContent);
        //    CreatePDFDocument(pageContent);
        //}
        //public void CreatePDFDocument(string strHtml)
        //{

        //    string strFileName = HttpContext.Current.Server.MapPath("test.pdf");
        //    // step 1: creation of a document-object
        //    Document document = new Document();
        //    // step 2:
        //    // we create a writer that listens to the document
        //    PdfWriter.GetInstance(document, new FileStream(strFileName, FileMode.Create));
        //    StringReader se = new StringReader(strHtml);
        //    HTMLWorker obj = new HTMLWorker(document);
        //    document.Open();
        //    obj.Parse(se);
        //    document.Close();
        //    ShowPdf(strFileName);

        //}
        //public void ShowPdf(string strFileName)
        //{
        //    Response.ClearContent();
        //    Response.ClearHeaders();
        //    Response.AddHeader("Content-Disposition", "inline;filename=" + strFileName);
        //    Response.ContentType = "application/pdf";
        //    Response.WriteFile(strFileName);
        //    Response.Flush();
        //    Response.Clear();
        //}
    }
}