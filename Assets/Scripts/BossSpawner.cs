using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour {
    public GameObject bossPrefab;
    void Update() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length.Equals(0)) {
            Instantiate(bossPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
