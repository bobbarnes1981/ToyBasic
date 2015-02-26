namespace Basic
{
    public interface IFrame
    {
        bool Exists(string name);

        T Get<T>(string name);

        void Set<T>(string name, T value);
    }
}
