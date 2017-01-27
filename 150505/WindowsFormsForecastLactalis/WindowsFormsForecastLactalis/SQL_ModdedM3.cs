using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsForecastLactalis
{
    class SQL_ModdedM3
    {
         private OdbcConnection conn;
        //private SqlConnection conn;
        private OdbcCommand Command;
        //private SqlCommand Command;
        private OdbcDataAdapter dataAddapter;
        //private SqlDataAdapter dataAddapter;
        private DataTable dataTable;

        public SQL_ModdedM3()
        {
            //Använder man dsn måste datorn där man kör också lägga upp i ODBC connection
            //conn = new OdbcConnection(@"Dsn=M3PRDDTA;uid=OCDBREAD;Pwd=ocdbread; system=SMM3;dbq=m3prddta;dftpkglib=QGPL;languageid=ENU;pkg=QGPL/DEFAULT(IBM),2,0,1,0,512;qrystglmt=-1;conntype=2");

            //Går man mot ip adress kan man köra från alla datorer som har client access
            //Prod
            //conn = new OdbcConnection("Driver={Client Access ODBC Driver (32-bit)};System=172.31.157.14;Uid=ocdbread;Pwd=ocdbread;dbq=m3prddta; ");

            //Spegling 
            conn = new OdbcConnection("Driver={Client Access ODBC Driver (32-bit)};System=172.31.157.15;Uid=ocdbread;Pwd=ocdbread;dbq=cusprddta; ");

            ////Test
            //conn = new OdbcConnection("Driver={Client Access ODBC Driver (32-bit)};System=172.31.157.25;Uid=ocdbread;Pwd=ocdbread;dbq=m3fdbtst; ");
            
            conn.Open();
        }

        public void WindowsSQLQuery(string textQuery)
        {
            Command = new OdbcCommand(textQuery, conn);
        }

        //returnerar En tabell
        public DataTable QueryEx()
        {
            //DataTable returnTable;
            dataAddapter = new OdbcDataAdapter(Command);
            dataTable = new DataTable();
            dataAddapter.Fill(dataTable);

            return dataTable;
        }

        //non Query returnerat ingen tabell, kanske skriver till databasen
        //Ett statement som inte retunerar tabell.
        public void NonQueryEx()
        {
            Command.ExecuteNonQuery();
        }

        public void Close()
        {
            conn.Close();
        }


        public string GetExpectadDateByPUNO(string PUNO, string PNLI)
        {
            string returnString = "";


            string query = @"SELECT IBEXDT from ZPLINE where IBCONO = '1' and IBPUNO = '" + PUNO + "' and IBPNLI = '" + PNLI + "'  ";
      
            WindowsSQLQuery(query);

            DataTable tempTable = QueryEx();

            DataRow[] currentRows = tempTable.Select(null, null, DataViewRowState.CurrentRows);

            if (currentRows.Length < 1)
            {
                Console.WriteLine("No Current Rows Found");
            }
            else
            {
                //loop trough all rows and write in tabs
                foreach (DataRow row in currentRows)
                {
                    string temp = row["IBEXDT"].ToString();
                    string year = temp.Substring(0, 4);
                    string month = temp.Substring(4, 2);
                    string day = temp.Substring(6, 2);
                    returnString = day+ "-"+month+"-"+year;
                }
            }

            //Console.WriteLine(returnInt);
            return returnString;
        }
            
    }
}
