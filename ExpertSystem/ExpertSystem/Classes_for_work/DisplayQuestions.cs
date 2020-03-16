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
        public SortedDictionary<int,string> questions_by_analysis() //вывод вопросов и фактов с предварительным анализом. Вызывается с каждым выбором
        {

            //опрашиваем факты, которые 
            //1) нужны для ответа - type fact = 0
            //2) которые еще не опрошены - label = 0
            //3) которые входят в хорошее правило, то есть правило, где есть совпадение по факту и значению rule check = 0

            //количество фактов, которые надо опросить, значительно уменьшится
            lines.Clear();
            //string query = "SELECT distinct [Question_fact],[ID_fact] FROM [Facts] INNER JOIN " +
            //    " ([Parcels] INNER JOIN [Conclusion] ON [Parcels].[Num_rule] = [Conclusion].[Num_of_rule])" +
            //    " ON [Facts].[ID_fact] = [Parcels].[Num_fact] WHERE [Facts].[Type_fact] = 0 AND [Parcels].[Label] = 0 AND [Conclusion].[Rule_check] = 0";

            string query = "SELECT distinct [Question_fact],[ID_fact] FROM [Facts] WHERE " +
                "[Facts].[Type_fact] = 0 AND [Facts].[ID_fact] = ANY ( SELECT distinct [Num_fact] FROM [Parcels] WHERE [Label] = 0 AND " +
                "[Num_rule] = ANY ( SELECT distinct [Num_of_rule] FROM [Conclusion] WHERE [Rule_check] = 0 ))";
            OleDbCommand command = new OleDbCommand(query, myConnection);
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                
                   lines.Add(Convert.ToInt32(reader[1]), reader[0].ToString());

            }
            reader.Close();
            questions();
            myConnection.Close();
           
            return lines;
        }



        public void questions() //проверка
        {
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


            List<string> questons = new List<string>();
            //примени словарь
            for (int i = 0; i < my_facts.Count; i++)
            {
                string query2 = "SELECT distinct [Question_fact] FROM [Facts] WHERE [ID_fact] =  " + my_facts[i];
                OleDbCommand command2 = new OleDbCommand(query2, myConnection);
                OleDbDataReader reader2 = command2.ExecuteReader();
                while (reader2.Read())
                {

                    questons.Add(reader2[0].ToString());
                    break;

                }
                reader2.Close();

            }
            questons.Distinct(); //вывод требуемых вопросов
            
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
