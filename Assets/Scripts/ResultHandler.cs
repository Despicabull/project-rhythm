using TMPro;
using UnityEngine;

public class ResultHandler : MonoBehaviour
{
    public GameplayHandler gameplayHandler;
    public GameObject resultPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI excellentText;
    public TextMeshProUGUI greatText;
    public TextMeshProUGUI goodText;
    public TextMeshProUGUI badText;
    public TextMeshProUGUI missText;

    public void CalculateResult()
    {
        resultPanel.SetActive(true);
        scoreText.text = "Score: " + gameplayHandler.scoreText.text;
        missText.text = "Miss: " + gameplayHandler.missHit;
        badText.text = "Bad: " + gameplayHandler.badHit;
        goodText.text = "Good: " + gameplayHandler.goodHit;
        greatText.text = "Great: " + gameplayHandler.greatHit;
        excellentText.text = "Excellent: " + gameplayHandler.excellentHit;
    }
}
