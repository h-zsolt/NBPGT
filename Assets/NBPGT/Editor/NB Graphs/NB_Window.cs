using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace HCZ.NBPGT.Editor
{
    public class NB_Window : EditorWindow
    {
        [SerializeField] private SerializedObject m_serializedObject;
        [SerializeField] private NB_TreeView m_nodeView;
        [SerializeField] private GeneratedNodeTreeAsset m_asset;
        public GeneratedNodeTreeAsset getAsset => m_asset;
        public static void Open(GeneratedNodeTreeAsset target)
        {
            NB_Window[] allWindows = Resources.FindObjectsOfTypeAll<NB_Window>();
            foreach (var window in allWindows)
            {
                if (window.getAsset == target)
                {
                    window.Focus();
                    return;
                }
            }

            NB_Window newWindow = CreateWindow<NB_Window>(typeof(NB_Window), typeof(SceneView));
            newWindow.titleContent = new GUIContent("Node Viewer: " + $"{target.name}");
            newWindow.Load(target);
        }

        private void Load(GeneratedNodeTreeAsset target)
        {
            m_asset = target;
            DrawGraph();
        }

        private void DrawGraph()
        {
            m_serializedObject = new SerializedObject(m_asset);
            m_nodeView = new NB_TreeView(m_serializedObject, this);
            m_nodeView.graphViewChanged += onChange;
            rootVisualElement.Add(m_nodeView);
        }

        private GraphViewChange onChange(GraphViewChange graphViewChange)
        {
            EditorUtility.SetDirty(m_asset);
            return graphViewChange;
        }

        private void OnEnable()
        {
            if (m_asset != null)
            {
                DrawGraph();
            }
        }

        private void OnGUI()
        {
            if (m_asset != null)
            {
                if (EditorUtility.IsDirty(m_asset))
                {
                    this.hasUnsavedChanges = true;
                }
                else
                {
                    this.hasUnsavedChanges = false;
                }
            }
        }
    }
}
