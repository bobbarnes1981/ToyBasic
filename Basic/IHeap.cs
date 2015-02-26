namespace Basic
{
    public interface IHeap
    {
        bool Exists(string name);

        void Set(string variable, object value);

        T Get<T>(string variable);

        object Get(string variable);

        void Clear();
    }
}
