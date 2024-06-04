using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HCZ.NBPGT
{
    [ToolNodeInfo("ExportToFile", "Export/ExportToFile", true, true)]
    [AdditionalPorts(new[] { typeof(bool), typeof(bool), typeof(string) }, null, new[] { "Only Selected", "Use Offset", "SObj Name" })]
    public class ExportToFile : NBPGT_Node
    {
        [Exposed("Only Selected")] public bool useOnlySelectedNodes = false;
        [Exposed("Use Offset")] public bool useOffset = true;
        [Exposed("SObj Name")] public string objectName = "Assets/NBPGT/Generative Graphs/Output/New";
        public ExportToFile() { }

        public override void OnProcess(NBPGT_Graph currentGraph, NBPGT_RunningData rData)
        {
            List<GeneratedNode> exportTargets = useOnlySelectedNodes ? new List<GeneratedNode>(rData.selectedNodes) : new List<GeneratedNode>(rData.generatedNodes);
            if (useOffset)
            {
                foreach (var node in exportTargets)
                {
                    node.MovePosition(rData.baseOffset);
                }
            }
            GeneratedNodeTreeAsset newAsset = ScriptableObject.CreateInstance<GeneratedNodeTreeAsset>();
            newAsset.setNodes(exportTargets);
            newAsset.setVisualizers(rData.visuals);
            
            AssetDatabase.CreateAsset(newAsset, objectName+".asset");
            EditorUtility.SetDirty(newAsset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

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
