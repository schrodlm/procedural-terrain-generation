using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PerlinNoise
{
    public static float[,] generateNoiseMap(int mapWidth, int mapHeight,int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset){
        float[,] noiseMap = new float[mapWidth,mapHeight]; 


        //seeding and offset set by user
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for(int i = 0; i < octaves; i++){
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            
            octaveOffsets[i] = new Vector2(offsetX,offsetY);
        }

        if(scale <= 0 ){
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;
        
        //when manipulating with map in editor, when upping scale we "zoom" into the top right corner but
        //zooming in the middle would make more sense
        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;



        for(int y = 0; y < mapHeight; y++){
            for(int x = 0; x < mapWidth; x++){

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for(int i = 0; i < octaves; i++){
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].x;

                    //*2 - 1 is there because we want PerlinNoise to create points from [-1,1]
                    float perlinValue = Mathf.PerlinNoise(sampleX,sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity; 
            
                }
            if(noiseHeight > maxNoiseHeight){
                maxNoiseHeight = noiseHeight;
            }
            else if(noiseHeight < minNoiseHeight){
                minNoiseHeight = noiseHeight;
            }


            noiseMap[x,y] = noiseHeight;
            }
        }

        for(int y = 0; y < mapHeight; y++){
            for(int x = 0; x < mapWidth; x++){
                //InverseLerp returns value between 0 and 1
                noiseMap[x,y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x,y]);
            }
        }

        return noiseMap;
    }
}
 