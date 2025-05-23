namespace SecureFileAccessProxy
{
    public class File : IFile
    {
        private readonly string _fileName;

        public File(string fileName)
        {
            _fileName = fileName;
        }

        public void Read()
        {
            Console.WriteLine($"[Access Granted] Reading sensitive content of file '{_fileName}'...");
        }

        // users other than Admin can read only metadata the  :)
        public void ReadMetadata()
        {
            Console.WriteLine($"[Limited Access] Reading metadata of file '{_fileName}'...");
            Console.WriteLine("The file was last modified by Admin, 2 mins ago");
            Console.WriteLine("Get Admin access to see the file");
        }
    }
}
