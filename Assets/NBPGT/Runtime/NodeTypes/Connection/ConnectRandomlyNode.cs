using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    [ToolNodeInfo("Connect Randomly", "Connect/Connect Randomly", true, true)]
    [AdditionalPorts(new[] { typeof(bool), typeof(bool), typeof(int) }, null, new[] { "Use Selected From", "Use Selected To", "Count" })]
    public class ConnectRandomlyNode : NBPGT_Node
    {
        [Exposed("Use Selected From")] public bool selectedFrom = false;
        [Exposed("Use Selected To")] public bool selectedTo = false;
        [Exposed("Count")] public int count = 1;

        public override void OnProcess(NBPGT_Graph currentGraph, NBPGT_RunningData rData)
        {
            for(int i = 0; i<count;i++)
            {
                List<GeneratedNode> fromPool = selectedFrom ? new List<GeneratedNode>(rData.selectedNodes) : new List<GeneratedNode>(rData.generatedNodes);
                List<GeneratedNode> toPool = selectedFrom ? new List<GeneratedNode>(rData.selectedNodes) : new List<GeneratedNode>(rData.generatedNodes);
                int fromMax = fromPool.Count;
                int fromIndex = rData.random.Next(0, fromMax);
                toPool.Remove(fromPool[fromIndex]);
                int toMax = toPool.Count;
                int toIndex = rData.random.Next(0, toMax);

                fromPool[fromIndex].MakeComponent("Connection").SetValue("Target", rData.generatedNodes.IndexOf(toPool[toIndex]));
            }

            base.OnProcess(currentGraph, rData);
        }
    }
}
