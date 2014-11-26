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
//using DAL;
using System.Drawing;
using System.Configuration; 

namespace ProjectSmartCargoManager
{
    public partial class AWBStockAllocMaster : System.Web.UI.Page
    {
        AWBStockAllocBAL objBAL = new AWBStockAllocBAL();
        DataSet ds;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadRegionDropdown();
                LoadCityDropdown();
                FillAWBStockAlloc_ALL();
            }
        }

        #region Load Region Dropdown
        public void LoadRegionDropdown()
        {
            DataSet ds = objBAL.GetRegion();
            if (ds != null)
            {
                ddlRegion.DataSource = ds;
                ddlRegion.DataMember = ds.Tables[0].TableName;
                ddlRegion.DataTextField = "RegionName";
                ddlRegion.DataValueField = "RegionCode";
                ddlRegion.DataBind();
                ddlRegion.Items.Insert(0, new ListItem("Select", string.Empty));
                
            }
        }
        #endregion Load Region Dropdown

        #region Load City Dropdown
        public void LoadCityDropdown()
        {
            DataSet ds = objBAL.GetCity();
            if (ds != null)
            {
                
                ddlCity.DataSource = ds;
                ddlCity.DataMember = ds.Tables[0].TableName;
                ddlCity.DataTextField = "CityName";
                ddlCity.DataValueField = "CityCode";
                ddlCity.DataBind();
                ddlCity.Items.Insert(0, new ListItem("Select", string.Empty));

                
            }
        }
        #endregion Load City Dropdown


        # region FillAWBStockAlloc_ALL
        public void FillAWBStockAlloc_ALL()
        {
            try
            {
                DataSet ds = objBAL.FillAWBStockAlloc_ALL();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            grdAWBStock.DataSource = ds;
                            grdAWBStock.DataMember = ds.Tables[0].TableName;
                            grdAWBStock.DataBind();
                            grdAWBStock.Visible = true;
                        }
                    }
                }
            }

            catch
            {
            }
        }

        # endregion FillAWBStockAlloc_ALL


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlRegion.SelectedItem.Text == "Select")
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please select Region');</SCRIPT>");
                    lblStatus.Text = "Please select Region";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

                if (ddlCity.SelectedItem.Text == "Select")
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please select City');</SCRIPT>");
                    lblStatus.Text = "Please select City";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

                if (txtAWBFrom.Text.Trim() == "")
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please Enter AWB From');</SCRIPT>");
                    lblStatus.Text = "Please Enter AWB From";
                    lblStatus.ForeColor = Color.Blue;
                    txtAWBFrom.Focus();
                    return;
                }

                if (txtAWBTo.Text.Trim() == "")
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please Enter AWB To');</SCRIPT>");
                    lblStatus.Text = "Please Enter AWB To";
                    lblStatus.ForeColor = Color.Blue;
                    txtAWBTo.Focus();
                    return;
                }
                
                #region Prepare Parameters
                object[] RateCardInfo = new object[4];
                int i = 0;
                
                //0
                RateCardInfo.SetValue(ddlRegion.SelectedValue, i);
                i++;

                //1
                RateCardInfo.SetValue(ddlCity.SelectedValue, i);
                i++;

                //3
                RateCardInfo.SetValue(txtAWBFrom.Text, i);
                i++;

                //4
                RateCardInfo.SetValue(txtAWBTo.Text, i);
               
                #endregion Prepare Parameters

                string res = "";
                res = objBAL.AddAWBStockAlloc(RateCardInfo);
                
                if (res != "error")
                {
                    ddlRegion.SelectedIndex = 0;
                    ddlCity.SelectedIndex = 0;
                    txtAWBFrom.Text = "";
                    txtAWBTo.Text = "";
                    
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                    lblStatus.Text = res;
                    lblStatus.ForeColor = Color.Green;
                    FillAWBStockAlloc_ALL();

                }
                else
                {
                    lblStatus.Text = res;
                    lblStatus.ForeColor = Color.Red;
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                }

            }
            catch (Exception ex)
            {

            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {

                #region Prepare Parameters
                object[] RateCardInfo = new object[2];
                int i = 0;

                //0
                RateCardInfo.SetValue(ddlRegion.SelectedValue, i);
                i++;

                //1
                RateCardInfo.SetValue(ddlCity.SelectedValue, i);
                

                #endregion Prepare Parameters



                DataSet ds = objBAL.FillAWBStockAlloc(RateCardInfo);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            grdAWBStock.DataSource = ds;
                            grdAWBStock.DataMember = ds.Tables[0].TableName;
                            grdAWBStock.DataBind();
                            grdAWBStock.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRegion.SelectedIndex > 0)
            {
                DataSet ds = objBAL.GetCityFiltered(ddlRegion.SelectedValue);
                if (ds != null)
                {

                    ddlCity.DataSource = ds;
                    ddlCity.DataMember = ds.Tables[0].TableName;
                    ddlCity.DataTextField = "CityName";
                    ddlCity.DataValueField = "CityCode";
                    ddlCity.DataBind();
                    ddlCity.Items.Insert(0, new ListItem("Select", string.Empty));


                }
            }
            else
            {
                LoadCityDropdown();
            }
        }
    }
}
