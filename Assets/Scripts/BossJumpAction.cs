using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossJumpAction : ActionBased {

    private GameObject boss;
    GameObject bossJumpHelperObject;
    public BossJumpAction(GameObject boss) {
        this.boss = boss;
    }
    public override void Start() {
        bossJumpHelperObject = new GameObject();
        bossJumpHelperObject.AddComponent<BossJumpActionHelper>();
        bossJumpHelperObject.GetComponent<BossJumpActionHelper>().WaitJumpingEnd(OnJumpingEnd, boss);
    }
    private void OnJumpingEnd() {

        InvokeEndEvent();
    }
}

class BossJumpActionHelper : MonoBehaviour {
    private System.Action action;
    private GameObject boss;
    private bool moveUpFinish = false;
    private bool moveDownFinish = false;
    private float jumpSpeed = 0.02f;
    private Vector3 startPoint;
    private float jumpDistance = 20f;
    public void WaitJumpingEnd(System.Action action, GameObject boss) {
        this.action = action;
        this.boss = boss;
        this.startPoint = new Vector3(boss.transform.position.x, boss.transform.position.y, boss.transform.position.z);
        StartCoroutine(CheckMoveUpEnd());
    }

    private void OnTimerEnd() {
        Destroy(gameObject);
        action?.Invoke();
    }

    public IEnumerator CheckMoveUpEnd() {
        GameObject planet = GameManager.GetInstance().GetPlanet();
        Vector3 destination = new Vector3((boss.transform.position.x - planet.transform.position.x) * 2, (boss.transform.position.y - planet.transform.position.y) * 2, startPoint.z);
        Debug.Log(Vector2.Distance(boss.transform.position, destination));
        while (!moveUpFinish) {
            boss.transform.position = Vector3.Lerp(boss.transform.position, destination, jumpSpeed);
            if (Vector3.Distance(boss.transform.position, destination) < jumpDistance) {
                moveUpFinish = true;
            }
            yield return null;
        }
        TurnDirection();
        StartCoroutine(CheckMoveDownEnd());
    }

    void TurnDirection() {
        Vector2 localScale = new Vector2(boss.transform.localScale.x, boss.transform.localScale.y);
        localScale.x *= -1;
        boss.transform.localScale = localScale;
    }

    public IEnumerator CheckMoveDownEnd() {
        while (!moveDownFinish) {
            boss.transform.position = Vector3.Lerp(boss.transform.position, startPoint, jumpSpeed);
            if (Vector3.Distance(boss.transform.position, startPoint) < 1) {
                moveDownFinish = true;
            }
            yield return null;
        }
        OnTimerEnd();
    }
}
