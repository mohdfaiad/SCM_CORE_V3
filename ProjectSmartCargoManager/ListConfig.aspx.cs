using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BAL;
using QID.DataAccess;
using System.Drawing;

namespace ProjectSmartCargoManager
{
    public partial class ListConfig : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                StnList();
            }
        }

        private void StnList()
        {
            try
            {
                DataSet ds = da.SelectRecords("spGetAirportCodes");
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            ddlOrigin.DataSource = ds;
                            ddlOrigin.DataMember = ds.Tables[0].TableName;
                            ddlOrigin.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            ddlOrigin.DataTextField = ds.Tables[0].Columns[1].ColumnName;
                            ddlOrigin.DataBind();
                            ddlOrigin.Items.Insert(0, new ListItem("Select", string.Empty));

                            ddlDestination.DataSource = ds;
                            ddlDestination.DataMember = ds.Tables[0].TableName;
                            ddlDestination.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            ddlDestination.DataTextField = ds.Tables[0].Columns[1].ColumnName;
                            ddlDestination.DataBind();
                            ddlDestination.Items.Insert(0, new ListItem("Select", string.Empty));

                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        # region btnList_Click
        protected void btnList_Click(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;

            #region Prepare Parameters

            object[] RateLineInfo = new object[7];
            string[] ColumnNames = new string[7];
            SqlDbType[] DataType = new SqlDbType[7];

            int i = 0;

            //0
            RateLineInfo.SetValue(ddlOrigin.SelectedValue, i);
            i++;

            //1
            RateLineInfo.SetValue(ddlDestination.SelectedValue, i);
            i++;

            if (txtFromDate.Text.Trim() == "")
            {
                if (txtToDate.Text.Trim() != "")
                {
                    lblStatus.Text = "Please enter From date";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
            }

            if (txtToDate.Text.Trim() == "")
            {
                if (txtFromDate.Text.Trim() != "")
                {
                    lblStatus.Text = "Please enter To date";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
            }

            //2
            if (txtFromDate.Text.Trim() != "")
            {
                DateTime dtfrom;

                try
                {
                    dtfrom = Convert.ToDateTime(txtFromDate.Text);
                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                    lblStatus.Text = "Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                RateLineInfo.SetValue(Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd HH:mm:ss"), i);
            }
            else
            {
                RateLineInfo.SetValue("", i);
            }
            i++;

            //3
            if (txtToDate.Text.Trim() != "")
            {
                DateTime dtto;

                try
                {
                    dtto = Convert.ToDateTime(txtToDate.Text);
                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                    lblStatus.Text = "Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                RateLineInfo.SetValue(Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd HH:mm:ss"), i);
            }
            else
            {
                RateLineInfo.SetValue("", i);
            }
            i++;

            //4
            RateLineInfo.SetValue("", i);
            i++;

            //5
            RateLineInfo.SetValue(txtChargeName.Text, i);
            i++;

            //6
            RateLineInfo.SetValue(txtParam.Text, i);

            int j = 0;
            ColumnNames.SetValue("Origin", j);
            DataType.SetValue(SqlDbType.VarChar, j);
            j++;


            ColumnNames.SetValue("Destination", j);
            DataType.SetValue(SqlDbType.VarChar, j);
            j++;


            ColumnNames.SetValue("FromDate", j);
            DataType.SetValue(SqlDbType.VarChar, j);
            j++;


            ColumnNames.SetValue("ToDate", j);
            DataType.SetValue(SqlDbType.VarChar, j);
            j++;

            ColumnNames.SetValue("ChargeCode", j);
            DataType.SetValue(SqlDbType.VarChar, j);
            j++;

            ColumnNames.SetValue("ChargeName", j);
            DataType.SetValue(SqlDbType.VarChar, j);
            j++;

            ColumnNames.SetValue("Parameter", j);
            DataType.SetValue(SqlDbType.VarChar, j);

            #endregion

            DataSet ds = da.SelectRecords("SP_GetListConfig", ColumnNames, RateLineInfo, DataType);
            if (ds != null)
            {
                if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                {

                    GRDList.DataSource = ds;
                    GRDList.DataMember = ds.Tables[0].TableName;
                    GRDList.DataBind();
                    Session["ds_ListConfig"] = ds;
                    lblStatus.Text = string.Empty;
                }
                else
                {
                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = Color.Red;
                    GRDList.DataSource = null;
                    GRDList.DataBind();
                }
            }
        }
        # endregion btnList_Click

        #region Grid Row Commmand "Edit" and "View"
        protected void GRDList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Edit" || e.CommandName == "View")
                {
                    string SrNo = ((HiddenField)GRDList.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("Hid")).Value;
                    Response.Redirect("~/SCMConfigLine.aspx?cmd=" + e.CommandName + "&SrNo=" + SrNo + "");
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = "" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }
        #endregion

        protected void GRDList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dsResult = (DataSet)Session["ds_ListConfig"];

            GRDList.PageIndex = e.NewPageIndex;
            GRDList.DataSource = dsResult.Copy();
            GRDList.DataBind();

        }

        #region DDLDestinationLevel_SelectedIndexChanged
        protected void DDLDestinationLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            ListDataViewBAL objDataViewBAL = new ListDataViewBAL();
            DataSet dsResult = new DataSet();
            string errormessage = "";


            if (DDLDestinationLevel.SelectedValue == "A")
            {
                DataSet ds = da.SelectRecords("spGetAirportCodes");
                ddlDestination.DataSource = ds;
                ddlDestination.DataMember = ds.Tables[0].TableName;
                ddlDestination.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                ddlDestination.DataTextField = ds.Tables[0].Columns[1].ColumnName;
                ddlDestination.DataBind();
                ddlDestination.Items.Insert(0, new ListItem("Select", string.Empty));
            }
            else if (DDLDestinationLevel.SelectedValue == "N")
            {
                DataSet ds = da.SelectRecords("SP_GetAllStationCodeName", "level", "country", SqlDbType.VarChar);
                ddlDestination.DataSource = ds;
                ddlDestination.DataMember = ds.Tables[0].TableName;
                ddlDestination.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                ddlDestination.DataTextField = ds.Tables[0].Columns[2].ColumnName;
                ddlDestination.DataBind();
                ddlDestination.Items.Insert(0, new ListItem("Select", string.Empty));
            }

            else
            {
                string level = DDLDestinationLevel.SelectedItem.Value;
                if (objDataViewBAL.FillDdl("Origin", ref dsResult, ref errormessage, new string[] { level }))
                {
                    ddlDestination.DataSource = dsResult;
                    ddlDestination.DataMember = dsResult.Tables[0].TableName;
                    ddlDestination.DataValueField = dsResult.Tables[0].Columns[0].ColumnName;
                    ddlDestination.DataTextField = dsResult.Tables[0].Columns[0].ColumnName;
                    ddlDestination.DataBind();
                    ddlDestination.Items.Insert(0, new ListItem("Select", string.Empty));
                }
            }
        }
        #endregion

        #region DDLOriginLevel_SelectedIndexChanged
        protected void DDLOriginLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListDataViewBAL objDataViewBAL = new ListDataViewBAL();
            DataSet dsResult = new DataSet();
            string errormessage = "";

            if (DDLOriginLevel.SelectedValue == "A")
            {
                DataSet ds = da.SelectRecords("spGetAirportCodes");
                ddlOrigin.DataSource = ds;
                ddlOrigin.DataMember = ds.Tables[0].TableName;
                ddlOrigin.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                ddlOrigin.DataTextField = ds.Tables[0].Columns[1].ColumnName;
                ddlOrigin.DataBind();
                ddlOrigin.Items.Insert(0, new ListItem("Select", string.Empty));
            }
            else if (DDLOriginLevel.SelectedValue == "N")
            {
                DataSet ds = da.SelectRecords("SP_GetAllStationCodeName", "level", "country", SqlDbType.VarChar);
                ddlOrigin.DataSource = ds;
                ddlOrigin.DataMember = ds.Tables[0].TableName;
                ddlOrigin.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                ddlOrigin.DataTextField = ds.Tables[0].Columns[2].ColumnName;
                ddlOrigin.DataBind();
                ddlOrigin.Items.Insert(0, new ListItem("Select", string.Empty));
            }
            else
            {
                string level = DDLOriginLevel.SelectedItem.Value;
                if (objDataViewBAL.FillDdl("Origin", ref dsResult, ref errormessage, new string[] { level }))
                {
                    ddlOrigin.DataSource = dsResult;
                    ddlOrigin.DataMember = dsResult.Tables[0].TableName;
                    ddlOrigin.DataValueField = dsResult.Tables[0].Columns[0].ColumnName;
                    ddlOrigin.DataTextField = dsResult.Tables[0].Columns[0].ColumnName;
                    ddlOrigin.DataBind();
                    ddlOrigin.Items.Insert(0, new ListItem("Select", string.Empty));
                }
            }
        }
        #endregion

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                if ((DataSet)Session["ds_ListConfig"] == null)
                    return;

                ds = (DataSet)Session["ds_ListConfig"];
                dt = (DataTable)ds.Tables[0];
                //ExportToExcel(dt, "RateLines.xls");
                //dt = city.GetAllCity();//your datatable 
                string attachment = "attachment; filename=ConfigList.xls";
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
            catch (Exception ex)
            { }
            finally
            {
                ds = null;
                dt = null;
            }

        }
    }
}
