using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using BAL;
using System.IO;
using QID.DataAccess;

namespace ProjectSmartCargoManager
{
    public partial class OperationAWBAuditLog : System.Web.UI.Page
    {
        SQLServer db = new SQLServer(Global.GetConnectionString());
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    txtAWBPrefix.Text = ((Instance)Session["ObjInstance"]).AWBPrefix;
                    #region Define PageSize for grid as per configuration
                    try
                    {
                        LoginBL objConfig = new LoginBL();
                        grdOperationAuditLog.PageSize = Convert.ToInt32(objConfig.GetMasterConfiguration("StdListPageSize"));
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
            catch (Exception)
            {
            }
        }
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";


                #region Prepare Parameters

                string[] paramname = new string[4];
                paramname[0] = "AWBNUmber";
                paramname[1] = "AWBPrefix";
                paramname[2] = "FltNo";
                paramname[3] = "FltDate";

                object[] paramvalue = new object[4];
                paramvalue[0] = txtAWBNumber.Text.Trim();

                paramvalue[1] = txtAWBPrefix.Text.Trim();
                paramvalue[2] = txtFlightNo.Text.Trim();
                paramvalue[3] = txtFltDt.Text.Trim();
                SqlDbType[] paramtype = new SqlDbType[4];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.VarChar;

                #endregion Prepare Parameters

                DataSet ds = new DataSet();
                ds = db.SelectRecords("SP_GetOperationAWBAuditLog", paramname, paramvalue, paramtype);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                grdOperationAuditLog.PageIndex = 0;
                                grdOperationAuditLog.DataSource = ds;
                                grdOperationAuditLog.DataMember = ds.Tables[0].TableName;
                                grdOperationAuditLog.DataBind();
                                grdOperationAuditLog.Visible = true;
                                Session["OpsAudit_ds"] = ds;
                            }
                            else if (ds.Tables[0].Rows.Count <= 0)
                            {

                                grdOperationAuditLog.DataSource = null;
                                grdOperationAuditLog.DataBind();
                               
                                lblStatus.Text = "Record does not exist";
                                lblStatus.ForeColor = Color.Red;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void grdOperationAuditLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataSet dstemp = (DataSet)Session["OpsAudit_ds"];
                grdOperationAuditLog.PageIndex = e.NewPageIndex;
                grdOperationAuditLog.DataSource = dstemp;
                grdOperationAuditLog.DataMember = dstemp.Tables[0].TableName;
                grdOperationAuditLog.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

        protected void grdOperationAuditLog_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("OperationAWBAuditLog.aspx");
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            #region Prepare Parameters
            DataTable dt = new DataTable("AWBAuditLog_dt");
            try
            {
                string[] paramname = new string[4];
                paramname[0] = "AWBNUmber";
                paramname[1] = "AWBPrefix";
                paramname[2] = "FltNo";
                paramname[3] = "FltDate";

                object[] paramvalue = new object[4];
                paramvalue[0] = txtAWBNumber.Text.Trim();

                paramvalue[1] = txtAWBPrefix.Text.Trim();
                paramvalue[2] = txtFlightNo.Text.Trim();
                paramvalue[3] = txtFltDt.Text.Trim();
                SqlDbType[] paramtype = new SqlDbType[4];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.VarChar;

            #endregion Prepare Parameters

                DataSet ds = new DataSet();
                ds = db.SelectRecords("SP_GetOperationAWBAuditLog", paramname, paramvalue, paramtype);
                dt = (DataTable)ds.Tables[0];
                if (dt.Rows.Count < 1)
                {
                    lblStatus.Text = "No records found";
                    lblStatus.ForeColor = Color.Red;

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
                    int i;
                    foreach (DataRow dr in dt.Rows)
                    {
                        tab = "";
                        for (i = 0; i < dt.Columns.Count; i++)
                        {
                            Response.Write(tab + dr[i].ToString());
                            tab = "\t";
                        }
                        Response.Write("\n");
                    }
                    Response.End();


                }
            }
            catch (Exception ex)
            { }
            finally
            { }

        }                
    }
}
