using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EffectManager))]
public class EffectManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EffectManager effectManager = (EffectManager)target;

        DrawDefaultInspector();
        
        EditorGUILayout.Space();
        if (GUILayout.Button("Load Effects"))
        {
            effectManager.LoadEffects();
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Sort All Effects"))
        {
            effectManager.SortAllLists();
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Save Effects"))
        {
            effectManager.SaveEffects();
        }
    }
}
