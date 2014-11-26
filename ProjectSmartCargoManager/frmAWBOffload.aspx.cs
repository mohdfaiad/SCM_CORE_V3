using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;
using BAL;
using System.Drawing;
using System.Text;

namespace ProjectSmartCargoManager
{
    public partial class frmAWBOffload : System.Web.UI.Page
    {
        BALProductType objBAL = new BALProductType();
        SQLServer da = new SQLServer(Global.GetConnectionString());

        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dtCurrentTable = new DataTable("AWBOffload_dtAWBData");
            DataTable dt1 = new DataTable("AWBOffload_dt1"); 

            try
            {
                if (!IsPostBack)
                {
                        dtCurrentTable= (DataTable)Session["AWBdata"];
                    string strDestination = string.Empty;

                    if (Request.QueryString["FltNo"] != null)
                        lblFlightNo.Text = Request.QueryString["FltNo"].ToString();

                    if (Request.QueryString["FltDt"] != null)
                        lblFlightDate.Text = Request.QueryString["FltDt"].ToString();

                    if (Request.QueryString["Station"] != null)
                        lblAirport.Text = Convert.ToString(Session["Station"]); //Request.QueryString["Station"].ToString();

                    if (Request.QueryString["ManifestMode"] != null)
                        Session["ManifestMode"] = Request.QueryString["ManifestMode"].ToString();

                    if (dtCurrentTable != null && dtCurrentTable.Rows.Count > 0)
                    {
                        grdAWBs.DataSource = dtCurrentTable;
                        grdAWBs.DataSource = dtCurrentTable;
                        grdAWBs.DataBind();
                    }

                        dt1= dtCurrentTable;

                    if (dt1.Rows.Count > 0)
                    {
                        DataRow drCurrentRow = null;
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            drCurrentRow = dtCurrentTable.NewRow();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text = dt1.Rows[i][0].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Text = dt1.Rows[i][1].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text = dt1.Rows[i][2].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtAvlPCS")).Text = dt1.Rows[i][3].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtAwlWeight")).Text = dt1.Rows[i][4].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtOrigin")).Text = dt1.Rows[i][5].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtDestination")).Text = dt1.Rows[i][6].ToString();

                            if(strDestination=="")
                                strDestination = dt1.Rows[i][6].ToString();

                            ((Label)grdAWBs.Rows[i].FindControl("lblULDNO")).Text = dt1.Rows[i][7].ToString();
                            ((Label)grdAWBs.Rows[i].FindControl("lblCartNumber")).Text = dt1.Rows[i]["CartNumber"].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Enabled = false;

                            ViewState["CurrentTable1"] = dtCurrentTable;
                        }
                    }

                    LoadGridRoutingDetail();

                    for (int i = 0; i < grdRouting.Rows.Count; i++)
                    {
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text = strDestination;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text = "";
                        ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text = "";

                        if ((DataTable)Session["AWBdata"] != null && ((DataTable)Session["AWBdata"]).Rows.Count > 1)
                        {
                            ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Enabled = false;
                            ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Enabled = false;
                            ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Enabled = false;
                            ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Enabled = false;
                        }
                    }

                    LoadAirlineCode("");

                    txtFltDest_TextChanged((TextBox)grdRouting.Rows[0].FindControl("txtFltDest"), null);                   
                    
                    //UpdatePartnerCode(1);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "alert('Error :'" + ex.Message + ")", true);
            }
            finally
            {
                if (dtCurrentTable != null)
                {
                    dtCurrentTable.Dispose();
                }
                if (dt1 != null)
                {
                    dt1.Dispose();
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "window.close();", true);
        }

        private void LoadAWBGrid()
        {
            DataTable myDataTable = new DataTable("AWBOffload_dtAWBData1");
            DataColumn myDataColumn;
            //DataSet Ds = new DataSet();

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "AWBNo";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Pieces";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Weight";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "AvlPCS";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "AvlWgt";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "ULDNo";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "CartNumber";
            myDataTable.Columns.Add(myDataColumn);

            DataRow dr;
            dr = myDataTable.NewRow();
            //dr["RowNumber"] = 1;
            dr["AWBNo"] = "";//"5";
            dr["Pieces"] = "";// "5";
            dr["Weight"] = "";
            dr["AvlPCS"] = "";
            dr["AvlWgt"] = "";
            dr["ULDNo"] = "";
            dr["CartNumber"] = "";

            myDataTable.Rows.Add(dr);
            ViewState["CurrentTable11"] = myDataTable;
            //Bind the DataTable to the Grid

            grdAWBs.DataSource = null;
            grdAWBs.DataSource = myDataTable;
            grdAWBs.DataBind();

            if (myDataTable != null)
                myDataTable.Dispose();
        }

        public void LoadAirlineCode(string filter)
        {
            DataSet ds = new DataSet("AWBOffload_ds");

            try
            {
                BookingBAL objBLL = new BookingBAL();

                //DataSet ds = objBLL.GetPartnerType(true);
                ds = null;

                if (CommonUtility.PartnerTypeMaster == null)
                {
                    BookingBAL objBookingBal = new BookingBAL();
                    CommonUtility.PartnerTypeMaster = objBookingBal.GetPartnerType(true);
                    objBookingBal = null;
                }

                ds = CommonUtility.PartnerTypeMaster;

                DropDownList ddl = new DropDownList();
                TextBox txtdest = new TextBox();
                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {
                    ddl = ((DropDownList)(grdRouting.Rows[i].FindControl("ddlPartnerType")));
                    //txtdest = ((TextBox)(grdRouting.Rows[i].FindControl("txtFltDest")));
                    if (txtdest.Text.Length < 1 || txtdest.Text == "")//(ddl.Text.Equals("SG",StringComparison.OrdinalIgnoreCase)|| ddl.Text=="")
                    {
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                ddl.Items.Clear();
                                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                                {
                                    ddl.Items.Add(ds.Tables[0].Rows[k][0].ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
        }

        private void UpdatePartnerCode(int rowindex)
        {
            DataSet dsResult = new DataSet("AWBOffload_dsResult");

            try
            {
                string errormessage = "";
                BookingBAL objBLL = new BookingBAL();
                DateTime dtCurrentDate = (DateTime)Session["IT"];

                if (CommonUtility.PartnerMaster == null)
                {
                    BookingBAL objBookingBal = new BookingBAL();
                    CommonUtility.PartnerMaster = objBookingBal.GetAvailabePartners();
                    objBookingBal = null;
                }

                dsResult = CommonUtility.PartnerMaster;

                //if (objBLL.GetAvailabePartners(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.ToUpper(), ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text.ToUpper(), dtCurrentDate, ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlPartnerType")).Text.ToUpper(), ref dsResult, ref errormessage))
                //{
                if (dsResult != null)
                {
                    if (dsResult.Tables.Count > 0)
                    {
                        DropDownList ddl = ((DropDownList)(grdRouting.Rows[rowindex].FindControl("ddlPartner")));
                        TextBox txtdest = ((TextBox)(grdRouting.Rows[rowindex].FindControl("txtFltDest")));

                        if (dsResult != null)
                        {
                            if (dsResult.Tables.Count > 0)
                            {
                                ddl.Items.Clear();
                                //ddl.Items.Add("Select");
                                for (int j = 0; j < dsResult.Tables.Count; j++)
                                {
                                    if (dsResult.Tables[j].Rows.Count > 0)
                                    {
                                        for (int k = 0; k < dsResult.Tables[j].Rows.Count; k++)
                                        {
                                            ddl.Items.Add(dsResult.Tables[j].Rows[k][0].ToString());
                                        }
                                    }
                                }
                                try
                                {
                                    if (ddl.Items.Count < 1)
                                    {
                                        ddl.Items.Add("Select");
                                    }
                                    ddl.Items.Add("Other");
                                }
                                catch (Exception ex) { }
                            }
                        }

                    }
                }
                //}

            }
            catch (Exception ex) { }

            finally
            {
                if (dsResult != null)
                {
                    dsResult.Dispose();
                }
            }
        }

        protected void btnAddRouteDetails_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            DataSet dsRoutDetails = new DataSet("AWBOffload_dsRouteDetails");
            try
            {
                string prevdest = "", prevtime = "", strDate = "";

                if (grdRouting.Rows.Count != 0)
                {
                    if (((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtFltDest")).Text.Trim() == "")
                    {
                        LBLRouteStatus.Text = "Set previous destination first.";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

                        return;
                    }

                    else
                    {
                        prevdest = ((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtFltDest")).Text.Trim();
                        if (prevdest == BookingBAL.DestStation)
                        {
                            prevdest = BookingBAL.OrgStation;
                        }

                    }
                }

                strDate = ((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtFdate")).Text.Trim();

                SaveRouteDetails();

                 dsRoutDetails = ((DataSet)Session["dsRoutDetails"]).Copy();
                DataRow row = dsRoutDetails.Tables[0].NewRow();

                row["FltOrigin"] = prevdest;
                row["FltDate"] = strDate;  //dtCurrentDate.ToString("dd/MM/yyyy");
                row["Airline"] = "5J"; //txtFlightCode.Text.ToString();

                dsRoutDetails.Tables[0].Rows.Add(row);

                Session["dsRoutDetails"] = dsRoutDetails.Copy();
                grdRouting.DataSource = null;
                grdRouting.DataSource = dsRoutDetails.Copy();
                grdRouting.DataBind();

                //Validation by Vijay
                ((TextBox)grdRouting.Rows[0].FindControl("txtFltOrig")).ReadOnly = true;
                // ((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtFltOrig")).ReadOnly = true;

                LoadDropDownAndCheckBoxRouteDetails();
                LoadAirlineCode("");
                // Session["Mod"] = "1";
                CheckPartnerFlightSupport();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "" + ex.Message;
            }

            finally
            {
               if (dsRoutDetails!=null)
               {
                   dsRoutDetails.Dispose();
               }
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

        }

        protected void btnDeleteRoute_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            DataSet dsRouteDetailsTemp = new DataSet("AWBOffload_dsRouteDetails2");
            DataSet dsRouteDetails = new DataSet("AWBOffload_dsRouteDetails3");
            try
            {
                SaveRouteDetails();

                dsRouteDetailsTemp = ((DataSet)Session["dsRoutDetails"]).Clone();
                dsRouteDetails = (DataSet)Session["dsRoutDetails"];

                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {
                    if (!((CheckBox)grdRouting.Rows[i].FindControl("CHKSelect")).Checked)
                    {
                        DataRow row = dsRouteDetailsTemp.Tables[0].NewRow();

                        row["FltOrigin"] = ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text;
                        row["FltDestination"] = ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text;
                        row["FltNumber"] = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem;
                        row["FltDate"] = ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text;
                        row["Pcs"] = ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim();
                        row["Wt"] = ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim();
                        row["Airline"] = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).SelectedItem;
                        row["PartnerType"] = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).SelectedItem;
                        row["Location"] = ((TextBox)grdRouting.Rows[i].FindControl("txtRouteLocation")).Text.Trim();
                        dsRouteDetailsTemp.Tables[0].Rows.Add(row);
                    }
                }

                Session["dsRoutDetails"] = dsRouteDetailsTemp.Copy();
                grdRouting.DataSource = null;
                grdRouting.DataSource = dsRouteDetailsTemp.Copy();
                grdRouting.DataBind();

                LoadDropDownAndCheckBoxRouteDetails();

                Session["Mod"] = "1";
                CheckPartnerFlightSupport();
            }
            catch (Exception ex)
            {
                LBLRouteStatus.Text = "" + ex.Message;
            }
            finally
            {
                if (dsRouteDetails != null)
                {
                    dsRouteDetails.Dispose();
                }
                if (dsRouteDetailsTemp != null)
                {
                    dsRouteDetailsTemp.Dispose();
                }
            }
        }

        public void SaveRouteDetails()
        {
            DataSet dsRoutDetails = new DataSet("AWBOffload_dsRouteDetails4");
            dsRoutDetails = ((DataSet)Session["dsRoutDetails"]).Clone();

            for (int i = 0; i < grdRouting.Rows.Count; i++)
            {
                DataRow row = dsRoutDetails.Tables[0].NewRow();

                row["FltOrigin"] = ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text;
                row["FltDestination"] = ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text;
                row["FltNumber"] = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text;
                row["FltTime"] = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Value;
                row["FltDate"] = ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text;
                row["Pcs"] = ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim();
                row["Wt"] = ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim();
                row["Airline"] = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).SelectedItem.Text;
                row["PartnerType"] = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).SelectedItem.Text;
                row["Location"] = ((TextBox)grdRouting.Rows[i].FindControl("txtRouteLocation")).Text.Trim();
                dsRoutDetails.Tables[0].Rows.Add(row);
            }

            Session["dsRoutDetails"] = dsRoutDetails.Copy();

            if (dsRoutDetails != null)
            {
                dsRoutDetails.Dispose();
            }
        }

        public void LoadDropDownAndCheckBoxRouteDetails()
        {
            DataSet dsRouteDetails = new DataSet("AWBOffload_dsRouteDetails5");
            try
            {
                dsRouteDetails = (DataSet)Session["dsRoutDetails"];
                for (int i = 0; i < dsRouteDetails.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).Items.Add(new ListItem(dsRouteDetails.Tables[0].Rows[i]["PartnerType"].ToString().Trim()));
                        DropDownList routedroplist = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType"));
                        routedroplist.Text = dsRouteDetails.Tables[0].Rows[i]["PartnerType"].ToString().Trim();
                    }
                    catch (Exception ex) { }

                    try
                    {
                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).Items.Add(new ListItem(dsRouteDetails.Tables[0].Rows[i]["Airline"].ToString().Trim()));
                        DropDownList routedroplist = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner"));
                        routedroplist.Text = dsRouteDetails.Tables[0].Rows[i]["Airline"].ToString().Trim();

                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Items.Clear();
                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Items.Add(new ListItem(dsRouteDetails.Tables[0].Rows[i]["FltNumber"].ToString().Trim(), dsRouteDetails.Tables[0].Rows[i]["FltTime"].ToString().Trim()));



                    }
                    catch (Exception ex) { }
                    ///ddlPartner

                }



                //    ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Items.Clear();
                //    ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Items.Add(new ListItem(dsRouteDetails.Tables[0].Rows[i]["FltNumber"].ToString().Trim(), dsRouteDetails.Tables[0].Rows[i]["FltTime"].ToString().Trim()));

                //}
            }
            catch (Exception ex)
            {
                LBLRouteStatus.Text = "" + ex.Message;
            }
            finally
            {
                if (dsRouteDetails != null)
                {
                    dsRouteDetails.Dispose();
                }
            }
        }

        private void CheckPartnerFlightSupport()
        {
            LoginBL objBL = new LoginBL();
            try
            {
                //if (!Convert.ToBoolean(objBL.GetMasterConfiguration("SupportPartnerFlight")))
                if (!Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "SupportPartnerFlight")))
                {
                    for (int intCount = 0; intCount < grdRouting.Rows.Count; intCount++)
                    {
                        ((DropDownList)(grdRouting.Rows[intCount].FindControl("ddlPartnerType"))).Enabled = false;
                        ((DropDownList)(grdRouting.Rows[intCount].FindControl("ddlPartner"))).Enabled = false;
                    }
                }
            }
            catch (Exception ex) { }
            finally
            {
                objBL = null;
            }
        }

        protected void txtFltDest_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int rowindex = 0;

                //validation by Vijay
                LBLRouteStatus.Text = "";

                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {
                    if (((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text == ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text)
                    {
                        LBLRouteStatus.Text = "kindly check Flight origin and destination in route details.";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text = "";
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Focus();
                        return;
                    }

                    if (((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")) == ((TextBox)sender) || ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")) == ((TextBox)sender))
                    {
                        rowindex = i;
                    }

                    if (((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")) == ((TextBox)sender))
                    {
                        rowindex = i;

                        //validation by Vijay
                        if (rowindex < grdRouting.Rows.Count - 1 && ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text.Trim() == ((TextBox)grdRouting.Rows[0].FindControl("txtOrigin")).Text.Trim())
                        {
                            LBLRouteStatus.Text = "kindly check Flight origin and destination in route details.";
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                            ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Focus();
                            return;
                        }

                        if (rowindex < grdRouting.Rows.Count - 1)
                        {
                            ((TextBox)grdRouting.Rows[rowindex + 1].FindControl("txtFltOrig")).Text = ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text;
                        }
                    }
                }

                BookingBAL.OrgStation = BookingBAL.OrgStation.Trim();

                ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text = ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.ToUpper().Trim();
                ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text = ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text.ToUpper().Trim();

                //string errormessage = "";
                UpdatePartnerCode(rowindex);
                GetFlightRouteData(rowindex);

                Session["Mod"] = "1";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
        }

        private void GetFlightRouteData(int rowindex)
        {
            DataSet dsresult = new DataSet("AWBOffload_dsresult1");
            DataSet ds = new DataSet("AWBOffload_ds1");

            try
            {
                string strPartnerCode = ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlPartner")).Text.Trim();

                string errormessage = "";
                // DataSet dsresult = GetFlightList(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text, ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text, ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFdate")).Text, hr, min, AllowedHr, ref errormessage);
                dsresult = GetFlightList(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.ToUpper(), ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text.ToUpper(), ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFdate")).Text, 0, 0, 0, ref errormessage, strPartnerCode);

                if (dsresult != null && dsresult.Tables.Count != 0)
                {
                    ds = (DataSet)Session["Flt"];
                    if (ds != null)
                    {
                        string name = "Table" + rowindex.ToString();
                        try
                        {
                            if (ds.Tables.Count > rowindex)
                            {
                                try
                                {
                                    if (ds.Tables[name] != null && ds.Tables[name].Rows.Count > 0)
                                    {
                                        ds.Tables.Remove(name);
                                        DataTable dt = new DataTable();
                                        dt = dsresult.Tables[0].Copy();
                                        dt.TableName = name;
                                        ds.Tables.Add(dt);
                                        ds.AcceptChanges();
                                        Session["Flt"] = ds.Copy();
                                    }
                                }
                                catch (Exception ex) { }

                            }
                            else if (ds.Tables.Count == 1)
                            {
                                Session["Flt"] = dsresult.Copy();
                            }
                        }
                        catch (Exception ex) { }
                    }
                    else
                    {
                        Session["Flt"] = dsresult.Copy();
                    }
                    DataRow row = dsresult.Tables[0].NewRow();
                    row["FltNumber"] = "";
                    row["ArrTime"] = "";

                    dsresult.Tables[0].Rows.Add(row);

                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataTextField = "FltNumber";
                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataValueField = "ArrTime";
                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataSource = dsresult.Tables[0].Copy();
                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataBind();

                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).SelectedIndex = dsresult.Tables[0].Rows.Count - 1;

                    //HidProcessFlag.Value = "1";
                }
                else
                {
                    LBLRouteStatus.Text = "no record found";
                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).Items.Clear();
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                    return;
                }

            }
            catch (Exception ex)
            { }

            finally
            {
                if (dsresult != null)
                {
                    dsresult.Dispose();
                }
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
        }

        public DataSet GetFlightList(string Origin, string Dest, string strdate, int hr, int min, int AllowedHr, ref string errormessage, string PartnerCode)
        {
            DataSet dsResult = new DataSet("AWBOffload_dsresult2");
            DateTime dtCurrentDate = (DateTime)Session["IT"];
            bool blnSelfAirline = false;
            DataSet dsAWBPrefixs = CommonUtility.AWBPrefixMaster;

            if (PartnerCode != "")
            {
                if (dsAWBPrefixs != null && dsAWBPrefixs.Tables.Count > 0 && dsAWBPrefixs.Tables[0].Rows.Count > 0)
                {
                    for (int intCount = 0; intCount < dsAWBPrefixs.Tables[0].Rows.Count; intCount++)
                    {
                        if (PartnerCode.ToUpper() == Convert.ToString(dsAWBPrefixs.Tables[0].Rows[intCount]["AirlinePrefix"]).ToUpper())
                        {
                            blnSelfAirline = true;
                            dsAWBPrefixs = null;
                            break;
                        }
                    }
                }
            }

            if (strdate.Trim() == "")
            {
                if (blnSelfAirline)
                {
                    if (new ShowFlightsBAL().GetFlightListforDay(Origin, Dest, ref dsResult, ref errormessage, dtCurrentDate, PartnerCode))
                    {
                        FormatRecords(Origin, Dest, ref dsResult, hr, min, AllowedHr);
                        return dsResult;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (new ShowFlightsBAL().GetPartnerFlightList(Origin, Dest, ref dsResult, ref errormessage, dtCurrentDate, PartnerCode))
                    {
                        FormatRecords(Origin, Dest, ref dsResult, hr, min, AllowedHr);
                        return dsResult;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {

                string[] splitdate = strdate.Split(new char[] { '/' });
                int year = int.Parse(splitdate[2]);
                int month = int.Parse(splitdate[1]);
                int day = int.Parse(splitdate[0]);
                DateTime dt = new DateTime(year, month, day);

                int diff = (dt - dtCurrentDate.Date).Days;

                if (blnSelfAirline)
                {
                    if (new ShowFlightsBAL().GetFlightListforDay(Origin, Dest, ref dsResult, ref errormessage, dt, PartnerCode))
                    {
                        FormatRecords(Origin, Dest, ref dsResult, hr, min, AllowedHr);
                        return dsResult;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (new ShowFlightsBAL().GetPartnerFlightList(Origin, Dest, ref dsResult, ref errormessage, dt, PartnerCode))
                    {
                        FormatRecords(Origin, Dest, ref dsResult, hr, min, AllowedHr);
                        return dsResult;
                    }
                    else
                    {
                        return null;
                    }
                }
            
           
        }
            if (dsResult != null)
            {
                dsResult.Dispose();
            }
        }

        public void FormatRecords(string org, string dest, ref DataSet dsResult, int PrevHr, int PrevMin, int AllowedHr)
        {
            int i = 0;
            string ScheduleID = "";
            DataSet dsNewResult = new DataSet("AWBOffload_dsNewResult");
            dsNewResult = dsResult.Clone();
            bool blOrignFlound, blDestFound;
            blOrignFlound = blDestFound = false;
            DateTime dtCurrentDate = (DateTime)Session["IT"];

            foreach (DataRow row in dsResult.Tables[0].Rows)
            {
                if (ScheduleID == "")
                {
                    if (row["FltOrigin"].ToString() != org)
                    {
                        continue;
                    }
                    else
                    {
                        blOrignFlound = true;
                    }

                    ScheduleID = row["ScheduleID"].ToString();
                    DataRow rw = dsNewResult.Tables[0].NewRow();

                    for (int j = 0; j < dsNewResult.Tables[0].Columns.Count; j++)
                    {
                        rw[j] = row[j];
                    }

                    if (row["FltDestination"].ToString() == dest)
                    {
                        blDestFound = true;
                    }

                    dsNewResult.Tables[0].Rows.Add(rw);

                }
                else if (ScheduleID.Trim() == row["ScheduleID"].ToString())
                {
                    if (!blDestFound)
                    {
                        dsNewResult.Tables[0].Rows[dsNewResult.Tables[0].Rows.Count - 1]["FltDestination"] = row["FltDestination"].ToString();
                        dsNewResult.Tables[0].Rows[dsNewResult.Tables[0].Rows.Count - 1]["ArrTime"] = row["ArrTime"].ToString();

                        if (row["FltDestination"].ToString() == dest)
                        {
                            blDestFound = true;
                        }

                    }

                }
                else
                {
                    if (row["FltOrigin"].ToString() != org)
                    {
                        continue;
                    }
                    else
                    {
                        blOrignFlound = true;
                        blDestFound = false;
                    }

                    ScheduleID = row["ScheduleID"].ToString();


                    DataRow rw = dsNewResult.Tables[0].NewRow();

                    for (int j = 0; j < dsNewResult.Tables[0].Columns.Count; j++)
                    {
                        rw[j] = row[j];
                    }

                    if (row["FltDestination"].ToString() == dest)
                    {
                        blDestFound = true;
                    }

                    dsNewResult.Tables[0].Rows.Add(rw);
                }

                i++;

            }

            dsResult = dsNewResult.Copy();
            DataView dv = new DataView(dsResult.Tables[0].Copy());
            dv.Sort = "DeptTime";

            dsResult = new DataSet();
            dsResult.Tables.Add(dv.ToTable().Copy());


            //TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            //DateTime dtIndianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);


            DataTable dt = dsResult.Tables[0].Clone();
            foreach (DataRow row in dsResult.Tables[0].Rows)
            {
                string depttime = row["DeptTime"].ToString();
                int hr = int.Parse(depttime.Substring(0, depttime.IndexOf(":")));
                int min = int.Parse(depttime.Substring(depttime.IndexOf(":") + 1));

                string[] strDate = row["FltDate"].ToString().Split('/');
                int intFltDate = int.Parse(strDate[0]);
                int intCurrentDt = dtCurrentDate.Day;

                bool canAdd = true;

                if (canAdd)
                {
                    DataRow rw = dt.NewRow();

                    for (int k = 0; k < dt.Columns.Count; k++)
                    {
                        rw[k] = row[k];
                    }

                    dt.Rows.Add(rw);
                }
            }

            dsResult = new DataSet();
            dsResult.Tables.Add(dt);

            if (dsResult != null)
            {
                dsResult.Dispose();
            }
            if (dsNewResult != null)
            {
                dsNewResult.Dispose();
            }
            
        }

        protected void ddlPartnerType_SelectionChange(object sender, EventArgs e)
        {
            try
            {
                int rowindex = 0;
                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {
                    if (((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")) == ((DropDownList)sender))
                    {
                        rowindex = i;
                    }
                }
                UpdatePartnerCode(rowindex);
                GetFlightRouteData(rowindex);
            }
            catch (Exception ex)
            { }
        }

        protected void ddlPartner_SelectionChange(object sender, EventArgs e)
        {
            try
            {
                int rowindex = 0;
                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {

                    if ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner") == (DropDownList)sender)
                    {
                        DropDownList ddl = (DropDownList)grdRouting.Rows[i].FindControl("ddlPartner");
                        DropDownList drop = (DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum");
                        TextBox txtbox = (TextBox)grdRouting.Rows[i].FindControl("txtFlightID");

                        if (ddl.Text.ToString().Equals("other", StringComparison.OrdinalIgnoreCase))
                        {
                            drop.Visible = false;
                            txtbox.Visible = true;
                        }
                        else
                        {
                            drop.Visible = true;
                            txtbox.Visible = false;
                        }
                        rowindex = i;
                        break;
                    }
                }
                GetFlightRouteData(rowindex);
                ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFdate")).Text = ((DateTime)Session["IT"]).ToString("dd/MM/yyyy");
                //((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).Text = "";
            }
            catch (Exception ex) { }
        }

        protected void txtFdate_TextChanged(object sender, EventArgs e)
        {
            //if (HidChangeDate.Value != "Y")
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
            //    return;
            //}
            DataSet dsresult = new DataSet("AWBOffload_dsResult3");
            try
            {
                int rowindex = 0;
                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {
                    if (((TextBox)grdRouting.Rows[i].FindControl("txtFdate")) == ((TextBox)sender) || ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")) == ((TextBox)sender))
                    {
                        rowindex = i;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Focus();
                    }
                }

                int hr = 0, min = 0, AllowedHr = 0;

                string errormessage = "";
                 dsresult = GetFlightList(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text, ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text, ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFdate")).Text, hr, min, AllowedHr, ref errormessage);

                if (dsresult != null && dsresult.Tables.Count != 0)
                {
                    Session["Flt"] = dsresult.Copy();
                    DataRow row = dsresult.Tables[0].NewRow();
                    row["FltNumber"] = "";
                    row["ArrTime"] = "";

                    dsresult.Tables[0].Rows.Add(row);

                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataTextField = "FltNumber";
                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataValueField = "ArrTime";
                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataSource = dsresult.Tables[0].Copy();
                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataBind();
                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).SelectedIndex = dsresult.Tables[0].Rows.Count - 1;

                }
                else
                {
                    LBLRouteStatus.Text = "no record found";
                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).Items.Clear();
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                    return;
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                if(dsresult!=null)
                {
                    dsresult.Dispose();
                }
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

        }

        public DataSet GetFlightList(string Origin, string Dest, string strdate, int hr, int min, int AllowedHr, ref string errormessage)
        {
            DataSet dsResult = new DataSet("AWBOffload_dsResult4");
            DateTime dtCurrentDate = (DateTime)Session["IT"];

            if (strdate.Trim() == "")
            {
                if (new ShowFlightsBAL().GetFlightList(Origin, Dest, 0, ref dsResult, ref errormessage, dtCurrentDate))
                {
                    FormatRecords(Origin, Dest, ref dsResult, hr, min, AllowedHr);
                    return dsResult;
                }
                else
                {
                    return null;
                }
            }
            else
            {

                string[] splitdate = strdate.Split(new char[] { '/' });
                int year = int.Parse(splitdate[2]);
                int month = int.Parse(splitdate[1]);
                int day = int.Parse(splitdate[0]);
                DateTime dt = new DateTime(year, month, day);

                int diff = (dt - dtCurrentDate.Date).Days;

                if (new ShowFlightsBAL().GetFlightList(Origin, Dest, diff, ref dsResult, ref errormessage, dtCurrentDate.Date))
                {
                    FormatRecords(Origin, Dest, ref dsResult, hr, min, AllowedHr);
                    return dsResult;
                }
                else
                {
                    return null;
                }

                if (dsResult != null)
                {
                    dsResult.Dispose();
                }

            }

           

        }

        protected void txtFltNumber_TextChanged(object sender, EventArgs e)
        {
            int rowindex = 0;
            for (int i = 0; i < grdRouting.Rows.Count; i++)
            {
                if (((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")) == ((DropDownList)sender))
                {
                    rowindex = i;
                }
            }

            DataSet ds = (DataSet)Session["Flt"];

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string Origin = ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.ToUpper();
                string Dest = ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text.ToUpper();
                string FlightNo = ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).SelectedItem.ToString();
                int ScheduleID = 0;

                for (int intCount = 0; intCount < ds.Tables[0].Rows.Count; intCount++)
                {
                    if (Origin == ds.Tables[0].Rows[intCount]["FltOrigin"].ToString() && Dest == ds.Tables[0].Rows[intCount]["FltDestination"].ToString()
                        && FlightNo == ds.Tables[0].Rows[intCount]["FltNumber"].ToString())
                    {
                        ScheduleID = int.Parse(ds.Tables[0].Rows[intCount]["ScheduleID"].ToString());
                    }
                }

                ((HiddenField)grdRouting.Rows[rowindex].FindControl("HidScheduleID")).Value = ScheduleID.ToString();
                //Session["Mod"] = "1";
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
        }

        protected void btnAddManifest_Click(object sender, EventArgs e)
        {
            BLExpManifest objExpManifest = new BLExpManifest();
            ShowFlightsBAL objBAL = new ShowFlightsBAL();
            DataSet objDS = new DataSet("AWBOffload_objDS");
            //DataSet dsawbData = null;
            //DataTable dsawb1 = null;
            string CartNumber = string.Empty;
            lblStatus.Text = "";
            DateTime dtCurrentDate = (DateTime)Session["IT"];

            Label1.Text = "";
            //int PCS = 0;

            //Added on 28 sept 

            string strMode = Convert.ToString(Session["ManifestMode"]);
            //double OffloadWGT = 0.0;

            #region "Offload Shipment"

            if (strMode == "O") //Offload Shipment
            {
                if (UpdatePanelRouteDetails.Visible == true)
                {
                    int chkPCS = 0;
                    double chkWGT = 0.0;
                    for (int i = 0; i < grdRouting.Rows.Count; i++)
                    {
                        if (((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim() == "")
                        {
                            Label1.Text = "Please enter valid Origin";
                            Label1.ForeColor = Color.Red;
                            return;
                        }

                        if (((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text.Trim() == "")
                        {
                            Label1.Text = "Please enter valid Destination";
                            Label1.ForeColor = Color.Red;
                            return;
                        }

                        if (((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim().ToUpper() ==
                            ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text.Trim().ToUpper())
                        {
                            Label1.Text = "Please enter valid Origin & Destination";
                            Label1.ForeColor = Color.Red;
                            return;
                        }

                        string Org = ((TextBox)grdRouting.Rows[0].FindControl("txtFltOrig")).Text.Trim();
                        if (Org.ToUpper() != lblAirport.Text.Trim().ToUpper())
                        {
                            Label1.Text = "Please enter valid Origin";
                            Label1.ForeColor = Color.Red;
                            return;
                        }

                        if (((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtFltDest")).Text.Trim().ToUpper() != ((TextBox)grdAWBs.Rows[0].FindControl("txtDestination")).Text.Trim().ToUpper())
                        {
                            Label1.Text = "Please enter valid Destination";
                            Label1.ForeColor = Color.Red;
                            return;
                        }

                        if (((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text == "" || ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text.Trim() == "")
                        {
                            Label1.ForeColor = Color.Red;
                            Label1.Text = "Please enter Next Flight Details.";
                            return;
                        }

                        //if (ddlReason.Text == "Others" && txtReason.Text.Trim() == "")
                        //{
                        //    Label1.Text = "Please enter Offload reason";
                        //    Label1.ForeColor = Color.Red;
                        //    return;
                        //}
                    }
                }

                if (txtNextFlight.Text.ToUpper().Trim() != "")
                {
                    string Origin = Convert.ToString(Session["Station"]);
                    string FltNo = txtNextFlight.Text.ToUpper().Trim();
                    string Dest = ((TextBox)grdAWBs.Rows[0].FindControl("txtDestination")).Text.Trim();
                    DateTime dt = DateTime.ParseExact(txtNFltDate.Text.Trim(), "dd/MM/yyyy", null);

                    int diff = (dt - dtCurrentDate.Date).Days;

                    string errormessage = string.Empty;
                    string flightID = lblFlightNo.Text;
                    string AWBNo = ((TextBox)grdAWBs.Rows[0].FindControl("txtAWBno")).Text.Trim();
                    AWBNo = AWBNo.Substring(AWBNo.Length - 8);
                    objBAL.GetFlightListForManifest(Origin, Dest, diff, ref objDS, ref errormessage, dtCurrentDate.Date, AWBNo, flightID, DateTime.ParseExact(lblFlightDate.Text, "dd/MM/yyyy", null));
                    bool blnExists = false;

                    if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
                    {
                        for (int ICount = 0; ICount < objDS.Tables[0].Rows.Count; ICount++)
                        {
                            if (FltNo == objDS.Tables[0].Rows[ICount]["FltNumber"].ToString().Trim())
                            {
                                blnExists = true;
                                break;
                            }
                        }
                    }

                    objBAL = null;
                    objDS = null;

                    if (blnExists == false)
                    {
                        Label1.ForeColor = Color.Red;
                        Label1.Text = "Please enter valid flight number.";
                        return;
                    }
                }
                //

                for (int intCount = 0; intCount < grdAWBs.Rows.Count; intCount++)
                {
                    string GrdAWBNumber = ((TextBox)grdAWBs.Rows[intCount].FindControl("txtAWBno")).Text.Trim();
                    int GrdPieces = Convert.ToInt32(((TextBox)grdAWBs.Rows[intCount].FindControl("txtAvlPCS")).Text.Trim());
                    float GrdWeight = float.Parse(((TextBox)grdAWBs.Rows[intCount].FindControl("txtAwlWeight")).Text.Trim());
                    string GrdOrigin = ((TextBox)grdAWBs.Rows[intCount].FindControl("txtOrigin")).Text.Trim();
                    string GrdDestination = ((TextBox)grdAWBs.Rows[intCount].FindControl("txtDestination")).Text.Trim();
                    string GrdULDNo = ((Label)grdAWBs.Rows[intCount].FindControl("lblULDNO")).Text.Trim();
                    string GrdCartNo = ((Label)grdAWBs.Rows[intCount].FindControl("lblCartNumber")).Text.Trim();

                    SaveOffLoadDetails(GrdAWBNumber, GrdPieces, GrdWeight, GrdOrigin, GrdDestination, GrdULDNo, GrdCartNo);
                }
                //Button Cancel called to clear off the previous data entry
                //btnCancel_Click(null, null);
                ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "javascript:CloseWindow();", true);
                return;
            }

            #endregion "Offload Shipment"

            #region "Re-Assign Shipment"

            if (strMode == "R") //Re-Assign Shipment
            {
                if (UpdatePanelRouteDetails.Visible == true)
                {
                    //int chkPCS = 0;
                    //double chkWGT = 0.0;
                    for (int i = 0; i < grdRouting.Rows.Count; i++)
                    {
                        //if (((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text == "Select")
                        //{
                        //    Label1.Text = "Please enter valid flight";
                        //    Label1.ForeColor = Color.Red;
                        //    return;
                        //}

                        if (((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim() == "")
                        {
                            Label1.Text = "Please enter valid Origin";
                            Label1.ForeColor = Color.Red;
                            return;
                        }
                        if (((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text.Trim() == "")
                        {
                            Label1.Text = "Please enter valid Destination";
                            Label1.ForeColor = Color.Red;
                            return;
                        }
                        if (((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim().ToUpper() ==
                            ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text.Trim().ToUpper())
                        {
                            Label1.Text = "Please enter valid Origin & Destination";
                            Label1.ForeColor = Color.Red;
                            return;
                        }
                        string Org = ((TextBox)grdRouting.Rows[0].FindControl("txtFltOrig")).Text.Trim();
                        if (Org.ToUpper() != lblAirport.Text.ToUpper())
                        {
                            Label1.Text = "Please enter valid Origin";
                            Label1.ForeColor = Color.Red;
                            return;
                        }

                        if (((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtFltDest")).Text.Trim().ToUpper() !=
                            ((TextBox)grdAWBs.Rows[0].FindControl("txtDestination")).Text.Trim().ToUpper())
                        {
                            Label1.Text = "Please enter valid Destination";
                            Label1.ForeColor = Color.Red;
                            return;
                        }

                        if (((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text.Trim() == "" ||
                            ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text.Trim() == "")
                        {
                            Label1.ForeColor = Color.Red;
                            Label1.Text = "Please enter Next Flight Details.";
                            return;
                        }

                        if (ddlReason.Text.Trim() == "Others" && txtReason.Text == "")
                        {
                            Label1.Text = "Please enter Offload reason";
                            Label1.ForeColor = Color.Red;
                            return;
                        }
                    }
                }

                if (txtNextFlight.Text.ToUpper().Trim() != "")
                {
                    string Origin = lblAirport.Text.Trim();
                    string FltNo = txtNextFlight.Text.ToUpper().Trim();
                    string Dest = ((TextBox)grdAWBs.Rows[0].FindControl("txtDestination")).Text.Trim();
                    DateTime dt = DateTime.ParseExact(txtNFltDate.Text.Trim(), "dd/MM/yyyy", null);

                    int diff = (dt - dtCurrentDate.Date).Days;

                    string errormessage = string.Empty;
                    string flightID = lblFlightNo.Text.Trim();

                    string AWBNo = ((TextBox)grdAWBs.Rows[0].FindControl("txtAWBno")).Text.Trim();
                    AWBNo = AWBNo.Substring(AWBNo.Length - 8);

                    objBAL.GetFlightListForManifest(Origin, Dest, diff, ref objDS, ref errormessage, dtCurrentDate.Date, AWBNo, flightID, 
                        DateTime.ParseExact(lblFlightDate.Text.Trim(),"dd/MM/yyyy",null));
                    bool blnExists = false;

                    if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
                    {
                        for (int ICount = 0; ICount < objDS.Tables[0].Rows.Count; ICount++)
                        {
                            if (FltNo == objDS.Tables[0].Rows[ICount]["FltNumber"].ToString())
                            {
                                blnExists = true;
                                break;
                            }
                        }
                    }

                    if (blnExists == false)
                    {
                        Label1.ForeColor = Color.Red;
                        Label1.Text = "Please enter valid flight number.";
                        return;
                    }
                }
                //

                for (int intCount = 0; intCount < grdAWBs.Rows.Count; intCount++)
                {
                    string GrdAWBNumber = ((TextBox)grdAWBs.Rows[intCount].FindControl("txtAWBno")).Text.Trim();
                    int GrdPieces = Convert.ToInt32(((TextBox)grdAWBs.Rows[intCount].FindControl("txtAvlPCS")).Text.Trim());
                    float GrdWeight = float.Parse(((TextBox)grdAWBs.Rows[intCount].FindControl("txtAwlWeight")).Text.Trim());
                    string GrdOrigin = ((TextBox)grdAWBs.Rows[intCount].FindControl("txtOrigin")).Text.Trim();
                    string GrdDestination = ((TextBox)grdAWBs.Rows[intCount].FindControl("txtDestination")).Text.Trim();
                    string GrdULDNo = ((Label)grdAWBs.Rows[intCount].FindControl("lblULDNO")).Text.Trim();
                    string GrdCartNo = ((Label)grdAWBs.Rows[intCount].FindControl("lblCartNumber")).Text.Trim();

                    SaveOffLoadDetails(GrdAWBNumber, GrdPieces, GrdWeight, GrdOrigin, GrdDestination, GrdULDNo, GrdCartNo);
                }
                //Button Cancel called to clear off the previous data entry
                //btnCancel_Click(null, null);
                ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "javascript:CloseWindow();", true);
                return;
            }

            #endregion "Re-Assign Shipment"
            if (objDS != null)
            {
                objDS.Dispose();
            }
        }

        public void LoadGridRoutingDetail()
        {
            DataTable myDataTable = new DataTable("AWBOffload_dt2");
            DataColumn myDataColumn;
            DataSet Ds = new DataSet("AWBOffload_DS2");
            DateTime dtCurrentDate = (DateTime)Session["IT"];
            DataSet dsRoutDetails = new DataSet("AWBOffload_dsRouteDetails2");

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "FltOrigin";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "FltDestination";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "FltNumber";
            myDataTable.Columns.Add(myDataColumn);


            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "FltTime";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "FltDate";
            myDataTable.Columns.Add(myDataColumn);


            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Pcs";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Wt";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "PartnerType";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Airline";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Location";
            myDataTable.Columns.Add(myDataColumn);

            DataRow dr;
            dr = myDataTable.NewRow();
            dr["FltOrigin"] = Session["Station"].ToString();
            dr["FltDestination"] = "";// "DEL";
            dr["FltNumber"] = "";// "IT101";
            dr["FltDate"] = dtCurrentDate.ToString("dd/MM/yyyy");// "14/Jan/2012";
            dr["Pcs"] = "";// 
            dr["Wt"] = "";// 

            myDataTable.Rows.Add(dr);

            grdRouting.DataSource = null;
            grdRouting.DataSource = myDataTable;
            grdRouting.DataBind();

            //((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtFltOrig")).ReadOnly = true;

            dsRoutDetails.Tables.Add(myDataTable);
            Session["dsRoutDetails"] = dsRoutDetails.Copy();

            if (Ds != null)
            {
                Ds.Dispose();
            }
            if(myDataTable!=null)
            {
                myDataTable.Dispose();
            }
            if (dsRoutDetails != null)
            {
                dsRoutDetails.Dispose();
            }
            // SetRouteGridValues(myDataTable);
        }

        private void SaveOffLoadDetails(string AWBNumber, int Pieces, float Weight, string Origin, string Destination,
            string ULDNo, string CartNo)
        {
            BLExpManifest objExpManifest = new BLExpManifest();
            CustomsImportBAL objCustoms = new CustomsImportBAL();
            DataSet dCust = new DataSet("AWBOffload_dsdCust");
            try
            {
                string AWBno = "", POU = "", POL = "", ActFLTno = "", Updatedby = "", OffLoadFltNo = "", OffloadLoc = "";
                int OffloadPCS = 0, AVLPCS = 0;
                double OffloadWGT = 0.0, AVLWGT = 0.0;
                DateTime dtOffLoadDate = new DateTime();
                string strMode = string.Empty;
                DateTime dtCurrentDate = (DateTime)Session["IT"];

                ActFLTno = lblFlightNo.Text;
                strMode = Convert.ToString(Session["ManifestMode"]);

                //if (strMode != "R")
                //{
                    if (UpdatePanelRouteDetails.Visible == true)
                    {
                        //int chkPCS = 0;
                        //double chkWGT = 0.0;
                        for (int i = 0; i < grdRouting.Rows.Count; i++)
                        {
                            if (((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim() == "")
                            {
                                Label1.Text = "Please enter valid Origin";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text.Trim() == "")
                            {
                                Label1.Text = "Please enter valid Destination";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim() == ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text.Trim())
                            {
                                Label1.Text = "Please enter valid Origin & Destination";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            string Org = ((TextBox)grdRouting.Rows[0].FindControl("txtFltOrig")).Text.Trim();
                            if (Org != lblAirport.Text)
                            {
                                Label1.Text = "Please enter valid Origin";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtFltDest")).Text.Trim().ToUpper() !=
                                Destination.ToUpper())
                            {
                                Label1.Text = "Please enter valid Destination";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text != ""
                                && ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text.Trim() == "")
                            {
                                Label1.ForeColor = Color.Red;
                                Label1.Text = "Please enter Next Flight Details.";
                                return;
                            }

                            if (((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text == "" &&
                                ((TextBox)grdRouting.Rows[i].FindControl("txtRouteLocation")).Text.Trim() == "")
                            {
                                Label1.ForeColor = Color.Red;
                                Label1.Text = "Please select either Flight # OR enter Location.";
                                return;
                            }
                            //Set flight date to current day if not selected.
                            if (((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text.Trim() == "")
                            {
                                ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text =
                                    Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                            }

                            //if (ddlReason.SelectedValue == "Others" && txtReason.Text == "")
                            //{
                            //    Label1.Text = "Please enter Offload reason";
                            //    Label1.ForeColor = Color.Red;
                            //    return;
                            //}
                        }
                    }
                //}

                //string ULDno = "";
                string AWBPrefix = string.Empty, Location = string.Empty, CartNumber = string.Empty;

                //For Arrival Date 

                string Number = AWBNumber;

                if (Number.Replace("&nbsp;", "") != "")
                {
                    AWBPrefix = Number.Substring(0, Number.IndexOf('-'));
                    AWBno = Number.Substring(Number.IndexOf('-') + 1, 8);
                }
                else
                {
                    AWBPrefix = "";
                    AWBno = "";
                }

                AVLPCS = Pieces;
                AVLWGT = Weight;
                //ULDno = ULDno;
                CartNumber = CartNo;

                DateTime dtActualDate = DateTime.ParseExact(lblFlightDate.Text.Trim(), "dd/MM/yyyy", null);

                bool blnResult = false;

                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {
                    POL = ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text;
                    //POU = ddlMainPOU.SelectedItem.Value.ToString();
                    POU = ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text; ;
                    Location = ((TextBox)grdRouting.Rows[i].FindControl("txtRouteLocation")).Text; ;
                    Updatedby = Convert.ToString(Session["Username"]);

                    OffLoadFltNo = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text;
                    if (OffLoadFltNo == "Select")
                        OffLoadFltNo = "";

                    OffloadLoc = ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text;
                    string strDate = ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text;
                    //Checks Nextflightdate is blank or not
                    //Swapnil--------------------------------------------------------
                    HomeBL objHome = new HomeBL();
                    int RoleId = Convert.ToInt32(Session["RoleID"]);
                    DataSet objDS = objHome.GetUserPermissions(((System.Web.UI.TemplateControl)(Page)).AppRelativeVirtualPath, RoleId);
                    objHome = null;

                    if (OffLoadFltNo == "" || strDate == "")
                        dtOffLoadDate = DateTime.ParseExact(Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                    //dtOffLoadDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
                    else
                        dtOffLoadDate = DateTime.ParseExact(((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text, "dd/MM/yyyy", null);

                    if (strDate.Trim() == "" && Location.Trim() == "")
                    {
                        Label1.ForeColor = Color.Red;
                        Label1.Text = "Please specify the valid location.";
                        return;
                    }

                    if (lblAirport.Text.Trim() == ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim())
                    {
                        OffloadPCS = OffloadPCS + Convert.ToInt32(AVLPCS);
                        OffloadWGT = OffloadWGT + Convert.ToDouble(AVLWGT);
                    }

                    string Reason = "";
                    //if (ddlReason.SelectedItem.Text.Trim() == "Select")
                    //{
                    //    lblStatus.Text = "Please select Reason";
                    //    lblStatus.ForeColor = Color.Red;
                    //    return;
                    //}
                    if (txtReason.Text.Trim() != "")
                    {
                        Reason = txtReason.Text.Trim();
                    }
                    else
                    {
                        Reason = ddlReason.Text;//ddlReason.SelectedValue;
                    }

                    string PartnerCode = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).SelectedValue;
                    string Partnertype = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).SelectedValue;

                    if (PartnerCode == "Other")
                        OffLoadFltNo = ((TextBox)grdRouting.Rows[i].FindControl("txtFlightID")).Text;

                    //Code Added to Check if the AWB is Valid for Customs Messaging
                    object[] QueryCheck = { AWBPrefix + "-" + AWBno, ActFLTno, dtActualDate.ToString("dd/MM/yyyy") };
                    dCust = objCustoms.CheckCustomsAWBAvailability(QueryCheck);

                    blnResult = objExpManifest.OffLoadShipmentinManifestForGHA(ActFLTno, OffLoadFltNo, OffloadLoc, AWBno, AVLPCS, AVLWGT, OffloadPCS, OffloadWGT,
                       Updatedby, POL, POU, "", dtOffLoadDate, Reason, strMode, dtActualDate, dtCurrentDate, PartnerCode, Partnertype, ULDNo,
                       Convert.ToDateTime(Session["IT"]), AWBPrefix, Location, "", CartNumber,"P");

                }
                if (blnResult)
                {
                    //Code Added by Deepak for Customs Messaging
                    try
                    {

                        object[] QueryValues = new object[3];

                        QueryValues[0] = AWBPrefix + "-" + AWBno;
                        QueryValues[1] = ActFLTno;
                        QueryValues[2] = lblFlightDate.Text;//dtOffLoadDate.ToString("dd/MM/yyyy");
                        string[] AWBDetails = AWBno.Split('-');
                        if (dCust != null)
                        {
                            if (dCust.Tables[0].Rows[0]["Validate"].ToString() == "True" && dCust.Tables[1].Rows[0]["Validate"].ToString() == "True")
                            {
                                StringBuilder sb = objCustoms.EncodingFRCMessage(QueryValues);
                                object[] QueryVal = { QueryValues[0], QueryValues[1], QueryValues[2], sb.ToString().ToUpper() };
                                if (objCustoms.UpdateFRCMessageOffload(QueryVal))
                                {
                                    if (sb != null)
                                    {
                                        if (sb.ToString() != "")
                                        {
                                            object[] QueryValMail = { "FRC", ActFLTno, lblFlightDate.Text };
                                            //Getting MailID for FRC Message
                                            DataSet dMail = objCustoms.GetCustomMessagesMailID(QueryValMail);
                                            string MailID = string.Empty;
                                            if (dMail != null)
                                            {
                                                MailID = dMail.Tables[0].Rows[0]["EmailID"].ToString();
                                            }
                                            cls_BL.addMsgToOutBox("FRC", sb.ToString().ToUpper(), "", MailID);
                                        }
                                    }
                                    DataSet dFRX = objCustoms.CheckFRXValidityOffload(QueryValues);
                                    if (dFRX != null)
                                    {
                                        if (dFRX.Tables[0].Rows.Count > 0)
                                        {
                                            if (dFRX.Tables[0].Rows[0]["Validate"].ToString() == "True")
                                            {
                                                StringBuilder sbFRX = objCustoms.EncodingFRXMessage(QueryValues);
                                                object[] QueryValFRX = { QueryValues[0], QueryValues[1], QueryValues[2], sbFRX.ToString().ToUpper() };
                                                if (objCustoms.UpdateFRXMessageOffload(QueryValFRX))
                                                {
                                                    if (sb != null)
                                                    {
                                                        if (sb.ToString() != "")
                                                        {
                                                            object[] QueryValMail = { "FRX", ActFLTno, lblFlightDate.Text };
                                                            //Getting MailID for FRX Message
                                                            DataSet dMail = objCustoms.GetCustomMessagesMailID(QueryValMail);
                                                            string MailID = string.Empty;
                                                            if (dMail != null)
                                                            {
                                                                MailID = dMail.Tables[0].Rows[0]["EmailID"].ToString();
                                                            }
                                                            cls_BL.addMsgToOutBox("FRX", sbFRX.ToString().ToUpper(), "", MailID);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        objCustoms = null;
                        dCust.Dispose();


                    }
                    catch (Exception)
                    { }
                    //pnlGrid.Visible = false;
                    //BtnList_Click(null, null);
                    //btnSave_Click(this, new EventArgs());
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                objExpManifest = null;
            if(dCust!=null)
            {
                dCust.Dispose();
            }
            }
        }
    }
}
