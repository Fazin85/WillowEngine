using OpenTK.Mathematics;

namespace WillowEngine
{
    public class Transform
    {
        public static Transform Default
        {
            get
            {
                return new() { Position = Vector3.Zero, Rotation = Vector3.Zero, Scale = new(1.0f, 1.0f, 1.0f) };
            }
        }

        public Transform? Parent
        {
            get
            {
                return _parent;
            }
        }

        private Transform? _parent;

        internal void SetParent(Transform parent)
        {
            _parent = parent;
        }

        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;
    }
}
