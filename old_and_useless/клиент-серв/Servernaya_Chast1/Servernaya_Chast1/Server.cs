using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net;
using NLog;
using System.Net.Sockets;
using System.Threading;


    class Server 
    {
        public bool isServerWork;  // статус сервера
        public Socket lisinter;    // сокет сервера
        public int port = 1000;      // порт для просулшки входящих сообщений
        public IPEndPoint point;   // точка (адрес порт) для прослушки сообщений
        private RichTextBox richTextBox;

        // сервер начинает работу. слушаем входящ сообщения
        public void ServerRun(RichTextBox rchbox)
        {
            var Log = LogManager.GetCurrentClassLogger();
            Log.Trace("запуск сервера"+System.DateTime.Now);

            richTextBox = rchbox;
            isServerWork = true;
            lisinter = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            point = new IPEndPoint(IPAddress.Any, port);
            lisinter.Bind(point);  // связываем
            lisinter.Listen(5);    // слушаем, в очереди max сообщений  5
            
        }


        // сервер подключается к клиенту. 
        public void ServerHandle()
        {
            Thread thread = new Thread(delegate()
                {
                             while (isServerWork)
                                {
                                    Socket client = lisinter.Accept(); // для созданного подключения создаем объект
                                                                       // обращаясь к объекту client мы можем принимать от него и отправлять ему сообщения
                                   

                                         MessageReceive(client);            // будем принимать пакеты. 
                                   
                                }
                });
           
            thread.Start();
        }

        // будем получать / обрабатывать сообщения от client и отправлять ему ответ 

        public void MessageReceive(Socket receiveClient)
        {
        Thread thread1 = new Thread(delegate()
            {
                      
                  while (isServerWork)
                {
                    byte[] bufer = new byte[256]; // для принятых байтов
                    receiveClient.Receive(bufer); // получаем сообщения в виде байтов от client

                    string comeMsg = Encoding.UTF8.GetString(bufer);
                    richTextBox.Text += comeMsg;
                }
                 });
           
            thread1.Start();
                  
                
        }



   

        public void closeme()
        {
            isServerWork = false;
            lisinter.Shutdown(SocketShutdown.Both);
            lisinter.Close();
        }





    }




//   логирование всего. запись в блокнот. стандарт NLog. 
//   разделить  клиента и сервер !
//   разобраться с потоками