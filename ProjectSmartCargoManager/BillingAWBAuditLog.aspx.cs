using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QID.DataAccess;
using System.Data;
using System.Drawing;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class BillingAWBAuditLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtAWBPrefix.Text = ((Instance)Session["ObjInstance"]).AWBPrefix;
                #region Define PageSize for grid as per configuration
                try
                {
                    LoginBL objConfig = new LoginBL();
                    grdBillingLog.PageSize = Convert.ToInt32(objConfig.GetMasterConfiguration("StdListPageSize"));
                    grdCCALog.PageSize = Convert.ToInt32(objConfig.GetMasterConfiguration("StdListPageSize"));
                    objConfig = null;
                }
                catch (Exception ex)
                { }
                #endregion
            }

            if (Session["AcceptPartnerAWB"].ToString().ToUpper() == "TRUE")
            {
                txtAWBPrefix.Enabled = true;
            }
            else
            {
                txtAWBPrefix.Enabled = false;

            }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            lblStatus.Text = string.Empty;
            try
            {
                SQLServer da = new SQLServer(Global.GetConnectionString());

                string AWBNo = txtAWBPrefix.Text.Trim() + txtAWBNo.Text.Trim();
                string AWB = txtAWBNo.Text.Trim();
                string[] Pname = new string[2];
                object[] Pvalue = new object[2];
                SqlDbType[] Ptype = new SqlDbType[2];
                int i = 0;

                Pname.SetValue("AWBNumber", i);
                Pvalue.SetValue(AWBNo, i);
                Ptype.SetValue(SqlDbType.VarChar, i);
                i++;

                Pname.SetValue("AWB", i);
                Pvalue.SetValue(AWB, i);
                Ptype.SetValue(SqlDbType.VarChar, i);
                i++;


                ds = da.SelectRecords("SP_GetBillingAWBAuditLog", Pname, Pvalue, Ptype);

                //If all tables are empty
                if (ds.Tables[0].Rows.Count <= 0 && ds.Tables[1].Rows.Count <= 0)
                {
                    grdBillingLog.DataSource = null;
                    grdBillingLog.DataBind();

                    grdCCALog.DataSource = null;
                    grdCCALog.DataBind();

                    ViewState["dsBillingAuditLog"] = null;

                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = Color.Red;
                    
                    return;
                }

                //Check Billing Log
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    grdBillingLog.DataSource = ds.Tables[0];
                    grdBillingLog.DataBind();
                    ViewState["dsBillingAuditLog"] = ds;
                }
                else
                {
                    grdBillingLog.DataSource = null;
                    grdBillingLog.DataBind();
                    ViewState["dsBillingAuditLog"] = null;
                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                //Check CCA Log
                if (ds != null && ds.Tables[1].Rows.Count > 0)
                {
                    grdCCALog.DataSource = ds.Tables[1];
                    grdCCALog.DataBind();
                }
                else
                {
                    grdCCALog.DataSource = null;
                    grdCCALog.DataBind();
                }
            }
            catch (Exception ex) { }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtAWBPrefix.Text = ((Instance)Session["ObjInstance"]).AWBPrefix;
            txtAWBNo.Text = string.Empty;
            lblStatus.Text = string.Empty;
            grdBillingLog.DataSource = null;
            grdBillingLog.DataBind();
            ViewState["dsBillingAuditLog"] = null;

            grdCCALog.DataSource = null;
            grdCCALog.DataBind();
        }

        protected void grdBillingLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (DataSet)ViewState["dsBillingAuditLog"];
                grdBillingLog.PageIndex = e.NewPageIndex;
                grdBillingLog.DataSource = ds.Tables[0];
                grdBillingLog.DataBind();
            }
            catch (Exception ex)
            { }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }

        protected void grdCCALog_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (DataSet)ViewState["dsBillingAuditLog"];
                grdCCALog.PageIndex = e.NewPageIndex;
                grdCCALog.DataSource = ds.Tables[1];
                grdCCALog.DataBind();
            }
            catch (Exception ex)
            { }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable("BillingAWB_dt");
            lblStatus.Text = string.Empty;
            try
            {
                SQLServer da = new SQLServer(Global.GetConnectionString());

                string AWBNo = txtAWBPrefix.Text.Trim() + txtAWBNo.Text.Trim();
                string AWB = txtAWBNo.Text.Trim();
                string[] Pname = new string[2];
                object[] Pvalue = new object[2];
                SqlDbType[] Ptype = new SqlDbType[2];
                int i = 0;

                Pname.SetValue("AWBNumber", i);
                Pvalue.SetValue(AWBNo, i);
                Ptype.SetValue(SqlDbType.VarChar, i);
                i++;

                Pname.SetValue("AWB", i);
                Pvalue.SetValue(AWB, i);
                Ptype.SetValue(SqlDbType.VarChar, i);
                i++;


                ds = da.SelectRecords("SP_GetBillingAWBAuditLog", Pname, Pvalue, Ptype);
                dt = (DataTable)ds.Tables[0];
                if (dt.Rows.Count < 1)
                {
                    lblStatus.Text = "No Records found";
                    lblStatus.ForeColor = System.Drawing.Color.Red;

                }
                else
                {
                    if (dt.Columns.Contains("Logo"))
                    { dt.Columns.Remove("Logo"); }
                    lblStatus.Text = "";
                    string attachment = "attachment; filename=Report.xls";
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", attachment);
                    Response.ContentType = "application/vnd.ms-excel";
                    string tab = "";
                    foreach (DataColumn dc in dt.Columns)
                    {
                        Response.Write(tab + dc.ColumnName);
                        tab = "\t";
                    }
                    Response.Write("\n");
                    int p;
                    foreach (DataRow dr in dt.Rows)
                    {
                        tab = "";
                        for (p = 0; p < dt.Columns.Count; p++)
                        {
                            Response.Write(tab + dr[p].ToString());
                            tab = "\t";
                        }
                        Response.Write("\n");
                    }
                    Response.End();

                }
            }
            catch (Exception ex)
            { }
            finally { }
        }

    }
}