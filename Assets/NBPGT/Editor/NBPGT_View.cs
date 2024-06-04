using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using System;
using System.Linq;
using UnityEditor.UIElements;

namespace HCZ.NBPGT.Editor
{
    //Visualizer for the graph data
    public class NBPGT_View : GraphView
    {
        public List<NBPGT_EditorNode> m_graphNodes;
        public Dictionary<string, NBPGT_EditorNode> m_nodeDictionary;
        public Dictionary<Edge, NBPGT_Connection> m_edgeDictionary;

        private NBPGT_Graph m_graph;
        private SerializedObject m_serializedObject;
        private NBPGT_EditorWindow m_window;
        private NBPGT_Search m_searchProvider;

        public NBPGT_View(SerializedObject serializedObject, NBPGT_EditorWindow window)
        {
            m_serializedObject = serializedObject;
            m_window = window;
            m_graph = (NBPGT_Graph)serializedObject.targetObject;
            m_searchProvider = ScriptableObject.CreateInstance<NBPGT_Search>();
            m_searchProvider.graph = this;
            this.nodeCreationRequest = ShowSearchWindow;

            m_graphNodes = new List<NBPGT_EditorNode>();
            m_nodeDictionary = new Dictionary<string, NBPGT_EditorNode>();
            m_edgeDictionary = new Dictionary<Edge, NBPGT_Connection>();

            StyleSheet style = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/NBPGT/Editor/USS/NBPGT_EditorStyle.uss");
            styleSheets.Add(style);

            GridBackground background = new GridBackground();
            background.name = "Grid";
            Add(background);
            background.SendToBack();

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());
            this.AddManipulator(new ContentZoomer());

            DrawNodes();
            DrawConnections();

            GUILayout.ExpandWidth(false);

            //After initial load to avoid infinite loop
            graphViewChanged += OnGraphViewChanged;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> allPorts = new List<Port>();
            List<Port> ports = new List<Port>();

            foreach(var node in m_graphNodes)
            {
                allPorts.AddRange(node.getPorts);
            }

            foreach (var port in allPorts)
            {
                if (startPort == port) { continue; } //Ignore itself
                if (startPort.node == port.node) { continue; } //Ignore node's ports
                if (startPort.direction == port.direction) { continue; } //Ignore same direction
                if (startPort.portType == port.portType)
                {
                    ports.Add(port);
                }
            }

            return ports;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            bool recordOnce = true;
            if(graphViewChange.elementsToRemove != null)
            {
                foreach(var element in graphViewChange.elementsToRemove)
                {
                    if(element.GetType() == typeof(NBPGT_EditorNode))
                    {
                        if(recordOnce)
                        {
                            Undo.RecordObject(m_serializedObject.targetObject, "Removed Node");
                            recordOnce = false;
                        }
                        RemoveNode((NBPGT_EditorNode)element);
                    }
                }
                foreach(var element in graphViewChange.elementsToRemove.OfType<Edge>())
                {
                    RemoveConnection(element);
                }
            }
            if(graphViewChange.movedElements != null)
            {
                Undo.RecordObject(m_serializedObject.targetObject, "Moved Elements");
                foreach(var element in graphViewChange.movedElements.OfType<NBPGT_EditorNode>().ToList())
                {
                    element.SavePosition();
                }
            }
            if(graphViewChange.edgesToCreate!= null)
            {
                foreach(var edge in graphViewChange.edgesToCreate)
                {
                    CreateEdge(edge);
                }
            }
            BindObject();
            return graphViewChange;
        }

        private void CreateEdge(Edge edge)
        {
            NBPGT_EditorNode inputNode = (NBPGT_EditorNode)edge.input.node;
            int inputIndex = inputNode.getPorts.IndexOf(edge.input);
            NBPGT_EditorNode outputNode = (NBPGT_EditorNode)edge.output.node;
            int outputIndex = outputNode.getPorts.IndexOf(edge.output);

            inputNode.RemoveProperty(edge.input.portName);
            outputNode.RemoveProperty(edge.output.portName);

            NBPGT_Connection connection = new NBPGT_Connection(inputNode.getNode.getID, inputIndex, outputNode.getNode.getID, outputIndex);
            m_graph.getConnections.Add(connection);
            m_edgeDictionary.Add(edge, connection);
            BindObject();
        }

        public NBPGT_EditorWindow getWindow => m_window;

        public void Add(NBPGT_Node node)
        {
            //Create node data
            Undo.RecordObject(m_serializedObject.targetObject, "Added Node");
            m_graph.getNodes.Add(node);
            m_serializedObject.Update();

            //Add node to editor, visual representation
            AddNodeToGraph(node);
        }

        private void AddNodeToGraph(NBPGT_Node node)
        {
            node.typeName = node.GetType().AssemblyQualifiedName;

            NBPGT_EditorNode editorNode = new NBPGT_EditorNode(node, m_serializedObject);
            editorNode.SetPosition(node.getPos);
            m_graphNodes.Add(editorNode);
            m_nodeDictionary.Add(node.getID, editorNode);
            AddElement(editorNode);
            BindObject();
        }
        private void ShowSearchWindow(NodeCreationContext obj)
        {
            m_searchProvider.target = (VisualElement)focusController.focusedElement;
            SearchWindow.Open(new SearchWindowContext(obj.screenMousePosition), m_searchProvider);
        }

        private void RemoveNode(NBPGT_EditorNode node)
        {
            m_graph.getNodes.Remove(node.getNode);
            m_nodeDictionary.Remove(node.getNode.getID);
            m_graphNodes.Remove(node);
            BindObject();
        }

        private void RemoveConnection(Edge edge)
        {
            if(m_edgeDictionary.TryGetValue(edge, out NBPGT_Connection connection))
            {
                NBPGT_EditorNode inputNode = (NBPGT_EditorNode)edge.input.node;
                NBPGT_EditorNode outputNode = (NBPGT_EditorNode)edge.output.node;

                inputNode.ReturnProperty(edge.input.portName);
                outputNode.ReturnProperty(edge.output.portName);

                m_graph.getConnections.Remove(connection);
                m_edgeDictionary.Remove(edge);
            }
            BindObject();
        }

        private void RemoveConnection(NBPGT_Connection connection)
        {
            foreach (var item in m_edgeDictionary.Where(kvp => kvp.Value == connection).ToList())
            {
                m_edgeDictionary.Remove(item.Key);
            }
            m_graph.getConnections.Remove(connection);
            BindObject();
        }

        private void DrawNodes()
        {
            foreach(var node in m_graph.getNodes)
            {
                AddNodeToGraph(node);
            }
            BindObject();
        }

        private void DrawConnections()
        {
            if(m_graph.getConnections == null) { return; }
            foreach(var connection in m_graph.getConnections)
            {
                DrawConnection(connection);
            }
            BindObject();
        }

        private void DrawConnection(NBPGT_Connection connection)
        {
            NBPGT_EditorNode inputNode = GetNode(connection.inputPort.nodeID);
            NBPGT_EditorNode outputNode = GetNode(connection.outputPort.nodeID);
            if(inputNode==null || outputNode ==null)
            {
                RemoveConnection(connection);
            }
            Port inputPort = inputNode.getPorts[connection.inputPort.nodeIndex];
            Port outputPort = outputNode.getPorts[connection.outputPort.nodeIndex];

            Edge edge = inputPort.ConnectTo(outputPort);
            AddElement(edge);
            m_edgeDictionary.Add(edge, connection);
            inputNode.RemoveProperty(edge.input.portName);
            outputNode.RemoveProperty(edge.output.portName);
        }

        private NBPGT_EditorNode GetNode(string nodeID)
        {
            NBPGT_EditorNode node = null;
            m_nodeDictionary.TryGetValue(nodeID, out node);
            return node;
        }

        private void BindObject()
        {
            m_serializedObject.Update();
            this.Bind(m_serializedObject);
        }
    }
}
