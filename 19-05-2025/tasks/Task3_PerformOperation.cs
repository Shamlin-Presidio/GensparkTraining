using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tasks
{
    class Task3_PerformOperation
    {
        public static double Run(double num1, double num2, char operation)
        {
            switch (operation)
            {
                case '+': return num1 + num2;
                case '-': return num1 - num2;
                case '*': return num1 * num2;
                case '/':
                    if (num2 != 0)
                        return num1 / num2;
                    else
                        throw new DivideByZeroException("Cannot divide by zero.");
                default:
                    throw new InvalidOperationException("Invalid operation.");
            }
        }
    }
}
