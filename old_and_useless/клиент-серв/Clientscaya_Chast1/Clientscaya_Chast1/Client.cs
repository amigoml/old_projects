using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace Clientscaya_Chast1
{
    class Client
    {
        private bool clientWork;

       
        private Socket client; // сокет клиента
        private IPAddress serverIP = IPAddress.Parse("127.0.0.1"); // IP сервера
        private int port = 1000; // порт сервера к которому будем подключаться


        public void ConnectToServer()
        {
            clientWork = true;
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                client.Connect(serverIP, port);
            }
            catch (SocketException SE)
            {
                MessageBox.Show(SE.Message);
            }
            

           // Reciever();
        }

       
        public void Send(string msg)
        {
            try
            {
                client.Send(Encoding.UTF8.GetBytes(msg));
            }
            catch (SocketException SE)
            {
                MessageBox.Show(SE.Message);
            }

        }


        public void Reciever()
        {
            
                if (clientWork)
                {
                    byte[] bufer = new byte[256];
                    client.Receive(bufer);                            // получаем сообщение
                    string msg = Encoding.UTF8.GetString(bufer);
                    MessageBox.Show(msg);

                    Send("сообщение от клиента");                           // отправляем обратно 

                }
            
        }

        public void Closing()
        {
            clientWork = false;
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }


    }
}
