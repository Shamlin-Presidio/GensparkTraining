using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryDesignPatternApp
{
    class VehicleFactory
    {
        public IVehicle GetVehicle(string type)
        {
            if (type == "car")
                return new Car();
            else if (type == "bike")
                return new Bike();
            else
                throw new ArgumentException("Unknown vehicle type");
        }
    }
}
