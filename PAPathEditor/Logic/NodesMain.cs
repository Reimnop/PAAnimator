using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PAPathEditor.Logic
{
    public class NodesMain
    {
        private List<Node> nodes = new List<Node>()
        {
            new Node() { Position = new Vector2(11, 1.3f), Id = 0 },
            new Node() { Position = new Vector2(1.1f, 13), Id = 1 },
            new Node() { Position = new Vector2(13, -5.6f), Id = 2 }
        };

        public void Update()
        {
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

            Vector2 viewPos = (new Vector4(ndc) * RenderGlobals.Projection.Inverted()).Xy;
            viewPos.Y = -viewPos.Y;

            return viewPos;
        }
    }
}
