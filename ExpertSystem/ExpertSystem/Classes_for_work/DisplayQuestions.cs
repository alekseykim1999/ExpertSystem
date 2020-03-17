using System;
using System.Collections.Generic;
using System.Data;
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

        SortedDictionary<int, string> lines = new SortedDictionary<int, string>();
        public DisplayQuestions()
        {
            myConnection = new OleDbConnection(connectString);
            myConnection.Open();
        }   
        

        public SortedDictionary<int, string> questions() //проверка
        {
            lines.Clear();
            List<int> numbers_of_good_rules = new List<int>();
           
            string query = "SELECT [Num_of_rule] FROM [Conclusion] WHERE [Rule_check] = 0";
            OleDbCommand command = new OleDbCommand(query, myConnection);
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {

                numbers_of_good_rules.Add(Convert.ToInt32(reader[0]));

            }
            reader.Close();
            List<int> numbers_of_good_facts = new List<int>();

            for (int i=0;i<numbers_of_good_rules.Count;i++)
            {
                string query2 = "SELECT [Num_fact] FROM [Parcels] WHERE [Label] = 0 AND [Num_rule] = " + numbers_of_good_rules[i];
                OleDbCommand command2 = new OleDbCommand(query2, myConnection);
                OleDbDataReader reader2 = command2.ExecuteReader();
                while (reader2.Read())
                {

                    numbers_of_good_facts.Add(Convert.ToInt32(reader2[0]));

                }
                reader2.Close();
                
            }
            numbers_of_good_facts.Sort();

            List<int> my_facts = new List<int>();
            int[] middle_codes = numbers_of_good_facts.OfType<int>().Distinct().ToArray();
            my_facts.AddRange(middle_codes);


            //применить словарь
            for (int i = 0; i < my_facts.Count; i++)
            {
                string query2 = "SELECT distinct [Question_fact],[ID_fact] FROM [Facts] WHERE [ID_fact] =  " + my_facts[i];
                OleDbCommand command2 = new OleDbCommand(query2, myConnection);
                OleDbDataReader reader2 = command2.ExecuteReader();
                while (reader2.Read())
                {

                    lines.Add(Convert.ToInt32(reader2[1]), reader2[0].ToString());                   

                }
                reader2.Close();

            }
            return lines;
            
        }

        public void Clear_Info() //приведения БД в изначальный вид
        {
            OleDbCommand cmd8 = new OleDbCommand();
            cmd8.Connection = myConnection;
            cmd8.CommandType = CommandType.Text;
            cmd8.CommandText = ("UPDATE [Conclusion] SET [Rule_check] = 0"); //в заключении все правила теперь считаются хорошими
            cmd8.ExecuteNonQuery();

            OleDbCommand cmd9 = new OleDbCommand();
            cmd9.Connection = myConnection;
            cmd9.CommandType = CommandType.Text;
            cmd9.CommandText = ("UPDATE [Parcels] SET [Label] = 0"); //все факты считаются неопрошенными
            cmd9.ExecuteNonQuery();
        }
    }
}
