using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    public class PortFlowFuncAttribute : Attribute
    {
        private string m_name = "";
        private int m_port = -1;
        public string getName => m_name;
        public int getPort => m_port;
        public PortFlowFuncAttribute(string outFlowName, int portIndex = -1)
        {
            m_name = outFlowName;
            m_port = portIndex;
        }
        public void SetPort(int newPort)
        {
            m_port = newPort;
        }
    }
}
