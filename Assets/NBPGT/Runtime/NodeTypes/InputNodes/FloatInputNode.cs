using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    [ToolNodeInfo("Float Input", "Input/Float", false, false)]
    [AdditionalPorts(null, new[] { typeof(float) }, new[] { "Float Out" })]
    public class FloatInputNode : GenericInputNode
    {
        [Exposed("Float")] public float m_value = 0.0f;

        public FloatInputNode() { }

        [PortFlowFunc("Float Out")] public float getValue() { return m_value; }
    }
}
