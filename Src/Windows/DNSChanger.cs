using System;
using System.Windows.Forms;
using System.Management;

namespace DNSChanger
{
    public partial class DNSChanger : Form
    {
        public DNSChanger()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Google DNS");
            comboBox1.Items.Add("Open DNS");

            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                textBoxDNS1.Text = "8.8.8.8";
                textBoxDNS2.Text = "8.8.4.4";
            }

            else if (comboBox1.SelectedIndex == 1)
            {
                textBoxDNS1.Text = "208.67.222.222";
                textBoxDNS2.Text = "208.67.220.220";
            }
        }

        int setORdefault; //default = 0 , set = 1

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            setORdefault = 0;
            setDNS("", "");
        }

        private void buttonSetDNS_Click(object sender, EventArgs e)
        {
            setORdefault = 1;
            setDNS(textBoxDNS1.Text, textBoxDNS2.Text);
        }

        public void setDNS(string dns1, string dns2)
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");

            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"])
                {
                    ManagementBaseObject DnsEntry = mo.GetMethodParameters("SetDNSServerSearchOrder");

                    if (setORdefault == 1)
                    {
                        string[] dns = new string[2];
                        dns[0] = dns1;
                        dns[1] = dns2;
                        DnsEntry["DNSServerSearchOrder"] = dns;
                    }
                    else
                    {
                        DnsEntry["DNSServerSearchOrder"] = new string[0];
                    }

                    ManagementBaseObject DnsMbo = mo.InvokeMethod("SetDNSServerSearchOrder", DnsEntry, null);

                    int returnCode = int.Parse(DnsMbo["returnvalue"].ToString());
                    if (returnCode == 0)
                    {
                        MessageBox.Show("Successful.");
                    }
                    else
                    {
                        MessageBox.Show("You need to run this app with administrator privileges.");
                    }
                }
            }
        }
    }
}