using System;
using System.Collections.Generic;
using ImGuiNET;
using System.Text;

namespace PAAnimator.Logic
{
    public static class DocumentationManager
    {
        public static bool WindowOpen = false;

        private static List<(string, string)> sections = new List<(string, string)>();

        private static int sectionIndex = 0;

        public static void Init(string docs)
        {
            string[] lines = docs.Split(Environment.NewLine);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                if (line.StartsWith("##"))
                {
                    sections.Add((line.Substring(2), string.Empty));
                    continue;
                }

                var str = sections[sections.Count - 1];
                str.Item2 += line + Environment.NewLine;
                sections[sections.Count - 1] = str;
            }
        }

        public static void RenderImGui()
        {
            if (WindowOpen)
                if (ImGui.Begin("Documentation", ref WindowOpen))
                {
                    if (ImGui.BeginChild("Sections", new System.Numerics.Vector2(175.0f, 0.0f), true))
                    {
                        for (int i = 0; i < sections.Count; i++)
                        {
                            if (ImGui.Selectable(sections[i].Item1, sectionIndex == i))
                                sectionIndex = i;
                        }
                        ImGui.EndChild();
                    }

                    ImGui.SameLine();

                    if (ImGui.BeginChild("Information"))
                    {
                        ImGui.TextWrapped(sections[sectionIndex].Item2);
                        ImGui.EndChild();
                    }

                    ImGui.End();
                }
        }
    }
}
