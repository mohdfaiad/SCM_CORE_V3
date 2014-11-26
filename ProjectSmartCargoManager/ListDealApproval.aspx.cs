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
//7-9-2012
namespace ProjectSmartCargoManager
{
    public partial class ListDealApproval : System.Web.UI.Page
    {
        AgentBAL objBLL = new AgentBAL();
        BALAgentDeal objdeal = new BALAgentDeal();
        bool ds;
        SQLServer da = new SQLServer(Global.GetConnectionString());
        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Request.QueryString["id"] == "0")
                {
                    btnApprove.Visible = true;
                    btnClose.Visible = true;

                }
                else if (Request.QueryString["id"] == "1")
                {
                    btnApprove.Visible = false;
                    btnClose.Visible = false;
                }
                LoadAgentName();
                getflightnumber(); 

            }

        }
        #endregion Load 
        /// <summary>
        /// Loads Agent Name in a dropdown
        /// </summary>
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
        /// <summary>
        /// Loads data  in a dropdown according to selection in a dropdown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Origin type
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
                    ddlOrigin.Items.Insert(0, new ListItem("All", string.Empty));
                }
            }
            catch (Exception ex)
            { }
        }
#endregion Origin Type
        /// <summary>
        /// Loads  data according to selection in a dropdwn of origin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region DestinationType
        protected void ddldestinationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListDataViewBAL objDataViewBAL = new ListDataViewBAL();
                DataSet dsResult = new DataSet();
                string errormessage = "";

                string level = ddlorigintype.SelectedItem.Value;
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
        #endregion destinatiionType
        /// <summary>
        /// Loads flght data in a dropdown
        /// </summary>
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
                            ddlFlightNumber.Items.Insert(0, new ListItem("All", ""));
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
        /// <summary>
        /// Paging for grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        #region PageIndex
        protected void grdDealApproval_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dsResult = (DataSet)Session["dsSpotRateDetails"];

            grdDealApproval.PageIndex = e.NewPageIndex;
            grdDealApproval.DataSource = dsResult.Copy();
            grdDealApproval.DataBind();
        }
        #endregion PageIndex
        /// <summary>
        /// Approve the rate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Approve
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            
            bool IsSelected = false;
            try
            {
                for (int i = 0; i < grdDealApproval.Rows.Count; i++)
                {
                    if (((CheckBox)grdDealApproval.Rows[i].FindControl("check")).Checked == true)
                    {
                        IsSelected = true;

                    }
                }
                if (IsSelected)
                {
                    for (int i = 0; i < grdDealApproval.Rows.Count; i++)
                    {
                        if (((CheckBox)grdDealApproval.Rows[i].FindControl("check")).Checked == true)
                        {
                            IsSelected = true;

                            string SrNo = ((Label)grdDealApproval.Rows[i].FindControl("lblSrNo")).Text;
                            string Rate = ((TextBox)grdDealApproval.Rows[i].FindControl("txtRate")).Text;
                            ds = objdeal.UpdateStatusApproval(SrNo,Rate);
                            

                        }

                    }
                    if (ds == true)
                    {
                        lblStatus.Text = "Deal Approved Successfully";
                        lblStatus.ForeColor = Color.Green;
                        return;
                    }
                    else
                    {
                        lblStatus.Text = "Deal Not Approved";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                }
                if (!IsSelected)
                {
                    lblStatus.Text = "Please select AWB Number(s) for Approval.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Deal Not Approved";
                lblStatus.ForeColor = Color.Red;
                return;
            }

        }
        #endregion Approve
        /// <summary>
        /// List data 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region list
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                string origin = "", destination = "", FlightNo = "", AgentCode = "",DealId="";
                string ApplicableFrom="", ApplicableTo="";
                if (ddlOrigin.SelectedItem.Text == "Select" || ddlOrigin.SelectedItem.Text=="All")
                {
                    origin = "";
                }
                else 
                {
                origin = ddlOrigin.SelectedItem.Text;
                }
                if (ddlDestination.SelectedItem.Text == "Select" || ddlDestination.SelectedItem.Text=="All"   )
                {
                    destination = "";
                }
                else
                {
                    destination = ddlDestination.SelectedItem.Text;
                }
                if (ddlFlightNumber.SelectedItem.Text == "All")
                {
                    FlightNo = "";
                }
                else
                {
                    FlightNo = ddlFlightNumber.SelectedItem.Text;
                }
                if (ddlAgentCode.SelectedItem.Text.Trim()== "Select")
                {
                    AgentCode = ""; 
                }
                else 
                {
                    AgentCode = ddlAgentCode.SelectedItem.Text;
                }
                if (txtApplicableFrom.Text == "")
                {
                    ApplicableFrom=""; 
                }
                else
                {
                    ApplicableFrom = txtApplicableFrom.Text;
                }
                if (txtApplicableTo.Text == "")
                {
                    ApplicableTo = "";
                }
                else
                {
                    ApplicableTo = txtApplicableTo.Text;
                }
                if (txtdealid.Text == "")
                {
                    DealId = "";
                }
                else
                {
                    DealId = txtdealid.Text;   
                }

                DataSet ds1;
                ds1 = objdeal.LIstDeals(origin,destination,FlightNo,AgentCode,ApplicableFrom,ApplicableTo,DealId);

                if (ds1 != null)
                {
                    if (ds1.Tables.Count > 0)
                    {
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            grdDealApproval.DataSource = ds1.Tables[0];
                            grdDealApproval.DataBind();
                            Session["dsSpotRateDetails"] = ds1.Copy();
                            LBLNoOfRecords.Text = "No. Of Records :" + ds1.Tables[0].Rows.Count;
                            //for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
                            //{
                            //    string Approval=ds1.Tables[0].Rows[k][10].ToString();
                            //    if (Approval == "Approved")
                            //    {

                            //        for (int i = 0; i < grdDealApproval.Rows.Count; i++)
                            //        {
                            //            if (Approval == "Approved")
                            //            {
                            //                for (int j = 0; j < grdDealApproval.Columns.Count; j++)
                            //                {
                            //                    grdDealApproval.Rows[i].Cells[j].Enabled = false;
                            //                }
                            //            }
                            //        }
                                    
                            //    }
                            //}
                            

                        }
                        else
                        {
                            lblStatus.Text = "No Deals Are there.Please try again";
                        }
                    }
                    else
                    {
                        lblStatus.Text = "No Deals Are there.Please try again";  
                    }
                }
     

            }
            catch (Exception ex)
            { 
            }
        }
        #endregion list
    }
}
