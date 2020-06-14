using UnityEngine;
using UnityEngine.UI;

public class KeyClickHandler : MonoBehaviour
{
    public KeyCode keyCode;
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckKeyboardInput();
    }

    void CheckKeyboardInput()
    {
        if (Input.GetKeyDown(keyCode))
        {
            FadeToColor(button.colors.pressedColor);
            button.onClick.Invoke();
        }
        else if (Input.GetKeyUp(keyCode))
        {
            FadeToColor(button.colors.normalColor);
        }
    }

    void FadeToColor(Color color)
    {
        Graphic graphic = GetComponent<Graphic>();
        graphic.CrossFadeColor(color, button.colors.fadeDuration, true, true);
    }
}
