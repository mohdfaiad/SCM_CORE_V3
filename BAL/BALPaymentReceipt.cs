using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QID.DataAccess;
using System.Data;

namespace BAL
{
    public class BALPaymentReceipt
    {
        string constr = "";
        public BALPaymentReceipt()
        {
        constr = Global.GetConnectionString();
        }
        #region Save
        //public bool SaveReceipt(object[] obj)
        //{
            //try
            //{
            //    string[] ParamNames = new string[] {"PaymentAdviceNumber","ServiceCode","AttributeName","AttributeValue","ServiceName","ChargeName",
            //    "Amount","Waive","GST","NetAmount","Currency","Remark","Total","FinalAmount","GSTOnTotal","BalanceGiven",
            //    "TDS","CheckWaive","GrandTotal","RoundOffAmount","CustommerCode","CustomerName","PaymntDetails","PaymentRemark"};
            //    SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Int, SqlDbType.Int, SqlDbType.Int, SqlDbType.Int };
            //    SQLServer da = new SQLServer(constr);

            //    bool res=da.InsertData("Sp_InsertPaymentDetails",ParamNames,ParamTypes,obj);
            //    if (res==true)
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //catch (Exception ex)

            //{ 

            //}
            //return false;        
        
       // }

#endregion save


    }
}
