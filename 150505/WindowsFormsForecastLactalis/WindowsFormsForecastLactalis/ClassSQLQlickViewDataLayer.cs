using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Data;

namespace WindowsFormsForecastLactalis
{
    class ClassSQLQlickViewDataLayer
    {
        private SqlConnection conn;
        private SqlCommand Command;
        private SqlDataAdapter dataAddapter;
        private DataTable dataTable;


        public ClassSQLQlickViewDataLayer()
        {
            //Navision SQL
            try
            {


                Stopwatch stopwatch = Stopwatch.StartNew();
                conn = new SqlConnection("Data Source=SMMFOSI0400.skanemejerier.lan; Initial Catalog=SKMJDWDataMarts;User ID=developerSKMJ;Password=SSc!!a7xZ");
                //OLEDB CONNECT TO [Provider=SQLOLEDB.1;Persist Security Info=True;User ID=QvReader;Initial Catalog=SKMJDWDataMarts;
                //Data Source=SMMFOSI0400.skanemejerier.lan;Use Procedure for Prepare=1;Auto Translate=True;Packet Size=4096;Workstation ID=SMMFOSI0402;Use Encryption for Data=False;Tag with column collation when possible=False] (XPassword is QNOHeIZOSKZAESVMSLRA);
                // User: qvreader
                // Pwd: qvreader

                conn.Open();
                stopwatch.Stop();
                double timeConnectSeconds = stopwatch.ElapsedMilliseconds / 1000.0;
                Console.WriteLine("Connect to Qlickview data base Correct! Time (s): " + timeConnectSeconds);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(new System.Windows.Forms.Form() { TopMost = true }, "Error in Qlick View Database Connection!");
                Console.WriteLine("Error Qlickview data base Connection: " + ex.Message);
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
                //Command.CommandTimeout = 1000;

                dataAddapter = new SqlDataAdapter(Command);
                dataTable = new DataTable();
                dataAddapter.Fill(dataTable);

                stopwatch2.Stop();
                double timeQuerySeconds = stopwatch2.ElapsedMilliseconds / 1000.0;
                Console.WriteLine("Time for Qlickview data base Query(s): " + timeQuerySeconds.ToString());
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(new System.Windows.Forms.Form() { TopMost = true }, "No Qlickview database Connection!     " + ex.Message);
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
    }
}
