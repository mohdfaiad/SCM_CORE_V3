using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using QID.DataAccess;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Data.SqlClient;
using BAL;
using System.Net.Mail;
using System.Net;
using System.Diagnostics;

namespace ProjectSmartCargoManager
{
    public partial class ePouchNew : System.Web.UI.Page
    {
        SQLServer db = new SQLServer(Global.GetConnectionString());
        static int imagecount = 0;
        EpouchBAL objEpouch = new EpouchBAL();

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    btnPrint.Visible = false;
                    Session["DocumentsData"] = null;
                    Session["Attachment"] = null;
                    Session["DocumentNames"] = null;
                    Session["Extension"] = null;
                    Session["DocumentType"] = null;
                    string AWBPrefix = "";
                    string AWBNo = "";
                    if (Session["ePouchAWBNo"] != null)
                    {

                        AWBNo = Session["ePouchAWBNo"].ToString();
                        if (AWBNo.Length > 8)
                        {
                            AWBPrefix = AWBNo.Substring(0, AWBNo.Length - 8);

                            AWBNo = AWBNo.Substring(AWBNo.Length - 8);
                        }
                        LoadingPopupGrid(grdePouch, AWBPrefix, AWBNo);
                        Session["ePouchAWBNo"] = null;

                    }
                    else
                    {
                        btnShow.Visible = true;
                        txtAWBPrefix.ReadOnly = false;
                        txtAWBNo.ReadOnly = false;
                        btnDisplay.Visible = false;
                        btnSend.Visible = false;
                        btnPrint.Visible = false;
                        //btnAdd.Visible = false;
                    }
                    if (Session["awbPrefix"] != null)
                        txtAWBPrefix.Text = Session["awbPrefix"].ToString();
                    else
                    {
                        MasterBAL objBal = new MasterBAL();
                        Session["awbPrefix"] = objBal.awbPrefix();
                        txtAWBPrefix.Text = Session["awbPrefix"].ToString();
                    }
                    //  GetPrefix();


                }
            }
            catch (Exception ex)
            { }

        }
        #endregion


        #region Loading Popup Grid Details
        public void LoadingPopupGrid(GridView gridview, string AWBPrefix, string AWBNo)
        {
            DataSet ds = new DataSet("ePouch_ds");

            try
            {
                string AgentCode = string.Empty;

                if (Convert.ToString(Session["AgentCode"]) != "")
                    AgentCode = Convert.ToString(Session["AgentCode"]);

                string[] param = { "AWBNo", "AgentCode" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { AWBPrefix + AWBNo, AgentCode };

                ds = db.SelectRecords("spGetDocumentDetailsAsPerAWB_TestNew", param, values, sqldbtype);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            gridview.DataSource = ds;
                            gridview.DataBind();
                            txtAWBPrefix.Text = AWBPrefix;
                            txtAWBNo.Text = AWBNo;
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                txtEmailID.Text = ds.Tables[1].Rows[0][0].ToString();
                            }
                            btnDisplay.Visible = true;
                            btnSend.Visible = true;
                            //btnAdd.Visible = true;
                            btnPrint.Visible = false;
                        }

                    }
                    else
                    {
                        lblStatus.Text = "AWB does not exist!";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                }

            }
            catch (Exception ex)
            { }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
        }

        #endregion

        #region Button Display
        protected void btnDisplay_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet("ePouch_ds1");

            try
            {
                btnPrint.Visible = false;
                Session["DocumentsData"] = null;
                InvoiceImage.ImageUrl = "";
                lblStatus.Text = "";
                InvoiceImage.Visible = false;
                btnNext.Visible = false;
                btnPrev.Visible = false;
                lblPageNo.Text = "";
                int count = 0;
                for (int i = 0; i < grdePouch.Rows.Count; i++)
                {
                    if (((RadioButton)(grdePouch.Rows[i].FindControl("rdbePouch"))).Checked)
                    {
                        count++;
                    }
                }

                if (count == 1)
                {
                    for (int i = 0; i < grdePouch.Rows.Count; i++)
                    {
                        if (((RadioButton)(grdePouch.Rows[i].FindControl("rdbePouch"))).Checked)
                        {
                            string ImageID = ((Button)(grdePouch.Rows[i].FindControl("btnUpload"))).CommandArgument.ToString();
                            string AWBNo = ((Label)(grdePouch.Rows[i].FindControl("AWBNo"))).Text;
                            string AirlinePrefix = ((Label)(grdePouch.Rows[i].FindControl("AirlinePrefix"))).Text;
                            string DocumentType = ((Label)(grdePouch.Rows[i].FindControl("lbldocumentName"))).Text;
                            string Uploaded = ((Label)(grdePouch.Rows[i].FindControl("lblIsUploaded"))).Text;

                            if (Uploaded != "N")
                            {
                                #region Commented Code for Retrieving Database files
                                //if (ImageID != "0")
                                //{
                                //    string FileName = AirlinePrefix + "-" + AWBNo + "-" + DocumentType + "-" + DocumentNo;
                                //    if (File.Exists(Server.MapPath(ConfigurationManager.AppSettings["DocumentsPath"].ToString() +FileName+".pdf")))
                                //    {

                                //        InvoiceImage.Visible = false;
                                //        //InvoiceImage.ImageUrl = "~/Images/nopreview.jpg";
                                //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>window.open('Documents/"+FileName+".pdf');</SCRIPT>", false);
                                //        imagecount = Convert.ToInt32(DocumentNo);
                                //        btnNext.Visible = true;
                                //        btnPrev.Visible = true;
                                //        lblPageNo.Text = "Page : " + imagecount.ToString();

                                //    }
                                //    else
                                //        if (File.Exists(Server.MapPath(ConfigurationManager.AppSettings["DocumentsPath"].ToString() + FileName + ".png")))
                                //        {
                                //            InvoiceImage.ImageUrl = ConfigurationManager.AppSettings["DocumentsPath"].ToString() + FileName + ".png";
                                //            InvoiceImage.Visible = true;
                                //            btnNext.Visible = true;
                                //            btnPrev.Visible = true;
                                //            imagecount = Convert.ToInt32(DocumentNo);
                                //            lblPageNo.Text = "Page : " + imagecount.ToString();
                                //        }
                                //        else
                                //        {
                                //            lblStatus.ForeColor = Color.Red;
                                //            lblStatus.Text = "No Image Found";
                                //            InvoiceImage.Visible = false;
                                //            btnNext.Visible = false;
                                //            btnPrev.Visible =false;
                                //            lblPageNo.Text = "";

                                //        }
                                //}
                                //else
                                //{
                                //    lblStatus.ForeColor = Color.Red;
                                //    lblStatus.Text = "No Image Found";
                                //    InvoiceImage.Visible = false;
                                //    btnNext.Visible = false;
                                //    btnPrev.Visible = false;
                                //    lblPageNo.Text = "";

                                //}
                                #endregion
                                string[] QueryNames = new string[2];
                                object[] QueryValues = new object[2];
                                SqlDbType[] QueryTypes = new SqlDbType[2];

                                QueryNames[0] = "DocID";
                                QueryNames[1] = "DocumentType";

                                QueryTypes[0] = SqlDbType.BigInt;
                                QueryTypes[1] = SqlDbType.VarChar;

                                QueryValues[0] = Convert.ToInt32(ImageID);
                                QueryValues[1] = DocumentType;
                                ds = db.SelectRecords("SP_GetePouchDocuments", QueryNames, QueryValues, QueryTypes);

                                if (ds != null)
                                {
                                    if (ds.Tables.Count > 0)
                                    {
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            imagecount = ds.Tables[0].Rows.Count;
                                            Session["DocumentsData"] = ds;
                                            btnPrint.Visible = true;
                                            if (ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"].ToString() == "png" || ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"].ToString() == "gif" || ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"].ToString() == "jpg" || ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"].ToString() == "jpeg")
                                            {
                                                InvoiceImage.ImageUrl = "~/ShowImage.ashx?id=" + ds.Tables[0].Rows[imagecount - 1]["SerialNumber"].ToString();
                                                InvoiceImage.Visible = true;
                                                btnNext.Visible = true;
                                                btnPrev.Visible = true;
                                                lblPageNo.Text = "Page : " + imagecount.ToString();
                                            }
                                            else
                                            {
                                                InvoiceImage.Visible = false;
                                                btnNext.Visible = true;
                                                btnPrev.Visible = true;
                                                lblPageNo.Text = "Page : " + imagecount.ToString();
                                                //System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                                                //response.ClearContent();
                                                //response.Clear();
                                                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                                //response.ContentType = "text/plain";
                                                //response.AddHeader("Content-Disposition", "attachment; filename=" + ds.Tables[0].Rows[imagecount-1]["DocumentName"] + ".pdf");
                                                //response.BinaryWrite((byte[])ds.Tables[0].Rows[imagecount-1]["Document"]);
                                                //response.Flush();
                                                //response.End();
                                                //byte[] filedata = (byte[])ds.Tables[0].Rows[imagecount - 1]["Document"];
                                                //string extension = ".pdf";// "pdf", etc
                                                //string filename = System.IO.Path.GetTempFileName() + "." + extension;
                                                //// string filename = Server.MapPath("Images/")+ds.Tables[0].Rows[imagecount - 1]["DocumentName"] + "." + extension; // Makes something like "C:\Temp\blah.tmp.pdf"

                                                //File.WriteAllBytes(filename, filedata);

                                                //Process process = new Process();
                                                //process.StartInfo.UseShellExecute = true;
                                                //process.StartInfo.FileName = filename;
                                                //process.Start();
                                                //process.Exited += (s, ee) => System.IO.File.Delete(filename);
                                                // btnPrint_Click(sender, e);
                                                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>window.open('Download.aspx?FileName=" + imagecount + "','Download', 'menubar=0, toolbar=0, location=0, status=0, resizable=0, width=100, height=50');</SCRIPT>", true);
                                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>Download(" + imagecount + ");</SCRIPT>", false);


                                            }
                                        }
                                    }
                                }


                            }
                            else
                            {
                                lblStatus.Text = "No documents uploaded to preview!";
                                lblStatus.ForeColor = Color.Red;
                            }
                        }

                    }

                }

                else
                    if (count > 1)
                    {
                        lblStatus.Text = "Please select a single document for preview!";
                        lblStatus.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblStatus.Text = "Please select a document for preview!";
                        lblStatus.ForeColor = Color.Red;

                    }

            }
            catch (Exception ex)
            { }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
            }

        }
        #endregion

        #region Button Upload
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                btnPrint.Visible = false;
                lblStatus.Text = "";
                int count = 0;
                count++;
                Button clickedButton = (Button)sender;
                GridViewRow row = (GridViewRow)clickedButton.Parent.Parent;
                int i = row.RowIndex;
                bool flag = false;
                //if (((Label)(grdePouch.Rows[i].FindControl("lblIsUploaded"))).Text == "Y")
                //{
                if (((FileUpload)(grdePouch.Rows[i].FindControl("fileupload_ePouch"))).HasFile)
                {
                    if (((FileUpload)(grdePouch.Rows[i].FindControl("fileupload_ePouch"))).PostedFile.ContentLength < 11534336)
                    {
                        string DocumentNo = "";
                        DocumentNo = ((Label)(grdePouch.Rows[i].FindControl("DocumentNo"))).Text;
                        DocumentNo = (Convert.ToInt32(DocumentNo) + 1).ToString();
                        string DocumentName = ((Label)(grdePouch.Rows[i].FindControl("lblDocumentName"))).Text;
                        string AWBNo = ((Label)(grdePouch.Rows[i].FindControl("AWBNo"))).Text;
                        string AirlinePrefix = ((Label)(grdePouch.Rows[i].FindControl("AirlinePrefix"))).Text;
                        string DocumentType = ((Label)(grdePouch.Rows[i].FindControl("DocumentType"))).Text;
                        string filename = "";
                        string Documentfilename = "";
                        //string fextension = ((FileUpload)(grdePouch.Rows[i].FindControl("fileupload_ePouch"))).FileName.Substring(((FileUpload)(grdePouch.Rows[i].FindControl("fileupload_ePouch"))).FileName.Length - 4);

                        Documentfilename = AirlinePrefix + "-" + AWBNo + "-" + DocumentType + "-" + DocumentNo;



                        #region Converting file to binary data
                        try
                        {
                            string Docfilename = ((FileUpload)(grdePouch.Rows[i].FindControl("fileupload_ePouch"))).FileName;
                            byte[] Document = new byte[((FileUpload)(grdePouch.Rows[i].FindControl("fileupload_ePouch"))).PostedFile.ContentLength];
                            HttpPostedFile uploadedDocument = ((FileUpload)(grdePouch.Rows[i].FindControl("fileupload_ePouch"))).PostedFile;
                            uploadedDocument.InputStream.Read(Document, 0, (int)((FileUpload)(grdePouch.Rows[i].FindControl("fileupload_ePouch"))).PostedFile.ContentLength);
                            //string extension = ((FileUpload)(grdePouch.Rows[i].FindControl("fileupload_ePouch"))).FileName.Substring(((FileUpload)(grdePouch.Rows[i].FindControl("fileupload_ePouch"))).FileName.Length - 4);
                            string extension = ((FileUpload)(grdePouch.Rows[i].FindControl("fileupload_ePouch"))).PostedFile.FileName.Substring(((FileUpload)(grdePouch.Rows[i].FindControl("fileupload_ePouch"))).PostedFile.FileName.IndexOf('.')+1);
                            //string[] fileExt = extension.Split('/');
                            //extension = fileExt[1];
                            flag = UpdateDocumentDetails(txtAWBPrefix.Text + txtAWBNo.Text, DocumentName, Session["UserName"].ToString(), DocumentNo, extension, Document, Documentfilename);
                        }
                        catch (Exception ex)
                        { }
                        #endregion
                        if (flag == true)
                        {
                            LoadingPopupGrid(grdePouch, txtAWBPrefix.Text, txtAWBNo.Text);
                            //DocumentNo = ((Label)(grdePouch.Rows[i].FindControl("DocumentNo"))).Text;
                            string DocumentID = ((Button)(grdePouch.Rows[i].FindControl("btnUpload"))).CommandArgument.ToString();
                            string path = ConfigurationManager.AppSettings["DocumentsPath"].ToString();
                            string fextension = ((FileUpload)(grdePouch.Rows[i].FindControl("fileupload_ePouch"))).FileName.Substring(((FileUpload)(grdePouch.Rows[i].FindControl("fileupload_ePouch"))).FileName.IndexOf('.') + 1);
                            if (fextension != "png" || fextension != "gif" || fextension != "jpg" || fextension != "jpeg")
                            {
                                filename = Path.Combine(path, AirlinePrefix + "-" + AWBNo + "-" + DocumentType + "-" + DocumentNo + "." + fextension);

                            }
                            else
                            {
                                filename = Path.Combine(path, AirlinePrefix + "-" + AWBNo + "-" + DocumentType + "-" + DocumentNo + ".png");

                            }
                            // ((FileUpload)(grdePouch.Rows[i].FindControl("fileupload_ePouch"))).SaveAs(Server.MapPath(filename));
                            InvoiceImage.Visible = false;
                            btnNext.Visible = false;
                            btnPrev.Visible = false;
                            lblPageNo.Text = "";
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Text = "File Saved Successfully";
                            //ImgLabel.ImageUrl = "~/Images/1362075092_accept.png";
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Operation failed";
                        }
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "File exceeds maximum limit of 11 MB!";

                    }
                    //}
                    //else
                    //{
                    //    lblStatus.ForeColor = Color.Red;
                    //    lblStatus.Text = "File already exists";

                    //}

                }
                else
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "File not selected to Upload";

                }
            }


            catch (Exception ex)
            { }
        }
        #endregion

        # region Updating Document Details to DB
        public bool UpdateDocumentDetails(string AWBNo, string DocumentName, string UploadedBy, string DocumentNo, string Extension, byte[] Document, string DocumentFileName)
        {
            try
            {
                bool flag = false;
                string[] QueryNames = new string[7];
                object[] QueryValues = new object[7];
                SqlDbType[] QueryTypes = new SqlDbType[7];

                QueryNames[0] = "AWBNo";
                QueryNames[1] = "DocumentName";
                QueryNames[2] = "UploadedBy";
                QueryNames[3] = "DocumentNo";
                QueryNames[4] = "Extension";
                QueryNames[5] = "Document";
                QueryNames[6] = "FileName";

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.VarChar;
                QueryTypes[3] = SqlDbType.VarChar;
                QueryTypes[4] = SqlDbType.VarChar;
                QueryTypes[5] = SqlDbType.VarBinary;
                QueryTypes[6] = SqlDbType.VarChar;


                QueryValues[0] = AWBNo;
                QueryValues[1] = DocumentName;
                QueryValues[2] = UploadedBy;
                QueryValues[3] = DocumentNo;
                QueryValues[4] = Extension;
                QueryValues[5] = Document;
                QueryValues[6] = DocumentFileName;

                flag = db.InsertData("SP_InsertUploadedDocuments", QueryNames, QueryTypes, QueryValues);
                return flag;

            }
            catch (Exception ex)
            { }
            return false;
        }
        #endregion

        #region Button Show
        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                Session["DocumentsData"] = null;
                if (txtAWBPrefix.Text != "" && txtAWBNo.Text != "")
                {
                    InvoiceImage.Visible = false;
                    btnNext.Visible = false;
                    btnPrev.Visible = false;
                    lblPageNo.Text = "";
                    grdePouch.DataSource = null;
                    grdePouch.DataBind();
                    LoadingPopupGrid(grdePouch, txtAWBPrefix.Text, txtAWBNo.Text);
                    btnPrint.Visible = false;


                }
                else
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Enter AWB Number";

                }
            }
            catch (Exception ex)
            { }

        }
        #endregion

        #region Button Ok Click
        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                lblStatus.ForeColor = Color.Red;
                string FromEmailID = ConfigurationManager.AppSettings["FromEmailID"].ToString();
                string EmailPwd = ConfigurationManager.AppSettings["Pass"].ToString();
                //string[] ImageID = (string[])Session["DocumentNames"];
                string[] Extension = (string[])Session["Extension"];
                string documentbody = "";
                string DocumentType = (string)Session["DocumentType"];
                MemoryStream[] Document = (MemoryStream[])Session["Attachment"];
                string[] DocumentName = (string[])Session["DocumentNames"];

                //for (int i = 0; i < DocumentName.Length; i++)
                //{
                //    documentbody += (i+1).ToString() + "." + DocumentName[i] + "\n";
                //}
                documentbody = "1." + DocumentType + "\n";
                string MessageBody = "Hello; \n\nPlease refer to the documents attached with your billing under the Air Waybill Number: " + txtAWBPrefix.Text + txtAWBNo.Text + "\n\nDocument List:\n" + documentbody + "\nThank You,\nSmartKargo\n\nNote: This is a system generated email. Please do not reply. If you were an unintended recipient kindly delete this email.";
                if (txtEmailID.Text != "")
                {
                    string[] emailid = txtEmailID.Text.Split(',');
                    for (int i = 0; i < emailid.Length; i++)
                    {
                        //bool flag = cls_BL.addMsgToOutBox("E-pouch documents for Air Waybill No: " + txtAWBNo.Text, MessageBody, "", emailid[i]);
                        //bool flag = sendMail(FromEmailID, emailid[i], EmailPwd, "E-pouch documents for Air Waybill No: " + txtAWBPrefix.Text + txtAWBNo.Text, MessageBody, false, Document, DocumentName, Extension);
                        
                        //bool flag = cls_BL.addMsgToOutBox("E-pouch documents for Air Waybill No: " + txtAWBPrefix.Text + txtAWBNo.Text, MessageBody, "", emailid[i], Document, DocumentName, Extension);
                        bool flag = cls_BL.addMsgToOutBox("Epouch documents for Air Waybill No: " + txtAWBPrefix.Text + txtAWBNo.Text, MessageBody, "", emailid[i], Document, DocumentName, Extension);

                        if (flag == false)
                        {
                            lblStatus.Text = "Email Sending Failed";
                            lblStatus.ForeColor = Color.Red;
                            return;

                        }
                    }
                    lblStatus.Text = "Email Sent Successfully";
                    lblStatus.ForeColor = Color.Green;

                }
                else
                { }
                Session["Attachment"] = null;
                Session["DocumentNames"] = null;
                Session["Extension"] = null;
                Session["DocumentType"] = null;
            }
            catch (Exception ex)
            { }

        }
        #endregion

        #region Button Send Email
        protected void btnSend_Click(object sender, EventArgs e)
        {
            #region Commented Code
            try
            {
                btnNext.Visible = false;
                btnPrev.Visible = false;
                lblPageNo.Text = "";
                InvoiceImage.ImageUrl = "";
                btnPrint.Visible = false;
                InvoiceImage.Visible = false;

                lblStatus.Text = "";


                int count = 0;
                MemoryStream[] Documents = new MemoryStream[0];
                string[] DocumentName = new string[0];
                string[] Extension = new string[0];
                for (int i = 0; i < grdePouch.Rows.Count; i++)
                {
                    if (((RadioButton)(grdePouch.Rows[i].FindControl("rdbePouch"))).Checked)
                    {
                        count++;
                    }
                }

                if (count == 1)
                {
                    for (int i = 0; i < grdePouch.Rows.Count; i++)
                    {
                        if (((RadioButton)(grdePouch.Rows[i].FindControl("rdbePouch"))).Checked)
                        {
                            string ImageID = ((Button)(grdePouch.Rows[i].FindControl("btnUpload"))).CommandArgument.ToString();
                            string AWBNo = ((Label)(grdePouch.Rows[i].FindControl("AWBNo"))).Text;
                            string AirlinePrefix = ((Label)(grdePouch.Rows[i].FindControl("AirlinePrefix"))).Text;
                            string DocumentType = ((Label)(grdePouch.Rows[i].FindControl("lbldocumentName"))).Text;
                            string Uploaded = ((Label)(grdePouch.Rows[i].FindControl("lblIsUploaded"))).Text;

                            if (Uploaded != "N")
                            {

                                string[] QueryNames = new string[2];
                                object[] QueryValues = new object[2];
                                SqlDbType[] QueryTypes = new SqlDbType[2];

                                QueryNames[0] = "DocID";
                                QueryNames[1] = "DocumentType";

                                QueryTypes[0] = SqlDbType.BigInt;
                                QueryTypes[1] = SqlDbType.VarChar;

                                QueryValues[0] = Convert.ToInt32(ImageID);
                                QueryValues[1] = DocumentType;
                                DataSet ds = db.SelectRecords("SP_GetePouchDocuments", QueryNames, QueryValues, QueryTypes);

                                if (ds != null)
                                {
                                    if (ds.Tables.Count > 0)
                                    {
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            //Documents = new MemoryStream[ds.Tables[0].Rows.Count-1];
                                            //DocumentName = new string[ds.Tables[0].Rows.Count - 1];
                                            //Extension = new string[ds.Tables[0].Rows.Count - 1];
                                            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                            {
                                                Array.Resize(ref Documents, Documents.Length + 1);
                                                Documents[Documents.Length - 1] = new MemoryStream((byte[])ds.Tables[0].Rows[j]["Document"]);
                                                Array.Resize(ref DocumentName, DocumentName.Length + 1);
                                                DocumentName[DocumentName.Length - 1] = ds.Tables[0].Rows[j]["DocumentName"].ToString();
                                                Array.Resize(ref Extension, Extension.Length + 1);
                                                Extension[Extension.Length - 1] = ds.Tables[0].Rows[j]["DocumentExtension"].ToString();
                                                //Documents[j] = new MemoryStream((byte[])ds.Tables[0].Rows[j]["Document"]);
                                                //DocumentName[j] = ds.Tables[0].Rows[j]["DocumentName"].ToString();
                                                //Extension[j] = ds.Tables[0].Rows[j]["DocumentExtension"].ToString();

                                            }
                                            Session["Attachment"] = Documents;
                                            Session["DocumentNames"] = DocumentName;
                                            Session["Extension"] = Extension;
                                            Session["DocumentType"] = DocumentType;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                lblStatus.Text = "No documents uploaded to send as an Email Attachment!";
                                lblStatus.ForeColor = Color.Red;
                                return;
                            }
                        }
                    }

                }
                #region Commented Code
                //lblStatus.Text = "";
                //int count = 0;
                //string[] ImageIDs = new string[0];
                //string[] DocumentNames = new string[0];
                //string[] DocumentNo = new string[0];
                //string[] AWBNo = new string[0];
                //string[] DocumentType = new string[0];
                //string[] AirlinePrefix = new string[0];
                //string[] FileName = new string[0];



                //    for (int i = 0; i < grdePouch.Rows.Count; i++)
                //    {
                //        if (((RadioButton)(grdePouch.Rows[i].FindControl("rdbePouch"))).Checked)
                //        {
                //            if (((Label)(grdePouch.Rows[i].FindControl("lblIsUploaded"))).Text != "N")
                //            {
                //                count++;
                //                Array.Resize(ref DocumentNo, DocumentNo.Length + 1);
                //                DocumentNo[DocumentNo.Length - 1] = ((Label)(grdePouch.Rows[i].FindControl("DocumentNo"))).Text;
                //                Array.Resize(ref AWBNo, AWBNo.Length + 1);
                //                AWBNo[AWBNo.Length - 1] = ((Label)(grdePouch.Rows[i].FindControl("AWBNo"))).Text;
                //                Array.Resize(ref DocumentType, DocumentType.Length + 1);
                //                DocumentType[DocumentType.Length - 1] = ((Label)(grdePouch.Rows[i].FindControl("DocumentType"))).Text;
                //                Array.Resize(ref AirlinePrefix, AirlinePrefix.Length + 1);
                //                AirlinePrefix[AirlinePrefix.Length - 1] = ((Label)(grdePouch.Rows[i].FindControl("AirlinePrefix"))).Text;
                //                Array.Resize(ref FileName, FileName.Length + 1);
                //                FileName[FileName.Length-1] = AirlinePrefix[AirlinePrefix.Length-1] + "-" + AWBNo[AirlinePrefix.Length-1] + "-" + DocumentType[DocumentType.Length-1] + "-" + DocumentNo[DocumentNo.Length-1];
                //                if (File.Exists(Server.MapPath(ServerPath + FileName[FileName.Length-1] + ".png")))
                //                {
                //                    FileName[FileName.Length - 1] = FileName[FileName.Length - 1] + ".png";
                //                }
                //                else
                //                    if (File.Exists(Server.MapPath(ServerPath + FileName[FileName.Length - 1] + ".pdf")))
                //                    {
                //                        FileName[FileName.Length - 1] = FileName[FileName.Length - 1] + ".pdf";
                //                    }
                //                Array.Resize(ref DocumentNames, DocumentNames.Length + 1);
                //                DocumentNames[DocumentNames.Length - 1] = ((Label)(grdePouch.Rows[i].FindControl("lbldocumentName"))).Text;

                //                //string DocumentNo = ((Label)(grdePouch.Rows[i].FindControl("DocumentNo"))).Text;
                //                //string AWBNo = ((Label)(grdePouch.Rows[i].FindControl("AWBNo"))).Text;
                //                //string AirlinePrefix = ((Label)(grdePouch.Rows[i].FindControl("AirlinePrefix"))).Text;
                //                //string DocumentType = ((Label)(grdePouch.Rows[i].FindControl("DocumentType"))).Text;
                //                //string img= ((Button)(grdePouch.Rows[i].FindControl("btnUpload"))).CommandArgument.ToString();
                //                //string img = "";
                //                //string doc =((Label)(grdePouch.Rows[i].FindControl("lbldocumentName"))).Text;
                //                //Array.Resize(ref ImageIDs, ImageIDs.Length + 1);
                //                //ImageIDs[ImageIDs.Length - 1] = img;
                //                //Array.Resize(ref DocumentNames, DocumentNames.Length + 1);
                //                //DocumentNames[DocumentNames.Length - 1] = doc;

                //            }

                //        }


                //    }
                //    if (count > 0)
                //    {
                //        Session["Attachment"] = FileName;
                //        Session["DocumentNames"] = DocumentNames;
                //        string[] InvoiceDocs = new string[0];
                //        string[] PackageDocs = new string[0];

                //        for (int i = 0; i < DocumentNo.Length; i++)
                //        {
                //            if (DocumentNames[i] == "Invoice")
                //            {
                //                for (int j = 0; j < Convert.ToInt32(DocumentNo[i]); j++)
                //                {
                //                    Array.Resize(ref InvoiceDocs, InvoiceDocs.Length + 1);
                //                    InvoiceDocs[j] = AirlinePrefix[i] + "-" + AWBNo[i] + "-" + DocumentType[i] + "-" + j+1;
                //                    if (File.Exists(Server.MapPath(ServerPath + InvoiceDocs[j] + ".png")))
                //                    {
                //                        InvoiceDocs[j] = InvoiceDocs[j] + ".png";
                //                    }
                //                    else
                //                        if (File.Exists(Server.MapPath(ServerPath + InvoiceDocs[j] + ".pdf")))
                //                        {
                //                            InvoiceDocs[j] = InvoiceDocs[j] + ".pdf";
                //                        }
                //                }
                //            }
                //            if (DocumentNames[i] == "Packing List")
                //            {
                //                for (int j = 0; j < Convert.ToInt32(DocumentNo[i]); j++)
                //                {
                //                    Array.Resize(ref PackageDocs, PackageDocs.Length + 1);
                //                    PackageDocs[j] = AirlinePrefix[i] + "-" + AWBNo[i] + "-" + DocumentType[i] + "-" + j + 1;
                //                    if (File.Exists(Server.MapPath(ServerPath + PackageDocs[j] + ".png")))
                //                    {
                //                       PackageDocs[j] = PackageDocs[j] + ".png";
                //                    }
                //                    else
                //                        if (File.Exists(Server.MapPath(ServerPath + PackageDocs[j] + ".pdf")))
                //                        {
                //                            PackageDocs[j] = PackageDocs[j] + ".pdf";
                //                        }
                //                }
                //            }
                //        }
                //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewEmailSplit();</script>", false);
                //    }

                //    if (count < 1)
                //    {

                //        lblStatus.Text = "No file uploaded to send";
                //        lblStatus.ForeColor = Color.Red;

                //    }





                //for (int i = 0; i < grdePouch.Rows.Count; i++)
                //{
                //    if (((RadioButton)(grdePouch.Rows[i].FindControl("rdbePouch"))).Checked)
                //    {
                //        if (((Label)(grdePouch.Rows[i].FindControl("lblIsUploaded"))).Text != "N")
                //        {
                //            count++;
                //            string ImageID = ((Button)(grdePouch.Rows[i].FindControl("btnUpload"))).CommandArgument.ToString();
                //            string AWBNo = ((Label)(grdePouch.Rows[i].FindControl("AWBNo"))).Text;
                //            string AirlinePrefix = ((Label)(grdePouch.Rows[i].FindControl("AirlinePrefix"))).Text;
                //            string DocumentType = ((Label)(grdePouch.Rows[i].FindControl("lbldocumentName"))).Text;


                //        }

                //    }


                //}
                #endregion
                if (count > 0)
                {



                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewEmailSplit();</script>", false);
                }

                if (count < 1)
                {

                    lblStatus.Text = "No Document selected!";
                    lblStatus.ForeColor = Color.Red;

                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

            #endregion

            #region New Commented Code
            //try
            //{
            //    if (Session["DocumentsData"] != null)
            //    {
            //        DataSet ds = (DataSet)Session["DocumentsData"];
            //        string AWBNo = ((Label)(grdePouch.Rows[imagecount - 1].FindControl("AWBNo"))).Text;
            //        string AirlinePrefix = ((Label)(grdePouch.Rows[imagecount - 1].FindControl("AirlinePrefix"))).Text;
            //        string DocumentType = ((Label)(grdePouch.Rows[imagecount - 1].FindControl("lbldocumentName"))).Text;
            //        Session["Attachment"] = ds.Tables[0].Rows[imagecount - 1]["Document"];
            //        Session["DocumentNames"] = ds.Tables[0].Rows[imagecount - 1]["DocumentName"].ToString();
            //        Session["Extension"] = ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"].ToString();
            //        Session["DocumentType"] = DocumentType;
            //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewEmailSplit();</script>", false);


            //    }

            //}
            //catch (Exception ex)
            //{
            //    lblStatus.Text = ex.Message;
            //    lblStatus.ForeColor = Color.Red;
            //}
            #endregion




        }
        #endregion

        public bool sendMail(string fromEmailId, string toEmailId, string password, string subject, string body, bool isBodyHTML, MemoryStream[] Document, string[] DocumentName, string[] Extension)
        {
            bool flag = false;
            try
            {
                MailMessage Mail = new MailMessage();
                Mail.From = new MailAddress(fromEmailId);
                Mail.To.Add(toEmailId.Trim());
                Mail.Subject = subject;
                Mail.IsBodyHtml = isBodyHTML;
                Mail.Body = body;
                SmtpClient smtp = new SmtpClient("smtpout.secureserver.net", 80);//previous 25
                smtp.Credentials = new NetworkCredential(fromEmailId, password);
                Mail.Priority = MailPriority.High;
                for (int i = 0; i < Document.Length; i++)
                {
                    if (Extension[i] == ".pdf")
                    {
                        Mail.Attachments.Add(new Attachment(Document[i], DocumentName[i], "application/pdf"));
                    }
                    else
                    {
                        Mail.Attachments.Add(new Attachment(Document[i], DocumentName[i], "image/png"));
                    }
                }


                try
                {
                    smtp.Send(Mail);
                    flag = true;
                    //clsLog.WriteLog("Mail Sent @ "+DateTime.Now.ToString());
                }
                catch (Exception ex)
                {
                    //clsLog.WriteLog("Exception while Sending Mail : "+ ex.Message );
                }

            }
            catch (Exception ex)
            {
                //clsLog.WriteLog("Exception while collection Mail Info : "+ ex.Message );
                flag = false;
            }
            return flag;
        }

        #region Button Previous
        protected void btnPrev_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                if (imagecount > 1)
                {

                    imagecount--;
                    if (Session["DocumentsData"] != null)
                    {
                        lblPageNo.Text = lblPageNo.Text = "Page : " + imagecount.ToString();
                        btnNext.Visible = true;
                        btnPrev.Visible = true;
                        DataSet ds = (DataSet)Session["DocumentsData"];
                        //if (ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"].ToString() ==".pdf")
                        //{

                        //}
                        if (ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"].ToString() == "png" || ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"].ToString() == "gif" || ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"].ToString() == "jpg" || ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"].ToString() == "jpeg" )
                        {
                            InvoiceImage.ImageUrl = "~/ShowImage.ashx?id=" + ds.Tables[0].Rows[imagecount - 1]["SerialNumber"].ToString();
                            InvoiceImage.Visible = true;
                            btnNext.Visible = true;
                            btnPrev.Visible = true;
                            lblPageNo.Text = "Page : " + imagecount.ToString();
                        }
                        else
                        {
                            #region Commented Code

                            //                            //FileStream sourceFile = null;
                            //                            //try
                            //                            //{ 
                            //                            //    Response.ContentType = "application/xml"; 
                            //                            //    Response.AddHeader("Content-Disposition", "attachment; filename=" + ds.Tables[0].Rows[imagecount-1]["DocumentName"] + ".pdf");
                            //                            //    //sourceFile = new FileStream(fileName, FileMode.Open); 
                            //                            //    //long FileSize; 
                            //                            //    //FileSize = sourceFile.Length; 
                            //                            //    byte[] getContent = (byte[])ds.Tables[0].Rows[imagecount - 1]["Document"];
                            //                            //    //sourceFile.Read(getContent, 0, (int)sourceFile.Length);
                            //                            //    //sourceFile.Close(); 
                            //                            //    Response.BinaryWrite(getContent); 
                            //                            //}
                            //                            //catch (Exception ex) { throw ex; }

                            //                            //System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
                            //                            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>window.open("+ds.Tables[0].Rows[imagecount-1]["Document"]+ds.Tables[0].Rows[imagecount-1]["DocumentName"] + ".pdf)</script>", false);
                            //                            //Response.ClearContent();
                            //                            //Response.Clear();
                            //                            InvoiceImage.Visible = false;
                            //                            btnNext.Visible = true;
                            //                            btnPrev.Visible = true;
                            //                            lblPageNo.Text = "Page : " + imagecount.ToString();
                            //                            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            //                            //Response.ContentType = "text/plain";
                            //                            //Response.AddHeader("Content-Disposition", "attachment; filename=" + ds.Tables[0].Rows[imagecount-1]["DocumentName"] + ".pdf");
                            //                            //Response.BinaryWrite((byte[])ds.Tables[0].Rows[imagecount-1]["Document"]);
                            //                            //Response.Flush();
                            //                            //Response.End();
                            //                            Response.Clear();

                            ////Send the file to the output stream

                            //Response.Buffer = true;

                            ////Response.AddHeader("Accept-Header",resultSet.Document.Length.ToString());

                            ////Try and ensure the browser always opens the file and doesn’t just prompt to “open/save”.

                            //Response.AddHeader("Content-Length", ((byte[])ds.Tables[0].Rows[imagecount-1]["Document"]).Length.ToString());

                            ////Response.AddHeader(“Content-Disposition”, “inline; filename=” + PDFName);
                            //                            Response.AddHeader("Content-Disposition", "inline; filename=" + ds.Tables[0].Rows[imagecount-1]["DocumentName"] + ".pdf");

                            //Response.AddHeader("Expires","0");

                            //Response.AddHeader("Pragma","cache");

                            //Response.AddHeader("Cache-Control","private");

                            ////Set the output stream to the correct content type (PDF).

                            //Response.ContentType = "application/pdf";

                            //Response.AddHeader("Accept-Ranges", "bytes");

                            ////Output the file

                            //Response.BinaryWrite((byte[])ds.Tables[0].Rows[imagecount - 1]["Document"]);

                            ////Flushing the Response to display the serialized data

                            ////to the client browser.

                            //Response.Flush();

                            //try{Response.End();}

                            //catch{}
                            #endregion
                            InvoiceImage.Visible = false;
                            btnNext.Visible = true;
                            btnPrev.Visible = true;
                            lblPageNo.Text = "Page : " + imagecount.ToString();
                            // byte[] filedata = (byte[])ds.Tables[0].Rows[imagecount - 1]["Document"];
                            // string extension = ".pdf" ;// "pdf", etc
                            // string filename = System.IO.Path.GetTempFileName() + "." + extension;
                            //// string filename = Server.MapPath("Images/")+ds.Tables[0].Rows[imagecount - 1]["DocumentName"] + "." + extension; // Makes something like "C:\Temp\blah.tmp.pdf"

                            // File.WriteAllBytes(filename, filedata);

                            // Process process = new Process();
                            // process.StartInfo.UseShellExecute = true;
                            // process.StartInfo.FileName = filename;
                            // process.Start();
                            // process.Exited += (s,ee) => System.IO.File.Delete(filename); 
                            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>window.open('Download.aspx?FileName=" + imagecount + ",'Download', 'menubar=0, toolbar=0, location=0, status=0, resizable=0, width=100, height=50');</SCRIPT>", true);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>Download(" + imagecount + ");</SCRIPT>", false);


                        }
                    }

                }

                #region Commented Code
                //int DocumentNo;
                //string AWBNo = "";
                //string DocumentType = "";
                //string AirlinePrefix = "";
                //string path = ConfigurationManager.AppSettings["DocumentsPath"].ToString();
                //for (int i = 0; i < grdePouch.Rows.Count; i++)
                //{
                //    if (((RadioButton)grdePouch.Rows[i].FindControl("rdbePouch")).Checked)
                //    {
                //        DocumentNo = Convert.ToInt32(((Label)grdePouch.Rows[i].FindControl("DocumentNo")).Text);
                //        DocumentType = ((Label)(grdePouch.Rows[i].FindControl("lblDocumentName"))).Text;
                //        AWBNo = ((Label)(grdePouch.Rows[i].FindControl("AWBNo"))).Text;
                //        AirlinePrefix = ((Label)(grdePouch.Rows[i].FindControl("AirlinePrefix"))).Text;
                //    }
                //}
                //int DocumentNo;
                //string AWBNo = "";
                //string DocumentType = "";
                //string AirlinePrefix = "";
                //string ImageID = "";
                //string path = ConfigurationManager.AppSettings["DocumentsPath"].ToString();
                //for (int i = 0; i < grdePouch.Rows.Count; i++)
                //{
                //    if (((RadioButton)grdePouch.Rows[i].FindControl("rdbePouch")).Checked)
                //    {
                //        DocumentNo = Convert.ToInt32(((Label)grdePouch.Rows[i].FindControl("DocumentNo")).Text);
                //        DocumentType = ((Label)(grdePouch.Rows[i].FindControl("DocumentType"))).Text;
                //        AWBNo = ((Label)(grdePouch.Rows[i].FindControl("AWBNo"))).Text;
                //        AirlinePrefix = ((Label)(grdePouch.Rows[i].FindControl("AirlinePrefix"))).Text;
                //        ImageID = ((Button)(grdePouch.Rows[i].FindControl("btnUpload"))).CommandArgument.ToString();
                //    }
                //}
                //string[] QueryNames = new string[2];
                //object[] QueryValues = new object[2];
                //SqlDbType[] QueryTypes = new SqlDbType[2];

                //QueryNames[0] = "DocID";
                //QueryNames[1] = "DocumentType";

                //QueryTypes[0] = SqlDbType.BigInt;
                //QueryTypes[1] = SqlDbType.VarChar;

                //QueryValues[0] = Convert.ToInt32(ImageID);
                //QueryValues[1] = DocumentType;
                //DataSet ds = db.SelectRecords("SP_GetePouchDocuments", QueryNames, QueryValues, QueryTypes);



                //string ImgName = "";
                //string FileName = AirlinePrefix + "-" + AWBNo + "-" + DocumentType + "-" + imagecount;
                //ImgName = Path.Combine(path, AirlinePrefix + "-" + AWBNo + "-" + DocumentType + "-" + imagecount);
                //if (File.Exists(Server.MapPath(ImgName + ".png")))
                //{
                //    InvoiceImage.Visible = true;
                //    InvoiceImage.ImageUrl = ImgName + ".png";
                //    lblPageNo.Text = "Page : " + imagecount.ToString();

                //}
                //else
                //    if (File.Exists(Server.MapPath(ImgName + ".pdf")))
                //    {
                //        InvoiceImage.Visible = false;
                //        //InvoiceImage.ImageUrl = "~/Images/nopreview.jpg";
                //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>window.open('Documents/" + FileName + ".pdf');</SCRIPT>", false);
                //        lblPageNo.Text = "Page : " + imagecount.ToString();
                //    }
                #endregion

            }
            catch (Exception ex)
            { }

        }
        #endregion

        #region Button Next
        protected void btnNext_Click(object sender, ImageClickEventArgs e)
        {
            DataSet ds = new DataSet("ePouch_ds2");
            try
            {
                //lblPageNo.Text = "";
                if (Session["DocumentsData"] != null)
                {
                    ds = (DataSet)Session["DocumentsData"];
                    if (imagecount < ds.Tables[0].Rows.Count)
                    {
                        imagecount++;
                        lblPageNo.Text = lblPageNo.Text = "Page : " + imagecount.ToString();
                        btnNext.Visible = true;
                        btnPrev.Visible = true;
                        if (ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"].ToString() != "png" || ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"].ToString() != "gif" || ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"].ToString() != "jpg" || ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"].ToString() != "jpeg")
                        {
                            InvoiceImage.Visible = false;
                        }
                        if (ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"].ToString() == "png" || ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"].ToString() == "gif" || ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"].ToString() == "jpg" || ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"].ToString() == "jpeg")
                        {

                            //imagecount = ds.Tables[0].Rows.Count - 1;
                            InvoiceImage.ImageUrl = "~/ShowImage.ashx?id=" + ds.Tables[0].Rows[imagecount - 1]["SerialNumber"].ToString();
                            InvoiceImage.Visible = true;
                            btnNext.Visible = true;
                            btnPrev.Visible = true;
                            lblPageNo.Text = "Page : " + imagecount.ToString();
                        }
                        else
                        {
                            //System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                            //response.ClearContent();
                            //response.Clear();
                            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            //response.ContentType = "text/plain";
                            //response.AddHeader("Content-Disposition", "attachment; filename=" + ds.Tables[0].Rows[imagecount-1]["DocumentName"] + ".pdf");
                            //response.BinaryWrite((byte[])ds.Tables[0].Rows[imagecount-1]["Document"]);
                            //InvoiceImage.Visible = false;
                            //btnNext.Visible = true;
                            //btnPrev.Visible = true;
                            //lblPageNo.Text = "Page : " + imagecount.ToString();
                            //response.Flush();
                            //response.End();

                            InvoiceImage.Visible = false;
                            btnNext.Visible = true;
                            btnPrev.Visible = true;
                            lblPageNo.Text = "Page : " + imagecount.ToString();
                            //byte[] filedata = (byte[])ds.Tables[0].Rows[imagecount - 1]["Document"];
                            //string extension = ".pdf";// "pdf", etc
                            //string filename = System.IO.Path.GetTempFileName() + "." + extension;
                            //// string filename = Server.MapPath("Images/")+ds.Tables[0].Rows[imagecount - 1]["DocumentName"] + "." + extension; // Makes something like "C:\Temp\blah.tmp.pdf"

                            //File.WriteAllBytes(filename, filedata);

                            //Process process = new Process();
                            //process.StartInfo.UseShellExecute = true;
                            //process.StartInfo.FileName = filename;
                            //process.Start();
                            //process.Exited += (s, ee) => System.IO.File.Delete(filename); 
                            //btnPrint_Click(sender, e);
                            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>window.open('Download.aspx?FileName=" + imagecount + ",'Download', 'menubar=0, toolbar=0, location=0, status=0, resizable=0, width=100, height=50');</SCRIPT>", true);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>Download(" + imagecount + ");</SCRIPT>", false);

                        }
                    }
                }

                #region Commented Code

                //int DocumentNo = 0;
                //string AWBNo = "";
                //string DocumentType = "";
                //string AirlinePrefix = "";
                //string ImageID = "";
                //string path = ConfigurationManager.AppSettings["DocumentsPath"].ToString();
                //for (int i = 0; i < grdePouch.Rows.Count; i++)
                //{
                //    if (((RadioButton)grdePouch.Rows[i].FindControl("rdbePouch")).Checked)
                //    {
                //        DocumentNo = Convert.ToInt32(((Label)grdePouch.Rows[i].FindControl("DocumentNo")).Text);
                //        DocumentType = ((Label)(grdePouch.Rows[i].FindControl("DocumentType"))).Text;
                //        AWBNo = ((Label)(grdePouch.Rows[i].FindControl("AWBNo"))).Text;
                //        AirlinePrefix = ((Label)(grdePouch.Rows[i].FindControl("AirlinePrefix"))).Text;
                //        ImageID = ((Button)(grdePouch.Rows[i].FindControl("btnUpload"))).CommandArgument.ToString();
                //    }
                //}
                //string[] QueryNames = new string[2];
                //object[] QueryValues = new object[2];
                //SqlDbType[] QueryTypes = new SqlDbType[2];

                //QueryNames[0] = "DocID";
                //QueryNames[1] = "DocumentType";

                //QueryTypes[0] = SqlDbType.BigInt;
                //QueryTypes[1] = SqlDbType.VarChar;

                //QueryValues[0] = Convert.ToInt32(ImageID);
                //QueryValues[1] = DocumentType;
                //DataSet ds = db.SelectRecords("SP_GetePouchDocuments", QueryNames, QueryValues, QueryTypes);
                //string ImgName = "";
                //string FileName = AirlinePrefix + "-" + AWBNo + "-" + DocumentType + "-" + imagecount;

                //ImgName = Path.Combine(path, AirlinePrefix + "-" + AWBNo + "-" + DocumentType + "-" + imagecount);
                //if (File.Exists(Server.MapPath(ImgName + ".png")))
                //{
                //    InvoiceImage.Visible = true;
                //    InvoiceImage.ImageUrl = ImgName + ".png";
                //    lblPageNo.Text = "Page : " + imagecount.ToString();

                //}
                //else
                //    if (File.Exists(Server.MapPath(ImgName + ".pdf")))
                //    {
                //        InvoiceImage.Visible = false;
                //        //InvoiceImage.ImageUrl = "~/Images/nopreview.jpg";
                //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>window.open('Documents/" + FileName + ".pdf');</SCRIPT>", false);
                //        lblPageNo.Text = "Page : " + imagecount.ToString();
                //    }
                //    else
                //    {
                //        --imagecount;
                //    }
                #endregion

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
            }

        }
        #endregion

        #region Button Print
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                #region Commented Code
                //try
                //{
                //    Session["DocumentsData"] = null;
                //    InvoiceImage.ImageUrl = "";
                //    lblStatus.Text = "";
                //    int count = 0;
                //    for (int i = 0; i < grdePouch.Rows.Count; i++)
                //    {
                //        if (((RadioButton)(grdePouch.Rows[i].FindControl("rdbePouch"))).Checked)
                //        {
                //            count++;
                //        }
                //    }

                //    if (count == 1)
                //    {
                //        for (int i = 0; i < grdePouch.Rows.Count; i++)
                //        {
                //            if (((RadioButton)(grdePouch.Rows[i].FindControl("rdbePouch"))).Checked)
                //            {
                //                string ImageID = ((Button)(grdePouch.Rows[i].FindControl("btnUpload"))).CommandArgument.ToString();
                //                string AWBNo = ((Label)(grdePouch.Rows[i].FindControl("AWBNo"))).Text;
                //                string AirlinePrefix = ((Label)(grdePouch.Rows[i].FindControl("AirlinePrefix"))).Text;
                //                string DocumentType = ((Label)(grdePouch.Rows[i].FindControl("lbldocumentName"))).Text;

                //                #region Commented Code for Retrieving Database files
                //                //if (ImageID != "0")
                //                //{
                //                //    string FileName = AirlinePrefix + "-" + AWBNo + "-" + DocumentType + "-" + DocumentNo;
                //                //    if (File.Exists(Server.MapPath(ConfigurationManager.AppSettings["DocumentsPath"].ToString() +FileName+".pdf")))
                //                //    {

                //                //        InvoiceImage.Visible = false;
                //                //        //InvoiceImage.ImageUrl = "~/Images/nopreview.jpg";
                //                //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>window.open('Documents/"+FileName+".pdf');</SCRIPT>", false);
                //                //        imagecount = Convert.ToInt32(DocumentNo);
                //                //        btnNext.Visible = true;
                //                //        btnPrev.Visible = true;
                //                //        lblPageNo.Text = "Page : " + imagecount.ToString();

                //                //    }
                //                //    else
                //                //        if (File.Exists(Server.MapPath(ConfigurationManager.AppSettings["DocumentsPath"].ToString() + FileName + ".png")))
                //                //        {
                //                //            InvoiceImage.ImageUrl = ConfigurationManager.AppSettings["DocumentsPath"].ToString() + FileName + ".png";
                //                //            InvoiceImage.Visible = true;
                //                //            btnNext.Visible = true;
                //                //            btnPrev.Visible = true;
                //                //            imagecount = Convert.ToInt32(DocumentNo);
                //                //            lblPageNo.Text = "Page : " + imagecount.ToString();
                //                //        }
                //                //        else
                //                //        {
                //                //            lblStatus.ForeColor = Color.Red;
                //                //            lblStatus.Text = "No Image Found";
                //                //            InvoiceImage.Visible = false;
                //                //            btnNext.Visible = false;
                //                //            btnPrev.Visible =false;
                //                //            lblPageNo.Text = "";

                //                //        }
                //                //}
                //                //else
                //                //{
                //                //    lblStatus.ForeColor = Color.Red;
                //                //    lblStatus.Text = "No Image Found";
                //                //    InvoiceImage.Visible = false;
                //                //    btnNext.Visible = false;
                //                //    btnPrev.Visible = false;
                //                //    lblPageNo.Text = "";

                //                //}
                //                #endregion
                //                string[] QueryNames = new string[2];
                //                object[] QueryValues = new object[2];
                //                SqlDbType[] QueryTypes = new SqlDbType[2];

                //                QueryNames[0] = "DocID";
                //                QueryNames[1] = "DocumentType";

                //                QueryTypes[0] = SqlDbType.BigInt;
                //                QueryTypes[1] = SqlDbType.VarChar;

                //                QueryValues[0] = Convert.ToInt32(ImageID);
                //                QueryValues[1] = DocumentType;
                //                DataSet ds = db.SelectRecords("SP_GetePouchDocuments", QueryNames, QueryValues, QueryTypes);

                //                if (ds != null)
                //                {
                //                    if (ds.Tables.Count > 0)
                //                    {
                //                        if (ds.Tables[0].Rows.Count > 0)
                //                        {
                //                            //imagecount = ds.Tables[0].Rows.Count;
                //                           // Session["DocumentsData"] = ds;
                //                            if (ds.Tables[0].Rows[0]["DocumentExtension"].ToString() != ".pdf")
                //                            {
                //                                //InvoiceImage.ImageUrl = "~/ShowImage.ashx?id=" + ds.Tables[0].Rows[imagecount - 1]["SerialNumber"].ToString();
                //                                //InvoiceImage.Visible = true;
                //                                //btnNext.Visible = true;
                //                                //btnPrev.Visible = true;
                //                                //lblPageNo.Text = "Page : " + imagecount.ToString();
                //                                System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                //                                response.ClearContent();
                //                                response.Clear();
                //                                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //                                response.ContentType = "text/plain";
                //                                response.AddHeader("Content-Disposition", "attachment; filename=" + ds.Tables[0].Rows[imagecount - 1]["DocumentName"] + ".png");
                //                                response.BinaryWrite((byte[])ds.Tables[0].Rows[0]["Document"]);
                //                                response.Flush();
                //                                response.End();

                //                            }
                //                            else
                //                            {
                //                                System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                //                                response.ClearContent();
                //                                response.Clear();
                //                                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //                                response.ContentType = "text/plain";
                //                                response.AddHeader("Content-Disposition", "attachment; filename=" + ds.Tables[0].Rows[imagecount - 1]["DocumentName"] + ".pdf");
                //                                response.BinaryWrite((byte[])ds.Tables[0].Rows[0]["Document"]);
                //                                response.Flush();
                //                                response.End();

                //                            }
                //                        }
                //                    }
                //                }


                //            }
                //        }

                //    }

                //    else
                //        if (count > 1)
                //        {
                //            lblStatus.Text = "Please select a single document for printing";
                //            lblStatus.ForeColor = Color.Red;
                //        }
                //        else
                //        {
                //            lblStatus.Text = "Please select a document for printing";
                //            lblStatus.ForeColor = Color.Red;

                //        }

                //}
                //catch (Exception ex)
                //{ }
                #endregion

                btnNext.Visible = false;
                btnPrev.Visible = false;
                lblPageNo.Text = "";
                DataSet ds=new DataSet("ePouch_ds3");
                #region New Commented Code
                if (Session["DocumentsData"] != null)
                {

                     ds = (DataSet)Session["DocumentsData"];
                    if (ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"].ToString() == "png" || ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"].ToString() == "gif" || ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"].ToString() == "jpeg" || ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"].ToString() == "jpg")
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
                        response.AddHeader("Content-Disposition", "attachment; filename=" + ds.Tables[0].Rows[imagecount - 1]["DocumentName"] + "." + ds.Tables[0].Rows[imagecount - 1]["DocumentExtension"].ToString());
                        response.BinaryWrite((byte[])ds.Tables[0].Rows[imagecount - 1]["Document"]);
                        response.Flush();
                        response.End();


                    }


                }
                #endregion
                if (ds != null)
                {
                    ds.Dispose();
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }
        #endregion

        #region Button DocumentType Save
        protected void btnSaveDocType_Click(object sender, EventArgs e)
        {
            try
            {
                lblDocStatus.Text = string.Empty;
                if (txtDocType.Text.Trim() != "")
                {
                    if (objEpouch.SaveAWBEpouchDocumentType(txtDocType.Text, Session["UserName"].ToString(), ((DateTime)Session["IT"])))
                    {
                        btnShow_Click(null, null);
                        lblStatus.Text = "Document Type Added Successfully!";
                        lblStatus.ForeColor = Color.Green;
                        return;
                    }
                }
                else
                {
                    lblDocStatus.Text = "Please enter Document type!";
                    lblDocStatus.ForeColor = Color.Blue;
                    return;
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion


    }
}
