using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    [Header("Map Settings")]
    public int mapWidth = 100;         
    public int mapHeight = 100;
    public float amplitude;
    public bool autoUpdate;
    
    [Header("Noise Settings")]
    public float noiseScale = 25f;
    
    public int octaves = 4;

    [Range(0, 1)]
    public float percistance = .5f;
    public float lacunarity = 2f;

    public int seed;
    public Vector2 offset;

    [Header("Falloff Map Settings")]
    public bool useFalloffMap;

    [Range(0, 1)]
    public float falloffStart;  
    [Range(0, 1)]
    public float falloffEnd;

    float[,] falloffMap;

    private void Awake() {
        falloffMap = FalloffGenerator.GenerateFalloffMap(mapWidth, mapHeight, falloffStart, falloffEnd);
    }

    public void GenerateMap() {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, percistance, lacunarity, offset);

        MapDisplay display = FindObjectOfType<MapDisplay>();

        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                if(useFalloffMap)
                    noiseMap[x, y] = Mathf.Clamp01(falloffMap[x, y] * noiseMap[x, y]);
            }
        }

        //display.DrawNoiseMap(noiseMap);
        //display.DrawNoiseMap(FalloffGenerator.GenerateFalloffMap(mapWidth, mapHeight, falloffStart, falloffEnd));
        display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, amplitude));
    }

    void OnValidate() {
        if(mapWidth < 1)
            mapWidth = 1;
        if(mapHeight < 1)
            mapHeight = 1;
        if(lacunarity < 1)
            lacunarity = 1;
        if(octaves < 0) 
            octaves = 0;
        falloffMap = FalloffGenerator.GenerateFalloffMap(mapWidth, mapHeight, falloffStart, falloffEnd);
    }
}
