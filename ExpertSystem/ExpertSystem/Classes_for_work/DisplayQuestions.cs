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
      
        public DisplayQuestions()
        {
            myConnection = new OleDbConnection(connectString);
            myConnection.Open();
        }
        public Dictionary<int,string> questions() //вывод всех вопросов
        {
            Dictionary<int,string> lines = new Dictionary<int, string>();
            string query = "SELECT Question_fact,ID_fact FROM Facts WHERE Type_fact = 0";
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

        public Dictionary<int,string> questions_by_analysis() //вывод вопросов и фактов с предварительным анализом. Вызывается с каждым выбором
        {

            //опрашиваем факты, которые 
            //1) нужны для ответа - type fact = 0
            //2) которые еще не опрошены - label = 0
            //3) которые входят в хорошее правило, то есть правило, где есть совпадение по факту и значению rule check = 0

            //количество фактов, которые надо опросить, значительно уменьшится
            Dictionary<int, string> lines = new Dictionary<int, string>();
            string query = "SELECT [Question_fact],[ID_fact] FROM [Facts] WHERE [Type_fact] = 0 AND " +
                "[ID_fact] = ( SELECT [Num_fact] FROM [Parcels] WHERE [Label] = 0 AND " +
                "[Num_rule] = ( SELECT [Num_of_rule] FROM [Conclusion] WHERE [Rule_check] = 0 ) )";
            OleDbCommand command = new OleDbCommand(query, myConnection);
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                lines.Add(Convert.ToInt32(reader[1]), reader[0].ToString());

            }
            reader.Close();
            myConnection.Close();
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
