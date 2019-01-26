using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int hitpoint = 100;
    public int Hitpoint { get; set; }
    [SerializeField]
    private float Velocity = 20f;
    [SerializeField]
    private int damage = 100;
    [SerializeField]
    private float targetTime = 3;
    [SerializeField]
    private float RotationSpeed = 10;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        targetTime -= Time.deltaTime;
        Vector3 moveDirection = GameObject.FindGameObjectWithTag("Player").transform.position - gameObject.transform.position;
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            if (Mathf.Abs(angle) > RotationSpeed) angle = angle / Mathf.Abs(angle) * RotationSpeed;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        if (targetTime <= 0)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(moveDirection.x * Velocity, moveDirection.y * Velocity));
        }
    }

    public void Dead()
    {
        Destroy(gameObject);
    }

    public int Hit(int damage)
    {
        hitpoint -= damage;
        if (hitpoint <= 0) Dead();
        return hitpoint;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent(typeof(IDamageable)) as IDamageable;
        if (
            damageable != null &&
            (collision.gameObject.tag == "Player")
            )
        {
            damageable.Hit(this.damage);
        }
    }
}
