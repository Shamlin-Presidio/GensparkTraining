using FactoryDesignPatternApp;

namespace FactoryDesignPatternApp
{
    class Program
    {
        static void Main()
        {
            var factory = new VehicleFactory();

            Console.Write("Enter vehicle type (car/bike): ");
            string input = Console.ReadLine()!.ToLower();
    
            IVehicle vehicle = factory.GetVehicle(input);
            vehicle.Drive();
        }
    }
}