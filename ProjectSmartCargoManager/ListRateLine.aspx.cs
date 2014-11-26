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
using System.Collections;

// 2012-07-17 vinayak

namespace ProjectSmartCargoManager
{
    public partial class ListRateLine : System.Web.UI.Page
    {
        bool flagSelAll = false;

        ListRateLineBAL objBAL = new ListRateLineBAL();
        MaintainRatesBAL mb = new MaintainRatesBAL();

        # region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                OriginList();
                DestinationList();
                lblUpdateStatus.Visible = false;
                //txtFromDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                //txtToDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                //FillGetListRateLine_ALL();

                //string agentCode = Convert.ToString(Session["ACode"]);
                //if (agentCode != "")
                //{
                //    grdListStock.Columns[13].Visible = false;
                //}
                // Fill RateCardDropDown
                DataSet dsResult = new DataSet();
                string errormessage = "";
                if (mb.FillDdl("DdlRateCard", ref dsResult, ref errormessage))
                {
                    DdlRateCardName.DataSource = null;
                    DdlRateCardName.DataTextField = "RateCardName";
                    DdlRateCardName.DataValueField = "ID";
                    DdlRateCardName.DataSource = dsResult.Tables[0].Copy();
                    DdlRateCardName.DataBind();
                    DdlRateCardName.Items.Insert(0, "Select");
                }

                HomeBL objHome = new HomeBL();
                int RoleId = Convert.ToInt32(Session["RoleID"]);
                DataSet objDS = objHome.GetUserPermissions(((System.Web.UI.TemplateControl)(Page)).AppRelativeVirtualPath, RoleId);
                objHome = null;

                if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < objDS.Tables[0].Rows.Count; i++)
                    {
                        if (objDS.Tables[0].Rows[i]["ControlId"].ToString() == "RateChargeEdit")
                        {
                            grdListStock.Columns[14].Visible = false;
                            break;
                        }
                    }
                }
                objDS = null;

                #region Define PageSize for grid as per configuration
                try
                {
                    LoginBL objConfig = new LoginBL();
                    grdListStock.PageSize = Convert.ToInt32(objConfig.GetMasterConfiguration("StdListPageSize"));
                    objConfig = null;
                }
                catch (Exception ex)
                { }
                #endregion
            }
            
        }
        # endregion Page_Load

        # region FillGetListRateLine_ALL
        public void FillGetListRateLine_ALL()
        {
            try
            {
                DataSet ds = objBAL.GetListRateLine_ALL();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            grdListStock.DataSource = ds;
                            grdListStock.DataMember = ds.Tables[0].TableName;
                            grdListStock.DataBind();
                            //grdListStock.Columns[0].Visible = false;
                            grdListStock.Visible = true;                            
                        }
                    }
                }
            }

            catch
            {

            }
        }

        # endregion FillGetListRateLine_ALL

        #region Origin List
        private void OriginList()
        {
            try
            {
                DataSet ds = objBAL.GetOriginList(ddlOrigin.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            ddlOrigin.DataSource = ds;
                            ddlOrigin.DataMember = ds.Tables[0].TableName;
                            ddlOrigin.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            ddlOrigin.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                            ddlOrigin.DataBind();
                            ddlOrigin.Items.Insert(0, new ListItem("Select", string.Empty));

                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion Origin List

        #region Destination List

        private void DestinationList()
        {
            try
            {
                DataSet ds = objBAL.GetDestinationList(ddlDestination.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            ddlDestination.DataSource = ds;
                            ddlDestination.DataMember = ds.Tables[0].TableName;
                            ddlDestination.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            ddlDestination.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                            ddlDestination.DataBind();
                            ddlDestination.Items.Insert(0, new ListItem("Select", string.Empty));

                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }



        #endregion Destination List

        #region btnList_Click
        protected void btnList_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            #region Prepare Parameters
            string AllIn = "", TACT = "";
            object[] RateLineInfo = new object[21];
            string values="";
            int i = 0;

            RateLineInfo.SetValue(DDLOriginLevel.SelectedIndex, i);
            i++;

            RateLineInfo.SetValue(DDLDestinationLevel.SelectedIndex, i);
            i++;

            RateLineInfo.SetValue(ddlOrigin.SelectedValue, i);

            i++;
            RateLineInfo.SetValue(ddlDestination.SelectedValue, i);

            if (txtFromDate.Text.Trim() == "")
            {
                if (txtToDate.Text.Trim() != "")
                {
                    lblStatus.Text = "Please enter From date";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
            }

            if (txtToDate.Text.Trim() == "")
            {
                if (txtFromDate.Text.Trim() != "")
                {
                    lblStatus.Text = "Please enter To date";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
            }


            i++;
            if (txtFromDate.Text.Trim() != "")
            {
                DateTime dtfrom;

                try
                {
                    //dtfrom = Convert.ToDateTime(txtFromDate.Text);
                    dtfrom = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                    lblStatus.Text = "Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                //RateLineInfo.SetValue(Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd HH:mm:ss"), i);
                string fromdate = dtfrom.ToString("MM/dd/yyyy");
                RateLineInfo.SetValue(fromdate, i);
            }
            else
            {
                RateLineInfo.SetValue("", i);
            }

            

            i++;

            if (txtToDate.Text.Trim() != "")
            {
                DateTime dtto;

                try
                {
                    //dtto = Convert.ToDateTime(txtToDate.Text);
                    dtto = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                    lblStatus.Text = "Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                //RateLineInfo.SetValue(Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd HH:mm:ss"), i);
                string todate = dtto.ToString("MM/dd/yyyy");
                RateLineInfo.SetValue(todate, i);
            }
            else
            {
                RateLineInfo.SetValue("", i);
            }

            i++;

            RateLineInfo.SetValue(ddlStatus.SelectedValue, i);

            i++;
            //added by jayant
            RateLineInfo.SetValue(ckhAllIn.Checked, i);
            i++;
            RateLineInfo.SetValue(ckhTACTRate.Checked, i);
            i++;
            RateLineInfo.SetValue(ckhULDRate.Checked, i);
            i++;
            RateLineInfo.SetValue(ckhHeavyRate.Checked, i);
            i++;
            RateLineInfo.SetValue(DdlRateCardName.SelectedIndex, i);
            i++;
            RateLineInfo.SetValue(txtParam.Text.Trim(),i);
            i++;
            RateLineInfo.SetValue(TXTAgentCode.Text.Trim(), i);
            i++;
            RateLineInfo.SetValue(TXTShipperCode.Text.Trim(), i);
            i++;
            RateLineInfo.SetValue(TXTIATAComCode.Text.Trim(), i);
            i++;
            RateLineInfo.SetValue(TXTProductType.Text.Trim(), i);
            i++;

            //Expires Dates
            if (txtExpfrm.Text.Trim() != "")
            {
                DateTime dtfrom;

                try
                {
                   // dtfrom = Convert.ToDateTime(txtExpfrm.Text);
                    dtfrom = DateTime.ParseExact(txtExpfrm.Text, "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                    lblStatus.Text = "Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                //RateLineInfo.SetValue(Convert.ToDateTime(txtExpfrm.Text).ToString("yyyy-MM-dd HH:mm:ss"), i);
                string Frmdate = dtfrom.ToString("MM/dd/yyyy");
                RateLineInfo.SetValue(Frmdate, i);

            }
            else
            {
                RateLineInfo.SetValue("", i);
            }



            i++;

            if (txtExpTo.Text.Trim() != "")
            {
                DateTime dtto;

                try
                {
                    //dtto = Convert.ToDateTime(txtExpTo.Text);
                    dtto = DateTime.ParseExact(txtExpTo.Text, "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                    lblStatus.Text = "Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                //RateLineInfo.SetValue(Convert.ToDateTime(txtExpTo.Text).ToString("yyyy-MM-dd HH:mm:ss"), i);
                string Todate = dtto.ToString("MM/dd/yyyy");
                RateLineInfo.SetValue(Todate, i);

            }
            else
            {
                RateLineInfo.SetValue("", i);
            }

            i++;

            RateLineInfo.SetValue(txtRateID.Text.Trim(), i);
            i++;

            //if (ddlRateType.SelectedIndex == 0)
            //{
            //    RateLineInfo.SetValue("", i);

            //}
            //else
            //    if (ddlRateType.SelectedIndex == 1)
            //    {
            //        RateLineInfo.SetValue("DOM", i);

            //    }
            //    else
            //        RateLineInfo.SetValue("INT", i);

            RateLineInfo.SetValue(ddlRateType.SelectedValue, i);
            #endregion Prepare Parameters


            DataSet ds = objBAL.GetListRateLine(RateLineInfo);
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            grdListStock.DataSource = ds;
                            grdListStock.DataMember = ds.Tables[0].TableName;
                            grdListStock.DataBind();
                            grdListStock.Visible = true;
                           
                            //grdListStock.Columns[0].Visible = false;
                            //btnClear_Click(null, null);
                             //values = new String[this.grdListStock.Rows.Count];
                            //for (int index = 0; index < this.grdListStock.Rows.Count; index++)
                            //    values[index] = this.grdListStock.Rows[index].Cells[0].Text;

                            
                            //Session["RateLIneExp"] = values; 
                            Session["LstRateline"] = ds;
                            lblStatus.Text = "";
                            lblUpdateStatus.Text = "";
                        }
                        else
                        {
                            lblStatus.Text = "No record availabe for given Search criteria";
                            lblStatus.ForeColor = Color.Red;
                            grdListStock.DataMember = null;
                            grdListStock.DataBind();
                            grdListStock.Visible = false;
                            lblUpdateStatus.Text = "";
                        }
                    }
                    else 
                    {
                        lblStatus.Text = "No record availabe for given Search criteria";
                        lblStatus.ForeColor = Color.Red;
                        grdListStock.Visible = false;
                        lblUpdateStatus.Text = "";
                    }
                }
                else
                {
                    lblStatus.Text = "No record availabe for given Search criteria";
                    lblStatus.ForeColor = Color.Red;
                    lblUpdateStatus.Text = "";
                   
                }
            }
            else
            {
                lblStatus.Text = "No record availabe for given Search criteria";
                lblStatus.ForeColor = Color.Red;
                lblUpdateStatus.Text = "";
            }
        }
        # endregion btnList_Click

        #region grid row commmand "Edit" and "View"
        protected void grdListStock_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit" || e.CommandName == "View")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = grdListStock.Rows[index];
                string RateCardId = ((LinkButton)grdListStock.Rows[index].FindControl("lblSrNo")).Text; //row.Cells[0].Text;
                Response.Redirect("MaintainRates.aspx?cmd=" + e.CommandName + "&RCName=" + RateCardId);
            }
        }
        #endregion grid row commmand "Edit" and "View"

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ListRateLine.aspx", false);
        }

        protected void DDLOriginLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListDataViewBAL objDataViewBAL = new ListDataViewBAL();
            DataSet dsResult = new DataSet();
            string errormessage = "";

            string level = DDLOriginLevel.SelectedItem.Value;
            if (objDataViewBAL.FillDdl("Origin", ref dsResult, ref errormessage, new string[] { level }))
            {              
                ddlOrigin.DataSource = dsResult;
                ddlOrigin.DataMember = dsResult.Tables[0].TableName;
                ddlOrigin.DataValueField = dsResult.Tables[0].Columns[0].ColumnName;
                ddlOrigin.DataTextField = dsResult.Tables[0].Columns[0].ColumnName;
                ddlOrigin.DataBind();
                ddlOrigin.Items.Insert(0, new ListItem("Select", string.Empty));
            }
                        
        }

        protected void DDLDestinationLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListDataViewBAL objDataViewBAL = new ListDataViewBAL();
            DataSet dsResult = new DataSet();
            string errormessage = "";

            string level = DDLDestinationLevel.SelectedItem.Value;
            if (objDataViewBAL.FillDdl("Origin", ref dsResult, ref errormessage, new string[] { level }))
            {
                ddlDestination.DataSource = dsResult;
                ddlDestination.DataMember = dsResult.Tables[0].TableName;
                ddlDestination.DataValueField = dsResult.Tables[0].Columns[0].ColumnName;
                ddlDestination.DataTextField = dsResult.Tables[0].Columns[0].ColumnName;
                ddlDestination.DataBind();
                ddlDestination.Items.Insert(0, new ListItem("Select", string.Empty));                
            }

        }
        #region Get Data to Export

        public string GetData()
        {
            string values = "";
            lblStatus.Text = "";
            #region Prepare Parameters
            string AllIn = "", TACT = "";
            object[] RateLineInfo = new object[21];
            int i = 0;

            RateLineInfo.SetValue(DDLOriginLevel.SelectedIndex, i);
            i++;

            RateLineInfo.SetValue(DDLDestinationLevel.SelectedIndex, i);
            i++;

            RateLineInfo.SetValue(ddlOrigin.SelectedValue, i);

            i++;
            RateLineInfo.SetValue(ddlDestination.SelectedValue, i);

            if (txtFromDate.Text.Trim() == "")
            {
                if (txtToDate.Text.Trim() != "")
                {
                    lblStatus.Text = "Please enter From date";
                    lblStatus.ForeColor = Color.Blue;
                    return null;
                }
            }

            if (txtToDate.Text.Trim() == "")
            {
                if (txtFromDate.Text.Trim() != "")
                {
                    lblStatus.Text = "Please enter To date";
                    lblStatus.ForeColor = Color.Blue;
                    return null;
                }
            }


            i++;
            if (txtFromDate.Text.Trim() != "")
            {
                DateTime dtfrom;

                try
                {
                    //dtfrom = Convert.ToDateTime(txtFromDate.Text);
                    dtfrom = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                    lblStatus.Text = "Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return null;
                }

                //RateLineInfo.SetValue(Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd HH:mm:ss"), i);
                string fromdate = dtfrom.ToString("MM/dd/yyyy");
                RateLineInfo.SetValue(fromdate, i);
            }
            else
            {
                RateLineInfo.SetValue("", i);
            }



            i++;

            if (txtToDate.Text.Trim() != "")
            {
                DateTime dtto;

                try
                {
                   // dtto = Convert.ToDateTime(txtToDate.Text);
                    dtto = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                    lblStatus.Text = "Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return null;
                }

                //RateLineInfo.SetValue(Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd HH:mm:ss"), i);
                string todate = dtto.ToString("MM/dd/yyyy");
                RateLineInfo.SetValue(todate, i);

            }
            else
            {
                RateLineInfo.SetValue("", i);
            }

            i++;

            RateLineInfo.SetValue(ddlStatus.SelectedValue, i);

            i++;
            //added by jayant
            RateLineInfo.SetValue(ckhAllIn.Checked, i);
            i++;
            RateLineInfo.SetValue(ckhTACTRate.Checked, i);
            i++;
            RateLineInfo.SetValue(ckhULDRate.Checked, i);
            i++;
            RateLineInfo.SetValue(ckhHeavyRate.Checked, i);
            i++;
            RateLineInfo.SetValue(DdlRateCardName.SelectedIndex, i);
            i++;
            RateLineInfo.SetValue(txtParam.Text.Trim(), i);
            i++;
            RateLineInfo.SetValue(TXTAgentCode.Text.Trim(), i);
            i++;
            RateLineInfo.SetValue(TXTShipperCode.Text.Trim(), i);
            i++;
            RateLineInfo.SetValue(TXTIATAComCode.Text.Trim(), i);
            i++;
            RateLineInfo.SetValue(TXTProductType.Text.Trim(), i);
            i++;

            //Expires Dates
            if (txtExpfrm.Text.Trim() != "")
            {
                DateTime dtfrom;

                try
                {
                    //dtfrom = Convert.ToDateTime(txtExpfrm.Text);
                    dtfrom = DateTime.ParseExact(txtExpfrm.Text, "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                    lblStatus.Text = "Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return null;
                }

                //RateLineInfo.SetValue(Convert.ToDateTime(txtExpfrm.Text).ToString("yyyy-MM-dd HH:mm:ss"), i);
                string Frmdate = dtfrom.ToString("MM/dd/yyyy");
                RateLineInfo.SetValue(Frmdate, i);

            }
            else
            {
                RateLineInfo.SetValue("", i);
            }



            i++;

            if (txtExpTo.Text.Trim() != "")
            {
                DateTime dtto;

                try
                {
                    //dtto = Convert.ToDateTime(txtExpTo.Text);
                    dtto = DateTime.ParseExact(txtExpTo.Text, "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                    lblStatus.Text = "Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return null;
                }

                //RateLineInfo.SetValue(Convert.ToDateTime(txtExpTo.Text).ToString("yyyy-MM-dd HH:mm:ss"), i);
                string Todate = dtto.ToString("MM/dd/yyyy");
                RateLineInfo.SetValue(Todate, i);

            }
            else
            {
                RateLineInfo.SetValue("", i);
            }

            i++;

            RateLineInfo.SetValue(txtRateID.Text.Trim(), i);
            i++;
            //if (ddlRateType.SelectedIndex == 0)
            //{
            //    RateLineInfo.SetValue("Dom", i);

            //}
            //else
            //    if (ddlRateType.SelectedIndex == 1)
            //    {
            //        RateLineInfo.SetValue("INT", i);

            //    }
            //    else
            //        RateLineInfo.SetValue("", i);

            RateLineInfo.SetValue(ddlRateType.SelectedValue, i);
            #endregion Prepare Parameters


            DataSet ds = objBAL.GetListRateLine(RateLineInfo);
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                           for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                           {
                            values = values + (string)(ds.Tables[0].Rows[j][14].ToString()) + ",";
                           }
                            
                        }
                        else
                        {
                            lblStatus.Text = "No record available for given Search criteria";
                            lblStatus.ForeColor = Color.Red;
                            grdListStock.DataMember = null;
                            grdListStock.DataBind();
                        }
                    }
                    else
                    {
                        lblStatus.Text = "No record available for given Search criteria";
                        lblStatus.ForeColor = Color.Red;
                    }
                }
                else
                {
                    lblStatus.Text = "No record available for given Search criteria";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            else
            {
                lblStatus.Text = "No record available for given Search criteria";
                lblStatus.ForeColor = Color.Red;
            }
            if(!string.IsNullOrEmpty(values))
                return values;
            else 
                return null;
        }
        #endregion

        public void GetExportData(string val)
        {
         DataSet ds = objBAL.getExpRateLine(val);
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //grdListStock.DataSource = ds;
                            //grdListStock.DataMember = ds.Tables[0].TableName;
                            //grdListStock.DataBind();
                            //grdListStock.Visible = true;
                            //grdListStock.Columns[0].Visible = false;
                            //btnClear_Click(null, null);
                            Session["Rateline"] = ds;
                            lblStatus.Text = "";
                        }
                        else
                        {
                            lblStatus.Text = "No record availabe for given Search criteria";
                            lblStatus.ForeColor = Color.Red;
                            grdListStock.DataMember = null;
                            grdListStock.DataBind();
                            Session["Rateline"] = null;
                        }
                    }
                    else
                    {
                        lblStatus.Text = "No record available for given Search criteria";
                        lblStatus.ForeColor = Color.Red;
                    }
                }
                else
                {
                    lblStatus.Text = "No record available for given Search criteria";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            else
            {
                lblStatus.Text = "No record available for given Search criteria";
                lblStatus.ForeColor = Color.Red;
            }
        }

        protected void btnExport_Click(object sender, EventArgs e) 
        {
            string values="";
            DataSet ds = null;
            DataTable dt = null;
            //Session["Rateline"] = null;
            try
            {
               values=GetData();
               if (!string.IsNullOrEmpty(values))
               {

                   GetExportData(values);

                   ds = (DataSet)Session["Rateline"];
                   dt = (DataTable)ds.Tables[0];
                   if (ds.Tables[0].Rows.Count > 0)
                   {
                       string attachment = "attachment; filename=RateLines.xls";
                       Response.ClearContent();
                       Response.ContentEncoding = System.Text.Encoding.UTF8;
                       Response.AddHeader("content-disposition", attachment);
                       Response.ContentType = "application/vnd.ms-excel";
                       string tab = "";
                       foreach (DataColumn dc in dt.Columns)
                       {
                           Response.Write(tab + dc.ColumnName);
                           //tab = "\t";
                           tab = "\t";
                       }
                       Response.Write("\n");
                       int i;
                       foreach (DataRow dr in dt.Rows)
                       {
                           tab = "";
                           for (i = 0; i < dt.Columns.Count; i++)
                           {
                               if (dr[i].ToString().Trim() == ",")
                                   dr[i] = "";
                            
                               Response.Write(tab + dr[i].ToString());
                               //tab = "\t";
                               tab = "\t";
                           }
                           Response.Write("\n");
                       }
                       Response.End();

                   }
                   else
                   {
                       lblStatus.Text = "No record found";
                       lblStatus.ForeColor = Color.Red;
                       Session["Rateline"] = null;
                   }
               }
               else
               {
                   lblStatus.Text = "No record found";
                   lblStatus.ForeColor = Color.Red;
                   Session["Rateline"] = null;

               }

                }

            catch (Exception ex)
            {

            }
            finally
            {
                ds = null;
                Session["Rateline"] = null;
                dt = null;
            }


        }
        #region updateRateLine
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string sr_no = "";
            DateTime dtfrm, dtTo;
            string[] param = new string[3];
            SqlDbType[] dbtypes = new SqlDbType[3];
            object[] values = new object[3];
            if (string.IsNullOrEmpty(txtUpdtFromDate.Text.ToString()) && string.IsNullOrEmpty(txtUpdtToDate.Text.ToString()))
            {
                lblUpdateStatus.Visible = true;
                lblUpdateStatus.Text = "Please enter Valid From To date";
                lblUpdateStatus.ForeColor = Color.Red;
                return;
            }
            if (!string.IsNullOrEmpty(txtUpdtFromDate.Text.ToString()))
            {
                try
                {
                    dtfrm = DateTime.ParseExact(txtUpdtFromDate.Text, "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    lblUpdateStatus.Visible = true;
                    lblUpdateStatus.Text = "Date format error";
                    return;
                }
            }
            else
            {
                lblUpdateStatus.Visible = true;
                lblUpdateStatus.Text = "Please enter Valid From date";
                lblUpdateStatus.ForeColor = Color.Red;
                return;
            }

            if (!string.IsNullOrEmpty(txtUpdtToDate.Text.ToString()))
            {
                try
                {
                    dtTo = DateTime.ParseExact(txtUpdtToDate.Text, "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    lblUpdateStatus.Visible = true;
                    lblUpdateStatus.Text = "Date format error";
                    return;
                }
            }
            else
            {
                lblUpdateStatus.Visible = true;
                lblUpdateStatus.Text = "Please enter Valid To date";
                lblUpdateStatus.ForeColor = Color.Red;
                return;
            }
            //if (grdListStock.DataSource != null)
            //{
                DataSet ds = (DataSet)Session["LstRateline"];
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    CheckBox chkal = (CheckBox)grdListStock.HeaderRow.FindControl("checkAll");
                    if (!chkal.Checked)
                    {
                        for (int i = 0; i < grdListStock.Rows.Count; i++)
                        {
                            CheckBox chkBox = (CheckBox)grdListStock.Rows[i].FindControl("chkUpdate");
                            if (chkBox.Checked)
                            {
                                LinkButton lbSrNo = new LinkButton();
                                lbSrNo = (LinkButton)grdListStock.Rows[i].FindControl("lblSrNo");
                                sr_no = sr_no + lbSrNo.Text + ",";
                            }

                        }
                    }
                    else
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            // CheckBox chkBox  = (CheckBox)grdListStock.Rows[i].FindControl("chkUpdate");
                            //
                            //LinkButton lbSrNo = new LinkButton();
                            //lbSrNo = (LinkButton)grdListStock.Rows[i].FindControl("lblSrNo");
                            sr_no = sr_no + ds.Tables[0].Rows[i]["SerialNumber"].ToString() + ",";
                            //sr_no = sr_no + lbSrNo.Text + ",";


                        }
                    }
                }
            
            else
            {
                lblUpdateStatus.Visible = true;
                lblUpdateStatus.Text = "Rateline updation failed";
                lblUpdateStatus.ForeColor = Color.Red;
                return;
            }
            if (sr_no == null || sr_no == "")
            {
                lblUpdateStatus.Visible = true;
                lblUpdateStatus.Text = "Please select rate id to update";
                lblUpdateStatus.ForeColor = Color.Red;
            }
            else
            {

                values[0] = sr_no;
                values[1] = dtfrm;
                values[2] = dtTo;

                bool val = objBAL.UpdateRateLine(values);
                if (val == true)
                {
                    lblUpdateStatus.Visible = true;
                    lblUpdateStatus.Text = "Rateline Updated Successfully";
                    lblUpdateStatus.ForeColor = Color.Green;
                }
                else
                {
                    lblUpdateStatus.Visible = true;
                    lblUpdateStatus.Text = "Rateline updation failed";
                    lblUpdateStatus.ForeColor = Color.Red;
                    return;
                }
                #region For Master Audit Log
                MasterAuditBAL ObjMAL = new MasterAuditBAL();
                #region Prepare Parameters
                object[] Params = new object[7];
                int j = 0;

                //1
                Params.SetValue("Rate Line", j);
                j++;

                //2
                string Value = txtUpdtFromDate.Text + "-" + txtUpdtToDate.Text;
                Params.SetValue(Value, j);
                j++;

                //3

                Params.SetValue("UPDATE", j);
                j++;

                //4

                Params.SetValue("", j);
                j++;


                //5

                Params.SetValue("Updation of Valid from and Valid to dates", j);
                j++;

                //6

                Params.SetValue(Session["UserName"], j);
                j++;

                //7
                Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), j);
                j++;


                #endregion Prepare Parameters
                ObjMAL.AddMasterAuditLog(Params);
                #endregion For Master Audit Log
                //code to update dates for specified srNo's

            }
        }
        #endregion updateRateLine
        protected void checkAll_CheckedChanged(object sender, EventArgs e)
        {
           

        }
  
        protected void grdListStock_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dsResult = (DataSet)Session["LstRateline"];

            grdListStock.PageIndex = e.NewPageIndex;
            grdListStock.DataSource = dsResult.Copy();
            grdListStock.DataBind();
           
        }
    }
}
