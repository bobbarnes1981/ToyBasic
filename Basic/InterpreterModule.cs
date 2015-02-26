using Basic.Expressions;
using Basic.Parsers;
using Ninject.Modules;

namespace Basic
{
    public class InterpreterModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILineBuffer>().To<LineBuffer>();
            Bind<IConsole>().To<Console>();
            Bind<IHeap>().To<Heap>();
            Bind<IStack>().To<Stack>();
            Bind<IStorage>().To<FileStorage>();

            Bind<IParser<ILine>>().To<LineParser>();
            Bind<IParser<INode>>().To<ExpressionParser>();
        }
    }
}
