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
    //28-7-2012
    //31-7-2012
    public partial class ArrivalReassign : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());
        BLExpManifest objExpMani = new BLExpManifest();
        BLArrival objExpManifest = new BLArrival();
        bool blnResult;
        int incr = 0;
        DataTable dt = new DataTable();
        
        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    LoadGrid();
                    if (Session["Arrival_Reassign"] != null)
                    {
                        dt = (DataTable)Session["Arrival_Reassign"];
                        sessiondata(incr);
                        txtorigin.Text= Session["Station"].ToString(); 
                    }
                }
                catch (Exception ex)
                {

                }
            }
            
        }
        #endregion Load
        
        #region destination
        protected void txtDestination_TextChanged(object sender, EventArgs e)
        {
            if (txtorigin.Text == "" && txtDestination.Text == "")
            {
                LBLStatus.Text = "Please Enter Origin and Destination";
                LBLStatus.ForeColor = Color.Red;
                return;
            }

            hiddest.Value = txtDestination.Text.ToUpper();
            Session["ArrivalReassignDest"] = hiddest.Value.ToUpper();
        }
        #endregion destination
        
        #region sessiondata
        public void sessiondata(int ro)
        {
            try
            {
                dt = (DataTable)Session["Arrival_Reassign"];

                if (ro >= dt.Rows.Count)
                {
                    return;
                }
                else
                {
                    grdArrivalReassign.DataSource = dt;
                    grdArrivalReassign.DataBind();
                }

                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    (((Label)grdArrivalReassign.Rows[i].FindControl("lblawbno")).Text) = dt.Rows[i][0].ToString();
                //    (((Label)grdArrivalReassign.Rows[i].FindControl("lblflightno")).Text) = dt.Rows[i][1].ToString();
                //    (((Label)grdArrivalReassign.Rows[i].FindControl("lblpieces")).Text) = dt.Rows[i][2].ToString();
                //    (((Label)grdArrivalReassign.Rows[i].FindControl("lblweight")).Text) = dt.Rows[i][3].ToString();
                //    (((Label)grdArrivalReassign.Rows[i].FindControl("lblorigin")).Text) = dt.Rows[i][4].ToString();
                //    (((Label)grdArrivalReassign.Rows[i].FindControl("lbldestination")).Text) = dt.Rows[i][5].ToString();
                //}

            }
            catch (Exception ex)
            { }
        }
        #endregion sessiondata
        
        #region LoadGrid
        public void LoadGrid()
        {
            //DataTable dt1 = new DataTable();
            DataTable myDataTable = new DataTable();
            DataColumn myDataColumn;
            DataSet Ds = new DataSet();

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "AWBNumber";
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
            myDataColumn.ColumnName = "FlightNumber";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Origin";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Destination";
            myDataTable.Columns.Add(myDataColumn);



            DataRow dr;
            dr = myDataTable.NewRow();

            dr["AWBNumber"] = "";// "TV";
            dr["Pieces"] = "";
            dr["Weight"] = "";
            dr["FlightNumber"] = "";
            dr["Origin"] = "";
            dr["Destination"] = "";

            myDataTable.Rows.Add(dr); ;


            grdArrivalReassign.DataSource = null;
            grdArrivalReassign.DataSource = myDataTable;
            grdArrivalReassign.DataBind();
        }
        #endregion Loadgrid
        
        #region save
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtorigin.Text == "" && txtDestination.Text == "" && txtflightno.Text == "")
            {
                LBLStatus.Text = "Please Enter Origin Destination and Flight Number";
                LBLStatus.ForeColor = Color.Red;
                return; 
            }

            try
            {
                #region ValidateFlight

                LBLStatus.Text = "";

                #region Paramname
                string[] paramname = new string[3];
                paramname[0] = "Source";
                paramname[1] = "Dest";
                paramname[2] = "FlightID"; 
                #endregion Paramname
                #region Paramvalue
                object[] paramvalue = new object[3];
                paramvalue[0] = txtorigin.Text.Trim();
                paramvalue[1] = txtDestination.Text.Trim();
                paramvalue[2] = txtflightno.Text;   

                #endregion ParamValue
                #region Paramtye
                SqlDbType[] paramtype = new SqlDbType[3];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;   
                #endregion Paramtype
                DataSet ds = da.SelectRecords("Sp_ValidateFlight", paramname, paramvalue, paramtype);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {

                }
                else
                {
                    LBLStatus.Text = "Sorry..Flight ID "+txtflightno.Text +" Is not available for this origin: " + txtorigin.Text + "to Destnation: " + txtDestination.Text;
                    LBLStatus.ForeColor = Color.Red;
                    return; 
                }
                #endregion ValidateFlight


                #region Save
                string AWBno = "", AWBPrefix = "", POU = "", POL = "", ActFLTno = "", Updatedby = "", OffLoadFltNo = "", OffloadLoc = "";
                int OffloadPCS = 0, AVLPCS = 0; 
                double OffloadWGT = 0.0, AVLWGT = 0.0;//string statusreassign="";
                DateTime dtdate, ActFlightDate;
                
                for (int i = 0; i < grdArrivalReassign.Rows.Count; i++)
                {
                    OffLoadFltNo = txtflightno.Text;

                    OffloadLoc = Convert.ToString(Session["Station"]);

                    ActFLTno = (((Label)grdArrivalReassign.Rows[i].FindControl("lblflightno")).Text);

                    POL = Convert.ToString(Session["Station"]);
                    POU = txtDestination.Text;

                    Updatedby = Convert.ToString(Session["Username"]);
                    AWBno = (((Label)grdArrivalReassign.Rows[i].FindControl("lblawbno")).Text);

                    AWBPrefix = AWBno.Substring(0, AWBno.IndexOf('-'));
                    AWBno = AWBno.Substring(AWBno.IndexOf('-') + 1, 8);

                    OffloadPCS = Convert.ToInt32(((Label)grdArrivalReassign.Rows[i].FindControl("lblpieces")).Text);
                    OffloadWGT = Convert.ToDouble(((Label)grdArrivalReassign.Rows[0].FindControl("lblweight")).Text);
                    AVLPCS = Convert.ToInt32(((Label)grdArrivalReassign.Rows[0].FindControl("lblpieces")).Text);
                    AVLWGT = Convert.ToDouble(((Label)grdArrivalReassign.Rows[0].FindControl("lblweight")).Text);
                    //statusreassign = "Reassign";
                    if (txtreassignmentdate.Text == "")
                    {
                        LBLStatus.Text = "Please enter Reassignment Date";
                        LBLStatus.ForeColor = Color.Red;
                        return;
                    }

                    dtdate =DateTime.ParseExact(txtreassignmentdate.Text,"dd-MM-yyyy",null) ;
                    string actdate = Session["ActlFlightDate"].ToString();
                    ActFlightDate = DateTime.ParseExact(actdate, "dd/MM/yyyy", null);

                    DateTime dtCurrentDate = DateTime.Now;
                    string PartnerCode = OffLoadFltNo.Substring(0, 2);

                    blnResult = objExpMani.OffLoadShipmentinManifestForGHA(ActFLTno, OffLoadFltNo, OffloadLoc, AWBno, AVLPCS, AVLWGT, OffloadPCS, OffloadWGT,
                       Updatedby, POL, POU, "", dtdate, "", "O", ActFlightDate, dtCurrentDate, PartnerCode, "AIR", "",
                       Convert.ToDateTime(Session["IT"]), AWBPrefix, "", "","");

                    if (blnResult == true)
                    {
                        string[] pname1 = new string[2];
                        pname1[0] = "AWBNumber";
                        pname1[1] = "ULDNumber";


                        object[] pvalue = new object[2];
                        pvalue[0] = AWBPrefix + AWBno;
                        pvalue[1] = "";
                      
                        SqlDbType[] ptype = new SqlDbType[2];
                        ptype[0] = SqlDbType.VarChar;
                        ptype[1] = SqlDbType.VarChar;

                        bool ds1 = da.UpdateData("SpUpdateStatus", pname1, ptype, pvalue);

                    }

                }
                #endregion Save
                if (blnResult == true)
                {

                    LBLStatus.Text = "AWB Reassigned Successfully";
                    LBLStatus.ForeColor = Color.Green;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>closePage();</script>", false);
                    return;
                }
                else
                {
                    LBLStatus.Text = "AWB Not Reassigned..Please try again. ";
                    LBLStatus.ForeColor = Color.Red;
                    return;
                }
            }
            catch (Exception ex)
            {
            }

        }
#endregion save
        
        #region function
        //SELECT FlightID from dbo.AirlineSchedule where FlightID like '"+ prefixText +"%' or FlightID like '"+ prefixText +"%'


        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string[] GetFlight(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            string[] orgdest = new ArrivalReassign().GetOrgDest();
            //SqlDataAdapter dad = new SqlDataAdapter("SELECT DISTINCT (FlightID) AS FlightID from dbo.AirlineSchedule where FlightID like '" + prefixText + "%' or FlightID like '" + prefixText + "%'", con);
            SqlDataAdapter dad = new SqlDataAdapter("SELECT DISTINCT (FlightID) AS FlightID from dbo.AirlineSchedule where Source like '" + orgdest[0] + "%' or Dest like '" + orgdest[1] + "%'", con);
            DataSet ds = new DataSet();
            dad.Fill(ds);
            List<string> list = new List<string>(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(dr[0].ToString());

            }

            return list.ToArray();
        }


        #endregion function

        public string[] GetOrgDest()
        {
            string[] result = { "", "" };
            if (result[0] != null && result[0]=="")
            {
                result[0] = Session["Station"].ToString().ToUpper();
                //result[0] = txtorigin.Text.ToUpper();//Session["ArrivalReassign_Org"].ToString();
            }
            if (result[1] != null && result[1] == "")
            {
                result[1] = Session["ArrivalReassignDest"].ToString().ToUpper();
            }
            
            return result;
        }
    }
}
