using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWeapon : Attacher
{
    const float COOLDOWN = 1f;

    [SerializeField]
    private int damage;
    [SerializeField]
    private float speed;
    private float cooldown;
    [SerializeField]
    private GameObject bulletPrefab;
    private List<GameObject> bullets = new List<GameObject>();

    // Start is called before the first frame update
    private void Start()
    {
        base.Start();
        cooldown = COOLDOWN;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (cooldown > 0.001f)
        {
            cooldown -= Time.deltaTime;
        }
        else
        {
            Shoot();
            cooldown = COOLDOWN;
        }
    }

    private void Shoot()
    {
        if (attachedObject == null) return;
        GameObject spawnedBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Bullet bullet = spawnedBullet.GetComponent<Bullet>();
        bullet.speed = speed;
        bullet.damage = damage;
        bullet.owner = this;
        bullet.bulletType = BULLET_TYPE.PLAYER;
        bullets.Add(spawnedBullet);
    }

    public void OnDestroyBullet(GameObject bullet)
    {
        bullets.Remove(bullet);
    }

}
