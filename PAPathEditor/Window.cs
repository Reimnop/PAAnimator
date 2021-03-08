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

            GLFW.SwapInterval(2);

            base.Run();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Viewport(0, 0, Size.X, Size.Y);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            PointDrawData drawData;
            drawData.Points = new Point[]
            {
                new Point() { Position = new Vector2(-1f, 8f), Highlighted = 0 },
                new Point() { Position = new Vector2(-1.25f, 1.94f), Highlighted = 0 },
                new Point() { Position = new Vector2(2.35f, -18.1f), Highlighted = 1 },
                new Point() { Position = new Vector2(-4.5f, 8.2f), Highlighted = 0 }
            };

            LineRenderer.PushDrawData(drawData);
            LineRenderer.Render(Matrix4.Identity, Matrix4.CreateOrthographic(71.1111111111f, 40, -1.0f, 1.0f));

            SwapBuffers();
        }
    }
}
