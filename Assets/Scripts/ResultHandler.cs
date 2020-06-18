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

    // Start is called before the first frame update
    void Start()
    {
        // Display results
        scoreText.text = "Score: " + GameResult.currentScore.ToString();
        accuracyText.text = "Accuracy: " + GameResult.accuracy.ToString();
        missText.text = "Miss: " + GameResult.missHit;
        badText.text = "Bad: " + GameResult.badHit;
        goodText.text = "Good: " + GameResult.goodHit;
        greatText.text = "Great: " + GameResult.greatHit;
        excellentText.text = "Excellent: " + GameResult.excellentHit;
    }
}
