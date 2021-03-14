using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using ImGuiNET;
using PAAnimator.Gui;
using System.Text;

namespace PAAnimator.Logic
{
    public class NodesManager
    {
        public Node SelectedNode;

        public void RenderImGui()
        {
            Project prj = ProjectManager.CurrentProject;

            if (ImGui.Begin("Nodes"))
            {
                if (ImGui.Button("Add Node"))
                {
                    ThreadManager.ExecuteOnMainThread(() => prj.Nodes.Add(new Node("Untitled Node")));
                }

                for (int i = 0; i < prj.Nodes.Count; i++)
                    RenderNodeProp(prj.Nodes[i]);

                ImGui.End();
            }

            if (ImGui.Begin("Node Editor"))
            {
                if (SelectedNode == null)
                {
                    ImGui.Text("No node selected");
                    goto EndNodeEditWindow;
                }

                ImGui.InputText("Name", ref SelectedNode.Name, 256);
                ImGui.DragFloat("Time", ref SelectedNode.Time);

                ImGui.Text("Transform");

                ImGui.PushID("node-position");
                ImGuiExtension.DragVector2("Position", ref SelectedNode.Position, Vector2.Zero);
                ImGui.PopID();

                ImGui.PushID("node-scale");
                ImGuiExtension.DragVector2("Scale", ref SelectedNode.Scale, Vector2.One);
                ImGui.PopID();

                ImGui.PushID("node-rotation");
                ImGui.DragFloat("Rotation", ref SelectedNode.Rotation);
                ImGui.PopID();

                if (SelectedNode.Time < 0)
                    SelectedNode.Time = 0;

                EndNodeEditWindow:
                ImGui.End();
            }
        }

        private void RenderNodeProp(Node node)
        {
            if (ImGui.Selectable($"Time: {node.Time} // {node.Name}", SelectedNode == node))
                SelectedNode = node;
        }

        public void Update()
        {
            Project prj = ProjectManager.CurrentProject;

            if (Input.GetKeyDown(Keys.Delete) && SelectedNode != null)
            {
                prj.Nodes.Remove(SelectedNode);
                SelectedNode = null;
            }

            //undo
            if ((Input.GetKey(Keys.LeftControl) && Input.GetKeyDown(Keys.Z)) ||
                (Input.GetKeyDown(Keys.LeftControl) && Input.GetKey(Keys.Z)))
            {
                UndoManager.Undo();
            }

            Vector2 rawPos = Input.GetMousePosition();
            Vector2 viewPos = MouseToView(rawPos);

            if (Input.GetMouseDown(MouseButton.Button1) && !ImGui.GetIO().WantCaptureMouse)
                SelectedNode = null;

            prj.Nodes.ForEach(x =>
            {
                x.Update(viewPos);
            });

            prj.Nodes.Sort((x, y) => x.Time.CompareTo(y.Time));

            PointDrawData drawData;
            drawData.Points = new Point[prj.Nodes.Count];
            for (int i = 0; i < drawData.Points.Length; i++)
            {
                if (prj.Nodes[i] == SelectedNode)
                {
                    prj.Nodes[i].Point.Highlighted = true;
                }

                drawData.Points[i] = prj.Nodes[i].Point;
            }

            LineRenderer.PushDrawData(drawData);
        }

        public static Vector2 MouseToView(Vector2 rawPos)
        {
            Vector2 ndc = new Vector2(rawPos.X / Window.Main.ClientSize.X, rawPos.Y / Window.Main.ClientSize.Y);
            ndc = ndc * 2.0f - Vector2.One;
            ndc.Y = -ndc.Y;

            Vector2 viewPos = (new Vector4(ndc) * RenderGlobals.Projection.Inverted()).Xy - RenderGlobals.View.ExtractTranslation().Xy; //kind of a hack.

            return viewPos;
        }

        public static Vector2 MouseDeltaToView(Vector2 delta)
        {
            Vector2 ndc = new Vector2(delta.X / Window.Main.ClientSize.X, delta.Y / Window.Main.ClientSize.Y);
            ndc.Y = -ndc.Y;

            Vector2 viewPos = (new Vector4(ndc) * RenderGlobals.Projection.Inverted()).Xy;

            return viewPos;
        }
    }
}
