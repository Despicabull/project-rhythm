using UnityEngine;

public class GameSettings
{
    public static int scorePerBlock = 100;
    public static float speed = 10f;
    public static float scoreMultiplier = 1f;
    public static float volume = 0.5f;
    public static string beatmapPath = Application.dataPath + "/Beatmaps/";
    public static string configPath = Application.dataPath + "/Config/";

    public static void IncreaseSpeed()
    {
        if (speed < 40f)
        {
            speed++;
        }
    }

    public static void DecreaseSpeed()
    {
        if (speed > 5f)
        {
            speed--;
        }
    }
}
