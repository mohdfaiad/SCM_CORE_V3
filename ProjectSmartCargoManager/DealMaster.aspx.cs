using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using BAL;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;   
//Swapnil 07-09-2012
namespace ProjectSmartCargoManager
{
    public partial class DealMaster : System.Web.UI.Page
    {
        EmbargoBAL Embal = new EmbargoBAL();
        AgentBAL objBLL = new AgentBAL();
        BALAgentDeal objDeal = new BALAgentDeal();
        SQLServer da = new SQLServer(Global.GetConnectionString());
        DataSet blnResult;
        DataSet res;
        //Load
        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    GetCommodity();
                    getflightnumber();
                    LoadGrid();
                    LoadAgentName();
                    btnSave.Visible = true;  
                   
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion Load
        //Initial grid initialisation
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
        //To load  origin places in a dropdown
        #region OrignType
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
            { }
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
        //ynamcally adding row to grid
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
            catch (Exception ex) { }
        }
        #endregion SaveGridData
        //Add new row to grid
        #region AddnewRow To Grid
        private void AddNewRowToGrid()
        {
            try
            {
                DataTable dtCreditInfo = (DataTable)Session["dtCreditInfo"];
                DataRow rw = dtCreditInfo.NewRow();

                dtCreditInfo.Rows.Add(rw);

                grdTonnage .DataSource = dtCreditInfo.Copy();
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
        //Add function
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
            { }
        }
        #endregion Add
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
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion SaveGridEmbargo
        //Delete row dynamically
        #region Delete
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            SaveMaterialGrid();
            DataTable dsMaterialDetailsTemp = ((DataTable)Session["dsMaterialDetails"]).Clone();
            DataTable dsMaterialDetails = (DataTable)Session["dsMaterialDetails"];


            for (int i = 0; i < grdTonnage.Rows.Count; i++)
            {

                DataRow rw = dsMaterialDetailsTemp.NewRow();
                rw["Tonnage"] = ((TextBox)grdTonnage.Rows[i].FindControl("txtTonnage")).Text;

                rw["Rate"] = ((TextBox)grdTonnage.Rows[i].FindControl("txtRate")).Text;

                dsMaterialDetailsTemp.Rows.Add(rw);

            }
            if (dsMaterialDetailsTemp != null)
            {
                dsMaterialDetailsTemp.Rows[dsMaterialDetailsTemp.Rows.Count - 1].Delete();
            }
            Session["dsMaterialDetails"] = dsMaterialDetailsTemp.Copy();
            grdTonnage.DataSource = dsMaterialDetailsTemp.Copy();
            grdTonnage.DataBind();

            for (int i = 0; i < dsMaterialDetailsTemp.Rows.Count; i++)
            {
                ((TextBox)grdTonnage.Rows[i].FindControl("txtTonnage")).Text = dsMaterialDetailsTemp.Rows[i]["Tonnage"].ToString();
                ((TextBox)grdTonnage.Rows[i].FindControl("txtRate")).Text = dsMaterialDetailsTemp.Rows[i]["Rate"].ToString();
               
            }

        }
        #endregion Delete
        //Paging for a grid
        #region SelectedIndexChanging
        protected void grdTonnage_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            //grdTonnage.PageIndex = e.NewPageIndex;
            //grdTonnage.DataSource = dsResult.Copy();
            //grdTonnage.DataBind();

        }
        #endregion SelectedIndexChanging
        //Load agent name in a dopdown
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
        //Save data in a database
        #region Save
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string AgentCode = "", OriginType = "", Origin = "", DestinationType = "", Destination = "", FlightNo = "", Commodity = "", Rate = "";
                string RateSlab="",SpotRateTonnageIsInclude="",Commissionable="",updatedBy="";
                DateTime ApplicableFrom,ApplicableTo,UpdatedOn;
                bool M=false, N=false, Q1=false , Q2=false ;

                double Tonnage, KickbackAmount, Threshold;
                for (int i = 0;i<grdTonnage.Rows.Count ; i++)
                {
                    Rate = ((TextBox)grdTonnage.Rows[i].FindControl("txtRate")).Text;
                    Tonnage = Convert.ToDouble(((TextBox)grdTonnage.Rows[i].FindControl("txtTonnage")).Text);

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
                    if (txtaplicablefrom.Text == "")
                    {
                        lblStatus.Text = "Please Enter Applicable From date";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    else
                    {
                        ApplicableFrom = DateTime.ParseExact(txtaplicablefrom.Text, "dd-MM-yyyy", null);
                    }
                    if (txtApplicableto.Text == "")
                    {
                        lblStatus.Text = "Please Enter Applicable To date";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    else
                    {
                        ApplicableTo = DateTime.ParseExact(txtApplicableto.Text, "dd-MM-yyyy", null);
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
                    if(chkN.Checked || chkM.Checked || chkA.Checked || chkB.Checked)
                    {
                    RateSlab = "Yes"; 
                    }
                    else{
                    RateSlab="No"; 
                    }
                    if (rdbyesCommisionable.Checked)
                    {
                        Commissionable = "Yes";
                    }
                    else if (rdbNoCommissionable.Checked)
                    {
                        Commissionable = "No";
                    }
                    if (chkM.Checked)
                    {
                        M = true;
                    }
                     if (chkN.Checked)
                    {
                        N = true;
                    }
                     if (chkA.Checked)
                    {
                         Q1=true ;
                    }
                    if (chkB.Checked)
                    {
                        Q2= true ;
                    }
                    if (rdbIncluded.Checked)
                    {
                        SpotRateTonnageIsInclude = "Include";
                    }
                    else if (rbExcluded.Checked)
                    {
                        SpotRateTonnageIsInclude = "Exclude";
                    }

                    KickbackAmount = Convert.ToDouble(txtkickbackamount.Text);
                    Threshold = Convert.ToDouble(txtthrshold.Text);
                    updatedBy = Session["UserName"].ToString();
                    UpdatedOn = Convert.ToDateTime(System.DateTime.Now.ToShortDateString());

                     res = objDeal.SaveAgentDeal(AgentCode, OriginType, Origin, DestinationType, Destination, FlightNo, Commodity, Rate, RateSlab, SpotRateTonnageIsInclude, Commissionable, updatedBy, ApplicableFrom, ApplicableTo, UpdatedOn, Tonnage, KickbackAmount, Threshold,M,N,Q1,Q2);

                }
                if (res.Tables[0].Rows.Count > 0)
                {
                    txtDealid.Text = res.Tables[0].Rows[0]["Column1"].ToString();
                    lblStatus.Text = "Deal Saved Successully";
                    lblStatus.ForeColor = Color.Green;
                    btnSave.Visible = false;  
                    return;
                }
                else
                {
                    lblStatus.Text = "Deal Not Saved Please check data.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }


            }
            catch (Exception ex) 
            {
                lblStatus.Text = "Deal Not Saved Please check data.";
                lblStatus.ForeColor = Color.Red;
                return;
            }
        }
        #endregion Save
        //clear data frmo textboxes
        #region Clear
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Visible = true;
                txtDealid.Text = "";
                txtaplicablefrom.Text = "";
                txtApplicableto.Text = "";
                ddlAgentCode.SelectedIndex = -1;
                LoadGrid();
                txtthrshold.Text = "";
                txtkickbackamount.Text = "";
                lblStatus.Text = "";  
            }
            catch (Exception ex)
            { }
        }
#endregion Clear
    }
}
