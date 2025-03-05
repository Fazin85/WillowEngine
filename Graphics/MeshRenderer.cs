using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace WillowEngine.Graphics
{
    public class MeshRenderer : Behavior
    {
        private readonly Mesh _mesh;
        private readonly Material _material;

        public MeshRenderer(Mesh mesh, Material material)
        {
            _mesh = mesh;
            _material = material;
        }

        public override void Render()
        {
            _mesh.BindVertexArray();
            IGPUBuffer? indexBuffer = _mesh.GetVertexBuffer("IBO");

            if (indexBuffer == null)
            {
                return;
            }

            _mesh.BindVertexArray();
            indexBuffer.Bind();

            _material.Bind();
            _material.SetUniformMatrix4("model", CreateModelMatrix(GameObject.Transform.Position, GameObject.Transform.Rotation, GameObject.Transform.Scale), true);

            GL.DrawElements(BeginMode.Triangles, indexBuffer.Length, DrawElementsType.UnsignedInt, 0);
        }

        public static Matrix4 CreateModelMatrix(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            Matrix4 scaleMatrix = Matrix4.CreateScale(scale);

            Matrix4 rotationX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X));
            Matrix4 rotationY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y));
            Matrix4 rotationZ = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z));

            Matrix4 rotationMatrix = rotationZ * rotationY * rotationX;

            Matrix4 translationMatrix = Matrix4.CreateTranslation(position);

            Matrix4 modelMatrix = scaleMatrix * rotationMatrix * translationMatrix;

            return modelMatrix;
        }

    }
}
