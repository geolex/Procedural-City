using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomTerrain))]
[CanEditMultipleObjects]
public class CustomTerrain_Editor : Editor
{
	CustomTerrain terrain;

	// Random Heights Method Properties
	SerializedProperty minHeightMap;
	SerializedProperty maxHeightMap;


	// Height Map Method Properties
	SerializedProperty heightMapTexture;
	SerializedProperty heightMapScale;
	SerializedProperty heightMapAmplitude;
	SerializedProperty heightMapAutoScale;

	// Perlin Noise Method Properties
	SerializedProperty perlinScaleX;
	SerializedProperty perlinScaleY;
	SerializedProperty perlinOffsetX;
	SerializedProperty perlinOffsetY;

	// Voronoi Method Properties
	SerializedProperty nbPeaks;
	SerializedProperty falloff;
	SerializedProperty dropoff;
	SerializedProperty type;

	// Folding Bools
	bool showRandom = false;
	bool showImage = false;
	bool showPerlin = false;
	bool showVoronoi = false;


	private void Awake()
	{
		terrain = (CustomTerrain)target;
	}

	private void OnEnable()
	{
		// Random Height Method Properties
		minHeightMap = serializedObject.FindProperty("m_minHeight");
		maxHeightMap = serializedObject.FindProperty("m_maxHeight");

		// Texture HeightMap Method Properties
		heightMapTexture = serializedObject.FindProperty("m_heightMapTexture");
		heightMapScale = serializedObject.FindProperty("m_heightMapScale");
		heightMapAmplitude = serializedObject.FindProperty("m_heightMapAmplitude");
		heightMapAutoScale = serializedObject.FindProperty("m_heightMapAutoScale");

		// Perlin Noise Method Properties
		perlinScaleX = serializedObject.FindProperty("m_perlinScaleX");
		perlinScaleY = serializedObject.FindProperty("m_perlinScaleY");
		perlinOffsetX = serializedObject.FindProperty("m_perlinOffsetX");
		perlinOffsetY = serializedObject.FindProperty("m_perlinOffsetY");

		// Voronoi Method Properties
		nbPeaks = serializedObject.FindProperty("m_nbPeaks");
		falloff = serializedObject.FindProperty("m_falloff");
		dropoff = serializedObject.FindProperty("m_dropoff");
		type = serializedObject.FindProperty("m_type");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		if(showRandom = EditorGUILayout.Foldout(showRandom, "Random HeightMap"))
		{
			DisplayRandom();
		}

		if (showImage = EditorGUILayout.Foldout(showImage, "Image HeightMap"))
		{
			DisplayImage();
		}

		if (showPerlin = EditorGUILayout.Foldout(showPerlin, "Perlin Noise"))
		{
			DisplayPerlin();
		}

		if (showVoronoi = EditorGUILayout.Foldout(showVoronoi, "Voronoi Tesselation"))
		{
			DisplayVoronoi();
		}

		if (GUILayout.Button("Reset HeightMap"))
		{
			terrain.ResetHeightMap();
		}

		serializedObject.ApplyModifiedProperties();
	}

	// Display Properties to generate Random Height Map
	private void DisplayRandom()
	{
		EditorGUILayout.LabelField("", GUI.skin.horizontalScrollbar);
		GUILayout.Label("Set Heights Between Random Values", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField(minHeightMap);
		EditorGUILayout.PropertyField(maxHeightMap);

		if(GUILayout.Button("Generate HeightMap"))
		{
			terrain.RandomHeight();
		}
	}

	private void DisplayImage()
	{
		EditorGUILayout.LabelField("", GUI.skin.horizontalScrollbar);
		GUILayout.Label("Set Heightmap Texture ans scaling", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField(heightMapTexture);
		EditorGUILayout.PropertyField(heightMapScale);
		EditorGUILayout.PropertyField(heightMapAmplitude);
		EditorGUILayout.PropertyField(heightMapAutoScale);

		if (GUILayout.Button("Generate HeightMap"))
		{
			terrain.ImageHeightMap();
		}
	}

	private void DisplayPerlin()
	{
		EditorGUILayout.LabelField("", GUI.skin.horizontalScrollbar);
		GUILayout.Label("Set Perlin Scale", EditorStyles.boldLabel);
		EditorGUILayout.Slider(perlinScaleX, 0f, 0.1f, new GUIContent("X Scale"));
		EditorGUILayout.Slider(perlinScaleY, 0f, 0.1f, new GUIContent("Y Scale"));
		EditorGUILayout.PropertyField(perlinOffsetX);
		EditorGUILayout.PropertyField(perlinOffsetY);

		if (GUILayout.Button("Generate HeightMap"))
		{
			terrain.PerlinNoise();
		}
	}

	private void DisplayVoronoi()
	{
		EditorGUILayout.LabelField("", GUI.skin.horizontalScrollbar);
		GUILayout.Label("Set Number of Peeks", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField(nbPeaks);
		EditorGUILayout.Slider(falloff, 0, 10, new GUIContent("Falloff"));
		EditorGUILayout.Slider(dropoff, 0, 10, new GUIContent("Dropoff"));
		EditorGUILayout.Slider(minHeightMap, 0, 1, new GUIContent("Min Height"));
		EditorGUILayout.Slider(maxHeightMap, 0, 1, new GUIContent("Max Height"));
		EditorGUILayout.PropertyField(type);
		

		if (GUILayout.Button("Generate Voronoi"))
		{
			terrain.Voronoi();
		}
	}
}
