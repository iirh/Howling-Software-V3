using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Authed;
using Newtonsoft.Json;
using Jose.jwe;
using System.Net.Mail;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using ManualMapInjection.Injection;
using System.Diagnostics;

namespace Howling_LoaderV3
{
    public partial class Main : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        Auth auth = new Auth();
        string path = Path.Combine(Directory.GetCurrentDirectory() + "\\", "Howling-Loader.exe");
        AutoEvNr.Data data = new JavaScriptSerializer().Deserialize<AutoEvNr.Data>(AutoEvNr.wb.DownloadString(Globals.UpdateLink));
        Checker checker = new Checker();

        public Main()
        {
            AutoEvNr.Check4Update(Globals.version);
            bool isLegit = checker.CheckFiles();
            if (!isLegit)
             {
                 Error.CstmError.Show("You don't have permission to access the tool due wrong/modified files!");
                 Application.Exit();
             }
            
            bool authed = auth.Authenticate(Globals.secret_key);
            if (authed != true)
            {
                Error.CstmError.Show("Please contact the Administration");
                Environment.Exit(0);
            }
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            if (Globals.helper == true)
            {
                tabControl1.SelectedTab = tabPage5;
                tabControl2.Visible = false;
                label1.Text = "Howling Software | Update";
            }
            timer1.Start();
            CheatList.Items.Add(Globals.cheat1);
            panel14.Size = new Size(582, 77);
            tabControl2.Location = new Point(-5, +-23);
            string text2 = new WebClient().DownloadString(Globals.informationlink);
            richTextBox1.Text = text2;
            label18.Text = "available cheats: " + CheatList.Items.Count;
            user_box.Text = Properties.Settings.Default.username;
            pw_box.Text = Properties.Settings.Default.password;
            checkBox1.Checked = Properties.Settings.Default.check;
        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void panel9_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel10_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
        }
        

        private void panel11_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool authed = auth.Authenticate(Globals.secret_key);
            string username1 = register_username.Text;
            string password = register_password.Text;
            string email = register_password.Text;
            string token = register_license.Text;
            bool register = auth.Register(username1, password, email, token);

            if (authed != true)
            {
                Error.CstmError.Show("Error. Please restart the Application");

            }

            if (register == true)
            {
                Error.CstmError.Show("User " + username1 + " successfully registered!");
                tabControl1.SelectedTab = tabPage1;
            }
            else
            {
                Error.CstmError.Show("Error please check your Informations");
                return;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
        }

        private void Launch_button_Click(object sender, EventArgs e)
        {
            Globals.username = user_box.Text;
            bool right = false;

            string password = pw_box.Text;
            if (user_box.Text == "" || pw_box.Text == "")
            {
                Error.CstmError.Show("Username or Password empty!");
            }
            else
            {
                bool login = auth.Login(Globals.username, password);     //Login
                if (login == true)
                {
                    if (auth.user.banned == true)
                    {
                        Error.CstmError.Show("Your Account have been banned!");
                        auth.OnBannedUser += Auth_OnBannedUser;
                    }
                    else
                    {
                        right = true;
                    }
                    if (auth.user.expired == true)
                    {
                        Error.CstmError.Show("Account time expired!");
                        auth.OnInvalidUser += Auth_OnInvalidUser;
                    }
                    else
                    {
                        right = true;
                    }
                    if (right == true)
                    {
                        Error.CstmError.Show("Welcome " + Globals.username + " !");
                        Properties.Settings.Default.username = user_box.Text;
                        Properties.Settings.Default.password = pw_box.Text;
                        Properties.Settings.Default.Save();
                        tabControl1.SelectedTab = tabPage4;
                        tabControl2.SelectedTab = tabPage7;
                        
                    }
                }
                else
                {
                    Error.CstmError.Show("Username or Password incorrect!");
                }
            }
        }

        private void Auth_OnInvalidUser(object sender, OnUserInvalidEvent e)
        {
            throw new NotImplementedException();
        }

        private void Auth_OnBannedUser(object sender, OnUserBannedEvent e)
        {
            throw new NotImplementedException();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (MailMessage mailMessage = new MailMessage())
            {
                string email = YourEmail_box.Text;
                mailMessage.From = new MailAddress(YourEmail_box.Text);
                mailMessage.To.Add(Globals.supportemail);
                mailMessage.Subject = "New Email from " + email.ToString() + " [Howling-Software]";
                mailMessage.Body = this.subjectbox.Text;
                mailMessage.IsBodyHtml = true;
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(Globals.credical_user, Globals.credical_pass);
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(mailMessage);
                }
            }
        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void Ok_btn_Click(object sender, EventArgs e)
        {
            try

            {

                if (!data.isClose)
                {
                    Error.CstmError.Show("Tool closed contact Discord: root#1418");
                    Application.ExitThread();
                }
                if (data.version > Globals.version)
                {
                    AutoEvNr.wb.DownloadProgressChanged += new System.Net.DownloadProgressChangedEventHandler(Wb_DownloadProgressChanged);
                    AutoEvNr.wb.DownloadFileCompleted += new AsyncCompletedEventHandler(Wb_DownloadFileCompleted);
                    AutoEvNr.wb.DownloadFileAsync(new Uri(data.downloadLink), data.filename);
                }
            }
            catch
            {
                Error.CstmError.Show("Someting went wrong. Restart the Application");
            }
        }

        private void Wb_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            string path1 = Path.Combine(Directory.GetCurrentDirectory() + "\\", data.filename);
            File.Move(path, path + ".old");
            File.Move(path1, "Howling-Loader.exe");
            Error.CstmError.Show("Download complete");
            Application.ExitThread();
        }

        private void Wb_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            progressBar1.Maximum = (int)e.TotalBytesToReceive / 100;
            progressBar1.Value = (int)e.BytesReceived / 100;
        }

        private void panel20_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
            tabControl2.SelectedTab = tabPage6;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                label1.Text = "Howling Software | Account";
            }
            else
            {
                if (tabControl1.SelectedTab == tabPage2) label1.Text = "Howling Software | Information";
                else
                {
                    if (tabControl1.SelectedTab == tabPage3) label1.Text = "Howling Software | Register";
                    else
                    {
                        if (tabControl1.SelectedTab == tabPage4) label1.Text = "Howling Software | Loader";
                        else
                        {
                            if (tabControl1.SelectedTab == tabPage9) label1.Text = "Howling Software | Settings";
                            else return;
                        }
                    }
                }
            }
        }

        private void panel21_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage9;
            tabControl2.SelectedTab = tabPage8;
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel20_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel21_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel22_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage4;
            tabControl2.SelectedTab = tabPage7;
        }

        private void panel23_Click(object sender, EventArgs e)
        {
            
        }

        #region Best Coder 
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                panel1.BackColor = colorDialog1.Color;
                panel4.BackColor = colorDialog1.Color;
                panel7.BackColor = colorDialog1.Color;
                panel8.BackColor = colorDialog1.Color;
                panel5.BackColor = colorDialog1.Color;
                panel2.BackColor = colorDialog1.Color;
                panel6.BackColor = colorDialog1.Color;

                Login_button.FlatAppearance.BorderColor = colorDialog1.Color;
                Register_Button.FlatAppearance.BorderColor = colorDialog1.Color;
                button2.FlatAppearance.BorderColor = colorDialog1.Color;
                button5.FlatAppearance.BorderColor = colorDialog1.Color;
                button6.FlatAppearance.BorderColor = colorDialog1.Color;
                button3.FlatAppearance.BorderColor = colorDialog1.Color;
                button1.FlatAppearance.BorderColor = colorDialog1.Color;
                Launch_Button.FlatAppearance.BorderColor = colorDialog1.Color;
                Ok_btn.FlatAppearance.BorderColor = colorDialog1.Color;
                button4.FlatAppearance.BorderColor = colorDialog1.Color;

                panel10.BackColor = colorDialog1.Color;
                panel11.BackColor = colorDialog1.Color;
                panel20.BackColor = colorDialog1.Color;
                panel21.BackColor = colorDialog1.Color;
                panel22.BackColor = colorDialog1.Color;
                
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(0, 162, 0);
            panel4.BackColor = Color.FromArgb(0, 162, 0);
            panel7.BackColor = Color.FromArgb(0, 162, 0);
            panel8.BackColor = Color.FromArgb(0, 162, 0);
            panel5.BackColor = Color.FromArgb(0, 162, 0);
            panel2.BackColor = Color.FromArgb(0, 162, 0);
            panel6.BackColor = Color.FromArgb(0, 162, 0);

            Login_button.FlatAppearance.BorderColor = Color.FromArgb(0, 162, 0);
            Register_Button.FlatAppearance.BorderColor = Color.FromArgb(0, 162, 0);
            button2.FlatAppearance.BorderColor = Color.FromArgb(0, 162, 0);
            button3.FlatAppearance.BorderColor = Color.FromArgb(0, 162, 0);
            button5.FlatAppearance.BorderColor = Color.FromArgb(0, 162, 0);
            button6.FlatAppearance.BorderColor = Color.FromArgb(0, 162, 0);
            button1.FlatAppearance.BorderColor = Color.FromArgb(0, 162, 0);
            Launch_Button.FlatAppearance.BorderColor = Color.FromArgb(0, 162, 0);
            Ok_btn.FlatAppearance.BorderColor = Color.FromArgb(0, 162, 0);
            button4.FlatAppearance.BorderColor = Color.FromArgb(0, 162, 0);

            panel10.BackColor = Color.FromArgb(0, 162, 0);
            panel11.BackColor = Color.FromArgb(0, 162, 0);
            panel20.BackColor = Color.FromArgb(0, 162, 0);
            panel21.BackColor = Color.FromArgb(0, 162, 0);
            panel22.BackColor = Color.FromArgb(0, 162, 0);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (colorDialog2.ShowDialog() == DialogResult.OK)
            {
                Login_button.ForeColor = colorDialog2.Color;
                Register_Button.ForeColor = colorDialog2.Color;
                button2.ForeColor = colorDialog2.Color;
                button3.ForeColor = colorDialog2.Color;
                button1.ForeColor = colorDialog2.Color;
                Launch_Button.ForeColor = colorDialog2.Color;
                Ok_btn.ForeColor = colorDialog2.Color;
                button4.ForeColor = colorDialog2.Color;

                label1.ForeColor = colorDialog2.Color;
                label17.ForeColor = colorDialog2.Color;
                label4.ForeColor = colorDialog2.Color;
                label5.ForeColor = colorDialog2.Color;
                label7.ForeColor = colorDialog2.Color;
                label8.ForeColor = colorDialog2.Color;
                label6.ForeColor = colorDialog2.Color;
                label10.ForeColor = colorDialog2.Color;
                label12.ForeColor = colorDialog2.Color;
                label15.ForeColor = colorDialog2.Color;
                label16.ForeColor = colorDialog2.Color;
                label2.ForeColor = colorDialog2.Color;
                label3.ForeColor = colorDialog2.Color;
                label11.ForeColor = colorDialog2.Color;
                label13.ForeColor = colorDialog2.Color;
                label14.ForeColor = colorDialog2.Color;
                label18.ForeColor = colorDialog2.Color;

                richTextBox1.ForeColor = colorDialog2.Color;

                Login_button.ForeColor = colorDialog2.Color;
                Register_Button.ForeColor = colorDialog2.Color;
                register_license.ForeColor = colorDialog2.Color;
                register_password.ForeColor = colorDialog2.Color;
                register_username.ForeColor = colorDialog2.Color;
                button2.ForeColor = colorDialog2.Color;
                button3.ForeColor = colorDialog2.Color;
                button1.ForeColor = colorDialog2.Color;
                button5.ForeColor = colorDialog2.Color;
                button4.ForeColor = colorDialog2.Color;
                button6.ForeColor = colorDialog2.Color;
                user_box.ForeColor = colorDialog2.Color;
                pw_box.ForeColor = colorDialog2.Color;
                Launch_Button.ForeColor = colorDialog2.Color;
                Ok_btn.ForeColor = colorDialog2.Color;
                checkBox1.ForeColor = colorDialog2.Color;

                YourEmail_box.ForeColor = colorDialog2.Color;
                subjectbox.ForeColor = colorDialog2.Color;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Login_button.ForeColor = colorDialog2.Color;
            Register_Button.ForeColor = colorDialog2.Color;
            button2.ForeColor = colorDialog2.Color;
            button3.ForeColor = colorDialog2.Color;
            button1.ForeColor = colorDialog2.Color;
            Launch_Button.ForeColor = colorDialog2.Color;
            Ok_btn.ForeColor = colorDialog2.Color;
            button4.ForeColor = colorDialog2.Color;

            label1.ForeColor = Color.White;
            label17.ForeColor = Color.White;
            label4.ForeColor = Color.White;
            label5.ForeColor = Color.White;
            label7.ForeColor = Color.White;
            label8.ForeColor = Color.White;
            label6.ForeColor = Color.White;
            label10.ForeColor = Color.White;
            label12.ForeColor = Color.White;
            label15.ForeColor = Color.White;
            label16.ForeColor = Color.White;
            label2.ForeColor = Color.White;
            label3.ForeColor = Color.White;
            label11.ForeColor = Color.White;
            label13.ForeColor = Color.White;
            label14.ForeColor = Color.White;
            label18.ForeColor = Color.White;

            richTextBox1.ForeColor = Color.White;

            Login_button.ForeColor = Color.White;
            Register_Button.ForeColor = Color.White;
            register_license.ForeColor = Color.White;
            register_password.ForeColor = Color.White;
            register_username.ForeColor = Color.White;
            button2.ForeColor = Color.White;
            button3.ForeColor = Color.White;
            button1.ForeColor = Color.White;
            button5.ForeColor = Color.White;
            button4.ForeColor = Color.White;
            button6.ForeColor = Color.White;
            user_box.ForeColor = Color.White;
            pw_box.ForeColor = Color.White;
            Launch_Button.ForeColor = Color.White;
            Ok_btn.ForeColor = Color.White;
            checkBox1.ForeColor = Color.White;

            YourEmail_box.ForeColor = Color.White;
            subjectbox.ForeColor = Color.White;
        }
        #endregion
        int r = 244;
        int g = 65;
        int b = 65;

        private void timer2_Tick(object sender, EventArgs e)
        {

        }
        #region Wtf is this
        private void timer_r_Tick(object sender, EventArgs e)
        {
            if (b >= 244)
            {
                r -= 1;
                panel1.BackColor = Color.FromArgb(r, g, b);
                panel4.BackColor = Color.FromArgb(r, g, b);
                panel7.BackColor = Color.FromArgb(r, g, b);
                panel8.BackColor = Color.FromArgb(r, g, b);
                panel5.BackColor = Color.FromArgb(r, g, b);
                panel2.BackColor = Color.FromArgb(r, g, b);
                panel6.BackColor = Color.FromArgb(r, g, b);

                Login_button.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                Register_Button.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button2.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button3.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button1.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button5.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button6.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                Launch_Button.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                Ok_btn.FlatAppearance.BorderColor =  Color.FromArgb(r, g, b);
                button4.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);

                panel10.BackColor = Color.FromArgb(r, g, b);
                panel11.BackColor = Color.FromArgb(r, g, b);
                panel20.BackColor = Color.FromArgb(r, g, b);
                panel21.BackColor = Color.FromArgb(r, g, b);
                panel22.BackColor = Color.FromArgb(r, g, b);
                if (r <= 65)
                {
                    timer_r.Stop();
                    timer_g.Start();

                    
                }
            }

            if (b <= 65)
            {
                r += 1;
                panel1.BackColor = Color.FromArgb(r, g, b);
                panel4.BackColor = Color.FromArgb(r, g, b);
                panel7.BackColor = Color.FromArgb(r, g, b);
                panel8.BackColor = Color.FromArgb(r, g, b);
                panel5.BackColor = Color.FromArgb(r, g, b);
                panel2.BackColor = Color.FromArgb(r, g, b);
                panel6.BackColor = Color.FromArgb(r, g, b);

                Login_button.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                Register_Button.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button2.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button3.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button5.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button6.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button1.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                Launch_Button.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                Ok_btn.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button4.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);

                panel10.BackColor = Color.FromArgb(r, g, b);
                panel11.BackColor = Color.FromArgb(r, g, b);
                panel20.BackColor = Color.FromArgb(r, g, b);
                panel21.BackColor = Color.FromArgb(r, g, b);
                panel22.BackColor = Color.FromArgb(r, g, b);
                if (r >= 244)
                {
                    timer_r.Stop();
                    timer_g.Start();

                }
            }
        }
        
        private void timer_b_Tick(object sender, EventArgs e)
        {
            if (g <= 65)
            {
                b += 1;
                panel1.BackColor = Color.FromArgb(r, g, b);
                panel4.BackColor = Color.FromArgb(r, g, b);
                panel7.BackColor = Color.FromArgb(r, g, b);
                panel8.BackColor = Color.FromArgb(r, g, b);
                panel5.BackColor = Color.FromArgb(r, g, b);
                panel2.BackColor = Color.FromArgb(r, g, b);
                panel6.BackColor = Color.FromArgb(r, g, b);

                Login_button.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                Register_Button.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button2.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button5.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button6.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button3.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button1.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                Launch_Button.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                Ok_btn.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button4.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);

                panel10.BackColor = Color.FromArgb(r, g, b);
                panel11.BackColor = Color.FromArgb(r, g, b);
                panel20.BackColor = Color.FromArgb(r, g, b);
                panel21.BackColor = Color.FromArgb(r, g, b);
                panel22.BackColor = Color.FromArgb(r, g, b);
                if (b >= 244)
                {
                    timer_b.Stop();
                    timer_r.Start();

                    
                }
            }

            if (g >= 244)
            {
                b -= 1;
                panel1.BackColor = Color.FromArgb(r, g, b);
                panel4.BackColor = Color.FromArgb(r, g, b);
                panel7.BackColor = Color.FromArgb(r, g, b);
                panel8.BackColor = Color.FromArgb(r, g, b);
                panel5.BackColor = Color.FromArgb(r, g, b);
                panel2.BackColor = Color.FromArgb(r, g, b);
                panel6.BackColor = Color.FromArgb(r, g, b);

                Login_button.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                Register_Button.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button2.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button3.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button5.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button6.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button1.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                Launch_Button.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                Ok_btn.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button4.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);

                panel10.BackColor = Color.FromArgb(r, g, b);
                panel11.BackColor = Color.FromArgb(r, g, b);
                panel20.BackColor = Color.FromArgb(r, g, b);
                panel21.BackColor = Color.FromArgb(r, g, b);
                panel22.BackColor = Color.FromArgb(r, g, b);
                if (b <= 65)
                {
                    timer_b.Stop();
                    timer_r.Start();

                    
                }
            }
        }

        private void timer_g_Tick(object sender, EventArgs e)
        {
            if (r <= 65)
            {
                g += 1;
                panel1.BackColor = Color.FromArgb(r, g, b);
                panel4.BackColor = Color.FromArgb(r, g, b);
                panel7.BackColor = Color.FromArgb(r, g, b);
                panel8.BackColor = Color.FromArgb(r, g, b);
                panel5.BackColor = Color.FromArgb(r, g, b);
                panel2.BackColor = Color.FromArgb(r, g, b);
                panel6.BackColor = Color.FromArgb(r, g, b);

                Login_button.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                Register_Button.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button2.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button3.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button5.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button6.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button1.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                Launch_Button.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                Ok_btn.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button4.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);

                panel10.BackColor = Color.FromArgb(r, g, b);
                panel11.BackColor = Color.FromArgb(r, g, b);
                panel20.BackColor = Color.FromArgb(r, g, b);
                panel21.BackColor = Color.FromArgb(r, g, b);
                panel22.BackColor = Color.FromArgb(r, g, b);
                if (g >= 244)
                {
                    timer_g.Stop();
                    timer_b.Start();

                    
                }
            }

            if (r >= 244)
            {
                g -= 1;
                panel1.BackColor = Color.FromArgb(r, g, b);
                panel4.BackColor = Color.FromArgb(r, g, b);
                panel7.BackColor = Color.FromArgb(r, g, b);
                panel8.BackColor = Color.FromArgb(r, g, b);
                panel5.BackColor = Color.FromArgb(r, g, b);
                panel2.BackColor = Color.FromArgb(r, g, b);
                panel6.BackColor = Color.FromArgb(r, g, b);

                Login_button.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                Register_Button.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button2.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button3.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button1.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button5.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button6.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                Launch_Button.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                Ok_btn.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);
                button4.FlatAppearance.BorderColor = Color.FromArgb(r, g, b);

                panel10.BackColor = Color.FromArgb(r, g, b);
                panel11.BackColor = Color.FromArgb(r, g, b);
                panel20.BackColor = Color.FromArgb(r, g, b);
                panel21.BackColor = Color.FromArgb(r, g, b);
                panel22.BackColor = Color.FromArgb(r, g, b);
                if (g <= 65)
                {
                    timer_g.Stop();
                    timer_b.Start();

                    
                }
            }
        }
        #endregion
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                Properties.Settings.Default.check = checkBox1.Checked;
                Properties.Settings.Default.Save();
                timer_b.Start();
            }
            else
            {
                Properties.Settings.Default.check = checkBox1.Checked;
                Properties.Settings.Default.Save();
                timer_b.Stop();
                timer_r.Stop();
                timer_g.Stop();
                 r = 244;
                 g = 65;
                 b = 65;
            }
        }

        private void panel14_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Launch_Button_Click_1(object sender, EventArgs e)
        {
            string selectedcheat = CheatList.GetItemText(CheatList.SelectedItem);
            var name = "csgo";
            var target = Process.GetProcessesByName(name).FirstOrDefault();
            if (target == null)
            {
                Error.CstmError.Show("Process not found");
                return;
            }

            if (selectedcheat == "Howling Cheat CSGO")
            {
                var injector = new ManualMapInjector(target) { AsyncInjection = true };
                string boom = $"hmodule = 0x{injector.Inject(Howling_LoaderV3.Properties.Resources.Cheat).ToInt64():x8}"; // put your cheat in  Resources 
            }

            if (selectedcheat == "")
            {
                Error.CstmError.Show("No Cheat selected");
            }
        }
    }
}
