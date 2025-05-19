using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace tasks
{
    class Task5_NumberDivisibleBy7
    {
        public static int Run(int[] numbers)
        {
            int count = 0;
            foreach (int number in numbers)
            {
                if (number % 7 == 0)
                    count++;
            }
            return count;
        }
    }
}
