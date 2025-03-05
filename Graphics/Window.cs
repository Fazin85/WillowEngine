using log4net;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace WillowEngine.Graphics
{
    public class Window : GameWindow
    {
#nullable disable
        private static Window _instance;
#nullable enable
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Window));

        internal Window(WindowCreationParams @params) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = new(@params.WindowWidth, @params.WindowHeight), Title = @params.WindowName })
        {
            if (_instance != null)
            {
                Logger.Info("Already initialized window");
                return;
            }

            _instance = this;
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            Time.DeltaTime = (float)args.Time;
            Time.Elapsed += Time.DeltaTime;

            Engine.Update();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            Engine.Render();
        }

        public static void SetSize(Vector2i size)
        {
            _instance.ClientSize = size;
        }

        public static Vector2i GetSize()
        {
            return _instance.ClientSize;
        }
    }
}
