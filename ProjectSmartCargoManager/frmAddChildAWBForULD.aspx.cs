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
    public partial class frmAddChildAWBForULD : System.Web.UI.Page
    {
        
        #region Variable Declaration
        SQLServer da = new SQLServer(Global.GetConnectionString());
        BLArrival BlArl = new BLArrival();
        int awbPcs = 0;
        float awbWt = 0;
        #endregion Variable Declaration

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //Set ULD, Flight # & date from query string
                    if (Request.QueryString["uld"] != null)
                    {
                        lblParentNumber.Text = Request.QueryString["uld"].ToString();
                    }
                    if (Request.QueryString["flt"] != null)
                    {
                        lblFlightNumber.Text = Request.QueryString["flt"].ToString();
                    }
                    if (Request.QueryString["dt"] != null)
                    {
                        lblFlightDate.Text = Request.QueryString["dt"].ToString();
                    }
                    lblPOU.Text = Session["Station"].ToString();

                    //Fetch child AWBs from database for respective ULD, Flight & Station.
                    LoadChildAWBGrid();

                }
            }
            catch (Exception)
            { }
        }
        #endregion Page_Load

        #region Load Child AWB Grid
        private void LoadChildAWBGrid()
        {
            //DataTable dt = null;
            DataTable dt = new DataTable("Addchild_Ldchld_dt");

            //DataSet ds = null;
            DataSet ds = new DataSet("Addchild_Ldchld_ds");

            try
            {
                //Load AWBs from session of respective ULD if available.
                if (Session["ChildAWB_" + lblParentNumber.Text] != null)
                {
                    dt = (DataTable)Session["ChildAWB_" + lblParentNumber.Text];
                }
                else
                {
                    object[] paramvalue = new object[4];

                    if (lblFlightNumber.Text.Trim() == "")
                    {
                        paramvalue[0] = "";
                    }
                    else
                    {
                        paramvalue[0] = lblFlightNumber.Text.Trim();
                    }
                    if (lblFlightDate.Text.Trim() == "")
                    {
                        paramvalue[1] = "";
                    }
                    else
                    {
                        paramvalue[1] = lblFlightDate.Text;
                        Session["ActlFlightDate"] = lblFlightDate.Text;
                    }
                    paramvalue[2] = Session["Station"].ToString();
                    paramvalue[3] = lblParentNumber.Text;

                    dt = BlArl.GetULDChildAWB(paramvalue);
                    
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    //Bind the DataTable to the Grid
                    GVArrDet.DataSource = null;
                    GVArrDet.DataSource = dt;
                    GVArrDet.DataBind();
                    Session["ChildAWB_" + lblParentNumber.Text] = dt.Copy();

                    //Add values in Discrepancy Grid. 
                    DataSet ddlds = new DataSet("Addchild_GetDis");
                    ddlds = da.SelectRecords("Sp_GetDiscrepancy");

                    for (int i = 0; i < GVArrDet.Rows.Count; i++)
                    {
                        DropDownList ddl = ((DropDownList)GVArrDet.Rows[i].FindControl("ddlDiscrepancy"));
                        if (ddlds != null && ddlds.Tables.Count > 0)
                        {
                            
                            if (ddl != null)
                            {
                                ddl.DataSource = ddlds.Tables[0];
                                ddl.DataTextField = ddlds.Tables[0].Columns[0].ColumnName;
                                ddl.DataValueField = ddlds.Tables[0].Columns[0].ColumnName;                                
                                ddl.DataBind();
                                ddl.SelectedValue = dt.Rows[i]["Discrepancy"].ToString();
                                ddl.Dispose();
                            }
                            ddlds.Dispose();
                        }
                        ((TextBox)GVArrDet.Rows[i].FindControl("Expectedpcs")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("ExpectedWeight")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("Owner")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("MftPcs")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("MftWt")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedWt")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedPcs")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("StdPcs")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("StdWt")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("BkdPcs")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("BkdWt")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("CustomStatusCode")).Enabled = true;
                        ((TextBox)GVArrDet.Rows[i].FindControl("Remark")).Enabled = true;

                        //Disable fields for all rows except newly added rows.
                        if (((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text != "")
                        {
                            ((TextBox)GVArrDet.Rows[i].FindControl("POL")).Enabled = false;
                            ((TextBox)GVArrDet.Rows[i].FindControl("ULDDestn")).Enabled = false;
                            ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Enabled = false;
                            ((TextBox)GVArrDet.Rows[i].FindControl("Origin")).Enabled = false;
                            ((TextBox)GVArrDet.Rows[i].FindControl("Destn")).Enabled = false;
                            ((TextBox)GVArrDet.Rows[i].FindControl("FlightNo")).Enabled = false;
                            ((TextBox)GVArrDet.Rows[i].FindControl("FltDate")).Enabled = false;
                        }

                        if (dt.Rows[i]["SecurityCheck"] != null && dt.Rows[i]["SecurityCheck"].ToString() != "")
                        {
                            ((CheckBox)GVArrDet.Rows[i].FindControl("SecurityCheck")).Checked =
                                Convert.ToBoolean(dt.Rows[i]["SecurityCheck"]);
                        }
                        if (dt.Rows[i]["CustomCheck"] != null && dt.Rows[i]["CustomCheck"].ToString() != "")
                        {
                            ((CheckBox)GVArrDet.Rows[i].FindControl("CustomCheck")).Checked =
                                Convert.ToBoolean(dt.Rows[i]["CustomCheck"]);
                        }
                        awbPcs = awbPcs + int.Parse(dt.Rows[i]["PCS"].ToString());
                        awbWt = awbWt + float.Parse(dt.Rows[i]["GrossWgt"].ToString());
                    }
                    if (ddlds != null)
                    {
                        ddlds.Dispose();
                    }
                }
                else
                {
                    LoadGridArrival();
                }

            }
            catch (Exception)
            {
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
        }
        #endregion Load Child AWB Grid

        #region Load Arrival Grid
        private void LoadGridArrival()
        {
            try
            {

                DataTable myDataTable = new DataTable("Addchild_ldgrd_mydt");
                DataColumn myDataColumn;

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ULDno";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "POL";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "FltNo";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ULDdest";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "AWBno";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Owner";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "StatedPCS";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "StatedWgt";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ExpectedPcs";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ExpectedWeight";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "PCS";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Org";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Dest";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Discrepancy";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Desc";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.Boolean");
                myDataColumn.ColumnName = "SecurityCheck";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.Boolean");
                myDataColumn.ColumnName = "CustomCheck";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "CustomStatusCode";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "RemainingPcs";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "status";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "statusreassign";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ArrivedPieces";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ArrivedWeight";
                myDataTable.Columns.Add(myDataColumn);
                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "GrossWgt";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "FltDate";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "BookedPcs";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "BookedWt";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "SCC";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "DESC";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Location";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ArrivalRemarks";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ManualAWB";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "RcvPcs";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "RcvWt";
                myDataTable.Columns.Add(myDataColumn);

                DataRow dr;
                dr = myDataTable.NewRow();
                dr["ULDno"] = "";
                dr["POL"] = "";//"5";
                dr["FltNo"] = "";// "5";
                dr["ULDdest"] = "";// "9";
                dr["AWBno"] = "";
                dr["Owner"] = "";
                dr["StatedPCS"] = "";
                dr["StatedWgt"] = "";
                dr["ExpectedPcs"] = "";
                dr["ExpectedWeight"] = "";
                dr["PCS"] = "";
                dr["Org"] = "";
                dr["Dest"] = "";
                dr["Discrepancy"] = "0";
                dr["Desc"] = "";
                dr["SecurityCheck"] = false;
                dr["CustomCheck"] = false;
                dr["CustomStatusCode"] = "";
                dr["RemainingPcs"] = "";
                dr["status"] = "";
                dr["statusreassign"] = "";
                dr["ArrivedPieces"] = "";
                dr["ArrivedWeight"] = "";
                dr["GrossWgt"] = "";
                dr["FltDate"] = "";
                dr["BookedPCS"] = "";
                dr["BookedWt"] = "";
                dr["SCC"] = "";
                dr["DESC"] = "";
                dr["Location"] = "";
                dr["ArrivalRemarks"] = "";
                dr["ManualAWB"] = "Y";
                dr["RcvPcs"] = "0";
                dr["RcvWt"] = "0";

                myDataTable.Rows.Add(dr);

                //Bind the DataTable to the Grid
                GVArrDet.DataSource = null;
                GVArrDet.DataSource = myDataTable;
                GVArrDet.DataBind();
                Session["ChildAWB_" + lblParentNumber.Text] = myDataTable.Copy();

                //Add values in Discrepancy Grid.
                if (GVArrDet.Rows.Count >= 1)
                {
                    DataSet ddlds = new DataSet("Addchild_DsArrgrd");
                    ddlds = da.SelectRecords("Sp_GetDiscrepancy");

                    if (ddlds != null && ddlds.Tables.Count > 0)
                    {
                        DropDownList ddl = ((DropDownList)GVArrDet.Rows[0].FindControl("ddlDiscrepancy"));
                        if (ddl != null)
                        {
                            ddl.DataSource = ddlds.Tables[0];
                            ddl.DataTextField = ddlds.Tables[0].Columns[0].ColumnName;
                            ddl.DataValueField = ddlds.Tables[0].Columns[0].ColumnName;
                            ddl.DataBind();
                            ddl.Dispose();
                        }
                        ddlds.Dispose();
                    }
                }

                ((TextBox)GVArrDet.Rows[0].FindControl("Expectedpcs")).Enabled = false;
                ((TextBox)GVArrDet.Rows[0].FindControl("ExpectedWeight")).Enabled = false;
                ((TextBox)GVArrDet.Rows[0].FindControl("Owner")).Enabled = false;
                ((TextBox)GVArrDet.Rows[0].FindControl("MftPcs")).Enabled = false;
                ((TextBox)GVArrDet.Rows[0].FindControl("MftWt")).Enabled = false;
                ((TextBox)GVArrDet.Rows[0].FindControl("txtArrivedWt")).Enabled = false;
                ((TextBox)GVArrDet.Rows[0].FindControl("txtArrivedPcs")).Enabled = false;
                ((TextBox)GVArrDet.Rows[0].FindControl("StdPcs")).Enabled = false;
                ((TextBox)GVArrDet.Rows[0].FindControl("StdWt")).Enabled = false;
                ((TextBox)GVArrDet.Rows[0].FindControl("BkdPcs")).Enabled = false;
                ((TextBox)GVArrDet.Rows[0].FindControl("BkdWt")).Enabled = false;
                ((TextBox)GVArrDet.Rows[0].FindControl("POL")).Enabled = true;
                ((TextBox)GVArrDet.Rows[0].FindControl("ULDDestn")).Enabled = true;
                ((TextBox)GVArrDet.Rows[0].FindControl("AWB")).Enabled = true;
                ((TextBox)GVArrDet.Rows[0].FindControl("Origin")).Enabled = true;
                ((TextBox)GVArrDet.Rows[0].FindControl("Destn")).Enabled = true;
                ((TextBox)GVArrDet.Rows[0].FindControl("CustomStatusCode")).Enabled = true;
                ((TextBox)GVArrDet.Rows[0].FindControl("Remark")).Enabled = true;
                ((TextBox)GVArrDet.Rows[0].FindControl("FlightNo")).Enabled = true;
                ((TextBox)GVArrDet.Rows[0].FindControl("FltDate")).Enabled = true;

                myDataColumn.Dispose();
                myDataTable.Dispose();

                awbPcs = 0;
                awbWt = 0;
            }
            catch (Exception)
            {
            }
        }
        #endregion Load Arrival Grid

        #region GetData for the dynaicaly added row
        public void Getdata(object sender, EventArgs e)
        {
            try
            {
                //int rowno=GVArrDet.Rows.Count;
                TextBox txt = sender as TextBox;
                GridViewRow row = txt.NamingContainer as GridViewRow;
                int awbrowno = row.RowIndex;

                string awbno = ((TextBox)GVArrDet.Rows[awbrowno].FindControl("AWB")).Text.Trim();
                if (awbno.Length > 9)
                {
                    lblStatus.Text = "";

                    DataSet ds = new DataSet("getdt_ds");
                    ds = BlArl.Getdata(awbno);

                    #region Bind Data
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            int PiecesCount = Convert.ToInt32(ds.Tables[0].Rows[0]["PiecesCount"].ToString());
                            double GrossWeight = Convert.ToDouble(ds.Tables[0].Rows[0]["GrossWeight"].ToString());
                            string Origin = ds.Tables[0].Rows[0]["OriginCode"].ToString();
                            string Destination = ds.Tables[0].Rows[0]["DestinationCode"].ToString();
                            string Commodity = ds.Tables[0].Rows[0]["CommodityCode"].ToString();
                            string FltNumber = ds.Tables[0].Rows[0]["FltNumber"].ToString();

                            ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("StdPcs"))).Text = PiecesCount.ToString();
                            ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("MftPcs"))).Text = PiecesCount.ToString();
                            ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("Expectedpcs"))).Text = PiecesCount.ToString();
                            ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("ExpectedWeight"))).Text = GrossWeight.ToString();

                            ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("StdWt"))).Text = GrossWeight.ToString();
                            ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("MftWt"))).Text = GrossWeight.ToString();

                            ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("POL"))).Text = Origin.ToString();
                            ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("Origin"))).Text = Origin.ToString();
                            ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("Destn"))).Text = Destination.ToString();
                            ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("ULDDestn"))).Text = Destination.ToString();
                            ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("lblDiscription"))).Text = Commodity.ToString();
                            ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("Remark"))).Text = FltNumber.ToString();
                            ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("ULD"))).Text = "Bulk";
                            ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("FlightNo"))).Text = "";
                            ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("RcvPcs"))).Text = "0";
                            ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("RcvWt"))).Text = "0";
                            ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("txtArrivedPcs"))).Text = "0";
                            ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("txtArrivedWt"))).Text = "0";
                            ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("FlightNo"))).Text = lblFlightNumber.Text;

                            //Disable read only fields to stop user from editing.
                            ((TextBox)GVArrDet.Rows[awbrowno].FindControl("Expectedpcs")).Enabled = false;
                            ((TextBox)GVArrDet.Rows[awbrowno].FindControl("ExpectedWeight")).Enabled = false;
                            ((TextBox)GVArrDet.Rows[awbrowno].FindControl("ULD")).Enabled = false;
                            ((TextBox)GVArrDet.Rows[awbrowno].FindControl("POL")).Enabled = false;
                            ((TextBox)GVArrDet.Rows[awbrowno].FindControl("ULDDestn")).Enabled = false;
                            ((TextBox)GVArrDet.Rows[awbrowno].FindControl("Owner")).Enabled = false; ;
                            ((TextBox)GVArrDet.Rows[awbrowno].FindControl("MftPcs")).Enabled = false; ;
                            ((TextBox)GVArrDet.Rows[awbrowno].FindControl("MftWt")).Enabled = false;
                            ((TextBox)GVArrDet.Rows[awbrowno].FindControl("txtArrivedWt")).Enabled = false;
                            ((TextBox)GVArrDet.Rows[awbrowno].FindControl("txtArrivedPcs")).Enabled = false;
                            ((TextBox)GVArrDet.Rows[awbrowno].FindControl("Origin")).Enabled = false;
                            ((TextBox)GVArrDet.Rows[awbrowno].FindControl("Destn")).Enabled = false;
                            ((TextBox)GVArrDet.Rows[awbrowno].FindControl("StdPcs")).Enabled = false;
                            ((TextBox)GVArrDet.Rows[awbrowno].FindControl("StdWt")).Enabled = false;
                            ((DropDownList)GVArrDet.Rows[awbrowno].FindControl("ddlDiscrepancy")).Enabled = false;
                            ((CheckBox)GVArrDet.Rows[awbrowno].FindControl("SecurityCheck")).Enabled = false;
                            ((CheckBox)GVArrDet.Rows[awbrowno].FindControl("CustomCheck")).Enabled = false;
                            ((TextBox)GVArrDet.Rows[awbrowno].FindControl("CustomStatusCode")).Enabled = false;
                            ((TextBox)GVArrDet.Rows[awbrowno].FindControl("FlightNo")).Enabled = false;
                            ((TextBox)GVArrDet.Rows[awbrowno].FindControl("lblDiscription")).Enabled = false;

                            ds.Dispose();
                            return;
                        }
                    }
                    //Set Flt # & Flt Date from top of page by default.
                    ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("FlightNo"))).Text = lblFlightNumber.Text;
                    ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("FltDate"))).Text = lblFlightDate.Text;
                    ////Data not found.
                    //lblStatus.Text = "AWB is not manifested..";
                    //lblStatus.ForeColor = Color.Red;
                    #endregion Bind Data

                }
                else
                {
                    lblStatus.Text = "Please enter awb prefix";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
            }
            catch (Exception)
            { }
        }
        #endregion GetData for the dynaicaly added row

        #region Save Grid Data
        public void SaveRouteDetails()
        {
            DataTable dtChildAWBInfo = new DataTable("Addchild_dtSaveRoute1");
            //DataTable dtChildAWBInfo = null;
            try
            {
                dtChildAWBInfo = ((DataTable)Session["ChildAWB_" + lblParentNumber.Text]).Clone();
                awbPcs = 0;
                awbWt = 0;
                for (int i = 0; i < GVArrDet.Rows.Count; i++)
                {
                    DataRow row = dtChildAWBInfo.NewRow();
                    row["ULDno"] = ((TextBox)GVArrDet.Rows[i].FindControl("ULD")).Text;
                    row["POL"] = ((TextBox)GVArrDet.Rows[i].FindControl("POL")).Text;
                    row["FltNo"] = ((TextBox)GVArrDet.Rows[i].FindControl("FlightNo")).Text;
                    row["FltDate"] = ((TextBox)GVArrDet.Rows[i].FindControl("FltDate")).Text;
                    row["ULDdest"] = ((TextBox)GVArrDet.Rows[i].FindControl("ULDDestn")).Text;
                    row["AWBno"] = ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text;
                    
                    if (((TextBox)GVArrDet.Rows[i].FindControl("StdPcs")).Text == "")
                        row["StatedPCS"] = "0";
                    else
                        row["StatedPCS"] = ((TextBox)GVArrDet.Rows[i].FindControl("StdPcs")).Text;

                    if (((TextBox)GVArrDet.Rows[i].FindControl("StdWt")).Text == "")
                        row["StatedWgt"] = "0.00";
                    else
                        row["StatedWgt"] = ((TextBox)GVArrDet.Rows[i].FindControl("StdWt")).Text;

                    if (((TextBox)GVArrDet.Rows[i].FindControl("Expectedpcs")).Text == "")
                        row["ExpectedPcs"] = "0";
                    else
                        row["ExpectedPcs"] = ((TextBox)GVArrDet.Rows[i].FindControl("Expectedpcs")).Text;

                    if (((TextBox)GVArrDet.Rows[i].FindControl("ExpectedWeight")).Text == "")
                        row["ExpectedWeight"] = "0";
                    else
                        row["ExpectedWeight"] = ((TextBox)GVArrDet.Rows[i].FindControl("ExpectedWeight")).Text;

                    if (((TextBox)GVArrDet.Rows[i].FindControl("MftPcs")).Text == "")
                        row["PCS"] = "0";
                    else
                        row["PCS"] = ((TextBox)GVArrDet.Rows[i].FindControl("MftPcs")).Text;

                    row["Org"] = ((TextBox)GVArrDet.Rows[i].FindControl("Origin")).Text;
                    row["Dest"] = ((TextBox)GVArrDet.Rows[i].FindControl("Destn")).Text;
                    row["Discrepancy"] = ((DropDownList)GVArrDet.Rows[i].FindControl("ddlDiscrepancy")).SelectedValue;
                    row["Desc"] = ((TextBox)GVArrDet.Rows[i].FindControl("lblDiscription")).Text;
                    row["SecurityCheck"] = ((CheckBox)GVArrDet.Rows[i].FindControl("SecurityCheck")).Checked.ToString();
                    row["CustomCheck"] = ((CheckBox)GVArrDet.Rows[i].FindControl("CustomCheck")).Checked.ToString();
                    row["CustomStatusCode"] = ((TextBox)GVArrDet.Rows[i].FindControl("CustomStatusCode")).Text;
                    if (((TextBox)GVArrDet.Rows[i].FindControl("Remainingpcs")).Text != "")
                        row["RemainingPcs"] = ((TextBox)GVArrDet.Rows[i].FindControl("Remainingpcs")).Text;
                    else
                        row["RemainingPcs"] = "0";

                    row["statusreassign"] = ((TextBox)GVArrDet.Rows[i].FindControl("statusreassign")).Text;
                    
                    if (((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedPcs")).Text == "")
                        row["ArrivedPieces"] = "0";
                    else
                        row["ArrivedPieces"] = ((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedPcs")).Text;

                    if (((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedWt")).Text == "")
                        row["ArrivedWeight"] = "0";
                    else
                        row["ArrivedWeight"] = ((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedWt")).Text;

                    if (((TextBox)GVArrDet.Rows[i].FindControl("MftWt")).Text == "")
                        row["GrossWgt"] = "0";
                    else
                        row["GrossWgt"] = ((TextBox)GVArrDet.Rows[i].FindControl("MftWt")).Text;

                    if (((TextBox)GVArrDet.Rows[i].FindControl("BkdPcs")).Text == "")
                        row["BookedPCS"] = "0";
                    else
                        row["BookedPCS"] = ((TextBox)GVArrDet.Rows[i].FindControl("BkdPcs")).Text;

                    if (((TextBox)GVArrDet.Rows[i].FindControl("BkdWt")).Text == "")
                        row["BookedWt"] = "0";
                    else
                        row["BookedWt"] = ((TextBox)GVArrDet.Rows[i].FindControl("BkdWt")).Text;

                    row["Location"] = ((TextBox)GVArrDet.Rows[i].FindControl("txtLocation")).Text;
                    row["ArrivalRemarks"] = ((TextBox)GVArrDet.Rows[i].FindControl("Remark")).Text;
                    row["SCC"] = ((TextBox)GVArrDet.Rows[i].FindControl("CommCode")).Text;
                    row["DESC"] = ((TextBox)GVArrDet.Rows[i].FindControl("CommDesc")).Text;
                    row["ManualAWB"] = ((HiddenField)GVArrDet.Rows[i].FindControl("hdnManualAWB")).Value;

                    if (((TextBox)GVArrDet.Rows[i].FindControl("RcvPcs")).Text == "")
                        row["RcvPcs"] = "0";
                    else
                        row["RcvPcs"] = ((TextBox)GVArrDet.Rows[i].FindControl("RcvPcs")).Text;

                    if (((TextBox)GVArrDet.Rows[i].FindControl("RcvWt")).Text == "")
                        row["RcvWt"] = "0";
                    else
                        row["RcvWt"] = ((TextBox)GVArrDet.Rows[i].FindControl("RcvWt")).Text;

                    dtChildAWBInfo.Rows.Add(row);

                    awbPcs = awbPcs + int.Parse(row["PCS"].ToString());
                    awbWt = awbWt + float.Parse(row["GrossWgt"].ToString());
                }
                
                Session["ChildAWB_" + lblParentNumber.Text] = dtChildAWBInfo.Copy();

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error [SaveRouteDetails]: " + ex.Message;
            }
            finally
            {
                if (dtChildAWBInfo != null)
                {
                    dtChildAWBInfo.Dispose();
                }
            }
        }
        #endregion SaveGridData

        #region Add New Row To Grid
        private void AddNewRowToGrid()
        {
            //DataTable dtChildAWBInfo = null;
            DataTable dtChildAWBInfo = new DataTable("Addchild_dtNewrw");
            //DataSet ddlds = null;
            DataSet ddlds = new DataSet("Addchild_ddldsNew");

            try
            {
                dtChildAWBInfo = (DataTable)Session["ChildAWB_" + lblParentNumber.Text];
                DataRow rw = dtChildAWBInfo.NewRow();

                dtChildAWBInfo.Rows.Add(rw);

                GVArrDet.DataSource = dtChildAWBInfo.Copy();
                GVArrDet.DataBind();

                //ddlds = new DataSet();
                ddlds = da.SelectRecords("Sp_GetDiscrepancy");
                DropDownList ddl;
                for (int i = 0; i < dtChildAWBInfo.Rows.Count; i++)
                {
                    DataRow row = dtChildAWBInfo.Rows[i];
                    if (GVArrDet.Rows.Count > 1)
                    {
                        ddl = ((DropDownList)GVArrDet.Rows[i].FindControl("ddlDiscrepancy"));
                        ddl.DataSource = ddlds.Tables[0];
                        ddl.DataTextField = ddlds.Tables[0].Columns[0].ColumnName;
                        ddl.DataValueField = ddlds.Tables[0].Columns[0].ColumnName;
                        ddl.DataBind();
                        if (dtChildAWBInfo.Rows[i]["Discrepancy"].ToString() == "")
                        {
                            ddl.SelectedValue = "0";
                        }
                        else
                        {
                            ddl.SelectedValue = dtChildAWBInfo.Rows[i]["Discrepancy"].ToString();
                        }
                        ddl.Dispose();
                    }
                    ((TextBox)GVArrDet.Rows[i].FindControl("ULD")).Text = row["ULDno"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("POL")).Text = row["POL"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("FlightNo")).Text = row["FltNo"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("FltDate")).Text = row["FltDate"].ToString();

                    if (row["ULDdest"] != null && row["ULDdest"].ToString() != "")
                        ((TextBox)GVArrDet.Rows[i].FindControl("ULDDestn")).Text = row["ULDdest"].ToString();
                    else
                        ((TextBox)GVArrDet.Rows[i].FindControl("ULDDestn")).Text = Session["Station"].ToString();

                    ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text = row["AWBno"].ToString();
                    //((TextBox)GVArrDet.Rows[i].FindControl("ULDDestn")).Text = row["Owner"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("StdPcs")).Text = row["StatedPCS"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("StdWt")).Text = row["StatedWgt"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("Expectedpcs")).Text = row["ExpectedPcs"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("ExpectedWeight")).Text = row["ExpectedWeight"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("MftPcs")).Text = row["PCS"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("Origin")).Text = row["Org"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("Destn")).Text = row["Dest"].ToString();
                    
                    ((TextBox)GVArrDet.Rows[i].FindControl("lblDiscription")).Text = row["Desc"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("CustomStatusCode")).Text = row["CustomStatusCode"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("Remainingpcs")).Text = row["RemainingPcs"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("statusreassign")).Text = row["statusreassign"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("ArrivedPcs")).Text = row["ArrivedPieces"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedWt")).Text = row["ArrivedWeight"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("MftWt")).Text = row["GrossWgt"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("Remark")).Text = row["ArrivalRemarks"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("txtLocation")).Text = row["Location"].ToString();
                    ((HiddenField)GVArrDet.Rows[i].FindControl("hdnManualAWB")).Value = row["ManualAWB"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("RcvPcs")).Text = row["RcvPcs"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("RcvWt")).Text = row["RcvWt"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("Expectedpcs")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("ExpectedWeight")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("ULD")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("Owner")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("MftPcs")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("MftWt")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedWt")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedPcs")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("StdPcs")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("StdWt")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("BkdPcs")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("BkdWt")).Enabled = false;

                    //Disable fields for all rows except newly added rows.
                    if (((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text != "")
                    {
                        ((TextBox)GVArrDet.Rows[i].FindControl("POL")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("ULDDestn")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("Origin")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("Destn")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("CustomStatusCode")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("Remark")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("FlightNo")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("FltDate")).Enabled = false;
                    }
                    else
                    {
                        ((HiddenField)GVArrDet.Rows[i].FindControl("hdnManualAWB")).Value = "Y";
                    }
                    if (row["SecurityCheck"] != null && row["SecurityCheck"].ToString() != "")
                    {
                        ((CheckBox)GVArrDet.Rows[i].FindControl("SecurityCheck")).Checked =
                            Convert.ToBoolean(row["SecurityCheck"]);
                    }
                    if (row["CustomCheck"] != null && row["CustomCheck"].ToString() != "")
                    {
                        ((CheckBox)GVArrDet.Rows[i].FindControl("CustomCheck")).Checked =
                            Convert.ToBoolean(row["CustomCheck"]);
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
            finally
            {
                if (ddlds != null)
                    ddlds.Dispose();

                if (dtChildAWBInfo != null)
                    dtChildAWBInfo.Dispose();
            }
        }
        #endregion AddnewRow To Grid

        #region btnAdd_Click
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                
                SaveRouteDetails();

                AddNewRowToGrid();

            }
            catch (Exception) { }

        }
        #endregion btnAdd_Click

        #region btnRemove_Click
        protected void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                SaveRouteDetails();
                for (int i = 0; i < GVArrDet.Rows.Count; i++)
                {   //Remove selected row from grid.
                    if (((CheckBox)GVArrDet.Rows[i].FindControl("check")).Checked)
                    {
                        RemoveRowFromGrid(i);
                    }
                }

                DataTable dtChildAWBInfo = new DataTable("btnRem_dt");
                dtChildAWBInfo = (DataTable)Session["ChildAWB_" + lblParentNumber.Text];

                DataSet ddlds = new DataSet("btnRem_dsddl");
                ddlds = da.SelectRecords("Sp_GetDiscrepancy");

                for (int i = 0; i < dtChildAWBInfo.Rows.Count; i++)
                {
                    DataRow row = dtChildAWBInfo.Rows[i];
                    DropDownList ddl;
                    if (GVArrDet.Rows.Count > 1 && ddlds != null)
                    {
                        ddl = ((DropDownList)GVArrDet.Rows[i].FindControl("ddlDiscrepancy"));
                        ddl.DataSource = ddlds.Tables[0];
                        ddl.DataTextField = ddlds.Tables[0].Columns[0].ColumnName;
                        ddl.DataValueField = ddlds.Tables[0].Columns[0].ColumnName;
                        ddl.DataBind();
                        if (dtChildAWBInfo.Rows[i]["Discrepancy"].ToString() == "")
                        {
                            ddl.SelectedValue = "0";
                        }
                        else
                        {
                            ddl.SelectedValue = dtChildAWBInfo.Rows[i]["Discrepancy"].ToString();
                        }
                        ddl.Dispose();
                    }
                    ((TextBox)GVArrDet.Rows[i].FindControl("ULD")).Text = row["ULDno"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("POL")).Text = row["POL"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("FlightNo")).Text = row["FltNo"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("FltDate")).Text = row["FltDate"].ToString();

                    if (row["ULDdest"] != null && row["ULDdest"].ToString() != "")
                        ((TextBox)GVArrDet.Rows[i].FindControl("ULDDestn")).Text = row["ULDdest"].ToString();
                    else
                        ((TextBox)GVArrDet.Rows[i].FindControl("ULDDestn")).Text = Session["Station"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text = row["AWBno"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("StdPcs")).Text = row["StatedPCS"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("StdWt")).Text = row["StatedWgt"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("Expectedpcs")).Text = row["ExpectedPcs"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("ExpectedWeight")).Text = row["ExpectedWeight"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("MftPcs")).Text = row["PCS"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("Origin")).Text = row["Org"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("Destn")).Text = row["Dest"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("lblDiscription")).Text = row["Desc"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("CustomStatusCode")).Text = row["CustomStatusCode"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("Remainingpcs")).Text = row["RemainingPcs"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("statusreassign")).Text = row["statusreassign"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("ArrivedPcs")).Text = row["ArrivedPieces"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedWt")).Text = row["ArrivedWeight"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("MftWt")).Text = row["GrossWgt"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("Remark")).Text = row["ArrivalRemarks"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("txtLocation")).Text = row["Location"].ToString();
                    ((HiddenField)GVArrDet.Rows[i].FindControl("hdnManualAWB")).Value = row["ManualAWB"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("RcvPcs")).Text = row["RcvPcs"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("RcvWt")).Text = row["RcvWt"].ToString();

                    if (row["SecurityCheck"] != null && row["SecurityCheck"].ToString() != "")
                    {
                        ((CheckBox)GVArrDet.Rows[i].FindControl("SecurityCheck")).Checked =
                            Convert.ToBoolean(row["SecurityCheck"]);
                    }
                    if (row["CustomCheck"] != null && row["CustomCheck"].ToString() != "")
                    {
                        ((CheckBox)GVArrDet.Rows[i].FindControl("CustomCheck")).Checked =
                            Convert.ToBoolean(row["CustomCheck"]);
                    }
                    ((TextBox)GVArrDet.Rows[i].FindControl("Expectedpcs")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("ExpectedWeight")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("ULD")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("Owner")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("MftPcs")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("MftWt")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedWt")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedPcs")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("StdPcs")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("StdWt")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("BkdPcs")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("BkdWt")).Enabled = false;

                    //Disable fields for all rows except newly added rows.
                    if (((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text != "")
                    {
                        ((TextBox)GVArrDet.Rows[i].FindControl("POL")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("ULDDestn")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("Origin")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("Destn")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("CustomStatusCode")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("Remark")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("FlightNo")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("FltDate")).Enabled = false;
                    }
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion btnAdd_Click

        #region Remove Row From Grid
        private void RemoveRowFromGrid(int rowIndex)
        {
            //DataTable dtChildAWBInfo = null;
            DataTable dtChildAWBInfo = new DataTable("Addchild_dtRmRw");

            //DataSet ddlds = null;
            DataSet ddlds = new DataSet("Addchild_dsddlRmRw");

            try
            {
                dtChildAWBInfo = (DataTable)Session["ChildAWB_" + lblParentNumber.Text];

                if (dtChildAWBInfo.Rows.Count > rowIndex && dtChildAWBInfo.Rows.Count > 1)
                {
                    dtChildAWBInfo.Rows.RemoveAt(rowIndex);

                    GVArrDet.DataSource = dtChildAWBInfo.Copy();
                    GVArrDet.DataBind();

                    Session["ChildAWB_" + lblParentNumber.Text] = dtChildAWBInfo;

                    //ddlds = new DataSet();
                    ddlds = da.SelectRecords("Sp_GetDiscrepancy");
                    DropDownList ddl;
                    for (int i = 0; i < dtChildAWBInfo.Rows.Count; i++)
                    {
                        DataRow row = dtChildAWBInfo.Rows[i];
                        if (GVArrDet.Rows.Count > 1)
                        {
                            ddl = ((DropDownList)GVArrDet.Rows[i].FindControl("ddlDiscrepancy"));
                            ddl.DataSource = ddlds.Tables[0];
                            ddl.DataTextField = ddlds.Tables[0].Columns[0].ColumnName;
                            ddl.DataValueField = ddlds.Tables[0].Columns[0].ColumnName;
                            ddl.DataBind();
                            ddl.Dispose();
                        }
                        ((TextBox)GVArrDet.Rows[i].FindControl("ULD")).Text = row["ULDno"].ToString();
                        ((TextBox)GVArrDet.Rows[i].FindControl("POL")).Text = row["POL"].ToString();
                        ((TextBox)GVArrDet.Rows[i].FindControl("FlightNo")).Text = row["FltNo"].ToString();
                        ((TextBox)GVArrDet.Rows[i].FindControl("FltDate")).Text = row["FltDate"].ToString();

                        if (row["ULDdest"] != null && row["ULDdest"].ToString() != "")
                            ((TextBox)GVArrDet.Rows[i].FindControl("ULDDestn")).Text = row["ULDdest"].ToString();
                        else
                            ((TextBox)GVArrDet.Rows[i].FindControl("ULDDestn")).Text = Session["Station"].ToString();

                        ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text = row["AWBno"].ToString();
                        //((TextBox)GVArrDet.Rows[i].FindControl("ULDDestn")).Text = row["Owner"].ToString();
                        ((TextBox)GVArrDet.Rows[i].FindControl("StdPcs")).Text = row["StatedPCS"].ToString();
                        ((TextBox)GVArrDet.Rows[i].FindControl("StdWt")).Text = row["StatedWgt"].ToString();
                        ((TextBox)GVArrDet.Rows[i].FindControl("Expectedpcs")).Text = row["ExpectedPcs"].ToString();
                        ((TextBox)GVArrDet.Rows[i].FindControl("ExpectedWeight")).Text = row["ExpectedWeight"].ToString();
                        ((TextBox)GVArrDet.Rows[i].FindControl("MftPcs")).Text = row["PCS"].ToString();
                        ((TextBox)GVArrDet.Rows[i].FindControl("Origin")).Text = row["Org"].ToString();
                        ((TextBox)GVArrDet.Rows[i].FindControl("Destn")).Text = row["Dest"].ToString();
                        ((DropDownList)GVArrDet.Rows[i].FindControl("ddlDiscrepancy")).Text = row["Discrepancy"].ToString();
                        ((TextBox)GVArrDet.Rows[i].FindControl("lblDiscription")).Text = row["Desc"].ToString();
                        ((TextBox)GVArrDet.Rows[i].FindControl("CustomStatusCode")).Text = row["CustomStatusCode"].ToString();
                        ((TextBox)GVArrDet.Rows[i].FindControl("Remainingpcs")).Text = row["RemainingPcs"].ToString();
                        ((TextBox)GVArrDet.Rows[i].FindControl("statusreassign")).Text = row["statusreassign"].ToString();
                        ((TextBox)GVArrDet.Rows[i].FindControl("ArrivedPcs")).Text = row["ArrivedPieces"].ToString();
                        ((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedWt")).Text = row["ArrivedWeight"].ToString();
                        ((TextBox)GVArrDet.Rows[i].FindControl("MftWt")).Text = row["GrossWgt"].ToString();
                        ((TextBox)GVArrDet.Rows[i].FindControl("Remark")).Text = row["ArrivalRemarks"].ToString();
                        ((TextBox)GVArrDet.Rows[i].FindControl("txtLocation")).Text = row["Location"].ToString();
                        ((HiddenField)GVArrDet.Rows[i].FindControl("hdnManualAWB")).Value = row["ManualAWB"].ToString();

                        if (row["SecurityCheck"] != null && row["SecurityCheck"].ToString() != "")
                        {
                            ((CheckBox)GVArrDet.Rows[i].FindControl("SecurityCheck")).Checked =
                                Convert.ToBoolean(row["SecurityCheck"]);
                        }
                        if (row["CustomCheck"] != null && row["CustomCheck"].ToString() != "")
                        {
                            ((CheckBox)GVArrDet.Rows[i].FindControl("CustomCheck")).Checked =
                                Convert.ToBoolean(row["CustomCheck"]);
                        }
                        ((TextBox)GVArrDet.Rows[i].FindControl("Expectedpcs")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("ExpectedWeight")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("ULD")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("Owner")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("MftPcs")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("MftWt")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedWt")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedPcs")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("StdPcs")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("StdWt")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("BkdPcs")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("BkdWt")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("POL")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("ULDDestn")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("Origin")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("Destn")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("CustomStatusCode")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("Remark")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("FlightNo")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("FltDate")).Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
            finally
            {
                if (ddlds != null)
                    ddlds.Dispose();

                if (dtChildAWBInfo != null)
                    dtChildAWBInfo.Dispose();
            }
        }
        #endregion Remove Row From Grid

        #region btnSave_Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //Validate data.
                for (int i = 0; i < GVArrDet.Rows.Count; i++)
                {

                    //Validate if AWB Number is not blank.
                    if (((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text == "" ||
                        ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text.Length < 8)
                    {
                        lblStatus.Text = "Please enter valid AWB Number (Prefix-Number)";
                        lblStatus.ForeColor = Color.Red;
                        ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Enabled = true;
                        ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Focus();
                        return;
                    }
                    //Validate if Flight # is entered.
                    if (((TextBox)GVArrDet.Rows[i].FindControl("FlightNo")).Text.Length < 2)
                    {
                        lblStatus.Text = "Please enter valid Flight Number (Prefix+Number)";
                        lblStatus.ForeColor = Color.Red;
                        ((TextBox)GVArrDet.Rows[i].FindControl("FlightNo")).Enabled = true;
                        ((TextBox)GVArrDet.Rows[i].FindControl("FlightNo")).Focus();
                        return;
                    }
                    //Validate if Flight Date is entered.
                    if (((TextBox)GVArrDet.Rows[i].FindControl("FltDate")).Text.Length < 8)
                    {
                        lblStatus.Text = "Please enter valid Flight Date (dd/MM/yyyy)";
                        lblStatus.ForeColor = Color.Red;
                        ((TextBox)GVArrDet.Rows[i].FindControl("FltDate")).Enabled = true;
                        ((TextBox)GVArrDet.Rows[i].FindControl("FltDate")).Focus();
                        return;
                    }
                    //Validate if Origin is entered.
                    if (((TextBox)GVArrDet.Rows[i].FindControl("Origin")).Text.Length < 3)
                    {
                        lblStatus.Text = "Please enter valid Origin Station";
                        lblStatus.ForeColor = Color.Red;
                        ((TextBox)GVArrDet.Rows[i].FindControl("Origin")).Enabled = true;
                        ((TextBox)GVArrDet.Rows[i].FindControl("Origin")).Focus();
                        return;
                    }
                    //Validate if Destination is entered.
                    if (((TextBox)GVArrDet.Rows[i].FindControl("Destn")).Text.Length < 3)
                    {
                        lblStatus.Text = "Please enter valid Destination Station";
                        lblStatus.ForeColor = Color.Red;
                        ((TextBox)GVArrDet.Rows[i].FindControl("Destn")).Enabled = true;
                        ((TextBox)GVArrDet.Rows[i].FindControl("Destn")).Focus();
                        return;
                    }
                    //Validate if POL is entered.
                    if (((TextBox)GVArrDet.Rows[i].FindControl("POL")).Text.Length < 3)
                    {
                        lblStatus.Text = "Please enter valid POL";
                        lblStatus.ForeColor = Color.Red;
                        ((TextBox)GVArrDet.Rows[i].FindControl("POL")).Enabled = true;
                        ((TextBox)GVArrDet.Rows[i].FindControl("POL")).Focus();
                        return;
                    }
                    //Validate if Rec Pcs is entered.
                    if (((TextBox)GVArrDet.Rows[i].FindControl("RcvPcs")).Text.Length == 0)
                    {
                        lblStatus.Text = "Please enter valid Received Pcs";
                        lblStatus.ForeColor = Color.Red;
                        ((TextBox)GVArrDet.Rows[i].FindControl("RcvPcs")).Enabled = true;
                        ((TextBox)GVArrDet.Rows[i].FindControl("RcvPcs")).Focus();
                        return;
                    }
                    //Validate if Rec Wt is entered.
                    if (((TextBox)GVArrDet.Rows[i].FindControl("RcvWt")).Text.Length == 0)
                    {
                        lblStatus.Text = "Please enter valid Received Wt";
                        lblStatus.ForeColor = Color.Red;
                        ((TextBox)GVArrDet.Rows[i].FindControl("RcvWt")).Enabled = true;
                        ((TextBox)GVArrDet.Rows[i].FindControl("RcvWt")).Focus();
                        return;
                    }

                    int chkconvert = Convert.ToInt32(((TextBox)GVArrDet.Rows[i].FindControl("Expectedpcs")).Text);
                    int chkcon = Convert.ToInt32(((TextBox)GVArrDet.Rows[i].FindControl("RcvPcs")).Text);
                    string origin = ((TextBox)GVArrDet.Rows[i].FindControl("POL")).Text;
                    string station = Session["Station"].ToString();
                    if (origin == station)
                    {
                        //btnList_Click(null, null);
                        lblStatus.Text = "Sorry Please select different destination for arrival..";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    decimal dcRemainingWt = Convert.ToDecimal(((TextBox)GVArrDet.Rows[i].FindControl("ExpectedWeight")).Text);
                    decimal dcReceivedWt = Convert.ToDecimal(((TextBox)GVArrDet.Rows[i].FindControl("RcvWt")).Text);

                    if (chkcon > chkconvert)
                    {
                        lblStatus.Text = "Received pieces can not be more than remaining pieces.";
                        ((TextBox)GVArrDet.Rows[i].FindControl("RcvPcs")).Focus();
                        return;
                    }
                    if (dcReceivedWt > dcRemainingWt)
                    {
                        lblStatus.Text = "Received weight can not be more than remaining weight.";
                        ((TextBox)GVArrDet.Rows[i].FindControl("RcvWt")).Focus();
                        return;
                    }
                    if (chkcon == chkconvert && dcReceivedWt != dcRemainingWt)
                    {
                        lblStatus.Text = "Please enter valid Weight.";
                        ((TextBox)GVArrDet.Rows[i].FindControl("RcvWt")).Focus();
                        return;
                    }
                    if (dcReceivedWt == dcRemainingWt && chkcon != chkconvert)
                    {
                        lblStatus.Text = "Please enter valid Pieces.";
                        ((TextBox)GVArrDet.Rows[i].FindControl("RcvPcs")).Focus();
                        return;
                    }
                }
                SaveRouteDetails();

                ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(),
                    "javascript:CloseWindow('" + GVArrDet.Rows.Count.ToString() + "','" + awbPcs.ToString() + "','" + awbWt.ToString() + "');", true);

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
                lblStatus.ForeColor = Color.Red;
                ex = null;
            }
        }
        #endregion btnSave_Click

        #region Set Received Weight
        public void SetReceivedWeight(object sender, EventArgs e)
        {
            try
            {
                TextBox txt = sender as TextBox;
                GridViewRow row = txt.NamingContainer as GridViewRow;
                string strPcs = ((TextBox)row.FindControl("RcvPcs")).Text;
                string strWt = txt.Text;
                //Set Booked Pcs
                if (((TextBox)row.FindControl("BkdPcs")).Text == "" || ((TextBox)row.FindControl("BkdPcs")).Text == "0")
                {
                    ((TextBox)row.FindControl("BkdPcs")).Text = strPcs;
                }
                //Set accepted Pcs
                if (((TextBox)row.FindControl("StdPcs")).Text == "" || ((TextBox)row.FindControl("StdPcs")).Text == "0")
                {
                    ((TextBox)row.FindControl("StdPcs")).Text = strPcs;
                }
                //Set manifested pcs
                if (((TextBox)row.FindControl("MftPcs")).Text == "" || ((TextBox)row.FindControl("MftPcs")).Text == "0")
                {
                    ((TextBox)row.FindControl("MftPcs")).Text = strPcs;
                }
                //Set remaining pcs
                if (((TextBox)row.FindControl("Expectedpcs")).Text == "" || ((TextBox)row.FindControl("Expectedpcs")).Text == "0")
                {
                    ((TextBox)row.FindControl("Expectedpcs")).Text = strPcs;
                }
                //Set Arrived Pcs
                if (((TextBox)row.FindControl("txtArrivedPcs")).Text == "")
                {
                    ((TextBox)row.FindControl("txtArrivedPcs")).Text = "0";
                }
                //Set booked weight
                if (((TextBox)row.FindControl("BkdWt")).Text == "" || ((TextBox)row.FindControl("BkdWt")).Text == "0"
                    || ((TextBox)row.FindControl("BkdWt")).Text == "0.00")
                {
                    ((TextBox)row.FindControl("BkdWt")).Text = strWt;
                }
                //Set accepted weight
                if (((TextBox)row.FindControl("StdWt")).Text == "" || ((TextBox)row.FindControl("StdWt")).Text == "0"
                    || ((TextBox)row.FindControl("StdWt")).Text == "0.00")
                {
                    ((TextBox)row.FindControl("StdWt")).Text = strWt;
                }
                //Set manifested weight
                if (((TextBox)row.FindControl("MftWt")).Text == "" || ((TextBox)row.FindControl("MftWt")).Text == "0"
                    || ((TextBox)row.FindControl("MftWt")).Text == "0.00")
                {
                    ((TextBox)row.FindControl("MftWt")).Text = strWt;
                }
                //Set remaining weight
                if (((TextBox)row.FindControl("ExpectedWeight")).Text == "" || ((TextBox)row.FindControl("ExpectedWeight")).Text == "0"
                    || ((TextBox)row.FindControl("ExpectedWeight")).Text == "0.00")
                {
                    ((TextBox)row.FindControl("ExpectedWeight")).Text = strWt;
                }
                //Set arrived weight
                if (((TextBox)row.FindControl("txtArrivedWt")).Text == "")
                {
                    ((TextBox)row.FindControl("txtArrivedWt")).Text = "0.00";
                }

            }
            catch (Exception)
            {
            }
        }
        #endregion Set Received Weight

        #region btnCancel_Click
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "javascript:Close();", true);
            }
            catch (Exception)
            {
            }
        }
        #endregion btnCancel_Click

    }
}
