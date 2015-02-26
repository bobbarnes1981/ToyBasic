using System.Collections.Generic;
using System.IO;

namespace Basic
{
    public class FileStorage : IStorage
    {
        public void Save(string filename, IEnumerable<string> lines)
        {
            // TODO: handle errors
            File.WriteAllLines(filename, lines);
        }

        public string[] Load(string filename)
        {
            // TODO: handle errors
            return File.ReadAllLines(filename);
        }
    }
}
