using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace WindowsFormsApplication3
{
    public class MyObject
    {
        //
       // public List<String> atributs= new List<string>();// видимо ты не нужна!
        //

        public ArrayList attrib = new ArrayList();

        Color color =new Color();

        public  MyObject( ArrayList myParams )
        {
            color = Color.MediumPurple;
            attrib = myParams;
            //
            foreach (var myParam in myParams)
            {
               // atributs.Add(myParam.ToString());
            }
            //
        }

        public void SetColor(Color newcolor)
        {
            color = newcolor;
        }


    }
}
