using Basic.Factories;
using Basic.Parsers;
using Ninject.Modules;

namespace Basic
{
    public class InterpreterModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IParser<ILine>>().To<LineParser>();
            Bind<IParser<object>>().To<InputParser>();


            Bind<ILineBuffer>().To<LineBuffer>();
            Bind<IConsole>().To<Console>();
            Bind<IHeap>().To<Heap>();
            Bind<IFactory<IFrame>>().To<FrameFactory>();
            Bind<IStack>().To<Stack>();
            Bind<IStorage>().To<FileStorage>();

            Bind<IRandom>().To<Random>();
        }
    }
}
