using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class DCMList : System.Web.UI.Page
    {
        DebitCreditProcessingBAL objBAL = new DebitCreditProcessingBAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblStatus.Text = "";
            }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            bindDCMList();
        }

        protected void bindDCMList()
        {
            try
            {
                string strfromdate, strtodate;
                lblStatus.Visible = false;
                lblStatus.Text = "";

                //Validation for From date
                if (txtDCMFrom.Text == "")
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
                    string day = txtDCMFrom.Text.Substring(0, 2);
                    string mon = txtDCMFrom.Text.Substring(3, 2);
                    string yr = txtDCMFrom.Text.Substring(6, 4);
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
                if (txtDCMTo.Text == "")
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
                    string day = txtDCMTo.Text.Substring(0, 2);
                    string mon = txtDCMTo.Text.Substring(3, 2);
                    string yr = txtDCMTo.Text.Substring(6, 4);
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


                DataSet DSInvoicedata = objBAL.GetDCMList(txtDCMNumber.Text, Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(strtodate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"), ddlDCMType.SelectedValue);

                if (DSInvoicedata != null && DSInvoicedata.Tables.Count > 0 && DSInvoicedata.Tables[0].Rows.Count > 0)
                {
                    Session["dsInvoice"] = DSInvoicedata;
                    grdDCMList.DataSource = DSInvoicedata.Tables[0];
                    grdDCMList.DataBind();
                    grdDCMList.Visible = true;
                    
                    
                    btnPrint.Visible = true;
                    

                }
                else
                {
                    grdDCMList.Visible = false;
                    btnPrint.Visible = false;
                    
                    lblStatus.Focus();
                    lblStatus.Visible = true;
                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = Color.Blue;
                }

            }
            catch (Exception ex)
            {

            }

        }
    }
}
