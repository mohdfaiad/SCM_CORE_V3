using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;
using System.Configuration;

namespace BAL
{
    public  class BALAWBTracLDetails
    {
        public BALAWBTracLDetails()
        {
           constr = Global.GetConnectionString();
        }
        //string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        public DataSet dsgetAWBRecord(string Pre,string AWB,ref string error) 
        {
            try
            {
                DataSet dsRec = new DataSet();
                SQLServer ss = new SQLServer(constr);

                string[] ColumnNames = new string[2];
                SqlDbType[] DataType = new SqlDbType[2];
                Object[] Values = new object[2];

                ColumnNames[0] = "Pre";
                DataType[0] = SqlDbType.NVarChar;
                Values[0] = Pre;

                ColumnNames[1] = "AWBNo";
                DataType[1] = SqlDbType.NVarChar;
                Values[1] = AWB;

                dsRec = ss.SelectRecords("spgetAWBTrackingListforAWBMessage", ColumnNames, Values, DataType);
                if (dsRec != null)
                {
                    if (dsRec.Tables != null)
                    {
                        if (dsRec.Tables.Count > 0)
                        {
                            if (dsRec.Tables[0].Rows.Count > 0)
                            {
                                error = "";
                                return (dsRec);
                            }
                            else
                                error = "No Record Available";
                        }
                        else
                            error = "Error during Fetching Record";
                    }
                    else
                        error = "Error during Fetching Record";
                }
                else
                    error = "Error during Fetching Record";
            }
            catch (Exception ex)
            {
                error = "Error: " + ex.Message;
            }
            return (null);
        }

         SQLServer db = new SQLServer("");
        string constr = "";
        DataSet res1;
        
        #region SaveData
        public bool savedata(string message,string MessageCode, string MessageDate,string Messagetime,string Details,string AWBPrefix,string AWBNumber)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();

                string[] Pname = new string[7];
                object[] Pvalue = new object[7];
                SqlDbType[] Ptype = new SqlDbType[7];

                Pname[0] = "AWBPrefix";
                Pname[1] = "AWBNumber";
                Pname[2] = "messageCode";
                Pname[3] = "message";
                Pname[4] = "MessageDate";
                Pname[5] = "Messagetime";
                Pname[6] = "Details";

                Pvalue[0] = AWBPrefix;
                Pvalue[1] = AWBNumber;
                Pvalue[2] = MessageCode;
                Pvalue[3] = message;
                Pvalue[4] = MessageDate;
                Pvalue[5] = Messagetime;
                Pvalue[6] = Details;

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;


                bool res = da.InsertData("SpUpdateAWBDetails", Pname, Ptype, Pvalue);

                return res;
            }
            catch (Exception ex)
            {
                return false;            
            }
        
        }
#endregion SaveData
    }
}
