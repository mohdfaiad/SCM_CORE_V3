#region IMPORT CLASSES

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Net.Mail;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
//using Quantumid.Act.mykfcargo.Business;
using System.Collections.Generic;
using System.Data.Common;
using System.Net;
using System.IO;
using System.Globalization;
using QID.DataAccess;
//using clsDataLib;
using BAL;
using System.Drawing;

#endregion IMPORT CLASSES


namespace MyKFCargoNewProj
{
    public partial class FrmTracerGeneration : System.Web.UI.Page
    {
        #region Variables
        MasterBAL objbal = new MasterBAL();
        #endregion

        #region PAGE LOAD EVENT

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                btnTracer.Attributes.Add("onclick", "javascript:OpenCreateTracer('Panel3','" + User.Identity.Name.ToUpper().ToString() + "');return false;");
                btnNewTracer.Attributes.Add("onclick", "javascript:OpenCreateTracer('Panel1','" + User.Identity.Name.ToUpper().ToString() + "');return false;");
                //btnNewTracer.Attributes.Add("onclick", "javascript:OpenCreateTracer('Panel1','" + User.Identity.Name.ToUpper().ToString() + "');return false;");

            }
            catch (Exception ex)
            {

            }
            
            if (!IsPostBack)
            {
                try
                {
                    ddlAWBStatus.SelectedIndex = 0;
                    //DdlOriginList.SelectedIndex = 0;
                    //DdlDestinationList.SelectedIndex = 0;
                    divgen.Visible = false;
                    Cache["attchFile"] = string.Empty;
                    Cache["attchInputStream"] = string.Empty;
                    Cache["Dt"] = string.Empty;
                    Cache["oDsCurr"] = string.Empty;
                    Cache["oDsSort"] = string.Empty;
                    DispStatus(false);
                    DispStatus(true);

                    Panel1.Visible = true;
                    HdnPanel.Value = "Panel1";
                    HdnIsPageLoad.Value = "Y";
                    Timer1.Enabled = true;
                    Session["FileByte"] = null;

                    txtAWBPrefix.Text = Convert.ToString(Session["awbPrefix"]);
                    txtAWBPrefix.Enabled = false;

                    //BindEmptyRow();

                }
                catch (Exception ex)
                {
                    //lblError.Text = "Error:" + ex.Message;
                    //lblError.ForeColor = Color.Red;
                }
            }

        }

        #endregion PAGE LOAD EVENT


        #region BIND DATASET WITH GRIDVIEW FUNCTION

        public void LoadGridView()
        {
            try
            {
                grdViewTracer.DataSource = null;
                #region commented gridview edit hidden value
                //if (hdnEdit.Value != "Y")
                //{
                //    hdnEdit.Value = "N";
                //}
                #endregion commented gridview edit hidden value

                DataSet oDs = new DataSet();
                oDs = LoadDataset();
                if (oDs != null)
                {
                    if (oDs.Tables[0].Rows.Count >= 1)
                    {
                        grdViewTracer.DataSource = null;
                        grdViewTracer.Visible = true;
                        grdViewTracer.DataSource = oDs;
                        grdViewTracer.DataBind();
                        //grdViewTracer.HeaderStyle.Wrap = false;
                        //DispError(string.Empty, false);
                        //grdViewTracer.FooterRow.Cells[1].Text = "GRAND TOTAL:";
                        //grdViewTracer.FooterRow.Cells[2].Text = oDs.Tables[0].Rows.Count.ToString();


                        #region Check if it Super User or Not to Enable/Diable Edit & Delete Buttons in Grid.
                         
                        try
                        {
                            if (Session["RoleName"].ToString() == "Super User")
                            {
                                for (int i = 0; i < grdViewTracer.Rows.Count; i++)
                                {
                                    ImageButton btnEdit = (ImageButton)grdViewTracer.Rows[i].FindControl("btnEditGrd");
                                    ((ImageButton)grdViewTracer.Rows[i].FindControl("btnEditGrd")).Enabled=true;

                                    ImageButton ImgClose = (ImageButton)grdViewTracer.Rows[i].FindControl("btnCloseGrd");
                                    ((ImageButton)grdViewTracer.Rows[i].FindControl("btnCloseGrd")).Enabled = true;
                                }

                            }
                            else
                            {
                                for (int i = 0; i < grdViewTracer.Rows.Count; i++)
                                {
                                    ImageButton btnEdit = (ImageButton)grdViewTracer.Rows[i].FindControl("btnEditGrd");
                                    ((ImageButton)grdViewTracer.Rows[i].FindControl("btnEditGrd")).Enabled = false;

                                    ImageButton ImgClose = (ImageButton)grdViewTracer.Rows[i].FindControl("btnCloseGrd");
                                    ((ImageButton)grdViewTracer.Rows[i].FindControl("btnCloseGrd")).Enabled = false;
                                }
                            }
                        }

                        catch (Exception ex)
                        {
                            lblError.Text = "Error:" + ex.Message;
                            lblError.ForeColor = Color.Red;
                        }
                        #endregion

                                //for (int i = 0; i < grdViewTracer.Rows.Count; i++)
                                //{
                                //    if (Session["RoleName"].ToString() == "Super User")
                                //    {
                                //        ImageButton btnEdit = (ImageButton)grdViewTracer.Rows[i].FindControl("btnEditGrd");
                                //        //btnEdit.Enabled = true;
                                //        ((ImageButton)grdViewTracer.Rows[i].FindControl("btnEditGrd")).Enabled = true;
                                //        ImageButton ImgClose = (ImageButton)grdViewTracer.Rows[i].FindControl("btnCloseGrd");
                                //        //ImgClose.Enabled = true;
                                //        ((ImageButton)grdViewTracer.Rows[i].FindControl("btnCloseGrd")).Enabled=true;
                                //    }
                                //    else
                                //    {
                                //        ImageButton btnEdit = (ImageButton)grdViewTracer.Rows[i].FindControl("btnEditGrd");
                                //        //btnEdit.Enabled = true;
                                //        ((ImageButton)grdViewTracer.Rows[i].FindControl("btnEditGrd")).Enabled = false;
                                //        ImageButton ImgClose = (ImageButton)grdViewTracer.Rows[i].FindControl("btnCloseGrd");
                                //        //ImgClose.Enabled = true;
                                //        ((ImageButton)grdViewTracer.Rows[i].FindControl("btnCloseGrd")).Enabled = false;
                                //    }
                                //}
                    }
                }
                else
                {
                    Timer2.Enabled = false;
                    grdViewTracer.Visible = false;
                    grdViewTracer.DataSource = null;
                    //ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No Record Found');</script>");
                    DispStatus(false);
                    DispError("No Record Found", true);
                }

            }

            catch (Exception ex)
            {
                grdViewTracer.DataSource = null;
                grdViewTracer.Visible = false;
                //ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Error Retrieving Record " + ex.Message + "');</script>");
                DispStatus(false);
                DispError("Error While Retrieving Record " + ex.Message, true);

            }
        }

        #endregion BIND DATASET WITH GRIDVIEW FUNCTION


        #region Display Data on List Button Cilck

        /// <summary>
        /// RETRIEVING DATA FROM CENTRAL DB TABLE TO FILL RETURNING DATASET FOR GRIDVIEW
        /// </summary>

        public DataSet LoadDataset()
        {
            DataSet oDs = new DataSet();
            try
            {
                DateTime dt1;
                DateTime dt2;

                dt1 = DateTime.ParseExact(txtFromDt.Text, "dd/MM/yyyy", null);
                dt2 = DateTime.ParseExact(txtToDt.Text, "dd/MM/yyyy", null);

                //string StrFinalFromDate = dt1.ToString("MM/dd/yyyy");
                //string StrFinalToDate = dt2.ToString("MM/dd/yyyy");

                //string StrFinalFromDate = FDate.Substring(3, 3) + FDate.Substring(0, 3) + FDate.Substring(6, 4) + " 00:00:00";
                //string StrFinalToDate = TDate.Substring(3, 3) + TDate.Substring(0, 3) + TDate.Substring(6, 4) + " 23:59:59";



                string AWBNo = string.Empty;
                string FlightNo = string.Empty;
                string TracerNo = string.Empty;
                string TracerStatus = string.Empty;
                string AWBStatus = string.Empty;
                string ErrMsg = string.Empty;
                string orgCodes = string.Empty;
                string DestCodes = string.Empty;

                GetLocCodes(ref orgCodes, ref DestCodes);
                AWBStatus = ddlAWBStatus.SelectedValue;
                TracerStatus = ddlTracerStatus.SelectedValue;

                //if (HdnIsPageLoad.Value == "Y")
                //{
                //    DestCodes = "'AMD','BLR','BOM','CCU','CJB','DEL','HYD','IDR','LKO','MAA','PNQ'";
                //}

                if (txtAwbno.Text != string.Empty && txtAWBPrefix.Text!=string.Empty)
                {
                    AWBNo = txtAWBPrefix.Text.Trim() +"-"+ txtAwbno.Text.Trim();//HdnNewAWB.Value;
                }
                else
                {
                    if (Panel1.Visible == true)
                    {
                        //if (RdbOtherFilters.SelectedIndex == 1)
                        //{
                        //    AWBNo = txtOtherFilter.Text;
                        //}

                        if (RdbOtherFilters.SelectedIndex == 1)
                        {
                            TracerNo = txtOtherFilter.Text;
                        }

                        if (RdbOtherFilters.SelectedIndex == 2)
                        {
                            FlightNo = txtFltNo.Text;
                        }

                    }
                    else
                    {

                        AWBNo = txtAWBPrefix.Text.Trim() + "-" + txtAwbno.Text.Trim();
                    }
                }


                int iVal = 0;
                string[] DParam = new string[9];
                object[] DValues = new object[9];
                SqlDbType[] DTypes = new SqlDbType[9];

                DParam.SetValue("FromDt", iVal);
                DValues.SetValue(dt1, iVal);
                DTypes.SetValue(SqlDbType.DateTime, iVal);
                iVal++;


                DParam.SetValue("ToDt", iVal);
                DValues.SetValue(dt2, iVal);
                DTypes.SetValue(SqlDbType.DateTime, iVal);
                iVal++;

                DParam.SetValue("AWBNo", iVal);
                DValues.SetValue(AWBNo, iVal);
                DTypes.SetValue(SqlDbType.VarChar, iVal);
                iVal++;

                DParam.SetValue("FltNo", iVal);
                DValues.SetValue(FlightNo, iVal);
                DTypes.SetValue(SqlDbType.VarChar, iVal);
                iVal++;

                DParam.SetValue("TracerNo", iVal);
                DValues.SetValue(TracerNo, iVal);
                DTypes.SetValue(SqlDbType.VarChar, iVal);
                iVal++;

                DParam.SetValue("TracerStatus", iVal);
                DValues.SetValue(TracerStatus, iVal);
                DTypes.SetValue(SqlDbType.VarChar, iVal);
                iVal++;

                DParam.SetValue("AWBStatus", iVal);
                DValues.SetValue(AWBStatus, iVal);
                DTypes.SetValue(SqlDbType.VarChar, iVal);
                iVal++;

                DParam.SetValue("OrgLocation", iVal);
                DValues.SetValue(orgCodes, iVal);
                DTypes.SetValue(SqlDbType.VarChar, iVal);
                iVal++;

                DParam.SetValue("DestLocation", iVal);
                DValues.SetValue(DestCodes, iVal);
                DTypes.SetValue(SqlDbType.VarChar, iVal);
                iVal++;

                //Database ObjData = new Database();
                SQLServer da = new SQLServer(Global.GetConnectionString());

                //oDs = ObjData.SelectRecords("SpGetPendAWBTracer", DParam, DValues, DTypes);
                oDs = da.SelectRecords("SpGetPendAWBTracer", DParam, DValues, DTypes);

                //txtAwbno.Text = string.Empty;
                //txtAWBPrefix.Text = string.Empty;//HdnNewAWB.Value = string.Empty;
                if (oDs != null)
                {
                    if (oDs.Tables[0].Rows.Count < 1)
                    {
                        ViewState["oDsCurr"] = null;
                        oDs = null;
                        lblError.Visible = true;
                        lblError.Text = "No Record Found!!!";
                    }
                    else
                    {

                        ViewState["oDsCurr"] = oDs;
                    }
                }
                else
                {
                    ViewState["oDsCurr"] = null;
                    oDs = null;
                }
            }
            catch (Exception)
            {

                oDs = null;
            }


            return oDs;

        }

        #endregion LOAD DATASET TO FILL GRIDVIEW FUNC


        #region LOAD DATASET TO FILL ORIGIN AND DESTINATION CHECKBOX LIST FUNCTION

        /// <summary>
        /// RETRIEVING DATA FROM CENTRAL DB TABLE TO FILL RETURNING DATASET FOR SOURCE AND DESTINATION
        /// </summary>


        public DataSet LoadALLStationCode()
        {
            DataSet oDs = new DataSet();
            SQLServer ObjData = new SQLServer(Global.GetConnectionString());
            try
            {

                oDs = ObjData.SelectRecords("SpGetAllStationForTracer");

            }
            catch (Exception)
            {

                oDs = null;
            }
            return oDs;
        }

        #endregion LOAD DATASET TO FILL ORIGIN AND DESTINATION CHECKBOX LIST FUNC


        #region BIND DATASET WITH SOURCE AND DESTINATION DROPDOWN_LIST FUNC

        /// <summary>
        /// BIND SOURCE AND DESTINATION CHECKBOX LIST WITH RETURNED DATASET FROM FUNCTION
        /// </summary>

        public void LoadAirportCode()
        {
            DataSet oDs = new DataSet();

            try
            {
                oDs = LoadALLStationCode();
                if (oDs != null)
                {
                    if (oDs.Tables[0].Rows.Count >= 1)
                    {
                        /// BINDING SORCE AIRPORT
                        //ChkSrcList.DataSource = oDs;
                        //ChkSrcList.DataTextField = "AirportCode";
                        //ChkSrcList.DataValueField = "AirportCode";
                        //ChkSrcList.DataBind();
                        //ChkSrcList.Items.Insert(0, "ALL");
                        //ChkSrcList.Items[0].Selected = true;


                        // BIND DATASET WITH Origin AND DESTINATION DROPDOWN BOX LIST FUNC

                        // BINDING ORIGIN AIRPORT
                        DdlOriginList.DataSource = oDs;
                        DdlOriginList.DataTextField ="AirportCode";
                        DdlOriginList.DataValueField = "AirportCode";
                        DdlOriginList.DataBind();
                        DdlOriginList.Items.Insert(0,"All");
                        DdlOriginList.Items[0].Selected = true;

                       // DdlOriginList.SelectedIndex = DdlOriginList.Items.IndexOf(DdlOriginList.Items.FindByText(Session["Station"].ToString()));

                        /// BINDING DESTINATION AIRPORT

                        //ChkDestList.DataSource = oDs;
                        //ChkDestList.DataTextField = "AirportCode";
                        //ChkDestList.DataValueField = "AirportCode";
                        //ChkDestList.DataBind();
                        //ChkDestList.Items.Insert(0, "ALL");
                        //ChkDestList.Items[0].Selected = true;

                        DdlDestinationList.DataSource = oDs;
                        DdlDestinationList.DataTextField = "AirportCode";
                        DdlDestinationList.DataValueField = "AirportCode";
                        DdlDestinationList.DataBind();
                        DdlDestinationList.Items.Insert(0,"All");
                        DdlDestinationList.Items[0].Selected = true;

                    }
                    else
                    {
                        //ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Error Loading Source And Destination Codes...');</script>");
                        DispError("Error In Loading Origin And Destination Codes...", true);
                        //ChkSrcList.Focus();
                        DdlOriginList.Focus();

                    }
                }
                else
                {
                    //ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Error Loading Source And Destination Codes...');</script>");
                    DispError("Error In Loading Origin And Destination Codes......", true);
                    //ChkSrcList.Focus();
                    DdlOriginList.Focus();

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Error In Loading Origin And Destination Codes..." + ex.Message + "');</script>");
                DispError("Error In Loading Origin And Destination Codes..." + ex.Message, true);
                //ChkSrcList.Focus();
                DdlOriginList.Focus();

            }

        }

        #endregion BIND DATASET WITH SOURCE AND DESTINATION DROPDOWN_LIST FUNC




        #region GRIDVIEW PAGE INDEX CHANGE EVENT

        /// <summary>
        /// ON GRIDVIEW NEXT PAGE NO CLICK EVENT IS RAISED TO NAVIGATE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void grdViewTracer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdViewTracer.PageIndex = e.NewPageIndex;
                LoadGridView();
            }
            catch (Exception)
            {
                LoadGridView();
            }

        }

        #endregion GRIDVIEW PAGE INDEX CHANGE EVENT


        #region GRIDVIEW ON ROW DATA BOUND EVENT

        /// <summary>
        /// ON GRIDVIEW DATABOUND PRE BIND JAVASCRIPT FUNCTIONS AND CHANGE TRACER IMAGE IF IT IS GENERATED
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void grdViewTracer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                #region commented gridview edit hidden value

                //hdnEdit.Value = "N";

                #endregion commented gridview edit hidden value

                string ErrMsg = String.Empty;
                if ((e.Row.RowType == DataControlRowType.DataRow))
                {
                    foreach (TableCell tc in e.Row.Cells)
                    {
                        tc.Attributes["style"] = "border-color:#990000";
                    }

                    DateTime dt1;
                    DateTime dt2;

                    dt1 = DateTime.ParseExact(txtFromDt.Text, "dd/MM/yyyy", null);
                    dt2 = DateTime.ParseExact(txtToDt.Text, "dd/MM/yyyy", null);

                    //string FDate = txtFromDt.Text;
                    //string TDate = txtToDt.Text;
                    //string StrFinalFromDate = FDate.Substring(0, 3) + FDate.Substring(3, 3) + FDate.Substring(6, 4) + " 00:00:00";
                    //string StrFinalToDate = TDate.Substring(0, 3) + TDate.Substring(3, 3) + TDate.Substring(6, 4) + " 23:59:59";

                    //string StrFinalFromDate = FDate.Substring(3, 3) + FDate.Substring(0, 3) + FDate.Substring(6, 4) + " 00:00:00";
                    //string StrFinalToDate = TDate.Substring(3, 3) + TDate.Substring(0, 3) + TDate.Substring(6, 4) + " 23:59:59";


                    Label txtAWBNo = (Label)e.Row.FindControl("lblAWBNo");
                    Label tFlightNo = (Label)e.Row.FindControl("lblFlightNo");
                    Label txtTracerNo = (Label)e.Row.FindControl("lblTracerNoV");
                    Label txtMsdPcs = (Label)e.Row.FindControl("lblMissedPcs");
                    Label txtFndPcs = (Label)e.Row.FindControl("lblFoundPcs");
                    string txtFndType = "Found";
                    Label txtFndLoc = (Label)e.Row.FindControl("lblFoundLoc");
                    ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnEditGrd");
                    LinkButton btnTracerNo = (LinkButton)e.Row.FindControl("lnkTracerNo");
                    LinkButton btnAWBNo = (LinkButton)e.Row.FindControl("lnkAWBNo");
                    LinkButton btnFlightNo = (LinkButton)e.Row.FindControl("lnkFlightNo");
                    string btnPnlVis = string.Empty;
                    if (Panel1.Visible == true)
                    {
                        btnPnlVis = btnRefresh.ClientID;
                    }
                    else if (Panel3.Visible == true)
                    {
                        btnPnlVis = btnRefresh.ClientID;
                    }
                    if (txtAWBNo != null && btnEdit != null)
                    {
                        btnEdit.Attributes.Add("onclick", "javascript:OpenTracerFoundADD('" + txtAWBNo.Text
                            + "','" + btnTracerNo.Text + "','" + txtMsdPcs.Text + "','" + txtFndPcs.Text + "','" + txtFndType + "','" + txtFndLoc.Text + "','" + btnPnlVis + "','" + User.Identity.Name.ToUpper().ToString() + "');return false;");
                    }
                    if (txtAWBNo != null && btnTracerNo != null)
                    {
                        btnTracerNo.Attributes.Add("onclick", "javascript:OpenTracerHistoryPopup('" + txtAWBNo.Text
                            + "','" + btnTracerNo.Text + "');return false;");

                        btnAWBNo.Attributes.Add("onclick", "javascript:OpenAWBTrackingPopup('" + txtAWBNo.Text + "');return false;");

                        btnFlightNo.Attributes.Add("onclick", "javascript:OpenCargoManifestPopup('" + tFlightNo.Text
                           + "','" + dt1 + "','" + dt2 + "');return false;");
                    }
                }

                #region commented gridview edit hidden value

                //if (hdnEdit.Value == "N")
                //{
                #endregion commented gridview edit hidden value

                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    Label lblGenTr = (e.Row.FindControl("lblIsGenTr") as Label);
                    Label lblCloseTr = (e.Row.FindControl("lblIsClosedTr") as Label);

                    if (lblGenTr.Text.ToUpper() == "TRUE")
                    {
                        ImageButton hl = (e.Row.FindControl("btnCaller") as ImageButton);
                        hl.ImageUrl = "~/images/stop-icon.png";
                        //hl.Width = 28;
                    }
                    if (lblCloseTr.Text.ToUpper() == "TRUE")
                    {
                        ImageButton ImgClose = (e.Row.FindControl("btnCloseGrd") as ImageButton);
                        ImgClose.ImageUrl = "~/images/fileclose.png";
                        ImgClose.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Error On Data Bound..." + ex.Message + "');</script>");
            }

        }

        #endregion GRIDVIEW ON ROW DATA BOUND EVENT


        #region GRIDVIEW ON ROW CREATED EVENT

        /// <summary>
        /// ON GRIDVIEW ROW CREATION HIDE FEW ROWS WHICH REQUIRED FOR BACK PROCESS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void grdViewTracer_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[11].CssClass = "hiddencol1";
                e.Row.Cells[16].CssClass = "hiddencol1";
                e.Row.Cells[17].CssClass = "hiddencol1";
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[11].CssClass = "hiddencol1";
                e.Row.Cells[16].CssClass = "hiddencol1";
                e.Row.Cells[17].CssClass = "hiddencol1";
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[11].CssClass = "hiddencol1";
                e.Row.Cells[16].CssClass = "hiddencol1";
                e.Row.Cells[17].CssClass = "hiddencol1";
            }
        }

        #endregion GRIDVIEW ON ROW CREATED EVENT


        #region Button List

        protected void BtnSearch_Click(object sender, EventArgs e)
        {

            try
            {
                DispError(string.Empty, false);
                //if (txtAwbno.Text == string.Empty)//HdnNewAWB.Value
                //{
                string errMsg = string.Empty;
                if (Panel1.Visible == true)
                {
                    if (RdbOtherFilters.SelectedIndex != 0)
                    {
                        if (RdbOtherFilters.SelectedIndex == 1)
                        {
                            if (txtOtherFilter.Text == string.Empty)
                            {
                                Timer2.Enabled = false;
                                DispStatus(false);
                                DispError("Tracer No Field Cannot Be Empty", true);
                                //ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('AWB No Field Cannot Be Empty...');</script>");
                                txtOtherFilter.Focus();
                                return;
                            }
                        }
                        else if (RdbOtherFilters.SelectedIndex == 2)
                        {
                            if (txtFltNo.Text == string.Empty)
                            {
                                Timer2.Enabled = false;
                                DispStatus(false);
                                DispError("Flight No Field Cannot Be Empty", true);
                                //ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Tracer No Field Cannot Be Empty...');</script>");
                                txtFltNo.Focus();
                                return;
                            }
                        }


                        bool chkVal = false;
                        if (hdnFltVal.Value == string.Empty)
                        {
                            chkVal = ValidateFilters(ref errMsg);
                        }
                        if (chkVal == true)
                        {
                            Timer2.Enabled = false;
                            DispStatus(false);
                            DispError(errMsg, true);
                            //ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + errMsg + "');</script>");
                            return;
                        }
                    }
                }

                else if (Panel3.Visible == true)
                {
                    if (txtAwbno.Text != string.Empty && txtAWBPrefix.Text!=string.Empty)
                    {
                        bool chkVal = false;
                        chkVal = ValidateFilters(ref errMsg);

                        if (chkVal == true)
                        {
                            Timer2.Enabled = false;
                            DispStatus(false);
                            //ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + errMsg + "');</script>");
                            DispError(errMsg, true);
                            return;
                        }
                    }
                }
                //}
                DispStatus(true);
                DispError("", false);
                //grdViewTracer.DataSource = null;
                grdViewTracer.Visible = false;
                HdnIsPageLoad.Value = "N";


                //Timer1.Interval = 1000;
                Timer2.Interval = 10;
                Timer2.Enabled = true;

            }
            catch (Exception ex)
            {
                Timer2.Enabled = false;
                DispStatus(false);
                DispError("Error While Retriving Records..." + ex.Message, true);
                //ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Error While Retriving Records" + ex.Message + "');</script>");
                return;
            }
        }

        #endregion 


        #region ON EXCEL IMAGE BUTTON CLICK EVENT

        /// <summary>
        /// ON EXCEL IMAGE BUTTON CLICK GENERATE AND OPEN EXCEL REPORT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        //protected void btnExcel_Click(object sender, ImageClickEventArgs e)
        //{
          protected void BtnExcel1_Click(object sender, EventArgs e)
        {

          
          DispError(string.Empty, false);
            try
            {
                HdnIsPageLoad.Value = "N";
                #region commented gridview edit hidden value

                //hdnEdit.Value = "N";

                #endregion commented gridview edit hidden value

                DataSet oDs = new DataSet();
                oDs = LoadDataset();
                if (oDs != null)
                {
                    if (oDs.Tables[0].Rows.Count >= 1)
                    {
                        Cache["TracerDs"] = oDs;
                        //oDs.WriteXml(Server.MapPath("~/XMLTracer.xml"));
                        Response.Redirect("~/FrmTracerExcel.aspx");//?TrcDs=" + (DataSet)Cache["TracerDs"]);
                    }
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Error Loading Excel" + ex.Message + "');</script>");

            }


        }

        #endregion ON EXCEL IMAGE BUTTON CLICK EVENT


        #region ON PDF IMAGE BUTTON CLICK EVENT

        /// <summary>
        /// ON PDF IMAGE BUTTON CLICK GENERATE AND OPEN PDF REPORT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void btnPDF_Click(object sender, ImageClickEventArgs e)
        {
            DataSet oDs = new DataSet();
            try
            {
                HdnIsPageLoad.Value = "N";

                oDs = LoadDataset();
                if (oDs != null)
                {
                    if (oDs.Tables[0].Rows.Count >= 1)
                    {

                        try
                        {
                            /// Export the Report to Response stream in PDF format and file name Customers

                            /// There are other format options available such as Word, Excel, CVS, and HTML in the ExportFormatType Enum given by crystal reports
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            ex = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Error Loading PDF" + ex.Message + "');</script>");
            }
        }

        #endregion ON PDF IMAGE BUTTON CLICK EVENT


        #region CommentedPrinter USED FOR PRINTING GRIDIVIEW
        //protected void btnPrint_Click(object sender, ImageClickEventArgs e)
        //{

        //    PrintHelper.PrintWebControl(grdViewTracer);

        //}
        //protected void btnPrint_Click1(object sender, ImageClickEventArgs e)
        //{
        //    PrintHelper.PrintWebControl(grdViewTracer);
        //}
        #endregion CommentedPrinter


        #region GENERATE TRACER - IMAGE BUTTON (GRIDVIEW) CLICK EVENT

        
        protected void grdViewTracer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                DispError("", false);
                Timer1.Enabled = false;
                Timer2.Enabled = false;
                int index = 0;
                
                if (e.CommandName == "GenTracer")
                {
                  
                    // gneratetrace.Visible = true;
                    index = Convert.ToInt32(e.CommandArgument);
                    hdnFltVal.Value = string.Empty;
                    GridViewRow row = grdViewTracer.Rows[index];
                    //Panel2.Visible = true;
                    //divgen.Visible = true;
                    //lblhead.Visible = false;
                    hdnTracerNo.Value = ((LinkButton)grdViewTracer.Rows[index].FindControl("lnkTracerNo")).Text;
                    hdnWeight.Value = ((Label)grdViewTracer.Rows[index].FindControl("lblChargeableWgt")).Text;
                    txtAwbNoTr.Text = ((Label)grdViewTracer.Rows[index].FindControl("lblAWBNo")).Text;
                    DataSet ds4 = new DataSet();
                    string AgentName = string.Empty;
                    ds4 = GetRecords(txtAwbNoTr.Text, "Agent");
                    if (ds4 != null)
                    {
                        if (ds4.Tables[0].Rows.Count > 0)
                        {
                            AgentName = ds4.Tables[0].Rows[0][0].ToString();
                            AgentName = AgentName + " ";
                        }
                    }
                    string StrDate = string.Empty;


                    txtOrg.Text = ((Label)grdViewTracer.Rows[index].FindControl("lblOrg")).Text;
                    txtDest.Text = ((Label)grdViewTracer.Rows[index].FindControl("lblDest")).Text;
                    txtFlightNo.Text = ((Label)grdViewTracer.Rows[index].FindControl("lblFlightNo")).Text;
                    txtShortage.Text = ((Label)grdViewTracer.Rows[index].FindControl("lblMissedPcs")).Text;
                    txtTotalPcs.Text = ((Label)grdViewTracer.Rows[index].FindControl("lblSentPcs")).Text;
                    StrDate = ((Label)grdViewTracer.Rows[index].FindControl("lblAwbDate")).Text;
                    //DtDate = Convert.ToDateTime(StrDate);
                    txtWgt.Text = ((Label)grdViewTracer.Rows[index].FindControl("lblChargeableWgt")).Text;

                    txtDate.Text = StrDate;
                    txtOrgAgent.Text = AgentName + txtOrg.Text;
                    txtDestAgent.Text = AgentName + txtDest.Text;
                    //grdViewTracer.Visible = false;

                    string TracerNo = ((LinkButton)grdViewTracer.Rows[index].FindControl("lnkTracerNo")).Text;
                    //if (Panel1.Visible == true)
                    //{
                    //    HdnPanel.Value = "Panel1";
                    //}
                    //if (Panel3.Visible == true)
                    //{
                    //    HdnPanel.Value = "Panel3";
                    //}
                    //Panel1.Visible = false;
                    //Panel3.Visible = false;
                    //divgen.Visible = true;
                   
                    //HiddenField1.Value = "&FltNo=" + txtFlightNo.Text + "&FltDt=" + txtDate.Text + "&Origin=" + txtOrg.Text + "&Dest=" + txtDest.Text + "&AWBNo=" + txtAwbNoTr.Text + "&TotalPCs=" + txtTotalPcs.Text + "&Weight=" + txtWgt.Text + "&Shortage=" + txtShortage.Text + "&Consigner=" + txtOrgAgent.Text + "&Consignee=" + txtDestAgent.Text;
                   
                    //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl", "doShowPanel1();", true);

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>OpenCreateTracer('Panel1','" + User.Identity.Name.ToUpper().ToString() + "','" + TracerNo.ToString()+ "');</script>", false);

                    #region Commented
                    //            Button callPopup = (row.FindControl("lbtnGenTracer") as Button);
                    //            string btnName = callPopup.ClientID.ToString();

                    //            string script = @"<script>
                    //                    function ConfirmClick() {
                    //                        
                    //                        document.getElementById('" + btnName + @"').click(); 
                    //                    }
                    //                    
                    //                    </script>";
                    //            if (!Page.ClientScript.IsStartupScriptRegistered("Mast"))
                    //            {
                    //                Page.ClientScript.RegisterStartupScript(this.GetType(),
                    //                    "Mast", script);
                    //            }

                    #endregion Commented
                }
                if (e.CommandName == "CloseTracer")
                {
                    string[] QueryPname = new string[9];
                    object[] QueryValue = new object[9];
                    SqlDbType[] QueryType = new SqlDbType[9];

                    try
                    {
                        index = Convert.ToInt32(e.CommandArgument);

                        int iMissedPcs = Convert.ToInt32(((Label)grdViewTracer.Rows[index].FindControl("lblMissedPcs")).Text);
                        int iLostPcs = Convert.ToInt32(((Label)grdViewTracer.Rows[index].FindControl("lblLostPcs")).Text);
                        if (iMissedPcs != iLostPcs)
                        {
                            if (iMissedPcs > 0)
                            {
                                DispError("Unable To Close Tracer As " + iMissedPcs + " Pcs Still Missing", true);
                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Cannot Close Tracer. As " + iMissedPcs + " Pcs Still Missing ');</SCRIPT>");
                                return;
                            }
                        }

                        hdnTracerNo.Value = ((LinkButton)grdViewTracer.Rows[index].FindControl("lnkTracerNo")).Text;
                        hdnWeight.Value = ((Label)grdViewTracer.Rows[index].FindControl("lblChargeableWgt")).Text;
                        txtOrg.Text = ((Label)grdViewTracer.Rows[index].FindControl("lblOrg")).Text;
                        txtDest.Text = ((Label)grdViewTracer.Rows[index].FindControl("lblDest")).Text;
                        txtTotalPcs.Text = ((Label)grdViewTracer.Rows[index].FindControl("lblSentPcs")).Text;
                        txtWgt.Text = ((Label)grdViewTracer.Rows[index].FindControl("lblChargeableWgt")).Text;

                        string TracerNo = ((LinkButton)grdViewTracer.Rows[index].FindControl("lnkTracerNo")).Text;
                        string AwbNo = ((Label)grdViewTracer.Rows[index].FindControl("lblAWBNo")).Text;

                        string Org = ((Label)grdViewTracer.Rows[index].FindControl("lblOrg")).Text;
                        string Dest = ((Label)grdViewTracer.Rows[index].FindControl("lblDest")).Text;
                        string FlightNo = ((Label)grdViewTracer.Rows[index].FindControl("lblFlightNo")).Text;
                        string StrAwbDate = ((Label)grdViewTracer.Rows[index].FindControl("lblAwbDate")).Text;
                        txtDate.Text = StrAwbDate;

                        //string StrFinalAwbDate = StrAwbDate.Substring(0, 3) + StrAwbDate.Substring(3, 3) + StrAwbDate.Substring(6, 4);
                        string StrFinalAwbDate = StrAwbDate.Substring(3, 3) + StrAwbDate.Substring(0, 3) + StrAwbDate.Substring(6, 4);
                        DateTime dt = DateTime.Parse(StrFinalAwbDate);

                        QueryPname[0] = "AWBNo";
                        QueryPname[1] = "TracerNo";
                        QueryPname[2] = "AWBDate";
                        QueryPname[3] = "Org";
                        QueryPname[4] = "Dest";
                        QueryPname[5] = "FltNo";
                        QueryPname[6] = "ReGenDate";
                        QueryPname[7] = "Remark";
                        QueryPname[8] = "MissedPcs";

                        QueryType[0] = SqlDbType.VarChar;
                        QueryType[1] = SqlDbType.BigInt;
                        QueryType[2] = SqlDbType.DateTime;
                        QueryType[3] = SqlDbType.VarChar;
                        QueryType[4] = SqlDbType.VarChar;
                        QueryType[5] = SqlDbType.VarChar;
                        QueryType[6] = SqlDbType.DateTime;
                        QueryType[7] = SqlDbType.VarChar;
                        QueryType[8] = SqlDbType.Int;

                        QueryValue.SetValue(AwbNo, 0);
                        QueryValue.SetValue(Convert.ToInt64(TracerNo), 1);
                        QueryValue.SetValue(dt, 2);
                        QueryValue.SetValue(Org, 3);
                        QueryValue.SetValue(Dest, 4);
                        QueryValue.SetValue(FlightNo, 5);
                        QueryValue.SetValue(Convert.ToDateTime(Session["IT"].ToString()), 6);
                        QueryValue.SetValue("Tracer Closed ", 7);
                        QueryValue.SetValue(0, 8);
                        SQLServer ObjData = new SQLServer(Global.GetConnectionString());
                        bool bFlag = ObjData.InsertData("spTracerUpdateIsClosed", QueryPname, QueryType, QueryValue);
                        if (bFlag == true)
                        {
                            DispError("Tracer Closed Successfully...", true);
                            //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Tracer Closed Successfully...');</SCRIPT>");
                            #region SEND TRACER EMAIL IF RECORDS UPDATED SUCCESSFULLY

                            int RetVal = SendEmail(AwbNo, "Closed", "", 0);

                            if (RetVal == 1)
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Tracer Closed,Email Sent Sucessfully');</SCRIPT>");
                                grdViewTracer.Visible = true;
                                //div1.Visible = true;
                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Tracer Closed Successfully.Closed Tracer EMail Sending Failed');</SCRIPT>");
                            }

                            #endregion SEND TRACER EMAIL IF RECORDS UPDATED SUCCESSFULLY
                            //LoadGridView();
                        }
                        else
                        {
                            //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Error Closing Tracer. Please Try Again....');</SCRIPT>");
                            DispError("Error Closing Tracer. Please Try Again....", true);
                            //LoadGridView();
                        }
                        txtAwbNoTr.Text = string.Empty;
                        txtOrg.Text = string.Empty;
                        txtDest.Text = string.Empty;
                        txtFlightNo.Text = string.Empty;
                        txtShortage.Text = string.Empty;
                        txtTotalPcs.Text = string.Empty;
                        txtDate.Text = string.Empty;
                        txtPkng.Text = string.Empty;
                        txtContents.Text = string.Empty;
                        txtOrgAgent.Text = string.Empty;
                        txtDestAgent.Text = string.Empty;
                        txtRemarks.Text = string.Empty;
                    }
                    catch (Exception)
                    {

                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Error Closing Tracer...'" + ex.Message + ");</SCRIPT>");
                        DispError("Error Closing Tracer. Please Try Again....", true);
                        txtAwbNoTr.Text = string.Empty;
                        txtOrg.Text = string.Empty;
                        txtDest.Text = string.Empty;
                        txtFlightNo.Text = string.Empty;
                        txtShortage.Text = string.Empty;
                        txtTotalPcs.Text = string.Empty;
                        txtDate.Text = string.Empty;
                        txtPkng.Text = string.Empty;
                        txtContents.Text = string.Empty;
                        txtOrgAgent.Text = string.Empty;
                        txtRemarks.Text = string.Empty;
                        txtDestAgent.Text = string.Empty;
                    }
                    LoadGridView();
                }
            }
            catch (Exception)
            {
                txtAwbNoTr.Text = string.Empty;
                txtOrg.Text = string.Empty;
                txtDest.Text = string.Empty;
                txtFlightNo.Text = string.Empty;
                txtShortage.Text = string.Empty;
                txtTotalPcs.Text = string.Empty;
                txtDate.Text = string.Empty;
                txtPkng.Text = string.Empty;
                txtContents.Text = string.Empty;
                txtOrgAgent.Text = string.Empty;
                txtDestAgent.Text = string.Empty;
                txtRemarks.Text = string.Empty;
                divgen.Visible = false;

                //ClientScript.RegisterStartupScript(this.GetType(), "HidePnl", "doHidePanel2();", true);
                grdViewTracer.Visible = true;
                //div1.Visible = true;
            }

        }

        #endregion GENERATE TRACER IMAGE BUTTON (GRIDVIEW) CLICK EVENT


        #region Button Submit - Panel "gneratetrace"

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DispError(string.Empty, false);
            if (Convert.ToInt32(txtShortage.Text) > Convert.ToInt32(txtTotalPcs.Text))
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Error Shortage cannot be more then Total Sent Pcs');</SCRIPT>");
                DispError("Shortage cannot be more then Total Sent Pcs", true);
                txtShortage.Text = string.Empty;
                txtShortage.Focus();
                return;
            }
            HdnIsPageLoad.Value = "N";

            try
            {
                //updMainPnl.Update();

                if (hdnFltVal.Value == string.Empty)
                {
                    string errMsg = string.Empty;

                    int retVal = CheckFiltersNoExists("FLIGHT", txtFlightNo.Text, ref errMsg);
                    if (retVal != 1)
                    {
                        //ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + errMsg + "');</script>");
                        DispError(errMsg, true);
                        txtFlightNo.Focus();
                        hdnFltVal.Value = string.Empty;
                        return;
                    }
                }

                //SqlConnection sqCon;
                //SqlCommand sqCmd;
                //string ConnectionString;

                //sqCmd = new SqlCommand();
                //sqCon = new SqlConnection();
                //ConnectionString = ConfigurationManager.ConnectionStrings["ConStr"].ToString();

                //sqCmd.Connection = sqCon;
                //sqCon.ConnectionString = ConnectionString;
                //sqCmd.CommandType = CommandType.StoredProcedure;
                //SqlParameter sqlPrs = new SqlParameter();

                string[] QueryPname = new string[9];
                object[] QueryValue = new object[9];
                SqlDbType[] QueryType = new SqlDbType[9];

                QueryPname[0] = "AWBNo";
                QueryPname[1] = "TracerNo";
                QueryPname[2] = "AWBDate";
                QueryPname[3] = "Org";
                QueryPname[4] = "Dest";
                QueryPname[5] = "FltNo";
                QueryPname[6] = "ReGenDate";
                QueryPname[7] = "Remark";
                QueryPname[8] = "MissedPcs";

                QueryType[0] = SqlDbType.VarChar;
                QueryType[1] = SqlDbType.BigInt;
                QueryType[2] = SqlDbType.DateTime;
                QueryType[3] = SqlDbType.VarChar;
                QueryType[4] = SqlDbType.VarChar;
                QueryType[5] = SqlDbType.VarChar;
                QueryType[6] = SqlDbType.DateTime;
                QueryType[7] = SqlDbType.VarChar;
                QueryType[8] = SqlDbType.Int;

                string StrAwbDate = txtDate.Text;
                string StrFinalAwbDate = StrAwbDate.Substring(3, 3) + StrAwbDate.Substring(0, 3) + StrAwbDate.Substring(6, 4);
                //string StrFinalAwbDate = StrAwbDate.Substring(0, 3) + StrAwbDate.Substring(3, 3) + StrAwbDate.Substring(6, 4);
                DateTime dt = DateTime.Parse(StrFinalAwbDate);
                ////IFormatProvider culture = new CultureInfo("en-EN", true); // use your culture info
                ////DateTime dt = DateTime.ParseExact(StrAwbDate, "MM/dd/yyyy", DateTimeStyles.NoCurrentDateDefault); 

                //for (int i = 0; i < 9; i++)
                //{
                //    sqCmd.Parameters.Add(QueryPname[i], QueryType[i]);
                //}


                QueryValue.SetValue(txtAwbNoTr.Text, 0);
                QueryValue.SetValue(Convert.ToInt64(hdnTracerNo.Value), 1);
                QueryValue.SetValue(dt, 2);
                QueryValue.SetValue(txtOrg.Text, 3);
                QueryValue.SetValue(txtDest.Text, 4);
                QueryValue.SetValue(txtFlightNo.Text, 5);
                QueryValue.SetValue(Convert.ToDateTime(Session["IT"].ToString()), 6);
                QueryValue.SetValue(txtRemarks.Text, 7);
                QueryValue.SetValue(Convert.ToInt32(txtShortage.Text), 8);



                //sqCmd.CommandText = "spCCTracerUpdateIsGenerated";
                //sqCmd.CommandType = CommandType.StoredProcedure;
                //sqCon.Open();
                //int result = sqCmd.ExecuteNonQuery();
                SQLServer ObjData = new SQLServer(Global.GetConnectionString());

                bool bFlag = ObjData.InsertData("spTracerUpdateIsGenerated", QueryPname, QueryType, QueryValue);

                if (bFlag != false)
                {
                    #region SEND TRACER EMAIL IF RECORDS UPDATED SUCCESSFULLY

                    int RetVal = SendEmail(txtAwbNoTr.Text, "Short", "", 0);
                    if (RetVal == 1)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Tracer sent Sucessfully');</SCRIPT>");
                        DispError("Tracer sent Sucessfully", true);
                        grdViewTracer.Visible = true;
                    }
                    else
                    {
                        DispError("Tracer sent Failed", true);
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Tracer sent Failed');</SCRIPT>");
                    }

                    #endregion SEND TRACER EMAIL IF RECORDS UPDATED SUCCESSFULLY
                }
                else
                {
                    DispError("Tracer sent Failed", true);
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Tracer sent Failed');</SCRIPT>");
                }


            }
            catch (Exception)
            {
                DispError("Tracer sent Failed", true);
                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Tracer sent Failed" + ex.Message + "');</SCRIPT>");
            }



            EmptyTracerControls();
            Cache["attchFile"] = string.Empty;
            Cache["attchInputStream"] = string.Empty;
            Cache["Dt"] = string.Empty;
            grdCurrArchived.DataSource = null;
            grdCurrArchived.Dispose();
            grdCurrArchived.Visible = false;
            if (HdnPanel.Value == "Panel1")
            {
                Panel1.Visible = true;
            }
            else
            {
                Panel3.Visible = true;
            }
            LoadGridView();
        }

        #endregion 


        #region ON GENERATE TRACER BACK BUTTON CLICK EVENT

        /// <summary>
        /// ON CLICK OF BACK BUTTON IN GENERATE AND SEND TRACER FRAME 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void btnBack_Click(object sender, EventArgs e)
        {
            DispError(string.Empty, false);
            EmptyTracerControls();
        }

        #endregion ON GENERATE TRACER BACK BUTTON CLICK EVENT


        #region RETRIVING DATASET FOR AGENTNAME/EMAILID FUNCTION

        /// <summary>
        /// RETRIEVING DATA FOR EITHER AGENT NAME IN TRACER GENERATION OR LIST OF EMAIL ID's
        /// </summary>
        /// <param name="AwbNo"></param>
        /// <param name="GetType"></param>
        /// <returns></returns>

        public DataSet GetRecords(string AwbNo, string GetType)
        {
            DataSet oDs = new DataSet();
            try
            {
                string spName = string.Empty;

                SQLServer ObjData = new SQLServer(Global.GetConnectionString());
                string[] DParam = new string[1];
                object[] DValues = new object[1];
                SqlDbType[] DTypes = new SqlDbType[1];

                DParam.SetValue("AWBNo", 0);
                DValues.SetValue(AwbNo, 0);
                DTypes.SetValue(SqlDbType.VarChar, 0);

                if (GetType == "Email")
                {
                    spName = "spGetTracerEmailID";
                }
                else
                {
                    spName = "spTracerGetAgent";
                }
                oDs = ObjData.SelectRecords(spName, DParam, DValues, DTypes);

                if (oDs != null)
                {
                    if (oDs.Tables[0].Rows.Count <= 0)
                    {
                        oDs = null;
                    }
                }
                else
                {
                    oDs = null;
                }


            }
            catch (Exception)
            {
                oDs = null;

            }
            return oDs;
        }

        #endregion RETRIVING DATASET FOR AGENTNAME/EMAILID FUNCTION


        # region SendEmail

        /// <summary>
        /// SENDING TRACER MAIL ON GENERATE TRACER BUTTON CLICK
        /// </summary>
        /// <param name="dsAwbNo"></param>
        /// <param name="mailType"></param>
        /// <param name="FoundLoc"></param>
        /// <param name="FoundCount"></param>
        /// <returns>INTEGER IN AS 1 SENT AND 0 NOT SENT</returns>

        private int SendEmail(string dsAwbNo, string mailType, string FoundLoc, Int32 FoundCount)
        {
            string SavedFileName = string.Empty;
            string strFileName = string.Empty;

            SmtpClient Smtp = new SmtpClient("smtpout.secureserver.net", 80);
            int RetVal = 1;

            try
            {

                string sQry = string.Empty;
                string ToEmailID = string.Empty;
                int cnt = 1;

                #region  LIVE EMAIL ID's 

                DataSet ds4 = new DataSet();
                ds4 = GetRecords(dsAwbNo, "Email");
                try
                {
                    if (ds4 != null)
                    {
                        if (ds4.Tables[0].Rows.Count >= 1)
                        {
                            cnt = 1;
                            for (int i = 0; i < ds4.Tables[0].Rows.Count; i++)
                            {
                                ToEmailID = ToEmailID + ds4.Tables[0].Rows[i][0].ToString() + ",";
                            }
                        }
                        else
                        {
                            cnt = 0;
                        }

                    }
                    else
                    {
                        cnt = 0;
                    }
                }
                catch (Exception)
                {
                    cnt = 0;
                }
                #endregion LIVE EMAIL ID's

                string FromEmailID = ConfigurationManager.AppSettings["FromEmailID"].ToString();
                string EmailPwd = ConfigurationManager.AppSettings["Pass"].ToString();

                // Get Client Name as per Client
                string client = objbal.clientName();

                if (cnt == 1)
                {

                    MailMessage Mail = new MailMessage();
                    Mail.From = new MailAddress(FromEmailID);

                    //ToEmailID = ToEmailID.Replace(",,", ",");
                    //Mail.To.Add(ToEmailID.Trim(','));

                    #region Replace commas from Emil id

                    string[] ArrToEmailID = ToEmailID.Split(',');

                    if (ArrToEmailID.Length > 0)
                    {
                        ToEmailID = "";
                        for (int i = 0; i < ArrToEmailID.Length; i++)
                        {
                            if (ArrToEmailID[i].Trim() != "")
                            {
                                ToEmailID += ArrToEmailID[i].Trim() + ",";
                            }
                        }
                        if (ToEmailID.Trim() != "")
                        {
                            if (ToEmailID.Substring(ToEmailID.Length - 1, 1) == ",")
                            {
                                ToEmailID = ToEmailID.Substring(0, ToEmailID.Length - 1);
                            }
                        }

                    }

                    Mail.To.Add(ToEmailID.Trim(','));

                    #endregion

                    string body = string.Empty;
                    string subject = string.Empty;
                    

                    if (mailType == "Short")
                    {
                        
                        //subject = client +" " + "TRACER:"+ " " + hdnTracerNo.Value + ": Shortage Detected on " + txtFlightNo.Text + " Source:" + txtOrg.Text + "  AWB No is " + txtAwbNoTr.Text + " and short pcs   " + txtShortage.Text;

                        // updated on 22 Sept 2014 for [JIRA] (AC-67) 
                        subject = "TRACER:" + " " + hdnTracerNo.Value + ": Shortage Detected on " + txtFlightNo.Text + " Source:" + txtOrg.Text + "  AWB No is " + txtAwbNoTr.Text + " and short pcs   " + txtShortage.Text;

                        Mail.Subject = subject;

                        Mail.IsBodyHtml = true;

                        #region HTML Msg Body
                        //body = "Dear All,<br/><br/> ";
                        //body += "This is to inform you that there is Shortage Detected.<br/><br/> ";
                        //body += "Details are as follows.<br/><br/>";

                        //body += "<table border = 1>";
                        //body += "<tr><td>FLIGHT NO</td> <td> " + txtFlightNo.Text + "</td></tr>";
                        //body += "<tr><td>FLIGHT DATE(AWB DATE)</td> <td> " + txtDate.Text + "</td></tr>";
                        //body += "<tr><td>ORGIN-DEST</td> <td> " + txtOrg.Text + " - " + txtDest.Text + "  </td></tr>";
                        //body += "<tr><td>AIRWAY BILL NO</td> <td> " + dsAwbNo + "</td></tr>";
                        //body += "<tr><td>PCS/WT</td> <td> " + txtTotalPcs.Text + "pcs/" + hdnWeight.Value + "kgs" + "</td></tr>";
                        //body += "<tr><td>SHORTAGE</td> <td> " + txtShortage.Text + "</td></tr>";
                        //body += "<tr><td>PACKAGING</td> <td> " + txtPkng.Text + "</td></tr>";
                        //body += "<tr><td>CONTENT</td> <td> " + txtContents.Text + "</td></tr>";
                        //body += "<tr><td>CONSIGNOR</td> <td> " + txtOrgAgent.Text + "</td></tr>";
                        //body += "<tr><td>CONSIGNEE</td> <td> " + txtDestAgent.Text + "</td></tr>";
                        //body += "<tr><td>REMARKS</td> <td> " + txtRemarks.Text + "</td></tr>";
                        ////body += "<tr><td>Updated By</td> <td> " + User.Identity.Name.ToUpper() + "</td></tr>";
                        //body += "<tr><td>Updated By</td> <td> " + Session["UserName"].ToString().ToUpper() + "</td></tr>";
                       #endregion

                        //Plain text mail
                        body = "Dear All, "+"\r\n";
                        body += "This is to inform you that there is Shortage Detected, Details are as follows" + "\r\n";

                        body += "FLIGHT NO:- " + txtFlightNo.Text + "\r\n";
                        body += "FLIGHT DATE(AWB DATE):- " + txtDate.Text + "\r\n";
                        body += "ORGIN-DEST:- " + txtOrg.Text + " - " + txtDest.Text + "\r\n";
                        body += "AIRWAY BILL NO:- " + dsAwbNo + "\r\n";
                        body += "PCS/WT:- " + txtTotalPcs.Text + "pcs/" + hdnWeight.Value + "kgs" +"\r\n";
                        body += "SHORTAGE:- " + txtShortage.Text  +"\r\n";
                        body += "PACKAGING:- " + txtPkng.Text + "\r\n";
                        body += "CONTENT:- " + txtContents.Text + "\r\n";
                        body += "CONSIGNOR:- " + txtOrgAgent.Text + "\r\n";
                        body += "CONSIGNEE:- " + txtDestAgent.Text + "\r\n";
                        body += "REMARKS:- " + txtRemarks.Text + "\r\n";
                        body += "Updated By :-" + Session["UserName"].ToString().ToUpper() + "\r\n";
                    }

                    if (mailType == "Closed")
                    {
                        //subject = client +" "+ "TRACER Closed For Tracer No: " + hdnTracerNo.Value + " Source:" + txtOrg.Text + "  AWB No is " + dsAwbNo;

                        // updated on 22 Sept 2014 for [JIRA] (AC-67)
                        subject = "TRACER Closed For Tracer No: " + hdnTracerNo.Value + " Source:" + txtOrg.Text + "  AWB No is " + dsAwbNo;

                        Mail.Subject = subject;

                        Mail.IsBodyHtml = true;

                        #region HTML Subject
                        //body = "Dear All,<br/><br/> ";
                        //body += "This is to inform you that, following Tracer is CLOSED:<br/><br/> ";

                        //body += "<table border = 1>";
                        //body += "<tr><td>Tracer NO</td> <td> " + hdnTracerNo.Value + "</td></tr>";
                        //body += "<tr><td>AIRWAY BILL NO</td> <td> " + dsAwbNo + "</td></tr>";
                        //body += "<tr><td>FLIGHT DATE(AWB DATE)</td> <td> " + txtDate.Text + "</td></tr>";
                        //body += "<tr><td>ORGIN-DEST</td> <td> " + txtOrg.Text + " - " + txtDest.Text + "  </td></tr>";
                        //body += "<tr><td>PCS/WT</td> <td> " + txtTotalPcs.Text + "pcs/" + hdnWeight.Value + "kgs" + "</td></tr>";
                        //body += "<tr><td>Updated By</td> <td> " + User.Identity.Name.ToUpper() + "</td></tr>";
                        #endregion

                        // Plain Text Message

                        body = "Dear All," + "\r\n";
                        body += "This is to inform you that, following Tracer is CLOSED:" + "\r\n";

                        body += "Tracer NO:-" + hdnTracerNo.Value + "\r\n";
                        body += "AIRWAY BILL NO:-" + dsAwbNo + "\r\n";
                        body += "FLIGHT DATE:-" + txtDate.Text + "\r\n";
                        body += "ORGIN-DEST:" + txtOrg.Text + " - " + txtDest.Text + "\r\n";
                        body += "PCS/WT:" + txtTotalPcs.Text + "pcs/" + hdnWeight.Value + "kgs" + "\r\n";
                        body += "Updated By:-" + Session["UserName"].ToString().ToUpper() + "\r\n";


                    }


                    body += "Note: This is an autogenerated E-mail. Please do not reply.";

                    Mail.Body = body.ToString();

                    #region Store Emails & Attachment in Database

                    ArrayList AttachmentFiles = new ArrayList();
                    ArrayList AttachInputStream = new ArrayList();

                    string[] Document = new string[0];
                    MemoryStream[] Attachments = new MemoryStream[0];

                    string[] Extension = new string[0];

                    if (Cache["attchFile"] != null)
                    {
                        if (Cache["attchFile"].ToString() != string.Empty)
                        {
                            AttachmentFiles = (ArrayList)Cache["attchFile"];
                            AttachInputStream = (ArrayList)Cache["attchInputStream"];
                            
                            List<byte[]> FDoc = new List<byte[]>();
                            FDoc = (List<byte[]>)Session["FileByte"];

                            for (int i = 0; i < AttachmentFiles.Count; i++)
                            {
                                //Stream IStream = (Stream)AttachInputStream[i];
                                //Mail.Attachments.Add(new Attachment(IStream, AttachmentFiles[i].ToString()));

                                MemoryStream IStream = new MemoryStream(FDoc[i]);
                                Mail.Attachments.Add(new Attachment(IStream, AttachmentFiles[i].ToString()));

                                Array.Resize(ref Document, Document.Length + 1);
                                Document[Document.Length - 1] = AttachmentFiles[i].ToString().Substring(0, AttachmentFiles[i].ToString().Length - 4);

                                Array.Resize(ref Attachments, Attachments.Length + 1);
                                Attachments[Attachments.Length - 1] = IStream;

                                Array.Resize(ref Extension, Extension.Length + 1);
                                Extension[Extension.Length - 1] = AttachmentFiles[i].ToString().Substring(AttachmentFiles[i].ToString().Length - 4);
                            
                            }
                        }
                        else
                        {
                            if (MyFile.Value != string.Empty)
                            {
                                Mail.Attachments.Add(new Attachment(MyFile.PostedFile.InputStream, MyFile.PostedFile.FileName));

                            }

                        }
                    }

                    // Function addMsgToOutBox to Send Emails & store the uploaded files into Database.(sp_AddAttachmentMessage)
                    bool result = false;

                    //result = cls_BL.addMsgToOutBox(subject, body.ToString(),FromEmailID, ToEmailID, Attachments, Document, Extension);
                    result = cls_BL.addMsgToOutBox(subject, body.ToString(), "", ToEmailID, Attachments, Document, Extension,Convert.ToDateTime(Session["IT"].ToString()));

                    #endregion
                                       
                }
                else
                {
                    RetVal = 0;
                }
            }

            catch (Exception)
            {
                MailMessage Mail = new MailMessage();
                Mail = (MailMessage)Cache["Email"];
                try
                {
                    Smtp.Send(Mail);

                    Mail.Attachments.Clear();
                    Mail.Attachments.Dispose();
                    RetVal = 1;
                    DispError("Tracer Mail Sent Successfully.", true);
                }
                catch (Exception)
                {

                    Mail = (MailMessage)Cache["Email"];
                    try
                    {
                        Smtp.Send(Mail);

                        Mail.Attachments.Clear();
                        Mail.Attachments.Dispose();
                        RetVal = 1;
                        DispError("Tracer Mail Sent Successfully.", true);
                    }
                    catch (Exception)
                    {

                        DispError("Tracer Mail Sent Failed.", true);
                        RetVal = 0;
                    }
                }
                
            }

            return RetVal;
        }

        # endregion SendEmail


        #region ON GENERATE TRACER UPLOAD(ATTACHED FILE) BUTTON CLICK EVENT

        /// <summary>
        /// UPLOAD ATTACHED FILE WITH TRACER TO SENT VIA EMAIL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void btnUpd_Click(object sender, EventArgs e)
        {
            try
            {
                DispError(string.Empty, false);


                if (MyFile.Value != string.Empty)
                {
                    ArrayList AttachmentFiles = new ArrayList();
                    ArrayList AttachInputStream = new ArrayList();
                    ArrayList AttachDT = new ArrayList();

                    List<byte[]> FDoc = new List<byte[]>();
                    DataTable myDataTable = new DataTable();
                    DataColumn myDataColumn;

                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = Type.GetType("System.String");
                    myDataColumn.ColumnName = "FileName";
                    myDataTable.Columns.Add(myDataColumn);

                    DataRow dr;
                    dr = myDataTable.NewRow();

                    if (Cache["attchFile"].ToString() != string.Empty)
                    {
                        AttachmentFiles = (ArrayList)Cache["attchFile"];
                        AttachInputStream = (ArrayList)Cache["attchInputStream"];
                        AttachDT = (ArrayList)Cache["Dt"];

                        if (Cache["Dt"].ToString() != string.Empty)
                        {
                            for (int i = 0; i < AttachDT.Count; i++)
                            {
                                dr = myDataTable.NewRow();
                                dr["FileName"] = AttachDT[i];
                                myDataTable.Rows.Add(dr);
                            }
                        }
                    }

                    Stream fs = MyFile.PostedFile.InputStream;
                    BinaryReader br = new BinaryReader(fs);
                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    if (Session["FileByte"] != null)
                    {
                        FDoc = (List<byte[]>)Session["FileByte"];
                        FDoc.Add(bytes);
                    }
                    else
                    {
                        FDoc.Add(bytes);
                    }
                    Session["FileByte"] = FDoc;

                    AttachmentFiles.Add(MyFile.PostedFile.FileName);
                    AttachInputStream.Add(MyFile.PostedFile.InputStream);
                    AttachDT.Add(MyFile.PostedFile.FileName);

                    Cache["attchFile"] = AttachmentFiles;
                    Cache["attchInputStream"] = AttachInputStream;
                    Cache["Dt"] = AttachDT;

                    dr = myDataTable.NewRow();
                    dr["FileName"] = MyFile.PostedFile.FileName;

                    myDataTable.Rows.Add(dr);
                    grdCurrArchived.DataSource = null;
                    grdCurrArchived.DataSource = myDataTable;
                    grdCurrArchived.DataBind();
                    grdCurrArchived.Visible = true;
                }
                else
                {
                    ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please Add Attachment To Upload...');</script>");
                    MyFile.Focus();
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please Add Attachment To Upload..." + ex.Message + "');</script>");
                MyFile.Focus();
            }

        }

        #endregion ON GENERATE TRACER UPLOAD(ATTACHED FILE) BUTTON CLICK EVENT


        #region ON GENERATE TRACER CURRENT ATTACHED FILES GRIDVIEW DELETE BUTTON CLICK EVENT

        /// <summary>
        /// DELETE BUTTON CLICKED FOR REMOVING ATTACHED FILE FROM CURRENTLY UPLOADED FILE DISPLAYED INSIDE GRIDVIEW INSIDE TRACER GENERATION FRAME
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void grdCurrArchived_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {

                int index = Convert.ToInt32(e.CommandArgument);

                ArrayList AttachmentFiles = new ArrayList();
                ArrayList AttachInputStream = new ArrayList();
                ArrayList AttachDT = new ArrayList();

                if (Cache["attchFile"].ToString() != string.Empty)
                {
                    AttachmentFiles = (ArrayList)Cache["attchFile"];
                    AttachInputStream = (ArrayList)Cache["attchInputStream"];
                    AttachDT = (ArrayList)Cache["Dt"];
                    AttachmentFiles.RemoveAt(index);
                    AttachInputStream.RemoveAt(index);
                    AttachDT.RemoveAt(index);
                    Cache["attchFile"] = AttachmentFiles;
                    Cache["attchInputStream"] = AttachInputStream;
                    Cache["Dt"] = AttachDT;
                    grdCurrArchived.DeleteRow(index);
                }
            }
        }

        #endregion ON GENERATE TRACER CURRENT ATTACHED FILES GRIDVIEW DELETE BUTTON CLICK EVENT


        #region ON GENERATE TRACER CURRENT ATTACHED FILES GRIDVIEW ROW DELETING EVENT

        /// <summary>
        /// AFTER ROW DELETED FROM CURRENTLY UPLOADED FILE GRIDVIEW INSIDE TRACER GENERATION FRAME THIS EVENT IS FIRED TO RELOAD GRIDVIEW AND UNBOUND DATA FROM ARRAY LIST
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void grdCurrArchived_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            ArrayList AttachDT = new ArrayList();
            AttachDT = (ArrayList)Cache["Dt"];
            DataTable myDataTable = new DataTable();
            DataColumn myDataColumn;

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "FileName";
            myDataTable.Columns.Add(myDataColumn);



            DataRow dr;
            dr = myDataTable.NewRow();
            if (Cache["Dt"].ToString() != string.Empty)
            {
                for (int i = 0; i < AttachDT.Count; i++)
                {
                    dr = myDataTable.NewRow();
                    dr["FileName"] = AttachDT[i];
                    myDataTable.Rows.Add(dr);
                }
            }
            grdCurrArchived.DataSource = null;
            grdCurrArchived.DataSource = myDataTable;
            grdCurrArchived.DataBind();
            if (myDataTable.Rows.Count >= 1)
            {
                lblUpdFile.Visible = true;
            }
            else
            {
                lblUpdFile.Visible = false;
            }
        }

        #endregion ON GENERATE TRACER CURRENT ATTACHED FILES GRIDVIEW ROW DELETING EVENT


        //#region MERGE ORIGIN DESTINATION COMMA SEPARATED CODES in CheckBox

        ///// <summary>
        ///// FOR MERGING ORIGIN AND DESTINATION LOCATION CODES AS COMMA SEPARATED IN A STRING.
        ///// </summary>
        ///// <param name="orgLoc"></param>
        ///// <param name="destLoc"></param>

        //public void GetLocCodes(ref string orgLoc, ref string destLoc)
        //{
        //    String newOrgLoc = String.Empty;
        //    String newDestLoc = String.Empty;

        //    if (ChkSrcList.SelectedIndex != 0)
        //    {

        //        for (int i = 0; i < ChkSrcList.Items.Count; i++)
        //        {
        //            if (ChkSrcList.Items[i].Selected == true)
        //            {
        //                newOrgLoc = newOrgLoc + "'" + ChkSrcList.Items[i].Text + "'";
        //            }
        //        }

        //        orgLoc = newOrgLoc.Replace("''", "','");
        //    }

        //    else
        //    {
        //        orgLoc = "ALL";
        //    }

        //    if (ChkDestList.SelectedIndex != 0)
        //    {

        //        for (int i = 0; i < ChkDestList.Items.Count; i++)
        //        {
        //            if (ChkDestList.Items[i].Selected == true)
        //            {
        //                newDestLoc = newDestLoc + "'" + ChkDestList.Items[i].Text + "'";
        //            }
        //        }

        //        destLoc = newDestLoc.Replace("''", "','");
        //    }

        //    else
        //    {
        //        destLoc = "ALL";
        //    }

        //}

        //#endregion MERGE ORIGIN DESTINATION COMMA SEPARATED CODES in CheckBox


        #region MERGE ORIGIN DESTINATION COMMA SEPARATED CODES in DropdownBox

        
        public void GetLocCodes(ref string orgLoc, ref string destLoc)
        {
            String newOrgLoc = String.Empty;
            String newDestLoc = String.Empty;

            if (DdlOriginList.SelectedIndex != 0)
            {

                //for (int i = 0; i < DdlOriginList.Items.Count; i++)
                //{
                //    if (DdlOriginList.Items[i].Selected == true)
                //    {
                //        newOrgLoc = newOrgLoc + "'" + DdlOriginList.Items[i].Text + "'";
                //    }
                //}
                orgLoc = DdlOriginList.SelectedValue.ToString();
                //orgLoc = newOrgLoc.Replace("''", "','");
            }

            else
            {
                orgLoc = "ALL";
            }

            if (DdlDestinationList.SelectedIndex != 0)
            {

                //for (int i = 0; i < DdlDestinationList.Items.Count; i++)
                //{
                //    if (DdlDestinationList.Items[i].Selected == true)
                //    {
                //        newDestLoc = newDestLoc + "'" + DdlDestinationList.Items[i].Text + "'";
                //    }
                //}

                //destLoc = newDestLoc.Replace("''", "','");

                destLoc = DdlDestinationList.SelectedValue.ToString();
            }

            else
            {
                destLoc = "ALL";
            }

        }

        #endregion MERGE ORIGIN DESTINATION COMMA SEPARATED CODES in DropdownBox


        #region CHECK AWBNO,TRACERNO AND FLIGHTNO EXISTS IN DATABASE

        /// <summary>
        /// TO CHECK FOR EXISTING FLIGHT,AWBNO AND TRACER NO FOR VALIDATION PURPOSE IF NOT EXIST THROWS ERROR
        /// </summary>
        /// <param name="FlightNo"></param>
        /// <returns></returns>

        public int CheckFiltersNoExists(string CheckType, string ValidateNo, ref string ErrorMsg)
        {
            int retVal = 0;
            DataSet oDs = new DataSet();
            ErrorMsg = string.Empty;
            try
            {

                /// BELOW FILLING DATASET WITH VALUE AS 0 OR 1 NOT EXIST OR EXIST
                string[] DParam = new string[2];
                object[] DValues = new object[2];
                SqlDbType[] DTypes = new SqlDbType[2];

                DParam.SetValue("CheckType", 0);
                DValues.SetValue(CheckType, 0);
                DTypes.SetValue(SqlDbType.VarChar, 0);

                DParam.SetValue("ValidateNo", 1);
                DValues.SetValue(ValidateNo, 1);
                DTypes.SetValue(SqlDbType.VarChar, 1);
                SQLServer ObjData = new SQLServer(Global.GetConnectionString());

                oDs = ObjData.SelectRecords("SpCheckExistsTracerFiltersLive", DParam, DValues, DTypes);

                if (oDs != null)
                {
                    if (oDs.Tables[0].Rows.Count >= 1)
                    {
                        /// TO ASSIGN DATASET VALUE TO INTEGER VARIABLE
                        retVal = Convert.ToInt16(oDs.Tables[0].Rows[0][0].ToString());
                        if (retVal == 0)
                        {
                            ErrorMsg = CheckType + " No Does Not Exists";
                        }
                    }
                    else
                    {
                        retVal = 0;
                        ErrorMsg = "Error Validating" + CheckType;
                    }
                }
                else
                {
                    retVal = 0;
                    ErrorMsg = "Error Validating" + CheckType;
                }
            }
            catch (Exception)
            {

                retVal = 2;
            }
            return retVal;
        }

        #endregion CHECK FLIGHTNO EXISTS IN DATABASE

        //#region SOURCE AIRPORT CHECKLIST SELECTED INDEX CHANGED

        ///// <summary>
        ///// FOR DISABLED OTHER SOURCE CODE IF ALL IS SELECTED ELSE ENABLED
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>

        //protected void ChkSrcList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    /// CHECKING FOR SOURCE CODE IS SELECTED AS ALL
        //    if (ChkSrcList.SelectedIndex == 0)
        //    {
        //        /// DISABLE OTHER AIRPORT CODES AS ALL IS SELECTED
        //        for (int i = 1; i < ChkSrcList.Items.Count; i++)
        //        {
        //            ChkSrcList.Items[i].Enabled = false;
        //            ChkSrcList.Items[i].Selected = false;

        //        }
        //        return;
        //    }
        //    else if (ChkSrcList.SelectedIndex < 0)
        //    {
        //        for (int i = 0; i < ChkSrcList.Items.Count; i++)
        //        {
        //            ChkSrcList.Items[i].Enabled = true;
        //            ChkSrcList.Items[i].Selected = false;
        //        }
        //        return;
        //    }

        //    if (ChkSrcList.SelectedIndex != 0)
        //    {
        //        if (ChkSrcList.Items[0].Enabled != false)
        //        {
        //            ChkSrcList.Items[0].Enabled = false;
        //            return;
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }
        //}

        //#endregion SOURCE AIRPORT CHECKLIST SELECTED INDEX CHANGED


        #region AIRPORT ORIGIN DROPDownLIST SELECTED INDEX CHANGED

        
       protected void DdlOriginList_SelectedIndexChanged(object sender, EventArgs e)
        {
                  
            /// CHECKING FOR SOURCE CODE IS SELECTED AS ALL
            if (DdlOriginList.SelectedIndex == 0)
            {
                /// DISABLE OTHER AIRPORT CODES AS ALL IS SELECTED
                for (int i = 1; i < DdlOriginList.Items.Count; i++)
                {
                    DdlOriginList.Items[i].Enabled = false;
                    DdlOriginList.Items[i].Selected = false;

                }
                return;
            }
            else if (DdlOriginList.SelectedIndex < 0)
            {
                for (int i = 0; i < DdlOriginList.Items.Count; i++)
                {
                    DdlOriginList.Items[i].Enabled = true;
                   DdlOriginList.Items[i].Selected = false;
                }
                return;
            }

            if (DdlOriginList.SelectedIndex != 0)
            {
                if (DdlOriginList.Items[0].Enabled != false)
                {
                    DdlOriginList.Items[0].Enabled = false;
                    return;
                }
                else
                {
                    return;
                }
            }
        }

        #endregion AIRPORT ORIGIN DROPDownLIST SELECTED INDEX CHANGED



       //#region DESTINATION AIRPORT CHECKLIST SELECTED INDEX CHANGED

        ///// <summary>
        ///// FOR DISABLED OTHER SOURCE CODE IF ALL IS SELECTED ELSE ENABLED OR WISE VERSA
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>

        //protected void ChkDestList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    /// CHECKING FOR SOURCE CODE IS SELECTED AS ALL
        //    if (ChkDestList.SelectedIndex == 0)
        //    {
        //        /// DISABLE OTHER AIRPORT CODES AS ALL IS SELECTED
        //        for (int i = 1; i < ChkDestList.Items.Count; i++)
        //        {
        //            ChkDestList.Items[i].Enabled = false;
        //            ChkDestList.Items[i].Selected = false;

        //        }
        //        return;
        //    }
        //    else if (ChkDestList.SelectedIndex < 0)
        //    {
        //        for (int i = 0; i < ChkDestList.Items.Count; i++)
        //        {
        //            ChkDestList.Items[i].Enabled = true;
        //            ChkDestList.Items[i].Selected = false;
        //        }
        //        return;
        //    }

        //    if (ChkDestList.SelectedIndex != 0)
        //    {
        //        if (ChkDestList.Items[0].Enabled != false)
        //        {
        //            ChkDestList.Items[0].Enabled = false;
        //            return;
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }
        //}

       //#endregion DESTINATION AIRPORT CHECKLIST SELECTED INDEX CHANGED

       #region AIRPORT DESTINATION DROPDOWN_LIST SELECTED INDEX CHANGED

        protected void DdlDestinationList_SelectedIndexChanged(object sender, EventArgs e)
        {

            /// CHECKING FOR SOURCE CODE IS SELECTED AS ALL
            if (DdlDestinationList.SelectedIndex == 0)
            {
                /// DISABLE OTHER AIRPORT CODES AS ALL IS SELECTED
                for (int i = 1; i < DdlDestinationList.Items.Count; i++)
                {
                    DdlDestinationList.Items[i].Enabled = false;
                    DdlDestinationList.Items[i].Selected = false;

                }
                return;
            }
            else if (DdlDestinationList.SelectedIndex < 0)
            {
                for (int i = 0; i < DdlDestinationList.Items.Count; i++)
                {
                    DdlDestinationList.Items[i].Enabled = true;
                    DdlDestinationList.Items[i].Selected = false;
                }
                return;
            }

            if (DdlDestinationList.SelectedIndex != 0)
            {
                if (DdlDestinationList.Items[0].Enabled != false)
                {
                    DdlDestinationList.Items[0].Enabled = false;
                    return;
                }
                else
                {
                    return;
                }
            }
        }

       #endregion AIRPORT DESTINATION DROPDOWN_LIST SELECTED INDEX CHANGED


        #region VALIDATING FILTERS ON VIEW BUTTON CLICK

        public bool ValidateFilters(ref string errMsg)
        {

            /// TO VALIDATE  AWB NO IF MISSED CHECKING IN TEXT CHANGED EVENT
            string StrFilters = string.Empty;
            string StrFiltersVal = string.Empty;

            bool chkBool = false;
            if (txtAwbno.Text == string.Empty)
            {
                if (RdbOtherFilters.SelectedIndex == 1)
                {
                    StrFilters = "TRACER";
                    StrFiltersVal = txtOtherFilter.Text;
                }
                if (RdbOtherFilters.SelectedIndex == 2)
                {
                    StrFilters = "FLIGHT";
                    StrFiltersVal = txtFltNo.Text;
                }
            }
            else
            {
                StrFilters = "AWB";
                if (txtAwbno.Text != string.Empty && txtAWBPrefix.Text!=string.Empty)//HdnNewAWB.Value
                {
                    StrFiltersVal = txtAWBPrefix.Text + txtAwbno.Text;
                }
                //else
                //{
                //    StrFiltersVal = HdnNewAWB.Value;
                //}
            }
            try
            {
                int retVal = CheckFiltersNoExists(StrFilters, StrFiltersVal, ref errMsg);
                if (retVal != 1)
                {
                    DispError(errMsg, true);
                    Timer2.Enabled = false;
                    DispStatus(false);
                    chkBool = true;
                }
                else
                {
                    chkBool = false;
                }

            }
            catch (Exception)
            {
                DispError(errMsg, true);
                Timer2.Enabled = false;
                DispStatus(false);
                chkBool = true;
            }

            return chkBool;
        }

        #endregion VALIDATING FILTERS ON VIEW BUTTON CLICK



        #region Get Data By Procedure Multiple Parameters
        /// <summary>
        /// Gets data in data reader object from database based on select stored procedure.
        /// </summary>
        /// <param name="selectProcedure">Procedure name to retrieve data from database.</param>
        /// <param name="QueryPName">Array containing parameter names without @.</param>
        /// <param name="QueryTypes">Array containing parameter data types.</param>
        /// <param name="QueryValues">Array containing parameter values.</param>
        /// <returns>SqlDataReader containing data returned by query.</returns>
        public SqlDataReader GetDataByProcedure(string selectProcedure, string[] QueryPName, object[] QueryValues, SqlDbType[] QueryTypes)
        {
            try
            {
                SqlConnection sqCon;
                SqlCommand sqCmd;
                string ConnectionString;

                sqCmd = new SqlCommand();
                sqCon = new SqlConnection();
                ConnectionString = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
                sqCon.ConnectionString = ConnectionString;
                if (sqCon.State != ConnectionState.Open)
                    sqCon.Open();
                sqCmd.Connection = sqCon;
                sqCmd.CommandType = CommandType.StoredProcedure;
                sqCmd.CommandText = selectProcedure;
                //Add parameters...
                for (int i = 0; i < QueryPName.Length; i++)
                {
                    sqCmd.Parameters.Add("@" + QueryPName[i], QueryTypes[i]).Value = QueryValues[i];
                }
                SqlDataReader dr = sqCmd.ExecuteReader();
                sqCmd.Dispose();
                return (dr);
            }
            catch (Exception)
            {
                //clsGlobal.LogMessage("DataAccess.GetData: " + ex.Message);
                return (null);
            }
        }
        #endregion


        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        public static string[] GetFlightList(string prefixText, int count, string contextKey)
        {
            List<string> CompletionSet = new List<string>();
            //SqlDataReader oDr = getFlightList(
            //string custItem = string.Empty;
            try
            {
                String[] DParam = { "PrefixText" };
                SqlDbType[] DTypes = { SqlDbType.VarChar };
                Object[] DValues = { prefixText };
                //Database ObjData = new Database();
                SQLServer ObjData = new SQLServer(BAL.Global.GetConnectionString());

                DataSet oDr = ObjData.SelectRecords("SpGetTracerFlights", DParam, DValues, DTypes);

                if (oDr != null && oDr.Tables.Count > 0 && oDr.Tables[0].Rows.Count > 0)
                {
                    for (int intCount = 0; intCount < oDr.Tables[0].Rows.Count; intCount++)
                    {
                        CompletionSet.Add(oDr.Tables[0].Rows[intCount][0].ToString());
                    }
                }
            }
            catch (Exception)
            {

                CompletionSet.Add("No Records Found");
            }

            return CompletionSet.ToArray();
        }

        #region EMPTY CONTROLS

        public void EmptyTracerControls()
        {
            //ClientScript.RegisterStartupScript(this.GetType(), "HidePnl", "doHidePanel2();", true);
            divgen.Visible = false;
            txtAwbNoTr.Text = string.Empty;
            txtOrg.Text = string.Empty;
            txtDest.Text = string.Empty;
            txtFlightNo.Text = string.Empty;
            txtShortage.Text = string.Empty;
            txtTotalPcs.Text = string.Empty;
            txtWgt.Text = string.Empty;
            txtDate.Text = string.Empty;
            txtPkng.Text = string.Empty;
            txtContents.Text = string.Empty;
            txtOrgAgent.Text = string.Empty;
            txtDestAgent.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            grdViewTracer.Visible = true;
            //btnAdv1.Items[0].Selected = false;
            if (HdnPanel.Value == "Panel1")
            {
                Panel1.Visible = true;
            }
            else
            {
                Panel3.Visible = true;
            }
        }

        #endregion EMPTY CONTROLS

        protected void RdbOtherFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RdbOtherFilters.SelectedIndex == 0)
            {
                txtFromDt.Enabled = true;
                txtToDt.Enabled = true;
                txtOtherFilter.Enabled = false;
                txtOtherFilter.Visible = false;
                txtFltNo.Visible = false;
                txtFltNo.Text = string.Empty;
                txtOtherFilter.Text = string.Empty;
                ddlAWBStatus.Enabled = true;
                ddlTracerStatus.Enabled = true;
                DdlOriginList.Enabled = true;
                DdlDestinationList.Enabled = true;
                //ChkSrcList.Enabled = true;
                //ChkDestList.Enabled = true;
                LoadGridView();
            }
            else if (RdbOtherFilters.SelectedIndex == 1)
            {
                txtFromDt.Enabled = false;
                txtToDt.Enabled = false;
                txtOtherFilter.Enabled = true;
                txtOtherFilter.Visible = true;
                txtFltNo.Visible = false;
                txtFltNo.Text = string.Empty;
                txtOtherFilter.Text = string.Empty;
                ddlAWBStatus.Enabled = false;
                ddlTracerStatus.Enabled = false;
                DdlOriginList.Enabled = true;
                DdlDestinationList.Enabled = true;
                //ChkSrcList.Enabled = false;
                //ChkDestList.Enabled = false;

            }
            else if (RdbOtherFilters.SelectedIndex == 2)
            {
                txtFromDt.Enabled = true;
                txtToDt.Enabled = true;
                txtOtherFilter.Visible = false;
                txtFltNo.Visible = true;
                txtFltNo.Text = string.Empty;
                txtOtherFilter.Text = string.Empty;
                ddlAWBStatus.Enabled = true;
                ddlTracerStatus.Enabled = true;
                DdlOriginList.Enabled = true;
                DdlDestinationList.Enabled = true;
                //ChkSrcList.Enabled = true;
                //ChkDestList.Enabled = true;
            }
        }
        protected void txtOtherFilter_TextChanged(object sender, EventArgs e)
        {
            if (RdbOtherFilters.SelectedIndex != 0)
            {
                string errMsg = string.Empty;
                bool chkVal = false;
                chkVal = ValidateFilters(ref errMsg);
                hdnFltVal.Value = "PASS";

                if (chkVal == true)
                {
                    hdnFltVal.Value = string.Empty;
                    DispError(errMsg, true);
                    //ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + errMsg + "');</script>");
                    return;
                }
            }
        }
        protected void txtFltNo_TextChanged(object sender, EventArgs e)
        {
            if (RdbOtherFilters.SelectedIndex != 0)
            {
                string errMsg = string.Empty;
                bool chkVal = false;
                chkVal = ValidateFilters(ref errMsg);
                hdnFltVal.Value = "PASS";

                if (chkVal == true)
                {
                    hdnFltVal.Value = string.Empty;
                    DispError(errMsg, true);
                    //ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + errMsg + "');</script>");
                    return;
                }
            }
        }
        protected void txtFlightNo_TextChanged(object sender, EventArgs e)
        {
            string errMsg = string.Empty;

            int retVal = CheckFiltersNoExists("FLIGHT", txtFlightNo.Text, ref errMsg);
            if (retVal != 1)
            {
                DispError(errMsg, true);
                //ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + errMsg + "');</script>");
                txtFlightNo.Focus();
                hdnFltVal.Value = string.Empty;
            }
            else
            {
                hdnFltVal.Value = "Passed";
            }
        }

        //protected void btnAdv_Click(object sender, EventArgs e)
        //{
        //    DispError("", false);
        //    //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl1", "doShowPanel1();", true);
        //    Panel3.Visible = false;
        //    Panel1.Visible = true;
        //    divgen.Visible = false;
        //    HdnPanel.Value = "Panel1";
        //    txtAwbno.Text = string.Empty;
        //    //HdnNewAWB.Value = string.Empty;
        //}


        #region Old CLose Button Code
        protected void btnCloseAdv_Click(object sender, ImageClickEventArgs e)
        {
            //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl3", "doShowPanel3();", true);
            DispError(string.Empty, false);
            HdnIsPageLoad.Value = "N";
            Panel1.Visible = false;
            divgen.Visible = false;
            txtAwbno.Text = string.Empty;
            Panel3.Visible = true;
            HdnPanel.Value = "Panel3";
            txtOtherFilter.Text = string.Empty;
            txtFltNo.Text = string.Empty;
            ddlAWBStatus.SelectedIndex = 0;
            ddlTracerStatus.SelectedIndex = 0;
            txtAWBPrefix.Text = string.Empty;
            //btnAdv1.Items[0].Selected = false;
            //HdnNewAWB.Value = string.Empty;

        }

        #endregion

        public string SortField
        {
            get
            {
                return (string)ViewState["_sortField"];
            }
            set
            {
                ViewState["_sortField"] = value;
            }
        }
        public string SortDir
        {
            get
            {
                return (string)ViewState["_sortDir"];
            }
            set
            {
                ViewState["_sortDir"] = value;
            }
        }

        public void DoDataBind()
        {
            DataSet oDs = new DataSet();
            ViewState["oDsSort"] = (DataSet)ViewState["oDsCurr"];
            oDs = (DataSet)ViewState["oDsSort"];
            DataTable dt = oDs.Tables[0];
            dt.DefaultView.Sort = SortField + " " + SortDir;

            grdViewTracer.DataSource = dt.DefaultView;
            grdViewTracer.DataBind();
        }

        protected void grdViewTracer_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression == SortField && SortDir != "desc")
            {
                SortDir = "desc";
            }
            else
            {
                SortDir = "asc";
            }

            SortField = e.SortExpression;

            DoDataBind();
        }

        protected void txtAwbno_TextChanged(object sender, EventArgs e)
        {
            //HdnNewAWB.Value = string.Empty;

            //if (txtAwbno.Text != string.Empty)
            //{
            //    HdnNewAWB.Value = txtAwbno.Text;
            //}
            //else 
            //{
            //    HdnNewAWB.Value = string.Empty;

            //}
        }


        #region Timer 1 Click
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            //if (lblLoadStat.Visible == false)
            //{
            //    return;
            //}
            Timer1.Enabled = false;

            try
            {

                txtFromDt.Text = Convert.ToDateTime(Session["IT"].ToString()).Date.AddDays(-7).ToString("dd/MM/yyyy");
                //txtToDt.Text = Convert.ToString(DateTime.Today.Date.AddDays(-1).ToString("dd/MM/yyyy"));
                txtToDt.Text = Convert.ToDateTime(Session["IT"].ToString()).Date.ToString("dd/MM/yyyy");
                

                //ddlAWBStatus.SelectedIndex = 1;
                LoadAirportCode();
                /// BELOW FOR ORIGIN AIRPORT CODE TO DISABLE OTHER AIRPORT CODES AS ALL IS SELECTED

                //for (int i = 1; i < ChkSrcList.Items.Count; i++)
                //{
                //    ChkSrcList.Items[i].Enabled = false;
                //    ChkSrcList.Items[i].Selected = false;
                //    ChkDestList.Items[i].Enabled = false;
                //    ChkDestList.Items[i].Selected = false;

                //}

                //for (int i = 1; i < DdlOriginList.Items.Count; i++)
                //{
                //    DdlOriginList.Items[i].Enabled = false;
                //    DdlOriginList.Items[i].Selected = false;
                //    DdlDestinationList.Items[i].Enabled = false;
                //    DdlDestinationList.Items[i].Selected = false;

                //}
                LoadGridView();
                //ClientScript.RegisterStartupScript(this.GetType(), "HideStat", "HideLoadingStat();", true);

                DispStatus(false);
                //Timer1.Enabled = true;

            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        private void DispStatus(bool FStat)
        {
           // lblLoadStat.Visible = FStat;
           // ImgLoading.Visible = FStat;
        }

        #region Timer 2 Click
        protected void Timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                divgen.Visible = true ;
                gneratetrace.Visible = false;
                Timer2.Enabled = false;
                string errMsg = string.Empty;

                LoadGridView();
                DispStatus(false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion 

        private void DispError(string ErrMsg, bool VisBool)
        {
            lblError.Text = ErrMsg;
            lblError.Visible = VisBool;
        }

        //protected void btnAdv_CheckedChanged(object sender, EventArgs e)
        //{
        //    DispError(string.Empty, false);
        //    Panel3.Visible = false;
        //    Panel1.Visible = true;
        //    divgen.Visible = false;
        //    HdnPanel.Value = "Panel1";
        //    txtAwbno.Text = string.Empty;
        //}

        protected void grdCurrArchived_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowType == DataControlRowType.DataRow))
            {

                foreach (TableCell tc in e.Row.Cells)
                {
                    tc.Attributes["style"] = "border-color:#990000";
                }
            }
        }

        protected void btnlnkAdv_Click(object sender, EventArgs e)
        {
            DispError(string.Empty, false);
            Panel3.Visible = false;
            Panel1.Visible = true;
            divgen.Visible = false;
            HdnPanel.Value = "Panel1";
            txtAwbno.Text = string.Empty;
            txtAWBPrefix.Text = string.Empty;
        }

        protected void BtnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/FrmTracerGeneration.aspx");
        }

        #region Button Clear
        //protected void BtnClear_Click(object sender, EventArgs e)
        //{
        //    DispError(string.Empty, false);
        //    HdnIsPageLoad.Value = "N";
        //    Panel1.Visible = false;
        //    divgen.Visible = false;
        //    txtAwbno.Text = string.Empty;
        //    Panel3.Visible = true;
        //    HdnPanel.Value = "Panel3";
        //    txtOtherFilter.Text = string.Empty;
        //    txtFltNo.Text = string.Empty;
        //    ddlAWBStatus.SelectedIndex = 0;
        //    ddlTracerStatus.SelectedIndex = 0;
        //    txtAWBPrefix.Text = string.Empty;
        //    //btnAdv1.Items[0].Selected = false;
        //    //HdnNewAWB.Value = string.Empty;
        //}

        #endregion


        //protected void btnAdv_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DispError(string.Empty, false);
        //    Panel3.Visible = false;
        //    Panel1.Visible = true;
        //    divgen.Visible = false;
        //    HdnPanel.Value = "Panel1";
        //    txtAwbno.Text = string.Empty;
        //}

        

        //protected void btnAdv1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DispError(string.Empty, false);
        //    Panel3.Visible = false;
        //    Panel1.Visible = true;
        //    divgen.Visible = false;
        //    HdnPanel.Value = "Panel1";
        //    txtAwbno.Text = string.Empty;
        //}

        #region Add Empty Row
        //protected void BindEmptyRow()
        //{
        //    DataTable dtEmpty = new DataTable();
        //    try
        //    {
        //        dtEmpty.Columns.Add("TracerNo");
        //        dtEmpty.Columns.Add("AWBDate");
        //        dtEmpty.Columns.Add("AWBNo");
        //        dtEmpty.Columns.Add("Origin");
        //        dtEmpty.Columns.Add("Dest");
        //        dtEmpty.Columns.Add("FltNo");
        //        dtEmpty.Columns.Add("ContentType");
        //        dtEmpty.Columns.Add("SentPcs");
        //        dtEmpty.Columns.Add("ChargebleWgt");
        //        dtEmpty.Columns.Add("MissedPcs");
        //        dtEmpty.Columns.Add("LastRecdStatus");
        //        dtEmpty.Columns.Add("RecdPcs");
        //        dtEmpty.Columns.Add("FoundPcs");
        //        dtEmpty.Columns.Add("LostPcs");
        //        dtEmpty.Columns.Add("FoundAtStcCode");
        //        dtEmpty.Columns.Add("IsGenTracer");
        //        dtEmpty.Columns.Add("IsGenTracer");

        //        DataRow dr = dtEmpty.NewRow();
        //        dr["TracerNo"] = "";
        //        dr["AWBDate"] = "";
        //        dr["AWBNo"] = "";
        //        dr["Origin"] = "";
        //        dr["Dest"] = "";
        //        dr["FltNo"] = "";
        //        dr["ContentType"] = "";
        //        dr["SentPcs"] = "";
        //        dr["ChargebleWgt"] = "";
        //        dr["MissedPcs"] = "";
        //        dr["LastRecdStatus"] = "";
        //        dr["RecdPcs"] = "";
        //        dr["FoundPcs"] = "";
        //        dr["LostPcs"] = "";
        //        dr["FoundAtStcCode"] = "";
        //        dr["IsGenTracer"] = "";
        //        dr["IsClosed"] = "";

        //        dtEmpty.Rows.Add(dr);

        //        grdViewTracer.DataSource = dtEmpty;
        //        grdViewTracer.DataBind();

        //        grdViewTracer.Columns[15].Visible = false;

        //    }
        //    catch (Exception ex)
        //    { }
        //    finally
        //    {
        //        if (dtEmpty != null)
        //            dtEmpty.Dispose();
        //    }
        //}
        #endregion

    }
}

