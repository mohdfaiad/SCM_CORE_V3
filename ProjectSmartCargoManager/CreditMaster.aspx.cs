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

namespace ProjectSmartCargoManager
{
    public partial class CreditMaster : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());
        BAL.CreditBAL objBLL = new BAL.CreditBAL();

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAgentDropdown();
                LoadGridCredit();
                LoadBank();
                LoadCreditDetails();
            }
            
         
            #region check agent validation
            //if (Session["AgentCode"].ToString() != "")
            //{
            //    foreach (ListItem item in ddlagentcode.Items)
            //    {
            //        if (item.Text == Session["AgentCode"].ToString())
            //        {
            //            txtagentname.Text = item.Value;
            //            ddlagentcode.SelectedValue = txtagentname.Text;
            //            ddlagentcode.Enabled = false;
            //            break;
            //        }
            //    }
            //}
            #endregion check agent validation
        }
        #endregion Page Load
        #region Load Agent Dropdown
        public void LoadAgentDropdown()
        {
            DataSet ds = objBLL.GetAgentList(Session["AgentCode"].ToString());
            if (ds != null)
            {
                ddlagentcode.DataSource = ds;
                ddlagentcode.DataMember = ds.Tables[0].TableName;
                ddlagentcode.DataTextField = "AgentName";
                ddlagentcode.DataValueField = "AgentCode";
                ddlagentcode.DataBind();
                ddlagentcode.SelectedIndex = -1;
            }
        }
        #endregion Load Grid Material Detail
        #region Clear
        public void clear()
        {
            txtcredit.Text = "";
            txtcreditremain.Text = "";
            txtinvoice.Text = "";
            ddlagentcode.SelectedIndex = 0;
            LoadGridCredit();
            LoadCreditDetails();
        }
        #endregion clear
        #region LoadBank Dropdown
        public void LoadBank()
        {
            DataSet ds = objBLL.GetBankList("");
            DropDownList ddl = new DropDownList();

            for (int i = 0; i < grdCreditinfo.Rows.Count; i++)
            {
                ddl = ((DropDownList)(grdCreditinfo.Rows[i].FindControl("ddlbankname")));
                if (ds != null)
                {
                    ddl.DataSource = ds;
                    ddl.DataMember = ds.Tables[0].TableName;
                    ddl.DataTextField = "BankName";
                    ddl.DataValueField = "BankName";
                    ddl.DataBind();
                }
            }
        
        }

        #endregion LoadBank Dropdown
        #region Save
        protected void btnSave_Click(object sender, EventArgs e)
        {
            
                    if (IsPostBack)
                    {

                        try
                        {
                            string[] paramname = new string[9];
                            paramname[0] = "AgentCode";
                            paramname[1] = "BankName";
                            paramname[2] = "BankGuranteeNumber";
                            paramname[3] = "BankGuranteeAmount";
                            paramname[4] = "ValidFrom";
                            paramname[5] = "ValidTo";
                            paramname[6] = "CreditAmount";
                            paramname[7] = "InvoiceBalance";
                            paramname[8] = "CreditRemaining";

                            object[] paramvalue = new object[9];
                            paramvalue[0] = ddlagentcode.SelectedValue;
                            for (int i = 0; i < grdCreditinfo.Rows.Count; i++)
                            {
                                paramvalue[1] = ((DropDownList)grdCreditinfo.Rows[i].FindControl("ddlbankname")).Text ;
                                paramvalue[2] = ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankgurantee")).Text;
                                paramvalue[3] = ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankguranteeamt")).Text;
                                paramvalue[4] = ((TextBox)grdCreditinfo.Rows[i].FindControl("txtsatrtdate")).Text;
                                paramvalue[5] = ((TextBox)grdCreditinfo.Rows[i].FindControl("txtvalidto")).Text;

                                paramvalue[6] = txtcredit.Text.Trim();
                                paramvalue[7] = txtinvoice.Text.Trim();
                                paramvalue[8] = txtcreditremain.Text.Trim();


                                SqlDbType[] paramtype = new SqlDbType[9];
                                paramtype[0] = SqlDbType.NVarChar;
                                paramtype[1] = SqlDbType.NVarChar;
                                paramtype[2] = SqlDbType.NVarChar;
                                paramtype[3] = SqlDbType.NVarChar;
                                paramtype[4] = SqlDbType.NVarChar;
                                paramtype[5] = SqlDbType.NVarChar;
                                paramtype[6] = SqlDbType.Float;
                                paramtype[7] = SqlDbType.Float;
                                paramtype[8] = SqlDbType.Float;

                                bool ds = da.InsertData("Sp_InsertCreditData", paramname, paramtype, paramvalue);

                            }
                           

                        }
                        catch (Exception ex)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "InsertFailure()", true);
                        }
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "Insert()", true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "SelectRow()", true);
                    }
                    clear();
                    LoadBank();
            
             //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "SelectRow()", true);
        }
        #endregion Save
        #region LoadgridCreditInfo Intial Row
        public void LoadGridCredit()
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
            myDataColumn.ColumnName = "ddlbankname";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Bank Gurantee Number";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Bank Gurantee Amount";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Valid From";
            myDataTable.Columns.Add(myDataColumn);


            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Valid To";
            myDataTable.Columns.Add(myDataColumn);

            DataRow dr;
            dr = myDataTable.NewRow();
            dr["RowNumber"] = 1;
            dr["ddlbankname"] = "select";//"5";
            dr["Bank Gurantee Number"] = "";// "5";
            dr["Bank Gurantee Amount"] = "";// "9";
            dr["Valid From"] = "";
            dr["Valid To"] = "";
            

            myDataTable.Rows.Add(dr);
            ViewState["CurrentTable"] = myDataTable ;
            //Bind the DataTable to the Grid
            
            grdCreditinfo.DataSource = null;
            grdCreditinfo.DataSource = myDataTable;
            grdCreditinfo.DataBind();
        }
        #endregion LoadgridCreditInfo Intial Row
        #region Check Credit and Invoice
        protected void txtinvoice_TextChanged(object sender, EventArgs e)
        {
            int credit=Convert.ToInt32(txtcredit.Text.Trim());
            int invoice=Convert.ToInt32(txtinvoice.Text.Trim());
            int Total;
            if (credit < invoice)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "Check()", true);
                txtinvoice.Text = "";
                txtcreditremain.Text = "";

            }
            else
            {
                Total = credit - invoice;
                (txtcreditremain.Text) = Convert.ToString(Total);
            }
        }
        #endregion Check Credit and Invoice
        #region TotalCreditCalculation
        public void TotalCredit(object sender, EventArgs e)
        {
            int incr = 0;
            try
            {
                //int temp = Convert.ToInt32(((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankguranteeamt")).Text);
                //if(temp!=)
                //{
                //} 
                for (int i = 0; i < grdCreditinfo.Rows.Count; i++)
                {
                    int TotalAmt = Convert.ToInt32(((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankguranteeamt")).Text);
                    incr = incr + TotalAmt;

                }
                txtcredit.Text = Convert.ToString(incr);
            }
            catch(Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "NumericValidation()", true);
                ((TextBox)grdCreditinfo.Rows[0].FindControl("txtbankguranteeamt")).Text = "";
            }
        }
        #endregion TotalCreditCalculation 
        #region TO add new row
        //protected void lnkadd_Click(object sender, EventArgs e)
        //{
        //    AddNewRowToGrid();
        //    LoadBank();
        //}
        public void Addrow()
        {
            AddNewRowToGrid();
            LoadBank();
        }
       protected void btnaddroow_Click(object sender, EventArgs e)
       {
                 AddNewRowToGrid();
                 LoadBank();
        }
        #endregion TO add new row
        #region AddnewRow To Grid
        private void AddNewRowToGrid()
        {
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
                                                
                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["RowNumber"] = i + 1;

                        dtCurrentTable.Rows[i - 1]["ddlbankname"] = ((DropDownList)grdCreditinfo.Rows[rowIndex].Cells[0].FindControl("ddlbankname")).Text;
                        dtCurrentTable.Rows[i - 1]["Bank Gurantee Number"] = ((TextBox)grdCreditinfo.Rows[rowIndex].Cells[1].FindControl("txtbankgurantee")).Text;
                        dtCurrentTable.Rows[i - 1]["Bank Gurantee Amount"] = ((TextBox)grdCreditinfo.Rows[rowIndex].Cells[2].FindControl("txtbankguranteeamt")).Text;
                        dtCurrentTable.Rows[i - 1]["Valid From"] = ((TextBox)grdCreditinfo.Rows[rowIndex].Cells[3].FindControl("txtsatrtdate")).Text;
                        dtCurrentTable.Rows[i - 1]["Valid To"] = ((TextBox)grdCreditinfo.Rows[rowIndex].Cells[4].FindControl("txtvalidto")).Text;


                        rowIndex++;
                    }
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtCurrentTable;

                    grdCreditinfo.DataSource = dtCurrentTable;
                    grdCreditinfo.DataBind();
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }

            //Set Previous Data on Postbacks
            SetPreviousData();



        }
        #endregion AddnewRow To Grid
        #region SetPreviousData
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
                        DropDownList ddl = (DropDownList)grdCreditinfo.Rows[rowIndex].Cells[0].FindControl("ddlbankname");
                        TextBox box2 = (TextBox)grdCreditinfo.Rows[rowIndex].Cells[1].FindControl("txtbankgurantee");
                        TextBox box3 = (TextBox)grdCreditinfo.Rows[rowIndex].Cells[2].FindControl("txtbankguranteeamt");
                        TextBox box4 = (TextBox)grdCreditinfo.Rows[rowIndex].Cells[3].FindControl("txtsatrtdate");
                        TextBox box5 = (TextBox)grdCreditinfo.Rows[rowIndex].Cells[4].FindControl("txtvalidto");

                        FillDropDownList(ddl);
                        if (dt.Rows[i]["ddlbankname"].ToString().Trim() != "")
                            ddl.Text = dt.Rows[i]["ddlbankname"].ToString();

                        box2.Text = dt.Rows[i]["Bank Gurantee Number"].ToString();
                        box3.Text = dt.Rows[i]["Bank Gurantee Amount"].ToString();
                        box4.Text = dt.Rows[i]["Valid From"].ToString();
                        box5.Text = dt.Rows[i]["Valid To"].ToString();
                        //Fill the DropDownList with Data
                        
                            rowIndex++;
                        
                    }
                }
            }


        }
        #endregion SetPreviousData
        #region FillDropDownList
        private void FillDropDownList(DropDownList ddl)
    {
        ArrayList arr = GetDummyData();
        foreach (ListItem item in arr)
        {
            ddl.Items.Add(item);
        }
    }
        #endregion SetPreviousData
        #region Dummy Data
        private ArrayList GetDummyData()
             {
                 ArrayList arr = new ArrayList();
                 arr.Add(new ListItem("AXIX", "1"));
                 arr.Add(new ListItem("HDFC", "2"));
                 arr.Add(new ListItem("Bank Of Baroda", "3"));

                 
                 return arr;
             }
             #endregion Dummy Data
        #region Clear
             protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlagentcode.SelectedIndex  = 0;
            LoadBank();
            //LoadCreditDetails();
        }
        #endregion Clear
        #region List
             protected void btnList_Click(object sender, EventArgs e)
             {
                 string[] paramname = new string[1];
                 paramname[0] = "AgentCode";
                 string AgentCode = string.Empty;

                 //if (FltDate == "")
                 //{

                 //    FltDate = "NA"; // NA=not available 
                 //}


                 object[] paramvalue = new object[1];

                 string str = ddlagentcode.SelectedItem.Text;

                 DataSet ds1 = objBLL.GetAgentList(Session["AgentCode"].ToString());
                 for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                 {
                     if (str == ds1.Tables[0].Rows[i][1].ToString())
                     {
                         paramvalue[0] = ds1.Tables[0].Rows[i][0];
                         break;
                     }

                 }
                 SqlDbType[] paramtype = new SqlDbType[1];
                 paramtype[0] = SqlDbType.VarChar;

                 DataSet ds = da.SelectRecords("SPGetAgentCreadit", paramname, paramvalue, paramtype);
                 if (ds != null)
                 {
                     if (ds.Tables.Count > 0)
                     {
                         if (ds.Tables[0].Rows.Count > 0)
                         {
                             grdCreditdetails.DataSource = ds.Tables[0];
                             grdCreditdetails.DataBind();
                         }
                         else
                         {
                             grdCreditdetails .Visible = false;
                             ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "NoData()", true);
                         }
                     }
                 }
             }
             #endregion List
        #region LoadCreditDetails
             public void LoadCreditDetails()
             {

                 string[] paramname = new string[1];
                 paramname[0] = "AgentCode";
                 string AgentCode = "";
                 //"1990-04-09 00:00:00.000"
                 object[] paramvalue = new object[1];
                 if (AgentCode == "")
                 {
                     AgentCode = ""; // NA=not available 
                     paramvalue[0] = "";
                 }

                SqlDbType[] paramtype = new SqlDbType[1];
                 paramtype[0] = SqlDbType.VarChar;



                 DataSet ds = da.SelectRecords("SPGetAgentCreadit", paramname, paramvalue, paramtype);
                 if (ds != null)
                 {
                     if (ds.Tables.Count > 0)
                     {
                         if (ds.Tables[0].Rows.Count > 0)
                         {
                             grdCreditdetails.DataSource = null;
                             grdCreditdetails.DataSource = ds.Tables[0].Copy();
                             grdCreditdetails.DataBind();
                            


                             //code for checking grid is emply or not


                         }
                     }
                 }

             }

             #endregion LoadCreditDetails
        #region Indexing
             protected void grdCreditdetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
             {
                 LoadCreditDetails();
                 grdCreditdetails.PageIndex = e.NewPageIndex;
                 grdCreditdetails.DataBind();
             }
             #endregion Indexing
        #region sortdirection
             private string ConvertSortDirectionToSql(SortDirection sortDirection)
             {
                 string newSortDirection = String.Empty;

                 switch (sortDirection)
                 {
                     case SortDirection.Ascending:
                         newSortDirection = "ASC";
                         break;

                     case SortDirection.Descending:
                         newSortDirection = "DESC";
                         break;
                 }

                 return newSortDirection;
             }
        #endregion sortdirection
        #region Sorting
             protected void grdCreditdetails_Sorting(object sender, GridViewSortEventArgs e)
             {
                 DataTable dataTable = grdCreditdetails .DataSource as DataTable;

                 if (dataTable != null)
                 {
                     DataView dataView = new DataView(dataTable);
                     dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);

                     grdCreditdetails.DataSource = dataView;
                     grdCreditdetails.DataBind();
                 }
             }
             #endregion Sorting 
        #region Cleartext
             protected void btntextclear_Click(object sender, EventArgs e)
             {
                 ddlagentcode.SelectedIndex =0;

             }
             #endregion ClearText

    }
}
