using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
//using clsDataLib;
using System.Drawing;
using QID.DataAccess;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class frmPOStatusChange : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    txtPONo.Focus();
                    btnSave.Visible = false;
                }
                catch (Exception)
                {
                }
            }
        }

        public void chkLineSD(object sender, EventArgs e)
        {
            try
            {
                //Get the button that raised the event
                CheckBox btn = (CheckBox)sender;

                //Get the row that contains this button
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;

                //Get rowindex
                int rowindex = gvr.RowIndex;
                
                if (((CheckBox)GVOrdDetails.Rows[rowindex].FindControl("chkPOLine")).Checked == true)
                {
                    for (int i = 0; i < ((CheckBoxList)GVOrdDetails.Rows[rowindex].FindControl("chList")).Items.Count; i++)
                    {
                        if(((CheckBoxList)GVOrdDetails.Rows[rowindex].FindControl("chList")).Items[i].Enabled==true)
                        ((CheckBoxList)GVOrdDetails.Rows[rowindex].FindControl("chList")).Items[i].Selected = true;
                    }
                }
                else
                {
                    for (int i = 0; i < ((CheckBoxList)GVOrdDetails.Rows[rowindex].FindControl("chList")).Items.Count; i++)
                    {
                        if (((CheckBoxList)GVOrdDetails.Rows[rowindex].FindControl("chList")).Items[i].Enabled == true)
                        ((CheckBoxList)GVOrdDetails.Rows[rowindex].FindControl("chList")).Items[i].Selected = false;
                    }
                }
                int ch = 0;
                for (int i = 0; i < GVOrdDetails.Rows.Count; i++)
                {
                    if (((CheckBox)GVOrdDetails.Rows[i].FindControl("chkPOLine")).Checked == true)
                    {
                    }
                    else
                    {
                        ch = 1;
                        break;
                    }
                }
                if (ch == 0)
                {
                    ((CheckBox)GVOrdDetails.HeaderRow.FindControl("chkfullPO")).Checked = true;
                }
                else
                {
                    ((CheckBox)GVOrdDetails.HeaderRow.FindControl("chkfullPO")).Checked = false;
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.Message;
                lblerror.ForeColor = Color.Red;
            }
        }
        public void chkHeadAll(object sender, EventArgs e)
        {
            try
            {
                if (((CheckBox)GVOrdDetails.HeaderRow.FindControl("chkfullPO")).Checked == true)
                {
                    for (int i = 0; i < GVOrdDetails.Rows.Count; i++)
                    {
                        ((CheckBox)GVOrdDetails.Rows[i].FindControl("chkPOLine")).Checked = true;
                        for (int j = 0; j < ((CheckBoxList)GVOrdDetails.Rows[i].FindControl("chList")).Items.Count; j++)
                        {
                            if (((CheckBoxList)GVOrdDetails.Rows[i].FindControl("chList")).Items[j].Enabled == true)
                                ((CheckBoxList)GVOrdDetails.Rows[i].FindControl("chList")).Items[j].Selected = true;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < GVOrdDetails.Rows.Count; i++)
                    {
                        ((CheckBox)GVOrdDetails.Rows[i].FindControl("chkPOLine")).Checked = false;
                        for (int j = 0; j < ((CheckBoxList)GVOrdDetails.Rows[i].FindControl("chList")).Items.Count; j++)
                        {
                            if (((CheckBoxList)GVOrdDetails.Rows[i].FindControl("chList")).Items[j].Enabled == true)
                                ((CheckBoxList)GVOrdDetails.Rows[i].FindControl("chList")).Items[j].Selected = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.Message;
                lblerror.ForeColor = Color.Red;
            }
        }
        public void chkChildSD(object sender, EventArgs e)
        {
            try
            {
                //Get the button that raised the event
                CheckBoxList btn = (CheckBoxList)sender;

                //Get the row that contains this button
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;

                //Get rowindex
                int rowindex = gvr.RowIndex;
                for (int i = 0; i < ((CheckBoxList)GVOrdDetails.Rows[rowindex].FindControl("chList")).Items.Count; i++)
                {
                        if(((CheckBoxList)GVOrdDetails.Rows[rowindex].FindControl("chList")).Items[i].Selected != true)
                        {
                        ((CheckBox)GVOrdDetails.Rows[rowindex].FindControl("chkPOLine")).Checked = false;
                        ((CheckBox)GVOrdDetails.HeaderRow.FindControl("chkfullPO")).Checked = false;
                        return;
                        }
                }
                ((CheckBox)GVOrdDetails.Rows[rowindex].FindControl("chkPOLine")).Checked = true; // this line directly bcoz if ant check box not check it will trow outside, if he reach here mean all r true
                int ch = 0;
                for (int i = 0; i < GVOrdDetails.Rows.Count; i++)
                {
                    if (((CheckBox)GVOrdDetails.Rows[i].FindControl("chkPOLine")).Checked == true)
                    {

                    }
                    else
                    {
                        ch = 1;
                        break;
                    }
                }
                if (ch == 0)
                {
                    ((CheckBox)GVOrdDetails.HeaderRow.FindControl("chkfullPO")).Checked = true;
                }
                else
                {
                    ((CheckBox)GVOrdDetails.HeaderRow.FindControl("chkfullPO")).Checked = false;
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.Message;
                lblerror.ForeColor = Color.Red;
            }
        }

        public void search()
        {
            lblerror.Text = "";
            if (txtPONo.Text.Trim() != "")
            {
                Session["NEWStatusPO"] = null;
                Session["innerStatusdata"] = null;
                Session["findStatuscnt"] = 0;
              
                try
                {
                    string[] paramnameRB = new string[1];
                    paramnameRB[0] = "pono";

                    object[] paramvalueRB = new object[1];
                    paramvalueRB[0] = txtPONo.Text.Trim();

                    SqlDbType[] paramtypeRB = new SqlDbType[1];
                    paramtypeRB[0] = SqlDbType.NVarChar;

                    SQLServer dbRBPO = new SQLServer(Global.GetConnectionString());
                    DataSet res = dbRBPO.SelectRecords("spgetPODetails", paramnameRB, paramvalueRB, paramtypeRB);
                    if (res != null && res.Tables.Count > 0)
                    {
                        if (res.Tables[0].Rows.Count > 0)
                        {
                            btnSave.Visible = false;

                            if (res.Tables[0].Rows[0][0].ToString() == "1")
                            {
                                Session["NEWStatusPO"] = (DataTable)res.Tables[1];
                                DataSet ds = new DataSet();

                                for (int i = 0; i < res.Tables[2].Rows.Count; i++)
                                {
                                    DataTable dt = new DataTable();
                                    dt.Columns.Add("RecULD",typeof(bool));
                                    dt.Columns.Add("RecULDNo");


                                    DataRow[] result = res.Tables[3].Select("PODetailsID = " + res.Tables[2].Rows[i][0].ToString());
                                    foreach (DataRow row in result)
                                    {
                                        dt.Rows.Add(row[2],row[0]);
                                    }
                                    ds.Tables.Add(dt);
                                }
                                Session["innerStatusdata"] = ds;
                                GVOrdDetails.DataSource = res.Tables[1];
                                GVOrdDetails.DataBind();
                                btnSave.Visible = true;
                                btnSave.Focus();
                            }
                            else if (res.Tables[0].Rows[0][0].ToString() == "2")
                            {
                                lblerror.Text = "PO Number does not exist";
                                lblerror.ForeColor = Color.Red;
                                txtPONo.Focus();
                                return;
                            }
                            else
                            {
                                lblerror.Text = "Error while fetching data. Please try again";
                                lblerror.ForeColor = Color.Red;
                                btnSearch.Focus();
                                return;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblerror.Text = "Error: " + ex.Message;
                    lblerror.ForeColor = Color.Red;
                    btnSave.Visible = false;
                }
            }
            else
            {
                lblerror.Text = "PO Number is required";
                lblerror.ForeColor = Color.Red;
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            search();
        }
        protected void GVOrdDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

               if (e.Row.RowIndex != -1 && e.Row.DataItem != null)
                {

                    //Itendifying checkboxlist in gridview template column  
                    CheckBoxList chklist = (CheckBoxList)e.Row.Cells[1].FindControl("chList");
                    CheckBox chkPOLine = (CheckBox)e.Row.Cells[1].FindControl("chkPOLine");

                    DataSet  innergrid = (DataSet)Session["innerStatusdata"];
                    if (innergrid.Tables[Convert.ToInt32(Session["findStatuscnt"].ToString())].Rows[0][1].ToString().Trim() != "")
                    {
                        chklist.DataSource = innergrid.Tables[Convert.ToInt32(Session["findStatuscnt"].ToString())];
                        chklist.DataTextField = "RecULDNo";
                        chklist.DataValueField = "RecULD";
                        chklist.DataBind();
                        for (int i = 0; i < chklist.Items.Count; i++)
                        {
                            if (chklist.Items[i].Value.ToLower() == "true")
                            {
                                chklist.Items[i].Selected = true;
                                chklist.Items[i].Enabled = false;
                            }
                        }
                    }
                    Session["findStatuscnt"] = Convert.ToInt32(Session["findStatuscnt"]) + 1;
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.Message;
                lblerror.ForeColor = Color.Red;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtPONo.Text = "";
            Session["NEWStatusPO"] = null;
            Session["innerStatusdata"] = null;
            Session["findStatuscnt"] = 0;
            btnSave.Visible = false;
            GVOrdDetails.DataSource = null;
            GVOrdDetails.DataBind();
            txtPONo.Focus();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool isFull = false;
                string idofFullline = string.Empty;
                string uldNoinline = string.Empty;
                #region checking all validation and type of selection
                if (txtPONo.Text.Trim() == "")
                {
                    lblerror.Text = "PO Number is required";
                    lblerror.ForeColor = Color.Red;
                    return;
                }
                if (((CheckBox)GVOrdDetails.HeaderRow.FindControl("chkfullPO")).Checked == true)
                {
                    isFull = true;
                }
                if (isFull == false)
                {
                    for (int i = 0; i < GVOrdDetails.Rows.Count; i++)
                    {
                        if (((CheckBox)GVOrdDetails.Rows[i].FindControl("chkPOLine")).Checked == true)
                        {
                            if (idofFullline == string.Empty)
                                idofFullline = "'" + ((Label)GVOrdDetails.Rows[i].FindControl("grdisRec")).Text.Trim() + "'";
                            else
                                idofFullline = idofFullline + "," + "'" + ((Label)GVOrdDetails.Rows[i].FindControl("grdisRec")).Text.Trim() + "'";
                        }
                        else
                        {
                            if (((Label)GVOrdDetails.Rows[i].FindControl("grdULDType")).Text.Trim() != "")
                            {
                                for (int j = 0; j < ((CheckBoxList)GVOrdDetails.Rows[i].FindControl("chList")).Items.Count; j++)
                                {
                                    if (((CheckBoxList)GVOrdDetails.Rows[i].FindControl("chList")).Items[j].Selected == true)
                                    {
                                        if (uldNoinline == string.Empty)
                                            uldNoinline = "'" + ((CheckBoxList)GVOrdDetails.Rows[i].FindControl("chList")).Items[j].Text.Trim() + "'";
                                        else
                                            uldNoinline = uldNoinline + "," + "'" + ((CheckBoxList)GVOrdDetails.Rows[i].FindControl("chList")).Items[j].Text.Trim() + "'";
                                    }
                                }
                            }
                        }

                        
                    }
                }
                #region Adding ULD in master Deepak(26APR14)
                try
                {
                    for (int i = 0; i < GVOrdDetails.Rows.Count; i++)
                    {
                        if (((Label)GVOrdDetails.Rows[i].FindControl("grdULDType")).Text.Trim() != "")
                        {
                            for (int j = 0; j < ((CheckBoxList)GVOrdDetails.Rows[i].FindControl("chList")).Items.Count; j++)
                            {
                                if (((CheckBoxList)GVOrdDetails.Rows[i].FindControl("chList")).Items[j].Selected == true)
                                {
                                    SaveULDNoinMaster(((CheckBoxList)GVOrdDetails.Rows[i].FindControl("chList")).Items[j].Text.Trim(), "0");

                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
                #endregion
                #endregion

                #region Update PO Status SP
                string[] paramnameRB = new string[7];
                paramnameRB[0] = "pono";
                paramnameRB[1] = "isFull";
                paramnameRB[2] = "linesFullSelect";
                paramnameRB[3] = "uldSelect";
                paramnameRB[4] = "updatedby";
                paramnameRB[5] = "updatedOn";
                paramnameRB[6] = "ActDelWH";

                object[] paramvalueRB = new object[7];
                paramvalueRB[0] = txtPONo.Text.Trim();
                paramvalueRB[1] = isFull;
                paramvalueRB[2] = idofFullline;
                paramvalueRB[3] = uldNoinline;
                paramvalueRB[4] = Session["UserName"].ToString();
                paramvalueRB[5] = System.DateTime.Now.ToString();
                paramvalueRB[6] = Session["Station"].ToString();

                SqlDbType[] paramtypeRB = new SqlDbType[7];
                paramtypeRB[0] = SqlDbType.NVarChar;
                paramtypeRB[1] = SqlDbType.Bit;
                paramtypeRB[2] = SqlDbType.NVarChar;
                paramtypeRB[3] = SqlDbType.NVarChar;
                paramtypeRB[4] = SqlDbType.NVarChar;
                paramtypeRB[5] = SqlDbType.DateTime;
                paramtypeRB[6] = SqlDbType.NVarChar;

                SQLServer dbRBUPPO = new SQLServer(Global.GetConnectionString());
                DataSet res = dbRBUPPO.SelectRecords("spupdateStatusPOReceived", paramnameRB, paramvalueRB, paramtypeRB);
                if (res != null && res.Tables.Count > 0)
                {
                    if (res.Tables[0].Rows.Count > 0)
                    {
                        if (res.Tables[0].Rows[0][0].ToString() == "0")
                        {
                            lblerror.Text = "Error during Ststus Update";
                            lblerror.ForeColor = Color.Red;
                        }
                        else if (res.Tables[0].Rows[0][0].ToString() == "1")
                        {
                            lblerror.Text = "PO Number does not exist";
                            lblerror.ForeColor = Color.Red;
                        }
                        else
                        {
                            lblerror.Text = "PO status updated successfully";
                            lblerror.ForeColor = Color.Green;
                            search();
                        }
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                lblerror.Text = ex.Message;
                lblerror.ForeColor = Color.Red;
            }

        }

        private bool SaveULDNoinMaster(string UDLNumber, string DollyWt)
        {
            string strULDPrefix = UDLNumber.Trim().Substring(0, 3);
            string strULDSuffix = UDLNumber.Trim().Substring(UDLNumber.Trim().Length - 2, 2);
            string strULDSerial = UDLNumber.Trim().Replace(strULDPrefix, "").Replace(strULDSuffix, "");

            BALULDMaster blULD = new BALULDMaster();

            blULD.SelectRecords(UDLNumber, strULDSuffix, 0, "0", "0", 0, 0, 0, "0", "", "", "", false, "", strULDSerial, Convert.ToString(Session["Station"]), Convert.ToString(Session["UserName"]), Convert.ToDateTime(Session["IT"]),
                "", 0, "0", "0", Convert.ToDateTime(Session["IT"]), "", "", "", false, "", "N", DollyWt);

            blULD = null;

            return true;
        }
    }
}
