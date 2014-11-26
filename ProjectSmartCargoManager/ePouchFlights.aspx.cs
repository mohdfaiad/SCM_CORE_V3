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
    public partial class ePouchFlights : System.Web.UI.Page
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
                    Session["FlightsDocumentsData"] = null;
                    Session["FlightsAttachment"] = null;
                    Session["FlightsDocumentNames"] = null;
                    Session["FlightsExtension"] = null;
                    Session["FlightsDocumentType"] = null;

                    txtFlightCode.Text = Session["AirlinePrefix"] != null ? Session["AirlinePrefix"].ToString() : string.Empty;
                    txtFlightDate.Text = Session["IT"] != null ? ((DateTime)Session["IT"]).ToString("dd/MM/yyyy") : string.Empty;
                    string FlightDate = "";
                    string FlightNo = "";
                    if (Session["ePouchFlightNo"] != null && Session["ePouchFlightDate"]!=null)
                    {
                        FlightNo = Session["ePouchFlightNo"].ToString();
                        FlightDate = Session["ePouchFlightDate"].ToString();
                        if (FlightNo != "" && FlightDate != "")
                        {
                            if (FlightNo.Length > 2)
                            {
                                txtFlightCode.Text = FlightNo.Substring(0, 2);
                                txtFlightID.Text = FlightNo.Substring(2);
                            }
                            txtFlightDate.Text = FlightDate;
                        }
                        LoadingPopupGrid(grdePouch, FlightNo, FlightDate);
                        Session["ePouchFlightNo"] = null;
                        Session["ePouchFlightDate"] = null;

                    }
                    else
                    {
                        btnShow.Visible = true;
                      
                        btnDisplay.Visible = false;
                        btnSend.Visible = false;
                        btnPrint.Visible = false;
                        //btnAdd.Visible = false;
                    }


                }
            }
            catch (Exception ex)
            { }

        }
        #endregion

        #region Loading Popup Grid Details
        public void LoadingPopupGrid(GridView gridview, string FlightNo, string FlightDate)
        {
            DataSet ds = new DataSet("ePouchFlt_ds");
            try
            {
                string[] QueryNames = { "FlightNo", "FlightDate" };
                object[] QueryValues = { FlightNo, FlightDate };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar };
                ds = db.SelectRecords("spGetDocumentDetailsFlights", QueryNames, QueryValues, QueryTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            gridview.DataSource = ds;
                            gridview.DataBind();
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
                        lblStatus.Text = "No records available for the given flight!";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                }

            }
            catch (Exception ex)
            { }
            finally
            {
                if(ds!=null)
                ds.Dispose();
            }
        }

        #endregion

        #region Button Display
        protected void btnDisplay_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet("ePouchFlt_ds1");
            try
            {
                btnPrint.Visible = false;
                Session["FlightsDocumentsData"] = null;
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
                            string FlightNo = ((Label)(grdePouch.Rows[i].FindControl("FlightNo"))).Text;
                            string AirlinePrefix = ((Label)(grdePouch.Rows[i].FindControl("AirlinePrefix"))).Text;
                            string FlightsDocumentType = ((Label)(grdePouch.Rows[i].FindControl("lbldocumentName"))).Text;
                            string Uploaded = ((Label)(grdePouch.Rows[i].FindControl("lblIsUploaded"))).Text;

                            if (Uploaded != "N")
                            {

                                string[] QueryNames = new string[2];
                                object[] QueryValues = new object[2];
                                SqlDbType[] QueryTypes = new SqlDbType[2];

                                QueryNames[0] = "DocID";
                                QueryNames[1] = "FlightsDocumentType";

                                QueryTypes[0] = SqlDbType.BigInt;
                                QueryTypes[1] = SqlDbType.VarChar;

                                QueryValues[0] = Convert.ToInt32(ImageID);
                                QueryValues[1] = FlightsDocumentType;
                                ds = db.SelectRecords("SP_GetFlightePouchDocuments", QueryNames, QueryValues, QueryTypes);

                                if (ds != null)
                                {
                                    if (ds.Tables.Count > 0)
                                    {
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            imagecount = ds.Tables[0].Rows.Count;
                                            Session["FlightsDocumentsData"] = ds;
                                            btnPrint.Visible = true;
                                            if (ds.Tables[0].Rows[imagecount - 1]["DocumentFlightsExtension"].ToString() == "png" || ds.Tables[0].Rows[imagecount - 1]["DocumentFlightsExtension"].ToString() == "gif" || ds.Tables[0].Rows[imagecount - 1]["DocumentFlightsExtension"].ToString() == "jpg" || ds.Tables[0].Rows[imagecount - 1]["DocumentFlightsExtension"].ToString() == "jpeg")
                                            {
                                                InvoiceImage.ImageUrl = "~/ShowFlightImage.ashx?id=" + ds.Tables[0].Rows[imagecount - 1]["SerialNumber"].ToString();
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
                    //  allows file size of upto 1 MB to be uploaded.
                    //if (((FileUpload)(grdePouch.Rows[i].FindControl("fileupload_ePouch"))).PostedFile.ContentLength < 1048576)

                    //  allows file size of upto 11 MB to be uploaded.(11 MB=11534336 Bytes)
                    if (((FileUpload)(grdePouch.Rows[i].FindControl("fileupload_ePouch"))).PostedFile.ContentLength < 11534336)
                    {
                        string DocumentNo = "";
                        DocumentNo = ((Label)(grdePouch.Rows[i].FindControl("DocumentNo"))).Text;
                        DocumentNo = (Convert.ToInt32(DocumentNo) + 1).ToString();
                        string DocumentName = ((Label)(grdePouch.Rows[i].FindControl("lblDocumentName"))).Text;
                        string FlightNo = ((Label)(grdePouch.Rows[i].FindControl("FLightNo"))).Text;
                        string AirlinePrefix = ((Label)(grdePouch.Rows[i].FindControl("AirlinePrefix"))).Text;
                        string FlightsDocumentType = ((Label)(grdePouch.Rows[i].FindControl("FlightsDocumentType"))).Text;
                        string filename = "";
                        string Documentfilename = "";
                        //string fFlightsExtension = ((FileUpload)(grdePouch.Rows[i].FindControl("fileupload_ePouch"))).FileName.Substring(((FileUpload)(grdePouch.Rows[i].FindControl("fileupload_ePouch"))).FileName.Length - 4);

                        Documentfilename = AirlinePrefix + "-" + FlightNo + "-" + FlightsDocumentType + "-" + DocumentNo;



                        #region Converting file to binary data
                        try
                        {
                            string Docfilename = ((FileUpload)(grdePouch.Rows[i].FindControl("fileupload_ePouch"))).FileName;
                            byte[] Document = new byte[((FileUpload)(grdePouch.Rows[i].FindControl("fileupload_ePouch"))).PostedFile.ContentLength];
                            HttpPostedFile uploadedDocument = ((FileUpload)(grdePouch.Rows[i].FindControl("fileupload_ePouch"))).PostedFile;
                            uploadedDocument.InputStream.Read(Document, 0, (int)((FileUpload)(grdePouch.Rows[i].FindControl("fileupload_ePouch"))).PostedFile.ContentLength);
                            string FlightsExtension = ((FileUpload)(grdePouch.Rows[i].FindControl("fileupload_ePouch"))).FileName.Substring(((FileUpload)(grdePouch.Rows[i].FindControl("fileupload_ePouch"))).FileName.IndexOf('.') + 1);

                            flag = UpdateDocumentDetails(txtFlightCode.Text.Trim() + txtFlightID.Text.Trim(),txtFlightDate.Text.Trim(), DocumentName, Session["UserName"].ToString(), DocumentNo, FlightsExtension, Document, Documentfilename);
                        }
                        catch (Exception ex)
                        { }
                        #endregion
                        if (flag == true)
                        {
                            LoadingPopupGrid(grdePouch,txtFlightCode.Text.Trim()+txtFlightID.Text.Trim(), txtFlightDate.Text.Trim());
                            //DocumentNo = ((Label)(grdePouch.Rows[i].FindControl("DocumentNo"))).Text;
                            string DocumentID = ((Button)(grdePouch.Rows[i].FindControl("btnUpload"))).CommandArgument.ToString();
                            string path = ConfigurationManager.AppSettings["DocumentsPath"].ToString();
                            string fFlightsExtension = ((FileUpload)(grdePouch.Rows[i].FindControl("fileupload_ePouch"))).FileName.Substring(((FileUpload)(grdePouch.Rows[i].FindControl("fileupload_ePouch"))).FileName.IndexOf('.') + 1);
                            if (fFlightsExtension != "png" || fFlightsExtension != "jpg" || fFlightsExtension != "gif" || fFlightsExtension != "jpeg")
                            {
                                filename = Path.Combine(path, AirlinePrefix + "-" + FlightNo + "-" + FlightsDocumentType + "-" + DocumentNo + "." + fFlightsExtension);

                            }
                            else
                            {
                                filename = Path.Combine(path, AirlinePrefix + "-" + FlightNo + "-" + FlightsDocumentType + "-" + DocumentNo + ".png");

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
                        //lblStatus.Text = "File exceeds maximum limit of 1 MB!";
                        lblStatus.Text = "File exceeds maximum limit of 10 MB!";

                    }
              

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
        public bool UpdateDocumentDetails(string FlightNo,string FlightDate, string DocumentName, string UploadedBy, string DocumentNo, string FlightsExtension, byte[] Document, string DocumentFileName)
        {
            try
            {
                bool flag = false;
                string[] QueryNames = new string[8];
                object[] QueryValues = new object[8];
                SqlDbType[] QueryTypes = new SqlDbType[8];

                QueryNames[0] = "FlightNo";
                QueryNames[1] = "DocumentName";
                QueryNames[2] = "UploadedBy";
                QueryNames[3] = "DocumentNo";
                QueryNames[4] = "FlightsExtension";
                QueryNames[5] = "Document";
                QueryNames[6] = "FileName";
                QueryNames[7] = "FlightDate";

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.VarChar;
                QueryTypes[3] = SqlDbType.VarChar;
                QueryTypes[4] = SqlDbType.VarChar;
                QueryTypes[5] = SqlDbType.VarBinary;
                QueryTypes[6] = SqlDbType.VarChar;
                QueryTypes[7] = SqlDbType.VarChar;


                QueryValues[0] = FlightNo;
                QueryValues[1] = DocumentName;
                QueryValues[2] = UploadedBy;
                QueryValues[3] = DocumentNo;
                QueryValues[4] = FlightsExtension;
                QueryValues[5] = Document;
                QueryValues[6] = DocumentFileName;
                QueryValues[7] = FlightDate;

                flag = db.InsertData("SP_InsertFlightUploadedDocuments", QueryNames, QueryTypes, QueryValues);
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
                Session["FlightsDocumentsData"] = null;
                if (txtFlightID.Text.Trim() != "" || txtFlightCode.Text.Trim() != "" || txtFlightDate.Text.Trim()!="")
                {
                    InvoiceImage.Visible = false;
                    btnNext.Visible = false;
                    btnPrev.Visible = false;
                    lblPageNo.Text = "";
                    grdePouch.DataSource = null;
                    grdePouch.DataBind();
                    LoadingPopupGrid(grdePouch, txtFlightCode.Text.Trim()+txtFlightID.Text.Trim(), txtFlightDate.Text.Trim());
                    btnPrint.Visible = false;


                }
                else
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Enter Flight Details!";
                    return;

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
                string[] FlightsExtension = (string[])Session["FlightsExtension"];
                string documentbody = "";
                string FlightsDocumentType = (string)Session["FlightsDocumentType"];
                MemoryStream[] Document = (MemoryStream[])Session["FlightsAttachment"];
                string[] DocumentName = (string[])Session["FlightsDocumentNames"];

                documentbody = "1." + FlightsDocumentType + "\n";
                string MessageBody = "Hello; \n\nPlease refer to the documents attached with your billing under the Flight Number: " + txtFlightCode.Text.Trim() + txtFlightID.Text.Trim() + "\n\nDocument List:\n" + documentbody + "\nThank You,\nSmartKargo\n\nNote: This is a system generated email. Please do not reply. If you were an unintended recipient kindly delete this email.";
                if (txtEmailID.Text != "")
                {
                    string[] emailid = txtEmailID.Text.Split(',');
                    for (int i = 0; i < emailid.Length; i++)
                    {
                        //bool flag = cls_BL.addMsgToOutBox("E-pouch documents for Flight No: " + txtFlightCode.Text.Trim() + txtFlightID.Text.Trim(), MessageBody, "", emailid[i], Document, DocumentName, FlightsExtension);

                        bool flag = cls_BL.addMsgToOutBox("Epouch documents for Flight No: " + txtFlightCode.Text.Trim() + txtFlightID.Text.Trim(), MessageBody, "", emailid[i], Document, DocumentName, FlightsExtension);

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
                Session["FlightsAttachment"] = null;
                Session["FlightsDocumentNames"] = null;
                Session["FlightsExtension"] = null;
                Session["FlightsDocumentType"] = null;
            }
            catch (Exception ex)
            { }

        }
        #endregion

        #region Button Send Email
        protected void btnSend_Click(object sender, EventArgs e)
        {
            #region Commented Code
            DataSet ds = new DataSet("ePouchFlt_ds2");
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
                string[] FlightsExtension = new string[0];
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
                            string FlightNo = ((Label)(grdePouch.Rows[i].FindControl("FlightNo"))).Text;
                            string AirlinePrefix = ((Label)(grdePouch.Rows[i].FindControl("AirlinePrefix"))).Text;
                            string FlightsDocumentType = ((Label)(grdePouch.Rows[i].FindControl("lbldocumentName"))).Text;
                            string Uploaded = ((Label)(grdePouch.Rows[i].FindControl("lblIsUploaded"))).Text;

                            if (Uploaded != "N")
                            {

                                string[] QueryNames = new string[2];
                                object[] QueryValues = new object[2];
                                SqlDbType[] QueryTypes = new SqlDbType[2];

                                QueryNames[0] = "DocID";
                                QueryNames[1] = "FlightsDocumentType";

                                QueryTypes[0] = SqlDbType.BigInt;
                                QueryTypes[1] = SqlDbType.VarChar;

                                QueryValues[0] = Convert.ToInt32(ImageID);
                                QueryValues[1] = FlightsDocumentType;
                                ds = db.SelectRecords("SP_GetFlightePouchDocuments", QueryNames, QueryValues, QueryTypes);

                                if (ds != null)
                                {
                                    if (ds.Tables.Count > 0)
                                    {
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                            {
                                                Array.Resize(ref Documents, Documents.Length + 1);
                                                Documents[Documents.Length - 1] = new MemoryStream((byte[])ds.Tables[0].Rows[j]["Document"]);
                                                Array.Resize(ref DocumentName, DocumentName.Length + 1);
                                                DocumentName[DocumentName.Length - 1] = ds.Tables[0].Rows[j]["DocumentName"].ToString();
                                                Array.Resize(ref FlightsExtension, FlightsExtension.Length + 1);
                                                FlightsExtension[FlightsExtension.Length - 1] = ds.Tables[0].Rows[j]["DocumentFlightsExtension"].ToString();

                                            }
                                            Session["FlightsAttachment"] = Documents;
                                            Session["FlightsDocumentNames"] = DocumentName;
                                            Session["FlightsExtension"] = FlightsExtension;
                                            Session["FlightsDocumentType"] = FlightsDocumentType;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                lblStatus.Text = "No documents uploaded to send as an Email FlightsAttachment!";
                                lblStatus.ForeColor = Color.Red;
                                return;
                            }
                        }
                    }

                }

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
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
            }

            #endregion

            




        }
        #endregion


        #region Button Previous
        protected void btnPrev_Click(object sender, ImageClickEventArgs e)
        {
            DataSet ds = new DataSet("ePouchFlt_ds3");
            try
            {

                if (imagecount > 1)
                {

                    imagecount--;
                    if (Session["FlightsDocumentsData"] != null)
                    {
                        lblPageNo.Text = lblPageNo.Text = "Page : " + imagecount.ToString();
                        btnNext.Visible = true;
                        btnPrev.Visible = true;
                        ds = (DataSet)Session["FlightsDocumentsData"];
                        //if (ds.Tables[0].Rows[imagecount - 1]["DocumentFlightsExtension"].ToString() ==".pdf")
                        //{

                        //}
                        if (ds.Tables[0].Rows[imagecount - 1]["DocumentFlightsExtension"].ToString() == "png" || ds.Tables[0].Rows[imagecount - 1]["DocumentFlightsExtension"].ToString() == "gif" || ds.Tables[0].Rows[imagecount - 1]["DocumentFlightsExtension"].ToString() == "jpeg" || ds.Tables[0].Rows[imagecount - 1]["DocumentFlightsExtension"].ToString() == "jpg")
                        {
                            InvoiceImage.ImageUrl = "~/ShowFlightImage.ashx?id=" + ds.Tables[0].Rows[imagecount - 1]["SerialNumber"].ToString();
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

                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>Download(" + imagecount + ");</SCRIPT>", false);


                        }
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

        #region Button Next
        protected void btnNext_Click(object sender, ImageClickEventArgs e)
        {
            DataSet ds = new DataSet("ePouchFlt_ds4");
            try
            {
                //lblPageNo.Text = "";
                if (Session["FlightsDocumentsData"] != null)
                {
                    ds = (DataSet)Session["FlightsDocumentsData"];
                    if (imagecount < ds.Tables[0].Rows.Count)
                    {
                        imagecount++;
                        lblPageNo.Text = lblPageNo.Text = "Page : " + imagecount.ToString();
                        btnNext.Visible = true;
                        btnPrev.Visible = true;
                        if (ds.Tables[0].Rows[imagecount - 1]["DocumentFlightsExtension"].ToString() != "png" || ds.Tables[0].Rows[imagecount - 1]["DocumentFlightsExtension"].ToString() != "gif" || ds.Tables[0].Rows[imagecount - 1]["DocumentFlightsExtension"].ToString() != "jpg" || ds.Tables[0].Rows[imagecount - 1]["DocumentFlightsExtension"].ToString() != "jpeg")
                        {
                            InvoiceImage.Visible = false;
                        }
                        if (ds.Tables[0].Rows[imagecount - 1]["DocumentFlightsExtension"].ToString() == "png" || ds.Tables[0].Rows[imagecount - 1]["DocumentFlightsExtension"].ToString() == "gif" || ds.Tables[0].Rows[imagecount - 1]["DocumentFlightsExtension"].ToString() == "jpg" || ds.Tables[0].Rows[imagecount - 1]["DocumentFlightsExtension"].ToString() == "jpeg")
                        {

                            //imagecount = ds.Tables[0].Rows.Count - 1;
                            InvoiceImage.ImageUrl = "~/ShowFlightImage.ashx?id=" + ds.Tables[0].Rows[imagecount - 1]["SerialNumber"].ToString();
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

                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>Download(" + imagecount + ");</SCRIPT>", false);

                        }
                    }
                }

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
            DataSet ds = new DataSet("ePouchFlt_ds5");
            try
            {

                btnNext.Visible = false;
                btnPrev.Visible = false;
                lblPageNo.Text = "";

                #region New Commented Code
                if (Session["FlightsDocumentsData"] != null)
                {

                    ds = (DataSet)Session["FlightsDocumentsData"];
                    if (ds.Tables[0].Rows[imagecount - 1]["DocumentFlightsExtension"].ToString() == "png" || ds.Tables[0].Rows[imagecount - 1]["DocumentFlightsExtension"].ToString() == "gif" || ds.Tables[0].Rows[imagecount - 1]["DocumentFlightsExtension"].ToString() == "jpeg" || ds.Tables[0].Rows[imagecount - 1]["DocumentFlightsExtension"].ToString() == "jpg")
                    {
                        System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                        response.ClearContent();
                        response.Clear();
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        response.ContentType = "text/plain";
                        response.AddHeader("Content-Disposition", "FlightsAttachment; filename=" + ds.Tables[0].Rows[imagecount - 1]["DocumentName"] + ".png");
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
                        response.AddHeader("Content-Disposition", "FlightsAttachment; filename=" + ds.Tables[0].Rows[imagecount - 1]["DocumentName"] + "." + ds.Tables[0].Rows[imagecount - 1]["DocumentFlightsExtension"]);
                        response.BinaryWrite((byte[])ds.Tables[0].Rows[imagecount - 1]["Document"]);
                        response.Flush();
                        response.End();


                    }


                }
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

        #region Button FlightsDocumentType Save
        protected void btnSaveDocType_Click(object sender, EventArgs e)
        {
            try
            {
                lblDocStatus.Text = string.Empty;
                if (txtDocType.Text.Trim() != "")
                {
                    if (objEpouch.SaveEpouchFlightsDocumentType(txtDocType.Text, Session["UserName"].ToString(), ((DateTime)Session["IT"])))
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
