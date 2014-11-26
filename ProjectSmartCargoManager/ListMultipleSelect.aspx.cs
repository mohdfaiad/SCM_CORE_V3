using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class ListMultipleSelect : System.Web.UI.Page
    {

        ListDataViewBAL objBAL = new ListDataViewBAL();
        string errormessage = "";
        DataSet dsResult = new DataSet("ListMultipleSelect_1");


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string parentpage = Request.QueryString["Parent"].ToString();

                    switch (parentpage)
                    {
                       
                        case "MaintainRatesParam":
                            //DataSet dsResult = new DataSet();
                            if (objBAL.FillDdl("Param", ref dsResult, ref errormessage, new string[] { Request.QueryString["param"].ToString() }))
                            {
                                GRD.DataSource = null;
                                GRD.DataSource = dsResult.Tables[0];
                                GRD.DataBind();
                            }

                            GRD.Columns[1].HeaderText = "Currency";
                            break;

                        case "MaintainRatesORG":
                            //DataSet dsResult = new DataSet();
                            string level = Request.QueryString["level"].ToString();
                            if (objBAL.FillDdl("Origin", ref dsResult, ref errormessage, new string[] { level }))
                            {
                                GRD.DataSource = null;
                                GRD.DataSource = dsResult.Tables[0];
                                GRD.DataBind();
                            }

                            GRD.Columns[1].HeaderText = "Origin";
                            break;

                        case "FrmMessageConfiguration":
                            //DataSet dsResult = new DataSet();
                            if (objBAL.FillDdl("MsgType", ref dsResult, ref errormessage, null))
                            {
                                GRD.DataSource = null;
                                GRD.DataSource = dsResult.Tables[0];
                                GRD.DataBind();
                            }

                            GRD.Columns[1].HeaderText = "MessageType";
                            break;
                    }

                    //if (Session["mode"].ToString() == "Edit" || Session["mode"].ToString() == "View")
                    //{
                    //    showCheckSelection(dsResult);
                    //}

                    showCheckSelection(dsResult);
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
            for (int i = 0; i < GRD.Rows.Count; i++)
            {
                if (((CheckBox)GRD.Rows[i].FindControl("ChkSelect")).Checked)
                {
                    if (strSelected == "")
                        strSelected += "" + ((Label)GRD.Rows[i].FindControl("LBLValue")).Text;
                    else
                        strSelected += "," + ((Label)GRD.Rows[i].FindControl("LBLValue")).Text;

                }
            }
            if (Request.QueryString["param"].ToString() != "AgentCode")
            {
                Session[Request.QueryString["param"].ToString()] = strSelected;
            }
            ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "javascript:CloseWindow('" + strSelected + "');", true);
            
        }

        protected void showCheckSelection(DataSet dsRes)
        {
            try
            {
                //Session variable is same as querystring "param"
                string sessionVar = Request.QueryString["Values"].ToString();
                if (sessionVar.Length < 1)
                    return;
                string[] arrParam = sessionVar.Split(',');
                if (sessionVar.Length > 0)
                {
                    for (int i = 0; i < GRD.Rows.Count; i++)
                    {
                        int ind = System.Array.IndexOf(arrParam, dsRes.Tables[0].Rows[i][0].ToString());
                        if (ind >= 0)
                        {
                            ((CheckBox)GRD.Rows[i].FindControl("ChkSelect")).Checked = true;
                        }
                        if (Session["mode"].ToString() == "View")
                        {
                            btnSave.Visible = false;
                            ((CheckBox)GRD.Rows[i].FindControl("ChkSelect")).Enabled = false;
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }
    }
}
