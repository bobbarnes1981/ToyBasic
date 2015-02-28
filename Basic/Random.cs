namespace Basic
{
    public class Random : IRandom
    {
        private System.Random m_random;

        public Random()
        {
            m_random = new System.Random();
        }

        public int Next()
        {
            return m_random.Next();
        }
    }
}
