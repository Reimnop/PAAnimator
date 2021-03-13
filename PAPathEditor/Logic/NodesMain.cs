using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using ImGuiNET;
using PAPathEditor.Gui;
using System.Text;

namespace PAPathEditor.Logic
{
    public class NodesMain : IDisposable
    {
        private List<Node> nodes = new List<Node>()
        {
            new Node() { Time = 0, Position = new Vector2(11, 1.3f), Id = 0 },
            new Node() { Time = 1, Position = new Vector2(1.1f, 13), Id = 1 },
            new Node() { Time = 2, Position = new Vector2(13, -5.6f), Id = 2 }
        };

        public Node SelectedNode;

        public NodesMain()
        {
            ImGuiController.RegisterImGui(RenderImGui);
        }

        public void Dispose()
        {
            ImGuiController.UnregisterImGui(RenderImGui);
        }

        private void RenderImGui()
        {
            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    ImGui.MenuItem("Export to prefab...");

                    ImGui.EndMenu();
                }

                ImGui.EndMainMenuBar();
            }

            if (ImGui.Begin("Nodes"))
            {
                if (ImGui.Button("Add Node"))
                {
                    ThreadManager.ExecuteOnMainThread(() => nodes.Add(new Node()));
                }

                for (int i = 0; i < nodes.Count; i++)
                    RenderNodeProp(nodes[i]);

                ImGui.End();
            }

            if (ImGui.Begin("Node Editor"))
            {
                if (SelectedNode == null)
                {
                    ImGui.Text("No node selected");
                    goto EndNodeEditWindow;
                }

                ImGui.DragFloat("Time", ref SelectedNode.Time);
                ImGuiExtension.DragVector2("Position", ref SelectedNode.Position, Vector2.Zero);

                ImGui.EndPopup();

                if (SelectedNode.Time < 0)
                    SelectedNode.Time = 0;

                EndNodeEditWindow:
                ImGui.End();
            }
        }

        private void RenderNodeProp(Node node)
        {
            if (ImGui.Selectable($"Time: {node.Time} // Position: {node.Position}", SelectedNode == node))
                SelectedNode = node;
        }

        public void Update()
        {
            if (Input.GetKeyDown(Keys.Delete) && SelectedNode != null)
            {
                nodes.Remove(SelectedNode);
                SelectedNode.Dispose();
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

            nodes.ForEach(x =>
            {
                x.Update(viewPos);
            });

            nodes.Sort((x, y) => x.Time.CompareTo(y.Time));

            PointDrawData drawData;
            drawData.Points = new Point[nodes.Count];
            for (int i = 0; i < drawData.Points.Length; i++)
            {
                drawData.Points[i] = nodes[i].Point;
            }

            LineRenderer.PushDrawData(drawData);
        }

        public static Vector2 MouseToView(Vector2 rawPos)
        {
            Vector2 ndc = new Vector2(rawPos.X / Window.Main.Size.X, rawPos.Y / Window.Main.Size.Y);
            ndc = ndc * 2.0f - Vector2.One;
            ndc.Y = -ndc.Y;

            Vector2 viewPos = (new Vector4(ndc) * RenderGlobals.Projection.Inverted()).Xy - RenderGlobals.View.ExtractTranslation().Xy; //kind of a hack.

            return viewPos;
        }

        public static Vector2 MouseDeltaToView(Vector2 delta)
        {
            Vector2 ndc = new Vector2(delta.X / Window.Main.Size.X, delta.Y / Window.Main.Size.Y);
            ndc.Y = -ndc.Y;

            Vector2 viewPos = (new Vector4(ndc) * RenderGlobals.Projection.Inverted()).Xy;

            return viewPos;
        }
    }
}
