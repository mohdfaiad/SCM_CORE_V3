using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using QID.DataAccess;

namespace BAL
{
   public class BALInstanceConfiguration
   {
       #region Variables

       string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();

       #endregion

       #region Save Config Details

       public DataSet SaveConfigDetails(object[] paramvalue)
       {
           DataSet result = new DataSet();

           try
           {
               SQLServer da = new SQLServer(constr);

               string[] paramname = new string[12];
               SqlDbType[] paramtype = new SqlDbType[12];

               paramname[0] = "ClientName";
               paramname[1] = "ClientAddress";
               paramname[2] = "EmailID";
               paramname[3] = "PhoneNum";
               paramname[4] = "MobileNum";
               paramname[5] = "FaxNum";
               paramname[6] = "RegOfficeAddress";
               paramname[7] = "RegOfficePhoneNum";
               paramname[8] = "ContactURL";
               paramname[9] = "CustomerSupportEmail";
               paramname[10] = "CustomerSupportPhone";
               paramname[11] = "ClientID";

               paramtype[0] = SqlDbType.VarChar;
               paramtype[1] = SqlDbType.VarChar;
               paramtype[2] = SqlDbType.VarChar;
               paramtype[3] = SqlDbType.VarChar;
               paramtype[4] = SqlDbType.VarChar;
               paramtype[5] = SqlDbType.VarChar;
               paramtype[6] = SqlDbType.VarChar;
               paramtype[7] = SqlDbType.VarChar;
               paramtype[8] = SqlDbType.VarChar;
               paramtype[9] = SqlDbType.VarChar;
               paramtype[10] = SqlDbType.VarChar;
               paramtype[11] = SqlDbType.VarChar;

               result = da.SelectRecords("SP_SaveInstanceConfig",paramname,paramvalue,paramtype);

               return result;
           }
           catch(Exception ex)
           {
               return null;
           }
       }

       #endregion

       #region List Config Details
       public DataSet ListConfigDetails()
       {
           DataSet dsData = new DataSet();
           try
           {
               SQLServer da = new SQLServer(constr);
               dsData = da.SelectRecords("SP_ListInstanceConfig");
               return dsData;
           }
           catch (Exception ex)
           {
               return null;
           }
       }

       #endregion

   }
}
