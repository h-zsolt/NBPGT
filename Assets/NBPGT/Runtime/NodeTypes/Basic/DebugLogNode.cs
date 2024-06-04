using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    [ToolNodeInfo("Debug Log", "Basic/Debug")]
    [AdditionalPorts(new[] { typeof(string) }, null, new[] { "Log" })]
    public class DebugLogNode : NBPGT_Node
    {
        [Exposed("Log")] public string logMessage = "Debug Log Node Test";
        public override void OnProcess(NBPGT_Graph currentGraph, NBPGT_RunningData rData)
        {
            Debug.Log(logMessage);
            base.OnProcess(currentGraph, rData);
        }
    }
}
