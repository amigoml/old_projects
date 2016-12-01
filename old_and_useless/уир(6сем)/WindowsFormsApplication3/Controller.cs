using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace WindowsFormsApplication3
{
    public class Controller
    {

        public Model model = null;
        public List<Claster> ListOfClasters= new List<Claster>();
        public ArrayList SelectedPoints =new ArrayList(); // выделенные точки на данный момент
        private List<ArrayList> SelectedPointsByClasters=new List<ArrayList>(); // списки выделенных точек. для хранения всех кластеров. 
        public List<Color> ClasterColor = new List<Color>();
        public int[] arrayOfClasterAndSelectedPoints; // массив 0123... - номер кластера. -2 обычная точка. -1 выделенная.


        public Controller()
        {
             model = new Model();

             arrayOfClasterAndSelectedPoints = new int[1]; //просто
        }

        /// <summary>
        /// считываем из экселя в грид
        /// </summary>
        /// <param name="dataGridView1">куда считываем</param>
        public void ReadFromExcel(System.Windows.Forms.DataGridView dataGridView1)
        {
           model = new Model();
           model.ReadFromExcell(dataGridView1);

           arrayOfClasterAndSelectedPoints = new int[model.GetCountObj()];
           for (int i = 0; i < arrayOfClasterAndSelectedPoints.Count(); i++)
           {
               arrayOfClasterAndSelectedPoints[i] = -2;
           }
        }
     
        public List<MyObject> GiveMeListOfMyObject()
        {
            return model.allObjects;
        }

        public int GetCountObj()
        {
            return model.GetCountObj();
        }

        public int GetCountAttrib()
        {
            return model.GetCountAttrib();
        }

        public List<ArrayList> GetListOfClasterPoints()
        {
            return SelectedPointsByClasters;
        }   

      /*  public List<Claster> GetClasters()
        {
            return ListOfClasters;
        }*/

        public void AddSelectedPoint(int number)  // выделенные точки добавляем сюда.
        {
            SelectedPoints.Add(number);

            int tmp1 = -1;
            int tmp2 = -1;

            for (int i = 0; i < SelectedPoints.Count; i++)
            {
                for (int j = 0; j < SelectedPoints.Count; j++)
                {
                    if ((SelectedPoints[i].ToString() == SelectedPoints[j].ToString()) && (i != j))
                    {
                        tmp1 = i;
                        tmp2 = j;
                    }
                }
            }
            if (tmp1 != -1 && tmp2 != -1) { SelectedPoints.RemoveAt(tmp1); SelectedPoints.RemoveAt(tmp2); }


            if (arrayOfClasterAndSelectedPoints[number] == -1) // выделена - снимаем выделение
            {
                arrayOfClasterAndSelectedPoints[number] = -2;
            }
            else
            {
                if (arrayOfClasterAndSelectedPoints[number] == -2)  // не выделена -  выделяем
                {
                    arrayOfClasterAndSelectedPoints[number] = -1;
                }  
            }
            

        }

        public void DisposeSelectedPoints()       // очищаем список выделенных точек 
        {
            SelectedPoints.Clear();
        }

        public MyObject GiveObject(int i) //возвращает объект
        {
           return model.GiveObject(i);
        }

        public void AddClaster(Color color) // Добавление кластера в список кластеров!
        {
            ClasterColor.Add(color);
            if (SelectedPoints.Count != 0)//если точек 0 то нельзя добавлять
            {
                 List<MyObject> ObjectsOfClaster = new List<MyObject>();

                 #region // лично херной маюсь
                /*
                SelectedPoints.Add(0);
                SelectedPoints.Add(6);
                SelectedPoints.Add(19);
                SelectedPoints.Add(20);
                SelectedPoints.Add(28);
                SelectedPoints.Add(36);
                SelectedPoints.Add(40);
                SelectedPoints.Add(47);
                SelectedPoints.Add(51);



                SelectedPoints.Add(3);
                SelectedPoints.Add(3);
                SelectedPoints.Add(3);
                SelectedPoints.Add(3);
                SelectedPoints.Add(3);
                SelectedPoints.Add(3);
                SelectedPoints.Add(3);
                SelectedPoints.Add(3);
                SelectedPoints.Add(3);
                SelectedPoints.Add(3);
                SelectedPoints.Add(3);
                SelectedPoints.Add(3);
                */
                 #endregion 


                 foreach (var point in SelectedPoints)
                {
                    ObjectsOfClaster.Add(GiveObject(Int32.Parse(point.ToString())));
                }

                Claster claster = new Claster(ObjectsOfClaster);
            
                ListOfClasters.Add(claster);
                SelectedPointsByClasters.Add(SelectedPoints);
                SelectedPoints= new ArrayList(); // clear делать нельзя, потому что добавили в список кластеров. иначе там очистятся тоже
            }

            int counter = arrayOfClasterAndSelectedPoints.Count();
            int numer = ListOfClasters.Count - 1;

            for (int i = 0; i < counter; i++)
            {
                if (arrayOfClasterAndSelectedPoints[i] == -1)
                {
                    arrayOfClasterAndSelectedPoints[i] = numer;
                }
            }

           
        }

        public int IsClasterOrSelected(int i) // -1 если выделена, номер если кластер, -2 если нет в списках кластеров
        {
            bool isSelected = false;
            bool isClaster = false;
            int num = 0;
            for (int j = 0; j < SelectedPoints.Count; j++)
            {
                if (Int32.Parse(SelectedPoints[j].ToString()) == i)
                {
                    isSelected = true;
                    num = -1;
                    break;
                }
            }
            if (!isSelected)
            {
                int k = 0;
                foreach (var selectedPointsByClaster in SelectedPointsByClasters)
                {
                    foreach (var myPoint in selectedPointsByClaster)
                    {
                        if (Int32.Parse(myPoint.ToString()) == i)
                        {
                            isClaster = true;
                            num = k;
                            break;
                        }
                    }
                    k++;
                }
            }

            if (!(isClaster) && !(isSelected))
            {
                num = -2;
            }
           return num;
          
        }

        public Color GetClasterColor(int num)
        {
           return ClasterColor[num];
        }

        public int GetNumberOfClasters()
        {
            return ListOfClasters.Count;
        } //колич кластеров

        public int GetNumberOfObjectsInClasters()
        {
            int counter = ListOfClasters.Count;
            int res = 0;
            for (int i = 0; i < counter; i++)
            {
                res+=ListOfClasters[i].GetCountOfObjects();
            }
            return res;
        }// количество объектов во всех кластерах

        /*  public ArrayList RunForel(int numOfClaster, double Radius, Graphics graphic, ArrayList params1, ArrayList params2,int width, int height)
           {
              // ArrayList Res;

                Claster claster = ListOfClasters[numOfClaster];
               // Res = claster.sadasdsd(numOfClaster, Radius, graphic, params1, params2,  width,  height);

               return Res;
           }*/

        public Claster GetClasterForForel(int i) // дает кластер содержащий этот объект
        {
            int k = IsClasterOrSelected(i);
            Claster claster = ListOfClasters[k];
            return claster;
        }

        public int GetNumInClaster(int i) // возвращает номер элемента в кластере
        {
            int k = IsClasterOrSelected(i);
            return SelectedPointsByClasters[k].IndexOf(i);
            
        }
        
        public void NewClasters()
       {
           int countClaster = GetNumberOfClasters();
           int counter = arrayOfClasterAndSelectedPoints.Count();
           ListOfClasters= new List<Claster>();
           SelectedPointsByClasters = new List<ArrayList>();

           for (int j = 0; j < countClaster; j++)
           {
               List<MyObject> list = new List<MyObject>();
               ArrayList list2= new ArrayList();

               for (int i = 0; i < counter; i++)
               {
                   if (arrayOfClasterAndSelectedPoints[i] == j)
                   {
                       list.Add(model.GiveObject(i));
                       list2.Add(i);
                   }
               }
               ListOfClasters.Add(new Claster(list));
               SelectedPointsByClasters.Add(list2);
           }

       }

        public void DisposeClasters()
        {
            SelectedPointsByClasters.Clear();
            ClasterColor.Clear();
            ListOfClasters.Clear();

            for (int i = 0; i < arrayOfClasterAndSelectedPoints.Count(); i++)
            {
                arrayOfClasterAndSelectedPoints[i] = -2;
            }
        }

        // доля межгруппового разброса T = s1/s0

        public double PartIntergroupRazbros()
        {

            List<MyObject> allobj = GiveMeListOfMyObject();
            if (ListOfClasters.Count > 0)
            {
                double S0 = allobj.Count*ListOfClasters[0].GetSumDistanse(allobj);

                double S1 = 0;

                foreach (var claster in ListOfClasters)
                {
                    S1 += claster.IntergroupRazbros(allobj);
                }

                return S1 / S0;
            }
            else
            {
              return 0;  
            }
        }


        public void ProjectInPCA(int demension)
        {
            if (demension>0)
            model.PCA_recount(demension);
        }

        public void Standartize()
        {
            model.Standartize();
        }
        
    }
}
