using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using BAL;

/*

2012-05-10 vinayak
2012-05-14 vinayak

*/

namespace ProjectSmartCargoManager
{
    public partial class SpotRateTransaction : System.Web.UI.Page
    {

        SpotRateTransactionBAL objBAL = new SpotRateTransactionBAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    DataSet dsAWBDetails = new DataSet();
                    dsAWBDetails.Tables.Add();

                    dsAWBDetails.Tables[0].Columns.Add("AWBNumber");
                    dsAWBDetails.Tables[0].Columns.Add("CommCode");
                    dsAWBDetails.Tables[0].Columns.Add("FrIATA");
                    dsAWBDetails.Tables[0].Columns.Add("FrMKT");
                    dsAWBDetails.Tables[0].Columns.Add("OcDueCar");
                    dsAWBDetails.Tables[0].Columns.Add("OcDueAgent");
                    dsAWBDetails.Tables[0].Columns.Add("SpotRate");
                    dsAWBDetails.Tables[0].Columns.Add("DynRate");
                    dsAWBDetails.Tables[0].Columns.Add("ServTax");

                    DataRow row = dsAWBDetails.Tables[0].NewRow();
                    dsAWBDetails.Tables[0].Rows.Add(row);


                    GRDAWBDetails.DataSource = dsAWBDetails;
                    GRDAWBDetails.DataBind();

                    Session["dsAWBDetails"] = dsAWBDetails.Copy();

                }

                if (Session["Station"].ToString() != "")
                {
                    DDLStation.Items.Clear();
                    DDLStation.Items.Add(Session["Station"].ToString());
                    DDLStation.SelectedIndex = 0;
                    DDLStation.Enabled = false;
                }
                else
                {
                    LBLStatus.ForeColor = Color.Red;
                    LBLStatus.Text = "Station is not set.";
                    return;
                }

                if (Session["UserName"].ToString() != "")
                {
                    TXTIssuedBy.Text = Session["UserName"].ToString();
                    TXTIssuedBy.Enabled = false;
                }

                TXTIssuedDate.Text = "" + DateTime.Now.ToString("yyyy-MM-dd");
                TXTIssuedDate.Enabled = false;

            }
            catch (Exception ex)
            {
                LBLStatus.ForeColor = Color.Red;
                LBLStatus.Text = "" + ex.Message;
            }

        }

        protected void TXTAWBNumber_TextChanged(object sender, EventArgs e)
        {
            try
            {
                LBLStatus.Text = "";
                
                DDLCommodityCode.Enabled = false;
                DDLCommodityCode.DataSource = null;
                DDLCommodityCode.Items.Clear();
                DDLCommodityCode.Items.Add("Select");

                if (TXTAWBNumber.Text.Trim() == "")
                {
                    LBLStatus.ForeColor = Color.Red;
                    LBLStatus.Text = "AWB Number is not valid.";                    
                    return;
                }
                try
                {
                    int.Parse(TXTAWBNumber.Text.Trim());
                }
                catch(Exception ex)
                {
                    LBLStatus.ForeColor = Color.Red;
                    LBLStatus.Text = "AWB Number is not valid.";                    
                    return;
                }

                DataSet dsResult=new DataSet();
                string errormessage="";
                if (objBAL.GetAWBCommodites(TXTAWBNumber.Text.Trim(), ref dsResult, ref errormessage))
                {
                    if (dsResult.Tables[0].Rows.Count != 0)
                    {
                        DDLCommodityCode.DataTextField = "CommodityCode";
                        DDLCommodityCode.DataValueField = "CommodityCode";
                        DDLCommodityCode.DataSource = dsResult.Tables[0];
                        DDLCommodityCode.DataBind();
                        
                    }
                    else
                    {
                        LBLStatus.ForeColor = Color.Red;
                        LBLStatus.Text = "Details not found.";                        
                        return;
                    }
                }
                else
                {
                    LBLStatus.ForeColor = Color.Red;
                    LBLStatus.Text = "Details not found. Error :" + errormessage;                    
                    return;
                }

                DDLCommodityCode.Enabled = true;
            }
            catch (Exception ex)
            {
                LBLStatus.ForeColor = Color.Red;
                LBLStatus.Text = "" + ex.Message;
            }
        }

        protected void btnSearchSpecific_Click(object sender, EventArgs e)
        {
            try
            {
                LBLStatus.Text = "";
                ClearAWBDetailsGrid();

                if (TXTAWBNumber.Text.Trim() == "")
                {
                    LBLStatus.ForeColor = Color.Red;
                    LBLStatus.Text = "AWB Number is not valid.";
                    return;
                }
                try
                {
                    int.Parse(TXTAWBNumber.Text.Trim());
                }
                catch (Exception ex)
                {
                    LBLStatus.ForeColor = Color.Red;
                    LBLStatus.Text = "AWB Number is not valid.";
                    return;
                }

                if (DDLCommodityCode.SelectedItem.Text.Trim() == "Select" || DDLCommodityCode.SelectedItem.Text.Trim() == "")
                {
                    LBLStatus.ForeColor = Color.Red;
                    LBLStatus.Text = "Commodity code not valid.";
                    return;

                }



                DataSet dsResult = new DataSet();
                string errormessage = "";
                object[] values = { TXTAWBNumber.Text.Trim(),DDLCommodityCode.SelectedItem.Text};

                if (objBAL.GetAWBCommDetails(TXTAWBNumber.Text.Trim(),values, ref dsResult, ref errormessage))
                {

                    DataSet dsAWBDetails = ((DataSet)Session["dsAWBDetails"]).Clone();
                    DataRow row = dsAWBDetails.Tables[0].NewRow();
                    
                    row["AWBNumber"]=TXTAWBNumber.Text;
                    row["CommCode"]=DDLCommodityCode.SelectedItem.Text;
                    row["FrIATA"]=dsResult.Tables[0].Rows[0]["Charge"].ToString();
                    row["FrMKT"]=dsResult.Tables[1].Rows[0]["Charge"].ToString();
                    row["OcDueCar"]=dsResult.Tables[2].Rows[0]["Charge"].ToString();
                    row["OcDueAgent"]=dsResult.Tables[3].Rows[0]["Charge"].ToString();
                    row["SpotRate"] = "";
                    row["DynRate"] = "";

                    decimal totaltax = 0;
                    totaltax += decimal.Parse(dsResult.Tables[0].Rows[0]["Tax"].ToString() == "" ? "0" : dsResult.Tables[0].Rows[0]["Tax"].ToString());
                    totaltax += decimal.Parse(dsResult.Tables[1].Rows[0]["Tax"].ToString() == "" ? "0" : dsResult.Tables[1].Rows[0]["Tax"].ToString());
                    totaltax += decimal.Parse(dsResult.Tables[2].Rows[0]["Tax"].ToString() == "" ? "0" : dsResult.Tables[2].Rows[0]["Tax"].ToString());
                    totaltax += decimal.Parse(dsResult.Tables[3].Rows[0]["Tax"].ToString() == "" ? "0" : dsResult.Tables[3].Rows[0]["Tax"].ToString());

                    row["ServTax"] = "" + totaltax;

                    dsAWBDetails.Tables[0].Rows.Add(row);

                    GRDAWBDetails.DataSource = dsAWBDetails.Copy();
                    GRDAWBDetails.DataBind();


                    Session["dsAWBDetails"] = dsAWBDetails.Copy();

                }
                else
                {
                    LBLStatus.ForeColor = Color.Red;
                    LBLStatus.Text = "Details not found. Error :" + errormessage;
                    return;
                }



            }
            catch (Exception ex)
            {
                LBLStatus.ForeColor = Color.Red;
                LBLStatus.Text = "" + ex.Message;
            }
        }

        public void ClearAWBDetailsGrid()
        {
            try
            {
                DataSet dsAWBDetails = new DataSet();
                dsAWBDetails.Tables.Add();

                dsAWBDetails.Tables[0].Columns.Add("AWBNumber");
                dsAWBDetails.Tables[0].Columns.Add("CommCode");
                dsAWBDetails.Tables[0].Columns.Add("FrIATA");
                dsAWBDetails.Tables[0].Columns.Add("FrMKT");
                dsAWBDetails.Tables[0].Columns.Add("OcDueCar");
                dsAWBDetails.Tables[0].Columns.Add("OcDueAgent");
                dsAWBDetails.Tables[0].Columns.Add("SpotRate");
                dsAWBDetails.Tables[0].Columns.Add("DynRate");
                dsAWBDetails.Tables[0].Columns.Add("ServTax");

                DataRow row = dsAWBDetails.Tables[0].NewRow();
                dsAWBDetails.Tables[0].Rows.Add(row);


                GRDAWBDetails.DataSource = dsAWBDetails;
                GRDAWBDetails.DataBind();

                Session["dsAWBDetails"] = dsAWBDetails.Copy();

            }
            catch (Exception ex)
            {
                LBLStatus.ForeColor = Color.Red;
                LBLStatus.Text = "" + ex.Message;
                return;
            }

        }

        public bool IsInputValid()
        {
            try
            {

                bool blSelected = false;
                for (int i = 0; i < GRDAWBDetails.Rows.Count; i++)
                {
                    if (((CheckBox)GRDAWBDetails.Rows[i].FindControl("CHKSelect")).Checked)
                        blSelected = true;

                }
                if (!blSelected)
                {
                    LBLStatus.ForeColor = Color.Red;
                    LBLStatus.Text = "Select at least one awb commodity to assign spot rate.";
                    return false;
                }

                try
                {
                    decimal.Parse(TXTSpotRate.Text.Trim());

                }catch(Exception ex)
                {
                    LBLStatus.ForeColor = Color.Red;
                    LBLStatus.Text = "Spot rate is invalid.";
                    return false;
                }

                if (DDLCurrency.SelectedItem.Text.Trim() == "Select")
                {

                    LBLStatus.ForeColor = Color.Red;
                    LBLStatus.Text = "Currency is invalid.";
                    return false;

                }


                try
                {
                    DateTime.Parse(TXTReqDate.Text.Trim());
                }
                catch (Exception ex)
                {
                    LBLStatus.ForeColor = Color.Red;
                    LBLStatus.Text = "Req Date is invalid.";
                    return false;
                }

                DateTime dtfrom, dtto;

                try
                {
                    dtfrom=DateTime.Parse(TXTValidFrom.Text.Trim());
                }
                catch (Exception ex)
                {
                    LBLStatus.ForeColor = Color.Red;
                    LBLStatus.Text = "Valid from date is invalid.";
                    return false;
                }

                try
                {
                    dtto=DateTime.Parse(TXTValidTo.Text.Trim());
                }
                catch (Exception ex)
                {
                    LBLStatus.ForeColor = Color.Red;
                    LBLStatus.Text = "Valid to date is invalid.";
                    return false;
                }

                if (dtfrom > dtto)
                {
                    LBLStatus.ForeColor = Color.Red;
                    LBLStatus.Text = "Valid from date is greter than valid to date.";
                    return false;
                }


                try
                {
                    if (TXTAuthorizedDate.Text.Trim() != "")
                        DateTime.Parse(TXTAuthorizedDate.Text.Trim());

                }
                catch (Exception ex)
                {
                    LBLStatus.ForeColor = Color.Red;
                    LBLStatus.Text = "Authorized date is invalid.";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                LBLStatus.ForeColor = Color.Red;
                LBLStatus.Text = "" + ex.Message;
                return false;
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (!IsInputValid())
                    return;


                // "SpotRateCategory", "SpotRate", "Currency", "Station", "ReqDate", 
                // "FWDName", "Remark", "IssuedBy", "IssuedDate", "AuthorizedBy", 
                // "AutherizedDate", "ValidFrom", "ValidTo" 
                object[] values = {
                                   DDLSpotRateCategory.SelectedItem.Text,
                                   decimal.Parse(TXTSpotRate.Text),
                                   DDLCurrency.SelectedItem.Text,
                                   DDLStation.SelectedItem.Text,
                                   DateTime.Parse(TXTReqDate.Text).ToString("yyyy-MM-dd"),
                                   TXTFWDName.Text,
                                   TXTRemarks.Text,
                                   TXTIssuedBy.Text,
                                   DateTime.Parse(TXTIssuedDate.Text).ToString("yyyy-MM-dd"),
                                   TXTAuthorizedBy.Text,
                                   TXTAuthorizedDate.Text.Trim()=="" ? "": DateTime.Parse(TXTAuthorizedDate.Text).ToString("yyyy-MM-dd"),
                                   TXTValidFrom.Text.Trim()=="" ? "": DateTime.Parse(TXTValidFrom.Text).ToString("yyyy-MM-dd"),
                                   TXTValidTo.Text.Trim()=="" ? "": DateTime.Parse(TXTValidTo.Text).ToString("yyyy-MM-dd")
                                  };

                DataSet ds = new DataSet();
                ds.Tables.Add();
                ds.Tables[0].Columns.Add("AWBNumber");
                ds.Tables[0].Columns.Add("CommodityCode");
                ds.Tables[0].Columns.Add("SpotRate");

                for (int i = 0; i < GRDAWBDetails.Rows.Count; i++)
                {
                    DataRow row = ds.Tables[0].NewRow();
                    row["AWBNumber"] = ((Label)GRDAWBDetails.Rows[i].FindControl("LBLAWBNumber")).Text;
                    row["CommodityCode"] = ((Label)GRDAWBDetails.Rows[i].FindControl("LBLCommCode")).Text;
                    row["SpotRate"] = TXTSpotRate.Text;

                    ds.Tables[0].Rows.Add(row);
                }


                if (objBAL.SaveSpotRates(values, ds))
                {
                    LBLStatus.ForeColor = Color.Green;
                    LBLStatus.Text = "Spot rate saved successfully.";
                }

            }
            catch (Exception ex)
            {
                LBLStatus.ForeColor = Color.Red;
                LBLStatus.Text = "" + ex.Message;                
            }
        }

    }
}
