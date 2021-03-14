using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAAnimator
{
    public static class BackgroundRenderer
    {
        private static Shader shader = Shader.Background;
        private static Mesh mesh;

        public static Texture Background;

        public static void Init()
        {
            float[] vertices = new float[] {
                 1.0f,  1.0f, 0.0f, 1.0f, 0.0f, // top right
                 1.0f, -1.0f, 0.0f, 1.0f, 1.0f, // bottom right
                -1.0f, -1.0f, 0.0f, 0.0f, 1.0f, // bottom left
                -1.0f,  1.0f, 0.0f, 0.0f, 0.0f  // top left 
            };

            uint[] indices = new uint[]
            {
                0, 1, 3,
                1, 2, 3
            };

            mesh = new Mesh("Node", vertices, indices, new VertexAttrib[]
            {
                new VertexAttrib(0, 3),
                new VertexAttrib(1, 2)
            });
        }

        public static void Render(Matrix4 view, Matrix4 projection)
        {
            if (Background == null)
                return;

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            shader.Use();
            shader.SetMatrix4("mvp", view * projection);

            Background.Use(TextureUnit.Texture0);

            mesh.Use();

            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }
    }
}
