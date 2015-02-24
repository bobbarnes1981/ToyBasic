using System.Collections.Generic;

namespace Basic
{
    public interface IStorage
    {
        void Save(string filename, IEnumerable<string> lines);
        string[] Load(string filename);
    }
}
