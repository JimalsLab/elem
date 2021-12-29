using UnityEngine;

public class Tile
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }
    public TileType Type { get; set; }
    public GameObject Obj { get; set; }

    public Tile(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Tile(TileType type)
    {
        Type = type;
    }
}