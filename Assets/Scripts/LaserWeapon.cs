using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeapon : Attacher
{
    const float COOLDOWN = 1.2f;

    public GameObject guidanceBeam;
    public GameObject laserBeamPrefab;
    private LaserBeam currentLaserBeam;

    [SerializeField]
    private int damage;
    private float cooldown;

    // Start is called before the first frame update
    void Start()
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
        RenderGuidanceBeam();
    }

    void RenderGuidanceBeam()
    {
        Color spriteColor = guidanceBeam.GetComponent<SpriteRenderer>().color;
        if (attachedObject == null)
        {
            spriteColor.a = 0;
            guidanceBeam.GetComponent<SpriteRenderer>().color = spriteColor;
            return;
        }
        spriteColor.a = (COOLDOWN - cooldown) / COOLDOWN;
        guidanceBeam.GetComponent<SpriteRenderer>().color = spriteColor;
    }

    private void Shoot()
    {
        if (attachedObject == null) return;
        GameObject spawned = Instantiate(laserBeamPrefab, guidanceBeam.transform.position, transform.rotation);
        currentLaserBeam = spawned.GetComponent<LaserBeam>();
        currentLaserBeam.damage = damage;
        currentLaserBeam.owner = this;
        currentLaserBeam.laserType = BULLET_TYPE.PLAYER;
    }

}
