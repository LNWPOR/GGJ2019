using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachableObject : MonoBehaviour
{
    private Dictionary<GameObject, int> attachers = new Dictionary<GameObject, int>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AddAttacher(GameObject attacher)
    {
        attachers.Add(attacher, 0);
    }

    public void RemoveAttacher(GameObject attacher)
    {
        attachers.Remove(attacher);
    }
}
