using TMPro;
using UnityEngine;

public enum Combo
{
    Miss,
    Bad,
    Good,
    Great,
    Excellent,
}

public class GameplayHandler : MonoBehaviour
{
    public int missHit = 0;
    public int badHit = 0;
    public int goodHit = 0;
    public int greatHit = 0;
    public int excellentHit = 0;
    public GameObject blockSpawn;
    public GameObject keyThreshold;
    public GameObject gameplayPanel;
    public KeyPanel keyPanel;
    public ResultHandler resultHandler;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI accuracyText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI hitText;
    private AudioProcessor processor;
    private AudioSource audioSource;
    private int currentScore = 0;
    private float accuracy = 0;
    private int blocksHit = 0;
    private int totalBlocks = 0;
    private readonly int[,] patterns = new int[6, 2] { { 0, 1 }, { 0, 2 }, { 0, 3 }, { 1, 2 }, { 1, 3 }, { 2, 3 } };

    // Start is called before the first frame update
    void Start()
    {
        processor = GetComponent<AudioProcessor>();
        processor.onBeat.AddListener(OnBeat);

        // Loads clip from the GameManager
        GameManager gameManager = FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = gameManager.audioClip;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            gameplayPanel.SetActive(false);
            resultHandler.CalculateResult();
        }
    }

    public void DisplayCombo(Combo combo)
    {
        float accuracyPoint = 0f;
        switch (combo) 
        {
            case Combo.Miss: // Resets the block hit value
                missHit++;
                comboText.text = "Miss";
                ResetHit();
                break;
            case Combo.Bad:
                badHit++;
                comboText.text = "Bad";
                accuracyPoint = 0.5f;
                accuracy += 0.25f;
                break;
            case Combo.Good:
                goodHit++;
                comboText.text = "Good";
                accuracyPoint = 1f;
                accuracy += 0.5f;
                break;
            case Combo.Great:
                greatHit++;
                comboText.text = "Great";
                accuracyPoint = 2f;
                accuracy += 0.75f;
                break;
            case Combo.Excellent:
                excellentHit++;
                comboText.text = "Excellent";
                accuracyPoint = 3f;
                accuracy += 1f;
                break;
        };
        totalBlocks++;
        SetAccuracy();
        SetScore(Mathf.RoundToInt(GameSettings.scorePerBlock * GameSettings.scoreMultiplier * accuracyPoint));
    }

    void OnBeat()
    {
        SpawnBlock(patterns);
    }

    void SpawnBlock(int[,] patterns)
    {
        int len = patterns.GetLength(1);
        int r = Random.Range(0, 5);
        for (int i = 0; i < len; i++)
        {
            int x = patterns[r, i];
            GameObject block = ObjectPooler.instance.GetObject();
            if (block == null) { return; }
            block.SetActive(true);
            Vector3 position = new Vector3(keyPanel.keys[x].transform.position.x, blockSpawn.transform.position.y, keyPanel.keys[x].transform.position.z);
            block.transform.position = position;
            keyPanel.keys[x].blocks.Add(block);
        }
    }

    void ResetHit()
    {
        blocksHit = 0;
        hitText.text = "";
    }

    void IncreaseHit()
    {
        blocksHit++;
        hitText.text = blocksHit.ToString();
    }

    void SetScore(int value)
    {
        currentScore += value;
        scoreText.text = currentScore.ToString("D7");
        IncreaseHit();
    }

    void SetAccuracy()
    {
        // accuracy = (accuracy / totalBlocks * 100f)
        accuracyText.text = (accuracy / totalBlocks * 100f).ToString("0.00") + "%";
    }
}
