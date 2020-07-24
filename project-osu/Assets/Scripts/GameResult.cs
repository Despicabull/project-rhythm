using System.IO;

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

    public static void SaveResult(string folderName)
    {
        string filename = folderName + "result.dat";
        if (File.Exists(filename))
        {
            StreamReader reader = new StreamReader(filename);
            // a[1] = savedScore
            string[] a = reader.ReadLine().Split();
            int savedScore = int.Parse(a[1]);
            reader.Close();
            StreamWriter writer = new StreamWriter(filename);
            writer.WriteLine("Score: {0}", CompareResult(currentScore, savedScore).ToString("D7"));
            writer.Close();
        }
        else
        {
            StreamWriter writer = new StreamWriter(filename);
            writer.WriteLine("Score: {0}", currentScore.ToString("D7"));
            writer.Close();
        }
    }

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

    static int CompareResult(int x, int y)
    {
        if (x > y) return x;
        else return y;
    }
}
