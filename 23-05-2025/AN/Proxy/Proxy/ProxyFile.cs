namespace SecureFileAccessProxy
{
    public class ProxyFile : IFile
    {
        private readonly File _realFile;
        private readonly User _user;

        public ProxyFile(string fileName, User user)
        {
            _realFile = new File(fileName);
            _user = user;
        }

        public void Read()
        {
            switch (_user.UserRole)
            {
                case Role.Admin:
                    _realFile.Read();
                    break;

                case Role.User:
                    _realFile.ReadMetadata();
                    break;

                case Role.Guest:
                    Console.WriteLine("[Access Denied] You do not have permission to read this file.");
                    break;

                default:
                    Console.WriteLine("[Access Denied] Unknown role.");
                    break;
            }
        }
    }
}
