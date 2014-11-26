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
using System.Text.RegularExpressions;

namespace ProjectSmartCargoManager
{
    public partial class BuildRoute : System.Web.UI.Page
    {
        #region variable
        BALRoutes objBAL = new BALRoutes();
        SQLServer da = new SQLServer(Global.GetConnectionString());
        DataSet ds = new DataSet();

        #endregion variable

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetAirportCode();
                BindEmptyRow();
                Session["ds"] = null;
                Session["RouteSrNo"] = null;
            }
        }
        #endregion page load

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

                listRouteGrid.DataSource = dtEmpty;
                listRouteGrid.DataBind();

                listRouteGrid.Columns[5].Visible = false;
                listRouteGrid.Columns[6].Visible = false;
            }
            catch (Exception ex)
            { }
            finally
            {
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

                            ddlVia.DataSource = ds;
                            ddlVia.DataMember = ds.Tables[0].TableName;
                            ddlVia.DataValueField = ds.Tables[0].Columns["AirportCode"].ColumnName;

                            ddlVia.DataTextField = ds.Tables[0].Columns["Airport"].ColumnName;
                            ddlVia.DataBind();
                            ddlVia.Items.Insert(0, "Select");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        # endregion GetCurrencyCode List

        # region Route List
        protected void btnList_Click(object sender, EventArgs e)
        {
            #region Selected Route
            try
            {
                lblstatus.Text = string.Empty;

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

                ds = objBAL.GetSelectedRoute(RouteInfo);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                listRouteGrid.PageIndex = 0;
                                listRouteGrid.DataSource = ds;
                                listRouteGrid.DataBind();
                                listRouteGrid.Visible = true;
                                //ds.Clear();
                                Session["ds"] = ds;

                                listRouteGrid.Columns[5].Visible = true;
                                listRouteGrid.Columns[6].Visible = true;
                            }
                            else
                            {
                                lblstatus.ForeColor = Color.Red;
                                lblstatus.Text = "Route does not exist...";
                                Session["ds"] = null;
                                BindEmptyRow();
                            }

                        }

                    }
                }

            }
            catch (Exception ex)
            {

            }
            #endregion Selected Route
        }
        # endregion btnList_Click

        # region Page Index Changing
        protected void listRouteGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataSet dsnew = (DataSet)Session["ds"];
                listRouteGrid.PageIndex = e.NewPageIndex;
                listRouteGrid.DataSource = dsnew;
                listRouteGrid.DataBind();

                for (int j = 0; j < listRouteGrid.Rows.Count; j++)
                {
                    if (((Label)(listRouteGrid.Rows[j].FindControl("lblact"))).Text.ToString() == "True")
                    {
                        ((Label)(listRouteGrid.Rows[j].FindControl("lblact"))).Text = "Active";
                    }

                    else if (((Label)(listRouteGrid.Rows[j].FindControl("lblact"))).Text.ToString() == "False")
                    {
                        ((Label)(listRouteGrid.Rows[j].FindControl("lblact"))).Text = "InActive";
                    }
                }


            }
            catch (Exception ex)
            {
            }
        }
        # endregion Page Index Changing

        #region Add New route
        protected void btnsave_Click(object sender, EventArgs e)
        {
            lblstatus.Text = string.Empty;

            try
            {
                if (btnsave.Text == "Save")
                {
                    #region Save
                    if (ddlOrigin.SelectedIndex > 0 && ddlDest.SelectedIndex > 0)
                    {
                        object[] RouteInfo = new object[4];
                        int i = 0;

                        #region Prepare Parameters

                        //0
                        RouteInfo.SetValue(ddlOrigin.SelectedValue.ToString().ToUpper(), i);
                        i++;

                        //1
                        RouteInfo.SetValue(ddlDest.SelectedValue.ToString().ToUpper(), i);
                        i++;

                        //2
                        string route = null;
                        //string route = srclbl.Text + txtRoute.Text.ToUpper() + destlbl.Text;
                        if (txtRoute.Text == "")
                            route = ddlOrigin.SelectedValue.ToString() + "-" + ddlDest.SelectedValue.ToString();
                        else
                        {
                            //Regex regex = new Regex("^[A-Za-z]{3}[-]*[A-Za-z]{0,3}$");
                            //if (regex.IsMatch(txtRoute.Text))
                            route = ddlOrigin.SelectedValue.ToString() + "-" + txtRoute.Text.ToUpper() + "-" + ddlDest.SelectedValue.ToString();
                            //else
                            //{
                            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "MsgAlert", "<SCRIPT LANGUAGE='javascript'>alert('Enter Route in correct format..')</script>", false);
                            //    return;
                            //}
                        }
                        RouteInfo.SetValue(route, i);
                        i++;

                        //3
                        RouteInfo.SetValue(chkAct.Checked, i);
                        i++;

                        #endregion Prepare Parameters

                        int ID = 0;
                        ID = objBAL.AddRoute(RouteInfo);
                        if (ID >= 0)
                        {
                            #region for Master Audit Log
                            MasterAuditBAL ObjMAL = new MasterAuditBAL();
                            #region Prepare Parameters
                            object[] Params = new object[7];
                            int k = 0;

                            //1
                            Params.SetValue("Airline Route", k);
                            k++;

                            //2
                            string stn = ddlOrigin.SelectedValue + "-" + ddlDest.SelectedValue;
                            Params.SetValue(stn, k);
                            k++;

                            //3
                            Params.SetValue("ADD", k);
                            k++;

                            //4
                            string Msg = "New Route added";
                            Params.SetValue(Msg, k);
                            k++;

                            //5
                            string Desc = route;
                            Params.SetValue(Desc, k);
                            k++;

                            //6

                            Params.SetValue(Session["UserName"], k);
                            k++;

                            //7
                            Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), k);
                            k++;


                            #endregion Prepare Parameters
                            ObjMAL.AddMasterAuditLog(Params);
                            #endregion
                            Clear();
                            btnList_Click(sender, e);
                            //srclbl.Text = destlbl.Text = string.Empty;
                            lblstatus.ForeColor = Color.Green;
                            lblstatus.Text = "Route Added Sucessfully...";
                        }
                        else
                        {
                            Clear();
                            btnList_Click(null, null);
                            lblstatus.ForeColor = Color.Red;
                            lblstatus.Text = "Route Insertion Failed...";
                        }
                    }
                    else
                    {
                        lblstatus.Text = "Please select origin and destination...";
                        lblstatus.ForeColor = Color.Red;
                    }
                    #endregion
                }
                else
                {
                    #region Update

                    if (ddlOrigin.SelectedIndex > 0 && ddlDest.SelectedIndex > 0)
                    {
                        object[] RouteInfo = new object[5];
                        int i = 0;

                        #region Prepare Parameters

                        //0
                        RouteInfo.SetValue(ddlOrigin.SelectedValue.ToString().ToUpper(), i);
                        i++;

                        //1
                        RouteInfo.SetValue(ddlDest.SelectedValue.ToString().ToUpper(), i);
                        i++;

                        //2
                        string route = null;
                        //string route = srclbl.Text + txtRoute.Text.ToUpper() + destlbl.Text;
                        if (txtRoute.Text == "")
                            route = ddlOrigin.SelectedValue.ToString() + "-" + ddlDest.SelectedValue.ToString();
                        else
                        {
                            //Regex regex = new Regex("^[A-Za-z]{3}[-]*[A-Za-z]{0,3}$");
                            //if (regex.IsMatch(txtRoute.Text))
                            route = ddlOrigin.SelectedValue.ToString() + "-" + txtRoute.Text.ToUpper() + "-" + ddlDest.SelectedValue.ToString();
                            //else
                            //{
                            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "MsgAlert", "<SCRIPT LANGUAGE='javascript'>alert('Enter Route in correct format..')</script>", false);
                            //    return;
                            //}
                        }
                        RouteInfo.SetValue(route, i);
                        i++;

                        //3
                        RouteInfo.SetValue(chkAct.Checked, i);
                        i++;

                        //4
                        int srno = (int)Session["RouteSrNo"];
                        RouteInfo.SetValue(srno, i);
                        i++;

                        #endregion Prepare Parameters

                        int ID = 0;
                        ID = objBAL.UpdateRoute(RouteInfo);
                        if (ID >= 0)
                        {
                            #region for Master Audit Log
                            MasterAuditBAL ObjMAL = new MasterAuditBAL();
                            #region Prepare Parameters
                            object[] Params = new object[7];
                            int k = 0;

                            //1
                            Params.SetValue("Airline Route", k);
                            k++;

                            //2
                            string stn = ddlOrigin.SelectedValue + "-" + ddlDest.SelectedValue;
                            Params.SetValue(stn, k);
                            k++;

                            //3
                            Params.SetValue("UPDATE", k);
                            k++;

                            //4
                            string Msg = "Route Updated";
                            Params.SetValue(Msg, k);
                            k++;

                            //5
                            string Desc = route;
                            Params.SetValue(Desc, k);
                            k++;

                            //6

                            Params.SetValue(Session["UserName"], k);
                            k++;

                            //7
                            Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), k);
                            k++;


                            #endregion Prepare Parameters
                            ObjMAL.AddMasterAuditLog(Params);
                            #endregion
                            Clear();
                            btnList_Click(sender, e);
                            //srclbl.Text = destlbl.Text = string.Empty;
                            lblstatus.ForeColor = Color.Green;
                            lblstatus.Text = "Route Updated Sucessfully...";

                        }
                        else
                        {
                            Clear();
                            btnList_Click(null, null);
                            lblstatus.ForeColor = Color.Red;
                            lblstatus.Text = "Route Insertion Failed...";
                        }
                    }
                    else
                    {
                        lblstatus.Text = "Please select origin and destination...";
                        lblstatus.ForeColor = Color.Red;
                    }

                    #endregion
                }
            }

            catch (Exception ex)
            { }
            finally
            {
            }
        }
        #endregion

        #region OLD Method
        protected void ddlDest_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlDest.SelectedIndex == 0)
            //{ destlbl.Text = ""; }
            //else
            //{
            //    destlbl.Font.Bold = true;
            //    destlbl.Text = "-" + ddlDest.SelectedItem.Text;
            //}
        }

        protected void ddlOrigin_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlOrigin.SelectedIndex == 0)
            //{ srclbl.Text = ""; }
            //else
            //{
            //    srclbl.Font.Bold = true;
            //    srclbl.Text = ddlOrigin.SelectedItem.Text + "-";
            //}
        }
        #endregion

        protected void Clear()
        {
            ddlOrigin.SelectedIndex = 0;
            ddlDest.SelectedIndex = 0;
            txtRoute.Text = string.Empty;
            lblstatus.Text = string.Empty;
            chkAct.Checked = false;
            btnsave.Text = "Save";
            BindEmptyRow();
        }

        protected void listRouteGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            lblstatus.Text = string.Empty;

            #region Edit
            if (e.CommandName == "Edit")
            {
                int RowIndex = Convert.ToInt32(e.CommandArgument);
                int srno = int.Parse(((Label)(listRouteGrid.Rows[RowIndex].FindControl("lblsrnoo"))).Text.ToString());
                Session["RouteSrNo"] = srno;
                try
                {

                    string org = ((Label)(listRouteGrid.Rows[RowIndex].FindControl("lblsource"))).Text.ToString();
                    ddlOrigin.SelectedIndex = ddlOrigin.Items.IndexOf(((ListItem)ddlOrigin.Items.FindByValue(org)));

                    string dest = ((Label)(listRouteGrid.Rows[RowIndex].FindControl("lbldest"))).Text.ToString();
                    ddlDest.SelectedIndex = ddlDest.Items.IndexOf(((ListItem)ddlDest.Items.FindByValue(dest)));

                    string route = ((Label)listRouteGrid.Rows[RowIndex].FindControl("lblroute")).Text;
                    route = route.Remove(0, 4);
                    route = route.Remove(route.Length - 4, 4);
                    txtRoute.Text = route;

                    string IsAct = ((Label)(listRouteGrid.Rows[RowIndex].FindControl("lblact"))).Text.ToString();
                    if (IsAct.ToUpper() == "ACTIVE")
                        chkAct.Checked = true;
                    else
                        chkAct.Checked = false;

                    btnsave.Text = "Update";

                }
                catch (Exception ex)
                { }
            }


            #endregion

            #region Delete

            if (e.CommandName == "DeleteRecord")
            {
                int RowIndex = Convert.ToInt32(e.CommandArgument);
                int srno = int.Parse(((Label)(listRouteGrid.Rows[RowIndex].FindControl("lblsrnoo"))).Text.ToString());
                string org = ((Label)(listRouteGrid.Rows[RowIndex].FindControl("lblsource"))).Text.ToString();
                string dest = ((Label)(listRouteGrid.Rows[RowIndex].FindControl("lbldest"))).Text.ToString();
                string route = ((Label)(listRouteGrid.Rows[RowIndex].FindControl("lblroute"))).Text.ToString();
                try
                {
                    #region Prepare Parameters
                    DataSet ds = new DataSet();
                    object[] Params = new object[1];
                    int i = 0;

                    //1
                    Params.SetValue(srno, i);
                    i++;

                    #endregion Prepare Parameters

                    int ID = 0;
                    int res = objBAL.DeleteRouteDetail(Params);
                    if (res == 0)
                    {
                        #region for Master Audit Log
                        MasterAuditBAL ObjMAL = new MasterAuditBAL();
                        #region Prepare Parameters
                        object[] Paramsss = new object[7];
                        int k = 0;

                        //1
                        Paramsss.SetValue("Airline Route", k);
                        k++;

                        //2
                        string stn = org + "-" + dest;
                        Paramsss.SetValue(stn, k);
                        k++;

                        //3
                        Paramsss.SetValue("DELETE", k);
                        k++;

                        //4
                        string Msg = "Route Deleted";
                        Paramsss.SetValue(Msg, k);
                        k++;

                        //5
                        string Desc = route;
                        Paramsss.SetValue(Desc, k);
                        k++;

                        //6

                        Paramsss.SetValue(Session["UserName"], k);
                        k++;

                        //7
                        Paramsss.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), k);
                        k++;

                        #endregion Prepare Parameters
                        ObjMAL.AddMasterAuditLog(Paramsss);
                        #endregion
                        Clear();
                        btnList_Click(null, null);
                        lblstatus.Text = "Record Deleted Successfully";
                        lblstatus.ForeColor = Color.Red;

                    }

                }

                catch (Exception ex)
                {

                }
            }
            # endregion Delete

        }

        protected void listRouteGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (ddlVia.SelectedIndex > 0)
            {
                if (txtRoute.Text == "")
                {
                    txtRoute.Text = ddlVia.SelectedValue.ToString();
                }
                else
                {
                    txtRoute.Text += "-" + ddlVia.SelectedValue.ToString();
                }
            }
            ddlVia.SelectedIndex = 0;
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            string text = txtRoute.Text;
            if (text != "")
            {
                if (txtRoute.Text.Length > 3)
                    txtRoute.Text = text.Substring(0, text.Length - 4);
                else
                    txtRoute.Text = text.Substring(0, text.Length - 3);

            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlOrigin.SelectedIndex = 0;
            ddlDest.SelectedIndex = 0;
            txtRoute.Text = string.Empty;
            lblstatus.Text = string.Empty;
            BindEmptyRow();

        }

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
