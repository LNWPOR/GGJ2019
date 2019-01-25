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
        bool currentFacing;
        if (newHorizontal > 0) {
            currentFacing = true;
        } else {
            currentFacing = false;
        }

        if (!currentFacing.Equals(isFacingRight)) {
            rigidbody.velocity = new Vector2(0f, 0f);
            rigidbody.angularVelocity = 0f;
            isFacingRight = currentFacing;
        }

        turn = newHorizontal;
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
        // Handle dead
    }
}
