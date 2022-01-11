using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class WorldMapManager : MonoBehaviour {

    public List<LevelManager> levels;
    public float difficultyScale;
    public float lootScale;
    public Difficulty difficulty = Difficulty.NORMAL;
    private void Start() {
        levels = new List<LevelManager>();
        difficulty = gameObject.GetComponent<SettingsManager>().difficulty;
        switch (difficulty)
        {
            case Difficulty.EASY:
                difficultyScale = 1.0f;
                lootScale = 2.0f;
                break;
            case Difficulty.NORMAL:
                difficultyScale = 2.0f;
                lootScale = 2.0f;
                break;
            case Difficulty.HARD:
                difficultyScale = 5.0f;
                lootScale = 3.0f;
                break;
            case Difficulty.STEPPING_ON_LEGO:
                difficultyScale = 10.0f;
                lootScale = 5.0f;
                break;
        }

        var levelManager = gameObject.AddComponent<LevelManager>();
        levels.Add(levelManager);
    }

    private void Update() {
        
    }

    public WorldMapManager(){
        
    }
}
