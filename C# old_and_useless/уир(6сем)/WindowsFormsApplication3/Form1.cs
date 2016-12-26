using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.Threading;
using Point = System.Drawing.Point;




namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        private Controller myController;

        private Form4 f4 = null;

        private int width;// ширина одного графика
        private int height;// высота одного графика

        const int rad = 3;  // радиус точки на матрице
        private int countOfIntervals = 5; // число интервалов в гистограмме, при возможности 

        private Bitmap btmap;
        private Graphics graphics;
        private int CountOfAttr = 0;  // переменная в которой храним количество атрибутов

        private bool flag = false;      // мы уже построили матрицу проекций = true . для того чтобы по пустым данным не тыкать на picturebox
        private bool visbleNotSelectedPoints = true; // показываем ли мы невыделенные точки

        private ArrayList[] myAtributs; // для постоения крупного плана матрицы значений

        // полигон
        List<Point> pointsOfPolygon = new List<Point>(); // точки моего полигона
        private Point lastPoint;
        private bool isMoovingMouse = false;
        // конец полигон

        // отображение второй формы тут(пикчер бокс)-учелич изображение
        private ArrayList[] myPoints;
        private ArrayList params1;
        private ArrayList params2;
        private Graphics graphics2;
        private Bitmap bitmap2;
        private string name1;
        private string name2;
        private int numRow;
        private int numCol;
        private int width2;// ширина одного графика
        private int height2;// высота одного графика
        public String[] allAxes;

        int gridstep = 5;
        int radius = 8;
        //конец отображение второй формы тут(пикчер бокс)-учелич изображение
        public int lpMetrics = 2;

        private bool ChoiseCentoids = false; // режим выбора центроидов
        private int CountOfSelectedCentroids = 0; // число выбранных центроидов
        private MyObject[] centroids;

        private bool ClastersVisible = true;
        private Form2 form2;
        private bool forel = false;

        private bool isRefreshedGrid = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) // скрыть показать точки выделенные
        {
            try
            {
                visbleNotSelectedPoints = !visbleNotSelectedPoints;
                flag = true;
                plotMatrix(myController.GiveMeListOfMyObject(), myController.GetCountObj(), myController.GetCountAttrib());
            }
            catch (Exception)
            {

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            myController = new Controller();
            // MessageBox.Show("Math.Pow(16,0.5) " + Math.Pow(16, 0.5) + "\n" + "Math.Pow( 16,(1/2) ) " + Math.Pow(16, (double)(1.0 / 2)) );
        }

        /*   private void button2_Click(object sender, EventArgs e)
           {
           
               myController.ReadFromExcel(dataGridView1);   // запись в датагрид из экселя + считывание в память
           
           }
           */

        public void Standartization() // стандартизация переменных
        {
            int countOfObjects = myAtributs[0].Count;
            double[] midleAtrr = new double[CountOfAttr];
            for (int i = 0; i < CountOfAttr; i++)
            {
                for (int j = 0; j < countOfObjects; j++)
                {
                    midleAtrr[i] += double.Parse(myAtributs[i][j].ToString()) / CountOfAttr;
                }
            }
            double[] DispersionOfAtrr = new double[CountOfAttr];

            for (int i = 0; i < CountOfAttr; i++)
            {
                for (int j = 0; j < countOfObjects; j++)
                {
                    DispersionOfAtrr[i] += Math.Pow(double.Parse(myAtributs[i][j].ToString()) - midleAtrr[i], 2) / CountOfAttr;
                }
            }

            ArrayList[] myNewAtr = new ArrayList[CountOfAttr];

            for (int i = 0; i < CountOfAttr; i++)
            {
                myNewAtr[i] = new ArrayList();
            }

            for (int i = 0; i < CountOfAttr; i++)
            {
                for (int j = 0; j < countOfObjects; j++)
                {
                    if (DispersionOfAtrr[i] == 0)
                    {
                        myNewAtr[i].Add(0);
                    }
                    else
                    {
                        myNewAtr[i].Add((double.Parse(myAtributs[i][j].ToString()) - midleAtrr[i]) / Math.Pow(DispersionOfAtrr[i], 0.5));
                    }

                }
            }

            myAtributs = myNewAtr;
        }

        public void plotMatrix(List<MyObject> allObjects, int countObj, int countAttrib) // строит матрицу проекций 
        {
            //только зачитанные данные могут здесь быть! проверка на не ноль количества атрибубтов

            width = (pictureBox1.Width - 10) / countAttrib;
            height = (pictureBox1.Height - 10) / countAttrib;
            /*  if (countAttrib == 1)
              {
                  width = width/2;
                  height = height/2;
              }*/

            btmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(btmap);
            CountOfAttr = countAttrib;

            myAtributs = new ArrayList[countAttrib];

            for (int i = 0; i < countAttrib; i++)
            {
                myAtributs[i] = new ArrayList();
            }

            foreach (var myobject1 in allObjects)
            {
                for (int i = 0; i < countAttrib; i++)
                {
                    myAtributs[i].Add(myobject1.attrib[i].ToString());
                }
            }

            // myAtriburs[] - нужно стандартизовать или нормализовать! 

            // Normalization();
            // or
            // Standartization();

            // что лучше?!??!

            // Standartization(); // будем вызывать всякий раз как будем перерисовывать?!?!?!?!?!?!?
            // стандартизация ни на что не повлияла. потому что при рисовании была нормализация. а при расчете данных кластера использовались изначальные значения переменных

            for (int i = 0; i < countAttrib; i++)
            {
                for (int j = 0; j < countAttrib; j++)
                {
                    if (i == j)
                    {
                        PlotGistogramm(myAtributs[i], graphics, i, j, width, height);
                    }
                    if (i != j || countAttrib == 1)
                    {
                        /*   if (countAttrib == 1)
                           {
                               plotGraph(myAtributs[i], myAtributs[j], graphics, 1, 0, width, height);
                           }
                           else*/
                        {
                            plotGraph(myAtributs[i], myAtributs[j], graphics, i, j, width, height);
                        }

                    }
                }
            }

            pictureBox1.Image = btmap;


        }

        public void PlotAxes(Graphics graphics, int countAttrib)
        {

            for (int i = 0; i < countAttrib; i++)
            {
                for (int j = 0; j < countAttrib; j++)
                {
                    int x1 = i * width;
                    int y1 = j * height;
                    // x1 y1 - верхний левый угол

                    var delta = (0.1 * width);
                    var deltaY = (0.1 * height);

                    graphics.DrawLine(Pens.Black, new System.Drawing.PointF((float)(x1 + width - delta), y1 + height), new System.Drawing.PointF(x1, y1 + height));
                    graphics.DrawLine(Pens.Black, new System.Drawing.PointF(x1, (float)(y1 + deltaY)), new System.Drawing.PointF(x1, y1 + height));
                }
            }



        }  // перерисовываем все заново. потому что выделяли график над которым находимся

        /// <summary>
        /// строим гистограммы
        /// </summary>
        /// <param name="params1">точки</param>
        /// <param name="graphics"></param>
        /// <param name="numRow"></param>
        /// <param name="numColumn"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void PlotGistogramm(ArrayList params1, Graphics graphics, int numRow, int numColumn, int width, int height)
        {
            int x1 = numRow * width;
            int y1 = numColumn * height;
            //x1 y1 - верхний левый угол

            var delta = (0.1 * width);
            var deltaY = (0.1 * height);

            graphics.DrawLine(Pens.Black, new System.Drawing.PointF((float)(x1 + width - delta), y1 + height), new System.Drawing.PointF(x1, y1 + height));
            graphics.DrawLine(Pens.Black, new System.Drawing.PointF(x1, (float)(y1 + deltaY)), new System.Drawing.PointF(x1, y1 + height));

            int count = params1.Count;

            // пока для цифровых входных символов
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

            double step2 = ((countOfIntervals - 1) / (width * 0.7));

            Brush usualBrush = Brushes.Chocolate;


            for (int i = 0; i < countOfIntervals; i++)
            {
                String drawString1 = (dataGridView1.Columns[numColumn].Name);

                // Create font and brush.
                System.Drawing.Font drawFont = new System.Drawing.Font("Arial", (float)16 * width / 200);
                SolidBrush drawBrush = new SolidBrush(Color.Black);

                // Create point for upper-left corner of drawing.
                PointF drawPoint1 = new PointF((float)(x1 + width * 0.25), (float)(y1 + height * 0.01));
                graphics.DrawString(drawString1, drawFont, drawBrush, drawPoint1);

                graphics.FillRectangle(usualBrush, (float)(x1 + width * 0.05 + (i) / step2), (float)(y1 + height * 0.99 - (countsOccurences[i] - 0) / step1), (float)(width * 0.7 / (countOfIntervals - 1)), (float)((y1 + height) - (y1 + height * 0.99 - (countsOccurences[i] - 0) / step1)));
                graphics.DrawRectangle(Pens.Black, (float)(x1 + width * 0.05 + (i) / step2), (float)(y1 + height * 0.99 - (countsOccurences[i] - 0) / step1), (float)(width * 0.7 / (countOfIntervals - 1)), (float)((y1 + height) - (y1 + height * 0.99 - (countsOccurences[i] - 0) / step1)));
            }



            //   countOfIntervals = 5; //???????????????
        }

        /// <summary>
        /// функция строящая матрицы проекций (одну за вызов)
        /// </summary>
        /// <param name="params1">переменные x</param>
        /// <param name="params2">переменные y</param>
        /// <param name="graphics"></param>
        /// <param name="numRow">номер в строке(столбец) - х </param>
        /// <param name="numColumn">здесь номер в колонке - у </param>
        /// <param name="width"> pictureBox1.Width / countAttrib </param>
        /// <param name="height"></param>
        /// 
        public void plotGraph(ArrayList params1, ArrayList params2, Graphics graphics, int numRow, int numColumn, int width, int height)
        {
            Pen usualPen = Pens.SeaGreen;
            Pen selectedPen = Pens.Red;


            Pen[] SelectedPensForClaster = new Pen[myController.GetNumberOfClasters()];
            int countOfClasters = myController.GetNumberOfClasters();
            for (int i = 0; i < countOfClasters; i++)
            {
                SelectedPensForClaster[i] = new Pen(myController.GetClasterColor(i));
            }

            Pen justPen = usualPen;

            int x1 = numRow * width;
            int y1 = numColumn * height;
            //x1 y1 - верхний левый угол



            var delta = (0.1 * width);
            var deltaY = (0.1 * height);

            graphics.DrawLine(Pens.Black, new System.Drawing.PointF((float)(x1 + width - delta), y1 + height), new System.Drawing.PointF(x1, y1 + height));
            graphics.DrawLine(Pens.Black, new System.Drawing.PointF(x1, (float)(y1 + deltaY)), new System.Drawing.PointF(x1, y1 + height));

            int count = params1.Count;

            // пока для цифровых входных символов
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

            if ((max1 != min1) && (min2 != max2))
            {
                for (int i = 0; i <= count - 1; i++)
                {
                    justPen = usualPen;

                    /*   for (int j = 0; j < myController.SelectedPoints.Count; j++)
                       {

                           /*if ((Int32.Parse(myController.SelectedPoints[j].ToString()) == i) && myController.SelectedPoints.Count!=0)
                           {
                               justPen = selectedPen;
                           }*/
                    int num = myController.IsClasterOrSelected(i);

                    if (num == -1)
                    {
                        justPen = selectedPen;
                    }
                    else
                    {
                        if (num >= 0)
                        {
                            justPen = SelectedPensForClaster[num];
                            if (!ClastersVisible) continue;
                        }
                    }


                    /*  }  */


                    if ((visbleNotSelectedPoints && justPen != selectedPen) || justPen == selectedPen)
                    {
                        graphics.DrawEllipse(justPen,
                                             (float)(x1 + width * 0.05 + (double.Parse(params1[i].ToString()) - min1) / step1),
                                             (float)(y1 + height * 0.9 - (double.Parse(params2[i].ToString()) - min2) / step2),
                                             rad, rad);
                    }


                }
            }
            else
            {
                if ((max1 == min1) && (max2 == min2))
                {
                    for (int i = 0; i < count; i++)
                    {
                        /*
                         for (int j = 0; j < myController.SelectedPoints.Count; j++)
                            {

                                if ((Int32.Parse(myController.SelectedPoints[j].ToString()) == i) && myController.SelectedPoints.Count != 0)
                                {
                                    justPen = selectedPen;
                                }
                            } 
                         */
                        int num = myController.IsClasterOrSelected(i);

                        if (num == -1)
                        {
                            justPen = selectedPen;
                        }
                        else
                        {
                            if (num >= 0)
                            {
                                justPen = SelectedPensForClaster[num];
                            }
                        }
                    }

                    if ((visbleNotSelectedPoints && justPen != selectedPen) || justPen == selectedPen)
                    {
                        graphics.DrawEllipse(justPen,
                                            (float)(x1 + width * 0.5),
                                            (float)(y1 + height * 0.9 - height * 0.45),
                                            rad, rad);
                    }


                }
                else
                {
                    if ((max1 == min1) && (max2 != min2))
                    {
                        for (int i = 0; i <= count - 1; i++)
                        {
                            justPen = usualPen;
                            /*
                            for (int j = 0; j < myController.SelectedPoints.Count; j++)
                            {

                                if (Int32.Parse(myController.SelectedPoints[j].ToString()) == i && myController.SelectedPoints.Count != 0)
                                {
                                    justPen = selectedPen;
                                }
                            }
                            */
                            int num = myController.IsClasterOrSelected(i);

                            if (num == -1)
                            {
                                justPen = selectedPen;
                            }
                            else
                            {
                                if (num >= 0)
                                {
                                    justPen = SelectedPensForClaster[num];
                                    if (!ClastersVisible) continue;
                                }
                            }
                            if ((visbleNotSelectedPoints && justPen != selectedPen) || justPen == selectedPen)
                            // если невыделенные точки будут видны на графике
                            {
                                graphics.DrawEllipse(justPen,
                                                     (float)(x1 + width * 0.5),
                                                     (float)
                                                     (y1 + height * 0.9 -
                                                      (double.Parse(params2[i].ToString()) - min2) / step2),
                                                      rad, rad);
                            }
                        }
                    }
                    else
                    {
                        if ((max1 != min1) && (max2 == min2))
                        {
                            for (int i = 0; i <= count - 1; i++)
                            {
                                justPen = usualPen;
                                /*
                                                                for (int j = 0; j < myController.SelectedPoints.Count; j++)
                                                                {

                                                                    if (Int32.Parse(myController.SelectedPoints[j].ToString()) == i && myController.SelectedPoints.Count != 0)
                                                                    {
                                                                        justPen = selectedPen;
                                                                    }
                                                                }
                                                                */
                                int num = myController.IsClasterOrSelected(i);

                                if (num == -1)
                                {
                                    justPen = selectedPen;
                                }
                                else
                                {
                                    if (num >= 0)
                                    {
                                        justPen = SelectedPensForClaster[num];
                                        if (!ClastersVisible) continue;
                                    }
                                }
                                if ((visbleNotSelectedPoints && justPen != selectedPen) || justPen == selectedPen)
                                // если невыделенные точки будут видны на графике
                                {
                                    graphics.DrawEllipse(justPen,
                                                         (float)
                                                         (x1 + width * 0.05 + (double.Parse(params1[i].ToString()) - min1) / step1),
                                                         (float)
                                                         (y1 + height * 0.9 - height * 0.45),
                                                          rad, rad);
                                }
                            }
                        }
                    }
                }
            }

        }

        public ArrayList[] GivePoints(ArrayList params1, ArrayList params2, int width, int height)  // запоминаем координаты точек чтобы не считать во второй форме
        {
            ArrayList[] myPoints = new ArrayList[2];

            for (int i = 0; i < 2; i++)
            {
                myPoints[i] = new ArrayList();
            }



            const int x1 = 0;
            const int y1 = 0;
            //x1 y1 - верхний левый угол

            int count = params1.Count;

            // пока для цифровых входных символов
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

            if ((max1 != min1) && (min2 != max2))
            {
                for (int i = 0; i <= count - 1; i++)
                {
                    myPoints[0].Add(x1 + width * 0.05 + (double.Parse(params1[i].ToString()) - min1) / step1);
                    myPoints[1].Add(y1 + height * 0.9 - (double.Parse(params2[i].ToString()) - min2) / step2);
                }
            }
            else
            {
                if ((max1 == min1) && (max2 == min2))
                {
                    myPoints[0].Add(x1 + width * 0.5);
                    myPoints[1].Add(y1 + height * 0.9 - height * 0.45);

                }
                else
                {
                    if ((max1 == min1) && (max2 != min2))
                    {
                        for (int i = 0; i <= count - 1; i++)
                        {
                            myPoints[0].Add(x1 + width * 0.5);
                            myPoints[1].Add(y1 + height * 0.9 - (double.Parse(params2[i].ToString()) - min2) / step2);
                        }
                    }
                    else
                    {
                        if ((max1 != min1) && (max2 == min2))
                        {
                            for (int i = 0; i <= count - 1; i++)
                            {

                                myPoints[0].Add(x1 + width * 0.05 + (double.Parse(params1[i].ToString()) - min1) / step1);
                                myPoints[1].Add(y1 + height * 0.9 - height * 0.45);

                            }
                        }
                    }
                }
            }

            return myPoints;
        }

        private void button3_Click(object sender, EventArgs e) // строим матрицу проекций
        {
            try
            {
                flag = true;
                plotMatrix(myController.GiveMeListOfMyObject(), myController.GetCountObj(), myController.GetCountAttrib());
            }
            catch (Exception)
            {

            }


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        public void DrawPictureInSecondForm(int numRow, int numColumn)   // рисуем нашу увеличенную проекцию во второй форме
        {


            if (form2 == null || form2.IsDisposed)
            {

                form2 = new Form2();
                ArrayList[] myPoints = GivePoints(myAtributs[numRow], myAtributs[numColumn], form2.getMePB().Width - 10,
                                                  form2.getMePB().Height - 10);

                String drawString2 = (dataGridView1.Columns[numRow].Name);
                String drawString1 = (dataGridView1.Columns[numColumn].Name); //подписи осей

                form2.Show();

                form2.getParamsForRefresh(myAtributs[numRow], myAtributs[numColumn], form2.getMePB().Width - 10,
                                          form2.getMePB().Height - 10, drawString1, drawString2, numRow, numColumn);

                form2.SetController(myController);
                form2.RedrawMe();
                form2.allAxes = new string[CountOfAttr];
                for (int i = 0; i < CountOfAttr; i++)
                {
                    form2.allAxes[i] = dataGridView1.Columns[i].Name;
                }
                form2.TakePoints(myPoints); // точки "нажатий"
            }


        }

        public void RedrawMe()  // перерисовка второго пикчербокса - увеличенной проекции на форме 1
        {
            int width = pictureBox2.Width - 20;
            int height = pictureBox2.Height - 20;
            if (graphics2 != null)
            {
                Graphics graph = graphics2;
                graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                // graph.Clear(Color.White);

                Brush usualPen = Brushes.SeaGreen;
                Brush selectedPen = Brushes.Red; // выделенная точка
                Brush overlapPen = Brushes.Blue; //совпадающие точки
                Brush justPen = usualPen;
                /* Brush[] SelectedPensForClaster = new Brush[5];       // количнство кластеров и цвет соответсвенно
                 ////
                 SelectedPensForClaster[0] = Brushes.Gold;
                 SelectedPensForClaster[1] = Brushes.Maroon;
                 SelectedPensForClaster[2] = Brushes.LightSalmon;
                 SelectedPensForClaster[3] = Brushes.Violet;*/

                Brush[] SelectedPensForClaster = new Brush[myController.GetNumberOfClasters()];
                int countOfClasters = myController.GetNumberOfClasters();
                for (int i = 0; i < countOfClasters; i++)
                {
                    SelectedPensForClaster[i] = new SolidBrush(myController.GetClasterColor(i));
                }


                int x1 = 0;
                int y1 = 0;
                //x1 y1 - верхний левый угол

                var delta = (0.1 * width);
                var deltaY = (0.1 * height);

                /*graph.DrawLine(Pens.Black, new System.Drawing.PointF((float)(x1 + width - delta), y1 + height), new System.Drawing.PointF(x1, y1 + height));  // горизонтальная
                  graph.DrawLine(Pens.Black, new System.Drawing.PointF(x1, (float)(y1 + deltaY)), new System.Drawing.PointF(x1, y1 + height)); // вертикальная*/

                int count = params1.Count;


                // рисуем сетку
                for (int i = 1; i <= gridstep; i++)
                {
                    graph.DrawLine(Pens.Thistle, new System.Drawing.PointF((float)(x1 + width), (y1 + height) * i / gridstep), new System.Drawing.PointF(x1, (y1 + height) * i / gridstep));
                    graph.DrawLine(Pens.Thistle, new System.Drawing.PointF((float)((x1 + width) * i / gridstep), (float)(y1 + deltaY)), new System.Drawing.PointF((float)((x1 + width) * i / gridstep), y1 + height));
                }
                // конец сетка

                graph.DrawLine(Pens.Black, new System.Drawing.PointF((float)(x1 + width - delta), y1 + height), new System.Drawing.PointF(x1, y1 + height));  // горизонтальная
                graph.DrawLine(Pens.Black, new System.Drawing.PointF(x1, (float)(y1 + deltaY)), new System.Drawing.PointF(x1, y1 + height)); // вертикальная



                ////////////////////////////////////// гистограмма или график
                if (name1 == name2)
                {

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
                        System.Drawing.Font drawFont2 = new System.Drawing.Font("Arial", 10, FontStyle.Regular); // точка подписи количества попаданий в интервал

                        SolidBrush drawBrush = new SolidBrush(Color.Black);

                        // Create point for upper-left corner of drawing.
                        PointF drawPoint1 = new PointF((float)(x1 + width * 0.25), (float)(y1 + height * 0.01));

                        // точка подписи количества попаданий в интервал 
                        PointF drawPoint2 = new PointF((float)(0.5 / step2 + x1 + (i) / step2), (float)(y1 + height * 0.95 - (countsOccurences[i] - 0) / step1));

                        graph.DrawString(drawString1, drawFont, drawBrush, drawPoint1);


                        graph.FillRectangle(usualBrush, (float)(x1 + (i) / step2), (float)(y1 + height * 0.99 - (countsOccurences[i] - 0) / step1),
                            (float)(1 / step2), (float)((y1 + height) - (y1 + height * 0.99 - (countsOccurences[i] - 0) / step1)));
                        graph.DrawRectangle(Pens.Black, (float)(x1 + (i) / step2), (float)(y1 + height * 0.99 - (countsOccurences[i] - 0) / step1),
                            (float)(1 / step2), (float)((y1 + height) - (y1 + height * 0.99 - (countsOccurences[i] - 0) / step1)));
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


                    // рисуем центроиды если есть  centroids
                    int countOfCentroids = myController.GetNumberOfClasters();
                    if (CountOfSelectedCentroids < countOfCentroids) countOfCentroids = CountOfSelectedCentroids;
                    for (int h = 0; h < countOfCentroids; h++)
                    {
                        if (centroids[h] != null)
                        {
                            SolidBrush drawBrush1 = new SolidBrush(myController.ClasterColor[h]);
                            graph.FillRectangle(drawBrush1,
                                                   (float)(x1 - 3 + width * 0.05 + (double.Parse(centroids[h].attrib[numRow].ToString()) - min1) / step1),
                                                   (float)(y1 - 3 + height * 0.9 - (double.Parse(centroids[h].attrib[numCol].ToString()) - min2) / step2),
                                                   radius + 6, radius + 6);

                            

                            /* graph.FillEllipse(drawBrush1,
                                                    (float)(x1 + width * 0.05 + (double.Parse(centroids[h].attrib[numRow].ToString()) - min1) / step1),
                                                    (float)(y1 + height * 0.9 - (double.Parse(centroids[h].attrib[numCol].ToString()) - min2) / step2),
                                                    radius+4 , radius+4 );*/
                        }

                    }



                    //



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
                            int num = myController.IsClasterOrSelected(i);

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

                            // подписи данных
                            PointF drawPoint3 = new PointF((float)(x1 + width * 0.05 + (double.Parse(params1[i].ToString()) - min1) / step1) + 4, (float)(y1 + height * 0.9 - (double.Parse(params2[i].ToString()) - min2) / step2) + 4);
                            SolidBrush drawBrush1 = new SolidBrush(Color.Indigo);
                            System.Drawing.Font drawFont2 = new System.Drawing.Font("Arial", 8, FontStyle.Regular);
                            graph.DrawString((i).ToString(), drawFont2, drawBrush1, drawPoint3);
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
                                int num = myController.IsClasterOrSelected(i);

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
                                    int num = myController.IsClasterOrSelected(i);

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
                                    PointF drawPoint3 = new PointF((float)(x1 + width * 0.5) + 4, (float)(y1 + height * 0.9 - (double.Parse(params2[i].ToString()) - min2) / step2) + 4);
                                    SolidBrush drawBrush1 = new SolidBrush(Color.Indigo);
                                    System.Drawing.Font drawFont2 = new System.Drawing.Font("Arial", 8, FontStyle.Regular);
                                    graph.DrawString((i).ToString(), drawFont2, drawBrush1, drawPoint3);
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
                                                    if ((double.Parse(params1[i].ToString()) == double.Parse(params1[k].ToString())) && (i != k))
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
                                        int num = myController.IsClasterOrSelected(i);

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
                                                             (float)(x1 + width * 0.05 + (double.Parse(params1[i].ToString()) - min1) / step1),
                                                             (float)
                                                             (y1 + height * 0.9 - height * 0.45),
                                                             radius, radius);

                                        PointF drawPoint3 = new PointF((float)(x1 + width * 0.05 + (double.Parse(params1[i].ToString()) - min1) / step1)+4, (float)(y1 + height * 0.9 - height * 0.45)+4);
                                        SolidBrush drawBrush1 = new SolidBrush(Color.Indigo);
                                        System.Drawing.Font drawFont2 = new System.Drawing.Font("Arial", 8, FontStyle.Regular);
                                        graph.DrawString((i).ToString(), drawFont2, drawBrush1, drawPoint3);
                                    }
                                }
                            }
                        }
                    }















                    #region debil
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
                            int num = myController.IsClasterOrSelected(i);

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


                            if (justPen == selectedPen) graph.FillEllipse(justPen,
                                                 (float)(x1 + width * 0.05 + (double.Parse(params1[i].ToString()) - min1) / step1),
                                                 (float)(y1 + height * 0.9 - (double.Parse(params2[i].ToString()) - min2) / step2),
                                                 radius, radius);

                            // подписи данных
                            PointF drawPoint3 = new PointF((float)(x1 + width * 0.05 + (double.Parse(params1[i].ToString()) - min1) / step1) + 4, (float)(y1 + height * 0.9 - (double.Parse(params2[i].ToString()) - min2) / step2) + 4);
                            SolidBrush drawBrush1 = new SolidBrush(Color.Indigo);
                            System.Drawing.Font drawFont2 = new System.Drawing.Font("Arial", 8, FontStyle.Regular);
                            //graph.DrawString((i).ToString(), drawFont2, drawBrush1, drawPoint3);
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
                                int num = myController.IsClasterOrSelected(i);

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



                            if(justPen==selectedPen)
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
                                    int num = myController.IsClasterOrSelected(i);

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

                                    if (justPen == selectedPen) graph.FillEllipse(justPen,
                                                         (float)(x1 + width * 0.5),
                                                         (float)(y1 + height * 0.9 - (double.Parse(params2[i].ToString()) - min2) / step2),
                                                          radius, radius);
                                    PointF drawPoint3 = new PointF((float)(x1 + width * 0.5) + 4, (float)(y1 + height * 0.9 - (double.Parse(params2[i].ToString()) - min2) / step2) + 4);
                                    SolidBrush drawBrush1 = new SolidBrush(Color.Indigo);
                                    System.Drawing.Font drawFont2 = new System.Drawing.Font("Arial", 8, FontStyle.Regular);
                                    //graph.DrawString((i).ToString(), drawFont2, drawBrush1, drawPoint3);
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
                                                    if ((double.Parse(params1[i].ToString()) == double.Parse(params1[k].ToString())) && (i != k))
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
                                        int num = myController.IsClasterOrSelected(i);

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

                                        if (justPen == selectedPen) graph.FillEllipse(justPen,
                                                             (float)(x1 + width * 0.05 + (double.Parse(params1[i].ToString()) - min1) / step1),
                                                             (float)
                                                             (y1 + height * 0.9 - height * 0.45),
                                                             radius, radius);

                                        PointF drawPoint3 = new PointF((float)(x1 + width * 0.05 + (double.Parse(params1[i].ToString()) - min1) / step1) + 4, (float)(y1 + height * 0.9 - height * 0.45) + 4);
                                        SolidBrush drawBrush1 = new SolidBrush(Color.Indigo);
                                        System.Drawing.Font drawFont2 = new System.Drawing.Font("Arial", 8, FontStyle.Regular);
                                        //graph.DrawString((i).ToString(), drawFont2, drawBrush1, drawPoint3);
                                    }
                                }
                            }
                        }
                    }
                    #endregion debilushka


                    // Create string to draw.
                    String drawString2 = name2;
                    String drawString1 = name1;   //подписи осей

                    // Create font and brush.
                    System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 16);
                    SolidBrush drawBrush = new SolidBrush(Color.Black);

                    // Create point for upper-left corner of drawing.
                    PointF drawPoint1 = new PointF(10, 0);
                    PointF drawPoint2 = new PointF((float)(pictureBox2.Width * 0.8), (float)(pictureBox2.Height * 0.9));


                    graph.DrawString(drawString1, drawFont, drawBrush, drawPoint1);
                    graph.DrawString(drawString2, drawFont, drawBrush, drawPoint2);

                }

                pictureBox2.Image = bitmap2;
            }

        }

        public void DrawPictureInFirstForm(int numRow, int numColumn)   // рисуем нашу увеличенную проекцию на этой же форме
        {
            this.numRow = numRow;
            this.numCol = numColumn;

            width2 = pictureBox2.Width - 10;
            height2 = pictureBox2.Height - 10;

            myPoints = GivePoints(myAtributs[numRow], myAtributs[numColumn], width2, height2);

            params1 = myAtributs[numRow];
            params2 = myAtributs[numColumn];

            name2 = (dataGridView1.Columns[numRow].Name);
            name1 = (dataGridView1.Columns[numColumn].Name); //подписи осей


            bitmap2 = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            graphics2 = Graphics.FromImage(bitmap2);


            RedrawMe();

            allAxes = new string[CountOfAttr];
            for (int i = 0; i < CountOfAttr; i++)
            {
                allAxes[i] = dataGridView1.Columns[i].Name;
            }

        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)   // выделяем графики _ открываем в новом окне
        {
            if (flag)
            {
                int numRow = (int)(Int32.Parse(e.X.ToString()) / (width));
                int numColumn = (int)(Int32.Parse(e.Y.ToString()) / (height));

                //  DrawPictureInSecondForm(numRow, numColumn);  // во второй форме (появляется новое окно)
                DrawPictureInFirstForm(numRow, numColumn); // в первой форме на соседнем pictureBox2
            }

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e) // смотрим над какой осью мы сейчас и выделяем ее
        {
            int x = Int32.Parse(e.X.ToString());
            int y = Int32.Parse(e.Y.ToString());

            if (flag)
            {
                int numRow = (int)(x / width);
                int numColumn = (int)(y / height);

                PlotAxes(graphics, CountOfAttr);

                int x1 = numRow * width;
                int y1 = numColumn * height;
                // x1 y1 - верхний левый угол

                var delta = (0.1 * width);
                var deltaY = (0.1 * height);
                if (numColumn < CountOfAttr && numRow < CountOfAttr)
                {
                    graphics.DrawLine(Pens.Red, new System.Drawing.PointF((float)(x1 + width - delta), y1 + height), new System.Drawing.PointF(x1, y1 + height));
                    graphics.DrawLine(Pens.Red, new System.Drawing.PointF(x1, (float)(y1 + deltaY)), new System.Drawing.PointF(x1, y1 + height));

                }

                pictureBox1.Image = btmap;
            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // MessageBox.Show(e.RowIndex.ToString() + " " + e.ColumnIndex.ToString());
        }

        private void button4_Click(object sender, EventArgs e) // снимаем выделения с точек 
        {
            myController.DisposeSelectedPoints();
            plotMatrix(myController.GiveMeListOfMyObject(), myController.GetCountObj(), myController.GetCountAttrib());
            RedrawMe();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myController = new Controller();
            myController.ReadFromExcel(dataGridView1);   // запись в датагрид из экселя + считывание в память
            listBox1.Items.Clear();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ActiveForm.Close();
            for (int o = 0; o <= 20; o++)
            {
                Thread.Sleep(30);
                this.Opacity = this.Opacity - 0.1;
            }
            Close();
        }

        private int k = 0;
        private void button2_Click_1(object sender, EventArgs e)
        {
            int rr = 0;
            foreach (var obj in myController.model.allObjects)
            {
                if (double.Parse((string)obj.attrib[4].ToString()) == k)
                {
                    myController.AddSelectedPoint(rr);
                }
                rr++;
            }

            if (myController.SelectedPoints.Count > 0)
            {
                k = 1;
                int c = myController.GetListOfClasterPoints().Count;

                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {
                    myController.AddClaster(colorDialog1.Color);
                }


                if (myController.GetListOfClasterPoints().Count > 0)
                {
                    if (f4 == null || f4.IsDisposed)
                    {
                        f4 = new Form4(
                            myController.ListOfClasters[c].GetCountOfObjects().ToString(),
                            Math.Round(myController.ListOfClasters[c].GetAverangeLengthFromCenter(), 3).ToString(),
                            Math.Round(myController.ListOfClasters[c].GetDiametr(), 3).ToString(),
                            Math.Round(myController.ListOfClasters[c].GetDispersion(), 3).ToString(),
                            myController.ListOfClasters[c].IsSgushenie(myController.GiveMeListOfMyObject()).ToString(),
                            Math.Round(myController.PartIntergroupRazbros(), 3).ToString(),
                            myController.ClasterColor[c]
                            );
                        f4.Show();
                    }
                    else
                    {
                        f4.Close();
                        f4 = new Form4(
                           myController.ListOfClasters[c].GetCountOfObjects().ToString(),
                           Math.Round(myController.ListOfClasters[c].GetAverangeLengthFromCenter(), 3).ToString(),
                           Math.Round(myController.ListOfClasters[c].GetDiametr(), 3).ToString(),
                           Math.Round(myController.ListOfClasters[c].GetDispersion(), 3).ToString(),
                           myController.ListOfClasters[c].IsSgushenie(myController.GiveMeListOfMyObject()).ToString(),
                           Math.Round(myController.PartIntergroupRazbros(), 3).ToString(),
                           myController.ClasterColor[c]
                           );
                        f4.Show();
                    }

                listBox1.Items.Add(c);
                }
                try
                {
                    RedrawMe();
                    plotMatrix(myController.GiveMeListOfMyObject(), myController.GetCountObj(),
                               myController.GetCountAttrib());
                }
                catch (Exception)
                {
                }

            }

            refreshGrids();

        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            int c = listBox1.SelectedIndex;
            if (listBox1.Items.Count > 0 && c >= 0)
            {
                if (f4 == null || f4.IsDisposed)
                {
                    f4 = new Form4(
                        myController.ListOfClasters[c].GetCountOfObjects().ToString(),
                        Math.Round(myController.ListOfClasters[c].GetAverangeLengthFromCenter(), 3).ToString(),
                        Math.Round(myController.ListOfClasters[c].GetDiametr(), 3).ToString(),
                        Math.Round(myController.ListOfClasters[c].GetDispersion(), 3).ToString(),
                        myController.ListOfClasters[c].IsSgushenie(myController.GiveMeListOfMyObject()).ToString(),
                        Math.Round(myController.PartIntergroupRazbros(), 3).ToString(),
                        myController.ClasterColor[c]
                        );

                    f4.Show();
                }
                else
                {
                    f4.Close();
                    f4 = new Form4(
                           myController.ListOfClasters[c].GetCountOfObjects().ToString(),
                           Math.Round(myController.ListOfClasters[c].GetAverangeLengthFromCenter(), 3).ToString(),
                           Math.Round(myController.ListOfClasters[c].GetDiametr(), 3).ToString(),
                           Math.Round(myController.ListOfClasters[c].GetDispersion(), 3).ToString(),
                           myController.ListOfClasters[c].IsSgushenie(myController.GiveMeListOfMyObject()).ToString(),
                           Math.Round(myController.PartIntergroupRazbros(), 3).ToString(),
                           myController.ClasterColor[c]
                           );
                    f4.Show();
                }
            /*
                 MessageBox.Show(
                "Количество объектов : " + myController.ListOfClasters[c].GetCountOfObjects().ToString() + "\n" +
                "Среднее расстояние от центра : " + myController.ListOfClasters[c].GetAverangeLengthFromCenter().ToString()  + "\n" +
                "Диаметр : " + myController.ListOfClasters[c].GetDiametr().ToString() + "\n" +
                "Дисперсия : " + myController.ListOfClasters[c].GetDispersion().ToString() + "\n" +
                "Сгущение : " + myController.ListOfClasters[c].IsSgushenie(myController.GiveMeListOfMyObject()).ToString() + "\n","Характеристики кластера");

                */
            }

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

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            isMoovingMouse = true;
            Point new_P = new Point(e.X, e.Y);
            lastPoint = new_P;
            pointsOfPolygon.Clear();
            pointsOfPolygon.Add(new_P);
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            int countOfPeresech;

            double A, B, C;
            double A1, B1, C1; // прямая на двух точках

            int count = myPoints[0].Count;

            for (int j = 0; j < count; j++)
            {
                // проверка на выделенность точки
                int num = myController.IsClasterOrSelected(j);

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
                C = -A * double.Parse(myPoints[0][j].ToString()) - B * double.Parse(myPoints[1][j].ToString());

                float dy = (float)(-C / B);

                // pictureBox2.Image = bitmap2;


                for (int i = 0; i < pointsOfPolygon.Count; i++)
                {
                    if (i == pointsOfPolygon.Count - 1)
                    {
                        A1 = pointsOfPolygon[0].Y - pointsOfPolygon[i].Y;
                        B1 = -(pointsOfPolygon[0].X - pointsOfPolygon[i].X);
                        C1 = -A1 * pointsOfPolygon[i].X - B1 * pointsOfPolygon[i].Y;
                    }
                    else
                    {
                        A1 = pointsOfPolygon[i + 1].Y - pointsOfPolygon[i].Y;
                        B1 = -(pointsOfPolygon[i + 1].X - pointsOfPolygon[i].X);
                        C1 = -A1 * pointsOfPolygon[i].X - B1 * pointsOfPolygon[i].Y;
                    }


                    PointF p1, p2, p3;

                    if ((A * B1 - A1 * B) != 0)
                    {
                        double Xp = (B * C1 - B1 * C) / (A * B1 - A1 * B);
                        double Yp = (C * A1 - C1 * A) / (A * B1 - A1 * B);

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

                            if (i + 1 == pointsOfPolygon.Count)
                            {
                                //ноль
                            }
                            else
                            {
                                double tmp1 = 0;
                                if (i + 2 > pointsOfPolygon.Count)
                                {
                                    tmp1 = A * pointsOfPolygon[1].X + B * pointsOfPolygon[1].Y + C;
                                }
                                else
                                {
                                    tmp1 = A * pointsOfPolygon[i + 2].X + B * pointsOfPolygon[i + 2].Y + C;
                                }

                                // знак этот ^
                                double tmp2 = A * pointsOfPolygon[i].X + B * pointsOfPolygon[i].Y + C;
                                // другой чем тут ^

                                if (tmp1 / Math.Abs(tmp1) != tmp2 / Math.Abs(tmp2))
                                {
                                    // прямые по разные стороны
                                    // MessageBox.Show("It's Ok!");
                                    countOfPeresech++;
                                    i++;
                                }

                            }
                        }

                        //

                        PointF Pp = new PointF((float)Xp, (float)Yp);


                        if (i == pointsOfPolygon.Count - 1)
                        {
                            p1 = new PointF(Pp.X,
                                            Pp.Y);
                            p2 = new PointF(pointsOfPolygon[i].X,
                                            pointsOfPolygon[i].Y);
                            p3 = new PointF(pointsOfPolygon[0].X,
                                            pointsOfPolygon[0].Y);
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

                if (countOfPeresech % 2 == 1)
                {
                    myController.AddSelectedPoint(j);
                    RedrawMe();
                    plotMatrix(myController.GiveMeListOfMyObject(), myController.GetCountObj(), myController.GetCountAttrib());
                }
            }
            if (isMoovingMouse) timer1.Enabled = true;
            isMoovingMouse = false;
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (name1 != name2)
            {
                if (isMoovingMouse)
                {

                    if (
                        Math.Sqrt((lastPoint.X - e.X) * (lastPoint.X - e.X) +
                                  (lastPoint.Y - e.Y) * (lastPoint.Y - e.Y)) >= radius)
                    // здесь должен быть !диаметр! моих исходных окружностей, чью принадлежность полигону я ищу
                    {
                        graphics2.FillEllipse(Brushes.Black, e.X, e.Y, 3, 3);
                        pictureBox2.Image = bitmap2;

                        pointsOfPolygon.Add(new Point(e.X, e.Y));
                        lastPoint = new Point(e.X, e.Y);
                    }

                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!isMoovingMouse)
            {
                graphics2.Clear(Color.White);
                RedrawMe();
                timer1.Enabled = false;
            }

        }

        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                for (int i = 0; i < myPoints[0].Count; i++)
                {
                    // проверка на выделенность точки
                    int num = myController.IsClasterOrSelected(i);

                    if (num == -1)
                    {
                        // выделенная все ок
                    }
                    else
                    {
                        if (num >= 0 && (e.Button != MouseButtons.Right))
                            // если кластер то не выделяем
                        {

                            if (
                                Math.Sqrt((e.X - double.Parse(myPoints[0][i].ToString()))*
                                          (e.X - double.Parse(myPoints[0][i].ToString())) +
                                          (e.Y - double.Parse(myPoints[1][i].ToString()))*
                                          (e.Y - double.Parse(myPoints[1][i].ToString()))) <= radius)
                            {

                                if (forel)
                                {
                                    RunForel(num, myController.GetNumInClaster(i), myController.GetClasterForForel(i),
                                             myController.GetClasterForForel(i).GetDiametr());

                                }


                                if (ChoiseCentoids)
                                {
                                    List<MyObject> allObjects = myController.GiveMeListOfMyObject();
                                    centroids[num] = allObjects[i];
                                    CountOfSelectedCentroids++;
                                }
                            }
                            continue;
                        }

                    }

                    if (
                        Math.Sqrt((e.X - double.Parse(myPoints[0][i].ToString()))*
                                  (e.X - double.Parse(myPoints[0][i].ToString())) +
                                  (e.Y - double.Parse(myPoints[1][i].ToString()))*
                                  (e.Y - double.Parse(myPoints[1][i].ToString()))) <= radius)
                    {
                        //MessageBox.Show(myPoints[0][i].ToString() + ' ' + myPoints[1][i]);
                        if (e.Button == MouseButtons.Right)
                        {
                            MyObject obj = myController.GiveObject(i);
                            string exitString = "";
                            for (int j = 0; j < obj.attrib.Count; j++)
                            {
                                exitString += allAxes[j] + "  : " + obj.attrib[j] + "\n";
                            }
                            MessageBox.Show(exitString);
                        }
                        else
                        {
                            if (!forel && (num < 0) && !ChoiseCentoids)
                            {
                                myController.AddSelectedPoint(i);
                                plotMatrix(myController.GiveMeListOfMyObject(), myController.GetCountObj(),
                                           myController.GetCountAttrib());

                            }
                        }
                    }
                }
                graphics2.Clear(Color.White);
                RedrawMe();
            }
            catch (Exception e2)
            {
            }
        }

        public
            void KMeans(List<MyObject> allObjects, /* int[] arrayOfClasterAndSelectedPoints, */ int countOfClaster, MyObject[] centroids)
        {
            if (centroids == null)
            {
                MessageBox.Show("выберите начальные точки");
            }
            else
            {

                int[] arrayOfClasterAndSelectedPoints = new int[allObjects.Count];
                double[] arrayOfInterval = new double[countOfClaster];
                int counter = allObjects.Count;

                for (int k = 0; k < 30; k++)
                {

                    for (int i = 0; i < counter; i++)
                    {
                        double min = Claster.GetDistanceFromObjectToCentroid(centroids[0], allObjects[i]);
                        int indexMin = 0;
                        for (int j = 0; j < countOfClaster; j++)
                        {
                            arrayOfInterval[j] = Claster.GetDistanceFromObjectToCentroid(centroids[j], allObjects[i]);
                            if (min > arrayOfInterval[j])
                            {
                                min = arrayOfInterval[j];
                                indexMin = j;
                            }
                        }
                        arrayOfClasterAndSelectedPoints[i] = indexMin;
                    }
                    // ищем новые центроиды
                    centroids = Claster.GetNewCentroids(countOfClaster, arrayOfClasterAndSelectedPoints, allObjects);
                    this.centroids = centroids;
                    // RedrawMe();
                }
                myController.arrayOfClasterAndSelectedPoints = arrayOfClasterAndSelectedPoints;
                myController.NewClasters();

                plotMatrix(myController.GiveMeListOfMyObject(), myController.GetCountObj(),
                           myController.GetCountAttrib());
                RedrawMe();
            }
        }

       
           



     private void KmeansBtn_Click(object sender, EventArgs e)
        {
            // выбор центроидов ???? где?
            lpMetrics = Int32.Parse(textBox1.Text);

            if (myController.GetNumberOfClasters() == 0)
            {
                MessageBox.Show("Кластеры не выделены");
            }
            else
            {

                List<MyObject> allObjects = myController.GiveMeListOfMyObject();
                int num = myController.GetNumberOfObjectsInClasters(); // количество центроидов и кластеров разумеется.

                int realnum = allObjects.Count - num + myController.GetNumberOfClasters();
                int counter = allObjects.Count;

                for (int i = 0; i < counter; i++)
                {
                    if (myController.IsClasterOrSelected(i) < 0)
                    {
                        myController.AddSelectedPoint(i);
                        myController.AddClaster(Color.Green);
                        centroids[myController.GetNumberOfClasters() - 1] = allObjects[i];

                        int c = myController.GetListOfClasterPoints().Count;
                        listBox1.Items.Add(c);
                    }
                }


                KMeans(allObjects, realnum, centroids);


                //обновляю список кластеров
                refreshGrids();

            }
        }

        public void refreshGrids()
        {
            if (!isRefreshedGrid)
            {
                dataGridView1.Columns.Add("кластер", "кластер");
                isRefreshedGrid = true;
            }


            int c = dataGridView1.Columns.Count;

            List<MyObject> a = myController.GiveMeListOfMyObject();
            int counter = a.Count;
            /*
            for (int j = 0; j < counter; j++) // по всем строкам   /// 1
            {
                if (myController.arrayOfClasterAndSelectedPoints[j] >= 0)
                    dataGridView1.Rows[j].Cells[c - 1].Value = myController.arrayOfClasterAndSelectedPoints[j];
                else
                    dataGridView1.Rows[j].Cells[c - 1].Value = "-";

            }*/


            
                for (int j = 0; j < counter; j++) // по всем строкам   /// 1
            {
                if (myController.IsClasterOrSelected(j) >= 0)
                    dataGridView1.Rows[j].Cells[c - 1].Value = myController.IsClasterOrSelected(j);
                else
                    dataGridView1.Rows[j].Cells[c - 1].Value = "-";

            }

        }

        private void button6_Click(object sender, EventArgs e) // выбор центроидов
        {
            myController.DisposeSelectedPoints();

            ChoiseCentoids = !ChoiseCentoids;

            int num = myController.GetNumberOfObjectsInClasters(); // количество объектов

            List<MyObject> allObjects = myController.GiveMeListOfMyObject();
            int realnum = allObjects.Count - num + myController.GetNumberOfClasters();

            num = realnum;
            centroids = new MyObject[num];
            CountOfSelectedCentroids = 0;
        }

        private void ForelBtn_Click(object sender, EventArgs e)
        {
            forel = !forel;
        }

        public void RunForel(int numOfClaster, int firstElement, Claster myClaster, double radius)
        {
            double[] radInProection = myClaster.GetDiametrInProection(numRow, numCol);  // две удаленные точки и диаметр .. всего 3 элемента в массиве
            List<ArrayList> listOfPoints = myController.GetListOfClasterPoints();
            ArrayList arrayList = listOfPoints[numOfClaster];

            //две выделенные точки
            int point1 = int.Parse(arrayList[(int)radInProection[0]].ToString());
            int point2 = int.Parse(arrayList[(int)radInProection[1]].ToString());
            // 

            int width = pictureBox2.Width - 20;
            int height = pictureBox2.Height - 20;

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
            var step1ForRad = ((width * 0.7) * (double.Parse(params1[point1].ToString()) - min1) / (max1 - min1));
            var step2 = ((max2 - min2) / (height * 0.7));
            var step2ForRad = ((height * 0.7) * (double.Parse(params2[point2].ToString()) - min2) / (max2 - min2));

            step1ForRad = ((width * 0.7) * (radius) / (max1 - min1));

            step2ForRad = ((height * 0.7) * (radius) / (max2 - min2));

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

                    if (length <= radius)
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

                graphics2.Clear(Color.White);
                RedrawMe();
                graphics2.FillEllipse(Brushes.LightPink, (float)(width * 0.05 + (xNewCenter[numRow] - min1) / step1), (float)(height * 0.9 - (xNewCenter[numCol] - min2) / step2), 9, 9);

                graphics2.DrawEllipse(Pens.LightSkyBlue, (float)(width * 0.05 - step1ForRad / 2 + (xNewCenter[numRow] - min1) / step1), (float)(height * 0.9 - step2ForRad / 2 - (xNewCenter[numCol] - min2) / step2), (float)(step1ForRad), (float)(step2ForRad));


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

        private void button5_Click(object sender, EventArgs e) // удаление списков кластеров.
        {
            try
            {
                myController.DisposeClasters();
                listBox1.Items.Clear();
                RedrawMe();
                plotMatrix(myController.GiveMeListOfMyObject(), myController.GetCountObj(), myController.GetCountAttrib());
                refreshGrids();
            }
            catch (Exception)
            {
            }

        }

        private double[] GetStepsForAllAttributs(List<MyObject> allObjects) // steps=max-min для всех атрибутов
        {
            int counterObj = allObjects.Count;
            int counterAtrr = allObjects[0].attrib.Count;

            double[] stepsArray = new double[counterAtrr];

            for (int j = 0; j < counterAtrr; j++)
            {
                double max1 = double.Parse(allObjects[0].attrib[j].ToString());
                double min1 = double.Parse(allObjects[0].attrib[j].ToString());


                for (int i = 0; i < counterObj; i++)
                {
                    if (double.Parse(allObjects[i].attrib[j].ToString()) > max1)
                    {
                        max1 = double.Parse(allObjects[i].attrib[j].ToString());
                    }
                    if (double.Parse(allObjects[i].attrib[j].ToString()) < min1)
                    {
                        min1 = double.Parse(allObjects[i].attrib[j].ToString());
                    }
                }
                stepsArray[j] = max1 - min1;
            }

            return stepsArray;

        }

        public int[] CalculationOfCountPointsInInterval() // количество точек попавших в интервал +-step
        {
            int Droblenie = Int32.Parse(textBox2.Text);

            List<MyObject> allObjects = myController.GiveMeListOfMyObject();
            int counterObj = allObjects.Count;
            int counterAtrr = allObjects[0].attrib.Count;

            double[] stepsArray = GetStepsForAllAttributs(allObjects);

            int[] counter = new int[counterObj];
            for (int i = 0; i < counterObj; i++)
            {
                counter[i] = 0;
            }

            for (int k = 0; k < counterObj; k++)
            {
                foreach (var myObject in allObjects)
                {
                    bool isInInterval = true;
                    for (int i = 0; i < counterAtrr; i++)
                    {
                        if (!(double.Parse(allObjects[k].attrib[i].ToString()) > (double.Parse(myObject.attrib[i].ToString()) - stepsArray[i] / Droblenie)
                            && double.Parse(allObjects[k].attrib[i].ToString()) < (double.Parse(myObject.attrib[i].ToString()) + stepsArray[i] / Droblenie)))
                        {
                            isInInterval = false;
                        }
                    }
                    if (isInInterval)
                    {
                        counter[k]++;
                    }
                }
            }
            return counter;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int[] countOfPointsInIntervals = CalculationOfCountPointsInInterval();
            List<MyObject> allObjects = myController.GiveMeListOfMyObject();
            int counterObj = allObjects.Count;
            int counterAtrr = allObjects[0].attrib.Count;


            graphics2.Clear(Color.White);
            RedrawMe();
            Printplotnst(countOfPointsInIntervals, counterAtrr, counterObj);
        }

        public void Printplotnst(int[] countOfPointsInIntervals, int countAttr, int countObj) // рисуем плотности (колич попаданий в интервал +- step)
        {
            int max = countOfPointsInIntervals[0];
            int min = countOfPointsInIntervals[0];

            for (int i = 0; i < countObj; i++)
            {
                if (countOfPointsInIntervals[i] > max) max = countOfPointsInIntervals[i];

                if (countOfPointsInIntervals[i] < min) min = countOfPointsInIntervals[i];
            }

            int width = pictureBox2.Width - 20;
            int height = pictureBox2.Height - 20;

            double max1 = double.Parse(params1[0].ToString());
            double min1 = double.Parse(params1[0].ToString());

            double max2 = double.Parse(params2[0].ToString());
            double min2 = double.Parse(params2[0].ToString());

            for (int i = 0; i <= countObj - 1; i++)   // ищем максимумы и минимумы 
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


            for (int i = 0; i < countObj; i++)
            {
                Color clr = Color.FromArgb(102, 255 * countOfPointsInIntervals[i] / max, 0, 255 * 1 / countOfPointsInIntervals[i]);
                Brush myBrash = new SolidBrush(clr);
                graphics2.FillEllipse(myBrash,
                                             (float)(-radius / 2 + width * 0.05 + (double.Parse(params1[i].ToString()) - min1) / step1),
                                             (float)(-radius / 2 + height * 0.9 - (double.Parse(params2[i].ToString()) - min2) / step2),
                                             2 * radius, 2 * radius);
            }

            pictureBox2.Image = bitmap2;

        }

        private void button10_Click(object sender, EventArgs e) // скрыть показать точки кластера
        {
            ClastersVisible = !ClastersVisible;
            if (ClastersVisible)
            {
                button10.Text = "Убрать точки кластера";
            }
            else
            {
                button10.Text = "Показать точки кластера";
            }
            try
            {
                graphics2.Clear(Color.White);
                RedrawMe();
                plotMatrix(myController.GiveMeListOfMyObject(), myController.GetCountObj(), myController.GetCountAttrib());
            }
            catch (Exception)
            {

            }

        }

        private NTree resultOfClasterization;

        public List<Claster> list_of_claster = new List<Claster>();  // список кластеров после иерарх кластеризации

        private void Ierarhic_Clasteriz(object sender, EventArgs e) // иерархич кластеризация (бинарный вариант)
        {
            Form3 form3=null;

            try
           {
                List<MyObject> allObjects = myController.GiveMeListOfMyObject();
                int counterObj = allObjects.Count;
                List<Claster> clasters = new List<Claster>();

                List<Claster> old_clasters = myController.ListOfClasters; // для учета старых кластеров

                foreach (var oldClaster in old_clasters)
                {
                    clasters.Add(oldClaster);
                }

                //добавление только невыделенных точек
                for (int j = 0; j < counterObj; j++)
                {
                    if (myController.IsClasterOrSelected(j) == -2 || myController.IsClasterOrSelected(j) == -1)
                    {
                        List<MyObject> listForClaster = new List<MyObject>();
                        listForClaster.Add(allObjects[j]);
                        clasters.Add(new Claster(listForClaster));
                    }
                }
                //

                //  NTree myTree = new NTree(allObjects); // все объекты как одиночные кластеры
                NTree myTree = new NTree(clasters);

                resultOfClasterization = myTree.Do_Ierarhical_Clasterization().trees[0];
                
                if (form3 == null || form3.IsDisposed)
                {
                    form3 = new Form3(list_of_claster, myController.GiveMeListOfMyObject());
                    form3.TakeTree(resultOfClasterization);
                    form3.Show();
                    form3.PlotIerarhicalVerticalDiagram(resultOfClasterization);
                }

                
                
           }
            catch (Exception r4)
            {
                MessageBox.Show(r4.ToString());
           }

        }

        private void button8_Click(object sender, EventArgs e) // обновиться!
        {
            NTree mytree = resultOfClasterization;


            //  this.button5_Click(sender,e);
            myController.DisposeSelectedPoints();
            myController.DisposeClasters();
            listBox1.Items.Clear();

            List<MyObject> allObject = myController.GiveMeListOfMyObject();

            int i = 0;
            foreach (var claster in list_of_claster)
            {
                List<MyObject> objInClaster = claster.GetObjects();
                foreach (var obj in objInClaster)
                {
                    if (allObject.IndexOf(obj) >= 0)
                    {
                        myController.AddSelectedPoint(allObject.IndexOf(obj));
                    }
                }

                Color[] colors = { Color.Red, Color.Blue, Color.Green, Color.Purple, Color.Aqua, Color.Brown, Color.LightSalmon, Color.Yellow };
                
                if (list_of_claster.Count <= 8)
                {
                    myController.AddClaster(colors[i]);
                }
                else
                {
                    Random rnd=new Random();
                    int random1 = rnd.Next(100);
                    int random2 = rnd.Next(50);
                    int random3 = rnd.Next(50,150);
                    myController.AddClaster(Color.FromArgb((int)Math.Truncate((255 * (double)random1 / 100)), 
                        (int)Math.Truncate(255 * (double)random2 / 50 * ((double)i / list_of_claster.Count)),
                        (int)Math.Truncate(255 * (double)random3 / 158 * ((double)(list_of_claster.Count - i) / list_of_claster.Count))));
                }

               
                i++;

                int c = myController.GetListOfClasterPoints().Count - 1;
                if (myController.GetListOfClasterPoints().Count > 0)
                {
                    /*
                    Form4 f4 = new Form4(
                        myController.ListOfClasters[c].GetCountOfObjects().ToString(),
                        Math.Round(myController.ListOfClasters[c].GetAverangeLengthFromCenter(), 3).ToString(),
                        Math.Round(myController.ListOfClasters[c].GetDiametr(), 3).ToString(),
                        Math.Round(myController.ListOfClasters[c].GetDispersion(), 3).ToString(),
                        myController.ListOfClasters[c].IsSgushenie(myController.GiveMeListOfMyObject()).ToString(),
                        Math.Round(myController.PartIntergroupRazbros(), 3).ToString(),
                        myController.ClasterColor[c]
                        );
                    f4.Show();*/

                    /*
                   MessageBox.Show("Количество объектов : " + myController.ListOfClasters[c].GetCountOfObjects().ToString() + "\n" +
                        "Среднее расстояние от центра : " + myController.ListOfClasters[c].GetAverangeLengthFromCenter().ToString() + "\n" +
                        "Диаметр : " + myController.ListOfClasters[c].GetDiametr().ToString() + "\n" +
                        "Дисперсия : " + myController.ListOfClasters[c].GetDispersion().ToString() + "\n" +
                        "Сгущение : " + myController.ListOfClasters[c].IsSgushenie(myController.GiveMeListOfMyObject()).ToString() + "\n", "Характеристики кластера");
                    */
                    listBox1.Items.Add(c);
                }
            }
            button3_Click(sender,e);
            refreshGrids();
            RedrawMe();
        }

        private void button9_Click(object sender, EventArgs e) // профильная диаграмма
        {
            try
            {
                if (graphics2 != null && myController.ListOfClasters.Count > 0)
                {
                    graphics2.Clear(Color.White);

                    int x1 = 0;
                    int y1 = 0;
                    //x1 y1 - верхний левый угол

                    int width = pictureBox2.Width - 10;
                    int height = pictureBox2.Height - 10;

                    var delta = (0.1 * width);
                    var deltaY = (0.05 * height);



                    // рисуем сетку
                    for (int i = 1; i <= gridstep; i++)
                    {
                        graphics2.DrawLine(Pens.Thistle,
                                           new System.Drawing.PointF((float)(x1 + width),
                                                                     (float)(y1 + height - deltaY) * i / gridstep),
                                           new System.Drawing.PointF(x1, (float)(y1 + height - deltaY) * i / gridstep));
                        graphics2.DrawLine(Pens.Thistle,
                                           new System.Drawing.PointF((float)((x1 + width) * i / gridstep),
                                                                     (float)(y1 + deltaY)),
                                           new System.Drawing.PointF((float)((x1 + width) * i / gridstep),
                                                                     (float)(y1 + height - deltaY)));
                    }
                    // конец сетка

                    graphics2.DrawLine(Pens.Black,
                                       new System.Drawing.PointF((float)(x1 + width), (float)(y1 + height - deltaY)),
                                       new System.Drawing.PointF(x1, (float)(y1 + height - deltaY))); // горизонтальная
                    graphics2.DrawLine(Pens.Black, new System.Drawing.PointF(x1, (float)(y1 + deltaY)),
                                       new System.Drawing.PointF(x1, (float)(y1 + height - deltaY))); // вертикальная



                    List<Claster> clasters = myController.ListOfClasters;

                    int count_of_atributs = myController.GiveMeListOfMyObject()[0].attrib.Count;

                    double[] minsteps = new double[count_of_atributs];
                    double[] maxsteps = new double[count_of_atributs];

                    minsteps = clasters[0].GetAverangeVector();
                    maxsteps = clasters[0].GetAverangeVector();

                    foreach (var claster in clasters)
                    {
                        double[] tmp = claster.GetAverangeVector();
                        for (int i = 0; i < count_of_atributs; i++)
                        {
                            if (minsteps[i] > tmp[i]) minsteps[i] = tmp[i];
                            if (maxsteps[i] < tmp[i]) maxsteps[i] = tmp[i];
                        }
                    }

                    double[] steps = new double[count_of_atributs];

                    for (int i = 0; i < count_of_atributs; i++)
                    {
                        steps[i] = (maxsteps[i] - minsteps[i]);

                        System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        String drawString = (dataGridView1.Columns[i].Name);
                        PointF drawPoint = new PointF((float)((double)(i) / count_of_atributs * width * 0.9 + width * 0.1),
                                                      (float)(height * 0.97));

                        graphics2.DrawString(drawString, drawFont, drawBrush, drawPoint);
                    }


                    int k = 0;
                    foreach (var claster in clasters)
                    {
                        double[] tmp = claster.GetAverangeVector();
                        Brush brush = new SolidBrush(myController.GetClasterColor(k));
                        Pen pen = new Pen(myController.GetClasterColor(k));

                        k++;
                        for (int i = 0; i < count_of_atributs; i++)
                        {
                            if (steps[i] == 0)
                            {
                                graphics2.FillEllipse(brush,
                                                      (float)((double)(i) / count_of_atributs * width * 0.9 + width * 0.1),
                                                      (float)(height * 0.5), 8, 8);
                                if (i > 0)
                                    graphics2.DrawLine(pen,
                                                       (float)((double)(i) / count_of_atributs * width * 0.9 + width * 0.1) +
                                                       4, (float)(height * 0.5) + 4,
                                                       (float)
                                                       ((double)(i - 1) / count_of_atributs * width * 0.9 + width * 0.1) + 4,
                                                       (float)(height * 0.5) + 4);

                            }
                            else
                            {
                                graphics2.FillEllipse(brush,
                                                      (float)((double)(i) / count_of_atributs * width * 0.9 + width * 0.1),
                                                      (float)
                                                      (height * 0.9 - ((tmp[i] - minsteps[i]) / steps[i]) * height * 0.8), 8, 8);
                                if (i > 0)
                                    graphics2.DrawLine(pen,
                                                       (float)((double)(i) / count_of_atributs * width * 0.9 + width * 0.1) +
                                                       4,
                                                       (float)
                                                       (height * 0.9 - ((tmp[i] - minsteps[i]) / steps[i]) * height * 0.8) + 4,
                                                       (float)
                                                       ((double)(i - 1) / count_of_atributs * width * 0.9 + width * 0.1) + 4,
                                                       (float)
                                                       (height * 0.9 -
                                                        ((tmp[i - 1] - minsteps[i - 1]) / steps[i - 1]) * height * 0.8) + 4);
                            }

                        }

                    }

                    pictureBox2.Image = bitmap2;
                }
            }
            catch (Exception)
            {
            }


        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private InputForm inputForm = null;

        private void спроецироватьВСобственноеПодпространствоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int subdem = 1;
            if ( inputForm == null || inputForm.IsDisposed )
            {
                inputForm = new InputForm( myController );
                inputForm.Show();
            } 
        
        }

        private void стандартизацияПеременныхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                myController.Standartize();
            }
            catch (Exception)
            {
                 
            }
        }

        private void данныеПоДвумГруппамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Classification pca = new Classification();

            Model md= new Model();
            md.ReadFromExcell(new DataGridView());
            double[,] tst = md.GetDataForClassification();

            Model сd = new Model();
            сd.ReadFromExcell(new DataGridView());
           
            double[,] dt = сd.GetDataForClassification();

            double[] fffffff = pca.ReturnDistanceToProjection( 
              tst ,  
                pca.Compute_eigenvectors( dt ), 
                pca.ReturnAverangeImage( dt )
                );

            MessageBox.Show("плохие \n" 
                       + fffffff[0].ToString() 
                + "\n" + fffffff[1].ToString() 

                + "\n хорошие \n" 
                + "\n" + fffffff[2].ToString()
                + "\n" + fffffff[3].ToString()

                );
        }

        private void срВзвешРасстояниеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Classification pca = new Classification();

            Model md = new Model();
            md.ReadFromExcell(new DataGridView());
            double[,] tst = md.GetDataForClassification();

            Model сd = new Model();
            сd.ReadFromExcell(new DataGridView());

            double[,] dt = сd.GetDataForClassification();


        }


    }
}
