using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;



namespace BAL
{
    public class BALClsFillCombo
    {
        //Database db = null;
        //SQLServer db = new SQLServer("constr");
        SQLServer db = new SQLServer("constr");
        string constr = "";
        DataSet res;
        DataSet ds;

        public BALClsFillCombo()
        {
            constr = Global.GetConnectionString();
        }

        #region FILL ALL COMBOBOX
        public void FillAllComboBoxes(string TableName, string SelectType, System.Web.UI.WebControls.DropDownList drp)
        {
            try
            {
                DataSet res = new DataSet();
                string[] pname = new string[2];
                pname[0] = "tblName";
                pname[1] = "defaultValue";

                object[] pvalue = new object[2];
                pvalue[0] = TableName;
                pvalue[1] = SelectType;

                SqlDbType[] ptype = new SqlDbType[2];
                ptype[0] = SqlDbType.VarChar;
                ptype[1] = SqlDbType.VarChar;

                res= db.SelectRecords("spFillComboBoxMasters", pname, pvalue, ptype);

                if (res!= null && res.Tables.Count > 0)
                {
                    if (res.Tables[0].Rows.Count > 0)
                    {
                        drp.Items.Clear();
                        //drpWWR.Items.Add("Select");
                        drp.DataSource = res.Tables[0];
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
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Fill SubWH Combo Based On WHCode
        public void FillSubWHBasedOnWH(string WHCode, System.Web.UI.WebControls.DropDownList drp)
        {
            try
            {
                string[] pname = new string[2];
                pname[0] = "tblName";
                pname[1] = "defaultValue";

                object[] pvalue = new object[2];
                pvalue[0] = "tblWHBasedSubWH";
                pvalue[1] = WHCode;

                SqlDbType[] ptype = new SqlDbType[2];
                ptype[0] = SqlDbType.VarChar;
                ptype[1] = SqlDbType.VarChar;

                DataSet res = db.SelectRecords("spFillComboBoxMasters", pname, pvalue, ptype);
                if (res != null && res.Tables.Count > 0)
                {
                    if (res.Tables[0].Rows.Count > 0)
                    {
                        drp.Items.Clear();
                        //drpWWR.Items.Add("Select");
                        drp.DataSource = res.Tables[0];
                        drp.DataTextField = "Code";
                        drp.DataValueField = "ID";
                        drp.DataBind();
                        drp.SelectedIndex = 0;
                    }
                    else
                    {
                        drp.Items.Clear();
                        drp.Items.Add(new System.Web.UI.WebControls.ListItem("SELECT","0"));
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
            catch (Exception Ex)
            { 

            }
        }
        #endregion

        #region Fill Are Combo Based On WHCode,SubWHCode
        public void FillAreaBasedOnWHSubWH(string WHCode, string SubWHCode, System.Web.UI.WebControls.DropDownList drp)
        {
            try
            {
                string[] pname = new string[2];
                pname[0] = "WHCode";
                pname[1] = "SubWHCode";

                object[] pvalue = new object[2];
                pvalue[0] = WHCode;
                pvalue[1] = SubWHCode;

                SqlDbType[] ptype = new SqlDbType[2];
                ptype[0] = SqlDbType.VarChar;
                ptype[1] = SqlDbType.VarChar;

                DataSet res = db.SelectRecords("sp_GetWHSubWHbasedAreaCodes", pname, pvalue, ptype);
                if (res != null && res.Tables.Count > 0)
                {
                    if (res.Tables[0].Rows.Count > 0)
                    {
                        drp.Items.Clear();
                        //drpWWR.Items.Add("Select");
                        drp.DataSource = res.Tables[0];
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
            catch (Exception Ex)
            {

            }
        }
        #endregion

        #region Return Dataset of Combobox
        public DataSet ReturnDataset(string TableName, string SelectType)
        { 
            try
            {
                string[] pname = new string[2];
                pname[0] = "tblName";
                pname[1] = "defaultValue";

                object[] pvalue = new object[2];
                pvalue[0] = TableName;
                pvalue[1] = SelectType;

                SqlDbType[] ptype = new SqlDbType[2];
                ptype[0] = SqlDbType.VarChar;
                ptype[1] = SqlDbType.VarChar;

                DataSet res= db.SelectRecords("spFillComboBoxMasters", pname, pvalue, ptype);
                return res;
                //if (res!= null && res.Tables.Count > 0)
                //{
                //    if (res.Tables[0].Rows.Count > 0)
                //    {
                //        drp.Items.Clear();
                //        //drpWWR.Items.Add("Select");
                //        drp.DataSource = res.Tables[0];
                //        drp.DataTextField = "Code";
                //        drp.DataValueField = "ID";
                //        drp.DataBind();
                //        drp.SelectedIndex = 0;
                //    }
                //    else
                //    {
                //        drp.Items.Clear();
                //        drp.SelectedIndex = 0;
                //    }
                //}
                //else
                //{
                //    drp.Items.Clear();
                //    drp.Items.Add(SelectType);
                //    drp.SelectedIndex = 0;
                //}
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

    }
}
