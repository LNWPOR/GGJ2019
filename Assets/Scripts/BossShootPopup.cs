using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShootPopup : ActionBased {

    private GameObject popupSpawner;
    private GameObject bossMouse;
    private Vector3 shootingDir;
    private List<GameObject> popupPrefabs;
    GameObject bossShootPopupHelperObject;

    public BossShootPopup(GameObject popupSpawner) {
        this.popupSpawner = popupSpawner;
        PopupSpawner popupSpawnerScript = popupSpawner.GetComponent<PopupSpawner>();
        this.bossMouse = popupSpawnerScript.bossMouse;
        this.popupPrefabs = popupSpawnerScript.popupPrefabs;
    }

    public override void Start() {
        shootingDir = popupSpawner.transform.localPosition - bossMouse.transform.localPosition;
        shootingDir.z = 0f;
        GameObject shootingPopup = popupPrefabs[Random.Range(0, popupPrefabs.Count)];

        bossShootPopupHelperObject = new GameObject("bossShootPopupHelperObject");
        bossShootPopupHelperObject.AddComponent<BossShootPopupHelper>();
        bossShootPopupHelperObject.GetComponent<BossShootPopupHelper>().WaitShootingEnd(OnShootingEnd, shootingDir, shootingPopup, popupSpawner);
    }

    private void OnShootingEnd() {
        InvokeEndEvent();
    }

}

class BossShootPopupHelper : MonoBehaviour {
    private System.Action action;
    private Vector3 shootingDir;
    private GameObject shootingPopup;
    private GameObject popupSpawner;
    private float shootingDuration = 1.5f;

    public void WaitShootingEnd(System.Action action, Vector3 shootingDir, GameObject shootingPopup, GameObject popupSpawner) {
        this.action = action;
        this.shootingDir = shootingDir;
        this.shootingPopup = shootingPopup;
        this.popupSpawner = popupSpawner;
        StartCoroutine(CheckShootingEnd());
    }

    private void OnTimerEnd() {
        action?.Invoke();
        Destroy(gameObject, 3f);
    }

    public IEnumerator CheckShootingEnd() {
        Vector3 spawnPoint = new Vector3(popupSpawner.transform.position.x, popupSpawner.transform.position.y, 0);
        GameObject shootedPopup = Instantiate(shootingPopup, spawnPoint, popupSpawner.transform.rotation) as GameObject;
        PopupController popupController = shootedPopup.GetComponent<PopupController>();
        popupController.ShootingDir = shootingDir;
        yield return new WaitForSeconds(shootingDuration);
        popupController.Rigidbody.isKinematic = false;
        OnTimerEnd();
    }
}
