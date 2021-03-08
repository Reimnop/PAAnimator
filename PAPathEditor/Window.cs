using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using PAPathEditor.Logic;
using PAPathEditor.Gui;

namespace PAPathEditor
{
    public sealed class Window : GameWindow
    {
        public static Window Main { private set; get; }

        private NodesMain nodes;
        private ImGuiController imGuiController;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) 
        {
            Main = this;
        }

        public override void Run()
        {
            Shader.InitDefaults();

            LineRenderer.Init();
            NodeRenderer.Init();
            ArrowRenderer.Init();

            imGuiController = new ImGuiController();
            imGuiController.Init();

            nodes = new NodesMain();

            base.Run();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            RenderGlobals.View = Matrix4.Identity;
            RenderGlobals.Projection = Matrix4.CreateOrthographic(71.1111111111f, 40, -10.0f, 10.0f);

            Input.InputUpdate(KeyboardState, MouseState);

            nodes.Update();

            imGuiController.Update((float)args.Time);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Viewport(0, 0, Size.X, Size.Y);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            LineRenderer.Render(RenderGlobals.View, RenderGlobals.Projection);

            imGuiController.RenderImGui();

            SwapBuffers();
        }
    }
}
