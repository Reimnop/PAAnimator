using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAPathEditor.Logic
{
    public static class MainController
    {
        public static NodesMain Nodes;

        private static Vector2 cameraPosition;
        private static float zoomLevel = 18.0f;

        public static void Init()
        {
            Nodes = new NodesMain();
        }

        public static void Update()
        {
            if (Input.GetMouse(MouseButton.Button3))
            {
                cameraPosition -= NodesMain.MouseDeltaToView(Input.GetMouseDelta()) * 2.0f;
            }

            zoomLevel += Window.Main.MouseState.ScrollDelta.Y * 2.0f;

            zoomLevel = Math.Clamp(zoomLevel, 8.0f, 90.0f);

            RenderGlobals.View = Matrix4.CreateTranslation(-cameraPosition.X, -cameraPosition.Y, 0.0f);
            RenderGlobals.Projection = Matrix4.CreateOrthographic(Window.Main.Size.X / zoomLevel, Window.Main.Size.Y / zoomLevel, -10.0f, 10.0f);

            Nodes.Update();
        }
    }
}
