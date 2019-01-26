using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    public static GameObject CreateObject(string path)
    {
        return Instantiate(ResourceLoadManager.LoadGameObjectFromResources(path));
    }
}
