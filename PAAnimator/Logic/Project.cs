using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PAAnimator.Logic
{
    public class Project
    {
        public List<Node> Nodes = new List<Node>()
        {
            new Node("First node :D") { Time = 0, Position = new Vector2(11, 1.3f) },
            new Node("second :D") { Time = 1, Position = new Vector2(1.1f, 13) },
            new Node("and third!") { Time = 2, Position = new Vector2(13, -5.6f) }
        };
    }
}
