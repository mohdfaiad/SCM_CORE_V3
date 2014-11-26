#region Using
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Xml;
using System.Web;
#endregion Using

namespace clsDataLib
{		
	
	/// <summary>
	/// Database class checks the database connection and retrives data from the database in the form of datasets.
	/// </summary>
	public class Database
	{
		#region General variables
		/// <summary>
		/// Genaral variables are the object declarations for Connection, Command,
		/// adapter and dataset with local scope.
		/// Connection string contains connection vaiables of the server.
		/// Query contains sql query to access the data from the database.
		/// </summary>

		private SqlConnection connection;
		private SqlCommand command;
		private SqlDataAdapter adapter;
		//private SqlTransaction transaction;
		private DataSet dataSet;

        public string ConnectionString;
		public string Query;

		#endregion General variables

		#region Database Constructor
		/// <summary>
		/// Database constructor initializes ConnectionString, Sqlcommand, SqlConnection,
		/// DataAdapter and DataSet objects.
		/// </summary>

		public Database()
		{
            ConnectionString = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
            command = new SqlCommand();
            connection = new SqlConnection();
            adapter = new SqlDataAdapter();
            dataSet = new DataSet();
            connection.ConnectionString = ConnectionString;
		}

        public Database(string Conn)
        {
            ConnectionString = ConfigurationManager.ConnectionStrings[Conn].ToString();
            command = new SqlCommand();
            connection = new SqlConnection();
            adapter = new SqlDataAdapter();
            dataSet = new DataSet();
            connection.ConnectionString = ConnectionString;
        }
 
		#endregion Database Constructor

		#region ExecuteQuery
		/// <summary>
		/// ExecuteQuery methode is used to check the database connection and to access the data from database.
		/// </summary>
		/// <param name="Query">Query contains the Sql statement to access data from the database.</param>
		/// <returns>ExecuteQuery methode returns the data in the form of dataset which contains collection of tables.</returns>

		public bool ExecuteQuery(string Query)
		{	
			bool status = false;

			try 
			{			
				command.CommandText = Query;			
				 command.Connection = connection;
				//Console.Out.WriteLine("Connection is " + connection.State);
				connection.Open ();
				try
				{		
						Console.Out.WriteLine("Connection is " + connection.State);
						if(command.ExecuteNonQuery () >= 1)
						{
							status =true;
						}					
				}
				catch(SqlException SqlEx)
				{
					Console.Out.WriteLine("Sql Exception : " + SqlEx.Message +SqlEx.StackTrace  );
				}

				//Console.Out.WriteLine("Execution status  : "+status.ToString());
				command.Connection.Close ();
				connection.Close ();
				//transaction.Commit();
			}
			catch(SqlException SqlEx)
			{
				Console.Out.WriteLine("Sql Exception : " + SqlEx.Message +SqlEx.StackTrace  );
				/*if(transaction != null)
					transaction.Rollback();*/
			}
			catch (Exception ex)
			{
				Console.Out.WriteLine("Exception : " + ex.Message +ex.StackTrace );
				/*if(transaction != null)
					transaction.Rollback();*/
			}

			return status;
		}

		#endregion ExecuteQuery		        

        #region GetRecords
        /// <summary>
		/// gets records from data tables.
		/// </summary>
		/// <param name="Query">select query</param>
		/// <returns>dataset with required columns</returns>

        
        public DataSet GetRecords(string Query)
		{
			try
			{
				command.CommandText = Query;
                command.CommandType = CommandType.Text;
				command.Connection = this.connection;
				adapter.SelectCommand = this.command;
                dataSet.Clear();
                dataSet.Tables.Clear();
				adapter.Fill(dataSet);
			}
			catch(SqlException SqlEx)
			{
				Console.Out.WriteLine("Sql Exception : " + SqlEx.Message +SqlEx.StackTrace  );
			}
			catch (Exception ex)
			{
				Console.Out.WriteLine("Exception : " + ex.Message +ex.StackTrace  );
			}

			return dataSet;
		}



		#endregion GetRecords

		#region UpdateRecords
		/// <summary>
		/// gets records from data tables.
		/// </summary>
		/// <param name="Query">select query</param>
		/// <returns>dataset with required columns</returns>

		public DataSet UpdateRecords(string SelectQuery,string UpdateQuery)
		{
			try
			{
				command.CommandText = SelectQuery;
				command.Connection = this.connection;
				
				adapter.SelectCommand = this.command;
				command.CommandText = UpdateQuery;
				command.Connection = this.connection;
				adapter.UpdateCommand = this.command;
				adapter.Fill(dataSet);				
			}
			catch(SqlException SqlEx)
			{
				Console.Out.WriteLine("Sql Exception : " + SqlEx.Message +SqlEx.StackTrace  );
			}
			catch (Exception ex)
			{
				Console.Out.WriteLine("Exception : " + ex.Message +ex.StackTrace  );
			}

			return dataSet;
		}

        public bool UpdateData(string updateProcedure, string[] QueryPName, object[] QueryValues, SqlDbType[] QueryTypes)
        {
            bool status = false;
            try
            {
                ////command.CommandText = SelectQuery;
                command.Parameters.Clear();
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = this.connection;
                ////adapter.SelectCommand = this.command;
                command.CommandText = updateProcedure;
                command.Connection = this.connection;
                ////adapter.InsertCommand = this.command;
                for (int i = 0; i < QueryValues.Length; i++)
                {
                    command.Parameters.Add(QueryPName[i], QueryTypes[i]).Value = QueryValues[i];
                }
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                ////adapter.Fill(dataSet);
                command.ExecuteNonQuery();
                status = true;
            }
            catch (SqlException SqlEx)
            {
                Console.Out.WriteLine("Sql Exception : " + SqlEx.Message + SqlEx.StackTrace);
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Exception : " + ex.Message + ex.StackTrace);
            }

            return status;
        }

		#endregion UpdateRecords
	
		#region InsertStoreProcedureData
		/// <summary>
		/// gets records from data tables.
		/// </summary>
		/// <param name="Query">select query</param>
		/// <returns>dataset with required columns</returns>

		public bool InsertData(string insertProcedure, string[] QueryPName, object[] QueryValues, SqlDbType[] QueryTypes)
		{
			bool status = false;
			try
			{
                ////command.CommandText = SelectQuery;
                command.Parameters.Clear();
				command.CommandType = CommandType.StoredProcedure;
				command.Connection = this.connection;
                ////adapter.SelectCommand = this.command;
				command.CommandText = insertProcedure;
				command.Connection = this.connection;
                ////adapter.InsertCommand = this.command;
				for(int i = 0; i < QueryValues.Length ; i++)
				{
					command.Parameters.Add(QueryPName[i], QueryTypes[i]).Value = QueryValues[i];					
				}
                if(connection.State == ConnectionState.Closed)
                    connection.Open();
                ////adapter.Fill(dataSet);
                command.ExecuteNonQuery();
				status = true;
			}
			catch(SqlException SqlEx)
			{
				Console.Out.WriteLine("Sql Exception : " + SqlEx.Message +SqlEx.StackTrace  );
			}
			catch (Exception ex)
			{
				Console.Out.WriteLine("Exception : " + ex.Message +ex.StackTrace  );
			}

			return status;
		}

		#endregion InsertStoreProcedureData

		#region SelectStoreProcedureRecords
		/// <summary>
		/// Fetch records based on parameters
		/// </summary>
		/// <param name="Query"></param>
		/// <param name="QueryPName"></param>
		/// <param name="QueryValues"></param>
		/// <param name="QueryTypes"></param>
		/// <returns></returns>
		public DataSet SelectRecords(string selectProcedure, string[] QueryPName,object[] QueryValues,SqlDbType[] QueryTypes)
		{
			try
			{
                command.Parameters.Clear();
                dataSet.Clear();
                //dataSet.Reset();
                command.CommandText = selectProcedure;
				command.CommandType = CommandType.StoredProcedure;
				command.Connection = this.connection;
				adapter.SelectCommand = this.command;

                for (int i = 0; i < QueryPName.Length; i++)
                {
                    command.Parameters.Add(QueryPName[i], QueryTypes[i]).Value = QueryValues[i];
                }
			
                adapter.Fill(dataSet);	
			}
			catch(SqlException SqlEx)
			{
				dataSet = null;
				Console.Out.WriteLine("Sql Exception : " + SqlEx.Message +SqlEx.StackTrace);
			}
			catch (Exception ex)
			{
				dataSet = null;
				Console.WriteLine("Exception : " + ex.Message +ex.StackTrace);
			}

			return dataSet;
		}

        /// <summary>
        /// Fetch records
        /// </summary>
        /// <param name="selectProcedure"></param>
        /// <returns></returns>
        public DataSet SelectRecords(string selectProcedure)
        {
            try
            {
                command.Parameters.Clear();
                dataSet.Clear();
                dataSet.Reset();
                command.CommandText = selectProcedure;
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = this.connection;
                adapter.SelectCommand = this.command;

                adapter.Fill(dataSet);
            }
            catch (SqlException SqlEx)
            {
                dataSet = null;
                Console.Out.WriteLine("Sql Exception : " + SqlEx.Message + SqlEx.StackTrace);
            }
            catch (Exception ex)
            {
                dataSet = null;
                Console.Out.WriteLine("Exception : " + ex.Message + ex.StackTrace);
            }

            return dataSet;
        }

		#endregion SelectStoreProcedureRecords		

		#region UpdateStoreProcedureRecord
		public bool UpdateData(string SelectQuery,string UpdateQuery,string[] QueryPName,object[] QueryValues,SqlDbType[] QueryTypes)
		{
			bool status = false;
			try
			{
				//command.CommandText = SelectQuery;
				command.CommandType = CommandType.StoredProcedure;
				command.Connection = this.connection;
				//adapter.SelectCommand = this.command;
				command.CommandText = UpdateQuery;
				command.Connection = this.connection;
				adapter.InsertCommand = this.command;

				for(int i = 0; i < QueryValues.Length ; i++)
				{
					command.Parameters.Add(QueryPName[i], QueryTypes[i]).Value = QueryValues[i];					
				}
                
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                command.ExecuteNonQuery();
				//adapter.Fill(dataSet);
				status = true;
			}
			catch(SqlException SqlEx)
			{
				Console.Out.WriteLine("Sql Exception : " + SqlEx.Message +SqlEx.StackTrace  );
			}
			catch (Exception ex)
			{
				Console.Out.WriteLine("Exception : " + ex.Message +ex.StackTrace  );
			}

			return status;
		}

		#endregion UpdateStoreProcedureRecord

        #region Server Updates
        //private void ServerUpdate(string airportCode, int awbno, int source, int dest1, int dest2, int itemCnt, decimal totWt, string ex_Flt, char status, DateTime idate, string login)
        //{
        //    DataSet ds = new DataSet();
        //    ds.Tables.Add("NewAWB");
        //    ds.Tables[0].Columns.Add("AWBNo");
        //    ds.Tables[0].Columns.Add("Source");
        //    ds.Tables[0].Columns.Add("Dest1");
        //    ds.Tables[0].Columns.Add("Dest2");
        //    ds.Tables[0].Columns.Add("ItemCount");
        //    ds.Tables[0].Columns.Add("TotalWeight");
        //    ds.Tables[0].Columns.Add("ExpectedFlightNo");
        //    ds.Tables[0].Columns.Add("AWBStatus");
        //    ds.Tables[0].Columns.Add("IssueDate");

        //    object[] iArr = {awbno,
        //                        source,
        //                        dest1,
        //                        dest2,
        //                        itemCnt,
        //                        totWt,
        //                        ex_Flt,
        //                        status,
        //                        idate
        //                    };

        //    ds.Tables[0].Rows[0].ItemArray = iArr;

        //    //ServiceReference1.airwaybillSoapClient.
        //}
        #endregion Server Updates

    }
}