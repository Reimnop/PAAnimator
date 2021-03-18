using ImGuiNET;
using OpenTK.Mathematics;
using PAAnimator.Logic;
using System.Collections.Generic;

namespace PAAnimator.Gui
{
    public static class ImGuiExtension
    {
        public static void DragVector2(string label, ref Vector2 value, Vector2 defaultValue, float speed = 1.0f)
        {
            ImGui.PushID("X-btn");
            if (ImGui.Button("X"))
            {
                value.X = defaultValue.X;
            }
            ImGui.PopID();
            ImGui.SameLine();

            ImGui.PushID("X-drag");
            ImGui.SetNextItemWidth(75.0f);
            ImGui.DragFloat(string.Empty, ref value.X, speed);
            ImGui.PopID();

            ImGui.SameLine();

            ImGui.PushID("X-btn");
            if (ImGui.Button("Y"))
            {
                value.Y = defaultValue.Y;
            }
            ImGui.PopID();
            ImGui.SameLine();

            ImGui.PushID("Y-drag");
            ImGui.SetNextItemWidth(75.0f);
            ImGui.DragFloat(string.Empty, ref value.Y, speed);
            ImGui.PopID();

            ImGui.SameLine();
            ImGui.Text(label);
        }
        public static void DragVector3(string label, ref Vector3 value, Vector3 defaultValue, float speed = 1.0f)
        {
            ImGui.PushID("X-btn");
            if (ImGui.Button("X"))
            {
                value.X = defaultValue.X;
            }
            ImGui.PopID();
            ImGui.SameLine();

            ImGui.PushID("X-drag");
            ImGui.SetNextItemWidth(75.0f);
            ImGui.DragFloat(string.Empty, ref value.X, speed);
            ImGui.PopID();

            ImGui.SameLine();

            ImGui.PushID("X-btn");
            if (ImGui.Button("Y"))
            {
                value.Y = defaultValue.Y;
            }
            ImGui.PopID();
            ImGui.SameLine();

            ImGui.PushID("Y-drag");
            ImGui.SetNextItemWidth(75.0f);
            ImGui.DragFloat(string.Empty, ref value.Y, speed);
            ImGui.PopID();

            ImGui.SameLine();

            ImGui.PushID("Z-btn");
            if (ImGui.Button("Z"))
            {
                value.Z = defaultValue.Z;
            }
            ImGui.PopID();
            ImGui.SameLine();

            ImGui.PushID("Z-drag");
            ImGui.SetNextItemWidth(75.0f);
            ImGui.DragFloat(string.Empty, ref value.Z, speed);
            ImGui.PopID();

            ImGui.SameLine();
            ImGui.Text(label);
        }
        public static void DragVector4(string label, ref Vector4 value, Vector4 defaultValue, float speed = 1.0f)
        {
            ImGui.PushID("X-btn");
            if (ImGui.Button("X"))
            {
                value.X = defaultValue.X;
            }
            ImGui.PopID();
            ImGui.SameLine();

            ImGui.PushID("X-drag");
            ImGui.SetNextItemWidth(75.0f);
            ImGui.DragFloat(string.Empty, ref value.X, speed);
            ImGui.PopID();

            ImGui.SameLine();

            ImGui.PushID("X-btn");
            if (ImGui.Button("Y"))
            {
                value.Y = defaultValue.Y;
            }
            ImGui.PopID();
            ImGui.SameLine();

            ImGui.PushID("Y-drag");
            ImGui.SetNextItemWidth(75.0f);
            ImGui.DragFloat(string.Empty, ref value.Y, speed);
            ImGui.PopID();

            ImGui.SameLine();

            ImGui.PushID("Z-btn");
            if (ImGui.Button("Z"))
            {
                value.Z = defaultValue.Z;
            }
            ImGui.PopID();
            ImGui.SameLine();

            ImGui.PushID("Z-drag");
            ImGui.SetNextItemWidth(75.0f);
            ImGui.DragFloat(string.Empty, ref value.Z, speed);
            ImGui.PopID();

            ImGui.SameLine();

            ImGui.PushID("W-btn");
            if (ImGui.Button("W"))
            {
                value.W = defaultValue.W;
            }
            ImGui.PopID();
            ImGui.SameLine();

            ImGui.PushID("W-drag");
            ImGui.SetNextItemWidth(75.0f);
            ImGui.DragFloat(string.Empty, ref value.W, speed);
            ImGui.PopID();

            ImGui.SameLine();
            ImGui.Text(label);
        }
        public static void BeginGlobalDocking()
        {
            ImGuiViewportPtr viewport = ImGui.GetMainViewport();

            ImGui.SetNextWindowPos(viewport.Pos);
            ImGui.SetNextWindowSize(viewport.Size);
            ImGui.SetNextWindowViewport(viewport.ID);
            ImGui.SetNextWindowBgAlpha(0.0f);

            ImGuiWindowFlags windowFlags = ImGuiWindowFlags.NoDocking | ImGuiWindowFlags.MenuBar;

            windowFlags |= ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse;
            windowFlags |= ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove;
            windowFlags |= ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus;

            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0.0f);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0.0f);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, System.Numerics.Vector2.Zero);
            ImGui.Begin("imgui-docking", windowFlags);
            ImGui.PopStyleVar(3);

            uint dockspaceID = ImGui.GetID("default-dockspace");
            ImGuiDockNodeFlags dockspaceFlags = ImGuiDockNodeFlags.PassthruCentralNode;
            ImGui.DockSpace(dockspaceID, System.Numerics.Vector2.Zero, dockspaceFlags);
        }

        #region Timeline
        public static void Timeline(ref float time, float min, float max, List<Node> nodes)
        {
            ImGuiIOPtr io = ImGui.GetIO();
            ImDrawListPtr drawList = ImGui.GetWindowDrawList();

            var pos = ImGui.GetCursorScreenPos();
            var avail = ImGui.GetContentRegionAvail();

            var mousePos = ImGui.GetMousePos();

            float w = avail.X;

            drawList.PushClipRect(
                pos,
                pos + new System.Numerics.Vector2(w, 56));

            //controls
            if (CheckRect(
                pos,
                pos + new System.Numerics.Vector2(w, 24),
                io.MouseClickedPos[0]) && ImGui.IsMouseDragging(ImGuiMouseButton.Left))
            {
                var mousePosRelative = mousePos - pos;
                time = mousePosRelative.X / w * (max - min) + min;

                time = System.Math.Clamp(time, min, max);
            }

            //frame
            drawList.AddRectFilled(
                pos, 
                pos + new System.Numerics.Vector2(w, 24), ImGui.GetColorU32(ImGuiCol.FrameBg));

            drawList.AddRect(
                pos + new System.Numerics.Vector2(0, 24), 
                pos + new System.Numerics.Vector2(w, 56), ImGui.GetColorU32(ImGuiCol.FrameBg));

            //draw nodes
            foreach (var node in nodes)
            {
                float nPos = (node.Time - min) / (max - min) * w;

                drawList.AddQuadFilled(
                    pos + new System.Numerics.Vector2(nPos - 6, 40),
                    pos + new System.Numerics.Vector2(nPos, 34),
                    pos + new System.Numerics.Vector2(nPos + 6, 40),
                    pos + new System.Numerics.Vector2(nPos, 46), ImGui.GetColorU32(ImGuiCol.FrameBg));
            }

            //handle
            float hPos = (time - min) / (max - min) * w;

            drawList.AddRectFilled(
                pos + new System.Numerics.Vector2(hPos - 8, 0),
                pos + new System.Numerics.Vector2(hPos + 8, 16), ImGui.GetColorU32(ImGuiCol.SliderGrab));

            drawList.AddLine(
                pos + new System.Numerics.Vector2(hPos, 24),
                pos + new System.Numerics.Vector2(hPos, 54), ImGui.GetColorU32(ImGuiCol.SliderGrab), 2.0f);

            drawList.AddTriangleFilled(
                pos + new System.Numerics.Vector2(hPos - 8, 16),
                pos + new System.Numerics.Vector2(hPos + 8, 16),
                pos + new System.Numerics.Vector2(hPos, 26), ImGui.GetColorU32(ImGuiCol.SliderGrab));

            ImGui.PopClipRect();
        }

        private static bool CheckRect(System.Numerics.Vector2 lowerLeft, System.Numerics.Vector2 upperRight, System.Numerics.Vector2 pos)
        {
            if (pos.X > lowerLeft.X && pos.Y > lowerLeft.Y &&
                pos.X < upperRight.X && pos.Y < upperRight.Y)
                return true;

            return false;
        }
        #endregion
    }
}
