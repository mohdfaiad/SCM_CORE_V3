using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QID.DataAccess;
using System.Data;
using System.Drawing;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class FrmRepairOrderList : System.Web.UI.Page
    {
        #region Varibales
        SQLServer da = new SQLServer(Global.GetConnectionString());
        BALRepairOrder objRepr = new BALRepairOrder();
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    GetDropDownData();
                    
                }
                catch (Exception)
                {
                }
            }
        }

        #endregion

        #region GetDropDownData

        protected void GetDropDownData()
        {
            DataSet dsDropDown = new DataSet();

            try
            {
                dsDropDown = da.SelectRecords("sp_GetRepairOrderDropDown");
                if (dsDropDown != null)
                {
                    ddlStation.Items.Clear();
                    ddlStation.DataSource = dsDropDown.Tables[0];
                    ddlStation.DataTextField = "AirportCode";
                    ddlStation.DataValueField = "Code";
                    ddlStation.DataBind();
                    ddlStation.Items.Insert(0, new ListItem("Select", "All"));
                                        
                }

            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region List Button
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DataSet dsList = new DataSet();
            try
            {
                lblStatus.Visible = false;

                string RONo = TxtOrder.Text.Trim();
                string RoDate = Convert.ToString(TxtOrderDate.Text.Trim());
                string Station = ddlStation.SelectedItem.Value;
                string ExpDeliveryDate = Convert.ToString(TxtExpDelDate.Text.Trim());

                dsList = objRepr.GetROList(RONo, RoDate, Station, ExpDeliveryDate);

                if (dsList != null && dsList.Tables.Count > 0 && dsList.Tables[0].Rows.Count > 0)
                {
                    lblStatus.Visible = false;
                    grdROrderList.DataSource = dsList.Tables[0];
                    grdROrderList.DataBind();
                    Session["ROList"] = dsList.Tables[0]; 
                }
                else 
                 {
                        lblStatus.Visible = true;
                        lblStatus.Text = "No Record Found";
                        lblStatus.ForeColor = Color.Red;
                       // grdROrderList.DataSource = dsList.Tables[0];
                       // grdROrderList.DataBind();
                }
                
                
                
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error:" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }

        #endregion

        #region Clear
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/FrmRepairOrderList.aspx");
        }

        #endregion

        #region grdROrderList Page Index
        protected void grdROrderList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = (DataSet)Session["ROList"];
                grdROrderList.PageIndex = e.NewPageIndex;
                grdROrderList.DataSource = ds.Tables[0];
                grdROrderList.DataBind();
            }
            catch(Exception ex)
            {
                lblStatus.Text = "Error:" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion
    }
}
