using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace PAAnimator
{
    public static class GridRenderer
    {
        private static Shader shader = Shader.Grid;
        private static Mesh mesh;

        public static void Init()
        {
            float[] vertices = new float[] {
                 1.0f,  1.0f, 0.0f,  // top right
                 1.0f, -1.0f, 0.0f,  // bottom right
                -1.0f, -1.0f, 0.0f,  // bottom left
                -1.0f,  1.0f, 0.0f,  // top left 
            };

            uint[] indices = new uint[]
            {
                0, 1, 3,
                1, 2, 3
            };

            mesh = new Mesh("Node", vertices, indices, new VertexAttrib[]
            {
                new VertexAttrib(0, 3)
            });
        }

        public static void Render(Matrix4 view, Matrix4 projection)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            shader.Use();
            shader.SetMatrix4("viewInverse", view.Inverted());
            shader.SetMatrix4("projInverse", projection.Inverted());

            mesh.Use();

            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }
    }
}
