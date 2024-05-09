using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Binary_File
{
    public partial class DisplayForm : Form
    {
        public DisplayForm()
        {
            InitializeComponent();
            displayFNtextBox.Text= Info.filename;
        }

        private void DisplayForm_Load(object sender, EventArgs e)
        {
            
        }
        private void DisplayBtn_Click(object sender, EventArgs e)
        {
            BinaryReader br = new BinaryReader(File.Open(Info.filename, FileMode.Open, FileAccess.Read));
            int num_of_records = (int)br.BaseStream.Length / Info.rec_size;
            if (num_of_records > 0) // If The file Not Empty
            {
                DisplayBtn.Text = "Next";// Change the Button Text

                br.BaseStream.Seek(Info.count, SeekOrigin.Begin); // Move to Specific Position in a File                
                NametextBox.Text = br.ReadString(); // Read Name
                TeltextBox.Text = br.ReadString(); // Read Tel
                StatusTextBox.Text = br.ReadString(); // Read Year
                GendertextBox.Text = br.ReadString(); // Read Gender
                AgeTextBox.Text = br.ReadInt32().ToString();
                NumOfRecLabel.Text = num_of_records.ToString();

                FileSizeLabel.Text = br.BaseStream.Length.ToString();
                if ((Info.count / Info.rec_size) >= (num_of_records - 1))
                    // If I reach the End of file , Go to the Beginning of file
                    Info.count = 0;
                else
                    Info.count += Info.rec_size;
            }
            else MessageBox.Show("Empty File");
            br.Close();

        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new MainForm().Show();
        }

        private void Searchbtn_Click(object sender, EventArgs e)
        {
            string searchName = NametextBox.Text.Trim(); // Get the name to search for
            if (string.IsNullOrEmpty(searchName))
            {
                MessageBox.Show("Please enter a name to search.");
                return;
            }

            using (BinaryReader br = new BinaryReader(File.Open(Info.filename, FileMode.Open, FileAccess.Read)))
            {
                int num_of_records = (int)br.BaseStream.Length / Info.rec_size;
                if (num_of_records > 0) // If the file is not empty
                {
                    for (int i = 0; i < num_of_records; i++)
                    {
                        long currentPosition = br.BaseStream.Position; // Store the current position

                        string name = br.ReadString(); // Read Name
                        if (name.Trim().Equals(searchName, StringComparison.OrdinalIgnoreCase)) // Case-insensitive comparison
                        {
                            // Display the record
                            NametextBox.Text = name;
                            TeltextBox.Text = br.ReadString(); // Read Tel
                            StatusTextBox.Text = br.ReadString(); // Read Year
                            GendertextBox.Text = br.ReadString(); // Read Gender
                            AgeTextBox.Text = br.ReadInt32().ToString();

                            NumOfRecLabel.Text = num_of_records.ToString();
                            FileSizeLabel.Text = br.BaseStream.Length.ToString();

                            return; // Exit the method after finding the record
                        }
                        else
                        {
                            // Move to the start of the next record
                            br.BaseStream.Seek(currentPosition + Info.rec_size, SeekOrigin.Begin);
                        }
                    }

                    MessageBox.Show("Name not found.");
                }
                else
                {
                    MessageBox.Show("Empty file.");
                }
            }
        }

        private void Updatebtn_Click(object sender, EventArgs e)
        {
            // Check if any field is empty
            if (string.IsNullOrWhiteSpace(NametextBox.Text) ||
                string.IsNullOrWhiteSpace(TeltextBox.Text) ||
                string.IsNullOrWhiteSpace(StatusTextBox.Text) ||
                string.IsNullOrWhiteSpace(GendertextBox.Text) ||
                string.IsNullOrWhiteSpace(AgeTextBox.Text))
            {
                MessageBox.Show("Please fill in all fields before updating.");
                return;
            }

            try
            {
                // Open the file for reading and writing
                using (FileStream fileStream = File.Open(Info.filename, FileMode.Open, FileAccess.ReadWrite))
                {
                    long currentPosition = 0;
                    bool recordFound = false;

                    using (BinaryReader br = new BinaryReader(fileStream))
                    {
                        int num_of_records = (int)fileStream.Length / Info.rec_size;

                        for (int i = 0; i < num_of_records; i++)
                        {
                            // Store the current position
                            currentPosition = fileStream.Position;

                            string name = br.ReadString(); // Read Name

                            // If the name matches the search name
                            if (name.Trim().Equals(NametextBox.Text.Trim(), StringComparison.OrdinalIgnoreCase))
                            {
                                recordFound = true;

                                // Move the file pointer to the start of the record
                                fileStream.Seek(currentPosition, SeekOrigin.Begin);

                                // Create a BinaryWriter to write updated data to the file
                                using (BinaryWriter bw = new BinaryWriter(fileStream))
                                {
                                    // Write updated data to the file with original length
                                    bw.Write(NametextBox.Text.PadRight(9).Substring(0, 9));
                                    bw.Write(TeltextBox.Text.PadRight(11).Substring(0, 11));
                                    bw.Write(StatusTextBox.Text.PadRight(15).Substring(0, 15));
                                    bw.Write(GendertextBox.Text.Substring(0, 1));
                                    bw.Write(int.Parse(AgeTextBox.Text));
                                }
                                break;
                            }
                            else
                            {
                                // Move to the start of the next record
                                fileStream.Seek(currentPosition + Info.rec_size, SeekOrigin.Begin);
                            }
                        }
                    }

                    if (recordFound)
                    {
                        MessageBox.Show("Record updated successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Record not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating the record: " + ex.Message);
            }
        }
    }
}
