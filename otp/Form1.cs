using Microsoft.Win32;
using OtpNet;
using System;
using System.Windows.Forms;

namespace otp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBoxSecret.Text = GetSecretKey();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                string strSecretKey = textBoxSecret.Text;
                byte[] secretKey = Base32Encoding.ToBytes(strSecretKey);
                Totp totp = new Totp(secretKey);
                string totp_now = totp.ComputeTotp();
                textBoxCode.Text = totp_now;
                SaveSecretKey(strSecretKey);
            }
            catch
            {
                textBoxCode.Text = "Invalid SecretKey";
            }
        }
        private void SaveSecretKey(string strSecretKey)
        {
            try
            {
                RegistryKey rk = Registry.CurrentUser.CreateSubKey(@"Software\TotpSecret");
                rk.SetValue("key", strSecretKey);
            }
            catch
            {
                return;
            }
        }

        private string GetSecretKey()
        {
            try
            {
                RegistryKey rk_app = Registry.CurrentUser.OpenSubKey(@"Software\TotpSecret");
                return rk_app.GetValue("key").ToString();
            }
            catch
            {
                return "";
            }
        }
    }
}
