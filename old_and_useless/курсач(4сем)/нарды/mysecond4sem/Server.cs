using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;



namespace mysecond4sem
{
    class Server
    {
        //private bool firstClient;
        public bool isServerWork;  // статус сервера
        public Socket lisinter;    // сокет сервера
        public int port = 1000;    // порт для просулшки входящих сообщений
        public IPEndPoint point;   // точка (адрес порт) для прослушки сообщений

        private Game game;
        private Socket myClient;
        

        // сервер начинает работу. слушаем входящ сообщения
        public void ServerRun(Game g)
        {
            game = g;
            //firstClient = false;
            isServerWork = true;

            lisinter = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            point = new IPEndPoint(IPAddress.Any, port);
            lisinter.Bind(point);  // связываем
            lisinter.Listen(1);    // слушаем, в очереди подкл max 1


        }


        // сервер подключается к клиенту. 
        public void ServerHandle()
        {
            
            Thread thread = new Thread(delegate()
                {
                    
                             while (isServerWork)
                                {
                                    Socket client = lisinter.Accept(); // для созданного подключения создаем объект
                                                                       
                                    {
                                        //firstClient = true;
                                        myClient = client;
                                        if (game!=null)
                                        {
                                            SendMessage("-" + game.getWhoMovedInt().ToString());
                                            SendMessage(0.ToString() + "*" + 0.ToString() + "*" + game.k1.ToString() + "*" + game.k2.ToString()); 
                                        }
                                       
                                    }

                                    MessageReceive(client);       // будем принимать пакеты.
                                }
                });
           
            thread.Start();
        }

        // будем получать / обрабатывать сообщения от client и отправлять ему ответ 

        public void MessageReceive(Socket receiveClient)
        {
        //Thread thread1 = new Thread(delegate()
        //   {
          //  Control.CheckForIllegalCrossThreadCalls = false;    
               while (isServerWork)
                {
                    byte[] bufer = new byte[256]; // для принятых байтов
                    receiveClient.Receive(bufer); // получаем сообщения в виде байтов от client

                    string msg = Encoding.UTF8.GetString(bufer);


                    
                        string[] coordinates = msg.Split(new Char[] { '*' }); // парсим строку

                        int x = Convert.ToInt32(coordinates[0]);
                        int y = Convert.ToInt32(coordinates[1]);
                        int k1 = Convert.ToInt32(coordinates[2]);
                        int k2 = Convert.ToInt32(coordinates[3]);
                        game.k1 = k1;
                        game.k2 = k2;
                        game.BonesIsKnowing = true;
                        game.GoFromLocal();

                        game.ClickGetN(x, y);
                    

                    
                     
                }
            //    });
           
          //  thread1.Start();
                  
         
        }

        public void SendMessage(string msg)
        {
            myClient.Send(Encoding.UTF8.GetBytes(msg));
        }

   
        public void closeme()
        {
            isServerWork = false;
            lisinter.Shutdown(SocketShutdown.Both);
            lisinter.Close();
        }


    }


}

