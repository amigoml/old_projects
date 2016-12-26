using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace mysecond4sem
{
   public  class Bones               // класс костей
    {
        
        private int one;
            
        public int getNum1()
        {
            Thread.Sleep(40);
            Random rand1 = new Random();
           
            one = rand1.Next();
            one = one%6 + 1;
            return one;
        }
       
       public  bool getNum() // вероятность выпадения дубля (1/8 + 1/6)
       {
           Random rand1 = new Random();
           int num = rand1.Next(1000);
           if (num%8 == 0 || num%6==3 )
           {
               return true;
           }
           else
           {
               return false;
           }
       }
        
      
    }
}
