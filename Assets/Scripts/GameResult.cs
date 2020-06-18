public class GameResult
{
    public static int missHit = 0;
    public static int badHit = 0;
    public static int goodHit = 0;
    public static int greatHit = 0;
    public static int excellentHit = 0;
    public static int currentScore = 0;
    public static float accuracy = 0;
    public static int blocksHit = 0;
    public static int totalBlocks = 0;

    public static void Reset()
    {
        missHit = 0;
        badHit = 0;
        goodHit = 0;
        greatHit = 0;
        excellentHit = 0;
        currentScore = 0;
        accuracy = 0;
        blocksHit = 0;
        totalBlocks = 0;
    }
}
