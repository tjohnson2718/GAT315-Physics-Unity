using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    // Variables
    public int width = 256;
    public int height = 256;
    public float scale = 20f;
    public Material grassMaterial;
    public Material rockMaterial;
    public Material snowMaterial;

    private void Start()
    {
        GenerateTerrain();
    }

    // Generate terrain using Perlin noise and apply materials
    void GenerateTerrain()
    {
        // Get the Terrain component attached to the GameObject
        Terrain terrain = GetComponent<Terrain>();

        // Generate terrain data and assign it to the terrain object
        terrain.terrainData = GenerateTerrain(terrain.terrainData);

        // Apply materials to the terrain based on its height
        ApplyMaterials(terrain);
    }

    // Generate terrain data using Perlin noise
    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        // Set the heightmap resolution of the terrain data
        terrainData.heightmapResolution = width + 1;

        // Set the size of the terrain
        terrainData.size = new Vector3(width, 100, height);

        // Generate heights for the terrain using Perlin noise
        terrainData.SetHeights(0, 0, GenerateHeights());

        // Return the modified terrain data
        return terrainData;
    }

    // Generate heights for the terrain using Perlin noise
    float[,] GenerateHeights()
    {
        // Initialize a 2D array to store height values
        float[,] heights = new float[width, height];

        // Loop through each pixel in the terrain grid
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Calculate coordinates within the Perlin noise space
                float xCoord = (float)x / width * scale;
                float yCoord = (float)y / height * scale;

                // Generate Perlin noise sample for the current coordinates
                float sample = Mathf.PerlinNoise(xCoord, yCoord);

                // Assign the Perlin noise sample as the height value
                heights[x, y] = sample;
            }
        }

        // Return the generated heights
        return heights;
    }

    // Apply materials to the terrain based on its height
    void ApplyMaterials(Terrain terrain)
    {
        // Get the TerrainData component of the terrain
        TerrainData terrainData = terrain.terrainData;

        // Get the heightmap data from the TerrainData
        float[,] heights = terrainData.GetHeights(0, 0, width, height);

        // Initialize a 2D array to store materials for each point on the terrain grid
        Material[,] materials = new Material[width, height];

        // Loop through each pixel in the terrain grid
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Get the normalized height value for the current point
                float normalizedHeight = heights[x, y];

                // Determine the material based on the height value
                if (normalizedHeight < 0.3f) // Grass/Dirt
                {
                    materials[x, y] = grassMaterial;
                }
                else if (normalizedHeight < 0.7f) // Rock
                {
                    materials[x, y] = rockMaterial;
                }
                else // Snow
                {
                    materials[x, y] = snowMaterial;
                }
            }
        }

        // Create and set the terrain material
        terrain.materialTemplate = CreateTerrainMaterial(materials);
    }

    // Create a terrain material using texture arrays
    Material CreateTerrainMaterial(Material[,] materials)
    {
        // Create a texture array to store textures for different materials
        Texture2DArray textureArray = new Texture2DArray(grassMaterial.mainTexture.width, grassMaterial.mainTexture.height, 3, TextureFormat.RGBA32, true);

        // Loop through each material type
        for (int i = 0; i < 3; i++)
        {
            // Get the texture for the current material type
            Texture2D texture = null;
            if (i == 0)
                texture = (Texture2D)grassMaterial.mainTexture;
            else if (i == 1)
                texture = (Texture2D)rockMaterial.mainTexture;
            else
                texture = (Texture2D)snowMaterial.mainTexture;

            // Set the pixels of the texture array for the current material type
            textureArray.SetPixels(texture.GetPixels(), i);
        }

        // Apply changes to the texture array
        textureArray.Apply();

        // Create a new material based on the grass material shader
        Material terrainMaterial = new Material(grassMaterial.shader);

        // Set the texture array as the albedo map for the terrain material
        terrainMaterial.SetTexture("_AlbedoMapArray", textureArray);

        // Return the terrain material
        return terrainMaterial;
    }
}
