using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable {
    private int hitpoint = 100;
    public int Hitpoint { get; set; }

    private Rigidbody2D rigidbody;
    [SerializeField]
    private float torqueSpeed = 10f;
    [SerializeField]
    private float jumpForce = 600f;
    private float turn;
    private bool isFacingRight = true;
    private bool isGrounded = true;
    private const int MAX_GROUNED_COLLIDER_CHECK = 20;
    private Collider2D[] colliders = new Collider2D[20];
    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        colliders = new Collider2D[MAX_GROUNED_COLLIDER_CHECK];
    }

    void Update() {
        UpdateHorizontalMove();
        UpdateJumpState();
    }

    void FixedUpdate() {

        if (IsGrounded()) {
            rigidbody.AddTorque(torqueSpeed * turn * -1 * GameManager.GetInstance().GetSpeedMultipier());
        }
    }

    bool IsGrounded() {
        int groundLayer = 8;
        colliders = new Collider2D[MAX_GROUNED_COLLIDER_CHECK];
        PhysicsScene2D.OverlapCollider(GetComponent<Collider2D>(), colliders, Physics2D.DefaultRaycastLayers);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i]) {
                if (colliders[i].gameObject.layer.Equals(groundLayer)) {
                    return true;
                }
            }
        }
        return false;
    }

    void UpdateHorizontalMove() {
        if (IsGrounded()) {
            float newHorizontal = Input.GetAxis("Horizontal");

            if (newHorizontal > 0 && !isFacingRight) {
                ChangeMoveDirection();
            } else if (newHorizontal < 0 && isFacingRight) {
                ChangeMoveDirection();
            }

            turn = newHorizontal;
        }
    }

    void ChangeMoveDirection() {
        rigidbody.velocity = new Vector2(0f, 0f);
        rigidbody.angularVelocity = 0f;
        isFacingRight = !isFacingRight;
    }

    void UpdateJumpState() {
        if (Input.GetButtonDown("Jump") && IsGrounded()) {
            rigidbody.AddForce(Vector2.up * jumpForce);
        }
    }
    public int Hit(int damage) {
        int remainingHP = this.hitpoint - damage;
        this.hitpoint = remainingHP;
        if (remainingHP <= 0) {
            this.Dead();
        }
        return remainingHP;
    }

    public void Dead() {
        // TODO: Handle dead
    }
}
