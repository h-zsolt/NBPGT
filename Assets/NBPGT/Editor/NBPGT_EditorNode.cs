using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEditor;

namespace HCZ.NBPGT.Editor
{
    public class NBPGT_EditorNode : Node
    {
        private NBPGT_Node m_graphNode;
        private Port m_outputPort;
        private List<Port> m_ports;
        private Dictionary<string, PropertyField> m_fieldDictionary;
        private Dictionary<string, int> m_portDictionary;

        private SerializedObject m_serializedObject;
        private SerializedProperty m_property;
        public List<Port> getPorts => m_ports;
        public NBPGT_Node getNode => m_graphNode;
        public NBPGT_EditorNode(NBPGT_Node node, SerializedObject graphObject)
        {
            UseDefaultStyling();
            this.AddToClassList("tool-node");
            m_graphNode = node;
            m_serializedObject = graphObject;
            m_ports = new List<Port>();
            m_fieldDictionary = new Dictionary<string, PropertyField>();
            m_portDictionary = new Dictionary<string, int>();

            Type typeInfo = node.GetType();
            ToolNodeInfoAttribute toolAtt = typeInfo.GetCustomAttribute<ToolNodeInfoAttribute>();
            RuleNodeInfoAttribute ruleAtt = typeInfo.GetCustomAttribute<RuleNodeInfoAttribute>();
            AdditionalPortsAttribute portAtt = typeInfo.GetCustomAttribute<AdditionalPortsAttribute>();

            title = toolAtt.getTitle;

            //Adds class lists, so the USS handles the style correctly
            string[] split = toolAtt.getItem.Split('/');
            foreach(string part in split)
            {
                this.AddToClassList(part.ToLower().Replace(' ', '-'));
            }

            this.name = typeInfo.Name;

            if(toolAtt.hasFlowInput)
            {
                CreateFlowInputPort();
            }
            if(toolAtt.hasFlowOutput)
            {
                CreateFlowOutputPort();
            }
            if(ruleAtt != null)
            {
                CreateRuleFlowOutputPort();
            }
            if(portAtt != null)
            {
                CreateAdditionalPorts(portAtt);
            }

            m_graphNode.portDictionary = new Dictionary<string, int>();

            foreach (FieldInfo property in typeInfo.GetFields())
            {
                var exposedAtt = property.GetCustomAttribute<ExposedAttribute>();
                if (exposedAtt is ExposedAttribute)
                {
                    if(m_portDictionary.TryGetValue(exposedAtt.getName, out int exposedPort))
                    {
                        exposedAtt.SetPortIndex(exposedPort);
                        m_graphNode.portDictionary.Add(exposedAtt.getName, exposedPort);
                    }
                    m_fieldDictionary.Add(exposedAtt.getName, DrawProperty(property.Name));
                    //field.RegisterValueChangeCallback(OnFieldChangedCallback);
                }
            }

            foreach(var method in typeInfo.GetMethods())
            {
                PortFlowFuncAttribute flowAtt = method.GetCustomAttribute<PortFlowFuncAttribute>();
                if (flowAtt == null) { continue; }
                if(m_portDictionary.TryGetValue(flowAtt.getName, out int flowPort))
                {
                    flowAtt.SetPort(flowPort);
                    m_graphNode.portDictionary.Add(flowAtt.getName, flowPort);
                }
            }
        }

        private void CreateAdditionalPorts(AdditionalPortsAttribute portAtt)
        {
            int nameIterator = 0;
            if (portAtt.getInput != null)
            {
                foreach (var inputPort in portAtt.getInput)
                {
                    var portType = typeof(NBPGT_PortTypes.VariablePort<>).MakeGenericType(inputPort);
                    var newPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, portType);
                    if (portAtt.getNames.Length > nameIterator)
                    {
                        newPort.portName = portAtt.getNames[nameIterator];
                        nameIterator++;
                    }
                    else 
                    {
                        newPort.portName = inputPort.ToString();
                    }
                    newPort.tooltip = "Input that takes variables of " + inputPort.ToString() + " type";
                    newPort.portColor = getTypeColor(inputPort);
                    m_ports.Add(newPort);
                    inputContainer.Add(newPort);
                    m_portDictionary.Add(newPort.portName, m_ports.IndexOf(newPort));
                }
            }
            if (portAtt.getOutput != null)
            {
                foreach (var outputPort in portAtt.getOutput)
                {
                    var portType = typeof(NBPGT_PortTypes.VariablePort<>).MakeGenericType(outputPort);
                    var newPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, portType);
                    if (portAtt.getNames.Length > nameIterator)
                    {
                        newPort.portName = portAtt.getNames[nameIterator];
                        nameIterator++;
                    }
                    else
                    {
                        newPort.portName = outputPort.ToString();
                    }
                    newPort.tooltip = "Output of " + outputPort.ToString() + " type";
                    newPort.portColor = getTypeColor(outputPort);
                    m_ports.Add(newPort);
                    outputContainer.Add(newPort);
                    m_portDictionary.Add(newPort.portName, m_ports.IndexOf(newPort));
                }
            }
        }

        private void CreateRuleFlowOutputPort()
        {
            var alternateOutput = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(NBPGT_PortTypes.FlowPort));
            alternateOutput.portName = "On Fail";
            alternateOutput.tooltip = "Flow output";
            m_ports.Add(alternateOutput);
            outputContainer.Add(alternateOutput);
            RuleNode nodeAsRule = (RuleNode)m_graphNode;
            nodeAsRule.truePortIndex = m_ports.IndexOf(m_outputPort);
            nodeAsRule.falsePortIndex = m_ports.IndexOf(alternateOutput);
        }

        private void OnFieldChangedCallback(SerializedPropertyChangeEvent evt)
        {
            //Mark dirty here
            throw new NotImplementedException();
        }

        private void FetchSerializedProperty()
        {
            SerializedProperty nodes = m_serializedObject.FindProperty("m_nodes");
            if(nodes.isArray)
            {
                int size = nodes.arraySize;
                for(int i = 0;i<size;i++)
                {
                    var element = nodes.GetArrayElementAtIndex(i);
                    var elementID = element.FindPropertyRelative("m_guid");
                    if(elementID.stringValue==m_graphNode.getID)
                    {
                        m_property = element;
                    }
                }
            }
        }

        public PropertyField DrawProperty(string propertyName)
        {
            if (m_property == null)
            {
                FetchSerializedProperty();
            }

            SerializedProperty prop = m_property.FindPropertyRelative(propertyName);
            if (prop == null) { return null; }
            PropertyField field = new PropertyField(prop);
            extensionContainer.Add(field);
            RefreshExpandedState();
            return field;
        }

        public void ReturnProperty(string exposedString)
        {
            Type typeInfo = m_graphNode.GetType();
            foreach (FieldInfo property in typeInfo.GetFields())
            {
                var exposedAtt = property.GetCustomAttribute<ExposedAttribute>();
                if(exposedAtt is ExposedAttribute && exposedAtt.getName == exposedString)
                {
                    if(m_fieldDictionary.TryGetValue(exposedString, out var answer)) { return; }
                    m_fieldDictionary.Add(exposedAtt.getName, DrawProperty(property.Name));
                    return;
                }
            }
        }

        public void RemoveProperty(string propertyName)
        {
            m_fieldDictionary.TryGetValue(propertyName, out var field);
            if(field == null) { return; }
            extensionContainer.Remove(field);
            m_fieldDictionary.Remove(propertyName);
        }

        private void CreateFlowOutputPort()
        {
            m_outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(NBPGT_PortTypes.FlowPort));
            m_outputPort.portName = "Output";
            m_outputPort.tooltip = "Flow output";
            m_outputPort.portColor = Color.white;
            m_ports.Add(m_outputPort);
            outputContainer.Add(m_outputPort);
            m_graphNode.outputIndex = m_ports.IndexOf(m_outputPort);
        }

        private void CreateFlowInputPort()
        {
            var inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(NBPGT_PortTypes.FlowPort));
            inputPort.portName = "Input";
            inputPort.tooltip = "Flow input";
            inputPort.portColor = Color.white;
            m_ports.Add(inputPort);
            inputContainer.Add(inputPort);
            m_graphNode.inputIndex = m_ports.IndexOf(inputPort);
        }

        public void SavePosition()
        {
            m_graphNode.SetPosition(GetPosition());
        }

        private Color getTypeColor(Type varType)
        {
            switch(varType.ToString())
            {
                case "System.String": return Color.yellow;
                case "System.Single": return Color.green;
                case "System.Int32": return Color.blue;
                case "System.Boolean": return Color.red;
                default: return Color.magenta;
            }
        }
    }
}
