﻿using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using PAAnimator.Gui;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Text;

namespace PAAnimator.Logic
{
    public class Node
    {
        public Vector2 Position;
        public Point Point;
        public float Time;

        public string Name;

        private bool dragging;

        private Vector2 currentViewPos;

        public Node(string name)
        {
            Name = name;
        }

        public void Update(Vector2 viewPos)
        {
            currentViewPos = viewPos;

            if (CheckPoint(currentViewPos))
            {
                if (Input.GetMouseDown(MouseButton.Button1))
                {
                    MainController.NodesManager.SelectedNode = this;

                    Vector2 oldPos = Position;
                    UndoManager.PushUndo(() => Position = oldPos);
                    dragging = true;
                }

                Point.Highlighted = 1;
            }
            else
            {
                Point.Highlighted = 0;
            }

            if (dragging && Input.GetMouseUp(MouseButton.Button1))
                dragging = false;

            if (dragging)
            {
                Vector2 pos = NodesManager.MouseToView(Input.GetMousePosition());

                if (Input.GetKey(Keys.LeftControl))
                {
                    pos.X = MathF.Round(pos.X);
                    pos.Y = MathF.Round(pos.Y);
                }

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