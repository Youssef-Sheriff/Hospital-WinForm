using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using static System.Net.WebRequestMethods;
using System.Xml.Linq;
using File = System.IO.File;

namespace Binary_File
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            CreateBtn.Visible = true; 
            DeleteBtn.Visible = true;
        }

        private void CreateBtn_Click(object sender, EventArgs e)
        {
            if (!FilenametextBox.Text.Equals(""))
            {
                Info.filename = "E:\\" + FilenametextBox.Text + ".txt";
            }
            else
            {
               MessageBox.Show("Enter File Name", "Note",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!File.Exists(Info.filename)) // if the file does not exists  
            {
                File.Create(Info.filename).Close();// We Should include using System.IO;
                MessageBox.Show("File is Created Successfully", "Note",MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                errorLabel.Visible = true; // Lable2 : visible = false  

        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            Info.filename = "E:\\" + FilenametextBox.Text + ".txt";
            File.Delete(Info.filename);
            MessageBox.Show("File is Deleted");
            FilenametextBox.Clear();
            errorLabel.Visible = false;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
       
            DeleteBtn.Visible = true;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateBtn.Visible = true;
           
        }

        private void addNewStudentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            new InsertForm().Show();
        }

        private void viewExistingStudentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            new DisplayForm().Show();
        }

       private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void FilenametextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Logoutbtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new LoginForm().Show();
        }
    }
}
