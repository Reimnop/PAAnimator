using Glide;
using OpenTK.Mathematics;
using PAPrefabToolkit;
using PAPrefabToolkit.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PAAnimator.Logic
{
    [Serializable]
    public class Project
    {
        public List<Node> Nodes = new List<Node>()
        {
            new Node("First node :D") { Time = 0, Position = new Vector2(11, 1.3f) },
            new Node("second :D") { Time = 1, Position = new Vector2(1.1f, 13) },
            new Node("and third!") { Time = 2, Position = new Vector2(13, -5.6f) }
        };

        public string ProjectName = "Untitled Project";

        public bool ProjectSettingsOpen = false;

        public Vector2 BackgroundOffset = Vector2.Zero;
        public Vector2 BackgroundScale = new Vector2(20.0f * 16.0f / 9.0f, 20.0f);
        public float BackgroundRotation = 0.0f;

        public Vector2 PreviewPosition = Vector2.Zero;
        public Vector2 PreviewScale = Vector2.One;
        public float PreviewRotation = 0.0f;

        public float Time = 0.0f;

        public void SerializeToFile(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, this);

                fs.Flush();
            }
        }

        public static Project FromFile(string path)
        {
            Project prj;

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                prj = (Project)formatter.Deserialize(fs);

                fs.Flush();
            }

            return prj;
        }

        public string ToPrefab()
        {
            Prefab prefab = new Prefab();
            prefab.Name = ProjectName;

            PrefabObject prefabObject = new PrefabObject(prefab, ProjectName);
            prefabObject.AutoKillType = PrefabObjectAutoKillType.LastKeyframe;
            prefabObject.ObjectType = PrefabObjectType.Empty;

            prefabObject.ObjectEvents.PositionEvents.Clear();
            prefabObject.ObjectEvents.ScaleEvents.Clear();
            prefabObject.ObjectEvents.RotationEvents.Clear();

            float cRot = 0.0f;

            float timeOffset = Nodes[0].Time;

            for (int i = 0; i < Nodes.Count; i++)
            {
                Node node = Nodes[i];

                #region Position
                if (!node.Bezier || i == Nodes.Count - 1)
                    prefabObject.ObjectEvents.PositionEvents.Add(new PrefabObject.Events.PositionEvent
                    {
                        Time = node.Time - timeOffset,
                        X = node.Position.X,
                        Y = node.Position.Y,
                        CurveType = i > 0 && Nodes[i - 1].Bezier ? PrefabObjectEasing.Linear : node.PositionEasing 
                    });
                else if (i != Nodes.Count - 1)
                {
                    //calculate length
                    float l = Nodes[i + 1].Time - node.Time;

                    //get control points
                    Vector2[] controls = new Vector2[node.Controls.Count + 2];

                    controls[0] = node.Position;
                    controls[node.Controls.Count + 1] = Nodes[i + 1].Position;

                    for (int j = 0; j < node.Controls.Count; j++)
                    {
                        controls[j + 1] = node.Position + node.Controls[j];
                    }

                    Func<float, float> easeFunc = Ease.ConversionTable[Nodes[i + 1].PositionEasing];

                    //calculate Bezier
                    for (float t = 0.0f; t < 1.0f; t += 0.025f)
                    {
                        Vector2 v = Helper.Bezier(controls, easeFunc(t));

                        if (t != 0.0f)
                            prefabObject.ObjectEvents.PositionEvents.Add(new PrefabObject.Events.PositionEvent
                            {
                                Time = node.Time + t * l - timeOffset,
                                X = v.X,
                                Y = v.Y,
                                CurveType = PrefabObjectEasing.Linear
                            });
                        else
                            prefabObject.ObjectEvents.PositionEvents.Add(new PrefabObject.Events.PositionEvent
                            {
                                Time = node.Time + t * l - timeOffset,
                                X = v.X,
                                Y = v.Y,
                                CurveType = node.PositionEasing
                            });
                    }
                }
                #endregion

                prefabObject.ObjectEvents.ScaleEvents.Add(new PrefabObject.Events.ScaleEvent
                {
                    Time = node.Time - timeOffset,
                    X = node.Scale.X,
                    Y = node.Scale.Y,
                    CurveType = node.ScaleEasing
                });

                prefabObject.ObjectEvents.RotationEvents.Add(new PrefabObject.Events.RotationEvent
                {
                    Time = node.Time - timeOffset,
                    X = node.Rotation - cRot,
                    CurveType = node.RotationEasing
                });

                cRot = node.Rotation;
            }

            return PrefabBuilder.BuildPrefab(prefab, noValidate: true);
        }
    }
}
