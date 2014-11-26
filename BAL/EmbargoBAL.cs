using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BAL;
using QID.DataAccess;      

namespace BAL
{
    public class EmbargoBAL
    {
        SQLServer db = new SQLServer("");
        string constr = "";
         #region Constructor
        public EmbargoBAL()
        {
           constr = Global.GetConnectionString();
        }
        #endregion Constructor

        #region SaveEmbargo


        public DataSet  SaveEmbago(string refno, string level, string status, DateTime startdate, DateTime enddate, string origintype, string origin, string Destinationtype, string Destination, string Description, string Remarks, string Suspend,string Parameter,string Applicable,string Value,string DaysOfWeek )
        {
            DataSet res=new DataSet() ;
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();

                string[] Pname = new string[16];
                object[] Pvalue = new object[16];
                SqlDbType[] Ptype = new SqlDbType[16];

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.DateTime;
                Ptype[4] = SqlDbType.DateTime;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;
                Ptype[8] = SqlDbType.VarChar;
                Ptype[9] = SqlDbType.VarChar;
                Ptype[10] = SqlDbType.VarChar;
                Ptype[11] = SqlDbType.VarChar;
                Ptype[12] = SqlDbType.VarChar;
                Ptype[13] = SqlDbType.VarChar;
                Ptype[14] = SqlDbType.VarChar;
                Ptype[15] = SqlDbType.VarChar;       
                
                Pvalue[0] = refno;
                Pvalue[1] = level;
                Pvalue[2] = status; 
                Pvalue[3] = startdate ;
                Pvalue[4] = enddate ;
                Pvalue[5] = origintype;
                Pvalue[6] = origin;
                Pvalue[7] = Destinationtype;
                Pvalue[8] = Destination ;
                Pvalue[9] = Description;
                Pvalue[10] = Remarks;
                Pvalue[11] = Suspend;
                Pvalue[12] = Parameter;
                Pvalue[13] = Applicable;
                Pvalue[14] = Value;
                Pvalue[15] = DaysOfWeek;  

//ReferenceNumber,RefLevel ,Status ,startDate,EndDate ,OriginType ,Origin,DestinationType,Destination,Discription,Remarks,Suspend )
                Pname[0] = "ReferenceNumber";
                Pname[1] = "RefLevel";
                Pname[2] = "Status";
                Pname[3] = "startDate";
                Pname[4] = "EndDate";
                Pname[5] = "OriginType";
                Pname[6] = "Origin";
                Pname[7] = "DestinationType";
                Pname[8] = "Destination";
                Pname[9] = "Discription";
                Pname[10] = "Remarks";
                Pname[11] = "Suspend";
                Pname[12] = "Parameter";
                Pname[13] = "Applicable";
                Pname[14] = "Value";
                Pname[15] = "DaysOfWeek"; 


                res = da.SelectRecords("SpInsertEmbargo", Pname,Pvalue,Ptype);
                 

                return res;
            }
               
            catch (Exception ex)
            {
                return null ; 
            }
            
        }
        #endregion SaveEmbargo

        #region DeleteEmbargo


        public DataSet DeleteEmbago(string refno)
        {
            DataSet res = new DataSet();
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();

                string[] Pname = new string[1];
                object[] Pvalue = new object[1];
                SqlDbType[] Ptype = new SqlDbType[1];

                Ptype[0] = SqlDbType.VarChar;
                //Ptype[1] = SqlDbType.VarChar;
                //Ptype[2] = SqlDbType.VarChar;
                //Ptype[3] = SqlDbType.DateTime;
                //Ptype[4] = SqlDbType.DateTime;
                //Ptype[5] = SqlDbType.VarChar;
                //Ptype[6] = SqlDbType.VarChar;
                //Ptype[7] = SqlDbType.VarChar;
                //Ptype[8] = SqlDbType.VarChar;
                //Ptype[9] = SqlDbType.VarChar;
                //Ptype[10] = SqlDbType.VarChar;
                //Ptype[11] = SqlDbType.VarChar;
                //Ptype[12] = SqlDbType.VarChar;
                //Ptype[13] = SqlDbType.VarChar;
                //Ptype[14] = SqlDbType.VarChar;
                //Ptype[15] = SqlDbType.VarChar;

                Pvalue[0] = refno;
                //Pvalue[1] = level;
                //Pvalue[2] = status;
                //Pvalue[3] = startdate;
                //Pvalue[4] = enddate;
                //Pvalue[5] = origintype;
                //Pvalue[6] = origin;
                //Pvalue[7] = Destinationtype;
                //Pvalue[8] = Destination;
                //Pvalue[9] = Description;
                //Pvalue[10] = Remarks;
                //Pvalue[11] = Suspend;
                //Pvalue[12] = Parameter;
                //Pvalue[13] = Applicable;
                //Pvalue[14] = Value;
                //Pvalue[15] = DaysOfWeek;

                //ReferenceNumber,RefLevel ,Status ,startDate,EndDate ,OriginType ,Origin,DestinationType,Destination,Discription,Remarks,Suspend )
                Pname[0] = "ReferenceNumber";
                //Pname[1] = "RefLevel";
                //Pname[2] = "Status";
                //Pname[3] = "startDate";
                //Pname[4] = "EndDate";
                //Pname[5] = "OriginType";
                //Pname[6] = "Origin";
                //Pname[7] = "DestinationType";
                //Pname[8] = "Destination";
                //Pname[9] = "Discription";
                //Pname[10] = "Remarks";
                //Pname[11] = "Suspend";
                //Pname[12] = "Parameter";
                //Pname[13] = "Applicable";
                //Pname[14] = "Value";
                //Pname[15] = "DaysOfWeek";


                res = da.SelectRecords("SpDeleteEmbargo", Pname, Pvalue, Ptype);


                return res;
            }

            catch (Exception ex)
            {
                return null;
            }

        }
        #endregion DeleteEmbargo


        #region ListEmbargo
        public DataSet ListEmbargoDetails(string Origintype,string Origin,string DestinationType,string Destinnation,string  EmbargoRefNumber,string Level,string ParameterCode,string FromDate,string ToDate,string Status)
        {
            DataSet res = new DataSet();
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();

                string[] Pname = new string[10];
                object[] Pvalue = new object[10];
                SqlDbType[] Ptype = new SqlDbType[10];


                Pname[0] = "originType";
                Pname[1] = "origin";
                Pname[2] = "destType";
                Pname[3] = "dest";
                Pname[4] = "refNo";
                Pname[5] = "reflevel";
                Pname[6] = "paraType";
                Pname[7] = "status";
                Pname[8] = "startDate";
                Pname[9] = "endDate";
               

                Pvalue[0] = Origintype;
                Pvalue[1] = Origin;
                Pvalue[2] = DestinationType;
                Pvalue[3] = Destinnation;
                Pvalue[4] = EmbargoRefNumber;
                Pvalue[5] = Level;
                Pvalue[6] = ParameterCode;
                Pvalue[7] = Status;
                Pvalue[8] = FromDate;
                Pvalue[9] = ToDate;
               

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;
                Ptype[8] = SqlDbType.VarChar;
                Ptype[9] = SqlDbType.VarChar;
               

                res = da.SelectRecords("SP_GetEmbargoDetails", Pname, Pvalue, Ptype);
                if (res != null)
                {
                    if (res.Tables != null)
                    {
                        if (res.Tables.Count > 0)
                        {
                            return res;
                        }
                    }
                }
   

            }
            catch (Exception ex)
            {
                return res;
            }
            return res;
        }

        #endregion ListEmbargo

        public DataSet GetEmbargoDetails(int Refno)
        { 
         DataSet res=new DataSet() ;
         try
         {
             SQLServer da = new SQLServer(constr);
             DataSet ds = new DataSet();

             string[] Pname = new string[1];
             object[] Pvalue = new object[1];
             SqlDbType[] Ptype = new SqlDbType[1];

             Ptype[0] = SqlDbType.VarChar;

             Pname[0] = "Refno";
             Pvalue[0] = Refno;

             res = da.SelectRecords("SpGetEmbargoDetails", Pname, Pvalue, Ptype);

             if (res != null)
             {
                 if (res.Tables != null)
                 {
                     if (res.Tables.Count > 0)
                     {
                         return res;
                     }
                 }
             }
             

         }
         catch (Exception ex)
         {
             return res; 
         }
         return res;
        }

        //Added-24thSept
        //unit testing Done
        # region Get Embergo List
        public DataSet GetEmbergoList(object[] PrefixInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[9];
                SqlDbType[] DataType = new SqlDbType[9];
                Object[] Values = new object[9];
                int i = 0;

                //1
                ColumnNames.SetValue("refno", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("Origintype", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("Origin", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("DestinationType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //5
                ColumnNames.SetValue("Destinnation", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //6
                ColumnNames.SetValue("Level", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //7
                ColumnNames.SetValue("FromDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //8
                ColumnNames.SetValue("ToDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //9
                ColumnNames.SetValue("Status", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                DataSet ds = new DataSet();
                ds = da.SelectRecords("spListEmbergo", ColumnNames, Values, DataType);
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
            catch (Exception ex)
            {
            }
            return (null);
        }
        # endregion Get Embergo List

    }
}
