using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tasks
{
    class Task2_FindAndPrintLargestNumber
    {
        public static double Run(double num1, double num2)
        {
            if (num1 > num2)
                return num1;
            else
                return num2;
        }
    }
}
