﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Data.SqlClient;
//using System.Data.Sql;
using System.Data.Odbc;
using System.Data;


namespace WindowsFormsForecastLactalis
{
    class SQL_M3Direct
    {
        private OdbcConnection conn;
        //private SqlConnection conn;
        private OdbcCommand Command;
        //private SqlCommand Command;
        private OdbcDataAdapter dataAddapter;
        //private SqlDataAdapter dataAddapter;
        private DataTable dataTable;

        public SQL_M3Direct()
        {
            //Använder man dsn måste datorn där man kör också lägga upp i ODBC connection
            //conn = new OdbcConnection(@"Dsn=M3PRDDTA;uid=OCDBREAD;Pwd=ocdbread; system=SMM3;dbq=m3prddta;dftpkglib=QGPL;languageid=ENU;pkg=QGPL/DEFAULT(IBM),2,0,1,0,512;qrystglmt=-1;conntype=2");

            //Går man mot ip adress kan man köra från alla datorer som har client access
            //Prod
            //conn = new OdbcConnection("Driver={Client Access ODBC Driver (32-bit)};System=172.31.157.14;Uid=ocdbread;Pwd=ocdbread;dbq=m3prddta; ");

            //Spegling 
            conn = new OdbcConnection("Driver={Client Access ODBC Driver (32-bit)};System=172.31.157.15;Uid=ocdbread;Pwd=ocdbread;dbq=m3prddta; ");

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


        public int GetZeroedForSpecificCustomer(string dateStart, string dateEnd, string customer, string itemNumber)
        {
            int returnInt = 0;
            string CheckKedjor = "";
            List<string> tempKedjor = StaticVariables.AssortmentM3_toKedjor[customer];
            Console.WriteLine(tempKedjor[0]);
            bool first;
            first = true;
            foreach(string kedja in tempKedjor)
            {
                if(first)
                {
                    first = false;
                }
                else
                {
                    CheckKedjor += " OR ";
                }

                string tempAdd = "";
                tempAdd = "(OCHCUS.OSCHL2 = 'XXXXX' OR OCHCUS.OSCHCT = 'XXXXX')";
                CheckKedjor += tempAdd.Replace("XXXXX", kedja);
            }


            string query = @"SELECT OOLINE.OBFACI, OOLINE.OBDIVI, OOLINE.OBWHLO, OCHCUS.OSCHCT, OOLINE.OBCUNO,  ";
            query = query + " OCUSMA.OKCUNM, OOLINE.OBORNO, OOLINE.OBPONR, OOLINE.OBITNO, OOLINE.OBITDS, OOLINE.OBORQT, "; 
            query = query + " OOLINE.OBIVQT, MITMAS.MMUNMS, OOLINE.OBDSDT, OCHCUS.OSLVDT, OCHCUS.OSCHL2,OOLINE.OBLNA2, ";
            query = query + " OCUSMA.OKCUCD, OOLINE.OBNEPR, OOLINE.OBSPUN ";
            query = query + " FROM M3PRDDTA.MITMAS MITMAS, M3PRDDTA.OCHCUS OCHCUS, M3PRDDTA.OCUSMA OCUSMA, "; 
            query = query + " M3PRDDTA.OOLINE OOLINE "; //, mmitty as ItemType ";
            query = query + " WHERE OOLINE.OBCONO = OCHCUS.OSCONO AND OCHCUS.OSLVDT = 99999999 AND  ";
            query = query + " OOLINE.OBCUNO = OCHCUS.OSCUNO AND OCHCUS.OSCONO = OCUSMA.OKCONO AND  ";
            query = query + " OCHCUS.OSCUNO = OCUSMA.OKCUNO AND OOLINE.OBITNO = MITMAS.MMITNO ";
            //query = query + "    ( ( (OCHCUS.OSCHL2 = 'XXXXX' OR OCHCUS.OSCHCT = 'XXXXX')   ";
            query = query + "AND    (" + CheckKedjor + ")  ";
            query = query + " AND OOLINE.OBWHLO='LSK' AND(OOLINE.OBORST In ('99')) AND  ";
            query = query + "    (OOLINE.OBDSDT Between YYYYY And ZZZZZ) AND MITMAS.MMITNO = 'RRRRR'  ";

            //query = query.Replace("XXXXX", customer);
            query = query.Replace("YYYYY", dateStart);
            query = query.Replace("ZZZZZ", dateEnd);
            query = query.Replace("RRRRR", itemNumber);

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
                    string temp = row["OBORQT"].ToString();
                    if(temp.Contains(','))
                    {
                        string[] strings = temp.Split(',');
                        temp = strings[0];
                    }
                    int tempInt = Convert.ToInt32(temp);
                    returnInt += tempInt;
                }
            }

            Console.WriteLine(returnInt);
            return returnInt;
        }


        public int GetZeroedForAllCustomers(string dateStart, string dateEnd, string itemNumber)
        {
            int returnInt = 0;
           

            string query = @"SELECT OOLINE.OBFACI, OOLINE.OBDIVI, OOLINE.OBWHLO, OCHCUS.OSCHCT, OOLINE.OBCUNO,  ";
            query = query + " OCUSMA.OKCUNM, OOLINE.OBORNO, OOLINE.OBPONR, OOLINE.OBITNO, OOLINE.OBITDS, OOLINE.OBORQT, ";
            query = query + " OOLINE.OBIVQT, MITMAS.MMUNMS, OOLINE.OBDSDT, OCHCUS.OSLVDT, OCHCUS.OSCHL2,OOLINE.OBLNA2, ";
            query = query + " OCUSMA.OKCUCD, OOLINE.OBNEPR, OOLINE.OBSPUN ";
            query = query + " FROM M3PRDDTA.MITMAS MITMAS, M3PRDDTA.OCHCUS OCHCUS, M3PRDDTA.OCUSMA OCUSMA, ";
            query = query + " M3PRDDTA.OOLINE OOLINE "; //, mmitty as ItemType ";
            query = query + " WHERE OOLINE.OBCONO = OCHCUS.OSCONO AND OCHCUS.OSLVDT = 99999999 AND  ";
            query = query + " OOLINE.OBCUNO = OCHCUS.OSCUNO AND OCHCUS.OSCONO = OCUSMA.OKCONO AND  ";
            query = query + " OCHCUS.OSCUNO = OCUSMA.OKCUNO AND OOLINE.OBITNO = MITMAS.MMITNO ";
            //query = query + "    ( ( (OCHCUS.OSCHL2 = 'XXXXX' OR OCHCUS.OSCHCT = 'XXXXX')   ";
            //query = query + "AND    (" + CheckKedjor + ")  ";
            query = query + " AND OOLINE.OBWHLO='LSK' AND(OOLINE.OBORST In ('99')) AND  ";
            query = query + "    (OOLINE.OBDSDT Between YYYYY And ZZZZZ) AND MITMAS.MMITNO = 'RRRRR'  ";

            //query = query.Replace("XXXXX", customer);
            query = query.Replace("YYYYY", dateStart);
            query = query.Replace("ZZZZZ", dateEnd);
            query = query.Replace("RRRRR", itemNumber);

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
                    string temp = row["OBORQT"].ToString();
                    if (temp.Contains(','))
                    {
                        string[] strings = temp.Split(',');
                        temp = strings[0];
                    }
                    int tempInt = Convert.ToInt32(temp);
                    returnInt += tempInt;
                }
            }

            Console.WriteLine(returnInt);
            return returnInt;
        }
    }
}
