using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using BAL;
using QID.DataAccess;

namespace ProjectSmartCargoManager
{
    public partial class TaxLineList : System.Web.UI.Page
    {

        ListOtherChargesBAL objBAL = new ListOtherChargesBAL();
        SQLServer da = new SQLServer(Global.GetConnectionString());

        # region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                StnList();
                
                //FillGetListOC_ALL();
                //LoadChargeType();
                //LoadCurrency();

                //string agentCode = Convert.ToString(Session["ACode"]);
                //if (agentCode != "")
                //{
                //    GRDList.Columns[14].Visible = false;
                //}
                //txtFromDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                //txtToDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                HomeBL objHome = new HomeBL();
                int RoleId = Convert.ToInt32(Session["RoleID"]);
                DataSet objDS = objHome.GetUserPermissions(((System.Web.UI.TemplateControl)(Page)).AppRelativeVirtualPath, RoleId);
                objHome = null;

                if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < objDS.Tables[0].Rows.Count; i++)
                    {
                        if (objDS.Tables[0].Rows[i]["ControlId"].ToString() == "OtherChargeEdit")
                        {
                            GRDList.Columns[14].Visible = false;
                            break;
                        }
                    }
                }
                objDS = null;

                #region Define PageSize for grid as per configuration
                try
                {
                    LoginBL objConfig = new LoginBL();
                    GRDList.PageSize = Convert.ToInt32(objConfig.GetMasterConfiguration("StdListPageSize"));
                    objConfig = null;
                }
                catch (Exception ex)
                { }
                #endregion
            }
        }
        # endregion Page_Load

        # region FillGetListOC_ALL
        public void FillGetListOC_ALL()
        {
            try
            {
                DataSet ds = objBAL.GetListOC_ALL();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            GRDList.DataSource = ds;
                            GRDList.DataMember = ds.Tables[0].TableName;
                            GRDList.DataBind();
                            Session["OtherCharges"] = ds;
                        }
                    }
                }
            }

            catch
            {

            }
        }
        # endregion FillGetListRateLine_ALL

        private void StnList()
        {
            try
            {
                DataSet ds = da.SelectRecords("spGetAirportCodes");
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            ddlOrigin.DataSource = ds;
                            ddlOrigin.DataMember = ds.Tables[0].TableName;
                            ddlOrigin.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            ddlOrigin.DataTextField = ds.Tables[0].Columns[1].ColumnName;
                            ddlOrigin.DataBind();
                            ddlOrigin.Items.Insert(0, new ListItem("Select", string.Empty));

                            ddlDestination.DataSource = ds;
                            ddlDestination.DataMember = ds.Tables[0].TableName;
                            ddlDestination.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            ddlDestination.DataTextField = ds.Tables[0].Columns[1].ColumnName;
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

        # region btnList_Click
          protected void btnList_Click(object sender, EventArgs e)
        {
            
            #region Prepare Parameters
            
            object[] RateLineInfo = new object[11];
            string[] ColumnNames = new string[11];
            SqlDbType[] DataType = new SqlDbType[11];
            
            int i = 0;

            //0
            RateLineInfo.SetValue(ddlOrigin.SelectedValue, i);
            i++;

            //1
            RateLineInfo.SetValue(ddlDestination.SelectedValue, i);
            i++;

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
            
            //2
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

            //3
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

            //4
            RateLineInfo.SetValue(ddlStatus.SelectedValue, i);
            i++;

            //5
            RateLineInfo.SetValue("", i);
            i++;


            //6
            RateLineInfo.SetValue(txtChargeName.Text, i);
            i++;

            //7
            RateLineInfo.SetValue(txtParam.Text, i);
            i++;

            RateLineInfo.SetValue(txtTaxID.Text, i);
            i++;
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

        

         


            int j = 0;
            ColumnNames.SetValue("Origin", j);
            DataType.SetValue(SqlDbType.VarChar, j);
            j++;


            ColumnNames.SetValue("Destination", j);
            DataType.SetValue(SqlDbType.VarChar, j);
            j++;


            ColumnNames.SetValue("FromDate", j);
            DataType.SetValue(SqlDbType.VarChar, j);
            j++;


            ColumnNames.SetValue("ToDate", j);
            DataType.SetValue(SqlDbType.VarChar, j);
            j++;


            ColumnNames.SetValue("Status", j);
            DataType.SetValue(SqlDbType.VarChar, j);
            j++;

            ColumnNames.SetValue("ChargeCode", j);
            DataType.SetValue(SqlDbType.VarChar, j);
            j++;

            ColumnNames.SetValue("ChargeName", j);
            DataType.SetValue(SqlDbType.VarChar, j);
            j++;

            ColumnNames.SetValue("Parameter", j);
            DataType.SetValue(SqlDbType.VarChar, j);
            j++;
            ColumnNames.SetValue("TaxID", j);
            DataType.SetValue(SqlDbType.VarChar, j);
            j++;

            ColumnNames.SetValue("ExpiresFrom", j);
            DataType.SetValue(SqlDbType.VarChar, j);
            j++;
            ColumnNames.SetValue("ExpiresTo", j);
            DataType.SetValue(SqlDbType.VarChar, j);
            j++;
          

            #endregion

            DataSet ds = da.SelectRecords("SP_GetTaxLineFilterd", ColumnNames, RateLineInfo, DataType);
            if (ds != null)
            {
                if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                {

                    GRDList.DataSource = ds;
                    GRDList.DataMember = ds.Tables[0].TableName;
                    GRDList.DataBind();
                    Session["TaxLineList"] = ds;
                    lblUpdateStatus.Text = string.Empty;
                    lblStatus.Text = string.Empty;
                }
                else
                {
                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = Color.Red;
                    GRDList.DataSource = null;
                    GRDList.DataBind();
                    lblUpdateStatus.Text = string.Empty;
                }
            }
        }
        # endregion btnList_Click
          #region UpdateTax
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
            DataSet ds = (DataSet)Session["TaxLineList"];
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                CheckBox chkal = (CheckBox)GRDList.HeaderRow.FindControl("checkAll");
                if (!chkal.Checked)
                {
                    for (int i = 0; i < GRDList.Rows.Count; i++)
                    {
                        CheckBox chkBox = (CheckBox)GRDList.Rows[i].FindControl("chkUpdate");
                        if (chkBox.Checked)
                        {
                            LinkButton lbSrNo = new LinkButton();
                            lbSrNo = (LinkButton)GRDList.Rows[i].FindControl("lblSrNo");
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
                lblUpdateStatus.Text = "Tax line updation failed";
                lblUpdateStatus.ForeColor = Color.Red;
                return;
            }
            if (sr_no == null || sr_no == "")
            {
                lblUpdateStatus.Visible = true;
                lblUpdateStatus.Text = "Please select Tax id to update";
                lblUpdateStatus.ForeColor = Color.Red;
            }
            else
            {

                string[] ColumnNames = new string[3];
                SqlDbType[] DataType = new SqlDbType[3];
                //Object[] Values = new object[3];
                int i = 0;
                    ColumnNames.SetValue("SrNo", i);
                    DataType.SetValue(SqlDbType.VarChar, i);
                    values[i] = sr_no;
                    i++;

                    ColumnNames.SetValue("FrmDate", i);
                    DataType.SetValue(SqlDbType.DateTime, i);
                    values[i] = dtfrm;
                    i++;

                    ColumnNames.SetValue("ToDate", i);
                    DataType.SetValue(SqlDbType.DateTime, i);
                    values[i] = dtTo;
                    i++;

                    bool val = da.UpdateData("sp_updateTaxConfigMaster", ColumnNames, DataType, values);
                if (val == true)
                {
                    lblUpdateStatus.Visible = true;
                    lblUpdateStatus.Text = "Tax line Updated Successfully";
                    lblUpdateStatus.ForeColor = Color.Green;
                }
                else
                {
                    lblUpdateStatus.Visible = true;
                    lblUpdateStatus.Text = "Tax line updation failed";
                    lblUpdateStatus.ForeColor = Color.Red;
                    return;
                }
                #region For Master Audit Log
                MasterAuditBAL ObjMAL = new MasterAuditBAL();
                #region Prepare Parameters
                object[] Params = new object[7];
                int j = 0;

                //1
                Params.SetValue("Tax line", j);
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
          #endregion UpdateTax
        #region Get Data to Export

        public void GetData()
          {

              #region Prepare Parameters

              object[] RateLineInfo = new object[11];
              string[] ColumnNames = new string[11];
              SqlDbType[] DataType = new SqlDbType[11];

              int i = 0;

              //0
              RateLineInfo.SetValue(ddlOrigin.SelectedValue, i);
              i++;

              //1
              RateLineInfo.SetValue(ddlDestination.SelectedValue, i);
              i++;

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

              //2
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

              //3
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

              //4
              RateLineInfo.SetValue(ddlStatus.SelectedValue, i);
              i++;

              //5
              RateLineInfo.SetValue("", i);
              i++;


              //6
              RateLineInfo.SetValue(txtChargeName.Text, i);
              i++;

              //7
              RateLineInfo.SetValue(txtParam.Text, i);
              i++;

              RateLineInfo.SetValue(txtTaxID.Text, i);
              i++;
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






              int j = 0;
              ColumnNames.SetValue("Origin", j);
              DataType.SetValue(SqlDbType.VarChar, j);
              j++;


              ColumnNames.SetValue("Destination", j);
              DataType.SetValue(SqlDbType.VarChar, j);
              j++;


              ColumnNames.SetValue("FromDate", j);
              DataType.SetValue(SqlDbType.VarChar, j);
              j++;


              ColumnNames.SetValue("ToDate", j);
              DataType.SetValue(SqlDbType.VarChar, j);
              j++;


              ColumnNames.SetValue("Status", j);
              DataType.SetValue(SqlDbType.VarChar, j);
              j++;

              ColumnNames.SetValue("ChargeCode", j);
              DataType.SetValue(SqlDbType.VarChar, j);
              j++;

              ColumnNames.SetValue("ChargeName", j);
              DataType.SetValue(SqlDbType.VarChar, j);
              j++;

              ColumnNames.SetValue("Parameter", j);
              DataType.SetValue(SqlDbType.VarChar, j);
              j++;
              ColumnNames.SetValue("TaxID", j);
              DataType.SetValue(SqlDbType.VarChar, j);
              j++;

              ColumnNames.SetValue("ExpiresFrom", j);
              DataType.SetValue(SqlDbType.VarChar, j);
              j++;
              ColumnNames.SetValue("ExpiresTo", j);
              DataType.SetValue(SqlDbType.VarChar, j);
              j++;


              #endregion

              DataSet ds = da.SelectRecords("SP_GetTaxLineFilterd", ColumnNames, RateLineInfo, DataType);
              if (ds != null)
              {
                  if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                  {

                      //GRDList.DataSource = ds;
                      //GRDList.DataMember = ds.Tables[0].TableName;
                      //GRDList.DataBind();
                      Session["TaxLineList"] = ds;
                      lblStatus.Text = string.Empty;
                  }
                  else
                  {
                      lblStatus.Text = "No Records Found";
                      lblStatus.ForeColor = Color.Red;
                      //GRDList.DataSource = null;
                      //GRDList.DataBind();
                  }
              }
          }
          #endregion


          #region Grid Row Commmand "Edit" and "View"
          protected void GRDList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Edit" || e.CommandName == "View")
                {
                    string SrNo = ((HiddenField)GRDList.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("Hid")).Value;
                    Response.Redirect("~/TaxLine.aspx?cmd=" + e.CommandName + "&SrNo=" + SrNo + "");
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = "" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }
        #endregion

        protected void GRDList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dsResult = (DataSet)Session["OtherCharges"];

            GRDList.PageIndex = e.NewPageIndex;
            GRDList.DataSource = dsResult.Copy();
            GRDList.DataBind();

        }

        #region DDLDestinationLevel_SelectedIndexChanged
        protected void DDLDestinationLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            SQLServer da=new SQLServer(Global.GetConnectionString());
            ListDataViewBAL objDataViewBAL = new ListDataViewBAL();
            DataSet dsResult = new DataSet();
            string errormessage = "";


            if (DDLDestinationLevel.SelectedValue == "A")
            {
                DataSet ds = da.SelectRecords("spGetAirportCodes");
                ddlDestination.DataSource = ds;
                ddlDestination.DataMember = ds.Tables[0].TableName;
                ddlDestination.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                ddlDestination.DataTextField = ds.Tables[0].Columns[1].ColumnName;
                ddlDestination.DataBind();
                ddlDestination.Items.Insert(0, new ListItem("Select", string.Empty));
            }
            else if (DDLDestinationLevel.SelectedValue == "N")
            {
                DataSet ds = da.SelectRecords("SP_GetAllStationCodeName", "level", "country", SqlDbType.VarChar);
                ddlDestination.DataSource = ds;
                ddlDestination.DataMember = ds.Tables[0].TableName;
                ddlDestination.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                ddlDestination.DataTextField = ds.Tables[0].Columns[2].ColumnName;
                ddlDestination.DataBind();
                ddlDestination.Items.Insert(0, new ListItem("Select", string.Empty));
            }

            else
            {
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
        }
        #endregion

        #region DDLOriginLevel_SelectedIndexChanged
        protected void DDLOriginLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListDataViewBAL objDataViewBAL = new ListDataViewBAL();
            DataSet dsResult = new DataSet();
            string errormessage = "";

            if (DDLOriginLevel.SelectedValue == "A")
            {
                DataSet ds = da.SelectRecords("spGetAirportCodes");
                ddlOrigin.DataSource = ds;
                ddlOrigin.DataMember = ds.Tables[0].TableName;
                ddlOrigin.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                ddlOrigin.DataTextField = ds.Tables[0].Columns[1].ColumnName;
                ddlOrigin.DataBind();
                ddlOrigin.Items.Insert(0, new ListItem("Select", string.Empty));
            }
            else if (DDLOriginLevel.SelectedValue == "N")
            {
                DataSet ds = da.SelectRecords("SP_GetAllStationCodeName", "level", "country", SqlDbType.VarChar);
                ddlOrigin.DataSource = ds;
                ddlOrigin.DataMember = ds.Tables[0].TableName;
                ddlOrigin.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                ddlOrigin.DataTextField = ds.Tables[0].Columns[2].ColumnName;
                ddlOrigin.DataBind();
                ddlOrigin.Items.Insert(0, new ListItem("Select", string.Empty));
            }
            else
            {
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
        }
        #endregion

        //#region ChargeType
        //private void LoadChargeType()
        //{
        //    try
        //    {
        //        OtherChargesBAL objBALL = new OtherChargesBAL();
        //        DataSet oth = objBALL.GetChargeType();
        //        if (oth != null)
        //        {
        //            if (oth.Tables.Count > 0)
        //            {
        //                DDLCharge.DataSource = oth.Tables[0];
        //                DDLCharge.DataTextField = "ChargeText";
        //                DDLCharge.DataValueField = "ChargeValue";
        //                DDLCharge.DataBind();
        //                DDLCharge.Items.Insert(0, new ListItem("Select", string.Empty));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    { }
        //}
        //#endregion

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            DataTable dt = null;
            Session["TaxLineList"] = null;
            try
            {
                //if ((DataSet)Session["OtherCharges"] == null)
                //    return;
                GetData();

                if (Session["TaxLineList"] == null)
                {
                    lblStatus.Text = "No Record Found";
                    return;
                }

                ds = (DataSet)Session["TaxLineList"];
                dt = (DataTable)ds.Tables[0];

               
                //ExportToExcel(dt, "RateLines.xls");
                //dt = city.GetAllCity();//your datatable 
                string attachment = "attachment; filename=TaxLineList.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                string tab = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    Response.Write(tab + dc.ColumnName);
                    tab = "\t";
                }
                Response.Write("\n");
                int i;
                foreach (DataRow dr in dt.Rows)
                {
                    tab = "";
                    for (i = 0; i < dt.Columns.Count; i++)
                    {
                        Response.Write(tab + dr[i].ToString());
                        tab = "\t";
                    }
                    Response.Write("\n");
                }
                Response.End();
            }
            catch (Exception ex)
            { }
            finally
            {
                ds = null;
                dt = null;
            }

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/TaxLineList.aspx", false);
        }

        //#region Currency
        //private void LoadCurrency()
        //{
        //    try
        //    {
        //        //OtherChargesBAL objBALL = new OtherChargesBAL();
        //        DataSet dsoth = new DataSet();
        //        dsoth = objBAL.GetCurrencyCode(ddlCurrency.SelectedItem);
        //        if (dsoth != null)
        //        {
        //            if (dsoth.Tables.Count > 0)
        //            {
        //                DDLCharge.DataSource = dsoth.Tables[0];
        //                DDLCharge.DataTextField = "Currency";
        //                DDLCharge.DataValueField = "CurrencyValue";
        //                DDLCharge.DataBind();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    { }
        //}
        //#endregion

    }
}
