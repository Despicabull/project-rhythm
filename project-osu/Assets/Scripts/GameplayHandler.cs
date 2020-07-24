using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject blockSpawn;
    public GameObject keyThreshold;
    public GameObject gameplayPanel;
    public GameObject pauseHandler;
    public Image outerTimerImage;
    public Image background;
    public KeyPanel keyPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI accuracyText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI hitText;
    public static bool isPaused = false;
    private BeatmapProcessor processor;
    private AudioSource audioSource;
    private float gameFinishTimer = 5f;
    private float hitComboDisplayTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        outerTimerImage.fillAmount = 0f;
        GameManager gameManager = FindObjectOfType<GameManager>();
        background.sprite = gameManager.images[gameManager.backgroundIndex];
        processor = GetComponent<BeatmapProcessor>();
        processor.onBeat.AddListener(OnBeat);
        // Resets the game result
        GameResult.Reset();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = gameManager.audioClip;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            if (!audioSource.isPlaying && gameFinishTimer > 0) gameFinishTimer -= Time.deltaTime;
            if (gameFinishTimer <= 0) FinishGame();

            if (hitComboDisplayTime > 0) hitComboDisplayTime -= Time.deltaTime;

            if (hitComboDisplayTime <= 0 && (comboText.text != "" || hitText.text != ""))
            {
                comboText.text = "";
                hitText.text = "";
                hitComboDisplayTime = 0f;
            }
            // Updates timer
            if (outerTimerImage.fillAmount < 1f)
            {
                outerTimerImage.fillAmount = Mathf.Clamp(audioSource.time / audioSource.clip.length, 0f, 1f);
            }
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
                SetScore(Mathf.RoundToInt(GameSetting.scorePerBlock * GameSetting.scoreMultiplier * accuracyPoint));
                break;
            case Combo.Good:
                GameResult.goodHit++;
                comboText.text = "Good";
                accuracyPoint = 1f;
                GameResult.accuracy += 0.5f;
                SetScore(Mathf.RoundToInt(GameSetting.scorePerBlock * GameSetting.scoreMultiplier * accuracyPoint));
                break;
            case Combo.Great:
                GameResult.greatHit++;
                comboText.text = "Great";
                accuracyPoint = 2f;
                GameResult.accuracy += 0.75f;
                SetScore(Mathf.RoundToInt(GameSetting.scorePerBlock * GameSetting.scoreMultiplier * accuracyPoint));
                break;
            case Combo.Excellent:
                GameResult.excellentHit++;
                comboText.text = "Excellent";
                accuracyPoint = 3f;
                GameResult.accuracy += 1f;
                SetScore(Mathf.RoundToInt(GameSetting.scorePerBlock * GameSetting.scoreMultiplier * accuracyPoint));
                break;
        };
        hitComboDisplayTime = 4f; // Resets the timer which the hit and combo text is being displayed
        GameResult.totalBlocks++;
        SetAccuracy();
    }

    public void Retry()
    {
        isPaused = false;
        // Loads main scene
        SceneHandler sceneHandler = FindObjectOfType<SceneHandler>();
        sceneHandler.LoadLevel(1);
    }

    public void Back()
    {
        isPaused = false;
        // Loads to menu scene
        SceneHandler sceneHandler = FindObjectOfType<SceneHandler>();
        sceneHandler.LoadLevel(0);
    }

    void OnBeat()
    {
        int r = Random.Range(1, 3);
        SpawnBlock(r);
    }

    void SpawnBlock(int count)
    {
        int temp = -1;
        for (int i = 0; i < count; i++)
        {
            int r = Random.Range(0, 4);
            while (temp == r)
            {
                r = Random.Range(0, 4);
            }
            GameObject block = ObjectPooler.instance.GetObject();
            if (block == null) return;
            block.SetActive(true);
            Vector3 position = new Vector3(keyPanel.keys[r].transform.position.x, blockSpawn.transform.position.y, keyPanel.keys[r].transform.position.z);
            block.transform.position = position;
            keyPanel.keys[r].blocks.Add(block);
            temp = r;
        }
    }

    void FinishGame()
    {
        // Loads result scene
        SceneHandler sceneHandler = FindObjectOfType<SceneHandler>();
        sceneHandler.LoadLevel(2);
    }

    void Pause()
    {
        audioSource.Pause();
        isPaused = true;
        pauseHandler.SetActive(true);
    }

    void Resume()
    {
        audioSource.UnPause();
        isPaused = false;
        pauseHandler.SetActive(false);
    }

    void KeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused) Pause();
            else Resume();
        }
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
