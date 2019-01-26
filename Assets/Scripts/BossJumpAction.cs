using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossJumpAction : ActionBased {

    private GameObject boss;
    GameObject bossJumpHelperObject;
    bool isTurn = false;
    public BossJumpAction(GameObject boss, bool isTurn = false) {
        this.boss = boss;
        this.isTurn = isTurn;
    }
    public override void Start() {
        bossJumpHelperObject = new GameObject();
        bossJumpHelperObject.AddComponent<BossJumpActionHelper>();
        bossJumpHelperObject.GetComponent<BossJumpActionHelper>().WaitJumpingEnd(OnJumpingEnd, boss, isTurn);
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
    //private float jumpDistance = 20f;
    private float moveDuration = 1f;
    private bool isTurn = false;
    public void WaitJumpingEnd(System.Action action, GameObject boss, bool isTurn) {
        this.action = action;
        this.boss = boss;
        this.startPoint = new Vector3(boss.transform.position.x, boss.transform.position.y, boss.transform.position.z);
        StartCoroutine(CheckMoveUpEnd());
    }

    private void OnTimerEnd() {
        action?.Invoke();
        Destroy(gameObject);
    }

    public IEnumerator CheckMoveUpEnd() {
        GameObject planet = GameManager.GetInstance().GetPlanet();
        Vector3 destination = new Vector3((boss.transform.position.x - planet.transform.position.x) * 2, (boss.transform.position.y - planet.transform.position.y) * 2, startPoint.z);
        StartCoroutine(WaitMoveUpEnd());
        while (!moveUpFinish) {
            boss.transform.position = Vector3.Lerp(boss.transform.position, destination, jumpSpeed);
            //if (Vector3.Distance(boss.transform.position, destination) < jumpDistance) {
            //    moveUpFinish = true;
            //}
            yield return null;
        }
        if (isTurn) {
            TurnDirection();
        }
        StartCoroutine(CheckMoveDownEnd());
    }

    void TurnDirection() {
        Vector2 localScale = new Vector2(boss.transform.localScale.x, boss.transform.localScale.y);
        localScale.x *= -1;
        boss.transform.localScale = localScale;
    }

    public IEnumerator CheckMoveDownEnd() {
        StartCoroutine(WaitMoveDownEnd());
        while (!moveDownFinish) {
            boss.transform.position = Vector3.Lerp(boss.transform.position, startPoint, jumpSpeed);
            //if (Vector3.Distance(boss.transform.position, startPoint) < 1) {
            //    moveDownFinish = true;
            //}
            yield return null;
        }
        OnTimerEnd();
    }

    public IEnumerator WaitMoveUpEnd() {
        yield return new WaitForSeconds(moveDuration);
        moveUpFinish = true;
    }

    public IEnumerator WaitMoveDownEnd() {
        yield return new WaitForSeconds(moveDuration * 3);
        moveDownFinish = true;
    }
}