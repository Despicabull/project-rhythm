using UnityEngine;
using UnityEngine.UI;

public class KeyClickHandler : MonoBehaviour
{
    public Graphic hitEffect;
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
        KeyInput();
    }

    void KeyInput()
    {
        if (!GameplayHandler.isPaused)
        {
            if (Input.GetKeyDown(keyCode))
            {
                FadeToColor(button.colors.pressedColor);
                FadeToAlpha(200f);
                button.onClick.Invoke();
            }
            else if (Input.GetKeyUp(keyCode))
            {
                FadeToColor(button.colors.normalColor);
                FadeToAlpha(0f);
            }
        }
    }

    void FadeToColor(Color color)
    {
        Graphic graphic = GetComponent<Graphic>();
        graphic.CrossFadeColor(color, button.colors.fadeDuration, true, true);
    }

    void FadeToAlpha(float alpha)
    {
        hitEffect.CrossFadeAlpha(alpha, button.colors.fadeDuration, true);
    }
}
