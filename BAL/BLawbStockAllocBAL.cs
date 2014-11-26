using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data ;
using System.Configuration;
using QID.DataAccess;

namespace BAL
{
    public class BLawbStockAllocBAL
    {
        string ConStr = Global.GetConnectionString();//ConfigurationManager.ConnectionStrings["ConStr"].ToString();

        #region Get Agent 
        public DataSet GetAgentName()
        {
            DataSet DsAgent = new DataSet();
            SQLServer db = new SQLServer(ConStr);

            try
            {
                
                DsAgent = db.SelectRecords("SPGetAgentName");


                return DsAgent;
            }
            catch (Exception ex)
            {
                return DsAgent;
            }

        }
        #endregion Get Agent

        #region DDL Sub Level getdata

        public DataSet Getsubleveldata(string SelectedItem)
        {
            DataSet DsAgent = new DataSet();
            SQLServer db = new SQLServer(ConStr);

            try
            {
                string[] Pname = new string[1];
                object[] Pvalue = new object[1];
                SqlDbType[] Ptype = new SqlDbType[1];


                Pname[0] = "SelectedItem";
                Ptype[0] = SqlDbType.NVarChar;
                Pvalue[0] = SelectedItem ;

                DsAgent = db.SelectRecords("SPGetSubLevel",Pname ,Pvalue ,Ptype );


                return DsAgent;
            }
            catch (Exception ex)
            {
                return DsAgent;
            } 
        
        }
        #endregion DDL Sub Level getdata


        #region Save AgentAWB Allocation data
        //row["AgentName"].ToString(), row["FromAWB"].ToString(), row["ToAWB"].ToString(), row["SubLevelCode"].ToString(),"City", Session["UserName"].ToString(),""  

        public DataSet SaveAgentAllocatedData(string Agentname, string FrmAWB, string ToAWB, string SubLevelCode, string City, string UserName,string str,string preAWB, DateTime Time)
        {
            DataSet result = new DataSet();
            try
            {
                SQLServer db = new SQLServer(ConStr);

                string[] Pname = new string[8];
                object[] Pvalue = new object[8];
                SqlDbType[] Ptype = new SqlDbType[8];


                Pname[0] = "ALevel";
                Pname[1] = "AFrom";
                Pname[2] = "ATo";
                Pname[3] = "ParName";
                Pname[4] = "ParType";
                Pname[5] = "User";
                Pname[6] = "AWB";
                Pname[7] = "Time"; 

                 //Agentname  FrmAWB  ToAWB SubLevel  city 


                 Ptype[0] = SqlDbType.VarChar;
                 Ptype[1] = SqlDbType.VarChar;
                 Ptype[2] = SqlDbType.VarChar;
                 Ptype[3] = SqlDbType.VarChar;
                 Ptype[4] = SqlDbType.VarChar;
                 Ptype[5] = SqlDbType.VarChar;
                 Ptype[6] = SqlDbType.VarChar;
                 Ptype[7] = SqlDbType.DateTime;   

                Pvalue[0] = Agentname ;
                Pvalue[1] = FrmAWB;
                Pvalue[2] = ToAWB;
                Pvalue[3] = SubLevelCode;
                Pvalue[4] = City;
                Pvalue[5] = UserName;
                Pvalue[6]= preAWB;
                Pvalue[7] = Time; 


                result = db.SelectRecords("SpStockAllocation", Pname, Pvalue, Ptype);   

                return result ;
            }
            catch (Exception ex)
            {
                return result = null;
            }

        }
        #endregion Save AgentAWB Allocation data


        #region SPValidateAgentAWBAlloctionData

        public DataSet SPValidateAgentAWBAlloctionData(string Agentname,string SubLevel, string FrmAWB, string ToAWB, string UpdatedOn)
        {
            DataSet result = new DataSet();
            try
            {
                SQLServer db = new SQLServer(ConStr);

                string[] Pname = new string[5];
                object[] Pvalue = new object[5];
                SqlDbType[] Ptype = new SqlDbType[5];


                Pname[0] = "Agentname";
                
                Pname[1] = "SubLevel";
                Pname[2] = "FrmAWB";
                Pname[3] = "ToAWB";
                Pname[4] = "UpdatedOn";


                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
               // Ptype[5] = SqlDbType.VarChar;


                Pvalue[0] = Agentname;
                //Pvalue[1] = Level;
                Pvalue[1] = SubLevel;
                Pvalue[2] = FrmAWB;
                Pvalue[3] = ToAWB;
                Pvalue[4] = UpdatedOn;




                result = db.SelectRecords("SPValidateAgentAWBAlloctionData", Pname, Pvalue, Ptype);

                return result;
            }
            catch (Exception ex)
            {
                return result = null;
            }

        }
        #endregion SPValidateAgentAWBAlloctionData


    }
}
