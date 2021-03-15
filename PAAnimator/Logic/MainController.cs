using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using PAAnimator.Gui;
using System;
using System.IO;
using System.Windows.Forms;

namespace PAAnimator.Logic
{
    public static class MainController
    {
        private static Vector2 cameraPosition;
        private static float zoomLevel = 18.0f;

        public static void Init()
        {
            NodesManager.Init();

            DocumentationManager.Init(File.ReadAllText("Assets/Documentation.txt"));

            ImGuiController.RegisterImGui(ImGuiRender);
        }

        public static void Update()
        {
            if (Input.GetMouse(MouseButton.Button3))
            {
                cameraPosition -= NodesManager.MouseDeltaToView(Input.GetMouseDelta()) * 2.0f;
            }

            if (!ImGui.GetIO().WantCaptureMouse)
                zoomLevel += Window.Main.MouseState.ScrollDelta.Y * 2.0f;

            zoomLevel = Math.Clamp(zoomLevel, 8.0f, 90.0f);

            RenderGlobals.View = Matrix4.CreateTranslation(-cameraPosition.X, -cameraPosition.Y, 0.0f);
            RenderGlobals.Projection = Matrix4.CreateOrthographic(Window.Main.ClientSize.X / zoomLevel, Window.Main.ClientSize.Y / zoomLevel, -10.0f, 10.0f);

            Window.Main.Title = "Project Arrhythmia Animator | " + ProjectManager.CurrentProject.ProjectName;

            NodesManager.Update();
        }

        private static void ImGuiRender()
        {
            //dockspace
            ImGuiExtension.BeginGlobalDocking();
            ImGui.End();

            //main menu bar
            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("Open"))
                        ProjectManager.OpenProject();

                    if (ImGui.MenuItem("Save"))
                        ProjectManager.SaveProject();

                    if (ImGui.MenuItem("Save as"))
                        ProjectManager.SaveProject(true);

                    if (ImGui.MenuItem("Export to prefab..."))
                    {
                        using (var sfd = new SaveFileDialog())
                        {
                            sfd.Filter = "Prefab|*.lsp";

                            sfd.ShowDialog();

                            if (!string.IsNullOrEmpty(sfd.FileName))
                            {
                                File.WriteAllText(sfd.FileName, ProjectManager.CurrentProject.ToPrefab());
                            }
                        }
                    }

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
                        ProjectManager.CurrentProject.ProjectSettingsOpen = true;
                    }

                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Help"))
                {
                    if (ImGui.MenuItem("Documentation"))
                    {
                        DocumentationManager.WindowOpen = true;
                    }
                    ImGui.EndMenu();
                }

                ImGui.EndMainMenuBar();
            }

            ProjectManager.RenderImGui();
            NodesManager.RenderImGui();

            DocumentationManager.RenderImGui();
        }
    }
}
