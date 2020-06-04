using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings
{
    public static float speed = 5f;
    public static Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>()
    {
        {"D", KeyCode.D},
        {"F", KeyCode.F},
        {"J", KeyCode.J},
        {"K", KeyCode.K},
    };
}
