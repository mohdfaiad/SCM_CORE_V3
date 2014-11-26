using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QID.DataAccess;
using System.Data;

namespace BAL
{
    public class PartnerBAL
    {
        #region Variables
        string constr = "";
        #endregion

        #region Constructor
        public PartnerBAL()
        {
            constr = Global.GetConnectionString();
        }
        #endregion

        #region GetPartnerTypeMaster
        public DataSet GetPartnerTypeMaster()
        {
                // Get Partner Type Master
                SQLServer da = new SQLServer(constr);
                string Query = "select 'Select' as Code,'1' as Description union SELECT Code, Description from PartnerTypeMaster order by [Description]";
                DataSet ds = da.GetDataset(Query);
                return ds;
        }
        #endregion

        #region List Partners
        public DataSet ListPartners(string PType, string PName, string PCode, string EmailID, string SITAID, string ValidFrom, string ValidTill)
        {
            SQLServer da = new SQLServer(constr);

            if (PType.Trim() == "Select") PType = null;
            if (PName == "") PName = null;
            if (PCode == "") PCode = null;
            if (EmailID == "") EmailID = null;
            if (SITAID == "") SITAID = null;
            if (ValidFrom == "") ValidFrom = null;
            if (ValidTill == "") ValidTill = null;
            //if (IsSchedule == "") IsSchedule = null;

            string[] ColumnNames = new string[7];
            SqlDbType[] DataType = new SqlDbType[7];
            Object[] Values = new object[7];

            int i = 0;

            i = 0;

            ColumnNames.SetValue("PType", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(PType, i);
            i++;

            ColumnNames.SetValue("PName", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(PName, i);
            i++;

            ColumnNames.SetValue("PCode", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(PCode, i);
            i++;

            ColumnNames.SetValue("EMailID", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(EmailID, i);
            i++;

            ColumnNames.SetValue("SITAID", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(SITAID, i);
            i++;

            ColumnNames.SetValue("ValidFrom", i);
            DataType.SetValue(SqlDbType.DateTime, i);
            Values.SetValue(ValidFrom, i);
            i++;

            ColumnNames.SetValue("ValidTill", i);
            DataType.SetValue(SqlDbType.DateTime, i);
            Values.SetValue(ValidTill, i);
            //i++;

            //ColumnNames.SetValue("IsScheduled",i);
            //DataType.SetValue(SqlDbType.Bit, i);
            //Values.SetValue(IsScheduled, i);

            DataSet ds = da.SelectRecords("spGetPartnerMasterDetails", ColumnNames, Values, DataType);
            if (ds!=null && ds.Tables.Count > 0)
            {
                return ds;
            }
            return null;
        }
        #endregion

        #region Insert Partner
        public bool InsertPartner(string PType, string PName, string PCode, string EmailID, string SITAID, string ValidFrom, string ValidTill, string User, string IsScheduled)
        {
            SQLServer da = new SQLServer(constr);

            if (PType.Trim() == "Select") PType = null;
            if (PName == "") PName = null;
            if (PCode == "") PCode = null;
            if (EmailID == "") EmailID = null;
            if (SITAID == "") SITAID = null;
            if (ValidFrom == "") ValidFrom = null;
            if (ValidTill == "") ValidTill = null;

            string[] ColumnNames = new string[10];
            SqlDbType[] DataType = new SqlDbType[10];
            Object[] Values = new object[10];

            int i = 0;

            ColumnNames.SetValue("SPType", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue("insert", i);
            i++;

            ColumnNames.SetValue("PType", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(PType, i);
            i++;

            ColumnNames.SetValue("PName", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(PName, i);
            i++;

            ColumnNames.SetValue("PCode", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(PCode, i);
            i++;

            ColumnNames.SetValue("EMailID", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(EmailID, i);
            i++;

            ColumnNames.SetValue("SITAID", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(SITAID, i);
            i++;

            ColumnNames.SetValue("ValidFrom", i);
            DataType.SetValue(SqlDbType.DateTime, i);
            Values.SetValue(ValidFrom, i);
            i++;

            ColumnNames.SetValue("ValidTill", i);
            DataType.SetValue(SqlDbType.DateTime, i);
            Values.SetValue(ValidTill, i);

            i++;
            ColumnNames.SetValue("User", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(User, i);
            i++;

            ColumnNames.SetValue("IsScheduled", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(IsScheduled, i);

            bool bl = da.ExecuteProcedure("sp_InsertUpdatePartner", ColumnNames, DataType, Values);
            return bl;
        }
        #endregion

        #region Delete Partner
        public bool DeletePartner(string PType, string PName, string PCode, string EmailID, string SITAID, string ValidFrom, string ValidTill, string User)
        {
            SQLServer da = new SQLServer(constr);

            if (PType.Trim() == "Select") PType = null;
            if (PName == "") PName = null;
            if (PCode == "") PCode = null;
            if (EmailID == "") EmailID = null;
            if (SITAID == "") SITAID = null;
            if (ValidFrom == "") ValidFrom = null;
            if (ValidTill == "") ValidTill = null;

            string[] ColumnNames = new string[9];
            SqlDbType[] DataType = new SqlDbType[9];
            Object[] Values = new object[9];

            int i = 0;

            ColumnNames.SetValue("SPType", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue("delete", i);
            i++;

            ColumnNames.SetValue("PType", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(PType, i);
            i++;

            ColumnNames.SetValue("PName", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(PName, i);
            i++;

            ColumnNames.SetValue("PCode", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(PCode, i);
            i++;

            ColumnNames.SetValue("EMailID", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(EmailID, i);
            i++;

            ColumnNames.SetValue("SITAID", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(SITAID, i);
            i++;

            ColumnNames.SetValue("ValidFrom", i);
            DataType.SetValue(SqlDbType.DateTime, i);
            Values.SetValue(ValidFrom, i);
            i++;

            ColumnNames.SetValue("ValidTill", i);
            DataType.SetValue(SqlDbType.DateTime, i);
            Values.SetValue(ValidTill, i);
            i++;

            ColumnNames.SetValue("User", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(User, i);
            

            bool bl = da.ExecuteProcedure("sp_InsertUpdatePartner", ColumnNames, DataType, Values);
            return bl;
        }
        #endregion



        public DataSet GetPartnerCargoCapacity(string p)
        {
            throw new NotImplementedException();
        }

        public DataSet GetPartnerOriginList(string p)
        {
            throw new NotImplementedException();
        }

        public int UpdatePartnerRouteDetails(object[] RouteInfo)
        {
            throw new NotImplementedException();
        }

        public int UpdatePartnerRouteDetailsForSameDate(object[] RouteInfo)
        {
            throw new NotImplementedException();
        }


    }
}
