using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace HCZ.NBPGT.Editor
{
    public class NB_TreeView : GraphView
    {
        private SerializedObject m_serializedObject;
        private GeneratedNodeTreeAsset m_asset;
        private List<NB_TreeVisualizer> m_visualizers;
        private NB_Window m_window;
        public NB_TreeView(SerializedObject serializedObject, NB_Window window)
        {
            m_visualizers = new List<NB_TreeVisualizer>();
            m_serializedObject = serializedObject;
            m_asset = (GeneratedNodeTreeAsset)serializedObject.targetObject;
            m_window = window;

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

            GUILayout.ExpandWidth(false);

            if (m_asset.GetVisualizers != null)
            {
                foreach (NB_TreeVisualizer visualizer in m_asset.GetVisualizers)
                {
                    visualizer.Init(this, m_asset.getNodes);
                    if(visualizer.getID == 1)
                    {
                        //((NodeVisualizer)visualizer).RenderVisuals();
                        NodeVisualizer refreshedVis = new NodeVisualizer(this, m_asset.getNodes);
                        refreshedVis.m_nodeImage = visualizer.m_nodeImage;
                        refreshedVis.RenderVisuals();
                        m_visualizers.Add(refreshedVis);
                    }
                    else
                    {
                        visualizer.Init(this, m_asset.getNodes);
                        visualizer.RenderVisuals();
                    }
                }
            }
            graphViewChanged += OnGraphViewChanged;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            //Add handling user input
            return graphViewChange;
        }
    }
}
