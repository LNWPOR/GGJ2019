using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachableObject : MonoBehaviour
{
    protected Dictionary<GameObject, int> attachers = new Dictionary<GameObject, int>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(this.name);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AddAttacher(GameObject attacher)
    {
        this.attachers.Add(attacher, 0);
    }

    public void RemoveAttacher(GameObject attacher)
    {
        this.attachers.Remove(attacher);
    }
}
