using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode{noiseMap, colorMap, Mesh};
    public DrawMode drawMode;

    //241-1 has great characteristics (it is divisible by 2,4,8,12)
    const int mapChunkSize = 241;
    [Range(0,6)]
    public int levelOfDetail;
        //deprecated - we use chunkSize instead
        //public int mapHeight;
        //public int mapWidth;
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public bool autoUpdate;

    public TerrainType[] regions;

    public void generateMap(){
        float[,] noiseMap = PerlinNoise.generateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale,octaves, persistance, lacunarity, offset);

        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];

        for(int y = 0; y < mapChunkSize; y++){
            for(int x = 0; x < mapChunkSize; x++){
                float currentHeight = noiseMap[x,y];
                for(int i = 0; i < regions.Length; i++){
                    if(currentHeight <= regions[i].height){
                        colorMap[y*mapChunkSize + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        MapDisplay mapDisplay = FindObjectOfType<MapDisplay>();

        if(drawMode == DrawMode.noiseMap)
            mapDisplay.drawTexture(TextureGenerator.textureFromHeightMap(noiseMap)); 
        else if(drawMode == DrawMode.colorMap){
            mapDisplay.drawTexture(TextureGenerator.textureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
        }
        else if(drawMode == DrawMode.Mesh){
            mapDisplay.drawMesh(MeshGenerator.generateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail),TextureGenerator.textureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
        }
    }

    // calls itself whenever one of the scripts variable is changed
    void OnValidate(){
        if(lacunarity < 1){
            lacunarity = 1;
        }

        if(octaves < 0)
            octaves = 0;
    }

    [System.Serializable]
    public struct TerrainType{
        
        public string name;
        public float height;
        public Color color;


    };
}
