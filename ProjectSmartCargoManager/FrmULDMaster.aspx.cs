using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using QID.DataAccess;
using BAL;
using System.Drawing;
using System.Data.SqlClient;
using System.Configuration;

namespace ProjectSmartCargoManager
{
    public partial class FrmULDMaster : System.Web.UI.Page
    {
        #region variables
        BALULDMaster uld = new BALULDMaster();
        clsFillCombo cfc = new clsFillCombo();
        string ULDNO = "";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            lblStatus.ForeColor = System.Drawing.Color.Red;
            lblStatus.Text = "";
            //txtULDLocDate.Attributes.Add("ReadOnly", "ReadOnly");

            try
            {
                if (!IsPostBack)
                {
                    //lblOwner.Text = ConfigurationManager.AppSettings["CompName"].ToString();
                    //uld.fillULDOwner("tblAirlineMaster", "Select", drpOwner);
                    cfc.FillAllComboBoxes("tblULDStatusMaster", "Select", ddlULDStatus);
                    cfc.FillAllComboBoxes("tblULDTypeMasters", "Select", ddlULDType);
                    cfc.FillAllComboBoxes("tblWarehouseMaster", "Select", ddlSubWH);
                    ddlSubWH.SelectedIndex = ddlSubWH.Items.IndexOf(ddlSubWH.Items.FindByText(Session["Station"].ToString()));

                    cfc.FillAllComboBoxes("currencymaster", "Select", ddluldcurr);
                    cfc.FillAllComboBoxes("tblULDOwner", "Select", drpOwner);
                    cfc.FillAllComboBoxes("ULDUSESTATUS","Free", ddlULDUseStatus);
                    //ddlULDType.SelectedIndex = 1;
                    //ddlULDStatus.SelectedIndex = 3;
                    txtULDLocDate.Text = DateTime.Now.ToString();
                    //ddlULDType.SelectedIndex = ddlULDType.Items.IndexOf(ddlULDType.Items.FindByText("AKE"));
                    ddlULDStatus.SelectedIndex = ddlULDStatus.Items.IndexOf(ddlULDStatus.Items.FindByText("SERVICEABLE"));
                    ddluldcurr.SelectedIndex = ddluldcurr.Items.IndexOf(ddluldcurr.Items.FindByText("USD"));



                    if (Request.QueryString["ULDNO"] == null || Request.QueryString["ULDNO"] == "")
                    {

                    }
                    else
                    {
                        ULDNO = Request.QueryString["ULDNO"].ToString();
                        ddlULDType.SelectedIndex = ddlULDType.Items.IndexOf(ddlULDType.Items.FindByText(ULDNO.Substring(0, 3)));
                        txtULDNo.Text = ULDNO.Substring(3, ULDNO.Length - 5);
                        drpOwner.SelectedIndex = drpOwner.Items.IndexOf(drpOwner.Items.FindByText(ULDNO.Substring(ULDNO.Length - 2, 2)));
                        btnSearch_Click(null, null);
                    }

                }
            }
            catch (Exception ex)
            { 
            
            }
            
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet("ULDMST_DS1");
            try
            {
                //string uldnum=ddlULDType.SelectedItem.Text.ToString() + txtULDNo.Text.Trim() + ConfigurationManager.AppSettings["CompName"].ToString();
                //string ownernm=ConfigurationManager.AppSettings["CompName"].ToString();

                string uldnum = ddlULDType.SelectedItem.Text.ToString() + txtULDNo.Text.Trim() + drpOwner.SelectedItem.Text.ToString();
                string va = ddlULDType.SelectedItem.Value.ToString();
                if (va.Contains('-'))
                { va = va.Remove(va.IndexOf('-')); }
                else
                {

                }

                string unm = Session["UserName"].ToString();
                DateTime dt = Convert.ToDateTime(Session["IT"]);

                string strddlwh;
                if (ddlWH.SelectedIndex == 0)
                {
                    strddlwh = "W";
                }
                else
                {
                    strddlwh = "S";
                }


                #region Commented
                //string[] paramname = new string[20];
                //paramname[0] = "ULDNo";
                //paramname[1] = "Owner";
                //paramname[2] = "PurchaseCost";
                //paramname[3] = "CostCurr";
                //paramname[4] = "EcoRprPt";
                //paramname[5] = "Length";
                //paramname[6] = "Width";
                //paramname[7] = "Height";
                //paramname[8] = "TareWgt";
                //paramname[9] = "ULDStatus";
                //paramname[10] = "Certification";
                //paramname[11] = "Remark";
                //paramname[12] = "isForeign";
                //paramname[13] = "ULDTypeID";
                //paramname[14] = "ULDSerialNo";
                //paramname[15] = "LocSource";
                //paramname[16] = "UpdatedBy";
                //paramname[17] = "UpdatedOn";
                //paramname[18] = "LocType";
                //paramname[19] = "LocID";



                //object[] paramvalue = new object[20];
                //paramvalue[0] = ddlULDType.SelectedItem.Text.ToString() + txtULDNo.Text.Trim() + ConfigurationManager.AppSettings["CompName"].ToString();
                //paramvalue[1] = ConfigurationManager.AppSettings["CompName"].ToString();
                //paramvalue[2] = txtULDPurCost.Text.Trim();
                //paramvalue[3] = ddluldcurr.SelectedItem.Text.Trim();
                //paramvalue[4] = txtULDEcoRprPoint.Text.Trim();
                //paramvalue[5] = txtULDL.Text.Trim();
                //paramvalue[6] = txtULDW.Text.Trim();
                //paramvalue[7] = txtULDH.Text.Trim();
                //paramvalue[8] = txtULDTare.Text.Trim();
                //paramvalue[9] = ddlULDStatus.Text.Trim();
                //paramvalue[10] = txtCertification.Text.Trim();
                //paramvalue[11] = txtComment.Text.Trim();
                //paramvalue[12] = "false";
                //string va = ddlULDType.SelectedItem.Value.ToString();
                //if (va.Contains('-'))
                //{ va = va.Remove(va.IndexOf('-')); }
                //else
                //{

                //}

                //paramvalue[13] = va;
                //paramvalue[14] = txtULDNo.Text.Trim();
                //paramvalue[15] = ddlDimSource.SelectedItem.Text.Trim();
                //paramvalue[16] = Session["ULDUserName"].ToString();
                //paramvalue[17] = Convert.ToDateTime(System.DateTime.Now.ToString());
                //if (ddlWH.SelectedIndex == 0)
                //{
                //    paramvalue[18] = "W";
                //}
                //else
                //{
                //    paramvalue[18] = "S";
                //}
                //paramvalue[19] = ddlSubWH.SelectedItem.Value.ToString();


                //SqlDbType[] paramtype = new SqlDbType[20];
                //paramtype[0] = SqlDbType.NVarChar;
                //paramtype[1] = SqlDbType.NVarChar;
                //paramtype[2] = SqlDbType.Real;
                //paramtype[3] = SqlDbType.NVarChar;
                //paramtype[4] = SqlDbType.NVarChar;
                //paramtype[5] = SqlDbType.Real;
                //paramtype[6] = SqlDbType.Real;
                //paramtype[7] = SqlDbType.Real;
                //paramtype[8] = SqlDbType.NVarChar;
                //paramtype[9] = SqlDbType.NVarChar;
                //paramtype[10] = SqlDbType.NVarChar;
                //paramtype[11] = SqlDbType.NVarChar;
                //paramtype[12] = SqlDbType.Bit;
                //paramtype[13] = SqlDbType.NVarChar;
                //paramtype[14] = SqlDbType.NVarChar;
                //paramtype[15] = SqlDbType.NVarChar;
                //paramtype[16] = SqlDbType.NVarChar;
                //paramtype[17] = SqlDbType.DateTime;
                //paramtype[18] = SqlDbType.NVarChar;
                //paramtype[19] = SqlDbType.BigInt;


                //SQLServer db =new SQLServer(constr
                //database  db = 
                //DataSet dsdrp = db.SelectRecords("spInsertULDMaster", paramname, paramvalue, paramtype);
                #endregion Commented


                DateTime dtLocatedon = Convert.ToDateTime(txtULDLocDate.Text);

                //DateTime dtLocatedon = Convert.ToDateTime(txtULDLocDate.Text).TimeOfDay();

                // txtULDLocDate.Text = "dtLocatedon";
                //dtLocatedon = System.DateTime.Now.ToString();

                if (txtULDPurCost.Text.Trim() == "")
                {
                    txtULDPurCost.Text = "0";
                }
                if (txtULDL.Text.Trim() == "")
                {
                    txtULDL.Text = "0";
                }
                if (txtULDW.Text.Trim() == "")
                {
                    txtULDW.Text = "0";
                }
                if (txtULDH.Text.Trim() == "")
                {
                    txtULDH.Text = "0";
                }
                if (txtULDTare.Text.Trim() == "")
                {
                    txtULDTare.Text = "0";
                }
                string IsReceived = "";
                if (chkIsReceived.Checked)
                {
                    IsReceived = "Y";
                }
                else
                {
                    IsReceived = "N";
                }
                decimal MaxGrossWt = 0;
                if (!decimal.TryParse(txtMaxGrossWt.Text.Trim(),out MaxGrossWt))
                {
                    txtMaxGrossWt.Text = "0";
                    MaxGrossWt = 0;
                }

                ds = uld.SelectRecords(uldnum, drpOwner.SelectedItem.Text.ToString(), 
                    float.Parse(txtULDPurCost.Text), ddluldcurr.SelectedItem.Text.ToString(), 
                    txtULDEcoRprPoint.Text.ToString(), float.Parse(txtULDL.Text), float.Parse(txtULDW.Text), 
                    float.Parse(txtULDH.Text.ToString()), txtULDTare.Text.ToString(), ddlULDStatus.SelectedValue, 
                    txtCertification.Text.ToString(), txtComment.Text.ToString(), false, va, txtULDNo.Text.ToString(), 
                    ddlDimSource.SelectedItem.Text.ToString(), unm, dt, strddlwh, 
                    Int16.Parse(ddlSubWH.SelectedItem.Value.ToString()), ddlULDWei.SelectedItem.Text.Trim(), 
                    ddlTareWei.SelectedItem.Text.Trim(), dtLocatedon, txtULDManu.Text.Trim(), "", "", 
                    chkIsThirdParty.Checked, ddlULDUseStatus.SelectedValue, IsReceived,txtDollyWt.Text.Trim(),
                    MaxGrossWt);

                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0][0].ToString() == "2")
                        {
                            lblStatus.Text = "ULD No already Available";
                        }
                        else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                        {
                            lblStatus.Text = "Error during ULD Insert";
                        }
                        else
                        {
                            lblStatus.Text = "Save Successfully";
                            lblStatus.ForeColor = System.Drawing.Color.Green;
                        }
                    }
                }

                //btnClear_Click(sender, e);

                //if (ds == true)
                //{
                //    lblStatus.Text = "Save Successfully done";
                //}
                //else {
                //    lblStatus.Text = "Save Failed";
                //}

            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }

        

        public void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmULDMaster.aspx", false);
        }

        protected void ddlWH_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlWH.SelectedIndex == 0)
            {
                cfc.FillAllComboBoxes("tblWarehouseMaster", "Select", ddlSubWH);
            }
            else
            {
                cfc.FillAllComboBoxes("tblWarehouseAndSubWarehouseMaster", "Select", ddlSubWH);
            }
        }

        protected void ddlULDType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlULDType.SelectedIndex >= 0)
            {
                DataSet ds = new DataSet("ULDMST_DS2");
                ds = uld.GetULDDimensions(ddlULDType.SelectedItem.Text.ToString());
                txtULDL.Text = ds.Tables[0].Rows[0][0].ToString();
                txtULDW.Text = ds.Tables[0].Rows[0][1].ToString();
                txtULDH.Text = ds.Tables[0].Rows[0][2].ToString();
                txtULDTare.Text = ds.Tables[0].Rows[0][3].ToString();
            }
              
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet("ULDMST_DS3");
            try
            {
                string uldnum = "";
                #region Commented
                //string[] paramname = new string[1];
                //paramname[0] = "ULDNo";

                //object[] paramvalue = new object[1];
                //paramvalue[0] = ddlULDType.SelectedItem.Text.ToString() + txtULDNo.Text.Trim() + ConfigurationManager.AppSettings["CompName"].ToString();

                //SqlDbType[] paramtype = new SqlDbType[1];
                //paramtype[0] = SqlDbType.NVarChar;

                //SQLServer db = new SQLServer();
                //DataSet ds = db.SelectRecords("spSearchULDNo", paramname, paramvalue, paramtype);

                #endregion Commented
                if (ULDNO != "")
                {
                    uldnum = ULDNO.ToString();
                }
                else
                {
                    uldnum = ddlULDType.SelectedItem.Text.ToString() + txtULDNo.Text.Trim() + drpOwner.SelectedItem.Text.ToString();
                }

                ds = uld.Search(uldnum);

                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //txtPO.Text = ds.Tables[0].Rows[0][""].ToString();
                        txtULDPurCost.Text = ds.Tables[0].Rows[0]["PurchaseCost"].ToString();
                        txtULDManu.Text = ds.Tables[0].Rows[0]["Manufacturer"].ToString();
                        ddluldcurr.SelectedIndex = ddluldcurr.Items.IndexOf(ddluldcurr.Items.FindByText(ds.Tables[0].Rows[0]["Currency"].ToString()));
                        txtULDEcoRprPoint.Text = ds.Tables[0].Rows[0]["EcoReportPoint"].ToString();
                        txtULDL.Text = ds.Tables[0].Rows[0]["Length"].ToString();
                        txtULDW.Text = ds.Tables[0].Rows[0]["Width"].ToString();
                        txtULDH.Text = ds.Tables[0].Rows[0]["Height"].ToString();
                        txtULDTare.Text = ds.Tables[0].Rows[0]["TareWeight"].ToString();
                        ddlULDStatus.SelectedValue = ds.Tables[0].Rows[0]["ULDStatusID"].ToString();
                        ddlULDType.SelectedValue = ds.Tables[0].Rows[0]["ULDTypeID"].ToString();
                        drpOwner.SelectedIndex = drpOwner.Items.IndexOf(drpOwner.Items.FindByText(ds.Tables[0].Rows[0]["Owner"].ToString()));
                        if (ds.Tables[0].Rows[0]["ULDUseStatus"] == null)
                        {
                            ddlULDUseStatus.SelectedValue = "0";
                        }
                        else
                        {
                            ddlULDUseStatus.SelectedValue = (ds.Tables[0].Rows[0]["ULDUseStatus"].ToString());
                        }
                        ddlSubWH.SelectedValue = ds.Tables[0].Rows[0]["LocationID"].ToString();
                        ddlDimSource.SelectedIndex = ddlDimSource.Items.IndexOf(ddlDimSource.Items.FindByText(ds.Tables[0].Rows[0]["LocationSource"].ToString()));
                        txtULDLocDate.Text = ds.Tables[0].Rows[0]["LocatedOn"].ToString();
                        txtComment.Text = "";
                        ddlTareWei.SelectedIndex = ddlTareWei.Items.IndexOf(ddlTareWei.Items.FindByText(ds.Tables[0].Rows[0]["TareWgVal"].ToString()));
                        ddlULDWei.SelectedIndex = ddlULDWei.Items.IndexOf(ddlULDWei.Items.FindByText(ds.Tables[0].Rows[0]["DimWgVal"].ToString()));
                        ddlWH.SelectedIndex = 0;
                        chkIsThirdParty.Checked = bool.Parse(ds.Tables[0].Rows[0]["IsThirdParty"].ToString());

                        if (ds.Tables[0].Rows[0]["IsReceived"] != null)
                        {
                            if (ds.Tables[0].Rows[0]["IsReceived"].ToString() == "Y")
                            {
                                chkIsReceived.Checked = true;
                            }
                            else
                            {
                                chkIsReceived.Checked = false;
                            }
                        }
                        else
                        {
                            chkIsReceived.Checked = false;
                        }
                        txtDollyWt.Text = ds.Tables[0].Rows[0]["DollyWt"].ToString();
                        txtMaxGrossWt.Text = ds.Tables[0].Rows[0]["MaxGrossWt"].ToString();
                        //ddlWH_SelectedIndexChanged(sender, e);
                        //for (int i = 0; i < ddlSubWH.Items.Count; i++)
                        //{
                        //    if (ddlSubWH.Items[i].Text == ds.Tables[0].Rows[0][3].ToString())
                        //    {
                        //        ddlSubWH.SelectedIndex = i;
                        //    }
                        //}
                        //ddlSubWH.Items.IndexOf(new ListItem(ds.Tables[0].Rows[0][3].ToString()));

                    }
                    else
                    {
                        lblStatus.Text = "No Such ULD Available";
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }        
    }
}
