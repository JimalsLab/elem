using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int score;
    public WorldMapManager worldMapManager;
    public UIManager uiManager;
    public SettingsManager settingsManager;
    private void Start() {
        settingsManager = gameObject.AddComponent<SettingsManager>();
        uiManager = gameObject.AddComponent<UIManager>();
    }

    private void Update() {
        
    }

    public void StartGame()
    {
        worldMapManager = gameObject.AddComponent<WorldMapManager>();
    }
}

