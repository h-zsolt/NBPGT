using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCZ.NBPGT
{
    [System.Serializable]
    public struct NBPGT_Connection
    {
        public NBPGT_ConnectionPort inputPort;
        public NBPGT_ConnectionPort outputPort;

        public NBPGT_Connection(NBPGT_ConnectionPort input, NBPGT_ConnectionPort output)
        {
            inputPort = input;
            outputPort = output;
        }

        public NBPGT_Connection(string inputPortID, int inputIndex, string outputPortID, int outputIndex)
        {
            inputPort = new NBPGT_ConnectionPort(inputPortID, inputIndex);
            outputPort = new NBPGT_ConnectionPort(outputPortID, outputIndex);
        }

        public static bool operator ==(NBPGT_Connection first, NBPGT_Connection second)
        {
            return first.inputPort == second.inputPort && first.outputPort == second.outputPort;
        }
        public static bool operator !=(NBPGT_Connection first, NBPGT_Connection second)
        {
            return first.inputPort != second.inputPort || first.outputPort != second.outputPort;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(inputPort, outputPort);
        }
    }

    [System.Serializable]
    public struct NBPGT_ConnectionPort
    {
        public string nodeID;
        public int nodeIndex;

        public NBPGT_ConnectionPort(string id, int index)
        {
            this.nodeID = id;
            this.nodeIndex = index;
        }

        public static bool operator ==(NBPGT_ConnectionPort first, NBPGT_ConnectionPort second)
        {
            return first.nodeID == second.nodeID && first.nodeIndex == second.nodeIndex;
        }
        public static bool operator !=(NBPGT_ConnectionPort first, NBPGT_ConnectionPort second)
        {
            return first.nodeID != second.nodeID || first.nodeIndex != second.nodeIndex;
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(nodeID, nodeIndex);
        }
    }
}
