using System;
using System.Collections.Generic;
using OpenTK.Mathematics;
using System.Text;
using Glide;

namespace PAAnimator.Logic.Animation
{
    public struct FrameData
    {
        public Vector2 Position;
        public Vector2 Scale;
        public float Rotation;
    }

    public static class Animator
    {
        public static FrameData GetCurrentFrameData(float time)
        {
            GetFirstAndLastNodes(time, out var first, out var last);

            List<Node> nodes = ProjectManager.CurrentProject.Nodes;

            if (first == null)
                return new FrameData
                {
                    Position = nodes[0].Position,
                    Scale = nodes[0].Scale,
                    Rotation = nodes[0].Rotation
                };

            if (last == null)
                return new FrameData
                {
                    Position = nodes[nodes.Count - 1].Position,
                    Scale = nodes[nodes.Count - 1].Scale,
                    Rotation = nodes[nodes.Count - 1].Rotation
                };

            float length = last.Time - first.Time;
            float t = (time - first.Time) / length;

            Func<float, float> posEaseFunc = Ease.ConversionTable[last.PositionEasing];
            Func<float, float> scaEaseFunc = Ease.ConversionTable[last.ScaleEasing];
            Func<float, float> rotEaseFunc = Ease.ConversionTable[last.RotationEasing];

            Vector2 pos = Vector2.Zero;
            Vector2 sca = Vector2.One;
            float rot = 1.0f;
            //pos ease
            if (first.Bezier)
            {
                //get control points
                Vector2[] controls = new Vector2[first.Controls.Count + 2];

                controls[0] = first.Position;
                controls[first.Controls.Count + 1] = last.Position;

                for (int j = 0; j < first.Controls.Count; j++)
                {
                    controls[j + 1] = first.Position + first.Controls[j];
                }

                //calculate Bezier
                pos = Helper.Bezier(controls, posEaseFunc(t));
            }
            else
            {
                float _t = posEaseFunc(t);

                pos.X = Lerp(first.Position.X, last.Position.X, _t);
                pos.Y = Lerp(first.Position.Y, last.Position.Y, _t);
            }
            //sca ease
            {
                float _t = scaEaseFunc(t);

                sca.X = Lerp(first.Scale.X, last.Scale.X, _t);
                sca.Y = Lerp(first.Scale.Y, last.Scale.Y, _t);
            }
            //rot ease
            {
                float _t = rotEaseFunc(t);

                rot = Lerp(first.Rotation, last.Rotation, _t);
            }

            return new FrameData
            {
                Position = pos,
                Scale = sca,
                Rotation = rot
            };
        }

        private static void GetFirstAndLastNodes(float time, out Node first, out Node last)
        {
            List<Node> nodes = ProjectManager.CurrentProject.Nodes;

            Node f = null, l = null;

            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Time < time)
                    f = nodes[i];
                else break;
            }

            for (int i = nodes.Count - 1; i > 0; i--)
            {
                if (nodes[i].Time >= time)
                    l = nodes[i];
                else break;
            }

            first = f;
            last = l;
        }

        private static float Lerp(float v0, float v1, float t)
        {
            return (1 - t) * v0 + t * v1;
        }
    }
}
