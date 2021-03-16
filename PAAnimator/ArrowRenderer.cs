using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Runtime.CompilerServices;

namespace PAAnimator
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

        public static void Render(Matrix4 view, Matrix4 projection, Point[] points)
        {
            Matrix4[] transMat = new Matrix4[points.Length - 1];

            for (int i = 1; i < points.Length; i++)
            {
                Vector2 midPoint;

                Vector2 targ;
                Vector2 prev;

                if (!points[i - 1].Bezier)
                {
                    midPoint = (points[i].Position + points[i - 1].Position) / 2.0f;

                    targ = points[i].Position;
                    prev = points[i - 1].Position;
                }
                else
                {
                    Point p = points[i - 1];

                    //get control points
                    Vector2[] controls = new Vector2[p.Controls.Length + 2];

                    controls[0] = p.Position;
                    controls[p.Controls.Length + 1] = points[i].Position;

                    for (int j = 0; j < p.Controls.Length; j++)
                    {
                        controls[j + 1] = p.Position + p.Controls[j];
                    }

                    //calculate Bezier
                    midPoint = Helper.Bezier(controls, 0.5f);

                    prev = Helper.Bezier(controls, 0.45f);
                    targ = Helper.Bezier(controls, 0.55f);
                }


                targ.X = targ.X - prev.X;
                targ.Y = targ.Y - prev.Y;

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
