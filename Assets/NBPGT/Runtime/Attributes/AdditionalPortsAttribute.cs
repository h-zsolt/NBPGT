using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    public class AdditionalPortsAttribute : Attribute
    {
        private Type[] m_input;
        private Type[] m_output;
        private string[] m_portNames;

        public Type[] getInput => m_input;
        public Type[] getOutput => m_output;
        public string[] getNames => m_portNames;

        public AdditionalPortsAttribute(Type[] inputPorts = null, Type[] outputPorts = null, string[] portNames = null)
        {
            m_input = inputPorts;
            m_output = outputPorts;
            m_portNames = portNames;
        }
    }
}
