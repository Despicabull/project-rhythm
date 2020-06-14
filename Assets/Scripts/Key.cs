using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public GameplayHandler gameplayHandler;
    public List<GameObject> blocks = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if (blocks.Count > 0)
        {
            if (blocks[0].GetComponent<BlocksMovement>().canBePressed)
            {
                float keyThresholdHeight = gameplayHandler.keyThreshold.GetComponent<RectTransform>().rect.height;
                if (blocks[0].transform.position.y <= gameplayHandler.keyThreshold.transform.position.y - (keyThresholdHeight / 2))
                {
                    gameplayHandler.DisplayCombo(Combo.Miss);
                    ResetState();
                }
            }
        }
    }

    public void KeyInput()
    {
        if (blocks.Count > 0)
        {
            if (blocks[0].GetComponent<BlocksMovement>().canBePressed)
            {
                float accuracyPoint = 0f;
                float keyThresholdHeight = gameplayHandler.keyThreshold.GetComponent<RectTransform>().rect.height;
                if (blocks[0].transform.position.y >= gameplayHandler.keyThreshold.transform.position.y + (keyThresholdHeight / 4)) // Bad
                {
                    gameplayHandler.DisplayCombo(Combo.Bad);
                    accuracyPoint = 0.5f;
                }
                else if (blocks[0].transform.position.y >= gameplayHandler.keyThreshold.transform.position.y) // Good
                {
                    gameplayHandler.DisplayCombo(Combo.Good);
                    accuracyPoint = 1f;
                }
                else if (blocks[0].transform.position.y >= gameplayHandler.keyThreshold.transform.position.y - (keyThresholdHeight / 4)) // Great
                {
                    gameplayHandler.DisplayCombo(Combo.Great);
                    accuracyPoint = 2f;
                }
                else if (blocks[0].transform.position.y >= gameplayHandler.keyThreshold.transform.position.y - (keyThresholdHeight / 2)) // Excellent
                {
                    gameplayHandler.DisplayCombo(Combo.Excellent);
                    accuracyPoint = 3f;
                }
                gameplayHandler.IncreaseScore(Mathf.RoundToInt(GameSettings.scorePerBlock * GameSettings.scoreMultiplier * accuracyPoint));
                ResetState();
            }
        }
    }

    private void ResetState()
    {
        blocks[0].GetComponent<BlocksMovement>().canBePressed = false;
        blocks[0].SetActive(false);
        blocks.RemoveAt(0);
    }
}
