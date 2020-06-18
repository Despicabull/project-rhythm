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
    private bool gamePaused = false;
    public GameObject blockSpawn;
    public GameObject keyThreshold;
    public GameObject gameplayPanel;
    public GameObject pauseHandler;
    public KeyPanel keyPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI accuracyText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI hitText;
    private AudioProcessor processor;
    private AudioSource audioSource;

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
            // Loads result scene
            SceneHandler sceneHandler = FindObjectOfType<SceneHandler>();
            sceneHandler.LoadLevel(2);
        }
        KeyInput();
    }

    public void DisplayCombo(Combo combo)
    {
        float accuracyPoint;
        switch (combo) 
        {
            case Combo.Miss: // Resets the block hit value
                GameResult.missHit++;
                comboText.text = "Miss";
                ResetHit();
                break;
            case Combo.Bad:
                GameResult.badHit++;
                comboText.text = "Bad";
                accuracyPoint = 0.5f;
                GameResult.accuracy += 0.25f;
                SetScore(Mathf.RoundToInt(GameSettings.scorePerBlock * GameSettings.scoreMultiplier * accuracyPoint));
                break;
            case Combo.Good:
                GameResult.goodHit++;
                comboText.text = "Good";
                accuracyPoint = 1f;
                GameResult.accuracy += 0.5f;
                SetScore(Mathf.RoundToInt(GameSettings.scorePerBlock * GameSettings.scoreMultiplier * accuracyPoint));
                break;
            case Combo.Great:
                GameResult.greatHit++;
                comboText.text = "Great";
                accuracyPoint = 2f;
                GameResult.accuracy += 0.75f;
                SetScore(Mathf.RoundToInt(GameSettings.scorePerBlock * GameSettings.scoreMultiplier * accuracyPoint));
                break;
            case Combo.Excellent:
                GameResult.excellentHit++;
                comboText.text = "Excellent";
                accuracyPoint = 3f;
                GameResult.accuracy += 1f;
                SetScore(Mathf.RoundToInt(GameSettings.scorePerBlock * GameSettings.scoreMultiplier * accuracyPoint));
                break;
        };
        GameResult.totalBlocks++;
        SetAccuracy();
    }

    void OnBeat()
    {
        SpawnBlock();
    }

    void SpawnBlock()
    {
        int r = Random.Range(0, 3);
        GameObject block = ObjectPooler.instance.GetObject();
        if (block == null) { return; }
        block.SetActive(true);
        Vector3 position = new Vector3(keyPanel.keys[r].transform.position.x, blockSpawn.transform.position.y, keyPanel.keys[r].transform.position.z);
        block.transform.position = position;
        keyPanel.keys[r].blocks.Add(block);
    }

    void KeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gamePaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    void Pause()
    {
        Time.timeScale = 0f;
        gamePaused = true;
    }

    void Resume()
    {
        Time.timeScale = 1f;
        gamePaused = false;
    }

    void ResetHit()
    {
        GameResult.blocksHit = 0;
        hitText.text = "";
    }

    void IncreaseHit()
    {
        GameResult.blocksHit++;
        hitText.text = GameResult.blocksHit.ToString();
    }

    void SetScore(int value)
    {
        GameResult.currentScore += value;
        scoreText.text = GameResult.currentScore.ToString("D7");
        IncreaseHit();
    }

    void SetAccuracy()
    {
        // accuracy = (accuracy / totalBlocks * 100f)
        accuracyText.text = (GameResult.accuracy / GameResult.totalBlocks * 100f).ToString("0.00") + "%";
    }
}
