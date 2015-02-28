namespace Basic
{
    public interface IConsole
    {
        void Output(string text);

        string Input();

        object ParseInput();

        void Clear();
    }
}
