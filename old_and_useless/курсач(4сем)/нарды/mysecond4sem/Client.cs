using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace mysecond4sem
{
    class Client
    {
        private bool clientWork;
        private bool flag;

        private Game game;
        private Socket client; // сокет клиента
        private IPAddress serverIP = IPAddress.Parse("127.0.0.1"); // IP сервера
        private int port = 1000; // порт сервера к которому будем подключаться


        public void ConnectToServer(Game g)
        {
            game = g;
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

            Thread thread1 = new Thread(delegate()
                                {
                                    MessageRecieve();
                                });
            thread1.Start();

        }

       
        public void SendMessage(string msg)
        {
            try
            {
                client.Send(Encoding.UTF8.GetBytes(msg));
            }
            catch 
            {
              //  MessageBox.Show(SE.Message);
            }

        }


        public void MessageRecieve()  // прием message 
        {
            try
            {

                while (clientWork)
                {
                    byte[] bufer = new byte[256];


                    try
                    {
                        client.Receive(bufer); // получаем сообщение
                    }
                    catch 
                    {
                        //   MessageBox.Show(socketException.ToString());
                    }


                    string msg = Encoding.UTF8.GetString(bufer);


                    if (msg[0] == new Char[] {'!'}[0]) // первое сообщение об игроке. каким он играет
                    {
                        int n = Convert.ToInt16(msg[1]) - 48;
                        if (n == 1)
                        {
                            game.youFirstNet = true;
                            game.youSecondNet = false;
                            game.doska.ShowBones("ваш ход. надо бросить кости");
                        }
                        if (n == 2)
                        {
                            game.youFirstNet = false;
                            game.youSecondNet = true;
                            game.doska.ShowBones("ход соперника");

                        }

                        // отправим в ответ начальные кости)
                        //SendMessage("$"+game.k1.ToString()+"*"+game.k2.ToString()+"*");

                    }
                    else
                    {
                        if (msg[0] == new Char[] {'-'}[0])
                        {
                            int n = Convert.ToInt16(msg[1]) - 48;
                            if (n == 1)
                            {
                                game.isFirstNet = true;
                                game.isSecondNet = false;

                            }
                            else
                            {
                                game.isFirstNet = false;
                                game.isSecondNet = true;

                            }
                            if (flag)
                            {
                                game.setWhoMoved(n);
                            }
                            else
                            {
                                flag = true;
                            }


                        }
                        else
                        {
                            string[] coordinates = msg.Split(new Char[] {'*'}); // парсим строку

                            int x = Convert.ToInt32(coordinates[0]);
                            int y = Convert.ToInt32(coordinates[1]);
                            int k1 = Convert.ToInt32(coordinates[2]);
                            int k2 = Convert.ToInt32(coordinates[3]);
                            game.k1 = k1;
                            game.k2 = k2;
                            game.setWhoMoved(game.getWhoMovedInt()); //////////////////////////////////
                            game.BonesIsKnowing = true;
                            game.GoFromLocal();
                            game.ClickGetN(x, y);

                        }

                    }

                }
            }
            catch
            {
                
            }
            


        }
    

        public void Closing()
        {
            clientWork = false;
            try
            {
              client.Shutdown(SocketShutdown.Both);
              client.Close();
            }
            catch 
            {
              //  MessageBox.Show(socketException.ToString());
            }

        }


    }
}


