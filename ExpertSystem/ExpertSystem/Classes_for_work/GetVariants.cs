using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpertSystem.Classes_for_work
{
  


    public class GetVariants:DisplayQuestions
    {
        private OleDbConnection myConnection;

        public int ID_fact { get; set; }

        public GetVariants()
        {
            myConnection = new OleDbConnection(connectString);
            myConnection.Open();
        }
        public Dictionary<int, string> values()
        {
            Dictionary<int, string> lines = new Dictionary<int, string>();
            string query = "SELECT [ID_value],[Name_value] FROM [Enable_values] WHERE [Num_fact] = " +  ID_fact;
            OleDbCommand command = new OleDbCommand(query, myConnection);
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                lines.Add(Convert.ToInt32(reader[0]), reader[1].ToString());
    
            }
            reader.Close();
            myConnection.Close();
            return lines;
        }


    }
}
