using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Attacher : MonoBehaviour
{
    protected AttachableObject attachedObject;
    private float attachCooldown;

    // Start is called before the first frame update
    public void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {
        if (attachCooldown > 0f)
        {
            attachCooldown -= Time.deltaTime;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (attachedObject == null && collision.gameObject.GetComponent<AttachableObject>() != null && attachCooldown < 0.001f)
        {
            AttachableObject collidedObject = collision.gameObject.GetComponent<AttachableObject>();
            attachedObject = collidedObject;
            collidedObject.AddAttacher(gameObject);
            transform.SetParent(collidedObject.transform);

            ContactPoint2D contact = collision.contacts[0];
            Vector2 direction = new Vector2(contact.point.x - collision.gameObject.transform.position.x, contact.point.y - collision.gameObject.transform.position.y);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Rigidbody2D r2D = gameObject.GetComponent<Rigidbody2D>();
            r2D.simulated = false;
        }
    }

    protected void Detach()
    {
        if (attachedObject == null) return;
        attachedObject.RemoveAttacher(gameObject);
        attachedObject = null;
        transform.SetParent(null);
        Rigidbody2D r2D = gameObject.GetComponent<Rigidbody2D>();
        r2D.simulated = true;
        attachCooldown = 3f;
    }
}