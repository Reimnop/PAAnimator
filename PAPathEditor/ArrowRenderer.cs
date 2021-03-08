using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace PAPathEditor
{
    public static class ArrowRenderer
    {
        private static Shader shader = Shader.ArrowShader;

        private static Mesh mesh;
        private static int SSBO;

        public static void Init()
        {
            float[] vertices = new float[] {
                -0.25f,  0.25f, 0.0f,
                 0.25f, -0.0f , 0.0f,
                -0.25f, -0.25f, 0.0f
            };

            uint[] indices = new uint[]
            {
                0, 1, 2
            };

            mesh = new Mesh("Node", vertices, indices, new VertexAttrib[]
            {
                new VertexAttrib(0, 3)
            });

            //init SSBO
            SSBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, SSBO);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, 0, IntPtr.Zero, BufferUsageHint.DynamicCopy);
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
        }

        public static void Render(Matrix4 view, Matrix4 projection, Vector2[] poses)
        {
            Matrix4[] transMat = new Matrix4[poses.Length - 1];

            for (int i = 1; i < poses.Length; i++)
            {
                Vector2 midPoint = (poses[i] + poses[i - 1]) / 2.0f;

                Vector2 targ = poses[i];
                Vector2 objectPos = midPoint;
                targ.X = targ.X - objectPos.X;
                targ.Y = targ.Y - objectPos.Y;

                float angle = MathF.Atan2(targ.Y, targ.X);

                transMat[i - 1] = Matrix4.Transpose(Matrix4.CreateRotationZ(angle) * Matrix4.CreateTranslation(new Vector3(midPoint)));
            }

            GL.NamedBufferData(SSBO, Unsafe.SizeOf<Matrix4>() * transMat.Length, transMat, BufferUsageHint.DynamicCopy);

            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 0, SSBO);

            shader.Use();
            shader.SetMatrix4("mvp", view * projection);

            mesh.Use();

            GL.DrawElementsInstanced(PrimitiveType.Triangles, 3, DrawElementsType.UnsignedInt, IntPtr.Zero, transMat.Length);
        }
    }
}
