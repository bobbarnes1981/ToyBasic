using Basic.Expressions;
using Basic.Parsers;
using Ninject.Modules;

namespace Basic
{
    public class InterpreterModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IBuffer>().To<Buffer>();
            Bind<IConsole>().To<Console>();
            Bind<IHeap>().To<Heap>();
            Bind<IStack>().To<Stack>();
            Bind<IStorage>().To<FileStorage>();

            Bind<IParser<ILine>>().To<Parsers.Line>();
            Bind<IParser<IExpression>>().To<Parsers.Expression>();
        }
    }
}
