﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager {
    private static GameManager _Instance;
    private GameObject planet;
    private SceneController scene;
    private int DeadCount;

    public GameObject GetPlanet() {
        if (!planet) {
            planet = GameObject.Find("Planet"); ;
        }
        return planet;
    }

    public int GetDeadCount()
    {
        return DeadCount;
    }

    public SceneController GetScene()
    {
        if (scene == null) scene = GameObject.Find("SceneController").GetComponent<SceneController>();
        return scene;
    }

    public static GameManager GetInstance() {
        if (_Instance == null) _Instance = new GameManager();
        return _Instance;
    }

    public GameManager() {
        planet = GameObject.Find("Planet");
    }
    private STATE _GameState;
    private float _SpeedMultipier = 1f;

    public enum STATE {
        GamePlay,
        BossFight
    }

    public void DoPlayerDead() {
        Debug.Log("<color=red>DoPlayerDead.</color>");
        ++DeadCount;
        GetScene().DoPlayerDead();
    }

    public void DoRestartScene()
    {
        SceneManager.LoadScene("TestCircleWorld");
    }

    public void DoBossDead() {
        Debug.Log("<color=green>DoBossDead.</color>");
        //_SpeedMultipier += 0.5f;

        // Speed up playtime
        Time.timeScale += 0.5f;

        Debug.Log("Gen new shit");
        // Increase planet size and chaos
        PlanetMeshGenerator planetMeshGen = this.GetPlanet().GetComponent<PlanetMeshGenerator>();
        planetMeshGen.noiseFrequency += 1.3f;
        planetMeshGen.terrainFluctuationMagnitude += 2;
        planetMeshGen.octavePersistence += 0.1f;
        planetMeshGen.startGenPlanetMeshProcess();


        SceneManager.LoadScene("TestCircleWorld");
    }

    public void DoBossFight() {
        _GameState = STATE.BossFight;
        Debug.Log("<color=orange>Now game is state : </color>" + _GameState);
    }

    public float GetSpeedMultipier() {
        return _SpeedMultipier;
    }
}
