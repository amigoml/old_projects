using System;

using System.Net.Sockets;

using System.Windows.Forms;

namespace Clientscaya_Chast1
{
    public partial class Form1 : Form
    {
        Client client1=new Client();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Form1_Closing(object sender, EventArgs e)
        {
            client1.Closing();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                try
                {
                    client1.ConnectToServer();
                    client1.Send("Hello! I'm " + textBox1.Text ); 
                } 
                catch (SocketException SE)
                {
                     MessageBox.Show(SE.Message);
                }
              
            }
            else
            {
                MessageBox.Show("введи имя! ");
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
           try
           {
                client1.Send(textBox1.Text+ " вещает "); // клиент только отправляет сообщения =)
           }
           catch (SocketException SE)
           {
               MessageBox.Show(SE.Message);
           }
           
          
        }
    }
}
