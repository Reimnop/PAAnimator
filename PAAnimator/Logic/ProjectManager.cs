using System;
using System.Collections.Generic;
using System.Text;
using ImGuiNET;
using OpenTK.Mathematics;
using PAAnimator.Gui;
using PAPrefabToolkit;
using PAPrefabToolkit.Data;

namespace PAAnimator.Logic
{
    public static class ProjectManager
    {
        public static Project CurrentProject = new Project();

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

                    ImGui.End();
                }
        }
    }
}
