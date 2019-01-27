using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour {
    private List<ActionBased> actionList;
    private AutoQueue monsterSpawnQueue = new AutoQueue();
    [SerializeField]
    private int totalMonster = 5;
    private int count = 0;
    void Start() {
        totalMonster += GameManager.GetInstance().AdditionMonster;
        actionList = GenerateActionList();
    }

    void Update() {
        if (monsterSpawnQueue.GetQueue().Count.Equals(0) && count < totalMonster) {
            count++;
            actionList = GenerateActionList();
            monsterSpawnQueue.AddAction(actionList[Random.Range(0, actionList.Count)]);
        }
    }

    List<ActionBased> GenerateActionList() {
        return new List<ActionBased>() {
            new BossSummonerCactus(Random.insideUnitCircle.normalized * 65, 10f, 180f),
            new BossSummonerBird(Random.insideUnitCircle.normalized * 80),
            new BossSummonerEnemy(Random.insideUnitCircle.normalized * 80, 50)
        };
    }
}
