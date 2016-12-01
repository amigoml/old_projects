using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace mysecond4sem
{
    class Human
    {
         protected Color color;            // цвет фишек
         public bool HeadEnabled;          // доступность ходов с головы.
         private bool HeadEnable2;         // второй раз с головы для случая дубля
         private bool headDouble;          // для случая с дублем
         private bool lowerPosition;       // true - снизу на доске  false- сверху
         public bool Active;               // есть ли сейчас право хода
         private Doska doska;

         private int StartPos;             // временные переписываемые данные о ходе( стартовая позиция)
         private int FinishPos;            // ( финишная позиция)





         public Human(Color color, bool LowerPosition, bool Active, Doska doska)
         {
         
            this.color = color;
            this.lowerPosition = LowerPosition;
            HeadEnabled = true;
            headDouble = false;
            this.Active = Active;
            this.doska = doska;
        }


        public bool ItsMyFishka(int n)       // узнать у "доски" моя ли это фишка  !!!!
        {
            return doska.askDesk(color, n);  // проверка равенства по цветам
        }


        public bool Take(bool headDouble, int StartPos)   // функция вроде бы для дубля дополнительные условия чтоб работало
        {
            if (this.headDouble)
            {
                headDouble = this.headDouble;
            }
            if (!headDouble && !this.headDouble)
            {
                if (StartPos == 0 && lowerPosition || StartPos == 12 && !lowerPosition)
                {
                  return HeadEnabled;  
                }
                else
                {
                   return true;
                }
                
            }

            bool result ;
            result = false;
            if (StartPos != 0 & StartPos != 12)
            {
                result = true;
            }
            else
            {
              if (headDouble & ( HeadEnable2 || HeadEnabled) && ((lowerPosition && StartPos == 0) || (!lowerPosition && StartPos == 12)))
              {
                result = HeadEnabled || HeadEnable2;
              } 
            }
            
            
            return result;
        }




        public void FirstClick(byte k1, byte k2, int StartPos, bool k1double, bool k2double, bool firstmove)  // 
        {
            if (k1 == k2 & firstmove)
            {
                headDouble = true;
            }
            
             this.StartPos = StartPos; 
             doska.AllocatedFishka(StartPos);                                     // подсветим фишку
             doska.ShowPossibleMoves(k1, k2, StartPos, k1double, k2double);      // сделать подсветку возможных ходов.


            if ((lowerPosition && StartPos == 0) || (!lowerPosition && StartPos == 12)) // дубль и голова
            {
                if (headDouble && !HeadEnabled)
                {
                    HeadEnable2 = false;
                }
                if (HeadEnabled)
                {
                    HeadEnabled = false;
                    if (headDouble)
                    {
                        HeadEnable2 = true;
                    }   
                }

             }
           
              
        
        }



        public void SecondClick(byte k1, byte k2, int FinishPos)  // ( очки на 1 и 2-ой костях, номер конечн позиции )
        {

                this.FinishPos = FinishPos;
                doska.Move(StartPos, FinishPos);
                doska.ReturnColor(FinishPos);      // уберем подсветку фишки
                doska.HidePossibleMoves();         // уберем подсветку доступн ходов 

        }

        public void zeroClick(int startdown)        // отмена клика
        {
           
            FinishPos = 0;
            doska.ReturnColor(StartPos);    // вернем исходный цвет фишке
            StartPos = 0;
            doska.HidePossibleMoves();      // уберем подсветку доступн ходов 
            
            if (startdown == 0 || startdown == 12)   // в случае дубля возвращаем флаги на шаг назад
            {
                if (HeadEnable2)
                {
                    HeadEnable2 = false;
                    HeadEnabled = true;
                }
                if (!HeadEnabled)
                {
                    HeadEnabled = true;
                }
            }
        }



        public Color GetColor
        {
            get { return color; }
        }

        public void Trancfer()
        {
            Active = !(Active);
            HeadEnabled = true;
            HeadEnable2 = false;
            headDouble = false;
        }

    }
}
