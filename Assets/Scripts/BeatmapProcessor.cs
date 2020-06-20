using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class BeatmapProcessor : MonoBehaviour
{
    public OnBeatEvent onBeat;
    private AudioSource audioSource;
    private StreamReader reader;
    private int currentBeat = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        reader = new StreamReader(gameManager.txtPath);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Parse as long as the audioSource is still playing
        if (audioSource.isPlaying) Parse();
        if (currentBeat > 30)
        {
            onBeat.Invoke();
            currentBeat = 0;
        }
    }

    void Parse()
    {
        currentBeat++;
        /*
        if (reader.Peek() > 0)
        {
            reader.Read();
            currentBeat++;
        }
        */
    }
}

[Serializable]
public class OnBeatEvent : UnityEvent
{

}
