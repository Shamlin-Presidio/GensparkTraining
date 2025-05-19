using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tasks
{
    class Task6_ElementFrequency
    {
        public static void Run(int[] numbers)
        {
            Dictionary<int, int> frequencyMap = new Dictionary<int, int>();

            foreach (int number in numbers)
            {
                if (frequencyMap.ContainsKey(number))
                    frequencyMap[number]++;
                else
                    frequencyMap[number] = 1;
            }

            foreach (var entry in frequencyMap)
            {
                Console.WriteLine($"{entry.Key} occurs {entry.Value} times");
            }
        }
    }
}
