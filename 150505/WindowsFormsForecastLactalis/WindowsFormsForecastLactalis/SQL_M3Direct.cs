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
        private Dictionary<string, string> custNameDict;

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

        public Dictionary<int, int> GetZeroedSpecificCustomerWholeYear(string dateStart, string dateEnd, string customer, string itemNumber)
        {
            //Here is the orders that are totally zeroed, it means nothing delivered.
            Dictionary<int, int> returnZeroedDict = new Dictionary<int, int>();
            for (int i = 0; i <= 53; i++)
            {
                returnZeroedDict.Add(i, 0);
            }
            //int returnInt = 0;
            string CheckKedjor = "";
            List<string> tempKedjor = StaticVariables.AssortmentM3_toKedjor[customer];
            Console.WriteLine(tempKedjor[0]);
            bool first;
            first = true;
            foreach (string kedja in tempKedjor)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    CheckKedjor += " OR ";
                }
                string tempAdd = "";
                tempAdd = "(OCHCUS.OSCHL2 = 'XXXXX' OR OCHCUS.OSCHCT = 'XXXXX')";
                tempAdd = "(OCHCUS.OSCHL2 LIKE 'XXXXX%' OR OCHCUS.OSCHCT  LIKE 'XXXXX%')";
                CheckKedjor += tempAdd.Replace("XXXXX", kedja);
            }


            string query = @"SELECT OOLINE.OBFACI, OOLINE.OBDIVI, OOLINE.OBWHLO, OCHCUS.OSCHCT, OOLINE.OBCUNO,  ";
            query = query + " OCUSMA.OKCUNM, OOLINE.OBORNO, OOLINE.OBPONR, OOLINE.OBITNO, OOLINE.OBITDS, OOLINE.OBORQT, ";
            query = query + " OOLINE.OBIVQT, MITMAS.MMUNMS, OOLINE.OBDSDT, OCHCUS.OSLVDT, OCHCUS.OSCHL2,OOLINE.OBLNA2, ";
            query = query + " OCUSMA.OKCUCD, OCUSMA.OKCUNM, OOLINE.OBNEPR, OOLINE.OBSPUN ";
            query = query + " FROM M3PRDDTA.MITMAS MITMAS, M3PRDDTA.OCHCUS OCHCUS, M3PRDDTA.OCUSMA OCUSMA, ";
            query = query + " M3PRDDTA.OOLINE OOLINE "; //, mmitty as ItemType ";
            query = query + " WHERE OOLINE.OBCONO = OCHCUS.OSCONO AND OCHCUS.OSLVDT = 99999999 AND  ";
            query = query + " OOLINE.OBCUNO = OCHCUS.OSCUNO AND OCHCUS.OSCONO = OCUSMA.OKCONO AND  ";
            query = query + " OCHCUS.OSCUNO = OCUSMA.OKCUNO AND OOLINE.OBITNO = MITMAS.MMITNO ";
            //query = query + "    ( ( (OCHCUS.OSCHL2 = 'XXXXX' OR OCHCUS.OSCHCT = 'XXXXX')   ";
            query = query + "AND    OCHCUS.OSCONO = '1' ";
            query = query + "AND    (" + CheckKedjor + ")  ";
            query = query + " AND OOLINE.OBWHLO In ('LSK', 'LIR')  AND(OOLINE.OBORST In ('99', '79')) AND  ";
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
                    string tempOrdered = row["OBORQT"].ToString();
                    string tempDev = row["OBIVQT"].ToString();
                    string tempDateString = row["OBDSDT"].ToString();
                    DateTime tempDate = StaticVariables.ParseExactStringToDate(tempDateString, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    int weekNumber = StaticVariables.GetForecastWeeknumberForDate(tempDate);

                    if (tempOrdered.Contains(','))
                    {
                        string[] strings = tempOrdered.Split(',');
                        tempOrdered = strings[0];
                    }
                    int tempOrderedInt = Convert.ToInt32(tempOrdered);

                    if (tempDev.Contains(','))
                    {
                        string[] strings = tempDev.Split(',');
                        tempDev = strings[0];
                    }
                    int tempDevInt = Convert.ToInt32(tempDev);
                    //returnInt += tempOrderedInt;

                    returnZeroedDict[weekNumber] += tempOrderedInt - tempDevInt;
                }
            }

            //Console.WriteLine(returnInt);
            return returnZeroedDict;
        }

        public Dictionary<string, string>  GetCustomerNameDict()
        {
            return custNameDict;
        }




        public Dictionary<string, int> GetZeroedAllCustomerWholeYearSortedOnCustomer(string dateStart, string dateEnd, string itemNumber)
        {
            //Here is the orders that are totally zeroed, it means nothing delivered.
            Dictionary<string, int> returnZeroedDict = new Dictionary<string, int>();
            custNameDict = new Dictionary<string, string>();



            string query = @"SELECT OOLINE.OBFACI, OOLINE.OBDIVI, OOLINE.OBWHLO, OCHCUS.OSCHCT, OOLINE.OBCUNO,  ";
            query = query + " OCUSMA.OKCUNM, OOLINE.OBORNO, OOLINE.OBPONR, OOLINE.OBITNO, OOLINE.OBITDS, OOLINE.OBORQT, ";
            query = query + " OOLINE.OBIVQT, OCUSMA.OKCUNM, MITMAS.MMUNMS, OOLINE.OBDSDT, OCHCUS.OSLVDT, OCHCUS.OSCHL2,OOLINE.OBLNA2, ";
            query = query + " OCUSMA.OKCUCD, OOLINE.OBNEPR, OOLINE.OBSPUN ";
            query = query + " FROM M3PRDDTA.MITMAS MITMAS, M3PRDDTA.OCHCUS OCHCUS, M3PRDDTA.OCUSMA OCUSMA, ";
            query = query + " M3PRDDTA.OOLINE OOLINE "; //, mmitty as ItemType ";
            query = query + " WHERE OOLINE.OBCONO = OCHCUS.OSCONO AND OCHCUS.OSLVDT = 99999999 AND  ";
            query = query + " OOLINE.OBCUNO = OCHCUS.OSCUNO AND OCHCUS.OSCONO = OCUSMA.OKCONO AND  ";
            query = query + " OCHCUS.OSCUNO = OCUSMA.OKCUNO AND OOLINE.OBITNO = MITMAS.MMITNO ";
            //query = query + "    ( ( (OCHCUS.OSCHL2 = 'XXXXX' OR OCHCUS.OSCHCT = 'XXXXX')   ";
            query = query + "AND    OCHCUS.OSCONO = '1' ";
            //query = query + "AND    (" + CheckKedjor + ")  ";
            query = query + " AND OOLINE.OBWHLO In ('LSK', 'LIR')  AND (OOLINE.OBORST In ('99', '79')) AND  ";
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
                    string tempKund = row["OSCHCT"].ToString().Trim();
                    string tempOrdered = row["OBORQT"].ToString();
                    string tempDev = row["OBIVQT"].ToString();
                    string tempDateString = row["OBDSDT"].ToString();
                    string tempKundnamnString = row["OKCUNM"].ToString();
                    DateTime tempDate = StaticVariables.ParseExactStringToDate(tempDateString, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    int weekNumber = StaticVariables.GetForecastWeeknumberForDate(tempDate);

                    if (tempOrdered.Contains(','))
                    {
                        string[] strings = tempOrdered.Split(',');
                        tempOrdered = strings[0];
                    }
                    int tempOrderedInt = Convert.ToInt32(tempOrdered);

                    if (tempDev.Contains(','))
                    {
                        string[] strings = tempDev.Split(',');
                        tempDev = strings[0];
                    }
                    int tempDevInt = Convert.ToInt32(tempDev);
                    //returnInt += tempOrderedInt;
                    if (!returnZeroedDict.ContainsKey(tempKund))
                    {
                        returnZeroedDict.Add(tempKund, tempOrderedInt - tempDevInt);
                        custNameDict.Add(tempKund, tempKundnamnString);
                    }
                    else
                    {
                        returnZeroedDict[tempKund] += tempOrderedInt - tempDevInt;
                    }
                }
            }

            //Console.WriteLine(returnInt);
            return returnZeroedDict;
        }


        public Dictionary<int, int> GetZeroedAllCustomersWholeYear(string dateStart, string dateEnd, string itemNumber)
        {
            //Here is the orders that are totally zeroed, it means nothing delivered.
            Dictionary<int, int> returnZeroedDict = new Dictionary<int, int>();
            for (int i = 0; i <= 53; i++)
            {
                returnZeroedDict.Add(i, 0);
            }
            //int returnInt = 0;



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
            query = query + "AND    OCHCUS.OSCONO = '1' ";
            query = query + " AND OOLINE.OBWHLO In ('LSK', 'LIR') AND(OOLINE.OBORST In ('99', '79')) AND  ";
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
                    string tempOrdered = row["OBORQT"].ToString();
                    string tempDev = row["OBIVQT"].ToString();
                    string tempDateString = row["OBDSDT"].ToString();
                    DateTime tempDate = StaticVariables.ParseExactStringToDate(tempDateString, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    int weekNumber = StaticVariables.GetForecastWeeknumberForDate(tempDate);

                    if (tempOrdered.Contains(','))
                    {
                        string[] strings = tempOrdered.Split(',');
                        tempOrdered = strings[0];
                    }
                    int tempOrderedInt = Convert.ToInt32(tempOrdered);

                    if (tempDev.Contains(','))
                    {
                        string[] strings = tempDev.Split(',');
                        tempDev = strings[0];
                    }
                    int tempDevInt = Convert.ToInt32(tempDev);
                    //returnInt += tempOrderedInt;

                    returnZeroedDict[weekNumber] += tempOrderedInt - tempDevInt;
                }
            }

            //Console.WriteLine(returnInt);
            return returnZeroedDict;
        }


        public int GetZeroedForSpecificCustomer(string dateStart, string dateEnd, string customer, string itemNumber)
        {
            //Here is the orders that are totally zeroed, it means nothing delivered.
            int returnInt = 0;
            string CheckKedjor = "";
            List<string> tempKedjor = StaticVariables.AssortmentM3_toKedjor[customer];
            Console.WriteLine(tempKedjor[0]);
            bool first;
            first = true;
            foreach (string kedja in tempKedjor)
            {
                if (first)
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
            query = query + " AND OOLINE.OBWHLO In ('LSK', 'LIR')  AND(OOLINE.OBORST In ('99')) AND  ";
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

        public Dictionary<string, double> GetReservedQuantityInWarehouse(string itemNBR)
        {
            Dictionary<string, double> dictReservedQuantity = new Dictionary<string, double>();
            //int returnInt = 0;


            string query = @" select mbreqt, mbwhlo from mitbal where mbcono = '1' and mbitno = '" + itemNBR + "'  ";


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
                    string qty_recv = row["mbreqt"].ToString();
                    string warehouse = row["mbwhlo"].ToString();

                    double qty = Convert.ToDouble(qty_recv);
                    //double antalPerStor = 1.0 / qty;

                    if (warehouse == "LSK" || warehouse == "LDI")
                    {
                        //Console.WriteLine("whole: " + whole_supl + "gives " + antalPerStor + "          of product: " + part_sale + "    kvot:  " + qty);
                        if (!dictReservedQuantity.ContainsKey(warehouse.Trim()))
                        {
                            dictReservedQuantity.Add(warehouse.Trim(), qty);
                        }
                        else
                        {
                            dictReservedQuantity[warehouse.Trim()] = dictReservedQuantity[warehouse.Trim()] + qty;
                        }
                    }
                }
            }

            //Console.WriteLine(returnInt);
            return dictReservedQuantity;
        }

        public Dictionary<string, string> GetMotherChildren()
        {
            Dictionary<string, string> motherChildList = new Dictionary<string, string>();
            int returnInt = 0;


            string query = @"select PMPRNO,  PMMTNO,  PMCNQT, PMPEUN, pmfaci from MPDMAT where pmcono = '1' and pmfaci = 'LAD' and pmbypr = '0' ";


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
                    string part_sale = row["PMPRNO"].ToString();
                    string whole_supl = row["PMMTNO"].ToString();
                    string purchaseEnhet = row["PMPEUN"].ToString();
                    string qty_partOfWhole = row["PMCNQT"].ToString();
                    if(purchaseEnhet.Contains("KRT"))
                    {
                        qty_partOfWhole = "1";
                    }
                    double qty = Convert.ToDouble(qty_partOfWhole);
                    double antalPerStor = 1.0 / qty;

                    if (qty > 0 && qty <= 1)
                    {
                        Console.WriteLine("whole: " + whole_supl + "gives " + antalPerStor + "          of product: " + part_sale + "    kvot:  " + qty);
                        if (!motherChildList.ContainsKey(whole_supl.Trim()))
                        {
                            motherChildList.Add(whole_supl.Trim(), part_sale.Trim() + ";" + qty);
                        }
                        else
                        {
                            motherChildList[whole_supl.Trim()] = motherChildList[whole_supl.Trim()] + "=" + part_sale.Trim() + ";" + qty;
                        }
                    }
                }
            }

            Console.WriteLine(returnInt);
            return motherChildList;
        }

        //select PMPRNO,  PMMTNO,  PMCNQT, pmfaci from MPDMAT where pmcono = '1' and pmfaci = 'LAD'


        public int GetZeroedForAllCustomers(string dateStart, string dateEnd, string itemNumber)
        {
            //Here is the orders that are totally zeroed, it means nothing delivered.
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
            query = query + " AND OOLINE.OBWHLO In ('LSK', 'LIR')  AND(OOLINE.OBORST In ('99')) AND  ";
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

        internal string GetBatchFromOrderLine(string orderNumber, string orderLine)
        {
            string returnDate = "XX";
            List<string> icrepnListan = new List<string>();

            string query = @" ";
            query += @"select icrepn from mplind ";
            query += @"where iccono = '1' and icpuno = '" + orderNumber + "' and icpnli = '" + orderLine + "' ";
            try
            {

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
                        string tempIcrepn = row["icrepn"].ToString().Trim();
                        icrepnListan.Add(tempIcrepn);



                    }
                }

                foreach (string icREPN in icrepnListan)
                {
                    query = @" ";
                    query += @"select MTBANO from mittra ";
                    query += @"where mtridn='" + orderNumber + "' and mtridl='" + orderLine + "' and mtcono='1' and mtwhlo='LSK' and mtrepn='" + icREPN + "' ";


                    WindowsSQLQuery(query);

                    tempTable = QueryEx();

                    currentRows = tempTable.Select(null, null, DataViewRowState.CurrentRows);

                    if (currentRows.Length < 1)
                    {
                        Console.WriteLine("No Current Rows Found");
                    }
                    else
                    {
                        //loop trough all rows and write in tabs
                        foreach (DataRow row in currentRows)
                        {
                            string tempBano = row["mtbano"].ToString().Trim();
                            if (returnDate.Length < tempBano.Length)
                            {
                                returnDate = tempBano;
                            }
                            else if (tempBano.Length > 5 && returnDate.Length > 5 && Convert.ToInt32(tempBano) < Convert.ToInt32(returnDate))
                            {
                                returnDate = tempBano;
                            }
                        }
                    }
                }
                returnDate = TransformToDansih(returnDate);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error finding batch on Köpsorder " + ex.Message);
                returnDate = "YY";
            }


            return returnDate;
        }

        private string TransformToDansih(string returnDate)
        {
            string temp = returnDate;
            string returnString = "XX";


            if (returnDate.Length == 6)
            {
                string month = temp.Substring(2, 2);
                string year = "20" + temp.Substring(0, 2);
                string day = temp.Substring(4, 2);
                returnString = day + "-" + month + "-" + year;
            }
            return returnString;
        }

        internal Dictionary<string, string> GetPolicyDictForProduct(string currentProdNumber)
        {
            int returnInt = 0;
            Dictionary<string, string> returnDictionary = new Dictionary<string, string>();


            string query = @" ";
            query += "Select hcprex, hcobv2,hcobv1, hcrgdt,hcaled, hcobv3,  ";
            query += "hcobv1  ";
            query += "from MADMTX  ";
            query += "Where  ";
            query += "hccono='001'  ";
            query += "And hcobv2='ZZZZZ' AND hcobv1 = 'LSK'";

            query = query.Replace("ZZZZZ", currentProdNumber);

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
                    string tempNamn = row["HCOBV3"].ToString().Trim();
                    string tempNumber = row["HCALED"].ToString().Trim();

                    if (!returnDictionary.ContainsKey(tempNamn))
                    {
                        returnDictionary.Add(tempNamn, tempNumber);
                    }
                }
            }

            Console.WriteLine(returnInt);
            return returnDictionary;
        }
    }
}
