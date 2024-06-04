using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    [ToolNodeInfo("Entry", "Basic/Entry", false, true)]
    [AdditionalPorts(null, new[] { typeof(string) }, new[] {"Origin Seed"})]
    public class EntryNode : NBPGT_Node
    {
        [Exposed("Seed")] public int seed = 23114;
        [Exposed("Base Offset")] public Vector2 offset = Vector2.zero;
        public List<GeneratedNode> generatedNodes;

        public override void OnProcess(NBPGT_Graph graph, NBPGT_RunningData rData)
        {
            Debug.Log("Entry " + getID + " is being executed");
            //return base.OnProcess(graph);
            base.OnProcess(graph, rData);
        }

        [PortFlowFunc("Origin Seed")] public string getSeed() { return seed.ToString(); }
    }
}
