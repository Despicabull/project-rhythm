using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BeatmapHandler : MonoBehaviour
{
    public GameObject beatmapPrefab;
    public GameObject content;
    private string path;
    private readonly List<Beatmap> beatmaps = new List<Beatmap>();

    // Start is called before the first frame update
    void Start()
    {
        path = "Beatmaps/";
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
            beatmap.GetComponent<Button>().onClick.AddListener(() => LoadBeatmap(beatmapTitle.text));
        }
    }

    void AddBeatmapsToList()
    {
        // Adds every file with an extension of mp3 to beatmaps
        DirectoryInfo info = new DirectoryInfo(Application.dataPath + "/Resources/" + path);
        FileInfo[] fileInfos = info.GetFiles("*.mp3");
        foreach (FileInfo fileInfo in fileInfos)
        {
            string filename = Path.GetFileNameWithoutExtension(fileInfo.FullName);
            Beatmap beatmap = new Beatmap(filename);
            beatmaps.Add(beatmap);
        }
        // Sorts the list
        beatmaps.Sort((x, y) => x.title.CompareTo(y.title));
    }

    void LoadBeatmap(string filename)
    {
        GameManager gameManager = FindObjectOfType<GameManager>(); ;
        gameManager.audioClip = Resources.Load<AudioClip>(path + filename);
        SceneHandler sceneHandler = FindObjectOfType<SceneHandler>();
        sceneHandler.LoadLevel(GameManager.gameplayID);
    }
}

public class Beatmap
{
    public string title;

    public Beatmap(string title)
    {
        this.title = title;
    }
}
