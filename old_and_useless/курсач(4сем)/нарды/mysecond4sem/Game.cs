using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace mysecond4sem
{
    public class Game      
    {
        public PictureBox field;
        private Label label;

        public bool isFirstNet;  // обращение-хранение хода через клиент
        public bool isSecondNet;

        public bool youFirstNet;  // первый второй игрок по сети
        public bool youSecondNet;

        public bool isGame;       // запущена ли игра
        private bool FirstClick;   // выделение фишки
        private bool SecondClick = true;  // ход выделенной фишкой
        private bool movedone;   // показатель завершенности хода

        private bool k1Double;  // показывает что на костях дубль!
        private bool k2Double; 

        public Doska doska;
        public Bones bones;

        private int startdown; // переменные для хранения ходов. позиции старта и финиша
        private int finishdown;

        bool Click;   // отреагировали ли на клик 
       

        private Human human;
        private Human human2;

        private bool firstmoveH1 = true ;
        private bool firstmoveH2 = true ;

        public int k1;        // временное хранение значений костей
        public int k2;

        public bool BonesIsKnowing ;

        private Color colorOfActivePlayer;  // переменная- цвет игрока который активен в данное время

        private string s; // для вывода чей ход сейчас


        public Game(PictureBox field1, Label lb)
        {
            this.field = field1;
            this.label = lb;
        }



        public void RunGame()      //  сделать выбор типа игры. в зависимости от этого создавать либо еще хьюмана либо СPU
        {
                        
            // спросить цвета у игроков было бы замечательным дополнением

            Color cl1 = Color.LightGreen;
            Color cl2 = Color.LightPink;

            doska = new Doska(field, label,cl1,cl2);
            bones = new Bones();


            bool firstPlayer = false; // ход верхих
            bool secondPlayer = false; // ход нижних

            k1 = bones.getNum1();
            k2 = bones.getNum1();

            if (k1 > k2)   // кто первым ходит.
              {
                firstPlayer = true;
              }
            else
              {
                secondPlayer = true;
              }
 

            human = new Human(cl1, true, firstPlayer, doska);
            human2 = new Human(cl2, false, secondPlayer, doska);

            movedone = false;

            // построить поле.
            // кинуть кости
            // дать право хода
            // получить координаты нажитий. выявить позиции. передать. проверить. сделать ход
            // рендер поля после хода
            // заблокировать право хода. -> передать опоненту.
            // .....
            // .......

        }

        //!!! test mode
        public void RunGame(int k)      //  сделать выбор типа игры. в зависимости от этого создавать либо еще хьюмана либо СPU
        {

            // спросить цвета у игроков было бы замечательным дополнением

            Color cl1 = Color.LightGreen;
            Color cl2 = Color.LightPink;

            doska = new Doska(field, label, cl1, cl2,15);
            bones = new Bones();


            bool firstPlayer = false; // ход верхих
            bool secondPlayer = false; // ход нижних

            k1 = bones.getNum1();
            k2 = bones.getNum1();

            if (k1 > k2)  
            {
                firstPlayer = true;
            }
            else
            {
                secondPlayer = true;
            }


            human = new Human(cl1, true, firstPlayer, doska);
            human2 = new Human(cl2, false, secondPlayer, doska);

            movedone = false;

          
        } //!!!!! test mode


        public void GetBonePoints()
        {
            bool dubl=bones.getNum(); // дубль или нет

            k1 = bones.getNum1();

            if (dubl)  // если дубль то
            {
                k2 = k1;    
            }
            else
            {
              k2 = bones.getNum1();  
            }
            
            
           // k2 = k1=5;               // специально делаю дубль

            if (human2.Active)
            {
                setWhoMoved(0);
            }
            else
            {
                setWhoMoved(1);
            }
            /*
                if (human.Active)
                {
                    s = " ход нижних ";
                }
                else
                {
                    s = " ход верхних ";
                }
            
                if (human.Active)
                {
                    colorOfActivePlayer = human.GetColor;
                }
                else
                {
                    colorOfActivePlayer = human2.GetColor;
                }

                if ((youFirstNet == getWhoMoved() && isFirstNet) || (!getWhoMoved() == youSecondNet && isSecondNet))
                {
                    s = "ваш ход";
                }
                else
                {
                    if (youFirstNet || youSecondNet)
                    {
                        s = "ход соперника";
                    }

                }

            
            doska.ShowBones(k1, k2,s);
           */
            if (k1 == k2)
            {
                k1Double = true;
                k2Double = true;
            }
            else
            {
                k1Double = false;
                k2Double = false;
            }

          /*
             if (doska.IsMoveExist(k1, k2, colorOfActivePlayer)) // проверка на сущ ходов. вдруг все закрыто?!
             {

             }
             else
             {
                 if (human.Active && !firstmoveH1 || human2.Active && !firstmoveH2)
                 {
                    TrancferOfMove(); 
                 }
                  
             }
          */
           

        }

      

        public void drawFishka(Graphics g)
        {
            
            doska.drawFishka(g);

        }

      

     public void ClickGetN(int X, int Y)        //  вычисляет  n  по координатам , и он делает ХОД
     {
        Click = true;

        int n = -100;


        if (X > 270 & X < 490 & Y < 155)
        {
            n = (int) ((490 - X)/38) + 1;  //  12.. 17

            n += 11;
        }

        if (X > 0 & X <= 240 & Y < 155)
        {
            n = (int) ((463 - X)/38) + 1; // 18..23

            n += 11;
        }


        if (X >= 270 & X < 490 & Y > 225)
        {
            n = (int) ((490 - X)/38) + 1; // 6..11

            n = 12 - n;
        }

        if (X > 0 & X <= 240 & Y > 225)
        {
            n = (int) ((463 - X)/38) + 1; // 0..5
            if (n == 13)     // для вывода
            {
                  //  n = 12;
            }
            n = 12 - n;

        }

         if (X > 490 && Y > 225) // для вывода верхних
         {
             n = -70;
         }
        
        
         if (n == 24)  // для вывода нижних
         {
             n = 70;
         }

       
        if (FirstClick)
        {
            finishdown = n;
        }
        if (SecondClick)
        {
            startdown = n;
        }
             


        if (FirstClick && !SecondClick && Click)    // второй клик
        {
          
       
            if (doska.IfHodIsOk(startdown, finishdown, k1, k2))  // проверка хода ( очки - разность позиций. не ставить на чужую фишку. )
            {

                if (finishdown == 70)
                {
                    finishdown = 24;
                }
                if (finishdown == -70)
                {
                    finishdown = 12;
                }

                //2
                if (human.Active)
                {
                    human.SecondClick((byte)k1, (byte)k2, n);
                    if (firstmoveH1)
                    {
                        firstmoveH1 = false;
                    }
                }
                if (human2.Active)
                {
                    human2.SecondClick((byte)k1, (byte)k2, n);
                    if (firstmoveH2)
                    {
                        firstmoveH2 = false;
                    }
                }
                //end 2-совершаем ход
                
                //3
                if (finishdown - startdown == k1 || finishdown + (24 - startdown) == k1) // использовали первую кость
                {
                    if (k1Double)
                    {
                        k1Double = false;
                    }
                    else
                    {
                        if (k2Double)
                        {
                            k2Double = false;
                        }
                        else
                        {
                            k1 = 0;
                        }

                    }

                }
                else
                {
                    if (finishdown - startdown == k2 || finishdown + (24 - startdown) == k2)
                        // использовали вторую кость
                    {
                        if (k2Double)
                        {
                            k2Double = false;
                        }
                        else
                        {
                            k2 = 0;
                        }

                    }
                    else
                    {
                        if (finishdown - startdown == 2*k1 || finishdown + (24 - startdown) == 2*k2)
                        {
                            if (k1Double & k2Double)
                            {
                                k1Double = false;
                                k2Double = false;
                            }
                            else
                            {
                                if (k1Double)
                                {
                                    k1Double = false;
                                    k1 = 0;
                                }
                                else
                                {
                                    if (k2Double)
                                    {
                                        k2Double = false;
                                        k2 = 0;
                                    }
                                    else
                                    {
                                        k1 = 0;
                                        k2 = 0;
                                    }
                                }
                            }

                        }
                        else
                        {
                            if (finishdown - startdown == k1 + k1 + k2 || finishdown + (24 - startdown) == k1 + k1 + k2)
                            {
                                k1Double = false;
                                k2Double = false;
                                k1 = 0;
                            }
                            else // обе кости -- либо вывод фишек с использ не всех очков
                            {

                                if (doska.vyvod() & (finishdown == 24 || finishdown == 12))
                                {
                                    if (finishdown - startdown < k1 || finishdown + (24 - startdown) < k1)
                                        // использовали первую кость
                                    {
                                        if (k1Double)
                                        {
                                            k1Double = false;
                                        }
                                        else
                                        {
                                            if (k2Double)
                                            {
                                                k2Double = false;
                                            }
                                            else
                                            {
                                                k1 = 0;
                                            }

                                        }

                                    }
                                    else
                                    {
                                        if (finishdown - startdown < k2 || finishdown + (24 - startdown) < k2)
                                            // использовали вторую кость
                                        {
                                            if (k2Double)
                                            {
                                                k2Double = false;
                                            }
                                            else
                                            {
                                                k2 = 0;
                                            }

                                        }
                                        else
                                        {
                                            if (finishdown - startdown < 2*k1 || finishdown + (24 - startdown) < 2*k2)
                                            {
                                                if (k1Double & k2Double)
                                                {
                                                    k1Double = false;
                                                    k2Double = false;
                                                }
                                                else
                                                {
                                                    if (k1Double)
                                                    {
                                                        k1Double = false;
                                                        k1 = 0;
                                                    }
                                                    else
                                                    {
                                                        if (k2Double)
                                                        {
                                                            k2Double = false;
                                                            k2 = 0;
                                                        }
                                                        else
                                                        {
                                                            k1 = 0;
                                                            k2 = 0;
                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                if (finishdown - startdown < k1 + k1 + k2 ||
                                                    finishdown + (24 - startdown) < k1 + k1 + k2)
                                                {
                                                    k1Double = false;
                                                    k2Double = false;
                                                    k1 = 0;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    k1 = 0;
                                    k2 = 0;
                                }



                            }
                        }
                    }
                }


                //end 3- здесь мы затираем использованные очки на костях


                    if (doska.IsMoveExist(k1, k2, colorOfActivePlayer)) //  если ходы еще есть
                    {
                        Click = false;
                        FirstClick = false;
                        SecondClick = true;
                    }
                    else  // если ходов больше нету, то передача хода
                    {
                        TrancferOfMove();
                    }       


            }

            else       // хода не было. отмена выделения. (не было навигации на ячейку или ход сделан не по выпавшим очкам)
            {
                
                if (human.Active)
                {
                    human.zeroClick(startdown);  // ход отменен
                   
                }
                if (human2.Active)
                {
                    human2.zeroClick(startdown);  // ход отменен
                    
                }

               
                Click = false;
                FirstClick = false;
                SecondClick = true;
               
            }
            

        }
          
        if (!FirstClick && SecondClick && n!=-100 && n<24 && n>-1 && Click && doska.myTable[n,0]!=0)  // первый клик
          {
            
              if (human.Active && human.ItsMyFishka(n)&& human.Take(k1==k2,n))
              {
                  human.FirstClick((byte) k1, (byte) k2, n,k1Double,k2Double,firstmoveH1);
                  FirstClick = true;
                  SecondClick = false;
                  movedone = false;
              }
              else
              {
                if (human2.Active && human2.ItsMyFishka(n) && human2.Take(k1==k2,n))
                {
                    human2.FirstClick((byte) k1, (byte) k2, n,k1Double,k2Double,firstmoveH2);
                    FirstClick = true;
                    SecondClick = false;
                    movedone = false;
                }
                else
                {
                  Click = false;
                  FirstClick = false;
                  SecondClick = true;
                  movedone = false;
                }
              }
              
          }
          else
          {
              Click = false;
              FirstClick = false;
              SecondClick = true;
              movedone = false;
          }
      

     }


        public void TrancferOfMove() // передача хода
        {
            human.Trancfer();
            human2.Trancfer();

            FirstClick = false;
            SecondClick = true;
            Click = false;
            movedone = true;

            if (youFirstNet || youSecondNet)
            {
                isFirstNet = !isFirstNet;
                isSecondNet = !isSecondNet;
                setWhoMoved(getWhoMovedInt());
                if (isFirstNet && youFirstNet || isSecondNet && youSecondNet)
                {
                    doska.ShowBones("ваш ход. надо бросить кости");
                }
                if (isFirstNet && youSecondNet || isSecondNet && youFirstNet)
                {
                    doska.ShowBones("ход соперника.");
                }
                    

            }
            else
            {
                if (human.Active)
                {
                    doska.ShowBones("ход нижних. надо бросить кости");
                    colorOfActivePlayer = human.GetColor;
                }
                else
                {
                    doska.ShowBones("ход верхних. бросить кости");
                    colorOfActivePlayer = human2.GetColor;
                }
   
            }

            
            



    }

        public bool MoveDone
        {
            get { return movedone; }

        }

        public bool firstClick
        {
            get { return FirstClick; }
        }

        public bool  getWhoMoved() // в момент игры тру- первый  фалс- второй 
        {
            bool res;
            res = false;
                if (human.Active) 
                {
                    res= true;
                }
                if (human2.Active)
                {
                    res= false;
                }
            return res;
        }

        public int getWhoMovedInt() // тру- первый  фалс- второй
        {
            int res;
            res = 0;
            if (human.Active)
            {
                res = 1;
            }
            if (human2.Active)
            {
                res = 0;
            }
            return res;
        }


        public void setWhoMoved(int n)
        {

            if (human.Active)
            {
                s = " ход нижних ";
            }
            else
            {
                s = " ход верхних ";
            }


            if (human.Active)
            {
                colorOfActivePlayer = human.GetColor;
            }
            else
            {
                colorOfActivePlayer = human2.GetColor;
            }


            doska.ShowBones(k1, k2, s);

            if (youFirstNet || youSecondNet)
            {
                if (n == 3)
                {
                 doska.ShowBones("Ждем соперника.");
                }
                else
                {
                    
                    if (n == 0)
                    {
                        human.Active = false;
                        human2.Active = true;
                    }
                    else
                    {
                        human.Active = true;
                        human2.Active = false;
                    }
                    if ((youFirstNet == getWhoMoved() && isFirstNet) || (!getWhoMoved() == youSecondNet && isSecondNet))
                    {
                        s = "ваш ход";
                    }
                    else
                    {
                        if (youFirstNet || youSecondNet)
                        {
                            s = "ход соперника";
                        }

                    }


                    doska.ShowBones(k1, k2, s);
                    if (k1 == 0 & k2 == 0||s=="")
                    {
                        doska.ShowBones("ddffr");
                    }
                }

           }
        }


        public void GoFromLocal()
        {

            doska.ShowBones(k1, k2, s);

            if (k1 == k2)
            {
                k1Double = true;
                k2Double = true;
            }
            else
            {
                k1Double = false;
                k2Double = false;
            }

        }

        // test mode!
        public int countOfFishka(int i)
        {
            int count = 0;
            if (i == 1)
            {
                count= doska.getCountOfFishka(human.GetColor);
            }
            if (i == 2)
            {
                count= doska.getCountOfFishka(human2.GetColor); 
            }
            return count;
        }

        // test mode
    }
}
