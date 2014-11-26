using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using QID.DataAccess;
using System.Data;

namespace ProjectSmartCargoManager.WebServices
{
    /// <summary>
    /// Summary description for WSIntegration
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WSIntegration : System.Web.Services.WebService
    {
        [WebMethod]
        public bool StoreBarCodeDetails(string BarCodeId, decimal GrossWt, decimal Length, decimal Breadth, decimal Height,
                string Units, string HeartBeat, string Location, string DeviceId, DateTime CreatedOn)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            string[] colNames = new string[11];
            string[] values = new string[11];
            SqlDbType[] dataTypes = new SqlDbType[11];
            int i = 0;

            colNames.SetValue("BarCodeId", i);
            values.SetValue(BarCodeId, i);
            dataTypes.SetValue(SqlDbType.VarChar, i);
            i++;

            colNames.SetValue("GrossWt", i);
            values.SetValue(GrossWt.ToString(), i);
            dataTypes.SetValue(SqlDbType.Decimal, i);
            i++;

            colNames.SetValue("Length", i);
            values.SetValue(Length.ToString(), i);
            dataTypes.SetValue(SqlDbType.Decimal, i);
            i++;

            colNames.SetValue("Breadth", i);
            values.SetValue(Breadth.ToString(), i);
            dataTypes.SetValue(SqlDbType.Decimal, i);
            i++;

            colNames.SetValue("Height", i);
            values.SetValue(Height.ToString(), i);
            dataTypes.SetValue(SqlDbType.Decimal, i);
            i++;

            colNames.SetValue("Units", i);
            values.SetValue(Units, i);
            dataTypes.SetValue(SqlDbType.VarChar, i);
            i++;

            colNames.SetValue("HeartBeat", i);
            values.SetValue(HeartBeat, i);
            dataTypes.SetValue(SqlDbType.VarChar, i);
            i++;

            colNames.SetValue("Location", i);
            values.SetValue(Location, i);
            dataTypes.SetValue(SqlDbType.VarChar, i);
            i++;

            colNames.SetValue("UserId", i);
            values.SetValue(DeviceId, i);
            dataTypes.SetValue(SqlDbType.VarChar, i);
            i++;

            colNames.SetValue("CreatedOn", i);
            values.SetValue(CreatedOn.ToString("MM/dd/yyyy HH:mm"), i);
            dataTypes.SetValue(SqlDbType.DateTime, i);
            i++;

            colNames.SetValue("IsBarCode", i);
            values.SetValue("true", i);
            dataTypes.SetValue(SqlDbType.Bit, i);

            bool blnResult = da.ExecuteProcedure("sp_StoreBarCodeDetails", colNames, dataTypes, values);

            return blnResult;
        }

        [WebMethod]
        public bool StoreLocationDetails(string HeartBeat, string Location, string DeviceId, DateTime HBTime)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            string[] colNames = new string[5];
            string[] values = new string[5];
            SqlDbType[] dataTypes = new SqlDbType[5];
            int i = 0;

            colNames.SetValue("HeartBeat", i);
            values.SetValue(HeartBeat, i);
            dataTypes.SetValue(SqlDbType.VarChar, i);
            i++;

            colNames.SetValue("Location", i);
            values.SetValue(Location, i);
            dataTypes.SetValue(SqlDbType.VarChar, i);
            i++;

            colNames.SetValue("UserId", i);
            values.SetValue(DeviceId, i);
            dataTypes.SetValue(SqlDbType.VarChar, i);
            i++;

            colNames.SetValue("CreatedOn", i);
            values.SetValue(HBTime.ToString("MM/dd/yyyy HH:mm"), i);
            dataTypes.SetValue(SqlDbType.DateTime, i);
            i++;

            colNames.SetValue("IsBarCode", i);
            values.SetValue("false", i);
            dataTypes.SetValue(SqlDbType.Bit, i);

            bool blnResult = da.ExecuteProcedure("sp_StoreBarCodeDetails", colNames, dataTypes, values);

            return blnResult;
        }
    }
}
