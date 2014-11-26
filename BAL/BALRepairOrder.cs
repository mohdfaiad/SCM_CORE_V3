using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;

namespace BAL
{
   public class BALRepairOrder
   {
       #region Variables
       string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
       SQLServer db = new SQLServer(Global.GetConnectionString());
       #endregion

       #region Save ULD List
       public DataSet SaveULDList(string ULDNumber, string PartNumber, string PartSrNumber, string NatureOfDamage, DateTime UpdatedOn, string UpdatedBy, string ROId)
       {
          // bool flag = false;
           DataSet ds = new DataSet();

           try
           {
               string[] Pname = new string[7];
               Pname[0] = "ULDNumber";
               Pname[1] = "PartNumber";
               Pname[2] = "PartSrNumber";
               Pname[3] = "NatureOfDamage";
               Pname[4] = "UpdatedOn";
               Pname[5] = "UpdatedBy";
               Pname[6] = "ROId";

               object[] Pvalue = new object[7];
               Pvalue[0] = ULDNumber;
               Pvalue[1] = PartNumber;
               Pvalue[2] = PartSrNumber;
               Pvalue[3] = NatureOfDamage;
               Pvalue[4] = UpdatedOn;
               Pvalue[5] = UpdatedBy;
               Pvalue[6] = ROId;

               SqlDbType[] Ptype = new SqlDbType[7];
               Ptype[0] = SqlDbType.VarChar;
               Ptype[1] = SqlDbType.VarChar;
               Ptype[2] = SqlDbType.VarChar;
               Ptype[3] = SqlDbType.VarChar;
               Ptype[4] = SqlDbType.DateTime;
               Ptype[5] = SqlDbType.VarChar;
               Ptype[6] = SqlDbType.VarChar;

               //flag = db.InsertData("Sp_AddULDForRepairOrder", Pname, Ptype, Pvalue);
               ds = db.SelectRecords("Sp_AddULDForRepairOrder", Pname, Pvalue, Ptype);
              // return true;
               if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
               {
                   return (ds);
               }

           }
           catch (Exception ex)
           {
               //return false;
           }
           return (null);
       }

       #endregion

       #region Repair Order Details
       public DataSet SetRepairDetails(string RONo, string RoDate, string Station, string ExpDeliveryDate, DateTime CreatedOn, string CreatedBy, DateTime UpdatedOn, string UpdatedBy)
       {
           DataSet ds = new DataSet();

           try
           {
               string[] Pname = new string[8];
               Pname[0] = "RONo";
               Pname[1] = "RoDate";
               Pname[2] = "Station";
               Pname[3] = "ExpDeliveryDate";
               Pname[4] = "CreatedOn";
               Pname[5] = "CreatedBy";
               Pname[6] = "UpdatedOn";
               Pname[7] = "UpdatedBy";

               object[] Pvalue = new object[8];
               Pvalue[0] = RONo;
               Pvalue[1] = RoDate;
               Pvalue[2] = Station;
               Pvalue[3] = ExpDeliveryDate;
               Pvalue[4] = CreatedOn;
               Pvalue[5] = CreatedBy;
               Pvalue[6] = UpdatedOn;
               Pvalue[7] = UpdatedBy;

               SqlDbType[] Ptype = new SqlDbType[8];
               Ptype[0] = SqlDbType.VarChar;
               Ptype[1] = SqlDbType.VarChar;
               Ptype[2] = SqlDbType.VarChar;
               Ptype[3] = SqlDbType.VarChar;
               Ptype[4] = SqlDbType.DateTime;
               Ptype[5] = SqlDbType.VarChar;
               Ptype[6] = SqlDbType.DateTime;
               Ptype[7] = SqlDbType.VarChar;

               ds = db.SelectRecords("Sp_RepairOrderDetails", Pname, Pvalue, Ptype);

               if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
               {
                   return (ds);
               }

           }
           catch (Exception ex)
           {
             
           }
           return (null);
       }

       #endregion 

       #region Delete ULD List
       public bool DeleteULDDetails(string RepairId) 
       {
           bool flag = false;
           try
           {
               string[] Pname = new string[7];
               Pname[0] = "RepairId";

               object[] Pvalue = new object[7];
               Pvalue[0] = RepairId;

               SqlDbType[] Ptype = new SqlDbType[7];
               Ptype[0] = SqlDbType.VarChar;

               flag = db.InsertData("Sp_DeleteULDForRepairOrder", Pname, Ptype, Pvalue);
               return true;


           }
           catch (Exception ex)
           {
               return false;
           }
       }


       #endregion

       #region Check ULD Use Status

       public DataSet CheckULDUSeStatus(string ULDNumber)
       {
           try
           {
               DataSet ds = db.SelectRecords("sp_CheckULDUseStatus", "ULDNumber", ULDNumber, SqlDbType.VarChar);
               if (ds != null)
               {
                   if (ds.Tables.Count > 0)
                   {
                       if (ds.Tables[0].Rows.Count > 0)
                       {
                           return ds;
                       }
                       else
                           return null;
                   }
                   else
                       return null;
               }
               else
                   return null;

           }
           catch (Exception ex)
           { return null; }
       }
       #endregion

       #region List Repair Order

       public DataSet GetROList(string RONo, string RoDate, string Station, string ExpDeliveryDate)
       {
           DataSet dsList = new DataSet();
           try
           {
               string[] Pname = new string[4];
               Pname[0] = "RONo";
               Pname[1] = "RoDate";
               Pname[2] = "Station";
               Pname[3] = "ExpDeliveryDate";

               object[] Pvalue = new object[4];
               Pvalue[0] = RONo;
               Pvalue[1] = RoDate;
               Pvalue[2] = Station;
               Pvalue[3] = ExpDeliveryDate;

               SqlDbType[] Ptype = new SqlDbType[4];
               Ptype[0] = SqlDbType.VarChar;
               Ptype[1] = SqlDbType.VarChar;
               Ptype[2] = SqlDbType.VarChar;
               Ptype[3] = SqlDbType.VarChar;

               dsList = db.SelectRecords("Sp_ListRepairOrder", Pname, Pvalue, Ptype);

               if (dsList != null && dsList.Tables.Count > 0 && dsList.Tables[0].Rows.Count > 0)
               {
                   return (dsList);
               }

           }
           catch (Exception ex)
           {

           }
           return null;
       }

       #endregion
   }
}
