using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    [ToolNodeInfo("Set Vector2", "Variable/Set Vector2", true, true)]
    [AdditionalPorts(new[] { typeof(Vector2), typeof(string) }, new[] { typeof(Vector2) }, new[] { "New Value", "Target", "Vector Out" })]
    public class SetVector2Node : NBPGT_Node
    {
        [Exposed("New Value")] public Vector2 newValue = Vector2.zero;
        [Exposed("Target")] public string targetName = string.Empty;
        private NBPGT_Graph m_currentGraph;
        public SetVector2Node() { }

        public override void OnProcess(NBPGT_Graph currentGraph, NBPGT_RunningData rData)
        {
            m_currentGraph = currentGraph;
            var targetNode = currentGraph.FindInputNodeByName(targetName);
            Type targetType = targetNode.GetType();
            if(targetType == typeof(Vector2Input))
            {
                ((Vector2Input)targetNode).setValue(newValue);
            }
            base.OnProcess(currentGraph, rData);
        }

        [PortFlowFunc("Vector Out")]
        public Vector2 getValue()
        {
            var targetNode = m_currentGraph.FindInputNodeByName(targetName);
            Type targetType = targetNode.GetType();
            if (targetType == typeof(Vector2Input))
            {
                return ((Vector2Input)targetNode).getValue();
            }
            return newValue;
        }
    }
}
