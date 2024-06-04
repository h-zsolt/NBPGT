using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    [ToolNodeInfo("Bool Input", "Input/Bool", false, false)]
    [AdditionalPorts(null, new[] { typeof(bool) }, new[] { "Bool Out" })]
    public class BoolInputNode : GenericInputNode
    {
        [Exposed("Bool")] public bool m_value = false;

        public BoolInputNode() {}
        [PortFlowFunc("Bool Out")]public BoolInputNode(bool value) { m_value = value; }
    }
}
