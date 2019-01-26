using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public LaserWeapon owner;
    public BULLET_TYPE laserType;

    public int damage;
    [SerializeField]
    private SpriteRenderer sprite;
    [SerializeField]
    private Collider2D laserCollider;
    const float DURATION = 0.3f;

    [SerializeField]
    private bool active = false;
    private float duration = DURATION;
    private Dictionary<GameObject, bool> hitCache = new Dictionary<GameObject, bool>();

    // Start is called before the first frame update
    void Start()
    {
        hitCache.Clear();
        RenderBeam(1);
        duration = DURATION;
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (duration > 0.001f)
        {
            duration -= Time.deltaTime;
            RenderBeam(duration / DURATION);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void RenderBeam(float alpha)
    {
        Color newColor = sprite.color;
        newColor.a = alpha;
        sprite.color = newColor;
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(collider.gameObject.name);
        HandleCollider(collider);
    }

    void HandleCollider(Collider2D collider)
    {
        Debug.Log(collider.gameObject.name);
        IDamageable damageable = collider.gameObject.GetComponent(typeof(IDamageable)) as IDamageable;
        if (
            active &&
            !hitCache.ContainsKey(collider.gameObject) &&
            damageable != null &&
            ((collider.gameObject.tag == "Player" && laserType == BULLET_TYPE.ENEMY) ||
            (collider.gameObject.tag == "Enemy" && laserType == BULLET_TYPE.PLAYER))
        )
        {
            Debug.Log("Laser Hit!: " + collider.gameObject.name);
            hitCache.Add(collider.gameObject, true);
            damageable.Hit(damage);
        }
    }
}
