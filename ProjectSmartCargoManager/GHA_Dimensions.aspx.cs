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
using System.Collections;


/*

 2012-04-05  vinayak
 2012-07-24  vinayak
 
*/

namespace ProjectSmartCargoManager
{
    public partial class GHA_Dimensions : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());
        BookingBAL objBLL = new BookingBAL();

        DataSet dsDimensions = new DataSet("GHA_Dimensions_1");
        LoginBL lBal = new LoginBL();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //
                if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() != "A")
                {
                    if (Session["AWBStatus"] != null && Convert.ToString(Session["AWBStatus"]) == "E")
                    {
                        btnSave.Enabled = false;
                        btnCancel.Enabled = false;
                    }
                    else
                    {
                        btnSave.Enabled = true;
                        btnCancel.Enabled = true;
                    }

                    if (Request.QueryString["QBA"] != null && Request.QueryString["QBA"].ToString() == "Y")
                    {
                        btnSave.Enabled = true;
                        btnCancel.Enabled = true;
                    }
                }

                Session["FocusShipper"] = "false";

                if (!IsPostBack)
                {
                    //lblawbno.Text = Request.QueryString["AwbNo"].ToString();

                    if (CheckVolumetricExeption())  //Volumetric Exeption check.
                    {
                        hdVolumetriccheck.Value = "1";
                    }
                    else
                    {
                        hdVolumetriccheck.Value = "0";
                    }

                    try
                    {
                        //chkBulk.Checked = Convert.ToBoolean(Session["IsBulk"]);
                    }
                    catch (Exception ex)
                    { }
                    lblCommodity.Text = Request.QueryString["commodity"].ToString();
                    LBLPcsCount.Text = Request.QueryString["PcsCount"].ToString();
                    lblGrossWt.Text = Request.QueryString["GrossWt"].ToString();

                    if (Request.QueryString["FltNo"] != null)
                    {
                        lblFlightNo.Text = Convert.ToString(Request.QueryString["FltNo"]);
                    }
                    else
                    {
                        lblFlightNoDisplay.Visible = false;
                        grdDimension.Columns[12].Visible = false;
                    }

                    if (Request.QueryString["FltDate"] != null)
                        lblFlightDate.Text = Convert.ToString(Request.QueryString["FltDate"]);
                    else
                    {
                        lblFlightDateDisplay.Visible = false;
                        grdDimension.Columns[13].Visible = false;
                    }

                    Session["RowIndex"] = Request.QueryString["RowIndex"].ToString();

                    //DataSet ds = (DataSet)Session["dsDimesionAll"];
                    //int intPieces = Convert.ToInt32(LBLPcsCount.Text.Trim());

                    //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    //{
                    //    for (int intCount = 0; intCount < ds.Tables[0].Rows.Count; intCount++)
                    //    {
                    //        if (intCount + 1 > intPieces)
                    //            ds.Tables[0].Rows.RemoveAt(intCount);
                    //    }
                    //    Session["dsDimesionAll"] = ds;
                    //}

                    if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() != "A")
                    {
                        Session["dsDimesionAll"] = GenerateAWBDimensions("", Convert.ToInt32(LBLPcsCount.Text.Trim()), (DataSet)Session["dsDimesionAll"], Convert.ToDecimal(lblGrossWt.Text.Trim()), false, "");

                    }
                    else
                    {
                        Session["dsDimesionAll"] = null;
                        Session["dsDimesionAllAcceptance"] = null;
                        Session["DsTopDim"] = null;
                        Session["RowIndex"] = 1;
                        string[] AWBDetails = (Request.QueryString["awbno"] != null ? Request.QueryString["awbno"].ToString() : string.Empty).Split('-');
                        DataSet Ds = new DataSet("GHA_Dimensions_2");
                        string[] PName = new string[3];
                        PName[0] = "AWBNo";
                        PName[1] = "FlightNo";
                        PName[2] = "FlightDate";

                        object[] PValue = new object[3];
                        PValue[0] = Request.QueryString["awbno"] != null ? Request.QueryString["awbno"].ToString() : string.Empty;
                        PValue[1] = Request.QueryString["FltNo"] != null ? Request.QueryString["FltNo"].ToString() : string.Empty;
                        PValue[2] = Request.QueryString["FltDate"] != null ? Request.QueryString["FltDate"].ToString() : string.Empty; ;

                        SqlDbType[] PType = new SqlDbType[3];
                        PType[0] = SqlDbType.VarChar;
                        PType[1] = SqlDbType.VarChar;
                        PType[2] = SqlDbType.VarChar;

                        Ds = da.SelectRecords("sp_getAWBDimensionForCargoAccp", PName, PValue, PType);

                        if (Ds != null)
                        {
                            if (Ds.Tables.Count > 0)
                            {
                                if (Ds.Tables[1].Rows.Count > 0)
                                {
                                    Session["dsDimesionAll"] = GenerateAWBDimensionsAcceptance(AWBDetails[1], Convert.ToInt32(LBLPcsCount.Text.Trim()), Ds, Convert.ToDecimal(lblGrossWt.Text.Trim()), false, AWBDetails[0], lblFlightNo.Text.Trim(), lblFlightDate.Text.Trim());
                                }
                            }
                        }
                    }



                    try
                    {
                        LoginBL objBAL = new LoginBL();
                        string unit = "";
                        unit = objBAL.GetMasterConfiguration("MeasurmentUnit");
                        if (unit.Length > 0)
                        {
                            if (unit.Equals("MKS", StringComparison.OrdinalIgnoreCase))
                            {
                                lblUnit.Text = "kg";
                            }
                            if (unit.Equals("FPS", StringComparison.OrdinalIgnoreCase))
                            {
                                lblUnit.Text = "lbs";
                            }
                        }

                        Session["SupportBag"] = "true";
                        try
                        {
                            Session["SupportBag"] = objBAL.GetMasterConfiguration("SupportBagging").ToString();
                        }
                        catch (Exception ez) { }
                        //Session["DsTopDim"] = null;
                        LoadGridDimensionDetails();
                        Session["cnt"] = Convert.ToInt32(((TextBox)grdDim.Rows[0].FindControl("txtPcs")).Text.Trim());

                    }
                    catch (Exception ex) { }

                    //session timeout
                    if (Session["UserName"] == null)
                        Response.Redirect("~/SessionTimeOut.aspx");
                    btnSavetop_Click(null, null);
                }
                LoginBL LBals = new LoginBL();
                bool scroll = Convert.ToBoolean(LBals.GetMasterConfiguration("Breadth"));
                if (scroll == true)
                {
                    grdDim.HeaderRow.Cells[3].Text = "Breadth";
                    grdDimension.HeaderRow.Cells[4].Text = "Breadth";
                }
                else
                {
                    grdDim.HeaderRow.Cells[3].Text = "Width";
                    grdDimension.HeaderRow.Cells[4].Text = "Width";

                }

                LoginBL LBal = new LoginBL();
                bool scrolls = Convert.ToBoolean(LBal.GetMasterConfiguration("Btn_DimSave"));
                if (scrolls == true)
                {
                    Button btn = ((Button)grdDim.FooterRow.FindControl("btnSave"));
                    string asd = btn.Text;
                    btn.Text = "Calculate";

                }
                else
                {
                    Button btn = ((Button)grdDim.FooterRow.FindControl("btnSave"));
                    // string asd = btn.Text;
                    btn.Text = "Save";


                }
                ((TextBox)grdDim.Rows[0].FindControl("txtLength")).Focus();
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowHideShipperDetials1();</script>", false);
                //btnSavetop_Click(null, null);
            }
            catch (Exception ex)
            { }
        }

        //private void OnKeyDownHandler(object sender, KeyEventArgs e)
        //{
        //    if (e.Key ==Key.Return)
        //    {
        //        btnSavetop_Click(null, null);
        //    }
        //}

        public void LoadGridDimensionDetails()
        {

            #region Comment

            //DataTable myDataTable = new DataTable();
            //DataColumn myDataColumn;
            //DataSet Ds = new DataSet();

            //myDataColumn = new DataColumn();
            //myDataColumn.DataType = Type.GetType("System.String");
            //myDataColumn.ColumnName = "Length";
            //myDataTable.Columns.Add(myDataColumn);

            //myDataColumn = new DataColumn();
            //myDataColumn.DataType = Type.GetType("System.String");
            //myDataColumn.ColumnName = "Breadth";
            //myDataTable.Columns.Add(myDataColumn);

            //myDataColumn = new DataColumn();
            //myDataColumn.DataType = Type.GetType("System.String");
            //myDataColumn.ColumnName = "Height";
            //myDataTable.Columns.Add(myDataColumn);

            //myDataColumn = new DataColumn();
            //myDataColumn.DataType = Type.GetType("System.String");
            //myDataColumn.ColumnName = "PcsCount";
            //myDataTable.Columns.Add(myDataColumn);


            //DataRow dr;
            //dr = myDataTable.NewRow();
            //dr["Length"] = "";//"5";
            //dr["Breadth"] = "";// "5";
            //dr["Height"] = "";// "9";
            //dr["PcsCount"] = "";
            //myDataTable.Rows.Add(dr);

            //grdDimension.DataSource = null;
            //grdDimension.DataSource = myDataTable;
            //grdDimension.DataBind();

            //dsDimensions = new DataSet();
            //dsDimensions.Tables.Add(myDataTable.Copy());

            //Session["dsDimensions"] = dsDimensions.Copy();

            #endregion
            DataSet dsDimensionsTemp = new DataSet("GHA_Dimensions_3");
            try
            {

                dsDimensionsTemp = (DataSet)Session["dsDimesionAll"];
                DataView dv = new DataView(dsDimensionsTemp.Tables[0].Copy());
                if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() != "A")
                {
                    dv.RowFilter = "RowIndex = " + Session["RowIndex"];
                }

                dsDimensions = new DataSet("GHA_Dimensions_4");
                dsDimensions.Tables.Add(dv.ToTable().Copy());

                if (dsDimensions.Tables[0].Rows.Count == 0)
                {
                    dsDimensions.Tables[0].Rows.Add(dsDimensions.Tables[0].NewRow());
                    dsDimensions.Tables[0].Rows[0]["RowIndex"] = Session["RowIndex"].ToString();

                    grdDimension.DataSource = dsDimensions.Copy();
                    grdDimension.DataBind();
                    
                    

                    Session["dsDimensions"] = dsDimensions.Copy();
                }
                else
                {
                    grdDimension.DataSource = dv.ToTable().Copy();
                    grdDimension.DataBind();

                    //ddlUnit.SelectedIndex = (dv.ToTable().Rows[0]["Units"].ToString() == "cms" ? 1 : 0);

                    if (dv.ToTable().Rows[0]["Units"].ToString().Trim() == "")
                        ddlUnit.SelectedIndex = 1;
                    else
                        ddlUnit.SelectedValue = dv.ToTable().Rows[0]["Units"].ToString();

                    LBLVolumeUnit.Text = "Cubic " + ddlUnit.Text;
                    //if (ddlUnit.SelectedIndex == 0)
                    //    LBLVolumeUnit.Text = "cubic Inch";
                    //else
                    //    LBLVolumeUnit.Text = "cubic cm";

                    Session["dsDimensions"] = dsDimensions.Copy();

                    for (int i = 0; i < grdDimension.Rows.Count; i++)
                    {
                        ((DropDownList)grdDimension.Rows[i].FindControl("ddlPieceType")).Text = dv.ToTable().Rows[i]["PieceType"].ToString();
                    }

                    CalculateTotal();
                    
                    foreach (DataControlField col in grdDimension.Columns)
                    {
                        if (Session["ULDACT"].ToString().ToUpper() == "FALSE" && col.HeaderText == "ULD#")
                        {
                            col.Visible = false;
                        }
                        if (Session["SupportBag"].ToString().Equals("FALSE", StringComparison.OrdinalIgnoreCase) && col.HeaderText == "Bag#")
                        {
                            col.Visible = false;
                        }
                    }
                }
                try
                {

                    string PcsCount = dsDimensions.Tables[0].Rows[dsDimensions.Tables[0].Rows.Count - 1][0].ToString();
                    string Pidfirst = dsDimensions.Tables[0].Rows[0][1].ToString();
                    string PidLast = dsDimensions.Tables[0].Rows[dsDimensions.Tables[0].Rows.Count - 1][1].ToString();
                    string FinalPid = Pidfirst + "-" + PidLast;
                    Session["PidLast"] = PidLast;

                    if (dsDimensionsTemp != null && dsDimensionsTemp.Tables.Count > 1 && dsDimensionsTemp.Tables[1].Rows.Count > 0)
                    {
                        grdDim.DataSource = dsDimensionsTemp.Tables[1];
                        grdDim.DataBind();
                        Session["DsTopDim"] = dsDimensionsTemp.Tables[1];
                    }
                    else if (Session["DsTopDim"] == null)
                    {
                        DataTable DsTopDim = new DataTable("GHA_Dimensions_15");
                        DsTopDim.Columns.Add("Length", typeof(string));
                        DsTopDim.Columns.Add("Height", typeof(string));
                        DsTopDim.Columns.Add("Breadth", typeof(string));
                        DsTopDim.Columns.Add("PcsCount", typeof(string));
                        DsTopDim.Columns.Add("Pids", typeof(string));
                        DsTopDim.Columns.Add("Pidsto", typeof(string));
                        DsTopDim.Columns.Add("IsBulk", typeof(string));

                        DsTopDim.Rows.Add("0", "0", "0", PcsCount, Pidfirst, PidLast);

                        grdDim.DataSource = DsTopDim;
                        grdDim.DataBind();
                        Session["DsTopDim"] = DsTopDim;
                    }
                    else
                    {
                        DataTable DsTopDim = new DataTable("GHA_Dimensions_16");
                        DsTopDim = (DataTable)Session["DsTopDim"];
                        grdDim.DataSource = DsTopDim;
                        grdDim.DataBind();                        
                    }
                    
                    //try
                    //{
                    //    int lastcnt = 0;
                    //    int Cnt = 0;// Convert.ToInt32(((TextBox)grdDim.Rows[0].FindControl("txtPcs")).Text.Trim());
                    //    string lastpid = "";
                    //    for (int j = 0; j < grdDim.Rows.Count; j++)
                    //    {
                    //        Cnt = Convert.ToInt32(((TextBox)grdDim.Rows[j].FindControl("txtPcs")).Text.Trim());
                    //        //for (int i = lastcnt; i < (lastcnt + Cnt); i++)
                    //        //{
                    //        //    //((TextBox)grdDimension.Rows[i].FindControl("txtLength")).Text = ((TextBox)grdDim.Rows[j].FindControl("txtLength")).Text.Trim();
                    //        //    //((TextBox)grdDimension.Rows[i].FindControl("txtBreadth")).Text = ((TextBox)grdDim.Rows[j].FindControl("txtBreadth")).Text.Trim();
                    //        //    //((TextBox)grdDimension.Rows[i].FindControl("txtHeight")).Text = ((TextBox)grdDim.Rows[j].FindControl("txtHeight")).Text.Trim();
                    //        //    //CalculateVolume(grdDimension.Rows[i].FindControl("txtHeight"), null);

                    //        //}
                    //        ((Label)grdDim.Rows[j].FindControl("lblpId")).Text = ((Label)grdDimension.Rows[lastcnt].FindControl("txtPieceID")).Text;
                    //        ((Label)grdDim.Rows[j].FindControl("lblpIdto")).Text = ((Label)grdDimension.Rows[lastcnt + Cnt - 1].FindControl("txtPieceID")).Text;
                    //        lastcnt = lastcnt + Cnt;
                    //    }


                    //}
                    //catch (Exception ex)
                    //{ }


                }
                catch (Exception ex)
                { }

 

            }
            catch (Exception ex)
            {
                dsDimensions = null;
                LBLStatus.Text = "" + ex.Message;
                return;
            }
            finally 
            {
                if (dsDimensions != null)
                    dsDimensions.Dispose();
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //Response.Redirect("FrmConBooking.aspx", false);
            //Session["dsDimesionAll"] = null;
            ScriptManager.RegisterStartupScript(btnCancel, btnCancel.GetType(), "HidUnhide", "window.close();", true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataSet dsDimensionsTemp = new DataSet("GHA_Dimensions_5");
            DataSet dsDimensionsAll = new DataSet("GHA_Dimensions_6");
            //Session["IsBulk"] = chkBulk.Checked;
            BLAssignULD objBuildULD = new BLAssignULD();
            try
            {
                bool ULDPresent = false;
                decimal wt = 0, grosswt = 0;
                
                if (Request.QueryString["GrossWt"].ToString().Trim() == "")
                {
                    grosswt = 0;
                }
                else
                {
                    try
                    {
                        grosswt = decimal.Parse(Request.QueryString["GrossWt"].ToString().Trim());
                    }
                    catch (Exception ex)
                    {
                        grosswt = 0;
                    }

                }

                if (TXTTotal.Text.Trim() != "")
                {
                    try
                    {
                        wt = decimal.Parse(TXTTotal.Text);

                    }
                    catch (Exception ex)
                    {
                        wt = 0;
                    }

                    if (grosswt > wt)
                    {
                        wt = grosswt;
                    }
                    else
                    {
                        //
                    }

                }

                dsDimensionsTemp = (DataSet)Session["dsDimesionAll"];
                DataView dv = new DataView(dsDimensionsTemp.Tables[0].Copy());
                dv.RowFilter = "RowIndex <> " + Session["RowIndex"];


                dsDimensionsAll = new DataSet("GHA_Dimensions_7");
                dsDimensionsAll.Tables.Add(dv.ToTable().Copy());


                dsDimensions = ((DataSet)Session["dsDimensions"]).Copy();
                 
                foreach (DataRow rw in dsDimensions.Tables[0].Rows)
                    rw["Units"] = ddlUnit.SelectedItem.Text.Trim();
                ArrayList arr = new ArrayList();
                if (dsDimensions != null)
                {
                    for (int intCount = 0; intCount < grdDimension.Rows.Count; intCount++)
                    {
                        if (((Label)grdDimension.Rows[intCount].FindControl("lblHide")).Text != "1")
                        {
                            dsDimensions.Tables[0].Rows[intCount]["Length"] = ((TextBox)grdDimension.Rows[intCount].FindControl("txtLength")).Text.Trim();
                            dsDimensions.Tables[0].Rows[intCount]["Breath"] = ((TextBox)grdDimension.Rows[intCount].FindControl("txtBreadth")).Text.Trim();
                            dsDimensions.Tables[0].Rows[intCount]["Height"] = ((TextBox)grdDimension.Rows[intCount].FindControl("txtHeight")).Text.Trim();
                            dsDimensions.Tables[0].Rows[intCount]["Wt"] = ((TextBox)grdDimension.Rows[intCount].FindControl("txtWeight")).Text.Trim();
                            dsDimensions.Tables[0].Rows[intCount]["Vol"] = ((Label)grdDimension.Rows[intCount].FindControl("lblVolume")).Text.Trim();
                            dsDimensions.Tables[0].Rows[intCount]["PieceType"] = ((DropDownList)grdDimension.Rows[intCount].FindControl("ddlPieceType")).Text.Trim();
                            dsDimensions.Tables[0].Rows[intCount]["BagNo"] = ((TextBox)grdDimension.Rows[intCount].FindControl("txtBagNo")).Text.Trim();
                            dsDimensions.Tables[0].Rows[intCount]["ULDNo"] = ((TextBox)grdDimension.Rows[intCount].FindControl("txtULDNo")).Text.Trim();
                            dsDimensions.Tables[0].Rows[intCount]["Location"] = ((TextBox)grdDimension.Rows[intCount].FindControl("txtLocation")).Text.Trim();
                            if (((Label)grdDimension.Rows[intCount].FindControl("lblBulk")).Text.Trim() == "1")
                                dsDimensions.Tables[0].Rows[intCount]["IsBulk"] = true;
                            else
                                dsDimensions.Tables[0].Rows[intCount]["IsBulk"] = false;

                            if (dsDimensions.Tables[0].Rows[intCount]["PieceType"].ToString() == "ULD")
                            {
                                ULDPresent = true;
                                if (dsDimensions.Tables[0].Rows[intCount]["ULDNo"].ToString() == "")
                                {
                                    LBLStatus.Text = "Please enter ULD Number.";
                                    LBLStatus.ForeColor = Color.Red;
                                    ((TextBox)grdDimension.Rows[intCount].FindControl("txtULDNo")).Focus();
                                    return;
                                }
                                else
                                {
                                    #region Validate ULD against ULD Master and Usage

                                    string NewULDNo = dsDimensions.Tables[0].Rows[intCount]["ULDNo"].ToString().Trim();
                                    if (NewULDNo != string.Empty)
                                    {
                                        string Result = objBuildULD.ValidateULDAdvanced(NewULDNo);
                                        if (Result != "Success")
                                        {
                                            LBLStatus.Text = Result;
                                            LBLStatus.ForeColor = Color.Red;
                                            return;
                                        }
                                    }

                                    #endregion

                                    arr.Add(dsDimensions.Tables[0].Rows[intCount]["ULDNo"].ToString());
                                    Session["PieceTypeULDNo_ArrayList"] = arr;
                                }
                            }

                            if (lblFlightNo.Text.Trim() != "" && lblFlightDate.Text.Trim() != null)
                            {
                                if (((CheckBox)grdDimension.Rows[intCount].FindControl("CHKSelect")).Checked)
                                {
                                    dsDimensions.Tables[0].Rows[intCount]["FlightNo"] = lblFlightNo.Text.Trim();
                                    dsDimensions.Tables[0].Rows[intCount]["FlightDate"] = lblFlightDate.Text.Trim();
                                }
                            }
                            else
                            {
                                dsDimensions.Tables[0].Rows[intCount]["FlightNo"] = ((TextBox)grdDimension.Rows[intCount].FindControl("txtFlightNo")).Text.Trim();
                                dsDimensions.Tables[0].Rows[intCount]["FlightDate"] = ((TextBox)grdDimension.Rows[intCount].FindControl("txtFlightDate")).Text.Trim();
                            }
                            
                        }

                        if (ULDPresent)
                        {
                            if (CheckDuplicateULD(dsDimensions))
                            {
                                LBLStatus.Text = "Duplicate ULD Number Present in details.Please enter valid ULD Number.";
                                LBLStatus.ForeColor = Color.Red;
                                return;
                            }
                        }
                    }

                    //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    //{
                    //    for (int intCount = 0; intCount < ds.Tables[0].Rows.Count; intCount++)
                    //    {
                    //        if (intCount + 1 > intPieces)
                    //            ds.Tables[0].Rows.RemoveAt(intCount);
                    //    }
                    //    dsDimensions = ds.Copy();
                    //    ds = null;
                    //}

                    DataSet ds = new DataSet("GHA_Dimensions_8");
                    ds = dsDimensions.Copy();
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {                        
                        int intPieceCnt = Convert.ToInt32(LBLPcsCount.Text.Trim());
                        int intGridCnt = ds.Tables[0].Rows.Count;

                        while (intGridCnt > intPieceCnt)
                        {
                            ds.Tables[0].Rows.RemoveAt(intGridCnt-1);
                            intGridCnt = intGridCnt - 1;
                        }
                        
                        dsDimensions = ds.Copy();
                        ds = null;
                    }
                }

                dsDimensionsTemp = dsDimensionsAll.Copy();
                dsDimensionsTemp.Merge(dsDimensions);

                if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() != "A")
                {
                    Session["dsDimesionAll"] = dsDimensionsTemp.Copy();
                }
                else
                {
                    Session["dsDimesionAllAcceptance"] = dsDimensionsTemp.Copy();

                    //Adding AWB to Dimensions DataTable
                    System.Data.DataColumn newAWBColumn = new DataColumn("AWBNumber", typeof(System.String));
                    newAWBColumn.DefaultValue = Request.QueryString["awbno"] != null ? Request.QueryString["awbno"].ToString() : string.Empty;
                    ((DataSet)Session["dsDimesionAllAcceptance"]).Tables[0].Columns.Add(newAWBColumn);

                    //Adding SHC to Dimensions DataTable
                    System.Data.DataColumn newSHCColumn = new DataColumn("SHC", typeof(System.String));
                    newSHCColumn.DefaultValue = Request.QueryString["SHC"] != null ? Request.QueryString["SHC"].ToString() : string.Empty;
                    ((DataSet)Session["dsDimesionAllAcceptance"]).Tables[0].Columns.Add(newSHCColumn);

                    //Adding CommodityCode to Dimensions DataTable
                    System.Data.DataColumn newCommodityColumn = new DataColumn("CommodityCode", typeof(System.String));
                    newCommodityColumn.DefaultValue = Request.QueryString["commodity"] != null ? Request.QueryString["commodity"].ToString() : string.Empty;
                    ((DataSet)Session["dsDimesionAllAcceptance"]).Tables[0].Columns.Add(newCommodityColumn);

                    //Adding DocNo to Dimensions DataTable
                    System.Data.DataColumn newDockColumn = new DataColumn("DockNo", typeof(System.String));
                    newDockColumn.DefaultValue = Session["DockNoFromDockAccp"] != null ? Session["DockNoFromDockAccp"].ToString() : string.Empty;
                    ((DataSet)Session["dsDimesionAllAcceptance"]).Tables[0].Columns.Add(newDockColumn);

                    ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "javascript:CloseWindowAcceptance('" + ((DataSet)Session["dsDimesionAllAcceptance"]).Tables[0].Rows.Count + "','" + wt + "');", true);

                }
                #region Bulk
                int PidFrom, PidTo;
                    
                try
                {
                    
                    for (int z = 0; z < grdDim.Rows.Count; z++)
                    {
                        if (((CheckBox)grdDim.Rows[z].FindControl("CkBulk")).Checked)
                        {
                            //Bulk
                            //dsDimensions.Tables[0].Rows[intCount]["IsBulk"] = ((CheckBox)grdDim.Rows[intCount].FindControl("CkBulk")).Checked;
                            PidFrom = Convert.ToInt32(((Label)grdDim.Rows[z].FindControl("lblpId")).Text);
                            PidTo = Convert.ToInt32(((Label)grdDim.Rows[z].FindControl("lblpIdto")).Text);

                            Session["Dim_PidFrom"] = PidFrom;
                            Session["Dim_PidTo"] = PidTo;
                            
                        }

                    }
                }
                catch (Exception ex)
                { }
                
                #endregion  
                Session["FocusShipper"] = "true";

                try
                {
                    string Piecetype = ((DropDownList)grdDimension.Rows[0].FindControl("ddlPieceType")).Text;
                    string Piecetypeatselect = "";
                    Session["Piecetype_DIM"] = null;
                    for (int j = 0; j < grdDimension.Rows.Count; j++)
                    {
                        Piecetypeatselect = string.Empty.ToString();
                        Piecetypeatselect = ((DropDownList)grdDimension.Rows[j].FindControl("ddlPieceType")).Text;
                        if (Piecetype == Piecetypeatselect)
                        {
                            Session["Piecetype_DIM"] = Piecetype;
                        }
                        else
                        {
                            Session["Piecetype_DIM"] = "Mixed";
                            ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "javascript:CloseWindow('" + wt + "','" + wt + "','" + txtMeterVolume.Text.Trim() + "');", true);
                            return;
                        }
                    }                    
                }
                catch (Exception ex)
                { }
                ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "javascript:CloseWindow('" + wt + "','" + wt + "','" + txtMeterVolume.Text.Trim() + "');", true);

            }
            catch (Exception ex)
            {
                dsDimensionsTemp = null;
                dsDimensionsAll = null;
                ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "Error()", true);
            }
            finally 
            {
                if (dsDimensionsTemp != null)
                    dsDimensionsTemp.Dispose();
                if (dsDimensionsAll != null)
                    dsDimensionsAll.Dispose();
                Session["VolumeMetric"] = null;
                objBuildULD = null;
            }
        }

        protected void grdDimension_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
                
        protected void btnAddRow_Click(object sender, EventArgs e)
        {
            try
            {
                SaveGrid();

                dsDimensions = (DataSet)Session["dsDimensions"];

                DataRow row = dsDimensions.Tables[0].NewRow();
                row["RowIndex"] = Session["RowIndex"].ToString();
                dsDimensions.Tables[0].Rows.Add(row);

                grdDimension.DataSource = dsDimensions.Tables[0];
                grdDimension.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "Error: " + ex.Message + "", true);
            }
        }
        
        public bool CheckTotalPcsCount()
        {
            try
            {
                
                int count = 0;

                for (int i = 0; i < grdDimension.Rows.Count; i++)
                {
                   count+= int.Parse(((TextBox)grdDimension.Rows[i].FindControl("txtPcs")).Text);
                }

                if (grdDimension.Rows.Count > 0 && count != int.Parse(LBLPcsCount.Text))
                {
                    LBLStatus.Text = "Total Pcs count should be " + LBLPcsCount.Text;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                //LBLStatus.Text = ""+ex.Message;
                return false;
            }

        }

        public void SaveGrid()
        {
            try
            {
                dsDimensions = (DataSet)Session["dsDimensions"];

                //if (dsDimensions == null && Session["dsDimesionAll"] != null)
                //{
                //    dsDimensions = ((DataSet)Session["dsDimesionAll"]).Copy();
                //}
              

                for (int i = 0; i < grdDimension.Rows.Count; i++)
                {

                    dsDimensions.Tables[0].Rows[i]["Length"] = ((TextBox)grdDimension.Rows[i].FindControl("txtLength")).Text;
                    dsDimensions.Tables[0].Rows[i]["Breadth"] = ((TextBox)grdDimension.Rows[i].FindControl("txtBreadth")).Text;
                    dsDimensions.Tables[0].Rows[i]["Height"] = ((TextBox)grdDimension.Rows[i].FindControl("txtHeight")).Text;
                    dsDimensions.Tables[0].Rows[i]["PcsCount"] = ((TextBox)grdDimension.Rows[i].FindControl("txtPcs")).Text;
                    dsDimensions.Tables[0].Rows[i]["RowIndex"] = Session["RowIndex"].ToString();
                }

              
                Session["dsDimensions"] = dsDimensions.Copy();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "Error: " + ex.Message + "", true);
            }
        }

        public bool IsInputValid()
        {
            try
            {
                float length = 0, breadth = 0, height = 0; 
                    int pcscount = 0, totalpcscount=0;

                for (int i = 0; i < grdDimension.Rows.Count; i++)
                {
                    try
                    {
                        length = float.Parse(((TextBox)grdDimension.Rows[i].FindControl("txtLength")).Text);

                    }
                    catch
                    {

                        //LBLStatus.Text = "Enter valid length row:" + i;
                        //LBLStatus.ForeColor = Color.Red;
                        //((TextBox)grdDimension.Rows[i].FindControl("txtLength")).Text = "";
                        ((TextBox)grdDimension.Rows[i].FindControl("txtLength")).Focus();
                        return false;
                    }

                    try
                    {
                        breadth = float.Parse(((TextBox)grdDimension.Rows[i].FindControl("txtBreadth")).Text);

                    }
                    catch
                    {
                        //LBLStatus.Text = "Enter valid breadth row:" + i;
                        //LBLStatus.ForeColor = Color.Red;
                        ((TextBox)grdDimension.Rows[i].FindControl("txtBreadth")).Text = "";
                        ((TextBox)grdDimension.Rows[i].FindControl("txtBreadth")).Focus();
                        return false;
                    }
                    try
                    {
                        height = float.Parse(((TextBox)grdDimension.Rows[i].FindControl("txtHeight")).Text);

                    }
                    catch
                    {
                        //LBLStatus.Text = "Enter valid height row:" + i;
                        //LBLStatus.ForeColor = Color.Red;
                        ((TextBox)grdDimension.Rows[i].FindControl("txtHeight")).Text = "";
                        ((TextBox)grdDimension.Rows[i].FindControl("txtHeight")).Focus();
                        return false;
                    }
                    try
                    {
                        if (((TextBox)grdDimension.Rows[i].FindControl("txtPcs")).Text.Trim() == "")
                        {
                            //LBLStatus.Text = "Fill pcscount for row:" + i;
                            //LBLStatus.ForeColor = Color.Red;
                            return false;
                        }
                        pcscount = int.Parse(((TextBox)grdDimension.Rows[i].FindControl("txtPcs")).Text);

                    }
                    catch
                    {
                        //LBLStatus.Text = "Enter valid pcscount row:" + i;
                        //LBLStatus.ForeColor = Color.Red;
                        ((TextBox)grdDimension.Rows[i].FindControl("txtPcs")).Focus();
                        return false;
                    }

                    totalpcscount += pcscount;

                    if (totalpcscount > int.Parse(LBLPcsCount.Text))
                    {
                        LBLStatus.Text = "Total pcs count should be smaller than " + LBLPcsCount.Text;
                        LBLStatus.ForeColor = Color.Red;
                        return false;
                    }

                }

                LBLStatus.Text = "";
                return true;
            }
            catch (Exception ex)
            {
                //LBLStatus.Text = "Error : while checking input validation.";
                //LBLStatus.ForeColor = Color.Red;
                return false;
            }
        }

        //protected void txtPcs_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        if (!IsInputValid())
        //        {
        //            TXTVolume.Text = "";
        //            TXTTotal.Text = "";
        //            return;
        //        }


        //        SaveGrid();

        //        dsDimensions = (DataSet)Session["dsDimensions"];

        //        decimal FinalTotal = 0;
        //        decimal FinalVolume = 0;

        //        for (int i = 0; i < dsDimensions.Tables[0].Rows.Count; i++)
        //        {
        //            if (dsDimensions.Tables[0].Rows[i]["Length"].ToString().Trim() != "" && dsDimensions.Tables[0].Rows[i]["Breadth"].ToString().Trim() != "" && dsDimensions.Tables[0].Rows[i]["Height"].ToString().Trim() != "")
        //            {
                        
        //                decimal total=0,volume=0;

        //                total = Convert.ToDecimal(dsDimensions.Tables[0].Rows[i]["Length"].ToString().Trim()) *
        //                        Convert.ToDecimal(dsDimensions.Tables[0].Rows[i]["Breadth"].ToString().Trim()) *
        //                        Convert.ToDecimal(dsDimensions.Tables[0].Rows[i]["Height"].ToString().Trim())*
        //                        Convert.ToDecimal(dsDimensions.Tables[0].Rows[i]["PcsCount"].ToString().Trim());

        //                volume = total;

        //                if (ddlUnit.Text.Trim() == "cms")
        //                {
        //                    total = total / 6000;
                            
        //                }
        //                else
        //                {
        //                    total = total / 366;
        //                }


        //                FinalTotal += total;
        //                FinalVolume += volume;
        //            }
                    
        //        }


        //        if (("" + FinalTotal).Contains('.'))
        //            TXTTotal.Text = ("" + FinalTotal).Substring(0, ("" + FinalTotal).IndexOf('.') +2);
        //        else
        //            TXTTotal.Text = "" + FinalTotal;

        //        TXTTotal.Text = Convert.ToDecimal(TXTTotal.Text).ToString("0.0");

        //        if (TXTTotal.Text.ToString().IndexOf(".") > 0 || Convert.ToInt32(TXTTotal.Text.Substring(0, TXTTotal.Text.ToString().IndexOf(".")))>0.0)
        //        {
        //            decimal dcRound = Convert.ToDecimal(TXTTotal.Text) % Convert.ToInt32(TXTTotal.Text.Substring(0, TXTTotal.Text.ToString().IndexOf(".")));

        //            if (dcRound <= 0.5m)
        //            {
        //                decimal dc = 0.5m - dcRound;
        //                TXTTotal.Text = (Convert.ToDecimal(TXTTotal.Text) + dc).ToString();
        //            }
        //            else
        //            {
        //                decimal dc = 1 - dcRound;
        //                TXTTotal.Text = (Convert.ToDecimal(TXTTotal.Text) + dc).ToString();
        //            }
        //        }

                
        //        //TXTTotal.Text = Convert.ToDecimal(TXTTotal.Text).ToString("0");


        //        if (("" + FinalVolume).Contains('.'))
        //            TXTVolume.Text = ("" + FinalVolume).Substring(0, ("" + FinalVolume).IndexOf('.') + 2);
        //        else
        //            TXTVolume.Text = "" + FinalVolume;

        //        TXTVolume.Text = "" + FinalVolume;

        //    }
        //    catch (Exception ex)
        //    {
        //        //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "Error: " + ex.Message + "", true);
        //    }
        //}

        protected void ddlUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlUnit.SelectedIndex == 0)
            //    LBLVolumeUnit.Text = "Cubic Inch";
            //else if (ddlUnit.SelectedIndex == 1)
            //    LBLVolumeUnit.Text = "Cubic CentiMeter";
            //else
            //    LBLVolumeUnit.Text = "Cubic Meter";

            LBLVolumeUnit.Text = "Cubic " + ddlUnit.Text.Trim();

            for (int rowindex = 0; rowindex < grdDimension.Rows.Count; rowindex++)
            {
                float Length = 0, Breadth = 0, Height = 0,Volume = 0,Weight = 0;
                

                if (((TextBox)grdDimension.Rows[rowindex].FindControl("txtLength")).Text.Trim() != "")
                    Length = float.Parse(((TextBox)grdDimension.Rows[rowindex].FindControl("txtLength")).Text.Trim());

                if (((TextBox)grdDimension.Rows[rowindex].FindControl("txtBreadth")).Text.Trim() != "")
                    Breadth = float.Parse(((TextBox)grdDimension.Rows[rowindex].FindControl("txtBreadth")).Text.Trim());

                if (((TextBox)grdDimension.Rows[rowindex].FindControl("txtHeight")).Text.Trim() != "")
                    Height = float.Parse(((TextBox)grdDimension.Rows[rowindex].FindControl("txtHeight")).Text.Trim());

                Volume = Length * Breadth * Height;

                if (ddlUnit.SelectedIndex == 1) //CMS
                {
                    Weight = Volume / 6000;

                }
                else if (ddlUnit.SelectedIndex == 0) //Inches
                {
                    Weight = Volume / 366.666666f;
                }
                else //Meters
                    Weight = Volume / 0.006f;

                //conversion for KG to lbs
                if (lblUnit.Text.Equals("lbs", StringComparison.OrdinalIgnoreCase))
                {
                    Weight = Weight * 2.20f;
                }
                ((Label)grdDimension.Rows[rowindex].FindControl("lblVolume")).Text = Volume.ToString("0.00");
                ((TextBox)grdDimension.Rows[rowindex].FindControl("txtWeight")).Text = Weight.ToString("0.00");
            }
            
            CalculateTotal();
        }

        protected void btnDeleteRow_Click(object sender, EventArgs e)
        {
            try
            {

                SaveGrid();

                dsDimensions = (DataSet)Session["dsDimensions"];
                DataSet newdsDimensions = new DataSet("GHA_Dimensions_9");
                    newdsDimensions = dsDimensions.Clone();

                for (int i = 0; i < grdDimension.Rows.Count; i++)
                {
                    if (!((CheckBox)grdDimension.Rows[i].FindControl("CHKSelect")).Checked)
                    {
                        DataRow row=newdsDimensions.Tables[0].NewRow();
                        row["Length"] = "" + dsDimensions.Tables[0].Rows[i]["Length"].ToString();
                        row["Breadth"] = "" + dsDimensions.Tables[0].Rows[i]["Breadth"].ToString();
                        row["Height"] = "" + dsDimensions.Tables[0].Rows[i]["Height"].ToString();
                        row["PcsCount"] = "" + dsDimensions.Tables[0].Rows[i]["PcsCount"].ToString();
                        row["RowIndex"] = Session["RowIndex"].ToString();

                        newdsDimensions.Tables[0].Rows.Add(row);
                    }
                }

                //if (newdsDimensions.Tables[0].Rows.Count == 0)
                //{
                //    DataRow row = newdsDimensions.Tables[0].NewRow();
                //    newdsDimensions.Tables[0].Rows.Add(row);
                //}

                grdDimension.DataSource = newdsDimensions.Copy();
                grdDimension.DataBind();

                Session["dsDimensions"] = newdsDimensions.Copy();

                //txtPcs_TextChanged(sender, e);

            }catch(Exception ex)
            {
                //LBLStatus.Text = "" + ex.Message;
                return;
            }
        }
        protected void CalculateVolumeWt(object sender, EventArgs e)
        {
            try
            {
                int rowindex = 0;
                TextBox TextBox = (TextBox)sender;
                GridViewRow grRow = (GridViewRow)TextBox.NamingContainer;
                rowindex = grRow.RowIndex;

                decimal Length = 0, Breadth = 0, Height = 0,Volume = 0, Weight = 0;
                int pcs = 0;
                //decimal 

                if (((TextBox)grdDim.Rows[rowindex].FindControl("txtLength")).Text.Trim() != "")
                    Length = decimal.Parse(((TextBox)grdDim.Rows[rowindex].FindControl("txtLength")).Text.Trim());

                if (((TextBox)grdDim.Rows[rowindex].FindControl("txtBreadth")).Text.Trim() != "")
                    Breadth = decimal.Parse(((TextBox)grdDim.Rows[rowindex].FindControl("txtBreadth")).Text.Trim());

                if (((TextBox)grdDim.Rows[rowindex].FindControl("txtHeight")).Text.Trim() != "")
                    Height = decimal.Parse(((TextBox)grdDim.Rows[rowindex].FindControl("txtHeight")).Text.Trim());

                if (((CheckBox)grdDim.Rows[rowindex].FindControl("CkBulk")).Checked)
                    pcs = 1;
                else if (((TextBox)grdDim.Rows[rowindex].FindControl("txtPcs")).Text.Trim() != "")
                    pcs = Convert.ToInt32(((TextBox)grdDim.Rows[rowindex].FindControl("txtPcs")).Text.Trim());
                
                Volume = Length * Breadth * Height*pcs;

                if (ddlUnit.Text.Trim().ToUpper() == "CMS")
                {
                    decimal VolumeMetric = 6000;

                    if (Session["VolumeMetric"] == null)
                    {
                        LoginBL objBL = new LoginBL();

                        string strVolumeMetric = objBL.GetMasterConfiguration("DomesticVolume").ToString();
                        if (strVolumeMetric != "")
                            VolumeMetric = GetVolumeMetric();
                        else
                            VolumeMetric = 6000;

                        Session["VolumeMetric"] = VolumeMetric;
                        objBL = null;
                    }
                    else
                    {
                        VolumeMetric = Convert.ToDecimal(Session["VolumeMetric"]);
                    }
                    
                    
                    
                        Weight = (Volume / VolumeMetric);
                }
                else if (ddlUnit.Text.Trim().ToUpper() == "INCHES")
                {
                    Weight = Volume / decimal.Parse((366.666666).ToString());
                }
                else if (ddlUnit.Text.Trim().ToUpper() == "METERS")
                {
                    Weight = Volume / decimal.Parse((0.006).ToString());
                }
                //conversion for KG to lbs
                if (lblUnit.Text.Equals("lbs", StringComparison.OrdinalIgnoreCase))
                {
                    Weight = Weight * decimal.Parse((2.20).ToString());
                }

                ((TextBox)grdDim.Rows[rowindex].FindControl("txtVol")).Text = (Math.Truncate(decimal.Parse(Volume.ToString()) * 100) / 100).ToString();
                ((TextBox)grdDim.Rows[rowindex].FindControl("txtVolWT")).Text = (Math.Truncate(decimal.Parse(Weight.ToString()) * 100) / 100).ToString();// Weight.ToString();

                CalculateTotalWt();

                if (((TextBox)grdDim.Rows[rowindex].FindControl("txtLength")) == ((TextBox)sender))
                    ((TextBox)grdDim.Rows[rowindex].FindControl("txtBreadth")).Focus();
                else if (((TextBox)grdDim.Rows[rowindex].FindControl("txtBreadth")) == ((TextBox)sender))
                    ((TextBox)grdDim.Rows[rowindex].FindControl("txtHeight")).Focus();
                else if (((TextBox)grdDim.Rows[rowindex].FindControl("txtHeight")) == ((TextBox)sender))
                    ((TextBox)grdDim.Rows[rowindex].FindControl("txtVolWT")).Focus();

            }
            catch (Exception ex)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "Error: " + ex.Message + "", true);
            }
        }
        protected void CalculateVolume(object sender, EventArgs e)
        {
            try
            {
                int rowindex = 0;
                TextBox TextBox = (TextBox)sender;
                GridViewRow grRow = (GridViewRow)TextBox.NamingContainer;
                rowindex = grRow.RowIndex;

                float Length = 0, Breadth = 0, Height = 0,Volume = 0, Weight = 0;
                //decimal 

                if (((TextBox)grdDimension.Rows[rowindex].FindControl("txtLength")).Text.Trim() != "")
                    Length = float.Parse(((TextBox)grdDimension.Rows[rowindex].FindControl("txtLength")).Text.Trim());

                if (((TextBox)grdDimension.Rows[rowindex].FindControl("txtBreadth")).Text.Trim() != "")
                    Breadth = float.Parse(((TextBox)grdDimension.Rows[rowindex].FindControl("txtBreadth")).Text.Trim());

                if (((TextBox)grdDimension.Rows[rowindex].FindControl("txtHeight")).Text.Trim() != "")
                    Height = float.Parse(((TextBox)grdDimension.Rows[rowindex].FindControl("txtHeight")).Text.Trim());

                Volume = Length * Breadth * Height;

                if (ddlUnit.Text.Trim().ToUpper() == "CMS")
                {
                    decimal VolumeMetric = 6000;

                    if (Session["VolumeMetric"] == null)
                    {
                        LoginBL objBL = new LoginBL();

                        string strVolumeMetric = objBL.GetMasterConfiguration("DomesticVolume").ToString();
                        if (strVolumeMetric != "")
                            VolumeMetric = GetVolumeMetric();
                        else
                            VolumeMetric = 6000;

                        Session["VolumeMetric"] = VolumeMetric;
                        objBL = null;
                    }
                    else
                    {
                        VolumeMetric = Convert.ToDecimal(Session["VolumeMetric"]);
                    }

                    Weight = Volume / float.Parse(VolumeMetric.ToString());                    
                }
                else if (ddlUnit.Text.Trim().ToUpper() == "INCHES")
                {
                    Weight = Volume / 366.666666f;
                }
                else if (ddlUnit.Text.Trim().ToUpper() == "METERS")
                {
                    Weight = Volume / 0.006f;
                }
                //conversion for KG to lbs
                if (lblUnit.Text.Equals("lbs", StringComparison.OrdinalIgnoreCase))
                {
                    Weight = Weight * 2.20f;
                }
                ((Label)grdDimension.Rows[rowindex].FindControl("lblVolume")).Text = Volume.ToString("0.00");
                ((TextBox)grdDimension.Rows[rowindex].FindControl("txtWeight")).Text = Weight.ToString("0.00");

                CalculateTotal();
                
                if (((TextBox)grdDimension.Rows[rowindex].FindControl("txtLength")) == ((TextBox)sender))
                    ((TextBox)grdDimension.Rows[rowindex].FindControl("txtBreadth")).Focus();
                else if (((TextBox)grdDimension.Rows[rowindex].FindControl("txtBreadth")) == ((TextBox)sender))
                    ((TextBox)grdDimension.Rows[rowindex].FindControl("txtHeight")).Focus();
                else if (((TextBox)grdDimension.Rows[rowindex].FindControl("txtHeight")) == ((TextBox)sender))
                    ((TextBox)grdDimension.Rows[rowindex].FindControl("txtWeight")).Focus();
                //Added to retain the child grid expansion after Volume Calculation
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowDimensionsGrid();</SCRIPT>", false);
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "Error: " + ex.Message + "", true);
            }
        }

        private void CalculateTotal()
        {
            decimal FinalTotal = 0, FinalVolume = 0, Volume = 0, Weight = 0;

            for (int i = 0; i < grdDimension.Rows.Count; i++)
            {
                Volume = 0; Weight = 0;

                if (((Label)grdDimension.Rows[i].FindControl("lblVolume")).Text.Trim() != "")
                    Volume = Convert.ToDecimal(((Label)grdDimension.Rows[i].FindControl("lblVolume")).Text.Trim());

                if (((TextBox)grdDimension.Rows[i].FindControl("txtWeight")).Text.Trim() != "")
                    Weight = Convert.ToDecimal(((TextBox)grdDimension.Rows[i].FindControl("txtWeight")).Text.Trim());

                FinalVolume = FinalVolume + Volume;
                FinalTotal = FinalTotal + Weight;
            }

            if (hdVolumetriccheck.Value == "1")  //Volumetric Exeption check.
            {
                TXTTotal.Text = Request.QueryString["GrossWt"].ToString();
            }
            else
            {
                TXTTotal.Text = (Math.Truncate(decimal.Parse((FinalTotal).ToString()) * 100) / 100).ToString();
            }
            TXTVolume.Text = (Math.Truncate(decimal.Parse((FinalVolume).ToString()) * 100) / 100).ToString();

            decimal TotalVolume = 0;

            if (TXTVolume.Text.Trim() != "")
                TotalVolume = Convert.ToDecimal(TXTVolume.Text.Trim());

            if (TotalVolume != 0)
            {
                if (ddlUnit.Text.Trim().ToUpper() == "CMS")
                {
                    txtMeterVolume.Text = (Math.Truncate(decimal.Parse((TotalVolume / 1000000).ToString()) * 100) / 100).ToString();
                }
                else if (ddlUnit.Text.Trim().ToUpper() == "INCHES")
                {
                    txtMeterVolume.Text = (Math.Truncate(decimal.Parse((TotalVolume / 61024).ToString()) * 100) / 100).ToString();
                }
                else if (ddlUnit.Text.Trim().ToUpper() == "METERS")
                {
                    txtMeterVolume.Text = TXTVolume.Text;
                }
            }
            else
                txtMeterVolume.Text = "0";
        }

        private DataSet GenerateAWBDimensions(string AWBNumber, int AWBPieces, DataSet Dimensions, decimal AWBWt, bool IsCreate,string AWBPrefix)
        {
            DataSet ds = new DataSet("GHA_Dimensions_10");
            BookingBAL BAL = new BookingBAL();

            ds = BAL.GenerateAWBDimensions(AWBNumber, AWBPieces, Dimensions, AWBWt, Convert.ToString(Session["UserName"]),
                Convert.ToDateTime(Session["IT"]), IsCreate, AWBPrefix, false);
            try
            {
                //chkBulk.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsBulk"].ToString());
                //Session["IsBulk"] = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsBulk"].ToString());
            }
            catch (Exception ex)
            { }
            BAL = null;
            Dimensions = null;
            return ds;
        }

        protected void CopyDimensions(object sender, EventArgs e)
        {
            try
            {
                int rowindex = 0;
                Button button = (Button)sender;
                GridViewRow grRow = (GridViewRow)button.NamingContainer;
                rowindex = grRow.RowIndex;

                float Length = 0, Breadth = 0, Height = 0; 
                    int RowCount=0;
                decimal Volume = 0, Weight = 0;
                string PieceType = string.Empty, BagNo = string.Empty, ULDNo = string.Empty, Location = string.Empty, FlightNo = string.Empty, FlightDt = string.Empty;

                if (((TextBox)grdDimension.Rows[rowindex].FindControl("txtLength")).Text.Trim() != "")
                    Length = float.Parse(((TextBox)grdDimension.Rows[rowindex].FindControl("txtLength")).Text.Trim());

                if (((TextBox)grdDimension.Rows[rowindex].FindControl("txtBreadth")).Text.Trim() != "")
                    Breadth = float.Parse(((TextBox)grdDimension.Rows[rowindex].FindControl("txtBreadth")).Text.Trim());

                if (((TextBox)grdDimension.Rows[rowindex].FindControl("txtHeight")).Text.Trim() != "")
                    Height = float.Parse(((TextBox)grdDimension.Rows[rowindex].FindControl("txtHeight")).Text.Trim());

                if (((Label)grdDimension.Rows[rowindex].FindControl("lblVolume")).Text.Trim() != "")
                    Volume = Convert.ToDecimal(((Label)grdDimension.Rows[rowindex].FindControl("lblVolume")).Text.Trim());

                if(((TextBox)grdDimension.Rows[rowindex].FindControl("txtWeight")).Text.Trim() !="")
                    Weight = Convert.ToDecimal(((TextBox)grdDimension.Rows[rowindex].FindControl("txtWeight")).Text.Trim());

                PieceType = ((DropDownList)grdDimension.Rows[rowindex].FindControl("ddlPieceType")).Text.Trim();
                BagNo = ((TextBox)grdDimension.Rows[rowindex].FindControl("txtBagNo")).Text.Trim();
                ULDNo = ((TextBox)grdDimension.Rows[rowindex].FindControl("txtULDNo")).Text.Trim();
                Location = ((TextBox)grdDimension.Rows[rowindex].FindControl("txtLocation")).Text.Trim();
                FlightNo = ((TextBox)grdDimension.Rows[rowindex].FindControl("txtFlightNo")).Text.Trim();
                FlightDt = ((TextBox)grdDimension.Rows[rowindex].FindControl("txtFlightDate")).Text.Trim();

                RowCount = Convert.ToInt32(((TextBox)grdDimension.Rows[rowindex].FindControl("txtLines")).Text.Trim());

                for (int intRow = rowindex + 1; intRow <= RowCount + rowindex; intRow++)
                {
                    if (intRow < grdDimension.Rows.Count)
                    {
                        ((TextBox)grdDimension.Rows[intRow].FindControl("txtLength")).Text = Length.ToString();
                        ((TextBox)grdDimension.Rows[intRow].FindControl("txtBreadth")).Text = Breadth.ToString();
                        ((TextBox)grdDimension.Rows[intRow].FindControl("txtHeight")).Text = Height.ToString();
                        ((Label)grdDimension.Rows[intRow].FindControl("lblVolume")).Text = Volume.ToString();
                        ((TextBox)grdDimension.Rows[intRow].FindControl("txtWeight")).Text = Weight.ToString();

                        ((DropDownList)grdDimension.Rows[intRow].FindControl("ddlPieceType")).Text = PieceType;
                        ((TextBox)grdDimension.Rows[intRow].FindControl("txtBagNo")).Text = BagNo;
                        ((TextBox)grdDimension.Rows[intRow].FindControl("txtULDNo")).Text = ULDNo;
                        ((TextBox)grdDimension.Rows[intRow].FindControl("txtLocation")).Text = Location;
                        ((TextBox)grdDimension.Rows[intRow].FindControl("txtFlightNo")).Text = FlightNo;
                        ((TextBox)grdDimension.Rows[intRow].FindControl("txtFlightDate")).Text = FlightDt;
                    }
                }

                CalculateTotal();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowDimensionsGrid();</SCRIPT>", false);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowHideShipperDetials();</SCRIPT>", false);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowDimensionsGrid();</SCRIPT>", false);
                //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "Error: " + ex.Message + "", true);
            }
        }

        private bool CheckDuplicateULD(DataSet ds) 
        {
            bool flag = false;
            try 
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++) 
                {
                    for (int j = (i + 1); j < ds.Tables[0].Rows.Count; j++)
                    {

                        if (ds.Tables[0].Rows[i]["ULDNo"].ToString().Length > 0 && ds.Tables[0].Rows[j]["ULDNo"].ToString().Length > 0)
                        {
                            if (ds.Tables[0].Rows[i]["ULDNo"].ToString().Equals(ds.Tables[0].Rows[j]["ULDNo"].ToString(), StringComparison.OrdinalIgnoreCase))
                            {
                                flag = true;
                                i = ds.Tables[0].Rows.Count;
                                j = ds.Tables[0].Rows.Count;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                flag = true;
            }
            return flag;
        }

        protected void grdDimension_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                
                if (Session["SupportBag"].ToString().Equals("FALSE", StringComparison.OrdinalIgnoreCase))
                {  
                    ((DropDownList)e.Row.Cells[8].FindControl("ddlPieceType")).Items.Remove("Bags");

                }
                if (Session["ULDACT"].ToString().ToUpper() == "FALSE")
                {
                   
                    ((DropDownList)e.Row.Cells[8].FindControl("ddlPieceType")).Items.Remove("ULD");

                }
              
             
                
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnAddRowtop_Click(object sender, EventArgs e)
        {
            try
            {
                SaveTopGrid();

                DataTable dstop = new DataTable("GHA_Dimensions_17");
                dstop = (DataTable)Session["DsTopDim"];

                DataRow row = dstop.NewRow();
                row["Length"] = "0";
                row["Height"] = "0";
                row["Breadth"] = "0";
                row["PcsCount"] = "0";
                row["IsBulk"] = false;
                dstop.Rows.Add(row);

                grdDim.DataSource = dstop;
                grdDim.DataBind();
                btnSavetop_Click(null, null);

            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowHideShipperDetials1();</script>", false);
                ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "Error: " + ex.Message + "", true);
            }

            LoginBL LBal = new LoginBL();
            bool scrolls = Convert.ToBoolean(LBal.GetMasterConfiguration("Btn_DimSave"));
            if (scrolls == true)
            {
                Button btn = ((Button)grdDim.FooterRow.FindControl("btnSave"));
                string asd = btn.Text;
                btn.Text = "Calculate";

            }
            else
            {
                Button btn = ((Button)grdDim.FooterRow.FindControl("btnSave"));
                string asd = btn.Text;
                btn.Text = "Save";


            }

        }
        protected void btnDeleteRowtop_Click(object sender, EventArgs e)
        {
            try
            {

                SaveTopGrid();

                DataTable dstop = new DataTable("GHA_Dimensions_18");
                   dstop = (DataTable)Session["DsTopDim"];
                   DataTable newdsDimensions = new DataTable("GHA_Dimensions_19");
                   newdsDimensions = dstop.Clone();

                for (int i = 0; i < grdDim.Rows.Count; i++)
                {
                    if (!((CheckBox)grdDim.Rows[i].FindControl("CHKSelect")).Checked)
                    {
                        DataRow row = newdsDimensions.NewRow();
                        row["Length"] = "" + dstop.Rows[i]["Length"].ToString();
                        row["Breadth"] = "" + dstop.Rows[i]["Breadth"].ToString();
                        row["Height"] = "" + dstop.Rows[i]["Height"].ToString();
                        row["PcsCount"] = "" + dstop.Rows[i]["PcsCount"].ToString();
                        row["Pids"] = "" + dstop.Rows[i]["Pids"].ToString();
                        row["Pidsto"] = "" + dstop.Rows[i]["Pidsto"].ToString();
                        row["IsBulk"] = "" + dstop.Rows[i]["IsBulk"].ToString();
                        //row["RowIndex"] = Session["RowIndex"].ToString();

                        newdsDimensions.Rows.Add(row);
                    }
                }

                grdDim.DataSource = newdsDimensions.Copy();
                grdDim.DataBind();

                Session["DsTopDim"] = newdsDimensions.Copy();
                btnSavetop_Click(null, null);
                if (grdDim.Rows.Count == 0)
                {
                    TXTVolume.Text = "0";
                    txtMeterVolume.Text = "0";
                    TXTTotal.Text = "0";
                }

            }
            catch (Exception ex)
            {
                LBLStatus.Text = "" + ex.Message;
                return;
            }

            LoginBL LBal = new LoginBL();
            bool scrolls = Convert.ToBoolean(LBal.GetMasterConfiguration("Btn_DimSave"));
            if (scrolls == true)
            {
                Button btn = ((Button)grdDim.FooterRow.FindControl("btnSave"));
                string asd = btn.Text;
                btn.Text = "Calculate";

            }
            else
            {
                Button btn = ((Button)grdDim.FooterRow.FindControl("btnSave"));
                string asd = btn.Text;
                btn.Text = "Save";


            }
        }

        protected void btnSavetop_Click(object sender, EventArgs e)
        {
            try
            {
                int lastcnt =0;
                int Cnt = 0;// Convert.ToInt32(((TextBox)grdDim.Rows[0].FindControl("txtPcs")).Text.Trim());
                string lastpid = "";
                decimal bulklength = 0, bulkwidth = 0, bulkheight = 0,bulkvol =0,bulkwt =0;
                for(int j=0;j<grdDim.Rows.Count;j++)
                {
                    Cnt = Convert.ToInt32(((TextBox)grdDim.Rows[j].FindControl("txtPcs")).Text.Trim());
                    //if (chkBulk.Checked == true)
                    //{
                    
                    CalculateVolumeWt(grdDim.Rows[j].FindControl("txtHeight"), null);
                        for (int i = lastcnt; i < (lastcnt + Cnt); i++)
                        {
                            ((Label)grdDimension.Rows[i].FindControl("lblHide")).Text = "0";
                            if (((CheckBox)grdDim.Rows[j].FindControl("CkBulk")).Checked)
                            {
                                decimal PcCount = Convert.ToDecimal(((TextBox)grdDim.Rows[j].FindControl("txtPcs")).Text.Trim());
                                bulklength = (Convert.ToDecimal(((TextBox)grdDim.Rows[j].FindControl("txtLength")).Text.Trim())) / PcCount;
                                bulkwidth = (Convert.ToDecimal(((TextBox)grdDim.Rows[j].FindControl("txtBreadth")).Text.Trim())) / PcCount;
                                bulkheight = (Convert.ToDecimal(((TextBox)grdDim.Rows[j].FindControl("txtHeight")).Text.Trim())) / PcCount;
                                bulkvol = (Convert.ToDecimal(((TextBox)grdDim.Rows[j].FindControl("txtVol")).Text.Trim())) / PcCount;
                                bulkwt = (Convert.ToDecimal(((TextBox)grdDim.Rows[j].FindControl("txtVolWT")).Text.Trim())) / PcCount;

                                ((TextBox)grdDimension.Rows[i].FindControl("txtLength")).Text = bulklength.ToString();//((TextBox)grdDim.Rows[j].FindControl("txtLength")).Text.Trim();
                                ((TextBox)grdDimension.Rows[i].FindControl("txtBreadth")).Text = bulkwidth.ToString(); //((TextBox)grdDim.Rows[j].FindControl("txtBreadth")).Text.Trim();
                                ((TextBox)grdDimension.Rows[i].FindControl("txtHeight")).Text = bulkheight.ToString();
                                ((Label)grdDimension.Rows[i].FindControl("lblVolume")).Text = bulkvol.ToString();
                                ((TextBox)grdDimension.Rows[i].FindControl("txtWeight")).Text = bulkwt.ToString();//((TextBox)grdDim.Rows[j].FindControl("txtHeight")).Text.Trim();
                                ((Label)grdDimension.Rows[i].FindControl("lblBulk")).Text = "1";
                            }
                            //bulklength = (Convert.ToInt32(((TextBox)grdDim.Rows[j].FindControl("txtLength")).Text.Trim())) / (Convert.ToInt32(((TextBox)grdDim.Rows[j].FindControl("txtPcs")).Text.Trim()));
                            //bulkwidth = (Convert.ToInt32(((TextBox)grdDim.Rows[j].FindControl("txtBreadth")).Text.Trim())) / (Convert.ToInt32(((TextBox)grdDim.Rows[j].FindControl("txtPcs")).Text.Trim()));
                            //bulkheight = (Convert.ToInt32(((TextBox)grdDim.Rows[j].FindControl("txtHeight")).Text.Trim())) / (Convert.ToInt32(((TextBox)grdDim.Rows[j].FindControl("txtPcs")).Text.Trim()));
                            else
                            {
                                decimal PcCount = Convert.ToDecimal(((TextBox)grdDim.Rows[j].FindControl("txtPcs")).Text.Trim());
                                ((TextBox)grdDimension.Rows[i].FindControl("txtLength")).Text = ((TextBox)grdDim.Rows[j].FindControl("txtLength")).Text.Trim();
                                ((TextBox)grdDimension.Rows[i].FindControl("txtBreadth")).Text = ((TextBox)grdDim.Rows[j].FindControl("txtBreadth")).Text.Trim();
                                ((TextBox)grdDimension.Rows[i].FindControl("txtHeight")).Text = ((TextBox)grdDim.Rows[j].FindControl("txtHeight")).Text.Trim();
                                ((Label)grdDimension.Rows[i].FindControl("lblVolume")).Text = (Convert.ToDecimal(((TextBox)grdDim.Rows[j].FindControl("txtVol")).Text.Trim()) / PcCount).ToString("0.00");
                                ((TextBox)grdDimension.Rows[i].FindControl("txtWeight")).Text = (Convert.ToDecimal(((TextBox)grdDim.Rows[j].FindControl("txtVolWt")).Text.Trim()) / PcCount).ToString("0.00");
                                ((Label)grdDimension.Rows[i].FindControl("lblBulk")).Text = "0";
                            }
                        }
                    //}
                    //else
                    //{
                    //    for (int i = lastcnt; i < (lastcnt + Cnt); i++)
                    //    {
                    //        ((TextBox)grdDimension.Rows[i].FindControl("txtLength")).Text = ((TextBox)grdDim.Rows[j].FindControl("txtLength")).Text.Trim();
                    //        ((TextBox)grdDimension.Rows[i].FindControl("txtBreadth")).Text = ((TextBox)grdDim.Rows[j].FindControl("txtBreadth")).Text.Trim();
                    //        ((TextBox)grdDimension.Rows[i].FindControl("txtHeight")).Text = ((TextBox)grdDim.Rows[j].FindControl("txtHeight")).Text.Trim();
                    //        CalculateVolume(grdDimension.Rows[i].FindControl("txtHeight"), null);

                    //    }
                    //}
                    ((Label)grdDim.Rows[j].FindControl("lblpId")).Text = ((Label)grdDimension.Rows[lastcnt].FindControl("txtPieceID")).Text;
                    ((Label)grdDim.Rows[j].FindControl("lblpIdto")).Text = ((Label)grdDimension.Rows[lastcnt + Cnt - 1].FindControl("txtPieceID")).Text;
                    lastcnt = lastcnt+Cnt;
                }
                
                int intPieces = Convert.ToInt32(LBLPcsCount.Text.Trim());
                
                if (e != null && grdDimension.Rows.Count > 0)
                {
                    for (int intCount = 0; intCount < grdDimension.Rows.Count; intCount++)
                    {
                        if (intCount + 1 > intPieces)
                        {
                            ((Label)grdDimension.Rows[intCount].FindControl("lblHide")).Text = "1";
                            grdDimension.Rows[intCount].Visible = false;
                        }
                    }
                }                
            }
            catch (Exception ex)
            { }
        }

        protected void txtPcstop_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int cnt = (int)Session["cnt"];
                int tempcnt = 0;
                int tempcnt1 = 0;
               
                for (int j = 0; j < grdDim.Rows.Count; j++)
                {
                    tempcnt = Convert.ToInt32(((TextBox)grdDim.Rows[j].FindControl("txtPcs")).Text.Trim());
                    tempcnt1=tempcnt1+tempcnt;
                }
                if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() != "A")
                {
                    if (!Convert.ToBoolean(Convert.ToInt32(lBal.GetPartnerAcceptMoreOrLess(Session["QB_PartnerCode"].ToString()))))
                    {
                        if (tempcnt != cnt)
                        {
                            LBLStatus.Text = "Please check piece count is not greater than or less than " + cnt;
                            LBLStatus.ForeColor = System.Drawing.Color.Red;
                            btnSave.Enabled = false;
                            return;
                        }
                    }
                    else
                    {
                        LBLStatus.Text = "";
                        btnSavetop_Click(null, null);
                    }
                }


            }

            catch (Exception ex)
            {
                
            }
        }
        public void SaveTopGrid()
        {
            try
            {
                DataTable Dssavetop = new DataTable("GHA_Dimensions_20");
                  Dssavetop =  (DataTable)Session["DsTopDim"];

               for (int i = 0; i < grdDim.Rows.Count; i++)
                {

                    Dssavetop.Rows[i]["Length"] = ((TextBox)grdDim.Rows[i].FindControl("txtLength")).Text;
                    Dssavetop.Rows[i]["Breadth"] = ((TextBox)grdDim.Rows[i].FindControl("txtBreadth")).Text;
                    Dssavetop.Rows[i]["Height"] = ((TextBox)grdDim.Rows[i].FindControl("txtHeight")).Text;
                    Dssavetop.Rows[i]["PcsCount"] = ((TextBox)grdDim.Rows[i].FindControl("txtPcs")).Text;
                    Dssavetop.Rows[i]["IsBulk"] = ((CheckBox)grdDim.Rows[i].FindControl("CkBulk")).Checked;
                    //Dssavetop.Rows[i]["RowIndex"] = Session["RowIndex"].ToString();
                }


               Session["DsTopDim"] = Dssavetop.Copy();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "Error: " + ex.Message + "", true);
            }
        }

        private DataSet GenerateAWBDimensionsAcceptance(string AWBNumber, int AWBPieces, DataSet Dimensions, decimal AWBWt, bool IsCreate, string AWBPrefix, string FlightNo, string FlightDate)
        {
            DataSet ds = new DataSet("GHA_Dimensions_11");
            BookingBAL BAL = new BookingBAL();

            ds = BAL.GenerateAWBDimensionsAcceptance(AWBNumber, AWBPieces, Dimensions, AWBWt, Convert.ToString(Session["UserName"]),
                Convert.ToDateTime(Session["IT"]), IsCreate, AWBPrefix, "1", FlightNo, FlightDate);

            BAL = null;
            Dimensions = null;
            return ds;
        }

        private decimal GetVolumeMetric()
        {
            string Origin = string.Empty;
            string Destination = string.Empty;
            BookingBAL objBal = new BookingBAL();
            DataSet objDS = new DataSet("GHA_Dimensions_12");
            decimal Volume = 6000;

            if (Request.QueryString["Origin"] != null && Request.QueryString["Origin"].ToString() != "")
            {
                Origin = Request.QueryString["Origin"].ToString();
            }
            else
            {
                return Volume;
            }

            if (Request.QueryString["Destination"] != null && Request.QueryString["Destination"].ToString() != "")
            {
                Destination = Request.QueryString["Destination"].ToString();
            }

            objDS = objBal.GetShipmentType(Origin, Destination);

            if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
            {
                Volume = Convert.ToDecimal(objDS.Tables[0].Rows[0]["Volume"]);
            }

            return Volume;
        }

        private bool CheckVolumetricExeption()
        {
            string AgentCode = string.Empty;
            string CommodityCode = lblCommodity.Text.Trim();
            DateTime dtExecutiondt = DateTime.Now;
            BookingBAL objBAL = new BookingBAL();
            bool blnResult = false;
            string ProductType = string.Empty, Shipper=string.Empty, Consignee=string.Empty;
            try
            {
                if (Request.QueryString["commodity"] != null && Request.QueryString["commodity"].ToString() != "")
                {
                    CommodityCode = Request.QueryString["commodity"].ToString();
                }

                if (Request.QueryString["AgtCode"] != null && Request.QueryString["AgtCode"].ToString() != "")
                {
                    AgentCode = Request.QueryString["AgtCode"].ToString();
                }

                if (Request.QueryString["ExecDt"] != null && Request.QueryString["ExecDt"].ToString() != "")
                {
                    dtExecutiondt = DateTime.ParseExact(Request.QueryString["ExecDt"].ToString(), Convert.ToString(Session["DateFormat"]), null).Date;
                }
                if (Request.QueryString["Product"] != null && Request.QueryString["Product"].ToString() != "")
                {
                    ProductType = Request.QueryString["Product"].ToString();
                }
                if (Request.QueryString["Shipper"] != null && Request.QueryString["Shipper"].ToString() != "")
                {
                    Shipper = Request.QueryString["Shipper"].ToString();
                }
                if (Request.QueryString["Consignee"] != null && Request.QueryString["Consignee"].ToString() != "")
                {
                    Consignee = Request.QueryString["Consignee"].ToString();
                }

                blnResult = objBAL.CheckVolumetricExemption(AgentCode, CommodityCode, dtExecutiondt,ProductType,Shipper,Consignee);
            }
            catch
            {
                blnResult = false;
            }
            return blnResult;
        }

        private void CalculateTotalWt()
        {
            decimal FinalTotal = 0, FinalVolume = 0, Volume = 0, Weight = 0;

            for (int i = 0; i < grdDim.Rows.Count; i++)
            {
                Volume = 0; Weight = 0;

                if (((TextBox)grdDim.Rows[i].FindControl("txtVol")).Text.Trim() != "")
                    Volume = Convert.ToDecimal(((TextBox)grdDim.Rows[i].FindControl("txtVol")).Text.Trim());

                if (((TextBox)grdDim.Rows[i].FindControl("txtVolWT")).Text.Trim() != "")
                    Weight = Convert.ToDecimal(((TextBox)grdDim.Rows[i].FindControl("txtVolWT")).Text.Trim());

                FinalVolume = FinalVolume + Volume;
                FinalTotal = FinalTotal + Weight;
            }

            if (hdVolumetriccheck.Value == "1")  //Volumetric Exeption check.
            {
                TXTTotal.Text = Request.QueryString["GrossWt"].ToString();
            }
            else
            {
                TXTTotal.Text = (Math.Truncate(decimal.Parse((FinalTotal).ToString()) * 100) / 100).ToString();
            }
            TXTVolume.Text = (Math.Truncate(decimal.Parse((FinalVolume).ToString()) * 100) / 100).ToString();

            decimal TotalVolume = 0;

            if (TXTVolume.Text.Trim() != "")
                TotalVolume = Convert.ToDecimal(TXTVolume.Text.Trim());

            if (TotalVolume != 0)
            {
                if (ddlUnit.Text.Trim().ToUpper() == "CMS")
                {
                    txtMeterVolume.Text = (Math.Truncate(decimal.Parse((TotalVolume / 1000000).ToString()) * 100) / 100).ToString();
                }
                else if (ddlUnit.Text.Trim().ToUpper() == "INCHES")
                {
                    txtMeterVolume.Text = (Math.Truncate(decimal.Parse((TotalVolume / 61024).ToString()) * 100) / 100).ToString();
                }
                else if (ddlUnit.Text.Trim().ToUpper() == "METERS")
                {
                    txtMeterVolume.Text = TXTVolume.Text;
                }
            }
            else
                txtMeterVolume.Text = "0";
        }
    }
}