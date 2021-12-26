using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapManager : MonoBehaviour
{
    public long seed;
    public int GrassPercentage = 50;
    public int DirtPercentage = 25;
    public int MapSize;
    public Material GGGG;
    public Material DGGG;
    public Material GDGG;
    public Material GGDG;
    public Material GGGD;
    public Material DDDD;
    public Material XXXX;

    public List<Tile> Map;
    // Start is called before the first frame update
    void Start()
    {
        generateMap(seed, MapSize, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void RegenMap()
    {
        seed += 1;
        Map ??= new List<Tile>();
        foreach (var tile in Map)
        {
            Destroy(tile.Obj);
        }
        Map.Clear();
        generateMap(seed, MapSize, 1, 1);
    }

    private void generateMap(long seed,int generationIterations, int playerSlotsNumber, int enemySlotsNumber)
    {
        Map = new List<Tile>();
        var originTile = new Tile(0, 0, 0);
        originTile.Type = TileType.GGDG;
        GameObject originCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        originCube.GetComponent<MeshRenderer>().material = GGDG;
        originTile.Obj = originCube;
        Map.Add(originTile);
        StartCoroutine(generateAdjacentTiles(seed, generationIterations, originTile));
    }

    private IEnumerator generateAdjacentTiles(long seed,int generationIterationsLeft,Tile originTile)
    {
        if (generationIterationsLeft > 0)
        {
            generationIterationsLeft -= 1;

            var northTile = new Tile(originTile.X + 1, originTile.Y, originTile.Z);
            if (Map.Where(m => m.X == northTile.X && m.Z == northTile.Z && m.Y == northTile.Y).Count() == 0)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(northTile.X, northTile.Y, northTile.Z);
                northTile.Type = CalculateTileType(seed,northTile, Map);
                cube.GetComponent<MeshRenderer>().material = getMaterialFromType(northTile.Type);
                northTile.Obj = cube;
                Map.Add(northTile);
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(generateAdjacentTiles(seed*2,generationIterationsLeft, northTile));
            }

            var eastTile = new Tile(originTile.X, originTile.Y, originTile.Z+1);
            if (Map.Where(m => m.X == eastTile.X && m.Z == eastTile.Z && m.Y == eastTile.Y).Count() == 0)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(eastTile.X, eastTile.Y, eastTile.Z);
                eastTile.Type = CalculateTileType(seed, eastTile, Map);
                cube.GetComponent<MeshRenderer>().material = getMaterialFromType(eastTile.Type);
                eastTile.Obj = cube;
                Map.Add(eastTile);
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(generateAdjacentTiles(seed*3,generationIterationsLeft, eastTile));
            }

            var southTile = new Tile(originTile.X - 1, originTile.Y, originTile.Z);
            if (Map.Where(m => m.X == southTile.X && m.Z == southTile.Z && m.Y == southTile.Y).Count() == 0)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(southTile.X, southTile.Y, southTile.Z);
                southTile.Type = CalculateTileType(seed, southTile, Map);
                cube.GetComponent<MeshRenderer>().material = getMaterialFromType(southTile.Type);
                southTile.Obj = cube;
                Map.Add(southTile);
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(generateAdjacentTiles(seed*5,generationIterationsLeft, southTile));
            }

            var westTile = new Tile(originTile.X, originTile.Y, originTile.Z - 1);
            if (Map.Where(m => m.X == westTile.X && m.Z == westTile.Z && m.Y == westTile.Y).Count() == 0)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(westTile.X, westTile.Y, westTile.Z);
                westTile.Type = CalculateTileType(seed, westTile, Map);
                cube.GetComponent<MeshRenderer>().material = getMaterialFromType(westTile.Type);
                westTile.Obj = cube;
                Map.Add(westTile);
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(generateAdjacentTiles(seed*7,generationIterationsLeft, westTile));
            }
        }
    }

    public TileType CalculateTileType(long seed, Tile tile, List<Tile> map)
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

        while (DirtPercentage+GrassPercentage >= (100-availableTileTypes.Count))
        {
            DirtPercentage -= availableTileTypes.Count;
            GrassPercentage -= availableTileTypes.Count;
        }

        var availablePicks = new List<TileType>();
        if (availableTileTypes.Contains(TileType.GGGG) && availableTileTypes.Count > 1)
        {
            availableTileTypes.Remove(TileType.GGGG);
            for (var i = 0; i < 100 - GrassPercentage - DirtPercentage; i++)
            {
                var r = Mathf.Abs((int)seed%availableTileTypes.Count);
                availablePicks.Add(availableTileTypes[r]);
                seed *= 3;
            }
            for (var i = 0; i < GrassPercentage; i++)
            {
                availablePicks.Add(TileType.GGGG);
            }
            for (var i = 0; i < DirtPercentage; i++)
            {
                availablePicks.Add(TileType.DDDD);
            }
        }
        else
        {
            availablePicks.AddRange(availableTileTypes);
        }

        return availablePicks[Mathf.Abs((int)seed % availablePicks.Count)];
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
            case TileType.GDGG:
                return GDGG;
            case TileType.GGDG:
                return GGDG;
            case TileType.GGGD:
                return GGGD;
            case TileType.DDDD:
                return DDDD;
            default:
                return XXXX;
        }
    }
    public enum TileType
    {
        XXXX,
        GGGG,
        DGGG,
        GDGG,
        GGDG,
        GGGD,
        DDDD
    }
}
