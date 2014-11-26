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
    public partial class MasterAuditLog : System.Web.UI.Page
    {
        SQLServer db = new SQLServer(Global.GetConnectionString());
  

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {


                    #region Define PageSize for grid as per configuration
                    try
                    {
                        txtValidFrm.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                            txtValidTo.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                        LoginBL objConfig = new LoginBL();
                        grdMasterAuditLog.PageSize = Convert.ToInt32(objConfig.GetMasterConfiguration("StdListPageSize"));
                        objConfig = null;
                    }
                    catch (Exception ex)
                    { }
                    #endregion
                    GetMaster();
                    
                }
            }
            catch (Exception ex)
            { }
        }
        # region Get GetMaster List
        private void GetMaster()
        {
            try
            {
                DataSet ds = db.SelectRecords("GetMasters");
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlMaster.DataSource = ds;
                            ddlMaster.DataMember = ds.Tables[0].TableName;
                            ddlMaster.DataValueField = ds.Tables[0].Columns["MasterKey"].ColumnName;

                            ddlMaster.DataTextField = ds.Tables[0].Columns["MasterKey"].ColumnName;
                            ddlMaster.DataBind();
                            ddlMaster.Items.Insert(0, "Select");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        # endregion 


        

        #region Get Pro Rate List
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                string Master="";
                string dtFrom="";
                string dtTo="";
                Master = ddlMaster.SelectedItem.Text.Trim() == "Select" ? "" : ddlMaster.SelectedItem.Text.Trim();
                if (txtValidFrm.Text != "" || txtValidTo.Text != "")
                {
                   DateTime From = DateTime.ParseExact(txtValidFrm.Text.Trim(), "dd/MM/yyyy", null);
                   dtFrom = From.ToString("MM/dd/yyyy");
                  DateTime To = DateTime.ParseExact(txtValidTo.Text.Trim(), "dd/MM/yyyy", null);
                  dtTo = To.ToString("MM/dd/yyyy");
                }
                
                #region Prepare Parameters
                
                string[] paramname = new string[4];
                paramname[0] = "Master";
                paramname[1] = "MasterValue";
                paramname[2] = "FromDate";
                paramname[3] = "ToDate";
               
                
                object[] paramvalue = new object[4];
                paramvalue[0] = Master;

                paramvalue[1] = txtMasterValue.Text.Trim();
                paramvalue[2] = dtFrom;
                paramvalue[3] = dtTo;
                SqlDbType[] paramtype = new SqlDbType[4];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.VarChar;

                #endregion Prepare Parameters

                DataSet ds = new DataSet();
                ds = db.SelectRecords("SP_GetMasterAuditLog", paramname, paramvalue, paramtype);
                Session["MasterAuditLog"]=ds;
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                grdMasterAuditLog.PageIndex = 0;
                                grdMasterAuditLog.DataSource = ds;
                                grdMasterAuditLog.DataMember = ds.Tables[0].TableName;
                                grdMasterAuditLog.DataBind();
                                grdMasterAuditLog.Visible = true;
                                Session["ds"] = ds;
                                
                                
                            }
                            else if (ds.Tables[0].Rows.Count <= 0)
                            {

                                grdMasterAuditLog.DataSource = null;
                                grdMasterAuditLog.DataBind();
                                //btnExport.Visible = false;
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
        #endregion Get Pro Rate List

        protected void grdMasterAuditLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataSet dstemp = (DataSet)Session["ds"];
                grdMasterAuditLog.PageIndex = e.NewPageIndex;
                grdMasterAuditLog.DataSource = dstemp;
                grdMasterAuditLog.DataMember = dstemp.Tables[0].TableName;
                grdMasterAuditLog.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MasterAuditLog.aspx");
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = null;
            DataTable dt = null;
            Session["MasterAuditLog"] = null;
            lblStatus.Text = "";
            try
            {
            if ((DataSet)Session["MasterAuditLog"] == null)
            {
                lblStatus.Text = "";
                string Master = "";
                string dtFrom = "";
                string dtTo = "";
                Master = ddlMaster.SelectedItem.Text.Trim() == "Select" ? "" : ddlMaster.SelectedItem.Text.Trim();
                if (txtValidFrm.Text != "" || txtValidTo.Text != "")
                {
                    DateTime From = DateTime.ParseExact(txtValidFrm.Text.Trim(), "dd/MM/yyyy", null);
                    dtFrom = From.ToString("MM/dd/yyyy");
                    DateTime To = DateTime.ParseExact(txtValidTo.Text.Trim(), "dd/MM/yyyy", null);
                    dtTo = To.ToString("MM/dd/yyyy");
                }

                #region Prepare Parameters

                string[] paramname = new string[4];
                paramname[0] = "Master";
                paramname[1] = "MasterValue";
                paramname[2] = "FromDate";
                paramname[3] = "ToDate";


                object[] paramvalue = new object[4];
                paramvalue[0] = Master;

                paramvalue[1] = txtMasterValue.Text.Trim();
                paramvalue[2] = dtFrom;
                paramvalue[3] = dtTo;
                SqlDbType[] paramtype = new SqlDbType[4];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.VarChar;

                #endregion Prepare Parameters

                DataSet ds = new DataSet();
                ds = db.SelectRecords("SP_GetMasterAuditLog", paramname, paramvalue, paramtype);
                Session["MasterAuditLog"] = ds;

            }
            
                dsExp = (DataSet)Session["MasterAuditLog"];
            

                dt = (DataTable)dsExp.Tables[0];
                if(dt.Rows.Count<1)
                {
                if ( Session["MasterAuditLog"] == null && dt == null )
                {
                   lblStatus.Text = "No records found";
                  lblStatus.ForeColor = Color.Red;
                   //SaveUserActivityLog(lblStatus.Text);
                   //ReportViewer1.Visible = false;
                   return;
                }
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
           }}
            catch (Exception ex)
           { }
            finally
           {
               dsExp = null;
                dt = null;
           }




        }

       
    }
}
