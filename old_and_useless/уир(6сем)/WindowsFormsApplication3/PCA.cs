using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication3
{
    public class PCA : IPlugin //для распараллеливания не статический
    {
       
        public string Nick
        {
            get
            {
               return "PCA"; 
            }
        }

        private double[] eigenvalues = null;
        private double[,] eigenvectors = null;


        public enum type_of_myPCA //todo если признаков больше чем объектов то inverse_problem
        {
            inverse_problem, // X'X
            direct_problem   // X X'
        }

        private int[] nums = null; //when i'm sorted eigVal, my EigVect became sorted to



        public PCA()
        {
            
        }

        /// <summary>
        /// задачу решаю методом уменьшения вычислений.
        /// если объектов больше чем признаков то решаю прямую задачу
        /// если признаков больше чем объектов то решаю обратную - получаю проекции на ГК
        /// и домножением ищу значения исходной задачи.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>преобразованные значения в пространстве ГК (values of obj in eigenspace)</returns>
        public double[,] Return_values_of_projections_on_Princ_comp(double[,] data)  // обычно если признаков больше чем объектов то inverse_problem
        {
            //todo
            //задачу решаю методом уменьшения вычислений.
            //если объектов больше чем признаков то решаю прямую задачу
            //если признаков больше чем объектов то решаю обратную - получаю проекции на ГК
            //и домножением ищу значения исходной задачи.
            //todo

            //standartize
            data = Standartize(data);
            //end

             if (data.GetLength(0) >= data.GetLength(1))
             {
                 double[,] transpoce_matrix = Trancpose_Matrix(data);
                 double[,] cov_matrix = null;

                 cov_matrix = Matrix_Multiple(transpoce_matrix, data);

                 //TODO чуток тут теряю в производительности из-за GC ПОТЕНЦИАЛЬНО 
                 transpoce_matrix = null;
                 GC.Collect();
                 Find_EigVal_EigVect(cov_matrix);
                 var d = Matrix_Multiple(data, eigenvectors);
                 return d;
             }
             else
             {
                 double[,] transpoce_matrix = Trancpose_Matrix(data);
                 double[,] cov_matrix = null;

                 cov_matrix = Matrix_Multiple(data,transpoce_matrix);

                  
                 Find_EigVal_EigVect(cov_matrix);

                 var d=Matrix_Multiple(transpoce_matrix, eigenvectors);
                 return d;
             }
        }

        /*public void Calculate_PCA(double[,] data, type_of_myPCA type)
        {
            //standartize
            data = Standartize(data);
            //end
            if (type == type_of_myPCA.inverse_problem)
            {
                double[,] transpoce_matrix = Trancpose_Matrix(data);
                double[,] cov_matrix = null;

                cov_matrix = Matrix_Multiple(transpoce_matrix, data);
               
                //TODO чуток тут теряю в производительности из-за сборщика ПОТЕНЦИАЛЬНО 
                transpoce_matrix = null;
                GC.Collect();
                Find_EigVal_EigVect(cov_matrix);
                 
            }
            else
            {
                if (type==type_of_myPCA.direct_problem)
                {
                    double[,] transpoce_matrix = Trancpose_Matrix(data);
                    double[,] cov_matrix = Matrix_Multiple(data, transpoce_matrix);
                    //чуток тут теряю в производительности из-за сборщика ПОТЕНЦИАЛЬНО TODO
                    transpoce_matrix = null;
                    GC.Collect();
                    Find_EigVal_EigVect(cov_matrix);
                }
            }

           
        }*/

        /// <summary>
        /// приведение к нулевому среднему и нулевой дисперсии
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public double[,] Standartize(double[,] data)
        {
            int m = data.GetLength(0); //число фоток
            int n = data.GetLength(1); //одно фото колич пикселей 

            // вычесть среднее
            double[] avg_data = new double[n];
            for (int i = 0; i < n; i++)
            {
                avg_data[i] = 0;
            } 

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    avg_data[j] += data[i, j] / m;
                }
            }
             
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    data[i, j] -= avg_data[j];
                }
            }

            #region  делаю единичной дисперсию  - стандартизую

            var data1 = new double[data.GetLength(1), data.GetLength(0)];

            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    data1[j, i] = data[i, j];
                }
            }

            double[] mean_square_deviation = GetDescriptorsFor_Component(data1);  //среднекв отклонения по пикселям

            GC.Collect();
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    if (mean_square_deviation[j] > 0) data[i, j] /= mean_square_deviation[j];
                }
            }

            //TODO 
            #endregion

            return data;
        }


        /// <summary>
        /// получаем набор дескрипторов - среднеквадратических отклонений
        /// </summary>
        /// <param name="bytes"> пиксели изображений </param>
        /// <returns> массив - в ячейке значение сигмы для этого пикселя </returns>
        /// TODO !! эта функция дублируется в PixelClasterization!!!!
        private static double[] GetDescriptorsFor_Component(double[,] bytes)
        {
            int count_of_pixels = bytes.GetLength(0);   //количество пикселей
            int count_of_photos = bytes.GetLength(1);   //количество картинок

            double[] descriptors = new double[count_of_pixels]; // дескрипторы - среднеквадратичное отклонение от (отклонения от среднего) 
            //для каждого пикселя (считай от количества фоток откл от среднего смотрим и для всего в 1 число загоняем)

            for (int i = 0; i < count_of_pixels; i++)
            {
                double[] massiv = new double[count_of_photos];
                for (int j = 0; j < count_of_photos; j++)
                    massiv[j] = bytes[i, j];  //bytes - массив, первый индекс - номер пикселя, второй индекс - номер фотки, значение массива - цвет

                //теперь в массиве massiv есть все значения i-того пикселя для всех j-фоток

                //считаю среднее для i-того пикселя. считаю разности от среднего для каждой фотографии. 3 кластера. (max, 1/2 (max+min) , min)
                //кластеризация к-средних. 

                double middle = 0;
                double sigma_sqr = 0;
                for (int j = 0; j < count_of_photos; j++)
                {
                    middle += massiv[j];
                }
                middle /= count_of_photos; //среднее

                for (int j = 0; j < count_of_photos; j++)
                {
                    sigma_sqr += Math.Pow(Math.Abs(massiv[j] - middle), 2);
                }
                sigma_sqr /= count_of_photos;    /////////////////// при кластеризации не дает никаких изменений!
                descriptors[i] = Math.Sqrt(sigma_sqr);  //среднеквадратическое отклонение

            }

            return descriptors;
        }


        /// <summary>
        /// finding EigVal EigVect
        /// </summary>
        /// <param name="cov_matrix"></param>
        private void Find_EigVal_EigVect(double[,] cov_matrix)
        {
            
            double[] tmp1 = null;
            double[,] tmp2 = null;
            double[,] right_eigvect = null;
           

            if (alglib.rmatrixevd(cov_matrix, cov_matrix.GetLength(0), 1, out eigenvalues, out tmp1, out tmp2,
                                  out right_eigvect))
            {
                // right_eigvect[значения внутри вектора, номер вектора]
                nums = new int[eigenvalues.GetLength(0)];
                for (int i = 0; i < eigenvalues.GetLength(0); i++)
                {
                    nums[i] = i; 
                }
                eigenvalues = shellSort(eigenvalues, ref nums);


                eigenvectors = new double[right_eigvect.GetLength(0), right_eigvect.GetLength(1)];
                for (int i = 0; i < right_eigvect.GetLength(0); i++)
                {
                    for (int j = 0; j < right_eigvect.GetLength(1); j++)
                    {
                        eigenvectors[i, j] = right_eigvect[i, nums[j]];
                    }
                }
            }
        }


        /// <summary>
        /// Транспонирование матрицы
        /// </summary>
        /// <param name="a1">матрица</param>
        /// <returns>транспонированную матрицу</returns>
        /// TODO !! ее можно в общий класс математики
        private  double[,] Trancpose_Matrix(double[,] a1)
        {
            int n1 = a1.GetLength(0);
            int n2 = a1.GetLength(1);
            double[,] res = new double[n2, n1];
            for (int i = 0; i < n1; i++)
            {
                for (int j = 0; j < n2; j++)
                {
                    res[j, i] = a1[i, j];
                }
            }
            return res;
            GC.Collect();
        } 


        /// <summary>
        /// умножение матриц (а1(1) == а2(0), строка равна столбцу)
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        /// TODO !! ее можно в общий класс математики
        private  double[,] Matrix_Multiple(double[,] a1, double[,] a2)
        {

            int n1 = a1.GetLength(0);
            int n2 = a1.GetLength(1);

            int k1 = a2.GetLength(0);
            int k2 = a2.GetLength(1);
            if (k1 == n2) // проверку  k1==n2 сделать по хорошему надо бы... но у меня матрицы там норм стоят если че)
            {
                double[,] res = new double[n1, k2];  

                for (int i = 0; i < n1; i++)
                {
                    for (int j = 0; j < k2; j++)
                    {
                        double mult = 0;
                        for (int k = 0; k < n2; k++)
                        {
                            mult += a1[i, k] * a2[k, j];
                        }
                        res[i, j] = mult;
                    }
                }
                return res; 
            }
            else
            {
                return null;
            }
            
        }

        /// <summary>
        /// сортировка шелла
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="nums"></param>
        public static double[] shellSort(double[] vector, ref int[] nums)
        {
            int step = vector.Length / 2;
            while (step > 0)
            {
                int i, j;
                for (i = step; i < vector.Length; i++)
                {
                    double value = vector[i];
                    int tmp = nums[i];
                    for (j = i - step; (j >= 0) && (vector[j] < value); j -= step)
                    {
                        vector[j + step] = vector[j];
                        nums[j + step] = nums[j];
                    }
                    nums[j + step] = tmp;
                    vector[j + step] = value;
                }
                step /= 2;
            }
            return vector;
        }




    }//close class
}
