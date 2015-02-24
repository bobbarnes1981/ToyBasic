using Ninject.Modules;

namespace Basic
{
    public class InterpreterModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IBuffer>().To<Buffer>();
            Bind<IConsole>().To<Console>();
            //Bind<IFrame>().To<Frame>();
            Bind<IHeap>().To<Heap>();
            Bind<IParser>().To<Parser>();
            Bind<IStack>().To<Stack>();
            Bind<IStorage>().To<FileStorage>();
        }
    }
}
