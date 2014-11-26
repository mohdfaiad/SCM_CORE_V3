///**********************************************************
// * Object Name: QID.DataAccess.SQLLite
// * Prepared By: Vishal Keshav Tillu
// * Prepared On: 28 Jul 2011
// * Description: This class provides for performing operations 
// *              related to SQLLite database from Win/ HH/ Web application.
// * Changed By: 
// * Changed On:
// * Change Description: 
//***********************************************************/
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Data;
//using System.Data.SQLite;

//namespace QID.DataAccess
//{

//    /// <summary>
//    /// Class to communicate with SQLLite database.
//    /// </summary>
//    class SQLLite
//    {

//        #region Variables
//        private SQLiteConnection con = new SQLiteConnection();
//        /// <summary>
//        /// Gets last error description (if any) during execution of a function.
//        /// </summary>
//        public string LastErrorDescription = "";
//        #endregion Variables

//        #region Constructor
//        /// <summary>
//        /// Constructor of SQLLite class.
//        /// </summary>
//        /// <param name="ConnectionString">
//        /// Connection string to be used for connecting to SQLLite database.
//        /// </param>
//        public SQLLite(String ConnectionString)
//        {
//            con.ConnectionString = ConnectionString;
//        }
//        #endregion Constructor

//        #region Update Data
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
//                SQLiteCommand cmd = new SQLiteCommand(UpdateQuery);
//                cmd.Connection = con;
//                //Execute query.
//                cmd.ExecuteNonQuery();
//                cmd.Dispose();
//                con.Close();
//                return (true);
//            }
//            catch (Exception ex)
//            {
//                LastErrorDescription = "SQLLite.UpdateData: " + ex.Message;
//                //Close connection.
//                if (con.State == ConnectionState.Open)
//                    con.Close();                
//                return (false);
//            }
//        }

//        #endregion

//        #region Insert Data
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
//                //Check for sql injection attack.
//                if (InsertQuery.ToUpper().Contains("DELETE "))
//                {
//                    LastErrorDescription = "Query contains prohibited Keywords.";
//                    return (false);
//                }
//                if (con.State != System.Data.ConnectionState.Open)
//                    con.Open();
//                SQLiteCommand cmd = new SQLiteCommand(InsertQuery);
//                cmd.Connection = con;
//                //Execute query.
//                cmd.ExecuteNonQuery();
//                con.Close();
//                return (true);
//            }
//            catch (Exception ex)
//            {
//                LastErrorDescription = "SQLLite.InsertData: " + ex.Message;
//                //Close connection.
//                if (con.State == ConnectionState.Open)
//                    con.Close();
//                return (false);
//            }
//        }
//        #endregion

//        #region Get Data
//        /// <summary>
//        /// Gets data from database based on SelectQuery.
//        /// </summary>
//        /// <param name="SelectQuery">Select Query to retrieve data from database.</param>
//        /// <returns>SqlCeDataReader containing data returned by query.</returns>
//        public SQLiteDataReader GetData(string SelectQuery)
//        {
//            LastErrorDescription = "";
//            try
//            {
//                //Check for sql injection attack.
//                if (SelectQuery.ToUpper().Contains("DELETE "))
//                {
//                    LastErrorDescription = "Query contains prohibited Keywords.";
//                    return (null);
//                }
//                if (con.State != System.Data.ConnectionState.Open)
//                    con.Open();
//                SQLiteCommand cmd = new SQLiteCommand(SelectQuery);
//                cmd.Connection = con;
//                //Execute query.
//                SQLiteDataReader dr = cmd.ExecuteReader();
//                cmd.Dispose();
//                return (dr);
//            }
//            catch (Exception ex)
//            {
//                LastErrorDescription = "SQLLite.GetData: " + ex.Message;
//                //Close connection.
//                if (con.State == ConnectionState.Open)
//                    con.Close();
//                return (null);
//            }
//        }
//        #endregion

//        #region GetString
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
//                string strGetString = "";
//                SQLiteDataReader dr = GetData(SelectQuery);
//                //Extract string data (if fetched).
//                if (dr != null)
//                {
//                    if (dr.Read())
//                    {
//                        strGetString = dr[0].ToString();
//                    }
//                    else
//                    {
//                        strGetString = null;
//                    }
//                    dr.Dispose();
//                }
//                return (strGetString);
//            }
//            catch (Exception ex)
//            {
//                LastErrorDescription = "SQLLite.GetString: " + ex.Message;
//                return (null);
//            }
//        }
//        #endregion

//        #region GetInteger
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
//                SQLiteDataReader dr = GetData(SelectQuery);
//                //Find int value fetched in datareader.
//                if (dr != null)
//                {
//                    if (dr.Read())
//                    {
//                        if (dr[0].ToString() == null)
//                            intGetInteger = 0;
//                        else
//                            intGetInteger = Convert.ToInt32(dr[0].ToString());
//                    }
//                    else
//                    {
//                        intGetInteger = 0;
//                    }
//                    dr.Dispose();
//                }
//                return (intGetInteger);
//            }
//            catch (Exception ex)
//            {
//                LastErrorDescription = "SQLLite.GetInteger: " + ex.Message;
//                return (0);
//            }
//        }
//        #endregion

//        #region GetBoolean
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
//                bool blnGetBoolean = false;
//                SQLiteDataReader dr = GetData(SelectQuery);
//                if (dr != null)
//                {
//                    if (dr.Read())
//                    {
//                        blnGetBoolean = true;
//                    }
//                    else
//                    {
//                        blnGetBoolean = false;
//                    }
//                    dr.Dispose();
//                }
//                return (blnGetBoolean);
//            }
//            catch (Exception ex)
//            {
//                LastErrorDescription = "SQLLite.GetBoolean: " + ex.Message;
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
//                SQLiteDataAdapter da = new SQLiteDataAdapter(Query, con);
//                DataSet ds = new DataSet();
//                da.Fill(ds);
//                return (ds);
//            }
//            catch (Exception ex)
//            {
//                LastErrorDescription = "SQLLite.GetDataset: " + ex.Message;
//                return (null);
//            }
//        }
//        #endregion

//    }
//}
