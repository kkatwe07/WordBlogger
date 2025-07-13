using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // added more vowels for constant letters to be spawned in endless mode
    public const string letters = "AAABCDEEEFGHIIIJKLMNOOOPQRSTUUUVWXYZ"; 
    private const float TILE_SIZE = 220f;

    public GameObject tilePrefab;
    public Transform gridParent;
    public Vector2Int gridSize = new Vector2Int(4, 4);

    private LetterTile[,] grid;

    public void InitGrid()
    {
        Vector2 offset = GetGridOffset();
        grid = new LetterTile[gridSize.x, gridSize.y];

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                GameObject tileGO = Instantiate(tilePrefab, gridParent);
                tileGO.transform.localPosition = new Vector3(
                    x * TILE_SIZE + offset.x,
                    -y * TILE_SIZE + offset.y,
                    0
                );
                LetterTile tile = tileGO.GetComponent<LetterTile>();

                tile.Init(RandomLetter(), TileType.Normal, x, y);
                grid[x, y] = tile;
            }
        }
    }

    private Vector2 GetGridOffset()
    {
        float width = gridSize.x * TILE_SIZE;
        float height = gridSize.y * TILE_SIZE;

        return new Vector2(-width / 2f + TILE_SIZE / 2f, height / 2f - TILE_SIZE / 2f);
    }

    public void FillEmptyTiles()
    {
        Vector2 offset = GetGridOffset();

        for (int x = 0; x < gridSize.x; x++)
        {
            List<LetterTile> columnTiles = new List<LetterTile>();

            for (int y = 0; y < gridSize.y; y++)
            {
                if (grid[x, y] != null)
                {
                    columnTiles.Add(grid[x, y]);
                    grid[x, y] = null;
                }
            }

            int currentY = gridSize.y - 1;
            foreach (var tile in columnTiles)
            {
                grid[x, currentY] = tile;
                tile.X = x;
                tile.Y = currentY;

                tile.transform.localPosition = new Vector3(
                    x * TILE_SIZE + offset.x,
                    -currentY * TILE_SIZE + offset.y,
                    0
                );

                currentY--;
            }

            for (int y = currentY; y >= 0; y--)
            {
                GameObject newTileGO = Instantiate(tilePrefab, gridParent);
                newTileGO.transform.localPosition = new Vector3(
                    x * TILE_SIZE + offset.x,
                    -y * TILE_SIZE + offset.y,
                    0
                );

                LetterTile newTile = newTileGO.GetComponent<LetterTile>();
                newTile.Init(RandomLetter(), TileType.Normal, x, y);
                grid[x, y] = newTile;
            }
        }
    }

    private char RandomLetter()
    {
        return letters[Random.Range(0, letters.Length)];
    }

    public void LoadStaticGrid(List<GridTileInfo> tileData)
    {
        ClearGrid();

        int index = 0;

        Vector2 offset = GetGridOffset();

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                GridTileInfo info = tileData[index++];

                GameObject tileGO = Instantiate(tilePrefab, gridParent);
                tileGO.transform.SetParent(gridParent, false);
                tileGO.transform.localPosition = new Vector3(x * TILE_SIZE + offset.x, -y * TILE_SIZE + offset.y, 0);
                LetterTile tile = tileGO.GetComponent<LetterTile>();
                tile.Init(info.letter[0], info.tileType, x, y);
                grid[x, y] = tile;
            }
        }
    }

    public LetterTile GetTileAt(int x, int y)
    {
        if (x >= 0 && x < gridSize.x && y >= 0 && y < gridSize.y)
            return grid[x, y];
        return null;
    }

    public void ClearGrid()
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        grid = new LetterTile[gridSize.x, gridSize.y];
    }

    public void SetGridSize(Vector2Int size)
    {
        gridSize = size;
    }

    public void ClearTileAt(int x, int y)
    {
        grid[x, y] = null;
    }

}