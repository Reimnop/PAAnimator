using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using PAAnimator.Logic;
using PAAnimator.Gui;

namespace PAAnimator
{
    public sealed class Window : GameWindow
    {
        public static Window Main { private set; get; }
        
        private ImGuiController imGuiController;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) 
        {
            Main = this;
        }

        public override void Run()
        {
            Shader.InitDefaults();

            BackgroundRenderer.Init();
            GridRenderer.Init();
            LineRenderer.Init();
            NodeRenderer.Init();
            ArrowRenderer.Init();

            imGuiController = new ImGuiController();
            imGuiController.Init();

            MainController.Init();

            base.Run();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            ThreadManager.ExecuteAll();

            Input.InputUpdate(KeyboardState, MouseState);

            MainController.Update();

            imGuiController.Update((float)args.Time);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Viewport(0, 0, Size.X, Size.Y);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            BackgroundRenderer.Render(RenderGlobals.View, RenderGlobals.Projection);
            GridRenderer.Render(RenderGlobals.View, RenderGlobals.Projection);
            LineRenderer.Render(RenderGlobals.View, RenderGlobals.Projection);

            imGuiController.RenderImGui();

            SwapBuffers();
        }
    }
}
