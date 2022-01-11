using UnityEngine;

public class Tile
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public TileType Type { get; set; }
    public GameObject Obj { get; set; }
    public bool isLocked {get;set;}
    public Vector3 destination { get; set; }

    public Tile(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
        isLocked = false;
    }

    public Tile(TileType type)
    {
        Type = type;
        isLocked = false;
    }
}