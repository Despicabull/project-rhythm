using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsHandler : MonoBehaviour
{
    public GameObject beatmapSpeedPanel;
    public Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.LoadSettings();
        AudioListener.volume = GameSettings.volume;
        volumeSlider.value = AudioListener.volume;
    }

    // Update is called once per frame
    void Update()
    {
        KeyInput();
    }

    public void KeyInput()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            GameSettings.IncreaseSpeed();
            ShowBeatmapSpeedPanel();
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            GameSettings.DecreaseSpeed();
            ShowBeatmapSpeedPanel();
        }
    }

    public void ModifyVolume(float volume)
    {
        AudioListener.volume = volume;
        GameSettings.volume = volume;
    }

    void ShowBeatmapSpeedPanel()
    {
        if (!beatmapSpeedPanel.activeInHierarchy)
        {
            beatmapSpeedPanel.SetActive(true);
        }
        beatmapSpeedPanel.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = string.Format("beatmap speed is set to {0} (fixed)", GameSettings.speed);
    }
}
