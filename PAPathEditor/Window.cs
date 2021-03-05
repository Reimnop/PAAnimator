using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace PAPathEditor
{
    public sealed class Window : GameWindow
    {
        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) { }

        public override void Run()
        {
            Shader.InitDefaults();
            LineRenderer.Init();
            NodeRenderer.Init();

            GLFW.SwapInterval(144);

            base.Run();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Viewport(0, 0, Size.X, Size.Y);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            PointDrawData drawData;
            Vector2[] points = new Vector2[256];
            Random rng = new Random();
            for (int i = 0; i < 256; i++)
            {
                points[i] = (new Vector2((float)rng.NextDouble(), (float)rng.NextDouble()) * 2.0f - Vector2.One) * 428.0f;
            }
            drawData.Points = points;

            LineRenderer.PushDrawData(drawData);
            NodeRenderer.PushDrawData(drawData);

            LineRenderer.Render(Matrix4.Identity, Matrix4.CreateOrthographic(Size.X, Size.Y, -1.0f, 1.0f));
            NodeRenderer.Render(Matrix4.Identity, Matrix4.CreateOrthographic(Size.X, Size.Y, -1.0f, 1.0f));

            SwapBuffers();
        }
    }
}
