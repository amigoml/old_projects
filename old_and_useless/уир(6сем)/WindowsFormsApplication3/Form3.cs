using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Form3 : Form
    {
        public Form3(List<Claster> list_of_claster,List<MyObject> allobjects)
        {
            InitializeComponent();
            this.list_of_claster = list_of_claster;
            this.allobjects = allobjects;
        }

        private List<MyObject> allobjects;
        private double step1;
        private double step2;
        private int width;
        private int height;
        private Bitmap bitmap;
        private Graphics graphics;
        private NTree myTree;

        private const double  myCoefficient=0.95;

        private void Form3_Load(object sender, EventArgs e)
        {
             width = pictureBox1.Width-10 ;
             height = pictureBox1.Height -10;


             bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
             graphics = Graphics.FromImage(bitmap);

            
        }

        public void TakeTree(NTree myNewTree)
        {
            myTree = myNewTree;
        }

        Pen pen = new Pen(Brushes.Black);


        public PointF recursivePlotting(NTree tree)
        {
            PointF res = new PointF(0, 0);

            foreach (var my_tree in tree.trees)
            {
                if (my_tree.trees.Count == 0 && my_tree.claster.GetObjects().Count > 1)
                {
                    double distanseForObject = myTree.GetDistanseInFirstClasterization(my_tree.claster.GetObjects()[0]);

                    // горизонтальная линия
                    graphics.DrawLine(pen,
                                     (float)(0.5 / step2 + (myTree.claster.GetObjects().IndexOf(my_tree.claster.GetObjects()[0])) / step2),
                                     (float)(height * myCoefficient),
                                     (float)(0.5 / step2 + ((myTree.claster.GetObjects().IndexOf(my_tree.claster.GetObjects()[my_tree.claster.GetObjects().Count - 1]))) / step2),
                                     (float)(height * myCoefficient));

                    float x = (float)(0.5 * ((myTree.claster.GetObjects().IndexOf(my_tree.claster.GetObjects()[0])) / step2 + (myTree.claster.GetObjects().IndexOf(my_tree.claster.GetObjects()[my_tree.claster.GetObjects().Count - 1])) / step2));
                   
                    // вертикальная линия
                    graphics.DrawLine(pen,
                                      (float)(0.5 / step2 + x),
                                      (float)(height * myCoefficient),
                                      (float)(0.5 / step2 + x),
                                      (float)(height * myCoefficient - distanseForObject / step1));

                    // запоминаем точки вертик линии
                    my_tree.TakePoint(new PointF((float)(0.5 / step2 + x), 
                                  (float)(height * myCoefficient - distanseForObject / step1)), 
                        new PointF((float)(0.5 / step2 + x),
                                    (float)(height * myCoefficient)));


                    if (res == new PointF(0, 0))
                    {
                        res.X = (float)(0.5 / step2 + x);
                        res.Y = (float)(height * myCoefficient - distanseForObject / step1);
                    }
                    else // соединяем два кластера
                    {
                        graphics.DrawLine(pen, res.X, res.Y,
                                        (float)(0.5 / step2 + x), (float)(height * myCoefficient - distanseForObject / step1));
                        res.X =
                            ((float)
                             ((0.5 / step2 + x) + res.X) / 2);
                        res.Y = (float)(height * myCoefficient - distanseForObject / step1);
                    }
                }
                else
                {


                    if (my_tree.claster.GetObjects().Count == 1)
                    {
                        double distanseForObject = myTree.GetDistanseInFirstClasterization(my_tree.claster.GetObjects()[0]);

                        graphics.DrawLine(pen,
                                          (float)(0.5 / step2 + (myTree.claster.GetObjects().IndexOf(my_tree.claster.GetObjects()[0])) / step2),
                                          (float)(height * myCoefficient),
                                          (float)(0.5 / step2 + ((myTree.claster.GetObjects().IndexOf(my_tree.claster.GetObjects()[0]))) / step2),
                                          (float)(height * myCoefficient - distanseForObject / step1));
                        // запоминаем точки вертик линии
                        my_tree.TakePoint(new PointF((float)(0.5 / step2 + ((myTree.claster.GetObjects().IndexOf(my_tree.claster.GetObjects()[0]))) / step2),
                                          (float)(height * myCoefficient - distanseForObject / step1)),
                                          new PointF((float)(0.5 / step2 + (myTree.claster.GetObjects().IndexOf(my_tree.claster.GetObjects()[0])) / step2),
                                          (float)(height * myCoefficient)));


                        if (res == new PointF(0, 0))
                        {
                            res.X =(float)(0.5 / step2 +((myTree.claster.GetObjects().IndexOf(my_tree.claster.GetObjects()[0]))) / step2);
                            res.Y = (float)(height * myCoefficient - distanseForObject / step1);
                        }
                        else // соединяем два одиночных кластера
                        {
                            graphics.DrawLine(pen, res.X, res.Y,
                                              (float)
                                              (0.5 / step2 +
                                               ((myTree.claster.GetObjects().IndexOf(my_tree.claster.GetObjects()[0]))) /
                                               step2), (float)(height * myCoefficient - distanseForObject / step1));
                            res.X =
                                ((float)
                                 (0.5 / step2 +
                                  ((myTree.claster.GetObjects().IndexOf(my_tree.claster.GetObjects()[0]))) / step2) +
                                 res.X) / 2;
                            res.Y = (float)(height * myCoefficient - distanseForObject / step1);
                        }
                    }
                    else // в кластере не 1 объект
                    {
                        PointF midlepoint = recursivePlotting(my_tree);
                        if (res == new PointF(0, 0))
                        {
                            res = midlepoint;
                        }
                        else
                        {
                            double distanseForClaster = tree.distance;
                           //гориз линия
                            graphics.DrawLine(pen, res.X, (float)(height * myCoefficient - distanseForClaster / step1), midlepoint.X, (float)(height * myCoefficient - distanseForClaster / step1));
                   
                            //2 верт линии левая и правая
                            graphics.DrawLine(pen, res.X, (float)(height * myCoefficient - distanseForClaster / step1), res.X, res.Y);
                            // запоминаем точки вертик линии
                        //    my_tree.trees[0].TakePoint(new PointF(res.X, (float)(height * myCoefficient - distanseForClaster / step1)), res);

                            graphics.DrawLine(pen, midlepoint.X, (float)(height * myCoefficient - distanseForClaster / step1), midlepoint.X, midlepoint.Y);
                            // запоминаем точки вертик линии
                       //     my_tree.trees[1].TakePoint(new PointF(midlepoint.X, (float)(height * myCoefficient - distanseForClaster / step1)), midlepoint);

                            NTree myUppertree = GetTreeWhereExist(myTree, my_tree); // дерево в котором это дерево - поддерево

                            // запоминаем точки вертик линии - если только не одиночный объект, потому что distanseForClaster=0
                            if (myUppertree.trees[0].claster.GetObjects().Count > 1 && myUppertree.trees[0].trees.Count>0)
                            myUppertree.trees[0].TakePoint(new PointF(res.X, (float)(height * myCoefficient - distanseForClaster / step1)), res);
                            if (myUppertree.trees[1].claster.GetObjects().Count > 1 && myUppertree.trees[1].trees.Count > 0)
                            myUppertree.trees[1].TakePoint(new PointF(midlepoint.X, (float)(height * myCoefficient - distanseForClaster / step1)), midlepoint);



                            res.X = (res.X + midlepoint.X) / 2;
                            res.Y = (float)(height * myCoefficient - distanseForClaster / step1);
                        }
                    }
                }


            }
            return res;
        }

        public NTree GetTreeWhereExist(NTree my_tree, NTree tree) // ищем корень этого листа (дерево к котором это дерево - лист)
        {
            NTree res = null;
            foreach (var is_thisTree in my_tree.trees)
            {
                if (is_thisTree == tree)
                {
                    res = my_tree;
                }
                if (res == null)
                {
                    res=GetTreeWhereExist(is_thisTree, tree);
                }
            }

            return res;
        }

        public void PlotIerarhicalVerticalDiagram(NTree myTree)
        {
            double max = myTree.distance;
            double min = 0;

            step1 = (max - min) / (height * myCoefficient);
            step2 = (double)(myTree.claster.GetObjects().Count) / width;

            int i = 0;

            PointF newPoint = recursivePlotting(myTree);

            foreach (var objects in myTree.claster.GetObjects())
            {
                Font drawFont = new Font("Arial", 10, FontStyle.Regular);
                SolidBrush drawBrush = new SolidBrush(Color.Blue);

                PointF drawPoint = new PointF((float)(0.5 / step2 + (i) / step2)-5, (float)(height * myCoefficient));

                graphics.DrawString((allobjects.IndexOf(objects)+1).ToString(), drawFont, drawBrush, drawPoint);

                i++;
            }

            pictureBox1.Image = bitmap;
        }

        public void WriteNubersObj()
        {
            double max = myTree.distance;
            double min = 0;

            step1 = (max - min) / (height * myCoefficient);
            step2 = (double)(myTree.claster.GetObjects().Count) / width;

            int i = 0;

            foreach (var objects in myTree.claster.GetObjects())
            {
                Font drawFont = new Font("Arial", 10, FontStyle.Regular);
                SolidBrush drawBrush = new SolidBrush(Color.Blue);

                PointF drawPoint = new PointF((float)(0.5 / step2 + (i) / step2-5), (float)(height * myCoefficient));

                graphics.DrawString((allobjects.IndexOf(objects)+1).ToString(), drawFont, drawBrush, drawPoint);

                i++;
            }

        }

        private double x_coord_Of_Line;
        private double y_coord_Of_Line;
        private double dist_for_clasters;
        private bool line_is_create = false;
        private bool is_mouse_down = false;

        private void button1_Click(object sender, EventArgs e)
        {
            line_is_create = true;
            y_coord_Of_Line = (pictureBox1.Height * myCoefficient / 2);
        }

        private void drawLine() //трассирующая линия
        {
            if (del_vert_Lines) // удаляем линии
            {
                graphics.Clear(Color.White);
                RedrawVertClasteriz(myTree);
                graphics.DrawLine(Pens.Red, 0, (float) y_coord_Of_Line, pictureBox1.Width, (float) y_coord_Of_Line);
                dist_for_clasters = (pictureBox1.Height*myCoefficient - y_coord_Of_Line - 10)*step1;
                textBox1.Text = dist_for_clasters.ToString();
                pictureBox1.Image = bitmap;
            }
            else
            {
                graphics.Clear(Color.White);
                PlotIerarhicalVerticalDiagram(myTree);
               // RedrawVertClasteriz(myTree);
                graphics.DrawLine(Pens.Black, 0, (float)y_coord_Of_Line, pictureBox1.Width, (float)y_coord_Of_Line);
                dist_for_clasters = (pictureBox1.Height * myCoefficient - y_coord_Of_Line-10) * step1;
                textBox1.Text = dist_for_clasters.ToString();
                pictureBox1.Image = bitmap; 

            }


            
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            is_mouse_down = true;
            if (del_vert_Lines) // удаляем линии
            {
                x_coord_Of_Line = e.X;
                y_coord_Of_Line = e.Y;

                graphics.Clear(Color.White);

                DelLine(myTree, new PointF((float)x_coord_Of_Line, (float)y_coord_Of_Line));

                graphics.Clear(Color.White);
                RedrawVertClasteriz(myTree);
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            is_mouse_down = false;
           
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (is_mouse_down && line_is_create)
            {
                y_coord_Of_Line = e.Y;
                drawLine();
            }

        }


        private bool del_vert_Lines = false;
        private List<Claster> list_of_claster;
        
        

        private void button2_Click(object sender, EventArgs e)
        {
            del_vert_Lines = true;
            list_of_claster.Add(myTree.claster);
           /* graphics.Clear(Color.White);
            RedrawVertClasteriz(myTree);
            pictureBox1.Image = bitmap;*/
        }

        private void RedrawVertClasteriz(NTree mytree)
        {
            WriteNubersObj();

            foreach (var tree in mytree.trees)
            {
                if (tree.is_in_big_claster)
                {
                    graphics.DrawLine(pen, tree.topPoint.X, tree.topPoint.Y, tree.bottomPoint.X,
                                      tree.bottomPoint.Y);
                }

                
                if (mytree.trees.Count > 1 && mytree.trees[0].is_in_big_claster && mytree.is_in_big_claster)
                    graphics.DrawLine(pen, mytree.trees[0].bottomPoint.X, mytree.trees[0].bottomPoint.Y, (mytree.trees[1].bottomPoint.X + mytree.trees[0].bottomPoint.X) / 2, mytree.trees[1].bottomPoint.Y);
                if (mytree.trees.Count > 1 && mytree.trees[1].is_in_big_claster && mytree.is_in_big_claster)
                    graphics.DrawLine(pen, (mytree.trees[1].bottomPoint.X + mytree.trees[0].bottomPoint.X) / 2, mytree.trees[0].bottomPoint.Y, mytree.trees[1].bottomPoint.X, mytree.trees[1].bottomPoint.Y);
                if (mytree.trees.Count > 1 && mytree.trees[1].is_in_big_claster && mytree.trees[0].is_in_big_claster)
                    graphics.DrawLine(pen, mytree.trees[0].bottomPoint.X, mytree.trees[0].bottomPoint.Y, mytree.trees[1].bottomPoint.X, mytree.trees[1].bottomPoint.Y);


                RedrawVertClasteriz(tree);
            }
            pictureBox1.Image = bitmap;
        }

        private void DelLine(NTree mytree, PointF point)
        {
            foreach (var tree in mytree.trees)
            {
                if (tree.topPoint.X + 2 > point.X && tree.topPoint.X - 2 < point.X && tree.bottomPoint.Y < point.Y &&
                    tree.topPoint.Y > point.Y)
                {
                    tree.is_in_big_claster = false;
                    list_of_claster.Add(tree.claster);
                    UpdateListOfClaster();
                }
                else
                {
                    DelLine(tree, point);
                }

                if (!mytree.trees[1].is_in_big_claster && !mytree.trees[0].is_in_big_claster)
                {
                    mytree.is_in_big_claster = false;
                    list_of_claster.Remove(mytree.claster);
                }
            }
        }

        private void DelLineInTrassir(NTree mytree, PointF point) // удаление трассировкой(без учета коорд Х)
        {
            foreach (var tree in mytree.trees)
            {
                if ( tree.bottomPoint.Y < point.Y &&  tree.topPoint.Y > point.Y)
                {
                    tree.is_in_big_claster = false;
                    list_of_claster.Add(tree.claster);
                    UpdateListOfClaster();
                }
                else
                {
                    DelLineInTrassir(tree, point);
                }

                if (!mytree.trees[1].is_in_big_claster && !mytree.trees[0].is_in_big_claster)
                {
                    mytree.is_in_big_claster = false;
                    list_of_claster.Remove(mytree.claster);
                }
            }
        }

        public void UpdateListOfClaster()   // не все удаляет.. удаление одинаковых ветвей.
        {

            List<Claster> tmp_for_del = new List<Claster>();
            List<Claster> tmp = new List<Claster>();

            bool flag = true;
            int countOfclaster = list_of_claster.Count;
            
            
            for (int i = 0; i < countOfclaster; i++)
                    {
                        for (int j = i+1; j < countOfclaster; j++)
                        {
                            foreach (var object1 in list_of_claster[i].GetObjects())
                            {
                                foreach (var object2 in list_of_claster[j].GetObjects())
                                {
                                    if (object1 == object2 && list_of_claster[i] != list_of_claster[j])
                                    {
                                        if (list_of_claster[i].GetObjects().Count < list_of_claster[j].GetObjects().Count)
                                        {
                                            //list_of_claster.Remove(claster1);
                                            if (flag)
                                            {
                                                tmp_for_del.Add(list_of_claster[j]);
                                                tmp.Add(list_of_claster[i]);
                                                flag = false;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            // list_of_claster.Remove(claster);
                                            if (flag)
                                            {
                                                tmp_for_del.Add(list_of_claster[i]);
                                                tmp.Add(list_of_claster[j]);
                                                flag = false;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            flag = true;
                        
                   
                }
            }

            int counter = tmp.Count;
            for (int i = 0; i < counter; i++)
            {
                List<MyObject> list_Of_Obj = tmp_for_del[i].GetObjects();
                List<MyObject> list_Of_Obj1 = tmp[i].GetObjects();
                List<MyObject> new_list = new List<MyObject>();
                foreach (var myObject in list_Of_Obj)
                {
                   // list_Of_Obj.Remove(myObject);
                    if (list_Of_Obj1.IndexOf(myObject) < 0)
                    {
                        new_list.Add(myObject);
                    }
                }
                if (new_list.Count != 0)
                {
                    Claster cl = new Claster(new_list);
                    if (list_of_claster.IndexOf(cl) < 0) list_of_claster.Add(new Claster(new_list));
                }
                
                list_of_claster.Remove(tmp_for_del[i]);
                
            }


        }

        private void button3_Click(object sender, EventArgs e) // трассировкой удаление
        {
            if (del_vert_Lines) // удаляем линии
            {
                graphics.Clear(Color.White);

                DelLineInTrassir(myTree, new PointF((float)x_coord_Of_Line, (float)y_coord_Of_Line));

                graphics.Clear(Color.White);
                RedrawVertClasteriz(myTree);
            }
        }

       





       

    }
}
