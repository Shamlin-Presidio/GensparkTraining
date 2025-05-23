using SingletonApp;

namespace SingletonApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the single instance of FileWriter
            FileWriter writer = FileWriter.GetInstance();

 
            writer.Write("first line");
            writer.Write("second line");
            writer.Write("end card");

            writer.Close();

            Console.WriteLine("File written and closed successfully.");
        }
    }
}