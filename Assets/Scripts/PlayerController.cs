using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : AttachableObject, IDamageable {
    const float LIFETIME = 500f;
    [SerializeField]
    public float lifetime;

    [SerializeField]
    private int hitpoint = 100;
    public int Hitpoint { get; set; }
    private bool IsDead;

    private Rigidbody2D rigidbody;
    [SerializeField]
    private float torqueSpeed = 10f;
    [SerializeField]
    private float jumpForce = 800f;
    [SerializeField]
    private Image lifeImage;
    private float turn;
    private bool isFacingRight = true;
    private const int MAX_GROUNED_COLLIDER_CHECK = 20;
    private Collider2D[] colliders = new Collider2D[20];
    private bool isAirSpinning = false;
    [SerializeField]
    private float airSpinningSpeed = 100f;
    //[SerializeField]
    //private float airSpinningDuration = 1.5f;
    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        colliders = new Collider2D[MAX_GROUNED_COLLIDER_CHECK];
        lifetime = LIFETIME;
    }

    void Update() {
        UpdateHorizontalMove();
        UpdateJumpState();
        UpdateLifetime();
        UpdateAirSpinning();
    }

    void FixedUpdate() {
        if (IsGrounded()) {
            rigidbody.AddTorque(torqueSpeed * turn * -1 * GameManager.GetInstance().GetSpeedMultipier());
        }

        if (isAirSpinning) {
            //rigidbody.AddTorque(torqueFastSpeed * turn * -1 * GameManager.GetInstance().GetSpeedMultipier());
            if (isFacingRight) {
                transform.Rotate(0, 0, -airSpinningSpeed);
            } else {
                transform.Rotate(0, 0, airSpinningSpeed);

            }
        }
    }

    void UpdateAirSpinning() {

        //if (Input.GetKeyDown(KeyCode.Q) && !IsGrounded() && !isAirSpinning) {
        //    isAirSpinning = true;
        //    StartCoroutine(AirSpin());
        //}
        if (Input.GetKeyDown(KeyCode.Q) && !IsGrounded()) {
            isAirSpinning = true;
        }
    }

    //IEnumerator AirSpin() {
    //    yield return new WaitForSeconds(airSpinningDuration);
    //    isAirSpinning = false;
    //}

    bool IsGrounded() {
        int groundLayer = 8;
        colliders = new Collider2D[MAX_GROUNED_COLLIDER_CHECK];
        PhysicsScene2D.OverlapCollider(GetComponent<Collider2D>(), colliders, Physics2D.DefaultRaycastLayers);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i]) {
                if (colliders[i].gameObject.layer.Equals(groundLayer)) {
                    isAirSpinning = false;
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
            GameObject planet = GameManager.GetInstance().GetPlanet();
            Vector3 jumpDirection = new Vector2((transform.position.x - planet.transform.position.x), (transform.position.y - planet.transform.position.y));
            rigidbody.AddForce(jumpDirection * jumpForce);
        }
    }

    void UpdateLifetime() {
        if (attachers.Count > 0) return;
        lifetime -= Time.deltaTime;
        float frac = lifetime / LIFETIME;
        lifeImage.fillAmount = frac;
        lifeImage.color = new Color(1f - frac, (frac) + 0f * (1f - frac), (frac) + 0f * (1f - frac));
        if (lifetime <= 0f) {
            Dead();
        }
    }

    public int Hit(int damage) {
        // int remainingHP = this.hitpoint - damage;
        // this.hitpoint = remainingHP;
        // if (remainingHP <= 0)
        // {
        if (attachers.Count > 0) {
            float minLifetime = float.MaxValue;
            Attacher oldestAttacher = null;
            foreach (var item in attachers) {
                GameObject obj = item.Key;
                Attacher attacher = obj.GetComponent<Attacher>();
                if (attacher.lifetime < minLifetime) {
                    minLifetime = attacher.lifetime;
                    oldestAttacher = attacher;
                }
            }
            if (oldestAttacher != null) {
                oldestAttacher.Detach();
            }
            return 1;
        }
        this.Dead();
        return 0;
        // }
        // return remainingHP;
    }

    public void Dead() {
        if (!IsDead) {
            IsDead = true;
            GameManager.GetInstance().DoPlayerDead();
        }
    }
}