using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoadManager : MonoBehaviour
{
    private static Dictionary<string, GameObject> _GameObjectList = new Dictionary<string, GameObject>();
    private static Dictionary<string, Sprite> _SpriteList = new Dictionary<string, Sprite>();

    public static Sprite LoadSpriteFromResources(string path)
    {
        if (!_SpriteList.ContainsKey(path))
        {
            _SpriteList.Add(path, Resources.Load<Sprite>(path));
        }

        return _SpriteList[path];
    }

    public static GameObject LoadGameObjectFromResources(string path)
    {
        if (!_GameObjectList.ContainsKey(path))
        {
            _GameObjectList.Add(path, Resources.Load<GameObject>(path));
        }

        return _GameObjectList[path];
    }
}
