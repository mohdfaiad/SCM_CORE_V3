using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BAL;
using System.Drawing;
using System.Collections;
using QID.DataAccess;
using System.IO;
using System.Globalization;

namespace ProjectSmartCargoManager
{
    public partial class PartnerSchedule : System.Web.UI.Page
    {
        #region Variables
        AirlineScheduleBAL OBJasb = new AirlineScheduleBAL();
        ArrayList CheckFlight = new ArrayList();
        SQLServer db = new SQLServer(Global.GetConnectionString());
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    FillPartnerType();            //Filling Partner Type to DropDownList
                    LoadGridSchedule();          //Adding a Blank Row To grid
                    AirCraftType();             //Filling DropDownList With Aircraft
                    AirCraftTypeinGridview();   //Filling Aircraft Type & Capacity in GridView
                    OriginList();               //Filling DropDownList with Origin
                    DestinationList();          //Filling DropDownList with Destination
                    LoadSourceInGridview();     //Filling GridView With Details
                    LoadDestinationInGridview(); //Filling DropDownList within GridView With Destination
                }
            }
            catch (Exception)
            {
            }
        }
        #region Add new Row To Grid
        private void AddNewRowToGrid()
        {
            try
            {
                int rowIndex = 0;


                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                dtCurrentTable.Rows.Clear();
                if (grdScheduleinfo.Rows.Count > 0)
                {
                    for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
                    {
                        //extract the TextBox values

                        drCurrentRow = dtCurrentTable.NewRow();
                        dtCurrentTable.Rows.Add(drCurrentRow);
                        drCurrentRow["RowNumber"] = i + 1;

                        dtCurrentTable.Rows[i]["From"] = ((DropDownList)grdScheduleinfo.Rows[i].Cells[0].FindControl("ddlFromOrigin")).Text;
                        dtCurrentTable.Rows[i]["To"] = ((DropDownList)grdScheduleinfo.Rows[i].Cells[1].FindControl("ddlToDest")).Text;

                        dtCurrentTable.Rows[i]["DeptDay"] = ((TextBox)grdScheduleinfo.Rows[i].Cells[2].FindControl("txtDeptDay")).Text;
                        dtCurrentTable.Rows[i]["Dept TimeHr"] = ((TextBox)grdScheduleinfo.Rows[i].Cells[2].FindControl("txtDeptTimeHr")).Text;
                        dtCurrentTable.Rows[i]["Dept TimeMin"] = ((TextBox)grdScheduleinfo.Rows[i].Cells[2].FindControl("txtDeptTimeMin")).Text;

                        dtCurrentTable.Rows[i]["ArrDay"] = ((TextBox)grdScheduleinfo.Rows[i].Cells[1].FindControl("txtArrivalDay")).Text;

                        dtCurrentTable.Rows[i]["Arrival TimeHr"] = ((TextBox)grdScheduleinfo.Rows[i].Cells[1].FindControl("txtArrivaltimeHr")).Text;
                        dtCurrentTable.Rows[i]["Arrival TimeMin"] = ((TextBox)grdScheduleinfo.Rows[i].Cells[1].FindControl("txtArrivaltimeMin")).Text;

                        dtCurrentTable.Rows[i]["chkMon"] = ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkMon")).Checked;
                        dtCurrentTable.Rows[i]["chkTues"] = ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkTues")).Checked;
                        dtCurrentTable.Rows[i]["chkwed"] = ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkwed")).Checked;
                        dtCurrentTable.Rows[i]["chkThur"] = ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkThur")).Checked;
                        dtCurrentTable.Rows[i]["chkFri"] = ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkFri")).Checked;
                        dtCurrentTable.Rows[i]["chkSat"] = ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkSat")).Checked;
                        dtCurrentTable.Rows[i]["chkSun"] = ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkSun")).Checked;
                        dtCurrentTable.Rows[i]["Status"] = ((DropDownList)grdScheduleinfo.Rows[i].Cells[3].FindControl("ddlStatus")).Text;

                        dtCurrentTable.Rows[i]["ddlAirCraft"] = ((DropDownList)grdScheduleinfo.Rows[i].Cells[4].FindControl("ddlAirCraft")).Text;
                        dtCurrentTable.Rows[i]["ddlTailNo"] = ((DropDownList)grdScheduleinfo.Rows[i].Cells[4].FindControl("ddlTailNo")).Text;
                        dtCurrentTable.Rows[i]["txtCapacity"] = ((TextBox)grdScheduleinfo.Rows[i].Cells[5].FindControl("txtCapacity")).Text;


                    }

                    //   dtCurrentTable.Rows.Add(drCurrentRow);
                    drCurrentRow = dtCurrentTable.NewRow();
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    // dtCurrentTable.Rows.Add();
                    ViewState["CurrentTable"] = dtCurrentTable;

                    grdScheduleinfo.DataSource = dtCurrentTable;
                    grdScheduleinfo.DataBind();

                }


                //Set Previous Data on Postbacks
                // SetPreviousData();
            }
            catch (Exception ex)
            {
            }


        }
        #endregion Add new Row To Grid

        #region Adding a Blank Row To grid
        private void LoadGridSchedule()
        {

            DataTable myDataTable = new DataTable();
            DataColumn myDataColumn;
            DataSet Ds = new DataSet();

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "RowNumber";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "From";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "To";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "DeptDay";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Dept TimeHr";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Dept TimeMin";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "ArrDay";
            myDataTable.Columns.Add(myDataColumn);



            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Arrival TimeHr";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Arrival TimeMin";
            myDataTable.Columns.Add(myDataColumn);




            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "chkMon";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "chkTues";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "chkwed";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "chkThur";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "chkFri";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "chkSat";
            myDataTable.Columns.Add(myDataColumn);


            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "chkSun";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Status";
            myDataTable.Columns.Add(myDataColumn);


            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "ddlAirCraft";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "ddlTailNo";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "txtCapacity";
            myDataTable.Columns.Add(myDataColumn);




            DataRow dr;
            dr = myDataTable.NewRow();
            dr["RowNumber"] = 1;
            dr["From"] = "select";//"5";
            dr["To"] = "";// "5";
            dr["Deptday"] = "";
            dr["Dept TimeHr"] = "";
            dr["Dept TimeMin"] = "";
            dr["Arrday"] = "";
            dr["Arrival TimeHr"] = "";// "9";
            dr["Arrival TimeMin"] = "";
            dr["chkSun"] = "";
            dr["chkMon"] = "";
            dr["chkTues"] = "";
            dr["chkwed"] = "";
            dr["chkThur"] = "";
            dr["chkFri"] = "";
            dr["chkSat"] = "";
            dr["Status"] = "";
            dr["ddlAirCraft"] = "";
            dr["ddlTailNo"] = "";
            dr["txtCapacity"] = "";



            myDataTable.Rows.Add(dr);
            ViewState["CurrentTable"] = myDataTable;
            //Bind the DataTable to the Grid

            grdScheduleinfo.DataSource = null;
            grdScheduleinfo.DataSource = myDataTable;
            grdScheduleinfo.DataBind();
        }
        #endregion LoadgridScheduleInfo Intial Row

        #region Filling DropDownList with Origin
        private void OriginList()
        {
            try
            {
                DataSet ds = OBJasb.GetOriginList(ddlOrigin.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            //Dest dropdown
                            DataRow row = ds.Tables[0].NewRow();

                            row[ds.Tables[0].Columns[0].ColumnName] = "Select";
                            ds.Tables[0].Rows.Add(row);

                            ddlOrigin.DataSource = ds;
                            ddlOrigin.DataMember = ds.Tables[0].TableName;
                            ddlOrigin.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            ddlOrigin.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                            ddlOrigin.DataBind();
                            ddlOrigin.Text = "Select";

                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion Origin List

        #region Filling DropDownList with Destination

        private void DestinationList()
        {
            try
            {
                DataSet ds = OBJasb.GetDestinationList(ddlDestination.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            //Dest dropdown
                            DataRow row = ds.Tables[0].NewRow();

                            row[ds.Tables[0].Columns[0].ColumnName] = "Select";
                            ds.Tables[0].Rows.Add(row);

                            ddlDestination.DataSource = ds;
                            ddlDestination.DataMember = ds.Tables[0].TableName;
                            ddlDestination.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            ddlDestination.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                            ddlDestination.DataBind();
                            ddlDestination.Text = "Select";

                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion Destination List

        #region Filling GridView With Details
        public void LoadSourceInGridview()
        {
            try
            {
                ArrayList arCheckOrigin = new ArrayList();

                DataSet ds = OBJasb.GetOriginList("");
                DropDownList ddl = new DropDownList();
                TextBox txtDeptDay = new TextBox();
                TextBox txtDepthr = new TextBox();
                TextBox txtDeptMin = new TextBox();
                TextBox txtArrDay = new TextBox();
                TextBox txtArrhr = new TextBox();
                TextBox txtArrMin = new TextBox();
                DropDownList ddlTempDest = new DropDownList();
                DropDownList ddlAircraft = new DropDownList();
                DropDownList ddlTailNo = new DropDownList();
                TextBox capacity = new TextBox();

                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
                {
                    ddl = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlFromOrigin")));

                    ddlTempDest = ((DropDownList)grdScheduleinfo.Rows[i].Cells[1].FindControl("ddlToDest"));

                    txtDeptDay = ((TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptDay")));
                    txtDepthr = ((TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeHr")));
                    txtDeptMin = ((TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeMin")));
                    txtArrDay = ((TextBox)(grdScheduleinfo.Rows[i].FindControl("txtArrivalDay")));
                    txtArrhr = ((TextBox)(grdScheduleinfo.Rows[i].FindControl("txtArrivaltimeHr")));
                    txtArrMin = ((TextBox)(grdScheduleinfo.Rows[i].FindControl("txtArrivalTimeMin")));
                    ddlAircraft = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlAirCraft")));
                    ddlTailNo = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlTailNo")));
                    capacity = ((TextBox)(grdScheduleinfo.Rows[i].FindControl("txtCapacity")));
                    if (ds != null)
                    {
                        //Source in Gridview Dropdown
                        DataRow row = ds.Tables[0].NewRow();
                        if (!arCheckOrigin.Contains("Select"))
                        {
                            row[ds.Tables[0].Columns[0].ColumnName] = "Select";
                            ds.Tables[0].Rows.Add(row);
                            arCheckOrigin.Add("Select");


                        }

                        ddl.DataSource = ds;
                        ddl.DataMember = ds.Tables[0].TableName;
                        ddl.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                        ddl.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                        ddl.DataBind();
                        txtDeptDay.Text = dtCurrentTable.Rows[i][3].ToString();
                        txtDepthr.Text = dtCurrentTable.Rows[i][4].ToString();
                        txtDeptMin.Text = dtCurrentTable.Rows[i][5].ToString();
                        txtArrDay.Text = dtCurrentTable.Rows[i][6].ToString();
                        txtArrhr.Text = dtCurrentTable.Rows[i][7].ToString();
                        txtArrMin.Text = dtCurrentTable.Rows[i][8].ToString();
                        try
                        {
                            //Mod Vikas 23Jul

                            ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkMon")).Checked = dtCurrentTable.Rows[i][9].ToString() != "" ? bool.Parse(dtCurrentTable.Rows[i][9].ToString()) : false;
                            ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkTues")).Checked = dtCurrentTable.Rows[i][10].ToString() != "" ? bool.Parse(dtCurrentTable.Rows[i][10].ToString()) : false;
                            ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkwed")).Checked = dtCurrentTable.Rows[i][11].ToString() != "" ? bool.Parse(dtCurrentTable.Rows[i][11].ToString()) : false;
                            ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkThur")).Checked = dtCurrentTable.Rows[i][12].ToString() != "" ? bool.Parse(dtCurrentTable.Rows[i][12].ToString()) : false;
                            ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkFri")).Checked = dtCurrentTable.Rows[i][13].ToString() != "" ? bool.Parse(dtCurrentTable.Rows[i][13].ToString()) : false;
                            ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSat")).Checked = dtCurrentTable.Rows[i][14].ToString() != "" ? bool.Parse(dtCurrentTable.Rows[i][14].ToString()) : false;
                            ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSun")).Checked = dtCurrentTable.Rows[i][15].ToString() != "" ? bool.Parse(dtCurrentTable.Rows[i][15].ToString()) : false;
                            //((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkMon")).Checked = bool.Parse(dtCurrentTable.Rows[i][9].ToString());
                            //((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkTues")).Checked = bool.Parse(dtCurrentTable.Rows[i][10].ToString());
                            //((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkwed")).Checked = bool.Parse(dtCurrentTable.Rows[i][11].ToString());
                            //((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkThur")).Checked = bool.Parse(dtCurrentTable.Rows[i][12].ToString());
                            //((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkFri")).Checked = bool.Parse(dtCurrentTable.Rows[i][13].ToString());
                            //((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSat")).Checked = bool.Parse(dtCurrentTable.Rows[i][14].ToString());
                            //((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSun")).Checked = bool.Parse(dtCurrentTable.Rows[i][15].ToString());


                            ((DropDownList)grdScheduleinfo.Rows[i].FindControl("ddlStatus")).Text = dtCurrentTable.Rows[i][16].ToString();
                        }
                        catch (Exception ex)
                        {

                        }
                        ddlAircraft.Text = dtCurrentTable.Rows[i][17].ToString();
                        ddlTailNo.Text = dtCurrentTable.Rows[i][18].ToString();
                        capacity.Text = dtCurrentTable.Rows[i][19].ToString();


                        // ddl.Text = "Select";
                        if (dtCurrentTable.Rows.Count > 1)
                        {
                            if (dtCurrentTable.Rows[i][1].ToString() != "select" && dtCurrentTable.Rows[i][1].ToString() != "")
                            {
                                ddl.Text = dtCurrentTable.Rows[i][1].ToString();

                                //txtDepthr.Text = dtCurrentTable.Rows[i][2].ToString();
                                //txtDeptMin.Text = dtCurrentTable.Rows[i][3].ToString();
                                //txtArrhr.Text = dtCurrentTable.Rows[i][4].ToString();



                            }
                            else
                            {
                                ddl.Text = dtCurrentTable.Rows[i - 1][2].ToString();

                                //ddl.Text = "Select";
                            }
                        }
                        else
                        {
                            ddl.Text = "Select";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

        }

        #endregion LoadSource Dropdown

        #region Show Capacity
        protected void showCapacityInGrid(object sender, EventArgs e)
        {
            for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
            {

                try
                {

                    string aircrafttype = ((DropDownList)grdScheduleinfo.Rows[i].FindControl("ddlAirCraft")).SelectedItem.ToString();

                    DataSet ds = OBJasb.GetCargoCapacity(aircrafttype);
                    if (ds != null)
                    {
                        if (ds.Tables != null)
                        {
                            if (ds.Tables.Count > 0 || ds.Tables[0].Rows.Count > 0)
                            {
                                txtCargoCapacity.Text = ds.Tables[0].Rows[0][0].ToString();
                                ((TextBox)grdScheduleinfo.Rows[i].FindControl("txtCapacity")).Text = ds.Tables[0].Rows[0][0].ToString();
                            }
                        }
                    }
                }

                catch (Exception)
                {
                }

                #region get Tail Number as per Aircraft Type
                try
                {
                    string AirCrtType = ((DropDownList)grdScheduleinfo.Rows[i].FindControl("ddlAirCraft")).SelectedItem.ToString();
                    DataSet dsTail = OBJasb.GetTailNumber(AirCrtType);

                    if (dsTail != null && dsTail.Tables.Count > 0 && dsTail.Tables[0].Rows.Count > 0)
                    {

                        ((DropDownList)grdScheduleinfo.Rows[i].FindControl("ddlTailNo")).Items.Clear();
                        ((DropDownList)grdScheduleinfo.Rows[i].FindControl("ddlTailNo")).DataSource = dsTail.Tables[0];
                        ((DropDownList)grdScheduleinfo.Rows[i].FindControl("ddlTailNo")).DataTextField = "TailNo";
                        ((DropDownList)grdScheduleinfo.Rows[i].FindControl("ddlTailNo")).DataValueField = "TailNo";

                        ((DropDownList)grdScheduleinfo.Rows[i].FindControl("ddlTailNo")).DataBind();

                    }
                }
                catch (Exception ex)
                {

                }
                #endregion 

            }

        }
        #endregion

        #region Filling DropDownList within GridView With Destination
        public void LoadDestinationInGridview()
        {
            try
            {
                ArrayList dtarCheckdest = new ArrayList();

                DataSet ds = OBJasb.GetDestinationList("");
                DropDownList ddlest = new DropDownList();
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];

                for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
                {
                    ddlest = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlToDest")));
                    if (ds != null)
                    {
                        //LoadDest in Gridview Dropdown 
                        DataRow row = ds.Tables[0].NewRow();
                        if (!dtarCheckdest.Contains("Select"))
                        {
                            row[ds.Tables[0].Columns[0].ColumnName] = "Select";
                            ds.Tables[0].Rows.Add(row);
                            dtarCheckdest.Add("Select");
                        }
                        ddlest.DataSource = ds;
                        ddlest.DataMember = ds.Tables[0].TableName;
                        ddlest.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                        ddlest.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                        ddlest.DataBind();
                        //ddl.Text = "Select";
                        if (dtCurrentTable.Rows.Count > 1)
                        {
                            if (dtCurrentTable.Rows[i][2].ToString() != "select" && dtCurrentTable.Rows[i][2].ToString() != "")
                            {
                                ddlest.Text = dtCurrentTable.Rows[i][2].ToString();
                            }
                            else
                            {
                                //ddlest.Text = "Select";
                                ddlest.Text = ddlDestination.SelectedValue;
                            }
                        }
                        else
                        {
                            ddlest.Text = "Select";
                        }
                        //ddlest.Text = "Select";
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        #endregion LoadDest Dropdown

        #region Add new Row to GridView
        protected void Addrow(object sender, EventArgs e)
        {
            try
            {
                AddNewRowToGrid();
                LoadSourceInGridview();
                LoadDestinationInGridview();


            }
            catch (Exception ex)
            {
            }
            // LoadGridSchedule();
        }
        #endregion

        #region Save Partner Schedule
        protected void btnSave_Click(object sender, EventArgs e)
        {



            ddlFlight.Text = ddlFlight.Text.ToUpper().Trim();
            lblStatus.Text = "";
            lblStatus.ForeColor = Color.Red;

            if (!ValidateData())
            {

                return;
            }



            #region Insert Partner Schedule Summary

            //DateTime dt1 = DateTime.ParseExact(txtFromdate.Text, "dd/MM/yyyy", null);
            //DateTime dt2 = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);

            DateTime dt1 = Convert.ToDateTime(txtFromdate.Text);
            DateTime dt2 = Convert.ToDateTime(txtToDate.Text);


            try
            {
                #region Prepare Parameters

                object[] ScheduleInfo = new object[14];
                int k = 0;

                //0
                ScheduleInfo.SetValue(dt1, k);
                k++;


                //1
                ScheduleInfo.SetValue(dt2, k);
                k++;

                //2
                ScheduleInfo.SetValue(ddlAirCraftType.SelectedValue, k);
                k++;

                //3
                ScheduleInfo.SetValue(ddlFlight.Text, k);
                k++;


                //4
                ScheduleInfo.SetValue(ddlOrigin.SelectedValue, k);
                k++;

                //5
                ScheduleInfo.SetValue(ddlDestination.SelectedValue, k);
                k++;


                //6
                string DeptTime = ((TextBox)grdScheduleinfo.Rows[0].FindControl("txtDeptTimeHr")).Text;// 
                DeptTime = DeptTime.PadLeft(2, '0');
                DeptTime = DeptTime + ":" + ((TextBox)grdScheduleinfo.Rows[0].FindControl("txtDeptTimeMin")).Text.PadLeft(2, '0');
                ScheduleInfo.SetValue(DeptTime, k);
                k++;

                //7
                //mohan
                //string arrTime = ((TextBox)grdScheduleinfo.Rows[0].FindControl("txtArrivaltimeHr")).Text;
                //arrTime = arrTime + ":" + ((TextBox)grdScheduleinfo.Rows[0].FindControl("txtArrivaltimeMin")).Text;
                //

                //mod vikas 28Jul
                string arrTime = ((TextBox)grdScheduleinfo.Rows[grdScheduleinfo.Rows.Count - 1].FindControl("txtArrivaltimeHr")).Text;
                arrTime = arrTime.PadLeft(2, '0');
                arrTime = arrTime + ":" + ((TextBox)grdScheduleinfo.Rows[grdScheduleinfo.Rows.Count - 1].FindControl("txtArrivaltimeMin")).Text.PadLeft(2, '0');
                //

                //9
                ScheduleInfo.SetValue(arrTime, k);
                k++;

                //8
                ScheduleInfo.SetValue(DateTime.Now.ToString(), k);
                k++;

                //9
                ScheduleInfo.SetValue("QIDUser", k);
                k++;

                //10
                ScheduleInfo.SetValue(txtCargoCapacity.Text.Trim(), k);
                k++;

                string strfrquency = "";
                if (((CheckBox)grdScheduleinfo.Rows[0].FindControl("chkMon")).Checked == true)
                    strfrquency = "1";
                else
                    strfrquency = "0";

                if (((CheckBox)grdScheduleinfo.Rows[0].FindControl("chkTues")).Checked == true)
                    strfrquency = strfrquency + ',' + "1";
                else
                    strfrquency = strfrquency + ',' + "0";

                if (((CheckBox)grdScheduleinfo.Rows[0].FindControl("chkwed")).Checked == true)
                    strfrquency = strfrquency + ',' + "1";
                else
                    strfrquency = strfrquency + ',' + "0";



                if (((CheckBox)grdScheduleinfo.Rows[0].FindControl("chkThur")).Checked == true)
                    strfrquency = strfrquency + ',' + "1";
                else
                    strfrquency = strfrquency + ',' + "0";



                if (((CheckBox)grdScheduleinfo.Rows[0].FindControl("chkFri")).Checked == true)
                    strfrquency = strfrquency + ',' + "1";
                else
                    strfrquency = strfrquency + ',' + "0";


                if (((CheckBox)grdScheduleinfo.Rows[0].FindControl("chkSat")).Checked == true)
                    strfrquency = strfrquency + ',' + "1";
                else
                    strfrquency = strfrquency + ',' + "0";


                if (((CheckBox)grdScheduleinfo.Rows[0].FindControl("chkSun")).Checked == true)
                    strfrquency = strfrquency + ',' + "1";
                else
                    strfrquency = strfrquency + ',' + "0";


                //11
                ScheduleInfo.SetValue(strfrquency, k);
                k++;

                Boolean IsDomestic = true;
                if (rbtDoeastic.Checked == true)
                {
                    IsDomestic = true;
                }
                else if (rbtInternational.Checked == true)
                {
                    IsDomestic = false;
                }

                //12
                ScheduleInfo.SetValue(IsDomestic, k);
                k++;

                //13
                //string PartnerCode = ((TextBox)grdScheduleinfo.Rows[0].FindControl("txtPartnerCode")).Text;
                string PartnerCode = txtPartnerCode.Text;
                ScheduleInfo.SetValue(PartnerCode, k);
                k++;

                #endregion Prepare Parameters
                int IsSchduleInsert = 0;
                IsSchduleInsert = OBJasb.SavePartnerSchedule(ScheduleInfo);
                //Call SP to Save database.
                if (IsSchduleInsert < 0)
                {

                    lblStatus.Text = "Error Save Partner schedule. Please try again...";
                    return;
                }
            #endregion

                #region Insert Partner Schedule RouteDetails
                #region Prepare Parameters

                int IsRouteInsert = 0;
                for (int j = 0; j < grdScheduleinfo.Rows.Count; j++)
                {
                    object[] RouteInfo = new object[19];
                    int i = 0;

                    //0
                    RouteInfo.SetValue(dt1, i);
                    i++;


                    //1
                    RouteInfo.SetValue(dt2, i);
                    i++;

                    //2
                    RouteInfo.SetValue(ddlFlight.Text, i);
                    i++;


                    //3
                    RouteInfo.SetValue(ddlOrigin.SelectedValue, i);
                    i++;

                    //4
                    RouteInfo.SetValue(ddlDestination.SelectedValue, i);
                    i++;

                    //5
                    RouteInfo.SetValue(((DropDownList)(grdScheduleinfo.Rows[j].FindControl("ddlFromOrigin"))).SelectedValue, i);
                    i++;
                    //6
                    RouteInfo.SetValue(((DropDownList)(grdScheduleinfo.Rows[j].FindControl("ddlToDest"))).SelectedValue, i);
                    i++;
                    string frquency = "";
                    if (((CheckBox)grdScheduleinfo.Rows[j].FindControl("chkMon")).Checked == true)
                        frquency = "1";
                    else
                        frquency = "0";

                    if (((CheckBox)grdScheduleinfo.Rows[j].FindControl("chkTues")).Checked == true)
                        frquency = frquency + ',' + "1";
                    else
                        frquency = frquency + ',' + "0";

                    if (((CheckBox)grdScheduleinfo.Rows[j].FindControl("chkwed")).Checked == true)
                        frquency = frquency + ',' + "1";
                    else
                        frquency = frquency + ',' + "0";



                    if (((CheckBox)grdScheduleinfo.Rows[j].FindControl("chkThur")).Checked == true)
                        frquency = frquency + ',' + "1";
                    else
                        frquency = frquency + ',' + "0";



                    if (((CheckBox)grdScheduleinfo.Rows[j].FindControl("chkFri")).Checked == true)
                        frquency = frquency + ',' + "1";
                    else
                        frquency = frquency + ',' + "0";


                    if (((CheckBox)grdScheduleinfo.Rows[j].FindControl("chkSat")).Checked == true)
                        frquency = frquency + ',' + "1";
                    else
                        frquency = frquency + ',' + "0";


                    if (((CheckBox)grdScheduleinfo.Rows[j].FindControl("chkSun")).Checked == true)
                        frquency = frquency + ',' + "1";
                    else
                        frquency = frquency + ',' + "0";


                    //7
                    RouteInfo.SetValue(frquency, i);
                    i++;



                    //8
                    string DeptTime1 = ((TextBox)grdScheduleinfo.Rows[j].FindControl("txtDeptTimeHr")).Text; // 
                    DeptTime1 = DeptTime1 + ":" + ((TextBox)grdScheduleinfo.Rows[j].FindControl("txtDeptTimeMin")).Text;
                    RouteInfo.SetValue(DeptTime1, i);
                    i++;

                    //9
                    string arrTime1 = ((TextBox)grdScheduleinfo.Rows[j].FindControl("txtArrivaltimeHr")).Text;
                    arrTime1 = arrTime1 + ":" + ((TextBox)grdScheduleinfo.Rows[j].FindControl("txtArrivaltimeMin")).Text;
                    //10
                    RouteInfo.SetValue(arrTime1, i);
                    i++;

                    //11
                    RouteInfo.SetValue(DateTime.Now.ToString(), i);
                    i++;

                    string UserID = Session["UserName"].ToString();
                    //12
                    RouteInfo.SetValue(UserID, i);
                    i++;

                    //13
                    //Parameter Added on 20 april
                    RouteInfo.SetValue(((DropDownList)(grdScheduleinfo.Rows[j].FindControl("ddlStatus"))).SelectedValue, i);
                    //string status = ((DropDownList)grdScheduleinfo.Rows[j].FindControl("ddlStatus")).SelectedValue;
                    //
                    //RouteInfo.SetValue(status, i);
                    i++;


                    //14
                    //parameter added on 3May12
                    string strDeptDay = ((TextBox)(grdScheduleinfo.Rows[j].FindControl("txtDeptDay"))).Text;
                    RouteInfo.SetValue(strDeptDay, i);
                    i++;

                    //15
                    string strArrday = ((TextBox)(grdScheduleinfo.Rows[j].FindControl("txtArrivalDay"))).Text;
                    RouteInfo.SetValue(strArrday, i);
                    i++;

                    //16
                    string strCapacity = ((TextBox)(grdScheduleinfo.Rows[j].FindControl("txtCapacity"))).Text;
                    RouteInfo.SetValue(strCapacity, i);
                    i++;

                    //17
                    string strAirCarType = ((DropDownList)(grdScheduleinfo.Rows[j].FindControl("ddlAirCraft"))).SelectedItem.ToString();
                    RouteInfo.SetValue(strAirCarType, i);
                    i++;

                    //18
                    string strPartnerCode = txtPartnerCode.Text;
                    RouteInfo.SetValue(strPartnerCode, i);
                    i++;

                    //19
                    string strTailNo = ((DropDownList)(grdScheduleinfo.Rows[j].FindControl("ddlTailNo"))).SelectedItem.ToString();
                    RouteInfo.SetValue(strTailNo, i);
                    


                #endregion Prepare Parameters
                    IsRouteInsert = 0;
                    IsRouteInsert = OBJasb.SavePartnerRouteDetails(RouteInfo);

                #endregion

                }
                if (IsRouteInsert < 0)
                {
                    lblStatus.Text = "Error Save Route Details. Please try again...";
                    return;
                }
                else
                {
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "Partner Schedule Save Successfully";

                    #region For Master Audit Log
                    MasterAuditBAL ObjMAL = new MasterAuditBAL();
                    #region Prepare Parameters
                    object[] Params = new object[7];
                    int i = 0;

                    //1
                    Params.SetValue("Partner Flight Schedule", i);
                    i++;

                    //2
                    Params.SetValue(ddlFlight.Text + "(" + ddlOrigin.SelectedValue + "-" + ddlDestination.SelectedValue + ")", i);
                    i++;

                    //3

                    Params.SetValue("ADD", i);
                    i++;

                    //4

                    Params.SetValue("", i);
                    i++;


                    //5

                    Params.SetValue("", i);
                    i++;

                    //6

                    Params.SetValue(Session["UserName"], i);
                    i++;

                    //7
                    Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), i);
                    i++;


                    #endregion Prepare Parameters
                    ObjMAL.AddMasterAuditLog(Params);
                    #endregion
                }

            }
            catch (Exception ex)
            {
            }

        }
        #endregion

        #region Validate Data
        /// <summary>
        /// Validate data entered by user.
        /// </summary>
        /// <returns>Returns True if valid data is entered.</returns>
        private bool ValidateData()
        {
            try
            {
                #region validate flight
                if (ddlFlight.Text.Trim() == "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Enter Flight#";
                    ddlFlight.Focus();
                    return false;
                }
                // bool strflight = ddlFlight.Text.StartsWith("*");
                //    string strflight = ddlFlight.Text.Substring(1, ddlFlight.Text.Length);


                //if (!(ddlFlight.Text.StartsWith("*"))||Reg)// || ddlFlight.Text.StartsWith("SG")))
                //{
                //    lblStatus.ForeColor = Color.Red;
                //    lblStatus.Text = "Please Enter valid Flight#";
                //    ddlFlight.Focus();
                //    return false;
                //}

                if (txtCargoCapacity.Text.Trim() == "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Enter Cargo Capacity of Flight";
                    txtCargoCapacity.Focus();
                    return false;
                }
                #endregion

                #region Validate From Date and To date
                if (txtFromdate.Text == "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please select valid From date";
                    txtFromdate.Focus();
                    return false;
                }
                if (txtToDate.Text == "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please select valid To date";
                    txtToDate.Focus();
                    return false;
                }

                DateTime dt1 = DateTime.ParseExact(txtFromdate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                DateTime dt2 = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                int chk = DateTime.Compare(dt1, dt2);

                if (chk > 0)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid To date";
                    txtFromdate.Focus();
                    return false;
                }
                #endregion
            }
            catch (Exception ex)
            {

            }

            //if (ddlFlight.Text == "Select")
            //{
            //    lblStatus.Text = "Please select valid flight Code";
            //    ddlFlight.Focus();
            //    return false;
            //}

            #region Validate Orgin And Destinaton
            if (ddlOrigin.Text == "Select")
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please select valid Origin Code";
                ddlOrigin.Focus();
                return false;
            }

            if (ddlDestination.Text == "Select")
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please select valid Destination Code";
                ddlDestination.Focus();
                return false;
            }
            //if (ddlOrigin.SelectedValue == ddlDestination.SelectedValue)
            //{
            //    lblStatus.ForeColor = Color.Red;
            //    lblStatus.Text = "Origin and Destination Code can not be same.";
            //    ddlDestination.Focus();
            //    return false;
            //}
            #endregion


            if (ddlAirCraftType.Text == "Select")
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please select AirCraft Type";
                ddlAirCraftType.Focus();
                return false;
            }
            //if (txtCargoCapacity.Text == "")
            //{
            //    lblStatus.ForeColor = Color.Red;
            //    lblStatus.Text = "Please enter Cargo Capacity";
            //    ddlAirCraftType.Focus();
            //    return false;

            //}

            //try
            //{// if ( Convert.ToDateTime(txtFromdate.Text) > Convert.ToDateTime(txtToDate.Text))

            //    DataSet dsCheck = OBJasb.GetAirlineScheduleforflight(ddlOrigin.SelectedValue, ddlFlight.Text, strFreq, Convert.ToDateTime(txtFromdate.Text), Convert.ToDateTime(txtToDate.Text));
            //    if (dsCheck != null)
            //    {
            //        if (dsCheck.Tables != null)
            //        {
            //            if (dsCheck.Tables.Count > 0 && dsCheck.Tables[0].Rows.Count > 0)
            //            {
            //                lblStatus.ForeColor = Color.Red;
            //                lblStatus.Text = "FlightID already register for this origin and frequency.";
            //                return false;
            //            }
            //            else
            //            {

            //            }
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{

            //}






            //Validate Route grid.
            if (grdScheduleinfo.Rows.Count < 1)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please enter Route Details";
                return false;
            }



            for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
            {
                DropDownList tempSource;

                //Validate FromSource code
                tempSource = (DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlFromOrigin"));
                if (tempSource.Text == "Select")//tempSource.SelectedIndex < 1)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please select valid Origin Code from route (row :" + (i + 1) + ")";
                    tempSource.Focus();
                    return false;
                }

                DropDownList tempDest;

                //Validate FromSource code
                tempDest = (DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlToDest"));
                if (tempDest.Text == "Select")//tempSource.SelectedIndex < 1)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please select valid Destination Code in route (row :" + (i + 1) + ")";
                    tempDest.Focus();
                    return false;
                }
                if (tempSource.SelectedValue == tempDest.SelectedValue)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Origin and Destination Code can not be same in route details. (row :" + (i + 1) + ")";
                    tempDest.Focus();
                    return false;
                }






                TextBox tempdeptTimeHr;

                //Validate code description
                tempdeptTimeHr = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeHr"));
                try
                {
                    if (tempdeptTimeHr.Text == "")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid Departure time in Hrs (row :" + (i + 1) + ")";
                        tempdeptTimeHr.Focus();
                        return false;
                    }
                    if (int.Parse(tempdeptTimeHr.Text) > 24)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid Departure time in Hrs (row :" + (i + 1) + ")";
                        tempdeptTimeHr.Focus();
                        return false;
                    }
                    if (int.Parse(tempdeptTimeHr.Text) < 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid Departure time in Hrs (row :" + (i + 1) + ")";
                        tempdeptTimeHr.Focus();
                        return false;
                    }
                }
                catch (Exception)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid Departure time in Hrs (row :" + (i + 1) + ")";
                    tempdeptTimeHr.Focus();
                    return false;
                }
                //tempdeptTimeHr = null;
                TextBox tempDeprtureTimeMin;

                //Validate code description
                tempDeprtureTimeMin = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeMin"));
                try
                {
                    if (tempDeprtureTimeMin.Text == "")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid Departure time in Min (row :" + (i + 1) + ")";
                        tempDeprtureTimeMin.Focus();
                        return false;
                    }
                    else if (int.Parse(tempDeprtureTimeMin.Text) > 60)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid Departure time in Min (row :" + (i + 1) + ")";
                        tempDeprtureTimeMin.Focus();
                        return false;
                    }
                    else if (int.Parse(tempDeprtureTimeMin.Text) < 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid Departure time in Min (row :" + (i + 1) + ")";
                        tempDeprtureTimeMin.Focus();
                        return false;
                    }
                }
                catch (Exception)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid Departure time in Min (row :" + (i + 1) + ")";
                    tempDeprtureTimeMin.Focus();
                    return false;
                }
                try
                {
                    if (int.Parse(tempDeprtureTimeMin.Text) == 0 && int.Parse(tempdeptTimeHr.Text) == 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid departue time (row :" + (i + 1) + ")";
                        tempDeprtureTimeMin.Focus();
                        return false;

                    }
                }
                catch (Exception)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid Departure time(row :" + (i + 1) + ")";
                    tempDeprtureTimeMin.Focus();
                    return false;
                }

                tempDeprtureTimeMin = null;
                tempdeptTimeHr = null;
                //Validate Dept Day check day is null
                TextBox txtCheckDeptDay;
                //Validate code description
                txtCheckDeptDay = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptDay"));

                try
                {
                    if (txtCheckDeptDay.Text.Trim() == "")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid Departure Day (row :" + (i + 1) + ")";
                        txtCheckDeptDay.Focus();
                        return false;
                    }
                    else if (int.Parse(txtCheckDeptDay.Text) > 2)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid Departure day (row :" + (i + 1) + ")";
                        txtCheckDeptDay.Focus();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid Departure day (row :" + (i + 1) + ")";
                    txtCheckDeptDay.Focus();
                    return false;

                }


                //Validate Dept Day check day is null
                TextBox txtCheckArrDay;
                //Validate code description
                txtCheckArrDay = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtArrivalDay"));

                try
                {
                    if (txtCheckArrDay.Text.Trim() == "")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid Arrival Day (row :" + (i + 1) + ")";
                        txtCheckArrDay.Focus();
                        return false;
                    }
                    else if (int.Parse(txtCheckArrDay.Text) > 2)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid Arrival day (row :" + (i + 1) + ")";
                        txtCheckArrDay.Focus();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid Arrival day (row :" + (i + 1) + ")";
                    txtCheckArrDay.Focus();
                    return false;

                }



                TextBox tempArrivalTimeHr;

                //Validate tempArrivalTimeHr
                tempArrivalTimeHr = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtArrivaltimeHr"));
                try
                {
                    if (tempArrivalTimeHr.Text == "")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid arrival time in Hrs (row :" + (i + 1) + ")";
                        tempArrivalTimeHr.Focus();
                        return false;
                    }
                    else if (int.Parse(tempArrivalTimeHr.Text) > 24)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid arrival time in Hrs (row :" + (i + 1) + ")";
                        tempArrivalTimeHr.Focus();
                        return false;

                    }
                    else if (int.Parse(tempArrivalTimeHr.Text) < 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid arrival time in Hrs (row :" + (i + 1) + ")";
                        tempArrivalTimeHr.Focus();
                        return false;
                    }
                }
                catch (Exception)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid arrival time in Hrs (row :" + (i + 1) + ")";
                    tempArrivalTimeHr.Focus();
                    return false;
                }
                //tempArrivalTimeHr = null;

                TextBox tempArrivalTimeMin;

                //Validate code description
                tempArrivalTimeMin = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtArrivaltimeMin"));
                try
                {
                    if (tempArrivalTimeMin.Text == "")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid arrival time in Min (row :" + (i + 1) + ")";
                        tempArrivalTimeMin.Focus();
                        return false;
                    }
                    else if (int.Parse(tempArrivalTimeMin.Text) > 60)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid arrival time in Min (row :" + (i + 1) + ")";
                        tempArrivalTimeMin.Focus();
                        return false;
                    }
                    else if (int.Parse(tempArrivalTimeMin.Text) < 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid arrival time in Min (row :" + (i + 1) + ")";
                        tempArrivalTimeMin.Focus();
                        return false;
                    }
                }
                catch (Exception)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid arrival time in Min (row :" + (i + 1) + ")";
                    tempArrivalTimeMin.Focus();

                    return false;
                }

                try
                {
                    if (int.Parse(tempArrivalTimeMin.Text) == 0 && int.Parse(tempArrivalTimeHr.Text) == 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid Arrrival time (row :" + (i + 1) + ")";
                        tempDeprtureTimeMin.Focus();
                        return false;

                    }
                }
                catch (Exception)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid arrival time(row :" + (i + 1) + ")";
                    tempArrivalTimeMin.Focus();
                    return false;
                }

                tempArrivalTimeHr = null;
                tempArrivalTimeMin = null;

                //Validate Arrival Departure time
                try
                {
                    tempArrivalTimeHr = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtArrivaltimeHr"));
                    tempdeptTimeHr = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeHr"));
                    int arrday = int.Parse(txtCheckArrDay.Text);
                    int DeptDay = int.Parse(txtCheckDeptDay.Text);

                    tempArrivalTimeMin = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtArrivaltimeMin"));
                    tempDeprtureTimeMin = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeMin"));


                    int ArrTimeHr = int.Parse(tempArrivalTimeHr.Text);
                    int DeptTimeHr = int.Parse(tempdeptTimeHr.Text);
                    if ((ArrTimeHr < DeptTimeHr) && (arrday < DeptDay))
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid arrival time in Hr (row :" + (i + 1) + ")";
                        tempdeptTimeHr.Focus();
                        return false;
                    }
                    else if ((ArrTimeHr < DeptTimeHr) && (arrday == DeptDay))
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid arrival time in Hr (row :" + (i + 1) + ")";
                        tempdeptTimeHr.Focus();
                        return false;
                    }
                    else if ((ArrTimeHr == DeptTimeHr) && (arrday < DeptDay))
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid arrival day in row " + (i + 1);
                        tempdeptTimeHr.Focus();
                        return false;
                    }
                    else if (arrday < DeptDay)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid arrival day (row :" + (i + 1) + ")";
                        txtCheckArrDay.Focus();
                        return false;
                    }

                    else if (int.Parse(tempArrivalTimeHr.Text) == int.Parse(tempdeptTimeHr.Text))
                    {
                        if (int.Parse(txtCheckArrDay.Text) == int.Parse(txtCheckDeptDay.Text))
                        {
                            if (int.Parse(tempArrivalTimeMin.Text) < int.Parse(tempDeprtureTimeMin.Text))
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Please enter valid arrival Min (row :" + (i + 1) + ")";
                                tempDeprtureTimeMin.Focus();
                                return false;
                            }
                            else if (int.Parse(tempArrivalTimeMin.Text) == int.Parse(tempDeprtureTimeMin.Text))
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Daparture time and Arrival time should be different in row" + (i + 1);
                                tempDeprtureTimeMin.Focus();
                                return false;
                            }

                        }
                        if (int.Parse(tempArrivalTimeMin.Text) == int.Parse(tempDeprtureTimeMin.Text))
                        {
                            if (int.Parse(txtCheckArrDay.Text) < int.Parse(txtCheckDeptDay.Text))
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Please enter valid arrival Day";
                                txtCheckArrDay.Focus();
                                return false;
                            }
                        }

                    }

                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid Departure and Arrival time in Min";
                    //tempdeptTimeHr.Focus();
                    return false;
                }

                //Validate Frequency

                string strFreq = "";
                try
                {
                    if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkMon")).Checked == false && ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkTues")).Checked == false && ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkwed")).Checked == false && ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkThur")).Checked == false && ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkFri")).Checked == false && ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSat")).Checked == false && ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSun")).Checked == false)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please Check Frequency for flights in row " + (0 + 1);
                        return false;
                    }
                    else
                    {
                        strFreq = "";


                        if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkMon")).Checked == true)
                            strFreq = "1";
                        else
                            strFreq = "0";

                        if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkTues")).Checked == true)
                            strFreq = strFreq + ',' + "1";
                        else
                            strFreq = strFreq + ',' + "0";

                        if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkwed")).Checked == true)
                            strFreq = strFreq + ',' + "1";
                        else
                            strFreq = strFreq + ',' + "0";



                        if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkThur")).Checked == true)
                            strFreq = strFreq + ',' + "1";
                        else
                            strFreq = strFreq + ',' + "0";



                        if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkFri")).Checked == true)
                            strFreq = strFreq + ',' + "1";
                        else
                            strFreq = strFreq + ',' + "0";


                        if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSat")).Checked == true)
                            strFreq = strFreq + ',' + "1";
                        else
                            strFreq = strFreq + ',' + "0";


                        if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSun")).Checked == true)
                            strFreq = strFreq + ',' + "1";
                        else
                            strFreq = strFreq + ',' + "0";



                    }
                }
                catch (Exception)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Check Frequency for flights in row " + (0 + 1);

                    return false;
                }

                TextBox txtCapacity = ((TextBox)grdScheduleinfo.Rows[i].FindControl("txtCapacity"));
                try
                {


                    if (txtCapacity.Text.Trim() == "")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid Cargo Capacity(row :" + (i + 1) + ")";
                        txtCapacity.Focus();
                        return false;
                    }
                    else if (float.Parse(txtCapacity.Text) < 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid Cargo Capacity (row :" + (i + 1) + ")";
                        txtCapacity.Focus();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid Cargo Capacity(row :" + (i + 1) + ")";
                    txtCapacity.Focus();
                    return false;

                }




                //Validate Source Wise Data from database
                try
                {


                    try
                    {
                        DateTime dt1from = DateTime.ParseExact(txtFromdate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                        DateTime dt1to = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);

                        try
                        {// if ( Convert.ToDateTime(txtFromdate.Text) > Convert.ToDateTime(txtToDate.Text))

                            DataSet dsCheck = OBJasb.GetPartnerScheduleforflight(ddlOrigin.SelectedValue, ddlFlight.Text, strFreq, dt1from, dt1to);
                            if (dsCheck != null)
                            {
                                if (dsCheck.Tables != null)
                                {
                                    if (dsCheck.Tables.Count > 0 && dsCheck.Tables[0].Rows.Count > 0)
                                    {
                                        //    Mohan
                                        //    lblStatus.ForeColor = Color.Red;
                                        //    lblStatus.Text = "FlightID already register for this origin and frequency.";
                                        //    return false;

                                        // mod Vikas 26 Jul
                                        //for (int cnt = 0; cnt < dsCheck.Tables[0].Rows.Count;cnt++ )
                                        //{
                                        //    if (strFreq == dsCheck.Tables[0].Rows[cnt][2].ToString())
                                        //    {
                                        //        lblStatus.ForeColor = Color.Red;
                                        //        lblStatus.Text = "FlightID already register for this origin and frequency.";
                                        //        return false;
                                        //    }
                                        //}
                                        // mod Vikas 26 Jul
                                        #region Check for Frequency against DB
                                        for (int rc = 0; rc < dsCheck.Tables[0].Rows.Count; rc++)//mod vikas 26Jul
                                        {

                                            for (int k = 0; k < strFreq.Length; k++)
                                            {
                                                string Frequency = dsCheck.Tables[0].Rows[rc][2].ToString();
                                                string stract = Frequency[k].ToString();
                                                string newfreq = strFreq[k].ToString();
                                                if (stract != "," || newfreq != ",")
                                                {
                                                    if (stract == newfreq)
                                                    {
                                                        if (stract == Convert.ToString(1))
                                                        {
                                                            lblStatus.ForeColor = Color.Red;
                                                            lblStatus.Text = "FlightID already register for this frequency";
                                                            return false;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                        //
                                    }
                                    else
                                    {

                                    }
                                }
                            }

                        }
                        catch (Exception ex)
                        {

                        }

















                        // if ( Convert.ToDateTime(txtFromdate.Text) > Convert.ToDateTime(txtToDate.Text))
                        //  DateTime dt1from=Convert.ToDateTime(txtFromdate.Text);
                        //     DateTime dt1to= Convert.ToDateTime(txtToDate.Text);

                        //DateTime dt1from = DateTime.ParseExact(txtFromdate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                        //DateTime dt1to = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null); 

                        DataSet dsCheckRoute = OBJasb.GetPartnerScheduleRouteforflight(tempSource.SelectedValue, tempDest.SelectedValue, ddlFlight.Text, strFreq, dt1from, dt1to);
                        if (dsCheckRoute != null)
                        {
                            if (dsCheckRoute.Tables != null)
                            {
                                if (dsCheckRoute.Tables.Count > 0 && dsCheckRoute.Tables[0].Rows.Count > 0)
                                {


                                    //Orignal Mohan
                                    for (int rc = 0; rc < dsCheckRoute.Tables[0].Rows.Count; rc++)//mod vikas 26Jul
                                    {
                                        #region Check for Frequency against DB
                                        for (int k = 0; k < strFreq.Length; k++)
                                        {
                                            string Frequency = dsCheckRoute.Tables[0].Rows[rc][0].ToString();
                                            string stract = Frequency[k].ToString();
                                            string newfreq = strFreq[k].ToString();
                                            if (stract != "," || newfreq != ",")
                                            {
                                                if (stract == newfreq)
                                                {
                                                    if (stract == Convert.ToString(1))
                                                    {
                                                        lblStatus.ForeColor = Color.Red;
                                                        lblStatus.Text = "FlightID already register for this frequency in route (row :" + (i + 1) + ")";
                                                        return false;
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        #region CheckAircraftType and Arrival Depeart Time //Vikas

                                        string deptTime = ((TextBox)grdScheduleinfo.Rows[0].FindControl("txtDeptTimeHr")).Text; //;// 
                                        deptTime = deptTime + ":" + ((TextBox)grdScheduleinfo.Rows[0].FindControl("txtDeptTimeMin")).Text;

                                        string arrTime = ((TextBox)grdScheduleinfo.Rows[0].FindControl("txtArrivaltimeHr")).Text;
                                        arrTime = arrTime + ":" + ((TextBox)grdScheduleinfo.Rows[0].FindControl("txtArrivaltimeMin")).Text;

                                        string acType = ((DropDownList)grdScheduleinfo.Rows[0].FindControl("ddlAirCraft")).Text;

                                        bool timeIsValid = true, acTypeIsValid = true;
                                        if (arrTime == dsCheckRoute.Tables[0].Rows[rc][2].ToString() || deptTime == dsCheckRoute.Tables[0].Rows[rc][3].ToString())
                                        {
                                            timeIsValid = false;
                                        }
                                        if (acType == dsCheckRoute.Tables[0].Rows[rc][1].ToString())
                                        {
                                            acTypeIsValid = false;
                                        }
                                        bool ISValidLoc = true;
                                        if (ddlOrigin.Text == dsCheckRoute.Tables[0].Rows[rc][4].ToString() && ddlDestination.Text == dsCheckRoute.Tables[0].Rows[rc][4].ToString())
                                        {
                                            ISValidLoc = false;
                                        }

                                        if (acTypeIsValid == false && timeIsValid == false && ISValidLoc == false)
                                        {


                                            lblStatus.ForeColor = Color.Red;
                                            lblStatus.Text = "Cannot register FlightID for this frequency in route (row :" + (i + 1) + ") please check Arrival-Departure time and Aircraft Type";
                                            return false;

                                        }


                                        #endregion


                                    }
                                    //
                                    //Mod Vikas 26Jul
                                    // for (int g = 0; g < grdScheduleinfo.Rows.Count; g++) 
                                    //{
                                    //    string GrdFreq = 


                                    //    for (int r = 0; r < dsCheckRoute.Tables[0].Rows.Count; r++)
                                    //    {
                                    //        string TabFreq = dsCheckRoute.Tables[0].Rows[r][0].ToString();


                                    //    }

                                    //}

                                    //
                                }
                                else
                                {

                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {

                    }


                    if (grdScheduleinfo.Rows.Count > 1)
                    {
                        if (i > 0)
                        {
                            tempSource = (DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlFromOrigin"));
                            tempDest = (DropDownList)(grdScheduleinfo.Rows[i - 1].FindControl("ddlToDest"));

                            if (tempSource.Text != tempDest.Text)
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Please Select From Source Code in route in row " + (i + 1);
                                tempSource.Focus();
                                return false;

                            }
                            txtCheckArrDay = (TextBox)(grdScheduleinfo.Rows[i - 1].FindControl("txtArrivalDay"));
                            txtCheckDeptDay = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptDay"));
                            if (int.Parse(txtCheckArrDay.Text) > int.Parse(txtCheckDeptDay.Text))
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Please enter valid Departure Day in row " + (i + 1);
                                txtCheckArrDay.Focus();
                                return false;
                            }

                            //tempDeprtureTimeMin = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeMin"));

                            //tempArrivalTimeMin = (TextBox)(grdScheduleinfo.Rows[i - 1].FindControl("txtArrivalTimeMin"));
                            //if (int.Parse(tempDeprtureTimeMin.Text) < int.Parse(tempArrivalTimeMin.Text))
                            //{
                            //    lblStatus.ForeColor = Color.Red;
                            //    lblStatus.Text = "Please enter valid Departure Time in Min";
                            //    tempDeprtureTimeMin.Focus();
                            //    return false;
                            //}


                            tempArrivalTimeHr = (TextBox)(grdScheduleinfo.Rows[i - 1].FindControl("txtArrivaltimeHr"));
                            tempdeptTimeHr = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeHr"));
                            int arrday = int.Parse(txtCheckArrDay.Text);
                            int DeptDay = int.Parse(txtCheckDeptDay.Text);
                            tempDeprtureTimeMin = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeMin"));

                            tempArrivalTimeMin = (TextBox)(grdScheduleinfo.Rows[i - 1].FindControl("txtArrivalTimeMin"));


                            int ArrTimeHr = int.Parse(tempArrivalTimeHr.Text);
                            int DeptTimeHr = int.Parse(tempdeptTimeHr.Text);
                            if ((ArrTimeHr > DeptTimeHr) && (arrday > DeptDay))
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Please enter valid arrival time in Hr in row " + (i + 1);
                                tempdeptTimeHr.Focus();
                                return false;
                            }
                            else if ((ArrTimeHr > DeptTimeHr) && (arrday == DeptDay))
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Please enter valid Departure time in Hr in row " + (i + 1);
                                tempdeptTimeHr.Focus();
                                return false;
                            }
                            else if ((ArrTimeHr == DeptTimeHr) && (arrday < DeptDay))
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Please enter valid arrival time in Hr in row " + (i + 1);
                                tempdeptTimeHr.Focus();
                                return false;
                            }
                            else if (arrday > DeptDay)
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Please enter valid departure day in row " + (i + 1);
                                txtCheckArrDay.Focus();
                                return false;
                            }
                            else if ((ArrTimeHr == DeptTimeHr) && (arrday == DeptDay) && (int.Parse(tempDeprtureTimeMin.Text)) < (int.Parse(tempArrivalTimeMin.Text)))
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Please enter valid Departure time in Min in row " + (i + 1);
                                tempdeptTimeHr.Focus();
                                return false;
                            }
                            else if ((ArrTimeHr == DeptTimeHr) && (arrday == DeptDay) && (int.Parse(tempDeprtureTimeMin.Text)) == (int.Parse(tempArrivalTimeMin.Text)))
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Please enter valid Departure time in row " + (i + 1);
                                tempdeptTimeHr.Focus();
                                return false;
                            }





                        }


                    }

                    //  int CheckDest = grdScheduleinfo.Rows.Count;
                    DropDownList ChecktempDest;
                    ChecktempDest = (DropDownList)(grdScheduleinfo.Rows[grdScheduleinfo.Rows.Count - 1].FindControl("ddlToDest"));
                    if (ChecktempDest.Text != ddlDestination.SelectedValue)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid To Destination Code in route (row " + (grdScheduleinfo.Rows.Count - 1) + ")";

                        ChecktempDest.Focus();
                        return false;
                    }


                }
                catch (Exception ex)
                {

                }

                //Validate Weeekdays check
                try
                {
                    if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkMon")).Checked == false && ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkTues")).Checked == false && ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkwed")).Checked == false && ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkThur")).Checked == false && ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkFri")).Checked == false && ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSat")).Checked == false && ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSun")).Checked == false)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please Check Frequency for flights in row " + (i + 1);
                        return false;
                    }
                }
                catch (Exception)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Check Frequency for flights in row " + (i + 1);

                    return false;
                }

            }

            return true;
        }
        #endregion Validate Data

        #region Filling DropDownList With Aircraft

        private void AirCraftType()
        {
            try
            {

                DataSet ds = OBJasb.GetAirCraftType();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            //Aircraft in Gridview Dropdown
                            DataRow row = ds.Tables[0].NewRow();

                            row[ds.Tables[0].Columns[0].ColumnName] = "Select";
                            ds.Tables[0].Rows.Add(row);


                            ddlAirCraftType.DataSource = ds;
                            ddlAirCraftType.DataMember = ds.Tables[0].TableName;
                            ddlAirCraftType.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            ddlAirCraftType.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                            ddlAirCraftType.DataBind();
                            ddlAirCraftType.Text = "Select";

                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion Aircraft type List

        #region Filling Aircraft Type & Capacity in GridView

        private void AirCraftTypeinGridview()
        {
            try
            {

                DataSet ds = OBJasb.GetAirCraftType();

                for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
                {
                    DropDownList ddlAircraft = (DropDownList)grdScheduleinfo.Rows[i].FindControl("ddlAirCraft");
                    if (ds != null)
                    {
                        if (ds.Tables != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                //Aircraft in Gridview Dropdown
                                DataRow row = ds.Tables[0].NewRow();

                                //row[ds.Tables[0].Columns[0].ColumnName] = "Select";
                                //ds.Tables[0].Rows.Add(row);


                                ddlAircraft.DataSource = ds;
                                ddlAircraft.DataMember = ds.Tables[0].TableName;
                                ddlAircraft.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                                ddlAircraft.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                                ddlAircraft.DataBind();
                                //ddlAircraft.Text = "Select";


                                DataSet dsCapacity = OBJasb.GetCargoCapacity(ds.Tables[0].Rows[0][0].ToString());
                                if (dsCapacity != null)
                                {
                                    if (dsCapacity.Tables != null)
                                    {
                                        if (dsCapacity.Tables.Count > 0 || dsCapacity.Tables[0].Rows.Count > 0)
                                        {
                                            txtCargoCapacity.Text = dsCapacity.Tables[0].Rows[0][0].ToString();
                                            //txtCapacity
                                            ((TextBox)grdScheduleinfo.Rows[i].FindControl("txtCapacity")).Text = dsCapacity.Tables[0].Rows[0][0].ToString();
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion Aircraft type List in gridview

        #region DDL Origin Index Change EventHandler
        protected void ddlOrigin_SelectedIndexChanged(object sender, EventArgs e)
        {

            DropDownList ddl = (DropDownList)(grdScheduleinfo.Rows[0].FindControl("ddlFromOrigin"));
            ddl.SelectedValue = ddlOrigin.SelectedValue;
            ddl.Enabled = false;
        }
        # endregion

        #region DDL AircraftType Index Change EventHandler
        protected void ddlAirCraftType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                DataSet ds = OBJasb.GetCargoCapacity(ddlAirCraftType.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0 || ds.Tables[0].Rows.Count > 0)
                        {
                            txtCargoCapacity.Text = ds.Tables[0].Rows[0][0].ToString();
                            //txtCapacity

                            for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
                            {

                                ((TextBox)grdScheduleinfo.Rows[i].FindControl("txtCapacity")).Text = ds.Tables[0].Rows[0][0].ToString();
                                ((DropDownList)grdScheduleinfo.Rows[i].FindControl("ddlAirCraft")).Text = ddlAirCraftType.SelectedValue;
                                showCapacityInGrid(null, null);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

        }
        #endregion

        #region DDL Destination Index Change EventHandler
        protected void ddlDestination_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)(grdScheduleinfo.Rows[0].FindControl("ddlToDest"));
            ddl.SelectedValue = ddlDestination.SelectedValue;

        }
        #endregion

        #region Add New Row to Grid
        protected void btnAddNewRow_Click(object sender, EventArgs e)
        {
            try
            {
                AddNewRowToGrid();
                LoadSourceInGridview();
                LoadDestinationInGridview();
                AirCraftTypeinGridview();
                ValidateGridDays();

            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region Save Grid State
        public void SaveGridState()
        {
            try
            {

                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                //  dtCurrentTable.Rows.Clear();
                if (grdScheduleinfo.Rows.Count > 0)
                {
                    for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
                    {
                        //extract the TextBox values

                        drCurrentRow = dtCurrentTable.NewRow();
                        //  dtCurrentTable.Rows.Add(drCurrentRow);
                        i = grdScheduleinfo.Rows.Count - 1;
                        drCurrentRow["RowNumber"] = i;
                        dtCurrentTable.Rows[i]["RowNumber"] = i + 1;
                        dtCurrentTable.Rows[i]["From"] = ((DropDownList)grdScheduleinfo.Rows[i].Cells[0].FindControl("ddlFromOrigin")).Text;
                        dtCurrentTable.Rows[i]["To"] = ((DropDownList)grdScheduleinfo.Rows[i].Cells[1].FindControl("ddlToDest")).Text;

                        dtCurrentTable.Rows[i]["DeptDay"] = ((TextBox)grdScheduleinfo.Rows[i].Cells[2].FindControl("txtDeptDay")).Text;
                        dtCurrentTable.Rows[i]["Dept TimeHr"] = ((TextBox)grdScheduleinfo.Rows[i].Cells[2].FindControl("txtDeptTimeHr")).Text;
                        dtCurrentTable.Rows[i]["Dept TimeMin"] = ((TextBox)grdScheduleinfo.Rows[i].Cells[2].FindControl("txtDeptTimeMin")).Text;

                        dtCurrentTable.Rows[i]["ArrDay"] = ((TextBox)grdScheduleinfo.Rows[i].Cells[1].FindControl("txtArrivalDay")).Text;

                        dtCurrentTable.Rows[i]["Arrival TimeHr"] = ((TextBox)grdScheduleinfo.Rows[i].Cells[1].FindControl("txtArrivaltimeHr")).Text;
                        dtCurrentTable.Rows[i]["Arrival TimeMin"] = ((TextBox)grdScheduleinfo.Rows[i].Cells[1].FindControl("txtArrivaltimeMin")).Text;

                        dtCurrentTable.Rows[i]["chkMon"] = ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkMon")).Checked;
                        dtCurrentTable.Rows[i]["chkTues"] = ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkTues")).Checked;
                        dtCurrentTable.Rows[i]["chkwed"] = ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkwed")).Checked;
                        dtCurrentTable.Rows[i]["chkThur"] = ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkThur")).Checked;
                        dtCurrentTable.Rows[i]["chkFri"] = ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkFri")).Checked;
                        dtCurrentTable.Rows[i]["chkSat"] = ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkSat")).Checked;
                        dtCurrentTable.Rows[i]["chkSun"] = ((CheckBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("chkSun")).Checked;
                        dtCurrentTable.Rows[i]["Status"] = ((DropDownList)grdScheduleinfo.Rows[i].Cells[3].FindControl("ddlStatus")).Text;

                    }
                    ViewState["CurrentTable"] = "";
                    ViewState["CurrentTable"] = dtCurrentTable;


                }
            }
            catch (Exception)
            {


            }
        }
        #endregion

        #region Delete row from Route grid
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (grdScheduleinfo.Rows.Count > 1)
            {
                //Save Grid state
                SaveGridState();

                //  dsSlabs = (DataSet)Session["dsSlabs"];
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                //  DataSet dsSlabsTemp = dsSlabs.Copy();

                for (int i = 0; i < dt.Rows.Count; i++)
                { //((CheckBox)grdScheduleinfo.Rows[2].FindControl("CHK")).Checked=true;

                    if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("CHK")).Checked == true)
                    {
                        string RowNumber = dt.Rows[i]["RowNumber"].ToString();

                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            if (RowNumber == dt.Rows[j]["RowNumber"].ToString())
                            {
                                dt.Rows.Remove(dt.Rows[j]);
                                break;
                            }
                        }
                    }
                }

                grdScheduleinfo.DataSource = null;
                grdScheduleinfo.DataSource = dt;
                grdScheduleinfo.DataBind();
                LoadSourceInGridview();
                LoadDestinationInGridview();
                AirCraftTypeinGridview();


                ViewState["CurrentTable"] = dt.Copy();
            }

        }
        #endregion

        #region DDL AirCraft IndexChanged Event Handler
        protected void ddlAirCraft_SelectedIndexChanged(object sender, EventArgs e)
        {

            //try
            //{


            //    string aircrafttype = ((DropDownList)grdScheduleinfo.Rows[].FindControl("")).SelectedItem(.ToString();

            //    DataSet ds = OBJasb.GetCargoCapacity(aircrafttype);
            //    if (ds != null)
            //    {
            //        if (ds.Tables != null)
            //        {
            //            if (ds.Tables.Count > 0 || ds.Tables[0].Rows.Count > 0)
            //            {
            //                txtCargoCapacity.Text = ds.Tables[0].Rows[0][0].ToString();
            //                //txtCapacity
            //                ((TextBox)grdScheduleinfo.Rows[i].FindControl("txtCapacity")).Text = ds.Tables[0].Rows[0][0].ToString();
            //            }
            //        }
            //    }
            //}

            //catch (Exception)
            //{
            //}
        }
        #endregion

        #region FromDate & ToDate TextChanged Event Handler
        protected void txtFromdate_TextChanged(object sender, EventArgs e)
        {
            ValidateGridDays();


        }

        protected void txtToDate_TextChanged(object sender, EventArgs e)
        {
            ValidateGridDays();

        }
        #endregion

        #region Validate Days in GridView
        protected void ValidateGridDays()//mod Vikas 31 Jul
        {
            try
            {
                if (txtFromdate.Text != "" && txtToDate.Text != "")
                {
                    int i = 0;
                    while (i < grdScheduleinfo.Rows.Count)
                    {
                        DateTime dt1 = DateTime.ParseExact(txtFromdate.Text, "dd/MM/yyyy", null);
                        DateTime dt2 = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);

                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSun")).Enabled = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkMon")).Enabled = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkTues")).Enabled = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkwed")).Enabled = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkThur")).Enabled = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkFri")).Enabled = false;
                        ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSat")).Enabled = false;

                        while (dt1 <= dt2)
                        {


                            string str = dt1.ToString("ddd");
                            if (str == "Sun")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSun")).Enabled = true;
                            }
                            if (str == "Mon")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkMon")).Enabled = true;
                            }
                            if (str == "Tue")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkTues")).Enabled = true;
                            }
                            if (str == "Wed")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkwed")).Enabled = true;
                            }
                            if (str == "Thu")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkThur")).Enabled = true;
                            }
                            if (str == "Fri")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkFri")).Enabled = true;
                            }
                            if (str == "Sat")
                            {
                                ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSat")).Enabled = true;
                            }
                            dt1 = dt1.AddDays(1);
                        }


                        //Uncheck Disabled Fields
                        if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSun")).Enabled == false)
                        {
                            ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSun")).Checked = false;
                        }
                        if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkMon")).Enabled == false)
                        {
                            ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkMon")).Checked = false;
                        }
                        if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkTues")).Enabled == false)
                        {
                            ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkTues")).Checked = false;
                        }
                        if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkwed")).Enabled == false)
                        {
                            ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkwed")).Checked = false;
                        }
                        if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkThur")).Enabled == false)
                        {
                            ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkThur")).Checked = false;
                        }
                        if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkFri")).Enabled == false)
                        {
                            ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkFri")).Checked = false;
                        }
                        if (((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSat")).Enabled == false)
                        {
                            ((CheckBox)grdScheduleinfo.Rows[i].FindControl("chkSat")).Checked = false;
                        }

                        i++;
                    }

                }
            }
            catch (Exception)
            {


            }

        }
        # endregion

        #region Close
        protected void BtnCLose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Home.aspx", false);
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Fill Partner Type Master
        public void FillPartnerType()
        {
            try
            {
                PartnerBAL objPM = new PartnerBAL();
                DataSet dsPartnerType = objPM.GetPartnerTypeMaster();
                if (dsPartnerType != null && dsPartnerType.Tables.Count > 0)
                {
                    if (dsPartnerType.Tables[0].Rows.Count > 0)
                    {
                        drpPartnerType.Items.Clear();
                        //drpWWR.Items.Add("Select");
                        drpPartnerType.DataSource = dsPartnerType.Tables[0];
                        drpPartnerType.DataTextField = "Code";
                        drpPartnerType.DataValueField = "Description";
                        drpPartnerType.DataBind();
                        drpPartnerType.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = "Error in Fill PartnerType!";
            }
        }
        #endregion

        #region SSIM button
        protected void btnSSIM_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                if (FileUpload_SSIM.HasFile)
                {
                    //if (FileUpload_SSIM.FileName.Substring(FileUpload_SSIM.FileName.Length - 5) == ".ssim")
                    //{
                        GetSSIMData(FileUpload_SSIM);
                    //}
                    //else
                    //{
                    //    lblStatus.Text = "Please select a valid SSIM file to export!";
                    //    lblStatus.ForeColor = Color.Red;
                    //}

                }
                else
                {
                    lblStatus.Text = "Please select a file to Import!";
                    lblStatus.ForeColor = Color.Red;
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion

        #region SSIM Export
        public void GetSSIMData(FileUpload SSIMUpload)
        {
            try
            {
                string[] DesignationCode = new string[0];
                string[] AirlinePrefix = new string[0];
                string[] ArrivalTimeZone = new string[0];
                string[] TailNo = new string[0];
                string[] Constant = new string[0];
                string[] Origin = new string[0];
                string[] Destination = new string[0];
                string[] AirCraftType = new string[0];
                string[] AirlineFrequency = new string[0];
                string[] DepartureTimeZone = new string[0];
                string[] Arrival = new string[0];
                string[] Departure = new string[0];
                string[] FilghtID = new string[0];
                string[] FlightNo = new string[0];
                string[] FromDate = new string[0];
                string[] ToDate = new string[0];
                string[] RowID = new string[0];
                string[] Frequency = new string[0];
                string[] FinalFrequency = new string[0];
                string[] AirFrequency = new string[0];
                string[] AirSchMasterFlightID = new string[0];
                string[] AirSchMasterOrigin = new string[0];
                string[] AirSchMasterDest = new string[0];
                string[] AirSchMasterArrivalTime = new string[0];
                string[] AirSchMasterDepartureTime = new string[0];
                string[] AirSchDeptTimeZone = new string[0];
                string[] AirSchArrTimeZone = new string[0];
                string[] AirSchFromDate = new string[0];
                string[] AirSchToDate = new string[0];
                string[] AirSchTailNo = new string[0];
                string[] AirSchAirCraftType = new string[0];
                string[] AirSchFrequency = new string[0];
                string[] AirSchAirlinePrefix = new string[0];

                if (FileUpload_SSIM.HasFile)
                {
                    Stream Content = FileUpload_SSIM.FileContent;
                    StreamReader sr = new StreamReader(Content);

                    do
                    {
                        string s = sr.ReadLine();


                        if (s.StartsWith("3"))
                        {
                            Array.Resize(ref RowID, RowID.Length + 1);
                            RowID[RowID.Length - 1] = s.Substring(0, 1);
                            Array.Resize(ref Constant, Constant.Length + 1);
                            Constant[Constant.Length - 1] = s.Substring(6, 8);
                            Array.Resize(ref FromDate, FromDate.Length + 1);
                            FromDate[FromDate.Length - 1] = s.Substring(14, 7);
                            FromDate[FromDate.Length - 1] = DateTime.ParseExact(FromDate[FromDate.Length - 1], "ddMMMyy", CultureInfo.InvariantCulture,
                                        DateTimeStyles.None).ToString("MM/dd/yyyy");
                            Array.Resize(ref ToDate, ToDate.Length + 1);
                            ToDate[ToDate.Length - 1] = s.Substring(21, 7);
                            ToDate[ToDate.Length - 1] = DateTime.ParseExact(ToDate[ToDate.Length - 1], "ddMMMyy", CultureInfo.InvariantCulture,
                                        DateTimeStyles.None).ToString("MM/dd/yyyy");
                            Array.Resize(ref DesignationCode, DesignationCode.Length + 1);
                            DesignationCode[DesignationCode.Length - 1] = s.Substring(2, 2);
                            Array.Resize(ref AirlineFrequency, AirlineFrequency.Length + 1);
                            AirlineFrequency[AirlineFrequency.Length - 1] = s.Substring(28, 7);
                            Array.Resize(ref Origin, Origin.Length + 1);
                            Origin[Origin.Length - 1] = s.Substring(36, 3);
                            Array.Resize(ref Departure, Departure.Length + 1);
                            Departure[Departure.Length - 1] = s.Substring(39, 4);
                            Array.Resize(ref DepartureTimeZone, DepartureTimeZone.Length + 1);
                            DepartureTimeZone[DepartureTimeZone.Length - 1] = s.Substring(43, 9);
                            Array.Resize(ref Arrival, Arrival.Length + 1);
                            Array.Resize(ref Destination, Destination.Length + 1);
                            Destination[Destination.Length - 1] = s.Substring(54, 3);
                            Arrival[Arrival.Length - 1] = s.Substring(57, 4);
                            Array.Resize(ref ArrivalTimeZone, ArrivalTimeZone.Length + 1);
                            ArrivalTimeZone[ArrivalTimeZone.Length - 1] = s.Substring(61, 9);
                            Array.Resize(ref AirCraftType, AirCraftType.Length + 1);
                            AirCraftType[AirCraftType.Length - 1] = s.Substring(72, 3);
                            Array.Resize(ref AirlinePrefix, AirlinePrefix.Length + 1);
                            AirlinePrefix[AirlinePrefix.Length - 1] = s.Substring(137, 2);
                            Array.Resize(ref FlightNo, FlightNo.Length + 1);
                            FlightNo[FlightNo.Length - 1] = s.Substring(140, 4);
                            Array.Resize(ref TailNo, TailNo.Length + 1);
                            TailNo[TailNo.Length - 1] = s.Substring(172, 13);
                            Array.Resize(ref FilghtID, FilghtID.Length + 1);
                            FilghtID[FilghtID.Length - 1] = s.Substring(137, 2) + s.Substring(140, 4);


                        }
                        // do your coding 
                        //Loop trough txt file and add lines to ListBox1  

                    }
                    while (sr.Peek() != -1);
                    sr.Close();
                    // GetSSIMData(System.IO.Path.GetFullPath(FileUpload_SSIM.PostedFile.FileName));

                }

                //if (File.Exists(Server.MapPath(FilePath)))
                //{
                //foreach (string s in )
                //{
                //    if (s.StartsWith("3"))
                //    {
                //        Array.Resize(ref RowID, RowID.Length + 1);
                //        RowID[RowID.Length - 1] = s.Substring(0, 1);
                //        Array.Resize(ref Constant, Constant.Length + 1);
                //        Constant[Constant.Length - 1] = s.Substring(6, 8);
                //        Array.Resize(ref FromDate, FromDate.Length + 1);
                //        FromDate[FromDate.Length - 1] = s.Substring(14, 7);
                //        FromDate[FromDate.Length - 1] = DateTime.ParseExact(FromDate[FromDate.Length - 1], "ddMMMyy", CultureInfo.InvariantCulture,
                //                    DateTimeStyles.None).ToString("MM/dd/yyyy");
                //        Array.Resize(ref ToDate, ToDate.Length + 1);
                //        ToDate[ToDate.Length - 1] = s.Substring(21, 7);
                //        ToDate[ToDate.Length - 1] = DateTime.ParseExact(ToDate[ToDate.Length - 1], "ddMMMyy", CultureInfo.InvariantCulture,
                //                    DateTimeStyles.None).ToString("MM/dd/yyyy");
                //        Array.Resize(ref DesignationCode, DesignationCode.Length + 1);
                //        DesignationCode[DesignationCode.Length - 1] = s.Substring(2, 2);
                //        Array.Resize(ref AirlineFrequency, AirlineFrequency.Length + 1);
                //        AirlineFrequency[AirlineFrequency.Length - 1] = s.Substring(28, 7);
                //        Array.Resize(ref Origin, Origin.Length + 1);
                //        Origin[Origin.Length - 1] = s.Substring(36, 3);
                //        Array.Resize(ref Departure, Departure.Length + 1);
                //        Departure[Departure.Length - 1] = s.Substring(39, 4);
                //        Array.Resize(ref DepartureTimeZone, DepartureTimeZone.Length + 1);
                //        DepartureTimeZone[DepartureTimeZone.Length - 1] = s.Substring(43, 9);
                //        Array.Resize(ref Arrival, Arrival.Length + 1);
                //        Array.Resize(ref Destination, Destination.Length + 1);
                //        Destination[Destination.Length - 1] = s.Substring(54, 3);
                //        Arrival[Arrival.Length - 1] = s.Substring(57, 4);
                //        Array.Resize(ref ArrivalTimeZone, ArrivalTimeZone.Length + 1);
                //        ArrivalTimeZone[ArrivalTimeZone.Length - 1] = s.Substring(61, 9);
                //        Array.Resize(ref AirCraftType, AirCraftType.Length + 1);
                //        AirCraftType[AirCraftType.Length - 1] = s.Substring(72, 3);
                //        Array.Resize(ref AirlinePrefix, AirlinePrefix.Length + 1);
                //        AirlinePrefix[AirlinePrefix.Length - 1] = s.Substring(137, 2);
                //        Array.Resize(ref FlightNo, FlightNo.Length + 1);
                //        FlightNo[FlightNo.Length - 1] = s.Substring(140, 4);
                //        Array.Resize(ref TailNo, TailNo.Length + 1);
                //        TailNo[TailNo.Length - 1] = s.Substring(172, 13);
                //        Array.Resize(ref FilghtID, FilghtID.Length + 1);
                //        FilghtID[FilghtID.Length - 1] = s.Substring(137, 2) + s.Substring(140, 4);


                //    }

                //}
                // }
                for (int i = 0; i < AirlineFrequency.Length; i++)
                {
                    string zero = "";
                    if (AirlineFrequency[i].Contains<char>(' '))
                    {
                        zero = AirlineFrequency[i].Replace(" ", "0");
                    }
                    else
                        zero = AirlineFrequency[i];
                    Array.Resize(ref Frequency, Frequency.Length + 1);
                    Frequency[Frequency.Length - 1] = zero;
                }
                for (int i = 0; i < Frequency.Length; i++)
                {
                    char[] chararr = new char[0];

                    foreach (char ch in Frequency[i])
                    {


                        if (ch == '2' || ch == '3' || ch == '4' || ch == '5' || ch == '6' || ch == '7')
                        {
                            Array.Resize(ref chararr, chararr.Length + 1);
                            chararr[chararr.Length - 1] = '1';

                        }
                        else
                        {
                            Array.Resize(ref chararr, chararr.Length + 1);
                            chararr[chararr.Length - 1] = ch;
                        }


                    }
                    Array.Resize(ref FinalFrequency, FinalFrequency.Length + 1);
                    FinalFrequency[FinalFrequency.Length - 1] = new string(chararr);


                }
                char[] finalchar = new char[0];
                for (int j = 0; j < FinalFrequency.Length; j++)
                {

                    FinalFrequency[j] = FinalFrequency[j].Insert(1, ",");
                    FinalFrequency[j] = FinalFrequency[j].Insert(3, ",");
                    FinalFrequency[j] = FinalFrequency[j].Insert(5, ",");
                    FinalFrequency[j] = FinalFrequency[j].Insert(7, ",");
                    FinalFrequency[j] = FinalFrequency[j].Insert(9, ",");
                    FinalFrequency[j] = FinalFrequency[j].Insert(11, ",");

                }
                for (int i = FilghtID.Length - 1; i > 0; i--)
                {
                    if (FilghtID[i].Contains("      "))
                    {
                        // FilghtID[i] = FilghtID[i + 1];
                    }
                    else
                    {

                        Array.Resize(ref AirSchMasterDest, AirSchMasterDest.Length + 1);
                        AirSchMasterDest[AirSchMasterDest.Length - 1] = Destination[i];
                        Array.Resize(ref AirSchMasterArrivalTime, AirSchMasterArrivalTime.Length + 1);
                        AirSchMasterArrivalTime[AirSchMasterArrivalTime.Length - 1] = Arrival[i];
                        Array.Resize(ref AirSchArrTimeZone, AirSchArrTimeZone.Length + 1);
                        AirSchArrTimeZone[AirSchArrTimeZone.Length - 1] = ArrivalTimeZone[i];
                        Array.Resize(ref AirSchMasterFlightID, AirSchMasterFlightID.Length + 1);
                        AirSchMasterFlightID[AirSchMasterFlightID.Length - 1] = FilghtID[i];
                        Array.Resize(ref AirSchToDate, AirSchToDate.Length + 1);
                        AirSchToDate[AirSchToDate.Length - 1] = ToDate[i];
                        Array.Resize(ref AirSchAirlinePrefix, AirSchAirlinePrefix.Length + 1);
                        AirSchAirlinePrefix[AirSchAirlinePrefix.Length - 1] = AirlinePrefix[i];
                        Array.Resize(ref AirSchMasterOrigin, AirSchMasterOrigin.Length + 1);
                        AirSchMasterOrigin[AirSchMasterOrigin.Length - 1] = Origin[i];
                        Array.Resize(ref AirSchFromDate, AirSchFromDate.Length + 1);
                        AirSchFromDate[AirSchFromDate.Length - 1] = FromDate[i];
                        Array.Resize(ref AirSchTailNo, AirSchTailNo.Length + 1);
                        AirSchTailNo[AirSchTailNo.Length - 1] = TailNo[i];
                        Array.Resize(ref AirSchFrequency, AirSchFrequency.Length + 1);
                        AirSchFrequency[AirSchFrequency.Length - 1] = FinalFrequency[i];
                        Array.Resize(ref AirSchMasterDepartureTime, AirSchMasterDepartureTime.Length + 1);
                        AirSchMasterDepartureTime[AirSchMasterDepartureTime.Length - 1] = Departure[i];
                        Array.Resize(ref AirSchDeptTimeZone, AirSchDeptTimeZone.Length + 1);
                        AirSchDeptTimeZone[AirSchDeptTimeZone.Length - 1] = DepartureTimeZone[i];
                        Array.Resize(ref AirSchAirCraftType, AirSchAirCraftType.Length + 1);
                        AirSchAirCraftType[AirSchAirCraftType.Length - 1] = AirCraftType[i];


                    }

                }

                #region Commented Code for Partner Schedule Upload Latest
                //for (int i = 0; i < FilghtID.Length; i++)
                //{


                //    if (i == 0 && FlightNo[i] != "    ")
                //    {
                //        Array.Resize(ref AirSchMasterDest, AirSchMasterDest.Length + 1);
                //        AirSchMasterDest[AirSchMasterDest.Length - 1] = Destination[i];
                //        Array.Resize(ref AirSchMasterArrivalTime, AirSchMasterArrivalTime.Length + 1);
                //        AirSchMasterArrivalTime[AirSchMasterArrivalTime.Length - 1] = Arrival[i];
                //        Array.Resize(ref AirSchArrTimeZone, AirSchArrTimeZone.Length + 1);
                //        AirSchArrTimeZone[AirSchArrTimeZone.Length - 1] = ArrivalTimeZone[i];
                //        Array.Resize(ref AirSchMasterFlightID, AirSchMasterFlightID.Length + 1);
                //        AirSchMasterFlightID[AirSchMasterFlightID.Length - 1] = FilghtID[i];
                //        Array.Resize(ref AirSchToDate, AirSchToDate.Length + 1);
                //        AirSchToDate[AirSchToDate.Length - 1] = ToDate[i];
                //        Array.Resize(ref AirSchAirlinePrefix, AirSchAirlinePrefix.Length + 1);
                //        AirSchAirlinePrefix[AirSchAirlinePrefix.Length - 1] = AirlinePrefix[i];
                //        Array.Resize(ref AirSchMasterOrigin, AirSchMasterOrigin.Length + 1);
                //        AirSchMasterOrigin[AirSchMasterOrigin.Length - 1] = Origin[i];
                //        Array.Resize(ref AirSchFromDate, AirSchFromDate.Length + 1);
                //        AirSchFromDate[AirSchFromDate.Length - 1] = FromDate[i];
                //        Array.Resize(ref AirSchTailNo, AirSchTailNo.Length + 1);
                //        AirSchTailNo[AirSchTailNo.Length - 1] = TailNo[i];
                //        Array.Resize(ref AirSchFrequency, AirSchFrequency.Length + 1);
                //        AirSchFrequency[AirSchFrequency.Length - 1] = FinalFrequency[i];
                //        Array.Resize(ref AirSchMasterDepartureTime, AirSchMasterDepartureTime.Length + 1);
                //        AirSchMasterDepartureTime[AirSchMasterDepartureTime.Length - 1] = Departure[i];
                //        Array.Resize(ref AirSchDeptTimeZone, AirSchDeptTimeZone.Length + 1);
                //        AirSchDeptTimeZone[AirSchDeptTimeZone.Length - 1] = DepartureTimeZone[i];
                //        Array.Resize(ref AirSchAirCraftType, AirSchAirCraftType.Length + 1);
                //        AirSchAirCraftType[AirSchAirCraftType.Length - 1] = AirCraftType[i];

                //    }
                //    if (i != 0 && i < FilghtID.Length - 1)
                //    {
                //        if ((FlightNo[i] != "    "))
                //        {
                //            Array.Resize(ref AirSchMasterDest, AirSchMasterDest.Length + 1);
                //            AirSchMasterDest[AirSchMasterDest.Length - 1] = Destination[i];
                //            Array.Resize(ref AirSchMasterArrivalTime, AirSchMasterArrivalTime.Length + 1);
                //            AirSchMasterArrivalTime[AirSchMasterArrivalTime.Length - 1] = Arrival[i];
                //            Array.Resize(ref AirSchArrTimeZone, AirSchArrTimeZone.Length + 1);
                //            AirSchArrTimeZone[AirSchArrTimeZone.Length - 1] = ArrivalTimeZone[i];
                //            Array.Resize(ref AirSchMasterFlightID, AirSchMasterFlightID.Length + 1);
                //            AirSchMasterFlightID[AirSchMasterFlightID.Length - 1] = FilghtID[i];
                //            Array.Resize(ref AirSchToDate, AirSchToDate.Length + 1);
                //            AirSchToDate[AirSchToDate.Length - 1] = ToDate[i];
                //            Array.Resize(ref AirSchAirlinePrefix, AirSchAirlinePrefix.Length + 1);
                //            AirSchAirlinePrefix[AirSchAirlinePrefix.Length - 1] = AirlinePrefix[i];


                //        }

                //        if (FlightNo[i] == "    " && (FilghtID[i] == FilghtID[i + 1]) && (Frequency[i] != Frequency[i + 1] || Destination[i] != Origin[i + 1]))
                //        {
                //            Array.Resize(ref AirSchMasterOrigin, AirSchMasterOrigin.Length + 1);
                //            AirSchMasterOrigin[AirSchMasterOrigin.Length - 1] = Origin[i];
                //            Array.Resize(ref AirSchFromDate, AirSchFromDate.Length + 1);
                //            AirSchFromDate[AirSchFromDate.Length - 1] = FromDate[i];
                //            Array.Resize(ref AirSchTailNo, AirSchTailNo.Length + 1);
                //            AirSchTailNo[AirSchTailNo.Length - 1] = TailNo[i];
                //            Array.Resize(ref AirSchFrequency, AirSchFrequency.Length + 1);
                //            AirSchFrequency[AirSchFrequency.Length - 1] = FinalFrequency[i];
                //            Array.Resize(ref AirSchMasterDepartureTime, AirSchMasterDepartureTime.Length + 1);
                //            AirSchMasterDepartureTime[AirSchMasterDepartureTime.Length - 1] = Departure[i];
                //            Array.Resize(ref AirSchDeptTimeZone, AirSchDeptTimeZone.Length + 1);
                //            AirSchDeptTimeZone[AirSchDeptTimeZone.Length - 1] = DepartureTimeZone[i];
                //            Array.Resize(ref AirSchAirCraftType, AirSchAirCraftType.Length + 1);
                //            AirSchAirCraftType[AirSchAirCraftType.Length - 1] = AirCraftType[i];



                //        }
                //        if (FlightNo[i] == "    " && (FilghtID[i] == FilghtID[i - 1]) && (Frequency[i] != Frequency[i - 1] || Origin[i] != Destination[i - 1]))
                //        {
                //            Array.Resize(ref AirSchMasterOrigin, AirSchMasterOrigin.Length + 1);
                //            AirSchMasterOrigin[AirSchMasterOrigin.Length - 1] = Origin[i];
                //            Array.Resize(ref AirSchFromDate, AirSchFromDate.Length + 1);
                //            AirSchFromDate[AirSchFromDate.Length - 1] = FromDate[i];
                //            Array.Resize(ref AirSchTailNo, AirSchTailNo.Length + 1);
                //            AirSchTailNo[AirSchTailNo.Length - 1] = TailNo[i];
                //            Array.Resize(ref AirSchFrequency, AirSchFrequency.Length + 1);
                //            AirSchFrequency[AirSchFrequency.Length - 1] = FinalFrequency[i];
                //            Array.Resize(ref AirSchMasterDepartureTime, AirSchMasterDepartureTime.Length + 1);
                //            AirSchMasterDepartureTime[AirSchMasterDepartureTime.Length - 1] = Departure[i];
                //            Array.Resize(ref AirSchDeptTimeZone, AirSchDeptTimeZone.Length + 1);
                //            AirSchDeptTimeZone[AirSchDeptTimeZone.Length - 1] = DepartureTimeZone[i];
                //            Array.Resize(ref AirSchAirCraftType, AirSchAirCraftType.Length + 1);
                //            AirSchAirCraftType[AirSchAirCraftType.Length - 1] = AirCraftType[i];


                //        }
                //        if ((FlightNo[i] == "    ") && (FilghtID[i] != FilghtID[i - 1]))
                //        {
                //            Array.Resize(ref AirSchMasterOrigin, AirSchMasterOrigin.Length + 1);
                //            AirSchMasterOrigin[AirSchMasterOrigin.Length - 1] = Origin[i];
                //            Array.Resize(ref AirSchFromDate, AirSchFromDate.Length + 1);
                //            AirSchFromDate[AirSchFromDate.Length - 1] = FromDate[i];
                //            Array.Resize(ref AirSchTailNo, AirSchTailNo.Length + 1);
                //            AirSchTailNo[AirSchTailNo.Length - 1] = TailNo[i];
                //            Array.Resize(ref AirSchFrequency, AirSchFrequency.Length + 1);
                //            AirSchFrequency[AirSchFrequency.Length - 1] = FinalFrequency[i];
                //            Array.Resize(ref AirSchMasterDepartureTime, AirSchMasterDepartureTime.Length + 1);
                //            AirSchMasterDepartureTime[AirSchMasterDepartureTime.Length - 1] = Departure[i];
                //            Array.Resize(ref AirSchDeptTimeZone, AirSchDeptTimeZone.Length + 1);
                //            AirSchDeptTimeZone[AirSchDeptTimeZone.Length - 1] = DepartureTimeZone[i];
                //            Array.Resize(ref AirSchAirCraftType, AirSchAirCraftType.Length + 1);
                //            AirSchAirCraftType[AirSchAirCraftType.Length - 1] = AirCraftType[i];

                //        }

                //        if ((FlightNo[i] != "    ") && (FilghtID[i] != FilghtID[i - 1]))
                //        {
                //            Array.Resize(ref AirSchMasterOrigin, AirSchMasterOrigin.Length + 1);
                //            AirSchMasterOrigin[AirSchMasterOrigin.Length - 1] = Origin[i];
                //            Array.Resize(ref AirSchFromDate, AirSchFromDate.Length + 1);
                //            AirSchFromDate[AirSchFromDate.Length - 1] = FromDate[i];
                //            Array.Resize(ref AirSchTailNo, AirSchTailNo.Length + 1);
                //            AirSchTailNo[AirSchTailNo.Length - 1] = TailNo[i];
                //            Array.Resize(ref AirSchFrequency, AirSchFrequency.Length + 1);
                //            AirSchFrequency[AirSchFrequency.Length - 1] = FinalFrequency[i];
                //            Array.Resize(ref AirSchMasterDepartureTime, AirSchMasterDepartureTime.Length + 1);
                //            AirSchMasterDepartureTime[AirSchMasterDepartureTime.Length - 1] = Departure[i];
                //            Array.Resize(ref AirSchDeptTimeZone, AirSchDeptTimeZone.Length + 1);
                //            AirSchDeptTimeZone[AirSchDeptTimeZone.Length - 1] = DepartureTimeZone[i];
                //            Array.Resize(ref AirSchAirCraftType, AirSchAirCraftType.Length + 1);
                //            AirSchAirCraftType[AirSchAirCraftType.Length - 1] = AirCraftType[i];


                //        }
                //        if ((FlightNo[i] != "    ") && (FlightNo[i - 1] != "    ") && (FilghtID[i] == FilghtID[i - 1]))
                //        {
                //            Array.Resize(ref AirSchMasterOrigin, AirSchMasterOrigin.Length + 1);
                //            AirSchMasterOrigin[AirSchMasterOrigin.Length - 1] = Origin[i];
                //            Array.Resize(ref AirSchFromDate, AirSchFromDate.Length + 1);
                //            AirSchFromDate[AirSchFromDate.Length - 1] = FromDate[i];
                //            Array.Resize(ref AirSchTailNo, AirSchTailNo.Length + 1);
                //            AirSchTailNo[AirSchTailNo.Length - 1] = TailNo[i];
                //            Array.Resize(ref AirSchFrequency, AirSchFrequency.Length + 1);
                //            AirSchFrequency[AirSchFrequency.Length - 1] = FinalFrequency[i];
                //            Array.Resize(ref AirSchMasterDepartureTime, AirSchMasterDepartureTime.Length + 1);
                //            AirSchMasterDepartureTime[AirSchMasterDepartureTime.Length - 1] = Departure[i];
                //            Array.Resize(ref AirSchDeptTimeZone, AirSchDeptTimeZone.Length + 1);
                //            AirSchDeptTimeZone[AirSchDeptTimeZone.Length - 1] = DepartureTimeZone[i];
                //            Array.Resize(ref AirSchAirCraftType, AirSchAirCraftType.Length + 1);
                //            AirSchAirCraftType[AirSchAirCraftType.Length - 1] = AirCraftType[i];


                //        }

                //        if ((FlightNo[i] == "    ") && (Frequency[i] != Frequency[i + 1] && FilghtID[i] == FilghtID[i + 1]))
                //        {
                //            Array.Resize(ref AirSchMasterDest, AirSchMasterDest.Length + 1);
                //            AirSchMasterDest[AirSchMasterDest.Length - 1] = Destination[i];
                //            Array.Resize(ref AirSchMasterArrivalTime, AirSchMasterArrivalTime.Length + 1);
                //            AirSchMasterArrivalTime[AirSchMasterArrivalTime.Length - 1] = Arrival[i];
                //            Array.Resize(ref AirSchArrTimeZone, AirSchArrTimeZone.Length + 1);
                //            AirSchArrTimeZone[AirSchArrTimeZone.Length - 1] = ArrivalTimeZone[i];
                //            Array.Resize(ref AirSchMasterFlightID, AirSchMasterFlightID.Length + 1);
                //            AirSchMasterFlightID[AirSchMasterFlightID.Length - 1] = FilghtID[i];
                //            Array.Resize(ref AirSchToDate, AirSchToDate.Length + 1);
                //            AirSchToDate[AirSchToDate.Length - 1] = ToDate[i];
                //            Array.Resize(ref AirSchAirlinePrefix, AirSchAirlinePrefix.Length + 1);
                //            AirSchAirlinePrefix[AirSchAirlinePrefix.Length - 1] = AirlinePrefix[i];

                //        }


                //    }
                //    if ((FilghtID.Length == i + 1) && FlightNo[i] != "    ")
                //    {
                //        Array.Resize(ref AirSchMasterDest, AirSchMasterDest.Length + 1);
                //        AirSchMasterDest[AirSchMasterDest.Length - 1] = Destination[i];
                //        Array.Resize(ref AirSchMasterArrivalTime, AirSchMasterArrivalTime.Length + 1);
                //        AirSchMasterArrivalTime[AirSchMasterArrivalTime.Length - 1] = Arrival[i];
                //        Array.Resize(ref AirSchArrTimeZone, AirSchArrTimeZone.Length + 1);
                //        AirSchArrTimeZone[AirSchArrTimeZone.Length - 1] = ArrivalTimeZone[i];
                //        Array.Resize(ref AirSchMasterFlightID, AirSchMasterFlightID.Length + 1);
                //        AirSchMasterFlightID[AirSchMasterFlightID.Length - 1] = FilghtID[i];
                //        Array.Resize(ref AirSchToDate, AirSchToDate.Length + 1);
                //        AirSchToDate[AirSchToDate.Length - 1] = ToDate[i];
                //        Array.Resize(ref AirSchAirlinePrefix, AirSchAirlinePrefix.Length + 1);
                //        AirSchAirlinePrefix[AirSchAirlinePrefix.Length - 1] = AirlinePrefix[i];

                //    }
                //}

                #endregion


                #region Preparing Parameters for SSIM Export to Update Airline Schedule

                string[] QueryNames = new string[12];
                object[] QueryValues = new object[12];
                SqlDbType[] QueryTypes = new SqlDbType[12];
                string[] Names = new string[14];
                object[] Values = new object[14];
                SqlDbType[] Types = new SqlDbType[14];

                for (int i = 0; i < AirSchMasterFlightID.Length; i++)
                {
                    QueryNames[0] = "FromDate";
                    QueryNames[1] = "ToDate";
                    QueryNames[2] = "FlightID";
                    QueryNames[3] = "Source";
                    QueryNames[4] = "Dest";
                    QueryNames[5] = "ScheduleDepttime";
                    QueryNames[6] = "SchArrtime";
                    QueryNames[7] = "frequency";
                    QueryNames[8] = "EquipmentNo";
                    QueryNames[9] = "ArrTimeZone";
                    QueryNames[10] = "DeptTimeZone";
                    QueryNames[11] = "FlightPrefix";


                    QueryValues[0] = AirSchFromDate[i].Trim();
                    QueryValues[1] = AirSchToDate[i].Trim();
                    QueryValues[2] = AirSchMasterFlightID[i].Trim();
                    QueryValues[3] = AirSchMasterOrigin[i].Trim();
                    QueryValues[4] = AirSchMasterDest[i].Trim();
                    QueryValues[5] = AirSchMasterDepartureTime[i].Trim();
                    QueryValues[6] = AirSchMasterArrivalTime[i].Trim();
                    QueryValues[7] = AirSchFrequency[i].Trim();
                    QueryValues[8] = AirSchTailNo[i].Trim();
                    QueryValues[9] = AirSchArrTimeZone[i].Trim();
                    QueryValues[10] = AirSchDeptTimeZone[i].Trim();
                    QueryValues[11] = AirSchAirlinePrefix[i].Trim();



                    QueryTypes[0] = SqlDbType.DateTime;
                    QueryTypes[1] = SqlDbType.DateTime;
                    QueryTypes[2] = SqlDbType.VarChar;
                    QueryTypes[3] = SqlDbType.VarChar;
                    QueryTypes[4] = SqlDbType.VarChar;
                    QueryTypes[5] = SqlDbType.VarChar;
                    QueryTypes[6] = SqlDbType.VarChar;
                    QueryTypes[7] = SqlDbType.VarChar;
                    QueryTypes[8] = SqlDbType.VarChar;
                    QueryTypes[9] = SqlDbType.VarChar;
                    QueryTypes[10] = SqlDbType.VarChar;
                    QueryTypes[11] = SqlDbType.VarChar;

                    if (db.InsertData("spSavePartnerSchedule_SSIM", QueryNames, QueryTypes, QueryValues))
                    {
                        lblStatus.Text = "Schedule Uploaded Successfully!";
                        lblStatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblStatus.Text = "Schedule Updation Failed!";
                        lblStatus.ForeColor = Color.Red;
                    }

                }
                for (int i = 0; i < AirSchMasterFlightID.Length; i++)
                {
                    Names[0] = "FromDate";
                    Names[1] = "ToDate";
                    Names[2] = "FlightID";
                    Names[3] = "Source";
                    Names[4] = "Dest";
                    Names[5] = "ScheduleDepttime";
                    Names[6] = "SchArrtime";
                    Names[7] = "frequency";
                    Names[8] = "ArrTimeZone";
                    Names[9] = "DeptTimeZone";
                    Names[10] = "AirCraftType";
                    Names[11] = "FlightPrefix";
                    //Names[12] = "AirSchSource";
                    //Names[13] = "AirSchDest";




                    Values[0] = AirSchFromDate[i].Trim();
                    Values[1] = AirSchToDate[i].Trim();
                    Values[2] = AirSchMasterFlightID[i].Trim();
                    Values[3] = AirSchMasterOrigin[i].Trim();
                    Values[4] = AirSchMasterDest[i].Trim();
                    Values[5] = AirSchMasterDepartureTime[i].Trim();
                    Values[6] = AirSchMasterArrivalTime[i].Trim();
                    Values[7] = AirSchFrequency[i].Trim();
                    Values[8] = AirSchTailNo[i].Trim();
                    Values[9] = AirSchArrTimeZone[i].Trim();
                    Values[10] = AirSchDeptTimeZone[i].Trim();
                    Values[11] = AirSchAirlinePrefix[i].Trim();
                    //for (int j = 0; j < AirSchMasterFlightID.Length; j++)
                    //{

                    //    if (FilghtID[i] == AirSchMasterFlightID[j] && FromDate[i] == AirSchFromDate[j] && ToDate[i] == AirSchToDate[j] && FinalFrequency[i] == AirSchFrequency[j])// && Arrival[i] == AirSchMasterArrivalTime[j] && ArrivalTimeZone[i] == AirSchArrTimeZone[j])
                    //    {
                    //        Values[12] = AirSchMasterOrigin[j];
                    //        Values[13] = AirSchMasterDest[j];
                    //    }


                    //}

                    Types[0] = SqlDbType.DateTime;
                    Types[1] = SqlDbType.DateTime;
                    Types[2] = SqlDbType.VarChar;
                    Types[3] = SqlDbType.VarChar;
                    Types[4] = SqlDbType.VarChar;
                    Types[5] = SqlDbType.VarChar;
                    Types[6] = SqlDbType.VarChar;
                    Types[7] = SqlDbType.VarChar;
                    Types[8] = SqlDbType.VarChar;
                    Types[9] = SqlDbType.VarChar;
                    Types[10] = SqlDbType.VarChar;
                    Types[11] = SqlDbType.VarChar;
                    //Types[12] = SqlDbType.VarChar;
                    //Types[13] = SqlDbType.VarChar;

                    if (db.InsertData("spSavePartnerRouteDetails_SSIM", Names, Types, Values))
                    {
                        lblStatus.Text = "Schedule Updated Successfully!";
                        lblStatus.ForeColor = Color.Green;
                    }
                    else
                    {

                        lblStatus.Text = "Schedule Updation Failed!";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }

                }



                #endregion

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }
        #endregion
    }
}
