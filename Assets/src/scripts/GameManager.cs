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
        settingsManager = gameObject.AddComponent<SettingsManager>();
    }

    private void Update() {
        
    }

    public void StartGame()
    {
        worldMapManager = gameObject.AddComponent<WorldMapManager>();
    }
}

