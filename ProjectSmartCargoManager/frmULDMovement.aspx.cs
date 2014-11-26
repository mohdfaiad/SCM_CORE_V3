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
    public partial class frmULDMovement : System.Web.UI.Page
    {

        #region Variables
        SQLServer db = new SQLServer(Global.GetConnectionString());
        clsFillCombo clsCombo = new clsFillCombo();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
                try
                {
                    clsCombo.FillAllComboBoxes("tblWarehouseMaster", "SELECT", ddlDestination);
                    clsCombo.FillAllComboBoxes("tblWarehouseMaster", "SELECT", ddlOrigin);
                    clsCombo.FillAllComboBoxes("tblWarehouseMaster", "SELECT", drpMoveOriginNew);
                    clsCombo.FillAllComboBoxes("tblWarehouseMaster", "SELECT", drpMoveDestNew);
                    clsCombo.FillAllComboBoxes("tblULDStatusMaster", "SELECT", drpMoveStatusNew);

                    Session["WareHouses"] = clsCombo.ReturnDataset("tblWarehouseMaster", "SELECT").Copy();
                    Session["ULDStatuses"] = clsCombo.ReturnDataset("tblULDStatusMaster", "SELECT").Copy();
                    
                    //txtMovedOnNew.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                    txtMovedOnNew.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                    txtMoveTimeNew.Text = Convert.ToDateTime(Session["IT"]).ToString("HH:mm");
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Error in Loading Page: " + ex.Message;
                }
                finally
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>enaCon()</script>", false);
                }
                DataTable dt = new DataTable("ULDMOV_DT1");
                dt.Columns.Add("ULDNumber");
                dt.Columns.Add("Origin");
                dt.Columns.Add("Destination");
                dt.Columns.Add("ULDStatus");
                dt.Columns.Add("FlightID");
                dt.Columns.Add("MovedOn");
                dt.Columns.Add("MovedOnTime");
                dt.Columns.Add("MovementType");
                dt.Columns.Add("UpdatedBy");

                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
                gvAddressGrpFTP.DataSource = dt;
                gvAddressGrpFTP.DataBind();
                dt.Dispose();
            }
        }

        #region Search Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //bool isMultiple = false;
                lblStatus.Text = "";
                btnAdd.Visible = false;
                btnEdit.Visible = false;
                btnDelete.Visible = false;
                //if (ddlOrigin.SelectedItem.Text == "SELECT" && ddlDestination.SelectedItem.Text == "SELECT" && txtULDNo.Text.Trim() == "" && txtFltNo.Text.Trim() == "" && txtFltdate.Text.Trim() == "" && ddlMovementType.SelectedItem.Text.Trim() == "SELECT")
                if (ddlOrigin.SelectedItem.Text == "SELECT" && txtULDNo.Text.Trim() == "" && txtFltNo.Text.Trim() == "" && txtFltdate.Text.Trim() == "" && ddlMovementType.SelectedItem.Text.Trim() == "SELECT" && drpMoveStatusNew.SelectedItem.Text.Trim()=="SELECT")
                {
                    lblStatus.Text = "Atleast one parameter is required";
                    return;
                }
                
                string Origin = "", Dest = "", FlightNo = "", FlightDt = "", Type = "", ULDNo = "", Status = "";
                if (ddlOrigin.SelectedItem.Text.Trim() == "SELECT")  Origin = ""; else Origin = ddlOrigin.SelectedItem.Text.Trim();
                //if (ddlDestination.SelectedItem.Text.Trim() == "SELECT")  Dest = ""; else Dest = ddlDestination.SelectedItem.Text.Trim();
                if (ddlMovementType.SelectedItem.Text.Trim() == "SELECT")  Type = ""; else Type = ddlMovementType.SelectedItem.Text.Trim();
                if (txtFltNo.Text.Trim() == "") FlightNo = ""; else FlightNo = txtFltNo.Text.Trim();
                if (txtFltdate.Text.Trim() == "") FlightDt = ""; else FlightDt = txtFltdate.Text.Trim();
                if (txtULDNo.Text.Trim() == "") ULDNo = ""; else ULDNo = txtULDNo.Text.Trim();
                if (drpMoveStatusNew.SelectedItem.Text.Trim() == "SELECT") Status = ""; else Status = drpMoveStatusNew.SelectedItem.Text.Trim();
                pnlMove.Visible = false;

                DataSet ds = new DataSet("ULDMOV_DS1");
                    ds = GetMovementDetails(Origin,Dest,FlightNo,FlightDt, Type,ULDNo, Status);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            Session["grdULDMoveData"] = ds.Tables[0];
                            gvAddressGrpFTP.DataSource = ds;
                            gvAddressGrpFTP.DataBind();
                            pnlMove.Visible = true;
                            drpMoveDestNew.SelectedIndex = 0;
                            txtMoveFlightNew.Text = "";
                            txtMoveFlightDate.Text = "";
                            txtMovedOnNew.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                            txtMoveTimeNew.Text = Convert.ToDateTime(Session["IT"]).ToString("HH:mm");
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "No Records found";
                        }
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "No Records found";
                    }
                    ds.Dispose();
                }
                else
                {
                    DataTable dt = new DataTable("ULDMOV_DT2");
                    dt.Columns.Add("ULDNumber");
                    dt.Columns.Add("Origin");
                    dt.Columns.Add("Destination");
                    dt.Columns.Add("ULDStatus");
                    dt.Columns.Add("FlightID");
                    dt.Columns.Add("MovedOn");
                    dt.Columns.Add("MovedOnTime");
                    dt.Columns.Add("MovementType");
                    dt.Columns.Add("UpdatedBy");

                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    gvAddressGrpFTP.DataSource = dt;
                    gvAddressGrpFTP.DataBind();
                    Session["grdULDMoveData"] = dt;

                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "No Records found";

                    if (dt != null)
                    {
                        dt.Dispose();
                    }
                }
                
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error in search :" + ex.Message;
            }
            finally 
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>enaCon();vistxtall()</script>", false);
            }
        }
        #endregion

        public DataSet GetMovementDetails(string Origin,string Destination,string FlightNo,string FlightDt,string MoveType, string ULDNo, string ULDStatus)
        {
            DataSet ds = new DataSet("ULDMOV_DS2");
            string[] QueryNames = new string[7];
            object[] QueryValues = new object[7];
            SqlDbType[] QueryTypes = new SqlDbType[7];
            try
            {
                lblStatus.Text = "";
                Session["grdULDMoveData"] = null;

                gvAddressGrpFTP.DataSource = null;
                gvAddressGrpFTP.DataBind();

                QueryNames[0] = "Origin";
                QueryNames[1] = "Destination";
                QueryNames[2] = "FlightNo";
                QueryNames[3] = "FlightDt";
                QueryNames[4] = "MovementType";
                QueryNames[5] = "ULDno";
                QueryNames[6] = "ULDSTatus";

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.VarChar;
                QueryTypes[3] = SqlDbType.VarChar;
                QueryTypes[4] = SqlDbType.VarChar;
                QueryTypes[5] = SqlDbType.VarChar;
                QueryTypes[6] = SqlDbType.VarChar;

                QueryValues[0] = Origin;
                QueryValues[1] = Destination;
                QueryValues[2] = FlightNo;
                QueryValues[3] = FlightDt;
                QueryValues[4] = MoveType;
                QueryValues[5] = ULDNo;
                QueryValues[6] = ULDStatus;

                ds = db.SelectRecords("spGETULDMovementDetailsNew", QueryNames, QueryValues, QueryTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        return ds;
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
            finally
            {
                QueryNames = null;
                QueryValues = null;
                QueryTypes = null;
            }
            return null;
        }

        public bool SaveMovementDetails(string ULDNumber, Int16 Status, Int64 LocationID, string LocationType, 
            string MovedOn, string UpdatedBy,string flightid, string Origin, string Destination, string StatusCode, 
            string MovType,string FlightDate)
        {
            DataSet dsUpdateMovement = new DataSet("ULDMOV_DS3");
            string[] QueryNames = new string[12];
            object[] QueryValues = new object[12];
            SqlDbType[] QueryTypes = new SqlDbType[12];
            try
            {

                QueryNames[0] = "ULDNo";
                QueryNames[1] = "Status";
                QueryNames[2] = "LocationID";
                QueryNames[3] = "LocationType";
                QueryNames[4] = "MovedOn";
                QueryNames[5] = "UpdatedBy";
                QueryNames[6] = "FlightID";
                QueryNames[7] = "Origin";
                QueryNames[8] = "Destination";
                QueryNames[9] = "StatusCode";
                QueryNames[10] = "MovType";
                QueryNames[11] = "FlightDate";

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.TinyInt;
                QueryTypes[2] = SqlDbType.BigInt;
                QueryTypes[3] = SqlDbType.VarChar;
                QueryTypes[4] = SqlDbType.DateTime;
                QueryTypes[5] = SqlDbType.VarChar;
                QueryTypes[6] = SqlDbType.VarChar;
                QueryTypes[7] = SqlDbType.VarChar;
                QueryTypes[8] = SqlDbType.VarChar;
                QueryTypes[9] = SqlDbType.VarChar;
                QueryTypes[10] = SqlDbType.VarChar;
                QueryTypes[11] = SqlDbType.VarChar;

                QueryValues[0] = ULDNumber;
                QueryValues[1] = Status;
                QueryValues[2] = LocationID;
                QueryValues[3] = LocationType;
                QueryValues[4] = Convert.ToDateTime(MovedOn);
                QueryValues[5] = UpdatedBy;
                QueryValues[6] = flightid;
                QueryValues[7] = Origin;
                QueryValues[8] = Destination;
                QueryValues[9] = StatusCode;
                QueryValues[10] = MovType;
                QueryValues[11] = FlightDate;

                dsUpdateMovement = db.SelectRecords("spInsertULDMoveData", QueryNames, QueryValues, QueryTypes);
                if (dsUpdateMovement != null && dsUpdateMovement.Tables.Count > 0)
                {
                    if (dsUpdateMovement.Tables[0].Rows.Count > 0)
                    {
                        if (dsUpdateMovement.Tables[0].Rows[0][0].ToString() == "SUCCESS")
                        {
                            return true;
                        }
                        else if (dsUpdateMovement.Tables[0].Rows[0][0].ToString() == "ABSENT")
                        {
                            lblStatus.Text = "Unknown ULD..!!";
                            return false;
                        }
                        else if (dsUpdateMovement.Tables[0].Rows[0][0].ToString() == "FLIGHT_ABSENT")
                        {
                            lblStatus.Text = "Unknown FlightNo..!!";
                            return false;
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {
                lblStatus.Text = "Error in Saving ULDMovement";
                return false;
            }
            finally
            {
                if (dsUpdateMovement != null)
                {
                    dsUpdateMovement.Dispose();
                }
                QueryNames = null;
                QueryValues = null;
                QueryTypes = null;
            }
        }

        #region ReturnLocationType
        private string ReturnLocationType(Int64 WHID, Int64 SubWHID, Int64 AreaID)
        {
            if (AreaID == 0)
            {
                if (SubWHID == 0)
                {
                    if (WHID == 0)
                    {
                        return "DNE";
                    }
                    else
                    {
                        return "W";
                    }
                }
                else
                {
                    return "S";
                }
            }
            else
            {
                return "A";
            }
        }

        #endregion

        #region ReturnLocationID
        private Int64 ReturnLocationCode(Int64 WHID, Int64 SubWHID, Int64 AreaID)
        {
            if (AreaID == 0)
            {
                if (SubWHID == 0)
                {
                    if (WHID == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return WHID;
                    }
                }
                else
                {
                    return SubWHID;
                }
            }
            else
            {
                return AreaID;
            }
        }

        #endregion

        #region Button Save
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = new Button();
                btn = (Button)sender;
                lblStatus.Text = "";
                bool flag = false;
                Button clickedButton = (Button)sender;
                GridViewRow row = (GridViewRow)clickedButton.Parent.Parent;
                int rowIndex = row.RowIndex;
                string LocationID = "";

                if (btn.Text == "Move")
                {
                    if (ddlOrigin.SelectedIndex != 0)
                    {
                        LocationID = ddlOrigin.SelectedValue;
                    }

                    // Make Row controls Editable and visible
                    MakeRowVisible("tblULDNumber", rowIndex,true);
                    MakeRowVisible("tblULDStatus", rowIndex,true);
                    MakeRowVisible("tblOrigin", rowIndex,true);
                    MakeRowVisible("tblDestination", rowIndex,true);
                    MakeRowVisible("tblFlightID", rowIndex,true);
                    MakeRowVisible("tblMovedOn", rowIndex,true);
                    MakeRowVisible("tblMovementType", rowIndex,true);

                    // Fill All the Comboboxes in the Row
                    AllocateGridDropdown(rowIndex);

                    btn.Text = "Save";
                }
                else
                {
                    if (((TextBox)(gvAddressGrpFTP.Rows[rowIndex].FindControl("txtULDNumber"))).Text != "")
                    {
                        string dattme = Convert.ToDateTime(Session["IT"]).ToString();
                        string MovedDate = "";
                        string MovedTime = "";

                        Table tb = new Table();
                        TableRow tr = new TableRow();
                        tb = (Table)(gvAddressGrpFTP.Rows[rowIndex].FindControl("tblOrigin"));
                        tr = tb.Rows[1];
                        DropDownList ddlTemp = (DropDownList)tr.FindControl("ddlOrigin");
                        string Origin = ddlTemp.SelectedItem.Text;

                        tb = (Table)(gvAddressGrpFTP.Rows[rowIndex].FindControl("tblDestination"));
                        tr = tb.Rows[1];
                        ddlTemp = (DropDownList)tr.FindControl("ddlDestination");
                        string Destination = ddlTemp.SelectedItem.Text;
                        Int64 LocID = Int64.Parse(ddlTemp.SelectedValue);

                        tb = (Table)(gvAddressGrpFTP.Rows[rowIndex].FindControl("tblULDStatus"));
                        tr = tb.Rows[1];
                        ddlTemp = (DropDownList)tr.FindControl("ddlULDStatus");
                        string StatusCode = ddlTemp.SelectedItem.Text;
                        Int16 StatusID = (Int16.Parse(ddlTemp.SelectedValue));

                        tb = (Table)(gvAddressGrpFTP.Rows[rowIndex].FindControl("tblMovementType"));
                        tr = tb.Rows[1];
                        ddlTemp = (DropDownList)tr.FindControl("ddlMovementType");
                        string MovementType = ddlTemp.SelectedItem.Text;

                        tb = (Table)(gvAddressGrpFTP.Rows[rowIndex].FindControl("tblFlightID"));
                        tr = tb.Rows[1];
                        TextBox txtTemp = (TextBox)tr.FindControl("txtFlightID");
                        string FlightID = txtTemp.Text.Trim();

                        tb = (Table)(gvAddressGrpFTP.Rows[rowIndex].FindControl("tblULDNumber"));
                        tr = tb.Rows[1];
                        txtTemp = (TextBox)tr.FindControl("txtULDNumber");
                        string ULDNumber = txtTemp.Text.Trim();

                        string fltDate = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");

                        tb = (Table)(gvAddressGrpFTP.Rows[rowIndex].FindControl("tblMovedOn"));
                        tr = tb.Rows[1];
                        txtTemp = (TextBox)tr.FindControl("txtMovedOn");
                        MovedDate = txtTemp.Text.Trim();
                        txtTemp = (TextBox)tr.FindControl("txtMoveTime");
                        MovedTime = txtTemp.Text.Trim();
                        //fltDate = txtTemp.Text;

                        /*-----------------------------Sumit 16/09/2014--------------------------*/
                        try
                        {
                            if (MovedDate.Trim() == "")
                            {
                                MovedDate = DateTime.Now.ToString("dd/MM/yyyy");
                            }
                            if (MovedTime.Trim() == "")
                            {
                                MovedTime = DateTime.Now.ToString("HH:mm:ss");
                            }
                            string DD = MovedDate.Substring(0, 2);
                            string MM = MovedDate.Substring(3, 2);
                            string YY = MovedDate.Substring(6, 4);
                            DateTime dtMovedDate;
                            dtMovedDate = DateTime.Parse( YY + "-" + MM + "-" + DD + " " + MovedTime.Trim());
                            dattme = dtMovedDate.ToString("yyyy-MM-dd HH:mm:ss");
                            //dtMovedDate = DateTime.Parse(dattme);
                            fltDate = MovedDate;
                        }
                        catch (Exception ex)
                        {
                            lblStatus.Text = "Invalid Moved On Date Time . . .";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        /*-----------------------------Sumit 16/09/2014--------------------------*/

                        if(ddlTemp != null)
                            ddlTemp.Dispose();
                        if(txtTemp != null)
                            txtTemp.Dispose();
                        if (tb != null)
                            tb.Dispose();
                        if (tr != null)
                            tr.Dispose();

                        lblStatus.Text = "Error in Saving..";
                        flag = SaveMovementDetails(ULDNumber, StatusID, LocID, "A", dattme, Session["UserName"].ToString(), 
                            FlightID, Origin, Destination, StatusCode, MovementType,fltDate);

                        if (flag)
                        {
                            // Make Row controls Editable and visible
                            MakeRowVisible("tblULDNumber", rowIndex, false);
                            MakeRowVisible("tblULDStatus", rowIndex, false);
                            MakeRowVisible("tblOrigin", rowIndex, false);
                            MakeRowVisible("tblDestination", rowIndex, false);
                            MakeRowVisible("tblFlightID", rowIndex, false);
                            MakeRowVisible("tblMovedOn", rowIndex, false);
                            MakeRowVisible("tblMovementType", rowIndex, false);
                            btn.Text = "Move";
                            btnSearch_Click(null, null);
                            lblStatus.Text = "Successfully Saved the Movement..";
                            lblStatus.ForeColor = Color.Green;
                        }
                        else
                        {
                            //lblStatus.Text = "Error in Saving..";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                    }
                }
                if (btn != null)
                {
                    btn.Dispose();
                }
                if (clickedButton != null)
                {
                    clickedButton.Dispose();
                }
                if (row != null)
                {
                    row.Dispose();
                }
            }
            catch (Exception)
            {
            }
            finally 
            {
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>enaCon();vistxtall()</script>", false);
            }
        }
        #endregion

        #region Fill All Dropdowns Of the Grid
        private void FillGridDropdowns()
        {
            clsCombo = new clsFillCombo();
            for (int i = 0; i < gvAddressGrpFTP.Rows.Count; i++)
            {
                DropDownList ddlStatus = (DropDownList)gvAddressGrpFTP.Rows[i].FindControl("ddlStatus");
                DropDownList ddlMovedToStation = (DropDownList)gvAddressGrpFTP.Rows[i].FindControl("ddlMovedToStation");
                DropDownList ddlMovedToSubLoc = (DropDownList)gvAddressGrpFTP.Rows[i].FindControl("ddlMovedToSubLoc");
                DropDownList ddlMovedToArea = (DropDownList)gvAddressGrpFTP.Rows[i].FindControl("ddlMovedToArea");

                FillComboboxes(ddlStatus, "SELECT", (DataSet)Session["Statuses"]);

                FillComboboxes(ddlMovedToStation, "SELECT", (DataSet)Session["WareHouses"]);

                FillComboboxes(ddlMovedToSubLoc, "SELECT", (DataSet)Session["SubWarehouses"]);

                FillComboboxes(ddlMovedToArea, "SELECT", (DataSet)Session["Areas"]);
                
                if(ddlStatus != null)
                    ddlStatus.Dispose();
                if (ddlMovedToStation != null)
                    ddlMovedToStation.Dispose();
                if (ddlMovedToSubLoc != null)
                    ddlMovedToSubLoc.Dispose();
                if (ddlMovedToArea != null)
                    ddlMovedToArea.Dispose();
            }
        }
        #endregion

        #region Allocate DatasettoGrid For Dropdowns
        private bool AllocateGridDropdown(int rowIndex)
        {
            try
            {
                Table tb = new Table();
                TableRow tr = new TableRow();
                tb = (Table)(gvAddressGrpFTP.Rows[rowIndex].FindControl("tblOrigin"));
                tr = tb.Rows[1];
                DropDownList ddlTemp = (DropDownList)tr.FindControl("ddlOrigin");
                FillComboboxes(ddlTemp, "SELECT", (DataSet)Session["WareHouses"]);

                tb = (Table)(gvAddressGrpFTP.Rows[rowIndex].FindControl("tblDestination"));
                tr = tb.Rows[1];
                ddlTemp = (DropDownList)tr.FindControl("ddlDestination");
                FillComboboxes(ddlTemp, "SELECT", (DataSet)Session["WareHouses"]);

                tb = (Table)(gvAddressGrpFTP.Rows[rowIndex].FindControl("tblDestination"));
                tr = tb.Rows[1];
                ddlTemp = (DropDownList)tr.FindControl("ddlULDStatus");
                FillComboboxes(ddlTemp, "SELECT", (DataSet)Session["ULDStatuses"]);

                if (ddlTemp != null)
                {
                    ddlTemp.Dispose();
                }
                if (tb != null)
                {
                    tb.Dispose();
                }
                if (tr != null)
                {
                    tr.Dispose();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region For Maintaining Sessions For All Combobox Values and Setting it
        private void FillComboboxes(DropDownList drp, string SelectType, DataSet dsdrp)
        {
            try
            {
                if (dsdrp != null && dsdrp.Tables.Count > 0)
                {
                    if (dsdrp.Tables[0].Rows.Count > 0)
                    {
                        drp.Items.Clear();
                        drp.DataSource = dsdrp.Tables[0];
                        drp.DataTextField = "Code";
                        drp.DataValueField = "ID";
                        drp.DataBind();
                        drp.SelectedIndex = 0;
                    }
                    else
                    {
                        drp.Items.Clear();
                        drp.SelectedIndex = 0;
                    }
                    dsdrp.Dispose();
                }
                else
                {
                    drp.Items.Clear();
                    drp.Items.Add(SelectType);
                    drp.SelectedIndex = 0;
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region Make Row Visible
        private void MakeRowVisible(string ID, int rowIndex, bool set)
        {
            try
            {
                Table tb = new Table();
                TableRow tr = new TableRow();
                tb = (Table)(gvAddressGrpFTP.Rows[rowIndex].FindControl(ID));
                tr = tb.Rows[1];
                tr.Visible = set;
                if (tb != null)
                {
                    tb.Dispose();
                }
                if (tr != null)
                {
                    tr.Dispose();
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region btnMoveNew_Click
        protected void btnMoveNew_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvAddressGrpFTP.Rows.Count > 0)
                {
                    if (drpMoveDestNew.SelectedIndex <= 0)
                    {
                        lblStatus.Text = "Please Select Destination to Move";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    if ((txtMoveFlightNew.Text.Trim() != "") && (txtMoveFlightDate.Text.Trim() == ""))
                    {
                        lblStatus.Text = "Please Enter Flight Date To Move";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    if ((txtMoveFlightNew.Text.Trim() == "") && (txtMoveFlightDate.Text.Trim() != ""))
                    {
                        lblStatus.Text = "Please Enter Flight# To Move";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }

                    string UserLoc = Session["Station"].ToString();
                    string FltDate = "";
                    string MoveDate = txtMovedOnNew.Text.Trim();
                    string dattme = Convert.ToDateTime(Session["IT"]).ToString();
                    string MoveTime = txtMoveTimeNew.Text.Trim();

                    try
                    {
                        if (txtMoveFlightDate.Text.Trim() != "")
                        {
                            
                            DateTime dtMovedDate;
                            dtMovedDate = DateTime.Parse(txtMoveFlightDate.Text.Trim().Substring(0, 2) + "-" + txtMoveFlightDate.Text.Trim().Substring(3, 2) + "-" + txtMoveFlightDate.Text.Trim().Substring(6, 4) + " " + MoveTime.Trim());
                            FltDate = dtMovedDate.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            FltDate = "";
                        }
                    }
                    catch (Exception ex)
                    {
                        FltDate = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    
                    try
                    {
                        DateTime dtMovedDate;
                        dtMovedDate = DateTime.Parse(MoveDate.Substring(0, 2) + "-" + MoveDate.Substring(3, 2) + "-" + MoveDate.Substring(6, 4) + " " + MoveTime.Trim());
                        dattme = dtMovedDate.ToString("yyyy-MM-dd HH:mm:ss");

                    }
                    catch(Exception ex)
                    {
                        dattme = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    int count = 0;
                    bool flag = false;
                    for (int i = 0; i < gvAddressGrpFTP.Rows.Count; i++)
                    {
                        
                        if (((CheckBox)(gvAddressGrpFTP.Rows[i].FindControl("chkSelect"))).Checked == true)
                        {
                            string ULDNo = ((Label)(gvAddressGrpFTP.Rows[i].FindControl("lblULDNumber"))).Text.Trim();
                            if (ULDNo.Trim() != "")
                            {
                                count++;
                                Int16 status = 0;
                                string origin = ((Label)(gvAddressGrpFTP.Rows[i].FindControl("lblOrigin"))).Text.Trim();
                                if (origin.Trim() == "")
                                {
                                    if (ddlOrigin.SelectedIndex > 0)
                                    {
                                        origin = ddlOrigin.SelectedItem.Text.Trim();
                                    }
                                    else
                                    {
                                        origin = Session["Station"].ToString();
                                    }
                                }
                                string dest = drpMoveDestNew.SelectedItem.Text;
                                Int64 LocID = Int64.Parse(drpMoveDestNew.SelectedValue);
                                string MoveType = "OUT";
                                string FlightNo = txtMoveFlightNew.Text.Trim();

                                string statuscode = ((Label)(gvAddressGrpFTP.Rows[i].FindControl("lblULDStatus"))).Text.Trim();

                                if (statuscode.Trim() == "")
                                {
                                    statuscode = "SERVICEABLE";
                                }

                                if (UserLoc.Trim().ToUpper() == origin.Trim().ToUpper())
                                {
                                    MoveType = "OUT";
                                }
                                else if (UserLoc.Trim().ToUpper() == dest.Trim().ToUpper())
                                {
                                    MoveType = "IN";
                                }

                                flag = SaveMovementDetails(ULDNo, status, LocID, "A", dattme, Session["UserName"].ToString(), FlightNo, origin, dest, statuscode, MoveType, FltDate);
                            }
                        }
                    }
                    if (count > 0)
                    {
                        if (flag == true)
                        {
                            btnSearch_Click(sender, e);
                            lblStatus.Text = "ULD moved successfully . . .";
                            lblStatus.ForeColor = Color.Green;
                            return;
                        }
                        else
                        {
                            lblStatus.Text = "ULD move Failed . . .";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                    }
                    else
                    {
                        lblStatus.Text = "Please select Records";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                }
                else
                {
                    lblStatus.Text = "No records to proceed";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
            }
            catch (Exception)
            { }
        }
        #endregion

        #region btnClear_Click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/frmULDMovement.aspx", false);
        }
        #endregion

    }
}
