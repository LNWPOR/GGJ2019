using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacher: MonoBehaviour
{
    private AttachableObject attachedObject;
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
            Rigidbody2D r2D = gameObject.GetComponent<Rigidbody2D>();
            r2D.simulated = false;
            ContactPoint2D contact = collision.contacts[0];
            transform.SetParent(collidedObject.transform);
            transform.position = new Vector3(
                contact.point.x,
                contact.point.y
            );
            Vector3 v = new Vector3(contact.point.x - collision.transform.position.x, contact.point.y - collision.transform.position.y);
            float angle = Vector3.Angle(Vector3.right, v);
            transform.rotation = Quaternion.Euler(0, 0, angle);
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