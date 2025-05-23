using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryDesignPatternApp
{
    public class Car : IVehicle
    {
        public void Drive()
        {
            Console.WriteLine("Driving a Car");
        }
    }
}
