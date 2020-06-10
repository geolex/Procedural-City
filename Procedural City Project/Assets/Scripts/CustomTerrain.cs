using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

struct Perlin
{
	public float m_scaleX;
	public float m_scaleY;
}

[RequireComponent(typeof(Terrain))]
public class CustomTerrain : MonoBehaviour
{
	[Header("Random Heights Constraints")]
	[SerializeField] float m_minHeight = 0f;
	[SerializeField] float m_maxHeight = 0.1f;

	[Header("Image HeightMap Method")]
	[SerializeField] Texture2D m_heightMapTexture;
	[SerializeField] float2 m_heightMapScale = new float2(1f, 1f);
	[SerializeField] [Range(0f, 10f)] float m_heightMapAmplitude = 1f;
	[SerializeField] bool m_heightMapAutoScale = false;

	[Header("Perlin Method")]
	[SerializeField] Perlin[] m_octaves;
	[SerializeField] float m_perlinScaleX = 0f;
	[SerializeField] float m_perlinScaleY = 0f;
	[SerializeField] int m_perlinOffsetX = 0;
	[SerializeField] int m_perlinOffsetY = 0;

	[Header("Voronoi Method")]
	[SerializeField] int m_nbPeaks;
	[SerializeField] float m_falloff;
	[SerializeField] float m_dropoff;
	[SerializeField] VoronoiType m_type;

	enum VoronoiType {Linear, Exponential, Combined}

	Terrain m_terrain;
	TerrainData m_terrainData;

	private void Init()
	{
		m_terrain = gameObject.GetComponent<Terrain>();
		m_terrainData = m_terrain.terrainData;
	}

	private void OnEnable(){
		Init();
	}

	public void ResetHeightMap()
	{
		if (!m_terrainData)
		{
			Init();
		}

		float[,] heightMap = m_terrainData.GetHeights(0, 0, m_terrainData.heightmapResolution, m_terrainData.heightmapResolution);

		for (int x = 0; x < m_terrainData.heightmapResolution; ++x)
		{
			for (int y = 0; y < m_terrainData.heightmapResolution; ++y)
			{
				heightMap[x, y] = 0f;
			}
		}

		m_terrainData.SetHeights(0, 0, heightMap);
	}

	public void RandomHeight()
	{
		if (!m_terrainData)
		{
			Init();
		}

		float[,] heightMap = m_terrainData.GetHeights(0, 0, m_terrainData.heightmapResolution, m_terrainData.heightmapResolution);

		for (int x = 0; x < heightMap.GetLength(0); ++x)
		{
			for (int y = 0; y < heightMap.GetLength(1); ++y)
			{
				heightMap[x, y] = UnityEngine.Random.Range(m_minHeight, m_maxHeight);
			}
		}

		m_terrainData.SetHeights(0, 0, heightMap);
	}

	public void ImageHeightMap()
	{
		if (!m_terrainData)
		{
			Init();
		}

		float widthScale, heightScale;
		float[,] heightMap = m_terrainData.GetHeights(0, 0, m_terrainData.heightmapResolution, m_terrainData.heightmapResolution);

		if (m_heightMapAutoScale)
		{
			widthScale = m_heightMapTexture.width / (float)heightMap.GetLength(0);
			heightScale = m_heightMapTexture.height / (float)heightMap.GetLength(1);
		}
		else
		{
			widthScale = m_heightMapScale.x;
			heightScale = m_heightMapScale.y;
		}

		for (int x = 0; x < heightMap.GetLength(0); ++x)
		{
			for (int y = 0; y < heightMap.GetLength(1); ++y)
			{
				heightMap[x, y] = m_heightMapTexture.GetPixel(Mathf.RoundToInt(x * widthScale), Mathf.RoundToInt(y * heightScale)).grayscale * m_heightMapAmplitude;
			}
		}

		m_terrainData.SetHeights(0, 0, heightMap);
	}

	public void PerlinNoise()
	{
		if (!m_terrainData)
		{
			Init();
		}

		float[,] heightMap = m_terrainData.GetHeights(0, 0, m_terrainData.heightmapResolution, m_terrainData.heightmapResolution);

		for (int x = 0; x < heightMap.GetLength(0); ++x)
		{
			for (int y = 0; y < heightMap.GetLength(1); ++y)
			{
				heightMap[x, y] = Mathf.PerlinNoise((x + m_perlinOffsetX) * m_perlinScaleX, (y + m_perlinOffsetY) * m_perlinScaleY);
			}
		}

		m_terrainData.SetHeights(0, 0, heightMap);
	}

	public void Voronoi()
	{
		if (!m_terrainData)
		{
			Init();
		}

		float[,] heightMap = m_terrainData.GetHeights(0, 0, m_terrainData.heightmapResolution, m_terrainData.heightmapResolution);

		float maxDistance = Vector2.Distance(Vector2.zero, new Vector2(m_terrainData.heightmapResolution, m_terrainData.heightmapResolution));

		for (int i = 0; i < m_nbPeaks; ++i)
		{
			Vector2Int pos = new Vector2Int(UnityEngine.Random.Range(0, m_terrainData.heightmapResolution), UnityEngine.Random.Range(0, m_terrainData.heightmapResolution));

			float alt = UnityEngine.Random.Range(0f, 1f);

			if (heightMap[pos.x, pos.y] < alt)
			{
				heightMap[pos.x, pos.y] = alt;
			}

			for (int x = 0; x < heightMap.GetLength(0); ++x)
			{
				for (int y = 0; y < heightMap.GetLength(1); ++y)
				{
					if(!(x == pos.x && y == alt))
					{
						float distance = Vector2.Distance(pos, new Vector2(x, y)) / maxDistance;
						float height = 0f;
						
						switch(m_type){
						case VoronoiType.Linear:
							height = alt - distance * m_falloff - Mathf.Pow(distance, m_dropoff);
							break;
						case VoronoiType.Exponential:
							height = alt - Mathf.Pow(distance, m_dropoff) * m_falloff;
							break;
						case VoronoiType.Combined:
							height = alt - distance * m_falloff - Mathf.Pow(distance, m_dropoff);
							break;
						}
						
						if (heightMap[x, y] < height)
						{
							heightMap[x, y] = height;
						}
					}
				}
			}
		}

		m_terrainData.SetHeights(0, 0, heightMap);
	}
}
