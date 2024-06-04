using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    [ToolNodeInfo("Line Aligner", "Positioner/Line Aligner", true, true)]
    [AdditionalPorts(new[] { typeof(bool), typeof(Vector2), typeof(Vector2) }, null, new[] { "Use Selected", "Distance", "Start Location" })]
    public class LineAligner : NBPGT_Node
    {
        [Exposed("Use Selected")] public bool useOnlySelectedNodes = false;
        [Exposed("Distance")] public Vector2 distanceBetween = new Vector2(20.0f, 20.0f);
        [Exposed("Start Location")] public Vector2 startLoc = Vector2.zero;

        public override void OnProcess(NBPGT_Graph currentGraph, NBPGT_RunningData rData)
        {
            List<GeneratedNode> targetNodes = useOnlySelectedNodes ? new List<GeneratedNode>(rData.selectedNodes) : new List<GeneratedNode>(rData.generatedNodes);
            
            for (int i = 0; i < targetNodes.Count; i++)
            {
                targetNodes[i].SetPosition(startLoc + (distanceBetween * i));
            }

            base.OnProcess(currentGraph, rData);
        }
    }
}
