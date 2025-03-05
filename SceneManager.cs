using log4net;

namespace WillowEngine
{
    public class SceneManager
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SceneManager));
        private static Scene? _currentScene;
        private static readonly List<Scene> _scenes = [];
        private static readonly object _sceneLock = new();

        public static void AddScene(Scene scene)
        {
            lock (_sceneLock)
            {
                _scenes.Add(scene);
            }
        }

        public static Scene? GetCurrentScene()
        {
            lock (_sceneLock)
            {
                return _currentScene;
            }
        }

        public static void SetActiveScene(string name)
        {
            Scene? scene = GetScene(name);

            if (scene == null)
            {
                Logger.Warn("Failed to get scene with name: " + name);
            }
            else
            {
                _currentScene = scene;
            }
        }

        internal static Scene? GetScene(string name)
        {
            lock (_sceneLock)
            {
                foreach (Scene scene in _scenes)
                {
                    if (scene.Name == name)
                    {
                        return scene;
                    }
                }
            }

            return null;
        }

        internal static GameObject? FindGameObject(string name)
        {
            return _currentScene?.FindGameObject(name);
        }

        internal static GameObject? CreateGameObject(string name)
        {
            if (_currentScene == null)
            {
                Logger.Warn("Failed to get scene with name: " + name);
                return null;
            }
            else
            {
                return _currentScene.NewGameObject(name);
            }
        }
    }
}
