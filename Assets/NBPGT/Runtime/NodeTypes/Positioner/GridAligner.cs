using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    [ToolNodeInfo("Grid Aligner", "Positioner/Grid Aligner", true, true)]
    [AdditionalPorts(new[] { typeof(bool), typeof(Vector2), typeof(Vector2) }, null, new[] { "Use Selected", "Distance", "Start Location" })]
    public class GridAligner : NBPGT_Node
    {
        [Exposed("Use Selected")] public bool useOnlySelectedNodes = false;
        [Exposed("Distance")] public Vector2 distanceBetween = new Vector2(20.0f, 20.0f);
        [Exposed("Start Location")] public Vector2 startLoc = Vector2.zero;

        public override void OnProcess(NBPGT_Graph currentGraph, NBPGT_RunningData rData)
        {
            List<GeneratedNode> targetNodes = useOnlySelectedNodes ? new List<GeneratedNode>(rData.selectedNodes) : new List<GeneratedNode>(rData.generatedNodes);
            int gridsize;
            for (gridsize = 1; gridsize * gridsize < targetNodes.Count; gridsize++) { }
            Debug.Log("Grid size: " + gridsize);
            for (int i = 0; i < gridsize; i++)
            {
                for (int j = 0; j < gridsize; j++)
                {
                    targetNodes[i * gridsize + j].SetPosition(new Vector2 (startLoc.x + (distanceBetween.x * j), startLoc.y + (distanceBetween.y * i)));
                }
            }
            base.OnProcess(currentGraph, rData);
        }
    }
}
