using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace stepTest2
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && comboBox1.Text != "" && dateTimePicker1.Text != "") // They can not be empty 
            {

                if (comboBox1.Text == "15" | comboBox1.Text == "20" | comboBox1.Text == "25" | comboBox1.Text == "30") // Step height need to be 15 or 20 or 25 or 30

                {
                    StepTest StepTest = new StepTest(textBox1.Text, comboBox1.Text, dateTimePicker1.Text);
                    StepTest.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Error ! Please choose step Height from 15, 20, 25 and 30.");
                }
            }else
                {
                    MessageBox.Show("Error ! Input cannot be empty.");
                } // error message 
        }
        
        }
    }

