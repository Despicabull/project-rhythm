using System.Collections;
using TMPro;
using UnityEngine;

public class GameplayHandler : MonoBehaviour
{
    public GameObject blockPrefab;
    public GameObject blockSpawn;
    public GameObject keyThreshold;
    public TextMeshProUGUI scoreText;
    public AudioSource audioSource;
    public KeyPanel keyPanel;
    public int missHit;
    public int badHit;
    public int goodHit;
    public int greatHit;
    public int excellentHit;
    private const float spawnRateConstant = 4f;
    private int currentScore;
    private float spawnRate;
    private bool isSpawning;

    // Start is called before the first frame update
    void Start()
    {
        spawnRate = GameSettings.speed / ((GameSettings.currentMap.mapSpeed / 60f) * spawnRateConstant);
        // spawnRate = speed / ((mapSpeed / 60f) * spawnRateConstant);
        isSpawning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnBlock(spawnRate));
        }
    }

    IEnumerator SpawnBlock(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        int random = Random.Range(0, keyPanel.keys.Length);
        GameObject block = Instantiate(blockPrefab, keyPanel.keys[random].gameObject.transform);
        keyPanel.keys[random].blocks.Add(block);
        Vector3 position = new Vector3(keyPanel.keys[random].transform.position.x, blockSpawn.transform.position.y, keyPanel.keys[random].transform.position.z);
        block.transform.position = position;
        isSpawning = false;
    }

    public void IncreaseScore(int value)
    {
        currentScore += value;
        scoreText.text = currentScore.ToString("D7");
    }
}
