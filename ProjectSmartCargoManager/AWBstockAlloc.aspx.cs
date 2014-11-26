using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using QID.DataAccess;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class AWBstockAlloc : System.Web.UI.Page
    {
        string gvUniqueID = String.Empty;
        int gvNewPageIndex = 0;
        int gvEditIndex = -1;
        string gvSortExpr = String.Empty;
        SqlConnection sc = null;
        SqlCommand scmd = null;

        SqlDataReader sdr = null;
        SqlConnection sc1 = null;
        SqlCommand scmd1 = null;
        //Database db = new Database();
        string ConStr = Global.GetConnectionString();//ConfigurationManager.ConnectionStrings["ConStr"].ToString();

        BLawbStockAllocBAL BLObj = new BLawbStockAllocBAL();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loaddata();
                // getLocationdata();

                GetAgentName();
            }
        }

        #region Get Agent Name
        public void GetAgentName()
        {
            try
            {
                DataSet DsAgent = new DataSet();
                SQLServer db = new SQLServer(ConStr);

                DsAgent = BLObj.GetAgentName();
                if (DsAgent != null && DsAgent.Tables[0].Rows.Count > 0)
                {
                    DDLAgent.DataSource = DsAgent;
                    DDLAgent.DataTextField = "AgentName";
                    DDLAgent.DataValueField = "AgentName"; ;
                    // DDLAgent.DataMember = "AgentName";
                    DDLAgent.DataBind();
                }
            }
            catch (Exception ex)
            { }

        }

        #endregion Get Agent Name

        #region Load Data Grid/Get Agent Name Data
        protected void loaddata()
        {
            // GVUBI.DataSource = null;
            // GVUBI.DataBind();
            try
            {
                SQLServer db = new SQLServer(ConStr);
                DataSet ds = db.SelectRecords("SpGetAgentAWBStockData");

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            GridView1.DataSource = ds;
                            GridView1.DataBind();

                        }
                    }
                }
            }
            catch (Exception ex)
            { }

        }
        #endregion Load Data Grid/Get Agent Name Data

        #region GridView1 Event Handlers
        //This event occurs for each row
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;
            string strSort = string.Empty;

            // Make sure we aren't in header/footer rows
            if (row.DataItem == null)
            {
                return;
            }

            DataTable dt1 = new DataTable();

            //Find Child GridView control

            GridView gv = new GridView();
            gv = (GridView)row.FindControl("GridView2");

            //Check if any additional conditions (Paging, Sorting, Editing, etc) to be applied on child GridView
            if (gv.UniqueID == gvUniqueID)
            {
                gv.PageIndex = gvNewPageIndex;
                gv.EditIndex = gvEditIndex;
                //Check if Sorting used
                //if (gvSortExpr != string.Empty)
                //{
                //    GetSortDirection();
                //    strSort = " ORDER BY " + string.Format("{0} {1}", gvSortExpr, gvSortDir);
                //}
                //Expand the Child grid
                ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + ((DataRowView)e.Row.DataItem)["Agentname"].ToString() + "','one');</script>");
            }

            //Prepare the query for Child GridView by passing the Customer ID of the parent row
            strSort = ((DataRowView)e.Row.DataItem)["Agentname"].ToString();
            sc = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString);
            sc.Open();
            scmd = new SqlCommand("SPGetAWbStockbyAGENTNAME", sc); //receipt come from xpress bag details
            scmd.CommandType = CommandType.StoredProcedure;
            scmd.Parameters.AddWithValue("@Agentname", strSort);
            sdr = scmd.ExecuteReader();
            dt1.Load(sdr);

            gv.DataSource = dt1;
            gv.DataBind();

            //for (int i = 0; i < dt1.Rows.Count; i++)
            //{
            //    if (dt1.Rows[i][4].ToString() == "True")
            //    {
            //        gv.Rows[i].Cells[6].Enabled = false;
            //    }
            //    else
            //    {
            //        gv.Rows[i].Cells[6].Enabled = true;
            //    }
            //}

            sc.Close();


            //Add delete confirmation message for Customer
            //LinkButton l = (LinkButton)e.Row.FindControl("linkDeleteCust");
            //l.Attributes.Add("onclick", "javascript:return " +
            //"confirm('Are you sure you want to delete this Customer " +
            //DataBinder.Eval(e.Row.DataItem, "CustomerID") + "')");

        }

        //This event occurs for any operation on the row of the grid
        //protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    //Check if Add button clicked
        //    if (e.CommandName == "AddCustomer")
        //    {
        //        try
        //        {
        //            //Get the values stored in the text boxes
        //            string strCompanyName = ((TextBox)GridView1.FooterRow.FindControl("txtCompanyName")).Text;
        //            string strContactName = ((TextBox)GridView1.FooterRow.FindControl("txtContactName")).Text;
        //            string strContactTitle = ((TextBox)GridView1.FooterRow.FindControl("txtContactTitle")).Text;
        //            string strAddress = ((TextBox)GridView1.FooterRow.FindControl("txtAddress")).Text;
        //            string strCustomerID = ((TextBox)GridView1.FooterRow.FindControl("txtCustomerID")).Text;

        //            //Prepare the Insert Command of the DataSource control
        //            string strSQL = "";
        //            strSQL = "INSERT INTO Customers (CustomerID, CompanyName, ContactName, " +
        //                    "ContactTitle, Address) VALUES ('" + strCustomerID + "','" + strCompanyName + "','" +
        //                    strContactName + "','" + strContactTitle + "','" + strAddress + "')";

        //            AccessDataSource1.InsertCommand = strSQL;
        //            AccessDataSource1.Insert();
        //            ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Customer added successfully');</script>");

        //            //Re bind the grid to refresh the data
        //            GridView1.DataBind();
        //        }
        //        catch (Exception ex)
        //        {
        //            ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + ex.Message.ToString().Replace("'", "") + "');</script>");
        //        }
        //    }
        //}

        //This event occurs on click of the Update button
        //protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        //{
        //    //Get the values stored in the text boxes
        //    string strCompanyName = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtCompanyName")).Text;
        //    string strContactName = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtContactName")).Text;
        //    string strContactTitle = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtContactTitle")).Text;
        //    string strAddress = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtAddress")).Text;
        //    string strCustomerID = ((Label)GridView1.Rows[e.RowIndex].FindControl("lblCustomerID")).Text;

        //    try
        //    {
        //        //Prepare the Update Command of the DataSource control
        //        string strSQL = "";
        //        strSQL = "UPDATE Customers set CompanyName = '" + strCompanyName + "'" +
        //                 ",ContactName = '" + strContactName + "'" +
        //                 ",ContactTitle = '" + strContactTitle + "'" +
        //                 ",Address = '" + strAddress + "'" +
        //                 " WHERE CustomerID = '" + strCustomerID + "'";
        //        ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Customer updated successfully');</script>");
        //    }
        //    catch { }
        //}

        //This event occurs after RowUpdating to catch any constraints while updating
        //protected void GridView1_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        //{
        //    //Check if there is any exception while deleting
        //    if (e.Exception != null)
        //    {
        //        ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + e.Exception.Message.ToString().Replace("'", "") + "');</script>");
        //        e.ExceptionHandled = true;
        //    }
        //}

        //This event occurs on click of the Delete button
        //protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    //Get the value        
        //    string strCustomerID = ((Label)GridView1.Rows[e.RowIndex].FindControl("lblCustomerID")).Text;

        //    //Prepare the delete Command of the DataSource control
        //    string strSQL = "";

        //    try
        //    {
        //        strSQL = "DELETE from Customers WHERE CustomerID = '" + strCustomerID + "'";
        //        ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Customer deleted successfully');</script>");
        //    }
        //    catch { }
        //}

        //This event occurs after RowDeleting to catch any constraints while deleting
        //protected void GridView1_RowDeleted(object sender, GridViewDeletedEventArgs e)
        //{
        //    //Check if there is any exception while deleting
        //    if (e.Exception != null)
        //    {
        //        ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + e.Exception.Message.ToString().Replace("'", "") + "');</script>");
        //        e.ExceptionHandled = true;
        //    }
        //}
        #endregion

        #region GridView2 Event Handlers
        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView gvTemp = (GridView)sender;
            gvUniqueID = gvTemp.UniqueID;
            gvNewPageIndex = e.NewPageIndex;
            loaddata();
        }

        //protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "AddOrder")
        //    {
        //        try
        //        {
        //            GridView gvTemp = (GridView)sender;
        //            gvUniqueID = gvTemp.UniqueID;

        //            //Get the values stored in the text boxes
        //            string strCustomerID = gvTemp.DataKeys[0].Value.ToString();  //Customer ID is stored as DataKeyNames
        //            string strFreight = ((TextBox)gvTemp.FooterRow.FindControl("txtFreight")).Text;
        //            string strShipperName = ((TextBox)gvTemp.FooterRow.FindControl("txtShipperName")).Text;
        //            string strShipAdress = ((TextBox)gvTemp.FooterRow.FindControl("txtShipAdress")).Text;

        //            //Prepare the Insert Command of the DataSource control
        //            string strSQL = "";
        //            strSQL = "INSERT INTO Orders (CustomerID, Freight, ShipName, " +
        //                    "ShipAddress) VALUES ('" + strCustomerID + "'," + float.Parse(strFreight) + ",'" +
        //                    strShipperName + "','" + strShipAdress + "')";

        //            ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Order added successfully');</script>");

        //            GridView1.DataBind();
        //        }
        //        catch (Exception ex)
        //        {
        //            ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + ex.Message.ToString().Replace("'", "") + "');</script>");
        //        }
        //    }
        //}
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Check if this is our Blank Row being databound, if so make the row invisible
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((DataRowView)e.Row.DataItem)["Agentname"].ToString() == String.Empty) e.Row.Visible = false;
            }
        }
        protected void GridView2_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView gvTemp = (GridView)sender;
            gvUniqueID = gvTemp.UniqueID;
            gvEditIndex = e.NewEditIndex;

            loaddata();
        }

        protected void GridView2_CancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView gvTemp = (GridView)sender;
            gvUniqueID = gvTemp.UniqueID;
            gvEditIndex = -1;
            loaddata();
        }




        protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridView gvTemp = (GridView)sender;
                gvUniqueID = gvTemp.UniqueID;

                //Get the values stored in the text boxes
                string strOrderID = ((Label)gvTemp.Rows[e.RowIndex].FindControl("lblOrderID")).Text;
                string strFreight = ((TextBox)gvTemp.Rows[e.RowIndex].FindControl("txtFreight")).Text;
                string strShipperName = ((TextBox)gvTemp.Rows[e.RowIndex].FindControl("txtShipperName")).Text;
                string strShipAdress = ((TextBox)gvTemp.Rows[e.RowIndex].FindControl("txtShipAdress")).Text;

                //Prepare the Update Command of the DataSource control
                AccessDataSource dsTemp = new AccessDataSource();
                dsTemp.DataFile = "App_Data/Northwind.mdb";
                string strSQL = "";
                strSQL = "UPDATE Orders set Freight = " + float.Parse(strFreight) + "" +
                         ",ShipName = '" + strShipperName + "'" +
                         ",ShipAddress = '" + strShipAdress + "'" +
                         " WHERE OrderID = " + strOrderID;
                dsTemp.UpdateCommand = strSQL;
                dsTemp.Update();
                ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Order updated successfully');</script>");

                //Reset Edit Index
                gvEditIndex = -1;

                loaddata();
            }
            catch { }
        }

        protected void GridView2_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            //Check if there is any exception while deleting
            if (e.Exception != null)
            {
                ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + e.Exception.Message.ToString().Replace("'", "") + "');</script>");
                e.ExceptionHandled = true;
            }
        }

        //protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    GridView gvTemp = (GridView)sender;
        //    gvUniqueID = gvTemp.UniqueID;

        //    //Get the value        
        //    string strOrderID = ((Label)gvTemp.Rows[e.RowIndex].FindControl("lblOrderID")).Text;

        //    //Prepare the Update Command of the DataSource control
        //    string strSQL = "";

        //    try
        //    {
        //        strSQL = "DELETE from Orders WHERE OrderID = " + strOrderID;
        //        AccessDataSource dsTemp = new AccessDataSource();
        //        dsTemp.DataFile = "App_Data/Northwind.mdb";
        //        dsTemp.DeleteCommand = strSQL;
        //        dsTemp.Delete();
        //        ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Order deleted successfully');</script>");
        //        GridView1.DataBind();
        //    }
        //    catch { }
        //}

        //protected void GridView2_RowDeleted(object sender, GridViewDeletedEventArgs e)
        //{
        //    //Check if there is any exception while deleting
        //    if (e.Exception != null)
        //    {
        //        ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + e.Exception.Message.ToString().Replace("'", "") + "');</script>");
        //        e.ExceptionHandled = true;
        //    }
        //}

        //protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    //Check if this is our Blank Row being databound, if so make the row invisible
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        if (((DataRowView)e.Row.DataItem)["OrderID"].ToString() == String.Empty) e.Row.Visible = false;
        //    }
        //}

        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridView gvTemp = (GridView)sender;
            gvUniqueID = gvTemp.UniqueID;
            gvSortExpr = e.SortExpression;
            loaddata();
        }
        #endregion

        protected void lstCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            getLocationdata();
        }

        private void getLocationdata()
        {
            try
            {
                //DataTable dt1 = new DataTable();
                //string strSort = lstCity.SelectedItem.Text;
                //sc = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString);

                //sc.Open();
                //scmd = new SqlCommand("spXpressgetLocationCodebyCityCode", sc); //receipt come from xpress bag details
                //scmd.CommandType = CommandType.StoredProcedure;
                //scmd.Parameters.AddWithValue("@CityCode", strSort);
                //sdr = scmd.ExecuteReader();
                //dt1.Load(sdr);
                //GridView1.DataSource = dt1;
                //GridView1.DataBind();
                //sc.Close();
            }
            catch (Exception ex)
            { }
        }

        #region DDL Level Index Change
        protected void DDLLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (DDLLevel.SelectedItem.Text == "Country")
                {
                    DDLSubLevel.SelectedItem.Text = "IND";
                }
                else if (DDLLevel.SelectedItem.Text == "Region")
                {
                    DataSet ds = BLObj.Getsubleveldata(DDLLevel.SelectedItem.Text);

                    DDLSubLevel.DataSource = ds;

                    DDLSubLevel.DataTextField = "RegionName";
                    DDLSubLevel.DataValueField = "RegionName";
                    DDLSubLevel.DataBind();

                }
                else if (DDLLevel.SelectedItem.Text == "Station")
                {
                    DataSet ds = BLObj.Getsubleveldata(DDLLevel.SelectedItem.Text);

                    DDLSubLevel.DataSource = ds;

                    DDLSubLevel.DataTextField = "CityCode";
                    DDLSubLevel.DataValueField = "CityCode";
                    DDLSubLevel.DataBind();
                }
                else if (DDLLevel.SelectedItem.Text == "City")
                {
                    DataSet ds = BLObj.Getsubleveldata(DDLLevel.SelectedItem.Text);

                    DDLSubLevel.DataSource = ds;

                    DDLSubLevel.DataTextField = "CityName";
                    DDLSubLevel.DataValueField = "CityName";
                    DDLSubLevel.DataBind();

                }


            }
            catch (Exception ex)
            { }
        }

        #endregion DDL Level Index Change


        //protected void btnAlloc_Click(object sender, EventArgs e)
        //{
        //    string deviceName = txtDeviceName.Text;
        //    string from = txtUBIDev.Text.PadLeft(13, '0');
        //    string to = txtToUBIDev.Text.PadLeft(13, '0');
        //    if (Convert.ToDouble(from) > Convert.ToDouble(to))
        //    {
        //       // getLocationdata();

        //        return;
        //    }
        //    if (from != "" && to != "" && deviceName != "")
        //    {
        //       // insertORupdateDevice(deviceName, from, to);
        //        txtDeviceName.Text = "";
        //        txtUBIDev.Text = "";
        //        txtToUBIDev.Text = "";
        //        loaddata();
        //    }
        //    else
        //    {
        //        ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "EmptyMsg();", true);

        //    }
        //}


        public void btnAlloc_Click(object sender, EventArgs e)
        {

            string Agentname = "", Level = "", SubLevel = "", FrmAWB = "", ToAWB = "", UpdatedOn = "";

            // string deviceName = txtDeviceName.Text;
            Agentname = DDLAgent.SelectedItem.Text;
            Level = DDLLevel.SelectedItem.Text;
            SubLevel = DDLSubLevel.SelectedItem.Text;
            FrmAWB = txtUBIDev.Text.PadLeft(8, '0');
            ToAWB = txtToUBIDev.Text.PadLeft(8, '0');
            UpdatedOn = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            if (Convert.ToDouble(FrmAWB) > Convert.ToDouble(ToAWB))
            {
                // getLocationdata();
                // GetAgentName();
                return;
            }
            if (FrmAWB != "" && ToAWB != "" && DDLAgent.SelectedIndex >= 0 && DDLSubLevel.SelectedIndex >= 0)
            {
                // insertORupdateDevice(deviceName, from, to);

               // DataSet res = BLObj.SaveAgentAllocatedData(Agentname, Level, SubLevel, FrmAWB, ToAWB, UpdatedOn);

                DDLAgent.SelectedIndex = 0;

                txtUBIDev.Text = "";
                txtToUBIDev.Text = "";
                loaddata();
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "EmptyMsg();", true);

            }


        }



    }
}
