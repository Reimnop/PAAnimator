using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

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

            base.Run();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Viewport(0, 0, Size.X, Size.Y);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            LineRenderer.PushDrawData(new LineDrawData { 
                Points = new Vector2[] 
                {
                    new Vector2(-124, 126),
                    new Vector2(435, 123),
                    new Vector2(784, 356)
                }
            });

            NodeRenderer.PushDrawData(new NodeDrawData
            {
                Nodes = new Vector2[]
                {
                    new Vector2(-124, 126),
                    new Vector2(435, 123),
                    new Vector2(784, 356)
                }
            });

            LineRenderer.Render(Matrix4.Identity, Matrix4.CreateOrthographic(Size.X, Size.Y, -1.0f, 1.0f));
            NodeRenderer.Render(Matrix4.Identity, Matrix4.CreateOrthographic(Size.X, Size.Y, -1.0f, 1.0f));

            SwapBuffers();
        }
    }
}
