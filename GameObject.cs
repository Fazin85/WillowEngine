namespace WillowEngine
{
    public class GameObject
    {
        public Transform Transform { get; set; }
        public string Name { get; set; }
        private readonly List<Behavior> _behaviors = [];
        private readonly object _behaviorLock = new();

        public GameObject(string name, Transform transform)
        {
            Transform = transform;
            Name = name;
        }

        public static GameObject? Find(string name)
        {
            return SceneManager.FindGameObject(name);
        }

        public static GameObject? Create(string name)
        {
            return SceneManager.CreateGameObject(name);
        }

        public void AddBehavior(Behavior behavior)
        {
            lock (_behaviorLock)
            {
                _behaviors.Add(behavior);
            }
        }

        public T? GetBehavior<T>() where T : Behavior
        {
            lock (_behaviorLock)
            {
                foreach (Behavior behavior in _behaviors)
                {
                    if (behavior is T t)
                    {
                        return t;
                    }
                }
            }

            return null;
        }

        public static GameObject Create(string name, Transform transform)
        {
            GameObject gameObject = SceneManager.CreateGameObject(name);
            gameObject.Transform = transform;

            return gameObject;
        }
    }
}
