using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;

namespace WindowsFormsApplication3
{
    public class Claster
    {
        // private ArrayList pointsOfClaster= new ArrayList();
        private List<MyObject> Objects= new List<MyObject>();
       
        static int lpMetrics=2;

        public Claster(List<MyObject> Objects)
        {
            this.Objects = Objects;
        }
        
        public double GetRo(double[] x1, double[] x2) // считаем расстояния между векторами
        {
            double res = 0;
              int countAtr = Objects[0].attrib.Count;
              for (int i = 0; i < countAtr; i++)
              {
                  x1[i] -= x2[i];
                  res += Math.Pow(x1[i], lpMetrics);
              }
              res = Math.Pow(res, 1.0 / lpMetrics);
            return res;
        }

        public double GetDiametr() //  
        {
            double[,] vectX = new double[Objects.Count, Objects[0].attrib.Count];
            double MaxDiametr = 0;

            int countObj = Objects.Count;
            int countAtr = Objects[0].attrib.Count;

            for (int i = 0; i < countObj; i++)
            {
                for (int j = 0; j < countAtr; j++)
                {
                    vectX[i, j] = double.Parse(Objects[i].attrib[j].ToString());
                }
            }

            double[] x1=new double[countAtr];
            double[] x2 = new double[countAtr];

            for (int i = 0; i < countObj; i++)
            {
                for (int k = 0; k < countObj; k++)
                {
                     for (int j = 0; j < countAtr; j++)   ////
                     {
                         x1[j] = vectX[i, j];   ////////////
                         x2[j] = vectX[k, j];
                     }
                     double length = GetRo(x1, x2);
                     if (length > MaxDiametr) MaxDiametr = length;
                }
                
            }
              
            return MaxDiametr;
        }

        public double GetRoInProection(double[] x1, double[] x2, int num1, int num2) // считаем расстояния между векторами в проекции
        {
            double res = 0;
            int countAtr = Objects[0].attrib.Count;
            for (int i = 0; i < countAtr; i++)
            {
                if (i == num1 || i == num2)
                {
                   x1[i] -= x2[i];
                   res += Math.Pow(x1[i], lpMetrics); 
                }
                
            }
            res = Math.Pow(res, 1.0 / lpMetrics);
            return res;
        }

        public double[] GetDiametrInProection(int num1, int num2) // для определения радиуса в проекции на эти оси
        {
            double[,] vectX = new double[Objects.Count, Objects[0].attrib.Count];
            double MaxDiametr = 0;

            int countObj = Objects.Count;
            int countAtr = Objects[0].attrib.Count;

            for (int i = 0; i < countObj; i++)
            {
                for (int j = 0; j < countAtr; j++)
                {
                    vectX[i, j] = double.Parse(Objects[i].attrib[j].ToString());
                }
            }

            double[] x1 = new double[countAtr];
            double[] x2 = new double[countAtr];
            int n1=0, n2 = 0;

            for (int i = 0; i < countObj; i++)
            {
                for (int k = 0; k < countObj; k++)
                {
                    for (int j = 0; j < countAtr; j++)   ////
                    {
                        x1[j] = vectX[i, j];   ////////////
                        x2[j] = vectX[k, j];
                    }
                    double length = GetRoInProection(x1, x2, num1,num2);
                    if (length > MaxDiametr)
                    {
                        MaxDiametr = length;
                        n1 = i;
                        n2 = k;
                    }
                }

            }
            double[] res=new double[3];
            res[0] = n1;
            res[1] = n2;
            res[2]=MaxDiametr;
           
            return res;
        } 

        public int GetCountOfObjects()
        {
            return  Objects.Count;
        }

        public float GetDispersion() // ??????  
        {
            // ,,,/ //    /  / // / / /       по отдельной переменной
            //int count = Objects[0].attrib.Count;
            //double p = 1/count;
            //double Mx = 0;
            //double Mx2 = 0;
            //double[] Dx= new double[count];

            //for (int i = 0; i < count ; i++)
            //{
            //    foreach (var myObject in Objects)
            //    {
            //         Mx += double.Parse(myObject.attrib[i].ToString())*p;
            //         Mx2 += double.Parse(myObject.attrib[i].ToString()) * double.Parse(myObject.attrib[i].ToString()) * p;
            //    }
            //    Dx[i] = Mx2 - Mx*Mx;

            //}

            //return (float)Dx[0];


            double[,] vectX = new double[Objects.Count,Objects[0].attrib.Count];
            double[] midleX = new double[Objects[0].attrib.Count];

            int countObj = Objects.Count;
            int countAtr = Objects[0].attrib.Count;

            for (int i = 0; i < countObj; i++)
            {
                for (int j = 0; j < countAtr; j++)
                {
                    vectX[i, j] = double.Parse(Objects[i].attrib[j].ToString());
                    midleX[j] += vectX[i, j];
                }
            }

            for (int i = 0; i < countAtr; i++)
            {
                midleX[i] = midleX[i]/countObj;
            }


            double[,] D = new double[countObj,countAtr];

            for (int i = 0; i < countObj; i++)
            {
                for (int j = 0; j < countAtr; j++)
                {
                    D[i,j] += vectX[i, j] - midleX[j]; 
                }
            }

            double[] NormsD= new double[countObj];

            for (int i = 0; i < countObj; i++)
            {
                NormsD[i] = 0;
                for (int j = 0; j < countAtr; j++)
                {
                    NormsD[i] += Math.Pow(D[i, j], lpMetrics); // считаем квадрат нормы вектора
                }
            }

            double DResult = 0;
            for (int i = 0; i < countObj; i++)
            {
                DResult += NormsD[i];
            }
            DResult = DResult/countObj;

            return (float) DResult;

        }

        public double[] GetAverangeVector() // среднее значение кластера по проекциям для профильной диаграммы
        {
            int countObj = Objects.Count;
            int countAtr = Objects[0].attrib.Count;
            double[,] vectX = new double[countObj, countAtr];
            double[] midleX = new double[countAtr];

            for (int i = 0; i < countObj; i++)
            {
                for (int j = 0; j < countAtr; j++)
                {
                    vectX[i, j] = double.Parse(Objects[i].attrib[j].ToString());
                    midleX[j] += vectX[i, j];
                }
            }

            for (int i = 0; i < countAtr; i++)
            {
                midleX[i] = midleX[i] / countObj;
            }

            return midleX;
        }

        public double GetAverangeLengthFromCenter()
        {
            int countObj = Objects.Count;
            int countAtr = Objects[0].attrib.Count;
            double AverangeLength = 0;
            double[,] vectX = new double[countObj, countAtr];
            double[] midleX = new double[countAtr];

            for (int i = 0; i < countObj; i++)
            {
                for (int j = 0; j < countAtr; j++)
                {
                    vectX[i, j] = double.Parse(Objects[i].attrib[j].ToString());
                    midleX[j] += vectX[i, j];
                }
            }

            for (int i = 0; i < countAtr; i++)
            {
                midleX[i] = midleX[i] / countObj;
            }

            double[] x1 = new double[countAtr];
           
            for (int i = 0; i < countObj; i++)
            {
                for (int j = 0; j < countAtr; j++)
                {
                    x1[j] = vectX[i, j];  ////////////////////
                }   
                
                double length = GetRo(x1, midleX);
                AverangeLength += length;
             
            }

            return AverangeLength/countObj;
        }

        public List<MyObject> GetObjects()
        {
            return Objects;
        }

     /*   public ArrayList sadasdsd(int number, double R, Graphics graphic, ArrayList params1, ArrayList params2, int width, int height) // будем смотреть какие точки лежат не дальше R от одной из точек.
        {
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

            var step2 = ((max2 - min2) / (height * 0.7));
            //////////////////////////////////////////////////////////////// painting



            int countObj = Objects.Count;
            int countAtr = Objects[0].attrib.Count;

            double[,] vectX = new double[countObj, countAtr];

            for (int i = 0; i < countObj; i++)
            {
                for (int j = 0; j < countAtr; j++)
                {
                    vectX[i, j] = double.Parse(Objects[i].attrib[j].ToString());
                }
            }

            double[] xCenter = new double[countAtr];  //  центр масс.

            for (int j = 0; j < countAtr; j++)
            {
                xCenter[j] = vectX[number, j]; 
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
                int num= ObjInSphere.Count;
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

                    double length = GetRo(ArrayX, xCenter);

                    if (length <= R)
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
                
                graphic.DrawEllipse(Pens.LightPink, (float)( width * 0.05 + (xNewCenter[0] - min1) / step1),(float)( height * 0.9 - (xNewCenter[1] - min2) / step2), 5, 5);

                // отмечаем новый цм
                //рисуем круг радиусом соответствующим

                bool flag = false;
                for (int i = 0; i < countAtr; i++)
                {
                    if (xNewCenter[i]!= xCenter[i])
                    {
                        flag = true;
                    }
                }
                flagForCycle = flag;

                if(flagForCycle) xCenter = xNewCenter;
            }

            return ObjInSphere;
        }

        */

        static public MyObject GetCenterOfMassInThisClaster(List<MyObject> claster) // цм кластера
        {
            int counter1 = claster.Count;
            int counter2 = claster[0].attrib.Count;
            ArrayList newAtribs = new ArrayList();
            for (int j = 0; j < counter2; j++)
            {
                double sumOfArtib = 0;
                for (int i = 0; i < counter1; i++)
                {
                    sumOfArtib += double.Parse(claster[i].attrib[j].ToString());
                }
                newAtribs.Add(sumOfArtib / counter1);
            }
            MyObject res = new MyObject(newAtribs);
            return res;
        }  //1

        static public double GetDistanceFromObjectToCentroid(MyObject obj1, MyObject obj2)
        {
            int count = obj1.attrib.Count;
            ArrayList abtribsDelta = new ArrayList();
            for (int i = 0; i < count; i++)
            {
                abtribsDelta.Add(double.Parse(obj1.attrib[i].ToString()) - double.Parse(obj2.attrib[i].ToString()));
            }
            double res = 0;
            for (int i = 0; i < count; i++)
            {
                res += Math.Pow(double.Parse(abtribsDelta[i].ToString()), lpMetrics);
            }
            res = Math.Pow(res, 1.0 / lpMetrics);
            return res;

        }  //2

        public double GetMaxSquareDistance(List<MyObject> Objects)
        {
            double max = 0;
            MyObject centroid = GetCenterOfMassInThisClaster(Objects);
            foreach (var myObject in Objects)
            {
                double dist = GetDistanceFromObjectToCentroid(myObject, centroid);
                if (dist > max)
                {
                    max = dist;
                }
            }
            return max;
        }  // 3   все это для поиска сгущения

        public double GetSumDistanse(List<MyObject> Objects)
        {
            double dist = 0;
            MyObject centroid = GetCenterOfMassInThisClaster(Objects);
            foreach (var myObject in Objects)
            {
                dist += Math.Pow(GetDistanceFromObjectToCentroid(myObject, centroid), lpMetrics);
                
            }
            return dist / Objects.Count;
        }

        public bool IsSgushenie(List<MyObject> Objects)
        {
            bool flag = false;
            double midD_OfAllPoints = GetSumDistanse(Objects);
            double midDOfClaster = Math.Pow(GetMaxSquareDistance(this.Objects), lpMetrics);
            if (midD_OfAllPoints > midDOfClaster)
            {
                flag = true;
            }
            return flag;
        }

        public double IntergroupRazbros(List<MyObject> Objects) // для подсчета доли межгруппового разброса (на вход все объекты)
        {
            double dist = 0;
            MyObject centroid = GetCenterOfMassInThisClaster(Objects);
            MyObject centroidOfClaster = GetCenterOfMassInThisClaster(this.Objects);
            dist = this.Objects.Count*Math.Pow(GetDistanceFromObjectToCentroid(centroidOfClaster, centroid), 2);
            return dist;
        }

        public double GetAverangeIntergroupDistanceBetweenClasters(List<MyObject> Objects1, List<MyObject> Objects2) // среднее межгрупповое расстояние для иерархической кластеризации
        {
            int counter1 = Objects1.Count;
            int counter2 = Objects2.Count;
            int count = Objects1[0].attrib.Count;
            double sum = 0; // храним сумму расстояний между точками

            for (int i = 0; i < counter1; i++)
            {
                for (int j = 0; j < counter2; j++)
                {
                    ArrayList abtribsDelta = new ArrayList();
                    for (int k = 0; k < count; k++)
                    {
                        abtribsDelta.Add(double.Parse(Objects1[i].attrib[k].ToString()) -
                                         double.Parse(Objects2[j].attrib[k].ToString()));
                    }
                    double res = 0;
                    for (int c = 0; c < count; c++)
                    {
                        res += Math.Pow(double.Parse(abtribsDelta[c].ToString()), lpMetrics);
                    }
                    res = Math.Pow(res, 1.0 / lpMetrics);
                    sum += res;
                }
            }

           

            return sum/(counter1*counter2);
        }

        //методы из первой формы Form1.cs
       static public MyObject[] GetNewCentroids(int countOfClaster, int[] arrayOfClasterAndSelectedPoints, List<MyObject> allObjects) // массив центроидов кластеров
        {
            MyObject[] newCentroids = new MyObject[countOfClaster];
            int counter = allObjects.Count;

            for (int j = 0; j < countOfClaster; j++)
            {
                List<MyObject> claster = new List<MyObject>();
                for (int i = 0; i < counter; i++)
                {
                    if (arrayOfClasterAndSelectedPoints[i] == j)
                    {
                        claster.Add(allObjects[i]);
                    }
                }
                newCentroids[j] = GetCenterOfMassInThisClaster(claster);
            }
            return newCentroids;
        }
        

    }

}
