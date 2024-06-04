using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    [ToolNodeInfo("String Input", "Input/String", false, false)]
    [AdditionalPorts(null, new[] { typeof(string) }, new[] { "String Out" })]
    public class StringInputNode : GenericInputNode
    {
        [Exposed("String")] public string m_value = "";

        public StringInputNode() { }
        public StringInputNode(string value) 
        { 
            m_value = value; 
        }
        [PortFlowFunc("String Out")] public string getValue() { return m_value; }
    }
}
