using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls.WebParts;
using QID.DataAccess;
using System.IO;
using System.Collections;
using BAL;
using System.Configuration;
using System.Drawing; 

namespace ProjectSmartCargoManager
{
    public partial class BillingWithRateAudit : System.Web.UI.Page
    {
        BillingBAL objBAL = new BillingBAL();
        BookingBAL objBook = new BookingBAL();
        MasterBAL objBal = new MasterBAL();
        Double ST = 0.0;
        DataSet ds;
        DataSet dsCommodity;
        static int rowind;
        static int commrowind;
        string AWBPrefix = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["awbPrefix"] != null)
                AWBPrefix = Session["awbPrefix"].ToString();
            else
            {
                Session["awbPrefix"] = objBal.awbPrefix();
                AWBPrefix = Session["awbPrefix"].ToString();
            }

            if (Session["ST"] != null)
                ST = Convert.ToDouble(Session["ST"].ToString());
            else
            {
                Session["ST"] = objBal.getServiceTax();
                ST = Convert.ToDouble(Session["ST"].ToString());
            }

            if (!IsPostBack)
            {
                Panel1.Visible = false;
                LoadAgentDropdown(); //AgentName 
                LoadAgentCodeDropdown(); //AgentCode 

                //Handler for discount
                string handlerDiscPer = ClientScript.GetPostBackEventReference(this.Button1, "");
                txtDiscount.Attributes.Add("onblur", handlerDiscPer);
                string handlerDiscAmt = ClientScript.GetPostBackEventReference(this.Button2, "");
                txtdiscamount.Attributes.Add("onblur", handlerDiscAmt);

                //Handler for commission
                string handlerCommPer = ClientScript.GetPostBackEventReference(this.Button3, "");
                txtcommission.Attributes.Add("onblur", handlerCommPer);
                string handlerCommAmt = ClientScript.GetPostBackEventReference(this.Button4, "");
                txtcommissionamt.Attributes.Add("onblur", handlerCommAmt);

                //Handler for Tds
                string handlerTdsPer = ClientScript.GetPostBackEventReference(this.Button5, "");
                txtTDSPer.Attributes.Add("onblur", handlerTdsPer);

                //Handler for GrossWt
                string handlerGrossWt = ClientScript.GetPostBackEventReference(this.Button6, "");
                txtgrosswt.Attributes.Add("onblur", handlerGrossWt);

                //Handler for ChargeWt
                string handlerChargeWt = ClientScript.GetPostBackEventReference(this.Button7, "");
                txtchargablewt.Attributes.Add("onblur", handlerChargeWt);

                //Handler for frtIATARate
                string handlerFrtIATARate = ClientScript.GetPostBackEventReference(this.Button8, "");
                txtiatarate.Attributes.Add("onblur", handlerFrtIATARate);

                //Handler for frtMKTRate
                string handlerFrtMKTRate = ClientScript.GetPostBackEventReference(this.Button11, "");
                txtmktrate.Attributes.Add("onblur", handlerFrtMKTRate);

                //Handler for ocda
                string handlerOcda = ClientScript.GetPostBackEventReference(this.Button9, "");
                txtocda.Attributes.Add("onblur", handlerOcda);

                //Handler for ocdc
                string handlerOcdc = ClientScript.GetPostBackEventReference(this.Button10, "");
                txtocdc.Attributes.Add("onblur", handlerOcdc); 
            }

        }

        #region Load Agent Dropdown
        public void LoadAgentDropdown()
        {
            DataSet ds = objBAL.GetAllAgents();
            if (ds != null)
            {
                ddlAgentName.DataSource = ds;
                ddlAgentName.DataMember = ds.Tables[0].TableName;
                ddlAgentName.DataTextField = "AgentName";
                ddlAgentName.DataValueField = "AgentCode";
                ddlAgentName.DataBind();
                ddlAgentName.SelectedIndex = -1;
            }
        }
        #endregion LoadAgentDropdown

        #region Load Agent Code Dropdown
        public void LoadAgentCodeDropdown()
        {
            DataSet ds = objBAL.GetAllAgents();
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
        #endregion LoadAgentCodeDropdown

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            bool validate = FieldValidation();
            if (validate)
            {
                bindGridView();
                pnlCommodityGrid.Visible = false;
                ClearPanel();
            }
        }

        protected void bindGridView()
        {
            

            #region Prepare Parameters
            object[] AwbRateInfo = new object[9];
            int i = 0;

            AwbRateInfo.SetValue(ddlAgentName.SelectedValue, i);
            i++;

            AwbRateInfo.SetValue(Convert.ToDateTime(txtbillingfrom.Text).ToString("yyyy-MM-dd HH:mm:ss"), i);
            i++;

            AwbRateInfo.SetValue(Convert.ToDateTime(txtbillingto.Text).ToString("yyyy-MM-dd HH:mm:ss"), i);
            i++;

            AwbRateInfo.SetValue(txtFlightNo.Text.Trim() , i);
            i++;

            AwbRateInfo.SetValue(txtOrigin.Text.Trim(), i);
            i++;

            AwbRateInfo.SetValue(txtDest.Text.Trim(), i);
            i++;

            AwbRateInfo.SetValue(ddlStatus.SelectedValue, i);
            i++;

            AwbRateInfo.SetValue(ddlSpotRate.SelectedValue, i);
            i++;

            AwbRateInfo.SetValue(ddlPayType.SelectedValue, i);

            #endregion Prepare Parameters
            

            ds = objBAL.GetAWBRateList(AwbRateInfo);


            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            try
                            {
                                //Code for calculations in dataset
                                //for (int indx = 0; indx < ds.Tables[0].Rows.Count; indx++)
                                //{
                                    //if (ds.Tables[0].Rows[indx]["FrtIATA"].ToString() != "")
                                    //{
                                    //    if (Convert.ToInt32(ds.Tables[0].Rows[indx]["FrtIATA"].ToString()) > 0)
                                    //    {
                                    //        //ds.Tables[0].Rows[indx]["TotalT"] = Convert.ToInt32(ds.Tables[0].Rows[indx]["FrtIATA"].ToString()) + Convert.ToInt32(ds.Tables[0].Rows[indx]["OCDueAgent"].ToString()) + Convert.ToInt32(ds.Tables[0].Rows[indx]["OCDueCar"].ToString());
                                    //        ds.Tables[0].Rows[indx]["TotalT"] = Convert.ToInt32(ds.Tables[0].Rows[indx]["FrtIATA"].ToString()) + Convert.ToInt32(ds.Tables[0].Rows[indx]["OCDueCar"].ToString());
                                    //    }
                                    //}
                                    //else if (ds.Tables[0].Rows[indx]["FrtMKT"].ToString() != "")
                                    //{
                                    //    //ds.Tables[0].Rows[indx]["TotalT"] = Convert.ToInt32(ds.Tables[0].Rows[indx]["FrtMKT"].ToString()) + Convert.ToInt32(ds.Tables[0].Rows[indx]["OCDueAgent"].ToString()) + Convert.ToInt32(ds.Tables[0].Rows[indx]["OCDueCar"].ToString());
                                    //    ds.Tables[0].Rows[indx]["TotalT"] = Convert.ToInt32(ds.Tables[0].Rows[indx]["FrtMKT"].ToString()) + Convert.ToInt32(ds.Tables[0].Rows[indx]["OCDueCar"].ToString());
                                    //}
                                    //else
                                    //{
                                    //    ds.Tables[0].Rows[indx]["TotalT"] = "0";
                                    //}


                                    //Total = FrtIATA + FrtMKT + OCDC
                                    //ds.Tables[0].Rows[indx]["TotalT"] = Convert.ToDouble(ds.Tables[0].Rows[indx]["FrtIATA"].ToString()) + Convert.ToDouble(ds.Tables[0].Rows[indx]["FrtMKT"].ToString()) + Convert.ToDouble(ds.Tables[0].Rows[indx]["OCDueCar"].ToString());

                                //}

                                //ds.AcceptChanges();



                                Session["dsDetails"] = ds;
                                grdBillingInfo.DataSource = ds.Tables[0];
                                grdBillingInfo.DataBind();
                                grdBillingInfo.Visible = true;

                                //code to disable awn number link
                                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                {
                                    if (ds.Tables[0].Rows[j]["Confirmed"].ToString() == "Finalized")
                                    {
                                        //grdBillingInfo.Rows[j].Cells[2].Enabled = false;
                                    }
                                }


                                //visible all the buttons
                                ChkSelectAll.Visible = true;
                                btnVerify.Visible = true;
                                btnApproved.Visible = true;
                                btnConfirmInvoice.Visible = true;
                                btnGenerateInvoice.Visible = true;
                                btnGenerateProforma.Visible = true;
                                lblStatus.Text = "";
                                ChkSelectAll.Checked = false;
                                pnlCommodityGrid.Visible = false;
                                Panel1.Visible = false;
                                ClearPanel();
                            }
                            catch (Exception ex)
                            {
                                
                            }
                        }
                        else
                        {
                            //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No records found.');</SCRIPT>");
                            lblStatus.Text = "No records found";
                            lblStatus.ForeColor = Color.Blue;
                            return;
                        }

                    }
                }
            }

        }

        protected void FilldsOnLoad(int ind)
        {
            double commPer = (100 / Convert.ToDouble(txttotalafterdiscount.Text.Trim())) * Convert.ToDouble(txtcommissionamt.Text.Trim());
            txtcommission.Text = Math.Round(commPer, 2).ToString();
            //save calculation of Discount in dataset
            ds.Tables[0].Rows[rowind]["Commission"] = txtcommissionamt.Text.Trim();
            txtRevisedTotal.Text = Math.Round(Convert.ToDouble(txttotalafterdiscount.Text.Trim()) - Convert.ToDouble(txtcommissionamt.Text.Trim()), 2).ToString();
            grdBillingInfo.Rows[rowind].Cells[11].Text = txtRevisedTotal.Text.Trim();

            //Calculation for TDS amount and Total after tax
            double tdsAmt = (Convert.ToDouble(txtTDSPer.Text.Trim()) / 100) * Convert.ToDouble(txtRevisedTotal.Text.Trim());
            txtTDSAmt.Text = Math.Round(tdsAmt, 2).ToString();
            //save calculation of TDS amount and Total after tax in dataset and gridview
            txttotalaftertax.Text = Math.Round(Convert.ToDouble(txtRevisedTotal.Text.Trim()) + Convert.ToDouble(txtTDSAmt.Text.Trim()), 2).ToString();
            grdBillingInfo.Rows[rowind].Cells[12].Text = txtTDSAmt.Text.Trim();
            grdBillingInfo.Rows[rowind].Cells[13].Text = txttotalaftertax.Text.Trim();

            ds.Tables[0].Rows[rowind]["RevisedTotal"] = txtRevisedTotal.Text.Trim();
            ds.Tables[0].Rows[rowind]["TDS"] = txtTDSAmt.Text.Trim();
            ds.Tables[0].Rows[rowind]["Final"] = txttotalaftertax.Text.Trim();




            //as columns loses its values again assign
            grdBillingInfo.Rows[rowind].Cells[9].Text = txtdiscamount.Text.Trim();
            grdBillingInfo.Rows[rowind].Cells[10].Text = txttotalafterdiscount.Text.Trim();


            ds.AcceptChanges();
            Session["dsDetails"] = ds;

        }

        protected void grdBillingInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Code to check if Multiple commodities exist for an AWB.
            //If exists, Show commodity grid and hide Panel1.
            //If single commodity exists, hide grid and show Panel1.
            if (e.CommandName == "AWBClick")
            {
                FieldValidation();
                rowind = Convert.ToInt32(e.CommandArgument) + (grdBillingInfo.PageIndex * grdBillingInfo.PageSize);
                bindCommGridOrPanel();
            }

        }

        protected void bindCommGridOrPanel()
        {
            DataSet dsdetails = (DataSet)Session["dsDetails"];
            #region Prepare Parameters
            object[] AwbRateInfo = new object[4];
            int i = 0;

            AwbRateInfo.SetValue(dsdetails.Tables[0].Rows[rowind]["AWBNumber"], i);
            i++;

            AwbRateInfo.SetValue(ddlAgentName.SelectedValue, i);
            i++;

            AwbRateInfo.SetValue(Convert.ToDateTime(txtbillingfrom.Text).ToString("yyyy-MM-dd HH:mm:ss"), i);
            i++;

            AwbRateInfo.SetValue(Convert.ToDateTime(txtbillingto.Text).ToString("yyyy-MM-dd HH:mm:ss"), i);
            #endregion Prepare Parameters


            dsCommodity = objBAL.GetAWBCommodityRateList(AwbRateInfo);


            if (dsCommodity != null)
            {
                if (dsCommodity.Tables != null)
                {
                    if (dsCommodity.Tables.Count > 0)
                    {
                        //Code to show only more than one commodities on click of AWB
                        //if (dsCommodity.Tables[0].Rows.Count > 1)

                        //Showing single as well as multiple commodities on click of AWB
                        if (dsCommodity.Tables[0].Rows.Count > 0)
                        {
                            try
                            {
                                //code to Replace Mkt rate with Spot rate is Spot Rate is available
                                //for (int j = 0; j < dsCommodity.Tables[0].Rows.Count; j++)
                                //{
                                //    if (dsCommodity.Tables[0].Rows[j]["isSpotRate"].ToString() == "Yes")
                                //    {
                                //        //dsCommodity.Tables[0].Rows[j]["FrtMKT"] = dsCommodity.Tables[0].Rows[j]["SpotRate"].ToString();
                                //    }
                                //}

                                //dsCommodity.AcceptChanges();

                                Session["dsCommodity"] = dsCommodity;

                                grdCommodity.DataSource = dsCommodity.Tables[0];
                                grdCommodity.DataBind();
                                

                                


                                //visible all the buttons
                                //ChkSelectAll.Visible = true;
                                //btnVerify.Visible = true;
                                btnApproved.Visible = true;
                                btnConfirmInvoice.Visible = true;
                                //btnGenerateInvoice.Visible = true;
                                //btnGenerateProforma.Visible = true;
                                lblStatus.Text = "";
                                pnlCommodityGrid.Visible = true;
                                grdCommodity.Visible = true;
                                Panel1.Visible = false;
                                ChkSelectAll.Checked = false;
                                ClearPanel();
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            Panel1.Visible = true;
                            pnlCommodityGrid.Visible = false;
                            fillBillingDetails(rowind);

                        }

                    }
                }
            }
        }

        protected bool FieldValidation()
        {
            if (txtbillingfrom.Text == "")
            {
                lblStatus.Text = "Please select Valid date";
                lblStatus.ForeColor = Color.Blue;
                return false;
            }
           
            DateTime dt;

            try
            {
                dt = Convert.ToDateTime(txtbillingfrom.Text);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Selected Date format invalid";
                lblStatus.ForeColor = Color.Red;
                return false;
            }


            //Validation for To date
            if (txtbillingto.Text == "")
            {
                lblStatus.Text = "Please select Valid date";
                lblStatus.ForeColor = Color.Blue;
                return false;
            }
            


            DateTime dtto;

            try
            {
                dtto = Convert.ToDateTime(txtbillingto.Text);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Selected Date format invalid";
                lblStatus.ForeColor = Color.Red;
                return false;
            }

            if (dtto < dt)
            {
                lblStatus.Text = "To date should be greater than From date";
                lblStatus.ForeColor = Color.Red;

                return false;
            }
            else
            {
                return true;
            } 
        }

        protected void fillBillingDetails(int ind)
        {
            DataSet dsdetails = (DataSet)Session["dsDetails"];
            txtAWBNo.Text = dsdetails.Tables[0].Rows[ind]["AWBNumber"].ToString();
            txtgrosswt.Text = dsdetails.Tables[0].Rows[ind]["GrossWeight"].ToString();
            txtchargablewt.Text = dsdetails.Tables[0].Rows[ind]["ChargedWeight"].ToString();
            if (dsdetails.Tables[0].Rows[ind]["FrtIATA"].ToString() != "")
            {
                if (Convert.ToDouble(dsdetails.Tables[0].Rows[ind]["FrtIATA"].ToString()) >= 0)
                {
                    txtiatarate.Text = dsdetails.Tables[0].Rows[ind]["FrtIATA"].ToString();
                }
            }
            if (dsdetails.Tables[0].Rows[ind]["FrtMKT"].ToString() != "")
            {
                if (Convert.ToDouble(dsdetails.Tables[0].Rows[ind]["FrtMKT"].ToString()) >= 0)
                {
                    txtmktrate.Text = dsdetails.Tables[0].Rows[ind]["FrtMKT"].ToString();
                }
            }
            

            txtocda.Text = dsdetails.Tables[0].Rows[ind]["OCDueAgent"].ToString();
            txtocdc.Text = dsdetails.Tables[0].Rows[ind]["OCDueCar"].ToString();

            txtcommodity.Text = dsdetails.Tables[0].Rows[ind]["CommodityCode"].ToString();
            txtdimensions.Text = dsdetails.Tables[0].Rows[ind]["Dimensions"].ToString();

            txtTotal.Text = dsdetails.Tables[0].Rows[ind]["TotalT"].ToString();
            txtDiscount.Text = "0";
            txtdiscamount.Text = "0";
            txttotalafterdiscount.Text = "";
            txtcommission.Text = dsdetails.Tables[0].Rows[ind]["Commission"].ToString();
            //txtcommissionamt.Text = "0";
            //txtTDSPer.Text = "12.5";
            txtTDSPer.Text = dsdetails.Tables[0].Rows[ind]["TDS"].ToString();
            txtRevisedTotal.Text = "";
            txtTDSAmt.Text = "";
            txttotalaftertax.Text = "";

            discountPerTextExit();
            discountAmtTextExit();
            commissionPerTextExit();
            commissionAmtTextExit();
            //Session["dsDetails"] = "";

        }

        protected void grdBillingInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dst = (DataSet)Session["dsDetails"];
            grdBillingInfo.PageIndex = e.NewPageIndex;
            grdBillingInfo.DataSource =dst.Tables[0];
            grdBillingInfo.DataBind();
        }

        protected void grdBillingInfo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void txtDiscount_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtdiscamount_TextChanged(object sender, EventArgs e)
        {

        }

        //function call when Gross weight textbox exit
        protected void Button6_Click(object sender, EventArgs e)
        {
            grossWtTextExit();
        }

        protected void grossWtTextExit()
        {
            try
            {
                if (grdCommodity.Visible == false)
                {
                    if (Convert.ToDouble(txtgrosswt.Text.Trim()) < 0)
                    {
                        lblStatus.Text = "Enter valid Gross Weight";
                        lblStatus.ForeColor = Color.Red;
                        return;

                    }
                    else
                    {
                        //code to do calc if other calc depends on Gross Weight
                        DataSet dsdetails = (DataSet)Session["dsDetails"];
                        dsdetails.Tables[0].Rows[rowind]["GrossWeight"] = txtgrosswt.Text.Trim();
                    }
                }
                else
                {
                    if (Convert.ToDouble(txtgrosswt.Text.Trim()) < 0)
                    {
                        lblStatus.Text = "Enter valid Gross Weight";
                        lblStatus.ForeColor = Color.Red;
                        return;

                    }
                    else
                    {
                        //code to do calc if other calc depends on Gross Weight
                        DataSet dsCommodity = (DataSet)Session["dsCommodity"];
                        dsCommodity.Tables[0].Rows[commrowind]["GrossWeight"] = txtgrosswt.Text.Trim();
                    }
                }

            }
            catch (Exception)
            {
                lblStatus.Text = "Enter valid Gross Weight";
                lblStatus.ForeColor = Color.Red;
                return;
            }
            
        }

        protected void Button7_Click(object sender, EventArgs e)
        {
            chargeWtTextExit();
        }

        protected void chargeWtTextExit()
        {
            
            try
            {
                if (grdCommodity.Visible == false)
                {
                    if (Convert.ToDouble(txtchargablewt.Text.Trim()) < 0)
                    {
                        lblStatus.Text = "Enter valid Chargable Weight";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    else
                    {
                        //code to do calc if other calc depends on Charged Weight
                        DataSet dsdetails = (DataSet)Session["dsDetails"];
                        
                        dsdetails.Tables[0].Rows[rowind]["ChargedWeight"] = txtchargablewt.Text.Trim();
                        grdBillingInfo.Rows[rowind].Cells[3].Text = txtchargablewt.Text.Trim();

                        DataSet dsRates = objBook.ProcessRates(dsdetails.Tables[0].Rows[rowind]["AWBNumber"].ToString(), "", 0, AWBPrefix);
                        txtiatarate.Text = dsRates.Tables[0].Rows[0]["FrIATA"].ToString();
                        txtmktrate.Text = dsRates.Tables[0].Rows[0]["FrMKT"].ToString();
                        txtocda.Text = dsRates.Tables[0].Rows[0]["OCDA"].ToString();
                        txtocdc.Text = dsRates.Tables[0].Rows[0]["OCDC"].ToString();
                        frtMKTRateTextExit();
                    }
                }
                else
                {
                    if (Convert.ToDouble(txtchargablewt.Text.Trim()) < 0)
                    {
                        lblStatus.Text = "Enter valid Chargable Weight";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    else
                    {
                        //code to do calc if other calc depends on Charged Weight
                        DataSet dsCommodity = (DataSet)Session["dsCommodity"];
                        dsCommodity.Tables[0].Rows[commrowind]["ChargedWeight"] = txtchargablewt.Text.Trim();
                        grdCommodity.Rows[commrowind].Cells[4].Text = txtchargablewt.Text.Trim();

                        DataSet dsRates = objBook.ProcessRates(dsCommodity.Tables[0].Rows[commrowind]["AWBNumber"].ToString(), txtcommodity.Text.Trim(), Convert.ToDecimal(txtchargablewt.Text.Trim()), AWBPrefix);
                        txtiatarate.Text = dsRates.Tables[0].Rows[0]["FrIATA"].ToString();
                        txtmktrate.Text = dsRates.Tables[0].Rows[0]["FrMKT"].ToString();
                        txtocda.Text = dsRates.Tables[0].Rows[0]["OCDA"].ToString();
                        txtocdc.Text = dsRates.Tables[0].Rows[0]["OCDC"].ToString();
                        frtMKTRateTextExit();
                    }
                }
            }
            catch (Exception)
            {
                lblStatus.Text = "Enter valid Chargable Weight";
                lblStatus.ForeColor = Color.Red;
                return;
            }
        }

        protected void Button8_Click(object sender, EventArgs e)
        {
            frtIATARateTextExit();
        }

        protected void frtIATARateTextExit()
        {
            try
            {
                if (grdCommodity.Visible == false)
                {
                    if (Convert.ToDouble(txtiatarate.Text.Trim()) < 0)
                    {
                        lblStatus.Text = "Enter valid IATA Rate";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    else
                    {
                        //code to do calc if other calc depends on Charged Weight
                        DataSet dsdetails = (DataSet)Session["dsDetails"];
                        //saving frtRate in frtIATA (not in frtMKT). so change IATA value
                        dsdetails.Tables[0].Rows[rowind]["frtIATA"] = txtiatarate.Text.Trim();
                        //txtTotal.Text = Math.Round(Convert.ToDouble(txtiatarate.Text.Trim()) + Convert.ToDouble(txtmktrate.Text.Trim()) + Convert.ToDouble(txtocdc.Text.Trim()), 2).ToString();
                        txtTotal.Text = Math.Round(Convert.ToDouble(txtmktrate.Text.Trim()) + Convert.ToDouble(txtocdc.Text.Trim()), 2).ToString();
                        dsdetails.Tables[0].Rows[rowind]["TotalT"] = txtTotal.Text.Trim();
                        dsdetails.AcceptChanges();
                        Session["dsDetails"] = dsdetails;
                        OCDATextExit();
                        OCDCTextExit();
                        discountPerTextExit();
                        //discountAmtTextExit();
                        commissionPerTextExit();
                        //commissionAmtTextExit();
                    }
                }
                else
                {
                    if (Convert.ToDouble(txtiatarate.Text.Trim()) < 0)
                    {
                        lblStatus.Text = "Enter valid IATA Rate";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    else
                    {
                        //code to do calc if other calc depends on Charged Weight
                        DataSet dsCommodity = (DataSet)Session["dsCommodity"];
                        //saving frtRate in frtIATA (not in frtMKT). so change IATA value
                        dsCommodity.Tables[0].Rows[commrowind]["frtIATA"] = txtiatarate.Text.Trim();
                        //txtTotal.Text = Math.Round(Convert.ToDouble(txtiatarate.Text.Trim()) + Convert.ToDouble(txtmktrate.Text.Trim()) + Convert.ToDouble(txtocdc.Text.Trim()), 2).ToString();
                        txtTotal.Text = Math.Round(Convert.ToDouble(txtmktrate.Text.Trim()) + Convert.ToDouble(txtocdc.Text.Trim()), 2).ToString();
                        dsCommodity.Tables[0].Rows[commrowind]["TotalT"] = txtTotal.Text.Trim();
                        dsCommodity.AcceptChanges();
                        Session["dsCommodity"] = dsCommodity;
                        OCDATextExit();
                        OCDCTextExit();
                        discountPerTextExit();
                        //discountAmtTextExit();
                        commissionPerTextExit();
                        //commissionAmtTextExit();
                    }
                }
            }
            catch (Exception)
            {
                lblStatus.Text = "Enter valid IATA Rate";
                lblStatus.ForeColor = Color.Red;
                return;
            }
        }

        protected void Button11_Click(object sender, EventArgs e)
        {
            frtMKTRateTextExit();
        }

        protected void frtMKTRateTextExit()
        {
            try
            {
                if (grdCommodity.Visible == false)
                {
                    if (Convert.ToDouble(txtmktrate.Text.Trim()) < 0)
                    {
                        lblStatus.Text = "Enter valid MKT Rate";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    else
                    {
                        //code to do calc if other calc depends on Charged Weight
                        DataSet dsdetails = (DataSet)Session["dsDetails"];
                        //saving frtRate in frtIATA (not in frtMKT). so change IATA value
                        dsdetails.Tables[0].Rows[rowind]["frtMKT"] = txtmktrate.Text.Trim();
                        //txtTotal.Text = Math.Round(Convert.ToDouble(txtiatarate.Text.Trim()) + Convert.ToDouble(txtmktrate.Text.Trim()) + Convert.ToDouble(txtocdc.Text.Trim()), 2).ToString();
                        txtTotal.Text = Math.Round(Convert.ToDouble(txtmktrate.Text.Trim()) + Convert.ToDouble(txtocdc.Text.Trim()), 2).ToString();
                        dsdetails.Tables[0].Rows[rowind]["TotalT"] = txtTotal.Text.Trim();
                        dsdetails.AcceptChanges();
                        Session["dsDetails"] = dsdetails;
                        OCDATextExit();
                        OCDCTextExit();
                        discountPerTextExit();
                        //discountAmtTextExit();
                        commissionPerTextExit();
                        //commissionAmtTextExit();
                    }
                }
                else
                {
                    if (Convert.ToDouble(txtmktrate.Text.Trim()) < 0)
                    {
                        lblStatus.Text = "Enter valid MKT Rate";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    else
                    {
                        //code to do calc if other calc depends on Charged Weight
                        DataSet dsCommodity = (DataSet)Session["dsCommodity"];
                        //saving frtRate in frtIATA (not in frtMKT). so change IATA value
                        dsCommodity.Tables[0].Rows[commrowind]["frtMKT"] = txtmktrate.Text.Trim();
                        //txtTotal.Text = Math.Round(Convert.ToDouble(txtiatarate.Text.Trim()) + Convert.ToDouble(txtmktrate.Text.Trim()) + Convert.ToDouble(txtocdc.Text.Trim()), 2).ToString();
                        txtTotal.Text = Math.Round(Convert.ToDouble(txtmktrate.Text.Trim()) + Convert.ToDouble(txtocdc.Text.Trim()), 2).ToString();
                        dsCommodity.Tables[0].Rows[commrowind]["TotalT"] = txtTotal.Text.Trim();
                        dsCommodity.AcceptChanges();
                        Session["dsCommodity"] = dsCommodity;
                        OCDATextExit();
                        OCDCTextExit();
                        discountPerTextExit();
                        //discountAmtTextExit();
                        commissionPerTextExit();
                        //commissionAmtTextExit();
                    }
                }
            }
            catch (Exception)
            {
                lblStatus.Text = "Enter valid MKT Rate";
                lblStatus.ForeColor = Color.Red;
                return;
            }
        }

        //protected void frtRateTextExit()
        //{
        //    try
        //    {
        //        if (Convert.ToDouble(txtrate.Text.Trim()) < 0)
        //        {
        //            lblStatus.Text = "Enter valid Rate(IATA/MKT)";
        //            lblStatus.ForeColor = Color.Red;
        //            return;
        //        }
        //        else
        //        {
        //            //code to do calc if other calc depends on Charged Weight
        //            DataSet dsdetails = (DataSet)Session["dsDetails"];
        //            //saving frtRate in frtIATA (not in frtMKT). so change IATA value
        //            dsdetails.Tables[0].Rows[rowind]["frtIATA"] = txtrate.Text.Trim();
        //            txtTotal.Text = Math.Round(Convert.ToDouble(txtrate.Text.Trim()) + Convert.ToDouble(txtocdc.Text.Trim()), 2).ToString();
        //            dsdetails.Tables[0].Rows[rowind]["TotalT"] = txtTotal.Text.Trim();
        //            dsdetails.AcceptChanges();
        //            Session["dsDetails"] = dsdetails;
        //            OCDATextExit();
        //            OCDCTextExit();
        //            discountPerTextExit();
        //            //discountAmtTextExit();
        //            commissionPerTextExit();
        //            //commissionAmtTextExit();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        lblStatus.Text = "Enter valid Rate(IATA/MKT)";
        //        lblStatus.ForeColor = Color.Red;
        //        return;
        //    }
        //}

        protected void Button9_Click(object sender, EventArgs e)
        {
            OCDATextExit();
        }

        protected void OCDATextExit()
        {
            try
            {
                if (grdCommodity.Visible == false)
                {
                    if (Convert.ToDouble(txtocda.Text.Trim()) < 0)
                    {
                        lblStatus.Text = "Enter valid OCDA";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    else
                    {
                        //code to do calc if other calc depends on Charged Weight
                        DataSet dsdetails = (DataSet)Session["dsDetails"];

                        dsdetails.Tables[0].Rows[rowind]["OCDueAgent"] = txtocda.Text.Trim();
                        grdBillingInfo.Rows[rowind].Cells[4].Text = txtiatarate.Text.Trim();
                        grdBillingInfo.Rows[rowind].Cells[5].Text = txtmktrate.Text.Trim();
                        grdBillingInfo.Rows[rowind].Cells[6].Text = txtocda.Text.Trim();
                        grdBillingInfo.Rows[rowind].Cells[7].Text = txtocdc.Text.Trim();


                        dsdetails.AcceptChanges();
                        Session["dsDetails"] = dsdetails;
                        discountPerTextExit();
                        //discountAmtTextExit();
                        commissionPerTextExit();
                        //commissionAmtTextExit();
                    }
                }
                else
                {
                    if (Convert.ToDouble(txtocda.Text.Trim()) < 0)
                    {
                        lblStatus.Text = "Enter valid OCDA";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    else
                    {
                        //code to do calc if other calc depends on Charged Weight
                        DataSet dsCommodity = (DataSet)Session["dsCommodity"];

                        dsCommodity.Tables[0].Rows[commrowind]["OCDueAgent"] = txtocda.Text.Trim();
                        grdCommodity.Rows[commrowind].Cells[5].Text = txtiatarate.Text.Trim();
                        grdCommodity.Rows[commrowind].Cells[6].Text = txtmktrate.Text.Trim();
                        grdCommodity.Rows[commrowind].Cells[7].Text = txtocda.Text.Trim();
                        grdCommodity.Rows[commrowind].Cells[8].Text = txtocdc.Text.Trim();


                        dsCommodity.AcceptChanges();
                        Session["dsCommodity"] = dsCommodity;
                        discountPerTextExit();
                        //discountAmtTextExit();
                        commissionPerTextExit();
                        //commissionAmtTextExit();
                    }
                }
            }
            catch (Exception)
            {
                lblStatus.Text = "Enter valid OCDA";
                lblStatus.ForeColor = Color.Red;
                return;
            }
        }

        protected void Button10_Click(object sender, EventArgs e)
        {
            OCDCTextExit();
        }

        protected void OCDCTextExit()
        {
            try
            {
                if (grdCommodity.Visible == false)
                {
                    if (Convert.ToDouble(txtocdc.Text.Trim()) < 0)
                    {
                        lblStatus.Text = "Enter valid OCDC";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    else
                    {
                        //code to do calc if other calc depends on OCDC
                        DataSet dsdetails = (DataSet)Session["dsDetails"];

                        dsdetails.Tables[0].Rows[rowind]["OCDueCar"] = txtocdc.Text.Trim();
                        grdBillingInfo.Rows[rowind].Cells[4].Text = txtiatarate.Text.Trim();
                        grdBillingInfo.Rows[rowind].Cells[5].Text = txtmktrate.Text.Trim();
                        grdBillingInfo.Rows[rowind].Cells[6].Text = txtocda.Text.Trim();
                        grdBillingInfo.Rows[rowind].Cells[7].Text = txtocdc.Text.Trim();

                        //txtTotal.Text = Math.Round(Convert.ToDouble(txtiatarate.Text.Trim()) + Convert.ToDouble(txtmktrate.Text.Trim()) + Convert.ToDouble(txtocdc.Text.Trim()), 2).ToString();
                        txtTotal.Text = Math.Round(Convert.ToDouble(txtmktrate.Text.Trim()) + Convert.ToDouble(txtocdc.Text.Trim()), 2).ToString();
                        grdBillingInfo.Rows[rowind].Cells[8].Text = txtTotal.Text.Trim();
                        dsdetails.Tables[0].Rows[rowind]["TotalT"] = txtTotal.Text.Trim();

                        dsdetails.AcceptChanges();
                        Session["dsDetails"] = dsdetails;
                        discountPerTextExit();
                        //discountAmtTextExit();
                        commissionPerTextExit();
                        //commissionAmtTextExit();
                    }
                }
                else
                {
                    if (Convert.ToDouble(txtocdc.Text.Trim()) < 0)
                    {
                        lblStatus.Text = "Enter valid OCDC";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    else
                    {
                        //code to do calc if other calc depends on OCDC
                        DataSet dsCommodity = (DataSet)Session["dsCommodity"];

                        dsCommodity.Tables[0].Rows[commrowind]["OCDueCar"] = txtocdc.Text.Trim();
                        grdCommodity.Rows[commrowind].Cells[5].Text = txtiatarate.Text.Trim();
                        grdCommodity.Rows[commrowind].Cells[6].Text = txtmktrate.Text.Trim();
                        grdCommodity.Rows[commrowind].Cells[7].Text = txtocda.Text.Trim();
                        grdCommodity.Rows[commrowind].Cells[8].Text = txtocdc.Text.Trim();

                        //txtTotal.Text = Math.Round(Convert.ToDouble(txtiatarate.Text.Trim()) + Convert.ToDouble(txtmktrate.Text.Trim()) + Convert.ToDouble(txtocdc.Text.Trim()), 2).ToString();
                        txtTotal.Text = Math.Round(Convert.ToDouble(txtmktrate.Text.Trim()) + Convert.ToDouble(txtocdc.Text.Trim()), 2).ToString();
                        grdCommodity.Rows[commrowind].Cells[9].Text = txtTotal.Text.Trim();
                        dsCommodity.Tables[0].Rows[commrowind]["TotalT"] = txtTotal.Text.Trim();

                        dsCommodity.AcceptChanges();
                        Session["dsCommodity"] = dsCommodity;
                        discountPerTextExit();
                        //discountAmtTextExit();
                        commissionPerTextExit();
                        //commissionAmtTextExit();
                    }
                }
            }
            catch (Exception)
            {
                lblStatus.Text = "Enter valid OCDC";
                lblStatus.ForeColor = Color.Red;
                return;
            }
        }
        

        protected void Button1_Click(object sender, EventArgs e)
        {
            discountPerTextExit();
    
        }

        protected void discountPerTextExit()
        {
            if (grdCommodity.Visible == false)
            {
                if (txtDiscount.Text.Trim() != "")
                {
                    DataSet dsdetails = (DataSet)Session["dsDetails"];
                    if (Convert.ToDouble(txtDiscount.Text.Trim()) > 100)
                    {
                        lblStatus.Text = "Invalid discount percent";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }

                    double discAmt = (Convert.ToDouble(txtDiscount.Text.Trim()) / 100) * Convert.ToDouble(txtTotal.Text.Trim());
                    txtdiscamount.Text = Math.Round(discAmt, 2).ToString();
                    //save calculation of Discount in both dataset and gridview
                    dsdetails.Tables[0].Rows[rowind]["Discount"] = txtdiscamount.Text.Trim();
                    grdBillingInfo.Rows[rowind].Cells[9].Text = txtdiscamount.Text.Trim();
                    txttotalafterdiscount.Text = Math.Round(Convert.ToDouble(txtTotal.Text.Trim()) - Convert.ToDouble(txtdiscamount.Text.Trim()), 2).ToString();
                    //save calculation of TAD in both dataset and gridview
                    dsdetails.Tables[0].Rows[rowind]["TAD"] = txttotalafterdiscount.Text.Trim();
                    grdBillingInfo.Rows[rowind].Cells[10].Text = txttotalafterdiscount.Text.Trim();
                    dsdetails.AcceptChanges();
                    Session["dsDetails"] = dsdetails;

                    commissionPerTextExit();
                    commissionAmtTextExit();
                }
            }
            else
            {
                if (txtDiscount.Text.Trim() != "")
                {
                    DataSet dsCommodity = (DataSet)Session["dsCommodity"]; 
                    if (Convert.ToDouble(txtDiscount.Text.Trim()) > 100)
                    {
                        lblStatus.Text = "Invalid discount percent";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }

                    double discAmt = (Convert.ToDouble(txtDiscount.Text.Trim()) / 100) * Convert.ToDouble(txtTotal.Text.Trim());
                    txtdiscamount.Text = Math.Round(discAmt, 2).ToString();
                    //save calculation of Discount in both dataset and gridview
                    dsCommodity.Tables[0].Rows[commrowind]["Discount"] = txtdiscamount.Text.Trim();
                    grdCommodity.Rows[commrowind].Cells[10].Text = txtdiscamount.Text.Trim();
                    txttotalafterdiscount.Text = Math.Round(Convert.ToDouble(txtTotal.Text.Trim()) - Convert.ToDouble(txtdiscamount.Text.Trim()), 2).ToString();
                    //save calculation of TAD in both dataset and gridview
                    dsCommodity.Tables[0].Rows[commrowind]["TAD"] = txttotalafterdiscount.Text.Trim();
                    grdCommodity.Rows[commrowind].Cells[11].Text = txttotalafterdiscount.Text.Trim();
                    dsCommodity.AcceptChanges();
                    Session["dsCommodity"] = dsCommodity;

                    commissionPerTextExit();
                    commissionAmtTextExit();
                }
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            discountAmtTextExit(); 
        }

        protected void discountAmtTextExit()
        {
            if (grdCommodity.Visible == false)
            {
                if (txtdiscamount.Text.Trim() != "")
                {
                    DataSet dsdetails = (DataSet)Session["dsDetails"];
                    if (Convert.ToDouble(txtdiscamount.Text.Trim()) > Convert.ToDouble(txtTotal.Text.Trim()))
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Invalid Discount Amount');</SCRIPT>");
                        lblStatus.Text = "Invalid discount amount";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }

                    double discPer = (100 / Convert.ToDouble(txtTotal.Text.Trim())) * Convert.ToDouble(txtdiscamount.Text.Trim());
                    txtDiscount.Text = Math.Round(discPer, 2).ToString();
                    //save calculation of Discount in both dataset and gridview
                    dsdetails.Tables[0].Rows[rowind]["Discount"] = txtdiscamount.Text.Trim();
                    grdBillingInfo.Rows[rowind].Cells[9].Text = txtdiscamount.Text.Trim();
                    txttotalafterdiscount.Text = Math.Round(Convert.ToDouble(txtTotal.Text.Trim()) - Convert.ToDouble(txtdiscamount.Text.Trim()), 2).ToString();
                    //save calculation of TAD in both dataset and gridview
                    dsdetails.Tables[0].Rows[rowind]["TAD"] = txttotalafterdiscount.Text.Trim();
                    grdBillingInfo.Rows[rowind].Cells[10].Text = txttotalafterdiscount.Text.Trim();
                    dsdetails.AcceptChanges();
                    Session["dsDetails"] = dsdetails;
                    commissionAmtTextExit();
                    commissionPerTextExit();

                }
                
            }
            else
            {
                if (txtdiscamount.Text.Trim() != "")
                {
                    DataSet dsCommodity = (DataSet)Session["dsCommodity"]; ;
                    if (Convert.ToDouble(txtdiscamount.Text.Trim()) > Convert.ToDouble(txtTotal.Text.Trim()))
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Invalid Discount Amount');</SCRIPT>");
                        lblStatus.Text = "Invalid discount amount";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }

                    double discPer = (100 / Convert.ToDouble(txtTotal.Text.Trim())) * Convert.ToDouble(txtdiscamount.Text.Trim());
                    txtDiscount.Text = Math.Round(discPer, 2).ToString();
                    //save calculation of Discount in both dataset and gridview
                    dsCommodity.Tables[0].Rows[commrowind]["Discount"] = txtdiscamount.Text.Trim();
                    grdCommodity.Rows[commrowind].Cells[10].Text = txtdiscamount.Text.Trim();
                    txttotalafterdiscount.Text = Math.Round(Convert.ToDouble(txtTotal.Text.Trim()) - Convert.ToDouble(txtdiscamount.Text.Trim()), 2).ToString();
                    //save calculation of TAD in both dataset and gridview
                    dsCommodity.Tables[0].Rows[commrowind]["TAD"] = txttotalafterdiscount.Text.Trim();
                    grdCommodity.Rows[commrowind].Cells[11].Text = txttotalafterdiscount.Text.Trim();
                    dsCommodity.AcceptChanges();
                    Session["dsCommodity"] = dsCommodity;
                    commissionAmtTextExit();
                    commissionPerTextExit();

                }
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            commissionPerTextExit();
            
        }

        protected void commissionPerTextExit()
        {
            if (grdCommodity.Visible == false)
            {
                if (txtcommission.Text.Trim() != "")
                {
                    DataSet dsdetails = (DataSet)Session["dsDetails"];
                    if (Convert.ToDouble(txtcommission.Text.Trim()) > 100)
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Invalid Commission Percent');</SCRIPT>");
                        lblStatus.Text = "Invalid commission percent";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }

                    double commAmt = (Convert.ToDouble(txtcommission.Text.Trim()) / 100) * Convert.ToDouble(txttotalafterdiscount.Text.Trim());
                    txtcommissionamt.Text = Math.Round(commAmt, 2).ToString();
                    //save calculation of Discount in dataset
                    dsdetails.Tables[0].Rows[rowind]["CommissionAmt"] = txtcommissionamt.Text.Trim();
                    txtRevisedTotal.Text = Math.Round(Convert.ToDouble(txttotalafterdiscount.Text.Trim()) - Convert.ToDouble(txtcommissionamt.Text.Trim()), 2).ToString();
                    grdBillingInfo.Rows[rowind].Cells[11].Text = txtRevisedTotal.Text.Trim();

                    //Calculation for TDS amount and Total after tax
                    double tdsAmt = (Convert.ToDouble(txtTDSPer.Text.Trim()) / 100) * Convert.ToDouble(txtRevisedTotal.Text.Trim());
                    txtTDSAmt.Text = Math.Round(tdsAmt, 2).ToString();
                    //save calculation of TDS amount and Total after tax in dataset and gridview
                    txttotalaftertax.Text = Math.Round(Convert.ToDouble(txtRevisedTotal.Text.Trim()) + Convert.ToDouble(txtTDSAmt.Text.Trim()), 2).ToString();
                    grdBillingInfo.Rows[rowind].Cells[12].Text = txtTDSAmt.Text.Trim();
                    grdBillingInfo.Rows[rowind].Cells[13].Text = txttotalaftertax.Text.Trim();

                    //as columns loses its values again assign
                    grdBillingInfo.Rows[rowind].Cells[9].Text = txtdiscamount.Text.Trim();
                    grdBillingInfo.Rows[rowind].Cells[10].Text = txttotalafterdiscount.Text.Trim();

                    //

                    dsdetails.AcceptChanges();
                    Session["dsDetails"] = dsdetails;
                }
                
            }
            else
            {
                if (txtcommission.Text.Trim() != "")
                {
                    DataSet dsCommodity = (DataSet)Session["dsCommodity"]; ;
                    if (Convert.ToDouble(txtcommission.Text.Trim()) > 100)
                    {
                        lblStatus.Text = "Invalid commission percent";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }

                    double commAmt = (Convert.ToDouble(txtcommission.Text.Trim()) / 100) * Convert.ToDouble(txttotalafterdiscount.Text.Trim());
                    txtcommissionamt.Text = Math.Round(commAmt, 2).ToString();
                    //save calculation of Discount in dataset
                    dsCommodity.Tables[0].Rows[commrowind]["CommissionAmt"] = txtcommissionamt.Text.Trim();
                    txtRevisedTotal.Text = Math.Round(Convert.ToDouble(txttotalafterdiscount.Text.Trim()) - Convert.ToDouble(txtcommissionamt.Text.Trim()), 2).ToString();
                    grdCommodity.Rows[commrowind].Cells[12].Text = txtRevisedTotal.Text.Trim();

                    //Calculation for TDS amount and Total after tax
                    double tdsAmt = (Convert.ToDouble(txtTDSPer.Text.Trim()) / 100) * Convert.ToDouble(txtRevisedTotal.Text.Trim());
                    txtTDSAmt.Text = Math.Round(tdsAmt, 2).ToString();
                    //save calculation of TDS amount and Total after tax in dataset and gridview
                    txttotalaftertax.Text = Math.Round(Convert.ToDouble(txtRevisedTotal.Text.Trim()) + Convert.ToDouble(txtTDSAmt.Text.Trim()), 2).ToString();
                    grdCommodity.Rows[commrowind].Cells[13].Text = txtTDSAmt.Text.Trim();
                    grdCommodity.Rows[commrowind].Cells[14].Text = txttotalaftertax.Text.Trim();

                    //as columns loses its values again assign
                    grdCommodity.Rows[commrowind].Cells[10].Text = txtdiscamount.Text.Trim();
                    grdCommodity.Rows[commrowind].Cells[11].Text = txttotalafterdiscount.Text.Trim();

                    //

                    dsCommodity.AcceptChanges();
                    Session["dsCommodity"] = dsCommodity;
                }
            }
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            commissionAmtTextExit();
            
        }

        protected void commissionAmtTextExit()
        {
            if (grdCommodity.Visible == false)
            {
                if (txtcommissionamt.Text.Trim() != "")
                {
                    DataSet dsdetails = (DataSet)Session["dsDetails"];
                    if (Convert.ToDouble(txtcommissionamt.Text.Trim()) > Convert.ToDouble(txttotalafterdiscount.Text.Trim()))
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Invalid Commission Amount');</SCRIPT>");
                        lblStatus.Text = "Invalid commision amount";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }

                    double commPer = (100 / Convert.ToDouble(txttotalafterdiscount.Text.Trim())) * Convert.ToDouble(txtcommissionamt.Text.Trim());
                    txtcommission.Text = Math.Round(commPer, 2).ToString();
                    //save calculation of Discount in dataset
                    dsdetails.Tables[0].Rows[rowind]["Commission"] = txtcommission.Text.Trim();
                    txtRevisedTotal.Text = Math.Round(Convert.ToDouble(txttotalafterdiscount.Text.Trim()) - Convert.ToDouble(txtcommissionamt.Text.Trim()), 2).ToString();
                    grdBillingInfo.Rows[rowind].Cells[11].Text = txtRevisedTotal.Text.Trim();

                    //Calculation for TDS amount and Total after tax
                    double tdsAmt = (Convert.ToDouble(txtTDSPer.Text.Trim()) / 100) * Convert.ToDouble(txtRevisedTotal.Text.Trim());
                    txtTDSAmt.Text = Math.Round(tdsAmt, 2).ToString();
                    //save calculation of TDS amount and Total after tax in dataset and gridview
                    txttotalaftertax.Text = Math.Round(Convert.ToDouble(txtRevisedTotal.Text.Trim()) + Convert.ToDouble(txtTDSAmt.Text.Trim()), 2).ToString();
                    grdBillingInfo.Rows[rowind].Cells[12].Text = txtTDSAmt.Text.Trim();
                    grdBillingInfo.Rows[rowind].Cells[13].Text = txttotalaftertax.Text.Trim();

                    dsdetails.Tables[0].Rows[rowind]["RevisedTotal"] = txtRevisedTotal.Text.Trim();
                    dsdetails.Tables[0].Rows[rowind]["TDSAmt"] = txtTDSAmt.Text.Trim();
                    dsdetails.Tables[0].Rows[rowind]["Final"] = txttotalaftertax.Text.Trim();




                    //as columns loses its values again assign
                    grdBillingInfo.Rows[rowind].Cells[9].Text = txtdiscamount.Text.Trim();
                    grdBillingInfo.Rows[rowind].Cells[10].Text = txttotalafterdiscount.Text.Trim();


                    dsdetails.AcceptChanges();
                    Session["dsDetails"] = dsdetails;
                }
            }
            else
            {
                if (txtcommissionamt.Text.Trim() != "")
                {
                    DataSet dsCommodity = (DataSet)Session["dsCommodity"]; ;
                    if (Convert.ToDouble(txtcommissionamt.Text.Trim()) > Convert.ToDouble(txttotalafterdiscount.Text.Trim()))
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Invalid Commission Amount');</SCRIPT>");
                        lblStatus.Text = "Invalid commision amount";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }

                    double commPer = (100 / Convert.ToDouble(txttotalafterdiscount.Text.Trim())) * Convert.ToDouble(txtcommissionamt.Text.Trim());
                    txtcommission.Text = Math.Round(commPer, 2).ToString();
                    //save calculation of Discount in dataset
                    dsCommodity.Tables[0].Rows[commrowind]["Commission"] = txtcommission.Text.Trim();
                    txtRevisedTotal.Text = Math.Round(Convert.ToDouble(txttotalafterdiscount.Text.Trim()) - Convert.ToDouble(txtcommissionamt.Text.Trim()), 2).ToString();
                    grdCommodity.Rows[commrowind].Cells[12].Text = txtRevisedTotal.Text.Trim();

                    //Calculation for TDS amount and Total after tax
                    double tdsAmt = (Convert.ToDouble(txtTDSPer.Text.Trim()) / 100) * Convert.ToDouble(txtRevisedTotal.Text.Trim());
                    txtTDSAmt.Text = Math.Round(tdsAmt, 2).ToString();
                    //save calculation of TDS amount and Total after tax in dataset and gridview
                    txttotalaftertax.Text = Math.Round(Convert.ToDouble(txtRevisedTotal.Text.Trim()) + Convert.ToDouble(txtTDSAmt.Text.Trim()), 2).ToString();
                    grdCommodity.Rows[commrowind].Cells[13].Text = txtTDSAmt.Text.Trim();
                    grdCommodity.Rows[commrowind].Cells[14].Text = txttotalaftertax.Text.Trim();

                    dsCommodity.Tables[0].Rows[commrowind]["RevisedTotal"] = txtRevisedTotal.Text.Trim();
                    dsCommodity.Tables[0].Rows[commrowind]["TDSAmt"] = txtTDSAmt.Text.Trim();
                    dsCommodity.Tables[0].Rows[commrowind]["Final"] = txttotalaftertax.Text.Trim();




                    //as columns loses its values again assign
                    grdCommodity.Rows[commrowind].Cells[10].Text = txtdiscamount.Text.Trim();
                    grdCommodity.Rows[commrowind].Cells[11].Text = txttotalafterdiscount.Text.Trim();


                    dsCommodity.AcceptChanges();
                    Session["dsCommodity"] = dsCommodity;
                }
            }
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            tdsPerTextExit();
        }

        protected void tdsPerTextExit()
        {
            if (grdCommodity.Visible == false)
            {
                if (txtTDSPer.Text.Trim() != "")
                {
                    DataSet dsdetails = (DataSet)Session["dsDetails"];
                    if (Convert.ToDouble(txtTDSPer.Text.Trim()) > 100)
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Invalid TDS Percent');</SCRIPT>");
                        lblStatus.Text = "Invalid TDS percent";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }

                    double tdsAmt = (Convert.ToDouble(txtTDSPer.Text.Trim()) / 100) * Convert.ToDouble(txtRevisedTotal.Text.Trim());
                    txtTDSAmt.Text = Math.Round(tdsAmt, 2).ToString();

                    //save calculation of TDS amount and Total after tax in dataset and gridview
                    txttotalaftertax.Text = Math.Round(Convert.ToDouble(txtRevisedTotal.Text.Trim()) + Convert.ToDouble(txtTDSAmt.Text.Trim()), 2).ToString();
                    grdBillingInfo.Rows[rowind].Cells[11].Text = txtRevisedTotal.Text.Trim();
                    grdBillingInfo.Rows[rowind].Cells[12].Text = txtTDSAmt.Text.Trim();
                    grdBillingInfo.Rows[rowind].Cells[13].Text = txttotalaftertax.Text.Trim();

                    dsdetails.Tables[0].Rows[rowind]["RevisedTotal"] = txtRevisedTotal.Text.Trim();
                    dsdetails.Tables[0].Rows[rowind]["TDS"] = txtTDSAmt.Text.Trim();
                    dsdetails.Tables[0].Rows[rowind]["Final"] = txttotalaftertax.Text.Trim();




                    //as columns loses its values again assign
                    grdBillingInfo.Rows[rowind].Cells[9].Text = txtdiscamount.Text.Trim();
                    grdBillingInfo.Rows[rowind].Cells[10].Text = txttotalafterdiscount.Text.Trim();


                    dsdetails.AcceptChanges();
                    Session["dsDetails"] = dsdetails;
                }
            }
            else
            {
                if (txtTDSPer.Text.Trim() != "")
                {
                    DataSet dsCommodity = (DataSet)Session["dsCommodity"]; ;
                    if (Convert.ToDouble(txtTDSPer.Text.Trim()) > 100)
                    {
                        lblStatus.Text = "Invalid TDS percent";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }

                    double tdsAmt = (Convert.ToDouble(txtTDSPer.Text.Trim()) / 100) * Convert.ToDouble(txtRevisedTotal.Text.Trim());
                    txtTDSAmt.Text = Math.Round(tdsAmt, 2).ToString();

                    //save calculation of TDS amount and Total after tax in dataset and gridview
                    txttotalaftertax.Text = Math.Round(Convert.ToDouble(txtRevisedTotal.Text.Trim()) + Convert.ToDouble(txtTDSAmt.Text.Trim()), 2).ToString();
                    grdCommodity.Rows[commrowind].Cells[11].Text = txtRevisedTotal.Text.Trim();
                    grdCommodity.Rows[commrowind].Cells[12].Text = txtTDSAmt.Text.Trim();
                    grdCommodity.Rows[commrowind].Cells[13].Text = txttotalaftertax.Text.Trim();

                    dsCommodity.Tables[0].Rows[commrowind]["RevisedTotal"] = txtRevisedTotal.Text.Trim();
                    dsCommodity.Tables[0].Rows[commrowind]["TDS"] = txtTDSAmt.Text.Trim();
                    dsCommodity.Tables[0].Rows[commrowind]["Final"] = txttotalaftertax.Text.Trim();

                    //as columns loses its values again assign
                    grdCommodity.Rows[commrowind].Cells[9].Text = txtdiscamount.Text.Trim();
                    grdCommodity.Rows[commrowind].Cells[10].Text = txttotalafterdiscount.Text.Trim();


                    dsCommodity.AcceptChanges();
                    Session["dsCommodity"] = dsCommodity;
                }
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            //Confirm Single AWB number
            if (Panel1.Visible == true)
            {
                
                try
                {
                    if (txtDiscount.Text.Trim() == "")
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter Discount % ');</SCRIPT>");
                        lblStatus.Text = "Please enter Discount %";
                        lblStatus.ForeColor = Color.Blue;
                        txtDiscount.Focus();
                        return;
                    }

                    if (txtdiscamount.Text.Trim() == "")
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter Discount amounut');</SCRIPT>");
                        lblStatus.Text = "Please enter Discount amount";
                        lblStatus.ForeColor = Color.Blue;
                        txtdiscamount.Focus();
                        return;
                    }
                    if (txtTDSPer.Text.Trim() == "")
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter TDS % ');</SCRIPT>");
                        lblStatus.Text = "Please enter TDS %";
                        lblStatus.ForeColor = Color.Blue;
                        txtTDSPer.Focus();
                        return;
                    }

                   
                    #region Prepare Parameters
                    object[] RateCardInfo = new object[20];
                    int i = 0;

                    //0
                    RateCardInfo.SetValue(txtAWBNo.Text, i);
                    i++;
                    
                    //AgentCode and AWBDate calculation in SP

                    RateCardInfo.SetValue(txtcommodity.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtdimensions.Text, i);
                    i++;

                    //1
                    RateCardInfo.SetValue(txtgrosswt.Text, i);
                    i++;

                    //2
                    RateCardInfo.SetValue(txtchargablewt.Text, i);
                    i++;

                    //3
                    RateCardInfo.SetValue(Convert.ToDouble(txtiatarate.Text) + Convert.ToDouble(txtmktrate.Text), i);
                    i++;

                    //4
                    RateCardInfo.SetValue(txtocdc.Text, i);
                    i++;


                    RateCardInfo.SetValue(txtocda.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtTotal.Text, i);
                    i++;


                    RateCardInfo.SetValue(txtDiscount.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtdiscamount.Text, i);
                    i++;

                    RateCardInfo.SetValue(txttotalafterdiscount.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtcommission.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtcommissionamt.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtRevisedTotal.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtTDSPer.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtTDSAmt.Text, i);
                    i++;

                    RateCardInfo.SetValue(txttotalaftertax.Text, i);
               
                    #endregion Prepare Parameters

                    string res = "";
                    res = objBAL.AddSingleBillingAWB(RateCardInfo);
                  

                    if (res != "error")
                    {
                        ClearPanel();
                        bindGridView();

                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                        lblStatus.Text =res;
                        lblStatus.ForeColor = Color.Blue;


                    }
                    else
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                        lblStatus.Text = res;
                        lblStatus.ForeColor = Color.Red;
                    }

                }
                catch (Exception ex)
                {

                }

            }
            else // Confirm checked AWB Number
            {
                for (int i = 0; i < grdBillingInfo.Rows.Count; i++)
                {
                    if (((CheckBox)grdBillingInfo.Rows[i].FindControl("ChkSelect")).Checked)
                    {
                        try
                        {
                            ds = (DataSet) Session["dsDetails"];

                            #region Prepare Parameters
                            object[] RateCardInfo = new object[18];
                            int irow = 0;

                            //0
                            RateCardInfo.SetValue(ds.Tables[0].Rows[i]["AWBNumber"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[i]["CommodityDesc"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[i]["Dimensions"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[i]["GrossWeight"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[i]["ChargedWeight"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(Convert.ToDouble(ds.Tables[0].Rows[i]["FrtIATA"].ToString()) + Convert.ToDouble(ds.Tables[0].Rows[i]["FrtMKT"].ToString()), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[i]["OCDueCar"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[i]["OCDueAgent"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[i]["TotalT"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[i]["Discount"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[i]["DiscountAmt"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[i]["TAD"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[i]["Commission"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[i]["CommissionAmt"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[i]["RevisedTotal"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[i]["TDS"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[i]["TDSAmt"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[i]["Final"].ToString(), irow);
                   
                         
                            #endregion Prepare Parameters

                            string res = "";
                            res = objBAL.AddSingleBillingAWB(RateCardInfo);


                            if (res != "error")
                            {
                                ClearPanel();

                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                                lblStatus.Text = res;
                                lblStatus.ForeColor = Color.Blue;


                            }
                            else
                            {
                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                                lblStatus.Text = res;
                                lblStatus.ForeColor = Color.Red;
                            }

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

                bindGridView();
            }
        }

       

        //protected void GenerateInvoiceReport(string awbList)
        //{
        //    #region Prepare Parameters
        //    object[] AwbRateInfo = new object[1];
        //    int i = 0;

        //    AwbRateInfo.SetValue(awbList, i);
           
        //    #endregion Prepare Parameters

        //    ds = objBAL.GetInvoiceDetails(AwbRateInfo);

        //    if (ds != null)
        //    {
        //        if (ds.Tables != null)
        //        {
        //            if (ds.Tables.Count > 0)
        //            {
        //                if (ds.Tables[0].Rows.Count > 0)
        //                {
        //                    try
        //                    {
        //                        Session["ShowExcel"] = "";

        //                        DataTable DTExport = new DataTable();

        //                        DTExport.Columns.Add("FrightCharges");
        //                        DTExport.Columns.Add("AWBCharges");
        //                        DTExport.Columns.Add("FuelSurcharge");
        //                        DTExport.Columns.Add("DueCarrier");
        //                        DTExport.Columns.Add("Servicetax");
        //                        DTExport.Columns.Add("TDSOnCommission");
        //                        DTExport.Columns.Add("ASubtotal");
        //                       // DTExport.Columns.Add("Less:-");
        //                        DTExport.Columns.Add("ACommission");
        //                        DTExport.Columns.Add("AServiceTax");
        //                        DTExport.Columns.Add("CCAWBs");
        //                        DTExport.Columns.Add("DebitNote");
        //                        DTExport.Columns.Add("TdsOnFreight");
        //                        DTExport.Columns.Add("BSubtotal");
        //                        DTExport.Columns.Add("NetDueToAirline");
        //                       // DTExport.Columns.Add("NET DUE TO AIRLINES : (A - B)");

        //                        //DTExport.Rows.Add(ds.Tables[0].Rows[0]["FreightRate"].ToString());
        //                        //DTExport.Rows.Add(ds.Tables[2].Rows[1]["Charge"].ToString());
        //                        //DTExport.Rows.Add(ds.Tables[2].Rows[2]["Charge"].ToString());
        //                        //DTExport.Rows.Add(ds.Tables[2].Rows[3]["Charge"].ToString());
        //                        double ServiceTax = Convert.ToDouble(ds.Tables[2].Rows[1]["Tax"].ToString()) + Convert.ToDouble(ds.Tables[2].Rows[2]["Tax"].ToString()) + Convert.ToDouble(ds.Tables[2].Rows[3]["Tax"].ToString());
        //                       // DTExport.Rows.Add(ServiceTax.ToString());
        //                        //DTExport.Rows.Add("-");
        //                        double SubTotalA = Convert.ToDouble(ds.Tables[0].Rows[0]["FreightRate"].ToString()) + Convert.ToDouble(ds.Tables[2].Rows[1]["Charge"].ToString()) + Convert.ToDouble(ds.Tables[2].Rows[2]["Charge"].ToString()) + Convert.ToDouble(ds.Tables[2].Rows[3]["Charge"].ToString())+ ServiceTax;
        //                        double TDSOnFreight = Convert.ToDouble(ds.Tables[0].Rows[0]["TDSAmt"].ToString());
        //                        double SubTotalB = TDSOnFreight;
        //                        DTExport.Rows.Add(ds.Tables[0].Rows[0]["FreightRate"].ToString(),ds.Tables[2].Rows[1]["Charge"].ToString(),ds.Tables[2].Rows[2]["Charge"].ToString(),ds.Tables[2].Rows[3]["Charge"].ToString(),ServiceTax.ToString(),"-",SubTotalA.ToString(),"-","-","-","-",TDSOnFreight,TDSOnFreight,SubTotalA-SubTotalB);

                               
        //                        Session["ShowExcel"] = DTExport;

        //                        Session["ShowExcelHeader"] = "";

        //                        DataTable DTExportHeader = new DataTable();

        //                     //   DTExportHeader.Columns.Add("SrNo");
        //                        DTExportHeader.Columns.Add("AWBNo");
        //                        DTExportHeader.Columns.Add("AWBDate");
        //                        DTExportHeader.Columns.Add("FlightNo");
        //                        DTExportHeader.Columns.Add("Origin");
        //                        DTExportHeader.Columns.Add("Dest");
        //                        DTExportHeader.Columns.Add("FreightType");
        //                        DTExportHeader.Columns.Add("NoOfPkgs");
        //                        DTExportHeader.Columns.Add("GrossWeight");
        //                        DTExportHeader.Columns.Add("ChargbleWeight");
        //                        DTExportHeader.Columns.Add("Rate");
        //                        DTExportHeader.Columns.Add("Freight");
        //                        DTExportHeader.Columns.Add("AWBCharges");
        //                        DTExportHeader.Columns.Add("FuelSurcharge");
        //                        DTExportHeader.Columns.Add("DueCarrier");
        //                        DTExportHeader.Columns.Add("DueAgent");
        //                        DTExportHeader.Columns.Add("Total");
        //                        DTExportHeader.Columns.Add("ServiceTax");
        //                        DTExportHeader.Columns.Add("TotalCharges");
        //                        DTExportHeader.Columns.Add("AgentName");
        //                        DTExportHeader.Columns.Add("Station");
        //                        DTExportHeader.Columns.Add("InvoiceNumber");


        //                        double Total = Convert.ToDouble(ds.Tables[0].Rows[0]["FreightRate"].ToString()) +
        //                            Convert.ToDouble(ds.Tables[2].Rows[1]["Charge"].ToString()) +
        //                            Convert.ToDouble(ds.Tables[2].Rows[2]["Charge"].ToString()) +
        //                            Convert.ToDouble(ds.Tables[2].Rows[3]["Charge"].ToString()) +
        //                            Convert.ToDouble(ds.Tables[0].Rows[0]["OCDueAgent"].ToString());

        //                        DTExportHeader.Rows.Add(
        //                            ds.Tables[0].Rows[0]["AWBNumber"].ToString(),
        //                            ds.Tables[0].Rows[0]["AWBDate"].ToString(),
        //                            "", //flightno
        //                            "", //origin
        //                            "", //dest
        //                            "", //frt type
        //                            "", //noofpackings
        //                            ds.Tables[0].Rows[0]["GrossWt"].ToString(),
        //                            ds.Tables[0].Rows[0]["ChargableWt"].ToString(),
        //                            ds.Tables[0].Rows[0]["FreightRate"].ToString(),
        //                            ds.Tables[2].Rows[1]["Charge"].ToString(),
        //                            ds.Tables[2].Rows[2]["Charge"].ToString(),
        //                            ds.Tables[2].Rows[3]["Charge"].ToString(),
        //                            ds.Tables[0].Rows[0]["OCDueAgent"].ToString(),
        //                            Total.ToString(), 
        //                            ServiceTax.ToString(),
        //                            (Total + ServiceTax).ToString(), 
        //                            "", //agent name
        //                            "", //station
        //                            ds.Tables[0].Rows[0]["InvoiceNumber"].ToString());

        //                        Session["ShowExcelHeader"] = DTExportHeader;


        //                        Response.Redirect("showBillingExcel.aspx",false);

        //                    }
        //                    catch (Exception ex)
        //                    {
                                
        //                    }
        //                }
        //                else
        //                {
        //                    ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No records found.');</SCRIPT>");
        //                    return;
        //                }

        //            }
        //        }
        //    }
        //}

        protected void GenerateProformaInvoiceReport(string awbList)
        {
            #region Prepare Parameters
            object[] AwbRateInfo = new object[1];
            int i = 0;

            AwbRateInfo.SetValue(awbList, i);

            #endregion Prepare Parameters

            ds = objBAL.GetInvoiceDetailsAWBS(AwbRateInfo);

            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            try
                            {
                                Session["ShowExcel"] = "";

                                DataTable DTExport = new DataTable();

                                DTExport.Columns.Add("FrightCharges");
                                DTExport.Columns.Add("AWBCharges");
                                DTExport.Columns.Add("FuelSurcharge");
                                DTExport.Columns.Add("DueCarrier");
                                DTExport.Columns.Add("Servicetax");
                                DTExport.Columns.Add("TDSOnCommission");
                                DTExport.Columns.Add("ASubtotal");
                                // DTExport.Columns.Add("Less:-");
                                DTExport.Columns.Add("ACommission");
                                DTExport.Columns.Add("AServiceTax");
                                DTExport.Columns.Add("CCAWBs");
                                DTExport.Columns.Add("DebitNote");
                                DTExport.Columns.Add("TdsOnFreight");
                                DTExport.Columns.Add("BSubtotal");
                                DTExport.Columns.Add("NetDueToAirline");
                                DTExport.Columns.Add("AgentName");
                                DTExport.Columns.Add("Station");
                                DTExport.Columns.Add("InvoiceNumber");




                                double FreightRate1, AWBCharges1, FuelSurcharge1, DueCarrier1, ServiceTax, tdsOnCommission, SubTotalA, commission, STonCommission, TDSOnFreight, SubTotalB;

                                FreightRate1 = 0.0; AWBCharges1 = 0.0; FuelSurcharge1 = 0.0; tdsOnCommission = 0.0;
                                DueCarrier1 = 0.0; SubTotalA = 0.0; commission = 0.0; STonCommission = 0.0; TDSOnFreight = 0.0; SubTotalB = 0.0;

                                FreightRate1 = Convert.ToDouble(ds.Tables[1].Rows[0]["FreightRate"].ToString());
                                AWBCharges1 = Convert.ToDouble(ds.Tables[2].Rows[0]["Charge"].ToString());
                                FuelSurcharge1 = Convert.ToDouble(ds.Tables[2].Rows[2]["Charge"].ToString());
                                DueCarrier1 = Convert.ToDouble(ds.Tables[2].Rows[1]["Charge"].ToString());

                                ServiceTax = Convert.ToDouble(ds.Tables[2].Rows[0]["Tax"].ToString()) + Convert.ToDouble(ds.Tables[2].Rows[2]["Tax"].ToString()) + Convert.ToDouble(ds.Tables[2].Rows[1]["Tax"].ToString());
                                //tdsOnCommission is 10% OF COMMISSION AMT
                                tdsOnCommission = Convert.ToDouble(ds.Tables[1].Rows[0]["CommissionAmt"].ToString()) * 10 / 100;
                                SubTotalA = FreightRate1 + AWBCharges1 + FuelSurcharge1 + DueCarrier1 + ServiceTax + tdsOnCommission;

                                commission = Convert.ToDouble(ds.Tables[1].Rows[0]["CommissionAmt"].ToString());
                                //Service tax on commission is ST% of commission amt
                                STonCommission = Convert.ToDouble(ds.Tables[1].Rows[0]["CommissionAmt"].ToString()) * ST/100;
                                TDSOnFreight = Convert.ToDouble(ds.Tables[1].Rows[0]["TDSAmt"].ToString());
                                SubTotalB = TDSOnFreight + commission + STonCommission;


                                DTExport.Rows.Add(
                                    FreightRate1, 
                                    AWBCharges1, 
                                    FuelSurcharge1, 
                                    DueCarrier1, 
                                    ServiceTax.ToString(),
                                    tdsOnCommission.ToString(), 
                                    SubTotalA.ToString(), 
                                    commission.ToString(),
                                    STonCommission.ToString(),
                                    "-", "-", 
                                    TDSOnFreight, 
                                    SubTotalB,
                                    SubTotalA - SubTotalB, 
                                    ds.Tables[4].Rows[0]["AgentName"].ToString(), //agent name
                                    ds.Tables[4].Rows[0]["Station"].ToString(), //station
                                    ds.Tables[0].Rows[0]["InvoiceNumber"].ToString());


                                Session["ShowExcel"] = DTExport;

                                Session["ShowExcelHeader"] = "";

                                DataTable DTExportHeader = new DataTable();

                               
                                DTExportHeader.Columns.Add("AWBNo");
                                DTExportHeader.Columns.Add("AWBDate");
                                DTExportHeader.Columns.Add("FlightNo");
                                DTExportHeader.Columns.Add("Origin");
                                DTExportHeader.Columns.Add("Dest");
                                DTExportHeader.Columns.Add("FreightType");
                                DTExportHeader.Columns.Add("NoOfPkgs");
                                DTExportHeader.Columns.Add("GrossWeight");
                                DTExportHeader.Columns.Add("ChargbleWeight");
                                DTExportHeader.Columns.Add("Rate");
                                DTExportHeader.Columns.Add("Freight");
                                DTExportHeader.Columns.Add("AWBCharges2");
                                DTExportHeader.Columns.Add("FuelSurcharge2");
                                DTExportHeader.Columns.Add("DueCarrier2");
                                DTExportHeader.Columns.Add("DueAgent");
                                DTExportHeader.Columns.Add("Total");
                                DTExportHeader.Columns.Add("ServiceTax2");
                                DTExportHeader.Columns.Add("TotalCharges");
                                DTExportHeader.Columns.Add("SrNo");

                                //ServiceTax = Convert.ToDouble(ds.Tables[2].Rows[1]["Tax"].ToString()) + Convert.ToDouble(ds.Tables[2].Rows[2]["Tax"].ToString()) + Convert.ToDouble(ds.Tables[2].Rows[3]["Tax"].ToString());
                                //Total = Convert.ToDouble(ds.Tables[0].Rows[0]["FreightRate"].ToString()) +
                                //    Convert.ToDouble(ds.Tables[2].Rows[1]["Charge"].ToString()) +
                                //    Convert.ToDouble(ds.Tables[2].Rows[2]["Charge"].ToString()) +
                                //    Convert.ToDouble(ds.Tables[2].Rows[3]["Charge"].ToString()) +
                                //    Convert.ToDouble(ds.Tables[0].Rows[0]["OCDueAgent"].ToString());


                                //DTExportHeader.Rows.Add(
                                //    ds.Tables[0].Rows[0]["AWBNumber"].ToString(),
                                //    ds.Tables[0].Rows[0]["AWBDate"].ToString(),
                                //    ds.Tables[3].Rows[0]["FltNumber"].ToString(), //flightno
                                //    ds.Tables[3].Rows[0]["FltOrigin"].ToString(), //origin
                                //    ds.Tables[3].Rows[0]["FltDestination"].ToString(), //dest
                                //    ds.Tables[5].Rows[0]["Type"].ToString(), //frt type
                                //    "", //noofpackings
                                //    ds.Tables[0].Rows[0]["GrossWt"].ToString(),
                                //    ds.Tables[0].Rows[0]["ChargableWt"].ToString(),
                                //    "",
                                //    ds.Tables[0].Rows[0]["FreightRate"].ToString(),
                                //    ds.Tables[2].Rows[1]["Charge"].ToString(),
                                //    ds.Tables[2].Rows[2]["Charge"].ToString(),
                                //    ds.Tables[2].Rows[3]["Charge"].ToString(),
                                //    ds.Tables[0].Rows[0]["OCDueAgent"].ToString(),
                                //    Total.ToString(),
                                //    ServiceTax.ToString(),
                                //    (Total + ServiceTax).ToString(),
                                //    "1");


                                double grossWt, chargableWt, FreightRate, AWBCharges, FuelSurcharge, DueCarrier, DueAgent, TotalT, ServiceTaxT, TotalCharges;
                                grossWt = 0.0; chargableWt = 0.0; FreightRate = 0.0; AWBCharges = 0.0; FuelSurcharge = 0.0;
                                DueCarrier = 0.0; DueAgent = 0.0; TotalT = 0.0; ServiceTaxT = 0.0; TotalCharges = 0.0;
                                double Total;
                                Total = 0.0;
                                
                                for (int rowno = 0; rowno < ds.Tables[0].Rows.Count; rowno++)
                                {
                                    ServiceTax = Convert.ToDouble(ds.Tables[6].Rows[rowno + (rowno * 3)]["Tax"].ToString())  //Tax for AWB charges
                                        + Convert.ToDouble(ds.Tables[6].Rows[(rowno + 2) + (rowno * 3)]["Tax"].ToString()) //Tax for Fuel charges
                                        + Convert.ToDouble(ds.Tables[6].Rows[(rowno + 1) + (rowno * 3)]["Tax"].ToString()); //Tax for Due carrier
                                    Total = Convert.ToDouble(ds.Tables[0].Rows[rowno]["FreightRate"].ToString()) +
                                        Convert.ToDouble(ds.Tables[6].Rows[rowno + (rowno * 3)]["Charge"].ToString()) +
                                        Convert.ToDouble(ds.Tables[6].Rows[(rowno + 2) + (rowno * 3)]["Charge"].ToString()) +
                                        Convert.ToDouble(ds.Tables[6].Rows[(rowno + 1) + (rowno * 3)]["Charge"].ToString()) +
                                        Convert.ToDouble(ds.Tables[0].Rows[rowno]["OCDueAgent"].ToString());


                                    DTExportHeader.Rows.Add(
                                    ds.Tables[0].Rows[rowno]["AWBNumber"].ToString(),
                                    ds.Tables[0].Rows[rowno]["AWBDate"].ToString(),
                                    ds.Tables[3].Rows[rowno]["FltNumber"].ToString(), //flightno
                                    ds.Tables[3].Rows[rowno]["FltOrigin"].ToString(), //origin
                                    ds.Tables[3].Rows[rowno]["FltDestination"].ToString(), //dest
                                    ds.Tables[5].Rows[rowno]["Type"].ToString(), //frt type
                                    "", //noofpackings
                                    ds.Tables[0].Rows[rowno]["GrossWt"].ToString(),
                                    ds.Tables[0].Rows[rowno]["ChargableWt"].ToString(),
                                    "",
                                    ds.Tables[0].Rows[rowno]["FreightRate"].ToString(),
                                    ds.Tables[6].Rows[rowno + (rowno *3)]["Charge"].ToString(),
                                    ds.Tables[6].Rows[(rowno + 2) + (rowno * 3)]["Charge"].ToString(),
                                    ds.Tables[6].Rows[(rowno + 1) + (rowno * 3)]["Charge"].ToString(),
                                    ds.Tables[0].Rows[rowno]["OCDueAgent"].ToString(),
                                    Total.ToString(),
                                    ServiceTax.ToString(),
                                    (Total + ServiceTax).ToString(),
                                    rowno+1);

                                    //Calculations to show Total of awbs at bottom.
                                    grossWt = grossWt + Convert.ToDouble(ds.Tables[0].Rows[rowno]["GrossWt"].ToString());
                                    chargableWt = chargableWt + Convert.ToDouble(ds.Tables[0].Rows[rowno]["ChargableWt"].ToString());
                                    FreightRate = FreightRate + Convert.ToDouble(ds.Tables[0].Rows[rowno]["FreightRate"].ToString());
                                    AWBCharges = AWBCharges + Convert.ToDouble(ds.Tables[6].Rows[rowno + (rowno * 3)]["Charge"].ToString());
                                    FuelSurcharge = FuelSurcharge + Convert.ToDouble(ds.Tables[6].Rows[(rowno + 2) + (rowno * 3)]["Charge"].ToString());
                                    DueCarrier = DueCarrier + Convert.ToDouble(ds.Tables[6].Rows[(rowno + 1) + (rowno * 3)]["Charge"].ToString());
                                    DueAgent = DueAgent + Convert.ToDouble(ds.Tables[0].Rows[rowno]["OCDueAgent"].ToString());
                                    TotalT = TotalT + Total;
                                    ServiceTaxT = ServiceTaxT + ServiceTax;
                                    TotalCharges = TotalCharges + Total + ServiceTax;

                                }

                                //to add row of sum of awbs
                                DTExportHeader.Rows.Add(
                                    "",
                                    "",
                                    "", //flightno
                                    "", //origin
                                    "", //dest
                                    "", //frt type
                                    "", //noofpackings
                                    grossWt.ToString(),
                                    chargableWt.ToString(),
                                    "",
                                    FreightRate.ToString(),
                                    AWBCharges.ToString(),
                                    FuelSurcharge.ToString(),
                                    DueCarrier.ToString(),
                                    DueAgent.ToString(),
                                    TotalT.ToString(),
                                    ServiceTaxT.ToString(),
                                    TotalCharges.ToString(),
                                    "");


                                Session["ShowExcelHeader"] = DTExportHeader;


                                Response.Write("<script>");
                                Response.Write("window.open('showProformaExcel.aspx','_blank')");
                                Response.Write("</script>");
 
                                    
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No records found.');</SCRIPT>");
                            lblStatus.Text = "No records found";
                            lblStatus.ForeColor = Color.Blue;
                            return;
                        }

                    }
                }
            }
        }

        protected void GenerateAgentInvoiceReport(string awbList)
        {
            #region Prepare Parameters
            object[] AwbRateInfo = new object[1];
            int i = 0;

            AwbRateInfo.SetValue(awbList, i);

            #endregion Prepare Parameters

            ds = objBAL.GetInvoiceDetailsAWBS(AwbRateInfo);

            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            try
                            {
                                Session["ShowExcel"] = "";

                                DataTable DTExport = new DataTable();

                                DTExport.Columns.Add("FrightCharges");
                                DTExport.Columns.Add("AWBCharges");
                                DTExport.Columns.Add("FuelSurcharge");
                                DTExport.Columns.Add("DueCarrier");
                                DTExport.Columns.Add("Servicetax");
                                DTExport.Columns.Add("TDSOnCommission");
                                DTExport.Columns.Add("ASubtotal");
                                // DTExport.Columns.Add("Less:-");
                                DTExport.Columns.Add("ACommission");
                                DTExport.Columns.Add("AServiceTax");
                                DTExport.Columns.Add("CCAWBs");
                                DTExport.Columns.Add("DebitNote");
                                DTExport.Columns.Add("TdsOnFreight");
                                DTExport.Columns.Add("BSubtotal");
                                DTExport.Columns.Add("NetDueToAirline");
                                DTExport.Columns.Add("AgentName");
                                DTExport.Columns.Add("Station");
                                DTExport.Columns.Add("InvoiceNumber");




                                //double FreightRate1, AWBCharges1, FuelSurcharge1, DueCarrier1, ServiceTax, SubTotalA, TDSOnFreight, SubTotalB;

                                //FreightRate1 = 0.0; AWBCharges1 = 0.0; FuelSurcharge1 = 0.0;
                                //DueCarrier1 = 0.0; SubTotalA = 0.0; TDSOnFreight = 0.0; SubTotalB = 0.0;

                                //FreightRate1 = Convert.ToDouble(ds.Tables[1].Rows[0]["FreightRate"].ToString());
                                //AWBCharges1 = Convert.ToDouble(ds.Tables[2].Rows[0]["Charge"].ToString());
                                //FuelSurcharge1 = Convert.ToDouble(ds.Tables[2].Rows[2]["Charge"].ToString());
                                //DueCarrier1 = Convert.ToDouble(ds.Tables[2].Rows[1]["Charge"].ToString());

                                //ServiceTax = Convert.ToDouble(ds.Tables[2].Rows[0]["Tax"].ToString()) + Convert.ToDouble(ds.Tables[2].Rows[2]["Tax"].ToString()) + Convert.ToDouble(ds.Tables[2].Rows[1]["Tax"].ToString());
                                //SubTotalA = FreightRate1 + AWBCharges1 + FuelSurcharge1 + DueCarrier1 + ServiceTax;

                                //TDSOnFreight = Convert.ToDouble(ds.Tables[1].Rows[0]["TDSAmt"].ToString());
                                //SubTotalB = TDSOnFreight;


                                //DTExport.Rows.Add(
                                //    FreightRate1,
                                //    AWBCharges1,
                                //    FuelSurcharge1,
                                //    DueCarrier1,
                                //    ServiceTax.ToString(),
                                //    "-",
                                //    SubTotalA.ToString(),
                                //    "-", "-", "-", "-",
                                //    TDSOnFreight,
                                //    TDSOnFreight,
                                //    SubTotalA - SubTotalB,
                                //    ds.Tables[4].Rows[0]["AgentName"].ToString(), //agent name
                                //    ds.Tables[4].Rows[0]["Station"].ToString(), //station
                                //    ds.Tables[0].Rows[0]["InvoiceNumber"].ToString());


                                double FreightRate1, AWBCharges1, FuelSurcharge1, DueCarrier1, ServiceTax, tdsOnCommission, SubTotalA, commission, STonCommission, TDSOnFreight, SubTotalB;
                                double netDueToAirline; netDueToAirline = 0.0;

                                FreightRate1 = 0.0; AWBCharges1 = 0.0; FuelSurcharge1 = 0.0;
                                DueCarrier1 = 0.0; SubTotalA = 0.0; commission = 0.0; STonCommission = 0.0; TDSOnFreight = 0.0; SubTotalB = 0.0;

                                FreightRate1 = Convert.ToDouble(ds.Tables[1].Rows[0]["FreightRate"].ToString());
                                AWBCharges1 = Convert.ToDouble(ds.Tables[2].Rows[0]["Charge"].ToString());
                                FuelSurcharge1 = Convert.ToDouble(ds.Tables[2].Rows[2]["Charge"].ToString());
                                DueCarrier1 = Convert.ToDouble(ds.Tables[2].Rows[1]["Charge"].ToString());

                                ServiceTax = Convert.ToDouble(ds.Tables[2].Rows[0]["Tax"].ToString()) + Convert.ToDouble(ds.Tables[2].Rows[2]["Tax"].ToString()) + Convert.ToDouble(ds.Tables[2].Rows[1]["Tax"].ToString());
                                //tdsOnCommission is 10% OF COMMISSION AMT
                                tdsOnCommission = Convert.ToDouble(ds.Tables[1].Rows[0]["CommissionAmt"].ToString()) * 10 / 100;
                                SubTotalA = FreightRate1 + AWBCharges1 + FuelSurcharge1 + DueCarrier1 + ServiceTax + tdsOnCommission;

                                commission = Convert.ToDouble(ds.Tables[1].Rows[0]["CommissionAmt"].ToString());
                                //Service tax on commission is ST% of commission amt
                                STonCommission = Convert.ToDouble(ds.Tables[1].Rows[0]["CommissionAmt"].ToString()) * ST / 100;
                                TDSOnFreight = Convert.ToDouble(ds.Tables[1].Rows[0]["TDSAmt"].ToString());
                                SubTotalB = TDSOnFreight + commission + STonCommission;
                                netDueToAirline = SubTotalA - SubTotalB;


                                DTExport.Rows.Add(
                                    FreightRate1, //MKT + OCDC
                                    AWBCharges1,
                                    FuelSurcharge1,
                                    DueCarrier1,
                                    ServiceTax.ToString(), //COMING FROM AWB SUMMARY
                                    tdsOnCommission.ToString(), //10% OF COMMISSION AMT
                                    SubTotalA.ToString(),
                                    commission.ToString(),
                                    STonCommission.ToString(), //ST% OF COMMISSION AMT
                                    "-", "-",
                                    TDSOnFreight, //COMING FROM AGENT MASTER
                                    SubTotalB,
                                    netDueToAirline,
                                    ds.Tables[4].Rows[0]["AgentName"].ToString(), //agent name
                                    ds.Tables[4].Rows[0]["Station"].ToString(), //station
                                    ds.Tables[0].Rows[0]["InvoiceNumber"].ToString());



                                Session["ShowExcel"] = DTExport;

                                Session["ShowExcelHeader"] = "";

                                DataTable DTExportHeader = new DataTable();


                                DTExportHeader.Columns.Add("AWBNo");
                                DTExportHeader.Columns.Add("AWBDate");
                                DTExportHeader.Columns.Add("FlightNo");
                                DTExportHeader.Columns.Add("Origin");
                                DTExportHeader.Columns.Add("Dest");
                                DTExportHeader.Columns.Add("FreightType");
                                DTExportHeader.Columns.Add("NoOfPkgs");
                                DTExportHeader.Columns.Add("GrossWeight");
                                DTExportHeader.Columns.Add("ChargbleWeight");
                                DTExportHeader.Columns.Add("Rate");
                                DTExportHeader.Columns.Add("Freight");
                                DTExportHeader.Columns.Add("AWBCharges2");
                                DTExportHeader.Columns.Add("FuelSurcharge2");
                                DTExportHeader.Columns.Add("DueCarrier2");
                                DTExportHeader.Columns.Add("DueAgent");
                                DTExportHeader.Columns.Add("Total");
                                DTExportHeader.Columns.Add("ServiceTax2");
                                DTExportHeader.Columns.Add("TotalCharges");
                                DTExportHeader.Columns.Add("SrNo");

                                

                                double grossWt, chargableWt, FreightRate, AWBCharges, FuelSurcharge, DueCarrier, DueAgent, TotalT, ServiceTaxT, TotalCharges;
                                grossWt = 0.0; chargableWt = 0.0; FreightRate = 0.0; AWBCharges = 0.0; FuelSurcharge = 0.0;
                                DueCarrier = 0.0; DueAgent = 0.0; TotalT = 0.0; ServiceTaxT = 0.0; TotalCharges = 0.0;
                                double Total;
                                Total = 0.0;

                                for (int rowno = 0; rowno < ds.Tables[0].Rows.Count; rowno++)
                                {
                                    ServiceTax = Convert.ToDouble(ds.Tables[6].Rows[rowno + (rowno * 3)]["Tax"].ToString())  //Tax for AWB charges
                                        + Convert.ToDouble(ds.Tables[6].Rows[(rowno + 2) + (rowno * 3)]["Tax"].ToString()) //Tax for Fuel charges
                                        + Convert.ToDouble(ds.Tables[6].Rows[(rowno + 1) + (rowno * 3)]["Tax"].ToString()); //Tax for Due carrier
                                    Total = Convert.ToDouble(ds.Tables[0].Rows[rowno]["FreightRate"].ToString()) +
                                        Convert.ToDouble(ds.Tables[6].Rows[rowno + (rowno * 3)]["Charge"].ToString()) +
                                        Convert.ToDouble(ds.Tables[6].Rows[(rowno + 2) + (rowno * 3)]["Charge"].ToString()) +
                                        Convert.ToDouble(ds.Tables[6].Rows[(rowno + 1) + (rowno * 3)]["Charge"].ToString()) +
                                        Convert.ToDouble(ds.Tables[0].Rows[rowno]["OCDueAgent"].ToString());


                                    DTExportHeader.Rows.Add(
                                    ds.Tables[0].Rows[rowno]["AWBNumber"].ToString(),
                                    ds.Tables[0].Rows[rowno]["AWBDate"].ToString(),
                                    ds.Tables[3].Rows[rowno]["FltNumber"].ToString(), //flightno
                                    ds.Tables[3].Rows[rowno]["FltOrigin"].ToString(), //origin
                                    ds.Tables[3].Rows[rowno]["FltDestination"].ToString(), //dest
                                    ds.Tables[5].Rows[rowno]["Type"].ToString(), //frt type
                                    "", //noofpackings
                                    ds.Tables[0].Rows[rowno]["GrossWt"].ToString(),
                                    ds.Tables[0].Rows[rowno]["ChargableWt"].ToString(),
                                    "", //RATE
                                    ds.Tables[0].Rows[rowno]["FreightRate"].ToString(),
                                    ds.Tables[6].Rows[rowno + (rowno * 3)]["Charge"].ToString(), //AWB OCDC
                                    ds.Tables[6].Rows[(rowno + 2) + (rowno * 3)]["Charge"].ToString(), //FUEL OCDC
                                    ds.Tables[6].Rows[(rowno + 1) + (rowno * 3)]["Charge"].ToString(), //DUE CARRIER OCDC
                                    ds.Tables[0].Rows[rowno]["OCDueAgent"].ToString(),
                                    Total.ToString(),
                                    ServiceTax.ToString(),
                                    (Total + ServiceTax).ToString(),
                                    rowno + 1);

                                    //Calculations to show Total of awbs at bottom.
                                    grossWt = grossWt + Convert.ToDouble(ds.Tables[0].Rows[rowno]["GrossWt"].ToString());
                                    chargableWt = chargableWt + Convert.ToDouble(ds.Tables[0].Rows[rowno]["ChargableWt"].ToString());
                                    FreightRate = FreightRate + Convert.ToDouble(ds.Tables[0].Rows[rowno]["FreightRate"].ToString());
                                    AWBCharges = AWBCharges + Convert.ToDouble(ds.Tables[6].Rows[rowno + (rowno * 3)]["Charge"].ToString());
                                    FuelSurcharge = FuelSurcharge + Convert.ToDouble(ds.Tables[6].Rows[(rowno + 2) + (rowno * 3)]["Charge"].ToString());
                                    DueCarrier = DueCarrier + Convert.ToDouble(ds.Tables[6].Rows[(rowno + 1) + (rowno * 3)]["Charge"].ToString());
                                    DueAgent = DueAgent + Convert.ToDouble(ds.Tables[0].Rows[rowno]["OCDueAgent"].ToString());
                                    TotalT = TotalT + Total;
                                    ServiceTaxT = ServiceTaxT + ServiceTax;
                                    TotalCharges = TotalCharges + Total + ServiceTax;

                                }

                                //to add row of sum of awbs
                                DTExportHeader.Rows.Add(
                                    "",
                                    "",
                                    "", //flightno
                                    "", //origin
                                    "", //dest
                                    "", //frt type
                                    "", //noofpackings
                                    grossWt.ToString(),
                                    chargableWt.ToString(),
                                    "",
                                    FreightRate.ToString(),
                                    AWBCharges.ToString(),
                                    FuelSurcharge.ToString(),
                                    DueCarrier.ToString(),
                                    DueAgent.ToString(),
                                    TotalT.ToString(),
                                    ServiceTaxT.ToString(),
                                    TotalCharges.ToString(),
                                    "");


                                Session["ShowExcelHeader"] = DTExportHeader;
                                 

                                //Code to update AWBInvoice and CreditMaster with InvoiceAmount

                                string res = "";
                                #region Prepare Parameters
                                object[] AgentinvoiceInfo = new object[3];
                                int j = 0;

                                AgentinvoiceInfo.SetValue(ds.Tables[1].Rows[0]["AgentCode"].ToString(), j);
                                j++;

                                AgentinvoiceInfo.SetValue(ds.Tables[1].Rows[0]["InvoiceNumber"].ToString(), j);
                                j++;

                                AgentinvoiceInfo.SetValue(netDueToAirline, j);

                                #endregion Prepare Parameters
                                res = objBAL.UpdateAWBInvoiceCreditMaster(AgentinvoiceInfo);


                                Response.Write("<script>");
                                Response.Write("window.open('showBillingExcel.aspx','_blank')");
                                Response.Write("</script>");


                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No records found.');</SCRIPT>");
                            lblStatus.Text = "No records found";
                            lblStatus.ForeColor = Color.Blue;
                            return;
                        }

                    }
                }
            }
        }

        protected void ddlAgentCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlAgentName.SelectedIndex = ddlAgentCode.SelectedIndex;
        }

        protected void ddlAgentName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlAgentCode.SelectedIndex = ddlAgentName.SelectedIndex;
        }

        protected void ChkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkSelectAll.Checked == true)
            {
                for (int i = 0; i < grdBillingInfo.Rows.Count; i++)
                {
                    ((CheckBox)grdBillingInfo.Rows[i].FindControl("ChkSelect")).Checked = true;
                }
            }
            else
            {
                for (int i = 0; i < grdBillingInfo.Rows.Count; i++)
                {
                    ((CheckBox)grdBillingInfo.Rows[i].FindControl("ChkSelect")).Checked = false;
                }
            }
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            //Process Rates against each AWB number
            DataSet dsdetailsN = (DataSet)Session["dsDetails"];
            
            int j;
            bool verify = false;
            bool SelectedAWB;
            SelectedAWB = false;
            
            for (j = 0; j < grdBillingInfo.Rows.Count; j++)
            {
                if (((CheckBox)grdBillingInfo.Rows[j].FindControl("ChkSelect")).Checked)
                {
                    SelectedAWB = true;
                    //Code to check if AWBs of status New are only selected to verify.
                    if (dsdetailsN.Tables[0].Rows[j]["Confirmed"].ToString() == "New")
                    {
                        try
                        {
                            DataSet dsRates = objBook.ProcessRates(dsdetailsN.Tables[0].Rows[j]["AWBNumber"].ToString(), "", 0, AWBPrefix);
                            
                            if (dsdetailsN.Tables[0].Rows[j]["FrtIATA"].ToString() == dsRates.Tables[0].Rows[0]["FrIATA"].ToString() && dsdetailsN.Tables[0].Rows[j]["FrtMKT"].ToString() == dsRates.Tables[0].Rows[0]["FrMKT"].ToString() && dsdetailsN.Tables[0].Rows[j]["OCDueCar"].ToString() == dsRates.Tables[0].Rows[0]["OCDC"].ToString() && dsdetailsN.Tables[0].Rows[j]["OCDueAgent"].ToString() == dsRates.Tables[0].Rows[0]["OCDA"].ToString())
                            {
                                verify = true;
                            }
                        }
                        catch
                        {

                        }

                        try
                        {
                            ds = (DataSet)Session["dsDetails"];

                            #region Prepare Parameters
                            object[] RateCardInfo = new object[20];
                            int irow = 0;

                            //0
                            RateCardInfo.SetValue(ds.Tables[0].Rows[j]["AWBNumber"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[j]["CommodityDesc"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[j]["Dimensions"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[j]["GrossWeight"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[j]["ChargedWeight"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[j]["FrtIATA"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[j]["FrtMKT"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[j]["OCDueCar"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[j]["OCDueAgent"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[j]["TotalT"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[j]["Discount"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[j]["DiscountAmt"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[j]["TAD"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[j]["Commission"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[j]["CommissionAmt"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[j]["RevisedTotal"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[j]["TDS"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[j]["TDSAmt"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(ds.Tables[0].Rows[j]["Final"].ToString(), irow);
                            irow++;

                            RateCardInfo.SetValue(verify, irow);


                            #endregion Prepare Parameters

                            string res = "";
                            res = objBAL.AddSingleVerifiedAWB(RateCardInfo);


                            if (res != "error")
                            {
                                ClearPanel();
                                

                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                                lblStatus.Text = res;
                                lblStatus.ForeColor = Color.Blue;


                            }
                            else
                            {
                                lblStatus.Text = res;
                                lblStatus.ForeColor = Color.Red;
                            }

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else
                    {
                        lblStatus.Text = "Only AWB's with status New can be verified.";
                        lblStatus.ForeColor = Color.Blue;
                        return;
                    }
                }
            }

            bindGridView();

            if (SelectedAWB == false)
            {
                lblStatus.Text = "Select AWB's to Verify.";
                lblStatus.ForeColor = Color.Blue;
                return;
            }
        }

        protected void btnGenerateInvoice_Click(object sender, EventArgs e)
        {
            //Generating the invoice for AWB's with status Proforma
            //( *** Multiple selection of AWBs *** ) 

            string awbList = "";
            string InvoiceNo1 = "";
            string InvoiceNo2 = "";
            DataSet dsdetailsN = (DataSet)Session["dsDetails"];
            int j;
            for (j = 0; j < grdBillingInfo.Rows.Count; j++)
            {
                if (((CheckBox)grdBillingInfo.Rows[j].FindControl("ChkSelect")).Checked)
                {
                    //Code to check if AWBs of status Proforma are only selected to generate invoice.
                    if (dsdetailsN.Tables[0].Rows[j]["Confirmed"].ToString() == "Proforma" || dsdetailsN.Tables[0].Rows[j]["Confirmed"].ToString() == "Confirmed")
                    {
                        try
                        {
                            if (awbList == "")
                            {

                                awbList = awbList + dsdetailsN.Tables[0].Rows[j]["AWBNumber"].ToString();
                            }
                            else
                            {
                                awbList = awbList + "," + dsdetailsN.Tables[0].Rows[j]["AWBNumber"].ToString();
                            }
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        lblStatus.Text = "Select AWB numbers with status Proforma or Confirmed to Generate Invoice.";
                        lblStatus.ForeColor = Color.Blue;
                        return;
                    }


                    //to check if AWB's of status Proforma are only selected to generate invoice.
                    if (InvoiceNo1 == "")
                    {
                        InvoiceNo1 = dsdetailsN.Tables[0].Rows[j]["InvoiceNumber"].ToString();
                    }
                    else
                    {
                        InvoiceNo2 = dsdetailsN.Tables[0].Rows[j]["InvoiceNumber"].ToString();

                        if (InvoiceNo1 != InvoiceNo2)
                        {
                            lblStatus.Text = "Select AWB numbers with same Invoice Number to Generate Invoice.";
                            lblStatus.ForeColor = Color.Blue;
                            return;
                        }
                    }

                    
                    
                }
            }


            #region Prepare Parameters
            object[] RateCardInfo = new object[1];
            int i = 0;

            //0
            RateCardInfo.SetValue(awbList, i);

            #endregion Prepare Parameters

            if (awbList != "")
            {
                //Code to update the status of AWBs select ed for Invoice genaration to "Final"
                string res = "";
                res = objBAL.UpdateAWBStatusFinal(RateCardInfo);

                if (res != "error")
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                    lblStatus.Text = res;
                    lblStatus.ForeColor = Color.Green;
                    bindGridView();
                    GenerateAgentInvoiceReport(awbList);

                }
                else
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                    lblStatus.Text = res;
                    lblStatus.ForeColor = Color.Red;
                }
            }
            else
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Select AWB Numbers to generate invoice');</SCRIPT>");
                lblStatus.Text = "Select AWB numbers with status Proforma or Confirmed to generate Agent Invoice";
                lblStatus.ForeColor = Color.Blue;
            }

        }

        protected void btnGenerateProforma_Click(object sender, EventArgs e)
        {
            //Generate proforma Invoice for the AWB's with status Billable

            //Code to check if AWB are selected with status Billable.

            string awbList = "";
            DataSet dsdetailsN = (DataSet)Session["dsDetails"];
            for (int y = 0; y < grdBillingInfo.Rows.Count; y++)
            {
                if (((CheckBox)grdBillingInfo.Rows[y].FindControl("ChkSelect")).Checked)
                {
                    if (dsdetailsN.Tables[0].Rows[y]["Confirmed"].ToString() == "Billable") //status should be "Billable"
                    {
                        if (awbList == "")
                        {
                            awbList = awbList + dsdetailsN.Tables[0].Rows[y]["AWBNumber"].ToString();
                        }
                        else
                        {
                            awbList = awbList + "," + dsdetailsN.Tables[0].Rows[y]["AWBNumber"].ToString();
                        }
                    }
                    else
                    {
                        lblStatus.Text = "Proforma Invoice can be generated only for Billable AWB's";
                        lblStatus.ForeColor = Color.Blue;
                        return;
                    }
                }
                
            }


            
            int j;
            for (j = 0; j < grdBillingInfo.Rows.Count; j++)
            {
                if (((CheckBox)grdBillingInfo.Rows[j].FindControl("ChkSelect")).Checked)
                {
                    
                    try
                    {
                        ds = (DataSet)Session["dsDetails"];

                        #region Prepare Parameters
                        object[] AWBInfo = new object[19];
                        int irow = 0;

                        //0
                        AWBInfo.SetValue(ds.Tables[0].Rows[j]["AWBNumber"].ToString(), irow);
                        irow++;

                        AWBInfo.SetValue(ds.Tables[0].Rows[j]["CommodityDesc"].ToString(), irow);
                        irow++;

                        AWBInfo.SetValue(ds.Tables[0].Rows[j]["Dimensions"].ToString(), irow);
                        irow++;

                        AWBInfo.SetValue(ds.Tables[0].Rows[j]["GrossWeight"].ToString(), irow);
                        irow++;

                        AWBInfo.SetValue(ds.Tables[0].Rows[j]["ChargedWeight"].ToString(), irow);
                        irow++;

                        AWBInfo.SetValue(ds.Tables[0].Rows[j]["FrtIATA"].ToString(), irow);
                        irow++;

                        AWBInfo.SetValue(ds.Tables[0].Rows[j]["FrtMKT"].ToString(), irow);
                        irow++;

                        AWBInfo.SetValue(ds.Tables[0].Rows[j]["OCDueCar"].ToString(), irow);
                        irow++;

                        AWBInfo.SetValue(ds.Tables[0].Rows[j]["OCDueAgent"].ToString(), irow);
                        irow++;

                        AWBInfo.SetValue(ds.Tables[0].Rows[j]["TotalT"].ToString(), irow);
                        irow++;

                        AWBInfo.SetValue(ds.Tables[0].Rows[j]["Discount"].ToString(), irow);
                        irow++;

                        AWBInfo.SetValue(ds.Tables[0].Rows[j]["DiscountAmt"].ToString(), irow);
                        irow++;

                        AWBInfo.SetValue(ds.Tables[0].Rows[j]["TAD"].ToString(), irow);
                        irow++;

                        AWBInfo.SetValue(ds.Tables[0].Rows[j]["Commission"].ToString(), irow);
                        irow++;

                        AWBInfo.SetValue(ds.Tables[0].Rows[j]["CommissionAmt"].ToString(), irow);
                        irow++;

                        AWBInfo.SetValue(ds.Tables[0].Rows[j]["RevisedTotal"].ToString(), irow);
                        irow++;

                        AWBInfo.SetValue(ds.Tables[0].Rows[j]["TDS"].ToString(), irow);
                        irow++;

                        AWBInfo.SetValue(ds.Tables[0].Rows[j]["TDSAmt"].ToString(), irow);
                        irow++;

                        AWBInfo.SetValue(ds.Tables[0].Rows[j]["Final"].ToString(), irow);


                        #endregion Prepare Parameters

                        string res = "";
                        res = objBAL.AddSingleBillingAWB(AWBInfo);


                        if (res != "error")
                        {
                            //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                            //lblStatus.Text = res;
                            //lblStatus.ForeColor = Color.Blue;

                            //if (awbList == "")
                            //{

                            //    awbList = awbList + dsdetailsN.Tables[0].Rows[j]["AWBNumber"].ToString();
                            //}
                            //else
                            //{
                            //    awbList = awbList + "," + dsdetailsN.Tables[0].Rows[j]["AWBNumber"].ToString();
                            //}

                        }
                        else
                        {
                            //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                            lblStatus.Text = res;
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                    }
                    catch
                    {

                    }
                }
            }


            #region Prepare Parameters
            object[] RateCardInfo = new object[1];
            int i = 0;

            //0
            RateCardInfo.SetValue(awbList, i);

            #endregion Prepare Parameters

            if (awbList != "")
            {
                string res = "";
                if (j == 1) //for single awb
                    res = objBAL.GenerateInvoiceNo(RateCardInfo);
                else  //for multiple awb
                    res = objBAL.GenerateInvoiceNoAWBS(RateCardInfo);

                if (res != "error")
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                    bindGridView();
                    GenerateProformaInvoiceReport(awbList);

                }
                else
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                    lblStatus.Text = res;
                    lblStatus.ForeColor = Color.Red;
                }
            }
            else
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Select AWB Numbers to generate invoice');</SCRIPT>");
                lblStatus.Text = "Select AWB numbers with status Billable to generate Proforma Invoice";
                lblStatus.ForeColor = Color.Blue;
            }

        }

        protected void btnApproved_Click(object sender, EventArgs e)
        {
            //Code to convert AWB's with status pending to Billable. Keep field open to edit.

            bool SelectedAWB;
            SelectedAWB = false;
            for (int j = 0; j < grdBillingInfo.Rows.Count; j++)
            {
                if (((CheckBox)grdBillingInfo.Rows[j].FindControl("ChkSelect")).Checked)
                {
                    SelectedAWB = true;
                }
            }
            if (SelectedAWB == true)
            {

                DataSet dsDetails = (DataSet)Session["dsDetails"];
                for (int j = 0; j < grdBillingInfo.Rows.Count; j++)
                {
                    if (((CheckBox)grdBillingInfo.Rows[j].FindControl("ChkSelect")).Checked)
                    {
                        //Code to check if AWBs of status New are only selected to verify.
                        if (dsDetails.Tables[0].Rows[j]["Confirmed"].ToString() == "Pending")
                        {
                            try
                            {
                                #region Prepare Parameters
                                object[] RateCardInfo = new object[19];
                                int irow = 0;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["AWBNumber"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["CommodityDesc"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["Dimensions"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["GrossWeight"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["ChargedWeight"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["FrtIATA"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["FrtMKT"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["OCDueCar"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["OCDueAgent"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["TotalT"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["Discount"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["DiscountAmt"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["TAD"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["Commission"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["CommissionAmt"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["RevisedTotal"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["TDS"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["TDSAmt"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["Final"].ToString(), irow);


                                #endregion Prepare Parameters

                                string res = "";
                                res = objBAL.AddSingleBillingAWB(RateCardInfo);


                                if (res != "error")
                                {
                                    ClearPanel();


                                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                                    lblStatus.Text = res;
                                    lblStatus.ForeColor = Color.Blue;


                                }
                                else
                                {
                                    lblStatus.Text = res;
                                    lblStatus.ForeColor = Color.Red;
                                }

                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            lblStatus.Text = "Only AWB's with status New can be verified.";
                            lblStatus.ForeColor = Color.Blue;
                            return;
                        }
                    }
                }

                bindGridView();
            }
            
            else if (Panel1.Visible == true)
            {
                if (grdCommodity.Visible == false)
                {
                    DataSet dsStaus = (DataSet)Session["dsDetails"];
                    if (dsStaus.Tables[0].Rows[rowind]["Confirmed"].ToString() == "Pending") //status should be "Pending"
                    {
                        try
                        {
                            if (txtDiscount.Text.Trim() == "")
                            {
                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter Discount % ');</SCRIPT>");
                                lblStatus.Text = "Please enter Discount %";
                                lblStatus.ForeColor = Color.Blue;
                                txtDiscount.Focus();
                                return;
                            }

                            if (txtdiscamount.Text.Trim() == "")
                            {
                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter Discount amounut');</SCRIPT>");
                                lblStatus.Text = "Please enter Discount amount";
                                lblStatus.ForeColor = Color.Blue;
                                txtdiscamount.Focus();
                                return;
                            }
                            if (txtTDSPer.Text.Trim() == "")
                            {
                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter TDS % ');</SCRIPT>");
                                lblStatus.Text = "Please enter TDS %";
                                lblStatus.ForeColor = Color.Blue;
                                txtTDSPer.Focus();
                                return;
                            }


                            #region Prepare Parameters
                            object[] RateCardInfo = new object[21];
                            int i = 0;

                            //0
                            RateCardInfo.SetValue(txtAWBNo.Text, i);
                            i++;

                            //AgentCode and AWBDate calculation in SP

                            RateCardInfo.SetValue(txtcommodity.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtdimensions.Text, i);
                            i++;

                            //1
                            RateCardInfo.SetValue(txtgrosswt.Text, i);
                            i++;

                            //2
                            RateCardInfo.SetValue(txtchargablewt.Text, i);
                            i++;

                            //3
                            RateCardInfo.SetValue(txtiatarate.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtmktrate.Text, i);
                            i++;

                            //4
                            RateCardInfo.SetValue(txtocdc.Text, i);
                            i++;


                            RateCardInfo.SetValue(txtocda.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtTotal.Text, i);
                            i++;


                            RateCardInfo.SetValue(txtDiscount.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtdiscamount.Text, i);
                            i++;

                            RateCardInfo.SetValue(txttotalafterdiscount.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtcommission.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtcommissionamt.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtRevisedTotal.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtTDSPer.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtTDSAmt.Text, i);
                            i++;

                            RateCardInfo.SetValue(txttotalaftertax.Text, i);

                            #endregion Prepare Parameters

                            string res = "";
                            res = objBAL.AddSingleBillingAWB(RateCardInfo);


                            if (res != "error")
                            {
                                ClearPanel();
                                bindGridView();

                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                                lblStatus.Text = res;
                                lblStatus.ForeColor = Color.Blue;


                            }
                            else
                            {
                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                                lblStatus.Text = res;
                                lblStatus.ForeColor = Color.Red;
                            }

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else if (dsStaus.Tables[0].Rows[rowind]["Confirmed"].ToString() == "Billable") //status should be "Pending"
                    {
                        lblStatus.Text = "Billable AWB's can't be Approved.";
                        lblStatus.ForeColor = Color.Blue;
                        return;
                    }
                    else
                    {
                        lblStatus.Text = "Please Verify AWB before Approve.";
                        lblStatus.ForeColor = Color.Blue;
                        return;
                    }
                }
                else //Approve selected commodity of AWB
                {
                    DataSet dsStaus = (DataSet)Session["dsCommodity"];
                    if (dsStaus.Tables[0].Rows[commrowind]["Confirmed"].ToString() == "Pending") //status should be "Pending"
                    {
                        try
                        {
                            if (txtDiscount.Text.Trim() == "")
                            {
                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter Discount % ');</SCRIPT>");
                                lblStatus.Text = "Please enter Discount %";
                                lblStatus.ForeColor = Color.Blue;
                                txtDiscount.Focus();
                                return;
                            }

                            if (txtdiscamount.Text.Trim() == "")
                            {
                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter Discount amounut');</SCRIPT>");
                                lblStatus.Text = "Please enter Discount amount";
                                lblStatus.ForeColor = Color.Blue;
                                txtdiscamount.Focus();
                                return;
                            }
                            if (txtTDSPer.Text.Trim() == "")
                            {
                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter TDS % ');</SCRIPT>");
                                lblStatus.Text = "Please enter TDS %";
                                lblStatus.ForeColor = Color.Blue;
                                txtTDSPer.Focus();
                                return;
                            }


                            #region Prepare Parameters
                            object[] RateCardInfo = new object[21];
                            int i = 0;

                            //0
                            RateCardInfo.SetValue(txtAWBNo.Text, i);
                            i++;

                            //AgentCode and AWBDate calculation in SP

                            RateCardInfo.SetValue(txtcommodity.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtdimensions.Text, i);
                            i++;

                            //1
                            RateCardInfo.SetValue(txtgrosswt.Text, i);
                            i++;

                            //2
                            RateCardInfo.SetValue(txtchargablewt.Text, i);
                            i++;

                            //3
                            RateCardInfo.SetValue(txtiatarate.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtmktrate.Text, i);
                            i++;

                            //4
                            RateCardInfo.SetValue(txtocdc.Text, i);
                            i++;


                            RateCardInfo.SetValue(txtocda.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtTotal.Text, i);
                            i++;


                            RateCardInfo.SetValue(txtDiscount.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtdiscamount.Text, i);
                            i++;

                            RateCardInfo.SetValue(txttotalafterdiscount.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtcommission.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtcommissionamt.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtRevisedTotal.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtTDSPer.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtTDSAmt.Text, i);
                            i++;

                            RateCardInfo.SetValue(txttotalaftertax.Text, i);

                            #endregion Prepare Parameters

                            string res = "";
                            res = objBAL.UpdateSingleBillingAWBCommodity(RateCardInfo);


                            if (res != "error")
                            {
                                ClearPanel();
                                bindGridView();

                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                                lblStatus.Text = res;
                                lblStatus.ForeColor = Color.Blue;


                            }
                            else
                            {
                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                                lblStatus.Text = res;
                                lblStatus.ForeColor = Color.Red;
                            }

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else if (dsStaus.Tables[0].Rows[commrowind]["Confirmed"].ToString() == "Billable") //status should be "Pending"
                    {
                        lblStatus.Text = "Billable AWB's can't be Approved.";
                        lblStatus.ForeColor = Color.Blue;
                        return;
                    }
                    else
                    {
                        lblStatus.Text = "Please Verify AWB before Approve.";
                        lblStatus.ForeColor = Color.Blue;
                        return;
                    }
                }

            }
            else
            {
                lblStatus.Text = "Please click on AWB number to approve.";
                lblStatus.ForeColor = Color.Red;
            }
            
        }

        protected void ClearPanel()
        {
            txtAWBNo.Text = "";
            txtgrosswt.Text = "";
            txtchargablewt.Text = "";
            txtiatarate.Text = "";
            txtmktrate.Text = "";
            txtocdc.Text = "";
            txtocda.Text = "";
            txtcommodity.Text = "";
            txtdimensions.Text = "";
            txtTotal.Text = "";
            txtDiscount.Text = "";
            txtdiscamount.Text = "";
            txttotalafterdiscount.Text = "";
            txtcommission.Text = "";
            txtcommissionamt.Text = "";
            txtTDSPer.Text = "";
            txtRevisedTotal.Text = "";
            txtTDSAmt.Text = "";
            txttotalaftertax.Text = "";
            Panel1.Visible = false;

        }

        protected void btnConfirmInvoice_Click(object sender, EventArgs e)
        {
            //Code to update AWB's with status 'Proforma'. Keep field open to edit.

            //Confirm Single AWB number
            bool SelectedAWB;
            SelectedAWB = false;
            for (int j = 0; j < grdBillingInfo.Rows.Count; j++)
            {
                if (((CheckBox)grdBillingInfo.Rows[j].FindControl("ChkSelect")).Checked)
                {
                    SelectedAWB = true;
                }
            }
            if (SelectedAWB == true)
            {

                DataSet dsDetails = (DataSet)Session["dsDetails"];
                for (int j = 0; j < grdBillingInfo.Rows.Count; j++)
                {
                    if (((CheckBox)grdBillingInfo.Rows[j].FindControl("ChkSelect")).Checked)
                    {
                        //Code to check if AWBs of status New are only selected to verify.
                        if (dsDetails.Tables[0].Rows[j]["Confirmed"].ToString() == "Proforma")
                        {
                            try
                            {
                                #region Prepare Parameters
                                object[] RateCardInfo = new object[19];
                                int irow = 0;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["AWBNumber"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["CommodityDesc"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["Dimensions"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["GrossWeight"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["ChargedWeight"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["FrtIATA"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["FrtMKT"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["OCDueCar"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["OCDueAgent"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["TotalT"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["Discount"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["DiscountAmt"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["TAD"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["Commission"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["CommissionAmt"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["RevisedTotal"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["TDS"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["TDSAmt"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(dsDetails.Tables[0].Rows[j]["Final"].ToString(), irow);


                                #endregion Prepare Parameters

                                string res = "";
                                res = objBAL.AddSingleBillingAWB(RateCardInfo);


                                if (res != "error")
                                {
                                    ClearPanel();


                                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                                    lblStatus.Text = res;
                                    lblStatus.ForeColor = Color.Blue;


                                }
                                else
                                {
                                    lblStatus.Text = res;
                                    lblStatus.ForeColor = Color.Red;
                                }

                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            lblStatus.Text = "Only AWB's with status Proforma can be confirmed.";
                            lblStatus.ForeColor = Color.Blue;
                            return;
                        }
                    }
                }

                bindGridView();
            }

            else if (Panel1.Visible == true)
            {
                if (grdCommodity.Visible == false)
                {
                    DataSet dsStaus = (DataSet)Session["dsDetails"];
                    if (dsStaus.Tables[0].Rows[rowind]["Confirmed"].ToString() == "Proforma")
                    {
                        try
                        {
                            if (txtgrosswt.Text.Trim() == "")
                            {
                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter Discount % ');</SCRIPT>");
                                lblStatus.Text = "Please enter Gross Weight";
                                lblStatus.ForeColor = Color.Blue;
                                txtgrosswt.Focus();
                                return;
                            }

                            if (txtchargablewt.Text.Trim() == "")
                            {
                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter Discount amounut');</SCRIPT>");
                                lblStatus.Text = "Please enter Chargable Weight";
                                lblStatus.ForeColor = Color.Blue;
                                txtchargablewt.Focus();
                                return;
                            }
                            if (txtiatarate.Text.Trim() == "")
                            {
                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter TDS % ');</SCRIPT>");
                                lblStatus.Text = "Please enter IATA Rate";
                                lblStatus.ForeColor = Color.Blue;
                                txtiatarate.Focus();
                                return;
                            }

                            if (txtmktrate.Text.Trim() == "")
                            {
                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter TDS % ');</SCRIPT>");
                                lblStatus.Text = "Please enter MKT Rate";
                                lblStatus.ForeColor = Color.Blue;
                                txtmktrate.Focus();
                                return;
                            }

                            if (txtocda.Text.Trim() == "")
                            {
                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter TDS % ');</SCRIPT>");
                                lblStatus.Text = "Please enter OCDA";
                                lblStatus.ForeColor = Color.Blue;
                                txtocda.Focus();
                                return;
                            }

                            if (txtocdc.Text.Trim() == "")
                            {
                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter TDS % ');</SCRIPT>");
                                lblStatus.Text = "Please enter OCDC";
                                lblStatus.ForeColor = Color.Blue;
                                txtocdc.Focus();
                                return;
                            }

                            if (txtDiscount.Text.Trim() == "")
                            {
                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter Discount % ');</SCRIPT>");
                                lblStatus.Text = "Please enter Discount %";
                                lblStatus.ForeColor = Color.Blue;
                                txtDiscount.Focus();
                                return;
                            }

                            if (txtdiscamount.Text.Trim() == "")
                            {
                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter Discount amounut');</SCRIPT>");
                                lblStatus.Text = "Please enter Discount amount";
                                lblStatus.ForeColor = Color.Blue;
                                txtdiscamount.Focus();
                                return;
                            }
                            if (txtTDSPer.Text.Trim() == "")
                            {
                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter TDS % ');</SCRIPT>");
                                lblStatus.Text = "Please enter TDS %";
                                lblStatus.ForeColor = Color.Blue;
                                txtTDSPer.Focus();
                                return;
                            }


                            #region Prepare Parameters
                            object[] RateCardInfo = new object[22];
                            int i = 0;

                            RateCardInfo.SetValue(dsStaus.Tables[0].Rows[rowind]["InvoiceNumber"].ToString(), i);
                            i++;

                            RateCardInfo.SetValue(txtAWBNo.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtcommodity.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtdimensions.Text, i);
                            i++;

                            //1
                            RateCardInfo.SetValue(txtgrosswt.Text, i);
                            i++;

                            //2
                            RateCardInfo.SetValue(txtchargablewt.Text, i);
                            i++;

                            //3
                            RateCardInfo.SetValue(txtiatarate.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtmktrate.Text, i);
                            i++;

                            //4
                            RateCardInfo.SetValue(txtocdc.Text, i);
                            i++;


                            RateCardInfo.SetValue(txtocda.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtTotal.Text, i);
                            i++;


                            RateCardInfo.SetValue(txtDiscount.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtdiscamount.Text, i);
                            i++;

                            RateCardInfo.SetValue(txttotalafterdiscount.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtcommission.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtcommissionamt.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtRevisedTotal.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtTDSPer.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtTDSAmt.Text, i);
                            i++;

                            RateCardInfo.SetValue(txttotalaftertax.Text, i);

                            #endregion Prepare Parameters

                            string res = "";
                            res = objBAL.UpdateProformaAWB(RateCardInfo);


                            if (res != "error")
                            {
                                ClearPanel();
                                bindGridView();

                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                                lblStatus.Text = res;
                                lblStatus.ForeColor = Color.Blue;


                            }
                            else
                            {
                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                                lblStatus.Text = res;
                                lblStatus.ForeColor = Color.Red;
                            }

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else
                    {
                        lblStatus.Text = "AWB's with status Proforma can only be confirmed.";
                        lblStatus.ForeColor = Color.Blue;
                    }
                }
                else //Confirm specific commodity of AWB
                {
                    DataSet dsStaus = (DataSet)Session["dsCommodity"];
                    if (dsStaus.Tables[0].Rows[commrowind]["Confirmed"].ToString() == "Proforma") //status should be "Pending"
                    {
                        try
                        {
                            if (txtDiscount.Text.Trim() == "")
                            {
                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter Discount % ');</SCRIPT>");
                                lblStatus.Text = "Please enter Discount %";
                                lblStatus.ForeColor = Color.Blue;
                                txtDiscount.Focus();
                                return;
                            }

                            if (txtdiscamount.Text.Trim() == "")
                            {
                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter Discount amounut');</SCRIPT>");
                                lblStatus.Text = "Please enter Discount amount";
                                lblStatus.ForeColor = Color.Blue;
                                txtdiscamount.Focus();
                                return;
                            }
                            if (txtTDSPer.Text.Trim() == "")
                            {
                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter TDS % ');</SCRIPT>");
                                lblStatus.Text = "Please enter TDS %";
                                lblStatus.ForeColor = Color.Blue;
                                txtTDSPer.Focus();
                                return;
                            }


                            #region Prepare Parameters
                            object[] RateCardInfo = new object[20];
                            int i = 0;

                            RateCardInfo.SetValue(dsStaus.Tables[0].Rows[commrowind]["InvoiceNumber"].ToString(), i);
                            i++;

                            RateCardInfo.SetValue(txtAWBNo.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtcommodity.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtdimensions.Text, i);
                            i++;

                            //1
                            RateCardInfo.SetValue(txtgrosswt.Text, i);
                            i++;

                            //2
                            RateCardInfo.SetValue(txtchargablewt.Text, i);
                            i++;

                            //3
                            RateCardInfo.SetValue(txtiatarate.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtmktrate.Text, i);
                            i++;

                            //4
                            RateCardInfo.SetValue(txtocdc.Text, i);
                            i++;


                            RateCardInfo.SetValue(txtocda.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtTotal.Text, i);
                            i++;


                            RateCardInfo.SetValue(txtDiscount.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtdiscamount.Text, i);
                            i++;

                            RateCardInfo.SetValue(txttotalafterdiscount.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtcommission.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtcommissionamt.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtRevisedTotal.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtTDSPer.Text, i);
                            i++;

                            RateCardInfo.SetValue(txtTDSAmt.Text, i);
                            i++;

                            RateCardInfo.SetValue(txttotalaftertax.Text, i);

                            #endregion Prepare Parameters

                            string res = "";
                            res = objBAL.UpdateProformaAWBCommodity(RateCardInfo);


                            if (res != "error")
                            {
                                ClearPanel();
                                bindGridView();

                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                                lblStatus.Text = res;
                                lblStatus.ForeColor = Color.Blue;


                            }
                            else
                            {
                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                                lblStatus.Text = res;
                                lblStatus.ForeColor = Color.Red;
                            }

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else if (dsStaus.Tables[0].Rows[commrowind]["Confirmed"].ToString() == "Billable") //status should be "Pending"
                    {
                        lblStatus.Text = "Billable AWB's can't be Approved.";
                        lblStatus.ForeColor = Color.Blue;
                        return;
                    }
                    else
                    {
                        lblStatus.Text = "Please Verify AWB before Approve.";
                        lblStatus.ForeColor = Color.Blue;
                        return;
                    }

                }
            }
            else
            {
                lblStatus.Text = "Please click on AWB number to Confirm.";
                lblStatus.ForeColor = Color.Red;
            }
        }

        protected void grdCommodity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataSet dsCommodity = (DataSet)Session["dsCommodity"];
            Panel1.Visible = true;
            commrowind = Convert.ToInt32(e.CommandArgument);
            fillCommodityBillingDetails(commrowind);
            if (dsCommodity.Tables[0].Rows[commrowind]["Confirmed"].ToString() == "New")
            {
                Panel1.Enabled = false;
            }
            else
            {
                Panel1.Enabled = true;
            }

        }

        protected void fillCommodityBillingDetails(int ind)
        {
            DataSet dsCommdetails = (DataSet)Session["dsCommodity"];
            txtAWBNo.Text = dsCommdetails.Tables[0].Rows[ind]["AWBNumber"].ToString();
            txtgrosswt.Text = dsCommdetails.Tables[0].Rows[ind]["GrossWeight"].ToString();
            txtchargablewt.Text = dsCommdetails.Tables[0].Rows[ind]["ChargedWeight"].ToString();
            if (dsCommdetails.Tables[0].Rows[ind]["FrtIATA"].ToString() != "")
            {
                if (Convert.ToDouble(dsCommdetails.Tables[0].Rows[ind]["FrtIATA"].ToString()) >= 0)
                {
                    txtiatarate.Text = dsCommdetails.Tables[0].Rows[ind]["FrtIATA"].ToString();
                }
            }
            if (dsCommdetails.Tables[0].Rows[ind]["FrtMKT"].ToString() != "")
            {
                if (Convert.ToDouble(dsCommdetails.Tables[0].Rows[ind]["FrtMKT"].ToString()) >= 0)
                {
                    txtmktrate.Text = dsCommdetails.Tables[0].Rows[ind]["FrtMKT"].ToString();
                }
            }


            txtocda.Text = dsCommdetails.Tables[0].Rows[ind]["OCDueAgent"].ToString();
            txtocdc.Text = dsCommdetails.Tables[0].Rows[ind]["OCDueCar"].ToString();

            txtcommodity.Text = dsCommdetails.Tables[0].Rows[ind]["CommodityCode"].ToString();
            txtdimensions.Text = dsCommdetails.Tables[0].Rows[ind]["Dimensions"].ToString();

            txtTotal.Text = dsCommdetails.Tables[0].Rows[ind]["TotalT"].ToString();
            txtDiscount.Text = "0";
            txtdiscamount.Text = "0";
            txttotalafterdiscount.Text = "";
            txtcommission.Text = dsCommdetails.Tables[0].Rows[ind]["Commission"].ToString();
            //txtcommissionamt.Text = "0";
            //txtTDSPer.Text = "12.5";
            txtTDSPer.Text = dsCommdetails.Tables[0].Rows[ind]["TDS"].ToString();
            txtRevisedTotal.Text = "";
            txtTDSAmt.Text = "";
            txttotalaftertax.Text = "";

            discountPerTextExit();
            discountAmtTextExit();
            commissionPerTextExit();
            commissionAmtTextExit();

        }

        

        
    }
}
