using UnityEngine;

public class BlocksMovement : MonoBehaviour
{
    public bool canBePressed;

    void Start()
    {
        canBePressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0, GameSettings.speed * 5f, 0);
        // transform.position -= new Vector3(0, GameSettings.speed * 10f, 0);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Activator")
        {
            canBePressed = true;
        }
    }
}
