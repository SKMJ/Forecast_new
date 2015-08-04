using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;

namespace WindowsFormsForecastLactalis
{
    class NavSQLExecute
    {
        //private OdbcConnection conn;
        private SqlConnection conn;
        //private OdbcCommand Command;
        private SqlCommand Command;
        //private OdbcDataAdapter dataAddapter;
        private SqlDataAdapter dataAddapter;
        private DataTable dataTable;


        public NavSQLExecute()
        {
            //Navision SQL
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                conn = new SqlConnection("Data Source=210.2.250.9,1433;Initial Catalog=NAV-ForecastData;User ID=sto;Password=sto");
                //conn = new SqlConnection("Data Source=210.2.250.9,1433;Network Library=DBMSSOCN;Initial Catalog=NAV-ForecastData;User ID=sto;Password=sto");
                //conn = new OdbcConnection("Dsn=Sto;uid=sto;Pwd=sto; app=Microsoft® Visual Studio® 2013;wsid=SM0676L;database=NAV-ForecastData;network=DBMSSOCN; ");

                conn.Open();
                stopwatch.Stop();
                double timeConnectSeconds = stopwatch.ElapsedMilliseconds / 1000.0;
                Console.WriteLine("Connect to data base Correct! Time (s): " + timeConnectSeconds);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error NavDB Connection: " + ex.Message);
            }
        }

        //returnerar En tabell
        public DataTable QueryExWithTableReturn(string textQuery)
        {
            Stopwatch stopwatch2 = Stopwatch.StartNew();
            try
            {
                //Command = new OdbcCommand(textQuery, conn);
                //dataAddapter = new OdbcDataAdapter(Command);
                Command = new SqlCommand(textQuery, conn);
                dataAddapter = new SqlDataAdapter(Command);
                dataTable = new DataTable();
                dataAddapter.Fill(dataTable);

                stopwatch2.Stop();
                double timeQuerySeconds = stopwatch2.ElapsedMilliseconds / 1000.0;
                Console.WriteLine("Time for Query(s): " + timeQuerySeconds.ToString());
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("No database Connection!     "+ ex.Message);
            }


            return dataTable;
        }

        //non Query returnerat ingen tabell, kanske skriver till databasen
        //Ett statement som inte retunerar tabell.
        public void NoTableReturnQueryEx(string textQuery)
        {
            //Command = new OdbcCommand(textQuery, conn);
            Command = new SqlCommand(textQuery, conn);
            Command.ExecuteNonQuery();
        }

        public void Close()
        {
            conn.Close();
        }

        public void InsertQueryToDatabase(string textQuery)
        {
            using (SqlConnection connection = new SqlConnection("ConnectionStringHere"))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;            // <== lacking
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT into tbl_staff (staffName, userID, idDepartment) VALUES (@staffName, @userID, @idDepart)";
                    command.Parameters.AddWithValue("@staffName", "name namesson");
                    command.Parameters.AddWithValue("@userID", "userIdExample");
                    command.Parameters.AddWithValue("@idDepart", "idDepart ");

                    try
                    {
                        connection.Open();
                        int recordsAffected = command.ExecuteNonQuery();
                    }
                    catch (SqlException)
                    {
                        // error here
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
    }
}
