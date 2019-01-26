using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BULLET_TYPE
{
    PLAYER,
    ENEMY
}

public class Bullet : MonoBehaviour
{
    static float DISTANCE = 100f;

    public int damage;
    public float speed;
    public MainWeapon owner;
    public BULLET_TYPE bulletType;

    private float travelTime;
    private Vector3 from;
    private Vector3 to;
    private float time = 0;

    public Bullet(int damage, float speed, BULLET_TYPE bulletType)
    {
        this.damage = damage;
        this.speed = speed;
        this.bulletType = bulletType;
    }

    // Start is called before the first frame update
    void Start()
    {
        from = transform.position;
        float angle = transform.rotation.eulerAngles.z;
        var vForce = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
        to = from + (vForce.normalized * DISTANCE);
        Debug.Log("from: " + from);
        Debug.Log("to: " + to);
        Debug.Log("travelTime: " + travelTime);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("DISTANCE: " + DISTANCE);
        Debug.Log("speed: " + speed);
        travelTime = DISTANCE / speed;
        time += Time.deltaTime;
        transform.position = Vector3.Lerp(from, to, time / travelTime);
        if (time >= travelTime)
        {
            DestroyBullet();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent(typeof(IDamageable)) as IDamageable;
        if (
            damageable != null &&
            (collision.gameObject.tag == "Player" && bulletType == BULLET_TYPE.ENEMY) ||
            (collision.gameObject.tag == "Enemy" && bulletType == BULLET_TYPE.PLAYER)
        ) {
            damageable.Hit(damage);
        }
    }

    private void DestroyBullet()
    {
        owner.OnDestroyBullet(gameObject);
        Destroy(gameObject);
    }
}
