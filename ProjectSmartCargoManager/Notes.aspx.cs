using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using QID.DataAccess;
using BAL;
using System.Drawing;

namespace ProjectSmartCargoManager
{
    public partial class Notes : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString != null)
                {
                    try
                    {
                        if (Request.QueryString["Prefix"] != null && Request.QueryString["Prefix"].ToString() != "")
                            txtAWBPrefix.Text = Request.QueryString["Prefix"].ToString();

                        if (Request.QueryString["No"] != null && Request.QueryString["No"].ToString() != "")
                            txtAWBNo.Text = Request.QueryString["No"].ToString();

                        if (Request.QueryString["FltNo"] != null && Request.QueryString["FltNo"].ToString() != "")
                            txtFltNo.Text = Request.QueryString["FltNo"].ToString();

                        if (Request.QueryString["FltDate"] != null && Request.QueryString["FltDate"].ToString() != "")
                            txtFltDt.Text = Request.QueryString["FltDate"].ToString();
                        
                        lblStatus.Text = string.Empty;
                        if (txtAWBNo.Text != "" || txtFltNo.Text != "" || txtFltDt.Text != "")
                        {
                            btnList_Click(null, null);
                        }

                        txtUser.Text = Session["UserName"].ToString();
                    }
                    catch (Exception ex)
                    { }
                }
            }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet("Notes_1");
            lblStatus.Text = string.Empty;
            try
            {
                #region Parameters

                object[] PValue = new object[6];
                string[] PName = new string[6];
                SqlDbType[] PType = new SqlDbType[6];

                int i = 0;

                PName.SetValue("AWBPrefix", i);
                PValue.SetValue(txtAWBPrefix.Text.Trim(), i);
                PType.SetValue(SqlDbType.VarChar, i);
                i++;

                PName.SetValue("AWBNo", i);
                PValue.SetValue(txtAWBNo.Text.Trim(), i);
                PType.SetValue(SqlDbType.VarChar, i);
                i++;

                PName.SetValue("FltDt", i);
                PValue.SetValue(txtFltDt.Text.Trim(), i);
                PType.SetValue(SqlDbType.VarChar, i);
                i++;

                PName.SetValue("FltNo", i);
                PValue.SetValue(txtFltNo.Text.Trim(), i);
                PType.SetValue(SqlDbType.VarChar, i);
                i++;

                PName.SetValue("User", i);
                PValue.SetValue(txtUser.Text.Trim(), i);
                PType.SetValue(SqlDbType.VarChar, i);
                i++;

                PName.SetValue("Comment", i);
                PValue.SetValue(txtComments.Text.Trim(), i);
                PType.SetValue(SqlDbType.VarChar, i);
                i++;


                #endregion

                ds = da.SelectRecords("spGetNotesList", PName, PValue, PType);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["dsNotes"] = ds;
                    grdNoteList.DataSource = ds.Tables[0];
                    grdNoteList.DataBind();
                }
                else
                {
                    ViewState["dsNotes"] = null;
                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = Color.Red;

                    grdNoteList.DataSource = null;
                    grdNoteList.DataBind();
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;
            if (txtAWBNo.Text.Trim() == "" && txtComments.Text.Trim() == "" && txtFltDt.Text.Trim() == "" 
                && txtFltNo.Text.Trim() == "")
            {
                lblStatus.Text = "Please enter either AWB Number or Flight Number.";
                lblStatus.ForeColor = Color.Red;
                return;
            }
            if (txtAWBNo.Text.Trim() == "")
            {
                txtAWBPrefix.Text = "";
            }
            if (txtAWBNo.Text.Trim() != "")
            {
                if (txtAWBPrefix.Text.Trim() == "")
                {
                    lblStatus.Text = "Please enter AWB Prefix.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
            }

            if (txtFltNo.Text.Trim() != "")
            {
                if (!CheckDate())
                {
                    return;
                }
            }
            if (txtComments.Text.Trim() == "")
            {
                lblStatus.Text = "Please enter Comment.";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            try
            {
                #region Parameters

                object[] PValue = new object[8];
                string[] PName = new string[8];
                SqlDbType[] PType = new SqlDbType[8];

                int i = 0;

                PName.SetValue("AWBPrefix", i);
                PValue.SetValue(txtAWBPrefix.Text.Trim(), i);
                PType.SetValue(SqlDbType.VarChar, i);
                i++;

                PName.SetValue("AWBNo", i);
                PValue.SetValue(txtAWBNo.Text.Trim(), i);
                PType.SetValue(SqlDbType.VarChar, i);
                i++;

                PName.SetValue("FltDate", i);
                PValue.SetValue(txtFltDt.Text.Trim(), i);
                PType.SetValue(SqlDbType.VarChar, i);
                i++;

                PName.SetValue("FltNo", i);
                PValue.SetValue(txtFltNo.Text.Trim().ToUpper(), i);
                PType.SetValue(SqlDbType.VarChar, i);
                i++;

                PName.SetValue("User", i);
                PValue.SetValue(Session["UserName"].ToString(), i);
                PType.SetValue(SqlDbType.VarChar, i);
                i++;

                PName.SetValue("Comment", i);
                PValue.SetValue(txtComments.Text.Trim(), i);
                PType.SetValue(SqlDbType.VarChar, i);
                i++;

                PName.SetValue("UpdatedOn", i);
                PValue.SetValue(Session["IT"], i);
                PType.SetValue(SqlDbType.DateTime, i);
                i++;

                PName.SetValue("UpdatedBy", i);
                PValue.SetValue(Session["UserName"].ToString(), i);
                PType.SetValue(SqlDbType.VarChar, i);
                i++;

                #endregion

                string res = "";
                //Getting return value from stored procedure in C# using "GetStringByProcedure" method.
                res = da.GetStringByProcedure("spAddNote", PName,PValue, PType);

                if (res.Length<1)
                {
                    //btnClear_Click(null,null);
                    txtComments.Text = "";
                    btnList_Click(null, null);
                    lblStatus.Text = "Note added successfully";
                    lblStatus.ForeColor = Color.Green;
                }
                else if(res.Length>1)
                {
                    lblStatus.Text = res;
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Note could not be added";
                lblStatus.ForeColor = Color.Green;
            }
            finally
            { }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ViewState["dsNotes"] = null;
            txtAWBNo.Text = string.Empty;
            txtFltDt.Text = txtFltNo.Text = string.Empty;
            txtComments.Text = string.Empty;
            grdNoteList.DataSource = null;
            grdNoteList.DataBind();
            lblStatus.Text = string.Empty;
        }

        protected void grdNoteList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet ds = new DataSet("Notes_2");
            try
            {
                ds = (DataSet)ViewState["dsNotes"];
                grdNoteList.PageIndex = e.NewPageIndex;
                grdNoteList.DataSource = ds;
                grdNoteList.DataBind();
            }
            catch (Exception ex)
            { }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }

        protected bool CheckDate()
        {
            if (txtFltDt.Text.Trim() == "")
            {
                lblStatus.Text = "Enter Flight Date";
                lblStatus.ForeColor = Color.Red;
                return false;
            }
            try
            {
                DateTime dt = DateTime.ParseExact(txtFltDt.Text.Trim(), "dd/MM/yyyy", null);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Enter Date in dd/MM/yyyy format";
                lblStatus.ForeColor = Color.Red;
                return false;
            }
            return true;
        }
    }
}

