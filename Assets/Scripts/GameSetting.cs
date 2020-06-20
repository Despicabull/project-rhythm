using UnityEngine;

public class GameSetting
{
    public static int scorePerBlock = 100;
    public static float speed = 10f;
    public static float scoreMultiplier = 1f;
    public static float volume = 0.5f;
    public static string beatmapPath = Application.dataPath + "/Beatmaps/";
    public static string configPath = Application.dataPath + "/Config/";

    public static void IncreaseSpeed()
    {
        speed++;
        speed = Mathf.Clamp(speed, 5f, 20f);
    }

    public static void DecreaseSpeed()
    {
        speed--;
        speed = Mathf.Clamp(speed, 5f, 20f);
    }
}
