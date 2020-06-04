using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    private string path;
    private List<Map> maps = new List<Map>();

    // Start is called before the first frame update
    void Start()
    {
        path = Application.dataPath + "\\Maps\\";
        GetMap(path);
        ListMap();
    }

    // Update is called once per frame
    void Update()
    {

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
        Map map = new Map(mapName, 5f);
        return map;
    }

    void ListMap()
    {
        for (int i = 0; i < maps.Count; i++)
        {
            Debug.Log(maps[i].mapName);
        }
    }
}
