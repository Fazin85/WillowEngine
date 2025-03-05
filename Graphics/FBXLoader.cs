using Assimp;
using log4net;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace WillowEngine.Graphics
{
    public class FBXLoader
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(FBXLoader));
        private readonly AssimpContext _importer = new();

        /// <summary>
        /// The mesh we are trying to load must have vertices and uvs
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>A new mesh</returns>
        public Mesh? LoadMesh(string filePath)
        {
            Mesh? mesh = null;
            try
            {
                FileStream fs = File.OpenRead(filePath);
                mesh = LoadMesh(fs);
                fs.Close();

            }
            catch (Exception e)
            {
                Logger.Warn("Failed to load mesh: " + e);
            }

            return mesh;
        }

        /// <summary>
        /// The mesh we are trying to load must have vertices and uvs
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>A new mesh</returns>
        public Mesh? LoadMesh(Stream stream)
        {
            var scene = _importer.ImportFileFromStream(stream, PostProcessSteps.Triangulate | PostProcessSteps.GenerateNormals);
            if (scene.MeshCount == 0)
            {
                return null;
            }

            var mesh = scene.Meshes[0];
            if (mesh == null)
            {
                return null;
            }

            GPUBuffer<Vector3> vertices = new(mesh.Vertices.Select(v => new Vector3(v.X, v.Y, v.Z)).ToArray(), WillowVertexAttribPointerType.Float, BufferTarget.ArrayBuffer);
            GPUBuffer<Vector3> normals = new(mesh.Normals.Select(v => new Vector3(v.X, v.Y, v.Z)).ToArray(), WillowVertexAttribPointerType.Float, BufferTarget.ArrayBuffer);
            GPUBuffer<Vector2> uvs = new(mesh.TextureCoordinateChannels[0].Select(v => new Vector2(v.X, v.Y)).ToArray(), WillowVertexAttribPointerType.Float, BufferTarget.ArrayBuffer);
            GPUBuffer<int> indices = new(mesh.GetIndices(), WillowVertexAttribPointerType.Integer, BufferTarget.ElementArrayBuffer);

            Mesh wMesh = new();
            var buffers = new Dictionary<string, IGPUBuffer>
            {
                { "aPos", vertices },
                { "aNormal", vertices },
                { "aTexCoord", uvs },
                { "IBO", indices }
            };
            wMesh.AddVertexBuffers(buffers);

            return wMesh;
        }
    }
}
