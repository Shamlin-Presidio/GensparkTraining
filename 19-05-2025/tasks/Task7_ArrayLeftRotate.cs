using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tasks
{
    class Task7_ArrayLeftRotate
    {
        public static void Run(int[] array)
        {
            if (array.Length <= 1)
            {
                Console.WriteLine("Rotation not required.");
                return;
            }

            int first = array[0];
            for (int i = 0; i < array.Length - 1; i++)
            {
                array[i] = array[i + 1];
            }
            array[array.Length - 1] = first;

            Console.WriteLine("Array after left rotation:");
            Console.WriteLine(string.Join(", ", array));
        }
    }
}
