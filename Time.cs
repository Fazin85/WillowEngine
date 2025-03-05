namespace WillowEngine
{
    public class Time
    {
        public static float DeltaTime
        {
            get
            {
                return _deltaTime;
            }
            internal set
            {
                _deltaTime = value;
            }
        }

        public static float Elapsed
        {
            get
            {
                return _elapsed;
            }
            internal set
            {
                _elapsed = value;
            }
        }

        private static float _deltaTime = 0.0f;
        private static float _elapsed = 0.0f;
    }
}
