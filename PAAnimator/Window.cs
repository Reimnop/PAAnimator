using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using PAAnimator.Gui;
using PAAnimator.Logic;

namespace PAAnimator
{
    public static class Time
    {
        public static float DeltaTime;
    }

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
            PreviewRenderer.Init();
            LineRenderer.Init();
            NodeRenderer.Init();
            ArrowRenderer.Init();
            BezierControlPointsRenderer.Init();

            imGuiController = new ImGuiController();
            imGuiController.Init();

            MainController.Init();

            base.Run();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            Time.DeltaTime = (float)args.Time;

            ThreadManager.ExecuteAll();

            Input.InputUpdate(KeyboardState, MouseState);

            MainController.Update();

            imGuiController.Update(Time.DeltaTime);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Viewport(0, 0, Size.X, Size.Y);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            //enable states
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            BackgroundRenderer.Render(RenderGlobals.View, RenderGlobals.Projection);
            GridRenderer.Render(RenderGlobals.View, RenderGlobals.Projection);
            PreviewRenderer.Render(RenderGlobals.View, RenderGlobals.Projection);
            LineRenderer.Render(RenderGlobals.View, RenderGlobals.Projection);
            BezierControlPointsRenderer.Render(RenderGlobals.View, RenderGlobals.Projection);

            imGuiController.RenderImGui();

            SwapBuffers();
        }
    }
}
