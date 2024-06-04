using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    [GenNodeInfo(), ToolNodeInfo("Generate X Nodes", "Generation/Generate X Nodes")]
    [AdditionalPorts(new[] { typeof(int) }, new[] { typeof(List<GeneratedNode>) }, new[] { "Count", "New Gen" })]
    public class GenerateXNodes : GenerativeNode
    {
        [Exposed("Count")] public int toGen;
        private List<GeneratedNode> m_newlyGenerated = new List<GeneratedNode>();

        public override void OnProcess(NBPGT_Graph currentGraph, NBPGT_RunningData rData)
        {
            m_newlyGenerated = new List<GeneratedNode>();

            for (int i = 0; i < toGen; i++)
            {
                var generatedNode = new GeneratedNode();
                rData.generatedNodes.Add(generatedNode);
                m_newlyGenerated.Add(generatedNode);
            }
            base.OnProcess(currentGraph, rData);
        }

        [PortFlowFunc("New Gen")] public List<GeneratedNode> getNewlyGenerated() { return m_newlyGenerated; }
    }
}
