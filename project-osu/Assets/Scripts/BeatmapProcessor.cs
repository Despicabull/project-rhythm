using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class BeatmapProcessor : MonoBehaviour
{
    public OnBeatEvent onBeat;
    private AudioSource audioSource;
    private StreamReader reader;
    private float beatTimer = 0;
    private bool readerClosed = false;
    private bool isBeat = false;
    private bool gameStarted = false;
    private float gameStartTimer = 2.5f;
    private float timer = 0f;
    private float spawnRate;

    // Start is called before the first frame update
    void Start()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        reader = new StreamReader(gameManager.txtPath);
        audioSource = GetComponent<AudioSource>();
        spawnRate = 1.2f / (GameSetting.difficultyIndex + 1);
    }

    // Update is called once per frame
    void Update()
    {
        // Parse as long as the audioSource is still playing
        if (!GameplayHandler.isPaused)
        {
            if (gameStartTimer <= 0) gameStarted = true;
            if (!gameStarted) gameStartTimer -= Time.deltaTime;
            else 
            {
                if (audioSource.isPlaying)
                {
                    ReadBeatmap();
                }
            }
        }
    }

    void ReadBeatmap()
    {
        if (!readerClosed)
        {
            if (!isBeat) Parse();
            if (timer > 0) timer -= Time.deltaTime;
            if (timer <= 0 && isBeat)
            {
                onBeat.Invoke();
                timer = 0f;
                isBeat = false;
            }
        }
        else
        {
            beatTimer += Time.deltaTime;
            if (beatTimer > spawnRate)
            {
                onBeat.Invoke();
                beatTimer = 0;
            }
        }
    }

    void Parse()
    {
        if (reader.Peek() > 0)
        {
            string line = reader.ReadLine();
            switch (line)
            {
                case "0.1s":
                    timer = 0.1f;
                    break;
                case "0.2s":
                    timer = 0.2f;
                    break;
                case "0.3s":
                    timer = 0.3f;
                    break;
                case "0.4s":
                    timer = 0.4f;
                    break;
                case "0.5s":
                    timer = 0.5f;
                    break;
                default:
                    timer = 1f;
                    break;
            };
            isBeat = true;
        }
        else
        {
            readerClosed = true;
            reader.Close();
        }
    }
}

[Serializable]
public class OnBeatEvent : UnityEvent
{

}
