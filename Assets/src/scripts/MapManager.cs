using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapManager : MonoBehaviour
{
    public long seed;
    public float scale = 1f;
    private float parsedSeed = 0.232432423424f;
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
        parsedSeed = float.Parse("0," + seed.ToString());

        RegenMap();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void RegenMap()
    {
        parsedSeed = float.Parse("0," + seed.ToString());
        Map ??= new List<Tile>();
        foreach (var tile in Map)
        {
            Destroy(tile.Obj);
        }
        Map.Clear();
        GenerateMap(seed, MapSize, 1, 1, scale);
        StartCoroutine(GenerateHills(Map));
    }

    private void GenerateMap(long seed, int generationIterations, int playerSlotsNumber, int enemySlotsNumber, float scale)
    {
        Map = new List<Tile>();
        var originTile = new Tile(0, 0, 0);
        originTile.Type = TileType.GGDG;
        GameObject originCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        originCube.GetComponent<MeshRenderer>().material = GGDG;
        originTile.Obj = originCube;
        Map.Add(originTile);
        StartCoroutine(generateAdjacentTiles(seed, generationIterations, originTile, scale));
    }

    private IEnumerator generateAdjacentTiles(long seed, int generationIterationsLeft, Tile originTile, float scale)
    {
        if (generationIterationsLeft > 0)
        {
            generationIterationsLeft -= 1;

            var northTile = new Tile(originTile.X + 1, CalculateNoise(originTile.X + 1, originTile.Z, scale), originTile.Z);
            if (!Map.Any(m => m.X == northTile.X && m.Z == northTile.Z && m.Y == northTile.Y))
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(northTile.X, northTile.Y, northTile.Z);
                northTile.Type = CalculateTileType(seed, northTile, Map);
                cube.GetComponent<MeshRenderer>().material = getMaterialFromType(northTile.Type);
                northTile.Obj = cube;
                Map.Add(northTile);
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(generateAdjacentTiles(seed * 2, generationIterationsLeft, northTile, scale));
            }

            var eastTile = new Tile(originTile.X, CalculateNoise(originTile.X, originTile.Z + 1, scale), originTile.Z + 1);
            if (!Map.Any(m => m.X == eastTile.X && m.Z == eastTile.Z && m.Y == eastTile.Y))
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(eastTile.X, eastTile.Y, eastTile.Z);
                eastTile.Type = CalculateTileType(seed, eastTile, Map);
                cube.GetComponent<MeshRenderer>().material = getMaterialFromType(eastTile.Type);
                eastTile.Obj = cube;
                Map.Add(eastTile);
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(generateAdjacentTiles(seed * 3, generationIterationsLeft, eastTile, scale));
            }

            var southTile = new Tile(originTile.X - 1, CalculateNoise(originTile.X - 1, originTile.Z, scale), originTile.Z);
            if (!Map.Any(m => m.X == southTile.X && m.Z == southTile.Z && m.Y == southTile.Y))
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(southTile.X, southTile.Y, southTile.Z);
                southTile.Type = CalculateTileType(seed, southTile, Map);
                cube.GetComponent<MeshRenderer>().material = getMaterialFromType(southTile.Type);
                southTile.Obj = cube;
                Map.Add(southTile);
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(generateAdjacentTiles(seed * 5, generationIterationsLeft, southTile, scale));
            }

            var westTile = new Tile(originTile.X, CalculateNoise(originTile.X, originTile.Z - 1, scale), originTile.Z - 1);
            if (!Map.Any(m => m.X == westTile.X && m.Z == westTile.Z && m.Y == westTile.Y))
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(westTile.X, westTile.Y, westTile.Z);
                westTile.Type = CalculateTileType(seed, westTile, Map);
                cube.GetComponent<MeshRenderer>().material = getMaterialFromType(westTile.Type);
                westTile.Obj = cube;
                Map.Add(westTile);
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(generateAdjacentTiles(seed * 7, generationIterationsLeft, westTile, scale));
            }
        }
    }

    private IEnumerator GenerateHills(List<Tile> map)
    {
        yield return new WaitForSeconds(0.1f);
        //pas de x- z+

        int randomX = MapSize -1 - int.Parse(seed.ToString().Substring(0, 1));
        int randomZ = MapSize -1 - int.Parse(seed.ToString().Substring(seed.ToString().Length -1, 1));

        var centerTile = GetClosestTile(randomX, randomZ, map);
    }

    private Tile GetClosestTile(int x, int z, List<Tile> map)
    {
        if (x < 0 && z > 0) // pour pas avoir la vision encombrée
        {
            x = Mathf.Abs(x);
        }

        while (!Map.Any(t => t.X == x && t.Z == z))
        {
            if (x < 0)
            {
                x += 1;
            }
            else
            {
                x -= 1;
            }
            if (z < 0)
            {
                z += 1;
            }
            else
            {
                z -= 1;
            }
        }

        return map.FirstOrDefault(t => t.X == x && t.Z == z);
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

        while (DirtPercentage + GrassPercentage >= (100 - availableTileTypes.Count))
        {
            DirtPercentage -= availableTileTypes.Count;
            GrassPercentage -= availableTileTypes.Count;
        }

        var availablePicks = new List<TileType>();
        if (availableTileTypes.Contains(TileType.GGGG) && availableTileTypes.Contains(TileType.DDDD) && availableTileTypes.Count > 1)
        {
            availableTileTypes.Remove(TileType.GGGG);
            availableTileTypes.Remove(TileType.DDDD);
            for (var i = 0; i < 100 - GrassPercentage - DirtPercentage; i++)
            {
                var r = Mathf.Abs((int)seed % availableTileTypes.Count);
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
        else if (availableTileTypes.Contains(TileType.DDDD) && availableTileTypes.Count > 1)
        {
            availableTileTypes.Remove(TileType.DDDD);
            for (var i = 0; i < 100 - DirtPercentage; i++)
            {
                var r = Mathf.Abs((int)seed % availableTileTypes.Count);
                availablePicks.Add(availableTileTypes[r]);
                seed *= 3;
            }
            for (var i = 0; i < DirtPercentage; i++)
            {
                availablePicks.Add(TileType.DDDD);
            }
        }
        else if (availableTileTypes.Contains(TileType.GGGG) && availableTileTypes.Count > 1)
        {
            availableTileTypes.Remove(TileType.GGGG);
            for (var i = 0; i < 100 - GrassPercentage; i++)
            {
                var r = Mathf.Abs((int)seed % availableTileTypes.Count);
                availablePicks.Add(availableTileTypes[r]);
                seed *= 3;
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

        return availablePicks[Mathf.Abs((int)seed % availablePicks.Count)];
    }

    public List<Tile> GetAdjacentTiles(Tile tile, List<Tile> map)
    {
        List<Tile> adjacentTiles = new List<Tile>
        {
            map.FirstOrDefault(t => t.X == tile.X+1 && t.Z == tile.Z) ?? new Tile(TileType.XXXX),
            map.FirstOrDefault(t => t.Z == tile.Z+1 && t.X == tile.X) ?? new Tile(TileType.XXXX),
            map.FirstOrDefault(t => t.X == tile.X-1 && t.Z == tile.Z) ?? new Tile(TileType.XXXX),
            map.FirstOrDefault(t => t.Z == tile.Z-1 && t.X == tile.X) ?? new Tile(TileType.XXXX)
        };
        return adjacentTiles;
    }

    private float CalculateNoise(float x, float z, float scale)
    {
        var calculatedX = x + x / (parsedSeed * x + 1) * scale;
        var calculatedZ = z + z / (parsedSeed * z + 1) * scale;
        var pn = Mathf.PerlinNoise(calculatedX, calculatedZ);
        return pn;
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
}
