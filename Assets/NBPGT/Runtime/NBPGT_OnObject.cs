using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HCZ.NBPGT
{
    public class NBPGT_OnObject : MonoBehaviour
    {
        public bool runAgain = false;
        [SerializeField] private NBPGT_Graph m_graph;
        [SerializeReference] private List<dynamic> m_inputs;
        private NBPGT_Graph m_graphInstance;

        private void OnEnable()
        {
            RunGraphInstance();
        }

        private void RunGraphInstance()
        {
            m_graphInstance = Instantiate(m_graph);
            for(int i = 0; i<m_graph.getNodes.Count;i++)
            {
                m_graphInstance.getNodes[i].portDictionary = m_graph.getNodes[i].portDictionary;
            }

            ExecuteGraph();
        }

        private void Update()
        {
            if(runAgain)
            {
                runAgain = false;
                RunGraphInstance();
            }
        }

        private void ExecuteGraph()
        {
            m_graphInstance.Init(gameObject);
            foreach(var entry in m_graphInstance.getNodes.OfType<EntryNode>())
            {
                NBPGT_RunningData newData = new NBPGT_RunningData(entry.seed, entry.offset);
                newData.generatedNodes = entry.generatedNodes;
                ProcessNode(entry, newData);
            }
        }

        private void ProcessNode(NBPGT_Node node, NBPGT_RunningData rData)
        {
            node.OnProcess(m_graphInstance, rData);
        }
    }
}
