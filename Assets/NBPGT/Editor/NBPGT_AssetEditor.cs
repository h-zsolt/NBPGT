using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace HCZ.NBPGT.Editor
{
    //Adds interaction from the inspector
    [CustomEditor(typeof(NBPGT_Graph))]
    public class NBPGT_AssetEditor : UnityEditor.Editor
    {
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int index)
        {
            Object asset = EditorUtility.InstanceIDToObject(instanceID);
            if(asset.GetType() == typeof(NBPGT_Graph))
            {
                NBPGT_EditorWindow.Open((NBPGT_Graph)asset);
                return true;
            }
            return false;
        }    
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            if(GUILayout.Button("Open"))
            {
                NBPGT_EditorWindow.Open((NBPGT_Graph)target);
            }
            if (GUILayout.Button("Execute"))
            {
                NBPGT_EditorWindow.Open((NBPGT_Graph)target);
                NBPGT_EditorWindow.Execute((NBPGT_Graph)target);
            }
        }
    }
}
