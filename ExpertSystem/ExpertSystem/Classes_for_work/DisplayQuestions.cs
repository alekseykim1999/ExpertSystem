using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpertSystem.Classes_for_work
{

    public class DisplayQuestions
    {
        public static string connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Knowledge.mdb;";
        private OleDbConnection myConnection;
      
        public DisplayQuestions()
        {
            myConnection = new OleDbConnection(connectString);
            myConnection.Open();
        }
        public Dictionary<int,string> questions()
        {
            Dictionary<int,string> lines = new Dictionary<int, string>();
            string query = "SELECT Question_fact,ID_fact FROM Facts";
            OleDbCommand command = new OleDbCommand(query, myConnection);
            OleDbDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                lines.Add(Convert.ToInt32(reader[1]), reader[0].ToString());
                
            }
            reader.Close();
            myConnection.Close();
            return lines;
        }
    }
}
