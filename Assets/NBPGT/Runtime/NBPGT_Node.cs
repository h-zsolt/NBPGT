using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HCZ.NBPGT
{
    [System.Serializable]
    public class NBPGT_Node
    {
        [SerializeField] private string m_guid;
        [SerializeField] private Rect m_position;
        public string typeName;
        public int outputIndex = -1;
        public int inputIndex = -1;
        public Dictionary<string, int> portDictionary; //Records the editor's port setup

        public string getID => m_guid;
        public Rect getPos => m_position;

        public NBPGT_Node()
        {
            NewGUID();
            portDictionary = new Dictionary<string, int>();
        }

        public void SetPosition(Rect newPosition)
        {
            m_position = newPosition;
        }

        public void SetPosition(Vector2 newPosition)
        {
            m_position = new Rect(newPosition.x, newPosition.y, m_position.width, m_position.height);
        }

        public void MovePosition(Rect moveBy)
        {
            m_position = new Rect(m_position.x + moveBy.x, m_position.y + moveBy.y, m_position.width, m_position.height);
        }

        public void MovePosition(Vector2 moveBy)
        {
            m_position = new Rect(m_position.x + moveBy.x, m_position.y + moveBy.y, m_position.width, m_position.height);
        }

        private void NewGUID()
        {
            m_guid = Guid.NewGuid().ToString();
        }

        public virtual void OnProcess(NBPGT_Graph currentGraph, NBPGT_RunningData rData)
        {
            //if(outputIndex == -1) { return string.Empty; }
            foreach (var outputNode in currentGraph.GetOutputNodes(m_guid, outputIndex))
            {
                currentGraph.ResolveInputValues(outputNode.getID);
                outputNode.OnProcess(currentGraph, rData);
            }
            //return string.Empty;
        }
    }
}
