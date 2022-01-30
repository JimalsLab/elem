using Assets.src.scripts;
using Assets.src.scripts.entities;
using Assets.src.scripts.tools;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public List<MapManager> maps = new List<MapManager>();
    public List<Heroe> heroes = new List<Heroe>();
    public List<Enemy> enemies = new List<Enemy>();
    public WorldMapManager WorldMapManager;

    int level = 0;

    public EnemyLevelBracketMatcher enemyMatcher;

    public void Start()
    {
        WorldMapManager = gameObject.GetComponent<WorldMapManager>();

        GetHeroes();
        GetEnemies();
        
        LaunchLevel(level, heroes, enemies);
    }

    public void Update()
    {
        
    }

    public void GetHeroes()
    {
        heroes = new List<Heroe>() { WorldMapManager.heroe };
    }

    public void GetEnemies()
    {
        enemyMatcher = new EnemyLevelBracketMatcher();
        var enemyTypes = enemyMatcher.GetEnemyTypesFromLevelBracket(level);
        enemies = new List<Enemy>(){ new Enemy(SelectEnemy(enemyTypes), level) };
    }

    public void LaunchLevel(int lvl, List<Heroe> heroes, List<Enemy> enemies)
    {
        var mapManager = gameObject.AddComponent<MapManager>();
        maps.Add(mapManager);

        maps.Last().InitMapWithParam(lvl);

        var combatManager = gameObject.AddComponent<CombatManager>();

        combatManager.InitiateCombat(mapManager, heroes, enemies);
    }

    private EnemyType SelectEnemy(List<EnemyType> enemyTypes)
    {
        var rnd = level * enemyTypes.Count() * WorldMapManager.Seed;
        return enemyTypes[(int)(rnd % enemyTypes.Count())];
    }
}
