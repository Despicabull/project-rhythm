using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KeyPanel : MonoBehaviour
{
    public GameObject blockPrefab;
    public GameObject start, end;
    public Key[] keys;
    private float spawnRate;
    private bool isSpawning = false;
    

    // Start is called before the first frame update
    void Start()
    {
        spawnRate = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnBlock(spawnRate));
        }
    }

    IEnumerator SpawnBlock(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        int random = Random.Range(0, keys.Length);
        GameObject block = Instantiate(blockPrefab, keys[random].gameObject.transform);
        keys[random].blocks.Add(block);
        Vector3 position = new Vector3(keys[random].transform.position.x, start.transform.position.y, keys[random].transform.position.z);
        block.transform.position = position;
        isSpawning = false;
    }
}
