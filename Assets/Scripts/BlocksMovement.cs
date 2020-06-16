using UnityEngine;

public class BlocksMovement : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0, GameSettings.speed, 0);
        // transform.position -= new Vector3(0, GameSettings.speed * 10f, 0);
    }
}
