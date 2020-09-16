using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDrop : MonoBehaviour
{
    [SerializeField] private GameObject smashFX = default;
    [SerializeField] private Transform smashPosition = default;

    private Rigidbody2D rb;
    private Vector2 intialPosition;
    private bool blockMovingBack;   

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        intialPosition = transform.position;
    }

    private void Update()
    {
        if (blockMovingBack)
        {
            transform.position = Vector2.MoveTowards(
                transform.position, intialPosition, 20f * Time.deltaTime);
        }
        if (transform.position.y == intialPosition.y)
        {
            blockMovingBack = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.gameObject.CompareTag("Ground") || _collision.gameObject.CompareTag("Player"))
        {
            CameraShake.Instance.ShakeCamera(5f, 0.1f);
            Instantiate(smashFX, smashPosition.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.gameObject.CompareTag("Player") && !blockMovingBack)
        {
            Invoke("DropBlock", 0.5f);
        }
    }

    private void DropBlock()
    {
        rb.isKinematic = false;
        Invoke("GetBlockBack", 3f);
    }

    private void GetBlockBack()
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        blockMovingBack = true;
    }
}
