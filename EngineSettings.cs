using log4net;
using System.Text.Json;

namespace WillowEngine
{
    internal class EngineSettings
    {
        private const string JsonFileName = "EngineSettings.json";

        internal static EngineSettings Default
        {
            get
            {
                return new EngineSettings() { DefaultWindowWidthX = 1280, DefaultWindowWidthY = 720, FixedUpdateRate = 60.0f, ProjectName = "MyProject" };
            }
        }

        private static readonly ILog Logger = LogManager.GetLogger(typeof(EngineSettings));
        internal float FixedUpdateRate { get; set; }
#nullable disable
        internal string ProjectName { get; set; }
#nullable enable
        internal int DefaultWindowWidthX { get; set; }
        internal int DefaultWindowWidthY { get; set; }

        public static EngineSettings? Load()
        {
            EngineSettings? settings;
            try
            {
                settings = JsonSerializer.Deserialize<EngineSettings>(File.ReadAllText(JsonFileName));
            }
            catch (Exception)
            {
                Logger.Warn($"Failed to load {JsonFileName}");
                return null;
            }

            return settings;
        }

        public static bool Save(EngineSettings settings)
        {
            try
            {
                string json = JsonSerializer.Serialize(settings);
                File.WriteAllText(JsonFileName, json);
            }
            catch (Exception e)
            {
                Logger.Warn("Failed to save EngineSettings: " + e.ToString());
                return false;
            }

            return true;
        }
    }
}
