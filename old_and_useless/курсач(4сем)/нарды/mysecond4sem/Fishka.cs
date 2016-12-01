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
    
    public class Fishka
    {
        private bool isExsist;
        private Color color;  // цвет  фишки от игрока 
        private Color AllocateColor = Color.Gold; // выделение цвет
        private Color WorkColor ;  //  с каким буду рисовать.

        public int uniqnumber;   //  0..29 уникальный номер  

        public int positionX;     //  0..24 позиция на доске
        public int positionY;

        const int Radius = 38;

        

        public Fishka(int uniqnum)
        {
            uniqnumber = uniqnum;

        }
        public Fishka(Color col, int posX, int posY)
        {
            color = col;
            WorkColor = color;
            positionX = posX;
            positionY = posY;
            isExsist = true;
        }

        public void move(int posX, int posY )   
        {
           
            positionX = posX;  
            positionY = posY;   
              
        }

        public void setAllocate()
        {
          WorkColor= AllocateColor;
        }

        public void setNoAllocate()
        {
            WorkColor = color;
        }


        public void Draw(Graphics field)
        {
            if (isExsist)
            {
                var myBrush = new SolidBrush(WorkColor);
                var myPen = new Pen(Color.Black, 2);

                Point p;


                if (positionX > 11)
                {
                    if (positionX > 17)
                    {
                        p = new Point(463 - (positionX - 11) * Radius, positionY * 12 - 8);
                    }
                    else
                    {

                        p = new Point(490 - (positionX - 11) * Radius, positionY * 12 - 8);
                    }

                }
                else
                {
                    if (positionX > 5)
                    {
                        p = new Point((positionX) * Radius + 34, 350 - positionY * 12);
                    }
                    else
                    {
                        p = new Point((positionX) * Radius + 7, 350 - positionY * 12);
                    }

                }


                field.FillEllipse(myBrush, p.X, p.Y, Radius, Radius);
                field.DrawEllipse(myPen, p.X, p.Y, Radius, Radius);
                myPen.Dispose();
                myBrush.Dispose();
            }
           

        }


        public bool CompairColors(Color cl1)
        {
            if (color == cl1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public Color GetColor
        {
            get { return color; }
        }

        public void Hide()
        {
            isExsist = false;
        }

    }
   

}
