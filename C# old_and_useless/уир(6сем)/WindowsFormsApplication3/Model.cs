using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;


namespace WindowsFormsApplication3
{
    public class Model
    {
        
        private int countObj = 0;  // колич атрибутов! 
        private int countAttrib = 0; // колич объектов

        public List<MyObject> allObjects = new List<MyObject>();  // список наших объектов

        public void Standartize()
        {
            double[,] data = new double[allObjects.Count, allObjects[0].attrib.Count];
            for (int i = 0; i < allObjects.Count; i++)
            {
                for (int j = 0; j < allObjects[i].attrib.Count; j++)
                {
                    data[i, j] = double.Parse((string)allObjects[i].attrib[j].ToString());
                }
            }


            PCA pca_obj = new PCA();

            data = pca_obj.Standartize(data);

            List<MyObject> MyRecountingObjects = new List<MyObject>();

            for (int i = 0; i < data.GetLength(0); i++)
            {
                ArrayList attr = new ArrayList();
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    attr.Add(data[i, j]);
                }
                MyObject obj = new MyObject(attr);
                MyRecountingObjects.Add(obj);

            }
            allObjects = MyRecountingObjects;

        }

        public void PCA_recount(int demension)
        {

            double[,] data=new double[allObjects.Count,allObjects[0].attrib.Count];
            for (int i = 0; i < allObjects.Count; i++)
            {
                for (int j = 0; j < allObjects[i].attrib.Count; j++)
                {
                    data[i, j] =  double.Parse((string) allObjects[i].attrib[j].ToString());
                }
            }


            PCA pca_obj = new PCA();

            data = pca_obj.Return_values_of_projections_on_Princ_comp(data);

            List<MyObject> MyRecountingObjects = new List<MyObject>();

            demension = demension > data.GetLength(1) ? data.GetLength(1) : demension;

            for (int i = 0; i < data.GetLength(0); i++)
            {
                ArrayList attr = new ArrayList();
                for (int j = 0; j < demension /*data.GetLength(1)*/; j++)
                {
                    attr.Add(data[i, j]);
                }
                MyObject obj=new MyObject(attr);
                MyRecountingObjects.Add(obj);
                
            }

            allObjects = MyRecountingObjects;

        }



        public Model()
        {
             
        }

        /// <summary>
        /// считываем из экселя в грид
        /// </summary>
        /// <param name="dataGridView1">куда считываем</param>
        public void ReadFromExcell(System.Windows.Forms.DataGridView dataGridView1)
        {
            Microsoft.Office.Interop.Excel.Application ObjExcel;
            Microsoft.Office.Interop.Excel.Workbook ObjWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet ObjWorkSheet;


            //Диалоговое окно выбора файла с фильтром
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Файл Excel|*.XLSX;*.XLS";
            openDialog.ShowDialog();
           try
            {
                //Приложение самого Excel
                ObjExcel = new Microsoft.Office.Interop.Excel.Application();
                //Книга.
                ObjWorkBook = ObjExcel.Workbooks.Open(openDialog.FileName);
                //Таблица.
                ObjWorkSheet = ObjExcel.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;
                //Ячейка
                //Microsoft.Office.Interop.Excel.Range rg = null;
                bool flag = true; //флаг на добавление строчек row в стринггрид только один раз много штучек)
                //Int32 row = 1;
                dataGridView1.Rows.Clear();
               
                

                List<double> arr = new List<double>();

                var lastCell = ObjWorkSheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell); //1 ячейку
                string[,] list = new string[lastCell.Column, lastCell.Row]; // все данные.
                // массив значений с листа равен по размеру листу

                countObj =lastCell.Column;
                countAttrib = lastCell.Row;

                for (int i = 0; i < lastCell.Column; i++) // цикл по всем колонкам
                {
                    dataGridView1.Columns.Add(i.ToString(), ObjWorkSheet.Cells[1, i + 1].Text.ToString());
                    dataGridView1.Columns[i].Name = ObjWorkSheet.Cells[1, i + 1].Text.ToString();   //&&&&&&&??????


                    if (flag) //создаю строки. 
                    {
                        dataGridView1.Rows.Add(lastCell.Row - 1);
                        flag = false;
                    }


                    for (int j = 0; j < lastCell.Row; j++) // по всем строкам   /// 1
                    {

                        list[i, j] = ObjWorkSheet.Cells[j + 2, i + 1].Text.ToString(); //считываем текст в строку
                        dataGridView1.Rows[j].Cells[i].Value = list[i, j];   /// -1

                    }

                }

                for (int j = 0; j < countAttrib-1; j++)
                {
                    ArrayList attribs = new ArrayList();
                    for (int i = 0; i < countObj ; i++)
                    {
                        attribs.Add(double.Parse(list[i, j]));
                    }
                    allObjects.Add(new MyObject(attribs)); // здесь наши объекты. 
                }


                ObjWorkBook.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя
         
                //Закрытие книгу Excel.


                //Закрытие приложения Excel.

                ObjExcel.Quit();

                //Обнуляем созданые объекты

                ObjWorkBook = null;

                ObjWorkSheet = null;

                ObjExcel = null;

                //Вызываем сборщик мусора для их уничтожения и освобождения памяти

                GC.Collect();

                MessageBox.Show("Файл успешно считан!", "Считывания excel файла");
            }
           catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message, "Ошибка при считывании excel файла"); }

        }

        public int GetCountObj()
        {
            countObj = allObjects.Count;
            return countAttrib;  
        }

        public int GetCountAttrib()
        {
            countObj = allObjects[0].attrib.Count;
            return countObj;
        }

        public MyObject GiveObject(int i)
        {
           int j = 0;
            MyObject tmp = null;
            foreach (var allObject in allObjects)
            {
                if (j == i)
                {
                    tmp = allObject;
                    break;
                }
                else j++;
            }
            return tmp;
        }

        /// <summary> возвращает данные для классификации 
        /// </summary>
        /// <returns></returns>
        public double[,] GetDataForClassification()
        {
            double[,] data = new double[allObjects.Count, allObjects[0].attrib.Count];
            for (int i = 0; i < allObjects.Count; i++)
            {
                for (int j = 0; j < allObjects[i].attrib.Count; j++)
                {
                    data[i, j] = double.Parse((string)allObjects[i].attrib[j].ToString());
                }
            }
            return data;
        }

    }
}
