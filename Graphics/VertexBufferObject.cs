using OpenTK.Graphics.OpenGL;

namespace WillowEngine.Graphics
{
    internal class VertexBufferObject : IDisposable
    {
        private bool _disposed;
        private readonly int _glId;

        internal VertexBufferObject()
        {
            _disposed = false;

            _glId = GL.GenVertexArray();
        }

        internal void Link(int location, IGPUBuffer buffer)
        {
            GL.BindVertexArray(_glId);
            buffer.Bind();
            if (buffer.VertexAttribPointerType == WillowVertexAttribPointerType.Float)
            {
                GL.VertexAttribPointer(location, buffer.ElementSize, VertexAttribPointerType.Float, false, 0, 0);
            }
            else if (buffer.VertexAttribPointerType == WillowVertexAttribPointerType.Integer)
            {
                GL.VertexAttribIPointer(location, buffer.ElementSize, VertexAttribIntegerType.Int, 0, 0);
            }

            GL.EnableVertexAttribArray(location);
            GL.BindVertexArray(0);
        }

        internal void Bind()
        {
            GL.BindVertexArray(_glId);
        }

#pragma warning disable CA1822 // Mark members as static
        internal void Unbind()
#pragma warning restore CA1822 // Mark members as static
        {
            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            GC.SuppressFinalize(this);
            GL.DeleteVertexArray(_glId);

            _disposed = true;
        }
    }
}
