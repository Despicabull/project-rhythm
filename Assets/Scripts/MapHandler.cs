using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    private string path;    
    private List<Map> maps = new List<Map>();

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        path = Application.dataPath + "\\Maps\\";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        GetMap(path);
        ListMap();
    }

    void GetMap(string path)
    {
        string[] files = Directory.GetFiles(path, "*.txt");
        foreach (string file in files)
        {
            Map map = ConvertFileToMap(file);
            maps.Add(map);
        }
    }

    Map ConvertFileToMap(string filename)
    {
        StreamReader reader = new StreamReader(filename);
        string mapName = Path.GetFileNameWithoutExtension(filename);
        float mapSpeed = float.Parse(reader.ReadLine());
        int mapBlocks = 0;
        while (reader.Peek() > -1) // Reads every line
        {
            reader.Read();
            mapBlocks++;
        }
        Map map = new Map(mapName, mapSpeed, mapBlocks);
        return map;
    }

    void ListMap()
    {
        for (int i = 0; i < maps.Count; i++)
        {
            Debug.Log(maps[i].mapBlocks);
        }
    }

    void LoadMap(Map map)
    {
        GameSettings.currentMap = map;
    }
}
