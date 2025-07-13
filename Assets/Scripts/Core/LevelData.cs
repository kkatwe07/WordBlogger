using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public int bugCount;
    public int wordCount;
    public int timeSec;
    public int totalScore;
    public Vector2Int gridSize;
    public List<GridTileInfo> gridData;
}

[Serializable]
public class GridTileInfo
{
    public TileType tileType;
    public string letter;
}

[Serializable]
public class LevelDataCollection
{
    public List<LevelData> data;
}