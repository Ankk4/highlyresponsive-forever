using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

public class TileMap : MonoBehaviour
{
    public static char TILE_SPLIT = ',';
    public static string MAP_DIRECTORY = "Maps\\";
    //public static string MAP_EXTENSION = ".map";

    [Header("Map data")]
    public Vector2 NumOfTiles;

    [Header("Tile set")]
    public GameObject RefTileSet;

    // Properties
    public List<List<Tile>> Map { get { return map; } }

    private float tileSize;
    private List<List<Tile>> map = new List<List<Tile>>(); // Row followed by column (Map[row number][col number])
    private List<Tile> tiles;

    // Use this for initialization
    void Start()
    {
        tiles = RefTileSet.GetComponentsInChildren<Tile>(true).ToList();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool Load(string name)
    {
        // Load file
        string path = MAP_DIRECTORY + name;
        var file = Resources.Load<TextAsset>(path);
        if (file)
        {
            List<List<int>> iMap = new List<List<int>>();
            var lines = file.text.Split('\n');

            // Convert string to int
            for (int lineNum = 0; lineNum < lines.Length; ++lineNum)
            {
                List<int> tempTiles = new List<int>(); // Temp row of tiles
                var tiles = lines[lineNum].Split(TILE_SPLIT); // Splitting row into individual tile ID in string
                // Converting string tile into tile ID and adding to temp tile list
                foreach (var tile in tiles)
                {
                    int tileID;
                    if (Int32.TryParse(tile, out tileID))
                    {
                        tempTiles.Add(tileID);
                    }
                }

                // Adding temp tiles to map
                iMap.Add(tempTiles);
            }

            if (loadMap(iMap))
            {
                return true;
            }
        }

        return false;
    }

    private bool loadMap(List<List<int>> iMap)
    {
        // Error checking
        if (NumOfTiles.x <= 0 || NumOfTiles.y <= 0 || iMap == null)
        {
            return false;
        }

        // Common data
        var screenSize = ScreenScript.GetScreenSize() - new Vector2(0, 4);
        tileSize = screenSize.x / NumOfTiles.x; // Calculate tile size

        for (int row = 0; row < NumOfTiles.y; ++row)
        {
            List<Tile> tempRow = new List<Tile>();

            for (int col = 0; col < NumOfTiles.x; ++col)
            {
                // Load tile from map
                int tileID = iMap[row][col];
                if (tileID != 0 && tileID < tiles.Count) // Skip empty tile
                {
                    var newTile = Instantiate(tiles[tileID]);
                    newTile.transform.parent = transform;
                    newTile.transform.localPosition = calcPos(screenSize, NumOfTiles, tileSize, row, col);
                    newTile.transform.localScale = new Vector3(tileSize, tileSize);

                    tempRow.Add(newTile); // Add to temp row

                    // Add to tile count
                    if (tileID == 1)
                    {
                        ++(transform.root.GetComponent<GameManager>().TileCount);
                    }
                }
                else
                {
                    // Empty tile
                    tempRow.Add(null);
                }
            }

            // Add temp row to map
            map.Add(tempRow);
        }

        // Set up default tile count
        transform.root.GetComponent<GameManager>().DefaultTileCount = transform.root.GetComponent<GameManager>().TileCount;

        return true;
    }

    public bool Unload()
    {
        // Destroy
        foreach (var row in map)
        {
            foreach (var tile in row)
            {
                Destroy(tile.gameObject);
            }
            row.Clear();
        }

        // Clear map
        map.Clear();

        return true;
    }

    public void Reset()
    {
        foreach (var row in map)
        {
            foreach (var tile in row)
            {
                if (tile)
                {
                    tile.Reset();
                }
            }
        }
    }

    private Vector2 calcPos(Vector2 screenSize, Vector2 numOfTiles, float tileSize, int rowIndex = 0, int colIndex = 0)
    {
        Vector2 offsetToTopLeft = new Vector2(-tileSize * 0.5f, tileSize * 0.5f); // Offset to bottom left of the tile based on tile's origin point

        return new Vector2(-screenSize.x * 0.5f + (colIndex * tileSize), screenSize.y * 0.5f - (rowIndex * tileSize)) - offsetToTopLeft;
    }
}
