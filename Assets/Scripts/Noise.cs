using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int width, int height, int seed, float noiseScale, int octaves, float persistence, float lacunarity, Vector2 offset){
        float [,] noisemap = new float[width, height];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for(int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if( noiseScale <= 0 ){
            noiseScale = 0.0001f;
        }

        float maxnoiseHeight = float.MinValue;
        float minnoiseHeight = float.MaxValue;

        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        for ( int y = 0; y < height; y++ ){
            for( int x = 0; x < width; x++ ){

                float amplitude = 1;
                float friquency = 1;
                float noiseHight = 0;

                for( int o = 0; o < octaves; o++)
                {
                    float sampleX = (x - halfWidth) / noiseScale * friquency + octaveOffsets[o].x;
                    float sampleY = (y - halfHeight) / noiseScale * friquency + octaveOffsets[o].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHight += perlinValue * amplitude;

                    amplitude *= persistence;
                    friquency *= lacunarity;
                }

                if( noiseHight > maxnoiseHeight)
                {
                    maxnoiseHeight = noiseHight;
                }else if(noiseHight < minnoiseHeight)
                {
                    minnoiseHeight = noiseHight;
                }
                noisemap[x, y] = noiseHight;
            }
        }


        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                noisemap[x, y] = Mathf.InverseLerp(minnoiseHeight, maxnoiseHeight, noisemap[x, y]);
            }
        }
                
 
        return noisemap;
    }
}
