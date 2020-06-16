using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioClip audioClip;

    // Start is called before the first frame update
    void Start()
    {
        // Creates folder which holds all beatmaps
        if (!Directory.Exists(GameSettings.beatmapPath))
        {
            Directory.CreateDirectory(GameSettings.beatmapPath);
        }
        if (!Directory.Exists(GameSettings.configPath))
        {
            Directory.CreateDirectory(GameSettings.configPath);
        }
        DontDestroyOnLoad(this);
    }

    public void SaveSettings()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(GameSettings.configPath + "config.cfg");
        PlayerConfig playerConfig = new PlayerConfig
        {
            speed = GameSettings.speed,
            volume = GameSettings.volume
        };
        bf.Serialize(file, playerConfig);
        file.Close();
    }

    public void LoadSettings()
    {
        if (File.Exists(GameSettings.configPath + "config.cfg")) // If config.dat exists then load otherwise creates config.dat
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(GameSettings.configPath + "config.cfg", FileMode.Open);
            PlayerConfig playerConfig = (PlayerConfig)bf.Deserialize(file);
            file.Close();
            GameSettings.speed = playerConfig.speed;
            GameSettings.volume = playerConfig.volume;
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
