using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using BAL;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;   
namespace ProjectSmartCargoManager
{
    public partial class ListEmbargoDetails : System.Web.UI.Page
    {
        EmbargoBAL Embal = new EmbargoBAL();
        SQLServer da = new SQLServer(Global.GetConnectionString());
        DataSet blnResult;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if(!IsPostBack)
                {
                    LoadGrid();
                    txtFromDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    txtToDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
            }
            catch (Exception ex)
            { }

        }
        #region OriginType
        protected void ddlorigintype_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListDataViewBAL objDataViewBAL = new ListDataViewBAL();
                DataSet dsResult = new DataSet();
                string errormessage = "";

                string level = ddlorigintype.SelectedItem.Value;
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
            catch (Exception ex)
            { }
        }
        #endregion OriginType
        #region List
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                //if (ddlorigintype.SelectedItem.Text == "" || ddlOrigin.SelectedItem.Text == "" || ddldestinationType.SelectedItem.Text == "" || txtEmbargoRefNo.Text == "" )
                //{
                //    LbLStatus.Text = "Please Provide a parameter for Search";
                //    LbLStatus.ForeColor = Color.Red;
                //    return;
                //}

                string Origintype = "", origin = "",status="", Destinationtype = "", destination = "", Level="", ParameterCode="";
                DateTime   FromDate,ToDate ;
                string EmbargoRefNumber, parameterValue;
                string dt1, dt2;
                Origintype = ddlorigintype.SelectedItem.Text;
                if (Origintype == "Select")
                {
                    Origintype = "";
                    origin = "";
                }
                else
                    if (ddlOrigin.SelectedItem.Text != "Select")
                    {
                        origin = ddlOrigin.SelectedItem.Text;
                    }
                    else
                        origin = "";
                //if (origin == "Select")
                //{
                //    origin = ""; 
                //}
                
                    Destinationtype = ddldestinationType.SelectedItem.Text;

                    if (Destinationtype == "Select")
                    {
                        Destinationtype = "";
                        destination = "";
                    }
                    else
                        if (ddlDestination.SelectedItem.Text != "Select")
                        {
                            destination = ddlDestination.SelectedItem.Text;
                        }
                        else
                            destination = "";
                //if (destination == "Select")
                //{
                //    destination = ""; 
                //}
                Level = ddlLevel.SelectedItem.Text;
                if (Level == "Select")
                {
                    Level = "";
                }
                ParameterCode = ddlParameter.SelectedItem.Text;
                if (ParameterCode == "Select")
                {
                    ParameterCode = ""; 
                }
                parameterValue =txtParametervalue.Text;
                if (parameterValue == null )
                {
                    parameterValue = ""; 
                     
                }
               
               // FromDate.ToString("mm/dd/yyyy");
                if (txtFromDate.Text == "")
                {
                    dt1 = "";
                }
                else
                {
                   // FromDate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null); 
                    
                    dt1 =txtFromDate.Text;
                }
                
                if (txtToDate.Text == "")
                {
                    dt2 = "";
                }
                else
                {
                    //ToDate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null); ;
                    dt2 = txtToDate.Text;
                }
                EmbargoRefNumber =(txtEmbargoRefNo.Text);
                if (EmbargoRefNumber == "")
                {
                    EmbargoRefNumber = "";   
                }
                status = ddlStatus.SelectedItem.Text;
                if (status == "Select")
                {
                    status = ""; 
                }
                blnResult = Embal.ListEmbargoDetails(Origintype, origin,Destinationtype,destination, EmbargoRefNumber, Level, ParameterCode, dt1, dt2,status);
                if (blnResult != null && blnResult.Tables[0].Rows.Count > 0)
                {
                    LbLStatus.Text = string.Empty;
                    GrdEmbargoDetails.DataSource = blnResult;
                    GrdEmbargoDetails.DataBind();
                    Session["EmbDetails"] = blnResult.Tables[0];
                    
                }
                else
                {
                    LbLStatus.Text = "No records found for given search criteria";
                    LbLStatus.ForeColor = System.Drawing.Color.Red;
                    GrdEmbargoDetails.DataSource = blnResult;
                    GrdEmbargoDetails.DataBind();
                    //GrdEmbargoDetails.Visible = false;
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion List
        #region DestinationType
        protected void ddldestinationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListDataViewBAL objDataViewBAL = new ListDataViewBAL();
                DataSet dsResult = new DataSet();
                string errormessage = "";

                string level = ddldestinationType.SelectedItem.Value;
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
            catch (Exception ex)
            { }
        }
        #endregion DestinationType
        //#region SelectMode
        //protected void SelectMode(object sender, EventArgs e)
        //{

        //    try
        //    {
        //        for (int i = 0; i < GrdEmbargoDetails.Rows.Count; i++)
        //        {
        //            string str = (((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlParameter")).Text);
        //            if (str == "Payment Type")
        //            {
        //                ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable")).Items.Add("PP");
        //                ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable")).Items.Add("CC");
        //                ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable")).Items.Add("CP");
        //                ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable")).Items.Add("PC");

        //            }
        //            else if (str == "Origin")
        //            {
        //                DataSet dsResult = new DataSet();
        //                //string errormessage = "";

        //                string level = (((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlParameter")).Text);
        //                dsResult = da.SelectRecords("SpGetCityCode");
        //                dsResult.Tables[0].Rows.Add("Select");


        //                DropDownList ddl = ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable"));

        //                ddl.DataSource = dsResult;
        //                ddl.DataMember = dsResult.Tables[0].TableName;
        //                ddl.DataValueField = dsResult.Tables[0].Columns[0].ColumnName;
        //                ddl.DataTextField = dsResult.Tables[0].Columns[0].ColumnName;
        //                ddl.DataBind();

        //                //ddl.DataMember = dsResult.Tables[0].TableName;
        //                //ddl.DataValueField = dsResult.Tables[0].Columns[0].ColumnName;
        //                //ddl.DataTextField = dsResult.Tables[0].Columns[0].ColumnName;
        //                //ddl.DataBind();
        //                //ddl.Items.Insert(0, new ListItem("Select", string.Empty));

        //            }
        //            else if (str == "Destination")
        //            {
        //                DataSet dsResult = new DataSet();
        //                //string errormessage = "";

        //                string level = (((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlParameter")).Text);
        //                dsResult = da.SelectRecords("SpGetCityCode");
        //                dsResult.Tables[0].Rows.Add("Select");


        //                DropDownList ddl = ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable"));

        //                ddl.DataSource = dsResult;
        //                ddl.DataMember = dsResult.Tables[0].TableName;
        //                ddl.DataValueField = dsResult.Tables[0].Columns[0].ColumnName;
        //                ddl.DataTextField = dsResult.Tables[0].Columns[0].ColumnName;
        //                ddl.DataBind();


        //            }
        //            else if (str == "Commodity")
        //            {
        //                DataSet dsResult = new DataSet();
        //                //string errormessage = "";

        //                string level = (((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlParameter")).Text);
        //                dsResult = da.SelectRecords("SpGetCommodity");
        //                //dsResult.Tables[0].Rows.Add("Select");


        //                DropDownList ddl = ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable"));

        //                ddl.DataSource = dsResult;
        //                ddl.DataMember = dsResult.Tables[0].TableName;
        //                ddl.DataValueField = dsResult.Tables[0].Columns[0].ColumnName;
        //                ddl.DataTextField = dsResult.Tables[0].Columns[0].ColumnName;
        //                ddl.DataBind();


        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    { }
        //}
        //#endregion Selectmode
        #region LoadGrid
        public void LoadGrid()
        {

            DataTable myDataTable = new DataTable();
            DataColumn myDataColumn;
            DataSet Ds = new DataSet();

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "ReferenceNumber";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "OriginType";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Origin";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "DestinationType";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Destination";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "RefLevel";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Discription";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "startDate";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "EndDate";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Remarks";
            myDataTable.Columns.Add(myDataColumn);
            
            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Status";
            myDataTable.Columns.Add(myDataColumn);


            DataRow dr;
            dr = myDataTable.NewRow();
            dr["ReferenceNumber"] = "";
            dr["OriginType"] = "";//"5";
            dr["Origin"] = "";// "5";
            dr["DestinationType"] = "";
            dr["Destination"] = "";
            dr["RefLevel"] = "";
            dr["Discription"] = "";
            dr["startDate"] = "";
            dr["EndDate"] = "";
            dr["Remarks"] = "";
            dr["Status"] = "";
            

            myDataTable.Rows.Add(dr);
            //ViewState["CurrentTable"] = myDataTable;
            //Bind the DataTable to the Grid

            GrdEmbargoDetails.DataSource = null;
            GrdEmbargoDetails.DataSource = myDataTable;
            GrdEmbargoDetails.DataBind();
            //DataSet dtCreditInfo = new DataSet();
            //dtCreditInfo.Tables.Add(myDataTable.Copy() );
            Session["dtCreditInfo"] = myDataTable.Copy();
        }
        #endregion LoadGrid
        #region Clear
        public void Clear()
        {
            try
            {
                txtParametervalue.Text= "";
                txtFromDate.Text = "";
                txtToDate.Text = "";
                LbLStatus.Text = string.Empty;
                txtEmbargoRefNo.Text = "";
                LoadGrid(); 
            }
            catch (Exception ex)
            { }
        
        }
        #endregion Clear
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Clear(); 
            }
            catch (Exception ex)
            { }
 
        }
        protected void GrdEmbargoDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Edit")
                { 
                 int index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = GrdEmbargoDetails.Rows[index];
                    string RefNo = ((Label)row.FindControl("lblEmbargoRefNo")).Text; //row.Cells[0].Text;
                    Response.Redirect("~/MaintainEmbargo.aspx?cmd=" + e.CommandName + "&RefNo=" + RefNo);
                }
            }
            catch (Exception exObj)
            {
                string ex = exObj.ToString();
            }
                
        }
        protected void GrdEmbargoDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdEmbargoDetails.PageIndex = e.NewPageIndex;
            GrdEmbargoDetails.DataSource = (DataTable)Session["EmbDetails"];
            GrdEmbargoDetails.DataBind();
        }
    }
}
