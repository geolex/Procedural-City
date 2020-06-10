using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Collections;

public class HeightMapGenerator : MonoBehaviour
{
    [Header("Map Size")]
    [Tooltip("Size on X Axis")] [SerializeField] int m_width;
    [Tooltip("Size on Z Axis")] [SerializeField] int m_depth;

    [Header("Map Details")]
    [Tooltip("Number of cells per Patch")][Range(8f,128f)][SerializeField] int m_detail;
    [Tooltip("Number of total cells on Terrain")][Range(1f, 100f)][SerializeField] int m_resolution;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    private float[,] GenerateHeightMap()
    {
        float[,] heightMap = new float[m_resolution, m_depthResolution];

        for(int width = 0; width < m_resolution; ++width)
        {
            for(int depth = 0; depth < m_depthResolution; ++depth)
            {
                heightMap[width, depth] = 100f;
            }
        }

        return heightMap;
    }
    */

    // NOT USEFUL
    public void GenerateTerrainMesh()
    {
        /*
        m_widthResolution = (m_width * m_widthDetail) + 1;
        m_depthResolution = (m_depth * m_depthDetail) + 1;

        m_widthStep = 1 / m_widthResolution;
        m_depthStep = 1 / m_depthResolution;

        Vector3[] vertices = new Vector3[m_widthResolution * m_depthResolution];
        Vector3[] normals = new Vector3[m_widthResolution * m_depthResolution];
        Vector2[] uv = new Vector2[m_widthResolution * m_depthResolution];

        // Setting up the Vertices with the correct coordinates
        for (int v = 0, z = 0; z < m_depthResolution; z++)
        {
            for (int x = 0; x < m_widthResolution; x++, v++)
            {
                float altitude = 5f;

                vertices[v] = new Vector3(x * m_widthStep - 0.5f, altitude, z * m_depthStep - 0.5f);
                normals[v] = Vector3.up;
                uv[v] = new Vector2(m_widthStep * x, m_depthStep * z);
            }
        }

        int[] triangles = new int[(m_widthResolution-1) * (m_depthResolution-1) * 6];

        Debug.Log(vertices.Length);
        Debug.Log(triangles.Length);

        // Setting Up the Triangles with the correct indices
        for (int t = 0, v = 0, z = 0; z < m_depthResolution - 1; z++, v++)
        {
            Debug.Log("Z = " + z);

            for (int x = 0; x < m_widthResolution - 1; x++, v++, t += 6)
            {
                Debug.Log("T = " + t);

                triangles[t] = v;
                triangles[t + 1] = v + m_widthResolution;
                triangles[t + 2] = v + 1;
                triangles[t + 3] = v + 1;
                triangles[t + 4] = v + m_widthResolution;
                triangles[t + 5] = v + m_widthResolution;
            }
        }

        


        Mesh terrainMesh = new Mesh();
        terrainMesh.vertices = vertices;
        terrainMesh.triangles = triangles;
        terrainMesh.normals = normals;
        terrainMesh.uv = uv;
        

        GameObject terrain = new GameObject();
        terrain.AddComponent<MeshFilter>().mesh = terrainMesh;
        terrain.AddComponent<MeshRenderer>();
        terrain.transform.parent = transform;
        */
        
    }





    public void ShapeTerrain()
    {
        Terrain terrain = GetComponent<Terrain>();

        terrain.terrainData.size = new Vector3(m_width, 16, m_depth);
        terrain.terrainData.SetDetailResolution(m_resolution, m_detail);

        terrain.terrainData.heightmapResolution = (m_width * m_detail);

        float[,] heightMap = terrain.terrainData.GetHeights(0, 0, m_width * m_detail, m_depth * m_detail);

        for(int i = 0; i < heightMap.GetLength(0); ++i)
        {
            for (int j = 0; j < heightMap.GetLength(1); ++j)
            {
                float height = ((float)(i + j)) / (heightMap.GetLength(0) + heightMap.GetLength(1));

                //Debug.Log(height);
                heightMap[i, j] = height;
            }

        }

        terrain.terrainData.SetHeights(0, 0, heightMap);
    }
}
