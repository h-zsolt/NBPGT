using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HCZ.NBPGT
{
    [GenNodeInfo(), ToolNodeInfo("Create Midpoints", "Generation/Midpoints")]
    [AdditionalPorts(new[] { typeof(bool) }, new[] { typeof(List<GeneratedNode>) }, new[] { "Use Selected", "New Gen" })]
    public class MidPointGenerator : GenerativeNode
    {
        private List<GeneratedNode> m_newlyGenerated = new List<GeneratedNode>();
        [Exposed("Use Selected")] public bool useOnlySelectedNodes = false;

        public override void OnProcess(NBPGT_Graph currentGraph, NBPGT_RunningData rData)
        {
            m_newlyGenerated = new List<GeneratedNode>();
            List<GeneratedNode> targetNodes = useOnlySelectedNodes ? new List<GeneratedNode>(rData.selectedNodes) : new List<GeneratedNode>(rData.generatedNodes);
            List<Vector2Int> reformConnections = new List<Vector2Int>();
            for (int i = 0; i < targetNodes.Count; i++)
            {
                //Using .ToList to not invalidate the foreach
                foreach(var connection in targetNodes[i].GetComponentsOfType("Connection").ToList())
                {
                    int selfIndex = rData.generatedNodes.IndexOf(targetNodes[i]);
                    int targetIndex = connection.GetValue("Target");
                    targetNodes[i].RemoveComponent(connection);
                    var generatedNode = new GeneratedNode();
                    m_newlyGenerated.Add(generatedNode);
                    generatedNode.SetPosition((targetNodes[i].GetPosVect() + rData.generatedNodes[targetIndex].GetPosVect()) / 2.0f);
                    rData.generatedNodes.Add(generatedNode);
                    int newNodeIndex = rData.generatedNodes.IndexOf(generatedNode);
                    reformConnections.Add(new Vector2Int(selfIndex, newNodeIndex));
                    reformConnections.Add(new Vector2Int(newNodeIndex, targetIndex));
                }
            }
            //Oops was infinite in there
            foreach(var reform in reformConnections)
            {
                rData.generatedNodes[reform.x].MakeComponent("Connection").SetValue("Target", reform.y);
            }
            base.OnProcess(currentGraph, rData);
        }

        [PortFlowFunc("New Gen")] public List<GeneratedNode> getNewlyGenerated() { return m_newlyGenerated; }
    }
}
