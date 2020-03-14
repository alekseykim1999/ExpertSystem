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
        public static string connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Knowledge.mdb;";
        private OleDbConnection myConnection;

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


            /*Работа с таблицей заключений
                 берем по очереди номер правила, соответсвующего данному заключению
                 проверяем в таблице посылок, есть ли у соответствующего правила метки, равные -1
                 если у правила есть хоть одна такая метка, то правило плохое
                 метка заключения Check_rule = 1
                 отсеиваютс все плохие правила, по которым нет смысла опрашивать факты
            */
        }
    }
}
