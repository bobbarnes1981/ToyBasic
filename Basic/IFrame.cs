namespace Basic
{
    public interface IFrame
    {
        T Get<T>(string name);
        void Set<T>(string name, T value);
    }
}
