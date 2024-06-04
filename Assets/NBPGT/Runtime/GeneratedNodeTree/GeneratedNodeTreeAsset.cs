using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    public class GeneratedNodeTreeAsset : ScriptableObject
    {
        [SerializeField] private List<GeneratedNode> m_generatedNodes;
        [SerializeField] private List<NB_TreeVisualizer> m_visualizers;
        public List<GeneratedNode> getNodes => m_generatedNodes;
        public List<NB_TreeVisualizer> GetVisualizers => m_visualizers;
        public void setNodes(List<GeneratedNode> newList)
        {
            m_generatedNodes = newList;
        }

        public void setVisualizers(List<NB_TreeVisualizer> visualizers)
        {
            m_visualizers = new List<NB_TreeVisualizer>(visualizers);
        }

        public GeneratedNodeTreeAsset()
        {
            m_generatedNodes = new List<GeneratedNode>();
            m_visualizers = new List<NB_TreeVisualizer>();
        }
    }
}
