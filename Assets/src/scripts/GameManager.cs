using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public WorldMapManager worldMapManager;
    public int score;
    public SettingsManager settingsManager;
    private void Start() {
        settingsManager = new SettingsManager();
        worldMapManager = new WorldMapManager(); // a terme ça ira dans une fction appelée quand on lance le jeu
    }

    private void Update() {
        
    }
}

