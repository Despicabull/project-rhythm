using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

enum SortType
{
    Ascending,
    Descending,
}

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
    public Transform sortButton;
    public TMP_InputField searchField;
    private SortType sortType = SortType.Ascending;
    private readonly List<Beatmap> beatmaps = new List<Beatmap>();

    // Start is called before the first frame update
    void Start()
    {
        AddBeatmapsToList();
        AddListToView(searchField.text);
    }

    public void ToggleSort()
    {
        sortButton.Rotate(new Vector3(0f, 0f, 180f));
        switch (sortType)
        {
            case SortType.Ascending:
                sortType = SortType.Descending;
                SortBeatmapsZA();
                break;
            case SortType.Descending:
                sortType = SortType.Ascending;
                SortBeatmapsAZ();
                break;
        };
        AddListToView(searchField.text);
    }

    void SortBeatmapsAZ()
    {
        for (int i = 0; i < beatmaps.Count - 1; i++)
        {
            int min_idx = i;
            for (int j = i + 1; j < beatmaps.Count; j++)
            {
                if (string.Compare(beatmaps[min_idx].title, beatmaps[j].title) > 0)
                {
                    min_idx = j;
                }
            }

            Beatmap temp = beatmaps[min_idx];
            beatmaps[min_idx] = beatmaps[i];
            beatmaps[i] = temp;
        }
    }

    void SortBeatmapsZA()
    {
        for (int i = 0; i < beatmaps.Count - 1; i++)
        {
            int min_idx = i;
            for (int j = i + 1; j < beatmaps.Count; j++)
            {
                if (string.Compare(beatmaps[min_idx].title, beatmaps[j].title) < 0)
                {
                    min_idx = j;
                }
            }

            Beatmap temp = beatmaps[min_idx];
            beatmaps[min_idx] = beatmaps[i];
            beatmaps[i] = temp;
        }
    }

    public void AddListToView(string key)
    {
        // Resets the view
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < beatmaps.Count; i++)
        {
            // Checks if beatmaps contain key from search field
            if (beatmaps[i].title.Contains(key))
            {
                GameObject beatmap = Instantiate(beatmapPrefab, content.transform);
                TextMeshProUGUI beatmapTitle = beatmap.transform.Find("Text_Title (TMP)").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI beatmapScore = beatmap.transform.Find("Text_Score (TMP)").GetComponent<TextMeshProUGUI>();
                beatmapTitle.text = beatmaps[i].title;
                beatmapScore.text = FindScore(beatmapTitle.text + "/");
                beatmap.GetComponent<Button>().onClick.AddListener(() => StartLoad(beatmapTitle.text + "/"));
            }
        }
    }

    string FindScore(string folderName)
    {
        string filename = GameSetting.beatmapPath + folderName + "result.dat";
        if (File.Exists(filename))
        {
            StreamReader reader = new StreamReader(filename);
            // a[1] = savedScore
            return reader.ReadLine();
        }
        return "";
    }

    void AddBeatmapsToList()
    {
        // Adds every folder to beatmaps
        DirectoryInfo info = new DirectoryInfo(GameSetting.beatmapPath);
        DirectoryInfo[] directoryInfos = info.GetDirectories();
        foreach (DirectoryInfo directoryInfo in directoryInfos)
        {
            string folderName = directoryInfo.Name;
            Beatmap beatmap = new Beatmap(folderName);
            beatmaps.Add(beatmap);
        }
    }

    void StartLoad(string folderName)
    {
        StartCoroutine(LoadBeatmap(folderName));
    }

    IEnumerator LoadBeatmap(string folderName)
    {
        string folderPath = GameSetting.beatmapPath + folderName;
        DirectoryInfo info = new DirectoryInfo(folderPath);
        FileInfo[] musicInfos = info.GetFiles("*.wav");
        FileInfo[] txtInfos = info.GetFiles("*.txt");
        // Checks if there is a .wav file and .txt file
        if (musicInfos.Length > 0 && txtInfos.Length > 0)
        {
            // Finds the first music file
            string musicPath = "file://" + folderPath + musicInfos[0].Name;
            // Finds the first txt file
            string txtPath = folderPath + txtInfos[0].Name;
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
                gameManager.folderPath = folderPath;
                gameManager.txtPath = txtPath;
                // Loads main scene
                SceneHandler sceneHandler = FindObjectOfType<SceneHandler>();
                sceneHandler.LoadLevel(1);
            }
        }
    }
}
