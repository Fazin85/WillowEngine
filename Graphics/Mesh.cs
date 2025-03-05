namespace WillowEngine.Graphics
{
    public class Mesh : IDisposable
    {
        private struct ShaderUniformData
        {
            public string UniformName;
            public int UniformIndex;
        }

        private readonly VertexBufferObject _vertexArrayObject;
        private readonly Dictionary<ShaderUniformData, IGPUBuffer> _vertexBuffers = [];
        private int _nextUniformIndex = 0;

        /// <summary>
        /// IMPORTANT: the index buffer has to named "IBO" for MeshRenderer to work
        /// </summary>
        public Mesh()
        {
            _vertexArrayObject = new();
        }

        public void AddVertexBuffer(string uniformName, IGPUBuffer buffer)
        {
            _vertexBuffers.Add(new() { UniformName = uniformName, UniformIndex = _nextUniformIndex }, buffer);
            _vertexArrayObject.Link(_nextUniformIndex, buffer);
            _nextUniformIndex++;
        }

        public void AddVertexBuffers(Dictionary<string, IGPUBuffer> dict)
        {
            foreach (var kvp in dict)
            {
                _vertexBuffers.Add(new() { UniformName = kvp.Key, UniformIndex = _nextUniformIndex }, kvp.Value);
                _vertexArrayObject.Link(_nextUniformIndex, kvp.Value);
                _nextUniformIndex++;
            }
        }

        public void BindVertexArray()
        {
            _vertexArrayObject.Bind();
        }

        public int GetUniformIndex(string uniformName)
        {
            foreach (ShaderUniformData data in _vertexBuffers.Keys)
            {
                if (data.UniformName == uniformName)
                {
                    return data.UniformIndex;
                }
            }

            return -1;
        }

        public IGPUBuffer? GetVertexBuffer(string uniformName)
        {
            foreach (var kvp in _vertexBuffers)
            {
                if (kvp.Key.UniformName == uniformName)
                {
                    return kvp.Value;
                }
            }

            return null;
        }

        public IEnumerable<IGPUBuffer> GetVertexBuffers()
        {
            return _vertexBuffers.Values;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _vertexArrayObject.Dispose();

            foreach (IGPUBuffer buffer in _vertexBuffers.Values)
            {
                buffer.Dispose();
            }
        }
    }
}
