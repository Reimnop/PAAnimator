using OpenTK.Mathematics;
using PAPrefabToolkit;
using PAPrefabToolkit.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

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

        public string ProjectName = "New Project";

        public bool ProjectSettingsOpen = false;

        public Vector2 BackgroundOffset = Vector2.Zero;
        public Vector2 BackgroundScale = new Vector2(20.0f * 16.0f / 9.0f, 20.0f);
        public float BackgroundRotation = 0.0f;

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

            for (int i = 0; i < Nodes.Count; i++)
            {
                Node node = Nodes[i];

                prefabObject.ObjectEvents.PositionEvents.Add(new PrefabObject.Events.PositionEvent
                {
                    Time = node.Time,
                    X = node.Position.X,
                    Y = node.Position.Y
                });

                prefabObject.ObjectEvents.ScaleEvents.Add(new PrefabObject.Events.ScaleEvent
                {
                    Time = node.Time,
                    X = node.Scale.X,
                    Y = node.Scale.Y
                });

                prefabObject.ObjectEvents.RotationEvents.Add(new PrefabObject.Events.RotationEvent
                {
                    Time = node.Time,
                    X = node.Rotation
                });
            }

            return PrefabBuilder.BuildPrefab(prefab, noValidate: true);
        }
    }
}
