using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PAAnimator
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 80)]
    public struct NodeData
    {
        public Matrix4 Transform;
        public bool Highlighted;
    }

    public static class NodeRenderer
    {
        private static Shader shader = Shader.NodeShader;

        private static Mesh mesh;
        private static int SSBO;

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
            SSBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, SSBO);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, 0, IntPtr.Zero, BufferUsageHint.DynamicCopy);
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
        }

        public static void Render(Matrix4 view, Matrix4 projection, Vector2[] poses, bool[] highlights)
        {
            NodeData[] nodeDatas = new NodeData[poses.Length];

            for (int i = 0; i < poses.Length; i++)
            {
                nodeDatas[i].Highlighted = highlights[i];
                nodeDatas[i].Transform = Matrix4.Transpose(Matrix4.CreateTranslation(new Vector3(poses[i])));
            }

            GL.NamedBufferData(SSBO, Unsafe.SizeOf<NodeData>() * nodeDatas.Length, nodeDatas, BufferUsageHint.DynamicCopy);

            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 0, SSBO);

            shader.Use();
            shader.SetMatrix4("mvp", view * projection);

            mesh.Use();

            GL.DrawElementsInstanced(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, IntPtr.Zero, poses.Length);
        }
    }
}
