using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using WillowEngine.Graphics;

namespace WillowEngine
{
    public class Engine
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Engine));
#nullable disable
        private static Window _window;
        private static EngineSettings _engineSettings;
#nullable enable
        private static bool _initialized = false;

        public static void Initialize()
        {
            _initialized = false;

            InitLogging();

            EngineSettings? settings = EngineSettings.Load();
            if (settings == null)
            {
                settings = EngineSettings.Default;
                EngineSettings.Save(settings);
            }

            _engineSettings = settings;

            _window = new(new() { WindowHeight = _engineSettings.DefaultWindowWidthX, WindowWidth = _engineSettings.DefaultWindowWidthY, WindowName = _engineSettings.ProjectName });

            _initialized = true;

            _window.Run();
        }

        internal static void Update()
        {

        }

        internal static void Render()
        {

        }

        private static void InitLogging()
        {
            var layout = new PatternLayout
            {
                ConversionPattern = "%date [%thread] %level %logger - %message%newline"
            };
            layout.ActivateOptions();
            var consoleAppender = new ConsoleAppender
            {
                Layout = layout
            };

            consoleAppender.ActivateOptions();

            var fileAppender = new FileAppender()
            {
                Layout = layout,
                File = "engine.log",
                AppendToFile = true
            };

            fileAppender.ActivateOptions();

            BasicConfigurator.Configure(consoleAppender, fileAppender);
        }

        public static void Deinitiaze()
        {
            _initialized = false;
            _window.Dispose();
        }
    }
}
