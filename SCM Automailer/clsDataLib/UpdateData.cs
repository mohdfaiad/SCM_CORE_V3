#region Using
using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
#endregion 

namespace clsDataLib
{
	/// <summary>
	/// Summary description for UpdateData.
	/// </summary>
	public class UpdateData
	{
		#region General variables

		private SqlConnection connection;
		private SqlCommand command;
		private SqlDataAdapter adapter;
		private DataSet dataSet;

		//public string ConnectionString;	
		//public string Query;

		#endregion General variables

		public UpdateData()
		{
			//
			// TODO: Add constructor logic here
			//
		}

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
				command = new SqlCommand();
				connection = new SqlConnection();
				adapter = new SqlDataAdapter();
				dataSet = new DataSet();

				command.CommandText = SelectQuery;
				command.Connection = this.connection;
				Database db = new Database();
				connection.ConnectionString = db.ConnectionString;

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

		#endregion UpdateRecords
	}
}
