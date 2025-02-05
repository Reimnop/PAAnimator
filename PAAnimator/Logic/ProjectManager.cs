﻿using ImGuiNET;
using OpenTK.Mathematics;
using PAAnimator.Gui;
using System.Windows.Forms;

using Keys = OpenTK.Windowing.GraphicsLibraryFramework.Keys;

namespace PAAnimator.Logic
{
    public static class ProjectManager
    {
        public static string CurrentProjectPath = null;
        public static Project CurrentProject = new Project();

        public static void Update()
        {
            if (Input.GetKeyCombo(Keys.LeftControl, Keys.S)) 
                SaveProject();

            if (Input.GetKeyCombo(Keys.LeftControl, Keys.LeftShift, Keys.S))
                SaveProject(true);

            if (Input.GetKeyCombo(Keys.LeftControl, Keys.O))
                OpenProject();
        }

        public static void RenderImGui()
        {
            //project settings popup
            if (CurrentProject.ProjectSettingsOpen)
                if (ImGui.Begin("Project Settings", ref CurrentProject.ProjectSettingsOpen))
                {
                    ImGui.InputText("Name", ref CurrentProject.ProjectName, 256);

                    ImGui.Text("Background");

                    ImGui.PushID("background-offset");
                    ImGuiExtension.DragVector2("Offset", ref CurrentProject.BackgroundOffset, Vector2.Zero);
                    ImGui.PopID();

                    ImGui.PushID("background-scale");
                    ImGuiExtension.DragVector2("Scale", ref CurrentProject.BackgroundScale, new Vector2(20.0f * 16.0f / 9.0f, 20.0f));
                    ImGui.PopID();

                    ImGui.PushID("background-rotation");
                    ImGui.DragFloat("Rotation", ref CurrentProject.BackgroundRotation);
                    ImGui.PopID();

                    ImGui.Text("Preview object");

                    ImGui.PushID("preview-pos");
                    ImGuiExtension.DragVector2("Position", ref CurrentProject.PreviewPosition, Vector2.Zero);
                    ImGui.PopID();

                    ImGui.PushID("preview-scale");
                    ImGuiExtension.DragVector2("Scale", ref CurrentProject.PreviewScale, new Vector2(20.0f * 16.0f / 9.0f, 20.0f));
                    ImGui.PopID();

                    ImGui.PushID("preview-rotation");
                    ImGui.DragFloat("Rotation", ref CurrentProject.PreviewRotation);
                    ImGui.PopID();

                    ImGui.End();
                }
        }

        public static void OpenProject()
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Project Arrhythmia Animator Project|*.paanim";

                ofd.ShowDialog();

                if (!string.IsNullOrEmpty(ofd.FileName))
                {
                    CurrentProjectPath = ofd.FileName;
                    CurrentProject = Project.FromFile(ofd.FileName);
                }
            }
        }

        public static void SaveProject(bool diff = false)
        {
            bool saveAs = diff || string.IsNullOrEmpty(CurrentProjectPath);

            if (saveAs)
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Project Arrhythmia Animator Project|*.paanim";

                    sfd.ShowDialog();

                    if (!string.IsNullOrEmpty(sfd.FileName))
                    {
                        CurrentProjectPath = sfd.FileName;
                        CurrentProject.SerializeToFile(sfd.FileName);
                    }
                }
                return;
            }

            CurrentProject.SerializeToFile(CurrentProjectPath);
        }
    }
}
