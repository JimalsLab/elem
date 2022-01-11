using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public List<MapManager> maps = new List<MapManager>();

    public void Start()
    {
        var mapManager = gameObject.AddComponent<MapManager>();
        maps.Add(mapManager);

        maps.Last().InitMapWithParam();
    }
}
