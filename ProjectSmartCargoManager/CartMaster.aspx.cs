using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using QID.DataAccess;
using System.Drawing;

namespace ProjectSmartCargoManager
{
    public partial class CartMaster : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStation();
                BindEmptyRow();
                Session["dsCartMaster"] = null;
                //btnUpdate.Visible = false;
            }
        }

        protected void BindEmptyRow()
        {DataTable dtEmpty=new DataTable();
            try
            {
                dtEmpty.Columns.Add("SerialNumber");
                dtEmpty.Columns.Add("CartNumber");
                dtEmpty.Columns.Add("CartDescription");
                dtEmpty.Columns.Add("Station");
                dtEmpty.Columns.Add("Location");
                dtEmpty.Columns.Add("Isactive",typeof(bool));

                DataRow dr = dtEmpty.NewRow();
                dr["SerialNumber"] = "";
                dr["CartNumber"] = "";
                dr["CartDescription"] = "";
                dr["Station"] = "";
                dr["Location"] = "";
                dr["Isactive"] = false;
                dtEmpty.Rows.Add(dr);

                grdCartList.DataSource = dtEmpty;
                grdCartList.DataBind();

                grdCartList.Columns[6].Visible = false;

            }
            catch (Exception ex)
            { }
            finally
            {
                if (dtEmpty != null)
                    dtEmpty.Dispose();
            }
        }

        protected void LoadStation()
        {
            DataSet dsStn = new DataSet();
            try
            {
                dsStn=da.SelectRecords("spGetAirportCodes");

                if (dsStn != null && dsStn.Tables[0].Rows.Count > 0)
                {
                    ddlStnCode.DataValueField = "AirportCode";
                    ddlStnCode.DataTextField = "Airport";
                    ddlStnCode.DataSource = dsStn.Tables[0];
                    ddlStnCode.DataBind();
                    ddlStnCode.Items.Insert(0, new ListItem("Select", "Select"));
                }
            }
            catch (Exception ex)
            { }
            finally { }
            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            lblStatus.Text = string.Empty;
            try
            {
                if (txtCartNo.Text == string.Empty)
                {
                    lblStatus.Text = "Enter Cart Number";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (btnSave.Text == "Save")
                {
                    #region Save

                    #region parameters
                    object[] ParamValue = new object[7];
                    string[] ParamName = { "CartNumber", "CartDescription", "Station", "Location", "Isactive", "UpdatedBy", "UpdatedOn" };
                    SqlDbType[] ParamType = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Bit, SqlDbType.VarChar, SqlDbType.DateTime };

                    int i = 0;

                    //0
                    ParamValue.SetValue(txtCartNo.Text.Trim(), i);
                    i++;

                    //1
                    ParamValue.SetValue(txtCartDesc.Text.Trim(), i);
                    i++;

                    //2
                    if (ddlStnCode.SelectedIndex == 0)
                        ParamValue.SetValue("", i);
                    else
                        ParamValue.SetValue(ddlStnCode.SelectedValue.ToString(), i);
                    i++;

                    //3
                    ParamValue.SetValue(txtLocation.Text.Trim(), i);
                    i++;

                    //4
                    ParamValue.SetValue(chkActive.Checked, i);
                    i++;

                    //5
                    ParamValue.SetValue(Session["UserName"].ToString(), i);
                    i++;

                    //6
                    DateTime UpdatedOn = Convert.ToDateTime(Session["IT"].ToString());
                    ParamValue.SetValue(UpdatedOn, i);

                    #endregion

                    ds = da.SelectRecords("SPSaveCartNumber", ParamName, ParamValue, ParamType);

                    if (ds.Tables[0].Rows[0][0].ToString().ToUpper() == "SAVE")
                    {
                        txtCartNo.Text = string.Empty;
                        txtCartDesc.Text = string.Empty;
                        ddlStnCode.SelectedIndex = 0;
                        txtLocation.Text = string.Empty;
                        chkActive.Checked = false;
                        btnSave.Text = "Save";
                        lblStatus.Text = string.Empty;

                        lblStatus.Text = "Cart Added Successfully";
                        lblStatus.ForeColor = Color.Green;

                        btnList_Click(null, null);
                    }
                    else
                    {
                        lblStatus.Text = "Cart Already Exists";
                        lblStatus.ForeColor = Color.Green;
                    }
                    #endregion
                }
                else
                {
                    #region Update

                    #region parameters
                    object[] ParamValue = new object[8];
                    string[] ParamName = { "CartNumber", "CartDescription", "Station", "Location", "Isactive", "UpdatedBy", "UpdatedOn","SrNo" };
                    SqlDbType[] ParamType = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Bit, SqlDbType.VarChar, SqlDbType.DateTime,SqlDbType.Int };

                    int i = 0;

                    //0
                    ParamValue.SetValue(txtCartNo.Text.Trim(), i);
                    i++;

                    //1
                    ParamValue.SetValue(txtCartDesc.Text.Trim(), i);
                    i++;

                    //2
                    if (ddlStnCode.SelectedIndex == 0)
                        ParamValue.SetValue("", i);
                    else
                        ParamValue.SetValue(ddlStnCode.SelectedValue.ToString(), i);
                    i++;

                    //3
                    ParamValue.SetValue(txtLocation.Text.Trim(), i);
                    i++;

                    //4
                    ParamValue.SetValue(chkActive.Checked, i);
                    i++;

                    //5
                    ParamValue.SetValue(Session["UserName"].ToString(), i);
                    i++;

                    //6
                    DateTime UpdatedOn = Convert.ToDateTime(Session["IT"].ToString());
                    ParamValue.SetValue(UpdatedOn, i);
                    i++;

                    //7
                    int srno=int.Parse(ViewState["SrNo"].ToString());
                    ParamValue.SetValue(int.Parse(ViewState["SrNo"].ToString()), i);

                    #endregion

                    bool res = da.ExecuteProcedure("SPUpdateCartNumber", ParamName, ParamType, ParamValue);

                    if (res)
                    {
                        txtCartNo.Text = string.Empty;
                        txtCartDesc.Text = string.Empty;
                        ddlStnCode.SelectedIndex = 0;
                        txtLocation.Text = string.Empty;
                        chkActive.Checked = false;
                        btnSave.Text = "Save";
                        lblStatus.Text = string.Empty;

                        lblStatus.Text = "Cart Updated Successfully";
                        lblStatus.ForeColor = Color.Green;
                        
                        btnList_Click(null, null);
                    }
                    else
                    {
                        lblStatus.Text = "Cart could not be updated";
                        lblStatus.ForeColor = Color.Green;
                    }
                    #endregion
                }
                
            }
            catch (Exception ex)
            { }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtCartNo.Text = string.Empty;
            txtCartDesc.Text = string.Empty;
            ddlStnCode.SelectedIndex = 0;
            txtLocation.Text = string.Empty;
            chkActive.Checked = false;
            btnSave.Text = "Save";
            lblStatus.Text = string.Empty;
            BindEmptyRow();
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            DataSet dsData = new DataSet();
            try
            {
                #region parameters
                object[] ParamValue = new object[3];
                string[] ParamName = { "CartNumber", "Station", "Flag" };
                SqlDbType[] ParamType = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar};

                int i = 0;

                //0
                ParamValue.SetValue(txtCartNo.Text.Trim(), i);
                i++;

                //1
                if (ddlStnCode.SelectedIndex == 0)
                    ParamValue.SetValue("", i);
                else
                ParamValue.SetValue(ddlStnCode.SelectedValue.ToString(), i);
                i++;

                //2
                ParamValue.SetValue("L", i);
                i++;

                #endregion

                dsData = da.SelectRecords("SPGetCartDetails", ParamName, ParamValue, ParamType);
                if (dsData != null && dsData.Tables[0].Rows.Count > 0)
                {
                    grdCartList.DataSource = dsData.Tables[0];
                    grdCartList.DataBind();
                    Session["dsCartMaster"] = dsData;
                    //btnUpdate.Visible = true;
                    grdCartList.Columns[6].Visible = true;
                }
                else
                {
                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = Color.Red;
                    Session["dsCartMaster"] = null;
                    BindEmptyRow();
                }

            }
            catch (Exception ex)
            { }
            finally { }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = null;
            DataTable dt = null;
            try
            {
                if (Session["dsCartMaster"] == null)
                {
                    btnList_Click(null, null);
                    BindEmptyRow();
                }
                lblStatus.Text = string.Empty;

                dsExp = (DataSet)Session["dsCartMaster"];
                if (dsExp != null && dsExp.Tables[0].Rows.Count > 0)
                {
                    dt = (DataTable)dsExp.Tables[0];

                    if (dt.Columns.Contains("SerialNumber"))
                        dt.Columns.Remove("SerialNumber");

                    string attachment = "attachment; filename=CartMaster.xls";
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

        protected void grdCartList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (DataSet)Session["dsCartMaster"];
                grdCartList.PageIndex = e.NewPageIndex;
                grdCartList.DataSource = ds.Tables[0];
                grdCartList.DataBind();
                
            }
            catch (Exception ex)
            { }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            try
            {
                #region parameters
                object[] ParamValue = new object[3];
                string[] ParamName = { "CartNumber", "Flag", "IsActive" };
                SqlDbType[] ParamType = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Bit };

                for(int j=0;j<grdCartList.Rows.Count;j++)
                {
                    int i = 0;
                //0
                string CartNo = ((Label)grdCartList.Rows[j].FindControl("lblCartNo")).Text.Trim();
                ParamValue.SetValue(CartNo, i);
                i++;

                //1
                ParamValue.SetValue("U", i);
                i++;

                //2
                bool IsAct = ((CheckBox)grdCartList.Rows[j].FindControl("chkIsAct")).Checked;
                ParamValue.SetValue(IsAct, i);
                i++;

                ds = da.SelectRecords("SPGetCartDetails", ParamName, ParamValue, ParamType);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString().ToUpper() == "UPDATE")
                    {
                        lblStatus.Text = "Record Updated";
                        lblStatus.ForeColor = Color.Green;
                    }
                }
                }
                #endregion
            }
            catch (Exception ex)
            { }
            finally
            {
            }
        }

        protected void grdCartList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            
        }

        protected void grdCartList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                lblStatus.Text = string.Empty;

                if (e.CommandName == "Edit")
                {
                    int RowIndex = Convert.ToInt32(e.CommandArgument);
                    
                    ViewState["SrNo"] = Convert.ToInt32(((Label)grdCartList.Rows[RowIndex].FindControl("lblSrNo")).Text);

                    txtCartNo.Text = ((Label)grdCartList.Rows[RowIndex].FindControl("lblCartNo")).Text;
                    txtCartDesc.Text = ((Label)grdCartList.Rows[RowIndex].FindControl("lblCartDesc")).Text;

                    string stn = ((Label)grdCartList.Rows[RowIndex].FindControl("lblStn")).Text;
                    ddlStnCode.SelectedIndex = ddlStnCode.Items.IndexOf(ddlStnCode.Items.FindByValue(stn));

                    txtLocation.Text = ((Label)grdCartList.Rows[RowIndex].FindControl("lblLocation")).Text;

                    chkActive.Checked = ((CheckBox)grdCartList.Rows[RowIndex].FindControl("chkIsAct")).Checked;

                    btnSave.Text = "Update";

                }
            }
            catch (Exception ex)
            { }
        }
    }
}
