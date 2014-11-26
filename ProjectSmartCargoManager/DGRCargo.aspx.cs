using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;
using BAL;
namespace ProjectSmartCargoManager
{
    public partial class DGRCargo : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());
        DGRCargoBAL dgr = new DGRCargoBAL();
        string AWBNumber = string.Empty; 
        bool blnres;
        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string awb = string.Empty;

                if (Session["DGRAWB"] == null)
                    awb = "";
                else
                    awb = (string)Session["DGRAWB"];

                lblAWBNumber.Text = awb; 
                LoadGrid();
                getDGR(awb);
               
            }
        }
#endregion Load
        public void getDGR(string AWBNumber)
        {
            try
            {
                DataSet ds = new DataSet("DGRCargo_1");
                ds = da.SelectRecords("SpListDGRCargo", "AWBNumber", AWBNumber, SqlDbType.VarChar);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            grdDGRCargo.DataSource = ds;
                            grdDGRCargo.DataBind();
                            
                            LoadDropDown();
                            string unid = ds.Tables[0].Rows[0]["UNID"].ToString();

                            foreach (GridViewRow row in grdDGRCargo.Rows)
                            {
                                for(int j=0;j<ds.Tables[0].Rows.Count;j++)
                                {
                                    if (((TextBox)row.FindControl("txtDescr")).Text.Contains(ds.Tables[0].Rows[j]["Description"].ToString()))
                                    {
                                        ((DropDownList)row.FindControl("ddlUNID")).Text = ds.Tables[0].Rows[j]["UNID"].ToString();
                                    }
                                }
                                
                            }
                            

                        }
                    }
                }
      
            }
            catch (Exception ex)
            { }

        }
        #region newMethod
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string[] GetUnidNo(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string");
            //SELECT FlightID from dbo.AirlineSchedule where FlightID like '"+ prefixText +"%' or FlightID like '"+ prefixText +"%'

            SqlDataAdapter dad = new SqlDataAdapter("SELECT UNIDNo from UNIDDetails where UNIDNo like '" + prefixText + "%' or UNIDNo like '" + prefixText + "%'", con);
            DataSet ds = new DataSet("DGRCargo_2");
            dad.Fill(ds);
            List<string> list = new List<string>(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(dr[0].ToString());

            }

            return list.ToArray();
        }

        #endregion newMethod
        #region GetUnid
        public void GetUNID(object sender, EventArgs e)
        {

        }
        #endregion GetUnid
        #region LoadGrid
        public void LoadGrid()
        {

            DataTable myDataTable = new DataTable("DGRCargo_11");
            DataColumn myDataColumn;
            DataSet Ds = new DataSet("DGRCargo_3");

            //myDataColumn = new DataColumn();
            //myDataColumn.DataType = Type.GetType("System.String");
            //myDataColumn.ColumnName = "UNID";
            //myDataTable.Columns.Add(myDataColumn);
            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Description";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Pieces";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Weight";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "ERGCode";
            myDataTable.Columns.Add(myDataColumn);


            DataRow dr;
            dr = myDataTable.NewRow();
            //dr["UNID"] = "";
            dr["Description"] = "";
            dr["Pieces"] = "";//"5";
            dr["Weight"] = "";// "5";
            dr["ERGCode"] = "";// "5";

            myDataTable.Rows.Add(dr);
            //ViewState["CurrentTable"] = myDataTable;
            //Bind the DataTable to the Grid

            grdDGRCargo.DataSource = null;
            grdDGRCargo.DataSource = myDataTable;
            grdDGRCargo.DataBind();

            //"select convert(varchar,UNID)+'('+Descriptoin+')' as UNIDDetail,UNID from unidmaster"
            string queryToGetUNID = "select top(10) convert(varchar,UNIDNo)+'('+ShippingName+')' as UNIDDetail,UNIDNo as UNID from uniddetails";
            DataSet ds = new DataSet("DGRCargo_4");
                ds =  da.GetDataset(queryToGetUNID);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (GridViewRow row in grdDGRCargo.Rows)
                    {
                        ((DropDownList)row.FindControl("ddlUNID")).DataSource = ds.Tables[0];
                        ((DropDownList)row.FindControl("ddlUNID")).DataTextField = "UNIDDetail";
                        ((DropDownList)row.FindControl("ddlUNID")).DataValueField = "UNID";
                        ((DropDownList)row.FindControl("ddlUNID")).DataBind();
                        ((DropDownList)row.FindControl("ddlUNID")).Items.Insert(0, new ListItem("Select"));
                    }
                }
            }
            //DataSet dtCreditInfo = new DataSet();
            //dtCreditInfo.Tables.Add(myDataTable.Copy() );
            Session["dtCreditInfo"] = myDataTable.Copy();
        }
        #endregion LoadGrid
        #region SaveGridData
        public void SaveRouteDetails()
        {
            try
            {
                DataTable dtCreditInfo = new DataTable("DGRCargo_12");
                   dtCreditInfo = ((DataTable)Session["dtCreditInfo"]).Clone();
                for (int i = 0; i < grdDGRCargo.Rows.Count; i++)
                {
                    DataRow row = dtCreditInfo.NewRow();
                    row["UNID"] = ((TextBox)grdDGRCargo.Rows[i].FindControl("txtUNID")).Text;

                    row["Pieces"] = ((TextBox)grdDGRCargo.Rows[i].FindControl("txtPCS")).Text;

                    row["Weight"] = ((TextBox)grdDGRCargo.Rows[i].FindControl("txtWeight")).Text;

                    row["ERGCde"] = ((TextBox)grdDGRCargo.Rows[i].FindControl("txtERGCode")).Text;

                    dtCreditInfo.Rows.Add(row);
                }

                Session["dtCreditInfo"] = dtCreditInfo.Copy();
            }
            catch (Exception ex) { }
        }
#endregion SaveGridData
        #region AddnewRow To Grid
        private void AddNewRowToGrid()
        {
            try
            {
                //DataTable dtCreditInfo = (DataTable)Session["dtCreditInfo"];
                //DataRow rw = dtCreditInfo.NewRow();

                //dtCreditInfo.Rows.Add(rw);

                //grdDGRCargo.DataSource = dtCreditInfo.Copy();
                //grdDGRCargo.DataBind();


                //for (int i = 0; i < dtCreditInfo.Rows.Count; i++)
                //{

                //    DataRow row = dtCreditInfo.Rows[i];

                //    ((TextBox)grdDGRCargo.Rows[i].FindControl("txtUNID")).Text = row["UNID"].ToString();
                //    ((TextBox)grdDGRCargo.Rows[i].FindControl("txtPCS")).Text = row["Pieces"].ToString();
                //    ((TextBox)grdDGRCargo.Rows[i].FindControl("txtWeight")).Text = row["Weight"].ToString();


                //}
                DataTable dt = new DataTable("DGRCargo_13");
                dt.Columns.Add("Description", typeof(string));
                dt.Columns.Add("UNID", typeof(string));
                dt.Columns.Add("Pieces", typeof(string));
                
                dt.Columns.Add("Weight", typeof(string));
                dt.Columns.Add("ERGCode", typeof(string));

                for (int i = 0; i < grdDGRCargo.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["Description"] = ((TextBox)grdDGRCargo.Rows[i].FindControl("txtDescr")).Text;
                    dr["UNID"] = ((DropDownList)grdDGRCargo.Rows[i].FindControl("ddlUNID")).Text;
                    dr["Pieces"] = ((TextBox)grdDGRCargo.Rows[i].FindControl("txtPCS")).Text;
                    dr["Weight"] = ((TextBox)grdDGRCargo.Rows[i].FindControl("txtWeight")).Text;
                    dr["ERGCode"] = ((TextBox)grdDGRCargo.Rows[i].FindControl("txtERGCode")).Text;
                    
                    dt.Rows.Add(dr);
                }
                DataRow dr1 = dt.NewRow();
                dr1["Description"] =string.Empty;
                dr1["UNID"] = string.Empty;
                dr1["Pieces"] = string.Empty;
                dr1["Weight"] = string.Empty;
                dr1["ERGCode"] = string.Empty;
                dt.Rows.Add(dr1);

                if (dt.Rows.Count > 0)
                {
                    grdDGRCargo.DataSource = dt;
                    grdDGRCargo.DataBind();

                    LoadDropDown();
                    for (int j = 0; j < grdDGRCargo.Rows.Count; j++)
                    {
                        string des = ((TextBox)grdDGRCargo.Rows[j].FindControl("txtDescr")).Text;
                        if (des.Trim() != "")
                        {
                            //if (((DropDownList)grdDGRCargo.Rows[j].FindControl("ddlUNID")).SelectedItem.Text.Contains(des))
                            //{
                                //string ddlText = ((DropDownList)grdDGRCargo.Rows[j].FindControl("ddlUNID")).SelectedItem.Text;
                                //string[] s = ddlText.Split('(');
                            
                            ((DropDownList)grdDGRCargo.Rows[j].FindControl("ddlUNID")).Text = dt.Rows[j]["UNID"].ToString();
                            
                            //}
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }

        }
        #endregion AddnewRow To Grid
        #region AddRow
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                LBLStatus.Text = "";
                //SaveRouteDetails();
                AddNewRowToGrid();
            }
            catch (Exception ex)
            { }
        }
        #endregion AddRow
        #region SaveGridEmbargo
        public void SaveMaterialGrid()
        {
            try
            {
                DataTable dtCreditInfo = new DataTable("DGRCargo_14");
                  dtCreditInfo =  ((DataTable)Session["dtCreditInfo"]).Clone();

                int i = 0;
                foreach (GridViewRow row in grdDGRCargo.Rows)
                {
                    i++;
                    DataRow rw = dtCreditInfo.NewRow();
                    //((DropDownList)row.FindControl("ddlMaterialCommCode")).SelectedItem.Text

                    rw["UNID"] = ((TextBox)row.FindControl("txtUNID")).Text;

                    rw["Pieces"] = ((TextBox)row.FindControl("txtPCS")).Text;

                    rw["Weight"] = ((TextBox)row.FindControl("txtWeight")).Text;

                    rw["ERGCode"] = ((TextBox)row.FindControl("txtERGCode")).Text;


                    dtCreditInfo.Rows.Add(row);


                    //dsMaterialDetails.Tables[0].Rows.Add(rw);
                }

                Session["dsMaterialDetails"] = dtCreditInfo.Copy();

            }
            catch (Exception ex)
            {
                LBLStatus.Text = "" + ex.Message;
                LBLStatus.ForeColor = Color.Red;
            }
        }
        #endregion SaveGridEmbargo
        #region BtnDelete
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //LBLStatus.Text = "";
            //SaveMaterialGrid();
            //DataTable dsMaterialDetailsTemp = ((DataTable)Session["dsMaterialDetails"]).Clone();
            //DataTable dsMaterialDetails = (DataTable)Session["dsMaterialDetails"];


            //for (int i = 0; i < grdDGRCargo.Rows.Count; i++)
            //{

            //    DataRow rw = dsMaterialDetailsTemp.NewRow();
            //    rw["UNID"] = ((TextBox)grdDGRCargo.Rows[i].FindControl("txtUNID")).Text;

            //    rw["Pieces"] = ((TextBox)grdDGRCargo.Rows[i].FindControl("txtPCS")).Text;

            //    rw["Weight"] = ((TextBox)grdDGRCargo.Rows[i].FindControl("txtWeight")).Text;
            //    //rw["RowIndex"] = "" + i;

            //    dsMaterialDetailsTemp.Rows.Add(rw);

            //}
            //if (dsMaterialDetailsTemp != null)
            //{
            //    dsMaterialDetailsTemp.Rows[dsMaterialDetailsTemp.Rows.Count - 1].Delete();
            //}
            //Session["dsMaterialDetails"] = dsMaterialDetailsTemp.Copy();
            //grdDGRCargo.DataSource = dsMaterialDetailsTemp.Copy();
            //grdDGRCargo.DataBind();

            //for (int i = 0; i < dsMaterialDetailsTemp.Rows.Count; i++)
            //{
            //    ((TextBox)grdDGRCargo.Rows[i].FindControl("txtUNID")).Text = dsMaterialDetailsTemp.Rows[i]["UNID"].ToString();
            //    ((TextBox)grdDGRCargo.Rows[i].FindControl("txtPCS")).Text = dsMaterialDetailsTemp.Rows[i]["Pieces"].ToString();
            //    ((TextBox)grdDGRCargo.Rows[i].FindControl("txtWeight")).Text=dsMaterialDetailsTemp.Rows[i]["Weight"].ToString();
            //}
            #region Delete
            if (grdDGRCargo.Rows.Count == 1)
                return;
            DataTable dt = new DataTable("DGRCargo_15");
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("UNID", typeof(string));
            dt.Columns.Add("Pieces", typeof(string));
            dt.Columns.Add("Weight", typeof(string));
            dt.Columns.Add("ERGCode", typeof(string));

            int grdcnt = grdDGRCargo.Rows.Count - 1;
            for (int i = 0; i < grdcnt; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Description"] = ((TextBox)grdDGRCargo.Rows[i].FindControl("txtDescr")).Text;
                dr["UNID"] = ((DropDownList)grdDGRCargo.Rows[i].FindControl("ddlUNID")).Text;
                dr["Pieces"] = ((TextBox)grdDGRCargo.Rows[i].FindControl("txtPCS")).Text;
                dr["Weight"] = ((TextBox)grdDGRCargo.Rows[i].FindControl("txtWeight")).Text;
                dr["ERGCode"] = ((TextBox)grdDGRCargo.Rows[i].FindControl("txtERGCode")).Text;
                
                dt.Rows.Add(dr);
            }
            //DataRow dr1 = dt.NewRow();
            //dr1["Description"] = string.Empty;
            //dr1["UNID"] = string.Empty;
            //dr1["Pieces"] = string.Empty;
            //dr1["Weight"] = string.Empty;
            //dt.Rows.Add(dr1);

            if (dt.Rows.Count > 0)
            {
                grdDGRCargo.DataSource = dt;
                grdDGRCargo.DataBind();

                LoadDropDown();
                for (int j = 0; j < grdDGRCargo.Rows.Count; j++)
                {
                    string des = ((TextBox)grdDGRCargo.Rows[j].FindControl("txtDescr")).Text;
                    if (des.Trim() != "")
                    {
                        //if (((DropDownList)grdDGRCargo.Rows[j].FindControl("ddlUNID")).SelectedItem.Text.Contains(des))
                        //{
                        //string ddlText = ((DropDownList)grdDGRCargo.Rows[j].FindControl("ddlUNID")).SelectedItem.Text;
                        //string[] s = ddlText.Split('(');

                        ((DropDownList)grdDGRCargo.Rows[j].FindControl("ddlUNID")).Text = dt.Rows[j]["UNID"].ToString();

                        //}
                    }
                }
            }
            #endregion 
        }
        #endregion btnDelete
        #region Save
        protected void btnSave_Click(object sender, EventArgs e)
        {
           string AWBNumber,UNID,Weight,UpdatedBy,UpdatedOn,PG,ERGCode,Desc;
             int Pieces;

             DataTable dtSurface = new DataTable("DGRCargo_16");

             dtSurface.Columns.Add("AWBNumber", typeof(string));
             dtSurface.Columns.Add("UNID", typeof(string));
             dtSurface.Columns.Add("Pieces", typeof(string));
             dtSurface.Columns.Add("Weight", typeof(string));
             dtSurface.Columns.Add("ERGCode", typeof(string));
             dtSurface.Columns.Add("Desc", typeof(string));           
             dtSurface.Columns.Add("UpdatedBy", typeof(string));
             dtSurface.Columns.Add("UpdatedOn", typeof(string));
             dtSurface.Columns.Add("PG", typeof(string));

            try
            {
                for (int i = 0; i < grdDGRCargo.Rows.Count; i++)
                {
                    if (((DropDownList)grdDGRCargo.Rows[i].FindControl("ddlUNID")).SelectedIndex == 0)
                    {
                        LBLStatus.Text = "Select UNID";
                        LBLStatus.ForeColor = Color.Red;
                        return;
                    }
                    AWBNumber = "";
                    UNID = ((DropDownList)grdDGRCargo.Rows[i].FindControl("ddlUNID")).SelectedItem.Value;
                    //UNID = ((TextBox)grdDGRCargo.Rows[i].FindControl("txtUNID")).Text;
                    Pieces =Convert.ToInt32(((TextBox)grdDGRCargo.Rows[i].FindControl("txtPCS")).Text);
                    Weight = ((TextBox)grdDGRCargo.Rows[i].FindControl("txtWeight")).Text;
                    ERGCode = ((TextBox)grdDGRCargo.Rows[i].FindControl("txtERGCode")).Text;
                    UpdatedBy = Session["UserName"].ToString();
                    UpdatedOn = DateTime.Now.ToShortDateString();
                    PG = ((DropDownList)grdDGRCargo.Rows[i].FindControl("ddlPG")).SelectedItem.Text;
                    Desc = ((TextBox)grdDGRCargo.Rows[i].FindControl("txtDescr")).Text;
                    
                    DataSet ds = new DataSet("DGRCargo_5");
                       ds =  da.SelectRecords("SpgetUNID", "UNID", UNID, SqlDbType.VarChar);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow drSurface = dtSurface.NewRow();

                        drSurface["AWBNumber"] = "";//0
                        drSurface["UNID"] = UNID;//1
                        drSurface["Pieces"] = Pieces;//2
                        drSurface["Weight"] = Weight;//3
                        drSurface["ERGCode"] = ERGCode;//4
                        drSurface["UpdatedBy"] = UpdatedBy;//5
                        drSurface["UpdatedOn"] = UpdatedOn;//6
                        drSurface["PG"] = PG;//7
                        drSurface["Desc"] = Desc;//8
                    
                        dtSurface.Rows.Add(drSurface);
                    }
                    else
                    {
                        LBLStatus.Text = "Please Enter valid UNID Number";
                        LBLStatus.ForeColor = Color.Red;
                        return;
                    }

                   
                }
                Session["DgrCargo"] = dtSurface;
                if (dtSurface!=null)
                {
                    if (dtSurface.Rows.Count > 0)
                    {
                        LBLStatus.Text = "DGR Cargo Saved Successfully";
                        LBLStatus.ForeColor = Color.Green;
                        return;
                    }
                }
                else
                {
                    LBLStatus.Text = "Sorry..DGR Cargo Not Saved.";
                    LBLStatus.ForeColor = Color.Red;
                    return;
                }
            }
            catch (Exception ex)
            {
                LBLStatus.Text = "Sorry..DGR Cargo Not Saved Please Check Data";
                LBLStatus.ForeColor = Color.Red;
                return;
            }

        }
        #endregion save
        #region Clear
        protected void btnclear_Click(object sender, EventArgs e)
        {
            try
            {

                LoadGrid();
                
            }
            catch (Exception ex)
            { 
            }
        }
        #endregion Clear

        protected void ddlUNID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in grdDGRCargo.Rows)
                {
                    if (((DropDownList)row.FindControl("ddlUNID")).SelectedIndex == 0)
                    {
                        ((TextBox)row.FindControl("txtDescr")).Text = string.Empty;
                        return;
                    }
                    string UNID = ((DropDownList)row.FindControl("ddlUNID")).SelectedItem.Text;
                    string[] s = UNID.Split('(');
                    ((TextBox)row.FindControl("txtDescr")).Text = s[1].TrimEnd(')');

                    try
                    {
                        string UNIDPG = s[0];
                        DataSet DsUNIDPG = new DataSet("DGRCargo_6");
                            DsUNIDPG = da.SelectRecords("SP_GetPackagingGroup", "UNIDPG", UNIDPG, SqlDbType.VarChar);

                        string [] PG = DsUNIDPG.Tables[0].Rows[0]["PG"].ToString().Split(',');

                        ((DropDownList)row.FindControl("ddlPG")).DataSource = PG;
                        ((DropDownList)row.FindControl("ddlPG")).DataBind();

                    }
                    catch (Exception ex)
                    { }
                }
            }
            catch (Exception ex)
            { }
            

        }

        private bool LoadDropDown()
        {
            string queryToGetUNID = "select top(12) convert(varchar,UNIDNo)+'('+ShippingName+')' as UNIDDetail,UNIDNo as UNID from uniddetails";
            DataSet ds = new DataSet("DGRCargo_7");
                ds = da.GetDataset(queryToGetUNID);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (GridViewRow row in grdDGRCargo.Rows)
                    {
                        ((DropDownList)row.FindControl("ddlUNID")).DataSource = ds.Tables[0];
                        ((DropDownList)row.FindControl("ddlUNID")).DataTextField = "UNIDDetail";
                        ((DropDownList)row.FindControl("ddlUNID")).DataValueField = "UNID";
                        ((DropDownList)row.FindControl("ddlUNID")).DataBind();
                        ((DropDownList)row.FindControl("ddlUNID")).Items.Insert(0, new ListItem("Select"));
                    }
                }
            }
            return true;
        }
    }
}
