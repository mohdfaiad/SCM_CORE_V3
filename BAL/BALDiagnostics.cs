using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QID.DataAccess;
using System.Data;

namespace BAL
{
    public class BALDiagnostics
    {

        public bool testBAL()
        {
            return true;
        }

        public bool testDAL()
        {
            try
            {
                string ConStr = Global.GetConnectionString();
                SQLServer objSQL = new SQLServer(ConStr);
                string Query = "select * from dbo.UserMasterNew";
                DataSet dsData = new DataSet();
                dsData = objSQL.GetDataset(Query);
                if (dsData != null && dsData.Tables.Count>0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }

    }
}
