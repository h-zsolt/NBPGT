using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    public class GenericInputNode : NBPGT_Node
    {
        [Exposed("Name")] public string m_name;
        public string getName => m_name;

        public GenericInputNode()
        {

        }
    }
}
