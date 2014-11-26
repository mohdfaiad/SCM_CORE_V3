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
    public partial class frmRepairOrder : System.Web.UI.Page
    {
        #region Variable
        
        SQLServer da = new SQLServer(Global.GetConnectionString());
        //BALRepairOrder ojbBAL = new BALRepairOrder();
        BALRepairOrder objRepr = new BALRepairOrder();

        #endregion

        #region Page Laod
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    GetDropDownData();
                    Session["ULDList"] = null;
                    TxtOrder.Enabled = false;
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

                    ddlPartSerialNo.Items.Clear();
                    ddlPartSerialNo.DataSource = dsDropDown.Tables[1];
                    ddlPartSerialNo.DataTextField = "Code";
                    ddlPartSerialNo.DataValueField = "ID";
                    ddlPartSerialNo.DataBind();
                    ddlPartSerialNo.Items.Insert(0, new ListItem("Select", "All"));

                }

            }
            catch (Exception ex)
            {
               
            }
        }

        #endregion


        #region Button Add
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = objRepr.CheckULDUSeStatus(txtULDNo.Text.Trim());
                if (ds != null)
                {
                    if (ds.Tables[0].Rows[0]["UldUseStatus"].ToString() == "2")
                    {
                        lblStatus.Text = "Partial ULD cannot be repaired!";
                        lblStatus.ForeColor = Color.Blue;
                        return;
                    }
                    
                }
                else
                { 
                    lblStatus.Text = "ULD does not exist!";
                    lblStatus.ForeColor=Color.Blue;
                    return;
                }
                
                DataTable dt = new DataTable();

                if (Session["ULDList"] != null)
                {
                    dt = (DataTable)Session["ULDList"];

                }
                else
                {
                    dt.Columns.Add("ULD");
                    dt.Columns.Add("ULDPart");
                    dt.Columns.Add("SerialNo");
                    dt.Columns.Add("NatureOfDmg");
                }


                DataRow dr = dt.NewRow();
                dr["ULD"] = txtULDNo.Text;
                dr["ULDPart"] = TxtPartNo.Text;
                dr["SerialNo"] = ddlPartSerialNo.SelectedItem;
                dr["NatureOfDmg"] = TxtNatureofDamage.Text;

                dt.Rows.Add(dr);
                grdRepairOrder.DataSource = dt;
                grdRepairOrder.DataBind();
                Session["ULDList"] = dt;

                txtULDNo.Text = string.Empty;
                TxtPartNo.Text = string.Empty;
                ddlPartSerialNo.SelectedIndex = 0;
                TxtNatureofDamage.Text = string.Empty;
            }
            catch (Exception Ex)
            { }

        }

        #endregion


        #region Button Save
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;
            int Cnt = 0;
            bool Result = false;
            string RepairId = "";

            // Region Save Repair Order Details

            try 
	           {	 
                   string RONo = TxtOrder.Text.Trim();
                   string RoDate = TxtOrderDate.Text.Trim();
                   string Station = ddlStation.SelectedItem.Value;
                   string ExpDeliveryDate = TxtExpDelDate.Text.Trim();

                   DateTime CreatedOn = DateTime.Parse(Session["IT"].ToString());
                   string CreatedBy = Session["UserName"].ToString();
                   DateTime UpdatedOn = DateTime.Parse(Session["IT"].ToString());
                   string UpdatedBy = Session["UserName"].ToString();


		             DataSet dsRes = new DataSet();
                     dsRes = objRepr.SetRepairDetails(RONo,RoDate, Station, ExpDeliveryDate, CreatedOn, CreatedBy, UpdatedOn,UpdatedBy);

                     if (dsRes != null && dsRes.Tables.Count > 0 && dsRes.Tables[0].Rows.Count > 0)
                     {
                         lblStatus.Text = "Record Save Successfully";
                         lblStatus.ForeColor = Color.Green;
                     }
                     else
                     {
                         lblStatus.Text = "Record Not Saved, Please Try Again";
                         lblStatus.ForeColor = Color.Red;
                     }

                     RepairId = dsRes.Tables[0].Rows[0]["MaxId"].ToString();
	           }

	        catch (Exception ex)
	        {
                lblStatus.Text = "Error:" + ex.Message;
                lblStatus.ForeColor = Color.Red;
	        }


            // Region Delete Repair Order Details
            try
            {
                //RepairId 
                Result = objRepr.DeleteULDDetails(RepairId);

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error:" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }


            // Region Insert ULD Details

            try
            {

                for (int i = 0; i < grdRepairOrder.Rows.Count; i++)
                {
                    //if (((CheckBox)grdRepairOrder.Rows[i].FindControl("grdChk")).Checked)
                    //{
                        Cnt++;
                        string ULD = ((Label)grdRepairOrder.Rows[i].FindControl("grdULDNo")).Text.Trim();
                        string PartNo = ((Label)grdRepairOrder.Rows[i].FindControl("grdULDPartNo")).Text.Trim();
                        //int PartSrNo = Convert.ToInt32(((Label)grdRepairOrder.Rows[i].FindControl("grdULDPartSerialNo")).Text.Trim());
                        string PartSrNo = ((Label)grdRepairOrder.Rows[i].FindControl("grdULDPartSerialNo")).Text.Trim();
                        string NatureOfDamage = ((Label)grdRepairOrder.Rows[i].FindControl("grdNatureofDamage")).Text.Trim();
                        DateTime UpdatedOn = Convert.ToDateTime(Session["IT"].ToString());
                        string UpdatedBy = Session["UserName"].ToString();

                        //Result = objRepr.SaveULDList(ULD, PartNo, PartSrNo, NatureOfDamage,UpdatedOn,UpdatedBy,Convert.ToInt32(RepairId));
                    DataSet ds = new DataSet();
                    ds = objRepr.SaveULDList(ULD, PartNo,PartSrNo, NatureOfDamage,UpdatedOn,UpdatedBy,RepairId);

                        lblStatus.Text = "Record added Successfully";
                        lblStatus.ForeColor = Color.Green;

                        TxtOrder.Enabled = true;
                        TxtOrder.Text = ds.Tables[1].Rows[0]["RONo"].ToString();
                    //}

                    //else
                    //{
                    //    lblStatus.Text = "Select atleast one record";
                    //    lblStatus.ForeColor = Color.Red;
                    //    return;
                    //}
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error:" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }

        #endregion


        #region Button Cancel
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/frmRepairOrder.aspx");
        }
        #endregion

        
    }
}
