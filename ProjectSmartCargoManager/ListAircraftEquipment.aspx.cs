using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.Data;
using System.Configuration;
using System.Data.Sql;
namespace ProjectSmartCargoManager
{
    public partial class ListAircraftEquipment : System.Web.UI.Page
    {
        AircraftBAL objBAL = new AircraftBAL();
        DataTable dTemp;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                // FillAircraftList();
                ClearAc();
                btnList_Click(null, null);
            }
        }
        # region FillAircraftList
        protected void FillAircraftList()
        {

            try
            {
                DataSet ds = objBAL.GetAircraftList();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlAircraftType.DataSource = ds.Tables[0];
                            ddlAircraftType.DataTextField = "AircraftType";
                            ddlAircraftType.DataValueField = "AircraftType";
                            ddlAircraftType.DataBind();
                            ddlVersion.DataSource = ds.Tables[1];
                            ddlVersion.DataTextField = "Version";
                            ddlVersion.DataValueField = "Version";
                            ddlVersion.DataBind();
                            ddlAircraftType.Items.Insert(0, "Select");
                            ddlVersion.Items.Insert(0, "Select");
                        }
                    }
                }
            }

            catch
            {
            }

        }

        # endregion FillGetRateCardList

        protected void ddlAircraftType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnList_Click(object sender, EventArgs e)
        {


            DataSet ds = new DataSet();
            try
            {

                object[] AcInfo = new object[2];
                int i = 0;

                //search parameers
                //ddlAircraftType.SelectedItem.Text.ToString();
                AcInfo.SetValue(ddlAircraftType.SelectedItem.Text.ToString(), i);
                i++;
                AcInfo.SetValue(ddlVersion.SelectedItem.Text.ToString(), i);


                ds = objBAL.GetAircraftEquipment(AcInfo);
                dTemp = ds.Tables[0];

                grdAcEq.DataSource = ds.Tables[0];
                grdAcEq.DataBind();
                ddlVersion.SelectedIndex = 0;
                ddlAircraftType.SelectedIndex = 0;

            }
            catch (Exception)
            {

                //  throw;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearAc();
        }
        protected void ClearAc()
        {

            FillAircraftList();
            grdAcEq.DataSource = "";
            grdAcEq.DataBind();
            ddlAircraftType.SelectedIndex = 0;
            ddlVersion.SelectedIndex = 0;
            // grdAcEq.Columns[9].Visible = false;
        }

        //protected void btnEdit_Click(object sender, EventArgs e)
        //{

        //    grdAcEq.Columns[9].Visible = true;//Enable Editing(Delete)


        //}

        protected void grdAcEq_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TextBox1.Text = grdAcEq.SelectedRow.ToString();

        }
        protected void grdAcEq_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //  int index = Convert.ToInt32(e.CommandName);
            try
            {
                int val = Convert.ToInt32(e.CommandArgument);
                EditAircraftEquipment(val);
            }
            catch (Exception)
            { }
        }
        public void EditAircraftEquipment(int val)//Saving Grid Data To Session
        {
            object[] EqInfo = new object[17];

            int i = 0;
            EqInfo.SetValue(grdAcEq.Rows[val].Cells[0].Text.Replace("&nbsp;", "").Trim(), i);
            i++;
            EqInfo.SetValue(grdAcEq.Rows[val].Cells[1].Text.Replace("&nbsp;", "").Trim(), i);
            i++;
            EqInfo.SetValue(grdAcEq.Rows[val].Cells[2].Text.Replace("&nbsp;", "").Trim(), i);
            i++;
            EqInfo.SetValue(grdAcEq.Rows[val].Cells[3].Text.Replace("&nbsp;", "").Trim(), i);
            i++;
            EqInfo.SetValue(grdAcEq.Rows[val].Cells[4].Text.Replace("&nbsp;", "").Trim(), i);
            i++;
            EqInfo.SetValue(grdAcEq.Rows[val].Cells[5].Text.Replace("&nbsp;", "").Trim(), i);
            i++;
            EqInfo.SetValue(grdAcEq.Rows[val].Cells[6].Text.Replace("&nbsp;", "").Trim(), i);
            i++;
            EqInfo.SetValue(grdAcEq.Rows[val].Cells[7].Text.Replace("&nbsp;", "").Trim(), i);
            i++;
            EqInfo.SetValue(grdAcEq.Rows[val].Cells[8].Text.Replace("&nbsp;", "").Trim(), i);
            i++;
            EqInfo.SetValue(grdAcEq.Rows[val].Cells[9].Text.Replace("&nbsp;", "").Trim(), i);
            i++;
            EqInfo.SetValue(grdAcEq.Rows[val].Cells[10].Text.Replace("&nbsp;", "").Trim(), i);
            i++;
            EqInfo.SetValue(grdAcEq.Rows[val].Cells[11].Text.Replace("&nbsp;", "").Trim(), i);
            i++;
            EqInfo.SetValue(grdAcEq.Rows[val].Cells[12].Text.Replace("&nbsp;", "").Trim(), i);
            i++;
            EqInfo.SetValue(grdAcEq.Rows[val].Cells[13].Text.Replace("&nbsp;", "").Trim(), i);
            i++;
            EqInfo.SetValue(grdAcEq.Rows[val].Cells[14].Text.Replace("&nbsp;", "").Trim(), i);
            i++;
            EqInfo.SetValue(grdAcEq.Rows[val].Cells[15].Text.Replace("&nbsp;", "").Trim(), i);
            i++;
            EqInfo.SetValue(grdAcEq.Rows[val].Cells[16].Text.Replace("&nbsp;", "").Trim(), i);
            Session["AcEq"] = EqInfo;
            Response.Redirect("EditAircraftEquipment.aspx?Srno=" + ((Label)grdAcEq.Rows[val].FindControl("lblsrno")).Text);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            int val = 0;
            EditAircraftEquipment(val);
        }


    }


}
