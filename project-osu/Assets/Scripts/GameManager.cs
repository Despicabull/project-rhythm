using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioClip audioClip;
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
        DontDestroyOnLoad(this);
    }

    public void SaveSettings()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(GameSetting.configPath + "config.ini");
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
        if (File.Exists(GameSetting.configPath + "config.ini")) // If config.ini exists then load otherwise creates config.dat
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(GameSetting.configPath + "config.ini", FileMode.Open);
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
