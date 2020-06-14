using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int menuID = 0;
    public static int gameplayID = 1;
    public AudioClip audioClip;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
