using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using BAL;
using QID.DataAccess;

namespace ProjectSmartCargoManager
{
    public partial class Config : System.Web.UI.Page
    {
        BALConfig objBal = new BALConfig();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetCurrencyCode();
               //GetDesigCode();
                GetExchRateType();
                
            }

        }

        # region Get Currency Code List
        private void GetCurrencyCode()
        {
            try
            {
                DataSet ds = objBal.GetCurrencyCodes();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlCurrency.DataSource = ds;
                            ddlCurrency.DataMember = ds.Tables[0].TableName;
                            ddlCurrency.DataValueField = ds.Tables[0].Columns["Code"].ColumnName;

                            ddlCurrency.DataTextField = ds.Tables[0].Columns["CurrencyCode"].ColumnName;
                            ddlCurrency.DataBind();
                            ddlCurrency.Items.Insert(0, "Select Currency");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        # endregion GetCurrencyCode List

        #region Get Designator Code
        private void GetDesigCode()
        {
            try
            {
                DataSet ds = objBal.GetDesigCodes();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            listPrefix.DataSource = ds;
                            listPrefix.DataMember = ds.Tables[0].TableName;
                            listPrefix.DataValueField = ds.Tables[0].Columns["AirlineCode"].ColumnName;

                            listPrefix.DataTextField = ds.Tables[0].Columns["AirlineCode"].ColumnName;
                            listPrefix.DataBind();

                            listCnote.DataSource = ds;
                            listCnote.DataMember = ds.Tables[1].TableName;
                            listCnote.DataValueField = ds.Tables[1].Columns["CNoteType"].ColumnName;

                            listCnote.DataTextField = ds.Tables[1].Columns["CNoteType"].ColumnName;
                            listCnote.DataBind();


                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region Add Prefix-OLD
        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    int count = listPrefix.Items.Count;

        //    try
        //    {

        //        string[] values = prefixhdn.Value.Split(',');
        //        for (int i = 0; i < values.Length; i++)
        //        {
        //            if (values[i] != "")
        //            {
        //                listPrefix.Items.Add(values[i]);
        //            }
        //        }
        //        if (txtPrefix.Text == "" || ddlCurrency.SelectedIndex == 0||listPrefix.Items.Count<=0)
        //        {
        //            lblStatus.Text = "Please Enter Both Values";
        //            lblStatus.ForeColor = Color.Red;
        //            return;
        //        }

        //        #region Prepare Parameters
        //        for (int j = 0; j < listPrefix.Items.Count; j++)
        //        {
        //            object[] Prefix = new object[3];
        //            int i = 0;

        //            //0
        //            Prefix.SetValue(listPrefix.Items[j].Value, i);
        //            i++;

        //            //1
        //            Prefix.SetValue(txtPrefix.Text, i);
        //            i++;

        //            //2
        //            Prefix.SetValue(ddlCurrency.SelectedValue.ToString(), i);
        //            i++;


        //        #endregion Prepare Parameters

        //            int ID = 0;
        //            ID = objBal.AddPrefix(Prefix);

        //            if (ID >= 0)
        //            {
        //                lblStatus.Text = "Record Added Successfully";
        //                lblStatus.ForeColor = Color.Green;
        //                //txtDesigCode.Text = txtPrefix.Text = string.Empty;
        //                //ddlCurrency.SelectedIndex = 0;
        //                //GetDesigCode();
        //            }
        //            else
        //            {
        //                lblStatus.ForeColor = Color.Red;
        //                lblStatus.Text = "Pefix Insertion Failed..";
        //            }
        //        }
        //        txtDesigCode.Text = txtPrefix.Text = string.Empty;
        //        ddlCurrency.SelectedIndex = 0;
        //        listPrefix.Items.Clear();
        //        prefixhdn.Value = null;
        //        }
               
        //    catch (Exception ex)
        //    {

        //    }

        //}
       #endregion 

        #region Add CNOTE-Old
        //protected void btnCnoteSave_Click(object sender, EventArgs e)
        //{
        //    string[] values = hdnCnote.Value.Split(',');
        //    for (int i = 0; i < values.Length; i++)
        //    {
        //        if (values[i] != "")
        //        {
        //            listCnote.Items.Add(values[i]);
        //        }
        //    }
        //    try
        //    {
        //        if (listCnote.Items.Count <= 0)
        //        {
        //            lblCnote.Text = "Please Enter Both Values";
        //            lblCnote.ForeColor = Color.Red;
        //            return;
        //        }

        //        #region Prepare Parameters
        //        for (int j = 0; j < listCnote.Items.Count; j++)
        //        {
        //            object[] Cnote = new object[2];
        //            int i = 0;

        //            //0
        //            Cnote.SetValue(listCnote.Items[j].Value, i);
        //            i++;

        //            //1
        //            Cnote.SetValue(ddlCnote.SelectedItem.Text, i);
        //            i++;

        //        #endregion Prepare Parameters

        //            int ID = 0;
        //            ID = objBal.AddCnote(Cnote);
        //            if (ID >= 0)
        //            {
        //                lblCnote.Text = "Record Added Successfully";
        //                lblCnote.ForeColor = Color.Green;
        //            }
        //            else
        //            {
        //                lblStatus.ForeColor = Color.Red;
        //                lblStatus.Text = "Record Failed..";
        //            }
        //        }
        //        txtCnoteType.Text = string.Empty;
        //        listCnote.Items.Clear();
        //        hdnCnote.Value = null;

        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}
        #endregion 

        #region Get Exch Rate
        private void GetExchRateType()
        {
            try
            {
                DataSet ds = objBal.GetExchrate();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            listExchng.DataSource = ds;
                            listExchng.DataMember = ds.Tables[0].TableName;
                            listExchng.DataValueField = ds.Tables[0].Columns["Currencytype"].ColumnName;

                            listExchng.DataTextField = ds.Tables[0].Columns["Currencytype"].ColumnName;
                            listExchng.DataBind();


                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
        
        #region Remove Exch Rate Type
        protected void btnExchRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (listExchng.SelectedIndex <= 0)
                {
                    lblExch.ForeColor = Color.Red;
                lblExch.Text = "Select value";
                }
                if (listExchng.SelectedIndex > 0)
                {
                    #region Prepare Parameters
                    object[] Prefix = new object[1];
                    int i = 0;

                    //0
                    string listitem = listExchng.SelectedItem.Text;
                    Prefix.SetValue(listitem, i);
                    i++;


                    #endregion Prepare Parameters

                    int ID = 0;
                    ID = objBal.RemoveExchRate(Prefix);
                    if (ID >= 0)
                    {
                        lblExch.Text = "Record Removed Successfully";
                        lblExch.ForeColor = Color.Green;
                        txtExchng.Text = string.Empty;
                        GetExchRateType();
                        lblCnote.Text = lblStatus.Text = string.Empty;
                    }
                    else
                    {
                        lblExch.ForeColor = Color.Red;
                        lblExch.Text = "Record Deletion Failed Failed..";
                    }
                }
                else
                {
                    lblExch.Text = "Select record";
                    lblExch.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {

            }
        }
      
        #endregion

        #region Save All Button
        protected void btnSaveAll_Click(object sender, EventArgs e)
        {
            lblStatus.Text = lblCnote.Text = string.Empty;

            #region Prefix
            string[] values = prefixhdn.Value.Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != "")
                {
                    listPrefix.Items.Add(values[i]);
                }
            }
            if (txtPrefix.Text == "" && ddlCurrency.SelectedIndex == 0 && listPrefix.Items.Count <= 0)
            {
                lblStatus.Text = string.Empty;
            }
            else if(txtPrefix.Text != "" && ddlCurrency.SelectedIndex >0 && listPrefix.Items.Count > 0)
            {
            int count = listPrefix.Items.Count;

            try
            {

                #region Prepare Parameters
                for (int j = 0; j < listPrefix.Items.Count; j++)
                {
                    object[] Prefix = new object[3];
                    int i = 0;

                    //0
                    Prefix.SetValue(listPrefix.Items[j].Value, i);
                    i++;

                    //1
                    Prefix.SetValue(txtPrefix.Text, i);
                    i++;

                    //2
                    Prefix.SetValue(ddlCurrency.SelectedValue.ToString(), i);
                    i++;

                #endregion Prepare Parameters

                    int ID = 0;
                    ID = objBal.AddPrefix(Prefix);

                    if (ID >= 0)
                    {
                        lblStatus.Text = "Record Added Successfully";
                        lblStatus.ForeColor = Color.Green;
                        //txtDesigCode.Text = txtPrefix.Text = string.Empty;
                        //ddlCurrency.SelectedIndex = 0;
                        //GetDesigCode();
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Pefix Insertion Failed..";
                    }
                }
                txtDesigCode.Text = txtPrefix.Text = string.Empty;
                ddlCurrency.SelectedIndex = 0;
                listPrefix.Items.Clear();
                prefixhdn.Value = null;
            }

            catch (Exception ex)
            {

            }
        }
            else
            {
                lblStatus.Text = "Please Select All Fields";
                lblStatus.ForeColor = Color.Red;
            }

            #endregion Prefix

            #region CNOTE

            string[] value = hdnCnote.Value.Split(',');
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] != "")
                {
                    listCnote.Items.Add(value[i]);
                }
            }
            if (listCnote.Items.Count <= 0)
            {
                lblCnote.Text = string.Empty;

            }
            else
            {
                try
                {

                    #region Prepare Parameters
                    for (int j = 0; j < listCnote.Items.Count; j++)
                    {
                        object[] Cnote = new object[2];
                        int i = 0;

                        //0
                        Cnote.SetValue(listCnote.Items[j].Value, i);
                        i++;

                        //1
                        Cnote.SetValue(ddlCnote.SelectedItem.Text, i);
                        i++;

                    #endregion Prepare Parameters

                        int ID = 0;
                        ID = objBal.AddCnote(Cnote);
                        if (ID >= 0)
                        {
                            lblCnote.Text = "Record Added Successfully";
                            lblCnote.ForeColor = Color.Green;
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Record Failed..";
                        }
                    }
                    txtCnoteType.Text = string.Empty;
                    listCnote.Items.Clear();
                    hdnCnote.Value = null;

                }
                catch (Exception ex)
                { }
            }
            #endregion CNOTE
        }
        #endregion Save All Button

        #region add Exch Rate type
        protected void btnExchAdd_Click(object sender, EventArgs e)
        {
            int count = listExchng.Items.Count;
            foreach (ListItem li in listExchng.Items)
            { string listitemtext = li.Text; }
            try
            {
                if (txtExchng.Text != "")
                {
                    #region Prepare Parameters
                    object[] Prefix = new object[1];
                    int i = 0;

                    //0
                    Prefix.SetValue(txtExchng.Text.ToUpper(), i);
                    i++;


                    #endregion Prepare Parameters

                    int ID = 0;
                    ID = objBal.AddExchRate(Prefix);
                    if (ID >= 0)
                    {
                        lblExch.Text = "Record Added Successfully";
                        lblExch.ForeColor = Color.Green;
                        txtExchng.Text = string.Empty;
                        GetExchRateType();
                        lblCnote.Text = lblStatus.Text = string.Empty;
                    }
                    else
                    {
                        lblExch.ForeColor = Color.Red;
                        lblExch.Text = "Insertion Failed..";
                    }
                }
                else
                {
                    lblExch.Text = "Text Cannot be blank";
                    lblExch.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}
       
   