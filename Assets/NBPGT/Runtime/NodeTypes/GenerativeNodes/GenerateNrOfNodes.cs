using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    [GenNodeInfo(), ToolNodeInfo("Generate Nr of Nodes", "Generation/Generate Nr of Nodes")]
    [AdditionalPorts(new[] { typeof(int), typeof(int) }, new[] { typeof(List<GeneratedNode>) }, new[] { "Min", "Max", "New Gen" })]
    public class GenerateNrOfNodes : GenerativeNode
    {
        [Exposed("Min")] public int inclusiveMin;
        [Exposed("Max")] public int exclusiveMax;
        private List<GeneratedNode> m_newlyGenerated = new List<GeneratedNode>();

        public override void OnProcess(NBPGT_Graph currentGraph, NBPGT_RunningData rData)
        {
            m_newlyGenerated = new List<GeneratedNode>();

            int numberToGen = rData.random.Next(inclusiveMin, exclusiveMax);
            //Debug.Log(numberToGen);
            for (int i = 0; i < numberToGen; i++)
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
