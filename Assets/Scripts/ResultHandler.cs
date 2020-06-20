using TMPro;
using UnityEngine;

public class ResultHandler : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI accuracyText;
    public TextMeshProUGUI excellentText;
    public TextMeshProUGUI greatText;
    public TextMeshProUGUI goodText;
    public TextMeshProUGUI badText;
    public TextMeshProUGUI missText;
    public TextMeshProUGUI rankingText;

    // Start is called before the first frame update
    void Start()
    {
        // Displays results
        scoreText.text = "Score: " + GameResult.currentScore.ToString("D7");
        accuracyText.text = "Accuracy: " + CalculateAccuracy().ToString("0.00") + "%";
        missText.text = "Miss: " + GameResult.missHit;
        badText.text = "Bad: " + GameResult.badHit;
        goodText.text = "Good: " + GameResult.goodHit;
        greatText.text = "Great: " + GameResult.greatHit;
        excellentText.text = "Excellent: " + GameResult.excellentHit;
        rankingText.text = CalculateRanking();
        // Saves results
        GameManager gameManager = FindObjectOfType<GameManager>();
        GameResult.SaveResult(gameManager.folderPath);
    }

    float CalculateAccuracy()
    {
        return GameResult.accuracy / GameResult.totalBlocks * 100f;
    }

    string CalculateRanking()
    {
        float accuracy = CalculateAccuracy();
        if (accuracy > 95f) return "S";
        else if (accuracy > 90f) return "A";
        else if (accuracy > 80f) return "B";
        else if (accuracy > 70f) return "C";
        else return "D";
    }
}
