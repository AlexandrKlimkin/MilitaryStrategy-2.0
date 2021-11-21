using System;
using UnityEngine;

[Serializable]
public class SquadData
{
    [HideInInspector]
    public string UnitName;
    [HideInInspector]
    public int UnitsCount;
    public Vector2Int Size;

    public SquadData(string unitName, Vector2Int size) {
        UnitName = unitName;
        Size = size;
        UnitsCount = Size.x * Size.y;
    }

    public SquadData(string unitName, int sizeX, int sizeY) {
        UnitName = unitName;
        Size = new Vector2Int(sizeX, sizeY);
        UnitsCount = sizeX * sizeY;
    }

}
