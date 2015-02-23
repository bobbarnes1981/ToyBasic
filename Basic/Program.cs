namespace Basic
{
    public class Program
    {
        static void Main(string[] args)
        {
            new Interpreter(new Buffer(), new Parser(), new Console(), new Heap(), new Stack()).Run();
        }
    }
}
