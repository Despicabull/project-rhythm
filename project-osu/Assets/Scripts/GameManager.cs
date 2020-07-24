using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public AudioClip audioClip;
    public Image background;
    public Sprite[] images;
    public int backgroundIndex = 0;
    public string folderPath;
    public string txtPath;

    void Awake()
    {
        Application.targetFrameRate = -1;
        // Creates folder which holds all beatmaps if specified folder doesn't exist
        if (!Directory.Exists(GameSetting.beatmapPath))
        {
            Directory.CreateDirectory(GameSetting.beatmapPath);
        }
        if (!Directory.Exists(GameSetting.configPath))
        {
            Directory.CreateDirectory(GameSetting.configPath);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Changes background
        backgroundIndex = Random.Range(0, images.Length);
        background.sprite = images[backgroundIndex];
        DontDestroyOnLoad(this);
    }

    public void SaveSettings()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(GameSetting.configPath + "config.cfg");
        PlayerConfig playerConfig = new PlayerConfig
        {
            speed = GameSetting.speed,
            volume = GameSetting.volume,
            difficultyIndex = GameSetting.difficultyIndex
        };
        bf.Serialize(file, playerConfig);
        file.Close();
    }

    public void LoadSettings()
    {
        if (File.Exists(GameSetting.configPath + "config.cfg")) // If config.dat exists then load otherwise creates config.dat
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(GameSetting.configPath + "config.cfg", FileMode.Open);
            PlayerConfig playerConfig = (PlayerConfig)bf.Deserialize(file);
            file.Close();
            GameSetting.speed = playerConfig.speed;
            GameSetting.volume = playerConfig.volume;
            GameSetting.difficultyIndex = playerConfig.difficultyIndex;
        }
        else
        {
            SaveSettings();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
