#region Using
using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
#endregion

namespace clsDataLib
{
	/// <summary>
	/// RetriveData class is used to access data from database and returns data in the form of dataset which has a collection of data tables.
	/// </summary>
	public class RetriveData
	{
		#region General variables

		private SqlConnection connection;
		private SqlCommand command;
		private SqlDataAdapter adapter;
		private DataSet dataSet;

		#endregion General variables

		public RetriveData()
		{
			//
			// TODO: Add constructor logic here
			//
		}

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
				command = new SqlCommand();
				connection = new SqlConnection();
				adapter = new SqlDataAdapter();
				dataSet = new DataSet();

				command.CommandText = Query;
				command.Connection = this.connection;
				Database db = new Database();
				connection.ConnectionString = db.ConnectionString;

				adapter.SelectCommand = this.command;
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

        #region GetDataTable
        /// <summary>
        /// Gets records from database into table.
        /// </summary>
        /// <param name="Query">select query</param>
        /// <returns>datatable with required columns</returns>

        public DataTable GetDataTable(string Query, string TableName)
        {
            try
            {
                command = new SqlCommand();
                connection = new SqlConnection();
                adapter = new SqlDataAdapter();
                DataSet ds = new DataSet();

                command.CommandText = Query;
                command.Connection = this.connection;
                Database db = new Database();
                connection.ConnectionString = db.ConnectionString;

                adapter.SelectCommand = this.command;
                ds.Tables.Add(TableName);
                adapter.Fill(ds.Tables[TableName]);
                return ds.Tables[TableName];
            }
            catch (SqlException SqlEx)
            {
                Console.Out.WriteLine("Sql Exception : " + SqlEx.Message + SqlEx.StackTrace);
                return null;
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Exception : " + ex.Message + ex.StackTrace);
                return null;
            }

        }
        #endregion

		#region GetXmlRecords
		public DataSet GetXmlRecords(string Query)
		{
			try
			{
				command = new SqlCommand();
				connection = new SqlConnection();
				adapter = new SqlDataAdapter();
				dataSet = new DataSet();

				command.CommandText = Query;
				command.Connection = this.connection;
				Database db = new Database();
				connection.ConnectionString = db.ConnectionString;

				adapter.SelectCommand = this.command;
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
		#endregion GetXmlRecords
	}
}
