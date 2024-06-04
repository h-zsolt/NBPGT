using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    [ToolNodeInfo("Number of Nodes Rule","Rules/Number Of Nodes Rule", true, true)]
    [AdditionalPorts(new[] { typeof(int), typeof(bool) }, null, new[] { "Amount", "More" })]
    public class NrOfNodesRule : RuleNode
    {
        [Exposed("Amount"), SerializeField] public int nodeCount = 0;
        [Exposed("More"), SerializeField] public bool moreThan = true;
        public override void OnProcess(NBPGT_Graph currentGraph, NBPGT_RunningData rData)
        {
            conditionMet = rData.generatedNodes.Count > nodeCount && moreThan;
            base.OnProcess(currentGraph, rData);
        }
    }
}
