using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BAL;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ProjectSmartCargoManager
{
    public partial class EditActiveFlights : System.Web.UI.Page
    {
        AirlineScheduleBAL OBJasb = new AirlineScheduleBAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            int SceduleID=0;
            if (Session["ScheduleID"] != null)
            {
                SceduleID=(int)Session["ScheduleID"];//Int16.Parse(dt.Rows[ind]["ScheduleID"].ToString())
                fillFlightDetails(SceduleID);

            }
        }

        protected void fillFlightDetails(int id)
        {
            try
            {
                DataSet dsdetails = (DataSet)Session["dsDetails"];
                DataTable dt = (DataTable)ViewState["FlightTable"];
                DataSet dsRoute = OBJasb.GetAirlineScheduleUsingRouteID(id);

                if (dsRoute.Tables[0].Rows.Count > 0)
                {

                    //  pnlSchedule.Enabled = false;
                    ViewState["CurrentTable"] = dsRoute.Tables[0];
                    DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                    grdScheduleinfo.DataSource = dtCurrentTable;
                    grdScheduleinfo.DataBind();
                    DataRow drCurrentRow = null;
                    LoadSourceInGridview();
                    // DestinationList(); 
                    LoadDestinationInGridview();
                    AirCraftTypeinGridview();
                    AirCraftTypeInEdit();
                    if (dtCurrentTable.Rows.Count > 0)
                    {
                        //extract the TextBox values
                        pnlSchedule.Visible = true;

                        pnlDestDetails.Visible = true;
                        pnlDestDetails.Enabled = false;
                        txtFromdate.Text = Convert.ToDateTime(dtCurrentTable.Rows[0][8]).ToString("dd/MM/yyyy");
                        txtToDate.Text = Convert.ToDateTime(dtCurrentTable.Rows[0][9]).ToString("dd/MM/yyyy");
                        try
                        {
                            // CheckFromDate = DateTime.ParseExact(dtCurrentTable.Rows[0][8].ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
//                            CheckFromDate = dtCurrentTable.Rows[0][8].ToString();

                            //DateTime.Parse(Convert.ToDateTime(dtCurrentTable.Rows[0][8]).ToString("dd-MM-yyyy"));
                            // CheckToDate = DateTime.ParseExact(dtCurrentTable.Rows[0][9].ToString(),"dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
  //                          CheckToDate = dtCurrentTable.Rows[0][9].ToString();
                            //DateTime.Parse(Convert.ToDateTime(dtCurrentTable.Rows[0][9]).ToString("dd-MM-yyyy"));
                        }
                        catch (Exception ex)
                        {
                        }

                        txtCargoCapacity.Text = dtCurrentTable.Rows[0][12].ToString();
                        string strAircraft = "";
                        strAircraft = dtCurrentTable.Rows[0][13].ToString();
                        try
                        {
                            if (strAircraft != "")
                            {
                                ddlLoadAirCraftType.Text = dtCurrentTable.Rows[0][13].ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        //    txtSource.Text = dtCurrentTable.Rows[0][14].ToString();
                        ddlOrigin1.Text = dtCurrentTable.Rows[0][14].ToString();
                        // txtDestination.Text = dtCurrentTable.Rows[0][15].ToString();
                        ddlDestination0.Text = dtCurrentTable.Rows[0][15].ToString();
                        if (dtCurrentTable.Rows[0][16].ToString() == "True")
                        {
                            chkDomestic0.Checked = true;
                            chkInternational0.Checked = false;
                        }
                        else
                        {
                            chkDomestic0.Checked = false;
                            chkInternational0.Checked = true;
                        }

                        // ddlDestination.SelectedValue = dtCurrentTable.Rows[0][8].ToString();

                        for (int i = 0; i <= dtCurrentTable.Rows.Count; i++)
                        {
                            //extract the TextBox values

                            drCurrentRow = dtCurrentTable.NewRow();
                            // drCurrentRow["RowNumber"] = i + 1;




                            DropDownList ddlFromOrigin = new DropDownList();

                            ddlFromOrigin = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlFromOrigin")));

                            ddlFromOrigin.SelectedValue = dtCurrentTable.Rows[i][1].ToString();

                            DropDownList ddlDest = new DropDownList();

                            ddlDest = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlToDest")));

                            ddlDest.SelectedValue = dtCurrentTable.Rows[i][2].ToString();


                            string FlightID = dtCurrentTable.Rows[i][0].ToString();
                            //     string flightandsource = FlightID + dtCurrentTable.Rows[i][1].ToString();
                            //if (!arFlight.Contains(FlightID))//FlightID))//&& (!arOrigin.Contains(dtCurrentTable.Rows[i][1].ToString())))
                            //{
                            //    //if ((!arOrigin.Contains(dtCurrentTable.Rows[i][1].ToString())))
                            //    //{

                            //    Label lblFlight = ((Label)(grdScheduleinfo.Rows[i].FindControl("lblFlight")));
                            //    lblFlight.Text = FlightID;
                            //    arFlight.Add(FlightID);
                            //    //  arFlight.Add(flightandsource);
                            //    // arOrigin.Add(dtCurrentTable.Rows[i][1].ToString());
                            //    //}
                            //}
                            //else
                            //{
                            //    arOrigin.Add(dtCurrentTable.Rows[i][1].ToString());
                            //}

                            ((TextBox)grdScheduleinfo.Rows[i].Cells[3].FindControl("txtDeptDay")).Text = dtCurrentTable.Rows[i][3].ToString();

                            string[] HrDept = dtCurrentTable.Rows[i][4].ToString().Split(':');

                            ((TextBox)grdScheduleinfo.Rows[i].Cells[2].FindControl("txtDeptTimeHr")).Text = HrDept[0];
                            ((TextBox)grdScheduleinfo.Rows[i].Cells[2].FindControl("txtDeptTimeMin")).Text = HrDept[1];

                            string[] HrArr = dtCurrentTable.Rows[i][6].ToString().Split(':');

                            ((TextBox)grdScheduleinfo.Rows[i].Cells[4].FindControl("txtArrivalDay")).Text = dtCurrentTable.Rows[i][5].ToString();

                            ((TextBox)grdScheduleinfo.Rows[i].Cells[1].FindControl("txtArrivaltimeHr")).Text = HrArr[0];
                            ((TextBox)grdScheduleinfo.Rows[i].Cells[1].FindControl("txtArrivaltimeMin")).Text = HrArr[1];

                            
                            try
                            {
                                ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlAirCraft"))).SelectedValue = dtCurrentTable.Rows[i][18].ToString();
                            }
                            catch (Exception ex)
                            {
                            }
                            ((TextBox)grdScheduleinfo.Rows[i].FindControl("txtCapacity")).Text = dtCurrentTable.Rows[i][17].ToString();

                            DropDownList ddlIsActive = new DropDownList();

                            ddlIsActive = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlIsActive")));

                            string Isactive = dtCurrentTable.Rows[i][11].ToString();
                            //if(Isactive=="True")
                            //{
                            ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlStatus"))).SelectedValue = Isactive;
                            //ddlIsActive.SelectedValue ="IsActive";
                            //}
                            //else 
                            // {
                            //     ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlIsActive"))).SelectedValue = "InActive"; 
                            //   //ddlIsActive.SelectedValue = "InActive";
                            // }

                        }


                        ViewState["CurrentTable"] = dtCurrentTable;

                        grdScheduleinfo.DataSource = dtCurrentTable;
                        grdScheduleinfo.DataBind();







                    }
                }
            }
            catch (Exception ex)
            {

            }


        }

        #region AirCraftType In Edit type List

        private void AirCraftTypeInEdit()
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
                            //Aircraft type in Gridview Dropdown

                            DataRow row = ds.Tables[0].NewRow();

                            row[ds.Tables[0].Columns[0].ColumnName] = "Select";
                            ds.Tables[0].Rows.Add(row);

                            ddlLoadAirCraftType.DataSource = ds;
                            ddlLoadAirCraftType.DataMember = ds.Tables[0].TableName;
                            ddlLoadAirCraftType.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            ddlLoadAirCraftType.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                            ddlLoadAirCraftType.DataBind();
                            ddlLoadAirCraftType.Text = "Select";
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion AirCraft List


        #region Origin List for Details
        private void OriginListforDetails()
        {
            try
            {
                DataSet ds = OBJasb.GetOriginList(ddlOrigin1.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            //Dest dropdown
                            // DataRow row = ds.Tables[0].NewRow();

                            //row[ds.Tables[0].Columns[0].ColumnName] = "All";
                            // ds.Tables[0].Rows.Add(row);

                            ddlOrigin1.DataSource = ds;
                            ddlOrigin1.DataMember = ds.Tables[0].TableName;
                            ddlOrigin1.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            ddlOrigin1.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                            ddlOrigin1.DataBind();
                            // ddlOrigin.Text = "All";

                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion Origin List

        #region AirCraft type List in gridview

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


        #region LoadDest in Gridview Dropdown
        public void LoadDestinationInGridview()
        {
            try
            {
                DataSet ds = OBJasb.GetDestinationList("");
                DropDownList ddl = new DropDownList();

                for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
                {
                    ddl = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlToDest")));
                    if (ds != null)
                    {
                        //LoadDest in Gridview Dropdown 
                        DataRow row = ds.Tables[0].NewRow();

                        row[ds.Tables[0].Columns[0].ColumnName] = "Select";
                        ds.Tables[0].Rows.Add(row);

                        ddl.DataSource = ds;
                        ddl.DataMember = ds.Tables[0].TableName;
                        ddl.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                        ddl.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                        ddl.DataBind();
                        ddl.Text = "Select";
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        #endregion LoadDest Dropdown

        #region LoadSource in Gridview Dropdown
        public void LoadSourceInGridview()
        {
            try
            {
                DataSet ds = OBJasb.GetOriginList("");
                DropDownList ddl = new DropDownList();

                for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
                {
                    ddl = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlFromOrigin")));
                    if (ds != null)
                    {
                        //Source in Gridview Dropdown
                        DataRow row = ds.Tables[0].NewRow();

                        row[ds.Tables[0].Columns[0].ColumnName] = "Select";
                        ds.Tables[0].Rows.Add(row);

                        ddl.DataSource = ds;
                        ddl.DataMember = ds.Tables[0].TableName;
                        ddl.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                        ddl.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                        ddl.DataBind();
                        ddl.Text = "Select";
                    }
                }
            }
            catch (Exception ex)
            {
            }

        }

        #endregion LoadSource Dropdown


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
                                //txtCapacity
                                ((TextBox)grdScheduleinfo.Rows[i].FindControl("txtCapacity")).Text = ds.Tables[0].Rows[0][0].ToString();
                            }
                        }
                    }
                }

                catch (Exception)
                {
                }


            }

        }



        protected void ddlOrigin_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlDestination2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlLoadAirCraftType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void txtFromdate_TextChanged(object sender, EventArgs e)
        {

        }

        protected void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void chkInternational0_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void txtSource1_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

        }

        protected void btnAddNewRow_Click(object sender, EventArgs e)
        {

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }

        protected void grdScheduleinfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void grdScheduleinfo_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {

        }





    }
}
