using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;

namespace BAL
{
    public class BALAgentCredit
    {
        SQLServer db = new SQLServer("");
        string constr = "";
        DataSet res;
        DataSet  ds;
        public BALAgentCredit()
        {
            constr = Global.GetConnectionString();

        }
        #region GetAgentCredit
        public DataSet GetAgent(string AgentCode,DateTime dt)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();

                string[] Pname = new string[2];
                object[] Pvalue = new object[2];
                SqlDbType[] Ptype = new SqlDbType[2];

                Pname[0] = "AgentCode";
                Pname[1] = "TranDate";

                Pvalue[0] = AgentCode;
                Pvalue[1] = dt;

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.DateTime;

                res = da.SelectRecords("Sp_GetGuranteeAmt", Pname, Pvalue, Ptype);
                if (res != null)
                {
                    if (res.Tables[0].Rows.Count > 0)
                    {
                        return res;
                    }
                }
            }
            catch (Exception ex)
            {
                return res;
            }
            return res;
        }
        #endregion GetAgentCredit
        #region SaveTransacation
        public bool SaveTransacation(string AgentName, string AgentCode, string paymentType, string TransactionType, double GuranteeAmont, double BalanceAmount, double Amount, string Remarks, string CreatedBy,string AWBNumber,string InvoiceNumber)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] paramname = new string[11];
                paramname[0] = "AgentName";
                paramname[1] = "AgentCode";
                paramname[2] = "paymentType";
                paramname[3] = "TransactionType";
                paramname[4] = "GuranteeAmount";
                paramname[5] = "BalanceAmount";
                paramname[6] = "Amount";
                paramname[7] = "Remarks";
                paramname[8] = "CreatedBy";
                paramname[9] = "AWBNumber";
                paramname[10] = "InvoiceNo";
 
                object[] paramvalue = new object[11];
                paramvalue[0] = AgentName;
                paramvalue[1] = AgentCode;
                paramvalue[2] = paymentType;
                paramvalue[3] = TransactionType;
                paramvalue[4] = GuranteeAmont;
                paramvalue[5] = BalanceAmount;
                paramvalue[6] = Amount;
                paramvalue[7] = Remarks;
                paramvalue[8] = CreatedBy;
                paramvalue[9] = AWBNumber;
                paramvalue[10] = InvoiceNumber;
  
                SqlDbType[] paramtype = new SqlDbType[11];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.VarChar;
                paramtype[4] = SqlDbType.Float ;
                paramtype[5] = SqlDbType.Float ;
                paramtype[6] = SqlDbType.Float;
                paramtype[7] = SqlDbType.VarChar;
                paramtype[8] = SqlDbType.VarChar;
                paramtype[9] = SqlDbType.VarChar;
                paramtype[10] = SqlDbType.VarChar;

                ds = da.SelectRecords("Sp_SaveTransaction", paramname, paramvalue, paramtype);
                return true;
            }
            catch (Exception ex)
            {
                return false; 
            }
        }
        #endregion  SaveTransacation
    }
}
