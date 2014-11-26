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
    public partial class PLIMasterNew : System.Web.UI.Page
    {
        EmbargoBAL Embal = new EmbargoBAL();
        AgentBAL objBLL = new AgentBAL();
        BALPLIMaster objPLI = new BALPLIMaster();
        SQLServer da = new SQLServer(Global.GetConnectionString());
        DataSet blnResult;
        DataSet res;

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    GetCommodity();
                    getflightnumber();
                    LoadGrid();
                    LoadAgentName();
                    btnSave.Visible = true;
                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    lblStatus.Text = "Error in Loading the page!";
                }
            }
        }
        #endregion

        #region Add
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                SaveRouteDetails();
                AddNewRowToGrid();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion Add

        #region SaveGridData
        public void SaveRouteDetails()
        {
            try
            {
                DataTable dtCreditInfo = ((DataTable)Session["dtCreditInfo"]).Clone();
                for (int i = 0; i < grdTonnage.Rows.Count; i++)
                {
                    DataRow row = dtCreditInfo.NewRow();
                    row["Tonnage"] = ((TextBox)grdTonnage.Rows[i].FindControl("txtTonnage")).Text;

                    row["Rate"] = ((TextBox)grdTonnage.Rows[i].FindControl("txtRate")).Text;

                    dtCreditInfo.Rows.Add(row);
                }
                Session["dtCreditInfo"] = dtCreditInfo.Copy();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion SaveGridData

        #region AddnewRow To Grid
        private void AddNewRowToGrid()
        {
            try
            {
                DataTable dtCreditInfo = (DataTable)Session["dtCreditInfo"];
                DataRow rw = dtCreditInfo.NewRow();
                dtCreditInfo.Rows.Add(rw);
                grdTonnage.DataSource = dtCreditInfo.Copy();
                grdTonnage.DataBind();
                for (int i = 0; i < dtCreditInfo.Rows.Count; i++)
                {
                    DataRow row = dtCreditInfo.Rows[i];
                    ((TextBox)grdTonnage.Rows[i].FindControl("txtTonnage")).Text = row["Tonnage"].ToString();
                    ((TextBox)grdTonnage.Rows[i].FindControl("txtRate")).Text = row["Rate"].ToString();
                }
            }
            catch (Exception e)
            {

            }

        }
        #endregion AddnewRow To Grid

        #region SelectedIndexChanging
        protected void grdTonnage_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            //grdTonnage.PageIndex = e.NewPageIndex;
            //grdTonnage.DataSource = dsResult.Copy();
            //grdTonnage.DataBind();
        }
        #endregion SelectedIndexChanging

        #region Clear
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Visible = true;
                txtDealid.Text = "";
                //txtaplicablefrom.Text = "";
                //txtApplicableto.Text = "";
                ddlAgentCode.SelectedIndex = -1;
                LoadGrid();

                txtAgentName.Text = "";
                txtFromDt.Text = "";
                txtToDt.Text = "";
                rbStandard.Checked = false;
                grdTonnage.HeaderRow.Cells[1].Text = "Tonnage";
                grdTonnage.HeaderRow.Cells[2].Text = "Rate";

                ddlorigintype.SelectedIndex = -1;
                ddlOrigin.SelectedIndex = -1;
                ddldestinationType.SelectedIndex = -1;
                ddlDestination.SelectedIndex = -1;
                ddlFlightNumber.SelectedIndex = -1;
                ddlCommodity.SelectedIndex = -1;
                //DateTime dt = DateTime.Parse(dsPLIData.Tables[1].Rows[0]["ApplicableTo"].ToString());
                //txtToDt.Text = dt.ToString("dd/MM/yyyy");

                rbEntireTonnage.Checked = true;

                txtRateDecide.Text = "";
                rbSpotInclude.Checked = true;


                // Assigning Exceptions

                TXTFlightNumber.Text = "";
                RBIncFN.Checked = false;
                TXTFlightCarrier.Text = "";
                RBIncFC.Checked = false;
                TXTHandlingCode.Text = "";
                RBIncHC.Checked = false;
                TXTAirLineCode.Text = "";
                RBIncAC.Checked = false;
                TXTIATAComCode.Text = "";
                RBIncCC.Checked = false;
                TXTAgentCode.Text = "";
                RBIncAD.Checked = false;
                TXTShipperCode.Text = "";
                RBIncSC.Checked = false;
                TXTParamORG.Text = "";
                RBIncORG.Checked = false;
                TXTParamDest.Text = "";
                RBIncDest.Checked = false;

                ChangeControlStatus(true);

                lblStatus.Visible = true;
                lblStatus.Text = "";
            }
            catch (Exception ex)
            { }
        }
        #endregion Clear

        #region Delete
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            // SaveMaterialGrid();
            DataTable dtCreditInfo = ((DataTable)Session["dtCreditInfo"]).Clone();
            Session["dsMaterialDetails"] = ((DataTable)Session["dtCreditInfo"]);
            DataTable dsMaterialDetailsTemp = ((DataTable)Session["dsMaterialDetails"]);
            DataTable dsMaterialDetails = (DataTable)Session["dsMaterialDetails"];

            //for (int i = 0; i < grdTonnage.Rows.Count; i++)
            //{
            //    DataRow rw = dsMaterialDetailsTemp.NewRow();
            //    rw["Tonnage"] = ((TextBox)grdTonnage.Rows[i].FindControl("txtTonnage")).Text;
            //    rw["Rate"] = ((TextBox)grdTonnage.Rows[i].FindControl("txtRate")).Text;
            //    dsMaterialDetailsTemp.Rows.Add(rw);
            //}
            for (int i = dsMaterialDetailsTemp.Rows.Count - 1; i >= 0; i--)
            {
                CheckBox chkRow = (CheckBox)grdTonnage.Rows[i].FindControl("check");
                bool chkRowVal = chkRow.Checked;
                if (dsMaterialDetailsTemp != null)
                {
                    if (chkRowVal)
                    {
                        DataRow dr = dsMaterialDetailsTemp.Rows[i];
                        dsMaterialDetailsTemp.Rows.Remove(dr);
                    }
                }
            }

            Session["dsMaterialDetails"] = dsMaterialDetailsTemp.Copy();
            grdTonnage.DataSource = dsMaterialDetailsTemp.Copy();
            grdTonnage.DataBind();

            //for (int i = 0; i < dsMaterialDetailsTemp.Rows.Count; i++)
            //{
            //    ((TextBox)grdTonnage.Rows[i].FindControl("txtTonnage")).Text = dsMaterialDetailsTemp.Rows[i]["Tonnage"].ToString();
            //    ((TextBox)grdTonnage.Rows[i].FindControl("txtRate")).Text = dsMaterialDetailsTemp.Rows[i]["Rate"].ToString();
            //}

        }
        #endregion Delete

        #region SaveGridEmbargo
        public void SaveMaterialGrid()
        {
            try
            {
                DataTable dtCreditInfo = ((DataTable)Session["dtCreditInfo"]).Clone();

                int i = 0;
                foreach (GridViewRow row in grdTonnage.Rows)
                {
                    i++;
                    DataRow rw = dtCreditInfo.NewRow();
                    //((DropDownList)row.FindControl("ddlMaterialCommCode")).SelectedItem.Text

                    rw["Tonnage"] = ((TextBox)row.FindControl("txtTonnage")).Text;

                    rw["Rate"] = ((TextBox)row.FindControl("txtRate")).Text;


                    dtCreditInfo.Rows.Add(row);


                    //dsMaterialDetails.Tables[0].Rows.Add(rw);
                }

                Session["dsMaterialDetails"] = dtCreditInfo.Copy();

            }
            catch (Exception ex)
            {
                lblStatus.Text = "" + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }
        #endregion SaveGridEmbargo

        #region LoadgridInfo
        public void LoadGrid()
        {
            try
            {
                DataTable myDataTable = new DataTable();
                DataColumn myDataColumn;
                DataSet Ds = new DataSet();

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Tonnage";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Rate";
                myDataTable.Columns.Add(myDataColumn);

                DataRow dr;
                dr = myDataTable.NewRow();
                dr["Tonnage"] = "";
                dr["Rate"] = "";//"5";

                myDataTable.Rows.Add(dr);


                grdTonnage.DataSource = null;
                grdTonnage.DataSource = myDataTable;
                grdTonnage.DataBind();

                Session["dtCreditInfo"] = myDataTable.Copy();
            }
            catch (Exception ex) { }
        }
        #endregion LoadgridInfo

        #region OrignType Selection Changed
        protected void ddlorigintype_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListDataViewBAL objDataViewBAL = new ListDataViewBAL();
                DataSet dsResult = new DataSet();
                string errormessage = "";

                string level = ddlorigintype.SelectedItem.Value;
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
            catch (Exception ex)
            { }
        }
        #endregion OriginType

        #region Load AgentName
        public void LoadAgentName()
        {
            try
            {
                DataSet ds = objBLL.GetAgentList(Session["AgentCode"].ToString());
                if (ds != null)
                {
                    ddlAgentCode.DataSource = ds;
                    ddlAgentCode.DataMember = ds.Tables[0].TableName;
                    ddlAgentCode.DataTextField = "AgentCode";
                    ddlAgentCode.DataValueField = "AgentName";
                    ddlAgentCode.DataBind();
                    ddlAgentCode.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion LoadAgentName

        //Loads destination places in a dropdown
        #region DestinationType
        protected void ddldestinationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListDataViewBAL objDataViewBAL = new ListDataViewBAL();
                DataSet dsResult = new DataSet();
                string errormessage = "";

                string level = ddldestinationType.SelectedItem.Value;
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
            catch (Exception ex)
            {

            }
        }
        #endregion DestinationType
        //loads commodity in a dropdown
        #region GetCommodity
        public void GetCommodity()
        {
            try
            {
                DataSet dsResult = new DataSet();
                //string errormessage = "";

                //string level = (((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlParameter")).Text);
                dsResult = da.SelectRecords("SpGetCommodity");
                //dsResult.Tables[0].Rows.Add("Select");


                // DropDownList ddl = ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable"));

                ddlCommodity.DataSource = dsResult;
                ddlCommodity.DataMember = dsResult.Tables[0].TableName;
                ddlCommodity.DataValueField = dsResult.Tables[0].Columns[0].ColumnName;
                ddlCommodity.DataTextField = dsResult.Tables[0].Columns[0].ColumnName;
                ddlCommodity.DataBind();
            }
            catch (Exception e) { }

        }
        #endregion GetCommodity
        //loads flight numbers in a dropdown
        #region GetFlightDropdown
        protected void getflightnumber()
        {
            try
            {
                DataSet ds = da.SelectRecords("SP_GetFlightID");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlFlightNumber.Items.Clear();

                            ddlFlightNumber.DataSource = ds.Tables[0];
                            ddlFlightNumber.DataTextField = "FlightID";
                            ddlFlightNumber.DataValueField = "FlightID";
                            ddlFlightNumber.DataBind();
                            ddlFlightNumber.Items.Insert(0, new ListItem("Select", ""));
                            ddlFlightNumber.SelectedIndex = -1;
                            //ddlFlightNumber.Items.Add("Select");
                            //ddlFlightNumber.SelectedIndex = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion GetFlightDropdown

        #region AgentCode Selection Change
        protected void ddlAgentCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtAgentName.Text = ddlAgentCode.SelectedValue;
        }
        #endregion


        #region Save Button Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string AgentCode = "", OriginType = "", Origin = "", DestinationType = "", Destination = "", FlightNo = "", Commodity = "", Rate = "", Sale = "";
                string UpdatedBy = "", Applicability = "", ApplicableRate = "", SpotRateTonnageIsInclude = ""; double Tonnage = 0.0, Percent = 0.0;
                DateTime ApplicableFrom, ApplicableTo, UpdatedOn;
                string PLIId = "";

                if (grdTonnage.Rows.Count < 2)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Add Target Data to the grid.!";
                    lblStatus.Visible = true;
                    return;
                }

                for (int i = 0; i < grdTonnage.Rows.Count - 1; i++)
                {
                    if (rbTonnage.Checked)
                    {
                        Rate = ((TextBox)grdTonnage.Rows[i].FindControl("txtRate")).Text;
                        if (((TextBox)grdTonnage.Rows[i].FindControl("txtTonnage")).Text == "")
                        {
                            Tonnage = 0.0;
                        }
                        else
                        {
                            Tonnage = Convert.ToDouble(((TextBox)grdTonnage.Rows[i].FindControl("txtTonnage")).Text);
                        }
                        Sale = ""; Percent = 0.0;
                    }
                    else if (rbStandard.Checked)
                    {
                        Sale = ((TextBox)grdTonnage.Rows[i].FindControl("txtRate")).Text;
                        if (((TextBox)grdTonnage.Rows[i].FindControl("txtTonnage")).Text == "")
                        {
                            Percent = 0.0;
                        }
                        else
                        {
                            Percent = Convert.ToDouble(((TextBox)grdTonnage.Rows[i].FindControl("txtTonnage")).Text);
                        }
                        Rate = ""; Tonnage = 0.0;
                    }
                    if (ddlAgentCode.SelectedItem.Text == "Select")
                    {
                        lblStatus.Text = "Please Select Agent Code";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    else
                    {
                        AgentCode = ddlAgentCode.SelectedItem.Text;
                    }
                    if (txtFromDt.Text == "")
                    {
                        lblStatus.Text = "Please Enter Applicable From date";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    else
                    {
                        ApplicableFrom = DateTime.ParseExact(txtFromDt.Text, "dd/MM/yyyy", null);
                    }
                    if (txtToDt.Text == "")
                    {
                        lblStatus.Text = "Please Enter Applicable To date";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    else
                    {
                        ApplicableTo = DateTime.ParseExact(txtToDt.Text, "dd/MM/yyyy", null);
                    }
                    OriginType = ddlorigintype.SelectedItem.Text;
                    Origin = ddlOrigin.SelectedItem.Text;
                    DestinationType = ddldestinationType.SelectedItem.Text;
                    Destination = ddlDestination.SelectedItem.Text;
                    if (ddlFlightNumber.SelectedItem.Text == "All")
                    {
                        FlightNo = "All";
                    }
                    else
                    {
                        FlightNo = ddlFlightNumber.SelectedItem.Text;
                    }
                    Commodity = ddlCommodity.SelectedItem.Text;
                    UpdatedBy = Session["UserName"].ToString();
                    UpdatedOn = Convert.ToDateTime(System.DateTime.Now.ToShortDateString());

                    if (rbEntireTonnage.Checked == true)
                    {
                        Applicability = "ETA";
                    }
                    else
                    {
                        Applicability = "ATT";
                    }

                    ApplicableRate = txtRateDecide.Text.Trim();

                    if (rbSpotExclude.Checked && !rbSpotInclude.Checked)
                    {
                        SpotRateTonnageIsInclude = "Excluded";
                    }
                    else
                    {
                        SpotRateTonnageIsInclude = "Included";
                    }
                    //res = objDeal.SaveAgentDeal(AgentCode, OriginType, Origin, DestinationType, Destination, FlightNo, Commodity, Rate, RateSlab, SpotRateTonnageIsInclude, Commissionable, updatedBy, ApplicableFrom, ApplicableTo, UpdatedOn, Tonnage, KickbackAmount, Threshold, M, N, Q1, Q2);
                    
                    if (i == 0)
                    {
                        if (txtDealid.Text == "")  // Insert Case
                        {
                            res = objPLI.SavePLIMaster("INSERT", AgentCode, OriginType, Origin, DestinationType, Destination, FlightNo, Commodity, rbStandard.Checked, Rate, Tonnage, Sale, Percent, Applicability, ApplicableRate, SpotRateTonnageIsInclude, ApplicableFrom, ApplicableTo, UpdatedBy, UpdatedOn, "");
                        }
                        else
                        {
                            res = objPLI.SavePLIMaster("UPDATE", AgentCode, OriginType, Origin, DestinationType, Destination, FlightNo, Commodity, rbStandard.Checked, Rate, Tonnage, Sale, Percent, Applicability, ApplicableRate, SpotRateTonnageIsInclude, ApplicableFrom, ApplicableTo, UpdatedBy, UpdatedOn, txtDealid.Text);
                        }
                        if (res != null)
                        {
                            PLIId = res.Tables[0].Rows[0]["Column1"].ToString();
                        }
                    }
                    else
                    {
                        res = objPLI.SavePLIMaster("INSERT", AgentCode, OriginType, Origin, DestinationType, Destination, FlightNo, Commodity, rbStandard.Checked, Rate, Tonnage, Sale, Percent, Applicability, ApplicableRate, SpotRateTonnageIsInclude, ApplicableFrom, ApplicableTo, UpdatedBy, UpdatedOn, PLIId);
                        //res = objPLI.SavePLIMaster("UPDATE", AgentCode, OriginType, Origin, DestinationType, Destination, FlightNo, Commodity, rbStandard.Checked, Rate, Tonnage, Sale, Percent, Applicability, ApplicableRate, SpotRateTonnageIsInclude, ApplicableFrom, ApplicableTo, UpdatedBy, UpdatedOn, PLIId);
                    }
                }

                if (res.Tables[0].Rows.Count > 0)
                {

                    txtDealid.Text = res.Tables[0].Rows[0]["Column1"].ToString();
                    lblStatus.Text = "Deal Saved Successully";
                    lblStatus.ForeColor = Color.Green;
                    btnSave.Visible = true;

                    if (objPLI.SavePLIExceptions(txtDealid.Text, TXTFlightNumber.Text, RBIncFN.Checked, TXTFlightCarrier.Text, RBIncFC.Checked, TXTHandlingCode.Text, RBIncHC.Checked, TXTAirLineCode.Text, RBIncAC.Checked, TXTIATAComCode.Text, RBIncCC.Checked, TXTAgentCode.Text, RBIncAD.Checked, TXTShipperCode.Text, RBIncSC.Checked, TXTParamORG.Text, RBIncORG.Checked, TXTParamDest.Text, RBIncDest.Checked))
                    {
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Error in Saving Excetions";
                    }
                    ChangeControlStatus(false);
                    txtDealid.Enabled = false;
                    btnSave.Enabled = false;
                    btnList.Enabled = false;
                    btnClose.Enabled = true;
                    btnClear.Enabled = true;
                    btnEdit.Enabled = true;

                    return;
                }
                else
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Deal Not Saved Please check data.";
                    return;
                }


            }
            catch (Exception ex)
            {
                lblStatus.Visible = true;
                lblStatus.Text = "Error in Listing : " + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion

        #region List Click
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet dsPLIData = new DataSet();
                dsPLIData = objPLI.ListPLIData(txtDealid.Text.Trim());
                if (dsPLIData != null && dsPLIData.Tables.Count > 2)
                {
                    if ((bool)dsPLIData.Tables[0].Rows[0]["isExists"])
                    {
                        ddlAgentCode.SelectedIndex = ddlAgentCode.Items.IndexOf(ddlAgentCode.Items.FindByText(dsPLIData.Tables[1].Rows[0]["AgentCode"].ToString()));
                        txtAgentName.Text = ddlAgentCode.SelectedValue;
                        txtFromDt.Text = Convert.ToDateTime(dsPLIData.Tables[1].Rows[0]["ApplicableFrom"].ToString()).ToString("dd/MM/yyyy");
                        txtToDt.Text = Convert.ToDateTime(dsPLIData.Tables[1].Rows[0]["ApplicableTo"].ToString()).ToString("dd/MM/yyyy");
                        rbStandard.Checked = (bool)dsPLIData.Tables[1].Rows[0]["isStandard"];
                        ddlorigintype.SelectedIndex = ddlorigintype.Items.IndexOf(ddlorigintype.Items.FindByText(dsPLIData.Tables[1].Rows[0]["OriginType"].ToString()));
                        ddlorigintype_SelectedIndexChanged(null, null);
                        ddlOrigin.SelectedIndex = ddlOrigin.Items.IndexOf(ddlOrigin.Items.FindByText(dsPLIData.Tables[1].Rows[0]["Origin"].ToString()));
                        ddldestinationType.SelectedIndex = ddldestinationType.Items.IndexOf(ddldestinationType.Items.FindByText(dsPLIData.Tables[1].Rows[0]["DestinationType"].ToString()));
                        ddldestinationType_SelectedIndexChanged(null, null);
                        ddlDestination.SelectedIndex = ddlDestination.Items.IndexOf(ddlDestination.Items.FindByText(dsPLIData.Tables[1].Rows[0]["Destination"].ToString()));
                        ddlFlightNumber.SelectedIndex = ddlFlightNumber.Items.IndexOf(ddlFlightNumber.Items.FindByText(dsPLIData.Tables[1].Rows[0]["FlightNo"].ToString()));
                        ddlCommodity.SelectedIndex = ddlCommodity.Items.IndexOf(ddlCommodity.Items.FindByText(dsPLIData.Tables[1].Rows[0]["Commodity"].ToString()));
                        //DateTime dt = DateTime.Parse(dsPLIData.Tables[1].Rows[0]["ApplicableTo"].ToString());
                        //txtToDt.Text = dt.ToString("dd/MM/yyyy");

                        // Assigning to Grridview
                        DataTable dtCredit = new DataTable();
                        dtCredit = dsPLIData.Tables[2];
                        DataRow drNewRow = dtCredit.NewRow();
                        dtCredit.Rows.Add(drNewRow);
                        Session["dtCreditInfo"] = dtCredit;
                        grdTonnage.DataSource = dtCredit;
                        grdTonnage.DataBind();

                        if ((bool)dsPLIData.Tables[1].Rows[0]["isStandard"])
                        {
                            grdTonnage.HeaderRow.Cells[1].Text = "Sale";
                            grdTonnage.HeaderRow.Cells[2].Text = "Discount(%)";
                        }
                        else
                        {
                            grdTonnage.HeaderRow.Cells[1].Text = "Tonnage";
                            grdTonnage.HeaderRow.Cells[2].Text = "Rate";
                        }

                        if (dsPLIData.Tables[1].Rows[0]["Applicability"].ToString() == "ETA")
                        {
                            rbEntireTonnage.Checked = true;
                        }
                        else
                        {
                            rbEntireTonnage.Checked = false;
                        }

                        txtRateDecide.Text = dsPLIData.Tables[1].Rows[0]["ApplicableRate"].ToString();

                        if (dsPLIData.Tables[1].Rows[0]["SpotRateTonnageIsInclude"].ToString() == "Included")
                        {
                            rbSpotInclude.Checked = true;
                        }
                        else
                        {
                            rbSpotInclude.Checked = false;
                        }


                        // Assigning Exceptions

                        TXTFlightNumber.Text = dsPLIData.Tables[3].Rows[0]["ParamValue"].ToString();
                        RBIncFN.Checked = (bool)dsPLIData.Tables[3].Rows[0]["IsInclude"];
                        TXTFlightCarrier.Text = dsPLIData.Tables[3].Rows[1]["ParamValue"].ToString();
                        RBIncFC.Checked = (bool)dsPLIData.Tables[3].Rows[1]["IsInclude"];
                        TXTHandlingCode.Text = dsPLIData.Tables[3].Rows[2]["ParamValue"].ToString();
                        RBIncHC.Checked = (bool)dsPLIData.Tables[3].Rows[2]["IsInclude"];
                        TXTAirLineCode.Text = dsPLIData.Tables[3].Rows[3]["ParamValue"].ToString();
                        RBIncAC.Checked = (bool)dsPLIData.Tables[3].Rows[3]["IsInclude"];
                        TXTIATAComCode.Text = dsPLIData.Tables[3].Rows[4]["ParamValue"].ToString();
                        RBIncCC.Checked = (bool)dsPLIData.Tables[3].Rows[4]["IsInclude"];
                        TXTAgentCode.Text = dsPLIData.Tables[3].Rows[5]["ParamValue"].ToString();
                        RBIncAD.Checked = (bool)dsPLIData.Tables[3].Rows[5]["IsInclude"];
                        TXTShipperCode.Text = dsPLIData.Tables[3].Rows[6]["ParamValue"].ToString();
                        RBIncSC.Checked = (bool)dsPLIData.Tables[3].Rows[6]["IsInclude"];
                        TXTParamORG.Text = dsPLIData.Tables[3].Rows[7]["ParamValue"].ToString();
                        RBIncORG.Checked = (bool)dsPLIData.Tables[3].Rows[7]["IsInclude"];
                        TXTParamDest.Text = dsPLIData.Tables[3].Rows[8]["ParamValue"].ToString();
                        RBIncDest.Checked = (bool)dsPLIData.Tables[3].Rows[8]["IsInclude"];

                        lblStatus.Visible = false;
                        lblStatus.Text = "";
                    }
                    else
                    {
                        btnClear_Click(null, null);
                        lblStatus.Visible = true;
                        lblStatus.Text = "No such PLIId";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                }
                ChangeControlStatus(false);
                btnEdit.Enabled = true;
            }
            catch (Exception ex)
            {
                lblStatus.Visible = true;
                lblStatus.Text = "Error in Listing : " + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion

        #region rbTonnage Change Event
        protected void rbTonnage_CheckedChanged(object sender, EventArgs e)
        {
            //grdTonnage.Columns[1].HeaderText = "Tonnage";
            //grdTonnage.Columns[2].HeaderText = "Rate";
            //grdTonnage.DataBind();
            grdTonnage.HeaderRow.Cells[1].Text = "Tonnage";
            grdTonnage.HeaderRow.Cells[2].Text = "Rate";
            //DataTable dtTonnage = (DataTable)Session["dtCreditInfo"];
            //grdTonnage.DataSource = dtTonnage;
            //grdTonnage.DataBind();
        }
        #endregion

        #region rbStandard Change Event
        protected void rbStandard_CheckedChanged(object sender, EventArgs e)
        {
            //grdTonnage.Columns[1].HeaderText = "Discount";
            //grdTonnage.Columns[2].HeaderText = "Percent";
            //grdTonnage.DataBind();
            grdTonnage.HeaderRow.Cells[1].Text = "Sale";
            grdTonnage.HeaderRow.Cells[2].Text = "Discount(%)";

            //DataTable dtTonnage = (DataTable)Session["dtCreditInfoStd"];
            //grdTonnage.DataSource = dtTonnage;
            //grdTonnage.DataBind();
        }
        #endregion

        #region Enable/Disable And Clear All Controls
        private void ChangeControlStatus(bool status)
        {

            foreach (Control c in this.Form.Controls)
                foreach (Control ctrl in c.Controls)

                    if (ctrl is TextBox)

                        ((TextBox)ctrl).Enabled = status;

                    else if (ctrl is Button)

                        ((Button)ctrl).Enabled = status;

                    else if (ctrl is RadioButton)

                        ((RadioButton)ctrl).Enabled = status;

                    else if (ctrl is ImageButton)

                        ((ImageButton)ctrl).Enabled = status;

                    else if (ctrl is CheckBox)

                        ((CheckBox)ctrl).Enabled = status;

                    else if (ctrl is DropDownList)

                        ((DropDownList)ctrl).Enabled = status;

                    else if (ctrl is HyperLink)

                        ((HyperLink)ctrl).Enabled = status;

        }
        private void ClearControls()
        {
            foreach (Control c in Page.Controls)
            {
                foreach (Control ctrl in c.Controls)
                {
                    if (ctrl is TextBox)
                    {
                        ((TextBox)ctrl).Text = string.Empty;
                    }
                }
            }
        }
        #endregion

        #region Edit Button Click Operations
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            ChangeControlStatus(true);
            txtDealid.Enabled = false;
            btnSave.Enabled = true;
            btnList.Enabled = false;
            btnClose.Enabled = true;
        }
        #endregion
    }
}
