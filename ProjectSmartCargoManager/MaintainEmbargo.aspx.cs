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
    public partial class MaintainEmbargo : System.Web.UI.Page
    {
        EmbargoBAL Embal = new EmbargoBAL();
        SQLServer da = new SQLServer(Global.GetConnectionString());
        DataSet blnResult;
        #region Pageload
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LoadGrid();
                    btnSave.Text = "Save";
                    //ddlDestination.Items.Add("Select");
                    //ddlOrigin.Items.Add("Select");
                    txtstartdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtenddate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    if (Request.QueryString["cmd"] == "Edit")
                    {
                        txtReferenceNo.Text = Request.QueryString["RefNo"].ToString();
                        btnDisplayDetails_Click(null, null);
                    }

                }
            }
            catch (Exception ex)
            { }
        }
        #endregion Pageload
        #region Save
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string refno = "", level = "", status = "", origintype = "", origin = "", Destinationtype = "", Destination = "", Description = "", Remarks = "";
                string parameter = "", Applicable = "", Suspend = "", DaysOfWeek = "";
                DateTime startdate = DateTime.Now;
                DateTime enddate = DateTime.Now;
                string value = string.Empty; ;

                if (txtReferenceNo.Text == "")
                {
                    refno = "";
                }
                else
                {
                    refno = txtReferenceNo.Text;
                    //DataSet dsdel = Embal.DeleteEmbago(refno);
                }
                            if (txtReferenceNo.Text == "")
                                refno = "";
                            else
                                refno = txtReferenceNo.Text;
                            level = ddlLevel.SelectedItem.Text;
                            status = ddlStatus.SelectedItem.Text;

                            if(!String.IsNullOrEmpty(txtstartdate.Text))
                                startdate = DateTime.ParseExact(txtstartdate.Text, "dd/MM/yyyy", null);
                            if (!String.IsNullOrEmpty(txtenddate.Text))
                            enddate = DateTime.ParseExact(txtenddate.Text, "dd/MM/yyyy", null);

                            if (ddlorigintype.SelectedItem.Text == ""||ddlorigintype.SelectedItem.Text=="Select")
                            {
                                lblStatus.Text = "Please Provide Origin Type";
                                lblStatus.ForeColor = Color.Red;
                                origin = "";
                                return;
                                
                            }
                            else
                            {
                                origintype = ddlorigintype.SelectedItem.Text;
                            }
                            if (ddlOrigin.SelectedItem.Text == "Select")
                            {
                                lblStatus.Text = "Please Provide Origin";
                                lblStatus.ForeColor = Color.Red;
                                return;
                            }
                            else
                            {
                                origin = ddlOrigin.SelectedValue;
                                //origin = ddlOrigin.SelectedItem.Text;
                            }
                            if (ddldestinationType.SelectedItem.Text == ""||ddldestinationType.SelectedItem.Text=="Select")
                            {
                                lblStatus.Text = "Please Provide Destination Type";
                                lblStatus.ForeColor = Color.Red;
                                Destination = "";
                                return;
                            }
                            else
                            {
                                Destinationtype = ddldestinationType.SelectedItem.Text;
                            }
                            if (ddlDestination.SelectedItem.Text == "Select")
                            {
                                lblStatus.Text = "Please Provide Destination";
                                lblStatus.ForeColor = Color.Red;
                                return;
                            }
                            else
                            {
                                Destination = ddlDestination.SelectedValue;
                                //  Destination = ddlDestination.SelectedItem.Text;
                            }
                            Description = txtDescription.Text;
                            Remarks = txtremarks.Text;
                            if (ChkSuspend.Checked == true)
                            {
                                Suspend = "true";
                            }
                            else
                            {
                                Suspend = "false";
                            }
                            parameter = (((DropDownList)GrdEmbargoDetails.Rows[0].FindControl("ddlParameter")).Text);
                            Applicable = (((DropDownList)GrdEmbargoDetails.Rows[0].FindControl("ddlApplicable")).Text);

                            value = ((TextBox)GrdEmbargoDetails.Rows[0].FindControl("txtvalues")).Text;
                            if (value == null)
                            {
                                value = "";
                            }
                            DaysOfWeek = "";
                            for (int j = 0; j < cblWeekdays.Items.Count; j++)
                            {

                                if (cblWeekdays.Items[j].Selected)
                                {
                                    DaysOfWeek += 1;
                                }
                                else
                                {
                                    DaysOfWeek += 0;
                                }
                            }

                            blnResult = Embal.SaveEmbago(refno, level, status, startdate, enddate, origintype,
                                     origin, Destinationtype, Destination, Description, Remarks, Suspend, parameter, Applicable, value, DaysOfWeek);
                            txtReferenceNo.Text = blnResult.Tables[0].Rows[0][0].ToString();
                      
                
                if (blnResult != null)
                {
                    string value1 = "SAVE";
                    MasterLog(value1);

                    lblStatus.Text = "Embargo Saved Succssfully";
                    lblStatus.ForeColor = Color.Green;
                    return;
                }
                else
                {
                    lblStatus.Text = "Embargo not Saved..Please check Data";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
            }

            catch (Exception ex)
            {
                lblStatus.Text = "Embargo not Saved..Please check Data";
                lblStatus.ForeColor = Color.Red;
                return;
            }
        }
        #endregion Save
        #region Clear
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                clear();
            }
            catch (Exception ex)
            { }

        }
        #endregion Clear
        #region clearFunction
        public void clear()
        {
            try
            {
                lblStatus.Text = "";
                txtReferenceNo.Text = "";
                //ddlLevel.SelectedItem.Text = "";
                ddlLevel.SelectedIndex = ddlStatus.SelectedIndex = 0;
                //txtstatus.Text = "";
                ChkSuspend.Checked = false;
                txtstartdate.Text = "";
                txtenddate.Text = "";
                //ddlorigintype.SelectedItem.Text = "";
                //ddlDestination.SelectedItem .Text = "";
                //ddldestinationType.SelectedItem.Text = "";
                //ddlOrigin.SelectedItem.Text = "";
                ddlorigintype.SelectedIndex = 0;// ddlOrigin.SelectedIndex = 0;
                ddldestinationType.SelectedIndex = 0;// ddlDestination.SelectedIndex = 0;
                ddldestinationType_SelectedIndexChanged(null, null);
                ddlorigintype_SelectedIndexChanged(null, null);
                txtDescription.Text = "";
                txtremarks.Text = "";
                cblWeekdays.SelectedIndex = -1;
                //cblWeekdays.ClearSelection();
                btnSave.Text = "Save";
                LoadGrid();
                //(((TextBox)GrdEmbargoDetails.Rows[0].FindControl("txtvalues")).Text) = "";
            }
            catch (Exception ex)
            { }
        }
        #endregion ClearFunction
        #region LoadGrid
        public void LoadGrid()
        {

            DataTable myDataTable = new DataTable();
            DataColumn myDataColumn;
            DataSet Ds = new DataSet();

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Parameter";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Applicable";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Values";
            myDataTable.Columns.Add(myDataColumn);



            DataRow dr;
            dr = myDataTable.NewRow();
            dr["Parameter"] = "";
            dr["Applicable"] = "";//"5";
            dr["Values"] = "";// "5";

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
        #region Save
        public void SaveRouteDetails()
        {
            try
            {
                DataTable dtCreditInfo = ((DataTable)Session["dtCreditInfo"]).Clone();
                for (int i = 0; i < GrdEmbargoDetails.Rows.Count; i++)
                {
                    DataRow row = dtCreditInfo.NewRow();
                    row["Parameter"] = ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlParameter")).SelectedItem.Text;

                    row["Values"] = ((TextBox)GrdEmbargoDetails.Rows[i].FindControl("txtvalues")).Text;

                    row["Applicable"] = ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable")).SelectedItem.Value;

                    dtCreditInfo.Rows.Add(row);
                }

                Session["dtCreditInfo"] = dtCreditInfo.Copy();
            }
            catch (Exception ex) { }
        }
        #endregion Save
        #region AddnewRow To Grid
        private void AddNewRowToGrid()
        {
            try
            {
                DataTable dtCreditInfo = (DataTable)Session["dtCreditInfo"];
                DataRow rw = dtCreditInfo.NewRow();

                dtCreditInfo.Rows.Add(rw);

                GrdEmbargoDetails.DataSource = dtCreditInfo.Copy();
                GrdEmbargoDetails.DataBind();


                for (int i = 0; i < dtCreditInfo.Rows.Count; i++)
                {

                    DataRow row = dtCreditInfo.Rows[i];

                    ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlParameter")).SelectedValue = row["Parameter"].ToString();
                    ((TextBox)GrdEmbargoDetails.Rows[i].FindControl("txtvalues")).Text = row["Values"].ToString();
                    ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable")).Items.Add(row["Applicable"].ToString());


                }


            }
            catch (Exception e)
            {

            }

        }
        #endregion AddnewRow To Grid
        #region SaveGridEmbargo
        public void SaveMaterialGrid()
        {
            try
            {
                DataTable dtCreditInfo = ((DataTable)Session["dtCreditInfo"]).Clone();

               // int i = 0;
                //foreach (GridViewRow row in GrdEmbargoDetails.Rows)
                //{
                for (int i = 0; i < GrdEmbargoDetails.Rows.Count; i++)
                {
                    //i++;
                    
                    DataRow rw = dtCreditInfo.NewRow();
                    //((DropDownList)row.FindControl("ddlMaterialCommCode")).SelectedItem.Text
                    rw["Parameter"] = (((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlParameter")).Text);
                    rw["Values"] = ((TextBox)GrdEmbargoDetails.Rows[i].FindControl("txtvalues")).Text;

                    rw["Applicable"] = (((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable")).Text);
                   
                    dtCreditInfo.Rows.Add(rw);


                    //dsMaterialDetails.Tables[0].Rows.Add(rw);
                }

                Session["dsMaterialDetails"] = dtCreditInfo.Copy();

            }
            catch (Exception ex)
            {
                lblStatus.Text = "" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion SaveGridEmbargo
        #region SelectMode
        public void SelectMode(object sender, EventArgs e)
        {
            try
            {
                //(((GridViewRow)((DropDownList)sender).NamingContainer)).RowIndex

                //for (int i = 0; i < GrdEmbargoDetails.Rows.Count; i++)
                //{
                int i = 0;
                try
                {
                    i = (((GridViewRow)((DropDownList)sender).NamingContainer)).RowIndex;
                }
                catch
                {
                    return;
                }

                if (i == -1)
                    return;

                string str = (((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlParameter")).Text);

                if (str == "Select")
                    ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable")).Items.Clear();

                if (str == "Payment Type")
                {
                    ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable")).Items.Clear();
                    ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable")).Items.Add("CC");
                    ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable")).Items.Add("CP");
                    ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable")).Items.Add("PC");
                    ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable")).Items.Add("PP");
                }
                else if (str == "Origin")
                {
                    DataSet dsResult = new DataSet();
                    //string errormessage = "";

                    string level = (((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlParameter")).Text);
                    dsResult = da.SelectRecords("SpGetCityCode");
                    dsResult.Tables[0].Rows.Add("Select");

                    ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable")).Items.Clear();
                    DropDownList ddl = ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable"));

                    ddl.DataSource = dsResult;
                    ddl.DataMember = dsResult.Tables[0].TableName;
                    ddl.DataValueField = dsResult.Tables[0].Columns[0].ColumnName;
                    ddl.DataTextField = dsResult.Tables[0].Columns[0].ColumnName;
                    ddl.DataBind();

                    //ddl.DataMember = dsResult.Tables[0].TableName;
                    //ddl.DataValueField = dsResult.Tables[0].Columns[0].ColumnName;
                    //ddl.DataTextField = dsResult.Tables[0].Columns[0].ColumnName;
                    //ddl.DataBind();
                    //ddl.Items.Insert(0, new ListItem("Select", string.Empty));
                }
                else if (str == "Destination")
                {
                    DataSet dsResult = new DataSet();
                    //string errormessage = "";

                    string level = (((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlParameter")).Text);
                    dsResult = da.SelectRecords("SpGetCityCode");
                    dsResult.Tables[0].Rows.Add("Select");

                    ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable")).Items.Clear();

                    DropDownList ddl = ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable"));

                    ddl.DataSource = dsResult;
                    ddl.DataMember = dsResult.Tables[0].TableName;
                    ddl.DataValueField = dsResult.Tables[0].Columns[0].ColumnName;
                    ddl.DataTextField = dsResult.Tables[0].Columns[0].ColumnName;
                    ddl.DataBind();
                }
                else if (str == "Commodity")
                {
                    DataSet dsResult = new DataSet();
                    //string errormessage = "";

                    string level = (((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlParameter")).Text);
                    dsResult = da.SelectRecords("SpGetCommodity");
                    //dsResult.Tables[0].Rows.Add("Select");
                    ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable")).Items.Clear();

                    DropDownList ddl = ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable"));

                    ddl.DataSource = dsResult;
                    ddl.DataMember = dsResult.Tables[0].TableName;
                    ddl.DataValueField = dsResult.Tables[0].Columns[0].ColumnName;
                    ddl.DataTextField = dsResult.Tables[0].Columns[0].ColumnName;
                    ddl.DataBind();
                }
                else if (str == "Flight Number")
                {
                    DataSet dsResult = new DataSet();
                    //string errormessage = "";

                    string level = (((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlParameter")).Text);
                    dsResult = da.SelectRecords("SP_GetFlightID");
                    //dsResult.Tables[0].Rows.Add("Select");
                    ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable")).Items.Clear();
                    DropDownList ddl = ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable"));

                    ddl.DataSource = dsResult;
                    ddl.DataMember = dsResult.Tables[0].TableName;
                    ddl.DataValueField = dsResult.Tables[0].Columns[0].ColumnName;
                    ddl.DataTextField = dsResult.Tables[0].Columns[0].ColumnName;
                    ddl.DataBind();
                }
                else if (str == "Booking")
                {
                    ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable")).Items.Clear();
                    ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable")).Items.Add("All");
                    ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable")).Items.Add("No");
                }

                //}
            }
            catch (Exception ex)
            { }
        }
        #endregion SelectMode
        #region AddRow
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                SaveRouteDetails();
                AddNewRowToGrid();
                string value = "ADD";
                MasterLog(value);

            }
            catch (Exception ex)
            { }

        }
        #endregion AddRow
        #region DeleteRow
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            #region commentedCode
            try
            {
                lblStatus.Text = "";
                SaveMaterialGrid();
                DataTable dsMaterialDetailsTemp = ((DataTable)Session["dsMaterialDetails"]).Clone();
                DataTable dsMaterialDetails = (DataTable)Session["dsMaterialDetails"];

                for (int i = 0; i < GrdEmbargoDetails.Rows.Count; i++)
                {
                    if (((CheckBox)GrdEmbargoDetails.Rows[i].FindControl("check")).Checked == false)
                    {

                        DataRow rw = dsMaterialDetailsTemp.NewRow();
                        rw["Parameter"] = ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlParameter")).SelectedItem.Text;

                        rw["Values"] = ((TextBox)GrdEmbargoDetails.Rows[i].FindControl("txtvalues")).Text;

                        rw["Applicable"] = ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable")).SelectedItem.Value;

                        //rw["check"] = "" + i;

                        dsMaterialDetailsTemp.Rows.Add(rw);
                        //}
                    }
                    //for (int i = 0; i < GrdEmbargoDetails.Rows.Count; i++)
                    //{
                    //    if (((CheckBox)GrdEmbargoDetails.Rows[i].FindControl("check")).Checked == true)
                    //    {

                    //        dsMaterialDetailsTemp.Rows.RemoveAt(i);
                    //    }
                    //}
                }
                        if (dsMaterialDetailsTemp != null)
                        {

                            string value = "DELETE";
                            MasterLog(value);

                           // dsMaterialDetailsTemp.Rows[dsMaterialDetailsTemp.Rows.Count].Delete();
                        }
                    
                
               
                Session["dsMaterialDetails"] = dsMaterialDetailsTemp.Copy();
                GrdEmbargoDetails.DataSource = dsMaterialDetailsTemp.Copy();
                GrdEmbargoDetails.DataBind();

                // GrdEmbargoDetails.DataSource = dsMaterialDetailsTemp.Copy();

                for (int i = 0; i < dsMaterialDetailsTemp.Rows.Count; i++)
                {
                    ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlParameter")).SelectedItem.Text = dsMaterialDetailsTemp.Rows[i]["Parameter"].ToString();
                    ((TextBox)GrdEmbargoDetails.Rows[i].FindControl("txtvalues")).Text = dsMaterialDetailsTemp.Rows[i]["Values"].ToString();
                    ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable")).Items.Add(dsMaterialDetailsTemp.Rows[i]["Applicable"].ToString());
                }

                //SaveMaterialGrid();
            }
            catch (Exception ex)
            { }
            #endregion commentedcode

        }
        #endregion DeleteRow

        public void MasterLog(string value)
        {
            MasterAuditBAL ObjMAL = new MasterAuditBAL();
            #region for Master Audit Log
            #region Prepare Parameters
            object[] Paramsmaster = new object[7];
            int count = 0;

            //1

            Paramsmaster.SetValue("Maintain Embargo", count);
            count++;

            //2

            string data = null;

            for (int j = 0; j < GrdEmbargoDetails.Rows.Count; j++)
            {
                if (((DropDownList)(GrdEmbargoDetails.Rows[j].FindControl("ddlParameter"))).SelectedIndex != 0)

                    data = (((TextBox)(GrdEmbargoDetails.Rows[j].FindControl("txtvalues"))).Text.ToString());

                Paramsmaster.SetValue(data, count);
            }
            count++;

            //3

            Paramsmaster.SetValue(value, count);
            count++;

            //4

            Paramsmaster.SetValue("", count);
            count++;


            //5

            Paramsmaster.SetValue("", count);
            count++;

            //6

            Paramsmaster.SetValue(Session["UserName"], count);
            count++;

            //7
            Paramsmaster.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"),count);
            count++;


            #endregion Prepare Parameters
            ObjMAL.AddMasterAuditLog(Paramsmaster);
            #endregion

        }

        #region OriginType
        protected void ddlorigintype_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListDataViewBAL objDataViewBAL = new ListDataViewBAL();
            DataSet dsResult = new DataSet();
            string errormessage = "";

            if (ddlorigintype.SelectedIndex == 0)
            {
                ddlOrigin.Items.Clear();
                return;
            }
            string level = ddlorigintype.SelectedItem.Value;
            if (objDataViewBAL.FillDdl("Origin", ref dsResult, ref errormessage, new string[] { level }))
            {
                ddlOrigin.DataSource = dsResult;
                ddlOrigin.DataMember = dsResult.Tables[0].TableName;
                ddlOrigin.DataValueField = dsResult.Tables[0].Columns["Value"].ColumnName;
                ddlOrigin.DataTextField = dsResult.Tables[0].Columns["Name"].ColumnName;
                ddlOrigin.DataBind();
                ddlOrigin.Items.Insert(0, new ListItem("All", "All"));
                ddlOrigin.SelectedIndex = 0;//-1;
                //ddlOrigin.Items.Insert(0, new ListItem("Select", string.Empty));
            }

        }
        #endregion OriginType
        #region DestinationType
        protected void ddldestinationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListDataViewBAL objDataViewBAL = new ListDataViewBAL();
                DataSet dsResult = new DataSet();
                string errormessage = "";

                if (ddldestinationType.SelectedIndex == 0)
                {
                    ddlDestination.Items.Clear();
                    return;
                }
                string level = ddldestinationType.SelectedItem.Value;
                if (objDataViewBAL.FillDdl("Origin", ref dsResult, ref errormessage, new string[] { level }))
                {
                    ddlDestination.DataSource = dsResult;
                    ddlDestination.DataMember = dsResult.Tables[0].TableName;
                    ddlDestination.DataValueField = dsResult.Tables[0].Columns["Value"].ColumnName;
                    ddlDestination.DataTextField = dsResult.Tables[0].Columns["Name"].ColumnName;
                    ddlDestination.DataBind();
                    //ddlDestination.Items.Insert(0, new ListItem("Select", string.Empty));
                    ddlDestination.Items.Insert(0, new ListItem("All", "All"));
                    ddlDestination.SelectedIndex = 0;// -1;
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion DestinationType
        #region DisplayDetails
        protected void btnDisplayDetails_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtReferenceNo.Text == "" && txtReferenceNo.Text == null)
                {
                    lblStatus.Text = "Please Enter Valid Reference Number";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                int refno;

                refno = Convert.ToInt32(txtReferenceNo.Text);

                blnResult = Embal.GetEmbargoDetails(refno);
                if (blnResult.Tables[0].Rows.Count > 0)
                {
                    //if (!blnResult.Tables[0].Columns.Contains("txtAmount"))
                    //    blnResult.Tables[0].Columns.Add("txtAmount");
                    lblStatus.Text = string.Empty;
                    GrdEmbargoDetails.DataSource = blnResult.Tables[0].Copy();
                    GrdEmbargoDetails.DataBind();

                    for (int i = 0; i < blnResult.Tables[0].Rows.Count; i++)
                    {
                        //ddlLevel.Items.Clear();
                        btnSave.Text = "Update";
                        //ddlLevel.SelectedItem.Text = blnResult.Tables[0].Rows[i]["RefLevel"].ToString();
                        //ddlStatus.SelectedItem.Text = blnResult.Tables[0].Rows[i]["Status"].ToString();
                        ddlLevel.SelectedIndex = ddlLevel.Items.IndexOf(((ListItem)ddlLevel.Items.FindByText(blnResult.Tables[0].Rows[i]["RefLevel"].ToString())));
                        ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(((ListItem)ddlStatus.Items.FindByText(blnResult.Tables[0].Rows[i]["Status"].ToString())));
                        DateTime dt = Convert.ToDateTime(blnResult.Tables[0].Rows[i]["StartDate"].ToString());
                        txtstartdate.Text = dt.ToString("dd/MM/yyyy");
                        DateTime dt1 = Convert.ToDateTime(blnResult.Tables[0].Rows[i]["EndDate"].ToString());
                        txtenddate.Text = dt1.ToString("dd/MM/yyyy");

                        //ddlorigintype.Items.Add(blnResult.Tables[0].Rows[i]["OriginType"].ToString());
                        //ddlOrigin.Items.Add(blnResult.Tables[0].Rows[i]["Origin"].ToString());
                        //ddldestinationType.SelectedItem.Text =blnResult.Tables[0].Rows[i]["DestinationType"].ToString();
                        //ddlDestination.SelectedItem.Text =(blnResult.Tables[0].Rows[i]["Destination"].ToString());
                        ddlorigintype.SelectedIndex = ddlorigintype.Items.IndexOf(((ListItem)ddlorigintype.Items.FindByText(blnResult.Tables[0].Rows[i]["OriginType"].ToString())));
                        ddldestinationType.SelectedIndex = ddldestinationType.Items.IndexOf(((ListItem)ddldestinationType.Items.FindByText(blnResult.Tables[0].Rows[i]["DestinationType"].ToString())));

                        ddldestinationType_SelectedIndexChanged(null, null);
                        ddlorigintype_SelectedIndexChanged(null, null);

                        ddlOrigin.SelectedIndex = ddlOrigin.Items.IndexOf(((ListItem)ddlOrigin.Items.FindByValue(blnResult.Tables[0].Rows[i]["Origin"].ToString())));
                        ddlDestination.SelectedIndex = ddlDestination.Items.IndexOf(((ListItem)ddlDestination.Items.FindByValue(blnResult.Tables[0].Rows[i]["Destination"].ToString())));

                        txtDescription.Text = blnResult.Tables[0].Rows[i]["Discription"].ToString();
                        txtremarks.Text = blnResult.Tables[0].Rows[i]["Remarks"].ToString();
                        if (blnResult.Tables[0].Rows[i]["Suspend"].ToString() == "true")
                        {
                            ChkSuspend.Checked = true;
                        }
                        else
                        {
                            ChkSuspend.Checked = false;
                        }
                        (((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlParameter")).Text) = blnResult.Tables[0].Rows[i]["Parameter"].ToString();
                        ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable")).Items.Add(blnResult.Tables[0].Rows[i]["Applicable"].ToString());
                        (((TextBox)GrdEmbargoDetails.Rows[i].FindControl("txtvalues")).Text) = blnResult.Tables[0].Rows[i]["Value"].ToString();
                        if (blnResult.Tables[0].Rows[0]["DaysOfWeek"].ToString() != null)
                        {
                            string getDaysofWeek = blnResult.Tables[0].Rows[0]["DaysOfWeek"].ToString();
                            for (int k = 0; k < getDaysofWeek.Length; k++)
                            {
                                if (getDaysofWeek[k] == '1')
                                {
                                    cblWeekdays.Items[k].Selected = true;
                                }
                                else if (getDaysofWeek[k] == '0')
                                {
                                    cblWeekdays.Items[k].Selected = false;
                                }
                                else
                                {
                                    cblWeekdays.Items.Clear();
                                }
                            }
                        }
                    }

                }
                else
                {
                    lblStatus.Text = "No Reference Number Found Please Check Again...";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion DisplayDetails

        protected void ddlOrigin_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Added-24th Sept
        //Region Embargo list in grid..-Unit Testing Done

        #region Embargo list in grid..
        #region Embergo List
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";

                #region Prepare Parameters

                object[] Params = new object[9];
                int i = 0;

                //1
                string acccode = null;
                if (txtReferenceNo.Text == "")
                    acccode = "";
                else
                    acccode = txtReferenceNo.Text;
                Params.SetValue(acccode, i);
                i++;

                //2
                string orgtype = null;
                if (ddlorigintype.SelectedIndex != 0)
                    orgtype = ddlorigintype.SelectedItem.Text;
                else orgtype = "";
                Params.SetValue(orgtype, i);
                i++;

                //3
                string org = null;
                if (ddlOrigin.SelectedIndex > 0)
                {
                    if (ddlOrigin.SelectedIndex != 0)
                        org = ddlOrigin.SelectedItem.Text;
                    else org = "";
                }
                else if (ddlOrigin.SelectedIndex <= 0)
                    org = "";

                Params.SetValue(org, i);
                i++;

                //4
                string desttype = null;
                if (ddldestinationType.SelectedIndex != 0)
                    desttype = ddldestinationType.SelectedItem.Text;
                else desttype = "";
                Params.SetValue(desttype, i);
                i++;

                //5
                string dest = null;
                if (ddlDestination.SelectedIndex > 0)
                {

                    if (ddlDestination.SelectedIndex != 0)
                        dest = ddlDestination.SelectedItem.Text;
                    else dest = "";
                }
                else if (ddlDestination.SelectedIndex <= 0)
                    dest = "";
                Params.SetValue(dest, i);
                i++;

                //6
                string level = null;
                if (ddlLevel.SelectedIndex != 0)
                    level = ddlLevel.SelectedItem.Text;
                else level = "";
                Params.SetValue(level, i);
                i++;

                //7
                if (txtstartdate.Text != "")
                {
                    DateTime frmdt = DateTime.ParseExact(txtstartdate.Text, "dd/MM/yyyy", null);
                    Params.SetValue(frmdt, i);
                    i++;
                }
                else
                {
                    Params.SetValue("", i);
                    i++;
                }

                //8
                if (txtenddate.Text != "")
                {
                    DateTime enddt = DateTime.ParseExact(txtenddate.Text, "dd/MM/yyyy", null);
                    Params.SetValue(enddt, i);
                    i++;
                }
                else
                {
                    Params.SetValue("", i);
                    i++;
                }

                //9
                string status = null;
                if (ddlStatus.SelectedIndex != 0)
                    status = ddlStatus.SelectedItem.Text;
                else status = "";
                Params.SetValue(status, i);
                i++;


                #endregion Prepare Parameters

                DataSet ds = new DataSet();
                ds = Embal.GetEmbergoList(Params);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                GrdListEmbergo.PageIndex = 0;
                                GrdListEmbergo.DataSource = ds;
                                GrdListEmbergo.DataMember = ds.Tables[0].TableName;
                                GrdListEmbergo.DataBind();
                                GrdListEmbergo.Visible = true;
                                ViewState["ds"] = ds;
                                btnClear_Click(null, null);
                                //ds.Clear();

                            }
                            else if (ds.Tables[0].Rows.Count == 0)
                            {
                                //Page_Load(null, null);
                                btnList_Click(null, null);
                                lblStatus.Text = "No Records Exists";
                                lblStatus.ForeColor = Color.Red;

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

        }
        #endregion Embergo List

        # region GrdListEmbergo_RowCommand
        protected void GrdListEmbergo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //    try
            //    {
            //        #region Edit
            //        if (e.CommandName == "Edit")
            //        {

            //            lblStatus.Text = "";
            //            int RowIndex = Convert.ToInt32(e.CommandArgument);

            //            Session["SrNum"] = ((Label)(GrdListEmbergo.Rows[RowIndex].FindControl("lblSrNo"))).Text.ToString();
            //            Label lblRefNo = (Label)GrdListEmbergo.Rows[RowIndex].FindControl("lblRefNo");
            //            Label lblstatus = (Label)GrdListEmbergo.Rows[RowIndex].FindControl("lblStatus");
            //            Label lblStDt = (Label)GrdListEmbergo.Rows[RowIndex].FindControl("lblStartDt");
            //            Label lblEndDt = (Label)GrdListEmbergo.Rows[RowIndex].FindControl("lblEndDt");
            //            Label lblorgtype = (Label)GrdListEmbergo.Rows[RowIndex].FindControl("lblOrgType");
            //            Label lblOrg = (Label)GrdListEmbergo.Rows[RowIndex].FindControl("lblOrg");
            //            Label lblDestType = (Label)GrdListEmbergo.Rows[RowIndex].FindControl("lblDestType");
            //            Label lblDest = (Label)GrdListEmbergo.Rows[RowIndex].FindControl("lblDest");
            //            Label lblDscrp = (Label)GrdListEmbergo.Rows[RowIndex].FindControl("lblDescrp");
            //            Label lblrem = (Label)GrdListEmbergo.Rows[RowIndex].FindControl("lblRemark");
            //            Label lblsusp = (Label)GrdListEmbergo.Rows[RowIndex].FindControl("lblSusp");
            //            Label lblparam = (Label)GrdListEmbergo.Rows[RowIndex].FindControl("lblParam");
            //            Label lblapplic = (Label)GrdListEmbergo.Rows[RowIndex].FindControl("lblApplic");
            //            Label lblval = (Label)GrdListEmbergo.Rows[RowIndex].FindControl("lblValue");
            //            Label days = (Label)GrdListEmbergo.Rows[RowIndex].FindControl("lblDays");

            //            txtReferenceNo.Text = lblRefNo.Text;
            //            txtstartdate.Text = lblStDt.Text;      txtenddate.Text = lblEndDt.Text;


            //            btnSave.Text = "Update";

            //            //# region Agent Details
            //            //try
            //            //{
            //            //    #region Prepare Parameters
            //            //    DataSet ds = new DataSet();
            //            //    object[] Params = new object[1];
            //            //    int i = 0;

            //            //    1
            //            //    Params.SetValue(TXTAgentCode.Text, i);
            //            //    i++;

            //            //    #endregion Prepare Parameters

            //            //    int ID = 0;
            //            //    ds = objBal.GetAgentDetails(Params);
            //            //    if (ds != null)
            //            //    {
            //            //        if (ds.Tables != null)

            //            //            if (ds.Tables.Count > 0)
            //            //            {
            //            //                if (ds.Tables[0].Rows.Count > 0)
            //            //                {
            //            //                    txtAgentName.Text = ds.Tables[0].Rows[0][0].ToString();
            //            //                    TXTCustomerCode.Text = ds.Tables[0].Rows[0][1].ToString();
            //            //                    GrdListEmbergo.PageIndex = 0;
            //            //                    GrdListEmbergo.DataSource = ds;
            //            //                    GrdListEmbergo.DataMember = ds.Tables[0].TableName;
            //            //                    GrdListEmbergo.DataBind();
            //            //                    GrdListEmbergo.Visible = true;
            //            //                    ds.Clear();

            //            //                }
            //            //            }
            //            //    }
            //            //}

            //            //catch (Exception ex)
            //            //{

            //            //}

            //            //# endregion Agent Details
            //        }
            //        #endregion Edit
            //        //#region Delete

            //        //if (e.CommandName == "DeleteRecord")
            //        //{
            //        //    int RowIndex = Convert.ToInt32(e.CommandArgument);
            //        //    int srno = int.Parse(((Label)(GrdListEmbergo.Rows[RowIndex].FindControl("lblSrNo"))).Text.ToString());
            //        //    # region Delete
            //        //    try
            //        //    {
            //        //        #region Prepare Parameters
            //        //        DataSet ds = new DataSet();
            //        //        object[] Params = new object[1];
            //        //        int i = 0;

            //        //        //1
            //        //        Params.SetValue(srno, i);
            //        //        i++;

            //        //        #endregion Prepare Parameters

            //        //        int ID = 0;
            //        //        int res = objBal.DeleteShipperDetail(Params);
            //        //        if (res == 0)
            //        //        {
            //        //            btnClear_Click(null, null);
            //        //            btnList_Click(null, null);
            //        //            lblStatus.Text = "Record Deleted Successfully";
            //        //            lblStatus.ForeColor = Color.Red;

            //        //        }

            //        //    }

            //        //    catch (Exception ex)
            //        //    {

            //        //    }
            //        //}
            //        //    # endregion Delete

            //        //#endregion Delete
            //    }
            //    catch (Exception ex)
            //    {
            //        lblStatus.Text = ex.Message;
            //    }

        }
        # endregion GrdListEmbergo_RowCommand

        # region GrdListEmbergo_PageIndexChanging
        protected void GrdListEmbergo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet ds = new DataSet();
            ds = (DataSet)ViewState["ds"];
            GrdListEmbergo.PageIndex = e.NewPageIndex;
            GrdListEmbergo.DataSource = ds;
            GrdListEmbergo.DataBind();
        }
        # endregion GrdListEmbergo_PageIndexChanging

        protected void btnDeleteEmbargo_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";

                #region Prepare Parameters


                string acccode = null;
                if (txtReferenceNo.Text == "")
                {
                    lblStatus.Text = "Enter Embargo Reference Number to delete";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else
                    acccode = txtReferenceNo.Text;

                #endregion Prepare Parameters

                DataSet ds = new DataSet();
                ds = Embal.DeleteEmbago(acccode);

                if (ds.Tables[0].Rows[0][0].ToString() == "D")
                {
                    lblStatus.Text = "Embargo deleted successfully";
                    lblStatus.ForeColor = Color.Green;
                    btnClear_Click(null, null);


                }
                else
                {
                    lblStatus.Text = "No Record Found.";
                    lblStatus.ForeColor = Color.Red;

                }
            }
            catch (Exception ex)
            {
            }

        }
        #endregion Embargo list in grid..
    }
}
