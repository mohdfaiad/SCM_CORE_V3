using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QID.DataAccess;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BAL
{
   public class BALAdminSupportTask
    {
       #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        SQLServer db = new SQLServer(Global.GetConnectionString());
        #endregion Variables

       #region Get Invoice Details
        public DataSet GetInvoiceDetails(string InvoiceNumber)
        {
            DataSet ds = new DataSet();
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] Pname = new string[1];
                object[] PValue = new object[1];
                SqlDbType[] PType = new SqlDbType[1];

                Pname[0] = "InvoiceNumber";
                PValue[0] = InvoiceNumber;
                PType[0] = SqlDbType.VarChar;

                ds = da.SelectRecords("SP_GetInvoiceList", Pname, PValue, PType);
             
                
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

                return ds;


            }
            catch (Exception ex)
            {
                return ds;
            }
        }

    #endregion

       #region Reopen Invoice
        public bool ReopenInvoice(string InvoiceNumber, DateTime UpdatedOn, string UpdatedBy)
        {
            bool result = false;

            try
            {
                SQLServer da = new SQLServer(constr);

                string[] Pname = { "InvoiceNumber", "UpdatedOn","UpdatedBy" };
                object[] PValue = {InvoiceNumber, UpdatedOn, UpdatedBy };
                SqlDbType[] PType = { SqlDbType.VarChar,SqlDbType.DateTime,SqlDbType.VarChar };

                result = da.ExecuteProcedure("SP_ReopenInvoice", Pname, PType, PValue);
                return true;
            }
            catch (Exception ex)
            {
                return false;   
          
            }
        }

        #endregion

       #region Ageent Code DropDown
        public DataSet GetAgentCode()
        {
            DataSet ds = new DataSet();

            try
            {
                SQLServer da = new SQLServer(constr);

                ds =da.SelectRecords("SPForAgentCode");
                return ds;
            }
            catch (Exception  ex)
            {
             return null;                
            }
        }

        #endregion

       #region Make AWB Void
        public DataSet AWBVoid(string AWBPrefix, string AWBNumber, string UpdatedBy, DateTime UpdatedOn)
        {
            DataSet ds = new DataSet();
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] Pname = new string[4];
                Pname[0] = "AWBPrefix";
                Pname[1] = "AWBNumber";
                Pname[2] = "UpdatedOn";
                Pname[3] = "UpdatedBy";


                object[] PValue = new object[4];
                PValue[0] = AWBPrefix;
                PValue[1] = AWBNumber;
                PValue[2] = UpdatedOn;
                PValue[3] = UpdatedBy;

                SqlDbType[] PType = new SqlDbType[4];
                PType[0] = SqlDbType.VarChar;
                PType[1] = SqlDbType.VarChar;
                PType[2] = SqlDbType.DateTime;
                PType[3] = SqlDbType.VarChar;

                ds = da.SelectRecords("SP_MakeAWBVoid", Pname,PValue,PType);
               // bool result;
               //result = da.ExecuteProcedure("SP_MakeAWBVoid", Pname, PType, PValue);


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

                return ds;

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        #endregion

        #region Get AWB List to Upadate Service Tax
        public DataSet GetAWBlistToST(string AWBPrefix,string AWBNumber)
        {
            DataSet ds = new DataSet();
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] Pname = new string[2];
                Pname[0] = "AWBPrefix";
                Pname[1] = "AWBNumber";


                object[] PValue = new object[2];
                PValue[0] = AWBPrefix;
                PValue[1] = AWBNumber;


                SqlDbType[] PType = new SqlDbType[2];
                PType[0] = SqlDbType.VarChar;
                PType[1] = SqlDbType.VarChar;


                ds = da.SelectRecords("SP_GetAWBListToST", Pname, PValue, PType);

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

                return ds;


            }
            catch (Exception ex)
            {
                return ds;
            }

        }

        #endregion

        #region Get AWB Details to Void
        public DataSet GetAWBListToVoid(string AWBPrefix, string AWBNumber)
        {
            DataSet ds = new DataSet();
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] Pname = new string[2];
                Pname[0] = "AWBPrefix";
                Pname[1] = "AWBNumber";
               

                object[] PValue = new object[2];
                PValue[0] = AWBPrefix;
                PValue[1] = AWBNumber;
              

                SqlDbType[] PType = new SqlDbType[2];
                PType[0] = SqlDbType.VarChar;
                PType[1] = SqlDbType.VarChar;


                ds = da.SelectRecords("SP_GetAWBListToVoid", Pname, PValue, PType);

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

                return ds;


            }
            catch (Exception ex)
            {
                return ds;
            }

        }
        #endregion

       #region Get AWB Details to RePreocess Rate
        
        public DataSet GetAWBListToReProcess(string AWBPrefix, string AWBNumber)
        {
            DataSet ds = new DataSet();
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] Pname = new string[2];
                Pname[0] = "AWBPrefix";
                Pname[1] = "AWBNumber";


                object[] PValue = new object[2];
                PValue[0] = AWBPrefix;
                PValue[1] = AWBNumber;


                SqlDbType[] PType = new SqlDbType[2];
                PType[0] = SqlDbType.VarChar;
                PType[1] = SqlDbType.VarChar;


                ds = da.SelectRecords("SP_GetAWBDetailsToReProcess", Pname, PValue, PType);

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

                return ds;


            }
            catch (Exception ex)
            {
                return ds;
            }

        }
        #endregion

       #region change AWB Rate
        public DataSet ChangeAWBRate(string AWBPrefix, string AWBNumber, string NewRate, DateTime UpdatedOn, 
            string UpdatedBy)
        {
            DataSet ds = new DataSet();
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] Pname = new string[5];
                Pname[0] = "AWBPrefix";
                Pname[1] = "AWBNumber";
                Pname[2] = "RatePerKg";
                Pname[3] = "UpdatedOn";
                Pname[4] = "UpdatedBy";

                object[] PValue = new object[5];
                PValue[0] = AWBPrefix;
                PValue[1] = AWBNumber;
                PValue[2] = NewRate;
                PValue[3] = UpdatedOn;
                PValue[4] = UpdatedBy;

                SqlDbType[] PType = new SqlDbType[5];
                PType[0] = SqlDbType.VarChar;
                PType[1] = SqlDbType.VarChar;
                PType[2] = SqlDbType.Int;
                PType[3] = SqlDbType.DateTime;
                PType[4] = SqlDbType.VarChar;

                ds = da.SelectRecords("spUpateAWBRateManually", Pname,PValue, PType);
                //result = da.ExecuteProcedure("spUpateAWBRateManually", Pname, PType, PValue);
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

                return ds;


            }
            catch (Exception ex)
            {
                return null;
            }

           

        }
        #endregion

       #region Agent Code Details

        public DataSet GetAgentCodeDetails(string AWBPrefix, string AWBNumber)
        {
            DataSet ds = new DataSet();
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] Pname = new string[2];
                Pname[0] = "AWBPrefix";
                Pname[1] = "AWBNumber";
                //Pname[2] = "AgentCode";

                object[] PValue = new object[2];
                PValue[0] = AWBPrefix;
                PValue[1] = AWBNumber;
                //PValue[2] = AgentCode;

                SqlDbType[] PType = new SqlDbType[2];
                PType[0] = SqlDbType.VarChar;
                PType[1] = SqlDbType.VarChar;
                //PType[2] = SqlDbType.VarChar;

                ds = da.SelectRecords("SP_AgentCodeDetails", Pname, PValue, PType);

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

                return ds;


            }
            catch (Exception ex)
            {
                return ds;
            }
        }


        #endregion

       #region AWB DateWise Details

        public DataSet GetAWBDateWiseDetails(string AWBPrefix, string AWBNumber, string AWBDate)
        {
            DataSet ds = new DataSet();
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] Pname = new string[3];
                Pname[0] = "AWBPrefix";
                Pname[1] = "AWBNumber";
                Pname[2] = "AWBDate";

                object[] PValue = new object[3];
                PValue[0] = AWBPrefix;
                PValue[1] = AWBNumber;
                PValue[2] = AWBDate;

                SqlDbType[] PType = new SqlDbType[3];
                PType[0] = SqlDbType.VarChar;
                PType[1] = SqlDbType.VarChar;
                PType[2] = SqlDbType.VarChar;

                ds = da.SelectRecords("SP_AWBDetails", Pname, PValue, PType);

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

                return ds;


            }
            catch (Exception ex)
            {
                return ds;
            }
        }
        #endregion

       #region Change AWB Date
        public DataSet SetAWBDate(string AWBPrefix, string AWBNumber, string AWBDate, DateTime UpdatedOn, string UpdatedBy)
        {
            DataSet ds = new DataSet();
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] Pname = new string[5];
                Pname[0] = "AWBPrefix";
                Pname[1] = "AWBNumber";
                Pname[2] = "AWBDate";
                Pname[3] = "UpdatedOn";
                Pname[4] = "UpdatedBy";

                object[] PValue = new object[5];
                PValue[0] = AWBPrefix;
                PValue[1] = AWBNumber;
                PValue[2] = AWBDate;
                PValue[3] = UpdatedOn;
                PValue[4] = UpdatedBy;

                SqlDbType[] PType = new SqlDbType[5];
                PType[0] = SqlDbType.VarChar;
                PType[1] = SqlDbType.VarChar;
                PType[2] = SqlDbType.VarChar;
                PType[3] = SqlDbType.DateTime;
                PType[4] = SqlDbType.VarChar;

                ds = da.SelectRecords("SP_UpdateAWBDate", Pname, PValue, PType);

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

                return ds;


            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        #endregion
        #region update Service Tax
        public DataSet UpdateServiceTax(string AWBPrefix, string AWBNumber, string UpdatedBy,DateTime UpdatedOn)
        {
            DataSet ds = new DataSet();
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] Pname = new string[4];
                Pname[0] = "AWBPrefix";
                Pname[1] = "AWBNumber";
                Pname[2] = "UpdatedBy";
                Pname[3] = "UpdatedOn";

                object[] PValue = new object[4];
                PValue[0] = AWBPrefix;
                PValue[1] = AWBNumber;
                PValue[2] = UpdatedBy;
                PValue[3] = UpdatedOn;

                SqlDbType[] PType = new SqlDbType[4];
                PType[0] = SqlDbType.VarChar;
                PType[1] = SqlDbType.VarChar;
                PType[2] = SqlDbType.VarChar;
                PType[3] = SqlDbType.DateTime;

                ds = da.SelectRecords("SP_UpdateServiceTaxInBooking", Pname, PValue, PType);

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

                return ds;


            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        #endregion


        #region Change Agent Code
        public DataSet SetAgentCode(string AWBPrefix, string AWBNumber, string AgentCode, string UserName, DateTime UpdatedOn)
        {
            DataSet ds = new DataSet();

            try
            {
                SQLServer da = new SQLServer(constr);

                string[] Pname = new string[5];
                Pname[0] = "AWBPrefix";
                Pname[1] = "AWBNumber";
                Pname[2] = "AgentCode";
                Pname[3] = "UpdatedBy";
                Pname[4] = "UpdatedOn";

                object[] PValue = new object[5];
                PValue[0] = AWBPrefix;
                PValue[1] = AWBNumber;
                PValue[2] = AgentCode;
                PValue[3] = UserName;
                PValue[4] = UpdatedOn;

                SqlDbType[] PType = new SqlDbType[5];
                PType[0] = SqlDbType.VarChar;
                PType[1] = SqlDbType.VarChar;
                PType[2] = SqlDbType.VarChar;
                PType[3] = SqlDbType.VarChar;
                PType[4] = SqlDbType.DateTime;

                //Result = da.SelectRecords("SP_ChangeAgentCode", Pname, PValue, PType);
                ds = da.SelectRecords("SP_ChangeAgentCode", Pname, PValue, PType);

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

                return ds;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

       #region       AWB Details Spot rate
        public DataSet AWBDetailsSpotrate(string AWBPrefix, string AWBNumber)
        {

            SQLServer da = new SQLServer(constr);
            try 
            {
                DataSet ds = new DataSet();

                string[] Pname = new string[2];
                object[] PValue = new object[2];
                SqlDbType[] PType = new SqlDbType[2];

                Pname[0] = "AWBNumber";
                PValue[0] = AWBNumber;
                PType[0] = SqlDbType.VarChar;

                Pname[1] = "AWBPrefix";
                PValue[1] = AWBPrefix;
                PType[1] = SqlDbType.VarChar;

                ds = da.SelectRecords("SP_GetAWBListSpotrate", Pname, PValue, PType);


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

                return ds;

            }
            catch (Exception ex)
            {
                return null;
            }
           
        }
        #endregion

       #region Remove AWB SpotRate
        public DataSet DeleteAWBSpotRate(string AWBPrefix, string AWBNumber,  DateTime UpdatedOn, string UpdatedBy)
        {

            SQLServer da = new SQLServer(constr);
            try
            {
                //bool status;
                DataSet ds = new DataSet();

                string[] Pname = {"AWBPrefix","AWBNumber","UpdatedOn","UpdatedBy"};
                object[] PValue = {AWBPrefix,AWBNumber,UpdatedOn,UpdatedBy};
                SqlDbType[] PType = {SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.DateTime,SqlDbType.VarChar};

                ds = da.SelectRecords("SP_RemoveAWBSpotRate", Pname, PValue, PType);

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

                return ds;

                //return status;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        #endregion

       #region       AWB Details DCM
         
  // public DataSet AWBDetailsDCM(string AWBPrefix, string AWBNumber, string FlightNo)        
  public DataSet AWBDetailsDCM(string AWBPrefix,string AWBNumber,string FlightNo, string FlightDate)        
   
   {

               SQLServer da = new SQLServer(constr);
               try
               {
                   DataSet ds = new DataSet();
                   string[] Pname = new string[4];
                   Pname[0] = "AWBPrefix";
                   Pname[1] = "AWBNumber";
                   Pname[2] = "FlightNo";
             Pname[3] = "FlightDate";

                   object[] PValue = new object[4];
                   PValue[0] = AWBPrefix;
                   PValue[1] = AWBNumber;
                   PValue[2] = FlightNo;
              PValue[3] = FlightDate;

                   SqlDbType[] PType = new SqlDbType[4];
                   PType[0] = SqlDbType.VarChar;
                   PType[1] = SqlDbType.VarChar;
                   PType[2] = SqlDbType.VarChar;
                 PType[3] = SqlDbType.VarChar;

                   //string[] Pname = new string[1];
                   //object[] PValue = new object[1];
                   //SqlDbType[] PType = new SqlDbType[1];

                   //Pname[0] = "AWBNumber";
                   //PValue[0] = AWBNumber;
                   //PType[0] = SqlDbType.VarChar;

                   ds = da.SelectRecords("SP_GetAWBListDCM", Pname, PValue, PType);
                   

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

                   return ds;

               }
               catch (Exception ex)
               {
                   return null;
               }

           }
           #endregion

       #region Remove AWB DCM


   public bool DeleteAWBDCM(string AWBPrefix,string AWBNumber, string FlightNo, string FlightDate, string UpdatedBy,
       DateTime UpdatedOn)
  
   {

       SQLServer da = new SQLServer(constr);
       try
       {
           bool status;

           string[] Pname = new string[6];
           Pname[0] = "AWBPrefix";
           Pname[1] = "AWBNumber";
           Pname[2] = "FlightNo";
           Pname[3] = "FlightDate";
           Pname[4] = "UpdatedBy";
           Pname[5] = "UpdatedOn";

           object[] PValue = new object[6];
           PValue[0] = AWBPrefix;
           PValue[1] = AWBNumber;
           PValue[2] = FlightNo;
           PValue[3] = FlightDate;
           PValue[4] = UpdatedBy;
           PValue[5] = UpdatedOn;

           SqlDbType[] PType = new SqlDbType[6];
           PType[0] = SqlDbType.VarChar;
           PType[1] = SqlDbType.VarChar;
           PType[2] = SqlDbType.VarChar;
           PType[3] = SqlDbType.VarChar;
           PType[4] = SqlDbType.VarChar;
           PType[5] = SqlDbType.DateTime;
         
           status = da.ExecuteProcedure("SP_RemoveAWBDCM", Pname, PType, PValue);

           return status;
       }



       catch (Exception ex)
       {
           return false;
       }

   }
   #endregion
    }

    
}
