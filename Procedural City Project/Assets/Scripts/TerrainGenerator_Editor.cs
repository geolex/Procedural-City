using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGenerator_Editor : ReflexiveEditor
{

    public override void OnInspectorGUI()
    {
	    TerrainGenerator m_target = (TerrainGenerator) target;

        DrawDefaultInspector();

        GUILayout.Space(20f);

	    if (GUILayout.Button("Generate Terrain"))
        {
	        CallFunction("InitTerrain", m_target);
        }
        
	    if (GUILayout.Button("Generate Mesh"))
	    {
		    CallFunction("InitMesh", m_target);
	    }
    }
}
