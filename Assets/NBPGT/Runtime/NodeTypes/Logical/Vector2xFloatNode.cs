using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    [ToolNodeInfo("Vector2 x Float", "Logical/Vector2 x Float", true, true)]
    [AdditionalPorts(new[] { typeof(Vector2), typeof(float) }, new[] { typeof(Vector2) }, new[] { "Vector In", "Float In", "Vector Out" })]
    public class Vector2xFloatNode : NBPGT_Node
    {
        private Vector2 m_answer = Vector2.zero;
        [Exposed("Vector In")] public Vector2 first;
        [Exposed("Float In")] public float second;
        public Vector2xFloatNode() { }

        public override void OnProcess(NBPGT_Graph currentGraph, NBPGT_RunningData rData)
        {
            m_answer = first * second;
            base.OnProcess(currentGraph, rData);
        }

        [PortFlowFunc("Vector Out")] public Vector2 getAnswer() { return m_answer; }
    }
}
