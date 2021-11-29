using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapManager : MonoBehaviour
{
    public int GrassPercentage;
    public int MapSize;
    public Material GGGG;
    public Material DGGG;
    public Material GGDG;
    public Material XXXX;

    public List<Tile> Map;
    // Start is called before the first frame update
    void Start()
    {
        generateMap(MapSize, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void RegenMap()
    {
        Map ??= new List<Tile>();
        foreach (var tile in Map)
        {
            Destroy(tile.Obj);
        }
        Map.Clear();
        generateMap(MapSize, 1, 1);
    }

    private void generateMap(int generationIterations, int playerSlotsNumber, int enemySlotsNumber)
    {
        Map = new List<Tile>();
        var originTile = new Tile(0, 0, 0);
        originTile.Type = TileType.GGDG;
        GameObject originCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        originCube.GetComponent<MeshRenderer>().material = GGDG;
        originTile.Obj = originCube;
        Map.Add(originTile);
        StartCoroutine(generateAdjacentTiles(generationIterations, originTile));
    }

    private IEnumerator generateAdjacentTiles(int generationIterationsLeft,Tile originTile)
    {
        if (generationIterationsLeft > 0)
        {
            generationIterationsLeft -= 1;

            var northTile = new Tile(originTile.X + 1, originTile.Y, originTile.Z);
            if (Map.Where(m => m.X == northTile.X && m.Z == northTile.Z && m.Y == northTile.Y).Count() == 0)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(northTile.X, northTile.Y, northTile.Z);
                northTile.Type = CalculateTileType(northTile, Map);
                cube.GetComponent<MeshRenderer>().material = getMaterialFromType(northTile.Type);
                northTile.Obj = cube;
                Map.Add(northTile);
            }
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(generateAdjacentTiles(generationIterationsLeft, northTile));

            var eastTile = new Tile(originTile.X, originTile.Y, originTile.Z+1);
            if (Map.Where(m => m.X == eastTile.X && m.Z == eastTile.Z && m.Y == eastTile.Y).Count() == 0)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(eastTile.X, eastTile.Y, eastTile.Z);
                eastTile.Type = CalculateTileType(eastTile, Map);
                cube.GetComponent<MeshRenderer>().material = getMaterialFromType(eastTile.Type);
                eastTile.Obj = cube;
                Map.Add(eastTile);
            }
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(generateAdjacentTiles(generationIterationsLeft, eastTile));

            var southTile = new Tile(originTile.X - 1, originTile.Y, originTile.Z);
            if (Map.Where(m => m.X == southTile.X && m.Z == southTile.Z && m.Y == southTile.Y).Count() == 0)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(southTile.X, southTile.Y, southTile.Z);
                southTile.Type = CalculateTileType(southTile, Map);
                cube.GetComponent<MeshRenderer>().material = getMaterialFromType(southTile.Type);
                southTile.Obj = cube;
                Map.Add(southTile);
            }
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(generateAdjacentTiles(generationIterationsLeft, southTile));

            var westTile = new Tile(originTile.X, originTile.Y, originTile.Z - 1);
            if (Map.Where(m => m.X == westTile.X && m.Z == westTile.Z && m.Y == westTile.Y).Count() == 0)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(westTile.X, westTile.Y, westTile.Z);
                westTile.Type = CalculateTileType(westTile, Map);
                cube.GetComponent<MeshRenderer>().material = getMaterialFromType(westTile.Type);
                westTile.Obj = cube;
                Map.Add(westTile);
            }
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(generateAdjacentTiles(generationIterationsLeft, westTile));
        }
    }

    public TileType CalculateTileType(Tile tile, List<Tile> map)
    {
        List<Tile> adjacentTiles = GetAdjacentTiles(tile, map);
        List<string> wildcardType = new List<string>
        {
            adjacentTiles[0]?.Type.ToString().Substring(2, 1) ?? "X",
            adjacentTiles[1]?.Type.ToString().Substring(3, 1) ?? "X",
            adjacentTiles[2]?.Type.ToString().Substring(0, 1) ?? "X",
            adjacentTiles[3]?.Type.ToString().Substring(1, 1) ?? "X"
        };


        List<TileType> availableTileTypes = new List<TileType>();

        foreach (var tileType in Enum.GetValues(typeof(TileType)))
        {
            availableTileTypes.Add((TileType)tileType);
        }

        foreach (var tileType in Enum.GetValues(typeof(TileType)))
        {
            var removeType = false;
            if (wildcardType[0] != "X" && wildcardType[0] != tileType.ToString().Substring(0, 1))
            {
                var a = tileType.ToString().Substring(0, 1);
                removeType = true;
            }
            if (wildcardType[1] != "X" && wildcardType[1] != tileType.ToString().Substring(1, 1))
            {
                removeType = true;
            }
            if (wildcardType[2] != "X" && wildcardType[2] != tileType.ToString().Substring(2, 1))
            {
                removeType = true;
            }
            if (wildcardType[3] != "X" && wildcardType[3] != tileType.ToString().Substring(3, 1))
            {
                removeType = true;
            }

            if (removeType)
            {
                availableTileTypes.Remove((TileType)tileType);
            }
        }

        if (availableTileTypes.Count == 0)
        {
            return TileType.XXXX;
        }

        System.Random random = new System.Random();
        GrassPercentage = GrassPercentage == 0 ? 50 : GrassPercentage;

        var availablePicks = new List<TileType>();
        if (availableTileTypes.Contains(TileType.GGGG) && availableTileTypes.Count > 1)
        {
            availableTileTypes.Remove(TileType.GGGG);
            for (var i = 0; i < 100 - GrassPercentage; i++)
            {
                var r = random.Next(0, availableTileTypes.Count);
                availablePicks.Add(availableTileTypes[r]);
            }
            for (var i = 0; i < GrassPercentage; i++)
            {
                availablePicks.Add(TileType.GGGG);
            }
        }
        else
        {
            availablePicks.AddRange(availableTileTypes);
        }

        var rnd = random.Next(0, availablePicks.Count);
        return availablePicks[rnd];
    }

    public List<Tile> GetAdjacentTiles(Tile tile,List<Tile> map)
    {
        List<Tile> adjacentTiles = new List<Tile>
        {
            map.Where(t => t.X == tile.X+1 && t.Z == tile.Z).FirstOrDefault() ?? new Tile(TileType.XXXX),
            map.Where(t => t.Z == tile.Z+1 && t.X == tile.X).FirstOrDefault() ?? new Tile(TileType.XXXX),
            map.Where(t => t.X == tile.X-1 && t.Z == tile.Z).FirstOrDefault() ?? new Tile(TileType.XXXX),
            map.Where(t => t.Z == tile.Z-1 && t.X == tile.X).FirstOrDefault() ?? new Tile(TileType.XXXX)
        };
        return adjacentTiles;
    }

    public Material getMaterialFromType(TileType type)
    {
        switch (type)
        {
            case TileType.GGGG:
                return GGGG;
            case TileType.DGGG:
                return DGGG;
            case TileType.GGDG:
                return GGDG;
            default:
                return XXXX;
        }
    }

    public class Tile
    {
        public string Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public TileType Type { get; set; }
        public GameObject Obj { get; set; }

        public Tile(int x, int y, int z)
        {
            Id = new Guid().ToString();
            X = x;
            Y = y;
            Z = z;
        }

        public Tile(TileType t)
        {
            Type = t;
        }
    }

    public enum TileType
    {
        XXXX,
        GGGG,
        DGGG,
        GGDG
    }
}
