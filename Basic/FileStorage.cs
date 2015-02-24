using System.Collections.Generic;
using System.IO;

namespace Basic
{
    public class FileStorage : IStorage
    {
        public void Save(string filename, IEnumerable<string> lines)
        {
            File.WriteAllLines(filename, lines);
        }

        public string[] Load(string filename)
        {
            return File.ReadAllLines(filename);
        }
    }
}
