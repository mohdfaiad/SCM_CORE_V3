using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using QID.DataAccess;
using System.Drawing;
using System.Collections;
using BAL;
using System.Text.RegularExpressions;
using System.Data;


namespace ProjectSmartCargoManager
{
    public partial class ListFrmMessageConfiguration : System.Web.UI.Page
    {
        MessageConfigurationBAL Conf = new MessageConfigurationBAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    getMessage();
                    lblStatus.Text = "";
                    GetAirportCode();
                }

            }
            catch (Exception ex)
            { }
        }
        
        #region List
        protected void btnList_Click(object sender, EventArgs e)
        {
            string PartnerCode, MessageType, Origin, Destination, TransitDestination, FlightNumber,PartnerType;

            if (Page.IsValid)
            {
                try
                {
                    //partner code
                    if (ddlPartnerCode.SelectedItem.Text.Trim() == "Select")
                    {
                        PartnerCode = "";
                    }
                    else
                    {
                        PartnerCode = ddlPartnerCode.SelectedItem.Text.Trim();
                    }

                    //partner type
                    if (ddlPartnerType.SelectedItem.Text.Trim() == "Select")
                    {
                        PartnerType = "";
                    }
                    else
                    {
                        PartnerType = ddlPartnerType.SelectedItem.Text.Trim();
                    }

                    //msg type
                    if (ddlMessageType.SelectedItem.Text == "Select")
                    {
                        MessageType = "";
                    }
                    else
                    {
                        MessageType = ddlMessageType.SelectedItem.Text.Trim();
                    }

                    //origin
                    if (ddlOrigin.SelectedItem.Text=="Select")
                    {
                        Origin = "";
                    }
                    else
                    {
                        Origin = ddlOrigin.SelectedValue.Trim();
                    }

                    //destination
                    if (ddlDestination.SelectedItem.Text=="Select")
                    {
                        Destination = "";
                    }
                    else
                    {
                        Destination = ddlDestination.SelectedValue.Trim();
                    }

                    //transit destination
                    if (ddlTransitDestination.SelectedItem.Text=="Select")
                    {
                        TransitDestination ="";
                    }
                    else
                    {
                        TransitDestination = ddlTransitDestination.SelectedValue.Trim();
                    }

                    //flt no
                    if (txtFltNo.Text == "")
                    {
                        FlightNumber = "";
                    }
                    else
                    {
                        FlightNumber = txtFltNo.Text.TrimEnd(',');
                    }

                    DataSet dsMsgInfo = Conf.getMessageData(PartnerCode,MessageType, Origin, Destination, TransitDestination, FlightNumber, PartnerType);

                    if (dsMsgInfo.Tables[0].Rows.Count > 0)
                    {
                        grdMsgList.DataSource = dsMsgInfo.Tables[0];
                        grdMsgList.DataBind();
                      //  clear();
                        lblStatus.Text = "";
                        Session["dsListMsgConfiguration"] = dsMsgInfo;
                    }
                    else
                    {
                        lblStatus.Text = "Sorry No Message Configuration For this Search ..";
                        lblStatus.ForeColor=Color.Red;
                       // clear();
                        return;
                    }
                }


                catch (Exception ex)
                { }
            }
        }
        #endregion List

        #region GetMessage
        public void getMessage()
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = null;
            try
            {
                ds = da.SelectRecords("Sp_GetMessages");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlMessageType.DataTextField = "MessageName";
                    ddlMessageType.DataSource = ds.Tables[0];
                    ddlMessageType.DataBind();
                    ddlMessageType.Items.Insert(0, new ListItem("Select", ""));
                    ddlMessageType.SelectedIndex = -1;

                    ddlPartnerCode.Items.Insert(0, new ListItem("Select", ""));
                    ddlPartnerCode.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            { }
            finally 
            {
                if (ds != null)
                    ds.Dispose();
                da = null;
            }
        }
        #endregion GetMessage

        #region PartnerType
        protected void ddlPartnerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = new DataSet();
            try
            {
                if (ddlPartnerType.SelectedIndex > -1 && ddlPartnerType.SelectedItem.Text.Length > 0)
                {   
                    ds = da.SelectRecords("spGetPartnerCode", "Type", ddlPartnerType.SelectedItem.Text.Trim(), SqlDbType.VarChar);
                    ddlPartnerCode.DataTextField = "PartnerCode";
                    ddlPartnerCode.DataSource = ds.Tables[0];
                    ddlPartnerCode.DataBind();
                }
                
            }
            catch (Exception ex)
            {
                ds = null;
            }
            finally { 
                if (ds != null)
                    ds.Dispose();
                da = null;
            }

        }
        #endregion PartnerType

        #region grid row commmand "Edit" and "View"
        protected void grdMsgList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = grdMsgList.Rows[index];
            string SrNo = row.Cells[0].Text;
            string cmd = e.CommandName;
            if (cmd == "Delete")
            {
                int srno = Convert.ToInt32(SrNo);
                int r = Conf.DelConfiguratinMessage(srno);
                btnList_Click(null, null);

                if (r == 0)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Record Deleted Successfuly";


                }
                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Error in deleting record";
                }
            }
            else if(cmd=="View" || cmd=="Edit")
            {
                Response.Redirect("FrmMessageConfiguration.aspx?cmd=" + e.CommandName + "&SrNo=" + SrNo);
            }
        }
        #endregion grid row commmand "Edit" and "View"

        private void GetAirportCode()
        {
            BALException objBAL = new BALException();
            DataSet ds = null;

            try
            {
                ds = objBAL.GetAirportCodeList(ddlOrigin.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlOrigin.DataSource = ds;
                            ddlOrigin.DataMember = ds.Tables[0].TableName;
                            ddlOrigin.DataValueField = "AirportCode";
                            ddlOrigin.DataTextField = "Airport";
                            ddlOrigin.DataBind();
                            ddlOrigin.Items.Insert(0, "Select");

                            ddlDestination.DataSource = ds;
                            ddlDestination.DataMember = ds.Tables[0].TableName;
                            ddlDestination.DataValueField = "AirportCode";
                            ddlDestination.DataTextField = "Airport";
                            ddlDestination.DataBind();
                            ddlDestination.Items.Insert(0, "Select");


                            ddlTransitDestination.DataSource = ds;
                            ddlTransitDestination.DataMember = ds.Tables[0].TableName;
                            ddlTransitDestination.DataValueField = "AirportCode";
                            ddlTransitDestination.DataTextField = "Airport";
                            ddlTransitDestination.DataBind();
                            ddlTransitDestination.Items.Insert(0, "Select");
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


        protected void grdMsgList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dsResult = (DataSet)Session["dsListMsgConfiguration"];

            grdMsgList.PageIndex = e.NewPageIndex;
            grdMsgList.DataSource = dsResult.Copy();
            grdMsgList.DataBind();

        }


        public void clear()
        {
            ddlTransitDestination.SelectedIndex = 0;
         ddlOrigin.SelectedIndex = 0;
          ddlDestination.SelectedIndex = 0;
            
            txtFltNo.Text = string.Empty;
            ddlMessageType.SelectedIndex = 0;
            ddlPartnerCode.SelectedIndex = 0;
            ddlPartnerType.SelectedIndex = 0;
        }
    }
}
