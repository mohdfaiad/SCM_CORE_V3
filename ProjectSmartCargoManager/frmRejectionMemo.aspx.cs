using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using QID.DataAccess;
using System.Data;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class frmRejectionMemo : System.Web.UI.Page
    {
        #region Variables
        SQLServer db = new SQLServer(Global.GetConnectionString());
        string strfromdate, strtodate;
        BALRejectionMemo RM = new BALRejectionMemo();
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }
        #endregion

        #region Button List
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                GetCCADetails();
                

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion

        #region Button Clear
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                txtAWB.Text = "";
                txtCCAFrom.Text = "";
                txtCCATo.Text = "";
                txtInvoiceNo.Text = "";
                txtPreAWB.Text = "";
                txtRMAmount.Text = "";
                txtRMNumber.Text = "";
                txtRMType.Text = "";
                chkInterline.Checked = false;
                GrdCCADetails.DataSource = null;
                GrdCCADetails.DataBind();

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion
         
        #region Button Generate Rejection Memo
        protected void btnGenerateRM_Click(object sender, EventArgs e)
        {
            try
            {

                lblStatus.Text = "";
                int count = 0;
                string SerialNo="";
                string UpdatedBy = Session["UserName"].ToString();
                object[] QueryValues = new object[2];
                for (int i = 0; i < GrdCCADetails.Rows.Count; i++)
                {
                    if (((CheckBox)GrdCCADetails.Rows[i].FindControl("ChkSelect")).Checked)
                    {
                        count++;
                        if (count == 1)
                        {
                            SerialNo = ((Label)GrdCCADetails.Rows[i].FindControl("lblSerialNo")).Text;
                        }
                        else
                        {
                            SerialNo += "," + ((Label)GrdCCADetails.Rows[i].FindControl("lblSerialNo")).Text;
                        }
                    }
                }
                if (count < 1)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "No Records Selected!";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    QueryValues[0] = SerialNo;
                    QueryValues[1] = UpdatedBy;
                    if (RM.GenerateRejectionMemo(QueryValues))
                    {
                        lblStatus.Text = "Rejection Memo(s) created successfully!";
                        lblStatus.ForeColor = Color.Green;
                        lblStatus.Visible = true;
                        
                    }
                    else
                    {
                        lblStatus.Text = "Rejection Memo genereation failed! Please try again...";
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Visible = true;
                    }
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
                lblStatus.Visible = true;
            }
        }
        #endregion

        #region Button Print Rejection Memo
        protected void btnRejectionMemo_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion

        #region Button Print Rejection Memo as Per AWB
        protected void btnRejectionMemoPerAWB_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion

        public void GetCCADetails()
        {
            try
            {
                int i = 0;
                object[] objCCA = new object[5];
                //objCCA.SetValue(txtCCA.Text, i);
                objCCA.SetValue(txtAWB.Text.Trim(), i);
                objCCA.SetValue(txtInvoiceNo.Text.Trim(), ++i);
                //objCCA.SetValue(txtAgent.Text, ++i);

                //Validation for From date
                if (txtCCAFrom.Text == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select Valid date";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

                DateTime dt;

                try
                {
                    //dt = Convert.ToDateTime(txtInvoiceFrom.Text);
                    //Change 03082012
                    string day = txtCCAFrom.Text.Substring(0, 2);
                    string mon = txtCCAFrom.Text.Substring(3, 2);
                    string yr = txtCCAFrom.Text.Substring(6, 4);
                    strfromdate = yr + "-" + mon + "-" + day;
                    dt = Convert.ToDateTime(strfromdate);
                }
                catch (Exception ex)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Selected Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                //Validation for To date
                if (txtCCATo.Text == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select Valid date";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

                DateTime dtto;

                try
                {
                    //dtto = Convert.ToDateTime(txtInvoiceTo.Text);
                    //Change 03082012
                    string day = txtCCATo.Text.Substring(0, 2);
                    string mon = txtCCATo.Text.Substring(3, 2);
                    string yr = txtCCATo.Text.Substring(6, 4);
                    strtodate = yr + "-" + mon + "-" + day;
                    dtto = Convert.ToDateTime(strtodate);
                }
                catch (Exception ex)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Selected Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                if (dtto < dt)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "To date should be greater than From date";
                    lblStatus.ForeColor = Color.Red;

                    return;
                }

                objCCA.SetValue(Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), ++i);
                objCCA.SetValue(Convert.ToDateTime(strtodate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"), ++i);
                objCCA.SetValue(chkInterline.Checked, ++i);
                //objCCA.SetValue(ddlCCAType.SelectedValue, ++i);
                //objCCA.SetValue(ddlStatus.SelectedValue, ++i);

                //ClearCCA();
                DataSet ds = RM.ListCCA(objCCA);
                if (ds != null)
                {

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //Session["dsCCA"] = ds;
                            GrdCCADetails.Visible = true;
                            lblStatus.Visible = false;
                            GrdCCADetails.DataSource = ds.Tables[0];
                            GrdCCADetails.DataBind();
                            btnGenerateRM.Visible = true;
                        }
                        else
                        {
                            GrdCCADetails.Visible = false;
                            lblStatus.Visible = true;
                            lblStatus.Text = "No Data Found";
                            lblStatus.ForeColor = Color.Blue;
                            btnGenerateRM.Visible = false;

                        }
                    }
                }
            }
            catch (Exception ex)
            { }

        }
    }
}
