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
    public class Doska
    {
        public const int LineWidth = 1;                    // толщина линии для рисования
        public Fishka[] myFishka = new Fishka[30];        // массив моих шашек
        public int[,] myTable = new int[24, 16];         // массив Стеков со значениями уникальн номеров фишек. - это поле. [n,0] - поле с числом фишек в столбике!
      
        public PictureBox field;
        private Label label;


        private int countFishkaOfFirst = 15;
        private int countFishkaOfSecond = 15;

        private Color ColorOfFirstPlayer;   // цвет первого игрока
        private Color ColorOfSecondPlayer;  // цвет второго игрока

        private int[] possibleMoves = new int[5];  // для показа доступных ходов для выделенной ячейки
        private bool showPossibleMoves;            // вкл выкл этот режим


        public Doska( PictureBox pictureBox1 , Label lb ,Color cl1, Color cl2 )  // конструктор
        {
            this.field = pictureBox1;
            this.label = lb;
            ColorOfFirstPlayer = cl1;
            ColorOfSecondPlayer = cl2; 


            var bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // сглаживание 
            

            for (int i = 0; i < 30; i++)
            {
                if (i < 15)
                {
                    myFishka[i] = new Fishka(i);
                    myFishka[i] = new Fishka(cl1, 0, i);
                    myTable[0, i + 1] = i;
                    myTable[0, 0] += 1;

                }
                else
                {
                    myFishka[i] = new Fishka(cl2, 12, i - 14);
                    myTable[12, i - 14] = i;
                    myTable[12, 0] += 1;
                }
            }


            foreach (var fishka in myFishka)
            {
                fishka.Draw(g);
            }

            pictureBox1.Image = bmp;
            g.Dispose();
            
            
        } // end  constructor


        //!!!!!!!!test mode
        public Doska(PictureBox pictureBox1, Label lb, Color cl1, Color cl2, int k)  
        {
            this.field = pictureBox1;
            this.label = lb;
            ColorOfFirstPlayer = cl1;
            ColorOfSecondPlayer = cl2;


            var bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);
            


            for (int i = 0; i < 30; i++)
            {
                if (i < 15)  // положение 19
                {
                    myFishka[i] = new Fishka(i);
                    myFishka[i] = new Fishka(cl1, 19, i);
                    myTable[19, i + 1] = i;
                    myTable[19, 0] += 1;

                }
                else  // положение 6
                {
                    myFishka[i] = new Fishka(cl2, 6, i - 14);
                    myTable[6, i - 14] = i;
                    myTable[6, 0] += 1;
                }
            }


            foreach (var fishka in myFishka)
            {
                fishka.Draw(g);
            }

            pictureBox1.Image = bmp;
            g.Dispose();


        } 
        //!!! test mode

        public void Move( int start, int finish)  // Процедура проверки и вызова процедуры перемещения фишки. не более
        {
              if (start >= 0 && myTable[start, 0] != 0 && (finish >= 0 || finish==-70 ) )
              {
               move(start,finish);  
              }          
        }

        public void move( int start, int finish) // процедура хода. 
        {
            if (finish == 70 || finish == -70)   // вывод фишки
            {
                
                int num = myTable[start, myTable[start, 0]];
                myFishka[num].Hide();
                myTable[start, myTable[start, 0]] = 0;
                myTable[start, 0] -= 1;

                if (myFishka[num].GetColor == ColorOfFirstPlayer)
                {
                    countFishkaOfFirst--;
                    if (countFishkaOfFirst == 0)
                    {
                        MessageBox.Show("1");
                    }
                }
                else
                {
                    countFishkaOfSecond--;
                    if (countFishkaOfSecond == 0)
                    {
                        MessageBox.Show("2");
                    }
                }

            }
            else   // обычный ход
            {
                myTable[finish, 0] += 1;
                myTable[finish, myTable[finish, 0]] = myTable[start, myTable[start, 0]];
                myTable[start, myTable[start, 0]] = 0;
                myTable[start, 0] -= 1;
                int number= myTable[finish, myTable[finish, 0]];
                myFishka[number].move(finish, myTable[finish, 0]);  
            }

           
           
        }

        public void drawFishka(Graphics g)    // нарисовать фишку
        {
            for (int i = 0; i <= 23; i++)
            {
                for (int j = 1; j <= myTable[i, 0]; j++)
                {
                   myFishka[ myTable[i,j] ].Draw(g);      
                }
            }
            Color cl1 = Color.SandyBrown;
            Color cl2 = Color.MintCream;
            if (showPossibleMoves)
            {
                for (int i=0; i<5; i++)
                {
                    if (possibleMoves[i] != 100)
                    {
                        if (possibleMoves[i] == 70 || possibleMoves[i] == -70) // указатель для вывода фишки
                        {
                            g.FillEllipse(new SolidBrush(cl2), GetMeXCoord(possibleMoves[i]),
                                          GetMeYCoord(possibleMoves[i]), 6, 40);
                        }
                        else
                        {
                            g.FillEllipse(new SolidBrush(cl1), GetMeXCoord(possibleMoves[i]), GetMeYCoord(possibleMoves[i]), 5, 20); 
                            // зарисовка указателей для возм ходов
                        }
                     
                       
                    }
                    
                }
            }

        }

        public int GetMeXCoord(int n)
        {
            int x=0;

            if (n > 11 & n < 18)
            {
                n -= 11;
                x=490 - (n) * 38 + 16;
                n = 100;
            }
                
            if (n>5 & n<12)
            {
                x= (n) * 38 + 16 + 27;
                n = 100;
            }

            if ( n>17 & n<24)
            {
                n -= 11;
                x =  463-(n) * 38 + 16 ;
                n = 100;
            }
            if (n >= 0 & n < 6)
            {
                x =  (n) * 38 + 16;
                
            }

            if (n == -70)
            {
                x = 495;
            }
            if (n == 70)
            {
                x = 3;
            }

            return x;

        }


        public int GetMeYCoord(int n)
        {
            if (n > 11)
            {
                return 5;
            }
            else
            {
               return 335;
            }
        }



        public void ShowBones(int k1,int k2 , string s)    // показать очки на костях и чей ход
        {
            Control.CheckForIllegalCrossThreadCalls = false;
         
            label.Text = (k1.ToString() + "  " + k2.ToString() + " " + s);
         
        }

        public void ShowBones(string s)    // показать кто сейчас должен ходить
        {
            
            label.Text = (s); 
        }


        public void AllocatedFishka(int n)   // выделение фишки
        {
            // myTable[n,myTable[n,0 ] ]   (-> возвращает номер верхней фишки на позиции n
            if (myTable[n,0 ]!=0)
            {
              myFishka[myTable[n,myTable[n,0 ] ]].setAllocate();
            }
            
        }


        public void ReturnColor(int n)    //  возврат цвета для фишки.( отмена выделения )
        {
            if (n != 70 & n != -70)
            {
              myFishka[myTable[n,  myTable[n, 0]  ]].setNoAllocate();  
            }
            
        }



        public bool askDesk(Color cl, int n)  // проверка по цвету
        {
          return myFishka[myTable[n,  myTable[n, 0]  ]].CompairColors(cl);
        }




        public bool IfHodIsOk(int start, int finish, int k1, int k2) // проверка ходов по костям.
        {
            bool result=false;
            int fin1=0;
            int fin2=0;

            //0 для вывода
            if (finish == 70)
            {
                finish = 24;
                fin1 = 24;
            }
            if (finish == -70)
            {
                finish = 12;
                fin2 = 12;
            }
            //0

            if (start == finish) // если не ход.
            {
                return false;
            }
            // 1  проверка хода по костям
            for (int i = 0; i <= 4; i++)
            {
                if (finish == possibleMoves[i])
                {
                    result = true;
                }
            }
            // end 1

           

          //3 проверка на возможность вывода фишки
            if (IfAllInHomePositions(myFishka[myTable[start, myTable[start, 0]]].GetColor))
            {
                if (myFishka[myTable[start, myTable[start, 0]]].GetColor == ColorOfSecondPlayer) // верхние
                {

                    if (fin2 == 12)
                    {
                        bool res2;
                        res2 = false;
                        if ( (start + k1 == 12 || start + k2 == 12 || start + k1 + k2 == 12) ||
                            (k1==k2 && (start+3*k1==12 || start+4*k1==12) ) )
                        {
                            res2 = true;
                            result = true;
                        }
                        if (!res2) // не получилось четко использовать очки 
                        {
                            res2 = true;
                            for (int i = 6; i < start; i++) // смотрим нет ли сзади наших фишек
                            {
                                if (myTable[i, 0] != 0 &&
                                    myFishka[myTable[i, myTable[i, 0]]].GetColor == ColorOfSecondPlayer)
                                {
                                    res2 = false; // незя выводить. позади есть фишки
                                }
                            }
                            if (res2) // неполное использование фишек. но можно выводить
                            {
                                result = true;
                            }

                        }


                    }
                }

                if (myFishka[myTable[start, myTable[start, 0]]].GetColor == ColorOfFirstPlayer) // нижние
                {
                    if (fin1 == 24)
                    {
                        bool res1;
                        res1 = false;
                        if ( (start + k1 == 24 || start + k2 == 24 || start + k1 + k2 == 24)||
                             (k1 == k2 && (start + 3 * k1 == 24 || start + 4 * k1 == 24)) )
                        {
                            res1 = true;
                            result = true;
                        }
                        if (!res1)   // не получилось четко использовать очки 
                        {
                            res1 = true;
                            for (int i = 18; i < start; i++) // смотрим нет ли сзади наших фишек
                            {
                                if (myTable[i, 0] != 0 && myFishka[myTable[i, myTable[i, 0]]].GetColor == ColorOfFirstPlayer)
                                {
                                    res1 = false;  // незя выводить. позади есть фишки
                                }
                            }
                            if (res1) // неполное использование фишек. но можно выводить
                            {
                                result = true;
                            }
                        }

                    }


                }
            }

           //3

         
            return result;
        }


        public void HidePossibleMoves()
        {
            showPossibleMoves = false;
        }


        public void ShowPossibleMoves(int k1, int k2, int startPos, bool k1double, bool k2double)
            // заполняет массив позициями на которые может пойти фишка
        {
            // без учета проверки хода
            showPossibleMoves = true;
            for (int i = 0; i <= 4; i++)
            {
                possibleMoves[i] = 100;
            }

            if (IfAllInHomePositions(myFishka[myTable[startPos, myTable[startPos, 0]]].GetColor)) // выводим фишки
            {
                //1 можно ли сразу вывести фишки
                if (startPos + k1 == 24)
                {
                    possibleMoves[4] = 70;
                }
                if (startPos + k1 == 12)
                {
                    possibleMoves[4] = -70;
                }

                if (startPos + k2 == 24)
                {
                    possibleMoves[4] = 70;
                }
                if (startPos + k2 == 12)
                {
                    possibleMoves[4] = -70;
                }

                if (startPos + k1 + k2 == 24)
                {
                    possibleMoves[4] = 70;
                }
                if (startPos + k1 + k2 == 12)
                {
                    possibleMoves[4] = -70;
                }
                if (k1 == k2)
                {
                    if (startPos + 3*k1 == 12 || startPos + 4*k1 == 12)
                    {
                        possibleMoves[4] = -70;
                    }
                    if (startPos + 3*k1 == 24 || startPos + 4*k1 == 24)
                    {
                        possibleMoves[4] = 70;
                    }
                }
                // 1 end


                // все фишки возле дома. но очки выпали не те.
                if (possibleMoves[4] == 100)
                {

                    //9 позади фишек нету-> можно выводить.
                    if (startPos < 12)
                    {
                        bool res2;
                        res2 = true;
                        for (int i = 6; i < startPos; i++) // смотрим нет ли сзади наших фишек
                        {
                            if (myTable[i, 0] != 0 &&
                                myFishka[myTable[i, myTable[i, 0]]].GetColor == ColorOfSecondPlayer)
                            {
                                res2 = false; // незя выводить. позади есть фишки
                            }
                        }

                        if (res2)
                        {

                            if (startPos + k1 > 12)
                            {
                                possibleMoves[4] = -70;
                            }


                            if (startPos + k2 > 12)
                            {
                                possibleMoves[4] = -70;
                            }

                            if (startPos + k1 + k2 > 12)
                            {
                                possibleMoves[4] = -70;
                            }

                            if (k1 == k2)
                            {
                                if (startPos + 3*k1 > 12 || startPos + 4*k1 > 12)
                                {
                                    possibleMoves[4] = -70;
                                }

                            }

                        }

                    }
                    if (startPos > 17)
                    {
                        bool res2;
                        res2 = true;
                        for (int i = 18; i < startPos; i++) // смотрим нет ли сзади наших фишек
                        {
                            if (myTable[i, 0] != 0 &&
                                myFishka[myTable[i, myTable[i, 0]]].GetColor == ColorOfFirstPlayer)
                            {
                                res2 = false; // незя выводить. позади есть фишки
                            }
                        }

                        if (res2)
                        {

                            if (startPos + k1 > 24)
                            {
                                possibleMoves[4] = 70;
                            }


                            if (startPos + k2 > 24)
                            {
                                possibleMoves[4] = 70;
                            }

                            if (startPos + k1 + k2 > 24)
                            {
                                possibleMoves[4] = 70;
                            }
                            if (k1 == k2)
                            {
                                if (startPos + 3*k1 > 23 || startPos + 4*k1 > 23)
                                {
                                    possibleMoves[4] = 70;
                                }
                            }

                        }

                    }
                    //9 end. вывод фишек


                    // все фишки возле дома. но позади есть Фишки- вывод не был позможен
                    if (possibleMoves[4] == 100)
                    {
                        //
                        if (startPos + k1 > 23)
                        {
                            possibleMoves[0] = startPos + k1 - 24;
                        }
                        else
                        {
                            possibleMoves[0] = startPos + k1;
                        }
                        //
                        if (k1 != k2)
                        {
                            if (startPos + k2 > 23)
                            {
                                possibleMoves[1] = startPos + k2 - 24;
                            }
                            else
                            {
                                possibleMoves[1] = startPos + k2;
                            }
                        }
                        else
                        {
                            if (startPos + k1 + k2 > 23)
                            {
                                possibleMoves[1] = startPos + k1 + k2 - 24;
                            }
                            else
                            {
                                possibleMoves[1] = startPos + k1 + k2;
                            }
                        }

                        //
                        if (k1 != k2)
                        {
                            if (startPos + k1 + k2 > 23)
                            {
                                possibleMoves[2] = startPos + k1 + k2 - 24;
                            }
                            else
                            {
                                possibleMoves[2] = startPos + k1 + k2;

                            }
                        }
                        else
                        {
                            if (startPos + k1 + k2 + k1 > 23)
                            {
                                if (k1double || k2double)
                                {
                                    possibleMoves[2] = startPos + k1 + k2 + k1 - 24;
                                }

                            }
                            else
                            {
                                if (k1double || k2double)
                                {
                                    possibleMoves[2] = startPos + k1 + k2 + k1;
                                }

                            }
                        }
                        if (k1 == k2)
                        {
                            if (k1double && k2double)
                            {
                                if (startPos + 2*k1 + 2*k2 > 23)
                                {
                                    possibleMoves[3] = startPos + 2*k1 + 2*k2 - 24;
                                }
                                else
                                {
                                    possibleMoves[3] = startPos + 2*k1 + 2*k2;
                                }
                            }

                        }

                        //
                        if (k1 == 0)
                        {
                            possibleMoves[0] = 100;
                        }
                        if (k2 == 0)
                        {
                            possibleMoves[1] = 100;
                        }
                        //1 убираем подсветку занятых противником позиций
                        for (int i = 0; i <= 3; i++)
                        {
                            if (possibleMoves[i] != 100 && myTable[possibleMoves[i], 0] != 0 &&
                                myFishka[myTable[startPos, myTable[startPos, 0]]].GetColor !=
                                myFishka[myTable[possibleMoves[i], myTable[possibleMoves[i], 0]]].GetColor)
                            {
                                possibleMoves[i] = 100;
                            }
                        }
                        //1  
                        //2 убрать ход который поведет на второй круг по доске
                        for (int i = 0; i <= 3; i++)
                        {
                            if ((myFishka[myTable[startPos, myTable[startPos, 0]]].GetColor == ColorOfSecondPlayer &&
                                 possibleMoves[i] >= 12 && startPos <= 11 & startPos >= 0) //  верхние
                                ||
                                (myFishka[myTable[startPos, myTable[startPos, 0]]].GetColor == ColorOfFirstPlayer &&
                                 possibleMoves[i] <= 11 && startPos >= 12 & startPos <= 23)) //  нижние
                            {
                                possibleMoves[i] = 100;
                            }
                        }
                        //2

                    }
                    else
                    {

                        // 12 простой ход фишкой

                        if (startPos + k1 > 23)
                        {
                            possibleMoves[0] = startPos + k1 - 24;
                        }
                        else
                        {
                            possibleMoves[0] = startPos + k1;
                        }
                        //
                        if (k1 != k2)
                        {
                            if (startPos + k2 > 23)
                            {
                                possibleMoves[1] = startPos + k2 - 24;
                            }
                            else
                            {
                                possibleMoves[1] = startPos + k2;
                            }
                        }
                        else
                        {
                            if (startPos + k1 + k2 > 23)
                            {
                                possibleMoves[1] = startPos + k1 + k2 - 24;
                            }
                            else
                            {
                                possibleMoves[1] = startPos + k1 + k2;
                            }
                        }

                        //
                        if (k1 != k2)
                        {
                            if (startPos + k1 + k2 > 23)
                            {
                                possibleMoves[2] = startPos + k1 + k2 - 24;
                            }
                            else
                            {
                                possibleMoves[2] = startPos + k1 + k2;
                            }
                        }
                        else
                        {
                            if (startPos + k1 + k2 + k1 > 23)
                            {
                                if (k1double || k2double)
                                {
                                    possibleMoves[2] = startPos + k1 + k2 + k1 - 24;
                                }

                            }
                            else
                            {
                                if (k1double || k2double)
                                {
                                    possibleMoves[2] = startPos + k1 + k2 + k1;
                                }

                            }
                        }
                        if (k1 == k2)
                        {
                            if (k1double && k2double)
                            {
                                if (startPos + 2*k1 + 2*k2 > 23)
                                {
                                    possibleMoves[3] = startPos + 2*k1 + 2*k2 - 24;
                                }
                                else
                                {
                                    possibleMoves[3] = startPos + 2*k1 + 2*k2;
                                }
                            }

                        }

                        //
                        if (k1 == 0)
                        {
                            possibleMoves[0] = 100;
                        }
                        if (k2 == 0)
                        {
                            possibleMoves[1] = 100;
                        }
                        //1 убираем подсветку занятых противником позиций
                        for (int i = 0; i <= 3; i++)
                        {
                            if (possibleMoves[i] != 100 && myTable[possibleMoves[i], 0] != 0 &&
                                myFishka[myTable[startPos, myTable[startPos, 0]]].GetColor !=
                                myFishka[myTable[possibleMoves[i], myTable[possibleMoves[i], 0]]].GetColor)
                            {
                                possibleMoves[i] = 100;
                            }
                        }
                        //1  
                        //2 убрать ход который поведет на второй круг по доске
                        for (int i = 0; i <= 3; i++)
                        {
                            if ((myFishka[myTable[startPos, myTable[startPos, 0]]].GetColor == ColorOfSecondPlayer &&
                                 possibleMoves[i] >= 12 && startPos <= 11 & startPos >= 0) //  верхние
                                ||
                                (myFishka[myTable[startPos, myTable[startPos, 0]]].GetColor == ColorOfFirstPlayer &&
                                 possibleMoves[i] <= 11 && startPos >= 12 & startPos <= 23)) //  нижние
                            {
                                possibleMoves[i] = 100;
                            }
                        }
                        //12 end. простой ход
                    }
                }


                else // не выводим фишки. обычное перемещение.
                {

                    //
                    if (startPos + k1 > 23)
                    {
                        possibleMoves[0] = startPos + k1 - 24;
                    }
                    else
                    {
                        possibleMoves[0] = startPos + k1;
                    }
                    //
                    if (k1 != k2)
                    {
                        if (startPos + k2 > 23)
                        {
                            possibleMoves[1] = startPos + k2 - 24;
                        }
                        else
                        {
                            possibleMoves[1] = startPos + k2;
                        }
                    }
                    else
                    {
                        if (startPos + k1 + k2 > 23)
                        {
                            possibleMoves[1] = startPos + k1 + k2 - 24;
                        }
                        else
                        {
                            possibleMoves[1] = startPos + k1 + k2;
                        }
                    }

                    //
                    if (k1 != k2)
                    {
                        if (startPos + k1 + k2 > 23)
                        {
                            possibleMoves[2] = startPos + k1 + k2 - 24;
                        }
                        else
                        {
                            possibleMoves[2] = startPos + k1 + k2;

                        }
                    }
                    else
                    {
                        if (startPos + k1 + k2 + k1 > 23)
                        {
                            if (k1double || k2double)
                            {
                                possibleMoves[2] = startPos + k1 + k2 + k1 - 24;
                            }

                        }
                        else
                        {
                            if (k1double || k2double)
                            {
                                possibleMoves[2] = startPos + k1 + k2 + k1;
                            }

                        }
                    }
                    if (k1 == k2)
                    {
                        if (k1double && k2double)
                        {
                            if (startPos + 2*k1 + 2*k2 > 23)
                            {
                                possibleMoves[3] = startPos + 2*k1 + 2*k2 - 24;
                            }
                            else
                            {
                                possibleMoves[3] = startPos + 2*k1 + 2*k2;
                            }
                        }

                    }

                    //
                    if (k1 == 0)
                    {
                        possibleMoves[0] = 100;
                    }
                    if (k2 == 0)
                    {
                        possibleMoves[1] = 100;
                    }


                    //1 убираем подсветку занятых противником позиций
                    for (int i = 0; i <= 3; i++)
                    {
                        if (possibleMoves[i] != 100 && myTable[possibleMoves[i], 0] != 0 &&
                            myFishka[myTable[startPos, myTable[startPos, 0]]].GetColor !=
                            myFishka[myTable[possibleMoves[i], myTable[possibleMoves[i], 0]]].GetColor)
                        {
                            possibleMoves[i] = 100;
                        }
                    }
                    //1   

                    // ColorOfFirstPlayer light green

                    //2 убрать ход который поведет на второй круг по доске
                    for (int i = 0; i <= 3; i++)
                    {
                        if ((myFishka[myTable[startPos, myTable[startPos, 0]]].GetColor == ColorOfSecondPlayer &&
                             possibleMoves[i] >= 12 && startPos <= 11 & startPos >= 0) //  верхние
                            ||
                            (myFishka[myTable[startPos, myTable[startPos, 0]]].GetColor == ColorOfFirstPlayer &&
                             possibleMoves[i] <= 11 && startPos >= 12 & startPos <= 23)) //  нижние
                        {
                            possibleMoves[i] = 100;
                        }
                    }
                    //2
                }
            }
            else
            {
                 //
                    if (startPos + k1 > 23)
                    {
                        possibleMoves[0] = startPos + k1 - 24;
                    }
                    else
                    {
                        possibleMoves[0] = startPos + k1;
                    }
                    //
                    if (k1 != k2)
                    {
                        if (startPos + k2 > 23)
                        {
                            possibleMoves[1] = startPos + k2 - 24;
                        }
                        else
                        {
                            possibleMoves[1] = startPos + k2;
                        }
                    }
                    else
                    {
                        if (startPos + k1 + k2 > 23)
                        {
                            possibleMoves[1] = startPos + k1 + k2 - 24;
                        }
                        else
                        {
                            possibleMoves[1] = startPos + k1 + k2;
                        }
                    }

                    //
                    if (k1 != k2)
                    {
                        if (startPos + k1 + k2 > 23)
                        {
                            possibleMoves[2] = startPos + k1 + k2 - 24;
                        }
                        else
                        {
                            possibleMoves[2] = startPos + k1 + k2;

                        }
                    }
                    else
                    {
                        if (startPos + k1 + k2 + k1 > 23)
                        {
                            if (k1double || k2double)
                            {
                                possibleMoves[2] = startPos + k1 + k2 + k1 - 24;
                            }

                        }
                        else
                        {
                            if (k1double || k2double)
                            {
                                possibleMoves[2] = startPos + k1 + k2 + k1;
                            }

                        }
                    }
                    if (k1 == k2)
                    {
                        if (k1double && k2double)
                        {
                            if (startPos + 2*k1 + 2*k2 > 23)
                            {
                                possibleMoves[3] = startPos + 2*k1 + 2*k2 - 24;
                            }
                            else
                            {
                                possibleMoves[3] = startPos + 2*k1 + 2*k2;
                            }
                        }

                    }

                    //
                    if (k1 == 0)
                    {
                        possibleMoves[0] = 100;
                    }
                    if (k2 == 0)
                    {
                        possibleMoves[1] = 100;
                    }


                    //1 убираем подсветку занятых противником позиций
                    for (int i = 0; i <= 3; i++)
                    {
                        if (possibleMoves[i] != 100 && myTable[possibleMoves[i], 0] != 0 &&
                            myFishka[myTable[startPos, myTable[startPos, 0]]].GetColor !=
                            myFishka[myTable[possibleMoves[i], myTable[possibleMoves[i], 0]]].GetColor)
                        {
                            possibleMoves[i] = 100;
                        }
                    }
                    //1   

                    // ColorOfFirstPlayer light green

                    //2 убрать ход который поведет на второй круг по доске
                    for (int i = 0; i <= 3; i++)
                    {
                        if ((myFishka[myTable[startPos, myTable[startPos, 0]]].GetColor == ColorOfSecondPlayer &&
                             possibleMoves[i] >= 12 && startPos <= 11 & startPos >= 0) //  верхние
                            ||
                            (myFishka[myTable[startPos, myTable[startPos, 0]]].GetColor == ColorOfFirstPlayer &&
                             possibleMoves[i] <= 11 && startPos >= 12 & startPos <= 23)) //  нижние
                        {
                            possibleMoves[i] = 100;
                        }
                    }
                    //2
                }
            


        }

       public bool IsMoveExist( int k1, int k2, Color color)  // проверяет есть ли доступные ходы для данных очков для игрока с цветом color
        {
            bool exist = false;
           if (k1 == 0 && k2 == 0)
           {
               return false;
           }

                for (int i = 0; i < 24; i++)
                {
                    if (myTable[i, 0] != 0)
                    {
                        int nomer = myTable[i, myTable[i, 0]];

                        if (myFishka[nomer].GetColor == color)
                        {
                            int delt;
                            if (k1 != 0)
                            {
                                if (i + k1 < 23)
                                {
                                    if (IfAllInHomePositions(myFishka[nomer].GetColor))
                                    {
                                        if (i + k1>12 && i<12)
                                        {
                                            delt = -70;
                                        }
                                        else
                                        {
                                            delt = i + k1;
                                        }
                                    }
                                    else
                                    {
                                      delt = i + k1;  
                                    }
                                    
                                }
                                else
                                {
                                    if(IfAllInHomePositions(myFishka[nomer].GetColor)) 
                                    {
                                        if (i + k1 > 23)
                                        {
                                           delt = 70;
                                        }
                                        else
                                        {
                                            delt = 24 - i + k1;
                                        }
                                       
                                    }
                                    else
                                    {
                                      delt = 24 - i + k1;  
                                    }
                                    
                                }

                                if (IfHodIsOk(i, delt, k1, k2))
                                {
                                    exist = true;
                                }
                            }
                            if (k2 != 0)
                            {
                                if (i + k2 < 23)
                                {
                                    if (IfAllInHomePositions(myFishka[nomer].GetColor))
                                    {
                                        if (i + k2 > 12 && i<12)
                                        {
                                            delt = -70;
                                        }
                                        else
                                        {
                                            delt = i + k2;  
                                        }
                                        
                                    }
                                    else
                                    {
                                       delt = i + k2; 
                                    }
                                    
                                }
                                else
                                {
                                    if (IfAllInHomePositions(myFishka[nomer].GetColor))
                                    {
                                        if (i + k2 > 23)
                                        {
                                            delt = 70;
                                        }
                                        else
                                        {
                                            delt = i + k2;  
                                        }
                                        
                                    }
                                    else
                                    {
                                      delt = 24 - i + k2;  
                                    }
                                    
                                }
                                if (IfHodIsOk(i, delt, k1, k2))
                                {
                                    exist = true;
                                }
                            }
                            if (k1 + k2 != 0)
                            {
                                if (i + k1 + k2 < 23)
                                {
                                    if (IfAllInHomePositions(myFishka[nomer].GetColor))
                                    {
                                        if (i + k1 + k2 > 12 && i<12)
                                        {
                                            delt = -70;
                                        }
                                        else
                                        {
                                            delt = i + k1 + k2;
                                        }
                                       
                                    }
                                    else
                                    {
                                       delt = i + k1 + k2;
                                    }
                                    
                                }
                                else
                                {
                                    if (IfAllInHomePositions(myFishka[nomer].GetColor))
                                    {
                                        if (i + k1 + k2 > 23)
                                        {
                                            delt = 70;
                                        }
                                        else
                                        {
                                            delt = 24 - i + k1 + k2; 
                                        }
                                        
                                    }
                                    else
                                    {
                                        delt = 24 - i + k1 + k2;
                                    }
                                    
                                }
                                if (IfHodIsOk(i, delt, k1, k2))
                                {
                                    exist = true;
                                }
                            }

                        }

                    }
                }
                
                return exist;
        }


       public bool IfAllInHomePositions(Color color)  // смотрим не стоят ли фишки возле дома
       {
            bool result = true;

  // нижние  -> дом 18..23
          if (color==ColorOfFirstPlayer) 
            {
                 for (int i = 0; i <= 17; i++)
                 {
                     if (myTable[i, 0] != 0)
                     {
                        if (myFishka[myTable[i, myTable[i, 0]]].GetColor == color)
                        {
                            result = false;
                        }
                     }
                 } 
            }

   // верхние (дом 6..11)
            if (color == ColorOfSecondPlayer) 
            {
                for (int i = 0; i <= 5; i++)
                {
                    if (myTable[i, 0] != 0)
                    {
                        if (myFishka[myTable[i, myTable[i, 0]]].GetColor == color)
                        {
                            result = false;
                        }
                    }
                }
                for (int i = 12; i <= 23; i++)
                {
                    if (myTable[i, 0] != 0)
                    {
                        if (myFishka[myTable[i, myTable[i, 0]]].GetColor == color)
                        {
                            result = false;
                        }
                    }
                } 
            }
        return result;

        }

       
        public bool vyvod() // узнаем есть ли возможность вывода фишки.
        {
            bool result;
            result = false;

            if (possibleMoves[4] == 70 || possibleMoves[4] == -70)
            {
                result = true;
            }

            return result;
        }



        public int getCountOfFishka(Color cl)
        {
            int count = 0;

            for (int i = 0; i < 24; i++)
            {
                if (myTable[i, 0] != 0)
                {
                    if (myFishka[myTable[i, myTable[i, 0]]].GetColor == cl)
                    {
                        count += myTable[i, 0];
                    }

                }
            }

            return count;

        }


      
  
    }


}