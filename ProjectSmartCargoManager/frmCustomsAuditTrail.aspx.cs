using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QID.DataAccess;
using BAL;
using System.Data;
using System.Drawing;

namespace ProjectSmartCargoManager
{
    public partial class frmCustomsAuditTrail : System.Web.UI.Page
    {
        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["AWB"] != null && Request.QueryString["AWBPrefix"] != null)
                    {
                        //if (Session["awbPrefix"] != null)
                        //{
                        //    txtprefix.Text = Session["awbPrefix"].ToString();
                        //    //txtPrefixMan.Text = Session["awbPrefix"].ToString();
                        //}
                        string AWBPrefix = Request.QueryString["AWBPrefix"].ToString();
                        txtprefix.Text = Request.QueryString["AWBPrefix"].ToString();
                        string AWBNumber = Request.QueryString["AWB"].ToString();
                        txtAWBNumber.Text = AWBNumber;
                        btnList_Click(sender, e);
                    }
                }
                try
                {

                    //if (Session["awbPrefix"] != null)
                    //{
                    //    txtprefix.Text = Session["awbPrefix"].ToString();
                    //    //txtPrefixMan.Text = Session["awbPrefix"].ToString();
                    //}
                    //else
                    //{
                    //    MasterBAL objBal = new MasterBAL();
                    //    Session["awbPrefix"] = objBal.awbPrefix();
                    //    txtprefix.Text = Session["awbPrefix"].ToString();
                    //    //txtPrefixMan.Text = Session["awbPrefix"].ToString();
                    //}
                }
                catch (Exception ex)
                {
                }   
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Button List
        protected void btnList_Click(object sender, EventArgs e)
        {
            SQLServer db = new SQLServer(Global.GetConnectionString());
            DataSet ds=null;
            try
            {
                lblStatus.Text = "";
                grdCustomsAudit.DataSource = null;
                grdCustomsAudit.DataBind();

                string AWBNumber;


                if (txtAWBNumber.Text != "")
                {
                    AWBNumber = txtAWBNumber.Text;
                }
                else
                {
                    lblStatus.Text = "Please Enter AWBNumber.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                ds = db.SelectRecords("SP_GetCustomsAuditDetails", "AWBNumber", txtprefix.Text.Trim() + txtAWBNumber.Text.Trim(), SqlDbType.VarChar);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            grdCustomsAudit.DataSource = ds;
                            grdCustomsAudit.DataBind();
                        }
                        else
                        {
                            lblStatus.Text = "No Records Found!";
                            lblStatus.ForeColor = Color.Red;
                            grdCustomsAudit.DataSource = null;
                            grdCustomsAudit.DataBind();
                            return;
                        }
                    }
                    else
                    {
                        lblStatus.Text = "No Records Found!";
                        lblStatus.ForeColor = Color.Red;
                        grdCustomsAudit.DataSource = null;
                        grdCustomsAudit.DataBind();
                        return;
                    }
                }
                else
                {
                    lblStatus.Text = "No Records Found!";
                    lblStatus.ForeColor = Color.Red;
                    grdCustomsAudit.DataSource = null;
                    grdCustomsAudit.DataBind();
                    return;
                }


            }
            catch (Exception ex)
            {
                ds = null;
            }
            finally 
            {
                if (ds != null)
                    ds.Dispose();
                db = null;
            }
        }
        #endregion

        #region Button Clear
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                grdCustomsAudit.DataSource = null;
                grdCustomsAudit.DataBind();
                txtAWBNumber.Text = string.Empty;
                txtprefix.Text = string.Empty;
            }
            catch (Exception ex)
            { }
        }
        #endregion
    }
}
