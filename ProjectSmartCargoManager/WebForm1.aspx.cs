using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Session["RoleID"] != null)
                {

                   // HideRoleID.Value = Session["RoleID"].ToString();
                    int roleid = Convert.ToInt32(Session["RoleID"].ToString());
                    txtSearchLink1_AutoCompleteExtender.ContextKey = roleid.ToString();
                }
            }

        }
        #region function
        //SELECT FlightID from dbo.AirlineSchedule where FlightID like '"+ prefixText +"%' or FlightID like '"+ prefixText +"%'


        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string[] GetMenu(string prefixText, int count, string contextKey)
        {
            string con = Global.GetConnectionString();

            // SqlConnection con = new SqlConnection("connection string");
            //SELECT FlightID from dbo.AirlineSchedule where FlightID like '"+ prefixText +"%' or FlightID like '"+ prefixText +"%'
            //int i=int.Parse(Session["RoleID"].ToString());
            SqlDataAdapter dad = new SqlDataAdapter("select Menu+'\r\n' as 'Menu' from UserRoleMenuAccess where Menu like '" + prefixText + "%' or Shortcut like '" + prefixText + "%'", con);
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
    }
}
