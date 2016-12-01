using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication3
{
    public interface IPlugin
    {
        /// <summary>
        /// название плагина
        /// </summary>
        string Nick { get; }

        /// <summary> Ищем значения проекций объектов в собственном подпространстве
        /// задачу решаю методом уменьшения вычислений.
        /// если объектов больше чем признаков то решаю прямую задачу
        /// если признаков больше чем объектов то решаю обратную - получаю проекции на ГК
        /// и домножением ищу значения исходной задачи.
        /// </summary>
        /// <param name="data"> матрица значений </param>
        /// <returns>преобразованные значения в пространстве ГК (values of obj in eigenspace)</returns>
        double[,] Return_values_of_projections_on_Princ_comp(double[,] data);



    }
}
