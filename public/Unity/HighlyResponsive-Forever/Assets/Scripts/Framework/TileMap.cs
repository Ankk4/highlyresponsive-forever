using UnityEngine;
using System.Collections.Generic;

public class TileMap : MonoBehaviour
{
    [Header("Map data")]
    public Vector2 NumOfTiles;
    public GameObject RefTileSet;

    private float TileSize;
    private List<List<GameObject>> Map = new List<List<GameObject>>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool Load()
    {
        // Error checking
        if (NumOfTiles.x <= 0 || NumOfTiles.y <= 0)
        {
            return false;
        }

        // Calculate tile size
        TileSize = ScreenScript.GetScreenSize().x / NumOfTiles.x;

        for (int row = 0; row < NumOfTiles.y; ++row)
        {
            for (int col = 0; col < NumOfTiles.x; ++col)
            {

            }
        }

        return true;
    }
}
