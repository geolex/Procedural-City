using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HeightMapGenerator))]
public class HeightMapGenerator_Editor : ReflexiveEditor
{

    public override void OnInspectorGUI()
    {
        HeightMapGenerator m_target = (HeightMapGenerator) target;

        DrawDefaultInspector();

        GUILayout.Space(20f);

        if (GUILayout.Button("Generate"))
        {
            CallFunction("ShapeTerrain", m_target);
        }
    }
}
