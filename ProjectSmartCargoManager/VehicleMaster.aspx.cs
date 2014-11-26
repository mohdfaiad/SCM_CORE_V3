using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QID.DataAccess;
using System.Data;
using System.Drawing;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class VehicleMaster : System.Web.UI.Page
    {
        #region Variable
        SQLServer da = new SQLServer(Global.GetConnectionString());
        
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetDropDownData();
                Session["dsListData"] = null;
                Session["dtEmpty"] = null;
                AddEmptyRow();
               
            }
        }

        protected void GetDropDownData()
        {
            DataSet dsDropDown = new DataSet();
            try
            {
                dsDropDown = da.SelectRecords("sp_GetVehicleMasterDropDown");
                if (dsDropDown != null)
                {
                    Session["dsDropDown"] = dsDropDown;

                    ddlStn.Items.Clear();
                    ddlStn.DataSource = dsDropDown.Tables[0];
                    ddlStn.DataTextField = "AirportCode";
                    ddlStn.DataValueField = "Code";
                    ddlStn.DataBind();
                    ddlStn.Items.Insert(0, new ListItem("Select","All"));

                    ddlVehicleType.Items.Clear();
                    ddlVehicleType.DataSource = dsDropDown.Tables[1];
                    ddlVehicleType.DataTextField = "VehTypeCode";
                    ddlVehicleType.DataValueField = "Code";
                    ddlVehicleType.DataBind();
                    ddlVehicleType.Items.Insert(0, new ListItem("Select", "All"));

                    ddlPartnerCode.Items.Clear();
                    ddlPartnerCode.DataSource = dsDropDown.Tables[2];
                    ddlPartnerCode.DataTextField = "PartnerCode";
                    ddlPartnerCode.DataValueField = "Code";
                    ddlPartnerCode.DataBind();
                    ddlPartnerCode.Items.Insert(0, new ListItem("Select", "All"));
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                if (dsDropDown != null)
                    dsDropDown.Dispose();
            }
                    
        }

        protected void AddEmptyRow()
        {
            DataTable dtMainData = new DataTable();
            DataTable dtEmpty = new DataTable();
            try
            {
                if (Session["dsListData"] == null)
                {
                    if (Session["dtEmpty"] == null)
                    {
                        dtEmpty.Columns.Add("SrNo");
                        dtEmpty.Columns.Add("VehNo");
                        dtEmpty.Columns.Add("DriverCode");
                        dtEmpty.Columns.Add("ChassisNum");
                        dtEmpty.Columns.Add("Manufacturer");
                        dtEmpty.Columns.Add("VehicleCapacity");
                        dtEmpty.Columns.Add("Latitude");
                        dtEmpty.Columns.Add("Longitude");

                        DataRow dr = dtEmpty.NewRow();
                        dr["SrNo"] = "0";
                        dr["VehNo"] = string.Empty;
                        dr["DriverCode"] = string.Empty;
                        dr["ChassisNum"] = string.Empty;
                        dr["Manufacturer"] = string.Empty;
                        dr["VehicleCapacity"] = string.Empty;
                        dr["Latitude"] = string.Empty;
                        dr["Longitude"] = string.Empty;
                        dtEmpty.Rows.Add(dr);
                        grdVehicleDetails.DataSource = dtEmpty;
                        grdVehicleDetails.DataBind();
                        Session["dtEmpty"] = dtEmpty;
                        BindGridDropDown(dtEmpty);
                    }
                    else
                    {
                        DataTable dtNewRowTable = (DataTable)Session["dtEmpty"];
                        DataRow dr = dtNewRowTable.NewRow();
                        dr["SrNo"] = "0";
                        dr["VehNo"] = string.Empty;
                        dr["ChassisNum"] = string.Empty;
                        dr["Manufacturer"] = string.Empty;
                        dr["VehicleCapacity"] = string.Empty;
                        dr["Latitude"] = string.Empty;
                        dr["Longitude"] = string.Empty;
                        dtNewRowTable.Rows.Add(dr);
                        grdVehicleDetails.DataSource = dtNewRowTable;
                        grdVehicleDetails.DataBind();
                        Session["dtEmpty"] = dtNewRowTable;
                        BindGridDropDown(dtNewRowTable);
                    }
                    
                }
                else
                {
                    dtMainData = (DataTable)Session["dsListData"];
                    DataRow dr = dtMainData.NewRow();
                    dr["SrNo"] = "0";
                    dr["VehNo"] = string.Empty;
                    dr["ChassisNum"] = string.Empty;
                    dr["Manufacturer"] = string.Empty;
                    dr["VehicleCapacity"] = string.Empty;
                    dr["Latitude"] = string.Empty;
                    dr["Longitude"] = string.Empty;
                    dtMainData.Rows.Add(dr);
                    
                    grdVehicleDetails.DataSource = dtMainData;
                    grdVehicleDetails.DataBind();

                    BindGridDropDown(dtMainData);
                }
            }
            catch (Exception ex)
            { }
            finally { }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            DataSet dsData = new DataSet();
            try
            {
                lblStatus.Text = string.Empty;

                #region Parameters
                object[] paramValue = new object[5];
                int i = 0;
                
                //0
                paramValue.SetValue(txtVehicleNo.Text.Trim(), i);
                i++;

                //1
                paramValue.SetValue(ddlVehicleType.SelectedValue, i);
                i++;

                //2
                paramValue.SetValue(txtDriverCode.Text.Trim(), i);
                i++;

                //3
                paramValue.SetValue(ddlPartnerCode.SelectedValue, i);
                i++;

                //4
                paramValue.SetValue(ddlStn.SelectedValue, i);
                i++;

                string[] ParamNames = { "VehNumber", "VehicleType", "DriverCode", "PartnerCode", "StationCode" };
                SqlDbType[] ParamTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };

                #endregion

                dsData = da.SelectRecords("spGetVehicleDetails", ParamNames, paramValue, ParamTypes);

                if (dsData != null && dsData.Tables[0].Rows.Count > 0)
                {
                    grdVehicleDetails.DataSource = dsData.Tables[0];
                    grdVehicleDetails.DataBind();
                    
                    txtDriverCode.Text = txtVehicleNo.Text = string.Empty;
                    ddlPartnerCode.SelectedIndex = 0;
                    ddlStn.SelectedIndex = 0;
                    ddlVehicleType.SelectedIndex = 0;

                    Session["dsListData"] = dsData.Tables[0];
                    BindGridDropDown(dsData.Tables[0]);
                }
                else
                {
                    lblStatus.Text = "Data not found";
                    lblStatus.ForeColor = Color.Red;
                    Session["dsListData"] = null;
                    grdVehicleDetails.DataSource = null;
                    grdVehicleDetails.DataBind();
                    AddEmptyRow();
                    return;
                }
                    
            }   
            catch (Exception ex)
            { }
            finally
            {

            }
        }

        protected void BindGridDropDown(DataTable dtVal)
        {
            DataSet dsDropDown = new DataSet();
            dsDropDown = (DataSet)Session["dsDropDown"];
            try
            {
                for (int i = 0; i < grdVehicleDetails.Rows.Count; i++)
                {
                    DropDownList ddlGridVehicleType = (DropDownList)grdVehicleDetails.Rows[i].FindControl("ddlGVehicleType");
                    DropDownList ddlGridStnCode = (DropDownList)grdVehicleDetails.Rows[i].FindControl("ddlGStnCode");
                    DropDownList ddlGridPartnerCode = (DropDownList)grdVehicleDetails.Rows[i].FindControl("ddlGPartnerCode");


                    ddlGridStnCode.Items.Clear();
                    ddlGridStnCode.DataSource = dsDropDown.Tables[0];
                    ddlGridStnCode.DataTextField = "AirportCode";
                    ddlGridStnCode.DataValueField = "Code";
                    ddlGridStnCode.DataBind();
                    ddlGridStnCode.Items.Insert(0, new ListItem("Select", "All"));


                    ddlGridVehicleType.Items.Clear();
                    ddlGridVehicleType.DataSource = dsDropDown.Tables[1];
                    ddlGridVehicleType.DataTextField = "VehTypeCode";
                    ddlGridVehicleType.DataValueField = "Code";
                    ddlGridVehicleType.DataBind();
                    ddlGridVehicleType.Items.Insert(0, new ListItem("Select", "All"));

                    ddlGridPartnerCode.Items.Clear();
                    ddlGridPartnerCode.DataSource = dsDropDown.Tables[2];
                    ddlGridPartnerCode.DataTextField = "PartnerCode";
                    ddlGridPartnerCode.DataValueField = "Code";
                    ddlGridPartnerCode.DataBind();
                    ddlGridPartnerCode.Items.Insert(0, new ListItem("Select", "All"));

                    if (Session["dsListData"] != null)
                    {
                        ddlGridPartnerCode.SelectedIndex = ddlGridPartnerCode.Items.IndexOf(ddlGridPartnerCode.Items.FindByValue(dtVal.Rows[i]["PartnerCode"].ToString()));
                        ddlGridVehicleType.SelectedIndex = ddlGridVehicleType.Items.IndexOf(ddlGridVehicleType.Items.FindByValue(dtVal.Rows[i]["VehType"].ToString()));
                        ddlGridStnCode.SelectedIndex = ddlGridStnCode.Items.IndexOf(ddlGridStnCode.Items.FindByValue(dtVal.Rows[i]["StationCode"].ToString()));
                        if (dtVal.Rows[i]["IsActive"].ToString().ToUpper() == "TRUE")
                            ((CheckBox)grdVehicleDetails.Rows[i].FindControl("chkIsAct")).Checked = true;
                    }

                }
            }
            catch (Exception ex)
            { }
            finally
            {
                if (dsDropDown != null)
                    dsDropDown.Dispose();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            AddEmptyRow();
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = string.Empty;
                int count = 0;
                bool DelRec = false;

                for (int i = 0; i < grdVehicleDetails.Rows.Count; i++)
                {
                    if (((CheckBox)grdVehicleDetails.Rows[i].FindControl("chkRecord")).Checked)
                    { count++; }
                }
                if (count == 0)
                {
                    lblStatus.Text = "Select one row";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                for (int i = 0; i < grdVehicleDetails.Rows.Count; i++)
                {
                    if (((CheckBox)grdVehicleDetails.Rows[i].FindControl("chkRecord")).Checked)
                    {
                        if (((Label)grdVehicleDetails.Rows[i].FindControl("lblSrNo")).Text.Trim() != "0")
                        {
                            //count++;
                            int srNo = Convert.ToInt32(((Label)grdVehicleDetails.Rows[i].FindControl("lblSrNo")).Text.Trim());

                            DelRec = da.ExecuteProcedure("spDeleteVehicleDetails", "SrNo", SqlDbType.BigInt, srNo);
                           
                        }
                    }
                }
                btnList_Click(null, null);

                if (DelRec)
                {
                    lblStatus.Text = "Record Deleted Successfully";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            { }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = string.Empty;
                int count = 0;
                bool result = false;

                for (int i = 0; i < grdVehicleDetails.Rows.Count; i++)
                {
                    if (((CheckBox)grdVehicleDetails.Rows[i].FindControl("chkRecord")).Checked)
                    {
                        count++;
                    }
                }
                if (count == 0)
                {
                    lblStatus.Text = "Select atleast one record";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                for (int i = 0; i < grdVehicleDetails.Rows.Count; i++)
                {
                    if (((CheckBox)grdVehicleDetails.Rows[i].FindControl("chkRecord")).Checked)
                    {
                        //count++;

                        #region Parameters

                        object[] ParamValue = new object[14];

                        ParamValue[0] = Convert.ToInt32(((Label)grdVehicleDetails.Rows[i].FindControl("lblSrNo")).Text.Trim());

                        ParamValue[1] = ((TextBox)grdVehicleDetails.Rows[i].FindControl("txtVehicleNo")).Text.Trim();

                        if (((DropDownList)grdVehicleDetails.Rows[i].FindControl("ddlGVehicleType")).SelectedIndex == 0)
                            ParamValue[2] = "";
                        else
                            ParamValue[2] = ((DropDownList)grdVehicleDetails.Rows[i].FindControl("ddlGVehicleType")).SelectedValue.ToString();

                        ParamValue[3] = ((TextBox)grdVehicleDetails.Rows[i].FindControl("txtDriverCode")).Text.Trim();

                        if (((DropDownList)grdVehicleDetails.Rows[i].FindControl("ddlGPartnerCode")).SelectedIndex == 0)
                            ParamValue[4] = "";
                        else
                            ParamValue[4] = ((DropDownList)grdVehicleDetails.Rows[i].FindControl("ddlGPartnerCode")).SelectedValue.ToString();

                        ParamValue[5] = ((TextBox)grdVehicleDetails.Rows[i].FindControl("txtChassisNo")).Text.Trim();

                        ParamValue[6] = ((TextBox)grdVehicleDetails.Rows[i].FindControl("txtManufacturer")).Text.Trim();

                        float VeHCapacity = 0;
                        if (((TextBox)grdVehicleDetails.Rows[i].FindControl("txtVehCapacity")).Text.Trim() == "")
                            ParamValue[7] = "";
                        else
                        {
                            if (float.TryParse(((TextBox)grdVehicleDetails.Rows[i].FindControl("txtVehCapacity")).Text.Trim(), out VeHCapacity))
                            {
                                ParamValue[7] = VeHCapacity;
                            }
                            else
                            {
                                lblStatus.Text = "Enter digits only";
                                lblStatus.ForeColor = Color.Red;
                                return;
                            }
                        }

                        ParamValue[8] = ((CheckBox)grdVehicleDetails.Rows[i].FindControl("chkRecord")).Checked;

                        if (((DropDownList)grdVehicleDetails.Rows[i].FindControl("ddlGStnCode")).SelectedIndex == 0)
                            ParamValue[9] = "";
                        else
                            ParamValue[9] = ((DropDownList)grdVehicleDetails.Rows[i].FindControl("ddlGStnCode")).SelectedValue.ToString();

                        float Latitude = 0;
                        if (((TextBox)grdVehicleDetails.Rows[i].FindControl("txtLatitude")).Text.Trim() == "")
                            ParamValue[10] = "";
                        else
                        {
                            if (float.TryParse(((TextBox)grdVehicleDetails.Rows[i].FindControl("txtLatitude")).Text.Trim(), out Latitude))
                            {
                                ParamValue[10] = Latitude.ToString();
                            }
                            else
                            {
                                lblStatus.Text = "Enter digits only";
                                lblStatus.ForeColor = Color.Red;
                                return;
                            }
                        }

                        float Longitude = 0;
                        if (((TextBox)grdVehicleDetails.Rows[i].FindControl("txtLongitude")).Text.Trim() == "")
                            ParamValue[11] = "";
                        else
                        {
                            if (float.TryParse(((TextBox)grdVehicleDetails.Rows[i].FindControl("txtLongitude")).Text.Trim(), out Longitude))
                            {
                                ParamValue[11] = Longitude;
                            }
                            else
                            {
                                lblStatus.Text = "Enter digits only";
                                lblStatus.ForeColor = Color.Red;
                                return;
                            }
                        }

                        ParamValue[12] = Session["UserName"].ToString();

                        ParamValue[13] = Convert.ToDateTime(Session["IT"].ToString());


                        string[] ParamName = { "VehicleID", "VehNumber", "VehicleType", "DriverCode", "PartnerCode", "ChassisNum", "Manufacturer", "VehicleCapacity", "IsActive", "StationCode", "Latitude", "Longitude", "UpdatedBy", "UpdatedOn" };
                        SqlDbType[] ParamType = { SqlDbType.BigInt, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Bit, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime };

                        #endregion

                        result = da.ExecuteProcedure("spSaveVehicleDetails", ParamName, ParamType, ParamValue);

                        if (result)
                        {
                            #region for Master Audit Log
                            MasterAuditBAL ObjMAL = new MasterAuditBAL();
                            #region Prepare Parameters
                            object[] Params = new object[7];
                            int k = 0;

                            //1
                            Params.SetValue("VehicleMaster", k);
                            k++;

                            //2
                            Params.SetValue(txtVehicleNo.Text.Trim(), k);
                            k++;

                            //3
                            Params.SetValue("ADD", k);
                            k++;

                            //4
                            Params.SetValue("Vehicle Record Added", k);
                            k++;

                            //5
                            string desc = "Vehicle No:" + txtVehicleNo.Text.Trim();
                            Params.SetValue(desc, k);
                            k++;

                            //6
                            Params.SetValue(Session["UserName"], k);
                            k++;

                            //7
                            Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), k);
                            k++;


                            #endregion Prepare Parameters
                            ObjMAL.AddMasterAuditLog(Params);
                            #endregion
                        }
                    }
                }
                btnList_Click(null, null);
                if (result)
                {
                    lblStatus.Text = "Record added Successfully";
                    lblStatus.ForeColor = Color.Green;
                }
            }
            catch (Exception ex)
            { }
            finally { }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtDriverCode.Text = txtVehicleNo.Text = string.Empty;
            lblStatus.Text = string.Empty;
            ddlPartnerCode.SelectedIndex = 0;
            ddlStn.SelectedIndex = 0;
            ddlVehicleType.SelectedIndex = 0;

            grdVehicleDetails.DataSource = null;
            grdVehicleDetails.DataBind();

            Session["dsListData"] = null;
            Session["dtEmpty"] = null;
            
            AddEmptyRow();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = null;
            DataTable dt = null;
            try
            {
                if (Session["dsListData"] == null)
                {
                    btnList_Click(null, null);
                    AddEmptyRow();
                }
                lblStatus.Text = string.Empty;

                dsExp = (DataSet)Session["dsListData"];
                if (dsExp != null && dsExp.Tables[0].Rows.Count > 0)
                {
                    dt = (DataTable)dsExp.Tables[0];

                    string attachment = "attachment; filename=VehicleMaster.xls";
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
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No records found for the selected search criteria!');</SCRIPT>", false);
                    return;
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                if (dsExp != null)
                    dsExp = null;
                if (dt != null)
                    dt = null;
            }
        }
    }
}
