namespace Basic
{
    public interface IStorage
    {
        void Save(string filename, string[] lines);
        string[] Load(string filename);
    }
}
