using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace HCZ.NBPGT
{
    [System.Serializable]
    public class NodeVisualizer : NB_TreeVisualizer
    {
        public NodeVisualizer() : base() { m_id = 1; }
        public NodeVisualizer(GraphView target, List<GeneratedNode> nodesToDisplay) : base(target, nodesToDisplay)
        {
            m_id = 1;
        }

        public override void Init(GraphView target, List<GeneratedNode> nodesToDisplay)
        {
            base.Init(target, nodesToDisplay);
            m_id = 1;
        }

        public void setImage(string imagePath)
        {
            var rawData = System.IO.File.ReadAllBytes(imagePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(rawData);
            m_nodeImage = new UnityEngine.UIElements.Background();
            m_nodeImage.sprite = Sprite.Create(texture, new Rect(Vector2.zero, new Vector2(texture.width, texture.height)),Vector2.zero);
        }


        public override void RenderVisuals()
        {
            foreach (var node in m_nodes)
            {
                GraphElement newElement = new Node();
                newElement.style.backgroundImage = m_nodeImage;
                newElement.SetPosition(node.getPos);
                
                m_targetView.AddElement(newElement);
            }
            base.RenderVisuals();
        }
    }
}
