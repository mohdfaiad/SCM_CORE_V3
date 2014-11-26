using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using QID.DataAccess;
using BAL;
using System.Drawing;

namespace ProjectSmartCargoManager
{
    public partial class HolidayMaster : System.Web.UI.Page
    {
        string Station, Country, Day, Date, UpdationTime, UpdatedBy;
        DateTime? DayFrom, DayTo,ValidFrom,ValidTo;
        
        #region variable
        BALHoliday objBal = new BALHoliday();
        SQLServer da = new SQLServer(Global.GetConnectionString());
        DataSet ds = new DataSet();
        #endregion variable

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlHolidayType.SelectedIndex = 0;
                ddlCountry.Enabled = ddlStn.Enabled = ddlDay.Enabled = false;
                txtDate.Enabled = txtDayFrm.Enabled = txtDayTo.Enabled = false;
                txtValidFrm.Enabled = txtValidTo.Enabled = false;
                GetAirportCode();
                GetCountryCode();
               // LoadGridUserList();
                //GetList();
            }
        }

        # region Get Airport Code List
        private void GetAirportCode()
        {
            try
            {
                DataSet ds = objBal.GetAirportCodes();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            ddlStn.DataSource = ds;
                            ddlStn.DataMember = ds.Tables[0].TableName;
                            ddlStn.DataValueField = ds.Tables[0].Columns["AirportCode"].ColumnName;

                            ddlStn.DataTextField = ds.Tables[0].Columns["Airport"].ColumnName;
                            ddlStn.DataBind();
                            ddlStn.Items.Insert(0, new ListItem("Select",""));
                       
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        # endregion GetCurrencyCode List

        #region Get Country Code
        private void GetCountryCode()
        {
            try
            {
                DataSet ds = objBal.GetCountryCodes();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlCountry.DataSource = ds;
                            ddlCountry.DataMember = ds.Tables[0].TableName;
                            ddlCountry.DataValueField = ds.Tables[0].Columns["CountryCode"].ColumnName;

                            ddlCountry.DataTextField = ds.Tables[0].Columns["CountryCode"].ColumnName;
                            ddlCountry.DataBind();
                            ddlCountry.Items.Insert(0, new ListItem("Select","Select"));

                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region Add Holidays
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {   
                #region Save
                if(btnSave.Text=="Save"){
                if (ddlHolidayType.SelectedIndex > 0)
                {
                    if (ddlStn.SelectedIndex == 0)
                    { Station = ""; }
                    else Station = ddlStn.SelectedItem.Text;

                    if (ddlCountry.SelectedIndex == 0)
                    { Country = ""; }
                    else Country = ddlCountry.SelectedItem.Text;

                    if (ddlDay.SelectedIndex == 0)
                    { Day = ""; }
                    else Day = ddlDay.SelectedItem.Text;

                    if (txtDate.Text == "")
                    { Date = ""; }
                    else Date = txtDate.Text;

                    if (txtDayFrm.Text == "" && txtDayTo.Text == "")
                    { DayFrom = DayTo = null;  /*DayFrom = DayTo = string.Empty;*/ }
                    else if (txtDayFrm.Text == "" || txtDayTo.Text == "")
                    { StatusLbl.Text = "Select date Range"; 
                        StatusLbl.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message12", "<SCRIPT LANGUAGE='javascript'>select();</script>", false);
                        return;
                    }
                    else
                    {
                        DayFrom = DateTime.Parse(txtDayFrm.Text);
                        DayTo = DateTime.Parse(txtDayTo.Text);
                        //DayFrom = txtDayFrm.Text;
                        //DayTo = txtDayTo.Text;
                    }

                    if (txtValidFrm.Text == "" && txtValidTo.Text == "")
                    { ValidFrom = ValidTo = null;  /*ValidFrom = ValidFrom = string.Empty;*/ }
                    else if (txtValidFrm.Text == "" || txtValidTo.Text == "")
                    {
                        StatusLbl.Text = "Select date Range";
                        StatusLbl.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message12", "<SCRIPT LANGUAGE='javascript'>select();</script>", false);
                        return;
                    }
                    else
                    {
                        ValidFrom = DateTime.Parse(txtValidFrm.Text);
                        ValidTo = DateTime.Parse(txtValidTo.Text);
                        //DayFrom = txtDayFrm.Text;
                        //DayTo = txtDayTo.Text;
                    }
                    UpdationTime = Session["IT"].ToString();
                    UpdatedBy = Session["UserName"].ToString();

                    #region Prepare Parameters
                    object[] Hldy = new object[10];
                    int i = 0;

                    //0
                    Hldy.SetValue(Station, i);
                    i++;

                    //1
                    Hldy.SetValue(Country, i);
                    i++;

                    //2
                    Hldy.SetValue(Day, i);
                    i++;

                    //3
                    //Hldy.SetValue(Date, i);
                    //i++;

                    //4
                    Hldy.SetValue(ddlHolidayType.SelectedItem.Value, i);
                    i++;

                    //5
                    Hldy.SetValue(DayFrom, i);
                    i++;

                    //6
                    Hldy.SetValue(DayTo, i);
                    i++;

                    //7
                    Hldy.SetValue(UpdationTime, i);
                    i++;

                    //8
                    Hldy.SetValue(UpdatedBy, i);
                    i++;

                    //9
                    Hldy.SetValue(ValidFrom, i);
                    i++;

                    //10
                    Hldy.SetValue(ValidTo, i);
                    i++;

                    #endregion Prepare Parameters

                    int ID = 0;
                    ID = objBal.AddHoliday(Hldy);
                    if (ID >= 0)
                    {
                        Clear(); 
                        GetHolidayList();
                        StatusLbl.Text = "Record Added Successfully";
                        StatusLbl.ForeColor = Color.Green;
                    }
                    else
                    {
                        StatusLbl.ForeColor = Color.Red;
                        StatusLbl.Text = "Record Insertion Failed..";
                    }
                }
                else
                {
                    StatusLbl.Text = "Select holiday Type";
                    StatusLbl.ForeColor = Color.Red;
                }
            }
            #endregion

                #region Update
                //if (btnSave.Text == "Update")
                //{
                //    if (ddlHolidayType.SelectedIndex > 0)
                //    {
                //        if (ddlStn.SelectedIndex == 0)
                //        { Station = ""; }
                //        else Station = ddlStn.SelectedItem.Text;

                //        if (ddlCountry.SelectedIndex == 0)
                //        { Country = ""; }
                //        else Country = ddlCountry.SelectedItem.Text;

                //        if (ddlDay.SelectedIndex == 0)
                //        { Day = ""; }
                //        else Day = ddlDay.SelectedItem.Text;

                //        if (txtDate.Text == "")
                //        { Date = ""; }
                //        else Date = txtDate.Text;

                //        if (txtDayFrm.Text == "" && txtDayTo.Text == "")
                //        { DayFrom = DayTo = null;  /*DayFrom = DayTo = string.Empty;*/ }
                //        else if (txtDayFrm.Text == "" || txtDayTo.Text == "")
                //        { StatusLbl.Text = "Select date Range"; StatusLbl.ForeColor = Color.Red; }
                //        else
                //        {
                //            DayFrom = DateTime.Parse(txtDayFrm.Text);
                //            DayTo = DateTime.Parse(txtDayTo.Text);
                //            //DayFrom = txtDayFrm.Text;
                //            //DayTo = txtDayTo.Text;
                //        }
                //        UpdationTime = Session["IT"].ToString();
                //        UpdatedBy = Session["UserName"].ToString();
                //        string srNum = Session["Srno"].ToString();
                //        #region Prepare Parameters
                //        object[] Hldy = new object[10];
                //        int i = 0;

                //        Hldy.SetValue(srNum, i);
                //        i++;

                //        //0
                //        Hldy.SetValue(Station, i);
                //        i++;

                //        //1
                //        Hldy.SetValue(Country, i);
                //        i++;

                //        //2
                //        Hldy.SetValue(Day, i);
                //        i++;

                //        //3
                //        Hldy.SetValue(Date, i);
                //        i++;

                //        //4
                //        Hldy.SetValue(ddlHolidayType.SelectedItem.Value, i);
                //        i++;

                //        //5
                //        Hldy.SetValue(DayFrom, i);
                //        i++;

                //        //6
                //        Hldy.SetValue(DayTo, i);
                //        i++;

                //        //7
                //        Hldy.SetValue(UpdationTime, i);
                //        i++;

                //        //8
                //        Hldy.SetValue(UpdatedBy, i);
                //        i++;


                //        #endregion Prepare Parameters

                //        int ID = 0;
                //        ID = objBal.UpdateHoliday(Hldy);
                //        if (ID >= 0)
                //        {

                //            StatusLbl.Text = "Record Updated Successfully";
                //            StatusLbl.ForeColor = Color.Green;
                //            ddlDay.SelectedIndex = ddlStn.SelectedIndex = ddlCountry.SelectedIndex = ddlHolidayType.SelectedIndex = 0;
                //            txtDate.Text = txtDayFrm.Text = txtDayTo.Text = string.Empty;
                //            btnSave.Text = "Save";
                //            GetList();
                //        }
                //        else
                //        {
                //            StatusLbl.ForeColor = Color.Red;
                //            StatusLbl.Text = "Record Updation Failed..";
                //        }
                //    }
                //    else
                //    {
                //        StatusLbl.Text = "Select holiday Type";
                //        StatusLbl.ForeColor = Color.Red;
                //    }
                //}
                #endregion
            }
                
            
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region btnList_Click
        protected void btnList_Click(object sender, EventArgs e)
        {
            GetHolidayList();
        }
        #endregion btnList_Click

        #region Delete Record
        protected void DeleteRecord()
        {
            int serialnumber = int.Parse(Session["Srno"].ToString());
            try
            {
                
                    #region Prepare Parameters
                    object[] Hldy = new object[1];
                    int i = 0;

                    //0
                    Hldy.SetValue(serialnumber, i);
                    i++;
                  #endregion Prepare Parameters

                    int ID = 0;
                    ID = objBal.DeleteRecord(Hldy);
                    if (ID >= 0)
                    {
                        GetHolidayList();
                        Clear();
                        StatusLbl.Text = "Record Deleted Successfully";
                        StatusLbl.ForeColor = Color.Green;
                       
                    }
                    else
                    {
                        StatusLbl.ForeColor = Color.Red;
                        StatusLbl.Text = "Record Deletion Failed..";
                    }
               
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        # region RowCommand
        protected void GrdHldy_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
            try
            {
                #region Edit
                //if (e.CommandName == "Edit")
                //{
                //    btnSave.Text = "Update";
                //    string StationText = ((Label)GrdHldy.Rows[RowIndex].FindControl("lblStn")).Text;
                //    ddlStn.SelectedIndex = ddlStn.Items.IndexOf((ListItem)ddlStn.Items.FindByText(StationText));

                //    string CountryText = ((Label)GrdHldy.Rows[RowIndex].FindControl("lblCountry")).Text;
                //    ddlCountry.SelectedIndex = ddlCountry.Items.IndexOf((ListItem)ddlCountry.Items.FindByText(CountryText));

                //    string DayText = ((Label)GrdHldy.Rows[RowIndex].FindControl("lblDay")).Text;
                //    ddlDay.SelectedIndex = ddlDay.Items.IndexOf((ListItem)ddlDay.Items.FindByText(DayText));

                //    string HldyTypeText = ((Label)GrdHldy.Rows[RowIndex].FindControl("lblHolidayType")).Text;
                //    ddlHolidayType.SelectedIndex = ddlHolidayType.Items.IndexOf((ListItem)ddlHolidayType.Items.FindByText(HldyTypeText));

                //    txtDate.Text = ((Label)GrdHldy.Rows[RowIndex].FindControl("lblDate")).Text;
                //    txtDayFrm.Text = ((Label)GrdHldy.Rows[RowIndex].FindControl("lblDateFrom")).Text;
                //    txtDayTo.Text = ((Label)GrdHldy.Rows[RowIndex].FindControl("lblDateTo")).Text;
                //    ClientScript.RegisterStartupScript(this.GetType(), "populate", "select()");
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "populate", "select()", true);

                //}
                #endregion Edit
                if (e.CommandName == "DeleteRecord")
                {
                    int RowIndex = Convert.ToInt32(e.CommandArgument);
                    Session["Srno"] = ((Label)GrdHldy.Rows[RowIndex].FindControl("lblSrNo")).Text;
                    DeleteRecord();
                }
            }
            catch (Exception ex)
            {

            }

        }
        # endregion RowCommand

        # region RowEditing
        protected void GrdHldy_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        # endregion grvCommodityList_RowEditing

        # region PageIndexChanging
        protected void GrdHldy_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
            
            DataSet dsneew = new DataSet();
            dsneew = (DataSet)ViewState["listds"];
            GrdHldy.PageIndex = e.NewPageIndex;
            GrdHldy.DataSource = dsneew;
            GrdHldy.DataBind();

            for (int j = 0; j < GrdHldy.Rows.Count; j++)
            {
                if (((Label)(GrdHldy.Rows[j].FindControl("lblHolidayType"))).Text.ToString() == "WO")
                {
                    ((Label)(GrdHldy.Rows[j].FindControl("lblHolidayType"))).Text = "Weekly Off";
                }

                else if (((Label)(GrdHldy.Rows[j].FindControl("lblHolidayType"))).Text.ToString() == "PH")
                {
                    ((Label)(GrdHldy.Rows[j].FindControl("lblHolidayType"))).Text = "Public Holiday";
                }
                else if (((Label)(GrdHldy.Rows[j].FindControl("lblHolidayType"))).Text.ToString() == "SH")
                {
                    ((Label)(GrdHldy.Rows[j].FindControl("lblHolidayType"))).Text = "Station Holiday";
                }
                else if (((Label)(GrdHldy.Rows[j].FindControl("lblHolidayType"))).Text.ToString() == "CH")
                {
                    ((Label)(GrdHldy.Rows[j].FindControl("lblHolidayType"))).Text = "Company Holiday";
                }
            }
            }
            catch (Exception ex)
            {
            }
        }

        # endregion grvCommodityList_PageIndexChanging

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
            myDataColumn.ColumnName = "Station";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Country";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Day";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Date";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "HolidayType";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "DateFrom";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "DateTo";
            myDataTable.Columns.Add(myDataColumn);

            //myDataColumn = new DataColumn();
            //myDataColumn.DataType = Type.GetType("System.String");
            //myDataColumn.ColumnName = "SrNo";
            //myDataTable.Columns.Add(myDataColumn);

            //myDataColumn = new DataColumn();
            //myDataColumn.DataType = Type.GetType("System.String");
            //myDataColumn.ColumnName = "IsAllStn";
            //myDataTable.Columns.Add(myDataColumn);


            DataRow dr;
            dr = myDataTable.NewRow();
            dr["SerialNumber"] = "";
            dr["Station"] = "";
            dr["Country"] = "";
            dr["Day"] = "";
            dr["Date"] = "";
            dr["HolidayType"] = "";
            dr["DateFrom"] = "";
            dr["DateTo"] = "";
            //dr["FreightRate"] = "";


            myDataTable.Rows.Add(dr);

            GrdHldy.DataSource = null;
            GrdHldy.DataSource = myDataTable;
            GrdHldy.DataBind();
            Session["dsdata"] = myDataTable.Copy();

        }
        #endregion
        protected void GetHolidayList()
        {
            #region Prepare Parameters
            object[] Hldy = new object[4];
            int i = 0;

            //string hltype;
            //if (ddlHolidayType.SelectedIndex == 0)
            //    hltype = "";
            //else
            //    hltype = ddlHolidayType.SelectedItem.Value;

            //0
            if (ddlHolidayType.SelectedIndex == 0)
            {
                Hldy.SetValue("", i);
                i++;
            }
            else
            {
                Hldy.SetValue(ddlHolidayType.SelectedItem.Value, i);
                i++;
            }

            //1
            if (ddlCountry.SelectedIndex == 0)
            {
                Hldy.SetValue("", i);
                i++;
            }
            else
            {
                Hldy.SetValue(ddlCountry.SelectedItem.Text, i);
                i++;
            }


            //2
            if (ddlStn.SelectedIndex == 0)
            {
                Hldy.SetValue("", i);
                i++;
            }
            else
            {
                Hldy.SetValue(ddlStn.SelectedItem.Text, i);
                i++;
            }
            //3
            if (ddlDay.SelectedIndex == 0)
            {
                Hldy.SetValue("", i);
                i++;
            }
            else
            {
                Hldy.SetValue("", i);
                i++;
            }

            ////4
            //if (txtValidFrm.Text == "")
            //{
            //    Hldy.SetValue(null, i);
            //    i++;
            //}
            //else
            //{
            //    DateTime validfrom ;//DateTime.ParseExact(txtValidFrm.Text, "yyyy/MM/dd", null);
            //    string day = txtValidFrm.Text.Substring(0, 2);
            //    string mon = txtValidFrm.Text.Substring(3, 2);
            //    string yr = txtValidFrm.Text.Substring(6, 4);
            //    string frmDate = yr + "-" + mon + "-" + day;
            //    validfrom = Convert.ToDateTime(frmDate);
            //    Hldy.SetValue(Convert.ToDateTime(frmDate).ToString("yyyy-MM-dd HH:mm:ss"), i);
            //    i++;
            //}

            ////5
            //if (txtValidTo.Text == "")
            //{
            //    Hldy.SetValue(null, i);
            //    i++;
            //}
            //else
            //{
            //    DateTime validto;// DateTime.ParseExact(txtValidTo.Text, "yyyy/MM/dd", null);
            //    string day = txtValidTo.Text.Substring(0, 2);
            //    string mon = txtValidTo.Text.Substring(3, 2);
            //    string yr = txtValidTo.Text.Substring(6, 4);
            //    string toDate = yr + "-" + mon + "-" + day;
            //    validto = Convert.ToDateTime(toDate);
            //    Hldy.SetValue(Convert.ToDateTime(toDate).ToString("yyyy-MM-dd HH:mm:ss"), i);
            //    i++;
            //}

            ////6
            //if (txtDayFrm.Text == "")
            //{
            //    Hldy.SetValue(null, i);
            //    i++;
            //}
            //else
            //{
            //    DateTime datefrom;// DateTime.ParseExact(txtDayFrm.Text, "yyyy/MM/dd", null);
            //    string day = txtDayFrm.Text.Substring(0, 2);
            //    string mon = txtDayFrm.Text.Substring(3, 2);
            //    string yr = txtDayFrm.Text.Substring(6, 4);
            //    string frmDate = yr + "-" + mon + "-" + day;
            //    datefrom = Convert.ToDateTime(frmDate);
            //    Hldy.SetValue(Convert.ToDateTime(frmDate).ToString("yyyy-MM-dd HH:mm:ss"), i);
            //    i++;
            //}

            ////7
            //if (txtDayTo.Text == "")
            //{
            //    Hldy.SetValue(null, i);
            //    i++;
            //}
            //else
            //{
            //    DateTime dateto;// DateTime.ParseExact(txtDayTo.Text, "yyyy/MM/dd", null);
            //    string day = txtDayTo.Text.Substring(0, 2);
            //    string mon = txtDayTo.Text.Substring(3, 2);
            //    string yr = txtDayTo.Text.Substring(6, 4);
            //    string toDate = yr + "-" + mon + "-" + day;
            //    dateto =Convert.ToDateTime(toDate);
            //    Hldy.SetValue(Convert.ToDateTime(toDate).ToString("yyyy-MM-dd HH:mm:ss"), i);
            //    i++;
            //}

            #endregion Prepare Parameters
            try
            {
                DataSet ds = new DataSet();
                ds = objBal.GetHolidayList(Hldy);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                GrdHldy.PageIndex = 0;
                                GrdHldy.DataSource = ds;
                                GrdHldy.DataMember = ds.Tables[0].TableName;
                                GrdHldy.DataBind();
                                GrdHldy.Visible = true;
                                ViewState["listds"] = ds;
                                //ds.Clear();
                                //Clear();
                                StatusLbl.Text = string.Empty;
                                
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message12", "<SCRIPT LANGUAGE='javascript'>select();</script>", false);
                            }
                            if (ds.Tables[0].Rows.Count == 0)
                            {
                                Clear();
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message12", "<SCRIPT LANGUAGE='javascript'>select();</script>", false);
                                StatusLbl.Text = "No records exists";
                                GrdHldy.DataSource = null;
                                GrdHldy.DataBind();
                                //GetHolidayList();

                            }
                        }
                    }
                }
                for (int j = 0; j < GrdHldy.Rows.Count; j++)
                {
                    if (((Label)(GrdHldy.Rows[j].FindControl("lblHolidayType"))).Text.ToString() == "WO")
                    {
                        ((Label)(GrdHldy.Rows[j].FindControl("lblHolidayType"))).Text = "Weekly Off";
                    }

                    else if (((Label)(GrdHldy.Rows[j].FindControl("lblHolidayType"))).Text.ToString() == "PH")
                    {
                        ((Label)(GrdHldy.Rows[j].FindControl("lblHolidayType"))).Text = "Public Holiday";
                    }
                    else if (((Label)(GrdHldy.Rows[j].FindControl("lblHolidayType"))).Text.ToString() == "SH")
                    {
                        ((Label)(GrdHldy.Rows[j].FindControl("lblHolidayType"))).Text = "Station Holiday";
                    }
                    else if (((Label)(GrdHldy.Rows[j].FindControl("lblHolidayType"))).Text.ToString() == "CH")
                    {
                        ((Label)(GrdHldy.Rows[j].FindControl("lblHolidayType"))).Text = "Company Holiday";
                    }
                }
                //ddlHolidayType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
            }
        }

        protected void Clear()
        {
            ddlHolidayType.SelectedIndex = 0;
            ddlCountry.SelectedIndex = 0;
            ddlStn.SelectedIndex = 0;
            ddlDay.SelectedIndex = 0;
            txtDate.Text = txtDayFrm.Text = txtDayTo.Text = string.Empty;
            txtValidFrm.Text = txtValidTo.Text = string.Empty;
            StatusLbl.Text = string.Empty;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {

            //ddlHolidayType.SelectedIndex = 0;
            //ddlCountry.Enabled = ddlStn.Enabled = ddlDay.Enabled = false;
            //txtDate.Enabled = txtDayFrm.Enabled = txtDayTo.Enabled = false;
            //txtValidFrm.Enabled = txtValidTo.Enabled = false;
            //GetAirportCode();
            //GetCountryCode();
            GrdHldy.DataSource = null;
            GrdHldy.DataBind();
        }

    }
}

