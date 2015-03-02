namespace Basic.Factories
{
    public interface IFactory<T>
    {
        T Build();
    }
}
