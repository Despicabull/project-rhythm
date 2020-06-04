using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public List<GameObject> blocks = new List<GameObject>();
    public KeyPanel keyPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckBlocks();
    }

    void CheckBlocks()
    {
        if (blocks.Count > 0)
        {
            if (blocks[0].transform.position.y < keyPanel.end.transform.position.y)
            {
                GameObject temp = blocks[0];
                blocks.RemoveAt(0);
                Destroy(temp);
            }
        }
    }
}
