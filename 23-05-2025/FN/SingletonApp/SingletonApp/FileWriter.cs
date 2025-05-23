using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingletonApp
{
    public sealed class FileWriter
    {
        private static FileWriter _instance;
        // since this is pvt, it can't be accessed anywhere else
        // we're now forced to write the other wise service layer functionalities
        private StreamWriter _writer;

        private FileWriter()
        {
            _writer = new StreamWriter("log.txt", append: true);
        }


        // this method is public, and is the only way to use this object
        
        public static FileWriter GetInstance()
        {
            if (_instance == null)
            {
                _instance = new FileWriter(); // call constructor, so only one file is created 
            }
            return _instance;
        }

        public void Write(string message)
        {
            _writer.WriteLine(message);
        }
        public void Close()
        {
            _writer.Close();
        }
    }
}
