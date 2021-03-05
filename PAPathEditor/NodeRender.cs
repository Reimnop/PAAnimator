using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace PAPathEditor
{
    public struct PointDrawData
    {
        public Vector2[] Points;
    }

    public static class NodeRenderer
    {
        private static Queue<PointDrawData> nodeDrawQueue = new Queue<PointDrawData>();

        private static Shader shader = Shader.NodeShader;

        private static Mesh mesh;
        private static int SSBO;

        public static void Init()
        {
            float[] vertices = new float[] {
                 8.0f,  8.0f, 0.0f,  // top right
                 8.0f, -8.0f, 0.0f,  // bottom right
                -8.0f, -8.0f, 0.0f,  // bottom left
                -8.0f,  8.0f, 0.0f,  // top left 
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

        public static void PushDrawData(PointDrawData drawData)
        {
            nodeDrawQueue.Enqueue(drawData);
        }

        public static void Render(Matrix4 view, Matrix4 projection)
        {
            while (nodeDrawQueue.Count > 0)
            {
                PointDrawData dd = nodeDrawQueue.Dequeue();

                GL.NamedBufferData(SSBO, Unsafe.SizeOf<Vector2>() * dd.Points.Length, dd.Points, BufferUsageHint.DynamicCopy);

                GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 0, SSBO);

                shader.Use();
                shader.SetMatrix4("mvp", view * projection);

                mesh.Use();

                GL.DrawElementsInstanced(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, IntPtr.Zero, dd.Points.Length);
            }
        }
    }
}
