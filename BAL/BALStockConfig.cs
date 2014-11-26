using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using QID.DataAccess;

namespace BAL
{
    public class BALStockConfig
    {
        #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        #endregion

        #region Constructor
        public BALStockConfig()
        {
            constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        }
        #endregion

        #region spGetStockConfigurationDetailsWithDropdownData
        public DataSet GetStockConfigurationDetailsWithDropdownData()
        {
            try
            {
                DataSet ds = new DataSet();
                SQLServer da = new SQLServer(constr);
                ds = da.SelectRecords("spGetStockConfigurationDetailsWithDropdownData");
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        #endregion

        #region Saving Data in DB
        public bool SaveStockDetails(string ID, string Selection, string StationID, string ULDType, string MinAvlQty, string EmailID, string ULDStatusID)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                bool flag = false;
                string[] QueryNames = new string[7];
                object[] QueryValues = new object[7];
                SqlDbType[] QueryTypes = new SqlDbType[7];

                QueryNames[0] = "ID";
                QueryNames[1] = "Selection";
                QueryNames[2] = "StationID";
                QueryNames[3] = "ULDType";
                QueryNames[4] = "MinAvlQty";
                QueryNames[5] = "EmailID";
                QueryNames[6] = "ULDStatusID";

                QueryTypes[0] = SqlDbType.BigInt;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.BigInt;
                QueryTypes[3] = SqlDbType.VarChar;
                QueryTypes[4] = SqlDbType.Int;
                QueryTypes[5] = SqlDbType.VarChar;
                QueryTypes[6] = SqlDbType.TinyInt;


                QueryValues[0] = ID;
                QueryValues[1] = Selection;
                QueryValues[2] = StationID;
                QueryValues[3] = ULDType;
                QueryValues[4] = MinAvlQty;
                QueryValues[5] = EmailID;
                QueryValues[6] = ULDStatusID;

                flag = da.UpdateData("spUpdateStockConfigurationData", QueryNames, QueryTypes, QueryValues);
                return flag;

            }
            catch (Exception ex)
            { }
            return false;
        }
        #endregion

        #region spGetDropdownDetailsforStockReport
        public DataSet GetDropdownDetailsforStockReport()
        {
            try
            {
                DataSet ds = new DataSet();
                SQLServer da = new SQLServer(constr);
                ds = da.SelectRecords("spGetDropdownDetailsforStockReport");
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        #endregion

        #region spGetStockList
        public DataSet GetStockList(string StationID, string ULDTypeID, string ULDStatusID)
        {
            try
            {
                DataSet ds = new DataSet();
                SQLServer da = new SQLServer(constr);

                string[] ColumnNames = new string[3];
                SqlDbType[] DataType = new SqlDbType[3];
                Object[] Values = new object[3];

                ColumnNames[0] = "StationID";
                DataType[0] = SqlDbType.VarChar;
                Values[0] = StationID;

                ColumnNames[1] = "ULDTypeID";
                DataType[1] = SqlDbType.VarChar;
                Values[1] = ULDTypeID;

                ColumnNames[2] = "ULDStatusID";
                DataType[2] = SqlDbType.VarChar;
                Values[2] = ULDStatusID;

                //ds = da.SelectRecords("spGetStockList", "StationID", StationID, SqlDbType.Int);
                ds = da.SelectRecords("spGetStockList", ColumnNames, Values, DataType);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        #endregion
    }
}
