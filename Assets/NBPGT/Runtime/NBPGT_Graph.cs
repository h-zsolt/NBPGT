using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HCZ.NBPGT
{
    //Graph data
    [CreateAssetMenu(menuName = "NBPGT/New Generative Graph")]
    public class NBPGT_Graph : ScriptableObject
    {
        [SerializeReference] private List<NBPGT_Node> m_nodes;
        [SerializeField] private List<NBPGT_Connection> m_connections;
        private Dictionary<string, NBPGT_Node> m_nodeDictionary; //Can't be serialized, needed for runtime
        public GameObject targetObject;
        public List<NBPGT_Node> getNodes => m_nodes;
        public List<NBPGT_Connection> getConnections => m_connections;

        public void Init(GameObject target)
        {
            targetObject = target;
            m_nodeDictionary = new Dictionary<string, NBPGT_Node>();
            foreach (var node in m_nodes)
            {
                m_nodeDictionary.Add(node.getID, node);
            }
        }

        public NBPGT_Graph()
        {
            m_nodes = new List<NBPGT_Node>();
            m_connections = new List<NBPGT_Connection>();
        }

        public NBPGT_Node FindNode(string answer)
        {
            if (m_nodeDictionary.TryGetValue(answer, out var node))
            {
                return node;
            }

            return null;
        }

        public List<NBPGT_Node> GetOutputNodes(string outputNodeID, int portIndex)
        { 
            List<NBPGT_Node> outputNodes = new List<NBPGT_Node>();
            foreach (var connection in m_connections)
            {
                if (connection.outputPort.nodeID == outputNodeID && connection.outputPort.nodeIndex == portIndex)
                {
                    var outgoingNode = FindNode(connection.inputPort.nodeID);
                    if (outgoingNode == null) { continue; } //ignore if find fails
                    outputNodes.Add(FindNode(connection.inputPort.nodeID));
                }
            }
            return outputNodes;
        }

        public NBPGT_Node FindInputNodeByName(string inputName)
        {
            if (inputName == null) { return null; }
            foreach(var node in m_nodes)
            {
                Type nodeType = node.GetType();
                if (!nodeType.IsSubclassOf(typeof(GenericInputNode))) { continue; }
                if(((GenericInputNode)node).getName != inputName) { continue; }
                return node;
            }
            return null;
        }

        public void ResolveInputValues(string targetNodeID)
        {
            var targetNode = FindNode(targetNodeID);
            Type typeInfo = targetNode.GetType();
            AdditionalPortsAttribute portAtt = typeInfo.GetCustomAttribute<AdditionalPortsAttribute>();
            if (portAtt == null) { return; } //Exit if it has no additional ports
            //Create a dictionary of potentional flow targets
            Dictionary<int, FieldInfo> exposedDictionary = new Dictionary<int, FieldInfo>();
            foreach (FieldInfo property in typeInfo.GetFields())
            {
                var exposedAtt = property.GetCustomAttribute<ExposedAttribute>();
                if (exposedAtt is ExposedAttribute && targetNode.portDictionary.TryGetValue(exposedAtt.getName, out int hit))
                {
                    //Using record instead of runtime as editor might not be on
                    exposedDictionary.Add(hit, property);
                }
            }
            //Loop through connections
            foreach (var connection in m_connections)
            {
                if (connection.inputPort.nodeID != targetNode.getID) { continue; } //Ignore other nodes' connections
                if (connection.inputPort.nodeIndex == targetNode.inputIndex) { continue; } //Ignore input flow port
                exposedDictionary.TryGetValue(connection.inputPort.nodeIndex, out FieldInfo targetField);
                if (targetField == null) { continue; } //Failed to find corresponding field to port id
                var providerNode = FindNode(connection.outputPort.nodeID);
                Type providerType = providerNode.GetType();
                ResolveInputValues(connection.outputPort.nodeID); //Recur just before pulling data
                //Loop through provider for input provider method
                foreach(var method in providerType.GetMethods())
                {
                    PortFlowFuncAttribute flowAtt = method.GetCustomAttribute<PortFlowFuncAttribute>();
                    if (flowAtt == null) { continue; } //Ignore normal methods
                    if (!providerNode.portDictionary.TryGetValue(flowAtt.getName, out int recordedIndex)) { continue; } //Not recorded
                    if (recordedIndex != connection.outputPort.nodeIndex) { continue; } //Wrong method
                    targetField.SetValue(targetNode, method.Invoke(providerNode, null)); //Invoke correct method
                }
            }
        }
    }
}
