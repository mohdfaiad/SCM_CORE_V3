using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace clsDataLib
{
	/// <summary>
	/// Summary description for AddData.
	/// </summary>
    public class AddData
    {

        #region General variables

        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataAdapter adapter;
        private DataSet dataSet;

        //public string ConnectionString;	
        //public string Query;

        #endregion General variables

        public AddData()
        {
        }

        #region InsertRecords
        /// <summary>
        /// gets records from data tables.
        /// </summary>
        /// <param name="Query">select query</param>
        /// <returns>dataset with required columns</returns>

        public bool InsertRecords(string SelectQuery, string InsertQuery)
        {
            bool status = false;
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
                command.CommandText = InsertQuery;
                command.Connection = this.connection;
                adapter.InsertCommand = this.command;
                adapter.Fill(dataSet);
                status = true;
            }
            catch (SqlException SqlEx)
            {
                Console.Out.WriteLine("Sql Exception : " + SqlEx.Message + SqlEx.StackTrace);
                status = false;
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Exception : " + ex.Message + ex.StackTrace);
                status = false;
            }

            return status;
        }

        public bool InsertRecords(string Procedure)
        {
            bool status = false;
            try
            {
                command = new SqlCommand();
                connection = new SqlConnection();
                adapter = new SqlDataAdapter();
                dataSet = new DataSet();

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = Procedure;
                command.Connection = this.connection;
                Database db = new Database();
                connection.ConnectionString = db.ConnectionString;

                if (command.ExecuteNonQuery() < 0)
                {
                    command.Dispose();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException SqlEx)
            {
                Console.Out.WriteLine("Sql Exception : " + SqlEx.Message + SqlEx.StackTrace);
                status = false;
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Exception : " + ex.Message + ex.StackTrace);
                status = false;
            }

            return status;
        }

        #endregion UpdateRecords

    }
}
