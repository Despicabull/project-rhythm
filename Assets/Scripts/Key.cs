using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public List<GameObject> blocks = new List<GameObject>();
    public GameplayHandler gameplayHandler;

    // Update is called once per frame
    void Update()
    {
        CheckBlocks();
    }

    public void CheckBlocks()
    {
        if (blocks.Count > 0)
        {
            if (blocks[0].GetComponent<BlocksMovement>().missed) // Miss
            {
                gameplayHandler.missHit++;
                GameObject temp = blocks[0];
                blocks.RemoveAt(0);
                Destroy(temp);
            }
        }
    }

    public void KeyInput()
    {
        if (blocks.Count > 0)
        {
            if (blocks[0].GetComponent<BlocksMovement>().canBePressed)
            {
                float accuracyPoint;
                float keyThresholdHeight = gameplayHandler.keyThreshold.GetComponent<RectTransform>().rect.height;
                if (blocks[0].transform.position.y >= gameplayHandler.keyThreshold.transform.position.y + (keyThresholdHeight / 4)) // Bad
                {
                    gameplayHandler.badHit++;
                    accuracyPoint = 0.5f;
                }
                else if (blocks[0].transform.position.y >= gameplayHandler.keyThreshold.transform.position.y) // Good
                {
                    gameplayHandler.goodHit++;
                    accuracyPoint = 1f;
                }
                else if (blocks[0].transform.position.y >= gameplayHandler.keyThreshold.transform.position.y - (keyThresholdHeight / 4)) // Great
                {
                    gameplayHandler.greatHit++;
                    accuracyPoint = 2f;
                }
                else // Excellent
                {
                    gameplayHandler.excellentHit++;
                    accuracyPoint = 3f;
                }
                gameplayHandler.IncreaseScore(Mathf.RoundToInt(GameSettings.scorePerBlock * GameSettings.scoreMultiplier * accuracyPoint));
                GameObject temp = blocks[0];
                blocks.RemoveAt(0);
                Destroy(temp);
            }
        }
    }
}
