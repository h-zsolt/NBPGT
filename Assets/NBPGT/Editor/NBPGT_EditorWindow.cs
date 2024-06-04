using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.Experimental.GraphView;
using System.Linq;

namespace HCZ.NBPGT.Editor
{
    //Makes the tool appear as a window / docked tab in Unity
    public class NBPGT_EditorWindow : EditorWindow
    {
        [SerializeField] public NBPGT_Graph m_currentGraph;
        [SerializeField] private NBPGT_View m_currentView;
        [SerializeField] private SerializedObject m_serializedObject;
        [SerializeField] private static NBPGT_Graph m_instance;

        public static void Open(NBPGT_Graph target)
        {
            //Check if graph is already open focus on it if it is
            NBPGT_EditorWindow[] editorWindows = Resources.FindObjectsOfTypeAll<NBPGT_EditorWindow>();
            foreach (var window in editorWindows)
            {
                if (window.getCurrentGraph == target)
                {
                    window.Focus();
                    return;
                }
            }
            //Create a new window if it isn't open yet
            //CreateWindow intake -> priority of docking
            NBPGT_EditorWindow newWindow = CreateWindow<NBPGT_EditorWindow>(typeof(NBPGT_EditorWindow), typeof(SceneView));
            newWindow.titleContent = new GUIContent("NBPGT Editor: " + $"{target.name}");
            newWindow.Load(target);
        }

        public static void Execute(NBPGT_Graph target)
        {
            m_instance = Instantiate(target);
            for (int i = 0; i < target.getNodes.Count; i++)
            {
                m_instance.getNodes[i].portDictionary = target.getNodes[i].portDictionary;
            }
            m_instance.Init(target.targetObject);
            foreach (var entry in m_instance.getNodes.OfType<EntryNode>())
            {
                NBPGT_RunningData newData = new NBPGT_RunningData(entry.seed, entry.offset);
                newData.generatedNodes = entry.generatedNodes;
                entry.OnProcess(m_instance, newData);
            }
        }

        public NBPGT_Graph getCurrentGraph => m_currentGraph;

        public void Load(NBPGT_Graph target)
        {
            m_currentGraph = target;
            DrawGraph();
        }

        private void DrawGraph()
        {
            m_serializedObject = new SerializedObject(m_currentGraph);
            m_currentView = new NBPGT_View(m_serializedObject, this);
            m_currentView.graphViewChanged += onChange;
            rootVisualElement.Add(m_currentView);
        }

        private GraphViewChange onChange(GraphViewChange graphViewChange)
        {
            EditorUtility.SetDirty(m_currentGraph);
            return graphViewChange;
        }

        //Redraw window if recompilation or something else happens
        private void OnEnable()
        {
            if(m_currentGraph != null)
            {
                DrawGraph();
            }    
        }

        private void OnGUI()
        {
            if(m_currentGraph != null)
            {
                if(EditorUtility.IsDirty(m_currentGraph))
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
