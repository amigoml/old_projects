using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net.Sockets;

namespace Servernaya_Chast1
{
    public partial class Form1 : Form
    {
        Server server = new Server();

        public Form1()
        {
            InitializeComponent();
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
      
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                server.ServerRun(richTextBox1);
                server.ServerHandle();
            }
            catch (SocketException SE)
            {
                MessageBox.Show(SE.Message);
            }
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            server.closeme();
            Application.Exit();
        }
    }
}
