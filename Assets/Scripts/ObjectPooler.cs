using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler instance;
    public GameObject poolObject;
    private readonly int poolSize = 50;

    private readonly List<GameObject> poolObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(poolObject);
            obj.transform.SetParent(transform, false);
            obj.SetActive(false);
            poolObjects.Add(obj);
        }
    }

    public GameObject GetObject()
    {
        for (int i = 0; i < poolObjects.Count; i++)
        {
            if (!poolObjects[i].activeInHierarchy)
            {
                return poolObjects[i];
            }
        }
        return null;
    }
}
