using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Web.UI.WebControls.WebParts;
using System.Configuration;
using System.Data.SqlClient;
using QID.DataAccess;
using BAL;
using System.Drawing;

/*
 
   2012/05/24 vinayak
   2012-07-10 vinayak

*/
namespace ProjectSmartCargoManager
{
    public partial class MaintainRates : System.Web.UI.Page
    {
        MaintainRatesBAL objBAL = new MaintainRatesBAL();
        ListRateLineBAL objBALList = new ListRateLineBAL();
        string errormessage = "";
        DataSet dsSlabs;
        DataSet dsULDSlabs;
        SQLServer da = new SQLServer(Global.GetConnectionString());
        string UserName;
        int rtlid;
        int rtlid1;

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ViewState["IsAddAnother"] = "F";
                    btnAddAnother.Visible = true;
                    // Fill RateCardDropDown
                    DataSet dsResult = new DataSet();
                    if (objBAL.FillDdl("DdlRateCard", ref dsResult, ref errormessage))
                    {
                        DdlRateCardName.DataSource = null;
                        DdlRateCardName.DataTextField = "RateCardName";
                        DdlRateCardName.DataValueField = "ID";
                        DdlRateCardName.DataSource = dsResult.Tables[0].Copy();
                        DdlRateCardName.DataBind();
                    }

                    //Fill Origin Destination dropdown lists
                    OriginList();
                    DestinationList();
                    BindRepeaterData();
                    LoadGLDropDown();
                    FillCurrencyCodes(TXTCurrency, "");

                    // Fill Default Values ///////////////
                    DataSet dsDefaultValues = new DataSet();
                    if (objBAL.GetDefaultValues(ref dsDefaultValues, ref errormessage))
                    {
                        foreach (DataRow row in dsDefaultValues.Tables[0].Rows)
                        {
                            if (row["ParameterName"].ToString() == "ServiceTax")
                            {
                                TXTTax.Text = "" + row["ParameterValue"].ToString();
                            }
                            else if (row["ParameterName"].ToString() == "RateLineDiscountPercent")
                            {
                                TXTDiscount.Text = "" + row["ParameterValue"].ToString();
                            }
                            else if (row["ParameterName"].ToString() == "RateLineCommissionPercent")
                            {
                                TXTComm.Text = "" + row["ParameterValue"].ToString();
                            }

                        }

                    }

                    dsSlabs = new DataSet();
                    dsSlabs.Tables.Add(new DataTable());
                    dsSlabs.Tables[0].Columns.Add("Type");
                    dsSlabs.Tables[0].Columns.Add("Weight");
                    dsSlabs.Tables[0].Columns.Add("Charge");
                    dsSlabs.Tables[0].Columns.Add("Cost");
                    dsSlabs.Tables[0].Columns.Add("DdlBased");
                    dsSlabs.Tables[0].Columns.Add("SrNo");

                    dsULDSlabs = new DataSet();
                    dsULDSlabs.Tables.Add(new DataTable());
                    dsULDSlabs.Tables[0].Columns.Add("ULDType");
                    dsULDSlabs.Tables[0].Columns.Add("Type");
                    dsULDSlabs.Tables[0].Columns.Add("Weight");
                    dsULDSlabs.Tables[0].Columns.Add("Charge");
                    dsULDSlabs.Tables[0].Columns.Add("SrNo");

                    Session["dsSlabs"] = dsSlabs.Copy();
                    Session["dsULDSlabs"] = dsULDSlabs.Copy();

                    //DataSet dsULDSlabsTemp = new DataSet();
                    ////dsULDSlabsTemp.Tables.Add(dsRateDetails.Tables[3].Copy());
                    //Session["dsULDSlabs"] = dsULDSlabsTemp.Copy();
                    //DataSet dsULDNo = da.SelectRecords("Sp_GetULDNo");
                    //for (int i = 0; i < dsULDNo.Tables[0].Rows.Count; i++)
                    //{
                    //    ((DropDownList)grdULDslabs.Rows[i].FindControl("DdlULDType")).Text = dsULDNo.Tables[0].Rows[i]["ULDNo"].ToString();

                    //}

                    if (Request.QueryString["cmd"] == "Edit" || Request.QueryString["cmd"] == "View")
                    {

                        getRateLineDetails();


                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>hideDiv('<%= ckhULDRate.ClientID %>');</script>", false);

                    }
                    else
                    {
                        AddNewSlab();
                        AddNewSlab();
                        AddNewSlab();
                        AddNewSlab();
                        AddNewULDSlab();
                        AddNewULDSlab();
                        //AddNewULDSlab();
                        //AddNewULDSlab();
                    }

                    RBExAC.Checked = true;
                    RBExCC.Checked = true;
                    RBExAD.Checked = true;
                    RBExFC.Checked = true;
                    RBExFN.Checked = true;
                    RBExHC.Checked = true;
                    RBExSC.Checked = true;
                    rbWeekdaysExclude.Checked = true;
                    rbDepIntExclude.Checked = true;
                    RBExPT.Checked = true;
                    RBExTS.Checked = true;
                    RBExIC.Checked = true;
                    RBExET.Checked = true;
                    if (ckhULDRate.Checked == true)
                    {
                        //btnULDAdd.Visible = true;
                        //btnULDDel.Visible = true;
                        //grdULDslabs.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivShow();</script>", false);

                    }
                    else
                    {

                        //btnULDAdd.Visible = false;
                        //btnULDDel.Visible = false;
                        //grdULDslabs.Visible = false;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivHide();</script>", false);

                    }

                    //Session["FlightNumber"] = "";
                    //Session["AirlineCode"] = "";
                    //Session["CommCode"] = "";
                    //Session["AgentCode"] = "";
                    //Session["Shipper"] = "";
                    //Session["mode"] = "";

                    if (Request.QueryString["cmd"] == "Edit")
                    {

                        btnBack.Visible = true;
                        btnSave.Text = "Update";
                        btnCancel.Visible = false;
                        Session["mode"] = "Edit";


                    }
                    else if (Request.QueryString["cmd"] == "View")
                    {

                        btnBack.Visible = true;
                        //TXTProductType.Text = false;
                        //TXTTransitStation.Text = false;
                        btnSave.Visible = false;
                        btnCancel.Visible = false;
                        Session["mode"] = "View";
                        disableForView();
                    }
                    else
                    {
                        btnSave.Text = "Save";
                        btnBack.Visible = false;
                        btnSave.Visible = true;
                        btnCancel.Visible = true;
                        Session["mode"] = "";
                    }
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>hideDiv('<%= ckhULDRate.ClientID %>');</script>", false);
                    //ClientScript.RegisterStartupScript(GetType(), "hideDiv('<%# ckhULDRate.ClientID %>')", "alert('hi!')", true);

                }

            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Origin List
        private void OriginList()
        {
            try
            {
                DataSet ds = objBALList.GetOriginList(DdlOriginLevel.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            ddlOrigin.DataSource = ds;
                            ddlOrigin.DataMember = ds.Tables[0].TableName;
                            ddlOrigin.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            ddlOrigin.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                            ddlOrigin.DataBind();
                            ddlOrigin.Items.Insert(0, new ListItem("Select", string.Empty));

                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion Origin List

        #region Destination List

        private void DestinationList()
        {
            try
            {
                DataSet ds = objBALList.GetDestinationList(DdlDestinationLevel.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            ddlDestination.DataSource = ds;
                            ddlDestination.DataMember = ds.Tables[0].TableName;
                            ddlDestination.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            ddlDestination.DataTextField = ds.Tables[0].Columns[0].ColumnName;
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



        #endregion Destination List

        #region getRateLineDetails
        public void getRateLineDetails()
        {
            string rateLineId = Request.QueryString["RCName"].ToString();
            DataSet dsRateCard = new DataSet();
            dsRateCard = objBAL.GetRateLine(rateLineId);
            fillRateLineDetails(dsRateCard);


        }
        #endregion getRateLineDetails

        #region Fill RateLine Grid

        public void fillRateLineGrid(string rateLineId)
        {
            DataSet ds = new DataSet();
            ds = da.SelectRecords("SP_GetRateLine", "RateLineId", rateLineId, SqlDbType.Int);





        }
        #endregion

        protected void grdListStock_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dsResult = (DataSet)Session["dsRateDetails"];

            grdListStock.PageIndex = e.NewPageIndex;
            grdListStock.DataSource = dsResult.Tables[4].Copy();
            grdListStock.DataBind();

        }

        #region fillRateLineDetails
        public void fillRateLineDetails(DataSet dsRateDetails)
        {
            //Code to fill details of rate line from dataset
            HidSrNo.Value = dsRateDetails.Tables[0].Rows[0]["RateLineSrNo"].ToString();
            rtlid1 = Convert.ToInt32(HidSrNo.Value);
            DdlRateCardName.SelectedValue = dsRateDetails.Tables[0].Rows[0]["RateCardSrNo"].ToString();
            DdlOriginLevel.SelectedIndex = Convert.ToInt32(dsRateDetails.Tables[0].Rows[0]["OriginLevel"].ToString());
            //TxtOrigin.Text = dsRateDetails.Tables[0].Rows[0]["Origin"].ToString();
            if (DdlOriginLevel.SelectedIndex > 0)
            {
                DdlOriginLevel_SelectedIndexChanged(null, null);
            }
            ddlOrigin.SelectedValue = dsRateDetails.Tables[0].Rows[0]["Origin"].ToString();

            //Session["Origin"] = dsRateDetails.Tables[0].Rows[0]["Origin"].ToString();
            DdlDestinationLevel.SelectedIndex = Convert.ToInt32(dsRateDetails.Tables[0].Rows[0]["DestinationLevel"].ToString());
            //TXTDestination.Text = dsRateDetails.Tables[0].Rows[0]["Destination"].ToString();
            if (DdlDestinationLevel.SelectedIndex > 0)
            {
                DdlDestinationLevel_SelectedIndexChanged(null, null);
            }
            ddlDestination.SelectedValue = dsRateDetails.Tables[0].Rows[0]["Destination"].ToString();

            //Session["Destination"] = dsRateDetails.Tables[0].Rows[0]["Destination"].ToString();
            TXTContrRef.Text = dsRateDetails.Tables[0].Rows[0]["ContrRef"].ToString();
            //FillCurrencyCodes(TXTCurrency, dsRateDetails.Tables[0].Rows[0]["CurrencyCode"].ToString());
            try
            {
                TXTCurrency.SelectedIndex = TXTCurrency.Items.IndexOf(TXTCurrency.Items.FindByText(dsRateDetails.Tables[0].Rows[0]["CurrencyCode"].ToString()));
                TXTCurrency.SelectedValue = dsRateDetails.Tables[0].Rows[0]["CurrencyCode"].ToString();// dsRateDetails.Tables[0].Rows[0]["CurrencyID"].ToString();

            }
            catch (Exception ex) { }

            //TXTCurrency.Text = dsRateDetails.Tables[0].Rows[0]["CurrencyCode"].ToString();
            //Session["Currency"] = dsRateDetails.Tables[0].Rows[0]["CurrencyCode"].ToString();
            //TXTCurrency.SelectedValue = dsRateDetails.Tables[0].Rows[0]["CurrencyCode"].ToString();

            DdlStatus.SelectedValue = dsRateDetails.Tables[0].Rows[0]["Status"].ToString();
            DdlRateBasis.SelectedValue = dsRateDetails.Tables[0].Rows[0]["RateBase"].ToString();
            TXTValidFrom.Text = dsRateDetails.Tables[0].Rows[0]["StartDate"].ToString();
            TXTValidTo.Text = dsRateDetails.Tables[0].Rows[0]["EndDate"].ToString();
            TXTComm.Text = dsRateDetails.Tables[0].Rows[0]["AgentCommPercent"].ToString();
            TXTDiscount.Text = dsRateDetails.Tables[0].Rows[0]["MaxDiscountPercent"].ToString();
            TXTTax.Text = dsRateDetails.Tables[0].Rows[0]["ServiceTax"].ToString();
            ckhAllIn.Checked = Convert.ToBoolean(dsRateDetails.Tables[0].Rows[0]["IsALLIn"]);
            ckhTACTRate.Checked = Convert.ToBoolean(dsRateDetails.Tables[0].Rows[0]["isTact"]);
            ckhULDRate.Checked = Convert.ToBoolean(dsRateDetails.Tables[0].Rows[0]["IsULD"]);
            try
            {
                if (dsRateDetails.Tables[0].Rows[0]["RateType"].ToString() != "DOM" && dsRateDetails.Tables[0].Rows[0]["RateType"].ToString() != "INT")
                    ddlRateType.SelectedValue = "";
                else
                    ddlRateType.SelectedValue = dsRateDetails.Tables[0].Rows[0]["RateType"].ToString();
            }
            catch (Exception exObj)
            { }
            try
            {
                if (dsRateDetails.Tables[0].Rows[0]["GLCode"].ToString() == "0")
                    ddlGLCode.Text = "Select";
                else
                ddlGLCode.Text = dsRateDetails.Tables[0].Rows[0]["GLCode"].ToString();
            }
            catch (Exception ex) { }

            ckhHeavyRate.Checked = Convert.ToBoolean(dsRateDetails.Tables[0].Rows[0]["IsHeavy"]);


            if (dsRateDetails != null && dsRateDetails.Tables.Count > 1 && dsRateDetails.Tables[1].Rows.Count > 0)
            {
                for (int intCount = 0; intCount < dsRateDetails.Tables[1].Rows.Count; intCount++)
                {
                    if (dsRateDetails.Tables[1].Rows[intCount]["ParamName"].ToString().Trim() == "FlightNum")
                    {
                        TXTFlightNumber.Text = dsRateDetails.Tables[1].Rows[intCount]["ParamValue"].ToString();

                        if (dsRateDetails.Tables[1].Rows[intCount]["IsInclude"].ToString() == "False")
                            RBExFN.Checked = true;
                        else
                            RBIncFN.Checked = true;
                    }
                    else if (dsRateDetails.Tables[1].Rows[intCount]["ParamName"].ToString().Trim() == "FlightCarrier")
                    {
                        TXTFlightCarrier.Text = dsRateDetails.Tables[1].Rows[intCount]["ParamValue"].ToString();

                        if (dsRateDetails.Tables[1].Rows[intCount]["IsInclude"].ToString() == "False")
                            RBExFC.Checked = true;
                        else
                            RBIncFC.Checked = true;
                    }
                    else if (dsRateDetails.Tables[1].Rows[intCount]["ParamName"].ToString().Trim() == "HandlingCode")
                    {
                        TXTHandlingCode.Text = dsRateDetails.Tables[1].Rows[intCount]["ParamValue"].ToString();

                        if (dsRateDetails.Tables[1].Rows[intCount]["IsInclude"].ToString() == "False")
                            RBExHC.Checked = true;
                        else
                            RBIncHC.Checked = true;
                    }
                    else if (dsRateDetails.Tables[1].Rows[intCount]["ParamName"].ToString().Trim() == "AirlineCode")
                    {
                        TXTAirLineCode.Text = dsRateDetails.Tables[1].Rows[intCount]["ParamValue"].ToString();

                        if (dsRateDetails.Tables[1].Rows[intCount]["IsInclude"].ToString() == "False")
                            RBExAC.Checked = true;
                        else
                            RBIncAC.Checked = true;
                    }
                    else if (dsRateDetails.Tables[1].Rows[intCount]["ParamName"].ToString().Trim() == "CommCode")
                    {
                        TXTIATAComCode.Text = dsRateDetails.Tables[1].Rows[intCount]["ParamValue"].ToString();

                        if (dsRateDetails.Tables[1].Rows[intCount]["IsInclude"].ToString() == "False")
                            RBExCC.Checked = true;
                        else
                            RBIncCC.Checked = true;
                    }
                    else if (dsRateDetails.Tables[1].Rows[intCount]["ParamName"].ToString().Trim() == "AgentCode")
                    {
                        TXTAgentCode.Text = dsRateDetails.Tables[1].Rows[intCount]["ParamValue"].ToString();

                        if (dsRateDetails.Tables[1].Rows[intCount]["IsInclude"].ToString() == "False")
                            RBExAD.Checked = true;
                        else
                            RBIncAD.Checked = true;
                    }
                    else if (dsRateDetails.Tables[1].Rows[intCount]["ParamName"].ToString().Trim() == "ShipperCode")
                    {
                        TXTShipperCode.Text = dsRateDetails.Tables[1].Rows[intCount]["ParamValue"].ToString();

                        if (dsRateDetails.Tables[1].Rows[intCount]["IsInclude"].ToString() == "False")
                            RBExSC.Checked = true;
                        else
                            RBIncSC.Checked = true;
                    }
                    else if (dsRateDetails.Tables[1].Rows[intCount]["ParamName"].ToString().Trim() == "EquipType")
                    {
                        txtEquipmentType.Text = dsRateDetails.Tables[1].Rows[intCount]["ParamValue"].ToString();

                        if (dsRateDetails.Tables[1].Rows[intCount]["IsInclude"].ToString() == "False")
                            RBExET.Checked = true;
                        else
                            RBIncET.Checked = true;
                    }
                    else if (dsRateDetails.Tables[1].Rows[intCount]["ParamName"].ToString().Trim() == "DaysOfWeek")
                    {
                        string wkDayval = dsRateDetails.Tables[1].Rows[intCount]["ParamValue"].ToString();

                        if (wkDayval != "")
                        {
                            for (int iD = 0; iD < 7; iD++)
                            {
                                if (wkDayval[iD] == '1')
                                {
                                    cblWeekdays.Items[iD].Selected = true;
                                }

                            }
                        }
                        rbWeekdaysInclude.Checked = bool.Parse(dsRateDetails.Tables[1].Rows[intCount]["IsInclude"].ToString());
                    }
                    else if (dsRateDetails.Tables[1].Rows[intCount]["ParamName"].ToString().Trim() == "DepInterval")
                    {
                        string DepInt = dsRateDetails.Tables[1].Rows[intCount]["ParamValue"].ToString();

                        if (DepInt != "")
                        {
                            if (Convert.ToInt16(DepInt.Substring(0, 2)) > 11)
                                CtlTimeFrom.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                            else
                                CtlTimeFrom.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;

                            CtlTimeFrom.Hour = Convert.ToInt16(DepInt.Substring(0, 2));
                            CtlTimeFrom.Minute = Convert.ToInt16(DepInt.Substring(3, 2));

                            if (Convert.ToInt16(DepInt.Substring(6, 2)) > 11)
                                CtlTimeTo.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                            else
                                CtlTimeTo.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;

                            CtlTimeTo.Hour = Convert.ToInt16(DepInt.Substring(6, 2));
                            CtlTimeTo.Minute = Convert.ToInt16(DepInt.Substring(9, 2));
                        }

                        rbDepIntInclude.Checked = bool.Parse(dsRateDetails.Tables[1].Rows[intCount]["IsInclude"].ToString());
                    }
                    else if (dsRateDetails.Tables[1].Rows[intCount]["ParamName"].ToString().Trim() == "ProductType")
                    {
                        TXTProductType.Text = dsRateDetails.Tables[1].Rows[intCount]["ParamValue"].ToString();

                        if (dsRateDetails.Tables[1].Rows[intCount]["IsInclude"].ToString() == "False")
                            RBExPT.Checked = true;
                        else
                            RBIncPT.Checked = true;
                    }
                    else if (dsRateDetails.Tables[1].Rows[intCount]["ParamName"].ToString().Trim() == "TransitStation")
                    {
                        TXTTransitStation.Text = dsRateDetails.Tables[1].Rows[intCount]["ParamValue"].ToString();

                        if (dsRateDetails.Tables[1].Rows[intCount]["IsInclude"].ToString() == "False")
                            RBExTS.Checked = true;
                        else
                            RBIncTS.Checked = true;
                    }
                    else if (dsRateDetails.Tables[1].Rows[intCount]["ParamName"].ToString().Trim() == "Proration")
                        txtProPer.Text = dsRateDetails.Tables[1].Rows[intCount]["ParamValue"].ToString();
                    else if (dsRateDetails.Tables[1].Rows[intCount]["ParamName"].ToString().Trim() == "IssueingCarrier")
                    {
                        TXTIssueingCarrier.Text = dsRateDetails.Tables[1].Rows[intCount]["ParamValue"].ToString();
                        if (dsRateDetails.Tables[1].Rows[intCount]["IsInclude"].ToString() == "False")
                            RBExIC.Checked = true;
                        else
                            RBIncIC.Checked = true;
                    }
                    else if (dsRateDetails.Tables[1].Rows[intCount]["ParamName"].ToString().Trim() == "SPAMarkup")
                        TXTSPAMarkup.Text = dsRateDetails.Tables[1].Rows[intCount]["ParamValue"].ToString();
                }
            }

            GRDRateSlabs.DataSource = dsRateDetails.Tables[2];
            GRDRateSlabs.DataBind();

            if (dsRateDetails.Tables[2].Rows.Count > 0)
            {
                for (int i = 0; i < dsRateDetails.Tables[2].Rows.Count; i++)
                {
                    ((CheckBox)GRDRateSlabs.Rows[i].FindControl("CHK")).Checked = true;
                    ((DropDownList)GRDRateSlabs.Rows[i].FindControl("DdlType")).SelectedValue = dsRateDetails.Tables[2].Rows[i]["Type"].ToString();
                    ((DropDownList)GRDRateSlabs.Rows[i].FindControl("DdlBased")).SelectedValue = dsRateDetails.Tables[2].Rows[i]["DdlBased"].ToString();
                    dsRateDetails.Tables[2].Rows[i]["SrNo"] = i;
                }
            }
            else
            {
                AddNewSlab();
                AddNewSlab();
                AddNewSlab();
                AddNewSlab();
            }

            DataSet dsSlabsTemp = new DataSet();
            dsSlabsTemp.Tables.Add(dsRateDetails.Tables[2].Copy());
            Session["dsSlabs"] = dsSlabsTemp.Copy();

            //if (dsRateDetails != null)
            //{
            //    if (dsRateDetails.Tables != null)
            //    {
            //        if (dsRateDetails.Tables.Count > 2)
            //        {
            //            if (dsRateDetails.Tables[3].Rows.Count > 0)
            //            {
            try
            {

                grdULDslabs.DataSource = dsRateDetails.Tables[3];
                grdULDslabs.DataBind();
                if (dsRateDetails.Tables[3].Rows.Count > 0)
                {
                    for (int i = 0; i < dsRateDetails.Tables[3].Rows.Count; i++)
                    {
                        ((CheckBox)grdULDslabs.Rows[i].FindControl("CHKULD")).Checked = true;
                        LoadULDTypes(i);
                        ((DropDownList)grdULDslabs.Rows[i].FindControl("DdlULDType")).SelectedValue = dsRateDetails.Tables[3].Rows[i]["ULDType"].ToString();
                        ((DropDownList)grdULDslabs.Rows[i].FindControl("DdlULDType1")).SelectedValue = dsRateDetails.Tables[3].Rows[i]["Type"].ToString();
                        //((TextBox)grdULDslabs.Rows[i].FindControl("TXTULDWeight")).Text = dsRateDetails.Tables[3].Rows[i]["Weight"].ToString();
                        //((TextBox)grdULDslabs.Rows[i].FindControl("TXTULDCharge")).Text = dsRateDetails.Tables[3].Rows[i]["Charge"].ToString();
                        dsRateDetails.Tables[3].Rows[i]["SrNo"] = i;
                    }
                }
                else
                {
                    AddNewULDSlab();
                    AddNewULDSlab();
                }

                DataSet dsULDSlabsTemp = new DataSet();
                dsULDSlabsTemp.Tables.Add(dsRateDetails.Tables[3].Copy());
                Session["dsULDSlabs"] = dsULDSlabsTemp.Copy();
                //}

            }




            catch (Exception ex)
            {
            }
            if (dsRateDetails != null)
            {
                if (dsRateDetails.Tables != null)
                {
                    if (dsRateDetails.Tables.Count > 0)
                    {
                        if (dsRateDetails.Tables[4].Rows.Count > 0)
                        {
                            grdListStock.DataSource = dsRateDetails;
                            grdListStock.DataMember = dsRateDetails.Tables[4].TableName;
                            grdListStock.DataBind();
                            grdListStock.Visible = true;
                            Session["dsRateDetails"] = dsRateDetails;
                            lblStatus.Text = "";
                        }

                    }

                }

            }

            //        }
            //    }
            //}


            //if (Request.QueryString["AgentCode"] == dsRateDetails.Tables[1].Rows[5]["ParamValue"].ToString())
            //{
            //    RBExAD.Enabled = false;
            //    RBExAD.Enabled = false;
            //    TXTAgentCode.Enabled = false;
            //}




        }
        #endregion fillRateLineDetails

        public void AddNewSlab()
        {
            try
            {
                dsSlabs = (DataSet)Session["dsSlabs"];
                dsSlabs.Tables[0].Rows.Add(dsSlabs.Tables[0].NewRow());

                if (dsSlabs.Tables[0].Rows.Count == 1)
                {
                    dsSlabs.Tables[0].Rows[0]["Type"] = "M";
                    dsSlabs.Tables[0].Rows[0]["SrNo"] = 0;
                    dsSlabs.Tables[0].Rows[0]["Weight"] = 0;
                    dsSlabs.Tables[0].Rows[0]["Charge"] = 0;
                    dsSlabs.Tables[0].Rows[0]["Cost"] = 0;
                    dsSlabs.Tables[0].Rows[0]["DdlBased"] = "B";

                }
                else if (dsSlabs.Tables[0].Rows.Count == 2)
                {
                    dsSlabs.Tables[0].Rows[1]["Type"] = "N";
                    dsSlabs.Tables[0].Rows[1]["SrNo"] = 1;
                    dsSlabs.Tables[0].Rows[1]["Weight"] = 0;
                    dsSlabs.Tables[0].Rows[1]["Charge"] = 0;
                    dsSlabs.Tables[0].Rows[1]["Cost"] = 0;
                    dsSlabs.Tables[0].Rows[1]["DdlBased"] = "B";
                }
                else
                {
                    dsSlabs.Tables[0].Rows[dsSlabs.Tables[0].Rows.Count - 1]["Type"] = "Q";
                    dsSlabs.Tables[0].Rows[dsSlabs.Tables[0].Rows.Count - 1]["SrNo"] = dsSlabs.Tables[0].Rows.Count - 1;
                    dsSlabs.Tables[0].Rows[dsSlabs.Tables[0].Rows.Count - 1]["Weight"] = 0;
                    dsSlabs.Tables[0].Rows[dsSlabs.Tables[0].Rows.Count - 1]["Charge"] = 0;
                    dsSlabs.Tables[0].Rows[dsSlabs.Tables[0].Rows.Count - 1]["Cost"] = 0;
                    dsSlabs.Tables[0].Rows[dsSlabs.Tables[0].Rows.Count - 1]["DdlBased"] = "B";
                }

                GRDRateSlabs.DataSource = null;
                GRDRateSlabs.DataSource = dsSlabs.Tables[0].Copy();
                GRDRateSlabs.DataBind();

                for (int i = 0; i < dsSlabs.Tables[0].Rows.Count; i++)
                {

                    ((DropDownList)GRDRateSlabs.Rows[i].FindControl("DdlType")).Text = dsSlabs.Tables[0].Rows[i]["Type"].ToString();
                    ((DropDownList)GRDRateSlabs.Rows[i].FindControl("DdlBased")).Text = dsSlabs.Tables[0].Rows[i]["DdlBased"].ToString();


                }

                Session["dsSlabs"] = dsSlabs;
                if (ckhULDRate.Checked == true)
                {
                    //btnULDAdd.Visible = true;
                    //btnULDDel.Visible = true;
                    //grdULDslabs.Visible = true;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivShow();</script>", false);

                }
                else
                {

                    //btnULDAdd.Visible = false;
                    //btnULDDel.Visible = false;
                    //grdULDslabs.Visible = false;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivHide();</script>", false);

                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error :" + ex.Message;
            }

        }

        public void AddNewULDSlab()
        {
            try
            {
                dsULDSlabs = (DataSet)Session["dsULDSlabs"];
                dsULDSlabs.Tables[0].Rows.Add(dsULDSlabs.Tables[0].NewRow());

                if (dsULDSlabs.Tables[0].Rows.Count == 1)
                {
                    //dsULDSlabs.Tables[0].Rows[0]["ULDType"] = "AKE";
                    //dsULDSlabs.Tables[0].Rows[0]["Type"] = "M";
                    dsULDSlabs.Tables[0].Rows[0]["SrNo"] = 0;
                    dsULDSlabs.Tables[0].Rows[0]["Weight"] = 0;
                    dsULDSlabs.Tables[0].Rows[0]["Charge"] = 0;

                }
                else if (dsULDSlabs.Tables[0].Rows.Count == 2)
                {
                    //dsULDSlabs.Tables[0].Rows[1]["ULDType"] = "UKA";
                    //dsULDSlabs.Tables[0].Rows[1]["Type"] = "OverPivot";
                    dsULDSlabs.Tables[0].Rows[1]["SrNo"] = 1;
                    dsULDSlabs.Tables[0].Rows[1]["Weight"] = 0;
                    dsULDSlabs.Tables[0].Rows[1]["Charge"] = 0;
                }
                else
                {
                    //dsULDSlabs.Tables[0].Rows[dsULDSlabs.Tables[0].Rows.Count - 1]["ULDType"] = "UKA";
                    //dsULDSlabs.Tables[0].Rows[dsULDSlabs.Tables[0].Rows.Count - 1]["Type"] = "OverPivot";
                    dsULDSlabs.Tables[0].Rows[dsULDSlabs.Tables[0].Rows.Count - 1]["SrNo"] = dsULDSlabs.Tables[0].Rows.Count - 1;
                    dsULDSlabs.Tables[0].Rows[dsULDSlabs.Tables[0].Rows.Count - 1]["Weight"] = 0;
                    dsULDSlabs.Tables[0].Rows[dsULDSlabs.Tables[0].Rows.Count - 1]["Charge"] = 0;
                }

                grdULDslabs.DataSource = null;
                grdULDslabs.DataSource = dsULDSlabs.Tables[0].Copy();
                grdULDslabs.DataBind();

                for (int i = 0; i < dsULDSlabs.Tables[0].Rows.Count; i++)
                {
                    LoadULDTypes(i);
                    ((DropDownList)grdULDslabs.Rows[i].FindControl("DdlULDType")).Text = dsULDSlabs.Tables[0].Rows[i]["ULDType"].ToString();

                }
                for (int i = 0; i < dsULDSlabs.Tables[0].Rows.Count; i++)
                {
                    ((DropDownList)grdULDslabs.Rows[i].FindControl("DdlULDType1")).Text = dsULDSlabs.Tables[0].Rows[i]["Type"].ToString();

                }

                Session["dsULDSlabs"] = dsULDSlabs;
                if (ckhULDRate.Checked == true)
                {
                    //btnULDAdd.Visible = true;
                    //btnULDDel.Visible = true;
                    //grdULDslabs.Visible = true;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivShow();</script>", false);

                }
                else
                {

                    //btnULDAdd.Visible = false;
                    //btnULDDel.Visible = false;
                    //grdULDslabs.Visible = false;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivHide();</script>", false);

                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error :" + ex.Message;
            }

        }

        protected void LBNAdd_OnClick(object sender, EventArgs e)
        {
            SaveGridState();

            AddNewSlab();
        }

        public bool SaveGridState()
        {
            dsSlabs = (DataSet)Session["dsSlabs"];

            for (int i = 0; i < dsSlabs.Tables[0].Rows.Count; i++)
            {
                dsSlabs.Tables[0].Rows[i]["Type"] = ((DropDownList)GRDRateSlabs.Rows[i].FindControl("DdlType")).Text;
                dsSlabs.Tables[0].Rows[i]["Weight"] = ((TextBox)GRDRateSlabs.Rows[i].FindControl("TXTWeight")).Text;
                dsSlabs.Tables[0].Rows[i]["Charge"] = ((TextBox)GRDRateSlabs.Rows[i].FindControl("TXTCharge")).Text;
                dsSlabs.Tables[0].Rows[i]["Cost"] = ((TextBox)GRDRateSlabs.Rows[i].FindControl("TXTCost")).Text;
                dsSlabs.Tables[0].Rows[i]["DdlBased"] = ((DropDownList)GRDRateSlabs.Rows[i].FindControl("DdlBased")).SelectedValue;

                if (dsSlabs.Tables[0].Rows[i]["Weight"].ToString().Trim() == "")
                    dsSlabs.Tables[0].Rows[i]["Weight"] = "0";

                if (dsSlabs.Tables[0].Rows[i]["Charge"].ToString().Trim() == "")
                    dsSlabs.Tables[0].Rows[i]["Charge"] = "0";

                //Added by Poorna for issue 10

                if (ckhULDRate.Checked == false)
                {
                    if (Convert.ToString(dsSlabs.Tables[0].Rows[i]["Type"]) == "M")
                    {
                        if (Convert.ToInt32(dsSlabs.Tables[0].Rows[i]["Weight"]) != 0)
                        {
                            lblStatus.Text = "Minimum weight should be 0.";
                            lblStatus.ForeColor = Color.Blue;
                            return false;
                        }

                        /* Issue raised on 28/09/2013 
                         * Need to add minimim charge as 0
                         * if (Convert.ToInt32(dsSlabs.Tables[0].Rows[i]["Charge"]) == 0)
                          {
                              lblStatus.Text = "Minimum charge should not be 0.";
                              lblStatus.ForeColor = Color.Blue;
                              return false;
                          }*/
                    }

                    if (Convert.ToString(dsSlabs.Tables[0].Rows[i]["Type"]) == "N")
                    {
                        if (Convert.ToInt32(dsSlabs.Tables[0].Rows[i]["Weight"]) != 0)
                        {
                            lblStatus.Text = "Normal weight should be 0.";
                            lblStatus.ForeColor = Color.Blue;
                            return false;
                        }

                        /* 24/06/2014 Normal charge can be zero
                         * if (Convert.ToDecimal(dsSlabs.Tables[0].Rows[i]["Charge"]) == 0)
                        {
                            lblStatus.Text = "Normal charge should not be 0.";
                            lblStatus.ForeColor = Color.Blue;
                            return false;
                        }*/
                    }
                }
                //End
            }

            Session["dsSlabs"] = dsSlabs.Copy();
            return true;
        }

        public bool SaveGridULDState()
        {
            dsULDSlabs = (DataSet)Session["dsULDSlabs"];

            for (int i = 0; i < dsULDSlabs.Tables[0].Rows.Count; i++)
            {
                dsULDSlabs.Tables[0].Rows[i]["ULDType"] = ((DropDownList)grdULDslabs.Rows[i].FindControl("DdlULDType")).Text;
                dsULDSlabs.Tables[0].Rows[i]["Type"] = ((DropDownList)grdULDslabs.Rows[i].FindControl("DdlULDType1")).Text;
                dsULDSlabs.Tables[0].Rows[i]["Weight"] = ((TextBox)grdULDslabs.Rows[i].FindControl("TXTULDWeight")).Text;
                dsULDSlabs.Tables[0].Rows[i]["Charge"] = ((TextBox)grdULDslabs.Rows[i].FindControl("TXTULDCharge")).Text;

                if (dsULDSlabs.Tables[0].Rows[i]["Weight"].ToString().Trim() == "")
                    dsULDSlabs.Tables[0].Rows[i]["Weight"] = "0";

                if (dsULDSlabs.Tables[0].Rows[i]["Charge"].ToString().Trim() == "")
                    dsULDSlabs.Tables[0].Rows[i]["Charge"] = "0";


                if (ckhULDRate.Checked == true)
                {
                    if (Convert.ToString(dsULDSlabs.Tables[0].Rows[i]["Type"]) == "M")
                    {
                        if (Convert.ToInt32(dsULDSlabs.Tables[0].Rows[i]["Weight"]) == 0)
                        {
                            lblStatus.Text = "Minimum ULD weight should not be 0.";
                            lblStatus.ForeColor = Color.Blue;
                            return false;
                        }

                        if (Convert.ToDecimal(dsULDSlabs.Tables[0].Rows[i]["Charge"]) == 0)
                        {
                            lblStatus.Text = "Minimum ULD charge should not be 0.";
                            lblStatus.ForeColor = Color.Blue;
                            return false;
                        }
                    }

                    if (Convert.ToString(dsULDSlabs.Tables[0].Rows[i]["Type"]) == "OverPivot")
                    {
                        if (Convert.ToInt32(dsULDSlabs.Tables[0].Rows[i]["Weight"]) == 0)
                        {
                            lblStatus.Text = "Normal ULD weight should nto be 0.";
                            lblStatus.ForeColor = Color.Blue;
                            return false;
                        }

                        if (Convert.ToDecimal(dsULDSlabs.Tables[0].Rows[i]["Charge"]) == 0)
                        {
                            lblStatus.Text = "Normal ULD charge should not be 0.";
                            lblStatus.ForeColor = Color.Blue;
                            return false;
                        }
                    }
                }
                //End
            }

            Session["dsULDSlabs"] = dsULDSlabs.Copy();
            return true;
        }

        protected void LBNDelete_OnClick(object sender, EventArgs e)
        {

            SaveGridState();

            dsSlabs = (DataSet)Session["dsSlabs"];
            DataSet dsSlabsTemp = dsSlabs.Copy();

            for (int i = 0; i < dsSlabs.Tables[0].Rows.Count; i++)
            {
                if (((CheckBox)GRDRateSlabs.Rows[i].FindControl("CHK")).Checked)
                {
                    string srno = dsSlabs.Tables[0].Rows[i]["SrNo"].ToString();

                    for (int j = 0; j < dsSlabsTemp.Tables[0].Rows.Count; j++)
                    {
                        if (srno == dsSlabsTemp.Tables[0].Rows[j]["SrNo"].ToString())
                        {
                            dsSlabsTemp.Tables[0].Rows.Remove(dsSlabsTemp.Tables[0].Rows[j]);
                            break;
                        }
                    }
                }
            }

            GRDRateSlabs.DataSource = null;
            GRDRateSlabs.DataSource = dsSlabsTemp.Tables[0];
            GRDRateSlabs.DataBind();

            for (int i = 0; i < dsSlabsTemp.Tables[0].Rows.Count; i++)
            {

                ((DropDownList)GRDRateSlabs.Rows[i].FindControl("DdlType")).Text = dsSlabsTemp.Tables[0].Rows[i]["Type"].ToString();
                ((DropDownList)GRDRateSlabs.Rows[i].FindControl("DdlBased")).Text = dsSlabsTemp.Tables[0].Rows[i]["DdlBased"].ToString();

            }

            Session["dsSlabs"] = dsSlabsTemp.Copy();
            if (ckhULDRate.Checked == true)
            {
                //btnULDAdd.Visible = true;
                //btnULDDel.Visible = true;
                //grdULDslabs.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivShow();</script>", false);

            }
            else
            {

                //btnULDAdd.Visible = false;
                //btnULDDel.Visible = false;
                //grdULDslabs.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivHide();</script>", false);

            }

        }

        public bool ValidateFields()
        {
            try
            {

                //if (TxtOrigin.Text.Trim() == "")
                //{
                //    //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "alert('Fill Origin.');", true);
                //    lblStatus.Text = "Fill Origin";
                //    lblStatus.ForeColor = Color.Blue;
                //    return false;
                //}
                if (ddlOrigin.SelectedIndex == 0)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "alert('Fill Origin.');", true);
                    lblStatus.Text = "Please select Origin";
                    lblStatus.ForeColor = Color.Blue;

                    //added by jayant for ULD Grid
                    if (ckhULDRate.Checked == true)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivShow();</script>", false);

                    }
                    else
                    {


                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivHide();</script>", false);

                    }
                    return false;
                    //end
                }
                //if (TXTDestination.Text.Trim() == "")
                //{
                //    //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "alert('Fill Destination.');", true);
                //    lblStatus.Text = "Fill Destination";
                //    lblStatus.ForeColor = Color.Blue;
                //    return false;
                //}
                if (ddlDestination.SelectedIndex == 0)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "alert('Fill Destination.');", true);
                    lblStatus.Text = "Please select Destination";
                    lblStatus.ForeColor = Color.Blue;
                    //added by jayant for ULD Grid
                    if (ckhULDRate.Checked == true)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivShow();</script>", false);

                    }
                    else
                    {


                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivHide();</script>", false);

                    }
                    //end
                    return false;

                }
                if (DdlOriginLevel.SelectedIndex < 2 || DdlDestinationLevel.SelectedIndex < 2)
                {
                    if (ddlOrigin.SelectedItem.Text.Trim() == ddlDestination.SelectedItem.Text.Trim())
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "alert('Origin and Destination can not be same.');", true);
                        lblStatus.Text = "Origin and Destination can not be same.";
                        lblStatus.ForeColor = Color.Blue;
                        //added by jayant for ULD Grid
                        if (ckhULDRate.Checked == true)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivShow();</script>", false);

                        }
                        else
                        {


                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivHide();</script>", false);

                        }
                        //end
                        return false;
                    }
                }
                if (TXTCurrency.Text.Trim() == "")
                {

                    //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "alert('Fill Currency.');", true);
                    lblStatus.Text = "Please enter valid Currency.";
                    lblStatus.ForeColor = Color.Blue;
                    //added by jayant for ULD Grid
                    if (ckhULDRate.Checked == true)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivShow();</script>", false);

                    }
                    else
                    {


                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivHide();</script>", false);

                    }
                    //end
                    return false;
                }
                if (TXTValidFrom.Text.Trim() == "")
                {

                    //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "alert('Fill Valid from.');", true);
                    lblStatus.Text = "Please enter valid From Date.";
                    lblStatus.ForeColor = Color.Blue;
                    //added by jayant for ULD Grid
                    if (ckhULDRate.Checked == true)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivShow();</script>", false);

                    }
                    else
                    {


                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivHide();</script>", false);

                    }
                    //end
                    return false;
                }
                if (TXTValidTo.Text.Trim() == "")
                {

                    //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "alert('Fill Valid to.');", true);
                    lblStatus.Text = "Please enter valid To Date.";
                    lblStatus.ForeColor = Color.Blue;
                    //added by jayant for ULD Grid
                    if (ckhULDRate.Checked == true)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivShow();</script>", false);

                    }
                    else
                    {


                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivHide();</script>", false);

                    }
                    //end
                    return false;

                }
                if (DdlRateCardName.SelectedItem.Text == "SPA")
                {

                    if (TXTIssueingCarrier.Text == "")
                    {
                        lblStatus.Text = "Please enter Issueing Carrier.";
                        lblStatus.ForeColor = Color.Blue;
                        //added by jayant for ULD Grid
                        if (ckhULDRate.Checked == true)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivShow();</script>", false);

                        }
                        else
                        {


                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivHide();</script>", false);

                        }
                        //end
                        return false;
                    }
                    if (TXTFlightCarrier.Text == "")
                    {
                        lblStatus.Text = "Please enter Flight Carrier.";
                        lblStatus.ForeColor = Color.Blue;
                        //added by jayant for ULD Grid
                        if (ckhULDRate.Checked == true)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivShow();</script>", false);

                        }
                        else
                        {


                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivHide();</script>", false);

                        }
                        //end
                        return false;
                    }
                }

                DateTime dtfrom, dtto;
                //DateTime todayDt = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                try
                {
                   // dtfrom = Convert.ToDateTime(TXTValidFrom.Text);
                    dtfrom = DateTime.ParseExact(TXTValidFrom.Text, "dd/MM/yyyy", null);
                    //if (dtfrom < todayDt)
                    //{
                    //    lblStatus.Text = "Rate Line can be created from current date";
                    //    lblStatus.ForeColor = Color.Red;
                    //    return false;
                    //}


                }
                catch (Exception)
                {
                    lblStatus.Text = "From Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }


                try
                {
                    //dtfrom = Convert.ToDateTime(TXTValidFrom.Text);
                    dtfrom = DateTime.ParseExact(TXTValidFrom.Text, "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Valid from date format invalid');</SCRIPT>");
                    lblStatus.Text = "From Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }

                try
                {
                   // dtto = Convert.ToDateTime(TXTValidTo.Text);
                    dtto = DateTime.ParseExact(TXTValidTo.Text, "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Valid to date format invalid');</SCRIPT>");
                    lblStatus.Text = "To Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }




                if (dtto < dtfrom)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Valid to date should be smaller than Valid from date');</SCRIPT>");
                    lblStatus.Text = "Valid to date should be greater than Valid from date";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }



                if (TXTComm.Text.Trim() == "")
                    TXTComm.Text = "0";


                if (TXTDiscount.Text.Trim() == "")
                    TXTDiscount.Text = "0";

                if (TXTTax.Text.Trim() == "")
                {

                    //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "alert('Fill Tax.');", true);
                    lblStatus.Text = "Fill Tax";
                    lblStatus.ForeColor = Color.Blue;
                    return false;

                }

                //Added by Poorna
                if (!DdlRateBasis.Text.Equals("RC", StringComparison.OrdinalIgnoreCase))
                {
                    if (RBIncFN.Checked == true && TXTFlightNumber.Text.Trim() == "")
                    {
                        lblStatus.Text = "Please enter Flight number to include.";
                        lblStatus.ForeColor = Color.Blue;
                        return false;
                    }

                    if (RBIncFC.Checked == true && TXTFlightCarrier.Text.Trim() == "")
                    {
                        lblStatus.Text = "Please enter Flight carrier to include.";
                        lblStatus.ForeColor = Color.Blue;
                        return false;
                    }

                    if (RBIncHC.Checked == true && TXTHandlingCode.Text.Trim() == "")
                    {
                        lblStatus.Text = "Please enter Handling code to include.";
                        lblStatus.ForeColor = Color.Blue;
                        return false;
                    }

                    if (RBIncAC.Checked == true && TXTAirLineCode.Text.Trim() == "")
                    {
                        lblStatus.Text = "Please enter Airline code to include.";
                        lblStatus.ForeColor = Color.Blue;
                        return false;
                    }

                    if (RBIncCC.Checked == true && TXTIATAComCode.Text.Trim() == "")
                    {
                        lblStatus.Text = "Please enter IATA Commodity code to include.";
                        lblStatus.ForeColor = Color.Blue;
                        return false;
                    }

                    if (RBIncAD.Checked == true && TXTAgentCode.Text.Trim() == "")
                    {
                        lblStatus.Text = "Please enter Agent code to include.";
                        lblStatus.ForeColor = Color.Blue;
                        return false;
                    }

                    if (RBIncSC.Checked == true && TXTShipperCode.Text.Trim() == "")
                    {
                        lblStatus.Text = "Please enter Shipper code to include.";
                        lblStatus.ForeColor = Color.Blue;
                        return false;
                    }

                    //End


                    if (SaveGridState() == false)
                        return false;

                    DataSet dsSlabs = (DataSet)Session["dsSlabs"];

                    if (dsSlabs.Tables[0].Rows.Count == 1 && (dsSlabs.Tables[0].Rows[0]["Charge"].ToString().Trim() == "" || dsSlabs.Tables[0].Rows[0]["Charge"].ToString().Trim() == "0"))
                    {

                        //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "alert('Fill Slabs.');", true);
                        lblStatus.Text = "Fill Slabs";
                        lblStatus.ForeColor = Color.Blue;
                        return false;

                    }

                    DataSet dsULDSlabs = (DataSet)Session["dsULDSlabs"];

                    if (dsULDSlabs.Tables[0].Rows.Count == 1 && (dsULDSlabs.Tables[0].Rows[0]["Charge"].ToString().Trim() == "" || dsULDSlabs.Tables[0].Rows[0]["Charge"].ToString().Trim() == "0"))
                    {

                        //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "alert('Fill Slabs.');", true);
                        lblStatus.Text = "Fill ULD Slabs";
                        lblStatus.ForeColor = Color.Blue;
                        return false;

                    }


                    // Validate duplicate/////////////////////////

                    int rateLineSrNo = 0;
                    if (Request.QueryString["cmd"] == "Edit" && ViewState["IsAddAnother"].ToString() == "F")
                        rateLineSrNo = Convert.ToInt32(HidSrNo.Value);

                    string wkDaysval = string.Empty;
                    for (int i = 0; i < cblWeekdays.Items.Count; i++)
                    {
                        if (cblWeekdays.Items[i].Selected == true)
                        {
                            wkDaysval += '1';
                        }
                        else
                            wkDaysval += '0';
                    }
                    string DepInervalFrom = CtlTimeFrom.Hour.ToString().PadLeft(2, '0') + ":" + CtlTimeFrom.Minute.ToString().PadLeft(2, '0');
                    DepInervalFrom = DepInervalFrom + "-" + CtlTimeTo.Hour.ToString().PadLeft(2, '0') + ":" + CtlTimeTo.Minute.ToString().PadLeft(2, '0');

                    DateTime FromDt1 = new DateTime();
                    DateTime ToDt1 = new DateTime();

                    FromDt1 = DateTime.ParseExact(TXTValidFrom.Text.Trim(), "dd/MM/yyyy", null);
                    ToDt1 = DateTime.ParseExact(TXTValidTo.Text.Trim(), "dd/MM/yyyy", null);

                    bool IsDuplicate = false;
                    string errormessage = "";
                    if (Request.QueryString["cmd"] != "Edit" || Request.QueryString["cmd"] != "View")
                    {
                        //object[] values = { int.Parse(DdlRateCardName.SelectedValue.ToString()), DdlOriginLevel.SelectedIndex,
                        //                    TxtOrigin.Text,DdlDestinationLevel.SelectedIndex ,TXTDestination.Text,
                        //                    Convert.ToDateTime(TXTValidFrom.Text).ToString("yyyy-MM-dd HH:mm:ss"),
                        //                    Convert.ToDateTime(TXTValidTo.Text).ToString("yyyy-MM-dd HH:mm:ss"),
                        //                    TXTFlightNumber.Text,TXTFlightCarrier.Text,TXTHandlingCode.Text,TXTAirLineCode.Text,TXTIATAComCode.Text,TXTAgentCode.Text,TXTShipperCode.Text,
                        //                    RBIncFN.Checked,RBIncFC.Checked,RBIncHC.Checked,RBIncAC.Checked,RBIncCC.Checked,RBIncAD.Checked,RBIncSC.Checked,
                        //                    rateLineSrNo
                        //                  };

                        object[] values = { int.Parse(DdlRateCardName.SelectedValue.ToString()), DdlOriginLevel.SelectedIndex,
                                    ddlOrigin.SelectedItem.Text,DdlDestinationLevel.SelectedIndex ,ddlDestination.SelectedItem.Text,
                                    //Convert.ToDateTime(TXTValidFrom.Text).ToString("yyyy-MM-dd HH:mm:ss"),
                                    //Convert.ToDateTime(TXTValidFrom.Text).ToString("dd/MM/yyyy"),
                                    FromDt1,
                                    //Convert.ToDateTime(TXTValidTo.Text).ToString("yyyy-MM-dd HH:mm:ss"),
                                    //Convert.ToDateTime(TXTValidTo.Text).ToString("dd/MM/yyyy"),
                                    ToDt1,
                                    TXTFlightNumber.Text,TXTFlightCarrier.Text,TXTHandlingCode.Text,TXTAirLineCode.Text,TXTIATAComCode.Text,TXTAgentCode.Text,TXTShipperCode.Text,
                                    RBIncFN.Checked,RBIncFC.Checked,RBIncHC.Checked,RBIncAC.Checked,RBIncCC.Checked,RBIncAD.Checked,RBIncSC.Checked,
                                    rateLineSrNo,TXTContrRef.Text,TXTCurrency.Text,DdlStatus.SelectedValue,DdlRateBasis.SelectedValue,TXTComm.Text,
                                    TXTDiscount.Text,TXTTax.Text,ckhAllIn.Checked,ckhTACTRate.Checked,ckhULDRate.Checked,ckhHeavyRate.Checked,
                                    wkDaysval,rbWeekdaysInclude.Checked, DepInervalFrom, rbDepIntInclude.Checked,TXTProductType.Text,TXTTransitStation.Text,RBIncPT.Checked,RBIncTS.Checked,
                                    txtProPer.Text.Trim(),RBInPro.Checked,TXTIssueingCarrier.Text,RBIncIC.Checked,TXTSPAMarkup.Text,txtEquipmentType.Text,RBIncET.Checked
                                  };

                        if (objBAL.CheckDuplicate(values, ref IsDuplicate, ref errormessage))
                        {
                            if (IsDuplicate)
                            {
                                lblStatus.Text = "RateLine already exists.";
                                lblStatus.ForeColor = Color.Red;
                                return false;
                            }
                        }
                        else
                        {
                            lblStatus.Text = "" + errormessage;
                            lblStatus.ForeColor = Color.Red;
                            return false;
                        }
                    }
                    //////////////////////////////////////////////
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
               
                if (!ValidateFields())
                    return;
                if (DdlRateBasis.Text.Equals("Relative Charge", StringComparison.OrdinalIgnoreCase) || DdlRateBasis.Text.Equals("RC", StringComparison.OrdinalIgnoreCase))
                {
                    AddSpecialCommodity();
                    return;
                }
                if (SaveGridState() == false)
                    return;
                if (SaveGridULDState() == false)
                    return;

                //Code to differntiate SAVE or UPDATE
                int rateLineSrNo;
                //to Update Rate Card
                //if (HidSrNo.Value != "")
                if (Request.QueryString["cmd"] == "Edit" && ViewState["IsAddAnother"].ToString() == "F")
                    rateLineSrNo = Convert.ToInt32(HidSrNo.Value);
                //to Save Rate Card
                else
                    rateLineSrNo = 0;

                string wkDaysval = string.Empty;
                for (int i = 0; i < cblWeekdays.Items.Count; i++)
                {
                    if (cblWeekdays.Items[i].Selected == true)
                    {
                        wkDaysval += '1';
                    }
                    else
                        wkDaysval += '0';
                }

                string DepInervalFrom = string.Empty;

                if (CtlTimeFrom.Hour == CtlTimeTo.Hour && CtlTimeFrom.Minute == CtlTimeTo.Minute)
                    DepInervalFrom = string.Empty;
                else
                {
                    DepInervalFrom = CtlTimeFrom.Hour.ToString().PadLeft(2, '0') + ":" + CtlTimeFrom.Minute.ToString().PadLeft(2, '0');
                    DepInervalFrom = DepInervalFrom + "-" + CtlTimeTo.Hour.ToString().PadLeft(2, '0') + ":" + CtlTimeTo.Minute.ToString().PadLeft(2, '0');
                }

                if (wkDaysval.IndexOf('1') == -1)
                    wkDaysval = string.Empty;


                string[] param = {"RateCardSrNo","OriginLevel","Origin","DestinationLevel",
                                  "Destination","ContrRef","CurrencyID","Status","StartDate","EndDate",
                                  "RateBase","AgentCommPercent","MaxDiscountPercent","ServiceTax",
                                  "FlightNumber","FlightCarrier","HandlingCode","AirlineCode","IATACommCode","AgentCode","ShipperCode",
                                  "FNInc","FCInc","HCInc","ACInc","CCInc","ADInc","SCInc","RateLineSrNo",
                                  "wkDays","wkDaysInc","DepInterval","DepIntervalInc", "IsAllIn","ProductType","TransitStation","PTInc","TSInc","TACTRate",
                                  "Proration","PROInc","IsULD","IsHeavy","IssueingCarrier","ICInc","SPAMarkup","EquipType","ETInc","GLCode","UpdatedBy","UpdatedOn","rateType"
                                };

                SqlDbType[] dbtypes = { SqlDbType.Int, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.Int,
                                        SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.DateTime,SqlDbType.DateTime,
                                        SqlDbType.VarChar,SqlDbType.Float,SqlDbType.Float,SqlDbType.Float,SqlDbType.VarChar,
                                        SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,
                                        SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Int,
                                        SqlDbType.VarChar, SqlDbType.Bit, SqlDbType.VarChar, SqlDbType.Bit, SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.VarChar, 
                                        SqlDbType.Bit, SqlDbType.Bit,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.VarChar,
                                        SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.DateTime,SqlDbType.VarChar
                                      };

                 object[] values = new object[52];
                 values[0] =  int.Parse(DdlRateCardName.SelectedValue.ToString());
                 values[1]= DdlOriginLevel.SelectedIndex;
                 values[2]=ddlOrigin.SelectedItem.Text;
                 values[3]= DdlDestinationLevel.SelectedIndex;
                 values[4]=ddlDestination.SelectedItem.Text;
                 values[5]= TXTContrRef.Text;
                 values[6]=TXTCurrency.SelectedValue;
                 values[7]=DdlStatus.SelectedValue;

                 //values[8]=Convert.ToDateTime(TXTValidFrom.Text).ToString("yyyy-MM-dd HH:mm:ss");
                 values[8] = DateTime.ParseExact(TXTValidFrom.Text, "dd/MM/yyyy", null);
                 //values[9]= Convert.ToDateTime(TXTValidTo.Text).ToString("yyyy-MM-dd HH:mm:ss");
                 values[9] = DateTime.ParseExact(TXTValidTo.Text, "dd/MM/yyyy", null);

                 values[10]=DdlRateBasis.Text;
                 values[11]= Convert.ToDouble(TXTComm.Text);
                 values[12]=Convert.ToDouble(TXTDiscount.Text);
                 values[13]=Convert.ToDouble(TXTTax.Text);
                 values[14]= TXTFlightNumber.Text;
                 values[15]= TXTFlightCarrier.Text;
                 values[16]=TXTHandlingCode.Text;
                 values[17]=TXTAirLineCode.Text;
                 values[18]=TXTIATAComCode.Text;
                 values[19]=TXTAgentCode.Text;
                 values[20]=TXTShipperCode.Text;
                 values[21]= RBIncFN.Checked;
                 values[22]=RBIncFC.Checked;
                 values[23]=RBIncHC.Checked;
                 values[24]= RBIncAC.Checked;
                 values[25]=RBIncCC.Checked;
                 values[26]=RBIncAD.Checked;
                 values[27]= RBIncSC.Checked;
                 values[28]=rateLineSrNo;
                 values[29]=wkDaysval;
                 values[30]=rbWeekdaysInclude.Checked;
                 values[31]=DepInervalFrom;
                 values[32]=rbDepIntInclude.Checked; 
                 values[33]=ckhAllIn.Checked;                      
                 values[34]=TXTProductType.Text;
                 values[35]=TXTTransitStation.Text;
                 values[36]=RBIncPT.Checked;
                 values[37]=RBIncTS.Checked;
                 values[38]=ckhTACTRate.Checked;
                 values[39]=txtProPer.Text.Trim();
                 values[40]=RBInPro.Checked;
                 values[41]=ckhULDRate.Checked;
                 values[42]=ckhHeavyRate.Checked;
                 values[43]=TXTIssueingCarrier.Text;
                 values[44]=RBIncIC.Checked;
                 values[45]=  TXTSPAMarkup.Text;
                 values[46]=txtEquipmentType.Text.Trim();
                 values[47]=RBIncET.Checked;
                 if (ddlGLCode.SelectedItem.Text == null||ddlGLCode.SelectedItem.Text.ToLower()=="select")
                     values[48] = 0;
                 else
                 values[48]=ddlGLCode.SelectedItem.Text;
                 values[49]=Session["UserName"];
                 values[50]=Session["IT"];
                 values[51] = ddlRateType.SelectedValue;
                SQLServer da = new SQLServer(Global.GetConnectionString());
                try
                {
                    DataSet dsResult = da.SelectRecords("SP_InsertRateLine", param, values, dbtypes);

               
                if (dsResult == null || dsResult.Tables.Count == 0 || dsResult.Tables[0].Rows.Count == 0)
                {
                    // error
                    lblStatus.Text = "Rate line insertion failed..";
                    return;
                }
                else
                {
                    // Slabs
                    int id;
                    //to Update Rate Card
                    //if (HidSrNo.Value != "")
                    if (Request.QueryString["cmd"] == "Edit" && ViewState["IsAddAnother"].ToString() == "F")
                        id = Convert.ToInt32(HidSrNo.Value);
                    //to Save Rate Card
                    else
                        id = int.Parse(dsResult.Tables[0].Rows[0][0].ToString());
                    rtlid = id;
                    
                    dsSlabs = (DataSet)Session["dsSlabs"];

                    //delete rate line slabs before insert or update for RateLineSrNo
                    param = new string[] { "RateLineSrNo"};
                    dbtypes = new SqlDbType[] { SqlDbType.Int};
                    values = new object[] { id};

                    if (!da.ExecuteProcedure("SP_DeleteRateLineSlabs", param, dbtypes, values))
                    {
                        // error
                    }
                    if (ckhULDRate.Checked == false)
                    {
                        for (int i = 0; i < dsSlabs.Tables[0].Rows.Count; i++)
                        {
                            param = new string[] { "RateLineSrNo", "SlabName", "Weight", "Charge","Cost","Based" };
                            dbtypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.VarChar, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float,SqlDbType.VarChar };
                            values = new object[] { id, dsSlabs.Tables[0].Rows[i]["Type"].ToString(), Convert.ToDouble(dsSlabs.Tables[0].Rows[i]["Weight"].ToString()), Convert.ToDouble(dsSlabs.Tables[0].Rows[i]["Charge"].ToString()), Convert.ToDouble(dsSlabs.Tables[0].Rows[i]["Cost"].ToString()), dsSlabs.Tables[0].Rows[i]["DdlBased"].ToString() };

                            if (!da.ExecuteProcedure("SP_InsertRateLineSlabs", param, dbtypes, values))
                            {
                                // error
                            }

                        }
                    }
                    try
                    {
                        // ULDSlabs (jayant)
                        int id1;
                        //to Update Rate Card
                        //if (HidSrNo.Value != "")
                        if (Request.QueryString["cmd"] == "Edit")
                            id1 = Convert.ToInt32(HidSrNo.Value);
                        //to Save Rate Card
                        else
                            id1 = int.Parse(dsResult.Tables[0].Rows[0][0].ToString());

                        dsULDSlabs = (DataSet)Session["dsULDSlabs"];

                        //delete rate line slabs before insert or update for RateLineSrNo
                        param = new string[] { "RateLineSrNo" };
                        dbtypes = new SqlDbType[] { SqlDbType.Int };
                        values = new object[] { id1 };
                        if (ckhULDRate.Checked == true)//jayant
                        {
                            for (int i = 0; i < dsULDSlabs.Tables[0].Rows.Count; i++)
                            {
                                param = new string[] { "RateLineSrNo", "SlabName", "Weight", "Charge", "ULDType" };
                                dbtypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.VarChar, SqlDbType.Float, SqlDbType.Float, SqlDbType.VarChar };
                                values = new object[] { id1, dsULDSlabs.Tables[0].Rows[i]["Type"].ToString(), Convert.ToDouble(dsULDSlabs.Tables[0].Rows[i]["Weight"].ToString()), Convert.ToDouble(dsULDSlabs.Tables[0].Rows[i]["Charge"].ToString()), dsULDSlabs.Tables[0].Rows[i]["ULDType"].ToString() };

                                if (!da.ExecuteProcedure("SP_InsertULDRateLineSlabs", param, dbtypes, values))
                                {
                                    // error
                                }

                            }
                        }
                        Remarks();
                    }
                    catch(Exception objEx)
                    {
                    }
                    // success
                    //blank all the fields
                    HidSrNo.Value = "";

                    if (Request.QueryString["cmd"] == "Edit" && ViewState["IsAddAnother"].ToString() == "F") //update done
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "alert('Rate Line record updated successfully!');", true);

                        #region For Master Audit Log
                        MasterAuditBAL ObjMAL = new MasterAuditBAL();
                        #region Prepare Parameters
                        object[] Params = new object[7];
                        int i = 0;

                        //1
                        Params.SetValue("Rate Line", i);
                        i++;

                        //2
                        string Value = DdlRateCardName.SelectedItem.Text + "(" + ddlOrigin.SelectedItem.Text + "-" + ddlDestination.SelectedItem.Text + ")";
                        Params.SetValue(Value, i);
                        i++;

                        //3

                        Params.SetValue("UPDATE", i);
                        i++;

                        //4

                        Params.SetValue("", i);
                        i++;


                        //5

                        Params.SetValue("", i);
                        i++;

                        //6

                        Params.SetValue(Session["UserName"], i);
                        i++;

                        //7
                        Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), i);
                        i++;


                        #endregion Prepare Parameters
                        ObjMAL.AddMasterAuditLog(Params);
                        #endregion
                        lblStatus.Text = "Rate Line record updated successfully!";
                        lblStatus.ForeColor = Color.Green;
                        if (ckhULDRate.Checked == true)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivShow();</script>", false);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivHide();</script>", false);
                        }
                    }
                    else //insert done
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "alert('Rate Line record added successfully!');", true);
                        lblStatus.Text = "Rate Line record added successfully!";
                        lblStatus.ForeColor = Color.Green;

                        #region For Master Audit Log
                        MasterAuditBAL ObjMAL = new MasterAuditBAL();
                        #region Prepare Parameters
                        object[] Params = new object[7];
                        int i = 0;

                        //1
                        Params.SetValue("Rate Line", i);
                        i++;

                        //2
                        string Value = DdlRateCardName.SelectedItem.Text + "(" + ddlOrigin.SelectedItem.Text + "-" + ddlDestination.SelectedItem.Text + ")";
                        Params.SetValue(Value, i);
                        i++;

                        //3

                        Params.SetValue("ADD", i);
                        i++;

                        //4

                        Params.SetValue("", i);
                        i++;


                        //5

                        Params.SetValue("", i);
                        i++;

                        //6

                        Params.SetValue(Session["UserName"], i);
                        i++;

                        //7
                        Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), i);
                        i++;


                        #endregion Prepare Parameters
                        ObjMAL.AddMasterAuditLog(Params);
                        #endregion

                        if (ckhULDRate.Checked == true)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivShow();</script>", false);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivHide();</script>", false);
                        }

                    }

                    ClearALL();
                    
                }
                }
                catch (Exception exobj)
                {
                    lblStatus.Text = "Rate line insertion failed..";
                    return;
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Rate line insertion failed..";
                return;
            }
        }

        public void ClearALL()
        {
            btnSave.Enabled = false;
            btnAddAnother.Visible = true;
            ViewState["IsAddAnother"] = "F";
            //Response.Redirect("ListRateLine.aspx");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListRateLine.aspx");
        }

        #region disable controls for view
        protected void disableForView()
        {
            DdlRateCardName.Enabled = false;
            DdlOriginLevel.Enabled = false;
            //TxtOrigin.Enabled = false;
            ddlOrigin.Enabled = false;
            DdlDestinationLevel.Enabled = false;
            //TXTDestination.Enabled = false;
            ddlDestination.Enabled = false;
            TXTContrRef.Enabled = false;
            TXTCurrency.Enabled = false;
            DdlStatus.Enabled = false;
            DdlRateBasis.Enabled = false;
            TXTValidFrom.Enabled = false;
            IBValidFrom.Enabled = false;
            TXTValidTo.Enabled = false;
            IBValidTo.Enabled = false;
            TXTComm.Enabled = false;
            TXTDiscount.Enabled = false;
            TXTTax.Enabled = false;
            ddlGLCode.Enabled = false;
            TXTFlightNumber.Enabled = false;
            TXTFlightCarrier.Enabled = false;
            TXTHandlingCode.Enabled = false;
            TXTAirLineCode.Enabled = false;
            TXTIATAComCode.Enabled = false;
            TXTAgentCode.Enabled = false;
            TXTShipperCode.Enabled = false;
            txtEquipmentType.Enabled = false;

            RBExAC.Enabled = false;
            RBExAD.Enabled = false;
            RBExCC.Enabled = false;
            RBExFC.Enabled = false;
            RBExFN.Enabled = false;
            RBExHC.Enabled = false;
            RBExSC.Enabled = false;
            RBExET.Enabled = false;
            RBIncAC.Enabled = false;
            RBIncAD.Enabled = false;
            RBIncCC.Enabled = false;
            RBIncFC.Enabled = false;
            RBIncFN.Enabled = false;
            RBIncHC.Enabled = false;
            RBIncSC.Enabled = false;
            RBIncET.Enabled = false;
            btnAdd.Visible = false;
            btnDelete.Visible = false;
            btnULDAdd.Visible = false;
            btnULDDel.Visible = false;

            GRDRateSlabs.Enabled = false;
            grdULDslabs.Enabled = false;
            ckhAllIn.Enabled = false;
            ckhTACTRate.Enabled = false;
            ckhULDRate.Enabled = false;
            TXTProductType.Enabled = false;
            RBIncPT.Enabled = false;
            IProd.Enabled = false;
            IAPC.Enabled = false;
            TXTTransitStation.Enabled = false;
            ckhHeavyRate.Enabled = false;
            txtComment.Enabled = false;


        }
        #endregion

        protected void DdlOriginLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListDataViewBAL objDataViewBAL = new ListDataViewBAL();
            DataSet dsResult = new DataSet();
            string errormessage = "";

            string level = DdlOriginLevel.SelectedItem.Value;
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

        protected void DdlDestinationLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListDataViewBAL objDataViewBAL = new ListDataViewBAL();
            DataSet dsResult = new DataSet();
            string errormessage = "";

            string level = DdlDestinationLevel.SelectedItem.Value;
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

        protected void btnAddULD_Click(object sender, EventArgs e)
        {
            SaveGridULDState();
            AddNewULDSlab();
        }

        #region RBIncFC_CheckedChanged
        protected void RBIncFC_CheckedChanged(object sender, EventArgs e)
        {
            if (RBIncFC.Checked == true && TXTFlightCarrier.Text.Trim() != "")
            {
                CheckFlightCarrier();
            }
        }
        #endregion

        #region TXTFlightCarrier_TextChanged
        protected void TXTFlightCarrier_TextChanged(object sender, EventArgs e)
        {
            if (RBIncFC.Checked == true && TXTFlightCarrier.Text.Trim() != "")
            {
                CheckFlightCarrier();
            }
        }
        #endregion

        #region CheckFlightCarrier
        private void CheckFlightCarrier()
        {
            try
            {

                bool flag = false;
                string FlightCarrier = TXTFlightCarrier.Text.Trim().ToUpper();
                string procedure = "SP_GetAWBPrefix";
                SQLServer objSQL = new SQLServer(Global.GetConnectionString());
                DataSet dsData = new DataSet();
                dsData = objSQL.SelectRecords(procedure);
                if (dsData != null)
                {
                    if (dsData.Tables.Count > 0)
                    {
                        if (dsData.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsData.Tables[0].Rows)
                            {
                                if (dr[0].ToString().Trim().ToUpper() == FlightCarrier.Trim().ToUpper())
                                {
                                    flag = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (flag == false)
                {
                    txtProPer.Enabled = true;
                    RBExPro.Enabled = true;
                    RBInPro.Enabled = true;
                    RBExPro.Checked = true;
                }
                else
                {
                    txtProPer.Enabled = false;
                    RBExPro.Enabled = false;
                    RBInPro.Enabled = false;
                }
                //added by jayant for ULD Grid
                if (ckhULDRate.Checked == true)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivShow();</script>", false);

                }
                else
                {


                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivHide();</script>", false);

                }
                //end
            }
            catch (Exception ex)
            { }
        }
        #endregion

        protected void btnULDAdd_Click(object sender, EventArgs e)
        {
            SaveGridULDState();
            AddNewULDSlab();
        }

        protected void btnULDDel_Click(object sender, EventArgs e)
        {
            SaveGridULDState();

            dsULDSlabs = (DataSet)Session["dsULDSlabs"];
            DataSet dsULDSlabsTemp = dsULDSlabs.Copy();

            for (int i = 0; i < dsULDSlabs.Tables[0].Rows.Count; i++)
            {
                if (((CheckBox)grdULDslabs.Rows[i].FindControl("CHKULD")).Checked)
                {
                    string srno = dsULDSlabs.Tables[0].Rows[i]["SrNo"].ToString();

                    for (int j = 0; j < dsULDSlabsTemp.Tables[0].Rows.Count; j++)
                    {
                        if (srno == dsULDSlabsTemp.Tables[0].Rows[j]["SrNo"].ToString())
                        {
                            dsULDSlabsTemp.Tables[0].Rows.Remove(dsULDSlabsTemp.Tables[0].Rows[j]);
                            break;
                        }
                    }
                }
            }

            grdULDslabs.DataSource = null;
            grdULDslabs.DataSource = dsULDSlabsTemp.Tables[0];
            grdULDslabs.DataBind();


            for (int i = 0; i < dsULDSlabsTemp.Tables[0].Rows.Count; i++)
            {
                ((DropDownList)grdULDslabs.Rows[i].FindControl("DdlULDType")).Text = dsULDSlabsTemp.Tables[0].Rows[i]["ULDType"].ToString();

            }
            for (int i = 0; i < dsULDSlabsTemp.Tables[0].Rows.Count; i++)
            {
                ((DropDownList)grdULDslabs.Rows[i].FindControl("DdlULDType1")).Text = dsULDSlabsTemp.Tables[0].Rows[i]["Type"].ToString();

            }

            Session["dsULDSlabs"] = dsULDSlabsTemp.Copy();
            if (ckhULDRate.Checked == true)
            {
                //btnULDAdd.Visible = true;
                //btnULDDel.Visible = true;
                //grdULDslabs.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivShow();</script>", false);

            }
            else
            {

                //btnULDAdd.Visible = false;
                //btnULDDel.Visible = false;
                //grdULDslabs.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivHide();</script>", false);

            }

        }

        #region ckhULDRate_CheckedChanged
        protected void ckhULDRate_CheckedChanged(object sender, EventArgs e)
        {
            if (ckhULDRate.Checked == true)
            {
                //btnULDDel.Visible = true;
                //btnULDAdd.Visible = true;
                //grdULDslabs.Visible = true;
                //CheckFlightCarrier();
            }
        }
        #endregion

        protected void Remarks()
        {

            if (Request.QueryString["cmd"] == "Edit")
                rtlid = Convert.ToInt32(HidSrNo.Value);

            string Date = Session["IT"].ToString();
            if (txtComment.Text.Trim() != "")
            {
                UserName = Session["UserName"].ToString();
                string[] param = { "name", "comments", "date", "RateCardSrNo" };
                SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int };
                object[] values = { UserName, txtComment.Text, Date, rtlid };
                DataSet dsadd = da.SelectRecords("[SP_InsertRemarks]", param, values, dbtypes);
            }
            txtComment.Text = "";
            BindRepeaterData();
        }

        protected void BindRepeaterData()
        {
            //if (Request.QueryString["cmd"] == "Edit")
            //    rtlid = Convert.ToInt32(HidSrNo.Value);
            //RepDetails.DataSource = null;
            if (Request.QueryString["cmd"] == "Edit" || Request.QueryString["cmd"] == "View")
                //rtlid=Convert.ToInt32(HidSrNo.Value);
                //rtlid = rtlid1;
                rtlid = Convert.ToInt32(Request.QueryString["RCName"].ToString());
            string[] param = { "RateCardSrNo" };
            SqlDbType[] dbtypes = { SqlDbType.Int };
            object[] values = { rtlid };
            DataSet ds = new DataSet();
            ds = da.SelectRecords("[SP_GetRemarks]", param, values, dbtypes);

            RepDetails.DataSource = ds;
            RepDetails.DataBind();

        }

        protected void AddSpecialCommodity()
        {
            try
            {

                string[] Params = new string[] 
                {   "CommodityCode",
                    "ChargePerUnit",
                    "TaxPercent",
                    "DiscountPercent",
                    "CommissionPercent",
                    "ChargeType",
                    "Minimum",
                    "OriginLevel",
                    "Origin",
                    "DestinationLevel",
                    "Destination",
                    "StartDate",
                    "EndDate",
                    "ServiceTax",
                    "TDSPercent",
                    "IsALLIn"};
                object[] PValues = new object[] 
                {
                    splCommodity.Text,
                    Convert.ToDouble(txtRelativePer.Text),
                    Convert.ToDouble(TXTTax.Text),
                    Convert.ToDouble(TXTDiscount.Text),
                    Convert.ToDouble(TXTComm.Text),
                    ddlAppliedOn.Text,
                    Convert.ToDouble(txtRelativeMin.Text),
                    DdlOriginLevel.SelectedIndex,
                    ddlOrigin.Text,
                    DdlDestinationLevel.SelectedIndex,
                    ddlDestination.Text,
                    //Convert.ToDateTime(TXTValidFrom.Text).ToString("yyyy-MM-dd HH:mm:ss"),
                    Convert.ToDateTime(TXTValidFrom.Text).ToString("dd/MM/yyyy"),

                    //Convert.ToDateTime(TXTValidTo.Text).ToString("yyyy-MM-dd HH:mm:ss"),
                    Convert.ToDateTime(TXTValidTo.Text).ToString("dd/MM/yyyy"),

                    Convert.ToDouble(TXTTax.Text),
                    Convert.ToDouble(TXTComm.Text),
                   ckhAllIn.Checked
                };
                SqlDbType[] PType = new SqlDbType[] 
                {
                    SqlDbType.VarChar,
                    SqlDbType.Float,
                    SqlDbType.Float,
                    SqlDbType.Float,
                    SqlDbType.Float,
                    SqlDbType.VarChar,
                    SqlDbType.Float,
                    SqlDbType.Int,
                    SqlDbType.VarChar,
                    SqlDbType.Int,
                    SqlDbType.VarChar,
                    SqlDbType.DateTime,
                    SqlDbType.DateTime,
                    SqlDbType.Float,
                    SqlDbType.Float,
                    SqlDbType.Bit


                };
                SQLServer da = new SQLServer(Global.GetConnectionString());
                DataSet dsResult = da.SelectRecords("spAddSPLCommodityRate", Params, PValues, PType);
                if (dsResult != null)
                {
                    if (dsResult.Tables.Count > 0)
                    {
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            lblStatus.Text = "Rate Line record Added successfully!";
                            lblStatus.ForeColor = Color.Green;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        #region Fill Currency
        private void FillCurrencyCodes(DropDownList drp, string SelectedCurrency)
        {
            try
            {
                BALCurrency BalCur = new BALCurrency();
                BookingBAL objBLL = new BookingBAL();
                DataSet dsCur = BalCur.GetCurrencyCodeList("");
                DataSet dsCurrency = null;

                if (dsCur != null && dsCur.Tables.Count > 0)
                {
                    if (dsCur.Tables[0].Rows.Count > 0)
                    {
                        drp.Items.Clear();
                        //drpWWR.Items.Add("Select");
                        try
                        {
                            drp.DataSource = dsCur.Tables[0];
                            drp.DataTextField = "Code";
                            drp.DataValueField = "ID";
                            drp.DataBind();
                        }
                        catch (Exception ex) { }

                        objBLL.GetAirpotCurrency(Session["Station"].ToString(), ref dsCurrency);
                        if (dsCurrency != null && dsCurrency.Tables[0].Rows.Count > 0)
                        {
                            string Currency = dsCurrency.Tables[0].Rows[0]["BookingCurrrency"].ToString();
                            drp.Text = Currency;
                        }
                        else
                            drp.SelectedIndex = drp.Items.IndexOf(drp.Items.FindByText(SelectedCurrency));
                    }
                    else
                    {
                        drp.Items.Clear();
                        drp.SelectedIndex = 0;
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region btnAddAnother_Click
        protected void btnAddAnother_Click(object sender, EventArgs e)
        {
            btnAddAnother.Visible = false;
            btnSave.Enabled = true;
            btnSave.Text = "Save";
            ddlDestination.SelectedIndex = 0;
            lblStatus.Text = "";
            ViewState["IsAddAnother"] = "T";
            // ckhULDRate.Checked = true;
        }
        #endregion btnAddAnother_Click

        #region Load ULD Types
        public void LoadULDTypes(int i)
        {
            try
            {

                DataSet ds = da.SelectRecords("SP_GetAvailabeULDTypes");
                DropDownList ddl = new DropDownList();
                {
                    ddl = ((DropDownList)(grdULDslabs.Rows[i].FindControl("DdlULDType")));
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            ddl.DataSource = ds;
                            ddl.DataMember = ds.Tables[0].TableName;
                            ddl.DataTextField = "TypeCode";
                            ddl.DataValueField = "VAL";
                            ddl.DataBind();
                            //ddl.Items.Clear();
                            //for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                            //{
                            //    ddl.Items.Add(ds.Tables[0].Rows[k][0].ToString());
                            //}
                        }
                    }
                }

            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Load DropDowns
        public void LoadGLDropDown()
        {
            try
            {
                DataSet ds = da.SelectRecords("SP_GetGLAccountCode");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlGLCode.DataSource = ds;
                            ddlGLCode.DataTextField = "GLAccountCode";
                            ddlGLCode.DataValueField = "GLAccountCode";
                            ddlGLCode.DataBind();
                           
                        }
                    }
                }
                ddlGLCode.Items.Insert(0, "Select");
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}
