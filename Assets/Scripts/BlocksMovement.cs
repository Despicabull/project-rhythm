using UnityEngine;

public class BlocksMovement : MonoBehaviour
{
    public bool canBePressed;
    public bool missed;

    // Start is called before the first frame update
    void Start()
    {
        canBePressed = false;
        missed = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0, (GameSettings.currentMap.mapSpeed * GameSettings.speed) / 60f, 0);
        // transform.position -= new Vector3(0, (GameSettings.currentMap.mapSpeed * GameSettings.speed) / 60f, 0);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Activator")
        {
            canBePressed = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Activator")
        {
            canBePressed = false;
            missed = true;
        }
    }
}
