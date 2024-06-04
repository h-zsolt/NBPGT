using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    [ToolNodeInfo("ExportToObject", "Export/ExportToObject", true, true)]
    [AdditionalPorts(new[] { typeof(bool), typeof(bool) }, null , new[] { "Only Selected" , "Use Offset"})]
    public class ExportToObject : NBPGT_Node
    {
        [Exposed("Only Selected")] public bool useOnlySelectedNodes = false;
        [Exposed("Use Offset")] public bool useOffset = true;
        public ExportToObject() { }

        public override void OnProcess(NBPGT_Graph currentGraph, NBPGT_RunningData rData)
        {
            List<GeneratedNode> exportTargets = useOnlySelectedNodes ? new List<GeneratedNode>(rData.selectedNodes) : new List<GeneratedNode>(rData.generatedNodes);
            if(useOffset)
            {
                foreach(var node in exportTargets)
                {
                    node.MovePosition(rData.baseOffset);
                }
            }
            GeneratedNodeTreeAsset newAsset = ScriptableObject.CreateInstance<GeneratedNodeTreeAsset>();

            newAsset.setNodes(exportTargets);
            newAsset.setVisualizers(rData.visuals);

            currentGraph.targetObject.AddComponent<RuntimeTreeContainer>().runtimeAsset = newAsset;
            //AssetDatabase.CreateAsset(); -> Other node
            if (useOffset)
            {
                foreach (var node in exportTargets)
                {
                    node.MovePosition(-rData.baseOffset);
                }
            }
            base.OnProcess(currentGraph, rData);
        }
    }
}
