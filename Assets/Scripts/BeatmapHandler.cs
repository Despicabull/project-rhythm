using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

struct Beatmap
{
    public string title;

    // Constructor
    public Beatmap(string title)
    {
        this.title = title;
    }
}

public class BeatmapHandler : MonoBehaviour
{
    public GameObject beatmapPrefab;
    public GameObject content;
    private readonly List<Beatmap> beatmaps = new List<Beatmap>();

    // Start is called before the first frame update
    void Start()
    {
        AddBeatmapsToList();
        AddListToView();
    }

    void AddListToView()
    {
        for (int i = 0; i < beatmaps.Count; i++)
        {
            GameObject beatmap = Instantiate(beatmapPrefab, content.transform);
            TextMeshProUGUI beatmapTitle = beatmap.transform.Find("Text_Title (TMP)").GetComponent<TextMeshProUGUI>();
            beatmapTitle.text = beatmaps[i].title;
            beatmap.GetComponent<Button>().onClick.AddListener(() => StartLoad(beatmapTitle.text));
        }
    }

    void AddBeatmapsToList()
    {
        // Adds every file with an extension of wav to beatmaps
        DirectoryInfo info = new DirectoryInfo(GameSettings.beatmapPath);
        FileInfo[] fileInfos = info.GetFiles("*.wav");
        foreach (FileInfo fileInfo in fileInfos)
        {
            string filename = fileInfo.Name;
            Beatmap beatmap = new Beatmap(filename);
            beatmaps.Add(beatmap);
        }
        // Sorts the list
        beatmaps.Sort((x, y) => x.title.CompareTo(y.title));
    }

    void StartLoad(string filename)
    {
        StartCoroutine(LoadBeatmap(filename));
    }

    IEnumerator LoadBeatmap(string filename)
    {
        string musicPath = "file://" + GameSettings.beatmapPath + filename;
        using (UnityWebRequest webRequest = UnityWebRequestMultimedia.GetAudioClip(musicPath, AudioType.WAV))
        {
            ((DownloadHandlerAudioClip)webRequest.downloadHandler).streamAudio = true;

            webRequest.SendWebRequest();
            while (!webRequest.isNetworkError && webRequest.downloadedBytes < 1024)
                yield return null;

            if (webRequest.isNetworkError)
            {
                Debug.LogError(webRequest.error);
                yield break;
            }


            // Plays sounds
            UIHandler uiHandler = FindObjectOfType<UIHandler>();
            uiHandler.PlayButtonSound();
            // Loads clip
            AudioClip clip = ((DownloadHandlerAudioClip)webRequest.downloadHandler).audioClip;
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.audioClip = clip;
            // Loads main scene
            SceneHandler sceneHandler = FindObjectOfType<SceneHandler>();
            sceneHandler.LoadLevel(1);
        }
    }
}
