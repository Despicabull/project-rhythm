using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    public string mapName;
    public float mapSpeed;

    public Map(string mapName, float mapSpeed)
    {
        this.mapName = mapName;
        this.mapSpeed = mapSpeed;
    }
}
