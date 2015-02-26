namespace Basic
{
    public interface IFrame
    {
        bool Exists(string name);

        void Set(string name, object value);

        T Get<T>(string name);

        object Get(string name);
    }
}
