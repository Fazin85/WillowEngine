using log4net;

namespace WillowEngine.Graphics
{
    public class Shader
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Shader));
        private readonly string? _vertexContent;
        private readonly string? _fragContent;
        private readonly bool _loaded = false;

        public Shader(string vertexFilePath, string fragmentFilePath)
        {
            try
            {
                _vertexContent = File.ReadAllText(vertexFilePath);
                _fragContent = File.ReadAllText(fragmentFilePath);
            }
            catch (Exception e)
            {
                Logger.Warn("Failed to load shader: " + e);
                return;
            }

            _loaded = true;
        }

        internal string GetVertexContent()
        {
            return _vertexContent!;
        }

        internal string GetFragContent()
        {
            return _fragContent!;
        }

        public bool IsLoaded()
        {
            return _loaded;
        }
    }
}
