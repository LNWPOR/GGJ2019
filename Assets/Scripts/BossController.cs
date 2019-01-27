using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : AttachableObject {
    public List<BossWeakpoint> weakpoints;
    private List<ActionBased> actionList;
    public GameObject popupSpawner;
    private AutoQueue _BossMainQueue = new AutoQueue();
    // Start is called before the first frame update
    void Start() {
        actionList = GenerateActionList();
        UpdateBossRotation();
        //_BossMainQueue.AddAction(new GroupAction(new IAction[] { new BossWalkAction(gameObject) }));
    }

    // Update is called once per frame
    void Update() {
        if (_BossMainQueue.GetQueue().Count.Equals(0)) {
            actionList = GenerateActionList();
            _BossMainQueue.AddAction(actionList[Random.Range(0, actionList.Count)]);
        }
    }

    List<ActionBased> GenerateActionList() {
        return new List<ActionBased>() {
            new BossWalkAction(gameObject),
            new BossJumpAction(gameObject, true),
            new BossJumpAction(gameObject),
            new BossShootPopup(popupSpawner)
            new BossSummonerCactus(Random.insideUnitCircle.normalized * 65, 10f, 180f),
            new BossSummonerBird(Random.insideUnitCircle.normalized * 80),
            new BossSummonerEnemy(Random.insideUnitCircle.normalized * 80, 50)
        };
    }


    void UpdateBossRotation() {
        GameObject planet = GameManager.GetInstance().GetPlanet();
        Vector2 between = transform.position - planet.transform.position;
        Vector2 ninetyDegrees = Vector2.Perpendicular(between) * -1;
        float angle = Mathf.Atan2(ninetyDegrees.y, ninetyDegrees.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void RemoveWeakpoint(BossWeakpoint weakpoint) {
        weakpoints.Remove(weakpoint);
        if (weakpoints.Count == 0) {
            this.Dead();
        }
    }

    private void Dead() {
        GameManager.GetInstance().DoBossDead();
    }
}
