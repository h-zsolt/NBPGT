using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    [System.Serializable]
    public class NodeComponent
    {
        [SerializeField] private List<dynamic> m_genericData;
        [SerializeField] private Dictionary<string, int> m_dictionary;
        [SerializeField] private string m_compName;

        public string getName => m_compName;

        public NodeComponent(string name)
        {
            m_compName = name;
            m_genericData = new List<dynamic>();
            m_dictionary = new Dictionary<string, int>();
        }

        public dynamic GetValue(string identifier)
        {
            if(m_dictionary.TryGetValue(identifier, out int index))
            {
                return m_genericData[index];
            }
            return null;
        }

        public void SetValue(string identifier, dynamic value)
        {
            if(m_dictionary.TryGetValue(identifier, out int index))
            {
                m_genericData[index] = value;
            }
            else
            {
                m_dictionary.Add(identifier, m_genericData.Count);
                m_genericData.Add(value);
            }
        }
    }
}
