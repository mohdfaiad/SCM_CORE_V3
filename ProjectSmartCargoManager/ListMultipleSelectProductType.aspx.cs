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
    public partial class ListMultipleSelectProductType : System.Web.UI.Page
    {
        ListDataViewBAL objBAL = new ListDataViewBAL();
        SQLServer da = new SQLServer(Global.GetConnectionString());
        string errormessage = "";
        DataSet dsResult = new DataSet();
        DataSet ddlds1 = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //string parentpage = Request.QueryString["Parent"].ToString();
                if (!IsPostBack)
                {
                    ddlds1 = da.SelectRecords("SP_GetProductType");
                    if (ddlds1.Tables.Count > 0)
                    {
                        if (ddlds1.Tables[0].Rows.Count > 0)
                        {
                            GRD.DataSource = null;
                            GRD.DataSource = ddlds1.Tables[0];
                            GRD.DataBind();


                        }
                        GRD.Columns[1].HeaderText = "ProductType";
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
            //string strselecteddesc = "";
            for (int i = 0; i < GRD.Rows.Count; i++)
            {
                if (((CheckBox)GRD.Rows[i].FindControl("ChkSelect")).Checked)
                {
                    if (strSelected == "")
                    {
                        strSelected += "" + ((Label)GRD.Rows[i].FindControl("LBLValue")).Text;
                        //strSelected += "" + ((Label)GRD.Rows[i].FindControl("LBLDetail")).Text;
                    }
                    else
                        strSelected += "," + ((Label)GRD.Rows[i].FindControl("LBLValue")).Text;
                }

            }

            ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "javascript:CloseWindow('" + strSelected + "');", true);
        }

        protected void showCheckSelection(DataSet ddlds1)
        {
            string sessionVar = Request.QueryString["txtValue"].ToString();

            if (sessionVar.Length > 0)
            {
                string[] arrParam = sessionVar.Split(',');
                //string[] arrParam = ddlds1.Tables[0].ToString().Split(',');
                for (int i = 0; i < GRD.Rows.Count; i++)
                {
                    int ind = System.Array.IndexOf(arrParam, ddlds1.Tables[0].Rows[i][0].ToString());
                    if (ind >= 0)
                    {
                        ((CheckBox)GRD.Rows[i].FindControl("ChkSelect")).Checked = true;
                    }
                    
                }
            }

        }
    }
}
