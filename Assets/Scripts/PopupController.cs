using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : MonoBehaviour {
    private Vector3 shootingDir;
    public Vector3 ShootingDir { get => shootingDir; set => shootingDir = value; }
    private Rigidbody2D rigidbody;
    public Rigidbody2D Rigidbody { get => rigidbody; set => rigidbody = value; }
    public float moveSpeed = 50f;
    public int damage = 3;

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 10f);
    }

    void Update() {
        //rigidbody.AddForce(shootingDir * moveSpeed);
        transform.Translate(shootingDir * moveSpeed * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D collider) {
        IDamageable damageable = collider.gameObject.GetComponent(typeof(IDamageable)) as IDamageable;
        if (
            damageable != null &&
            (collider.gameObject.tag == "Player")
        ) {
            damageable.Hit(damage);
            Destroy(gameObject);
        }
    }
}
