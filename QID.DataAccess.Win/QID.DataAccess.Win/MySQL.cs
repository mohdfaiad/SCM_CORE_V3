///**********************************************************
// * Object Name: QID.DataAccess.MySQL
// * Prepared By: Vishal Keshav Tillu
// * Prepared On: 27 Jul 2011
// * Description: This class provides for performing operations 
// *              related to MySQL database from Win/ HH/ Web application.
// * Changed By: 
// * Changed On:
// * Change Description: 
//***********************************************************/
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Data;
//using MySql.Data.MySqlClient;

//namespace QID.DataAccess
//{
    
//    /// <summary>
//    /// Class to communicate with MySQL database.
//    /// </summary>
//    public class MySQL
//    {
//        #region Variables
//        MySqlConnection con = new MySqlConnection();
//        private DataSet ds = new DataSet("DAMYSQL");        
//        /// <summary>
//        /// Gets last error description (if any) during execution of a function.
//        /// </summary>
//        public string LastErrorDescription = "";
//        #endregion Variables

//        #region Constructor
//        /// <summary>
//        /// Constructor of SQLServer class.
//        /// </summary>
//        /// <param name="ConnectionString">
//        /// Connection string to be used for connecting to MySQL database.
//        /// </param>
//        public MySQL(string ConnectionString)
//        {            
//            con.ConnectionString = ConnectionString;
//        }
//        #endregion Constructor

//        #region GetData
//        /// <summary>
//        /// Gets data in data reader object from database based on SelectQuery.
//        /// </summary>
//        /// <param name="SelectQuery">Select Query to retrieve data from database.</param>
//        /// <returns>SqlDataReader containing data returned by query.
//        /// \n\rPlease make sure you dispose datareader object after reading data in
//        /// your application.
//        /// </returns>
//        public MySqlDataReader GetData(string SelectQuery)
//        {
//            LastErrorDescription = "";
//            try
//            {                
//                if (con.State != ConnectionState.Open)
//                    con.Open();
//                MySqlCommand cmd = new MySqlCommand(SelectQuery);
//                cmd.Connection = con;
//                MySqlDataReader dr = cmd.ExecuteReader();
//                cmd.Dispose();
//                return (dr);
//            }
//            catch (Exception ex)
//            {
//                LastErrorDescription = "MySQL.GetData: " + ex.Message;
//                return (null);
//            }
//        }
//        #endregion

//        #region Get String
//        /// <summary>
//        /// Gets string result form database based on SelectQuery.
//        /// </summary>
//        /// <param name="SelectQuery">Select Query to retrieve data from database.</param>
//        /// <returns>Single string data returned by query.</returns>
//        public string GetString(string SelectQuery)
//        {
//            LastErrorDescription = "";
//            try
//            {
//                string strGetString;
//                MySqlDataReader dr = GetData(SelectQuery);
//                if (dr == null)
//                    return (null);
//                //If data found.
//                if (dr.Read())
//                    strGetString = dr[0].ToString();
//                else
//                    strGetString = "";
//                //Dispose data reader.
//                dr.Dispose();
//                return (strGetString);
//            }
//            catch (Exception ex)
//            {
//                LastErrorDescription = "MySQL.GetString: " + ex.Message;
//                return (null);
//            }
//        }
//        #endregion

//        #region Get String By Procedure Multiple Param
//        /// <summary>
//        /// Gets String result from database based on StoredProcedure.
//        /// </summary>
//        /// <param name="SelectQuery">Select Query to retrieve data from database.</param>
//        /// <returns>Single string data returned by query.</returns>
//        public string GetStringByProcedure(string ProcedureName, string[] QueryPName, object[] QueryPValues, SqlDbType[] QueryPTypes)
//        {
//            LastErrorDescription = "";
//            try
//            {
//                string strGetString = "";
//                DataSet ds = SelectRecords(ProcedureName, QueryPName, QueryPValues, QueryPTypes);
//                if (ds != null)
//                {
//                    if (ds.Tables.Count > 0)
//                    {
//                        if (ds.Tables[0].Rows.Count > 0)
//                        {
//                            strGetString = ds.Tables[0].Rows[0][0].ToString();
//                        }
//                    }
//                    ds.Dispose();
//                }
//                else
//                {
//                    strGetString = "";
//                }
//                return (strGetString);
//            }
//            catch (Exception ex)
//            {
//                LastErrorDescription = "MySQL.GetStringByProcedure: " + ex.Message;
//                return (null);
//            }
//        }
//        #endregion

//        #region Get String By Procedure Single Parameter
//        /// <summary>
//        /// Gets String result from database based on StoredProcedure.
//        /// </summary>
//        ///<param name="ProcedureName">Name of stored procedure to execute.</param>
//        ///<param name="QueryPName">Name of single parameter to be passed to stored procedure.</param>
//        ///<param name="QueryPTypes">Type of single parameter to be passed to stored procedure.</param>
//        ///<param name="QueryPValues">Value of single parameter to be passed to stored procedure.</param>
//        /// <returns>Single String value returned by query.</returns>
//        public string GetStringByProcedure(string ProcedureName, string QueryPName, object QueryPValues, SqlDbType QueryPTypes)
//        {
//            LastErrorDescription = "";
//            try
//            {
//                string strGetString = "";
//                //Execute select procedure with single parameter.
//                DataSet ds = SelectRecords(ProcedureName, QueryPName, QueryPValues, QueryPTypes);
//                if (ds != null)
//                {
//                    if (ds.Tables.Count > 0)
//                    {
//                        if (ds.Tables[0].Rows.Count > 0)
//                        {
//                            strGetString = ds.Tables[0].Rows[0][0].ToString();
//                        }
//                    }
//                    ds.Dispose();
//                }
//                else
//                {
//                    strGetString = "";
//                }
//                return (strGetString);
//            }
//            catch (Exception ex)
//            {
//                LastErrorDescription = "MySQL.GetStringByProcedure: " + ex.Message;
//                return (null);
//            }
//        }
//        #endregion Get Integer By Procedure With Single Parameter

//        #region Get Integer By Query
//        /// <summary>
//        /// Gets int result from database based on SelectQuery.
//        /// </summary>
//        /// <param name="SelectQuery">Select Query to retrieve data from database.</param>
//        /// <returns>Single int data returned by query.</returns>
//        public int GetInteger(string SelectQuery)
//        {
//            LastErrorDescription = "";
//            try
//            {
//                int intGetInteger = 0;
//                MySqlDataReader dr = GetData(SelectQuery);
//                if (dr != null)
//                {
//                    if (dr.Read())
//                    {
//                        if (dr[0].ToString() == null || dr[0].ToString() == "")
//                            intGetInteger = 0;
//                        else
//                            intGetInteger = Convert.ToInt32(dr[0].ToString());
//                    }
//                    else
//                    {
//                        intGetInteger = 0;
//                    }
//                    //Dispose data reader.
//                    dr.Dispose();
//                }
//                return (intGetInteger);
//            }
//            catch (Exception ex)
//            {
//                LastErrorDescription = "MySQL.GetInteger: " + ex.Message;
//                return (0);
//            }
//        }
//        #endregion

//        #region Get Integer By Procedure
//        /// <summary>
//        /// Gets int result from database based on StoredProcedure.
//        /// </summary>
//        /// <param name="SelectQuery">Select Query to retrieve data from database.</param>
//        /// <returns>Single int data returned by query.</returns>
//        public int GetIntegerByProcedure(string ProcedureName, string[] QueryPName, object[] QueryPValues, SqlDbType[] QueryPTypes)
//        {
//            LastErrorDescription = "";
//            try
//            {
//                int intGetInteger = 0;
//                DataSet ds = SelectRecords(ProcedureName, QueryPName, QueryPValues, QueryPTypes);
//                if (ds != null)
//                {
//                    if (ds.Tables.Count > 0)
//                    {
//                        if (ds.Tables[0].Rows.Count > 0)
//                        {
//                            intGetInteger = int.Parse(ds.Tables[0].Rows[0][0].ToString());
//                        }
//                    }
//                    ds.Dispose();
//                }
//                else
//                {
//                    intGetInteger = 0;
//                }
//                return (intGetInteger);
//            }
//            catch (Exception ex)
//            {
//                LastErrorDescription = "MySQL.GetIntegerByProcedure: " + ex.Message;
//                return (0);
//            }
//        }
//        #endregion

//        #region Get Integer By Procedure With Single Parameter
//        /// <summary>
//        /// Gets int result from database based on StoredProcedure.
//        /// </summary>
//        ///<param name="ProcedureName">Name of stored procedure to execute.</param>
//        ///<param name="QueryPName">Name of single parameter to be passed to stored procedure.</param>
//        ///<param name="QueryPTypes">Type of single parameter to be passed to stored procedure.</param>
//        ///<param name="QueryPValues">Value of single parameter to be passed to stored procedure.</param>
//        /// <returns>Single int data returned by query.</returns>
//        public int GetIntegerByProcedure(string ProcedureName, string QueryPName, object QueryPValues, SqlDbType QueryPTypes)
//        {
//            LastErrorDescription = "";
//            try
//            {
//                int intGetInteger = 0;
//                //Execute select procedure with single parameter.
//                DataSet ds = SelectRecords(ProcedureName, QueryPName, QueryPValues, QueryPTypes);
//                if (ds != null)
//                {
//                    if (ds.Tables.Count > 0)
//                    {
//                        if (ds.Tables[0].Rows.Count > 0)
//                        {
//                            intGetInteger = int.Parse(ds.Tables[0].Rows[0][0].ToString());
//                        }
//                    }
//                    ds.Dispose();
//                }
//                else
//                {
//                    intGetInteger = 0;
//                }
//                return (intGetInteger);
//            }
//            catch (Exception ex)
//            {
//                LastErrorDescription = "MySQL.GetIntByProcSingParam: " + ex.Message;
//                return (0);
//            }
//        }
//        #endregion Get Integer By Procedure With Single Parameter

//        #region Get Boolean
//        /// <summary>
//        /// Gets boolean result from database for presence of data for given SelectQuery.
//        /// </summary>
//        /// <param name="SelectQuery">Select Query to check presence of data in database.</param>
//        /// <returns>True if data is present else False.</returns>
//        public bool GetBoolean(string SelectQuery)
//        {
//            LastErrorDescription = "";
//            try
//            {
//                bool blnGetBoolean;
//                MySqlDataReader dr = GetData(SelectQuery);
//                if (dr == null)
//                {
//                    blnGetBoolean = false;
//                }
//                else
//                {
//                    if (dr.Read())
//                    {
//                        blnGetBoolean = true;
//                    }
//                    else
//                    {
//                        blnGetBoolean = false;
//                    }
//                    //Dispose data reader.
//                    dr.Dispose();
//                }
//                return (blnGetBoolean);
//            }
//            catch (Exception ex)
//            {
//                LastErrorDescription = "MySQL.GetBoolean: " + ex.Message;
//                return (false);
//            }
//        }
//        #endregion

//        #region Get Dataset
//        /// <summary>
//        /// Gets dataset containing one table and result of query.
//        /// </summary>
//        /// <param name="Query">Select Query.</param>
//        /// <returns>Dataset with one data table.</returns>
//        public DataSet GetDataset(string Query)
//        {
//            LastErrorDescription = "";
//            try
//            {
//                if (Query.Length <= 0 || Query.ToUpper().Contains("UPDATE ") || Query.ToUpper().Contains("INSERT ") || Query.ToUpper().Contains("DELETE "))
//                {
//                    return (null);
//                }
//                MySqlDataAdapter da = new MySqlDataAdapter(Query, con);
//                DataSet ds = new DataSet();
//                da.Fill(ds);
//                da.Dispose();
//                return (ds);
//            }
//            catch (Exception ex)
//            {
//                LastErrorDescription = "MySQL.GetDataset: " + ex.Message;                
//                return (null);
//            }
//        }
//        #endregion

//        #region Update Data With Query
//        /// <summary>
//        /// Updates data in database based on given UpdateQuery.
//        /// </summary>
//        /// <param name="UpdateQuery">Update Query to update data in database.</param>
//        /// <returns>True if update is successful else False.</returns>
//        public bool UpdateData(string UpdateQuery)
//        {
//            LastErrorDescription = "";
//            try
//            {
//                if (con.State != System.Data.ConnectionState.Open)
//                    con.Open();
//                MySqlCommand cmd = new MySqlCommand(UpdateQuery);
//                cmd.Connection = con;
//                //Execute query.
//                cmd.ExecuteNonQuery();
//                //Dispose command object.
//                cmd.Dispose();
//                con.Close();
//                return (true);  //Return true if query executed without errors.
//            }
//            catch (Exception ex)
//            {
//                LastErrorDescription = "MySQL.UpdateDataQuery: " + ex.Message;                
//                return (false);
//            }
//        }
//        #endregion Update Data With Query

//        #region Update Data With Procedure Multiple Param
//        /// <summary>
//        /// Updates data in database through specified stored procedure & multiple parameters.
//        /// </summary>
//        /// <param name="Procedure">Name of stored procedure to be executed.</param>
//        /// <param name="ColumnNames">Array containing Parameter names.</param>
//        /// <param name="DataType">Array containing Data type of parameters.</param>
//        /// <param name="Values">Values of parameters.</param>
//        /// <returns>True if update successful.</returns>
//        public bool UpdateData(string Procedure, string[] ColumnNames, SqlDbType[] DataType, object[] Values)
//        {
//            LastErrorDescription = "";
//            try
//            {
//                if (con.State != System.Data.ConnectionState.Open)
//                    con.Open();
//                MySqlCommand cmd = new MySqlCommand();
//                cmd.CommandType = CommandType.StoredProcedure;
//                //Add parameter names, data types & values for command.
//                for (int i = 0; i < ColumnNames.Length; i++)
//                {
//                    cmd.Parameters.Add("@" + ColumnNames.GetValue(i).ToString(), (SqlDbType)DataType.GetValue(i));
//                    cmd.Parameters[i].Value = Values.GetValue(i);
//                }
//                cmd.Connection = con;
//                cmd.CommandText = Procedure;
//                //Execute query and dispose command object.
//                cmd.ExecuteNonQuery();
//                cmd.Dispose();
//                con.Close();
//                return (true);                
//            }
//            catch (Exception ex)
//            {
//                LastErrorDescription = "MySQL.UpdateDataProc: " + ex.Message;                
//                return (false);
//            }
//        }
//        #endregion

//        #region Update Data With Procedure Single Parameter
//        /// <summary>
//        /// Updates data in database through specified stored procedure & single parameter.
//        /// </summary>
//        /// <param name="Procedure">Name of stored procedure to be executed.</param>
//        /// <param name="ColumnName">Parameter name.</param>
//        /// <param name="DataType">Data type of parameter.</param>
//        /// <param name="Values">Value of parameter.</param>
//        /// <returns>True if update successful.</returns>
//        public bool UpdateData(string Procedure, string ColumnName, SqlDbType DataType, object Value)
//        {
//            LastErrorDescription = "";
//            try
//            {
//                if (con.State != System.Data.ConnectionState.Open)
//                    con.Open();
//                MySqlCommand cmd = new MySqlCommand();
//                cmd.CommandType = CommandType.StoredProcedure;
//                //Add parameter name, data types & value for command.                
//                cmd.Parameters.Add("@" + ColumnName, (SqlDbType)DataType);
//                cmd.Parameters[0].Value = Value;

//                cmd.Connection = con;
//                cmd.CommandText = Procedure;

//                //Execute query and dispose command object.
//                cmd.ExecuteNonQuery();
//                cmd.Dispose();
//                con.Close();
//                return (true);
//            }
//            catch (Exception ex)
//            {
//                LastErrorDescription = "MySQL.UpdateDataSingParamProc: " + ex.Message;
//                return (false);
//            }
//        }
//        #endregion

//        #region Insert Data Query
//        /// <summary>
//        /// Inserts data in database based on given InsertQuery.
//        /// </summary>
//        /// <param name="InsertQuery">Insert Query to Insert data in database.</param>
//        /// <returns>True if Insert is successful else False.</returns>
//        public bool InsertData(string InsertQuery)
//        {
//            LastErrorDescription = "";
//            try
//            {
//                if (con.State != System.Data.ConnectionState.Open)
//                    con.Open();
//                MySqlCommand cmd = new MySqlCommand(InsertQuery);
//                cmd.Connection = con;
//                //Execute query.
//                cmd.ExecuteNonQuery();
//                cmd.Dispose();
//                con.Close();
//                return (false);
//            }
//            catch (Exception ex)
//            {
//                LastErrorDescription = "MySQL.InsertDataQuery: " + ex.Message;                
//                return (false);
//            }
//        }
//        #endregion Insert Data Query

//        #region Insert Data Stored Procedure Multiple Param
//        /// <summary>
//        /// Inserts data in database through stored procedure for mulitple parameters.
//        /// </summary>
//        /// <param name="Procedure">Name of stored procedure to be executed.</param>
//        /// <param name="ColumnNames">Columns in which data is to be inserted.</param>
//        /// <param name="DataType">Data type of respective columns.</param>
//        /// <param name="Values">Value to be inserted in each column.</param>
//        /// <returns>True if insert successful.</returns>
//        public bool InsertData(string Procedure, string[] ColumnNames, SqlDbType[] DataType, object[] Values)
//        {
//            LastErrorDescription = "";
//            try
//            {
//                if (con.State != System.Data.ConnectionState.Open)
//                    con.Open();
//                MySqlCommand cmd = new MySqlCommand();
//                cmd.CommandType = CommandType.StoredProcedure;
//                //Add parameter names, data types & values for command.
//                for (int i = 0; i < ColumnNames.Length; i++)
//                {
//                    cmd.Parameters.Add("@" + ColumnNames.GetValue(i).ToString(), (SqlDbType)DataType.GetValue(i));
//                    cmd.Parameters[i].Value = Values.GetValue(i);
//                }
//                cmd.Connection = con;
//                cmd.CommandText = Procedure;
//                //Execute query.
//                cmd.ExecuteNonQuery();
//                cmd.Dispose();
//                con.Close();
//                return (true);
//            }
//            catch (Exception ex)
//            {
//                LastErrorDescription = "MySQL.InsertDataProc: " + ex.Message;                
//                return (false);
//            }
//        }
//        #endregion Insert Data Stored Procedure Multiple Param

//        #region Execute Procedure Multiple Param
//        /// <summary>
//        /// Execute stored procedure with parameters specified in arrays.
//        /// </summary>
//        /// <param name="Procedure">Name of stored procedure to execute.</param>
//        /// <param name="ColumnNames">Array containing Names of parameters without @</param>
//        /// <param name="DataType">Data type of respective parameter specified in ColumnNames array.</param>
//        /// <param name="Values">Value of respective parameter specified in ColumnNames array.</param>
//        /// <returns>True if execute succeded.</returns>
//        public bool ExecuteProcedure(string Procedure, string[] ColumnNames, SqlDbType[] DataType, object[] Values)
//        {
//            LastErrorDescription = "";
//            try
//            {
//                if (con.State != System.Data.ConnectionState.Open)
//                    con.Open();
//                MySqlCommand cmd = new MySqlCommand();
//                cmd.CommandType = CommandType.StoredProcedure;

//                //Add parameter names, data types & values to command.
//                for (int i = 0; i < ColumnNames.Length; i++)
//                {
//                    cmd.Parameters.Add("@" + ColumnNames.GetValue(i).ToString(), (SqlDbType)DataType.GetValue(i));
//                    cmd.Parameters[i].Value = Values.GetValue(i);
//                }

//                cmd.Connection = con;
//                cmd.CommandText = Procedure;
//                cmd.ExecuteNonQuery();
//                cmd.Dispose();
//                con.Close();
//                return (true);                
//            }
//            catch (Exception ex)
//            {
//                LastErrorDescription = "MySQL.ExecuteProcedure: " + ex.Message;                
//                return (false);
//            }
//        }
//        #endregion Execute Procedure Multiple Param

//        #region Execute Procedure Single Param
//        /// <summary>
//        /// Execute stored procedure with single parameter.
//        /// </summary>
//        /// <param name="Procedure">Name of stored procedure to execute.</param>
//        /// <param name="ColumnName">Name of parameter without @</param>
//        /// <param name="DataType">Data type of parameter.</param>
//        /// <param name="Value">Value of parameter.</param>
//        /// <returns>True if execute succeded.</returns>
//        public bool ExecuteProcedure(string Procedure, string ColumnName, SqlDbType DataType, object Value)
//        {
//            LastErrorDescription = "";
//            try
//            {
//                if (con.State != System.Data.ConnectionState.Open)
//                    con.Open();
//                MySqlCommand cmd = new MySqlCommand();
//                cmd.CommandType = CommandType.StoredProcedure;

//                ColumnName = "@" + ColumnName;

//                cmd.Parameters.Add(ColumnName, DataType);
//                cmd.Parameters[ColumnName].Value = Value;

//                cmd.Connection = con;
//                cmd.CommandText = Procedure;
//                if (cmd.ExecuteNonQuery() > 0)
//                {
//                    cmd.Dispose();
//                    return (true);
//                }
//                else
//                {
//                    cmd.Dispose();
//                    return (true);
//                }
//            }
//            catch (Exception ex)
//            {
//                LastErrorDescription = "MySQL.ExecuteProcedureSingParam: " + ex.Message;                
//                return (false);
//            }
//        }
//        #endregion Execute Procedure Single Param

//        #region Execute Procedure No Param
//        /// <summary>
//        /// Execute stored procedure with single parameters.
//        /// </summary>
//        /// <param name="Procedure">Name of stored procedure to execute.</param>
//        /// <returns>True if execute succeded.</returns>
//        public bool ExecuteProcedure(string Procedure)
//        {
//            LastErrorDescription = "";
//            try
//            {
//                if (con.State != System.Data.ConnectionState.Open)
//                    con.Open();
//                MySqlCommand cmd = new MySqlCommand();
//                cmd.CommandType = CommandType.StoredProcedure;
//                cmd.Connection = con;
//                cmd.CommandText = Procedure;
                
//                //Execute query.
//                cmd.ExecuteNonQuery();
//                cmd.Dispose();
//                con.Close();
//                return (true);
//            }
//            catch (Exception ex)
//            {
//                LastErrorDescription = "MySQL.ExecuteProcedureNoParam: " + ex.Message;                                
//                return (false);
//            }
//        }
//        #endregion

//        #region Select Store Procedure Records Multi Param
//        /// <summary>
//        /// Fetch records using Stored Procedure with parameters.
//        /// </summary>
//        /// <param name="selectProcedure">Stored Procedure name.</param>
//        /// <param name="QueryPName">Array containing names of parameters.</param>
//        /// <param name="QueryValues">Array containing values of parameters.</param>
//        /// <param name="QueryTypes">Array containing data types of parameters.</param>
//        /// <returns>Dataset containing records returned by executing stored procedure.</returns>
//        public DataSet SelectRecords(string selectProcedure, string[] QueryPName, object[] QueryValues, SqlDbType[] QueryTypes)
//        {
//            LastErrorDescription = "";
//            DataSet dataSet = new DataSet();
//            try
//            {
//                if (con.State != System.Data.ConnectionState.Open)
//                    con.Open();
//                MySqlDataAdapter adapter = new MySqlDataAdapter();
//                MySqlCommand cmd = new MySqlCommand();
//                cmd.CommandType = CommandType.StoredProcedure;
//                cmd.CommandText = selectProcedure;
//                cmd.Connection = con;
//                adapter.SelectCommand = cmd;

//                //Add parameters to command.
//                for (int i = 0; i < QueryPName.Length; i++)
//                {
//                    cmd.Parameters.Add("@" + QueryPName[i], QueryTypes[i]).Value = QueryValues[i];
//                }

//                adapter.Fill(dataSet);
//            }
//            catch (MySqlException SqlEx)
//            {
//                dataSet = null;
//                LastErrorDescription = "MySQL.SelectRecords.1: " + SqlEx.Message;
//            }
//            catch (Exception ex)
//            {
//                dataSet = null;
//                LastErrorDescription = "MySQL.SelectRecords.2: " + ex.Message + ex.StackTrace;
//            }
//            return dataSet;
//        }
//        #endregion Select Store Procedure Records

//        #region Select Store Procedure Records No Param
//        /// <summary>
//        /// Fetch records using Stored Procedure without parameters.
//        /// </summary>
//        /// <param name="selectProcedure">Name of Stored Procedure.</param>
//        /// <returns>Dataset containing records returned by executing stored procedure.</returns>
//        public DataSet SelectRecords(string selectProcedure)
//        {
//            LastErrorDescription = "";
//            DataSet dataSet = new DataSet();
//            try
//            {

//                if (con.State != System.Data.ConnectionState.Open)
//                    con.Open();
//                MySqlDataAdapter adapter = new MySqlDataAdapter();
//                MySqlCommand command = new MySqlCommand();
//                command.CommandType = CommandType.StoredProcedure;
//                command.CommandText = selectProcedure;
//                command.CommandType = CommandType.StoredProcedure;
//                command.Connection = this.con;
//                adapter.SelectCommand = command;

//                adapter.Fill(dataSet);

//            }
//            catch (MySqlException SqlEx)
//            {
//                dataSet = null;
//                LastErrorDescription = "MySQL.SelectRecordsNoParam.1: " + SqlEx.Message + SqlEx.StackTrace;
//            }
//            catch (Exception ex)
//            {                
//                dataSet = null;
//                LastErrorDescription = "MySQL.SelectRecordsNoParam.2: " + ex.Message;                
//            }
//            return dataSet;

//        }
//        #endregion Select Store Procedure Records  Single Param

//        #region Select Store Procedure Records Single Param
//        /// <summary>
//        /// Fetch records using Stored Procedure with single parameter.
//        /// </summary>
//        /// <param name="selectProcedure">Stored Procedure name.</param>
//        /// <param name="QueryPName">String containing name of parameter.</param>
//        /// <param name="QueryValues">Object containing value of parameter.</param>
//        /// <param name="QueryTypes">SqlDbType containing data type of parameter.</param>
//        /// <returns>Dataset containing records returned by executing stored procedure.</returns>
//        public DataSet SelectRecords(string selectProcedure, string QueryPName, object QueryValues, SqlDbType QueryTypes)
//        {
//            DataSet dataSet = new DataSet();
//            LastErrorDescription = "";
//            try
//            {
//                if (con.State != System.Data.ConnectionState.Open)
//                    con.Open();
//                MySqlDataAdapter adapter = new MySqlDataAdapter();
//                MySqlCommand cmd = new MySqlCommand();
//                cmd.CommandType = CommandType.StoredProcedure;
//                cmd.CommandText = selectProcedure;
//                cmd.Connection = con;
//                adapter.SelectCommand = cmd;

//                //Add parameter to command object.
//                cmd.Parameters.Add("@" + QueryPName, QueryTypes).Value = QueryValues;

//                adapter.Fill(dataSet);
//            }
//            catch (MySqlException SqlEx)
//            {
//                dataSet = null;
//                LastErrorDescription = "MySQL.SelectRecordsSingParam.1: " + SqlEx.Message;
//            }
//            catch (Exception ex)
//            {
//                dataSet = null;
//                LastErrorDescription = "MySQL.SelectRecordsSingParam.2: " + ex.Message;
//            }
//            return dataSet;

//        }

//        #endregion Select Store Procedure Records Single Param

//        #region Get DataTable
//        /// <summary>
//        /// Gets dataset containing one table and result of query.
//        /// </summary>
//        /// <param name="Query">Select Query.</param>
//        /// <returns>Dataset with one data table.</returns>
//        public DataSet GetDataTable(string TableName, string Query, ref DataSet ds)
//        {
//            LastErrorDescription = "";
//            try
//            {
//                MySqlDataAdapter da = new MySqlDataAdapter(Query, con);
//                ds.Tables.Add(TableName);
//                da.Fill(ds.Tables[TableName]);
//            }
//            catch (Exception ex)
//            {
//                LastErrorDescription = "MySQL.GetDataTable: " + ex.Message;
//            }
//            return (ds);
//        }
//        #endregion

//    }
//}
