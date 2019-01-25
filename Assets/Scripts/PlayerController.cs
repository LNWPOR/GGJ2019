using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable {
    private int hitpoint = 100;
    public int Hitpoint { get; set; }

    private Rigidbody2D rigidbody;
    [SerializeField]
    private float torqueSpeed = 10f;
    private float turn;
    private bool isFacingRight = true;

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update() {
        updateHorizontalMove();
    }

    void FixedUpdate() {
        rigidbody.AddTorque(torqueSpeed * turn * -1);
    }

    void updateHorizontalMove() {
        float newHorizontal = Input.GetAxis("Horizontal");

        if (newHorizontal > 0 && !isFacingRight) {
            changeMoveDirection();
        } else if (newHorizontal < 0 && isFacingRight) {
            changeMoveDirection();
        }

        turn = newHorizontal;
    }

    void changeMoveDirection() {
        rigidbody.velocity = new Vector2(0f, 0f);
        rigidbody.angularVelocity = 0f;
        isFacingRight = !isFacingRight;
    }

    void updateJumpState() {
        if (Input.GetButtonDown("Jump") && isGrouded()) {
            rigidbody.AddForce(Vector2.up * 600f);
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

    bool isGrouded() {
        return rigidbody.velocity.y == 0;
    }
}
