using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security.Policy;
using System.Reflection.Emit;

namespace Binary_File
{
    public partial class InsertForm : Form
    {
        public InsertForm()
        {
            InitializeComponent();
            displayFNtextBox.Text = Info.filename;

        }

        private void InsertForm_Load(object sender, EventArgs e)
        {

        }
             

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            BinaryWriter bw = new BinaryWriter(File.Open(Info.filename,FileMode.Open, FileAccess.Write));
            int length = (int)bw.BaseStream.Length;

            if (length != 0) 
            {
                
                bw.BaseStream.Seek(length, SeekOrigin.Begin);

            }


            NametextBox.Text = NametextBox.Text.PadRight(9); 
            bw.Write(NametextBox.Text.Substring(0, 9));

            TeltextBox.Text = TeltextBox.Text.PadRight(11); 
            bw.Write(TeltextBox.Text.Substring(0, 11));

            StatusTextBox.Text = StatusTextBox.Text.PadRight(15);
            bw.Write(StatusTextBox.Text.Substring(0, 15));

            bw.Write(GendertextBox.Text.Substring(0, 1)); // Gender 

            bw.Write(int.Parse(AgetextBox.Text)); // age 

            // 40 + 4 -> 44 
            length += Info.rec_size;
  
            NametextBox.Clear();
            TeltextBox.Clear();
            StatusTextBox.Clear();
            GendertextBox.Clear();
            AgetextBox.Clear();

            NumOfRecLabel.Text = (length / Info.rec_size).ToString(); //  update number of records label
            FileSizeLabel.Text = length.ToString();//update file length label 
            MessageBox.Show(" Data is Saved Successfully ");
            bw.Close();
        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new MainForm().Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void FileSizeLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
