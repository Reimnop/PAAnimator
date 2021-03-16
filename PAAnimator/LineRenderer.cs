using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace PAAnimator
{
    public struct Point
    {
        public Vector2 Position;
        public bool Bezier;
        public Vector2[] Controls;
        public bool Highlighted;
    }

    public struct PointDrawData
    {
        public Point[] Points;
    }

    public static class LineRenderer
    {
        private static Queue<PointDrawData> lineDrawQueue = new Queue<PointDrawData>();

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

        public static void PushDrawData(PointDrawData drawData)
        {
            lineDrawQueue.Enqueue(drawData);
        }

        public static void Render(Matrix4 view, Matrix4 projection)
        {
            while (lineDrawQueue.Count > 0)
            {
                PointDrawData dd = lineDrawQueue.Dequeue();

                Point[] points = dd.Points;

                List<Vector2> lineVertices = new List<Vector2>();

                for (int i = 0; i < points.Length; i++)
                {
                    Point p = points[i];

                    if (!p.Bezier || i == points.Length - 1)
                    {
                        lineVertices.Add(p.Position);
                        continue;
                    }

                    if (i != points.Length - 1)
                    {
                        //get control points
                        Vector2[] controls = new Vector2[p.Controls.Length + 2];

                        controls[0] = p.Position;
                        controls[p.Controls.Length + 1] = points[i + 1].Position;

                        for (int j = 0; j < p.Controls.Length; j++)
                        {
                            controls[j + 1] = p.Position + p.Controls[j];
                        }

                        //calculate Bezier
                        for (float t = 0.0f; t < 1.0f; t += 0.05f)
                        {
                            lineVertices.Add(Helper.Bezier(controls, t));
                        }
                    }
                }

                Vector2[] lineVerticesArr = lineVertices.ToArray();

                GL.NamedBufferData(VBO, Unsafe.SizeOf<Vector2>() * lineVerticesArr.Length, lineVerticesArr, BufferUsageHint.DynamicDraw);

                shader.Use();
                shader.SetMatrix4("mvp", view * projection);
                shader.SetVector3("color", new Vector3(0.8f, 0.8f, 0.8f));

                GL.BindVertexArray(VAO);

                GL.DrawArrays(PrimitiveType.LineStrip, 0, lineVerticesArr.Length);

                NodeRenderer.Render(view, projection, points);
                ArrowRenderer.Render(view, projection, points);
            }
        }
    }
}
