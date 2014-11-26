using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.Data;

namespace ProjectSmartCargoManager
{
	public partial class CCAProcessing : System.Web.UI.Page
	{

        CCAProcessingBAL objBAL = new CCAProcessingBAL();
		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                
                lblStatus.Text = "";
                txtCCANumber.Text = "";
            }
        }
        #region GetCCANumber
        protected string GetCCANumber()
        {
            try
            {
                DataSet Ds = new DataSet();
                Ds = objBAL.GetCCANumber();
                return Ds.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception)
            {

                return null;
            }
           
        }
        # endregion GetCCANumber

        

        #region Fill Current & Revised Data
        protected void btnList_Click(object sender, EventArgs e)
        {
             //ClearFields();
             
             lblStatus.Text = "";
             txtCCANumber.Text = "";
            DataSet ds = new DataSet();
            DataTable dt;
            try
            {
                //string FromDate="",ToDate="";
                object[] objCCA = new object[6];
                int i = 0;
                //if (txtAgentFrom.Text!="")
                //FromDate = DateTime.ParseExact(txtAgentFrom.Text.Trim(), "dd-MMM-yyyy", null).ToString();
                //if (txtAgentTo.Text != "")
                //ToDate=DateTime.ParseExact(txtAgentFrom.Text.Trim(), "dd-MMM-yyyy", null).ToString();



                objCCA.SetValue(txtPreAWB.Text, i);
                objCCA.SetValue(txtAWB.Text, ++i);
                objCCA.SetValue(txtInvoiceNo.Text, ++i);
                objCCA.SetValue(txtAgent.Text, ++i);
                objCCA.SetValue(txtAgentFrom.Text != "" ? (DateTime.ParseExact(txtAgentFrom.Text.Trim(), "dd/MM/yyyy", null)).ToShortDateString().ToString() : txtAgentFrom.Text = "", ++i);
                objCCA.SetValue(txtAgentTo.Text != "" ? (DateTime.ParseExact(txtAgentTo.Text.Trim(), "dd/MM/yyyy", null)).ToShortDateString().ToString() : txtAgentTo.Text = "", ++i);
                    
           

                ds = objBAL.FillCurrentCCA(objCCA);

                dt = ds.Tables[0];
                




                txtCGrossWt.Text = dt.Rows[0][0].ToString();
                txtCCharWt.Text = dt.Rows[0][1].ToString();
                txtCFreight.Text = dt.Rows[0][2].ToString();
                txtCOCDC.Text = dt.Rows[0][3].ToString();
                txtCOCDA.Text = dt.Rows[0][4].ToString();
                txtCTax.Text = dt.Rows[0][5].ToString();
                txtCTot.Text = dt.Rows[0][6].ToString();

                //fill Revised
                txtRGrossWt.Text = txtCGrossWt.Text;
                txtRCharWt.Text = txtCCharWt.Text;
                txtRFreight.Text = txtCFreight.Text;
                txtROCDA.Text = txtCOCDA.Text;
                txtROCDC.Text = txtCOCDC.Text;
                txtRTax.Text = txtCTax.Text;
                txtRTot.Text = txtCTot.Text;







            }
            catch (Exception)
            {

                //  throw;
            }
        }
        #endregion Fill Current & Revised Data

        #region Save CCA Processing
        protected void btnSave_Click(object sender, EventArgs e)
        {
            object[] objCCA = new object[24];
            int i = 0;
            int valid = 1;

            try
            {

                if (txtRGrossWt.Text == txtCGrossWt.Text && txtRCharWt.Text == txtCCharWt.Text && txtRFreight.Text == txtCFreight.Text && txtROCDA.Text == txtCOCDA.Text && txtROCDC.Text == txtCOCDC.Text && txtRTax.Text == txtCTax.Text && txtRTot.Text == txtCTot.Text)
                {
                    lblStatus.Text = "Current & Revised Fields Are Identical";

                    // Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "show_alert2()", true);
                }
                else
                {if(txtRCre.Text=="" && txtRDeb.Text=="")
                { lblStatus.Text = "Please Enter Debit or Credit"; }
                else
                    {
                        string CCANo = GetCCANumber();
                        txtCCANumber.Text = CCANo;
                        objCCA.SetValue(txtPreAWB.Text.ToUpper(), i);
                        i++;
                        objCCA.SetValue(txtAWB.Text, i);
                        i++;
                        objCCA.SetValue(txtInvoiceNo.Text, i);
                        i++;
                        objCCA.SetValue(txtAgent.Text, i);
                        i++;
                        objCCA.SetValue(txtAgentFrom.Text != "" ? (DateTime.ParseExact(txtAgentFrom.Text.Trim(), "dd/MM/yyyy", null)).ToShortDateString().ToString() : txtAgentFrom.Text = "", i);
                        i++;
                        objCCA.SetValue(txtAgentTo.Text != "" ? (DateTime.ParseExact(txtAgentTo.Text.Trim(), "dd/MM/yyyy", null)).ToShortDateString().ToString() : txtAgentTo.Text = "", i);
                        i++;
                        objCCA.SetValue(CCANo, i);
                        i++;
                        objCCA.SetValue(txtCGrossWt.Text, i);
                        i++;
                        objCCA.SetValue(txtCCharWt.Text, i);
                        i++;
                        objCCA.SetValue(txtCFreight.Text, i);
                        i++;
                        objCCA.SetValue(txtCOCDC.Text, i);
                        i++;
                        objCCA.SetValue(txtCOCDA.Text, i);
                        i++;
                        objCCA.SetValue(txtCTax.Text, i);
                        i++;
                        objCCA.SetValue(txtCTot.Text, i);
                        i++;
                        objCCA.SetValue(txtRGrossWt.Text, i);
                        i++;
                        objCCA.SetValue(txtRCharWt.Text, i);
                        i++;
                        objCCA.SetValue(txtRFreight.Text, i);
                        i++;
                        objCCA.SetValue(txtRemarks.Text, i);
                        i++;
                        objCCA.SetValue(txtROCDC.Text, i);
                        i++;
                        objCCA.SetValue(txtROCDA.Text, i);
                        i++;
                        objCCA.SetValue(txtRDeb.Text, i);
                        i++;
                        objCCA.SetValue(txtRCre.Text, i);
                        i++;
                        objCCA.SetValue(txtRTax.Text, i);
                        i++;
                        objCCA.SetValue(txtRTot.Text, i);
                        // string CCANo = txtCCANumber.Text.ToString();





                        valid = objBAL.SaveCCAProcessing(objCCA);

                        if (valid == 0)
                        {
                            ClearAll();
                            lblStatus.Text = "CCA " + CCANo.ToString() + " Generated Successfuly";
                            // Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "show_alert()", true);


                        }
                        else
                            lblStatus.Text = "CCA Generation Failed ";
                        // Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "show_alert1()", true);

                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
          
        }
        #endregion Save CCAProcessing

        #region UpdateRevisedTab
        protected void UpdateRevised()
        { 
        
        }
        #endregion

        protected void ClearAll()////Clear All Fields
        {
    txtAgentTo.Text=txtAgentFrom.Text=txtInvoiceNo.Text=txtPreAWB.Text=txtAWB.Text= txtAgent.Text= txtCGrossWt.Text = txtCCharWt.Text = txtCFreight.Text = txtCOCDC.Text = txtCOCDA.Text = txtCTax.Text = txtCTot.Text = txtRGrossWt.Text = txtRCharWt.Text = txtRFreight.Text = txtRDeb.Text = txtRemarks.Text = txtRCre.Text = txtRTax.Text = txtRTot.Text = txtAgent.Text =txtROCDA.Text=txtROCDC.Text= "";

        }
        protected void ClearFields()//Clear Current & Revised tab Fields
        {

          txtCCANumber.Text=  txtCGrossWt.Text = txtCCharWt.Text = txtCFreight.Text = txtCOCDC.Text = txtCOCDA.Text = txtCTax.Text = txtCTot.Text = txtRGrossWt.Text = txtRCharWt.Text = txtRFreight.Text = txtRDeb.Text = txtRemarks.Text = txtRCre.Text = txtRTax.Text = txtRTot.Text = txtAgent.Text = txtROCDA.Text = txtROCDC.Text = "";

        }
	}
}
