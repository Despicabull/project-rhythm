﻿using System.Collections.Generic;
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
            float keyThresholdHeight = gameplayHandler.keyThreshold.GetComponent<RectTransform>().rect.height;
            if (blocks[0].transform.position.y < gameplayHandler.keyThreshold.transform.position.y - (keyThresholdHeight / 2)) // Miss
            {
                gameplayHandler.DisplayCombo(Combo.Miss);
                ResetState();
            }
        }
    }

    public void KeyInput()
    {
        if (blocks.Count > 0)
        {
            float keyThresholdHeight = gameplayHandler.keyThreshold.GetComponent<RectTransform>().rect.height;
            if (blocks[0].transform.position.y <= gameplayHandler.keyThreshold.transform.position.y + (keyThresholdHeight / 2)) // Indicates that the block is within the threshold
            {
                if (blocks[0].transform.position.y >= gameplayHandler.keyThreshold.transform.position.y + (5 * keyThresholdHeight / 8) ||
                    blocks[0].transform.position.y <= gameplayHandler.keyThreshold.transform.position.y - (5 * keyThresholdHeight / 8))
                {
                    gameplayHandler.DisplayCombo(Combo.Bad);
                }
                else if (blocks[0].transform.position.y >= gameplayHandler.keyThreshold.transform.position.y + (3 * keyThresholdHeight / 8) ||
                    blocks[0].transform.position.y <= gameplayHandler.keyThreshold.transform.position.y - (3 * keyThresholdHeight / 8))
                {
                    gameplayHandler.DisplayCombo(Combo.Good);
                }
                else if (blocks[0].transform.position.y >= gameplayHandler.keyThreshold.transform.position.y + (1 * keyThresholdHeight / 8) ||
                    blocks[0].transform.position.y <= gameplayHandler.keyThreshold.transform.position.y - (1 * keyThresholdHeight / 8))
                {
                    gameplayHandler.DisplayCombo(Combo.Great);
                }
                else if (blocks[0].transform.position.y >= gameplayHandler.keyThreshold.transform.position.y + (-1 * keyThresholdHeight / 8) ||
                    blocks[0].transform.position.y <= gameplayHandler.keyThreshold.transform.position.y - (-1 * keyThresholdHeight / 8))
                {
                    gameplayHandler.DisplayCombo(Combo.Excellent);
                }
                ResetState();
            }
        }
    }

    private void ResetState()
    {
        blocks[0].SetActive(false);
        blocks.RemoveAt(0);
    }
}
