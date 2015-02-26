using Basic.Commands;

namespace Basic
{
    public interface ILine
    {
        int Number { get; set; }

        ICommand Command { get; }
    }
}
