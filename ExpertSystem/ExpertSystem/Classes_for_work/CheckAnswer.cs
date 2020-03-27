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
        List<int> number = new List<int>();
        CreateLabels creator = new CreateLabels(); //объект для расставления меток из промежуточных правил
        int completed_rules = 0;
        int general_rules = 0;

        int good_answer_fact = 0;


        public string GetAnswer()
        {
            myConnection = new OleDbConnection(connectString);
            myConnection.Open();
            string answer = "";
            string query_5 = "SELECT [Num_of_rule] FROM [Conclusion] WHERE [Rule_check] = 0"; //получить все правила
            OleDbCommand command_5 = new OleDbCommand(query_5, myConnection);
            OleDbDataReader reader5 = command_5.ExecuteReader();
            while (reader5.Read())
            {
                    number.Add(Convert.ToInt32(reader5[0]));
            }
            reader5.Close();


            int number_of_good_rule = 0;
            for (int i = 0; i < number.Count; i++)
            {
                    string query_56 = "SELECT count ([Num_rule]) FROM [Parcels] WHERE [Label] = 1 AND [Num_rule] = " + number[i];

                    OleDbCommand command_56 = new OleDbCommand(query_56, myConnection);
                    OleDbDataReader reader56 = command_56.ExecuteReader();
                    while (reader56.Read())
                    {
                        completed_rules =Convert.ToInt32(reader56[0]);  //получаем число строк, относящихся к правилу и выполняющихся
                    }
                    reader56.Close();

                    string query = "SELECT count ([Num_rule]) FROM [Parcels] WHERE [Num_rule] = " + number[i];

                    OleDbCommand command = new OleDbCommand(query, myConnection);
                    OleDbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        general_rules =Convert.ToInt32(reader[0]); //получаем общее число строк, относящееся к правилу
                    }
                    reader.Close();

                    if(completed_rules==general_rules) //число строк правила совпадает с числом строк для выполнения. Одно правило выполнилось
                    {
                        number_of_good_rule = number[i];
                        string query_check = "SELECT [Num_fact] FROM [Conclusion] WHERE [Num_of_rule] = " + number[i]; //получить выполнившиеся правила
                        OleDbCommand command_check = new OleDbCommand(query_check, myConnection);
                        OleDbDataReader reader_chk = command_check.ExecuteReader();
                        while (reader_chk.Read())
                        {
                            good_answer_fact=Convert.ToInt32(reader_chk[0]); //получаем номера факта-ответа, который выполнился
                        }
                        reader_chk.Close();
                    }
            }

            int type_fact = 0;

            if(good_answer_fact>0) //если вдруг вполнилось правило
            {
                //надо проверить, факт выполнившегося правила целевой или промежуточный
                string query_check = "SELECT [Type_fact] FROM [Facts] WHERE [Id_fact] = " + good_answer_fact; //получить выполнившиеся правила
                OleDbCommand command_check = new OleDbCommand(query_check, myConnection);
                OleDbDataReader reader_chk = command_check.ExecuteReader();
                while (reader_chk.Read())
                {
                    type_fact = Convert.ToInt32(reader_chk[0]); //получаем тип факта
                    
                }
                reader_chk.Close();

                if(type_fact==1)//если факт целевой
                {
                    int id = 0;
                    string query_51 = "SELECT [Num_en_val] FROM [Conclusion] WHERE [Rule_check]=0 AND [Num_fact] = " + good_answer_fact
                        + "AND [Num_of_rule] = " + number_of_good_rule; //получить выполнившиеся правила
                    OleDbCommand command_51 = new OleDbCommand(query_51, myConnection);
                    OleDbDataReader reader51 = command_51.ExecuteReader();
                    while (reader51.Read())
                    {
                        id=Convert.ToInt32(reader51[0]);
                    }
                    reader51.Close();

                    string query_56 = "SELECT [Name_Value] FROM [Enable_values] WHERE [ID_value] = " + id;

                    OleDbCommand command_56 = new OleDbCommand(query_56, myConnection);
                    OleDbDataReader reader56 = command_56.ExecuteReader();
                    while (reader56.Read())
                    {
                        answer += reader56[0].ToString();

                    }
                    reader56.Close();
                }
                if (type_fact == 2)//если факт промежуточный
                {
                    answer += "* ";
                    int id = 0;
                    string query_51 = "SELECT [Num_en_val] FROM [Conclusion] WHERE [Rule_check]=0 AND [Num_fact] = " + good_answer_fact
                        + "AND [Num_of_rule] = " + number_of_good_rule; //получить выполнившиеся правила
                    OleDbCommand command_51 = new OleDbCommand(query_51, myConnection);
                    OleDbDataReader reader51 = command_51.ExecuteReader();
                    while (reader51.Read())
                    {
                        id = Convert.ToInt32(reader51[0]);
                    }
                    reader51.Close();

                    string query_56 = "SELECT [Name_Value] FROM [Enable_values] WHERE [ID_value] = " + id;

                    OleDbCommand command_56 = new OleDbCommand(query_56, myConnection);
                    OleDbDataReader reader56 = command_56.ExecuteReader();
                    while (reader56.Read())
                    {
                        answer += reader56[0].ToString();

                    }
                    reader56.Close();


                }
            } 
            myConnection.Close();
            return answer;        
        }   
    }
}
