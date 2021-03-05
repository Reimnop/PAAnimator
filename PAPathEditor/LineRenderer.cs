using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace PAPathEditor
{
    public struct LineDrawData
    {
        public Vector2[] Points;
    }

    public static class LineRenderer
    {
        private static Queue<LineDrawData> lineDrawQueue = new Queue<LineDrawData>();

        private static Shader shader = Shader.LineShader;

        private static int VBO, VAO;

        public static void Init()
        {
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

        public static void PushDrawData(LineDrawData drawData)
        {
            lineDrawQueue.Enqueue(drawData);
        }

        public static void Render(Matrix4 view, Matrix4 projection)
        {
            while (lineDrawQueue.Count > 0)
            {
                LineDrawData dd = lineDrawQueue.Dequeue();

                GL.NamedBufferData(VBO, Unsafe.SizeOf<Vector2>() * dd.Points.Length, dd.Points, BufferUsageHint.DynamicDraw);

                shader.Use();
                shader.SetMatrix4("mvp", view * projection);

                GL.BindVertexArray(VAO);

                GL.DrawArrays(PrimitiveType.LineStrip, 0, dd.Points.Length);
            }
        }
    }
}
