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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        void PrintTokens()
        {
            for (int i = 0; i < TINY_LANGUAGE_MS2_.tinylanguage_scanner.Tokens.Count; i++)
            {
                dataGridView1.Rows.Add(TINY_LANGUAGE_MS2_.tinylanguage_scanner.Tokens.ElementAt(i).lexeme, TINY_LANGUAGE_MS2_.tinylanguage_scanner.Tokens.ElementAt(i).token_type);
            }
        }

        void PrintErrors()
        {
            for (int i = 0; i < ERRORS.Error_List.Count; i++)
            {
                textBox2.Text += ERRORS.Error_List[i];
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            TINY_LANGUAGE_MS2_.TokenStream.Clear();
            dataGridView1.Rows.Clear();
            treeView1.Nodes.Clear();
            ERRORS.Error_List.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            string Code = textBox1.Text.ToLower();
            TINY_LANGUAGE_MS2_.Start_Compiling(Code);
            PrintTokens();
            treeView1.Nodes.Add(Parser.PrintParseTree(TINY_LANGUAGE_MS2_.treeroot));
            PrintErrors();
        }
    }
}
