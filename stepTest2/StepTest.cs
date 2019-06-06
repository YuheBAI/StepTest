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
    public partial class StepTest : Form
    {


        SqlConnection cs = new SqlConnection(ConfigurationManager.ConnectionStrings["stepTest2.Properties.Settings.Database1ConnectionString"].ConnectionString);
        public static SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        // initialisation for database 

        public string initials, heightString, testDate;
        public int age, MaxHR, height;
        public double MaxHR85, MaxHR50;
        public double Aerobic_Capacity, AC;
        public string Fitness_Rating;
        public string name, gender;
        public double y1, y2, y3, y4, y5;
        public double sumX, sumY, Xsquare, XY, sumXsquare, sumXY;
        public double m, b,n;
        public int id;
        List<double> listX = new List<double>();
        List<double> listY = new List<double>();

        private void btnNext_Click(object sender, EventArgs e)
        {

            SqlDataAdapter da = new SqlDataAdapter("select Id from TestInformation where Name = '" + txtName.Text
               + "'", cs); // get the ID of the current subject

            DataTable dt = new DataTable();
            da.Fill(dt);
            try
            {
                id = int.Parse(dt.Rows[0][0].ToString());
            }

            catch (Exception) // if no this person
            {
                txtName.Clear();
                txtAge.Clear();
                comboBoxGender.Text = "";
            }

            SqlDataAdapter da1 = new SqlDataAdapter("select top 1 * from TestInformation where Id > " + id
                + " order by Id asc ", cs); // get the ID of next person

            DataTable dt1 = new DataTable();
            da1.Fill(dt1);

            try
            {
                txtName.Text = dt1.Rows[0][1].ToString();
                txtAge.Text = dt1.Rows[0][2].ToString();
                comboBoxGender.Text = dt1.Rows[0][3].ToString(); // put the information

            } catch (Exception) // if no next person
            {
                txtName.Clear();
                txtAge.Clear();
                comboBoxGender.Text = ""; //reset
            }
            btnOK1.Enabled = true;
            value1.Clear();
            value2.Clear();
            value3.Clear();
            value4.Clear();
            value5.Clear();
            txtRemark.Clear();
            // clear all 5 text boxes.
        }

        // import a txt file database 
        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                List<string> cList = GetInputFromFile(path);
                addToDb(cList);
            }
        }
        
        private List<string> GetInputFromFile(string path)
        {
            StreamReader sr = new StreamReader(path);
            List<string> memberList = new List<string>();
            while (sr.Peek() != -1)
            {
                memberList.Add(sr.ReadLine());
            }
            sr.Close();
            return memberList; // list of the txt file information 
        }

        private void addToDb(List<string> cList) // method addtoDb
        {
            foreach (string item in cList)
            {
                string[] wordArray = item.Split(',');
                Console.WriteLine(wordArray);
                da.InsertCommand = new SqlCommand("INSERT INTO TestInformation (Name, Age, Gender) VALUES (@Name, @Age, @Gender)", cs);
                da.InsertCommand.Parameters.Add("@Name", SqlDbType.NVarChar).Value = wordArray[0].ToString();
                da.InsertCommand.Parameters.Add("@Age", SqlDbType.Int).Value = int.Parse(wordArray[1]);
                da.InsertCommand.Parameters.Add("@Gender", SqlDbType.NVarChar).Value = wordArray[2].ToString();

                cs.Open();
                da.InsertCommand.ExecuteNonQuery();
                cs.Close();
                // add to database using sql query
            }
                MessageBox.Show("Imported Successfully");

        }
        
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) // about
        {
            MessageBox.Show("Step Test Application Version 1.0 \nThis application is created by Yuhe BAI, Glyndwr University. ");
        }
        
        //print
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bmp, 0, 0);
        }

        Bitmap bmp;

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Graphics g = this.CreateGraphics();
            bmp = new Bitmap(this.Size.Width, this.Size.Height, g);
            Graphics mg = Graphics.FromImage(bmp);
            mg.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, this.Size);
            printPreviewDialog1.ShowDialog();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = cs;
            cs.Open();

            // if this person already exists in the database 
            // update 
            sqlCommand.CommandText = "if exists(select * from TestInformation where Name = '" + name + "')"
            + "UPDATE TestInformation SET TestersInitials = '" + initials
            + "', StepHeight = " + height
            + ", TestDate = '" + testDate
            + "', AerobicCapacity = '" + AC
            + "', FitnessRating = '" + Fitness_Rating
            + "', Remarks = '" + txtRemark.Text
            + "' WHERE Name = '" + txtName.Text + "' "
            // if this person do not exist in the database 
            // insert into
            + " else INSERT INTO TestInformation VALUES ( '" + name
            + "'," + age
            + ",'" + gender
            + "','" + initials
            + "'," + height
            + ",'" + testDate
            + "','" + AC
            + "','" + Fitness_Rating
            + "','" + txtRemark.Text + "')";


            sqlCommand.ExecuteNonQuery();
            MessageBox.Show("Saved Successfully.");
            cs.Close();
            btnOK1.Enabled = true;

        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {

            value1.Clear();
            value2.Clear();
            value3.Clear();
            value4.Clear();
            value5.Clear();
            // clear all 5 text boxes.
        }

        private void nextToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SqlDataAdapter da = new SqlDataAdapter("select Id from TestInformation where Name = '" + txtName.Text
               + "'", cs); // get the ID of the current subject

            DataTable dt = new DataTable();
            da.Fill(dt);
            try
            {
                id = int.Parse(dt.Rows[0][0].ToString());
            }

            catch (Exception )// if no this person
            {
                txtName.Clear();
                txtAge.Clear();
                comboBoxGender.Text = "";
            }

            SqlDataAdapter da1 = new SqlDataAdapter("select top 1 * from TestInformation where Id > " + id
                + " order by Id asc ", cs); // get the ID of next person

            DataTable dt1 = new DataTable();
            da1.Fill(dt1);

            try
            {
                // put the information
                txtName.Text = dt1.Rows[0][1].ToString();
                txtAge.Text = dt1.Rows[0][2].ToString();
                comboBoxGender.Text = dt1.Rows[0][3].ToString();

            }
            catch (Exception)// if no next person
            {
                txtName.Clear();
                txtAge.Clear();
                comboBoxGender.Text = ""; // reset
            }
            btnOK1.Enabled = true;
            value1.Clear();
            value2.Clear();
            value3.Clear();
            value4.Clear();
            value5.Clear();
            txtRemark.Clear();
            // clear all 5 text boxes.

        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter("select Id from TestInformation where Name = '" + txtName.Text
               + "'", cs); // get the ID of the current subject

            DataTable dt = new DataTable();
            da.Fill(dt);
            try
            {
                id = int.Parse(dt.Rows[0][0].ToString());
            }

            catch (Exception ) // if no this person 
            {
                txtName.Clear();
                txtAge.Clear();
                comboBoxGender.Text = "";
            }

            SqlDataAdapter da1 = new SqlDataAdapter("select top 1 * from TestInformation where Id < " + id
                + " order by Id DESC ", cs); // go to the previous line

            DataTable dt1 = new DataTable();
            da1.Fill(dt1);

            try
            {
                // put the information 
                txtName.Text = dt1.Rows[0][1].ToString();
                txtAge.Text = dt1.Rows[0][2].ToString();
                comboBoxGender.Text = dt1.Rows[0][3].ToString();

            }
            catch (Exception ) // if no previous person
            {
                txtName.Clear();
                txtAge.Clear();
                comboBoxGender.Text = ""; // reset 
            }
            btnOK1.Enabled = true;
            value1.Clear();
            value2.Clear();
            value3.Clear();
            value4.Clear();
            value5.Clear();
            txtRemark.Clear();
            // clear all 5 text boxes.
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpForm helpForm = new HelpForm();
            helpForm.Show(); // open the help form

        }

        private void lastToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SqlDataAdapter da = new SqlDataAdapter("select Id from TestInformation where Name = '" + txtName.Text
               + "'", cs); // get the ID of the current subject

            DataTable dt = new DataTable();
            da.Fill(dt);
            try
            {
                id = int.Parse(dt.Rows[0][0].ToString());
            }

            catch (Exception) // if no this person 
            {
                txtName.Clear();
                txtAge.Clear();
                comboBoxGender.Text = "";
            }

            SqlDataAdapter da1 = new SqlDataAdapter("select top 1 * from TestInformation where Id < " + id
                + " order by Id DESC ", cs); // go to the previous row 

            DataTable dt1 = new DataTable();
            da1.Fill(dt1);

            try
            {
                // put the information
                txtName.Text = dt1.Rows[0][1].ToString();
                txtAge.Text = dt1.Rows[0][2].ToString();
                comboBoxGender.Text = dt1.Rows[0][3].ToString(); 

            }
            catch (Exception ) // if no previous person
            
                {
                txtName.Clear();
                txtAge.Clear();
                comboBoxGender.Text = ""; // reset
            }
            btnOK1.Enabled = true;
            value1.Clear();
            value2.Clear();
            value3.Clear();
            value4.Clear();
            value5.Clear();
            txtRemark.Clear();
            // clear all 5 text boxes.

        }

        private void exportDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = cs;
            cs.Open();

            // if this person already exists in the database 
            // update
            sqlCommand.CommandText = "if exists(select * from TestInformation where Name = '" + name + "')"
            + "UPDATE TestInformation SET TestersInitials = '" + initials
            + "', StepHeight = " + height
            + ", TestDate = '" + testDate
            + "', AerobicCapacity = '" + AC
            + "', FitnessRating = '" + Fitness_Rating
            + "', Remarks = '" + txtRemark.Text
            + "' WHERE Name = '" + txtName.Text + "' "
            // if this person do not exist in the database 
            //insert into
         
            + " else INSERT INTO TestInformation VALUES ( '" + name 
            + "'," + age 
            + ",'" + gender
            + "','" + initials
            + "'," + height
            + ",'" + testDate
            + "','" + AC
            + "','" + Fitness_Rating
            + "','" + txtRemark.Text + "')";
            

            sqlCommand.ExecuteNonQuery();
            MessageBox.Show("Saved Successfully.");
            cs.Close();
            // using sql query

            btnOK1.Enabled = true;
        }
        
        private void StepTest_Load(object sender, EventArgs e)
        {
            
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            value1.Clear();
            value2.Clear();
            value3.Clear();
            value4.Clear();
            value5.Clear();
            // clear all 5 text boxes.
        } // private void btnReset_Click(object sender, EventArgs e)
        
        public StepTest(string initial, string stepHeight, string date)
        {
            InitializeComponent();

            heightString = stepHeight;
            initials = initial;
            testDate = date;
            // get the step height, tester initials and test date from form1

            height = int.Parse(heightString); // int of step height 

            lblInitials.Text = initials;
            lblStepHeight.Text = heightString;
            lblTestDate.Text = testDate;

            initListX(); // initialize x values
        }
        
        private void databaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Database Database = new Database(); 
            Database.Show(); // open the database form
        } // private void databaseToolStripMenuItem_Click(object sender, EventArgs e)

        private void exitToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        } // exit the app

        private void btnOK2_Click(object sender, EventArgs e)
        {
            // if only textbox1 has entered value
            if (value1.Text != "" & value2.Text == "" & value3.Text == "" & value4.Text == "" & value5.Text == "")
            {
                MessageBox.Show("You need to enter at least two values !");
            } // if only one value entered 

            if (value1.Text != "" & value2.Text != "" & value3.Text == "" & value4.Text == "" & value5.Text == "")
            {
                y1 = int.Parse(value1.Text);
                y2 = int.Parse(value2.Text);
                
                listX.RemoveAt(4);
                listX.RemoveAt(3);
                listX.RemoveAt(2);
                // remove x3, x4, x5 since only 2 values entered.

                calculate2input(y1, y2);
                
            } // if 2 values entered

            if (value1.Text != "" & value2.Text != "" & value3.Text != "" & value4.Text == "" & value5.Text == "")
            {

                y1 = int.Parse(value1.Text);
                y2 = int.Parse(value2.Text);
                y3 = int.Parse(value3.Text);

                listX.RemoveAt(4);
                listX.RemoveAt(3); // remove x4, x5 since 3 values entered.

                calculate3input(y1, y2, y3);

            } // if 3 values entered

            if (value1.Text != "" & value2.Text != "" & value3.Text != "" & value4.Text != "" & value5.Text == "")
            {

                y1 = int.Parse(value1.Text);
                y2 = int.Parse(value2.Text);
                y3 = int.Parse(value3.Text);
                y4 = int.Parse(value4.Text);

                listX.RemoveAt(4); // remove x5 since 4 values entered.
                calculate4input(y1, y2, y3, y4);


            } // if 4 values entered

            if (value1.Text != "" & value2.Text != "" & value3.Text != "" & value4.Text != "" & value5.Text != "")
            {

                y1 = int.Parse(value1.Text);
                y2 = int.Parse(value2.Text);
                y3 = int.Parse(value3.Text);
                y4 = int.Parse(value4.Text);
                y5 = int.Parse(value5.Text);

                calculate5input(y1, y2, y3, y4, y5);

            } // if 5 values entered

            
        }

        private void StepTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        } // exit the app 

        private void btnOK1_Click(object sender, EventArgs e)
        {
            
            if (checkBox1.Checked & checkBox2.Checked & checkBox3.Checked & txtName.Text!="" &comboBoxGender.Text!="" &txtAge.Text!=""
                & 0 < int.Parse(txtAge.Text) & int.Parse(txtAge.Text)<120) // if all values are correct 
            {

                btnOK1.Enabled = false; // disable the button OK1

                age = int.Parse(txtAge.Text);
                name = txtName.Text;
                gender = comboBoxGender.Text;

                MaxHR = 220 - age;
                MaxHR85 = MaxHR * 0.85;
                MaxHR50 = MaxHR * 0.5;

                lblMaxHR.Text = "MaxHR : " + MaxHR.ToString();
                lbl85MaxHR.Text = "85%MaxHR : " + MaxHR85.ToString();
            }
            else
            {
                MessageBox.Show("Error ! Please check.");
            }
        } // private void btnOK1_Click(object sender, EventArgs e)
        
        public void calculate2input(double y1, double y2)
        {

            listY.Add(y1);
            listY.Add(y2); // generate list of Y

            for (int i = listY.Count - 1; i >= 0; i--)
            {
                if (listY[i] < MaxHR50 | listY[i] >= MaxHR85)
                {
                    listY.RemoveAt(i);
                    listX.RemoveAt(i);
                    // remove points if outside ranges. 
                }
            }

            if (listX.Count <= 1) // Less than 2 validvalues. 
            {
                MessageBox.Show("HR values error. Please test again.");

                listY.Clear();
                listX.Clear();
                initListX(); // reset values;

            }
            else // calculate process now 
            {
                CalculateAndPlot();
            }

        }

        public void calculate3input( double y1, double y2, double y3)
        {
            listY.Add(y1);
            listY.Add(y2);
            listY.Add(y3); // generate list of Y

            for (int i = listY.Count - 1; i >= 0; i--)
            {
                if (listY[i] < MaxHR50 | listY[i] >= MaxHR85)
                {
                    listY.RemoveAt(i);
                    listX.RemoveAt(i);
                    // remove points if outside ranges. 
                }
            }

            if (listX.Count <= 1) // Less than 2 validvalues. 
            {
                MessageBox.Show("HR values error. Please test again.");

                listY.Clear();
                listX.Clear();
                initListX(); // reset values

            }
            else // calculate process now 
            {
                CalculateAndPlot();
            }
        }

        public void calculate4input(double y1, double y2, double y3, double y4)
        {

            listY.Add(y1);
            listY.Add(y2);
            listY.Add(y3);
            listY.Add(y4); // generate list of Y

            for (int i = listY.Count - 1; i >= 0; i--)
            {
                if (listY[i] < MaxHR50 | listY[i] >= MaxHR85)
                {
                    listY.RemoveAt(i);
                    listX.RemoveAt(i);
                    // remove points if outside ranges. 
                }
            }

            if (listX.Count <= 1) // Less than 2 validvalues. 
            {
                MessageBox.Show("HR values error. Please test again.");

                listY.Clear();
                listX.Clear();
                initListX(); // reset values 

            }
            else // calculate process now 
            {
                CalculateAndPlot();
            }

        }

        public void calculate5input(double y1, double y2, double y3, double y4, double y5)
        {
            listY.Add(y1);
            listY.Add(y2);
            listY.Add(y3);
            listY.Add(y4);
            listY.Add(y5); // generate list of Y

            for (int i = listY.Count - 1; i >= 0; i--)
            {
                if (listY[i] < MaxHR50 | listY[i] >= MaxHR85)
                {
                    listY.RemoveAt(i);
                    listX.RemoveAt(i);
                    // remove points if outside ranges. 
                }
            }

            if (listX.Count <= 1) // Less than 2 validvalues. 
            {
                MessageBox.Show("HR values error. Please test again.");

                listY.Clear();
                listX.Clear();
                initListX(); // reset values;


            }
            else // calculate process now 
            {
                CalculateAndPlot();
            }


        }

        public void initListX()
        {
            listX.Clear();
            switch (height)
            {
                case 15:

                    listX.Add(11);
                    listX.Add(14);
                    listX.Add(18);
                    listX.Add(21);
                    listX.Add(25);
                    
                    break;

                case 20:

                    listX.Add(12);
                    listX.Add(17);
                    listX.Add(21);
                    listX.Add(25);
                    listX.Add(29);
                    
                    break;


                case 25:

                    listX.Add(14);
                    listX.Add(19);
                    listX.Add(24);
                    listX.Add(28);
                    listX.Add(33);
                    
                    break;

                case 30:

                    listX.Add(16);
                    listX.Add(21);
                    listX.Add(27);
                    listX.Add(32);
                    listX.Add(37);
                    
                    break;
            }
        } // public void initListX()
        
        public void CalculateAndPlot()
        {
            sumX = listX.Sum();
            sumY = listY.Sum();
            n = listX.Count;
            sumXsquare = 0;
            sumXY = 0; // initialize sum

            // calculation here
            for (int i = 0; i < listY.Count; i++)
            {
                Xsquare = Math.Pow(listX[i], 2); // x square
                XY = listX[i] * listY[i];  // x*y

                Console.Write("  square of x" + (i+1) + " = " + Xsquare);
                Console.Write("  x" + (i+1) + " * y" + (i+1) + " = " + XY); // for testing 

                sumXsquare += Xsquare;
                sumXY += XY;
            }
            Console.WriteLine("");
            Console.WriteLine(" sum of x =" + sumX);
            Console.WriteLine(" sum of y =" + sumY);
            Console.WriteLine(" sum of x square =" + sumXsquare);
            Console.WriteLine(" sum of XY =" + sumXY);
            // for testing

            m = (sumXY - (sumX * sumY) / n) / (sumXsquare - (Math.Pow(sumX, 2)) / n);
            b = (sumY / n) - (m * (sumX / n));
            Aerobic_Capacity = (MaxHR - b) / m;
            AC = Math.Round(Aerobic_Capacity, 2); // show only two digits after decimal 

            Console.WriteLine(" m =" + m);
            Console.WriteLine(" b =" + b);
            Console.WriteLine(" Aerobic Capacity = " + Aerobic_Capacity);
            // for testing
            

            // plot here
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            } // clear all information in the chart.

            chart1.Series["MaxHR"].Points.AddXY(10, MaxHR);
            chart1.Series["MaxHR"].Points.AddXY(76, MaxHR);
            // MaxHR line
            chart1.Series["85%MaxHR"].Points.AddXY(10, MaxHR85);
            chart1.Series["85%MaxHR"].Points.AddXY(76, MaxHR85);
            // 85% max HR line
            chart1.Series["50%MaxHR"].Points.AddXY(10, MaxHR50);
            chart1.Series["50%MaxHR"].Points.AddXY(76, MaxHR50);
            // 50% MAX HR Line

            for (int i = listY.Count - 1; i >= 0; i--)
            {
               chart1.Series["HRvalues"].Points.AddXY(listX[i], listY[i]);
            } // plot all the points



            chart1.Series["BestLine"].Points.AddXY(10, 10*m+b);
            chart1.Series["BestLine"].Points.AddXY(Aerobic_Capacity, MaxHR);
            // y = mx + b 
            chart1.Series["AerobicCapacity"].Points.AddXY(Aerobic_Capacity, MaxHR);
            chart1.Series["AerobicCapacity"].Points.AddXY(Aerobic_Capacity, 0);


            listY.Clear();
            listX.Clear();
            initListX();
            // Reinitialize listX and delete listY

            lblAC.Text = AC.ToString();
            fitnessRating(); // get the fitness rating

        }

        public void fitnessRating() 
            // compare the Aerobic Capacity with the sheet to get the fitness Rating.
        {
            switch (gender)
            {
                case "Male" :
                    if (age >= 15 & age <= 19)
                    {
                        if (AC >= 60)
                        { Fitness_Rating = "Excellent"; }
                        else if (AC >= 48 & AC < 60)
                        { Fitness_Rating = "Good"; }
                        else if (AC >= 39 & AC < 48)
                        { Fitness_Rating = "Average"; }
                        else if (AC >= 30 & AC < 39)
                        { Fitness_Rating = "Below Average"; }
                        else if (AC < 30)
                        { Fitness_Rating = "Poor"; }
                    }

                    else if (age >= 20 & age <= 29)
                    {
                        if (AC >= 55)
                        { Fitness_Rating = "Excellent"; }
                        else if (AC >= 44 & AC < 55)
                        { Fitness_Rating = "Good"; }
                        else if (AC >= 35 & AC < 44)
                        { Fitness_Rating = "Average"; }
                        else if (AC >= 28 & AC < 35)
                        { Fitness_Rating = "Below Average"; }
                        else if (AC < 28)
                        { Fitness_Rating = "Poor"; }

                    }

                    else if (age >= 30 & age <= 39)
                    {
                        if (AC >= 50)
                        { Fitness_Rating = "Excellent"; }
                        else if (AC >= 40 & AC < 50)
                        { Fitness_Rating = "Good"; }
                        else if (AC >= 34 & AC < 40)
                        { Fitness_Rating = "Average"; }
                        else if (AC >= 26 & AC < 34)
                        { Fitness_Rating = "Below Average"; }
                        else if (AC < 26)
                        { Fitness_Rating = "Poor"; }
                    }

                    else if (age >= 40 & age <= 49)
                    {
                        if (AC >= 46)
                        { Fitness_Rating = "Excellent"; }
                        else if (AC >= 37 & AC < 45)
                        { Fitness_Rating = "Good"; }
                        else if (AC >= 32 & AC < 37)
                        { Fitness_Rating = "Average"; }
                        else if (AC >= 25 & AC < 32)
                        { Fitness_Rating = "Below Average"; }
                        else if (AC < 25)
                        { Fitness_Rating = "Poor"; }
                    }

                    else if (age >= 50 & age <= 59)
                    {
                        if (AC >= 44)
                        { Fitness_Rating = "Excellent"; }
                        else if (AC >= 35 & AC < 44)
                        { Fitness_Rating = "Good"; }
                        else if (AC >= 29 & AC < 35)
                        { Fitness_Rating = "Average"; }
                        else if (AC >= 23 & AC < 29)
                        { Fitness_Rating = "Below Average"; }
                        else if (AC < 29)
                        { Fitness_Rating = "Poor"; }

                    }

                    else if (age >= 60 & age <= 65)
                    {
                        if (AC >= 40)
                        { Fitness_Rating = "Excellent"; }
                        else if (AC >= 33 & AC < 40)
                        { Fitness_Rating = "Good"; }
                        else if (AC >= 25 & AC < 33)
                        { Fitness_Rating = "Average"; }
                        else if (AC >= 20 & AC < 25)
                        { Fitness_Rating = "Below Average"; }
                        else if (AC < 20)
                        { Fitness_Rating = "Poor"; }

                    }

                    break;

                case "Female":

                    if (age >= 15 & age <= 19)
                    {
                        if (AC >= 55)
                        { Fitness_Rating = "Excellent"; }
                        else if (AC >= 44 & AC < 55)
                        { Fitness_Rating = "Good"; }
                        else if (AC >= 36 & AC < 44)
                        { Fitness_Rating = "Average"; }
                        else if (AC >= 29 & AC < 36)
                        { Fitness_Rating = "Below Average"; }
                        else if (AC < 29)
                        { Fitness_Rating = "Poor"; }
                    }

                    else if (age >= 20 & age <= 29)
                    {
                        if (AC >= 50)
                        { Fitness_Rating = "Excellent"; }
                        else if (AC >= 40 & AC < 50)
                        { Fitness_Rating = "Good"; }
                        else if (AC >= 32 & AC < 40)
                        { Fitness_Rating = "Average"; }
                        else if (AC >= 27 & AC < 32)
                        { Fitness_Rating = "Below Average"; }
                        else if (AC < 27)
                        { Fitness_Rating = "Poor"; }

                    }

                    else if (age >= 30 & age <= 39)
                    {
                        if (AC >= 46)
                        { Fitness_Rating = "Excellent"; }
                        else if (AC >= 36 & AC < 46)
                        { Fitness_Rating = "Good"; }
                        else if (AC >= 30 & AC < 36)
                        { Fitness_Rating = "Average"; }
                        else if (AC >= 25 & AC < 30)
                        { Fitness_Rating = "Below Average"; }
                        else if (AC < 25)
                        { Fitness_Rating = "Poor"; }
                    }

                    else if (age >= 40 & age <= 49)
                    {
                        if (AC >= 43)
                        { Fitness_Rating = "Excellent"; }
                        else if (AC >= 34 & AC < 43)
                        { Fitness_Rating = "Good"; }
                        else if (AC >= 28 & AC < 34)
                        { Fitness_Rating = "Average"; }
                        else if (AC >= 22 & AC < 28)
                        { Fitness_Rating = "Below Average"; }
                        else if (AC < 22)
                        { Fitness_Rating = "Poor"; }
                    }

                    else if (age >= 50 & age <= 59)
                    {
                        if (AC >= 41)
                        { Fitness_Rating = "Excellent"; }
                        else if (AC >= 33 & AC < 41)
                        { Fitness_Rating = "Good"; }
                        else if (AC >= 26 & AC < 33)
                        { Fitness_Rating = "Average"; }
                        else if (AC >= 21 & AC < 26)
                        { Fitness_Rating = "Below Average"; }
                        else if (AC < 21)
                        { Fitness_Rating = "Poor"; }

                    }

                    else if (age >= 60 & age <= 65)
                    {
                        if (AC >= 39)
                        { Fitness_Rating = "Excellent"; }
                        else if (AC >= 31 & AC < 39)
                        { Fitness_Rating = "Good"; }
                        else if (AC >= 24 & AC < 31)
                        { Fitness_Rating = "Average"; }
                        else if (AC >= 19 & AC < 24)
                        { Fitness_Rating = "Below Average"; }
                        else if (AC < 19)
                        { Fitness_Rating = "Poor"; }

                    }

                    break;
            }

            lblFitnessRating.Text = Fitness_Rating; // display the fitness rating 

        } // public void fitnessRating()
    }


}
