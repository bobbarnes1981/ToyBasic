using System.IO;

namespace Basic
{
    public class FileStorage : IStorage
    {
        public void Save(string filename, string[] lines)
        {
            File.WriteAllLines(filename, lines);
        }

        public string[] Load(string filename)
        {
            return File.ReadAllLines(filename);
        }
    }
}
