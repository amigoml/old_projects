using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace mysecond4sem
{
    public partial class myActiveForm : Form
    {
        public Game game;       
        private bool isGame ;   // показывает началась игра или нет


        private bool isNet;

        // public bool BonesIsKnowing;


       
        Client client = new Client();

        
        public myActiveForm()
        {
            InitializeComponent();

        }

        private void myActiveForm_Load(object sender, EventArgs e)
        {
        }


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {

            if(isGame && game.BonesIsKnowing)
            {
                if (isNet && (game.getWhoMoved()==game.youFirstNet|| !game.getWhoMoved()==game.youSecondNet))
                {
                    client.SendMessage("-" + game.getWhoMovedInt().ToString());
                    Thread.Sleep(100);
                    client.SendMessage(e.X.ToString() + "*" + e.Y.ToString() + "*" + game.k1.ToString() + "*" + game.k2.ToString() + "*");
                    game.ClickGetN(e.X, e.Y);
                }
                
                if (!isNet)
                {
                    game.ClickGetN(e.X, e.Y);
                }
              
                
            } 

            if (isGame && game.MoveDone)
            {
                game.BonesIsKnowing = false;
   
                  if (isNet && game.getWhoMoved()) 
                  {
                      client.SendMessage("-" + 0.ToString());
                  }
                  if (isNet && !game.getWhoMoved()) 
                  {
                      client.SendMessage("-" + 1.ToString());
                     
                  }

            }
            
        }



        private void button1_Click(object sender, EventArgs e)
        {
            game=new Game(pictureBox1,label1);
            game.RunGame();   // иниц тип игры. создалась доска, игроки.
            isGame = true;
            button1.Visible = false;
            button4.Visible = true;
            button6.Visible = true;


        }

        

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            

            Point po = new Point(0, 0);
            int n;

            if (isGame)
            {
              label3.Text = game.countOfFishka(1).ToString();
              label4.Text = game.countOfFishka(2).ToString();  
            }
            

            if (e.X > 270 & e.X < 490 & e.Y < 155)
            {
                n = (int)((490 - e.X) / 38) + 1;                     //  12.. 17
                po.Y = 10;
                po.X = 490 - (n) * 38 + 16;

            }

            if (e.X > 0 & e.X < 235 & e.Y < 155)
            {
                n = (int)((463 - e.X) / 38) + 1;                      // 18..23 
                po.Y = 10;
                po.X = 463 - (n) * 38 + 16;

            }


            if (e.X > 270 & e.X < 490 & e.Y > 225)
            {
                n = (int)((490 - e.X) / 38) + 1;                      // 6..11
                po.Y = 330;
                po.X = 490 - (n) * 38 + 16;

            }

            if (e.X > 0 & e.X < 235 & e.Y > 225)
            {
                n = (int)((463 - e.X) / 38) + 1;                      // 0..5
                po.Y = 330;
                po.X = 463 - (n) * 38 + 16;

            }


            SolidBrush brush = new SolidBrush(Color.Lime);

            var bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;  // сглаживание поверхностей

            

            if (isGame)                   // перерисовка фишек
            {
                game.drawFishka(g);

            }

            g.FillEllipse(brush, po.X, po.Y, 10, 10);    // зарисовка моего указателя

            pictureBox1.Image = bmp;
            g.Dispose();


            if (isGame&&game.MoveDone)
            {
                button2.Visible = true;
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
           
            game.GetBonePoints();
            game.BonesIsKnowing = true;

            if (isNet)
            {
               client.SendMessage(0.ToString() + "*" + 0.ToString() + "*" + game.k1.ToString() + "*" + game.k2.ToString() + "*"); 
            }
         
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e) // test mode!!!!!!
        {
            game = new Game(pictureBox1, label1);
            game.RunGame(5);   
            isGame = true;
            game.isGame = true;
            button1.Visible = false;
            button2.Visible = true;
        }

     
        private void button6_Click(object sender, EventArgs e)
        {
            button2.Visible = true;
            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            button4.Visible = false;
            button6.Visible = false;
            isNet = true;

            try
            {
                client.ConnectToServer(game); 
            }
            catch (SocketException socketException)
            {
                MessageBox.Show(socketException.ToString());
            }

            Thread.Sleep(10); // все дело в том что сообщениям нужно время чтобы дойти

            if (game.youFirstNet)
            {
                label2.Text = "Я играю нижними!";
            }
            else
            {
                if (game.youSecondNet)
                {
                    label2.Text = "Я играю верхними!";
                }
                else
                {
                    label2.Text = "Проблемы подключения...";
                }
            }
            



        }

        private void button4_Click(object sender, EventArgs e)
        {
            button2.Visible = true;
            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            button4.Visible = false;
            button6.Visible = false;

        }

        private void myActiveForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isNet&&client != null)
            {
                client.Closing();
            }
        }



    }
}
