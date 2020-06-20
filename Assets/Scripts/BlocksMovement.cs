using UnityEngine;

public class BlocksMovement : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (!GameplayHandler.isPaused)
        {
            transform.Translate(new Vector3(0f, -1 * GameSetting.speed, 0f));
        }
    }
}
