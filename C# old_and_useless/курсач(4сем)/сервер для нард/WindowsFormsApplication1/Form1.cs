using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{


    public partial class Form1 : Form
    {
        Server server = new Server();

        public Form1()
        {
            InitializeComponent();
            BackColor = Color.Khaki;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                server.ServerRun();
            }
            catch (SocketException SE)
            {
               // MessageBox.Show(SE.Message);
            }

            button1.Text = "Идет прием сообщений";
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            server.closeme();
        }


    }
}
