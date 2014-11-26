using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;

namespace BAL
{
   public class ListRateCardBAL
    {


       SQLServer db = new SQLServer(Global.GetConnectionString());


       public DataSet GetRateCardList(object[] RateListInfo)
       {
           try
           {

               string[] ColumnNames = new string[2];
               SqlDbType[] DataType = new SqlDbType[2];
               Object[] Values = new object[2];
               int i = 0;
               
               i = 0;
               //0
               ColumnNames.SetValue("RateCardType", i);
               DataType.SetValue(SqlDbType.VarChar, i);
               Values.SetValue(RateListInfo.GetValue(i), i);
               i++;

               //1
               ColumnNames.SetValue("Date", i);
               DataType.SetValue(SqlDbType.DateTime, i);
               Values.SetValue(RateListInfo.GetValue(i), i);


               DataSet ds = db.SelectRecords("SP_GetRateCardList", ColumnNames, Values, DataType);
               if (ds != null)
               {
                   if (ds.Tables != null)
                   {
                       if (ds.Tables.Count > 0)
                       {
                           return (ds);
                       }
                   }
               }
           }

           catch (Exception)
           {

           }
           return (null);
       }


       public DataSet GetRateCardList_ALL()
       {
           try
           {



               DataSet ds = db.SelectRecords("SP_GetRateCardList_ALL");
               if (ds != null)
               {
                   if (ds.Tables != null)
                   {
                       if (ds.Tables.Count > 0)
                       {
                           return (ds);
                          
                       }
                   }
               }
           }

           catch (Exception)
           {

           }
           return (null);
       }
    }
}
