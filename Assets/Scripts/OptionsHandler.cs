using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Difficulty
{
    Easy,
    Normal,
    Hard,
    Insane,
}

public class OptionsHandler : MonoBehaviour
{
    public GameObject beatmapSpeedPanel;
    public Slider volumeSlider;
    public TextMeshProUGUI difficultyText;
    private float beatmapSpeedPanelDisplayTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.LoadSettings();
        difficultyText.text = ((Difficulty)GameSetting.difficultyIndex).ToString();
        AudioListener.volume = GameSetting.volume;
        volumeSlider.value = GameSetting.volume;
    }

    // Update is called once per frame
    void Update()
    {
        if (beatmapSpeedPanelDisplayTime > 0)
        {
            beatmapSpeedPanelDisplayTime -= Time.deltaTime;
        }
        if (beatmapSpeedPanelDisplayTime <= 0 && beatmapSpeedPanel.activeInHierarchy)
        {
            beatmapSpeedPanel.SetActive(false);
            beatmapSpeedPanelDisplayTime = 0f;
        }
        KeyInput();
    }

    public void ModifyVolume(float volume)
    {
        AudioListener.volume = volume;
        GameSetting.volume = volume;
    }

    public void IncreaseDifficulty()
    {
        GameSetting.difficultyIndex++;
        GameSetting.difficultyIndex = Mathf.Clamp(GameSetting.difficultyIndex, 0, 3);
        difficultyText.text = ((Difficulty)GameSetting.difficultyIndex).ToString();
    }

    public void DecreaseDifficulty()
    {
        GameSetting.difficultyIndex--;
        GameSetting.difficultyIndex = Mathf.Clamp(GameSetting.difficultyIndex, 0, 3);
        difficultyText.text = ((Difficulty)GameSetting.difficultyIndex).ToString();
    }

    void KeyInput()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            GameSetting.IncreaseSpeed();
            ShowBeatmapSpeedPanel();
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            GameSetting.DecreaseSpeed();
            ShowBeatmapSpeedPanel();
        }
    }

    void ShowBeatmapSpeedPanel()
    {
        if (!beatmapSpeedPanel.activeInHierarchy)
        {
            beatmapSpeedPanel.SetActive(true);
        }
        beatmapSpeedPanel.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = string.Format("beatmap speed is set to {0} (fixed)", GameSetting.speed);
        beatmapSpeedPanelDisplayTime = 2f;
        // Saves the game speed
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.SaveSettings();
    }
}
