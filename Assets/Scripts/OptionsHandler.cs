using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsHandler : MonoBehaviour
{
    public GameObject beatmapSpeedPanel;
    public Slider volumeSlider;
    private float beatmapSpeedPanelDisplayTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.LoadSettings();
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

    void KeyInput()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            GameSetting.IncreaseSpeed();
            ShowBeatmapSpeedPanel();
        }
        else if (Input.GetKeyDown(KeyCode.F3))
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
