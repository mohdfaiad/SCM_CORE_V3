using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using QID.DataAccess;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Configuration;
using System.Data.SqlClient;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class ShowULDList : System.Web.UI.Page
    {
        BLAssignULD objAssignULD = new BLAssignULD();
        DataSet dsULDList = new DataSet();   
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LoadULDDropdown();
                    btnList_Click(null, null);
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(btnCancel, btnCancel.GetType(), "HidUnhide", "window.close();", true);
        }

        protected void getULDInCurrentLocation(string Location,string ULDType,string ULDStatus,string ULDUseStatus)
        {
            try
            {
                grdULDList.DataSource = null;
                grdULDList.DataBind();
                grdULDSummaryList.DataSource = null;
                grdULDSummaryList.DataBind();
                dsULDList = objAssignULD.GetULDListFromLocation(Location, ULDType, ULDStatus, ULDUseStatus);

                if (dsULDList != null && dsULDList.Tables.Count > 0 && dsULDList.Tables[0].Rows.Count > 0)
                {
                    LBLStatus.Text = "";
                    grdULDList.DataSource = dsULDList.Tables[0];
                    grdULDList.DataBind();
                    
                    grdULDSummaryList.DataSource = dsULDList.Tables[1];
                    grdULDSummaryList.DataBind();

                    for (int cnt = 0; cnt < grdULDList.Rows.Count; cnt++)
                    {
                        if(grdULDList.Rows[cnt].Cells[12].Text.Trim().Equals("Y", StringComparison.OrdinalIgnoreCase))
                        {
                            grdULDList.Rows[cnt].BackColor = CommonUtility.ColorHighlightedGrid;
                        }
                    }
                }
                else
                {
                    LBLStatus.Text = "No data found.";
                    LBLStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        #region Button Export
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string Station = Request.QueryString["LOC"] != null ? Request.QueryString["LOC"].ToString() : string.Empty;
                string ULDType = ddlULDType.SelectedValue.Trim();
                string ULDStatus = ddlULDStatus.SelectedValue.Trim();
                string ULDUseStatus = ddlULDUseStatus.SelectedValue.Trim();
                dsULDList = objAssignULD.GetULDListFromLocation(Station, ULDType, ULDStatus, ULDUseStatus);

                if (dsULDList != null && dsULDList.Tables.Count > 0 && dsULDList.Tables[0].Rows.Count > 0)
                {
                    string attachment = "attachment; filename=" + Station + "ULD.xls";
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", attachment);
                    Response.ContentType = "application/vnd.ms-excel";
                    string tab = "";
                    foreach (DataColumn dc in dsULDList.Tables[0].Columns)
                    {
                        Response.Write(tab + dc.ColumnName);
                        tab = "\t";
                    }
                    Response.Write("\n");
                    int i;
                    foreach (DataRow dr in dsULDList.Tables[0].Rows)
                    {
                        tab = "";
                        for (i = 0; i < dsULDList.Tables[0].Columns.Count; i++)
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
            {

            }
        }
        #endregion

        #region Button List
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
              
                string Station = Request.QueryString["LOC"] != null ? Request.QueryString["LOC"].ToString() : string.Empty;
                string ULDType = ddlULDType.SelectedValue.Trim();
                string ULDStatus = ddlULDStatus.SelectedValue.Trim();
                string ULDUseStatus = ddlULDUseStatus.SelectedValue.Trim();

                getULDInCurrentLocation(Station, ULDType, ULDStatus, ULDUseStatus);


            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Load ULD DropDowns
        public void LoadULDDropdown()
        {
            try
            {
                SQLServer db = new SQLServer(Global.GetConnectionString());
                DataSet ds = db.SelectRecords("spGetULDMasterData");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlULDType.DataSource = ds.Tables[0];
                    ddlULDType.DataTextField = "ULDType";
                    ddlULDType.DataValueField = "ULDType";
                    ddlULDType.DataBind();
                    if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                    {
                        ddlULDStatus.DataSource = ds.Tables[1];
                        ddlULDStatus.DataTextField = "ULDStatus";
                        ddlULDStatus.DataValueField = "ID";
                        ddlULDStatus.DataBind();
                    }
                    if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                    {
                        ddlULDUseStatus.DataSource = ds.Tables[2];
                        ddlULDUseStatus.DataTextField = "ULDUseStatus";
                        ddlULDUseStatus.DataValueField = "ULDStatusID";
                        ddlULDUseStatus.DataBind();
                    }

                }
                ddlULDType.Items.Insert(0, "All");
                ddlULDStatus.Items.Insert(0, "All");
                ddlULDUseStatus.Items.Insert(0, "All");
                
            }
            catch (Exception ex)
            { }
        }
        #endregion

    }
}
