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
    private float popupDurationTime = 3f;

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        Destroy(gameObject, popupDurationTime);
    }

    void Update() {
        //rigidbody.AddForce(shootingDir * moveSpeed);
        transform.Translate(shootingDir * moveSpeed * Time.deltaTime);
        UpdatePopupRotation();
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

    void UpdatePopupRotation() {
        GameObject planet = GameManager.GetInstance().GetPlanet();
        Vector2 between = transform.position - planet.transform.position;
        Vector2 ninetyDegrees = Vector2.Perpendicular(between) * -1;
        float angle = Mathf.Atan2(ninetyDegrees.y, ninetyDegrees.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
