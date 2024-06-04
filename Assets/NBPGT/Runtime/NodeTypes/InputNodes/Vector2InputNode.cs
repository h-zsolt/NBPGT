using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    [ToolNodeInfo("Vector2 Input", "Input/Vector2", false, false)]
    [AdditionalPorts(null, new[] { typeof(Vector2) }, new[] { "Vector2 Out" })]
    public class Vector2Input : GenericInputNode
    {
        [Exposed("Vector2")] public Vector2 m_value = Vector2.zero;

        public Vector2Input(){}
        public void setValue(Vector2 newValue)
        {
            m_value = newValue;
        }

        [PortFlowFunc("Vector2 Out")] public Vector2 getValue() { return m_value; }
    }
}
