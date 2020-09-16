using UnityEngine;

public class Brick : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 initialPosition;
    private bool platformMovingBack;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (platformMovingBack)
        {
            transform.position = Vector2.MoveTowards(
                transform.position, initialPosition, 20f * Time.deltaTime);
        }
        if (transform.position.y == initialPosition.y)
        {
            platformMovingBack = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.gameObject.CompareTag("Player") && !platformMovingBack)
        {
            Invoke("DropPlatform", 0.5f);
        }      
    }

    private void DropPlatform()
    {
        rb.isKinematic = false;
        Invoke("GetPlatformBack", 3f);
    }

    private void GetPlatformBack()
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        platformMovingBack = true;
    }
}
