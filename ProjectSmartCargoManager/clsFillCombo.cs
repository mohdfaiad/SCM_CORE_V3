using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using QID.DataAccess;



namespace ProjectSmartCargoManager
{
    public class clsFillCombo
    {
        SQLServer db = null;
        public clsFillCombo()
        {
             db = new SQLServer(Global.GetConnectionString());
        }

        #region FILL ALL COMBOBOX
        public void FillAllComboBoxes(string TableName, string SelectType, System.Web.UI.WebControls.DropDownList drp)
        {
            string[] pname = new string[2];
            object[] pvalue = new object[2];
            SqlDbType[] ptype = new SqlDbType[2];
            DataSet dsdrp = null;
            try
            {
                pname[0] = "tblName";
                pname[1] = "defaultValue";

                pvalue[0] = TableName;
                pvalue[1] = SelectType;

                ptype[0] = SqlDbType.VarChar;
                ptype[1] = SqlDbType.VarChar;

                dsdrp = db.SelectRecords("spFillComboBoxMasters", pname, pvalue, ptype);
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
            finally
            {
                pname = null;
                pvalue = null;
                ptype = null;

                if (dsdrp != null)
                    dsdrp.Dispose();
            }

        }
        #endregion

        #region FILL ALL COMBOBOX
        public DataSet FillAllComboBoxeswithFlight(string TableName, string SelectType, System.Web.UI.WebControls.DropDownList drp)
        {
            string[] pname = new string[2];
            object[] pvalue = new object[2];
            SqlDbType[] ptype = new SqlDbType[2];
            DataSet dsdrp = null;
            try
            {
                pname[0] = "tblName";
                pname[1] = "defaultValue";

                pvalue[0] = TableName;
                pvalue[1] = SelectType;

                ptype[0] = SqlDbType.VarChar;
                ptype[1] = SqlDbType.VarChar;

                dsdrp = db.SelectRecords("spFillComboBoxMastersNew", pname, pvalue, ptype);
                if (dsdrp != null && dsdrp.Tables.Count > 0)
                {
                    if (dsdrp.Tables[0].Rows.Count > 0)
                    {
                        drp.Items.Clear();
                        //drpWWR.Items.Add("Select");
                        drp.DataSource = dsdrp.Tables[0];
                        drp.DataTextField = "Code";
                        drp.DataValueField = "ID";
                        drp.DataBind();
                        drp.SelectedIndex = -1;
                    }
                    else
                    {
                        drp.Items.Clear();
                        drp.SelectedIndex = 0;
                    }
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
                if (dsdrp != null)
                    dsdrp.Dispose();
                return (null);
            }
            finally
            {
                pname = null;
                pvalue = null;
                ptype = null;
            }
            return dsdrp;
        }
        #endregion

        #region Fill SubWH Combo Based On WHCode
        public void FillSubWHBasedOnWH(string WHCode, System.Web.UI.WebControls.DropDownList drp)
        {
            string[] pname = new string[2];
            object[] pvalue = new object[2];
            SqlDbType[] ptype = new SqlDbType[2];
            DataSet dsdrp = null;
            try
            {
                pname[0] = "tblName";
                pname[1] = "defaultValue";

                pvalue[0] = "tblWHBasedSubWH";
                pvalue[1] = WHCode;

                ptype[0] = SqlDbType.VarChar;
                ptype[1] = SqlDbType.VarChar;

                dsdrp = db.SelectRecords("spFillComboBoxMasters", pname, pvalue, ptype);
                if (dsdrp != null && dsdrp.Tables.Count > 0)
                {
                    if (dsdrp.Tables[0].Rows.Count > 0)
                    {
                        drp.Items.Clear();
                        //drpWWR.Items.Add("Select");
                        drp.DataSource = dsdrp.Tables[0];
                        drp.DataTextField = "Code";
                        drp.DataValueField = "ID";
                        drp.DataBind();
                        drp.SelectedIndex = 0;
                    }
                    else
                    {
                        drp.Items.Clear();
                        drp.Items.Add(new System.Web.UI.WebControls.ListItem("SELECT", "0"));
                        drp.SelectedIndex = 0;
                    }
                }
                else
                {
                    drp.Items.Clear();
                    drp.Items.Add(new System.Web.UI.WebControls.ListItem("SELECT", "0"));
                    drp.SelectedIndex = 0;
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                pname = null;
                pvalue = null;
                ptype = null;
                if (dsdrp != null)
                    dsdrp.Dispose();
            }
        }
        #endregion

        #region Fill Are Combo Based On WHCode,SubWHCode
        public void FillAreaBasedOnWHSubWH(string WHCode, string SubWHCode, System.Web.UI.WebControls.DropDownList drp)
        {
            string[] pname = new string[2];
            object[] pvalue = new object[2];
            SqlDbType[] ptype = new SqlDbType[2];
            DataSet dsdrp = null;

            try
            {
                pname[0] = "WHCode";
                pname[1] = "SubWHCode";

                pvalue[0] = WHCode;
                pvalue[1] = SubWHCode;

                ptype[0] = SqlDbType.VarChar;
                ptype[1] = SqlDbType.VarChar;

                dsdrp = db.SelectRecords("sp_GetWHSubWHbasedAreaCodes", pname, pvalue, ptype);
                if (dsdrp != null && dsdrp.Tables.Count > 0)
                {
                    if (dsdrp.Tables[0].Rows.Count > 0)
                    {
                        drp.Items.Clear();
                        //drpWWR.Items.Add("Select");
                        drp.DataSource = dsdrp.Tables[0];
                        drp.DataTextField = "Code";
                        drp.DataValueField = "ID";
                        drp.DataBind();
                        drp.SelectedIndex = 0;
                    }
                    else
                    {
                        drp.Items.Clear();
                        drp.Items.Add(new System.Web.UI.WebControls.ListItem("SELECT", "0"));
                        drp.SelectedIndex = 0;
                    }
                }
                else
                {
                    drp.Items.Clear();
                    drp.Items.Add(new System.Web.UI.WebControls.ListItem("SELECT", "0"));
                    drp.SelectedIndex = 0;
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                pname = null;
                pvalue = null;
                ptype = null;
                if (dsdrp != null)
                    dsdrp.Dispose();
            }
        }
        #endregion
        
        #region Return Dataset of Combobox
        public DataSet ReturnDataset(string TableName, string SelectType)
        {
            string[] pname = new string[2];
            object[] pvalue = new object[2];
            SqlDbType[] ptype = new SqlDbType[2];
            DataSet dsdrp = null;
            try
            {
                pname[0] = "tblName";
                pname[1] = "defaultValue";

                pvalue[0] = TableName;
                pvalue[1] = SelectType;

                ptype[0] = SqlDbType.VarChar;
                ptype[1] = SqlDbType.VarChar;

                dsdrp = db.SelectRecords("spFillComboBoxMasters", pname, pvalue, ptype);
                return dsdrp;
                
            }
            catch (Exception)
            {
                if (dsdrp != null)
                {
                    dsdrp.Dispose();
                }
                return null;
            }
            finally
            {
                pname = null;
                pvalue = null;
                ptype = null;
            }
        }
        #endregion
    }
}
