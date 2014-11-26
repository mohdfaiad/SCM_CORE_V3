using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;
using BAL;
using System.Web.UI;

namespace ProjectSmartCargoManager
{
    public partial class DriverMaps : System.Web.UI.Page
    {
        SQLServer db = new SQLServer(Global.GetConnectionString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string JSON = ConvertDataTabletoString();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>initialize('" + JSON + "');</SCRIPT>", false);

            }
        }


        // This method is used to convert datatable to json string
        public string ConvertDataTabletoString()
        {
            try
            {
                DataSet ds = db.SelectRecords("sp_GetMapsDataDriver");
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            DataTable dt = ds.Tables[0];
                            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                            Dictionary<string, object> row;
                            foreach (DataRow dr in dt.Rows)
                            {
                                row = new Dictionary<string, object>();
                                foreach (DataColumn col in dt.Columns)
                                {
                                    row.Add(col.ColumnName, dr[col]);
                                }
                                rows.Add(row);
                            }
                            return serializer.Serialize(rows);
                        }
                        return null;
                    }
                    return null;
                }
                return null;
            }
            catch (Exception ex)
            { return null; }


        }
    }
}
