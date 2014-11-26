using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using QID.DataAccess;
using System.Data;
using System.Drawing;

namespace ProjectSmartCargoManager
{
    public partial class ListRoute : System.Web.UI.Page
    {
        #region variable
        BALRoutes objBAL = new BALRoutes();
        SQLServer da = new SQLServer(Global.GetConnectionString());
        DataSet ds = new DataSet();

        #endregion variable

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetAirportCode();
                Session["ds"] = null;
                BindEmptyRow();
                #region Define PageSize for grid as per configuration
                try
                {
                    LoginBL objConfig = new LoginBL();
                    ListGrd.PageSize = Convert.ToInt32(objConfig.GetMasterConfiguration("StdListPageSize"));
                    objConfig = null;
                }
                catch (Exception ex)
                { }
                #endregion
            }
        }

        protected void BindEmptyRow()
        {
            DataTable dtEmpty = new DataTable();
            try
            {
                dtEmpty.Columns.Add("SerialNumber");
                dtEmpty.Columns.Add("Source");
                dtEmpty.Columns.Add("Destination");
                dtEmpty.Columns.Add("Route");
                dtEmpty.Columns.Add("IsActive");

                DataRow dr = dtEmpty.NewRow();
                dtEmpty.Rows.Add(dr);

                ListGrd.DataSource = dtEmpty;
                ListGrd.DataBind();

            }
            catch (Exception ex)
            { }
            finally {
                if (dtEmpty != null)
                    dtEmpty.Dispose();
            }
        }

        # region Get Airport Code List
        private void GetAirportCode()
        {
            try
            {
                DataSet ds = objBAL.GetAirportCodes();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlOrigin.DataSource = ds;
                            ddlOrigin.DataMember = ds.Tables[0].TableName;
                            ddlOrigin.DataValueField = ds.Tables[0].Columns["AirportCode"].ColumnName;

                            ddlOrigin.DataTextField = ds.Tables[0].Columns["Airport"].ColumnName;
                            ddlOrigin.DataBind();
                            ddlOrigin.Items.Insert(0, "Select");


                            ddlDest.DataSource = ds;
                            ddlDest.DataMember = ds.Tables[0].TableName;
                            ddlDest.DataValueField = ds.Tables[0].Columns["AirportCode"].ColumnName;

                            ddlDest.DataTextField = ds.Tables[0].Columns["Airport"].ColumnName;
                            ddlDest.DataBind();
                            ddlDest.Items.Insert(0, "Select");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        # endregion GetCurrencyCode List

        #region Get All Route List
        private void GetList()
        {
            try
            {
                #region Prepare Parameters
                object[] RouteInfo = new object[2];
                int i = 0;

                //0
                string or = null;
                if (ddlOrigin.SelectedIndex == 0)
                    or = "";
                else
                    or = ddlOrigin.SelectedValue.ToString();
                RouteInfo.SetValue(or, i);
                i++;

                //1
                string dest = null;
                if (ddlDest.SelectedIndex == 0)
                    dest = "";
                else
                    dest = ddlDest.SelectedValue.ToString();
                RouteInfo.SetValue(dest, i);
                i++;

                #endregion Prepare Parameters
                DataSet ds = new DataSet();
                ds = objBAL.GetSelectedRoute(RouteInfo);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                ListGrd.PageIndex = 0;
                                ListGrd.DataSource = ds;
                                ListGrd.DataMember = ds.Tables[0].TableName;
                                ListGrd.DataBind();
                                ListGrd.Visible = true;
                                Session["ds"] = ds;
                                //ds.Clear();

                                //for (int j = 0; j < ListGrd.Rows.Count; j++)
                                //{
                                //    if (((Label)(ListGrd.Rows[j].FindControl("lblact"))).Text.ToString() == "True")
                                //    {
                                //        ((Label)(ListGrd.Rows[j].FindControl("lblact"))).Text = "Active";
                                //    }

                                //    else if (((Label)(ListGrd.Rows[j].FindControl("lblact"))).Text.ToString() == "False")
                                //    {
                                //        ((Label)(ListGrd.Rows[j].FindControl("lblact"))).Text = "InActive";
                                //    }
                                //}
                                
                                //ddlDest.SelectedIndex = ddlOrigin.SelectedIndex = 0;
                                lblstatus.Text = string.Empty;

                            }
                            else
                            {
                                lblstatus.Text = "Route does not exist..";
                                lblstatus.ForeColor = Color.Red;
                                BindEmptyRow();
                                Session["ds"] = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        # region btnList_Click
        protected void btnList_Click(object sender, EventArgs e)
        {
            GetList();
        }
        # endregion btnList_Click

        # region Page Index Changing
        protected void ListGrd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet ds = new DataSet();
            ds = (DataSet)(Session["ds"]);
            ListGrd.PageIndex = e.NewPageIndex;
            ListGrd.DataSource = ds;//.Copy();
            ListGrd.DataBind();

            for (int j = 0; j < ListGrd.Rows.Count; j++)
            {
                if (((Label)(ListGrd.Rows[j].FindControl("lblact"))).Text.ToString() == "True")
                {
                    ((Label)(ListGrd.Rows[j].FindControl("lblact"))).Text = "Active";
                }

                else if (((Label)(ListGrd.Rows[j].FindControl("lblact"))).Text.ToString() == "False")
                {
                    ((Label)(ListGrd.Rows[j].FindControl("lblact"))).Text = "InActive";
                }
            }




        }
        # endregion Page Index Changing

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = null;
            DataTable dt = null;
            try
            {
                if (Session["ds"] == null)
                {
                    btnList_Click(null, null);
                    BindEmptyRow();
                }
                lblstatus.Text = string.Empty;

                dsExp = (DataSet)Session["ds"];
                if (dsExp != null && dsExp.Tables[0].Rows.Count > 0)
                {
                    dt = (DataTable)dsExp.Tables[0];

                    #region Del Col
                    if (dt.Columns.Contains("SerialNumber"))
                        dt.Columns.Remove("SerialNumber");
                    //if (dt.Columns.Contains("IsActive"))
                    //    dt.Columns.Remove("IsActive");
                    #endregion

                    string attachment = "attachment; filename=RouteList.xls";
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
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No records found for the selected search criteria!');</SCRIPT>", false);
                    return;
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                if (dsExp != null)
                    dsExp = null;
                if (dt != null)
                    dt = null;
            }
        }
    }
}
