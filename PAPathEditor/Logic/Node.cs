using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using PAPathEditor.Gui;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Text;

namespace PAPathEditor.Logic
{
    public class Node
    {
        public Vector2 Position;
        public Point Point;
        public float Time;

        public ulong Id;

        private bool dragging;

        private Vector2 currentViewPos;

        public Node()
        {
            ImGuiController.RegisterImGui(ImGuiFunc);
        }

        private void ImGuiFunc()
        {
            if (CheckPoint(currentViewPos))
                if (Input.GetMouseDown(MouseButton.Button2))
                {
                    ImGui.OpenPopup("NodeConfig" + Id);
                }

            if (ImGui.BeginPopup("NodeConfig" + Id))
            {
                ImGui.DragFloat("Time", ref Time);
                ImGuiExtension.DragVector2("Position", ref Position, Vector2.Zero);

                ImGui.EndPopup();
            }

            if (Time < 0)
                Time = 0;
        }

        public void Update(Vector2 viewPos)
        {
            currentViewPos = viewPos;

            if (CheckPoint(currentViewPos))
            {
                if (Input.GetMouseDown(MouseButton.Button1))
                {
                    dragging = true;
                }

                Point.Highlighted = 1;
            }
            else
            {
                Point.Highlighted = 0;
            }

            if (Input.GetMouseUp(MouseButton.Button1))
            {
                dragging = false;
            }

            if (dragging)
            {
                Vector2 pos = NodesMain.MouseToView(Input.GetMousePosition());
                Position = pos;
            }

            Point.Position = Position;
        }

        public bool CheckPoint(Vector2 pos)
        {
            Vector2 lowerLeft = Position - new Vector2(0.4f, 0.4f);
            Vector2 upperRight = Position + new Vector2(0.4f, 0.4f);

            if (pos.X > lowerLeft.X && pos.Y > lowerLeft.Y &&
                pos.X < upperRight.X && pos.Y < upperRight.Y)
                return true;

            return false;
        }
    }
}
