using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsForecastLactalis
{
    public class ChainStructureHandler
    {
        public List<String> GetChainsFromCustomer(string customer)
        {
            List<String> chains = new List<string>();
            NavSQLExecute sql = new NavSQLExecute();
            string query = "";
            query = String.Format(@"SELECT [Chain] FROM " + StaticVariables.TableChainCustomer + @" WHERE Customer = '{0}'", customer);
            DataTable table = sql.QueryExWithTableReturn(query);
            DataRow[] currentRows = table.Select(null, null, DataViewRowState.CurrentRows);

            if (currentRows.Length < 1)
            {
                Console.WriteLine("No chains found");
            }
            else
            {
                foreach (DataRow row in currentRows)
                {
                    chains.Add(row["Chain"].ToString());
                }
            }
            return chains;
        }
    }
}
