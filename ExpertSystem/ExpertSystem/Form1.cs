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
        List<int> id_facts = new List<int>();
        List<string> name_questions = new List<string>();
        List<int> id_value = new List<int>();
        int counter = 0;
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
            GetVariants values = new GetVariants();
            values.ID_fact = numFact;
            Dictionary<int, string> enable_val = values.values();
            foreach(var item in enable_val)
            {
                listBox1.Items.Add(item.Value);
                id_value.Add(item.Key);
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void Select_Click(object sender, EventArgs e)
        {
            int current_question = ++counter;
            listBox2.Items.Add(listBox1.SelectedItem);
            int num_value= id_value[listBox1.SelectedIndex];
            int numFact = id_facts[current_question - 1];
            listBox1.Items.Clear();
            textBox1.Text = "";
            int numFactNext = id_facts[current_question]; //,берет ID следующего факта для вывода на форму
            textBox1.Text = name_questions[current_question].ToString();
            AddValues(numFactNext);


        }

        private void Consult_btn_Click(object sender, EventArgs e)
        {
            if(counter>0)
            {
                counter = 0;
            }
            DisplayQuestions cleaner = new DisplayQuestions();
            cleaner.Clear_Info();
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
