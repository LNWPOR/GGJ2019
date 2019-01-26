using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossJumpAction : ActionBased {

    public GameObject boss;
    GameObject bossJumpHelperObject;
    public BossJumpAction(GameObject boss) {
        this.boss = boss;
    }
    public override void Start() {
        boss.GetComponent<Animator>().SetTrigger("isJumping");
        bossJumpHelperObject = new GameObject();
        bossJumpHelperObject.AddComponent<BossJumpActionHelper>();
        bossJumpHelperObject.GetComponent<BossJumpActionHelper>().WaitJumpingEnd(OnJumpingEnd, boss);
    }
    private void OnJumpingEnd() {
        Vector2 localScale = new Vector2(boss.transform.localScale.x, boss.transform.localScale.y);
        localScale.x *= -1;
        boss.transform.localScale = localScale;
        InvokeEndEvent();
    }
}

class BossJumpActionHelper : MonoBehaviour {
    private System.Action action;
    private GameObject boss;
    public void WaitJumpingEnd(System.Action action, GameObject boss) {
        this.action = action;
        this.boss = boss;
        StartCoroutine(CheckAnimationEnd());
    }

    private void OnTimerEnd() {
        Destroy(gameObject);
        action?.Invoke();
    }

    public IEnumerator CheckAnimationEnd() {
        Animator bossAnim = boss.GetComponent<Animator>();
        while (bossAnim.GetCurrentAnimatorStateInfo(0).length > bossAnim.GetCurrentAnimatorStateInfo(0).normalizedTime) {
            yield return null;
        }
        OnTimerEnd();
    }
}
