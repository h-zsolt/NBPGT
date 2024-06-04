using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    [RuleNodeInfo()]
    public class RuleNode : NBPGT_Node
    {
        [SerializeField] public bool conditionMet = false;
        public int truePortIndex = -1;
        public int falsePortIndex = -1;
        [SerializeField, Exposed("Max Fail")] public int maxFailCount = 100;
        [SerializeField] public int m_currentFailCount = 0;

        public override void OnProcess(NBPGT_Graph currentGraph, NBPGT_RunningData rData)
        {
            rData.random.Next();
            outputIndex = conditionMet ? truePortIndex : falsePortIndex;
            /*
            for (int i = 0; i < maxFailCount && !m_conditionMet; i++)
            {
                base.OnProcess(currentGraph, rData);
                this.OnProcess(currentGraph, rData);
            }*/
            base.OnProcess(currentGraph, rData);
            if(!conditionMet)
            {
                if (m_currentFailCount < maxFailCount)
                {
                    m_currentFailCount++;
                    this.OnProcess(currentGraph, rData); //Fully rerun this node
                }
                else
                {
                    outputIndex = truePortIndex;
                    base.OnProcess(currentGraph, rData); //Only run next node
                }
            }
        }
    }
}
