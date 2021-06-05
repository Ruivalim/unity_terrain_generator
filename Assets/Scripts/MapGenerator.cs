using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour
{

    public enum DrawMode {NoiseMap, ColorMap, Mesh};
    public DrawMode drawMode;

    const int mapChunkSize = 241;

    const int mapWidth = mapChunkSize;
    const int mapHeight = mapChunkSize;

    [Range(0,6)]
    public int levelOfDetail;
    public float noiseScale;
    public int octaves;
    [Range(0,1)]
    public float persistence;
    public float lacunarity;
    public float heightMultiplier;

    public AnimationCurve meshHightCurve;

    public int seed;
    public Vector2 offset;

    public bool autoUpdate;

    public TerrainTypes[] regions;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistence, lacunarity, offset);

        Color[] colarMap = new Color[mapHeight * mapWidth];
        for(int y = 0; y < mapHeight; y++)
        {
            for(int x = 0; x < mapWidth; x++)
            {
                float currentHight = noiseMap[x, y];
                for( int i = 0; i < regions.Length; i++)
                {
                    if (currentHight <= regions[i].height)
                    {
                        colarMap[y * mapWidth + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        MapDisplay mapDisplay = FindObjectOfType<MapDisplay>();

        if( drawMode == DrawMode.NoiseMap)
        {
            mapDisplay.DrawTexture(TextureGenerator.textureFromHeightMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColorMap)
        {
            mapDisplay.DrawTexture(TextureGenerator.textureFromColorMap(colarMap, mapWidth, mapHeight));
        }
        else if( drawMode == DrawMode.Mesh)
        {
            mapDisplay.DrawMesh(MashGenerator.GenerateTerrainMesh(noiseMap, heightMultiplier, meshHightCurve, levelOfDetail), TextureGenerator.textureFromColorMap(colarMap, mapWidth, mapHeight));
        }
        
    }

    private void OnValidate()
    {
        if( lacunarity < 1)
        {
            lacunarity = 1;
        }
        if( octaves < 0)
        {
            octaves = 0;
        }
        if (heightMultiplier < 1)
        {
            heightMultiplier = 1;
        }
    }

}

[System.Serializable]
public struct TerrainTypes
{
    public string name;
    public float height;
    public Color color;
}