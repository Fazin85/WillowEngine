using log4net;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace WillowEngine.Graphics
{
    public class Material : IDisposable
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Material));
        private readonly Shader _shader;
        private readonly int _glId = 0;
        private readonly Dictionary<string, int> _uniformCache = [];
        private bool _disposed = false;

        public Material(Shader shader)
        {
            _shader = shader;

            if (!shader.IsLoaded())
            {
                Logger.Warn("Tried to create material with unloaded shader");
                _disposed = true;
                return;
            }

            _glId = GL.CreateProgram();

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);

            GL.ShaderSource(vertexShader, _shader.GetVertexContent());
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(fragmentShader, _shader.GetFragContent());
            GL.CompileShader(fragmentShader);

            GL.AttachShader(_glId, vertexShader);
            GL.AttachShader(_glId, fragmentShader);

            GL.LinkProgram(_glId);

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            GL.UseProgram(_glId);

            GL.GetProgram(_glId, GetProgramParameterName.ActiveUniforms, out int count);

            for (int i = 0; i < count; i++)
            {
                const int BufferSize = 64;

                GL.GetActiveUniform(_glId, i, BufferSize, out _, out _, out _, out string name);

                int location = GL.GetUniformLocation(_glId, name);
                _uniformCache.Add(name, location);
            }

            GL.UseProgram(0);
        }

        public void Bind()
        {
            GL.UseProgram(_glId);
        }

#pragma warning disable CA1822 // Mark members as static
        public void Unbind()
#pragma warning restore CA1822 // Mark members as static
        {
            GL.UseProgram(0);
        }

        public void SetUniformFloat(string uniformName, float value)
        {
            GL.Uniform1(_uniformCache[uniformName], value);
        }

        public void SetUniformVector4(string uniformName, Vector4 value)
        {
            GL.Uniform4(_uniformCache[uniformName], value);
        }

        public void SetUniformMatrix4(string uniformName, Matrix4 matrix)
        {
            GL.UniformMatrix4(_uniformCache[uniformName], false, ref matrix);
        }

        public void SetUniformMatrix4(string uniformName, Matrix4 matrix, bool transpose)
        {
            GL.UniformMatrix4(_uniformCache[uniformName], transpose, ref matrix);
        }

        public void SetUniformVector2i(string uniformName, Vector2i vector)
        {
            GL.Uniform2(_uniformCache[uniformName], vector.X, vector.Y);
        }

        public void SetUniformVector3(string uniformName, Vector3 vector)
        {
            GL.Uniform3(_uniformCache[uniformName], vector.X, vector.Y, vector.Z);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            GC.SuppressFinalize(this);
            GL.DeleteProgram(_glId);

            _disposed = true;
        }
    }
}
