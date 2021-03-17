using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using PAAnimator.Logic;
using PAAnimator.Logic.Animation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PAAnimator
{
    public static class PreviewRenderer
    {
        private static Shader shader = Shader.Preview;
        private static Mesh mesh;

        public static Texture PreviewTexture;

        public static void Init()
        {
            float[] vertices = new float[] {
                 0.5f,  0.5f, 0.0f, 1.0f, 0.0f, // top right
                 0.5f, -0.5f, 0.0f, 1.0f, 1.0f, // bottom right
                -0.5f, -0.5f, 0.0f, 0.0f, 1.0f, // bottom left
                -0.5f,  0.5f, 0.0f, 0.0f, 0.0f
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
            Project prj = ProjectManager.CurrentProject;

            FrameData data = Animator.GetCurrentFrameData(prj.Time);

            Matrix4 model =
                Matrix4.CreateScale(new Vector3(data.Scale.X * prj.PreviewScale.X, data.Scale.Y * prj.PreviewScale.Y, 1.0f)) *
                Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(data.Rotation + prj.PreviewRotation)) *
                Matrix4.CreateTranslation(new Vector3(data.Position + prj.PreviewPosition));

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            shader.Use();
            shader.SetMatrix4("mvp", model * view * projection);

            PreviewTexture?.Use(TextureUnit.Texture0);

            mesh.Use();

            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }
    }
}
