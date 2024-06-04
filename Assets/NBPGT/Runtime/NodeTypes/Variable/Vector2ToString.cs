using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    [ToolNodeInfo("Vector2 to String", "Variable/Vector2 to String",false , false)]
    [AdditionalPorts(new[] { typeof(Vector2)}, new[] { typeof(string) }, new[] { "Vector", "String Out"})]
    public class Vector2ToString : NBPGT_Node
    {
        [Exposed("Vector")] public Vector2 m_vector;
        public Vector2ToString() { }

        [PortFlowFunc("String Out")]
        public string toString()
        {
            return "{"+m_vector.x.ToString()+";"+m_vector.y.ToString()+"}";
        }
    }
}
