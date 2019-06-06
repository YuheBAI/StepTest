using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace stepTest2
{
    public partial class Database : Form
    {


        SqlConnection cs = new SqlConnection(ConfigurationManager.ConnectionStrings["stepTest2.Properties.Settings.Database1ConnectionString"].ConnectionString);
        public static SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        // initialisation for the database 

        public Database()
        {
            InitializeComponent();
        }

        private void testInformationBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.testInformationBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.database1DataSet);

        }

        private void Database_Load(object sender, EventArgs e)
        {
            // This line of code loads data into the 'database1DataSet.TestInformation' table. 
            this.testInformationTableAdapter.Fill(this.database1DataSet.TestInformation);

        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                StepTest.txtName.Text = testInformationDataGridView.SelectedRows[0].Cells[1].Value.ToString();
                StepTest.txtAge.Text = testInformationDataGridView.SelectedRows[0].Cells[2].Value.ToString();
                StepTest.comboBoxGender.Text = testInformationDataGridView.SelectedRows[0].Cells[3].Value.ToString();
                // update the text boxes in the step test form

                Close(); // close the form

            }
            catch (Exception)
            {
                MessageBox.Show("Please select a row!");
            }
            
        }

        //  print

        Bitmap bmp;

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Graphics g = this.CreateGraphics();
            bmp = new Bitmap(this.Size.Width, this.Size.Height, g);
            Graphics mg = Graphics.FromImage(bmp);
            mg.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, this.Size);
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bmp, 0, 0);
        }

        
        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Text File |*.txt"; // need to be a txt file
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {

                Stream s = File.Open(saveFileDialog1.FileName, FileMode.CreateNew);
                StreamWriter writer = new StreamWriter(s);
                writer.Write("\t" + "Id" + "\t" + "|"
                    + "\t" + "Name" + "\t" + "|"
                    + "\t" + "Age" + "\t" + "|"
                    + "\t" + "Gender" + "\t" + "|"

                    + "\t" + "Tester's Initials" + "\t" + "|"
                    + "\t" + "Step Height" + "\t" + "|"
                    + "\t" + "Test Date" + "\t" + "|"

                    + "\t" + "Aerobic Capacity" + "\t" + "|"
                    + "\t" + "Fitness Rating" + "\t" + "|"
                    + "\t" + "Remarks" + "\t" + "|");
                writer.WriteLine(""); // write the Column names


                for (int i = 0; i < testInformationDataGridView.Rows.Count - 1; i++) // row
                {
                    for (int j = 0; j < testInformationDataGridView.Columns.Count; j++) // column
                    {
                        writer.Write("\t" + testInformationDataGridView.Rows[i].Cells[j].Value.ToString() + "\t" + "|");

                    }
                    writer.WriteLine("");
                }
                writer.Close();
                MessageBox.Show("Exported successfully");
            }

        }
    }
}
