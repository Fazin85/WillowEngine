using OpenTK.Graphics.OpenGL;

namespace WillowEngine.Graphics
{
    public class GPUBuffer<T> : IDisposable, IGPUBuffer where T : unmanaged
    {
        private readonly int _glId;
        private bool _disposed = false;
        private readonly WillowVertexAttribPointerType _vertexAttribPointerType;
        private readonly BufferTarget _buffetTarget;
        private readonly int _length;

        public unsafe int ElementSize => sizeof(T) / 4;

        public WillowVertexAttribPointerType VertexAttribPointerType => _vertexAttribPointerType;

        public BufferTarget BufferTarget => _buffetTarget;

        public int Length => _length;

        public GPUBuffer(T[] data, WillowVertexAttribPointerType vertexAttribPointerType, BufferTarget bufferTarget)
        {
            _glId = GL.GenBuffer();
            _vertexAttribPointerType = vertexAttribPointerType;
            _buffetTarget = bufferTarget;
            _length = data.Length;

            GL.BindBuffer(_buffetTarget, _glId);
            GL.BufferData(_buffetTarget, data.Length * ElementSize * 4, data, BufferUsageHint.StaticDraw);
        }

        public void Bind()
        {
            GL.BindBuffer(_buffetTarget, _glId);
        }

        public void Unbind()
        {
            GL.BindBuffer(_buffetTarget, 0);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            GC.SuppressFinalize(this);
            GL.DeleteBuffer(_glId);
            _disposed = true;
        }
    }
}
