using DG.Tweening;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Menu Button")]
    public Transform menuButton;
    public Transform playButton;
    public Transform optionsButton;
    public Transform exitButton;
    private bool menuPressed;

    // Start is called before the first frame update
    void Start()
    {
        menuPressed = false;
    }

    public void PlayButtonSound()
    {
        audioSource.Play();
    }

    public void MenuButtonControl()
    {
        if (!menuPressed)
        {
            menuButton.DOLocalMoveX(-menuButton.GetComponent<RectTransform>().rect.width / 4, 1f, true);
            playButton.DOLocalMoveX(playButton.GetComponent<RectTransform>().rect.width * 0.75f, 1f, true);
            optionsButton.DOLocalMoveX(optionsButton.GetComponent<RectTransform>().rect.width * 0.75f, 1f, true);
            exitButton.DOLocalMoveX(exitButton.GetComponent<RectTransform>().rect.width * 0.75f, 1f, true);
            menuPressed = true;
        }
        else
        {
            menuButton.DOLocalMoveX(0, 1f, true);
            playButton.DOLocalMoveX(0, 1f, true);
            optionsButton.DOLocalMoveX(0, 1f, true);
            exitButton.DOLocalMoveX(0, 1f, true);
            menuPressed = false;
        }
    }
}
