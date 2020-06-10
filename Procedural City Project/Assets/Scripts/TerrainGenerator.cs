using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

[RequireComponent(typeof(MeshFilter))]
public class TerrainGenerator : MonoBehaviour
{
	uint SEED_VALUE = 42;

	[Header("Map Size")]
	[Tooltip("Size on X Axis")] [SerializeField] int m_width;
	[Tooltip("Size on Z Axis")] [SerializeField] int m_depth;

	[Header("Terrain Options")]
	[Tooltip("Number of cells per Patch")][Range(8f,128f)][SerializeField] int m_mapDetail;
	[Tooltip("Number of total cells on Terrain")][Range(1f, 100f)][SerializeField] int m_mapResolution;
	
	[Header("Mesh Options")]
	[Tooltip("Display Gizmos")] [SerializeField] bool m_useGizmos = false;
	[Tooltip("Vertices per meter")] [Range(1f, 100f)] [SerializeField] int m_meshResolution = 1;
	[Tooltip("Vertices per meter")] [Range(1f, 255f)] [SerializeField] int m_submeshResolution = 1;



	[Space]
	
	[SerializeField] bool m_useVoronoi = true;
	[Header("Voronoi Options")]
	[SerializeField] int m_centroidAmount;
	
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	protected void Start()
	{
		

		if (GetComponent<Terrain>())
		{
			InitTerrain();
		}
		else if(GetComponent<MeshFilter>())
		{
			InitMesh();
		}
		else
		{
			Debug.Log("Please attach the TerrainGenerator script to a Unity Terrain");
		}
		
		
	}

	public void InitTerrain(){

		TerrainData terrainData = GetComponent<Terrain>().terrainData;

		terrainData.size = new Vector3(m_width, 16, m_depth);
		terrainData.SetDetailResolution(m_mapResolution, m_mapDetail);

		terrainData.heightmapResolution = (m_width * m_mapDetail);
		
		if(m_useVoronoi){
			VoronoiGeneration(terrainData);
		}
	}

	public void InitMesh()
	{
		Mesh mesh = GetComponent<MeshFilter>().sharedMesh;

		int nbChunks = m_meshResolution / m_submeshResolution;

		for (int i = 0; i < nbChunks; ++i)
		{
			for(int j = 0; j < nbChunks; ++j)
			{
				Vector3 position = new Vector3(((float)m_width / (float)nbChunks) * j, 0, ((float)m_depth / (float)nbChunks) * i);
				Vector3 size = new Vector3(((float)m_width / (float)nbChunks), 0, ((float)m_depth / (float)nbChunks));

				GameObject gameobject = new GameObject();
				gameobject.AddComponent<MeshRenderer>();
				gameobject.AddComponent<MeshFilter>().mesh = CreateMesh(position, size, m_submeshResolution);
			}
		}


		/*
		int widthResolution = Mathf.RoundToInt(((float)m_width * (float) m_submeshResolution) + 1);
		int depthResolution = Mathf.RoundToInt(((float)m_depth * (float) m_submeshResolution) + 1);

		float widthPace = 1f / m_submeshResolution;
		float depthPace = 1f / m_submeshResolution;
		
		
		Debug.Log("MapSize : " + widthResolution + " by " + depthResolution);
		Debug.Log("Width pace : " + widthPace);
		Debug.Log("Depth pace : " + depthPace);


		// INIT VERTICES
		Vector3[] vertices = new Vector3[widthResolution * depthResolution];

		for (int x = 0; x < widthResolution; ++x)
		{
			for (int y = 0; y < depthResolution; ++y)
			{
				vertices[x * widthResolution + y].Set(x * widthPace, 0f, y * depthPace);
				//Debug.Log(vertices[x * widthResolution + y]);
			}
		}

		// INIT NORMALS
		Vector3[] normals = new Vector3[widthResolution * depthResolution];

		int i = 0;
		for (int x = 0; x < widthResolution; ++x)
		{
			for (int y = 0; y < depthResolution; ++y)
			{
				normals[i++] = Vector3.up;
			}
		}


		// INIT TRIANGLES
		int[] triangles = new int[(widthResolution-1) * (depthResolution-1) * 2 * 3];

		int j = 0;
		for (int x = 0; x < widthResolution - 1; ++x)
		{
			for (int y = 0; y < depthResolution - 1; ++y)
			{
				triangles[j++] = x * widthResolution + y;
				triangles[j++] = x * widthResolution + y + 1;
				triangles[j++] = (x + 1) * widthResolution + y;

				triangles[j++] = x * widthResolution + y + 1;
				triangles[j++] = (x + 1) * widthResolution + y + 1;
				triangles[j++] = (x + 1) * widthResolution + y;
			}
		}
		
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		*/
	}

	private Mesh CreateMesh(Vector3 _position, Vector3 _size, int _resolution)
	{
		Mesh mesh = new Mesh();

		int widthResolution = _resolution;
		int depthResolution = _resolution;

		float widthPace = (_size.x - _position.x) / (_resolution-1);
		float depthPace = (_size.y - _position.y) / (_resolution-1);


		// INIT VERTICES
		Vector3[] vertices = new Vector3[widthResolution * depthResolution];

		for (int x = 0; x < widthResolution; ++x)
		{
			for (int y = 0; y < depthResolution; ++y)
			{
				vertices[x * widthResolution + y].Set(x * widthPace, 0f, y * depthPace);
				//Debug.Log(vertices[x * widthResolution + y]);
			}
		}

		// INIT NORMALS
		Vector3[] normals = new Vector3[widthResolution * depthResolution];

		int i = 0;
		for (int x = 0; x < widthResolution; ++x)
		{
			for (int y = 0; y < depthResolution; ++y)
			{
				normals[i++] = Vector3.up;
			}
		}


		// INIT TRIANGLES
		int[] triangles = new int[(widthResolution - 1) * (depthResolution - 1) * 2 * 3];

		int j = 0;
		for (int x = 0; x < widthResolution - 1; ++x)
		{
			for (int y = 0; y < depthResolution - 1; ++y)
			{
				triangles[j++] = x * widthResolution + y;
				triangles[j++] = x * widthResolution + y + 1;
				triangles[j++] = (x + 1) * widthResolution + y;

				triangles[j++] = x * widthResolution + y + 1;
				triangles[j++] = (x + 1) * widthResolution + y + 1;
				triangles[j++] = (x + 1) * widthResolution + y;
			}
		}

		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;

		return mesh;
	}







	private void OnDrawGizmos()
	{
		if (!m_useGizmos)
		{
			return;
		}

		if (GetComponent<MeshFilter>())
		{
			Mesh mesh = GetComponent<MeshFilter>().sharedMesh;

			foreach(Vector3 vertex in mesh.vertices)
			{
				Gizmos.DrawSphere(vertex, .1f);
			}
		}
	}

	// Will create a terrain as a Gameobject in the scene
	private GameObject SpawnTerrain(){
		
		TerrainData terrainData = new TerrainData();
		
		GameObject terrain = Terrain.CreateTerrainGameObject(terrainData);
		
		return terrain;
	}
	
	
	
	private void ChangeHeights_EXAMPLE(TerrainData _data){
		float[,] heightMap = _data.GetHeights(0, 0, m_width * m_mapDetail, m_depth * m_mapDetail);

		for(int i = 0; i < heightMap.GetLength(0); ++i)
		{
			for (int j = 0; j < heightMap.GetLength(1); ++j)
			{
				float height = ((float)(i + j)) / (heightMap.GetLength(0) + heightMap.GetLength(1));
				heightMap[i, j] = height;
			}

		}
		_data.SetHeights(0, 0, heightMap);
	}
	
	
	
	#region VORONOI GENERATION
	
	private void VoronoiGeneration(TerrainData _data){
		
		float maxX = _data.size.x;
		float maxY = _data.size.y;

		Unity.Mathematics.Random rand = new Unity.Mathematics.Random(SEED_VALUE);
		
		float2[] centroids = new float2[m_centroidAmount];
		for(int i = 0; i < m_centroidAmount; ++i){
			centroids[i] = rand.NextFloat2(new float2(maxX, maxY));
			
			Debug.Log(centroids[i]);
		}
		
		// Foreach unit of space, get closest centroid.

		
	}
	
	#endregion
	
	
	
	
}
