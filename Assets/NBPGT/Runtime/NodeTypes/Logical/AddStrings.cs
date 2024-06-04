using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    [ToolNodeInfo("String + String", "Logical/String + String", false, false)]
    [AdditionalPorts(new[] { typeof(string), typeof(string) }, new[] { typeof(string)}, new[] { "First", "Second", "Out" })]
    public class AddStrings : NBPGT_Node
    {
        [Exposed("First")] public string m_first = string.Empty;
        [Exposed("Second")] public string m_second = string.Empty;

        public override void OnProcess(NBPGT_Graph currentGraph, NBPGT_RunningData rData)
        {
            base.OnProcess(currentGraph, rData);
        }

        [PortFlowFunc("Out")] public string addedString() { return m_first + m_second; }
    }
}
