using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWalkAction : ActionBased {
    private GameObject boss;
    GameObject bossWalkHelperObject;
    GameObject rotationParentObject;
    public BossWalkAction(GameObject boss) {
        this.boss = boss;
    }
    public override void Start() {
        bossWalkHelperObject = new GameObject("bossWalkHelperObject");
        bossWalkHelperObject.AddComponent<BossWalkActionHelper>();
        BossWalkActionHelper bookWalkActionHelper = bossWalkHelperObject.GetComponent<BossWalkActionHelper>();
        if (!rotationParentObject) {
            rotationParentObject = bookWalkActionHelper.GenerateRotationHelperObject();
            boss.transform.parent = rotationParentObject.transform;
        }
        bookWalkActionHelper.WaitJumpingEnd(OnWalkingEnd, rotationParentObject, boss);
    }

    private void OnWalkingEnd() {
        InvokeEndEvent();
    }

}

class BossWalkActionHelper : MonoBehaviour {
    private System.Action action;
    private GameObject boss;
    private GameObject rotationParentObject;
    private Vector3 startPoint;
    private bool walkingFinish = false;
    private float moveSpeed = 20f;
    private float moveDuration = 1f;
    public void WaitJumpingEnd(System.Action action, GameObject rotationParentObject, GameObject boss) {
        this.action = action;
        this.boss = boss;
        this.rotationParentObject = rotationParentObject;
        //this.startPoint = new Vector3(boss.transform.position.x, boss.transform.position.y, boss.transform.position.z);

        StartCoroutine(CheckWalkEnd());
    }


    private void OnTimerEnd() {
        action?.Invoke();
        boss.transform.parent = null;
        Destroy(rotationParentObject, 3f);
        Destroy(gameObject, 3f);
    }

    public GameObject GenerateRotationHelperObject() {
        return Instantiate(new GameObject("rotationParentObject"), new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
    }

    public IEnumerator CheckWalkEnd() {
        //GameObject planet = GameManager.GetInstance().GetPlanet();
        //Vector2 between = boss.transform.position - planet.transform.position;
        //Vector2 ninetyDegrees = Vector2.Perpendicular(between);
        //Vector3 destination = ninetyDegrees * 2;
        //destination.z = startPoint.z;
        if (boss.transform.localScale.x > 0) {
            //    destination *= -1;
            moveSpeed *= -1;
        }


        StartCoroutine(WaitWalkEnd());
        while (!walkingFinish) {
            rotationParentObject.transform.Rotate(0, 0, moveSpeed * Time.deltaTime, Space.Self);
            //boss.transform.Translate(destination * Time.deltaTime * moveSpeed);
            //boss.transform.position += destination * Time.deltaTime * moveSpeed;
            yield return null;
        }
        OnTimerEnd();
    }

    public IEnumerator WaitWalkEnd() {
        yield return new WaitForSeconds(moveDuration);
        walkingFinish = true;
    }
}
