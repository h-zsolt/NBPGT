using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    [ToolNodeInfo("Diffuser", "Positioner/Diffuser", true, true)]
    [AdditionalPorts(new[] { typeof(bool), typeof(Vector2), typeof(Vector2) }, null, new[] { "Use Selected", "DiffuseX", "DiffuseY" })]
    public class Diffuser : NBPGT_Node
    {
        [Exposed("Use Selected")] public bool useOnlySelectedNodes = false;
        [Exposed("DiffuseX")] public Vector2 xRange = Vector2.zero;
        [Exposed("DiffuseY")] public Vector2 yRange = Vector2.zero;

        public override void OnProcess(NBPGT_Graph currentGraph, NBPGT_RunningData rData)
        {
            List<GeneratedNode> targetNodes = useOnlySelectedNodes ? new List<GeneratedNode>(rData.selectedNodes) : new List<GeneratedNode>(rData.generatedNodes);

            foreach(var node in targetNodes)
            {
                float diffStrength = (float)rData.random.NextDouble();
                float xDistance = ((1 - diffStrength) * xRange.x) + (diffStrength * xRange.y);
                float yDistance = ((1 - diffStrength) * yRange.x) + (diffStrength * yRange.y);
                node.MovePosition(new Vector2(xDistance, yDistance));
            }
            base.OnProcess(currentGraph, rData);
        }
    }
}
