using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ProjectSmartCargoManager
{
    public partial class StockMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                LoadGridItemDetail();
            }
        }

        #region LoadGridItemDetail

        public void LoadGridItemDetail()
        {
            DataTable myDataTable = new DataTable();
            DataColumn myDataColumn;
            DataSet Ds = new DataSet();

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "comdity";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "desp";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "pcs";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "gwgt";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "dimension";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "vol";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "uldgrp";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "fltdate";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "fltno";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "status";
            myDataTable.Columns.Add(myDataColumn);


            //DataRow dr;
            //dr = myDataTable.NewRow();
            //dr["comdity"] = "Mobile";
            //dr["desp"] = "Expensive Fragile";
            //dr["pcs"] = "50";
            //dr["gwgt"] = "10KG";
            //dr["dimension"] = "YES";
            //dr["vol"] = "";
            //dr["uldgrp"] = "";
            //dr["fltdate"] = "14/Nov/2011";
            //dr["fltno"] = "IT104";
            //dr["status"] = "Confirmed";
            //myDataTable.Rows.Add(dr);

            //dr = myDataTable.NewRow();
            //dr["comdity"] = "TV";
            //dr["desp"] = "Expensive Fragile";
            //dr["pcs"] = "1";
            //dr["gwgt"] = "10KG";
            //dr["dimension"] = "NO";
            //dr["vol"] = "";
            //dr["uldgrp"] = "";
            //dr["fltdate"] = "14/Nov/2011";
            //dr["fltno"] = "IT101";
            //dr["status"] = "Confirmed";


            //myDataTable.Rows.Add(dr);
            myDataTable.Rows.Add(myDataTable.NewRow());
            grdStock.DataSource = myDataTable;
            grdStock.DataBind();
        }

        #endregion LoadGridItemDetail

        protected void grdStock_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AddRow")
            {
                //AddNewRowToGrid();
            }
        }

        #region AddControlsToGridview
        /// <summary>
        /// BUILD AND BIND TEXTBOX CONTROL TO GRIDVIEW ROW ON PAGE LOAD EVENT
        /// </summary>

        private void SetInitialRow()
        {

            DataTable dt = new DataTable();

            DataRow dr = null;

            dt.Columns.Add(new DataColumn("Column1", typeof(string)));

            dt.Columns.Add(new DataColumn("Column2", typeof(string)));

            dt.Columns.Add(new DataColumn("Column3", typeof(string)));

            dt.Columns.Add(new DataColumn("Column4", typeof(string)));

            dt.Columns.Add(new DataColumn("Column5", typeof(string)));

            dt.Columns.Add(new DataColumn("Column6", typeof(string)));

            dt.Columns.Add(new DataColumn("Column7", typeof(string)));

            dt.Columns.Add(new DataColumn("Column8", typeof(string)));

            dt.Columns.Add(new DataColumn("Column9", typeof(string)));

            dt.Columns.Add(new DataColumn("Column10", typeof(string)));

            dt.Columns.Add(new DataColumn("Column11", typeof(string)));

            dr = dt.NewRow();


            dr["Column1"] = string.Empty;

            dr["Column2"] = string.Empty;

            dr["Column3"] = string.Empty;

            dr["Column4"] = string.Empty;

            dr["Column5"] = string.Empty;

            dr["Column6"] = string.Empty;

            dr["Column7"] = string.Empty;

            dr["Column8"] = string.Empty;

            dr["Column9"] = string.Empty;

            dr["Column10"] = string.Empty;

            dr["Column11"] = string.Empty;

            dt.Rows.Add(dr);

            //dr = dt.NewRow();



            //Store the DataTable in ViewState

            ViewState["CurrentTable"] = dt;



            grdStock.DataSource = dt;

            grdStock.DataBind();

        }

        /// <summary>
        /// BUILD AND BIND TEXTBOX CONTROL TO GRIDVIEW ROW ON PAGE LOAD EVENT
        /// </summary>
        private void SetPreviousData()
        {

            int rowIndex = 0;

            if (ViewState["CurrentTable"] != null)
            {

                DataTable dt = (DataTable)ViewState["CurrentTable"];

                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        CheckBox box1 = (CheckBox)grdStock.Rows[rowIndex].Cells[0].FindControl("check");

                        TextBox box2 = (TextBox)grdStock.Rows[rowIndex].Cells[1].FindControl("TextBox1");
                        ImageButton Img1 = (ImageButton)grdStock.Rows[rowIndex].Cells[1].FindControl("btn1");

                        TextBox box3 = (TextBox)grdStock.Rows[rowIndex].Cells[2].FindControl("TextBox2");

                        TextBox box4 = (TextBox)grdStock.Rows[rowIndex].Cells[3].FindControl("TextBox3");

                        TextBox box5 = (TextBox)grdStock.Rows[rowIndex].Cells[4].FindControl("TextBox4");

                        LinkButton lkbtn1 = (LinkButton)grdStock.Rows[rowIndex].Cells[5].FindControl("BtnDimension");

                        TextBox box6 = (TextBox)grdStock.Rows[rowIndex].Cells[6].FindControl("TextBox5");

                        TextBox box7 = (TextBox)grdStock.Rows[rowIndex].Cells[7].FindControl("TextBox6");
                        TextBox box8 = (TextBox)grdStock.Rows[rowIndex].Cells[8].FindControl("TextBox7");
                        TextBox box9 = (TextBox)grdStock.Rows[rowIndex].Cells[9].FindControl("TextBox8");
                        DropDownList box10 = (DropDownList)grdStock.Rows[rowIndex].Cells[10].FindControl("ddlstatus");



                        box1.Checked = Convert.ToBoolean(dt.Rows[i]["Column1"].ToString());

                        box2.Text = dt.Rows[i]["Column2"].ToString();

                        box3.Text = dt.Rows[i]["Column3"].ToString();

                        box4.Text = dt.Rows[i]["Column4"].ToString();

                        box5.Text = dt.Rows[i]["Column5"].ToString();

                        lkbtn1.Text = dt.Rows[i]["Column6"].ToString();

                        box6.Text = dt.Rows[i]["Column7"].ToString();

                        box7.Text = dt.Rows[i]["Column8"].ToString();

                        box8.Text = dt.Rows[i]["Column9"].ToString();

                        box9.Text = dt.Rows[i]["Column10"].ToString();

                        box10.SelectedValue = dt.Rows[i]["Column11"].ToString();

                        rowIndex++;

                    }

                }

            }

        }

        /// <summary>
        /// FOR ADDING ROWS ON BUTTON CLICK INSIDE GRIDVIEW
        /// </summary>
        private void AddNewRowToGrid()
        {
            //ViewState["AppAmt"] = Convert.ToString(txtStAmtApp.Text);
            int rowIndex = 0;

            if (ViewState["CurrentTable"] != null)
            {

                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];

                DataRow drCurrentRow = null;

                if (dtCurrentTable.Rows.Count > 0)
                {

                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {

                        //extract the TextBox values

                        //TextBox box1 = (TextBox) grdStock.Rows[rowIndex].Cells[0].FindControl("TextBox1");

                        CheckBox box1 = (CheckBox)grdStock.Rows[rowIndex].Cells[0].FindControl("check");

                        TextBox box2 = (TextBox)grdStock.Rows[rowIndex].Cells[1].FindControl("TextBox1");
                        ImageButton Img1 = (ImageButton)grdStock.Rows[rowIndex].Cells[1].FindControl("btn1");

                        TextBox box3 = (TextBox)grdStock.Rows[rowIndex].Cells[2].FindControl("TextBox2");

                        TextBox box4 = (TextBox)grdStock.Rows[rowIndex].Cells[3].FindControl("TextBox3");

                        TextBox box5 = (TextBox)grdStock.Rows[rowIndex].Cells[4].FindControl("TextBox4");

                        LinkButton lkbtn1 = (LinkButton)grdStock.Rows[rowIndex].Cells[5].FindControl("BtnDimension");

                        TextBox box6 = (TextBox)grdStock.Rows[rowIndex].Cells[6].FindControl("TextBox5");

                        TextBox box7 = (TextBox)grdStock.Rows[rowIndex].Cells[7].FindControl("TextBox6");
                        TextBox box8 = (TextBox)grdStock.Rows[rowIndex].Cells[8].FindControl("TextBox7");
                        TextBox box9 = (TextBox)grdStock.Rows[rowIndex].Cells[9].FindControl("TextBox8");
                        DropDownList box10 = (DropDownList)grdStock.Rows[rowIndex].Cells[10].FindControl("ddlstatus");

                        drCurrentRow = dtCurrentTable.NewRow();

                        drCurrentRow["RowNumber"] = i + 1;

                        dtCurrentTable.Rows[i - 1]["Column1"] = box1.Checked;

                        dtCurrentTable.Rows[i - 1]["Column2"] = box2.Text;

                        dtCurrentTable.Rows[i - 1]["Column3"] = box3.Text;

                        dtCurrentTable.Rows[i - 1]["Column4"] = box4.Text;

                        dtCurrentTable.Rows[i - 1]["Column5"] = box5.Text;

                        dtCurrentTable.Rows[i - 1]["Column6"] = lkbtn1.Text;

                        dtCurrentTable.Rows[i - 1]["Column7"] = box6.Text;

                        dtCurrentTable.Rows[i - 1]["Column8"] = box7.Text;

                        dtCurrentTable.Rows[i - 1]["Column9"] = box8.Text;

                        dtCurrentTable.Rows[i - 1]["Column10"] = box9.Text;

                        dtCurrentTable.Rows[i - 1]["Column11"] = box10.SelectedValue;



                        rowIndex++;

                    }

                    dtCurrentTable.Rows.Add(drCurrentRow);

                    ViewState["CurrentTable"] = dtCurrentTable;



                    grdStock.DataSource = dtCurrentTable;

                    grdStock.DataBind();

                }

            }

            else
            {

                Response.Write("ViewState is null");

            }



            //Set Previous Data on Postbacks

            SetPreviousData();
            //txtStAmtApp.Text = ViewState["AppAmt"].ToString();
        }

        /// <summary>
        /// SET ROW ADDING PREVIOUS TEXTBOX CONTINUE PROCESS
        /// </summary>


        /// <summary>
        /// CALLING GRIDVIEW ROW COMMAND TO ADD NEW ROW IN RUNTIME.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        #region GRIDVIEWADDROWCOMMAND
        //protected void grdStock_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "AddRow")
        //    {
        //        //AddNewRowToGrid();
        //    }
        //}
        #endregion GRIDVIEWADDROWCOMMAND

        #endregion AddControlsToGridview

    }
}
