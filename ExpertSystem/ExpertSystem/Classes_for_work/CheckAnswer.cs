using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpertSystem.Classes_for_work
{
    class CheckAnswer
    {
        string connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Knowledge.mdb;";
        public OleDbConnection myConnection;



      


        public string GetAnswer()
        {
            myConnection = new OleDbConnection(connectString);
            myConnection.Open();
            string answer = "";

            try
            {
                string query_5 = "SELECT [Name_Value] FROM [Enable_values] WHERE [ID_value] = " +
               "( SELECT [Num_en_val] FROM [Conclusion] WHERE [Rule_Check] = 0)"; //получить разрешенное значение
                OleDbCommand command_5 = new OleDbCommand(query_5, myConnection);
                OleDbDataReader reader5 = command_5.ExecuteReader();
                while (reader5.Read())
                {
                    answer += reader5[0].ToString();
                }
                reader5.Close();
            }
            catch
            {
                answer = "Ответа нет";
            }
            return answer;
        }
    }
}
