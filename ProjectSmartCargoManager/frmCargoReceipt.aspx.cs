using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data;
using QID.DataAccess;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class frmCargoReceipt : System.Web.UI.Page
    {
        MasterBAL objMst = new MasterBAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Session["CargoRec"] == null)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>javascript:alert('No Record Available');callclose();</SCRIPT>");
                    }
                    else
                    {
                        DataTable dt = (DataTable)Session["CargoRec"];
                        txtSendID.Text = dt.Rows[0][12].ToString();
                        txtAirCodeR.Text = dt.Rows[0][11].ToString();
                        txtVol.Text = dt.Rows[0][10].ToString();
                        txtAirCodeD.Text = dt.Rows[0][8].ToString();
                        txtAirCodeO.Text = dt.Rows[0][7].ToString();
                        txtGW.Text = dt.Rows[0][6].ToString();
                        txtNoOFPiece.Text = dt.Rows[0][5].ToString();
                        txtAirCode.Text = dt.Rows[0][4].ToString();
                        txtDMT.Text = dt.Rows[0][3].ToString();
                        txtShipperAddress.Text = dt.Rows[0][2].ToString();
                        txtShipperName.Text = dt.Rows[0][1].ToString();
                        txtAWB.Text = dt.Rows[0][0].ToString();
                        string strOwner = "";
                        try
                        {



                            DataSet dsAirlineDetails = objMst.GetAirlineDetails("", "");
                            if (dsAirlineDetails.Tables.Count > 0)
                            {
                                if (dsAirlineDetails.Tables[0].Rows.Count > 0)
                                {
                                    for (int p = 0; p < dsAirlineDetails.Tables[0].Rows.Count; p++)
                                    {
                                        strOwner = dsAirlineDetails.Tables[0].Rows[0][0].ToString();
                                        txtIssuedBy.Text = strOwner;
                                        break;
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }
        private bool CheckEmail(string EmailAddress)
        {
            string strPattern = "^([0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
            if (System.Text.RegularExpressions.Regex.IsMatch(EmailAddress, strPattern))
            { return true; }
            return false;
        }
        protected void btnsendmail_Click(object sender, EventArgs e)
        {

            try
            {
                string[] chkmail = txtSendID.Text.Trim().Split(',');
                for (int i = 0; i < chkmail.Length; i++)
                {
                    //if (chkmail[i].ToString().Trim() != "")
                    {
                        bool re = CheckEmail(chkmail[i].ToString().Trim());
                        if (re == false)
                        {
                            lblError.Text = "Please Check mail id " + chkmail[i];
                           return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            try
            {

                StringWriter SWriter = new StringWriter();
                HtmlTextWriter HTWriter = new HtmlTextWriter(SWriter);
                mailmain.RenderControl(HTWriter);
                string tablevalue = SWriter.GetStringBuilder().ToString();

                string[] paramname = new string[] { "SendMail", "AWBNo","SendTo" };

                object[] paramvalue = new object[] { tablevalue, txtAWB.Text.Trim(),txtSendID.Text.Trim() };

                SqlDbType[] paramtype = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.NVarChar, SqlDbType.NVarChar };

                SQLServer da = new SQLServer(Global.GetConnectionString());

                bool res = da.InsertData("spSaveCargoReceipt", paramname, paramtype, paramvalue);
                if (res == true)
                {
                    lblError.Text = "Mail Sending in Queue";
                    ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>javascript:callclose();</SCRIPT>");
                }
                else
                {
                    lblError.Text = "Error during sending Mail";
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error during sending Mail";
            }
        }
    }
}
