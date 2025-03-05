namespace WillowEngine
{
    public class Scene
    {
        public string Name
        {
            get
            {
                return _name;
            }
        }

        private readonly List<GameObject> _gameObjects = [];
        private readonly object _gameObjectLock = new();
        private readonly string _name;

        public Scene(string name)
        {
            _name = name;
        }

        internal GameObject? FindGameObject(string name)
        {
            lock (_gameObjectLock)
            {
                foreach (GameObject o in _gameObjects)
                {
                    if (o.Name == name)
                    {
                        return o;
                    }
                }

                return null;
            }
        }

        internal GameObject NewGameObject(string name)
        {
            lock (_gameObjectLock)
            {
                GameObject gameObject = new(name, Transform.Default);
                _gameObjects.Add(gameObject);

                return gameObject;
            }
        }
    }
}
