using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barreir : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.parent.GetComponent<Rigidbody2D>().simulated);
        gameObject.GetComponent<Rigidbody2D>().simulated = !transform.parent.GetComponent<Rigidbody2D>().simulated;
        gameObject.transform.localPosition = new Vector3(2, 0, 0);
        gameObject.transform.localRotation = new Quaternion(0, 0, 90, 90);
    }
}
