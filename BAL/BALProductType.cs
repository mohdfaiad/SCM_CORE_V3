using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;
using System.Data.SqlClient;

namespace BAL
{
    public class BALProductType
    {
        
        SQLServer db = new SQLServer("");
        string constr = "";
        DataSet res;

        public BALProductType()
        {
            constr = Global.GetConnectionString();
        }

        # region AddProductType
        public DataSet AddProductType(object[] ProductTypeInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[8];
                SqlDbType[] DataType = new SqlDbType[8];
                Object[] Values = new object[8];
                int i = 0;

                //0
                ColumnNames.SetValue("ProdType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("IsActive", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("createdOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("createdBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("ProductDescription", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
                i++;

                //5
                ColumnNames.SetValue("IsMail", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("IsShipper", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Priority", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
           
                DataSet ds = da.SelectRecords("spAddProductType", ColumnNames, Values, DataType);
                return ds;
                //if (!da.ExecuteProcedure("spAddProductType", ColumnNames, DataType, Values))
                //    return (-1);
                //else
                //{
                //    return (0);
                //}
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        # endregion AddRegion

        #region Save Product Type Config
        public int SaveProductTypeConfig(object[] ProductTypeInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[20];
                SqlDbType[] DataType = new SqlDbType[20];
                Object[] Values = new object[20];
                int i = 0;
                
                //0
                ColumnNames.SetValue("SerialNumber", i);
                DataType.SetValue(SqlDbType.BigInt, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("ProdType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("OriginCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("DestCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("FlightNum", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
                i++;

                //5
                ColumnNames.SetValue("FlightDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
                i++;

                //6
                ColumnNames.SetValue("DayOfWeek", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
                i++;

                //7
                ColumnNames.SetValue("Month", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
                i++;

                //8
                ColumnNames.SetValue("IsActive", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
                i++;

                //9
                ColumnNames.SetValue("CreatedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
                i++;

                //10
                ColumnNames.SetValue("CreatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
                i++;

                //11
                ColumnNames.SetValue("UpdatedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
                i++;

                //12
                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
                i++;

                //13
                ColumnNames.SetValue("FromDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
                i++;

                //14
                ColumnNames.SetValue("ToDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
                i++;

                //15
                ColumnNames.SetValue("AllocatedCapacity", i);
                DataType.SetValue(SqlDbType.Decimal, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
                i++;

                //16
                ColumnNames.SetValue("CommCategory", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
                i++;

                //17
                ColumnNames.SetValue("CommCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
                i++;

                //18
                ColumnNames.SetValue("CapacityThreshold", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
                i++;

                //19
                ColumnNames.SetValue("Priority", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ProductTypeInfo.GetValue(i), i);
                i++;

                if (!da.ExecuteProcedure("spSaveProductTypeConfig", ColumnNames, DataType, Values))
                    return (-1);
                else
                {
                    return (0);
                }
            }
            catch (Exception ex)
            {
                return (-1);
            }

        }
        # endregion AddRegion

        #region Update Product Type
        public int UpdateProductType(object[] UpdateProductTypeInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[9];
                SqlDbType[] DataType = new SqlDbType[9];
                Object[] Values = new object[9];
                int i = 0;

                //0
                ColumnNames.SetValue("prodtype",i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateProductTypeInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("IsAct", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(UpdateProductTypeInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("CreationTime", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(UpdateProductTypeInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("CreatedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateProductTypeInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("serNum", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(UpdateProductTypeInfo.GetValue(i), i);
                i++;
                
                //5
                ColumnNames.SetValue("ProductDescription", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateProductTypeInfo.GetValue(i), i);
                i++;

                //6
                ColumnNames.SetValue("IsMail", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(UpdateProductTypeInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("IsShipper", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(UpdateProductTypeInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Priority", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(UpdateProductTypeInfo.GetValue(i), i);
           


                if (!da.ExecuteProcedure("spUpdateProduct", ColumnNames, DataType, Values))
                    return (-1);
                else
                {
                    return (0);
                }
            }
            catch (Exception ex)
            {
                return (-1);
            }

        }
        # endregion UpdateCommodity

        #region Update Product Type Config
        public int UpdateProductTypeConfig(object[] UpdateProductTypeInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[5];
                SqlDbType[] DataType = new SqlDbType[5];
                Object[] Values = new object[5];
                int i = 0;

                //0
                ColumnNames.SetValue("prodtype", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateProductTypeInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("IsAct", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(UpdateProductTypeInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("CreationTime", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(UpdateProductTypeInfo.GetValue(i), i);
                i++;



                //3
                ColumnNames.SetValue("CreatedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateProductTypeInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("serNum", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(UpdateProductTypeInfo.GetValue(i), i);
                i++;


                if (!da.ExecuteProcedure("spUpdateProduct", ColumnNames, DataType, Values))
                    return (-1);
                else
                {
                    return (0);
                }
            }
            catch (Exception ex)
            {
                return (-1);
            }

        }
        # endregion Update Product Type Config

        #region Get Product Type List
        public DataSet GetProductTypeList(object[] PrefixInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[3];
                SqlDbType[] DataType = new SqlDbType[3];
                Object[] Values = new object[3];
                int i = 0;

                //1
                ColumnNames.SetValue("prodtype", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("proddesc", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("priority", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;
                
                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetProdutList",ColumnNames, Values, DataType);
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
        # endregion Get Product Type List

        #region Get Product Types
        public DataSet GetProductTypes()
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                DataSet ds = new DataSet();
                ds = da.SelectRecords("SP_GetProductType");
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
        # endregion Get Product Type List

        #region Get Partner Codes
        public DataSet GetPartnerCodes()
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                DataSet ds = new DataSet();
                ds = da.SelectRecords("SP_GetPartnerCodeName","AddSelectRow","true", SqlDbType.Bit);
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
        #endregion Get Partner Codes

        #region Commodity Category
        public DataSet GetCommodityCategory()
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("spGetCommodityCategory", "ForProductTypeConfig","true", SqlDbType.Bit);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return (ds);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return (null);
        }
        #endregion

        #region Get Product Type Config List
        public DataSet GetProductTypeConfigList(object[] PrefixInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[9];
                SqlDbType[] DataType = new SqlDbType[9];
                Object[] Values = new object[9];
                int i = 0;

                //1
                ColumnNames.SetValue("prodtype", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("OriginCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("DestCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("FlightNum", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //5
                ColumnNames.SetValue("FromDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //6
                ColumnNames.SetValue("ToDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //7
                ColumnNames.SetValue("CommCategory", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //8
                ColumnNames.SetValue("CommCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //9
                ColumnNames.SetValue("Priority", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);

                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetProdutTypeConfigList", ColumnNames, Values, DataType);
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
        # endregion Get Product Type Config List

        //Added 26th Sept..Unit Testing done
        # region Delete
        public int DeleteProdTypeDetail(object[] PrefixInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;

                //1
                ColumnNames.SetValue("srno", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;


                if (!da.ExecuteProcedure("spDeleteProdTypeDetail", ColumnNames, DataType, Values))
                    return (-1);
                else
                {
                    return (0);
                }
            }
            catch (Exception ex)
            {
                return (-1);
            }

        }
        # endregion Delete

        #region Delete Product Type Config
        public int DeleteProdTypeConfig(Int64 intSerialNumber)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                if (!da.ExecuteProcedure("spDeleteProductTypeConfig", "srno", SqlDbType.BigInt, intSerialNumber))
                    return (-1);
                else
                {
                    return (0);
                }
            }
            catch (Exception ex)
            {
                return (-1);
            }

        }
        # endregion Delete Product Type Config

        #region Get Matching Product Type
        /// <summary>
        /// Gets Product Type matching with the combination of values supplied.
        /// </summary>
        /// <param name="origin">Can be set to "" if to be ignored.</param>
        /// <param name="destination">Can be set to "" if to be ignored.</param>
        /// <param name="flightnum">Can be set to "" if to be ignored.</param>
        /// <param name="flightdate">Can be set to "" if to be ignored.</param>
        /// <param name="commcategory">Can be set to "" if to be ignored.</param>
        /// <param name="commcode">Can be set to "" if to be ignored.</param>
        /// <param name="priority">Can be set to "" if to be ignored.</param>
        /// <param name="DayOfWeek">Can be set to "" if to be ignored.</param>
        /// <param name="Month">Can be set to "" if to be ignored.</param>
        /// <param name="weight">Can be set to 0 if to be ignored.</param>
        /// <param name="shipDate">Can be set to "" if to be ignored.</param>
        /// <returns>Dataset having product type details along with applicable rate per kg</returns>
        public DataSet GetMatchingProductType(string origin, string destination, string flightnum, string flightdate,
            string commcategory, string commcode, decimal weight, string shipDate)
        {
            try
            {
                
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[8];
                SqlDbType[] DataType = new SqlDbType[8];
                Object[] Values = new object[8];
                int i = 0;

                //1
                ColumnNames.SetValue("OriginCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(origin, i);
                i++;

                //2
                ColumnNames.SetValue("DestCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(destination, i);
                i++;

                //3
                ColumnNames.SetValue("FlightNum", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(flightnum, i);
                i++;

                //4
                ColumnNames.SetValue("FlightDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(flightdate, i);
                i++;

                //5
                ColumnNames.SetValue("CommCategory", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(commcategory, i);
                i++;

                //6
                ColumnNames.SetValue("CommCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(commcode, i);
                i++;

                //7
                ColumnNames.SetValue("Weight", i);
                DataType.SetValue(SqlDbType.Decimal, i);
                Values.SetValue(weight, i);
                i++;

                //8
                ColumnNames.SetValue("ShipmentDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(shipDate, i);

                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetConfiguredProductTypes", ColumnNames, Values, DataType);
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
        # endregion Get Matching Product Type

    }
}
