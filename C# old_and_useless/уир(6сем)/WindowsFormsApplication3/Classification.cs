using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication3
{
    class Classification
    {
         
        private int width;
        private int[] nums = null;

        //+++++++++++++++++++++++++++++++++++++++++++
          
        /// <summary> считаем расстояние между исх ихображением и его проекцией в пространстве СВ которые даем на вход
        /// </summary>
        /// <param name="pathOfImage">классифицируемые изображения</param>
        /// <param name="eigenvectors"> собственные вектора какого то набора </param>
        /// <returns>foreach test image distanse to  projection himself into eigenspace</returns>
        public double[] ReturnDistanceToProjection(double[,] test, double[,] eigenvectors, double[] avg_img)
        {  
            test = Trancpose_Matrix(test);

            double[,] proj = Reconstruct_Projected_Image(test, eigenvectors, avg_img);

            return Dist(test, proj); //foreach test image distanse to  projection himself into eigenspace
        }


        /// <summary> считаем средний портрет выборки
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public double[] ReturnAverangeImage(double[,] images)
        { 
            int m = images.GetLength(0); //число фоток
            int n = images.GetLength(1); //одно фото колич пикселей 

            // вычесть среднее
            double[] averange_img = new double[n];
            for (int i = 0; i < n; i++)
            {
                averange_img[i] = 0;
            }

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    averange_img[j] += images[i, j] / m;
                }
            }
            return averange_img;
        }


        /// <summary>
        /// будем сюда подавать path to the images of faces of some one nation and computing eigvect
        ///  (и потом их надо записывать в файл текстовый)
        /// </summary>
        /// <param name="images"> images of some nation </param>
        /// <returns> sorted eigenvectors </returns>
        public double[,] Compute_eigenvectors( double[,] images )
        {
              

            int m = images.GetLength(0); //число фоток
            int n = images.GetLength(1); //одно фото колич пикселей 
/*
            // вычесть среднее
            double[] averange_img = new double[n];
            for (int i = 0; i < n; i++)
            {
                averange_img[i] = 0;
            }

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    averange_img[j] += images[i, j] / m;
                }
            }
              

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    images[i, j] -= averange_img[j];
                }
            }

            averange_img = null;  //TODO больше не нужен?
            GC.Collect(); 

            #region  делаю единичной дисперсию  - стандартизую

            var images1 = new double[images.GetLength(1), images.GetLength(0)];

            for (int i = 0; i < images.GetLength(0); i++)
            {
                for (int j = 0; j < images.GetLength(1); j++)
                {
                    images1[j, i] = images[i, j];
                }
            }

            double[] mean_square_deviation = GetDescriptorsFor_Component(images1);  //среднекв отклонения по пикселям

            GC.Collect();
            for (int i = 0; i < images.GetLength(0); i++)
            {
                for (int j = 0; j < images.GetLength(1); j++)
                {
                    if (mean_square_deviation[j] > 0) images[i, j] /= mean_square_deviation[j];
                }
            }

            //TODO 
            #endregion
*/
            double[,] cov = Matrix_Multiple(images, Trancpose_Matrix(images));  //  ков матрица на самом деле A'A. Но у меня как на хабре в статье AA'

            /**/

            double[] eigenvalues = null;
            double[] tmp1 = null;
            double[,] tmp2 = null;
            double[,] right_eigenvectors = null;

            if (alglib.rmatrixevd(cov, cov.GetLength(0), 1, out eigenvalues, out tmp1, out tmp2, out right_eigenvectors))
            {
                // right_eigvectors[значения внутри вектора, номер вектора]

                nums = new int[eigenvalues.GetLength(0)];
                for (int i = 0; i < eigenvalues.GetLength(0); i++)
                {
                    nums[i] = i;// eigenvalues.GetLength(0) - i - 1;
                }
                eigenvalues = shellSort(eigenvalues, ref nums);


                


                //TODO я по правде здесь сортировал вектора
                
                // проверка на соотв сз своему св
                //
                double[,] eigvect = new double[right_eigenvectors.GetLength(1), right_eigenvectors.GetLength(1)];
                for (int i = 0; i < right_eigenvectors.GetLength(1); i++)
                {
                    for (int j = 0; j < right_eigenvectors.GetLength(1); j++)
                    {
                        eigvect[i, j] = right_eigenvectors[i, nums[j]];
                    }
                }
                 
               // var tm = Matrix_Multiple(cov, eigvect);
                for (int i = 0; i < right_eigenvectors.GetLength(1); i++)
                {
                    for (int j = 0; j < right_eigenvectors.GetLength(1); j++)
                    {
                        eigvect[i, j] *= eigenvalues[j];
                    }
                }

                //
                //сравнивал tm и eigenvect 
                //проверка на соотв сз своему св
      
                 

                //собственный вектор исходной матрицы AA' нахожу так:
                tmp2 = Matrix_Multiple(Trancpose_Matrix(images), eigvect); // right_eigenvectors); //eigvect); //транспонировал потому что у меня размерности наоборот с тем что пишут в статьях
                // насчет транспонирования. в статье с хабра которую я копировал про МГК тоже транспонирование, так что с реализацией не ошибся вроде)
                //eigenfaces = tmp2;
            }
            return tmp2;
        }


        /// <summary> Построение изображения спроецированного в пространство главных компонент. 
        /// </summary>
        private double[,] Reconstruct_Projected_Image(double[,] image, double[,] eigenfaces, double[] averange_img)
        {

            var weight_coefficients = Matrix_Multiple(Trancpose_Matrix(eigenfaces), (image)); // weight_coefficients_of_eigenfaces

            double[,] reconstructed_image = new double[eigenfaces.GetLength(0), image.GetLength(1)];
             
            for (int i = 0; i < reconstructed_image.GetLength(0); i++)
            {
                for (int j = 0; j < reconstructed_image.GetLength(1); j++)
                {
                    reconstructed_image[i, j] = 0;
                }
            }

            //построение проекции
            int number_of_subdeminsion = eigenfaces.GetLength(1);
            
                for (int i = 0; i < eigenfaces.GetLength(0); i++) //по каждому пикселю
                {
                    for (int j = 0; j < number_of_subdeminsion; j++) //число собственных лиц (базис) номер! TODO number_of_subdeminsion
                    {
                        for (int k = 0; k < reconstructed_image.GetLength(1); k++)
                        {
                            reconstructed_image[i, k] += eigenfaces[i, j] * weight_coefficients[j, k];
                            // weight_coefficients[j, номер изображения в тестовой выборке]
                        }
                    }
                }

                //++++++++++++++
                // пропорционально сжимаю значения... чтобы красивая фотография получилась ---- перегнал в метод преобразования в bitmap
                // double max = 0.0, min = double.MaxValue;
            /*
                double[] max_val = new double[reconstructed_image.GetLength(1)];
                double[] min_val = new double[reconstructed_image.GetLength(1)];
                for (int i = 0; i < reconstructed_image.GetLength(1); i++)
                {
                    max_val[i] = 0;
                    min_val[i] = double.MaxValue;
                }


                for (int i = 0; i < reconstructed_image.GetLength(0); i++)
                {
                    for (int j = 0; j < reconstructed_image.GetLength(1); j++)
                    {
                        if (reconstructed_image[i, j] > max_val[j]) max_val[j] = reconstructed_image[i, j];
                        if (reconstructed_image[i, j] < min_val[j]) min_val[j] = reconstructed_image[i, j];
                    }
                }

                for (int i = 0; i < reconstructed_image.GetLength(0); i++)
                {
                    for (int j = 0; j < reconstructed_image.GetLength(1); j++)
                    {
                        reconstructed_image[i, j] = 1 * (reconstructed_image[i, j] - min_val[j]) / (max_val[j] - min_val[j]);
                    }
                }

                // todo посмотреть что будет если уберу среднее изображение!!!!тогда не работает!!!!!
                for (int i = 0; i < averange_img.GetLength(0); i++)
                {
                    for (int j = 0; j < reconstructed_image.GetLength(1); j++)
                    {
                       // reconstructed_image[i, j] += averange_img[i];
                    }
                } 


            //++++++++++++++
            // пропорционально сжимаю значения... чтобы красивая фотография получилась ---- перегнал в метод преобразования в bitmap
            // double max = 0.0, min = double.MaxValue;
           /*
              max_val = new double[reconstructed_image.GetLength(1)];
              min_val = new double[reconstructed_image.GetLength(1)];
            for (int i = 0; i < reconstructed_image.GetLength(1); i++)
            {
                max_val[i] = 0;
                min_val[i] = double.MaxValue;
            }
           

            for (int i = 0; i < reconstructed_image.GetLength(0); i++)
            {
                for (int j = 0; j < reconstructed_image.GetLength(1); j++)
                {
                    if (reconstructed_image[i, j] > max_val[j]) max_val[j] = reconstructed_image[i, j];
                    if (reconstructed_image[i, j] < min_val[j]) min_val[j] = reconstructed_image[i, j];
                }
            }

            for (int i = 0; i < reconstructed_image.GetLength(0); i++)
            {
                for (int j = 0; j < reconstructed_image.GetLength(1); j++)
                {
                    reconstructed_image[i, j] = 1 * (reconstructed_image[i, j] - min_val[j]) / (max_val[j] - min_val[j]);
                }

            }
             */
            GC.Collect();
            return reconstructed_image;

        }

        
        /// <summary> Euclidean distance between two selection (пачка) of images
        ///  </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private double[] Dist(double[,] x, double[,] y)
        {
            double[] sum = new double[x.GetLength(1)];
            int counter = x.GetLength(1);
            for (int n = 0; n < counter; n++)
            {
                sum[n] = 0;
                for (int i = 0; i < x.GetLength(0); i++)
                {
                    sum[n] += Math.Pow(x[i, n] - y[i, n], 2);
                }
                sum[n] = (Math.Sqrt(sum[n]));
            }
            return sum;
        }

         

        /// <summary> сортировка шелла
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="nums"></param>
        private double[] shellSort(double[] vector, ref int[] nums)
        {
            int step = vector.Length / 2;
            while (step > 0)
            {
                int i, j;
                for (i = step; i < vector.Length; i++)
                {
                    double value = vector[i];
                    int tmp = nums[i];
                    for (j = i - step; (j >= 0) && (vector[j] > value); j -= step)
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


        /// <summary> Транспонирование матрицы
        /// </summary>
        /// <param name="a1">матрица</param>
        /// <returns>транспонированную матрицу</returns>
        /// TODO !! ее можно в общий класс математики
        private double[,] Trancpose_Matrix(double[,] a1)
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

        /// <summary> умножение матриц (а1(1) == а2(0), строка равна столбцу)
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        /// TODO !! ее можно в общий класс математики
        private double[,] Matrix_Multiple(double[,] a1, double[,] a2)
        {

            int n1 = a1.GetLength(0);
            int n2 = a1.GetLength(1);

            int k1 = a2.GetLength(0);
            int k2 = a2.GetLength(1);

            double[,] res = new double[n1, k2];  // проверку  k1==n2 сделать по хорошему надо бы... но у меня матрицы там норм стоят если че)

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

        /// <summary> получаем набор дескрипторов - среднеквадратических отклонений
        /// </summary>
        /// <param name="bytes"> пиксели изображений </param>
        /// <returns> массив - в ячейке значение сигмы для этого пикселя </returns>
        /// TODO !! эта функция дублируется в PixelClasterization!!!!
        private double[] GetDescriptorsFor_Component(double[,] bytes)
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

    



    }//closing this class
    
}

   
