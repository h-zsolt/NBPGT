using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    public class ExposedAttribute : Attribute
    {
        private string m_portName;
        private int m_portIndex;
        public string getName => m_portName;
        public int getIndex => m_portIndex;
        public ExposedAttribute(string portName, int portIndex = -1)
        {
            m_portName = portName;
            m_portIndex = portIndex;
        }

        public void SetPortIndex(int newIndex)
        {
            m_portIndex = newIndex;
        }
    }
}
