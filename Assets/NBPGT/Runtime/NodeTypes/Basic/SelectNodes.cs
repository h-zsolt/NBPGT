using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    [ToolNodeInfo("Select Nodes", "Basic/Select Nodes", true, true)]
    [AdditionalPorts(new[] { typeof(List<GeneratedNode>) }, new[] { typeof(List<GeneratedNode>) }, new[] { "Select", "Pass"})]
    public class SelectNodes : NBPGT_Node
    {
        [Exposed("Select")] public List<GeneratedNode> m_selectionTargets = new List<GeneratedNode>();

        public override void OnProcess(NBPGT_Graph currentGraph, NBPGT_RunningData rData)
        {
            rData.selectedNodes = m_selectionTargets;
            base.OnProcess(currentGraph, rData);
        }

        [PortFlowFunc("Pass")] public List<GeneratedNode> getSelection() { return m_selectionTargets; }
    }
}
