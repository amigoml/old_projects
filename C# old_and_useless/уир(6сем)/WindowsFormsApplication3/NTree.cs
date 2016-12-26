using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication3
{
    public class NTree
    {
        public double distance;
        public Claster claster;
        public List<NTree> trees;


        private List<Claster> list_of_clasters;

        // удаление вертикальных линий
        public PointF topPoint;
        public PointF bottomPoint;
        public bool is_in_big_claster = true;   //показывает отделен ли потомок от единого кластера(было в одиночном удалении очень нужно)
        //end удаление вертикальных линий

       public NTree(double new_distance, Claster new_claster)
       {
           distance = new_distance;
           claster = new_claster;
           trees = new List<NTree>();
       }

       public NTree(List<MyObject> allObjects)
       { 
            trees = new List<NTree>();
            foreach (var myObject in allObjects)
            {
                List<MyObject> listForClaster=new List<MyObject>();
                listForClaster.Add(myObject);
               // clasters.Add(new Claster(listForClaster));
                trees.Add(new NTree(0, new Claster(listForClaster)));
            }
           distance = 0;
       }

       public NTree(List<Claster> clasters)
       {
           trees = new List<NTree>();
           foreach (var claster in clasters)
           {
               trees.Add( new NTree(0,claster) );
           }
           distance = 0;
       }

       public Claster JoinClasters(Claster cl1, Claster cl2)
       {
           List<MyObject> l1 = new List<MyObject>();
           if (cl1 != null)
           {
                l1 = cl1.GetObjects();
           }
           List<MyObject> l2 = cl2.GetObjects();
        
           List<MyObject> l3= new List<MyObject>();
           if (cl1 != null)
           {
               foreach (var myObject in l1)
               {
                   l3.Add(myObject);
               }
           }
           foreach (var myObject in l2)
           {
               l3.Add(myObject);
           }
           return new Claster(l3);

       }

       public NTree Do_Ierarhical_Clasterization()
        {
            int counter;
            while (trees.Count > 1)
            {

                counter = trees.Count;
                double[,] distance = new double[counter,counter];
                double minDist = double.MaxValue;  //!!!!!!!!!!!!!!!!!!!!!!!!! TODO error was here because int less than double
                int tmp = 0;

                for (int i = 0; i < counter; i++)
                {
                    for (int j = i + 1; j < counter; j++)
                    {
                        if (i != j)
                        {
                          //  MyObject obj1 = Claster.GetCenterOfMassInThisClaster(trees[i].claster.GetObjects());
                            //MyObject obj2 = Claster.GetCenterOfMassInThisClaster(trees[j].claster.GetObjects());
                           // distance[i, j] = Claster.GetDistanceFromObjectToCentroid(obj1, obj2);
                            if (trees[j].claster != null && trees[i].claster!=null) 
                                distance[i, j] = trees[j].claster.GetAverangeIntergroupDistanceBetweenClasters(
                                    trees[i].claster.GetObjects(), trees[j].claster.GetObjects()
                                    );
                            // теперь без самопересечений
                            if (minDist > distance[i, j])
                            {
                                minDist = distance[i, j];
                                tmp = i;
                            }
                        }
                    }
                }

                Claster new_claster = null;
                NTree new_tree = null;
                int[] nums = new int[trees.Count]; // смотрим какие кластеры на этом шаге попали в объединение

              //  for (int k = 0; k < counter; k++)
              //  {
                int k = tmp;
                    for (int j = k+1; j < counter; j++)
                    {
                        if (minDist == distance[k, j])
                        {
                            nums[k] = 1;
                            nums[j] = 1;
                            break; // для бинарности добавления
                        }
                    }
             //   }

                new_tree = new NTree(minDist, new_claster);
                for (int i = 0; i < counter; i++)
                {
                    if (nums[i] == 1)
                    {
                        new_claster = JoinClasters(new_claster, trees[i].claster);
                        new_tree.trees.Add(trees[i]);
                    }
                }
                /////
                if (new_claster == null) System.Windows.Forms.MessageBox.Show("axaxaxax!");
                //////
                

                new_tree.claster = new_claster;
                for (int i = counter-1; i >= 0; i--)
                {
                    if (nums[i] == 1)
                    {
                        trees.Remove(trees[i]);
                    }
                }
                if (new_tree != null && new_tree.claster!=null) //на нуль проверку сделал
                {
                     trees.Add(new_tree);
                }

                ////
                foreach (var tree in trees)
                {
                    if (tree.claster == null)
                    {
                        System.Windows.Forms.MessageBox.Show("axaxaxax!");
                    }
                }
                /////
                this.distance = minDist;
            }
            return this;
        }

       public double GetDistanseInFirstClasterization(MyObject obj) // расстояние на котором объект вошел в кластер 
        {
            NTree searchingTree = this;
            double result = 0;
            while (searchingTree.claster.GetObjects().Count != 1 && searchingTree.trees.Count!=0 )
            {
                foreach (var nTree in searchingTree.trees)
                {
                    if (nTree.claster.GetObjects().IndexOf(obj) >= 0)
                    {
                        result = searchingTree.distance;
                        searchingTree = nTree;
                        break;
                    }
                }
            }
            return result;
        }
       
       public Claster GetNeighborhoods(MyObject obj)
        {
            NTree searchingTree = this;
            Claster result = null;
            while (searchingTree.claster.GetObjects().Count != 1)
            {

                foreach (var nTree in searchingTree.trees)
                {
                    if (nTree.claster.GetObjects().IndexOf(obj) >= 0)
                    {
                        result = searchingTree.claster;
                        searchingTree = nTree;
                        break;
                    }
                }
            }
            return result;
        }

       public void TakePoint(PointF point1, PointF point2) // присвоение точек соед вертик линий
        {
            topPoint=point2;
            bottomPoint=point1;
        }

       public List<Claster> GetMeClasters()
        {
            list_of_clasters=new List<Claster>();
            NTree searchingTree = this;
           
            return list_of_clasters;
        }

       
       
         


    }


}
