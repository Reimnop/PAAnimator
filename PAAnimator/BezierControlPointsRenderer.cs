using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace PAAnimator
{
    public static class BezierControlPointsRenderer
    {
        private static Queue<Vector2[]> drawQueue = new Queue<Vector2[]>();

        private static Shader handleShader = Shader.BezierHandle;
        private static Shader lineShader = Shader.LineShader;

        private static Mesh mesh;
        private static int SSBO;

        private static int VBO, VAO;

        public static void Init()
        {
            float[] vertices = new float[] {
                 0.2f,  0.2f, 0.0f,  // top right
                 0.2f, -0.2f, 0.0f,  // bottom right
                -0.2f, -0.2f, 0.0f,  // bottom left
                -0.2f,  0.2f, 0.0f,  // top left 
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

            //init SSBO
            SSBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, SSBO);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, 0, IntPtr.Zero, BufferUsageHint.DynamicCopy);
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);

            //init line renderer
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, 0, IntPtr.Zero, BufferUsageHint.DynamicDraw);

            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, sizeof(float) * 2, 0);
            GL.EnableVertexAttribArray(0);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public static void PushDrawQueue(Vector2[] points) 
        {
            drawQueue.Enqueue(points);
        }

        public static void Render(Matrix4 view, Matrix4 projection)
        {
            while (drawQueue.Count > 0)
            {
                Vector2[] points = drawQueue.Dequeue();
                Matrix4[] transforms = new Matrix4[points.Length];

                for (int i = 0; i < points.Length; i++)
                {
                    Matrix4 model = Matrix4.CreateTranslation(new Vector3(points[i]));

                    if (i == 0)
                        model = Matrix4.CreateScale(1.5f) * model;

                    transforms[i] = Matrix4.Transpose(model);
                }

                GL.NamedBufferData(VBO, Unsafe.SizeOf<Vector2>() * points.Length, points, BufferUsageHint.DynamicDraw);
                GL.NamedBufferData(SSBO, Unsafe.SizeOf<Matrix4>() * transforms.Length, transforms, BufferUsageHint.DynamicCopy);

                //draw line
                lineShader.Use();
                lineShader.SetMatrix4("mvp", view * projection);
                lineShader.SetVector3("color", new Vector3(0.5f, 0.5f, 0.5f));

                GL.BindVertexArray(VAO);

                GL.DrawArrays(PrimitiveType.LineStrip, 0, points.Length);

                //draw handles
                GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 0, SSBO);

                handleShader.Use();
                handleShader.SetMatrix4("mvp", view * projection);

                mesh.Use();

                GL.DrawElementsInstanced(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, IntPtr.Zero, points.Length);
            }
        }
    }
}
