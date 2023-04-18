using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TINY_LANGUAGE_MS2_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        void PrintTokens()
        {
            for (int i = 0; i < TINY_LANGUAGE_MS2_.tinylanguage_scanner.Tokens.Count; i++)
            {
                dataGridView2.Rows.Add(TINY_LANGUAGE_MS2_.tinylanguage_scanner.Tokens.ElementAt(i).lexeme, TINY_LANGUAGE_MS2_.tinylanguage_scanner.Tokens.ElementAt(i).token_type);
            }
        }

        void PrintErrors()
        {
            for (int i = 0; i < ERRORS.Error_List.Count; i++)
            {
                textBox2.Text += ERRORS.Error_List[i];
                textBox2.Text += "\r\n";
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            //string Code=textBox1.Text.ToLower();
            string Codes = textBox1.Text;
            TINY_LANGUAGE_MS2_.Start_Compiling(Codes);
            PrintTokens();
            // PrintLexemes();
            PrintErrors();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            TINY_LANGUAGE_MS2_.TokenStream.Clear();
        }

        private void dataGridView2_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
