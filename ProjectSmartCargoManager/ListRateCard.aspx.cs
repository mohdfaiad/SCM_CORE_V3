using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Web.UI.WebControls.WebParts;
using System.Configuration;
using System.Data.SqlClient;
using BAL;
using System.Drawing;

namespace ProjectSmartCargoManager
{
    public partial class ListRateCard : System.Web.UI.Page
    {
        ListRateCardBAL objBAL = new ListRateCardBAL();


        # region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillGetRateCardList_ALL();
            }
        }
        # endregion Page_Load

        # region FillGetRateCardList_ALL
        public void FillGetRateCardList_ALL()
        {
            try
            {
                DataSet ds = objBAL.GetRateCardList_ALL();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            grdCountry.DataSource = ds;
                            grdCountry.DataMember = ds.Tables[0].TableName;
                            grdCountry.DataBind();
                            //grdCountry.Columns[0].Visible = false;
                            grdCountry.Visible = true;
                        }
                    }
                }
            }

            catch
            {
            }
        }

        # endregion FillGetRateCardList_ALL

        # region btnList_Click
        protected void btnList_Click(object sender, EventArgs e)
        {
            #region Prepare Parameters
            object[] RateListInfo = new object[2];
            int i = 0;

            //0
            RateListInfo.SetValue(ddlRateCardType.SelectedValue, i);
            i++;

            //Validation for date
            if (txtDate.Text == "")
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please select Valid date');</SCRIPT>");
                lblStatus.Text = "Please select Valid date";
                lblStatus.ForeColor = Color.Blue;
                // MessageBox.Show("Please Enter FlightID's which is not Operated");

                return;
            }

            DateTime dt;

            try
            {
                //dt = Convert.ToDateTime(txtDate.Text);

                dt = DateTime.ParseExact(txtDate.Text.ToString(), "dd/MM/yyyy", null);
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                lblStatus.Text = "Date format invalid";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            //1
           // RateListInfo.SetValue(Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd HH:mm:ss"), i);
            RateListInfo.SetValue(dt, i);


            #endregion Prepare Parameters


            DataSet ds = objBAL.GetRateCardList(RateListInfo);
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        grdCountry.DataSource = ds;
                        grdCountry.DataMember = ds.Tables[0].TableName;
                        grdCountry.DataBind();
                        //grdCountry.Columns[0].Visible = false;
                        grdCountry.Visible = true;
                    }
                }
            }
        }


        # endregion btnList_Click

        # region btnClear_Click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            //if(ddlRateCardType.SelectedIndex > 0)
            //    ddlRateCardType.SelectedIndex = 0;
            //txtDate.Text = "";
            Response.Redirect("~/ListRateCard.aspx", false);
        }

        # endregion btnClear_Click

        # region grid rowCommand Edit and View
        protected void grdCountry_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = grdCountry.Rows[index];
            string RateCardId = row.Cells[0].Text;
            Response.Redirect("RateCardMaster.aspx?cmd=" + e.CommandName + "&RCName=" + RateCardId);
        }
        # endregion grid rowCommand Edit and View
    }
}
