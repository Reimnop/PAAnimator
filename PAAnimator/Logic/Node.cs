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
        private Vector2 temp;

        public Node(string name)
        {
            Name = name;
        }

        public void Check(Vector2 viewPos)
        {
            if (CheckPoint(viewPos) && Input.GetMouseDown(MouseButton.Button1))
            {
                NodesManager.SelectedNode = this;
                NodesManager.CurrentlyDragging = this;

                temp = Position;
            }
        }

        public void Update(Vector2 viewPos)
        {
            Point.Position = Position;

            if (CheckPoint(viewPos))
                Point.Highlighted = true;
            else
                Point.Highlighted = false;
        }

        public void OnDrag()
        {
            if (Input.GetMouseUp(MouseButton.Button1))
            {
                NodesManager.CurrentlyDragging = null;
                return;
            }
            
            temp += NodesManager.MouseDeltaToView(Window.Main.MouseState.Delta) * 2.0f;
            Position = temp;

            if (Input.GetKey(Keys.LeftControl))
            {
                Position.X = MathF.Round(Position.X);
                Position.Y = MathF.Round(Position.Y);
            }
        }

        public bool CheckPoint(Vector2 pos)
        {
            Vector2 lowerLeft = Position - new Vector2(0.5f, 0.5f);
            Vector2 upperRight = Position + new Vector2(0.5f, 0.5f);

            if (pos.X > lowerLeft.X && pos.Y > lowerLeft.Y &&
                pos.X < upperRight.X && pos.Y < upperRight.Y)
                return true;

            return false;
        }
    }
}
