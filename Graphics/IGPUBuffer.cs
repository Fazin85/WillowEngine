using OpenTK.Graphics.OpenGL;

namespace WillowEngine.Graphics
{
    public interface IGPUBuffer
    {
        int ElementSize { get; }
        WillowVertexAttribPointerType VertexAttribPointerType { get; }
        BufferTarget BufferTarget { get; }
        int Length { get; }
        void Bind();
        void Unbind();
        void Dispose();
    }
}
