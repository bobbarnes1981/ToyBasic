using Ninject;

namespace Basic
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new StandardKernel(new InterpreterModule()).Get<Interpreter>().Run();
        }
    }
}
