using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class SpecialHandlingCodeMaster : System.Web.UI.Page
    {
        SpecialHandlingCodeMasterBAL objBAL = new SpecialHandlingCodeMasterBAL();
        LoginBL lBal = new LoginBL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["dsSHC"] = null;
               // LoadGridUserList();

                bool scroll = Convert.ToBoolean(lBal.GetMasterConfiguration("KnownShipper"));
                if (scroll == false)
                {
                    chkShipper.Visible = false;
                    lblShipper.Visible = false;
                }
                else
                {
                    chkShipper.Visible = true;
                    lblShipper.Visible = true;
                }

                #region Define PageSize for grid as per configuration
                try
                {
                    LoginBL objConfig = new LoginBL();
                    grvSpecHandlingCodeList.PageSize = Convert.ToInt32(objConfig.GetMasterConfiguration("StdListPageSize"));
                    objConfig = null;
                }
                catch (Exception ex)
                { }
                #endregion
            }
        }
        
        #region Add New Row to Grid
        public void LoadGridUserList()
        {

            DataTable myDataTable = new DataTable();
            DataColumn myDataColumn;
            DataSet Ds = new DataSet();

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "SerialNumber";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "SpecialHandelingCode";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Description";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "IsActive";
            myDataTable.Columns.Add(myDataColumn);


            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "IsShipper";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "isNoToc";
            myDataTable.Columns.Add(myDataColumn);


            DataRow dr = myDataTable.NewRow();
            //dr["SerialNumber"] = "";
            //dr["SpecialHandelingCode"] = "";
            //dr["Description"] = "";
            //dr["IsActive"] = "";
            //dr["Date"] = "";
            //dr["HolidayType"] = "";
            //dr["DateFrom"] = "";
            //dr["DateTo"] = "";
            //dr["FreightRate"] = "";
            myDataTable.Rows.Add(dr);
            
            grvSpecHandlingCodeList.DataSource = myDataTable;
            grvSpecHandlingCodeList.DataBind();
           //grvSpecHandlingCodeList.Columns[5].Visible = false;
            //Session["dsdata"] = myDataTable.Copy();

        }
        #endregion

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSpecialHandlingCode.Text = string.Empty;
            txtSpecialHandlingCodeDescription.Text = string.Empty;
            chkActive.Checked =chkShipper.Checked= false;
            lblStatus.Text = string.Empty;
            Session["dsSHC"] = null;
            //LoadGridUserList();
            grvSpecHandlingCodeList.DataSource = null;
            grvSpecHandlingCodeList.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                
                # region Save
                if (btnSave.Text == "Save")
                {
                    if (!ValidateFields())
                        return;
                    try
                    {
                        if (txtSpecialHandlingCode.Text == "")
                        {
                            lblStatus.Text = "Please Enter SpecialHandling Code";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                        if (txtSpecialHandlingCodeDescription.Text == "")
                        {
                            lblStatus.Text = "Please Enter Special Handling Code Description";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }



                        #region Prepare Parameters
                        object[] SpecHandlingcodeInfo = new object[1];
                        int j = 0;

                        //0
                        SpecHandlingcodeInfo.SetValue(txtSpecialHandlingCode.Text.Trim().ToUpper(), j);
                        j++;

                        ////1
                        //CountrycodeInfo.SetValue(txtSpecialHandlingCodeDescription.Text.Trim(), j);
                        //j++;




                        #endregion Prepare Parameters

                        DataSet ds = new DataSet();
                        ds = objBAL.chkSpecHandlingCodeList(SpecHandlingcodeInfo);


                        if (ds != null)
                        {
                            if (ds.Tables != null)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Text = "Special Handling Code Already Exist ";
                                }
                                else
                                {

                                    #region Prepare Parameters
                                    object[] SpecHandlingInfo = new object[5];
                                    int i = 0;

                                    //0
                                    SpecHandlingInfo.SetValue(txtSpecialHandlingCode.Text.Trim().ToUpper(), i);
                                    i++;

                                    //1
                                    SpecHandlingInfo.SetValue(txtSpecialHandlingCodeDescription.Text.Trim(), i);
                                    i++;


                                    //2
                                    if (chkActive.Checked == true)
                                    {
                                        SpecHandlingInfo.SetValue("true", i);
                                        i++;
                                    }
                                    else
                                    {
                                        SpecHandlingInfo.SetValue("false", i);
                                        i++;

                                    }
                                    if (chkShipper.Checked == true)
                                    {
                                        SpecHandlingInfo.SetValue("true", i);
                                    }
                                    else
                                    {
                                        SpecHandlingInfo.SetValue("false", i);
                                    }

                                    i++;
                                    try
                                    {
                                        if (ddlisnotoc.SelectedItem.Value == "Select")
                                        {
                                            SpecHandlingInfo.SetValue("", i);
                                        }
                                        else if (ddlisnotoc.SelectedItem.Value == "DGR")
                                        {
                                            SpecHandlingInfo.SetValue(ddlisnotoc.SelectedValue.ToString(), i);
                                        }
                                        else
                                        {
                                            SpecHandlingInfo.SetValue(ddlisnotoc.SelectedValue.ToString(), i);

                                        }
                                    }
                                    catch (Exception ex)
                                    { }
                                    #endregion Prepare Parameters

                                    int ID = 0;
                                    ID = objBAL.AddSpecHandlingCode(SpecHandlingInfo);
                                    if (ID >= 0)
                                    {
                                        #region For Master Audit Log
                                        MasterAuditBAL ObjMAL = new MasterAuditBAL();
                                        #region Prepare Parameters
                                        object[] Params = new object[7];
                                        int k = 0;

                                        //1
                                        Params.SetValue("Special Handling Code", k);
                                        k++;

                                        //2
                                        Params.SetValue(txtSpecialHandlingCode.Text, k);
                                        k++;

                                        //3

                                        Params.SetValue("ADD", k);
                                        k++;

                                        //4

                                        Params.SetValue("", k);
                                        k++;


                                        //5

                                        Params.SetValue("", k);
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

                                        txtSpecialHandlingCode.Text = string.Empty;
                                        txtSpecialHandlingCodeDescription.Text = string.Empty;
                                        chkActive.Checked = chkShipper.Checked=false;
                                        btnList_Click(null, null);
                                        lblStatus.ForeColor = Color.Green;
                                        lblStatus.Text = "Special handling Code Added Sucessfully..";
                                        btnSave.Text = "Save";
                                    }
                                    else
                                    {
                                        lblStatus.ForeColor = Color.Red;
                                        lblStatus.Text = "Special handling Code Insertion Failed..";
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                # endregion Save

                # region Update
                if (btnSave.Text == "Update")
                {
                    try
                    {
                        #region Prepare Parameters
                        object[] updateSpecHandlingInfo = new object[6];
                        int i = 0;

                        //0
                        updateSpecHandlingInfo.SetValue(txtSpecialHandlingCode.Text.Trim().ToUpper(), i);
                        i++;

                        //1
                        updateSpecHandlingInfo.SetValue(txtSpecialHandlingCodeDescription.Text.Trim(), i);
                        i++;

                        //2
                        if (chkActive.Checked == true)
                        {
                            updateSpecHandlingInfo.SetValue("True", i);
                            i++;
                        }
                        else
                        {
                            updateSpecHandlingInfo.SetValue("False", i);
                            i++;
                        }

                        //3 
                        string srnum = Session["SrNum"].ToString();
                        updateSpecHandlingInfo.SetValue(srnum, i);
                        i++;

                        if (chkShipper.Checked == true)
                        {
                            updateSpecHandlingInfo.SetValue("true", i);
                        }
                        else
                        {
                            updateSpecHandlingInfo.SetValue("false", i);
                        }
                        i++;
                        try
                        {
                            if (ddlisnotoc.SelectedItem.Value == "Select")
                            {
                                updateSpecHandlingInfo.SetValue("", i);
                            }
                            else if (ddlisnotoc.SelectedItem.Value == "DGR")
                            {
                                updateSpecHandlingInfo.SetValue(ddlisnotoc.SelectedValue.ToString(), i);
                            }
                            else
                            {
                                updateSpecHandlingInfo.SetValue(ddlisnotoc.SelectedValue.ToString(), i);

                            }
                        }
                        catch (Exception ex)
                        { }

                        #endregion Prepare Parameters

                        int UpdateID = 0;
                        UpdateID = objBAL.UpdateSpecHandlingCode(updateSpecHandlingInfo);
                        if (UpdateID >= 0)
                        {
                            #region For Master Audit Log
                            MasterAuditBAL ObjMAL = new MasterAuditBAL();
                            #region Prepare Parameters
                            object[] Params = new object[7];
                            int j = 0;

                            //1
                            Params.SetValue("Special Handling Code", j);
                            j++;

                            //2
                            Params.SetValue(txtSpecialHandlingCode.Text, j);
                            j++;

                            //3

                            Params.SetValue("UPDATE", j);
                            j++;

                            //4

                            Params.SetValue("", j);
                            j++;


                            //5

                            Params.SetValue("", j);
                            j++;

                            //6

                            Params.SetValue(Session["UserName"], j);
                            j++;

                            //7
                            Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), j);
                            j++;


                            #endregion Prepare Parameters
                            ObjMAL.AddMasterAuditLog(Params);
                            #endregion

                            txtSpecialHandlingCode.Text = string.Empty;
                            txtSpecialHandlingCodeDescription.Text = string.Empty;
                            chkActive.Checked =chkShipper.Checked= false;
                            btnList_Click(null, null);
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Text = "Special Handling Code Updated Sucessfully..";
                            btnSave.Text = "Save";
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Special Handling Code Updation Failed..";
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
                # endregion Update
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {

            try
                {
                    lblStatus.Text = string.Empty;

                    #region Prepare Parameters
                    object[] SpecHandlingListInfo = new object[4];
                    int i = 0;

                    //0
                    SpecHandlingListInfo.SetValue(txtSpecialHandlingCode.Text.Trim(), i);
                    i++;

                    //1
                    SpecHandlingListInfo.SetValue(txtSpecialHandlingCodeDescription.Text.Trim(), i);
                    i++;

                    //2
                    if (chkActive.Checked == true)
                    {
                        SpecHandlingListInfo.SetValue("true", i);
                    }
                    else
                    {
                        SpecHandlingListInfo.SetValue("false", i);
                    }
                //3
                    i++;
                    try
                    {
                        if (ddlisnotoc.SelectedItem.Value == "Select")
                        {
                            SpecHandlingListInfo.SetValue("", i);
                        }
                        else if (ddlisnotoc.SelectedItem.Value == "DGR")
                        {
                            SpecHandlingListInfo.SetValue(ddlisnotoc.SelectedValue.ToString(), i);
                        }
                        else
                        {
                            SpecHandlingListInfo.SetValue(ddlisnotoc.SelectedValue.ToString(), i);

                        }  
                    }
                    catch (Exception ex)
                    { }

                    #endregion Prepare Parameters

                    DataSet ds = new DataSet();
                    ds = objBAL.spGetSpecialHandlllingCodeList(SpecHandlingListInfo);
                    if (ds != null)
                    {
                        if (ds.Tables != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    grvSpecHandlingCodeList.PageIndex = 0;
                                    grvSpecHandlingCodeList.DataSource = ds;
                                    grvSpecHandlingCodeList.DataMember = ds.Tables[0].TableName;
                                    grvSpecHandlingCodeList.DataBind();
                                    grvSpecHandlingCodeList.Visible = true;
                                    grvSpecHandlingCodeList.Columns[4].Visible = true;
                                    Session["dsSHC"] = ds;
                                    grvSpecHandlingCodeList.Columns[5].Visible = true;
                                    grvSpecHandlingCodeList.Columns[6].Visible = true;
                                }
                                else
                                {
                                    lblStatus.Text = "No records found...";
                                    lblStatus.ForeColor = Color.Red;
                                    LoadGridUserList();
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                }


        }

        # region grvSpecHandlingCodeList_RowCommand
        protected void grvSpecHandlingCodeList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Edit")
                {
                    lblStatus.Text = "";
                    int RowIndex = Convert.ToInt32(e.CommandArgument);
                    Label lblSpecHandlingCode = (Label)grvSpecHandlingCodeList.Rows[RowIndex].FindControl("lblSpecHandlingCode");
                    Label lblDescription = (Label)grvSpecHandlingCodeList.Rows[RowIndex].FindControl("lblDescription");
                    Label lblstat = (Label)grvSpecHandlingCodeList.Rows[RowIndex].FindControl("lblStatus");
                    Session["SrNum"] = ((Label)grvSpecHandlingCodeList.Rows[RowIndex].FindControl("lblSrno")).Text;
                    Label lblShipper = (Label)grvSpecHandlingCodeList.Rows[RowIndex].FindControl("lblShipper");

                    try
                    {
                        Label isnotoc = ((Label)grvSpecHandlingCodeList.Rows[RowIndex].FindControl("lbldgr"));
                        if (isnotoc.Text == string.Empty || isnotoc.Text == "")
                            ddlisnotoc.SelectedItem.Text = "Select";
                        else
                            ddlisnotoc.SelectedValue = ((Label)grvSpecHandlingCodeList.Rows[RowIndex].FindControl("lbldgr")).Text;

                    }
                    catch (Exception ex)
                    { 
                    
                    }

                    txtSpecialHandlingCode.Text = lblSpecHandlingCode.Text;
                    txtSpecialHandlingCodeDescription.Text = lblDescription.Text;
                    

                    if (lblstat.Text == "Active")
                    {
                        chkActive.Checked = true;
                    }

                    else
                    {
                        chkActive.Checked = false;
                    }

                    if (lblShipper.Text == "Active")
                    {
                      chkShipper.Checked = true;
                    }

                    else
                    {
                        chkShipper.Checked = false;
                    }

                    btnSave.Text = "Update";

                }
            }
            catch (Exception ex)
            {

            }

        }
        # endregion grvSpecHandlingCodeList_RowCommand

        # region grvSpecHandlingCodeList_RowEditing
        protected void grvSpecHandlingCodeList_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        # endregion grvSpecHandlingCodeList_RowEditing

        # region grvSpecHandlingCodeList_PageIndexChanging
        protected void grvSpecHandlingCodeList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet ds = (DataSet)Session["dsSHC"];
            grvSpecHandlingCodeList.PageIndex = e.NewPageIndex;
            grvSpecHandlingCodeList.DataSource = ds.Tables[0];
            grvSpecHandlingCodeList.DataBind();
            grvSpecHandlingCodeList.Columns[4].Visible = true;
        }
        # endregion grvSpecHandlingCodeList_PageIndexChanging

        #region newMethod
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string[] GetSHCCode(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string");
            //SELECT FlightID from dbo.AirlineSchedule where FlightID like '"+ prefixText +"%' or FlightID like '"+ prefixText +"%'

            //SqlDataAdapter dad = new SqlDataAdapter("SELECT AirportName  +'   ('+ AirportCode +')' as AirportCode from AirportMaster where AirportName like '" + prefixText + "%' or AirportCode like '" + prefixText + "%'", con);
            SqlDataAdapter dad = new SqlDataAdapter("SELECT SpecialHandelingCode as Code from tblSpecialHandlingCodeMaster where SpecialHandelingCode like '" + prefixText + "%'", con);
            DataSet ds = new DataSet();
            dad.Fill(ds);
            List<string> list = new List<string>(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(dr[0].ToString());

            }

            return list.ToArray();
        }

        #endregion

        public bool ValidateFields()
        {
            try
            {
                bool IsDuplicate = false;
                string errormessage = "";


                object[] values = { txtSpecialHandlingCode.Text
                                  };

                if (objBAL.CheckDuplicate(values, ref IsDuplicate, ref errormessage))
                {
                    if (IsDuplicate)
                    {
                        lblStatus.Text = "SHC Code already exists.";
                        lblStatus.ForeColor = Color.Red;
                        return false;
                    }
                }
                else
                {
                    lblStatus.Text = "" + errormessage;
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
