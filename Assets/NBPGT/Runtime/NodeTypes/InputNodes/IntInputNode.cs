using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    [ToolNodeInfo("Int Input","Input/Int", false, false)]
    [AdditionalPorts(null, new[] { typeof(int) }, new[] { "Int Out" })]
    public class IntInputNode : GenericInputNode
    {
        [PortFlowFunc("Int Out")] public int getValue() 
        {
            return m_value; 
        }

        public void setValue(int newValue)
        {
            m_value = newValue;
        }

        [Exposed("Int")] public int m_value = 0;
        public IntInputNode(){}
    }
}
