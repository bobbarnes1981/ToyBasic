using Basic.Factories;
using Basic.Parser;
using Basic.Tokenizer;
using Ninject.Modules;

namespace Basic
{
    public class InterpreterModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IParser>().To<Parser.Parser>();
            Bind<ITokenizer>().To<Tokenizer.Tokenizer>();

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
