using OpenTK.Graphics.OpenGL4;
using System;

namespace PAPathEditor
{
    public struct VertexAttrib
    {
        public int Location;
        public int Size;
        public VertexAttribPointerType PointerType;
        public int SizeOfType;
        public bool Normalized;

        public VertexAttrib(int location, int size, bool normalized = false)
        {
            Location = location;
            Size = size;

            PointerType = VertexAttribPointerType.Float;
            SizeOfType = sizeof(float);

            Normalized = normalized;
        }

        public VertexAttrib(int location, int size, VertexAttribPointerType pointerType, int sizeOfType, bool normalized = false)
        {
            Location = location;
            Size = size;
            PointerType = pointerType;
            SizeOfType = sizeOfType;
            Normalized = normalized;
        }
    }

    public sealed class Mesh : IDisposable
    {
        public string Name;

        private float[] vertices;
        private uint[] indices;

        private int oldVertexCount;
        private int oldIndexCount;

        private int VAO, VBO, EBO;

        public Mesh(string name, float[] vertices, uint[] indices, VertexAttrib[] attribs, BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw)
        {
            Name = name;
            this.vertices = vertices;
            this.indices = indices;
            Init(attribs, vertices, indices, bufferUsageHint);
        }

        ~Mesh()
        {
            ThreadManager.ExecuteOnMainThread(() => Dispose());
        }

        private void Init(VertexAttrib[] attribs, float[] vertices, uint[] indices, BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw)
        {
            int vertexSize = vertices.Length * sizeof(float);
            int indexSize = indices.Length * sizeof(uint);

            oldVertexCount = vertices.Length;
            oldIndexCount = indices.Length;

            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();
            EBO = GL.GenBuffer();

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertexSize, vertices, bufferUsageHint);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indexSize, indices, bufferUsageHint);

            int size = 0;
            foreach (VertexAttrib attrib in attribs)
                size += attrib.Size * attrib.SizeOfType;

            int c = 0;
            for (int i = 0; i < attribs.Length; i++)
            {
                GL.VertexAttribPointer(attribs[i].Location, attribs[i].Size, attribs[i].PointerType, attribs[i].Normalized, size, c);
                GL.EnableVertexAttribArray(attribs[i].Location);
                c += attribs[i].Size * attribs[i].SizeOfType;
            }

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        public void UpdateMeshData(float[] vertices, uint[] indices)
        {
            this.vertices = vertices;
            this.indices = indices;

            int vertexSize = vertices.Length * sizeof(float);
            int indexSize = indices.Length * sizeof(uint);

            if (vertices.Length > oldVertexCount)
                GL.NamedBufferData(VBO, vertexSize, vertices, BufferUsageHint.DynamicDraw);
            else
                GL.NamedBufferSubData(VBO, IntPtr.Zero, vertexSize, vertices);

            if (indices.Length > oldIndexCount)
                GL.NamedBufferData(EBO, indexSize, indices, BufferUsageHint.DynamicDraw);
            else
                GL.NamedBufferSubData(EBO, IntPtr.Zero, indexSize, indices);

            oldVertexCount = vertices.Length;
            oldIndexCount = indices.Length;
        }

        public int GetVerticesCount()
        {
            return vertices.Length;
        }

        public int GetIndicesCount()
        {
            return indices.Length;
        }

        public float[] GetVertices()
        {
            return vertices;
        }

        public uint[] GetIndices()
        {
            return indices;
        }

        public void Use()
        {
            GL.BindVertexArray(VAO);
        }

        public void Dispose()
        {
            GL.DeleteVertexArray(VAO);
            GL.DeleteBuffer(VBO);
            GL.DeleteBuffer(EBO);

            GC.SuppressFinalize(this);
        }
    }
}
