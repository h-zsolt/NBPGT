using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace HCZ.NBPGT.Editor
{
    [CustomEditor(typeof(GeneratedNodeTreeAsset))]
    public class NB_AssetEditor : UnityEditor.Editor
    {
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int index)
        {
            Object asset = EditorUtility.InstanceIDToObject(instanceID);
            if (asset.GetType() == typeof(GeneratedNodeTreeAsset))
            {
                NB_Window.Open((GeneratedNodeTreeAsset)asset);
                return true;
            }
            return false;
        }
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            if (GUILayout.Button("Open"))
            {
                NB_Window.Open((GeneratedNodeTreeAsset)target);
            }
        }
    }
}
