using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.Data;
using System.Drawing;
using System.Configuration;

namespace ProjectSmartCargoManager
{
    public partial class frmStockList : System.Web.UI.Page
    {
        #region Variables
        //SQLServer db = new SQLServer(Global.GetConnectionString());
        BALStockConfig objBAL = new BALStockConfig();
        EMAILOUT SendEmail = new EMAILOUT();
        clsFillCombo cfc = new clsFillCombo();

        #endregion
        
        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    if (Session["UserName"] != null)
                    {
                        LoadingDropdownDetails(ddlWareHouse);
                        cfc.FillAllComboBoxes("tblULDTypeMaster", "SELECT", ddlULDType);
                        cfc.FillAllComboBoxes("tblULDStatusMaster", "SELECT", ddlULDStatus);
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Button Search Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                gvAddressGrpFTP.DataSource = null;
                gvAddressGrpFTP.DataBind();
                lblStatus.Text = "";
                string StationID = "";
                if (ddlWareHouse.SelectedIndex != 0)
                {
                    StationID = ddlWareHouse.SelectedValue;
                }
                else
                { StationID = "0"; }
                DataSet ds = GetStockList_SearchCriteria(StationID);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            gvAddressGrpFTP.DataSource = ds;
                            gvAddressGrpFTP.DataBind();
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "No Records found";
                        }
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "No Records found";
                    }
                }
                else
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "No Records found";
                }

            }
            catch (Exception ex)
            { }

        }
        #endregion

        #region Button Save
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                bool flag = false;
                Session["Body"] = null;
                Button clickedButton = (Button)sender;
                GridViewRow row = (GridViewRow)clickedButton.Parent.Parent;
                int rowIndex = row.RowIndex;
                string Station = ((Label)(gvAddressGrpFTP.Rows[rowIndex].FindControl("lblStation"))).Text;
                string ULDType = ((Label)(gvAddressGrpFTP.Rows[rowIndex].FindControl("txtULDType"))).Text;
                string MinAvlQty = ((Label)(gvAddressGrpFTP.Rows[rowIndex].FindControl("txtMinAvlQty"))).Text;
                string AvlQty = ((Label)(gvAddressGrpFTP.Rows[rowIndex].FindControl("txtAvlQty"))).Text;
                string EmailID = ((Label)(gvAddressGrpFTP.Rows[rowIndex].FindControl("txtEmailID"))).Text;
                string Body = "Hi,\n\nPlease take appropriate action for reduced ULD stock Mentioned below.\n\nStation:" + Station + "\nULD Type:" + ULDType + "\nAvailable Quantity: " + AvlQty + "\nMinimum Required Quantity: " + MinAvlQty + "\n\nThanks,\nSmartKargo.";
                Session["Body"] = Body;
                txtEmailIDPopUp.Text = EmailID;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);


            }
            catch (Exception ex)
            { }

        }
        #endregion

        #region Loading Dropdown Lists
        public void LoadingDropdownDetails(DropDownList ddlWareHouse)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = objBAL.GetDropdownDetailsforStockReport();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            ddlWareHouse.DataSource = ds.Tables[1];
                            ddlWareHouse.DataTextField = "WHCode";
                            ddlWareHouse.DataValueField = "ID";
                            ddlWareHouse.DataBind();
                            ddlWareHouse.Items.Insert(0, "ALL");

                        }
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Getting Stock List as Per Search Criteria
        public DataSet GetStockList_SearchCriteria(string StationID)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = objBAL.GetStockList(StationID,txtEmailIDPopUp.Text,txtEmailIDPopUp.Text);//SelectRecords("spGetStockList", "StationID", StationID, SqlDbType.Int);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;

                        }
                    }
                }
                return ds;

            }
            catch (Exception ex)
            { return null; }

        }

        #endregion

        #region Button Send Popup
        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                bool flag = false;
                string frmEmailID = ConfigurationManager.AppSettings["FromEmailID"].ToString();
                string Password = ConfigurationManager.AppSettings["Password"].ToString();
                string[] multiEmail = txtEmailIDPopUp.Text.Split(',');
                string Subject = "ULD Stock Reduction Alert";
                string body = Session["Body"].ToString();
                for (int i = 0; i < multiEmail.Length; i++)
                {
                    flag = SendEmail.sendMail(frmEmailID, multiEmail[i], Password, Subject, body, false);
                    if (flag == true)
                    {
                        lblStatus.ForeColor = Color.Green;
                        lblStatus.Text = "Message Sent Successfully";
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Message Sending Failed";
                        return;
                    }
                }
            }
            catch (Exception ex)
            { }

        }
        #endregion

        #region AddressGrpFTPSelection Changed
        protected void gvAddressGrpFTP_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        #endregion

    }
}
