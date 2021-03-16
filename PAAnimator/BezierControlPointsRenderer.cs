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

        private static Shader shader = Shader.BezierHandle;

        private static Mesh mesh;
        private static int SSBO;

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
                    transforms[i] = Matrix4.Transpose(Matrix4.CreateTranslation(new Vector3(points[i])));
                }

                GL.NamedBufferData(SSBO, Unsafe.SizeOf<Matrix4>() * transforms.Length, transforms, BufferUsageHint.DynamicCopy);

                GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 0, SSBO);

                shader.Use();
                shader.SetMatrix4("mvp", view * projection);

                mesh.Use();

                GL.DrawElementsInstanced(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, IntPtr.Zero, points.Length);
            }
        }
    }
}
