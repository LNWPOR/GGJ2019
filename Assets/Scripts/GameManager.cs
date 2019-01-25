using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _Instance;
    public static GameManager GetInstance()
    {
        if (_Instance == null) _Instance = new GameManager();
        return _Instance;
    }
    private STATE _GameState;
    private float _SpeedMultipier = 1f;

    public enum STATE
    {
        GamePlay,
        BossFight
    }

    public void DoPlayerDead()
    {
        Debug.Log("<color=red>DoPlayerDead.</color>");
        SceneManager.LoadScene("SampleScene");
    }

    public void DoBossDead()
    {
        Debug.Log("<color=green>DoBossDead.</color>");
        _SpeedMultipier += 0.5f;
        SceneManager.LoadScene("SampleScene");
    }

    public void DoBossFight()
    {
        _GameState = STATE.BossFight;
        Debug.Log("<color=orange>Now game is state : </color>" + _GameState);
    }

    public float GetSpeedMultipier()
    {
        return _SpeedMultipier;
    }
}
