public class Map
{
    public string mapName;
    public float mapSpeed;
    public int mapBlocks;
    private float maxMapSpeed = 120f;
    private float minMapSpeed = 15f;

    public Map(string mapName, float mapSpeed, int mapBlocks)
    {
        this.mapName = mapName;
        this.mapBlocks = mapBlocks;
        SetMapSpeed(mapSpeed);
    }

    public void SetMapSpeed(float value)
    {
        if (value > maxMapSpeed){ value = maxMapSpeed; }
        if (value < minMapSpeed){ value = minMapSpeed; }
        mapSpeed = value;
    }
}
