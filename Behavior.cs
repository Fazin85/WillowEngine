namespace WillowEngine
{
    public abstract class Behavior
    {
        public GameObject GameObject { get; }

        public virtual void Start()
        {

        }
        public virtual void Update()
        {

        }

        public virtual void PreRender()
        {

        }

        public virtual void Render()
        {

        }

        public virtual void FixedUpdate()
        {

        }

        public virtual void OnDestroy()
        {

        }
    }
}
