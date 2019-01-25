using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacher : MonoBehaviour
{
    private AttachableObject attachedObject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!attachedObject && collision.gameObject.GetComponent<AttachableObject>() != null)
        {
            AttachableObject collidedObject = collision.gameObject.GetComponent<AttachableObject>();
            attachedObject = collidedObject;
            collidedObject.AddAttacher(gameObject);
        }
    }
}