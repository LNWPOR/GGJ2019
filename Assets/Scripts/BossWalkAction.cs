using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWalkAction : ActionBased {
    private GameObject boss;
    GameObject bossWalkHelperObject;
    public BossWalkAction(GameObject boss) {
        this.boss = boss;
    }
    public override void Start() {
        bossWalkHelperObject = new GameObject();
        bossWalkHelperObject.AddComponent<BossWalkActionHelper>();
        bossWalkHelperObject.GetComponent<BossWalkActionHelper>().WaitJumpingEnd(OnWalkingEnd, boss);
    }

    private void OnWalkingEnd() {

        InvokeEndEvent();
    }

}

class BossWalkActionHelper : MonoBehaviour {
    private System.Action action;
    private GameObject boss;
    private Vector3 startPoint;
    private bool walkingFinish = false;
    private float moveSpeed = 1f;
    private float moveDuration = 0.5f;
    public void WaitJumpingEnd(System.Action action, GameObject boss) {
        this.action = action;
        this.boss = boss;
        this.startPoint = new Vector3(boss.transform.position.x, boss.transform.position.y, boss.transform.position.z);

        StartCoroutine(CheckWalkEnd());
    }

    private void OnTimerEnd() {
        Destroy(gameObject);
        action?.Invoke();
    }

    public IEnumerator CheckWalkEnd() {
        GameObject planet = GameManager.GetInstance().GetPlanet();
        Vector2 between = boss.transform.position - planet.transform.position;
        Vector2 ninetyDegrees = Vector2.Perpendicular(between);
        Vector3 destination = ninetyDegrees * 2;
        destination.z = startPoint.z;
        if (boss.transform.localScale.x > 0) {
            destination *= -1;
        }
        StartCoroutine(WaitWalkEnd());
        while (!walkingFinish) {
            boss.transform.Translate(destination * Time.deltaTime * moveSpeed);
            yield return null;
        }
    }

    public IEnumerator WaitWalkEnd() {
        yield return new WaitForSeconds(moveDuration);
        walkingFinish = true;
    }
}
