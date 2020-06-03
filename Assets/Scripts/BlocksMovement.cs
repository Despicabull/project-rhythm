using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksMovement : MonoBehaviour
{
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = GameSettings.speed;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y - speed, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, position, 0.1f);
    }
}
