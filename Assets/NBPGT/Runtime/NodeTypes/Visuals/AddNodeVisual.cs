using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    [ToolNodeInfo("Add Node Visual", "Visual/Add Node Visual", true, true)]
    [AdditionalPorts(new[] { typeof(string), typeof(Vector2) }, null, new[] { "Path to image", "Scale" })]
    public class AddNodeVisual : NBPGT_Node
    {
        [Exposed("Path to image")] public string targetPath = "Assets/Resources/Images/Circle.png";
        public override void OnProcess(NBPGT_Graph currentGraph, NBPGT_RunningData rData)
        {
            NodeVisualizer newVisual = new NodeVisualizer();
            newVisual.setImage(targetPath);
            rData.visuals.Add(newVisual);
            base.OnProcess(currentGraph, rData);
        }
    }
}
