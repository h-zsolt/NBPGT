using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HCZ.NBPGT
{
    [System.Serializable]
    public class GeneratedNode
    {
        [SerializeField] private Rect m_position;
        [SerializeField] private List<NodeComponent> m_nodeComponents;

        public List<NodeComponent> getComponents => m_nodeComponents;
        public Rect getPos => m_position;
        public Vector2 GetPosVect()
        {
            return new Vector2(m_position.x, m_position.y);
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

        public GeneratedNode()
        {
            m_nodeComponents = new List<NodeComponent>();
        }

        public NodeComponent GetComponent(string type)
        {
            foreach(var component in m_nodeComponents)
            {
                if(component.getName == type)
                {
                    return component;
                }
            }
            return null;
        }
        
        public NodeComponent GetMakeComp(string type)
        {
            foreach (var component in m_nodeComponents)
            {
                if (component.getName == type)
                {
                    return component;
                }
            }
            var newComponent = new NodeComponent(type);
            m_nodeComponents.Add(newComponent);
            return newComponent;
        }

        public NodeComponent MakeComponent(string type)
        {
            var newComponent = new NodeComponent(type);
            m_nodeComponents.Add(newComponent);
            return newComponent;
        }

        public List<NodeComponent> GetComponentsOfType(string type)
        {
            List<NodeComponent> answer = new List<NodeComponent>();
            foreach (var component in m_nodeComponents)
            {
                if (component.getName == type)
                {
                    answer.Add(component);
                }
            }
            return answer;
        }

        public bool RemoveComponentOfType(string type)
        {
            foreach (var component in m_nodeComponents.ToList())
            {
                if (component.getName == type)
                {
                    m_nodeComponents.Remove(component);
                    return true;
                }
            }
            return false;
        }

        public bool RemoveAllComponentsOfType(string type)
        {
            bool successful = false;
            foreach (var component in m_nodeComponents.ToList())
            {
                if (component.getName == type)
                {
                    m_nodeComponents.Remove(component);
                    successful = true;
                }
            }
            return successful;
        }

        public bool RemoveComponent(NodeComponent compToRemove)
        {
            foreach (var component in m_nodeComponents.ToList())
            {
                if (component == compToRemove)
                {
                    m_nodeComponents.Remove(component);
                    return true;
                }
            }
            return false;
        }
    }
}
