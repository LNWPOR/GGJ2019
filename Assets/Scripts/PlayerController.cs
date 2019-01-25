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
    private float distToGround;
    public bool isGrounded = true;
    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        distToGround = GetComponent<Collider2D>().bounds.extents.y;
    }

    void Update() {
        updateHorizontalMove();
        updateJumpState();
    }

    void FixedUpdate() {
        if (isGrounded) {
            rigidbody.AddTorque(torqueSpeed * turn * -1);
        }
    }

    void updateHorizontalMove() {
        if (isGrounded) {
            float newHorizontal = Input.GetAxis("Horizontal");

            if (newHorizontal > 0 && !isFacingRight) {
                changeMoveDirection();
            } else if (newHorizontal < 0 && isFacingRight) {
                changeMoveDirection();
            }

            turn = newHorizontal;
        }
    }

    void changeMoveDirection() {
        rigidbody.velocity = new Vector2(0f, 0f);
        rigidbody.angularVelocity = 0f;
        isFacingRight = !isFacingRight;
    }

    void updateJumpState() {
        if (Input.GetButtonDown("Jump") && isGrounded) {
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

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            isGrounded = false;
        }
    }
}
