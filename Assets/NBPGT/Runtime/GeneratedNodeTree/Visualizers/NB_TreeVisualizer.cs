using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEditor;

namespace HCZ.NBPGT
{
    [System.Serializable]
    public class NB_TreeVisualizer
    {

        [SerializeField] public UnityEngine.UIElements.Background m_nodeImage;
        [SerializeField] internal GraphView m_targetView;
        [SerializeField] internal List<GeneratedNode> m_nodes;
        [SerializeField] internal List<GraphElement> m_elements; 
        [SerializeField] internal int m_id = 0;
        public int getID => m_id;

        public NB_TreeVisualizer() { }

        public NB_TreeVisualizer(GraphView target, List<GeneratedNode> nodesToDisplay)
        {
            Init(target, nodesToDisplay);
        }

        public virtual void Init(GraphView target, List<GeneratedNode> nodesToDisplay)
        {
            m_elements = new List<GraphElement>();
            m_targetView = target;
            m_nodes = nodesToDisplay;
        }

        public virtual void RenderVisuals()
        {

        }
    }
}
