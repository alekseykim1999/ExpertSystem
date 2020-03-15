using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpertSystem.Classes_for_work
{
    class CreateLabels
    {
        string connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Knowledge.mdb;";
        public OleDbConnection myConnection;

        public CreateLabels()
        {
            
        }
        public void WriteLabel(int numFact,int NumEnVal) 
        {
            /*Работа с таблицей посылок
            находим там запись. в которой номер факта = NumFact
            проверяем соответсвующий номер разрешенного значения
            если номер разрешенного значения  = NumEnVal, то правило хорошее
            ставим Label = 1 , то есть имеет смысл дальше опрашивать факты по этому правилу

            иначе, если не совпадают, то правило плохое
            ставим Label = -1, выбранное значени не совпадает с тем, что в правиле. 
            правило считается плохим, то есть дальше опрашивать не нужно
            */

            myConnection = new OleDbConnection(connectString);
            myConnection.Open();

            OleDbCommand cmd8 = new OleDbCommand();
            cmd8.Connection = myConnection;
            cmd8.CommandType = CommandType.Text;
            cmd8.CommandText = ("UPDATE [Parcels] SET [Label] = 1 WHERE [Num_Fact] = @numFact AND [Num_en_val] = @numValue"); //там, где сопадает, ставим 1 - правило пока хорошее
            cmd8.Parameters.AddWithValue("@numFact", numFact); //подставляем номер факта и номер его разрешенного значения
            cmd8.Parameters.AddWithValue("@numValue", NumEnVal);
            cmd8.ExecuteNonQuery();

            OleDbCommand cmd9 = new OleDbCommand();
            cmd9.Connection = myConnection;
            cmd9.CommandType = CommandType.Text;
            cmd9.CommandText = ("UPDATE [Parcels] SET [Label] = -1 WHERE [Num_Fact] = @numFact AND [Num_en_val] <> @numValue");  //там, где не совпало, ставим -1, правило плохое
            cmd9.Parameters.AddWithValue("@numFact", numFact);
            cmd9.Parameters.AddWithValue("@numValue", NumEnVal);
            cmd9.ExecuteNonQuery();

            
            /*Работа с таблицей заключений
                 берем по очереди номер правила, соответсвующего данному заключению
                 проверяем в таблице посылок, есть ли у соответствующего правила метки, равные -1
                 если у правила есть хоть одна такая метка, то правило плохое
                 метка заключения Check_rule = 1
                 отсеиваютс все плохие правила, по которым нет смысла опрашивать факты
            */
            string query = "SELECT [Num_rule] FROM [Parcels] WHERE [Label] = -1"; //взять номера правил, которые уже не выполнятся
            OleDbCommand command = new OleDbCommand(query, myConnection);
            OleDbDataReader reader = command.ExecuteReader();
            List<string> id_of_bad_rules = new List<string>();
            while (reader.Read())
            {
                id_of_bad_rules.Add(reader[0].ToString()); // получения номеров правил, где стоит -1, то есть плохих правил
            }
            reader.Close();
            id_of_bad_rules.Distinct(); //убрать дублирование

            OleDbCommand cmd10 = new OleDbCommand();
            cmd10.Connection = myConnection;
            cmd10.CommandType = CommandType.Text;
            for (int i=0;i<id_of_bad_rules.Count;i++) //проход по невыполненным правилам
            {
               
                cmd10.CommandText = ("UPDATE [Conclusion] SET [Rule_check] = 1 WHERE [Num_of_rule] = " + id_of_bad_rules[i]); //установить метку заключения. Данного ответа уже не будет, правило плохое
                cmd10.ExecuteNonQuery();
            }
            myConnection.Close(); //закрыть подключение

        }
    }
}
