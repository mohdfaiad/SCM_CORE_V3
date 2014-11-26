using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class ListMultiplePackagingCharges : System.Web.UI.Page
    {
        ListDataViewBAL objBAL = new ListDataViewBAL();
        SQLServer da = new SQLServer(Global.GetConnectionString());
        string errormessage = "";
        DataSet dsResult = new DataSet("ListMultiplePackagingCharges_1");
        DataSet ddlds1 = new DataSet("ListMultiplePackagingCharges_2");

        protected void Page_Load(object sender, EventArgs e)
        {
            
            try
            {
                //string parentpage = Request.QueryString["Parent"].ToString();
                if (!IsPostBack)
                {
                    ddlds1 = da.GetDataset("select DISTINCT ChargeHeadCode as Value,ChargeHeadName as Detail from dbo.OtherChargesMaster WHERE IsPackaging = 1");
                    if (ddlds1.Tables.Count > 0)
                    {
                        if (ddlds1.Tables[0].Rows.Count > 0)
                        {
                            GRD.DataSource = null;
                            GRD.DataSource = ddlds1.Tables[0];
                            GRD.DataBind();
                        }
                        GRD.Columns[1].HeaderText = "Packaging Charge code";
                    }
                    showCheckSelection(ddlds1);
                }
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "alert('Error :'" + ex.Message + ")", true);
            }
            
            
        
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "window.close();", true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strSelected = "";
            int Piececount = 0;
            //string strselecteddesc = "";
            for (int i = 0; i < GRD.Rows.Count; i++)
            {
                if (((CheckBox)GRD.Rows[i].FindControl("ChkSelect")).Checked)
                {
                    if (((TextBox)GRD.Rows[i].FindControl("txtPieces")).Text.Trim() == "" || ((TextBox)GRD.Rows[i].FindControl("txtPieces")).Text.Trim() == "0")
                    {
                        lblError.Visible = true;
                        lblError.Text = "Please enter valid Pieces.";
                        ((TextBox)GRD.Rows[i].FindControl("txtPieces")).Focus();
                        return;
                    }

                    Piececount = Piececount + Convert.ToInt16(((TextBox)GRD.Rows[i].FindControl("txtPieces")).Text.Trim());

                    if (strSelected == "")
                    {
                        strSelected += "" + ((Label)GRD.Rows[i].FindControl("LBLValue")).Text.Trim() + "-" + ((TextBox)GRD.Rows[i].FindControl("txtPieces")).Text.Trim();
                        //strSelected += "" + ((Label)GRD.Rows[i].FindControl("LBLDetail")).Text;
                    }
                    else
                        strSelected += "," + ((Label)GRD.Rows[i].FindControl("LBLValue")).Text.Trim() + "-" + ((TextBox)GRD.Rows[i].FindControl("txtPieces")).Text.Trim();
                }                
            }

            //if (Piececount > Convert.ToInt16(hdnPieces.Value))
            //{
            //    lblError.Visible = true;
            //    lblError.Text = "Packaging pieces can not be more than AWB pieces.";
            //    return;
            //}
            
            ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "javascript:CloseWindow('" + strSelected + "');", true);
        }
        protected void showCheckSelection(DataSet ddlds1)
        {
            string sessionVar = Request.QueryString["txtValue"].ToString();

            if (Request.QueryString["txtPieces"] != null)
                hdnPieces.Value = Request.QueryString["txtPieces"].ToString();

            if (sessionVar.Length > 0)
            {
                string[] arrParam = sessionVar.Split(',');
            //string[] arrParam = ddlds1.Tables[0].ToString().Split(',');
                for (int i = 0; i < GRD.Rows.Count; i++)
                {
                    for (int j = 0; j < arrParam.Length; j++)
                    {
                        if (arrParam[j].Contains('-'))
                        {
                            string[] str = arrParam[j].Split('-');
                            if (((Label)GRD.Rows[i].FindControl("LBLValue")).Text.Equals(str[0], StringComparison.OrdinalIgnoreCase))
                            {
                                ((CheckBox)GRD.Rows[i].FindControl("ChkSelect")).Checked = true;
                                ((TextBox)GRD.Rows[i].FindControl("txtPieces")).Text = str[1].ToString();
                            }
                        }
                    }
            
                    //int ind = System.Array.IndexOf(arrParam, ddlds1.Tables[0].Rows[i][0].ToString());
                    //if (ind >= 0)
                    //{
                    //    ((CheckBox)GRD.Rows[i].FindControl("ChkSelect")).Checked = true;
                    //}
                    ////if (Session["mode"].ToString() == "View")
                    //{
                    //    btnSave.Visible = false;
                    //    ((CheckBox)GRD.Rows[i].FindControl("ChkSelect")).Enabled = false;
                    //}
                }
            }

        }

    }
}
