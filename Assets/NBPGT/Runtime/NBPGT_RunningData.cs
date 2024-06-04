using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    [System.Serializable]
    public class NBPGT_RunningData
    {
        public System.Random random;
        public List<GeneratedNode> generatedNodes;
        public List<GeneratedNode> selectedNodes;
        public Vector2 baseOffset;
        public List<NB_TreeVisualizer> visuals;

        public NBPGT_RunningData(int seed, Vector2 offset)
        {
            visuals = new List<NB_TreeVisualizer>();
            random = new System.Random(seed);
            generatedNodes = new List<GeneratedNode>();
            baseOffset = offset;
        }

    }
}
