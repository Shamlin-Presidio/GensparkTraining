using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tasks
{
    class Task8_MergeArrays
    {
        public static void Run(int[] arr1, int[] arr2)
        {
            int[] merged = new int[arr1.Length + arr2.Length];

            for (int i = 0; i < arr1.Length; i++)
                merged[i] = arr1[i];

            for (int i = 0; i < arr2.Length; i++)
                merged[arr1.Length + i] = arr2[i];

            Console.WriteLine("Merged array:");
            Console.WriteLine(string.Join(", ", merged));
        }
    }
}
