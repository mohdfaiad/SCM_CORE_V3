using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls.WebParts;
using QID.DataAccess;
using System.IO;
using System.Collections;
using BAL;

using System.Configuration;
//7-6-2012
namespace ProjectSmartCargoManager
{
    public partial class ListAgentMaster : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());
        AgentBAL objBLL = new AgentBAL();
        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                
                if (!IsPostBack)
                {
                    LoadAgentName();
                    LoadAirportCodes();
                    btnExport.Enabled = false;
                    //GetData();
                    //LoadGridCreditDetails();
                    //GetCity();
                    //((TextBox)grdCreditdetails.Rows[0].FindControl("SerialNumber")).Visible = false;

                    #region Define PageSize for grid as per configuration
                    try
                    {
                        LoginBL objConfig = new LoginBL();
                        grdCreditdetails.PageSize = Convert.ToInt32(objConfig.GetMasterConfiguration("StdListPageSize"));
                        objConfig = null;
                    }
                    catch (Exception ex)
                    { }
                    #endregion
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion Load
        #region GetCity
        //public void GetCity()
        //{
        //    try
        //    {
        //        SQLServer db = new SQLServer(Global.GetConnectionString());
        //        // SQLServer db = new SQLServer(ConnectionString);
        //        //ConnectionString
        //        DataSet ds = db.SelectRecords("Sp_GetCityName");

        //        if (ds != null)
        //        {
        //            if (ds.Tables.Count > 0)
        //            {
        //                if (ds.Tables[0].Rows.Count > 0)
        //                {

        //                    ddlCity.DataSource = ds;
        //                    ddlCity.DataMember = ds.Tables[0].TableName;
        //                    ddlCity.DataTextField = "CityCode";

        //                    ddlCity.DataBind();
        //                    ddlCity.SelectedIndex = -1;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    { }

      //  }
        #endregion GetCity
        #region Load AgentName
        public void LoadAgentName()
        {
            try
            {
                DataSet ds = objBLL.GetAgentListNew(Convert.ToString(Session["AgentCode"]));
                if (ds != null)
                {
                    ddlagentcode.DataSource = ds;
                    ddlagentcode.DataMember = ds.Tables[0].TableName;
                    ddlagentcode.DataTextField = "AgentCode";
                    ddlagentcode.DataValueField = "AgentName";
                    ddlagentcode.DataBind();
                    ddlagentcode.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion LoadAgentName

        #region Load Airport Codes
        public void LoadAirportCodes()
        {
            DataSet ds = null;

            try
            {
                ds = da.SelectRecords("spGetAirportCodes");

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlAirportCode.DataSource = ds;
                    ddlAirportCode.DataMember = ds.Tables[0].TableName;
                    ddlAirportCode.DataValueField = ds.Tables[0].Columns["AirportCode"].ColumnName;
                    ddlAirportCode.DataTextField = ds.Tables[0].Columns["Airport"].ColumnName;
                    ddlAirportCode.DataBind();
                    ddlAirportCode.Items.Insert(0, new ListItem("Select", "0"));
                    //ddlAirportCode.SelectedIndex = -1;

                }

            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Function
        public void GetData()
        {
            try
            {

                string[] paramname = new string[2];
                paramname[0] = "AgentCode";
                paramname[1] = "City";

                string AgentCode = string.Empty;
                string City = string.Empty;


                object[] paramvalue = new object[2];


                if (ddlagentcode.Text.Trim() == "" || ddlagentcode.Text.Trim() == "Select")
                {
                    paramvalue[0] = "";
                }
                else
                {
                    paramvalue[0] = ddlagentcode.SelectedItem.Text  ;
                }

                //if (txtcity.Text.Trim() == "")
                //{
                //    paramvalue[1] = "";
                //}
                //else
                //{
                //    paramvalue[1] = txtcity.Text.Trim() ;

                //}

                if (ddlAirportCode.Text.Trim() == "" || ddlAirportCode.Text.Trim() == "Select")
                {
                    paramvalue[1] = ddlAirportCode.SelectedItem.Value;

                }
                else
                {
                    paramvalue[1] = "";

                }


                SqlDbType[] paramtype = new SqlDbType[2];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;

                DataSet ds = da.SelectRecords("SPGetAgentCreadit", paramname, paramvalue, paramtype);
                if (ds != null)
                {
                    Session["dsData"] = ds.Tables[0];  
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            grdCreditdetails.DataSource = ds.Tables[0];
                            grdCreditdetails.DataBind();
                        }
                        else
                        {
                            grdCreditdetails.Visible = false;
                            lblStatus.Text = "No Data Found";
                            //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "NoData()", true);
                            //LoadGridCreditDetails();
                        }
                    }
                }
            }
            catch (Exception ex)
            { }

        }
        #endregion Function
        #region List
        protected void btnList_Click1(object sender, EventArgs e)
        {

            try
            {

                string[] paramname = new string[2];
                paramname[0] = "AgentCode";
                paramname[1] = "City";

                string AgentCode = string.Empty;
                string City = string.Empty;


                object[] paramvalue = new object[2];


                if (ddlagentcode.Text.Trim() == "" || ddlagentcode.Text.Trim() == "Select")
                {
                    paramvalue[0] = "";
                }
                else
                {
                    //AgentCode = ddlagentcode.SelectedItem.Text;
                    //paramvalue[0] = ddlagentcode.SelectedItem.Text.Substring(0,AgentCode.IndexOf(' '));
                    AgentCode = ddlagentcode.SelectedItem.Text.ToString();
                    string str = "- "+ddlagentcode.SelectedItem.Value.ToString();
                    AgentCode = AgentCode.Replace(str, "");
                    AgentCode = AgentCode.Trim(' ');
                    //AgentCode = AgentCode.Trim('-');

                    paramvalue[0] = AgentCode;
                }

                //if (txtcity.Text.Trim() == "")
                //{
                //    paramvalue[1] = "";
                //}
                //else
                //{
                //    paramvalue[1] = txtcity.Text;

                //}

                if (ddlAirportCode.Text.Trim() == "" || ddlAirportCode.SelectedItem.Text == "Select")
                {
                    paramvalue[1] = "";
                }
                else
                {
                    City = ddlAirportCode.SelectedItem.Text.ToString();
                   // string str2 = "- " + ddlAirportCode.SelectedItem.Value.ToString();

                    //string str2 = ddlAirportCode.SelectedItem.Value.ToString();
                    //City = City.Replace(str2, "");
                    //City = City.Trim(' ');
                    //paramvalue[1] = City;

                    string str2 = ddlAirportCode.SelectedItem.Value.ToString();
                    str2 = str2.Replace(City, "");
                    str2 = str2.Trim(' ');
                    paramvalue[1] = str2;
                }


                SqlDbType[] paramtype = new SqlDbType[2];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;

                DataSet ds = da.SelectRecords("SPGetAgentCreadit", paramname, paramvalue, paramtype);
                if (ds != null)
                {
                    Session["dsData"] = ds.Tables[0];
                   
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Session["AgentMaster"] = ds;
                            btnExport.Enabled = true;
                            grdCreditdetails.DataSource = ds.Tables[0];
                            grdCreditdetails.DataBind();
                           grdCreditdetails.Visible = true;
                        }
                        else
                        {
                            grdCreditdetails.Visible = false;
                            //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "NoData()", true);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DGRAlert", "<SCRIPT LANGUAGE='javascript'>NoData();</script>", false);
                            LoadGridCreditDetails();
                            //LoadGridCreditDetails();
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion List

        #region Export
        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            DataTable dt = null;
            try
            {
                if (Session["AgentMaster"] == null)
                {
                    lblStatus.Text = "No Record Found";
                    return;
                }
                ds = (DataSet)Session["AgentMaster"];
                dt = (DataTable)ds.Tables[1];
                string attachment = "attachment; filename=AgentMaster.xls";
                Response.ClearContent();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                string tab = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    Response.Write(tab + dc.ColumnName);
                    //tab = "\t";
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
                        //tab = "\t";
                        tab = "\t";
                    }
                    Response.Write("\n");
                }
                Response.End();

            }

            catch (Exception ex)
            { 
            
            }
            finally
            {
                ds = null;
                dt = null;
            }
        
        }
#endregion Export
        //#region LoadGridCreditDetails
        //public void LoadGridCreditDetails()
        //{

        //    DataTable myDataTable = new DataTable();
        //    DataColumn myDataColumn;
        //    DataSet Ds = new DataSet();


        //    myDataColumn = new DataColumn();
        //    myDataColumn.DataType = Type.GetType("System.String");
        //    myDataColumn.ColumnName = "SerialNumber";
        //    myDataTable.Columns.Add(myDataColumn);

        //    myDataColumn = new DataColumn();
        //    myDataColumn.DataType = Type.GetType("System.String");
        //    myDataColumn.ColumnName = "AgentName";
        //    myDataTable.Columns.Add(myDataColumn);

        //    myDataColumn = new DataColumn();
        //    myDataColumn.DataType = Type.GetType("System.String");
        //    myDataColumn.ColumnName = "bankName";
        //    myDataTable.Columns.Add(myDataColumn);

        //    myDataColumn = new DataColumn();
        //    myDataColumn.DataType = Type.GetType("System.String");
        //    myDataColumn.ColumnName = "BankGuranteeNumber";
        //    myDataTable.Columns.Add(myDataColumn);

        //    myDataColumn = new DataColumn();
        //    myDataColumn.DataType = Type.GetType("System.String");
        //    myDataColumn.ColumnName = "BankGuranteeAmount";
        //    myDataTable.Columns.Add(myDataColumn);

        //    myDataColumn = new DataColumn();
        //    myDataColumn.DataType = Type.GetType("System.String");
        //    myDataColumn.ColumnName = "CreditAmount";
        //    myDataTable.Columns.Add(myDataColumn);

        //    myDataColumn = new DataColumn();
        //    myDataColumn.DataType = Type.GetType("System.String");
        //    myDataColumn.ColumnName = "CreditRemaining";
        //    myDataTable.Columns.Add(myDataColumn);

        //    myDataColumn = new DataColumn();
        //    myDataColumn.DataType = Type.GetType("System.String");
        //    myDataColumn.ColumnName = "CustomerCode";
        //    myDataTable.Columns.Add(myDataColumn);

        //    myDataColumn = new DataColumn();
        //    myDataColumn.DataType = Type.GetType("System.String");
        //    myDataColumn.ColumnName = "City";
        //    myDataTable.Columns.Add(myDataColumn);

        //    myDataColumn = new DataColumn();
        //    myDataColumn.DataType = Type.GetType("System.String");
        //    myDataColumn.ColumnName = "Email";
        //    myDataTable.Columns.Add(myDataColumn);

        //    myDataColumn = new DataColumn();
        //    myDataColumn.DataType = Type.GetType("System.String");
        //    myDataColumn.ColumnName = "IATAAgentCode";
        //    myDataTable.Columns.Add(myDataColumn);



        //    DataRow dr;
        //    dr = myDataTable.NewRow();
        //    dr["SerialNumber"] = "";
        //    dr["AgentName"] = "";//"5";
        //    dr["bankName"] = "";// "5";
        //    dr["BankGuranteeNumber"] = "";// "9";
        //    dr["BankGuranteeAmount"] = "";
        //    dr["CreditAmount"] = "";
        //    dr["CreditRemaining"] = "";

        //    dr["CustomerCode"] = "";
        //    dr["City"] = "";
        //    dr["Email"] = "";
        //    dr["IATAAgentCode"] = "";

        //    myDataTable.Rows.Add(dr);

        //    grdCreditdetails .DataSource = null;
        //    grdCreditdetails.DataSource = myDataTable;
        //    grdCreditdetails.DataBind();
        //}
        //#endregion LoadGridCreditDetails
        #region Clear
        protected void btntextclear_Click(object sender, EventArgs e)
        {
            //ddlagentcode.SelectedIndex = -1;
            //ddlAirportCode.SelectedIndex = -1;
            //txtcity.Text = string.Empty;
            //txtagentname.Text = string.Empty;
            //ddlCity.SelectedIndex = -1;
            //GetData();
            Response.Redirect("~/ListAgentMaster.aspx");
        }
        #endregion Clear
        #region LoadGrid
        public void LoadGridCreditDetails()
        {
            try
            {
                DataTable myDataTable = new DataTable();
                DataColumn myDataColumn;
                DataSet Ds = new DataSet();

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "SerialNumber";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "AgentName";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "AgentCode";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "CustomerCode";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "IATAAgentCode";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "City";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ControllingLocator";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ValidFrom";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ValidTo";
                myDataTable.Columns.Add(myDataColumn);

                DataRow dr;
                dr = myDataTable.NewRow();
                dr["SerialNumber"] = "";//"5";
                dr["AgentName"] = "";// "5";
                dr["AgentCode"] = "";// "9";
                dr["CustomerCode"] = "";
                dr["IATAAgentCode"] = "";
                dr["City"] = "";
                dr["ControllingLocator"] = "";
                dr["ValidFrom"] = "";
                dr["ValidTo"] = "";
                myDataTable.Rows.Add(dr);

                grdCreditdetails.DataSource = null;
                grdCreditdetails.DataSource = myDataTable;
                grdCreditdetails.DataBind();
            }
            catch (Exception ex)
            { }
        }
        #endregion LoadGrid
        #region CreditDetailsRowCommand
        protected void grdCreditdetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Edit" || e.CommandName == "View")
                {
                    //int index = Convert.ToInt32(e.CommandArgument) + (grdCreditdetails.PageIndex * grdCreditdetails.PageSize);
                    int index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = grdCreditdetails.Rows[index];
                    string CreditSrNo = ((Label)row.FindControl("lblSrNo")).Text; //row.Cells[0].Text;
                    Response.Redirect("~/AgentMaster.aspx?cmd=" + e.CommandName + "&CreditNo=" + CreditSrNo);
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion CreditDetailsRowCommand
        #region Indexchange To show name
        protected void ddlagentcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtagentname.Text = ddlagentcode.SelectedValue;
        }
        #endregion Indexchange To show name

        protected void grdCreditdetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCreditdetails.PageIndex = e.NewPageIndex;
            grdCreditdetails.DataSource = (DataTable)Session["dsData"];
            grdCreditdetails.DataBind();
            btnExport.Enabled = true;
        }


    }
}
