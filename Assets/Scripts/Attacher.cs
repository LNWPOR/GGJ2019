using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacher: MonoBehaviour
{
    private AttachableObject attachedObject;
    private float cooldown;

    // Start is called before the first frame update
   public void Start()
    {
        Debug.Log(this.name);
    }

    // Update is called once per frame
   public void Update()
    {
        if (this.cooldown > 0f)
        {
            this.cooldown -= Time.deltaTime;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (this.attachedObject == null && collision.gameObject.GetComponent<AttachableObject>() != null && this.cooldown < 0.001f)
        {
            AttachableObject collidedObject = collision.gameObject.GetComponent<AttachableObject>();
            this.attachedObject = collidedObject;
            collidedObject.AddAttacher(gameObject);
            Rigidbody2D r2D = gameObject.GetComponent<Rigidbody2D>();
            r2D.simulated = false;
            foreach (ContactPoint2D contactPoint2D in collision.contacts)
            {
                transform.SetParent(collidedObject.transform);
                transform.position = new Vector3(
                    contactPoint2D.point.x,
                    contactPoint2D.point.y
                );
           }
        }
    }

    protected void Detach()
    {
        if (this.attachedObject == null) return;
        this.attachedObject.RemoveAttacher(gameObject);
        this.attachedObject = null;
        transform.SetParent(null);
        Rigidbody2D r2D = gameObject.GetComponent<Rigidbody2D>();
        r2D.simulated = true;
        this.cooldown = 3f;
    }
}