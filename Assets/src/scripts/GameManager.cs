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
    public CameraManager cameraManager;

    private void Start() {
        settingsManager = gameObject.AddComponent<SettingsManager>();
        uiManager = gameObject.AddComponent<UIManager>();
        cameraManager = gameObject.GetComponent<CameraManager>();
    }

    private void Update() {
        
    }

    public void StartGame()
    {
        uiManager.ToggleMenu();
        cameraManager.ToggleCameras();
        worldMapManager = gameObject.AddComponent<WorldMapManager>();
    }
}

