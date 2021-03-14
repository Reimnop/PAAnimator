using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using PAAnimator.Gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PAAnimator.Logic
{
    public static class MainController
    {
        public static Project CurrentProject = new Project();

        public static NodesManager NodesManager;

        private static Vector2 cameraPosition;
        private static float zoomLevel = 18.0f;

        public static void Init()
        {
            NodesManager = new NodesManager();

            ImGuiController.RegisterImGui(ImGuiRender);
        }

        public static void Update()
        {
            if (Input.GetMouse(MouseButton.Button3))
            {
                cameraPosition -= NodesManager.MouseDeltaToView(Input.GetMouseDelta()) * 2.0f;
            }

            zoomLevel += Window.Main.MouseState.ScrollDelta.Y * 2.0f;

            zoomLevel = Math.Clamp(zoomLevel, 8.0f, 90.0f);

            RenderGlobals.View = Matrix4.CreateTranslation(-cameraPosition.X, -cameraPosition.Y, 0.0f);
            RenderGlobals.Projection = Matrix4.CreateOrthographic(Window.Main.Size.X / zoomLevel, Window.Main.Size.Y / zoomLevel, -10.0f, 10.0f);

            NodesManager.Update();
        }

        private static void ImGuiRender()
        {
            //dockspace
            ImGuiExtension.BeginGlobalDocking();
            ImGui.End();

            //project settings popup
            if (ImGui.BeginPopup("ProjectSettings"))
            {
                ImGui.Text("project settings");
                ImGui.EndPopup();
            }

            //main menu bar
            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    ImGui.MenuItem("Export to prefab...");

                    if (ImGui.BeginMenu("Import"))
                    {
                        if (ImGui.MenuItem("Background"))
                        {
                            using (var ofd = new OpenFileDialog())
                            {
                                ofd.Filter = "Image|*.png;*.jpg";

                                ofd.ShowDialog();

                                if (!string.IsNullOrEmpty(ofd.FileName))
                                {
                                    BackgroundRenderer.Background = new Texture().FromFile(ofd.FileName);
                                }
                            }
                        }

                        ImGui.End();
                    }

                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Edit"))
                {
                    if (ImGui.MenuItem("Project Settings"))
                    {
                        ImGui.OpenPopup("ProjectSettings");
                    }

                    ImGui.EndMenu();
                }

                ImGui.EndMainMenuBar();
            }

            NodesManager.RenderImGui();
        }
    }
}
