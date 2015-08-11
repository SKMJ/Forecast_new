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
                System.Windows.Forms.MessageBox.Show("No database Connection!     " + ex.Message);
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

        public void InsertBudgetLine(string custNumber, string custName, string prodNumber, string startdato, int ammount)
        {

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;            // <== lacking
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT into Debitor_Budgetlinjepost (DebitorBogføringsruppe, Varenr, Type, Startdato, Antal, Navn_DebBogfGr, Tastedato) VALUES (@DebitorBogføringsruppe, @Varenr, @Type, @Startdato, @Antal, @Navn_DebBogfGr, @Tastedato)";
                command.Parameters.AddWithValue("@DebitorBogføringsruppe", custNumber);
                command.Parameters.AddWithValue("@Varenr", prodNumber);
                command.Parameters.AddWithValue("@Type", "4");
                command.Parameters.AddWithValue("@Startdato", startdato);
                command.Parameters.AddWithValue("@Antal", ammount);
                command.Parameters.AddWithValue("@Navn_DebBogfGr", custName);
                DateTime time = DateTime.Now;              // Use current time
                string format = "yyyy-MM-dd HH:MM:ss";    // modify the format depending upon input required in the column in database 
                command.Parameters.AddWithValue("@Tastedato", time.ToString(format));

                try
                {
                    int recordsAffected = command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    System.Windows.Forms.MessageBox.Show("Info can not be added to database     " + ex.Message);
                }
            }
        }


        public void InsertReguleretBudgetLine(string prodNumber, string startdato, int ammount, string comment)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;            // <== lacking
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT into Debitor_Budgetlinjepost (Varenr, Type, Startdato, Antal, Tastedato, Kommentar) VALUES ( @Varenr, @Type, @Startdato, @Antal, @Tastedato, @Kommentar)";
                command.Parameters.AddWithValue("@Varenr", prodNumber);
                command.Parameters.AddWithValue("@Type", "5");
                command.Parameters.AddWithValue("@Startdato", startdato);
                command.Parameters.AddWithValue("@Antal", ammount);
                command.Parameters.AddWithValue("@Kommentar", comment);
                DateTime time = DateTime.Now;              // Use current time
                string format = "yyyy-MM-dd HH:MM:ss";    // modify the format depending upon input required in the column in database 
                command.Parameters.AddWithValue("@Tastedato", time.ToString(format));
                try
                {
                    int recordsAffected = command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    System.Windows.Forms.MessageBox.Show("Reguleret Info can not be added to database     " + ex.Message);
                }
            }
        }
    }
}
