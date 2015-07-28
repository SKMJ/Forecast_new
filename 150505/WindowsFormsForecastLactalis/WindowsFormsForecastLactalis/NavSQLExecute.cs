using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Data;
using System.Diagnostics;

namespace WindowsFormsForecastLactalis
{
    class NavSQLExecute
    {
        private OdbcConnection conn;
        //private SqlConnection conn;
        private OdbcCommand Command;
        //private SqlCommand Command;
        private OdbcDataAdapter dataAddapter;
        //private SqlDataAdapter dataAddapter;
        private DataTable dataTable;


        public NavSQLExecute()
        {            
            //Navision SQL
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                conn = new OdbcConnection("Dsn=Sto;uid=sto;Pwd=sto; app=Microsoft® Visual Studio® 2013;wsid=SM0676L;database=NAV-ForecastData;network=DBMSSOCN; ");

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
           
            Command = new OdbcCommand(textQuery, conn);
            dataAddapter = new OdbcDataAdapter(Command);
            dataTable = new DataTable();
            dataAddapter.Fill(dataTable);

            stopwatch2.Stop();
            double timeQuerySeconds = stopwatch2.ElapsedMilliseconds / 1000.0;
            Console.WriteLine("Time for Query(s): " + timeQuerySeconds.ToString());

            return dataTable;
        }

        //non Query returnerat ingen tabell, kanske skriver till databasen
        //Ett statement som inte retunerar tabell.
        public void NoTableReturnQueryEx(string textQuery)
        {
            Command = new OdbcCommand(textQuery, conn);
            Command.ExecuteNonQuery();
        }

        public void Close()
        {
            conn.Close();
        }
    }
}
