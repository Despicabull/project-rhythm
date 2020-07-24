using UnityEngine;

public class BlocksMovement : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (!GameplayHandler.isPaused)
        {
            transform.Translate(GameSetting.speedMultiplier * GameSetting.speed * Vector3.down * Time.deltaTime);
        }
    }
}
