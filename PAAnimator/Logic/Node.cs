using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using PAPrefabToolkit.Data;
using System;

namespace PAAnimator.Logic
{
    [Serializable]
    public class Node
    {
        public float Time;

        public Vector2 Position = Vector2.Zero;
        public Vector2 Scale = Vector2.One;
        public float Rotation = 0.0f;

        public PrefabObjectEasing PositionEasing = PrefabObjectEasing.Linear;
        public PrefabObjectEasing ScaleEasing = PrefabObjectEasing.Linear;
        public PrefabObjectEasing RotationEasing = PrefabObjectEasing.Linear;

        public string Name;

        [NonSerialized]
        public Point Point;

        [NonSerialized]
        private bool dragging;

        [NonSerialized]
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

                Point.Highlighted = true;
            }
            else
            {
                Point.Highlighted = false;
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
