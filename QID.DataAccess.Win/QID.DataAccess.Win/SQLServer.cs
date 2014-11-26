/**********************************************************
 * Object Name: QID.DataAccess.SQLServer
 * Prepared By: Vishal Keshav Tillu
 * Prepared On: 27 Jul 2011
 * Description: This class provides for performing operations 
 *              related to SQLServer database from Win/ Web application.
 * Changed By: 
 * Changed On:
 * Change Description: 
***********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace QID.DataAccess
{

    /// <summary>
    /// Class to communicate with SQLServer database.1
    /// </summary>
    public class SQLServer
    {
        
        #region Variables
        private SqlConnection con = new SqlConnection();
        private DataSet ds = new DataSet("DASQLServer");        
        /// <summary>
        /// Gets last error description (if any) during execution of a function.
        /// </summary>
        public string LastErrorDescription = "";
        #endregion Variables

        #region Constructor
        /// <summary>
        /// Constructor of SQLServer class.
        /// </summary>
        /// <param name="ConnectionString">
        /// Connection string to be used for connecting to SQLServer database.
        /// </param>
        public SQLServer(string ConnectionString)
        {
            con.ConnectionString = ConnectionString;

        }
        #endregion Constructor

        #region GetData
        /// <summary>
        /// Gets data in data reader object from database based on SelectQuery.
        /// </summary>
        /// <param name="SelectQuery">Select Query to retrieve data from database.</param>
        /// <returns>SqlDataReader containing data returned by query.
        /// \n\rPlease make sure you dispose datareader object after reading data in
        /// your application.
        /// </returns>
        public SqlDataReader GetData(string SelectQuery)
        {
            LastErrorDescription = "";
            SqlDataReader dr = null;
            try
            {                
                if (con.State != ConnectionState.Open)
                    con.Open();
                SqlCommand cmd = new SqlCommand(SelectQuery);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();
                cmd.Dispose();
                return (dr);
            }
            catch (Exception ex)
            {
                LastErrorDescription = "SQLServer.GetData: " + ex.Message;
                if (dr != null)
                    dr.Dispose();
                return (null);
            }            
        }
        #endregion

        #region Get String
        /// <summary>
        /// Gets string result form database based on SelectQuery.
        /// </summary>
        /// <param name="SelectQuery">Select Query to retrieve data from database.</param>
        /// <returns>Single string data returned by query.</returns>
        public string GetString(string SelectQuery)
        {
            LastErrorDescription = "";
            SqlDataReader dr = null;
            try
            {
                string strGetString;
                dr = GetData(SelectQuery);
                if (dr == null)
                    return (null);
                //If data found.
                if (dr.Read())
                    strGetString = dr[0].ToString();
                else
                    strGetString = "";
                //Dispose data reader.
                dr.Dispose();
                return (strGetString);
            }
            catch (Exception ex)
            {
                LastErrorDescription = "SQLServer.GetString: " + ex.Message;
                return (null);
            }
            finally
            {
                if (dr != null)
                    dr.Dispose();
            }
        }
        #endregion

        #region Get String By Procedure Multiple Param
        /// <summary>
        /// Gets String result from database based on StoredProcedure.
        /// </summary>
        /// <param name="SelectQuery">Select Query to retrieve data from database.</param>
        /// <returns>Single string data returned by query.</returns>
        public string GetStringByProcedure(string ProcedureName, string[] QueryPName, object[] QueryPValues, SqlDbType[] QueryPTypes)
        {
            LastErrorDescription = "";
            DataSet ds = null;
            try
            {
                string strGetString = "";
                ds = SelectRecords(ProcedureName, QueryPName, QueryPValues, QueryPTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            strGetString = ds.Tables[0].Rows[0][0].ToString();
                        }
                    }
                    ds.Dispose();
                }
                else
                {
                    strGetString = "";
                }
                return (strGetString);
            }
            catch (Exception ex)
            {
                LastErrorDescription = "SQLServer.GetStringByProcedure: " + ex.Message;
                return (null);
            }
            finally
            {
                QueryPName = null;
                QueryPValues = null;
                QueryPTypes = null;

                if (ds != null)
                    ds.Dispose();
            }
            
        }
        #endregion

        #region Get String By Procedure Single Parameter
        /// <summary>
        /// Gets String result from database based on StoredProcedure.
        /// </summary>
        ///<param name="ProcedureName">Name of stored procedure to execute.</param>
        ///<param name="QueryPName">Name of single parameter to be passed to stored procedure.</param>
        ///<param name="QueryPTypes">Type of single parameter to be passed to stored procedure.</param>
        ///<param name="QueryPValues">Value of single parameter to be passed to stored procedure.</param>
        /// <returns>Single String value returned by query.</returns>
        public string GetStringByProcedure(string ProcedureName, string QueryPName, object QueryPValues, SqlDbType QueryPTypes)
        {
            LastErrorDescription = "";
            try
            {
                string strGetString = "";
                //Execute select procedure with single parameter.
                DataSet ds = SelectRecords(ProcedureName, QueryPName, QueryPValues, QueryPTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            strGetString = ds.Tables[0].Rows[0][0].ToString();
                        }
                    }
                    ds.Dispose();
                }
                else
                {
                    strGetString = "";
                }
                return (strGetString);
            }
            catch (Exception ex)
            {
                LastErrorDescription = "SQLServer.GetStringByProcedure: " + ex.Message;
                return (null);
            }
            finally
            {
                QueryPName = null;
                QueryPValues = null;                

                if (ds != null)
                    ds.Dispose();
            }
           
        }
        #endregion Get Integer By Procedure With Single Parameter

        #region Get Integer By Query
        /// <summary>
        /// Gets int result from database based on SelectQuery.
        /// </summary>
        /// <param name="SelectQuery">Select Query to retrieve data from database.</param>
        /// <returns>Single int data returned by query.</returns>
        public int GetInteger(string SelectQuery)
        {
            LastErrorDescription = "";
            SqlDataReader dr = null;
            try
            {
                int intGetInteger = 0;
                dr = GetData(SelectQuery);
                if (dr != null)
                {
                    if (dr.Read())
                    {
                        if (dr[0].ToString() == null || dr[0].ToString() == "")
                            intGetInteger = 0;
                        else
                            intGetInteger = Convert.ToInt32(dr[0].ToString());
                    }
                    else
                    {
                        intGetInteger = 0;
                    }
                    //Dispose data reader.
                    dr.Dispose();
                }
                return (intGetInteger);
            }
            catch (Exception ex)
            {
                LastErrorDescription = "SQLServer.GetInteger: " + ex.Message;
                return (0);
            }
            finally
            {
                if (dr != null)
                    dr.Dispose();
            }
        }
        #endregion

        #region Get Integer By Procedure
        /// <summary>
        /// Gets int result from database based on StoredProcedure.
        /// </summary>
        /// <param name="SelectQuery">Select Query to retrieve data from database.</param>
        /// <returns>Single int data returned by query.</returns>
        public int GetIntegerByProcedure(string ProcedureName, string[] QueryPName, object[] QueryPValues, SqlDbType[] QueryPTypes)
        {
            LastErrorDescription = "";
            DataSet ds = null;
            try
            {
                int intGetInteger = 0;
                ds = SelectRecords(ProcedureName, QueryPName, QueryPValues, QueryPTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            intGetInteger = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                        }
                    }
                    ds.Dispose();
                }
                else
                {
                    intGetInteger = 0;
                }
                return (intGetInteger);
            }
            catch (Exception ex)
            {
                LastErrorDescription = "SQLServer.GetIntegerByProcedure: " + ex.Message;
                return (0);
            }
            finally
            {
                QueryPName = null;
                QueryPValues = null;
                QueryPTypes = null;

                if (ds != null)
                    ds.Dispose();
            }
        }
        #endregion

        #region Get Integer By Procedure With Single Parameter
        /// <summary>
        /// Gets int result from database based on StoredProcedure.
        /// </summary>
        ///<param name="ProcedureName">Name of stored procedure to execute.</param>
        ///<param name="QueryPName">Name of single parameter to be passed to stored procedure.</param>
        ///<param name="QueryPTypes">Type of single parameter to be passed to stored procedure.</param>
        ///<param name="QueryPValues">Value of single parameter to be passed to stored procedure.</param>
        /// <returns>Single int data returned by query.</returns>
        public int GetIntegerByProcedure(string ProcedureName, string QueryPName, object QueryPValues, SqlDbType QueryPTypes)
        {
            LastErrorDescription = "";
            DataSet ds = null;
            try
            {
                int intGetInteger = 0;
                //Execute select procedure with single parameter.
                ds = SelectRecords(ProcedureName, QueryPName, QueryPValues, QueryPTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            intGetInteger = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                        }
                    }
                    ds.Dispose();
                }
                else
                {
                    intGetInteger = 0;
                }
                return (intGetInteger);
            }
            catch (Exception ex)
            {
                LastErrorDescription = "SQLServer.GetIntByProcSingParam: " + ex.Message;
                return (0);
            }
            finally
            {
                QueryPName = null;
                QueryPValues = null;
                
                if (ds != null)
                    ds.Dispose();
            }
        }
        #endregion Get Integer By Procedure With Single Parameter

        #region Get Boolean
        /// <summary>
        /// Gets boolean result from database for presence of data for given SelectQuery.
        /// </summary>
        /// <param name="SelectQuery">Select Query to check presence of data in database.</param>
        /// <returns>True if data is present else False.</returns>
        public bool GetBoolean(string SelectQuery)
        {
            LastErrorDescription = "";
            SqlDataReader dr = null;
            try
            {
                bool blnGetBoolean;
                dr = GetData(SelectQuery);
                if (dr == null)
                {
                    blnGetBoolean = false;
                }
                else
                {
                    if (dr.Read())
                    {
                        blnGetBoolean = true;
                    }
                    else
                    {
                        blnGetBoolean = false;
                    }
                    //Dispose data reader.
                    dr.Dispose();
                }
                return (blnGetBoolean);
            }
            catch (Exception ex)
            {
                LastErrorDescription = "SQLServer.GetBoolean: " + ex.Message;
                return (false);
            }
            finally
            {
                if (dr != null)
                    dr.Dispose();
            }
        }
        #endregion

        #region Get Dataset
        /// <summary>
        /// Gets dataset containing one table and result of query.
        /// </summary>
        /// <param name="Query">Select Query.</param>
        /// <returns>Dataset with one data table.</returns>
        public DataSet GetDataset(string Query)
        {
            LastErrorDescription = "";
            SqlDataAdapter da = null;
            DataSet ds = new DataSet();
            try
            {
                if (Query.Length <= 0 || Query.ToUpper().Contains("UPDATE ") || Query.ToUpper().Contains("INSERT ") || Query.ToUpper().Contains("DELETE "))
                {
                    return (null);
                }
                da = new SqlDataAdapter(Query, con);
                
                da.Fill(ds);
                da.Dispose();
                return (ds);
            }
            catch (Exception ex)
            {
                LastErrorDescription = "SQLServer.GetDataset: " + ex.Message;
                ds = null;
                return (null);
            }
            finally
            {
                if (da != null)
                    da.Dispose();
            }
        }
        #endregion

        #region Update Data With Query
        /// <summary>
        /// Updates data in database based on given UpdateQuery.
        /// </summary>
        /// <param name="UpdateQuery">Update Query to update data in database.</param>
        /// <returns>True if update is successful else False.</returns>
        public bool UpdateData(string UpdateQuery)
        {
            LastErrorDescription = "";
            SqlCommand cmd = null;
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                cmd = new SqlCommand(UpdateQuery);
                cmd.Connection = con;
                //Execute query.
                cmd.ExecuteNonQuery();
                //Dispose command object.
                cmd.Dispose();
                con.Close();
                return (true);  //Return true if query executed without errors.
            }
            catch (Exception ex)
            {
                LastErrorDescription = "SQLServer.UpdateDataQuery: " + ex.Message;                
                return (false);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                
                if (con.State == ConnectionState.Open)
                    con.Close();                
            }
        }
        #endregion Update Data With Query

        #region Update Data With Procedure Multiple Param
        /// <summary>
        /// Updates data in database through specified stored procedure & multiple parameters.
        /// </summary>
        /// <param name="Procedure">Name of stored procedure to be executed.</param>
        /// <param name="ColumnNames">Array containing Parameter names.</param>
        /// <param name="DataType">Array containing Data type of parameters.</param>
        /// <param name="Values">Values of parameters.</param>
        /// <returns>True if update successful.</returns>
        public bool UpdateData(string Procedure, string[] ColumnNames, SqlDbType[] DataType, object[] Values)
        {
            LastErrorDescription = "";
            SqlCommand cmd = null;
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                //Add parameter names, data types & values for command.
                for (int i = 0; i < ColumnNames.Length; i++)
                {
                    cmd.Parameters.Add("@" + ColumnNames.GetValue(i).ToString(), (SqlDbType)DataType.GetValue(i));
                    cmd.Parameters[i].Value = Values.GetValue(i);
                }
                cmd.Connection = con;
                cmd.CommandText = Procedure;
                //Execute query and dispose command object.
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                return (true);                
            }
            catch (Exception ex)
            {
                LastErrorDescription = "SQLServer.UpdateDataProc: " + ex.Message;                
                return (false);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

                if (con.State == ConnectionState.Open)
                    con.Close();
                ColumnNames = null;
                DataType = null;
                Values = null;
            }
        }

        public bool UpdateData(string Procedure, string[] ColumnNames, SqlDbType[] DataType, object[] Values, ref string ErrorMessage)
        {
            LastErrorDescription = "";
            SqlCommand cmd = null;
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                //Add parameter names, data types & values for command.
                for (int i = 0; i < ColumnNames.Length; i++)
                {
                    cmd.Parameters.Add("@" + ColumnNames.GetValue(i).ToString(), (SqlDbType)DataType.GetValue(i));
                    cmd.Parameters[i].Value = Values.GetValue(i);
                }
                cmd.Connection = con;
                cmd.CommandText = Procedure;
                //Execute query and dispose command object.
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                return (true);
            }
            catch (Exception ex)
            {
                LastErrorDescription = "SQLServer.UpdateDataProc: " + ex.Message;
                ErrorMessage = ex.Message;
                return (false);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

                if (con.State == ConnectionState.Open)
                    con.Close();
                ColumnNames = null;
                DataType = null;
                Values = null;
            }
        }

        #endregion

        #region Update Data With Procedure Single Parameter
        /// <summary>
        /// Updates data in database through specified stored procedure & single parameter.
        /// </summary>
        /// <param name="Procedure">Name of stored procedure to be executed.</param>
        /// <param name="ColumnName">Parameter name.</param>
        /// <param name="DataType">Data type of parameter.</param>
        /// <param name="Values">Value of parameter.</param>
        /// <returns>True if update successful.</returns>
        public bool UpdateData(string Procedure, string ColumnName, SqlDbType DataType, object Value)
        {
            LastErrorDescription = ""; SqlCommand cmd = null;
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                //Add parameter name, data types & value for command.                
                cmd.Parameters.Add("@" + ColumnName, (SqlDbType)DataType);
                cmd.Parameters[0].Value = Value;

                cmd.Connection = con;
                cmd.CommandText = Procedure;

                //Execute query and dispose command object.
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                return (true);
            }
            catch (Exception ex)
            {
                LastErrorDescription = "SQLServer.UpdateDataSingParamProc: " + ex.Message;
                return (false);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

                if (con.State == ConnectionState.Open)
                    con.Close();
                
                Value = null;
            }
        }
        #endregion

        #region Insert Data Query
        /// <summary>
        /// Inserts data in database based on given InsertQuery.
        /// </summary>
        /// <param name="InsertQuery">Insert Query to Insert data in database.</param>
        /// <returns>True if Insert is successful else False.</returns>
        public bool InsertData(string InsertQuery)
        {
            LastErrorDescription = "";
            SqlCommand cmd = null;
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                cmd = new SqlCommand(InsertQuery);
                cmd.Connection = con;
                //Execute query.
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                return (true);
            }
            catch (Exception ex)
            {
                LastErrorDescription = "SQLServer.InsertDataQuery: " + ex.Message;                
                return (false);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
        #endregion Insert Data Query

        #region Insert Data Stored Procedure Multiple Param
        /// <summary>
        /// Inserts data in database through stored procedure for mulitple parameters.
        /// </summary>
        /// <param name="Procedure">Name of stored procedure to be executed.</param>
        /// <param name="ColumnNames">Columns in which data is to be inserted.</param>
        /// <param name="DataType">Data type of respective columns.</param>
        /// <param name="Values">Value to be inserted in each column.</param>
        /// <returns>True if insert successful.</returns>
        public bool InsertData(string Procedure, string[] ColumnNames, SqlDbType[] DataType, object[] Values)
        {
            LastErrorDescription = "";
            SqlCommand cmd = null;
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                //Add parameter names, data types & values for command.
                for (int i = 0; i < ColumnNames.Length; i++)
                {
                    cmd.Parameters.Add("@" + ColumnNames.GetValue(i).ToString(), (SqlDbType)DataType.GetValue(i));
                    cmd.Parameters[i].Value = Values.GetValue(i);
                }
                cmd.Connection = con;
                cmd.CommandText = Procedure;
                //Execute query.
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                return (true);
            }
            catch (Exception ex)
            {
                LastErrorDescription = "SQLServer.InsertDataProc: " + ex.Message;                
                return (false);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

                if (con.State == ConnectionState.Open)
                    con.Close();
                DataType = null;
                Values = null;
                ColumnNames = null;
            }
        }
        #endregion Insert Data Stored Procedure Multiple Param

        #region Execute Procedure Multiple Param
        /// <summary>
        /// Execute stored procedure with parameters specified in arrays.
        /// </summary>
        /// <param name="Procedure">Name of stored procedure to execute.</param>
        /// <param name="ColumnNames">Array containing Names of parameters without @</param>
        /// <param name="DataType">Data type of respective parameter specified in ColumnNames array.</param>
        /// <param name="Values">Value of respective parameter specified in ColumnNames array.</param>
        /// <returns>True if execute succeded.</returns>
        public bool ExecuteProcedure(string Procedure, string[] ColumnNames, SqlDbType[] DataType, object[] Values)
        {
            LastErrorDescription = "";
            SqlCommand cmd = null;
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                //Add parameter names, data types & values to command.
                for (int i = 0; i < ColumnNames.Length; i++)
                {
                    cmd.Parameters.Add("@" + ColumnNames.GetValue(i).ToString(), (SqlDbType)DataType.GetValue(i));
                    cmd.Parameters[i].Value = Values.GetValue(i);
                }

                cmd.Connection = con;
                cmd.CommandText = Procedure;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                return (true);                
            }
            catch (Exception ex)
            {
                LastErrorDescription = "SQLServer.ExecuteProcedure: " + ex.Message;                
                return (false);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

                if (con.State == ConnectionState.Open)
                    con.Close();
                DataType = null;
                Values = null;
                ColumnNames = null;
            }
        }
        #endregion Execute Procedure Multiple Param

        #region Execute Procedure Single Param
        /// <summary>
        /// Execute stored procedure with single parameter.
        /// </summary>
        /// <param name="Procedure">Name of stored procedure to execute.</param>
        /// <param name="ColumnName">Name of parameter without @</param>
        /// <param name="DataType">Data type of parameter.</param>
        /// <param name="Value">Value of parameter.</param>
        /// <returns>True if execute succeded.</returns>
        public bool ExecuteProcedure(string Procedure, string ColumnName, SqlDbType DataType, object Value)
        {
            LastErrorDescription = "";
            SqlCommand cmd = null;
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                ColumnName = "@" + ColumnName;

                cmd.Parameters.Add(ColumnName, DataType);
                cmd.Parameters[ColumnName].Value = Value;

                cmd.Connection = con;
                cmd.CommandText = Procedure;
                if (cmd.ExecuteNonQuery() > 0)
                {
                    cmd.Dispose();
                    return (true);
                }
                else
                {
                    cmd.Dispose();
                    return (true);
                }

            }
            catch (Exception ex)
            {
                LastErrorDescription = "SQLServer.ExecuteProcedureSingParam: " + ex.Message;                
                return (false);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

                if (con.State == ConnectionState.Open)
                    con.Close();
                Value = null;
            }
        }
        #endregion Execute Procedure Single Param

        #region Execute Procedure No Param
        /// <summary>
        /// Execute stored procedure with single parameters.
        /// </summary>
        /// <param name="Procedure">Name of stored procedure to execute.</param>
        /// <returns>True if execute succeded.</returns>
        public bool ExecuteProcedure(string Procedure)
        {
            LastErrorDescription = "";
            SqlCommand cmd = null;
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.CommandText = Procedure;
                
                //Execute query.
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                return (true);
            }
            catch (Exception ex)
            {
                LastErrorDescription = "SQLServer.ExecuteProcedureNoParam: " + ex.Message;                                
                return (false);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
        #endregion

        #region Select Store Procedure Records Multi Param
        /// <summary>
        /// Fetch records using Stored Procedure with parameters.
        /// </summary>
        /// <param name="selectProcedure">Stored Procedure name.</param>
        /// <param name="QueryPName">Array containing names of parameters.</param>
        /// <param name="QueryValues">Array containing values of parameters.</param>
        /// <param name="QueryTypes">Array containing data types of parameters.</param>
        /// <returns>Dataset containing records returned by executing stored procedure.</returns>
        public DataSet SelectRecords(string selectProcedure, string[] QueryPName, object[] QueryValues, SqlDbType[] QueryTypes)
        {
            LastErrorDescription = "";
            DataSet dataSet = new DataSet();
            SqlCommand cmd = null;
            SqlDataAdapter adapter = null;
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                adapter = new SqlDataAdapter();
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = selectProcedure;
                cmd.Connection = con;
                adapter.SelectCommand = cmd;
                adapter.SelectCommand.CommandTimeout = 10000;

                //Add parameters to command.
                for (int i = 0; i < QueryPName.Length; i++)
                {
                    cmd.Parameters.Add("@" + QueryPName[i], QueryTypes[i]).Value = QueryValues[i];
                }

                adapter.Fill(dataSet);
            }            
            catch (Exception ex)
            {
                dataSet = null;
                LastErrorDescription = "SQLServer.SelectRecords.2: " + ex.Message + ex.StackTrace;
            }
            finally
            {
                if (adapter != null)
                    adapter.Dispose();

                if (cmd != null)
                    cmd.Dispose();

                if (con.State == ConnectionState.Open)
                    con.Close();
                QueryPName = null;
                QueryTypes = null;
                QueryValues = null;
            }
            return dataSet;
        }
        #endregion Select Store Procedure Records

        #region Select Store Procedure Records No Param
        /// <summary>
        /// Fetch records using Stored Procedure without parameters.
        /// </summary>
        /// <param name="selectProcedure">Name of Stored Procedure.</param>
        /// <returns>Dataset containing records returned by executing stored procedure.</returns>
        public DataSet SelectRecords(string selectProcedure)
        {
            LastErrorDescription = "";
            DataSet dataSet = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand command = new SqlCommand();
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
               
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = selectProcedure;
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = this.con;
                adapter.SelectCommand = command;

                adapter.Fill(dataSet);

            }
            catch (SqlException SqlEx)
            {
                dataSet = null;
                LastErrorDescription = "SQLServer.SelectRecordsNoParam.1: " + SqlEx.Message + SqlEx.StackTrace;
            }
            catch (Exception ex)
            {                
                dataSet = null;
                LastErrorDescription = "SQLServer.SelectRecordsNoParam.2: " + ex.Message;                
            }
            finally
            {
                if (adapter != null)
                    adapter.Dispose();

                if (command != null)
                    command.Dispose();

                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            return dataSet;

        }
        #endregion Select Store Procedure Records  Single Param

        #region Select Store Procedure Records Single Param
        /// <summary>
        /// Fetch records using Stored Procedure with single parameter.
        /// </summary>
        /// <param name="selectProcedure">Stored Procedure name.</param>
        /// <param name="QueryPName">String containing name of parameter.</param>
        /// <param name="QueryValues">Object containing value of parameter.</param>
        /// <param name="QueryTypes">SqlDbType containing data type of parameter.</param>
        /// <returns>Dataset containing records returned by executing stored procedure.</returns>
        public DataSet SelectRecords(string selectProcedure, string QueryPName, object QueryValues, SqlDbType QueryTypes)
        {
            DataSet dataSet = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand();
            LastErrorDescription = "";

            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = selectProcedure;
                cmd.Connection = con;
                adapter.SelectCommand = cmd;

                //Add parameter to command object.
                cmd.Parameters.Add("@" + QueryPName, QueryTypes).Value = QueryValues;

                adapter.Fill(dataSet);
            }
            catch (SqlException SqlEx)
            {
                dataSet = null;
                LastErrorDescription = "SQLServer.SelectRecordsSingParam.1: " + SqlEx.Message;
            }
            catch (Exception ex)
            {
                dataSet = null;
                LastErrorDescription = "SQLServer.SelectRecordsSingParam.2: " + ex.Message;
            }
            finally
            {
                if (adapter != null)
                    adapter.Dispose();

                if (cmd != null)
                    cmd.Dispose();

                if (con.State == ConnectionState.Open)
                    con.Close();
                QueryValues = null;
            }
            return dataSet;

        }

        #endregion Select Store Procedure Records Single Param

        #region Get DataTable
        /// <summary>
        /// Gets dataset containing one table and result of query.
        /// </summary>
        /// <param name="Query">Select Query.</param>
        /// <returns>Dataset with one data table.</returns>
        public DataSet GetDataTable(string TableName, string Query, ref DataSet ds)
        {
            LastErrorDescription = "";
            SqlDataAdapter da = null;
            try
            {
                da = new SqlDataAdapter(Query, con);
                ds.Tables.Add(TableName);
                da.Fill(ds.Tables[TableName]);
                da.Dispose();
            }
            catch (Exception ex)
            {
                LastErrorDescription = "SQLServer.GetDataTable: " + ex.Message;
            }
            finally
            {
                if (da != null)
                    da.Dispose();          
            }
            return (ds);
        }
        #endregion

    }
}