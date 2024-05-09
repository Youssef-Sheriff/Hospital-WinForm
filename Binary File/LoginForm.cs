using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Binary_File
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            string username = "admin";
            string password = "admin";

            // Hash the password
            string hashedPassword = HashPassword(password);

            // Store the hashed username and password in the binary file
            StoreCredentials("G:\\Login.txt", username, hashedPassword);
            PasstxtBox.PasswordChar = '*';
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
                
        private void Loginbtn_Click(object sender, EventArgs e)
        {
            
            string username = UsertxtBox.Text;
            string password = PasstxtBox.Text;
            if (username == "" && password == "")
            {
                errorLabel.Text = "Please Enter User and Pass";
                errorLabel.Visible = true;
            }
            else if (username == "")
            {
                errorLabel.Text = "Please Enter Username";
                errorLabel.Visible = true;
            }
            else if(password == "")
            {
                errorLabel.Text = "Please Enter Password";
                errorLabel.Visible = true;
            }
            else
            {
                bool isLoggedIn = VerifyLogin("G:\\Login.txt", username, password);
                if (isLoggedIn)
                {
                    this.Hide();
                    new MainForm().Show();
                }
                else
                {
                    errorLabel.Text = "Invalid user or pass";
                    errorLabel.Visible = true;
                }
            }
           

        }
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
        static void StoreCredentials(string filePath, string username, string hashedPassword)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                writer.Write(username);
                writer.Write(hashedPassword);
            }
        }
        private bool VerifyLogin(string filePath, string username, string password)
        {
            if (File.Exists(filePath))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
                {
                    string storedUsername = reader.ReadString();
                    string storedHashedPassword = reader.ReadString();

                    if (storedUsername == username)
                    {
                        string hashedPassword = HashPassword(password);
                        return storedHashedPassword == hashedPassword;
                    }
                }
            }
            return false;
        }
    }
}
