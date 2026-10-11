using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Tilemaps;

public class WorldGeneration : MonoBehaviour
{
    [Header("World Settings")]
    public int worldWidth = 500;
    public int worldHeight = 200;

    [Header("Generation Settings")]
    public float landmassScale = 0.02f;
    public float temperatureScale = 0.005f;
    public float humidityScale = 0.005f;

    [Header("Noise Settings")]
    public float noiseScale = 0.1f;
    [Range(-1, 1)] public float landThreshold = -0.5f;

    [Header("Tilemaps")]
    public Tilemap backgroundTilemap;
    public Tilemap foregroundTilemap;

    [Header("Tiles")]
    public TileBase waterTile;
    public TileBase grassTile;
    public TileBase desertTile;
    public TileBase snowTile;
        public TileBase jungleTile;
    public TileBase taigaTile;
    public TileBase steppeTile;
    public TileBase tundraTile;
    public TileBase savannaTile; 


    public int seed = 0;
    float[,] landmassArray;
    float[,] temperatureArray;
    float[,] humidityArray;

    void Start()
    {
        GenerateWorld();
    }

    void GenerateWorld()
    {
        foregroundTilemap.ClearAllTiles();
        // backgroundTilemap.ClearAllTiles();
        var offset = new float2(seed, seed);
        landmassArray = new float[worldWidth, worldHeight];
        temperatureArray = new float[worldWidth, worldHeight];
        humidityArray = new float[worldWidth, worldHeight];

        for(int x = 0; x < worldWidth; x++)
        {
            for(int y = 0; y < worldHeight; y++)
            {
                var position = new float2(
                    x * noiseScale + offset.x,
                    y * noiseScale + offset.y
                );
                

                landmassArray[x, y] = noise.snoise(position);
                temperatureArray[x, y] = noise.snoise(position + 2453);
                humidityArray[x, y] = noise.snoise(position + 3333);

                var landmassNoise = noise.snoise(new float2(
                    x * landmassScale + offset.x,
                    y * landmassScale + offset.y
                    ));
                var temperatureNoise = noise.snoise(new float2(
                    x * temperatureScale + offset.x,
                    y * temperatureScale + offset.y
                    ));

                var humidityNoise = noise.snoise(new float2(
                    x * humidityScale + offset.x,
                    y * humidityScale + offset.y
                    ));
            }
        }


        for(int x = 0; x < worldWidth; x++)
        {
            for(int y = 0; y < worldHeight; y++)
            {
                Vector3Int cell = new Vector3Int(x, y, 0);

                float land = landmassArray[x, y];
                float temperature = temperatureArray[x, y];
                float humidity = humidityArray[x, y];

                if(land < landThreshold)
                {
                    foregroundTilemap.SetTile(cell, waterTile);
                    continue;
                }
                
                if(temperature < -0.33f && humidity < -0.33f)
                {
                    foregroundTilemap.SetTile(cell, snowTile);
                }
                else if(temperature < -0.33f && humidity > 0.33f)
                {
                    foregroundTilemap.SetTile(cell, taigaTile);
                }
                else if (temperature < -0.33f)
                {
                    foregroundTilemap.SetTile(cell, tundraTile);
                }
                else if (temperature <= 0.33f && humidity < -0.33f)
                {
                    foregroundTilemap.SetTile(cell, steppeTile);
                }
                else if (temperature <= 0.33f)
                {
                    foregroundTilemap.SetTile(cell, grassTile);
                }
                else if (temperature > 0.33f && humidity < -0.33f)
                {
                    foregroundTilemap.SetTile(cell, desertTile);
                }
                else if (temperature > 0.33f && humidity > 0.33f)
                {
                    foregroundTilemap.SetTile(cell, jungleTile);
                }
                else
                {
                    foregroundTilemap.SetTile(cell, savannaTile);
                }
            }
        }
        
    }
}
