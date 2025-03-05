using log4net;

namespace WillowEngine
{
    public class Debug
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Debug));

        public static void Log(string message)
        {
            Logger.Info(message);
        }

        public static void Log(object? message)
        {
            if (message != null)
            {
                Logger.Info(message.ToString());
            }
        }
    }
}
