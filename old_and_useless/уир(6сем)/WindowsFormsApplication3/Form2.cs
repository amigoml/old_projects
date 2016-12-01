using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Form2 : Form
    {
        private ArrayList[] myPoints;
        private Controller controller;
       // private MyObject myObject; // для получения информации о точке

        
        private bool ClastersVisible = true; // режим показа точек кластеров

        private bool forel = false; // режим фореля!
        // полигон
        List<Point> pointsOfPolygon = new List<Point>(); // точки моего полигона
        private Point lastPoint;
        private bool isMoovingMouse = false;
        // конец полигон

        private ArrayList params1;
        private ArrayList params2;
        private Graphics graph;
        private Bitmap bitmap;
        private int width;
        private int height;
        private string name1;
        private string name2;
        private int numRow;
        private int numCol;
        public String[] allAxes;
        


        const int radius = 8;
        const int gridstep = 5; // количество линий (горизонтальных или вертикальных)
        private const int countOfIntervals = 5; // число интервалов в гистограмме, при возможности 


        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
         
        public void SetController(Controller controller)
        {
            this.controller = controller;
        }

        public  PictureBox getMePB()
        {
            return pictureBox1;
        }

        public void SetPicture(Bitmap btmp)
        {
            pictureBox1.Image = btmp;
        }

        public void TakePoints(ArrayList[] Points)
        {
           myPoints = Points;
        }

        public void RunForel(int numOfClaster, int firstElement, Claster myClaster, double radius)
        {
            double[] radInProection = myClaster.GetDiametrInProection(numRow, numCol);  // две удаленные точки и диаметр .. всего 3 элемента в массиве
            List<ArrayList> listOfPoints = controller.GetListOfClasterPoints();
            ArrayList arrayList = listOfPoints[numOfClaster];

            //две выделенные точки
            int point1 = int.Parse(arrayList[(int) radInProection[0]].ToString());
            int point2 = int.Parse(arrayList[(int)radInProection[1]].ToString());
            // 


            ////////////////////////////////////////////////////////// painting
            int count = params1.Count;


            double max1 = double.Parse(params1[0].ToString());
            double min1 = double.Parse(params1[0].ToString());

            double max2 = double.Parse(params2[0].ToString());
            double min2 = double.Parse(params2[0].ToString());

            for (int i = 0; i <= count - 1; i++)   // ищем максимумы и минимумы 
            {
                if (double.Parse(params1[i].ToString()) > max1)
                {
                    max1 = double.Parse(params1[i].ToString());
                }
                if (double.Parse(params1[i].ToString()) < min1)
                {
                    min1 = double.Parse(params1[i].ToString());
                }

                if (double.Parse(params2[i].ToString()) > max2)
                {
                    max2 = double.Parse(params2[i].ToString());
                }
                if (double.Parse(params2[i].ToString()) < min2)
                {
                    min2 = double.Parse(params2[i].ToString());
                }
            }

            var step1 = ((max1 - min1) / (width * 0.7));
            var step1ForRad = ( (width * 0.7) * (double.Parse(params1[point1].ToString()) - min1) / (max1 - min1) );
            var step2 = ((max2 - min2) / (height * 0.7));
            var step2ForRad = ( (height * 0.7) * (double.Parse(params2[point2].ToString()) - min2) / (max2 - min2) );

            //////////////////////////////////////////////////////////////// painting

            List<MyObject> objects = myClaster.GetObjects();

            int countObj = objects.Count;
            int countAtr = objects[0].attrib.Count;

            double[,] vectX = new double[countObj, countAtr];

            for (int i = 0; i < countObj; i++)
            {
                for (int j = 0; j < countAtr; j++)
                {
                    vectX[i, j] = double.Parse(objects[i].attrib[j].ToString());
                }
            }

            double[] xCenter = new double[countAtr];  //  заполняем начальный центр масс.

            for (int j = 0; j < countAtr; j++)
            {
                xCenter[j] = vectX[firstElement, j];
            }

            Boolean flagForCycle = true;

            ArrayList ObjInSphere = new ArrayList();

            for (int i = 0; i < countObj; i++)
            {
                ObjInSphere.Add(i);
            }

            int m = 0; //  tmp counter
            ///////////////////.............

            while (flagForCycle)
            {
                int num = ObjInSphere.Count;
                int[] numOfObjects = new int[num];

                ObjInSphere.CopyTo(numOfObjects);

                ObjInSphere.Clear();

                double[] ArrayX = new double[countAtr];  // какой то объект
                int myCountObjectsInSphere = 0;

                for (int i = 0; i < num; i++)
                {

                    for (int j = 0; j < countAtr; j++)
                    {
                        ArrayX[j] = vectX[i, j];  ////////////////////
                    }

                    double length = myClaster.GetRo(ArrayX, xCenter);

                    if (length <= radius/2)
                    {
                        myCountObjectsInSphere++;
                        ObjInSphere.Add(i);
                    }
                }

                double[] xNewCenter = new double[countAtr];  // новый центр масс.

                foreach (var nums in ObjInSphere)
                {
                    m = Int32.Parse(nums.ToString());
                    for (int j = 0; j < countAtr; j++)
                    {
                        xNewCenter[j] += vectX[m, j] / ObjInSphere.Count;
                    }
                }

                graph.Clear(Color.White);
                RedrawMe();
                graph.FillEllipse(Brushes.LightPink, (float)(width * 0.05 + (xNewCenter[numRow] - min1) / step1), (float)(height * 0.9 - (xNewCenter[numCol] - min2) / step2), 9, 9);

                graph.DrawEllipse(Pens.LightSkyBlue, (float)(width * 0.05 - step1ForRad / 2 + (xNewCenter[numRow] - min1) / step1), (float)(height * 0.9 - step2ForRad/2 - (xNewCenter[numCol] - min2) / step2), (float)(step1ForRad), (float)(step2ForRad));

                
                // отмечаем новый цм
                // рисуем круг радиусом соответствующим

                bool flag = false;
                for (int i = 0; i < countAtr; i++)
                {
                    if (xNewCenter[i] != xCenter[i])
                    {
                        flag = true;
                    }
                }
                flagForCycle = flag;

                if (flagForCycle) xCenter = xNewCenter;
            }

            string res = null;
            for (int i = 0; i < ObjInSphere.Count; i++)
            {
                res += ObjInSphere[i].ToString() + "\n";
            }
            MessageBox.Show(res);

        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
           
            //MessageBox.Show(e.X.ToString() + ' ' + e.Y.ToString());

            for (int i = 0; i < myPoints[0].Count; i++)
            {
                // проверка на выделенность точки
                int num = controller.IsClasterOrSelected(i);

                if (num == -1)
                {
                    // выделенная все ок
                }
                else
                {
                    if (num >= 0) // если кластер то не выделяем
                    {
                        if (forel)
                        {
                            if (
                                Math.Sqrt((e.X - double.Parse(myPoints[0][i].ToString()))*
                                          (e.X - double.Parse(myPoints[0][i].ToString())) +
                                          (e.Y - double.Parse(myPoints[1][i].ToString()))*
                                          (e.Y - double.Parse(myPoints[1][i].ToString()))) <= radius)
                            {

                                if (forel)
                                {
                                    RunForel( num, controller.GetNumInClaster(i), controller.GetClasterForForel(i), controller.GetClasterForForel(i).GetDiametr());
                                }
                            }
                        }
                        continue;
                    }
                }

                if (Math.Sqrt((e.X - double.Parse(myPoints[0][i].ToString())) * (e.X - double.Parse(myPoints[0][i].ToString())) + (e.Y - double.Parse(myPoints[1][i].ToString())) * (e.Y - double.Parse(myPoints[1][i].ToString()))) <=  radius)
                {
                    //MessageBox.Show(myPoints[0][i].ToString() + ' ' + myPoints[1][i]);
                    if (e.Button == MouseButtons.Right)
                    {
                        MyObject obj = controller.GiveObject(i);
                        string exitString = "";
                        for (int j = 0; j < obj.attrib.Count; j++)
                        {
                            exitString += allAxes[j] + "  : " + obj.attrib[j].ToString() + "\n";
                        }
                        MessageBox.Show(exitString);
                    }
                    else
                    {
                        if (!forel)
                        {
                            controller.AddSelectedPoint(i);
                            RedrawMe();
                        }
                    }
                }
            }

        } // выделение точки одиночным нажатием

        public void getParamsForRefresh(ArrayList params1, ArrayList params2, int width, int height, string name1, string name2,int numRow, int numCol)
        {
            this.params1 = params1;
            this.params2 = params2;
            this.width = width;
            if (name1 == name2)
            {
                this.height = (int) (height*0.95);
            }
            else
            {
             this.height = height ;   
            }
            
            this.name1 = name1;
            this.name2 = name2;
            this.numRow = numRow;
            this.numCol = numCol;
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graph = Graphics.FromImage(bitmap);
            graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            
        }

        public void RedrawMe()
        {
           
            // graph.Clear(Color.White);

            Brush usualPen = Brushes.SeaGreen; 
            Brush selectedPen = Brushes.Red; // выделенная точка
            Brush overlapPen = Brushes.Blue; //совпадающие точки
            Brush justPen = usualPen;
            Brush[] SelectedPensForClaster = new Brush[5];       // количнство кластеров и цвет соответсвенно
            ////
            SelectedPensForClaster[0] = Brushes.Gold;
            SelectedPensForClaster[1] = Brushes.Maroon;
            SelectedPensForClaster[2] = Brushes.LightSalmon;
            SelectedPensForClaster[3] = Brushes.Violet;


            int x1 = 0;
            int y1 = 0;
            //x1 y1 - верхний левый угол

            var delta = (0.1 * width);
            var deltaY = (0.1 * height);

            graph.DrawLine(Pens.Black, new System.Drawing.PointF((float)(x1 + width - delta), y1 + height), new System.Drawing.PointF(x1, y1 + height));  // горизонтальная
            graph.DrawLine(Pens.Black, new System.Drawing.PointF(x1, (float)(y1 + deltaY)), new System.Drawing.PointF(x1, y1 + height)); // вертикальная

            int count = params1.Count;
           

            // рисуем сетку
            for (int i = 1; i <= gridstep; i++)
            {
                graph.DrawLine(Pens.Thistle, new System.Drawing.PointF((float)(x1 + width ), (y1 + height) * i / gridstep), new System.Drawing.PointF(x1, (y1 + height) * i / gridstep));
                graph.DrawLine(Pens.Thistle, new System.Drawing.PointF((float)((x1 + width) * i / gridstep), (float)(y1 + deltaY)), new System.Drawing.PointF((float)((x1 + width) * i / gridstep), y1 + height)); 
            }
            // конец сетка

            graph.DrawLine(Pens.Black, new System.Drawing.PointF((float)(x1 + width - delta), y1 + height), new System.Drawing.PointF(x1, y1 + height));  // горизонтальная
            graph.DrawLine(Pens.Black, new System.Drawing.PointF(x1, (float)(y1 + deltaY)), new System.Drawing.PointF(x1, y1 + height)); // вертикальная



            ////////////////////////////////////// гистограмма или график
            if (name1 == name2)
            {
                /*
                graph.DrawLine(Pens.Black, new System.Drawing.PointF((float)(x1 + width - delta), y1 + height), new System.Drawing.PointF(x1, y1 + height));
                graph.DrawLine(Pens.Black, new System.Drawing.PointF(x1, (float)(y1 + deltaY)), new System.Drawing.PointF(x1, y1 + height));

                */

                double max = double.Parse(params1[0].ToString());
                double min = double.Parse(params1[0].ToString());

                for (int i = 0; i <= count - 1; i++)   // ищем максимумы и минимумы 
                {
                    if (double.Parse(params1[i].ToString()) > max)
                    {
                        max = double.Parse(params1[i].ToString());
                    }
                    if (double.Parse(params1[i].ToString()) < min)
                    {
                        min = double.Parse(params1[i].ToString());
                    }
                }

                var step = ((max - min) / (countOfIntervals - 1));

                int[] countsOccurences = new int[countOfIntervals];   // считаем количество вхождений в данный интервал

                for (int i = 0; i < count; i++)
                {
                    int num;
                    if (double.Parse(params1[i].ToString()) == min)
                    {
                        num = 0;
                    }
                    else
                    {
                        num = (int)((double.Parse(params1[i].ToString()) - min) / step);
                    }
                    countsOccurences[num]++;
                }

                int min1 = countsOccurences[0];
                int max1 = countsOccurences[0];

                for (int i = 0; i < countOfIntervals; i++)
                {
                    if (countsOccurences[i] > max1)
                    {
                        max1 = countsOccurences[i];
                    }
                    if (countsOccurences[i] < min1)
                    {
                        min1 = countsOccurences[i];
                    }
                }


                double step1 = ((double)(max1 - 0) / (height * 0.7)); // у

                double step2 = ((countOfIntervals - 1) / (width * 0.8)); ///////  почему 0,8 подошло!ОО!О!О!!???????


                Brush usualBrush = Brushes.Chocolate;

                for (int i = 0; i < countOfIntervals; i++)
                {
                    String drawString1 = name1;

                    // Create font and brush.
                    System.Drawing.Font drawFont = new System.Drawing.Font("Arial", (float)16 * width / 200, FontStyle.Regular);
                    System.Drawing.Font drawFont2 = new System.Drawing.Font("Arial", 13, FontStyle.Regular); // точка подписи количества попаданий в интервал
                  
                    SolidBrush drawBrush = new SolidBrush(Color.Black);
                    /*
                    // Create point for upper-left corner of drawing.
                    PointF drawPoint1 = new PointF((float)(x1 + width * 0.25), (float)(y1 + height * 0.01));

                    // точка подписи количества попаданий в интервал (и подписи интервалов)
                    PointF drawPoint2 = new PointF((float)((width * 0.7 / countOfIntervals) / 2+ x1 + (i) / step2), (float)(y1 + height  - (countsOccurences[i] - 0) / step1));

                    graph.DrawString(drawString1, drawFont, drawBrush, drawPoint1);
                   

                    graph.FillRectangle(usualBrush, (float)(x1  + (i) / step2), (float)(y1 + height * 0.99 - (countsOccurences[i] - 0) / step1), 
                        (float)(width * 0.7 / (countOfIntervals-1)), (float)((y1 + height) - (y1 + height * 0.99 - (countsOccurences[i] - 0) / step1)));
                    graph.DrawRectangle(Pens.Black, (float)(x1   + (i) / step2), (float)(y1 + height * 0.99 - (countsOccurences[i] - 0) / step1),
                        (float)(width * 0.7 / (countOfIntervals - 1)), (float)((y1 + height) - (y1 + height * 0.99 - (countsOccurences[i] - 0) / step1)));
                    graph.DrawString(countsOccurences[i].ToString(), drawFont2, drawBrush, drawPoint2);


                    // подписи данных
                    PointF drawPoint3 = new PointF((float)((x1 + width - delta) * i / gridstep) , (float)(height ));
                    SolidBrush drawBrush1 = new SolidBrush(Color.LightGreen);

                    graph.DrawString((i*step+min).ToString(), drawFont2, drawBrush1, drawPoint3);
                    */

                    // Create point for upper-left corner of drawing.
                    PointF drawPoint1 = new PointF((float)(x1 + width * 0.25), (float)(y1 + height * 0.01));

                    // точка подписи количества попаданий в интервал 
                    PointF drawPoint2 = new PointF((float)(0.5/step2 + x1 + (i) / step2), (float)(y1 + height*0.95 - (countsOccurences[i] - 0) / step1));

                    graph.DrawString(drawString1, drawFont, drawBrush, drawPoint1);


                    graph.FillRectangle(usualBrush, (float)(x1 + (i) / step2), (float)(y1 + height * 0.99 - (countsOccurences[i] - 0) / step1),
                        (float)(1/step2), (float)((y1 + height) - (y1 + height * 0.99 - (countsOccurences[i] - 0) / step1)));
                    graph.DrawRectangle(Pens.Black, (float)(x1 + (i) / step2), (float)(y1 + height * 0.99 - (countsOccurences[i] - 0) / step1),
                        (float)(1/step2), (float)((y1 + height) - (y1 + height * 0.99 - (countsOccurences[i] - 0) / step1)));
                    graph.DrawString(countsOccurences[i].ToString(), drawFont2, drawBrush, drawPoint2);


                    // подписи данных
                    PointF drawPoint3 = new PointF((float)((x1 + width) * i / countOfIntervals), (float)(height));
                    SolidBrush drawBrush1 = new SolidBrush(Color.Indigo);

                    graph.DrawString((i * step + min).ToString(), drawFont2, drawBrush1, drawPoint3);

                }
                
            }
            else                ////////график
            {
               
                double max1 = double.Parse(params1[0].ToString());
                double min1 = double.Parse(params1[0].ToString());

                double max2 = double.Parse(params2[0].ToString());
                double min2 = double.Parse(params2[0].ToString());

                for (int i = 0; i <= count - 1; i++)   // ищем максимумы и минимумы 
                {
                    if (double.Parse(params1[i].ToString()) > max1)
                    {
                        max1 = double.Parse(params1[i].ToString());
                    }
                    if (double.Parse(params1[i].ToString()) < min1)
                    {
                        min1 = double.Parse(params1[i].ToString());
                    }

                    if (double.Parse(params2[i].ToString()) > max2)
                    {
                        max2 = double.Parse(params2[i].ToString());
                    }
                    if (double.Parse(params2[i].ToString()) < min2)
                    {
                        min2 = double.Parse(params2[i].ToString());
                    }
                }




                var step1 = ((max1 - min1) / (width * 0.7));

                var step2 = ((max2 - min2) / (height * 0.7));

                if ((max1 != min1) && (min2 != max2))  /////////////////////////////////////////////////////////////////////// случай раз
                {
                    for (int i = 0; i <= count - 1; i++)
                    {
                        justPen = usualPen;

                        

                        //  проверка того что точки лежат друг под другом!
                       
                            for (int k = 0; k < count - 1; k++)
                            {
                                if (k != i)
                                {
                                    if ((double.Parse(params1[i].ToString()) == double.Parse(params1[k].ToString())) && (double.Parse(params2[i].ToString()) == double.Parse(params2[k].ToString())) && (i != k))
                                    {
                                        justPen = overlapPen;
                                    }
                                }
                            }
                        
                        // конец  проверка того что точки лежат друг под другом!

                        // проверка на выделенность точки
                            int num = controller.IsClasterOrSelected(i);

                            if (num == -1)
                            {
                                justPen = selectedPen;
                            }
                            else
                            {
                                if (num >= 0)
                                {
                                    if (ClastersVisible)
                                    {
                                        justPen = SelectedPensForClaster[num];
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                    
                                }
                            }
                       /* for (int j = 0; j < controller.SelectedPoints.Count; j++)
                        {

                            if (Int32.Parse(controller.SelectedPoints[j].ToString()) == i)
                            {
                                justPen = selectedPen;
                            }
                        }*/


                        graph.FillEllipse(justPen,
                                             (float)(x1 + width * 0.05 + (double.Parse(params1[i].ToString()) - min1) / step1),
                                             (float)(y1 + height * 0.9 - (double.Parse(params2[i].ToString()) - min2) / step2),
                                             radius, radius);

                    }
                }
                else
                {
                    if ((max1 == min1) && (max2 == min2)) /////////////////////////////////////////////////////// 
                    {
                        if (count > 1)
                        {
                            justPen = overlapPen;
                        }
                        // проверка на выделенность точки
                        
                            for (int i = 0; i < count; i++)
                            {/*
                                for (int j = 0; j < controller.SelectedPoints.Count; j++)
                                {

                                    if (Int32.Parse(controller.SelectedPoints[j].ToString()) == i)
                                    {
                                        justPen = selectedPen;
                                    }
                                }*/
                                int num = controller.IsClasterOrSelected(i);

                                if (num == -1)
                                {
                                    justPen = selectedPen;
                                }
                                else
                                {
                                    if (num >= 0)
                                    {
                                        if (ClastersVisible)
                                        {
                                            justPen = SelectedPensForClaster[num];
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                }
                            }
                        
                      


                        graph.FillEllipse(justPen,
                                                 (float)(x1 + width * 0.5),
                                                 (float)(y1 + height * 0.9 - height * 0.45),
                                                  radius, radius);

                    }
                    else
                    {
                        if ((max1 == min1) && (max2 != min2)) /////////////////////////////////
                        {
                            for (int i = 0; i <= count - 1; i++)
                            {
                              
                                justPen = usualPen;
                               

                                //  проверка того что точки лежат друг под другом!
                              
                                    for (int k = 0; k < count - 1; k++)
                                    {
                                        if (k != i)
                                        {
                                            if ((double.Parse(params2[i].ToString()) == double.Parse(params2[k].ToString())) && (i != k))
                                            {
                                                justPen = overlapPen;
                                            }
                                        }
                                    }
                                // конец  проверка того что точки лежат друг под другом!

                                // проверка на выделенность точки
                                /*
                                for (int j = 0; j < controller.SelectedPoints.Count; j++)
                                {

                                    if (Int32.Parse(controller.SelectedPoints[j].ToString()) == i)
                                    {
                                        justPen = selectedPen;
                                    }
                                }*/
                                    int num = controller.IsClasterOrSelected(i);

                                    if (num == -1)
                                    {
                                        justPen = selectedPen;
                                    }
                                    else
                                    {
                                        if (num >= 0)
                                        {
                                            if (ClastersVisible)
                                            {
                                                justPen = SelectedPensForClaster[num];
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                    }

                                graph.FillEllipse(justPen,
                                                     (float)(x1 + width * 0.5),
                                                     (float)(y1 + height * 0.9 - (double.Parse(params2[i].ToString()) - min2) / step2),
                                                      radius, radius);
                            }
                        }
                        else
                        {
                            if ((max1 != min1) && (max2 == min2))     ////////////////////////////////////////
                            {

                                for (int i = 0; i <= count - 1; i++)
                                {

                                    justPen = usualPen;
                                  

                                    //  проверка того что точки лежат друг под другом!
                                    if (justPen != selectedPen) // если точка не выделена то можно подкрашивать на совпадения 
                                    {
                                        for (int k = 0; k < count - 1; k++)
                                        {
                                            if (k != i)
                                            {
                                                if ((double.Parse(params1[i].ToString()) == double.Parse(params1[k].ToString()))  && (i != k))
                                              //  if (((x1 + width * 0.05 + (double.Parse(params1[i].ToString()) - min1) / step1) - (x1 + width * 0.05 + (double.Parse(params1[k].ToString()) - min1) / step1)) <= radius)
                                                {
                                                    justPen = overlapPen;
                                                }
                                            }
                                        }
                                    }
                                    // конец  проверка того что точки лежат друг под другом!
                                    // проверка на выделенность точки
                                    /*
                                    for (int j = 0; j < controller.SelectedPoints.Count; j++)
                                    {

                                        if (Int32.Parse(controller.SelectedPoints[j].ToString()) == i)
                                        {
                                            justPen = selectedPen;
                                        }
                                    }
                                    */
                                    int num = controller.IsClasterOrSelected(i);

                                    if (num == -1)
                                    {
                                        justPen = selectedPen;
                                    }
                                    else
                                    {
                                        if (num >= 0)
                                        {
                                            if (ClastersVisible)
                                            {
                                                justPen = SelectedPensForClaster[num];
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                    }

                                    graph.FillEllipse(justPen,
                                                         (float)(x1 +width*0.05+ (double.Parse(params1[i].ToString()) - min1) / step1),
                                                         (float)
                                                         (y1 + height * 0.9 - height * 0.45),
                                                         radius, radius);
                                }
                            }
                        }
                    }
                }

                // Create string to draw.
                String drawString2 = name2;
                String drawString1 = name1;   //подписи осей

                // Create font and brush.
                System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 16);
                SolidBrush drawBrush = new SolidBrush(Color.Black);

                // Create point for upper-left corner of drawing.
                PointF drawPoint1 = new PointF(10, 0);
                PointF drawPoint2 = new PointF((float)(pictureBox1.Width * 0.8), (float)(pictureBox1.Height * 0.9));


                graph.DrawString(drawString1, drawFont, drawBrush, drawPoint1);
                graph.DrawString(drawString2, drawFont, drawBrush, drawPoint2);
 
            }
            
            pictureBox1.Image = bitmap;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClastersVisible = !ClastersVisible;
            if (ClastersVisible)
            {
                button1.Text = "Убрать точки кластера";
            }
            else
            {
                button1.Text = "Показать точки кластера";
            }

           graph.Clear(Color.White);
           RedrawMe();
        }

        public double GetRadius(PointF p1, PointF p2)
        {
            double Res;
            Res = (double)Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));

            return Res;
        }

        public double GetMax(double p1, double p2)
        {
            if (p1 > p2)
            {
                return p1;
            }
            else
            {
                return p2;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            
                isMoovingMouse = true;
                Point new_P = new Point(e.X, e.Y);
                lastPoint = new_P;
                pointsOfPolygon.Clear();
                pointsOfPolygon.Add(new_P);
            
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            
                int countOfPeresech;
                isMoovingMouse = false;

                double A, B, C;
                double A1, B1, C1; // прямая на двух точках

                int count = myPoints[0].Count;

                for (int j = 0; j < count; j++)
                {
                    // проверка на выделенность точки
                    int num = controller.IsClasterOrSelected(j);

                    if (num == -1)
                    {
                        // выделенная все ок
                    }
                    else
                    {
                        if (num >= 0) // если кластер то не выделяем
                        {
                            continue;
                        }
                    }

                    countOfPeresech = 0;
                    // уравнение трассированного луча из точки Ax+By+C=0
                    A = (0);
                    B = -(int.MaxValue - double.Parse(myPoints[0][j].ToString()));
                    C = -A*double.Parse(myPoints[0][j].ToString()) - B*double.Parse(myPoints[1][j].ToString());

                    float dy = (float) (-C/B);
           
                    pictureBox1.Image = bitmap;


                    for (int i = 0; i < pointsOfPolygon.Count; i++)
                    {
                        if (i == pointsOfPolygon.Count - 1)
                        {
                            A1 = pointsOfPolygon[0].Y - pointsOfPolygon[i].Y;
                            B1 = -(pointsOfPolygon[0].X - pointsOfPolygon[i].X);
                            C1 = -A1*pointsOfPolygon[i].X - B1*pointsOfPolygon[i].Y;
                        }
                        else
                        {
                            A1 = pointsOfPolygon[i + 1].Y - pointsOfPolygon[i].Y;
                            B1 = -(pointsOfPolygon[i + 1].X - pointsOfPolygon[i].X);
                            C1 = -A1*pointsOfPolygon[i].X - B1*pointsOfPolygon[i].Y;
                        }


                        PointF p1, p2, p3;

                        if ((A*B1 - A1*B) != 0)
                        {
                            double Xp = (B*C1 - B1*C)/(A*B1 - A1*B);
                            double Yp = (C*A1 - C1*A)/(A*B1 - A1*B);

                            if (Xp < double.Parse(myPoints[0][j].ToString())) continue;

                            // проверка что т.п. это точка полигона

                            bool isOnPolygon = false;

                            if (i == pointsOfPolygon.Count - 1)
                            {
                                isOnPolygon = (Xp == pointsOfPolygon[0].X && Yp == pointsOfPolygon[0].Y);
                            }
                            else
                            {
                                isOnPolygon = (Xp == pointsOfPolygon[i + 1].X && Yp == pointsOfPolygon[i + 1].Y);
                            }


                            if ((Xp == pointsOfPolygon[i].X && Yp == pointsOfPolygon[i].Y) || isOnPolygon)
                            {
                                MessageBox.Show("Yes, point in line");
                                if (i + 1 == pointsOfPolygon.Count)
                                {
                                    //ноль
                                }
                                else
                                {
                                    double tmp1 = A*pointsOfPolygon[i + 2].X + B*pointsOfPolygon[i + 2].Y + C;
                                        // знак другой чем
                                    double tmp2 = A*pointsOfPolygon[i].X + B*pointsOfPolygon[i].Y + C;
                                        // знак другой чем тут

                                    if (tmp1/Math.Abs(tmp1) != tmp2/Math.Abs(tmp2))
                                    {
                                        // прямые по разные стороны
                                        MessageBox.Show("It's Ok!");
                                        countOfPeresech++;
                                        i++;
                                    }

                                }
                            }

                            //

                            PointF Pp = new PointF((float) Xp, (float) Yp);


                            if (i == pointsOfPolygon.Count - 1)
                            {
                                p1 = new PointF(Pp.X ,
                                                Pp.Y);
                                p2 = new PointF(pointsOfPolygon[i].X,
                                                pointsOfPolygon[i].Y);
                                p3 = new PointF(pointsOfPolygon[0].X ,
                                                pointsOfPolygon[0].Y );
                            }
                            else
                            {
                               
                                p1 = new PointF(Pp.X, Pp.Y);
                                p3 = new PointF(pointsOfPolygon[i].X, pointsOfPolygon[i].Y);
                                p2 = new PointF(pointsOfPolygon[i + 1].X, pointsOfPolygon[i + 1].Y);

                            }

                            if (GetMax(GetRadius(p1, p2), GetRadius(p1, p3)) < GetRadius(p2, p3))
                            {
                              
                                double f1 = GetMax(GetRadius(p3, p2), GetRadius(p3, p1));
                                double f2 = GetRadius(p1, p2);
                                countOfPeresech++;

                            }
                        }
                    }
                    
                    if (countOfPeresech%2 == 1)
                    {
                        controller.AddSelectedPoint(j);
                        RedrawMe();
                    }
                }

            
            if(!forel)
            timer1.Enabled = true;

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (name1 != name2)
            {
                if (isMoovingMouse)
                {

                    if (
                        Math.Sqrt((lastPoint.X - e.X)*(lastPoint.X - e.X) +
                                  (lastPoint.Y - e.Y)*(lastPoint.Y - e.Y)) >= radius)
                        // здесь должен быть !диаметр! моих исходных окружностей, чью принадлежность полигону я ищу
                    {
                        graph.FillEllipse(Brushes.Black, e.X, e.Y, 3, 3);
                        pictureBox1.Image = bitmap;

                        pointsOfPolygon.Add(new Point(e.X, e.Y));
                        lastPoint = new Point(e.X, e.Y);
                    }

                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            graph.Clear(Color.White);
            RedrawMe();
            timer1.Enabled = false;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            forel = !forel;
        }







    }
}
