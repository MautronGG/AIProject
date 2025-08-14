using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileData
{
    public int m_prefabIndex;
    public Vector2 m_position;
}

public class LevelData
{
    public List<TileData> m_tiles = new List<TileData>();
}
