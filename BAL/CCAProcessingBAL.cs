using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using QID.DataAccess;

namespace BAL
{
    public class CCAProcessingBAL
    {
        string constr = "";
        public CCAProcessingBAL()
        {
            constr = Global.GetConnectionString();
        }

        # region GetCCANumber
        public DataSet GetCCANumber()
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("spGetCurrentCCANumber");
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
        #endregion GetCCANumber

        #region CCAProcessing

        public DataSet CCAProcessing(object[] AcEq)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ParamNames = new string[] { "AWBNumber" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar };
                DataSet ds = da.SelectRecords("spGetDCAWBProcessing", ParamNames, AcEq, ParamTypes);
                //DataSet ds = da.SelectRecords("spGetAircraftEquipment", "Manufacturer", AcInfo, SqlDbType.VarChar);
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

        #endregion DCAWBProcessing

        #region SaveCCAProcessing
        public int SaveCCAProcessing(object[] AcEq)
        {
            try
            {
                string[] ParamNames = new string[] { "preAWB", "AWBNumber", "InvoiceNo", "AgentCode", "FromDate", "ToDate", "CCANumber", "CurrentGrossWt", "CurrentChargableWt", "CurrentFreight", "CurrentOCDC", "CurrentOCDA", "CurrentTax", "CurrentTotal", "RevisedGrossWt", "RevisedChargableWt", "RevisedFreight", "Remarks", "RevisedOCDC", "RevisedOCDA", "Debit", "Credit", "RevisedTax", "RevisedTotal" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                SQLServer da = new SQLServer(constr);
                bool res = da.UpdateData("spSaveCCAAWBProcessing", ParamNames, ParamTypes, AcEq);
                if (res)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {
            }
            return (-1);
        }
        #endregion SaveCCAProcessing

        #region Fill Current CCA
        public DataSet FillCurrentCCA(object []objCCA)
        {
            try
            {
                string[] ParamNames = new string[] {"preAWB", "AWBNumber","InvoiceNo","AgentCode","FromDate","ToDate" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("spFillCurrentCCA", ParamNames, objCCA, ParamTypes);
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

                return null;
                
            }
            return null;
        }
            
    #endregion

        #region List CCA
        public DataSet ListCCA(object[] objCCA)
        {
            try
            {
                string[] ParamNames = new string[] { "CCANumber", "preAWB", "AWBNumber", "InvoiceNo", "AgentCode" };
                SqlDbType[] ParamTypes = new SqlDbType[] {SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("SpGetCCAProcessing", ParamNames, objCCA, ParamTypes);
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

                return null;

            }
            return null;
        }

        #endregion

    }
}
