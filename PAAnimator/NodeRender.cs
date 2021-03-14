using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace PAAnimator
{
    public static class NodeRenderer
    {
        private static Shader shader = Shader.NodeShader;

        private static Mesh mesh;
        private static int SSBO_0;
        private static int SSBO_1;

        public static void Init()
        {
            float[] vertices = new float[] {
                 0.4f,  0.4f, 0.0f,  // top right
                 0.4f, -0.4f, 0.0f,  // bottom right
                -0.4f, -0.4f, 0.0f,  // bottom left
                -0.4f,  0.4f, 0.0f,  // top left 
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
            SSBO_0 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, SSBO_0);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, 0, IntPtr.Zero, BufferUsageHint.DynamicCopy);
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);

            SSBO_1 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, SSBO_1);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, 0, IntPtr.Zero, BufferUsageHint.DynamicCopy);
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
        }

        public static void Render(Matrix4 view, Matrix4 projection, Vector2[] poses, uint[] highlights)
        {
            GL.NamedBufferData(SSBO_0, Unsafe.SizeOf<Vector2>() * poses.Length, poses, BufferUsageHint.DynamicCopy);
            GL.NamedBufferData(SSBO_1, sizeof(uint) * highlights.Length, highlights, BufferUsageHint.DynamicCopy);

            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 0, SSBO_0);
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 1, SSBO_1);

            shader.Use();
            shader.SetMatrix4("mvp", view * projection);

            mesh.Use();

            GL.DrawElementsInstanced(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, IntPtr.Zero, poses.Length);
        }
    }
}
