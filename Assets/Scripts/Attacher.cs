using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacher: MonoBehaviour
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

            Vector3 collidedPos = collision.transform.position;
            Vector2 contactPos = collision.contacts[0].point;
            Collider2D c2D = gameObject.GetComponent<Collider2D>();
            c2D.enabled = false;
            Rigidbody2D r2D = gameObject.GetComponent<Rigidbody2D>();
            r2D.simulated = false;
            Vector3 collidedRot = collision.transform.rotation.eulerAngles;
            float collidedAngle = -collidedRot.z;
            Vector3 localPos = new Vector3(
                collidedPos.x - contactPos.x,
                collidedPos.y - contactPos.y
            );
            float contactAngle = Vector3.Angle(Vector3.up, localPos);
            Vector3 localPosRotated = Quaternion.Euler(0, 0, collidedAngle) * localPos;
            transform.localPosition = localPosRotated;
            transform.localRotation = Quaternion.AngleAxis(contactAngle - collidedAngle, new Vector3(0, 0, 1));
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