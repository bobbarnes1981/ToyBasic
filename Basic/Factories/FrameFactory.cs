namespace Basic.Factories
{
    public class FrameFactory : IFactory<IFrame>
    {
        public IFrame Build()
        {
            return new Frame();
        }
    }
}
