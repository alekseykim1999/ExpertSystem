using ExpertSystem.Classes_for_work;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExpertSystem
{
    public partial class Form1 : Form
    {
        List<int> id_facts = new List<int>(); //хранит id факта
        List<string> name_questions = new List<string>(); //хранит вопросы к фактам
        List<int> id_value = new List<int>(); //хранит id разрешенного значения

        CreateLabels creator = new CreateLabels(); //объект для расставления меток
        GetVariants values = new GetVariants(); //создает объект для вывода разрешенных значений
        int counter = 0; //счетчик, чтобы идти по вопросам
        public Form1()
        {
            InitializeComponent();
        }
        private void Get_All_Questions()
        {
            DisplayQuestions writer = new DisplayQuestions();
            Dictionary<int, string> displayer = writer.questions();
            foreach (var item in displayer)
            {
                id_facts.Add(item.Key); //записывает ID факта
                name_questions.Add(item.Value); //записывает вопросы
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void AddValues(int numFact)
        {
            
            values.ID_fact = numFact; //сохраняет номер данного факта
            Dictionary<int, string> enable_val = values.values();
            foreach(var item in enable_val)
            {
                listBox1.Items.Add(item.Value); //записывает разрешенные значения
                id_value.Add(item.Key); //записывает id разрешенных значений
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void Select_Click(object sender, EventArgs e)
        {
            int current_question = ++counter; //текущий вопрос
            listBox2.Items.Add(textBox1.Text + " "+ listBox1.SelectedItem); //добавление в историю
            int num_value= id_value[listBox1.SelectedIndex]; //номер выбранного разрешенного значения
            int numFact = id_facts[current_question - 1]; //номер факта

            creator.WriteLabel(numFact, num_value);

            listBox1.Items.Clear(); //очищение
            textBox1.Text = "";
            id_facts.Clear();
            id_value.Clear();
            name_questions.Clear();
            Get_All_Questions();
            try
            {
                int numFactNext = id_facts[current_question]; //,берет ID следующего факта для вывода на форму
                textBox1.Text = name_questions[current_question].ToString(); //берет следующий вопрос
                AddValues(numFactNext); //выводит разрешенные значения следующего факта
            }
            catch
            {
                CheckAnswer displayer = new CheckAnswer();
                string answer = displayer.GetAnswer();
                MessageBox.Show(answer);
                Select.Enabled = false;
            }
           


        }

        private void Consult_btn_Click(object sender, EventArgs e)
        {
            Select.Enabled = true;
            DisplayQuestions cleaner = new DisplayQuestions();
            cleaner.Clear_Info();
            if (counter>0) 
            {
                counter = 0;
            }
           
            textBox1.Text = "";
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            id_facts.Clear();
            name_questions.Clear();
            id_value.Clear();

            Get_All_Questions();
            AddValues(counter);
            int numFact = id_facts[counter];
            textBox1.Text = name_questions[counter].ToString();
            AddValues(numFact);




        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
