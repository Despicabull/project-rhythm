using UnityEngine;

public class GameSetting
{
    // 0 Easy, 1 Normal, 2 Hard, 3 Insane
    public static int difficultyIndex = 1;
    public static float speed = 15f;
    public const int scorePerBlock = 100;
    public const float speedMultiplier = 60f;
    public static float scoreMultiplier = 1f;
    public static float volume = 0.5f;
    public static string beatmapPath = Application.dataPath + "/Beatmaps/";
    public static string configPath = Application.dataPath + "/Config/";

    public static void IncreaseSpeed()
    {
        speed++;
        speed = Mathf.Clamp(speed, 5f, 30f);
    }

    public static void DecreaseSpeed()
    {
        speed--;
        speed = Mathf.Clamp(speed, 5f, 30f);
    }
}
