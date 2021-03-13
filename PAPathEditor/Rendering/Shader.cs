using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;

namespace PAPathEditor
{
    public sealed class Shader : IDisposable
    {
        #region DefaultShader
        public static Shader Grid;
        public static Shader Background;
        public static Shader LineShader;
        public static Shader NodeShader;
        public static Shader ArrowShader;
        #endregion

        private static int currentProgram;

        private int handle;

        private readonly Dictionary<string, int> uniformLocations;

        public Shader(string vertSource, string fragSource)
        {
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertSource);
            CompileShader(vertexShader);

            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragSource);
            CompileShader(fragmentShader);

            handle = GL.CreateProgram();

            GL.AttachShader(handle, vertexShader);
            GL.AttachShader(handle, fragmentShader);

            LinkProgram(handle);

            GL.DetachShader(handle, vertexShader);
            GL.DetachShader(handle, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);

            GL.GetProgram(handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

            uniformLocations = new Dictionary<string, int>();

            for (var i = 0; i < numberOfUniforms; i++)
            {
                var key = GL.GetActiveUniform(handle, i, out _, out _);
                var location = GL.GetUniformLocation(handle, key);

                uniformLocations.Add(key, location);
            }
        }

        ~Shader()
        {
            ThreadManager.ExecuteOnMainThread(() => Dispose());
        }

        public static Shader FromSourceFile(string vertPath, string fragPath)
        {
            return new Shader(File.ReadAllText(vertPath), File.ReadAllText(fragPath));
        }

        public static void InitDefaults()
        {
            Grid = FromSourceFile("Assets/Shaders/grid-vs.vert", "Assets/Shaders/grid-fs.frag");
            Background = FromSourceFile("Assets/Shaders/bg-vs.vert", "Assets/Shaders/bg-fs.frag");
            LineShader = FromSourceFile("Assets/Shaders/line-vs.vert", "Assets/Shaders/line-fs.frag");
            NodeShader = FromSourceFile("Assets/Shaders/node-vs.vert", "Assets/Shaders/node-fs.frag");
            ArrowShader = FromSourceFile("Assets/Shaders/arrow-vs.vert", "Assets/Shaders/arrow-fs.frag");
        }

        private static void CompileShader(int shader)
        {
            GL.CompileShader(shader);

            GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
            if (code != (int)All.True)
            {
                var infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
            }
        }

        private static void LinkProgram(int program)
        {
            GL.LinkProgram(program);

            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
            if (code != (int)All.True)
            {
                throw new Exception($"Error occurred whilst linking Program({program})");
            }
        }

        public void Use()
        {
            if (currentProgram == handle)
                return;

            ForceUse();
        }

        public void ForceUse()
        {
            GL.UseProgram(handle);
            currentProgram = handle;
        }

        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(handle, attribName);
        }

        /// <summary>
        /// Set a uniform int on this shader.
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        public void SetInt(string name, int data)
        {
            if (uniformLocations.TryGetValue(name, out int loc))
                GL.Uniform1(loc, data);
        }

        /// <summary>
        /// Set a uniform float on this shader.
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        public void SetFloat(string name, float data)
        {
            if (uniformLocations.TryGetValue(name, out int loc))
                GL.Uniform1(loc, data);
        }

        /// <summary>
        /// Set a uniform Matrix4 on this shader
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        /// <remarks>
        ///   <para>
        ///   The matrix is transposed before being sent to the shader.
        ///   </para>
        /// </remarks>
        public void SetMatrix4(string name, Matrix4 data)
        {
            if (uniformLocations.TryGetValue(name, out int loc))
                GL.UniformMatrix4(loc, true, ref data);
        }

        /// <summary>
        /// Set a uniform Vector3 on this shader.
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        public void SetVector3(string name, Vector3 data)
        {
            if (uniformLocations.TryGetValue(name, out int loc))
                GL.Uniform3(loc, data);
        }

        public void SetVector4(string name, Vector4 data)
        {
            if (uniformLocations.TryGetValue(name, out int loc))
                GL.Uniform4(loc, data);
        }

        public int GetHandle()
        {
            return handle;
        }

        public void Dispose()
        {
            GL.DeleteShader(handle);

            GC.SuppressFinalize(this);
        }
    }
}
