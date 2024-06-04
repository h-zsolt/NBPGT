using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HCZ.NBPGT
{
    public class ToolNodeInfoAttribute : Attribute
    {
        private string m_nodeTitle;
        private string m_menuItem;
        private bool m_hasFlowInput;
        private bool m_hasFlowOutput;

        public string getTitle => m_nodeTitle;
        public string getItem => m_menuItem;
        public bool hasFlowInput => m_hasFlowInput;
        public bool hasFlowOutput => m_hasFlowOutput;

        public ToolNodeInfoAttribute(string title, string menuItem = "", bool hasFlowInput = true, bool hasFlowOutput = true)
        {
            m_nodeTitle = title;
            m_menuItem = menuItem;
            m_hasFlowInput = hasFlowInput;
            m_hasFlowOutput = hasFlowOutput;
        }
    }
}
