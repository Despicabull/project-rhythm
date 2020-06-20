using DG.Tweening;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public AudioSource audioSourceButton;
    public AudioSource audioSourceKeyButton;

    [Header("Menu Button")]
    public Transform menuButton;
    public Transform playButton;
    public Transform optionsButton;
    public Transform exitButton;
    private bool menuPressed = false;

    public void PlayButtonSound()
    {
        audioSourceButton.Play();
    }

    public void PlayKeyButtonSound()
    {
        audioSourceKeyButton.Play();
    }

    public void MenuButtonControl()
    {
        if (!menuPressed)
        {
            menuButton.DOLocalMoveX(-menuButton.GetComponent<RectTransform>().rect.width / 4, 0.5f, true);
            playButton.DOLocalMoveX(playButton.GetComponent<RectTransform>().rect.width * 0.65f, 0.5f, true);
            optionsButton.DOLocalMoveX(optionsButton.GetComponent<RectTransform>().rect.width * 0.65f, 0.5f, true);
            exitButton.DOLocalMoveX(exitButton.GetComponent<RectTransform>().rect.width * 0.65f, 0.5f, true);
            menuPressed = true;
        }
        else
        {
            menuButton.DOLocalMoveX(0, 0.5f, true);
            playButton.DOLocalMoveX(0, 0.5f, true);
            optionsButton.DOLocalMoveX(0, 0.5f, true);
            exitButton.DOLocalMoveX(0, 0.5f, true);
            menuPressed = false;
        }
    }
}
