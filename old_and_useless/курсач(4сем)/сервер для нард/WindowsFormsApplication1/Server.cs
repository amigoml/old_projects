using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class Server
    {
        
        private bool firstClient;
        private bool isServerWork;  // статус сервера
        private Socket lisinter;    // сокет сервера
        private int port = 1000;    // порт для просулшки входящих сообщений
        private IPEndPoint point;   // точка (адрес порт) для прослушки сообщений

        private int k1;
        private int k2;
       
        private Socket[] myClient = new Socket[2];
        

        // сервер начинает работу. слушаем входящ сообщения
        public void ServerRun()
        {
            isServerWork = true;
            lisinter = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            point = new IPEndPoint(IPAddress.Any, port);
            lisinter.Bind(point);  // связываем
            lisinter.Listen(1);    // слушаем, в очереди подкл max 2
            ServerHandle();

        }


        
        public void ServerHandle()    // сервер подключается к клиенту. 
        {
            Thread thread = new Thread(delegate()
                {

                            while (isServerWork)
                                {
                                   Socket client = lisinter.Accept(); // для созданного подключения создаем объект

                                   if (!firstClient)
                                   {
                                       firstClient = true;
                                       myClient[0] = client;
                                       SendMessage(client,"!"+1.ToString());  // отслылаем каким он по счету подключился
                                       Thread.Sleep(100);
                                       SendMessage(client,"-" + 1.ToString()); // ставим по умолчанию ход первого
                                   }
                                   else
                                   {
                                       myClient[1] = client;
                                       SendMessage(client,"!" + 2.ToString());  // второй по подключению
                                       Thread.Sleep(100);
                                       SendMessage(client, "-" + 1.ToString());  // ходит первый
                                       Thread.Sleep(100);
                                       //SendMessage(client, 1.ToString() + "*" + 1.ToString() + "*" + k1.ToString() + "*" + k2.ToString() + "*");  // начальные кости
                                   }
                                     
                                    MessageReceive(client);       // будем принимать пакеты.

                                }
   
                            
                });
           
            thread.Start();
        }

        // будем получать / обрабатывать сообщения от client и отправлять ему ответ 

        public void MessageReceive(Socket receiveClient)
        {
        Thread thread1 = new Thread(delegate()
           {
               try
               {
                   while (isServerWork)
                   {
                       byte[] bufer = new byte[256]; // для принятых байтов
                       receiveClient.Receive(bufer); // получаем сообщения в виде байтов от client

                       string msg = Encoding.UTF8.GetString(bufer);

                       
                       string[] coordinates = msg.Split(new Char[] { '*' }); // парсим строку

                       if (coordinates[0] =="$") 
                       {
                        k1 = Convert.ToInt32(coordinates[1]);
                        k2 = Convert.ToInt32(coordinates[2]);  
                       }
                       
                       

                       if (receiveClient == myClient[0])
                       {
                           SendMessage(myClient[1], msg);   // приняли от первого, отправляем второму
                       }
                       else
                       {
                           SendMessage(myClient[0], msg);  // от 2 к 1
                       }

                   }

               }
               catch (SocketException socketException)
               {
                //   MessageBox.Show(socketException.ToString());
               }

               
               });
           
           thread1.Start();
                  
         
        }

        public void SendMessage(Socket sendClient, string msg)
        {
            try
            {
                if (sendClient != null)
                {
                   sendClient.Send(Encoding.UTF8.GetBytes(msg));
                }
            }
            catch (SocketException socketException)
            {
              //  MessageBox.Show(socketException.ToString());
            }
            
        }

   
        public void closeme()
        {
            isServerWork = false;
            try
            {
                if (lisinter != null)
                {
                 lisinter.Shutdown(SocketShutdown.Both);
                 lisinter.Close();  
                }

            }
            catch (SocketException socketException)
            {
              //  MessageBox.Show(socketException.ToString());
            }
            
        }


    }
}
