using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KeyPanel : MonoBehaviour
{
    public Transform block;
    public Key[] keys;
    private float speed;
    

    // Start is called before the first frame update
    void Start()
    {
        speed = GameSettings.speed;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnBlock();
    }

    void SpawnBlock()
    {
        int random = Random.Range(0, keys.Length);
        Instantiate(block, keys[random].transform);
    }
}
