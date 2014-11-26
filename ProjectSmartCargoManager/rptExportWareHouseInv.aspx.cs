using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using BAL;
using System.Drawing;
using QID.DataAccess;
using System.IO;

namespace ProjectSmartCargoManager
{
    public partial class rptExportWareHouseInv : System.Web.UI.Page
    {
        //BALException objBAL = new BALException();
        //SQLServer da = new SQLServer(Global.GetConnectionString());

        public static string CurrTime = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetAirportCode();
                //LoadGridUserList();
                btnExport.Visible = true;

                DateTime dtCurr = Convert.ToDateTime(Session["IT"].ToString());
                CurrTime = dtCurr.ToString("dd/MM/yyyy hh:mm:ss tt");

                txtvalidfrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtvalidto.Text = DateTime.Now.ToString("dd/MM/yyyy");

            }
        }

        #region Add New Row to Grid
        public void LoadGridUserList()
        {

            DataTable myDataTable = new DataTable();
            DataColumn myDataColumn;
            DataSet Ds = new DataSet();

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "AWBPrefix";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "AWBNumber";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "OriginDate";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Origin";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Destination";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "BookedFlt";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "BookedFltDate";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "TotalPieces";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "TotalWeight";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Description";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Shipper";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Consignee";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "PiecesOnhand";
            myDataTable.Columns.Add(myDataColumn);


            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "DatePieceswereOnhand";
            myDataTable.Columns.Add(myDataColumn);


            DataRow dr;
            dr = myDataTable.NewRow();
            dr["AWBPrefix"] = "";
            dr["AWBNumber"] = "";
            dr["FltOrigin"] = "";
            dr["FltDestination"] = "";
            dr["FltDate"] = "";
            dr["FltNumber"] = "";
            dr["AcceptedPcs"] = "";
            dr["AcceptedWt"] = "";
            dr["OffloadFlightNo"] = "";
            dr["OffloadPcs"] = "";
            dr["OffloadWeight"] = "";
            dr["OffloadOn"] = "";
            dr["OffloadLoc"] = "";
            dr["ShipperName"] = "";


            myDataTable.Rows.Add(dr);

            grdexpList.DataSource = null;
            grdexpList.DataSource = myDataTable;
            grdexpList.DataBind();
            Session["dsdata"] = myDataTable.Copy();

        }
        #endregion


        # region Get Origin List

        private void GetAirportCode()
        {
            BALException objBAL = new BALException();
            DataSet ds = null;

            try
            {
                ds = objBAL.GetAirportCodeList(ddlAirport.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlAirport.DataSource = ds;
                            ddlAirport.DataMember = ds.Tables[0].TableName;
                            ddlAirport.DataValueField = "AirportCode";

                            ddlAirport.DataTextField = "Airport";
                            ddlAirport.DataBind();
                            ddlAirport.Items.Insert(0, "All");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                objBAL = null;
                if (ds != null)
                    ds.Dispose();
            }
        }

        # endregion GetOriginCode List

        protected void btnList_Click(object sender, EventArgs e)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet dsexp = null;
            string[] PName = new string[3];
            object[] PValue = new object[3];
            SqlDbType[] PType = new SqlDbType[3];

            if (txtvalidfrom.Text == "" || txtvalidto.Text == "")
            {
                lblStatus.Text = "Please select date";
                lblStatus.ForeColor = Color.Red;
                grdexpList.Visible = false;
                return;
            }
            #region ValidateDays
            ReportBAL objBal = new ReportBAL();
            string strResult = string.Empty;

            try
            {
                strResult = objBal.GetReportInterval(DateTime.ParseExact(txtvalidfrom.Text.Trim(), "dd/MM/yyyy", null), DateTime.ParseExact(txtvalidto.Text.Trim(), "dd/MM/yyyy", null));
            }
            catch
            {
                strResult = "";
            }
            finally
            {
                objBal = null;
            }
            if (strResult != "")
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = strResult;
                grdexpList.Visible = false;
                txtvalidfrom.Focus();
                return;
            }
            #endregion

            try
            {
                string Airport;
                string frmdt = "", todt = "";
                int i = 0;
                if (ddlAirport.SelectedItem.Text.Trim() == "All")
                    Airport = "";
                else
                    Airport = ddlAirport.SelectedValue.ToString().Trim();
               // Airport = ddlAirport.SelectedValue.ToString();
                #region Parameter

                PName[0] = "AirportCode";
                PName[1] = "Frmdt";
                PName[2] = "Todt";

                PValue[0] = Airport;
                if (txtvalidfrom.Text != "")
                {
                    frmdt = txtvalidfrom.Text.Trim();
                    PValue[1] = frmdt;

                }

                if (txtvalidto.Text != "")
                {
                    todt = txtvalidto.Text.Trim();
                   // PValue[2] = Convert.ToDateTime(todt).ToString("dd/MM/yyyy");
                    PValue[2] = todt;
                }

                PType[0] = SqlDbType.VarChar;
                PType[1] = SqlDbType.VarChar;
                PType[2] = SqlDbType.VarChar;

                #endregion

                dsexp = da.SelectRecords("[GetExpWareHouseInvList]", PName, PValue, PType);
                Session["dsexp"] = dsexp;
                try
                {
                    //Session["dsExp"] = dsFLAB;
                    if (dsexp != null)
                    {
                        if (dsexp.Tables != null)
                        {
                            if (dsexp.Tables.Count > 0)
                            {
                                if (dsexp.Tables[0].Rows.Count > 0)
                                {
                                    grdexpList.PageIndex = 0;
                                    grdexpList.DataSource = dsexp;
                                    grdexpList.DataMember = dsexp.Tables[0].TableName;
                                    grdexpList.DataBind();
                                    grdexpList.Visible = true;
                                    //dsFLAB.Clear();
                                    btnExport.Visible = true;
                                    lblStatus.Text = string.Empty;
                                    //for (int j = 0; j < grdexpList.Rows.Count; j++)
                                    //{
                                    //    if (((Label)(grdexpList.Rows[j].FindControl("lblOffloadDate"))).Text.ToString() == "1/1/1900 12:00:00 AM")
                                    //    {
                                    //        ((Label)(grdexpList.Rows[j].FindControl("lblOffloadDate"))).Text = "-";
                                    //    }

                                    //    //else if (((Label)(grvNFLABList.Rows[j].FindControl("lblLoadedStatus"))).Text.ToString() == "False")
                                    //    //{
                                    //    //    ((Label)(grvNFLABList.Rows[j].FindControl("lblLoadedStatus"))).Text = "Not Loaded";
                                    //    //}
                                    //}
                                    Session["WarhouseExport"] = dsexp.Tables[0];
                                    SaveUserActivityLog("");
                                }
                                else
                                {
                                    lblStatus.Text = "No data found for given search criteria";
                                    lblStatus.ForeColor = Color.Red;
                                   grdexpList.Visible = false;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    String ErrorLog = ex.Message;
                    SaveUserActivityLog(ErrorLog);
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                da = null;
                if (dsexp != null)
                    dsexp.Dispose();
                PName = null;
                PValue = null;
                PType = null;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtvalidfrom.Text = "";//DateTime.Now.ToString("dd/MM/yyyy");
            txtvalidto.Text = "";//;DateTime.Now.ToString("dd/MM/yyyy");
            grdexpList.Visible = false;
            ddlAirport.SelectedIndex = 0;
            btnExport.Visible = true;
            lblStatus.Text = string.Empty;

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = null;
            DataTable dt = null;

            try
            {
                if ((DataSet)Session["dsexp"] == null)
                {   //if(ds == null)
                    SaveUserActivityLog(lblStatus.Text);
                    return;
                }

                dsExp = (DataSet)Session["dsexp"];
                //dsExp = ds;
                dt = (DataTable)dsExp.Tables[0];
                //dt = city.GetAllCity();//your datatable 
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
            catch (Exception ex)
            { }
            finally
            {
                if (dsExp != null)
                    dsExp.Dispose();
                if (dt != null)
                    dt.Dispose();
            }
        }

        # region grdexpList_RowCommand
        protected void grdexpList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //try
            //{
            //    if (e.CommandName == "Manage")
            //    {
            //        lblStatus.Text = "";
            //        //int RowIndex = Convert.ToInt32(e.CommandArgument);
            //        //Label lblAWBNumber = (Label)grvNFLABList.Rows[RowIndex].FindControl("lblAWBNumber");
            //        //Label lblOrigin = (Label)grvNFLABList.Rows[RowIndex].FindControl("lblOrigin");
            //        //Label lblDestination = (Label)grvNFLABList.Rows[RowIndex].FindControl("lblDestination");
            //        ////Label lblFromDate = (Label)grvNFLABList.Rows[RowIndex].FindControl("lblFromDate");
            //        ////Label lblToDate = (Label)grvNFLABList.Rows[RowIndex].FindControl("lblToDate");
            //        //Label lblAgentCode = (Label)grvNFLABList.Rows[RowIndex].FindControl("lblAgentCode");



            //        //btnSave.Text = "Update";

            //    }
            //}
            //catch (Exception ex)
            //{

            //}

        }
        # endregion grdexpList_RowCommand

        # region grdexpList_RowEditing
        protected void grdexpList_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        # endregion grdexpList_RowEditing

        # region grdexpList_PageIndexChanging
        protected void grdexpList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //SQLServer da = new SQLServer(Global.GetConnectionString());
            //DataSet dsExp = null;
            //string[] PName = new string[3];
            //object[] PValue = new object[3];
            //SqlDbType[] PType = new SqlDbType[3];

            //try
            //{
            //    string Airport;
            //    string frmdt = "", todt = "";
            //    //int i = 0;
            //    if (ddlAirport.SelectedItem.Text.Trim() == "All")
            //        Airport = "";
            //    else
            //        Airport = ddlAirport.SelectedValue.ToString().Trim();
            //   // Airport = ddlAirport.SelectedItem.Text.Trim() == "All" ? "" : ddlAirport.SelectedItem.Text.Trim();
            //    #region Parameter

            //    PName[0] = "AirportCode";
            //    PName[1] = "Frmdt";
            //    PName[2] = "Todt";

            //    PValue[0] = Airport;
            //    if (txtvalidfrom.Text != "")
            //    {
            //        frmdt = txtvalidfrom.Text.Trim();
            //        PValue[1] = Convert.ToDateTime(frmdt).ToString("dd/MM/yyyy");

            //    }

            //    if (txtvalidto.Text != "")
            //    {
            //        todt = txtvalidto.Text.Trim();
            //        PValue[2] = Convert.ToDateTime(todt).ToString("dd/MM/yyyy");

            //    }

            //    PType[0] = SqlDbType.VarChar;
            //    PType[1] = SqlDbType.VarChar;
            //    PType[2] = SqlDbType.VarChar;

            //    #endregion

            //    dsExp = da.SelectRecords("GetExpWareHouseInvList", PName, PValue, PType);
            //    //DataSet dsFLAB = da.SelectRecords("GetOffloadAWBNo", PName, paramvalue, paramtype);

            //    if (dsExp != null)
            //    {
            //        if (dsExp.Tables != null)
            //        {
            //            if (dsExp.Tables.Count > 0)
            //            {
            //                if (dsExp.Tables[0].Rows.Count > 0)
            //                {
            //                    grdexpList.PageIndex = 0;
            //                    grdexpList.DataSource = dsExp;
            //                    grdexpList.DataMember = dsExp.Tables[0].TableName;
            //                    grdexpList.DataBind();
            //                    grdexpList.Visible = true;
            //                    //dsFLAB.Clear();
            //                }
            //            }
            //        }
            //    }

            //    grdexpList.PageIndex = e.NewPageIndex;
            //    grdexpList.DataSource = dsExp.Copy();
            //    grdexpList.DataBind();

            //    for (int j = 0; j < grdexpList.Rows.Count; j++)
            //    {
            //        if (((Label)(grdexpList.Rows[j].FindControl("lblOffloadDate"))).Text.ToString() == "1/1/1900 12:00:00 AM")
            //        {
            //            ((Label)(grdexpList.Rows[j].FindControl("lblOffloadDate"))).Text = "-";
            //        }

            //        //else if (((Label)(grvNFLABList.Rows[j].FindControl("lblLoadedStatus"))).Text.ToString() == "False")
            //        //{
            //        //    ((Label)(grvNFLABList.Rows[j].FindControl("lblLoadedStatus"))).Text = "Not Loaded";
            //        //}
            //    }
            //}
            //catch (Exception)
            //{ }
            //finally
            //{
            //    da = null;
            //    if (dsExp != null)
            //        dsExp.Dispose();
            //    PName = null;
            //    PValue = null;
            //    PType = null;
            //}

            DataTable ds = (DataTable)Session["WarhouseExport"];
            grdexpList.PageIndex = e.NewPageIndex;
            grdexpList.DataSource = ds;
            grdexpList.DataBind();

              
        }
        # endregion grvNFLABList_PageIndexChanging
        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            // taking all parameters as user selected in report in one variable - "Param"
            string Param = "Station:" + ddlAirport.Text.ToString() + ", FrmDt:" + txtvalidfrom.Text.ToString() + ", ToDt:" + txtvalidto.Text.ToString();

            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), " WareHouse Inventory Export", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
        }
    }
}
