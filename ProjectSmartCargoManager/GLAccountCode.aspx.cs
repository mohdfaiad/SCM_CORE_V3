using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using BAL;
using System.Data.Sql;
using System.Data.SqlClient;
using QID.DataAccess;

namespace ProjectSmartCargoManager
{
    public partial class GLAccountCode : System.Web.UI.Page
    {
        DataSet dsSlabs = new DataSet();
        SQLServer da = new SQLServer(Global.GetConnectionString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                LoadGLDropDown();
            }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetGLCode(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string");
            SqlDataAdapter dad = new SqlDataAdapter("SELECT distinct GLAccountCode FROM  GLAccountMaster", con);
            DataSet ds = new DataSet();
            dad.Fill(ds);
            List<string> list = new List<string>(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(dr[0].ToString());
            }
            return list.ToArray();
        }

        
        #region Load DropDowns
        public void LoadGLDropDown()
        {
            try
            {
                for (int i = 1; i < 100; i++) 
                {
                    //ddlAirport.Items.Add(new ListItem("GL"+i.ToString().PadLeft(3,'0')));
                    //ddlfreight.Items.Add(new ListItem("GL" + i.ToString().PadLeft(3, '0')));
                    //ddlOCDC.Items.Add(new ListItem("GL" + i.ToString().PadLeft(3, '0')));
                    //ddlOCDA.Items.Add(new ListItem("GL" + i.ToString().PadLeft(3, '0')));
                    //ddlSecuritySur.Items.Add(new ListItem("GL" + i.ToString().PadLeft(3, '0')));
                    //ddlFuelSur.Items.Add(new ListItem("GL" + i.ToString().PadLeft(3, '0')));
                    //ddlCartage.Items.Add(new ListItem("GL" + i.ToString().PadLeft(3, '0')));
                    //ddlScreenig.Items.Add(new ListItem("GL" + i.ToString().PadLeft(3, '0')));
                    //ddlMiscCharge.Items.Add(new ListItem("GL" + i.ToString().PadLeft(3, '0')));
                    //ddlComission.Items.Add(new ListItem("GL" + i.ToString().PadLeft(3, '0')));
                    //ddlDiscount.Items.Add(new ListItem("GL" + i.ToString().PadLeft(3, '0')));
                    //ddlServiceTax.Items.Add(new ListItem("GL" + i.ToString().PadLeft(3, '0')));
                    //ddlTaxCommission.Items.Add(new ListItem("GL" + i.ToString().PadLeft(3, '0')));
                    //ddlTaxDiscount.Items.Add(new ListItem("GL" + i.ToString().PadLeft(3, '0')));
                    //ddlTaxFreight.Items.Add(new ListItem("GL" + i.ToString().PadLeft(3, '0')));
                    //ddlTDS.Items.Add(new ListItem("GL" + i.ToString().PadLeft(3, '0')));

                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Save Method
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SAVEGLCode("Airport", ddlAirport.Text);
                SAVEGLCode("Freight", ddlfreight.Text);
                SAVEGLCode("OCDC", ddlOCDC.Text);
                SAVEGLCode("OCDA", ddlOCDA.Text);
                SAVEGLCode("Security", ddlSecuritySur.Text);
                SAVEGLCode("Fuel", ddlFuelSur.Text);
                SAVEGLCode("Cartage", ddlCartage.Text);
                SAVEGLCode("Screening", ddlScreenig.Text);
                SAVEGLCode("Misc", ddlMiscCharge.Text);
                SAVEGLCode("Commission", ddlComission.Text);
                SAVEGLCode("Dsicount", ddlDiscount.Text);
                SAVEGLCode("Service Tax", ddlServiceTax.Text);
                SAVEGLCode("TaxOnCommission", ddlTaxCommission.Text);
                SAVEGLCode("TaxOnDiscount", ddlTaxDiscount.Text);
                SAVEGLCode("TaxOnFreight", ddlTaxFreight.Text);
                SAVEGLCode("TDS", ddlTDS.Text);                
            }
            catch(Exception ex) { }
        }
        #endregion

        #region DBCall Save
        private void SAVEGLCode(string Desc,string Code) 
        {
            try 
            {
                string[] PName = new string[] 
                {
                   "GLCode",
	               "Description"
                };

                SqlDbType[] PType = new SqlDbType[] 
                {
                    SqlDbType.VarChar,
                    SqlDbType.VarChar
                };

                object[] PValue = new object[]
                {     
                    Code,
                    Desc
                };
                if (da.ExecuteProcedure("spAddGLcodes", PName, PType, PValue))
                { }
            }
            catch (Exception ex) { }
        }
        #endregion

        protected void btnclear_Click(object sender, EventArgs e)
        {
            try 
            {
                ddlAirport.Text = "";
                ddlfreight.Text = "";
                ddlOCDC.Text = "";
                ddlOCDA.Text = "";
                ddlSecuritySur.Text = "";
                ddlFuelSur.Text = "";
                ddlCartage.Text = "";
                ddlScreenig.Text = "";
                ddlMiscCharge.Text = "";
                ddlComission.Text = "";
                ddlDiscount.Text = "";
                ddlServiceTax.Text = "";
                ddlTaxCommission.Text = "";
                ddlTaxDiscount.Text = "";
                ddlTaxFreight.Text = "";
                ddlTDS.Text = "";                
            }
            catch (Exception ex) { }
        }
    }
}
